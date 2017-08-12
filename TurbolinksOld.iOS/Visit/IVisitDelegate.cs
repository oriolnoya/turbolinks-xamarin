namespace Turbolinks.iOS
{
    public interface IVisitDelegate
    {
        void DidInitializeWebView(Visit visit);

        void WillStart(Visit visit);
        void DidStart(Visit visit);
        void DidComplete(Visit visit);
        void DidFail(Visit visit);
        void DidFinish(Visit visit);

        void WillLoadResponse(Visit visit);
        void DidRender(Visit visit);

        void RequestDidStart(Visit visit);
        void RequestDidFail(Visit visit, Foundation.NSError error);
        void RequestDidFinish(Visit visit);
    }
}
