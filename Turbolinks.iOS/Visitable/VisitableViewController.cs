namespace Turbolinks.iOS
{
    using System;
    using Foundation;
    using UIKit;
    using WebKit;

    public class VisitableViewController : UIViewController, IVisitable
    {

        VisitableView _visitableView;


        public IVisitableDelegate VisitableDelegate { get; set; }

        public NSUrl VisitableUrl { get; }

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
            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[view]|", 0, null, NSDictionary.FromObjectAndKey(_visitableView, new NSString("view"))));
            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[view]|", 0, null, NSDictionary.FromObjectAndKey(_visitableView, new NSString("view"))));
        }

        public void ActivateVisitableWebView(WKWebView webView)
        {
            _visitableView.ActivateWebView(webView, this);
        }

        public void ClearVisitableScreenshot()
        {
            _visitableView.ClearScreenshot();
        }

        public void DeactivateVisitableWebView()
        {
            _visitableView.DeactivateWebView();
        }

        public void HideVisitableActivityIndicator()
        {
            _visitableView.HideActivityIndicator();
        }

        public void HideVisitableScreenshot()
        {
            _visitableView.HideScreenshot();
        }

        public void ReloadVisitable()
        {
            VisitableDelegate?.DidRequestReload(this);
        }

        public void ShowVisitableActivityIndicator()
        {
            _visitableView.ShowActivityIndicator();
        }

        public void ShowVisitableScreenshot()
        {
            _visitableView.ShowScreenshot();
        }

        public void UpdateVisitableScreenshot()
        {
            _visitableView.UpdateScreenshot();
        }

        public void VisitableDidRefresh()
        {
            VisitableView.RefreshControl.EndRefreshing();
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
            VisitableView.RefreshControl.BeginRefreshing();
        }
    }
}
