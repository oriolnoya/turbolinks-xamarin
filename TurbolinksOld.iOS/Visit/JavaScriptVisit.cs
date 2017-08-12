using System;
namespace Turbolinks.iOS
{
    public class JavaScriptVisit : Visit, IWebViewVisitDelegate
    {
        string _identifier = "(pending)";

        public JavaScriptVisit(IVisitable visitable, Enums.Action action, WebView webView) : base(visitable, action, webView)
        {
        }

        protected override void StartVisit()
        {
            _webView.VisitDelegate = this;
            _webView.VisitLocation(_location, _action, _restorationIdentifier);
        }

        protected override void CancelVisit()
        {
            _webView.CancelVisit(_identifier);
            FinishRequest();
        }

        protected override void FailVisit()
        {
            FinishRequest();
        }


		#region IWebViewVisitDelegate

		void IWebViewVisitDelegate.DidStartVisit(string identifier, bool hasCachedSnapshot)
        {
            _identifier = identifier;
            _hasCachedSnapshot = hasCachedSnapshot;

            _delegate?.DidStart(this);
            _webView.IssueRequestForVisit(_identifier);

            AfterNavigationCompletion(() =>
            {
                _webView.ChangeHistoryForVisit(_identifier);
                _webView.LoadCachedSnapshotForVisit(_identifier);
            });
        }

        void IWebViewVisitDelegate.DidStartRequestForVisit(string identifier)
        {
            if (_identifier == identifier)
                StartRequest();
        }

        void IWebViewVisitDelegate.DidCompleteRequestForVisit(string identifier)
        {
            if(_identifier == identifier)
            {
                AfterNavigationCompletion(() => 
                {
                    _delegate?.WillLoadResponse(this);
                    _webView.LoadResponseForVisit(_identifier);
                });
            }
        }

        void IWebViewVisitDelegate.DidFailRequestForVisit(string identifier, int statusCode)
        {
            if(_identifier == identifier)
            {
                Fail(() => 
                {
                    NSError error = (statusCode == 0) ? new NSError(Enums.ErrorCode.NetworkFailure, "A network error ocurred") : new NSError(Enums.ErrorCode.HttpFailure, statusCode);
                    _delegate?.RequestDidFail(this, error);
                });
            }
        }

        void IWebViewVisitDelegate.DidFinishRequestForVisit(string identifier)
        {
            if (_identifier == identifier)
                FinishRequest();
        }

        void IWebViewVisitDelegate.DidRenderForVisit(string identifier)
        {
            if (_identifier == identifier)
                _delegate?.DidRender(this);
        }

        void IWebViewVisitDelegate.DidCompleteVisit(string identifier, string restorationIdentifier)
        {
            if (_identifier == identifier)
            {
                _restorationIdentifier = restorationIdentifier;
                Complete();
            }
        }

        #endregion


    }
}
