namespace TurbolinksDemo.iOS
{
    using UIKit;

    public class NumbersViewController : UITableViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Numbers";
            TableView.RegisterClassForCellReuse(typeof(UITableViewCell), new Foundation.NSString("CellIdentifier") );
        }

        public override System.nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override System.nint RowsInSection(UITableView tableView, System.nint section)
        {
            return 100;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("CellIdentifier", indexPath);

            cell.TextLabel.Text = $"Row {(indexPath.Row + 1)}";

            return cell;
        }
    }
}
