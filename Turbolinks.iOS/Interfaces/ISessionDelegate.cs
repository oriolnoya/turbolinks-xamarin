namespace Turbolinks.iOS.Interfaces
{
	using System;

    public interface ISessionDelegate
    {
        void SessionDidProposeVisitToURLWithAction(Session session, Foundation.NSUrl URL, Action action);

        void SessionDidFailRequestForVisitableWithError(Session session, Visitable visitable, NSError error);

        void SessionOpenExternalURL(Session session, Foundation.NSUrl URL);

        void SessionDidLoadWebView(Session session);

        void SessionDidStartRequest(Session session);

        void SessionDidFinishRequest(Session session);
    }
}
