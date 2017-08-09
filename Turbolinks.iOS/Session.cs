namespace Turbolinks.iOS
{
    using System;
    using Foundation;
    using Turbolinks.iOS.Interfaces;
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

        NSDictionary _visitableRestorationIdentifiers = new NSDictionary();

        string RestorationIdentifierForVisitable(IVisitable visitable)
        {
            return _visitableRestorationIdentifiers.ObjectForKey(visitable.VisitableViewController).ToString();
        }

        void StoreRestorationIdentifier(string restorationIdentifier, IVisitable visitable)
        {
            _visitableRestorationIdentifiers.SetValueForKey(new NSString(restorationIdentifier), visitable.VisitableViewController);
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
			throw new NotImplementedException();
		}

		void IWebViewDelegate.DidInvalidatePage()
		{
			throw new NotImplementedException();
		}

		void IWebViewDelegate.DidFailJavaScriptEvaluation(Foundation.NSError error)
		{
			throw new NotImplementedException();
		}

		#endregion



		#region IVisitDelegate implementation
		
        void IVisitDelegate.DidInitializeWebView(Visit visit)
        {
            _initialized = true;
            _delegate?.DidLoadWebView();
        }

        void IVisitDelegate.WillStart(Visit visit)
        {
            visit.Visitable.ShowVisitableScreenshot();
            ActivateVisitable(visit.Visitable);
        }

        void IVisitDelegate.DidStart(Visit visit)
        {
            
        }

        void IVisitDelegate.DidComplete(Visit visit)
        {
            throw new NotImplementedException();
        }

        void IVisitDelegate.DidFail(Visit visit)
        {
            throw new NotImplementedException();
        }

        void IVisitDelegate.DidFinish(Visit visit)
        {
            throw new NotImplementedException();
        }

        void IVisitDelegate.WillLoadResponse(Visit visit)
        {
            throw new NotImplementedException();
        }

        void IVisitDelegate.DidRender(Visit visit)
        {
            throw new NotImplementedException();
        }

        void IVisitDelegate.RequestDidStart(Visit visit)
        {
            _delegate.DidStartRequest();
        }

        void IVisitDelegate.RequestDidFail(Visit visit, Foundation.NSError error)
        {
            _delegate?.DidFailRequestForVisitable(visit.Visitable, error);
        }

        void IVisitDelegate.RequestDidFinish(Visit visit)
        {
            _delegate.DidFinishRequest();
        }

        void IVisitableDelegate.ViewWillAppear(IVisitable visitable)
        {
            throw new NotImplementedException();
        }

        void IVisitableDelegate.ViewDidAppear(IVisitable visitable)
        {
            throw new NotImplementedException();
        }

        void IVisitableDelegate.DidRequestReload(IVisitable visitable)
        {
            throw new NotImplementedException();
        }

        void IVisitableDelegate.DidRequestRefresh(IVisitable visitable)
        {
            throw new NotImplementedException();
        }

        #endregion



        #region IWKNavigationDelegate implementation



        #endregion
    }
}
