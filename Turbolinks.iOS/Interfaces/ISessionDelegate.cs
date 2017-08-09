namespace Turbolinks.iOS.Interfaces
{
	using System;

    public interface ISessionDelegate
    {
        void DidProposeVisitToURL(Foundation.NSUrl URL, Action action);

        void DidFailRequestForVisitable(IVisitable visitable, NSError error);

        void OpenExternalURL(Foundation.NSUrl URL);

        void DidLoadWebView();

        void DidStartRequest();

        void DidFinishRequest();
    }
}
