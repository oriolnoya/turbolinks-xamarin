namespace Turbolinks.iOS
{
    using Foundation;

    public interface IWebViewDelegate
    {
        void DidProposeVisit(NSUrl location, Enums.Action action);

        void DidInvalidatePage();

        void DidFailJavaScriptEvaluation(Foundation.NSError error);
    }
}
