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

        public Visitable TopMostVisitable => _topMostVisit?.Visitable;

        public void Visit(Visitable visitable)
        {
            VisitVisitable(visitable, Enums.Action.Advance);
        }

        void VisitVisitable(Visitable visitable, Enums.Action action)
        {
            if (visitable.VisitableURL == null)
                return;

            visitable.VisitableDelegate = this;

            Visit visit;

            if (_initialized)
            {
                visit = new JavascriptVisit(visitable, action, _webView);
                visit.RestorationIdentifier = RestorationIdentifierForVisitable(visitable);
            }
            else
            {
                visit = ColdBootVisit(visitable, action, _webView);
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

        Visitable _activatedVisitable;

        void ActivateVisitable(Visitable visitable)
        {
            if (visitable != _activatedVisitable)
            {
                DeactivateVisitable(_activatedVisitable, true);

                visitable.ActivateVisitableWebView(_webView);

                _activatedVisitable = visitable;
            }
        }

        void DeactivateVisitable(Visitable visitable, bool showScreenshot = false)
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

        string RestorationIdentifierForVisitable(Visitable visitable)
        {
            return _visitableRestorationIdentifiers.ObjectForKey(visitable.VisitableViewController);
        }

        void StoreRestorationIdentifier(string restorationIdentifier, Visitable visitable)
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

        #endregion



        #region IWKNavigationDelegate implementation



        #endregion
    }
}
