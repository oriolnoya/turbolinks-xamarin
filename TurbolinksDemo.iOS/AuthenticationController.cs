namespace TurbolinksDemo.iOS
{
    using System;
    using CoreGraphics;
    using Foundation;
    using UIKit;
    using WebKit;

    public class AuthenticationController : UIViewController, IWKNavigationDelegate
    {
        public NSUrl Url;
        public WKWebViewConfiguration WebViewConfiguration;
        public IAuthenticationControllerDelegate Delegate;

        WKWebView _webView;

        public WKWebView WebView
        {
            get
            {
                if(_webView == null)
                {
                    var configuration = WebViewConfiguration != null ? WebViewConfiguration : new WKWebViewConfiguration();
                    _webView = new WKWebView(CGRect.Empty, configuration);
                    _webView.TranslatesAutoresizingMaskIntoConstraints = false;
                    _webView.NavigationDelegate = this;
                }
                return _webView;
            }
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.AddSubview(WebView);
            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[view]|", 0, null, NSDictionary.FromObjectAndKey(WebView, new NSString("view"))));
            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[view]|", 0, null, NSDictionary.FromObjectAndKey(WebView, new NSString("view"))));

            if (Url != null)
                WebView.LoadRequest(new NSUrlRequest(Url));
        }

		#region IWKNavigationDelegate implementation

		[Export("webView:decidePolicyForNavigationAction:decisionHandler:")]
		public virtual void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
		{
            if(navigationAction.Request.Url != null && navigationAction.Request.Url != Url)
            {
                decisionHandler(WKNavigationActionPolicy.Cancel);
                Delegate?.AuthenticationControllerDidAuthenticate(this);
                return;
            }

            decisionHandler(WKNavigationActionPolicy.Allow);
		}

		#endregion
	}

    public interface IAuthenticationControllerDelegate
    {
        void AuthenticationControllerDidAuthenticate(AuthenticationController authenticationController);    
    }
}
