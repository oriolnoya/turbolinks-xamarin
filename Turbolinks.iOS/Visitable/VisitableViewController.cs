namespace Turbolinks.iOS
{
    using System;
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

        UIViewController IVisitable.VisitableViewController => this as UIViewController;

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

        void InstallVisitableView()
        {
            View.AddSubview(_visitableView);
            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[view]|", 0, null, NSDictionary.FromObjectAndKey(VisitableView, new NSString("view"))));
            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[view]|", 0, null, NSDictionary.FromObjectAndKey(VisitableView, new NSString("view"))));
        }

        public void ActivateVisitableWebView(WKWebView webView)
        {
            VisitableView?.ActivateWebView(webView, this);
        }

        public void ClearVisitableScreenshot()
        {
            VisitableView?.ClearScreenshot();
        }

        public void DeactivateVisitableWebView()
        {
            VisitableView?.DeactivateWebView();
        }

        public void HideVisitableActivityIndicator()
        {
            VisitableView?.HideActivityIndicator();
        }

        public void HideVisitableScreenshot()
        {
            VisitableView?.HideScreenshot();
        }

        public void ReloadVisitable()
        {
            VisitableDelegate?.DidRequestReload(this);
        }

        public void ShowVisitableActivityIndicator()
        {
            VisitableView?.ShowActivityIndicator();
        }

        public void ShowVisitableScreenshot()
        {
            VisitableView?.ShowScreenshot();
        }

        public void UpdateVisitableScreenshot()
        {
            VisitableView?.UpdateScreenshot();
        }

        public void VisitableDidRefresh()
        {
            VisitableView?.RefreshControl.EndRefreshing();
        }

        public void VisitableDidRender()
        {
            Title = _visitableView.WebView.Title;
        }

        public void VisitableViewDidRequestRefresh()
        {
            VisitableDelegate?.DidRequestRefresh(this);
        }

        public void VisitableWillRefresh()
        {
            VisitableView?.RefreshControl.BeginRefreshing();
        }
    }
}
