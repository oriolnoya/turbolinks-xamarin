namespace Turbolinks.iOS.WebViews
{
    using Foundation;

    public interface IWebViewDelegate
    {
        void DidProposeVisit(NSUrl location, Enums.Action action);

        void DidInvalidatePage();

        void DidFailJavaScriptEvaluation(NSError error);
    }
}
