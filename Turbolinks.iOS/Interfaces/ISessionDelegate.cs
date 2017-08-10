namespace Turbolinks.iOS.Interfaces
{
	using System;

    public interface ISessionDelegate
    {
        void DidProposeVisitToURL(Session session, Foundation.NSUrl URL, Enums.Action action);

        void DidFailRequestForVisitable(Session session, IVisitable visitable, Foundation.NSError error);

        void OpenExternalURL(Session session, Foundation.NSUrl URL);

        void DidLoadWebView(Session session);

        void DidStartRequest(Session session);

        void DidFinishRequest(Session session);
    }
}
