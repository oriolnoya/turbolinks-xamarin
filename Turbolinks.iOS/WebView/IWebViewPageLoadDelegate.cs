namespace Turbolinks.iOS.WebViews
{
    public interface IWebViewPageLoadDelegate
    {
        void DidLoadPage(string restorationIdentifier);
    }
}
