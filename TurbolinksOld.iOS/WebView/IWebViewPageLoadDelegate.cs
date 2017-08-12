namespace Turbolinks.iOS
{
    public interface IWebViewPageLoadDelegate
    {
        void DidLoadPage(string restorationIdentifier);
    }
}
