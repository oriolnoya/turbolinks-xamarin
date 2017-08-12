namespace Turbolinks.iOS
{
    using Foundation;

    public interface IWebViewDelegate
    {
        void DidProposeVisit(WebView webView, NSUrl location, Enums.Action action);
        void DidInvalidatePage(WebView webView);
        void DidFailJavaScriptEvaluation(WebView webView, Foundation.NSError error);
    }
}
