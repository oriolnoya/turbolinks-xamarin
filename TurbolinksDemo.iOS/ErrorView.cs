namespace TurbolinksDemo.iOS
{
	using System;
	using UIKit;

    public partial class ErrorView : UIView
    {
		Error _error;

        public ErrorView (IntPtr handle) : base (handle)
        {
        }

		public Error Error
		{
			get => _error;
			set
			{
				_error = value;
				TitleLabel.Text = Error?.Title;
				MessageLabel.Text = Error.Message;
			}
		}
    }
}