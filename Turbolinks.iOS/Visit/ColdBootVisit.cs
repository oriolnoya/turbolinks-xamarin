namespace Turbolinks.iOS
{
	using System;
    using UIKit;
    using WebKit;

    public class ColdBootVisit : Visit, IWKNavigationDelegate, IWebViewPageLoadDelegate
    {
        WKNavigation _navigation;

        public ColdBootVisit(Visitable visitable, Enums.Action action, WebView webView) : base(visitable, action, webView)
        {
        }

        protected override void StartVisit()
        {
            _webView.NavigationDelegate = this;
            _webView.PageLoadDelegate = this;

            var request = new Foundation.NSUrlRequest(_location);
            _navigation = _webView.LoadRequest(request);

            _delegate?.DidStart(this);
            StartRequest();
        }

        protected override void CancelVisit()
        {
            RemoveNavigationDelegate();
            _webView.StopLoading();
            FinishRequest();
        }

        protected override void CompleteVisit()
        {
            RemoveNavigationDelegate();
            _delegate?.DidInitializeWebView(this);
        }

        protected override void FailVisit()
        {
            RemoveNavigationDelegate();
            FinishRequest();
        }

        void RemoveNavigationDelegate()
        {
            if (_webView.NavigationDelegate == this)
                _webView.NavigationDelegate = null;
        }


        #region WKNavigationDelegate

        [Foundation.Export("webView:didFinishNavigation:")]
        public void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            if (_navigation == navigation)
                FinishRequest();
        }

        [Foundation.Export("webView:decidePolicyForNavigationAction:decisionHandler:")]
        public void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
        {
            if (navigationAction.NavigationType == WKNavigationType.LinkActivated)
            {
                decisionHandler.Invoke(WKNavigationActionPolicy.Cancel);
                if (navigationAction.Request.Url != null)
                    UIApplication.SharedApplication.OpenUrl(navigationAction.Request.Url);
            }
            else
                decisionHandler.Invoke(WKNavigationActionPolicy.Allow);
        }

        [Foundation.Export("webView:decidePolicyForNavigationResponse:decisionHandler:")]
        public void DecidePolicy(WKWebView webView, WKNavigationResponse navigationResponse, Action<WKNavigationResponsePolicy> decisionHandler)
        {
            if (navigationResponse.Response != null && (navigationResponse.Response as Foundation.NSHttpUrlResponse) != null)
            {
                var httpResponse = navigationResponse.Response as Foundation.NSHttpUrlResponse;

                if (httpResponse.StatusCode >= 200 && httpResponse.StatusCode < 300)
                    decisionHandler(WKNavigationResponsePolicy.Allow);
                else
                {
                    decisionHandler(WKNavigationResponsePolicy.Cancel);
                    Fail(() =>
                    {
                        var error = new NSError(Enums.ErrorCode.HttpFailure, (int)httpResponse.StatusCode);
                        _delegate?.RequestDidFail(this, error);
                    });
                }
            }
            else
            {
                decisionHandler(WKNavigationResponsePolicy.Cancel);
                Fail(() =>
                {
                    var error = new NSError(Enums.ErrorCode.NetworkFailure, "An unknown error ocurred");
                    _delegate?.RequestDidFail(this, error);
                });
            }
        }

        [Foundation.Export("webView:didFailProvisionalNavigation:withError:")]
        public void DidFailProvisionalNavigation(WKWebView webView, WKNavigation navigation, Foundation.NSError error)
        {
            if (navigation == _navigation)
            {
                Fail(() =>
                {
                    var localError = new NSError(Enums.ErrorCode.NetworkFailure, error);
                    _delegate?.RequestDidFail(this, localError);
                });
            }
        }

        [Foundation.Export("webView:didFailNavigation:withError:")]
        public void DidFailNavigation(WKWebView webView, WKNavigation navigation, Foundation.NSError error)
        {
            if (navigation == _navigation)
            {
                Fail(() =>
                {
                    var localError = new NSError(Enums.ErrorCode.NetworkFailure, error);
                    _delegate?.RequestDidFail(this, error);
                });
            }
        }

		#endregion


		#region IWebViewPageLoadDelegate

		void IWebViewPageLoadDelegate.DidLoadPage(string restorationIdentifier)
		{
            _restorationIdentifier = restorationIdentifier;
            _delegate?.DidRender(this);
            Complete();
		}

        #endregion


    }
}
