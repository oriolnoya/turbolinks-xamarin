namespace Turbolinks.iOS
{
    using System;
    using Foundation;

    public partial class Visit : NSObject
    {
        protected IVisitDelegate _delegate;

        Visitable _visitable;
        Enums.Action _action;
        protected WebView _webView;
        Enums.VisitState _state;

        protected NSUrl _location;
        bool _hasCachedSnapshot;
        protected string _restorationIdentifier;

        public Visit(Visitable visitable, Enums.Action action, WebView webView)
        {
            _visitable = visitable;
            _location = visitable.VisitableUrl;
            _action = action;
            _webView = webView;
            _state = Enums.VisitState.Initialized;
        }

        public IVisitDelegate Delegate => _delegate;

        void Start()
        {
            if (_state == Enums.VisitState.Initialized)
            {
                _delegate?.WillStart(this);
                _state = Enums.VisitState.Started;
                StartVisit();
            }
        }

        void Cancel()
        {
            if (_state == Enums.VisitState.Started)
            {
                _state = Enums.VisitState.Canceled;
                CancelVisit();
            }
        }

        protected void Complete()
        {
            if (_state == Enums.VisitState.Started)
            {
                _state = Enums.VisitState.Completed;
                CompleteVisit();
                _delegate?.DidComplete(this);
                _delegate?.DidFinish(this);
            }
        }

        protected void Fail(Action callback)
        {
            if(_state == Enums.VisitState.Started)
            {
                _state = Enums.VisitState.Failed;
                callback?.Invoke();
                FailVisit();
                _delegate?.DidFail(this);
                _delegate.DidFinish(this);
            }    
        }

        protected virtual void StartVisit(){}
        protected virtual void CancelVisit(){}
        protected virtual void CompleteVisit(){}
        protected virtual void FailVisit(){}

        #region Navigation

        bool _navigationCompleted = false;
        Action _navigationCallback;

        void CompleteNavigation()
        {
            if (_state == Enums.VisitState.Started && !_navigationCompleted)
            {
                _navigationCompleted = true;
                _navigationCallback?.Invoke();
            }
        }

        void AfterNavigationCompletion(Action callback)
        {
            if (_navigationCompleted)
                callback.Invoke();
            else
            {
                var previousNavigationCallback = _navigationCallback;
                _navigationCallback = () =>
                {
                    previousNavigationCallback.Invoke();
                    if (_state != Enums.VisitState.Canceled)
                        callback.Invoke();
                };
            }
        }

        #endregion


        #region Request State

        bool _requestStarted;
        bool _requestFinished;

        protected void StartRequest()
        {
            if (!_requestStarted)
            {
                _requestStarted = true;
                _delegate?.RequestDidStart(this);
            }
        }

        protected void FinishRequest()
        {
            if(_requestStarted && !_requestFinished)
            {
                _requestFinished = true;
                _delegate?.RequestDidFinish(this);
            }
        }

        #endregion

    }
}
