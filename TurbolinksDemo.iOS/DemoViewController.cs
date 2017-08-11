namespace TurbolinksDemo.iOS
{
    using System;
    using Foundation;
    using Turbolinks.iOS;
    using UIKit;

    public class DemoViewController : VisitableViewController
    {

        public DemoViewController(NSUrl url) : base(url)
        {
            
        }

        ErrorView _errorView;
        public ErrorView ErrorView
        {
            get
            {
                if(_errorView == null)
                {
                    _errorView = NSBundle.MainBundle.LoadNib("ErrorView", this, new NSDictionary()).GetItem<ErrorView>(0);
                    _errorView.TranslatesAutoresizingMaskIntoConstraints = false;
                    _errorView.RetryButton.AddTarget(this, new ObjCRuntime.Selector("retry:"), UIControlEvent.TouchUpInside);
                }
                return _errorView;
            }
        }

        public void PresentError(Error error)
        {
            ErrorView.Error = error;
            View.AddSubview(ErrorView);
            InstallErrorViewConstraints();
        }

        void InstallErrorViewConstraints()
        {
            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[view]|", 0, null, NSDictionary.FromObjectAndKey(ErrorView, new NSString("view"))));
            View.AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[view]|", 0, null, NSDictionary.FromObjectAndKey(ErrorView, new NSString("view"))));
        }

        [Export("retry:")]
        void Retry(NSObject sender)
        {
            ErrorView.RemoveFromSuperview();
            ReloadVisitable();
        }
    }
}
