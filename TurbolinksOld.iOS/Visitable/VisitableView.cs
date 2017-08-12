namespace Turbolinks.iOS
{
    using CoreGraphics;
    using Foundation;
    using UIKit;
    using WebKit;

    public class VisitableView : UIView
    {
        public VisitableView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public VisitableView(CGRect frame) : base(frame)
        {
            Initialize();
        }

        void Initialize()
        {
            InstallHiddenScrollView();
            InstallActivityIndicatorView();
        }


        #region WebView

        public WKWebView WebView { get; set; }

        UIEdgeInsets _contentInset;
        public UIEdgeInsets ContentInset
        {
            get => _contentInset;
            set
            {
                _contentInset = value;
                UpdateContentInsets();
            }
        }

        IVisitable _visitable;

        public void ActivateWebView(WKWebView webView, IVisitable visitable)
        {
            WebView = webView;
            _visitable = visitable;
            AddSubview(webView);
            AddFillConstraints(webView);
            UpdateContentInsets();
            InstallRefreshControl();
            ShowOrHideWebView();
        }

        public void DeactivateWebView()
        {
            RemoveRefreshControl();
            WebView?.RemoveFromSuperview();
            WebView = null;
            _visitable = null;
        }

        void ShowOrHideWebView()
        {
            if(WebView != null)
                WebView.Hidden = IsShowingScreenshot;
        }

        #endregion


        #region Refresh Control

        UIRefreshControl _refreshControl;
		public UIRefreshControl RefreshControl
		{
			get
			{
				if (_refreshControl == null)
				{
                    _refreshControl = new UIRefreshControl();
                    _refreshControl.AddTarget(this, new ObjCRuntime.Selector("refresh:"), UIControlEvent.ValueChanged);
				}
				return _refreshControl;
			}
		}

        bool _allowsPullToRefresh;
        public bool AllowsPullToRefresh
        {
            get => _allowsPullToRefresh;
            set
            {
                _allowsPullToRefresh = value;
                if (_allowsPullToRefresh)
                    InstallRefreshControl();
                else
                    RemoveRefreshControl();
            }
        }

        public bool IsRefreshing => RefreshControl.Refreshing;

        void InstallRefreshControl()
        {
            if (WebView?.ScrollView != null && AllowsPullToRefresh)
                WebView.ScrollView.AddSubview(RefreshControl);
        }

        void RemoveRefreshControl()
        {
            RefreshControl.EndRefreshing();
            RefreshControl.RemoveFromSuperview();
        }

        [Export("refresh:")]
        void Refresh(NSObject sender)
        {
            _visitable?.VisitableViewDidRequestRefresh();
        }

		#endregion


		#region Activity Indicator

        UIActivityIndicatorView _activityIndicatorView;
		public UIActivityIndicatorView ActivityIndicatorView
		{
			get
			{
				if (_activityIndicatorView == null)
				{
                    _activityIndicatorView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White);
                    _activityIndicatorView.TranslatesAutoresizingMaskIntoConstraints = false;
                    _activityIndicatorView.Color = UIColor.Gray;
                    _activityIndicatorView.HidesWhenStopped = true;
				}
				return _activityIndicatorView;
			}
		}

		void InstallActivityIndicatorView()
		{
            AddSubview(ActivityIndicatorView);
            AddFillConstraints(ActivityIndicatorView);
		}

		public void ShowActivityIndicator()
		{
            if(!IsRefreshing)
            {
                ActivityIndicatorView.StartAnimating();
                BringSubviewToFront(ActivityIndicatorView);
            }
		}

        public void HideActivityIndicator()
        {
            ActivityIndicatorView.StopAnimating();
        }

		#endregion



		#region Screenshots

        UIView _screenshotContainerView;
		public UIView ScreenshotContainerView
		{
			get
			{
				if (_screenshotContainerView == null)
				{
                    _screenshotContainerView = new UIView(CGRect.Empty);
                    _screenshotContainerView.TranslatesAutoresizingMaskIntoConstraints = false;
                    _screenshotContainerView.BackgroundColor = BackgroundColor;
				}
				return _screenshotContainerView;
			}
		}

        UIView _screenshotView;

        bool IsShowingScreenshot => ScreenshotContainerView.Superview != null;

        public void UpdateScreenshot()
        {
            if (WebView == null) return;
            if (!IsShowingScreenshot) return;

            var screenshot = WebView.SnapshotView(false);
            if (screenshot == null) return;

            _screenshotView?.RemoveFromSuperview();
            screenshot.TranslatesAutoresizingMaskIntoConstraints = false;
            ScreenshotContainerView.AddSubview(screenshot);

            ScreenshotContainerView.AddConstraints(new NSLayoutConstraint[]
            {
                NSLayoutConstraint.Create(screenshot, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, ScreenshotContainerView, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(screenshot, NSLayoutAttribute.Top, NSLayoutRelation.Equal, ScreenshotContainerView, NSLayoutAttribute.Top, 1, 0),
                NSLayoutConstraint.Create(screenshot, NSLayoutAttribute.Width, NSLayoutRelation.Equal, 1, screenshot.Bounds.Size.Width),
                NSLayoutConstraint.Create(screenshot, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, screenshot.Bounds.Size.Height)
            });

            _screenshotView = screenshot;
        }

        public void ShowScreenshot()
        {
            if(!IsShowingScreenshot && !IsRefreshing)
            {
                AddSubview(ScreenshotContainerView);
                AddFillConstraints(ScreenshotContainerView);
                ShowOrHideWebView();
            }
        }

        public void HideScreenshot()
        {
            ScreenshotContainerView.RemoveFromSuperview();
            ShowOrHideWebView();
        }

        public void ClearScreenshot()
        {
            _screenshotView?.RemoveFromSuperview();
        }

		#endregion



		#region ScrollView

        UIScrollView _hiddenScrollView;
        public UIScrollView HiddenScrollView
		{
			get
			{
				if (_hiddenScrollView == null)
				{
                    _hiddenScrollView = new UIScrollView(CGRect.Empty);
                    _hiddenScrollView.TranslatesAutoresizingMaskIntoConstraints = false;
                    _hiddenScrollView.ScrollsToTop = false;
				}
				return _hiddenScrollView;
			}
		}

        void InstallHiddenScrollView()
        {
            InsertSubview(HiddenScrollView, 0);
            AddFillConstraints(HiddenScrollView);
        }

		#endregion



		#region Layout

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            UpdateContentInsets();
        }

        bool NeedsUpdateForContentInsets(UIEdgeInsets insets)
        {
            if (WebView?.ScrollView == null) return false;
            return WebView.ScrollView.ContentInset.Top != insets.Top || WebView.ScrollView.ContentInset.Bottom != insets.Bottom;
        }
		
        void UpdateWebViewScrollViewInsets(UIEdgeInsets insets)
        {
            if (WebView?.ScrollView == null || !NeedsUpdateForContentInsets(insets) || IsRefreshing) return;

            WebView.ScrollView.ScrollIndicatorInsets = insets;
            WebView.ScrollView.ContentInset = insets;
        }

		void UpdateContentInsets()
		{
            UpdateWebViewScrollViewInsets(ContentInset != UIEdgeInsets.Zero ? ContentInset : HiddenScrollView.ContentInset);
		}

		#endregion


        void AddFillConstraints(UIView view)
        {
			AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[view]|", 0, null, NSDictionary.FromObjectAndKey(view, new NSString("view"))));
			AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[view]|", 0, null, NSDictionary.FromObjectAndKey(view, new NSString("view"))));
        }
		
    }
}
