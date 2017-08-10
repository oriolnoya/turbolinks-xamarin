namespace Turbolinks.iOS
{
    using System;
    using System.Collections.Generic;
    using Foundation;
    using Turbolinks.iOS.Interfaces;
    using UIKit;
    using WebKit;

    public class Session : NSObject, IWebViewDelegate, IWKNavigationDelegate, IVisitDelegate, IVisitableDelegate
    {
        ISessionDelegate _delegate;

        WebView _webView;
        bool _initialized;
        bool _refreshing;

        public Session(WKWebViewConfiguration webViewConfiguration)
        {
            _webView = new WebView(webViewConfiguration);

            _webView.Delegate = this;
        }

        public WKWebView WebView => _webView;


        #region Visiting

        Visit _currentVisit;
        Visit _topMostVisit;

        public IVisitable TopMostVisitable => _topMostVisit?.Visitable;

        public void Visit(IVisitable visitable)
        {
            VisitVisitable(visitable, Enums.Action.Advance);
        }

        void VisitVisitable(IVisitable visitable, Enums.Action action)
        {
            if (visitable.VisitableUrl == null)
                return;

            visitable.VisitableDelegate = this;

            Visit visit;

            if (_initialized)
            {
                visit = new JavaScriptVisit(visitable, action, _webView);
                visit.RestorationIdentifier = RestorationIdentifierForVisitable(visitable);
            }
            else
            {
                visit = new ColdBootVisit(visitable, action, _webView);
            }

            _currentVisit?.Cancel();
            _currentVisit = visit;

            visit.Delegate = this;
            visit.Start();
        }

        public void Reload()
        {
            if (TopMostVisitable != null)
            {
                _initialized = false;
                Visit(TopMostVisitable);
                _topMostVisit = _currentVisit;
            }
        }

        #endregion


        #region Visitable activation

        IVisitable _activatedVisitable;

        void ActivateVisitable(IVisitable visitable)
        {
            if (visitable != _activatedVisitable)
            {
                DeactivateVisitable(_activatedVisitable, true);

                visitable.ActivateVisitableWebView(_webView);

                _activatedVisitable = visitable;
            }
        }

        void DeactivateVisitable(IVisitable visitable, bool showScreenshot = false)
        {
            if (visitable == _activatedVisitable)
            {
                if (showScreenshot)
                {
                    visitable.UpdateVisitableScreenshot();
                    visitable.ShowVisitableScreenshot();
                }

                visitable.DeactivateVisitableWebView();

                _activatedVisitable = null;
            }
        }

        #endregion


        #region Visitable restoration identifiers

        Dictionary<UIViewController, string> _visitableRestorationIdentifiers = new Dictionary<UIViewController, string>();

        string RestorationIdentifierForVisitable(IVisitable visitable)
        {
            return _visitableRestorationIdentifiers[visitable.VisitableViewController];
        }

        void StoreRestorationIdentifier(string restorationIdentifier, IVisitable visitable)
        {
            _visitableRestorationIdentifiers[visitable.VisitableViewController] = restorationIdentifier;
        }

        void CompleteNavigtationForCurrentVisit()
        {
            if (_currentVisit != null)
            {
                _topMostVisit = _currentVisit;
                _currentVisit.CompleteNavigation();
            }
        }

		#endregion



		#region IWebViewDelegate implementation

		void IWebViewDelegate.DidProposeVisit(NSUrl location, Enums.Action action)
		{
            _delegate?.DidProposeVisitToURL(this, location, action);
		}

		void IWebViewDelegate.DidInvalidatePage()
		{
            if(TopMostVisitable != null)
            {
                TopMostVisitable.UpdateVisitableScreenshot();
                TopMostVisitable.ShowVisitableScreenshot();
                TopMostVisitable.ShowVisitableActivityIndicator();
                Reload();
            }
		}

		void IWebViewDelegate.DidFailJavaScriptEvaluation(Foundation.NSError error)
		{
            if(_currentVisit != null && _initialized)
            {
                _initialized = false;
                _currentVisit.Cancel();
                Visit(_currentVisit.Visitable);
            }
		}

		#endregion



		#region IVisitDelegate implementation
		
        void IVisitDelegate.DidInitializeWebView(Visit visit)
        {
            _initialized = true;
            _delegate?.DidLoadWebView(this);
        }

        void IVisitDelegate.WillStart(Visit visit)
        {
            visit.Visitable.ShowVisitableScreenshot();
            ActivateVisitable(visit.Visitable);
        }

        void IVisitDelegate.DidStart(Visit visit)
        {
            if (!visit.HasCachedSnapshot)
                visit.Visitable.ShowVisitableActivityIndicator();
        }

        void IVisitDelegate.DidComplete(Visit visit)
        {
            if (!string.IsNullOrEmpty(visit.RestorationIdentifier))
                StoreRestorationIdentifier(visit.RestorationIdentifier, visit.Visitable);
        }

        void IVisitDelegate.DidFail(Visit visit)
        {
            visit.Visitable.ClearVisitableScreenshot();
            visit.Visitable.ShowVisitableScreenshot();
        }

        void IVisitDelegate.DidFinish(Visit visit)
        {
            if (_refreshing)
            {
                _refreshing = false;
                visit.Visitable.VisitableDidRefresh();
            }
        }

        void IVisitDelegate.WillLoadResponse(Visit visit)
        {
            visit.Visitable.UpdateVisitableScreenshot();
            visit.Visitable.ShowVisitableScreenshot();
        }

        void IVisitDelegate.DidRender(Visit visit)
        {
            visit.Visitable.HideVisitableScreenshot();
            visit.Visitable.HideVisitableActivityIndicator();
            visit.Visitable.VisitableDidRender();
        }

        void IVisitDelegate.RequestDidStart(Visit visit)
        {
            _delegate?.DidStartRequest(this);
        }

        void IVisitDelegate.RequestDidFail(Visit visit, Foundation.NSError error)
        {
            _delegate?.DidFailRequestForVisitable(this, visit.Visitable, error);
        }

        void IVisitDelegate.RequestDidFinish(Visit visit)
        {
            _delegate.DidFinishRequest(this);
        }

		#endregion


		#region IVisitableDelegate implementation

		void IVisitableDelegate.ViewWillAppear(IVisitable visitable)
        {
            if (_topMostVisit == null || _currentVisit == null) return;

            if(visitable == _topMostVisit.Visitable && visitable.VisitableViewController.IsMovingToParentViewController)
            {
                if (_topMostVisit.State == Enums.VisitState.Completed)
                    _currentVisit.Cancel();
                else
                    VisitVisitable(visitable, Enums.Action.Advance);
            }
            else if(visitable == _currentVisit.Visitable && _currentVisit.State == Enums.VisitState.Started)
            {
                CompleteNavigtationForCurrentVisit();
            }
            else if(visitable != _topMostVisit.Visitable)
            {
                VisitVisitable(visitable, Enums.Action.Restore);
            }
        }

        void IVisitableDelegate.ViewDidAppear(IVisitable visitable)
        {
            if(_currentVisit != null && visitable == _currentVisit.Visitable)
            {
                CompleteNavigtationForCurrentVisit();
                if (_currentVisit.State != Enums.VisitState.Failed)
                    ActivateVisitable(visitable);
            }
            else if(_topMostVisit != null && visitable == _topMostVisit.Visitable && _topMostVisit.State == Enums.VisitState.Completed)
            {
                visitable.HideVisitableScreenshot();
                visitable.HideVisitableActivityIndicator();
                ActivateVisitable(visitable);
            }
        }

        void IVisitableDelegate.DidRequestReload(IVisitable visitable)
        {
            if (visitable == TopMostVisitable)
                Reload();
        }

        void IVisitableDelegate.DidRequestRefresh(IVisitable visitable)
        {
            if(visitable == TopMostVisitable)
            {
                _refreshing = true;
                visitable.VisitableWillRefresh();
                Reload();
            }
        }

		#endregion



		#region IWKNavigationDelegate implementation

		[Export("webView:decidePolicyForNavigationAction:decisionHandler:")]
		public virtual void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
        {
            var navigationDecision = new NavigationDecision(navigationAction);
            decisionHandler.Invoke(navigationDecision.Policy);

            if (navigationDecision.ExternallyOpenableURL != null)
                OpenExternalURL(navigationDecision.ExternallyOpenableURL);
            else if (navigationDecision.ShouldReloadPage)
                Reload();
        }

        void OpenExternalURL(NSUrl url)
        {
            _delegate?.OpenExternalURL(this, url);
        }

		#endregion
	}

    class NavigationDecision
    {
        WKNavigationAction _navigationAction;

        public NavigationDecision(WKNavigationAction navigationAction)
        {
            _navigationAction = navigationAction;
        }

        public WKNavigationActionPolicy Policy => 
            (_navigationAction.NavigationType == WKNavigationType.LinkActivated || IsMainFrameNavigation) ? WKNavigationActionPolicy.Cancel : WKNavigationActionPolicy.Allow;

        public NSUrl ExternallyOpenableURL => (_navigationAction.Request.Url != null && ShouldOpenURLExternally) ? _navigationAction.Request.Url : null;

        public bool ShouldOpenURLExternally =>
            (_navigationAction.NavigationType == WKNavigationType.LinkActivated || (IsMainFrameNavigation && _navigationAction.NavigationType == WKNavigationType.Other));

        public bool ShouldReloadPage => (IsMainFrameNavigation && _navigationAction.NavigationType == WKNavigationType.Reload);

        public bool IsMainFrameNavigation => _navigationAction.TargetFrame?.MainFrame ?? false;

    }
}
