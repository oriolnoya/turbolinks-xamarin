namespace Turbolinks.iOS
{
    public interface IVisitableDelegate
    {
        void ViewWillAppear(IVisitable visitable);
        void ViewDidAppear(IVisitable visitable);
        void DidRequestReload(IVisitable visitable);
        void DidRequestRefresh(IVisitable visitable);
    }
}
