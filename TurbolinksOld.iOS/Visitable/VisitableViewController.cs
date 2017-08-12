namespace Turbolinks.iOS
{
    using Foundation;
    using UIKit;
    using WebKit;

    public class VisitableViewController : UIViewController, IVisitable
    {
        public IVisitableDelegate VisitableDelegate { get; set; }
        public NSUrl VisitableUrl { get; }

        public VisitableViewController(NSUrl url)
        {
            VisitableUrl = url;
        }

        #region Visitable View

        VisitableView _visitableView;
        public VisitableView VisitableView
        {
            get
            {
                if (_visitableView == null)
                {
                    _visitableView = new VisitableView(CoreGraphics.CGRect.Empty);
                    _visitableView.TranslatesAutoresizingMaskIntoConstraints = false;
                }
                return _visitableView;
            }
        }

        void InstallVisitableView()
        {
            View.AddSubview(_visitableView);
            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[view]|", 0, null, NSDictionary.FromObjectAndKey(VisitableView, new NSString("view"))));
            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[view]|", 0, null, NSDictionary.FromObjectAndKey(VisitableView, new NSString("view"))));
        }

        #endregion


        #region Visitable

        public void VisitableDidRender()
        {
            Title = _visitableView.WebView?.Title;
        }

        #endregion


        #region View Lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.White;
            InstallVisitableView();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            VisitableDelegate?.ViewWillAppear(this);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            VisitableDelegate?.ViewDidAppear(this);
        }

        #endregion


        #region IVisitable

        UIViewController IVisitable.VisitableViewController => this as UIViewController;

		public void ReloadVisitable()
		{
			VisitableDelegate?.DidRequestReload(this);
		}

        public void ActivateVisitableWebView(WKWebView webView)
        {
            VisitableView?.ActivateWebView(webView, this);
        }

        public void DeactivateVisitableWebView()
        {
            VisitableView?.DeactivateWebView();
        }

		public void ShowVisitableActivityIndicator()
		{
			VisitableView?.ShowActivityIndicator();
		}

        public void HideVisitableActivityIndicator()
        {
            VisitableView?.HideActivityIndicator();
        }

		public void UpdateVisitableScreenshot()
		{
			VisitableView?.UpdateScreenshot();
		}
		
		public void ShowVisitableScreenshot()
		{
			VisitableView?.ShowScreenshot();
		}
  
        public void HideVisitableScreenshot()
        {
            VisitableView?.HideScreenshot();
        }

		public void ClearVisitableScreenshot()
		{
			VisitableView?.ClearScreenshot();
		}

		public void VisitableWillRefresh()
		{
			VisitableView?.RefreshControl.BeginRefreshing();
		}

        public void VisitableDidRefresh()
        {
            VisitableView?.RefreshControl.EndRefreshing();
        }

        public void VisitableViewDidRequestRefresh()
        {
            VisitableDelegate?.DidRequestRefresh(this);
        }

        #endregion

    }
}
