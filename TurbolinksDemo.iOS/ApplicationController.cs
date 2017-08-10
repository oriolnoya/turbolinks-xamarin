namespace TurbolinksDemo.iOS
{
	using System;
    using Foundation;
    using Turbolinks.iOS;
    using Turbolinks.iOS.Interfaces;
    using UIKit;
    using WebKit;

    public partial class ApplicationController : UINavigationController, ISessionDelegate, IWKScriptMessageHandler
	{
        NSUrl URL = new NSUrl("http://localhost:9292");
        WKProcessPool _webViewProcessPool = new WKProcessPool();

        WKWebViewConfiguration _webViewConfiguration;
        Session _session;

		public ApplicationController (IntPtr handle) : base (handle)
		{
		}

        public WKWebViewConfiguration WebViewConfiguration
        {
            get
            {
                if(_webViewConfiguration == null)
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
                if(_session == null)
                {
                    _session = new Session(_webViewConfiguration);
                    _session.Delegate = this;
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
            
        }
	}
}
