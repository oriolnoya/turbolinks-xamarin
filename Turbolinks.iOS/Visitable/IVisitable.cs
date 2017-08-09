namespace Turbolinks.iOS
{
    using Foundation;
    using UIKit;

    public interface IVisitable
    {
        IVisitableDelegate VisitableDelegate { get; set; }
        NSUrl VisitableUrl { get; }
        VisitableView VisitableView { get; }

        UIViewController VisitableViewController { get; }

        void VisitableDidRender();
        void ReloadVisitable();
        void ActivateVisitableWebView(WebKit.WKWebView webView);
        void DeactivateVisitableWebView();
        void ShowVisitableActivityIndicator();
        void HideVisitableActivityIndicator();
        void UpdateVisitableScreenshot();
        void ShowVisitableScreenshot();
        void HideVisitableScreenshot();
        void ClearVisitableScreenshot();
        void VisitableWillRefresh();
        void VisitableDidRefresh();
        void VisitableViewDidRequestRefresh();
    }
}
