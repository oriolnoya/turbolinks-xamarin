namespace TurbolinksDemo.iOS
{
    using System;
    using Foundation;
    using Turbolinks.iOS;
    using Turbolinks.iOS.Enums;
    using Turbolinks.iOS.Interfaces;
    using UIKit;
    using WebKit;

    public partial class ApplicationController : UINavigationController, ISessionDelegate, IWKScriptMessageHandler, IAuthenticationControllerDelegate
    {
        NSUrl URL = new NSUrl("http://localhost:9292");
        WKProcessPool _webViewProcessPool = new WKProcessPool();

        WKWebViewConfiguration _webViewConfiguration;
        Session _session;

        public ApplicationController(IntPtr handle) : base(handle)
        {
        }

        public WKWebViewConfiguration WebViewConfiguration
        {
            get
            {
                if (_webViewConfiguration == null)
                {
                    _webViewConfiguration = new WKWebViewConfiguration();
                    _webViewConfiguration.UserContentController.AddScriptMessageHandler(this, "turbolinksDemo");
                    _webViewConfiguration.ProcessPool = _webViewProcessPool;
                    _webViewConfiguration.ApplicationNameForUserAgent = "TurbolinksDemo";
                }
                return _webViewConfiguration;
            }
        }

        public Session Session
        {
            get
            {
                if (_session == null)
                {
                    _session = new Session(WebViewConfiguration)
                    {
                        Delegate = this
                    };
                }
                return _session;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            PresentVisitableForSession(Session, URL);
        }

        void PresentVisitableForSession(Session session, NSUrl url, Turbolinks.iOS.Enums.Action action = Turbolinks.iOS.Enums.Action.Advance)
        {
            var visitable = new DemoViewController(url);

            if (action == Turbolinks.iOS.Enums.Action.Advance)
                PushViewController(visitable, true);
            else if (action == Turbolinks.iOS.Enums.Action.Replace)
            {
                PopViewController(false);
                PushViewController(visitable, false);
            }

            session.Visit(visitable);
        }

        void PresentNumbersViewController()
        {
            var viewController = new NumbersViewController();
            PushViewController(viewController, true);
        }

        void PresentAuthenticationController()
        {
            var authenticationController = new AuthenticationController()
            {
                Title = "Sign in",
                Delegate = this,
                WebViewConfiguration = _webViewConfiguration,
                Url = URL.Append("sign-in", false)
            };

            var authNavigationController = new UINavigationController(authenticationController);
            PresentViewController(authNavigationController, true, null);
        }

        #region ISessionDelegate

        void ISessionDelegate.DidProposeVisitToURL(Session session, NSUrl URL, Turbolinks.iOS.Enums.Action action)
        {
            if (URL.Path == "/numbers")
                PresentNumbersViewController();
            else
                PresentVisitableForSession(session, URL, action);
        }

        void ISessionDelegate.DidFailRequestForVisitable(Session session, IVisitable visitable, Foundation.NSError error)
        {
            var demoViewController = visitable as DemoViewController;
            if (demoViewController == null) return;

            var errorCode = (ErrorCode)(int)error.Code;

            switch(errorCode)
            {
                case ErrorCode.HttpFailure:
                    var statusCode = error.UserInfo["statusCode"] as NSNumber;

                    switch(statusCode.Int32Value)
                    {
                        case 401:
                            PresentAuthenticationController();
                            break;
                        case 404:
                            demoViewController.PresentError(Error.HTTPNotFoundError);
                            break;
                        default:
                            demoViewController.PresentError(new Error(statusCode.Int32Value));
                            break;
                    }

                    break;
                case ErrorCode.NetworkFailure:
                    demoViewController.PresentError(Error.NetworkError);
                    break;
            }

        }

        void ISessionDelegate.OpenExternalURL(Session session, NSUrl URL)
        {
            UIApplication.SharedApplication.OpenUrl(URL);
        }

        void ISessionDelegate.DidLoadWebView(Session session)
        {
            session.WebView.NavigationDelegate = session;
        }

        void ISessionDelegate.DidStartRequest(Session session)
        {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
        }

        void ISessionDelegate.DidFinishRequest(Session session)
        {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
        }

		#endregion



		#region IWKScriptMessageHandler

		void IWKScriptMessageHandler.DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            if (message.Body == null && (message.Body as NSString == null)) return;

            var alertController = UIAlertController.Create("Turbolinks", message.Body.ToString(), UIAlertControllerStyle.Alert);
            alertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
            PresentViewController(alertController, true, null);
        }

		#endregion



		#region IAuthenticationControllerDelegate

		void IAuthenticationControllerDelegate.AuthenticationControllerDidAuthenticate(AuthenticationController authenticationController)
		{
            Session.Reload();
            DismissViewController(true, null);
		}

		#endregion


	}
}
