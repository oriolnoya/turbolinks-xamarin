namespace Turbolinks.iOS.WebViews
{
    public interface IWebViewVisitDelegate
    {
        void DidStartVisit(string identifier, bool hasCachedSnapshot);

        void DidStartRequestForVisit(string identifier);

        void DidCompleteRequestForVisit(string identifier);

        void DidFailRequestForVisit(string identifier, int statusCode);

        void DidFinishRequestForVisit(string identifier);

        void DidRenderForVisit(string identifier);

        void DidCompleteVisit(string identifier, string restorationIdentifier);
    }
}
