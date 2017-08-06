using System;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using WebKit;

namespace TurbolinksBinding.iOS
{

    // @interface Turbolinks_Swift_145 (NSError)
    [Category]
    [BaseType(typeof(NSError))]
    interface NSError_Turbolinks_Swift_145
    {
    }

    // @interface Session : NSObject
    [BaseType(typeof(NSObject))]
    interface Session
    {
        // @property (readonly, nonatomic, strong) WKWebView * _Nonnull webView;
        [Export("webView", ArgumentSemantic.Strong)]
        WKWebView WebView { get; }

        // -(instancetype _Nonnull)initWithWebViewConfiguration:(WKWebViewConfiguration * _Nonnull)webViewConfiguration __attribute__((objc_designated_initializer));
        [Export("initWithWebViewConfiguration:")]
        [DesignatedInitializer]
        IntPtr Constructor(WKWebViewConfiguration webViewConfiguration);

        // -(void)reload;
        [Export("reload")]
        void Reload();
    }

    // @interface Turbolinks_Swift_161 (Session) <WKNavigationDelegate>
    [Category]
    [BaseType(typeof(Session))]
    interface Session_Turbolinks_Swift_161 : IWKNavigationDelegate
    {
        // -(void)webView:(WKWebView * _Nonnull)webView decidePolicyForNavigationAction:(WKNavigationAction * _Nonnull)navigationAction decisionHandler:(void (^ _Nonnull)(WKNavigationActionPolicy))decisionHandler;
        [Export("webView:decidePolicyForNavigationAction:decisionHandler:")]
        void WebView(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler);
    }

    // @interface Turbolinks_Swift_166 (Session)
    [Category]
    [BaseType(typeof(Session))]
    interface Session_Turbolinks_Swift_166
    {
    }

    // @interface Turbolinks_Swift_170 (Session)
    [Category]
    [BaseType(typeof(Session))]
    interface Session_Turbolinks_Swift_170
    {
    }

    // @interface Turbolinks_Swift_174 (Session)
    [Category]
    [BaseType(typeof(Session))]
    interface Session_Turbolinks_Swift_174
    {
    }

    // @interface VisitableView : UIView
    [BaseType(typeof(UIView))]
    interface VisitableView
    {
        // -(instancetype _Nullable)initWithCoder:(NSCoder * _Nonnull)aDecoder __attribute__((objc_designated_initializer));
        [Export("initWithCoder:")]
        [DesignatedInitializer]
        IntPtr Constructor(NSCoder aDecoder);

        // -(instancetype _Nonnull)initWithFrame:(CGRect)frame __attribute__((objc_designated_initializer));
        [Export("initWithFrame:")]
        [DesignatedInitializer]
        IntPtr Constructor(CGRect frame);

        // @property (nonatomic, strong) WKWebView * _Nullable webView;
        [NullAllowed, Export("webView", ArgumentSemantic.Strong)]
        WKWebView WebView { get; set; }

        // -(void)deactivateWebView;
        [Export("deactivateWebView")]
        void DeactivateWebView();

        // @property (nonatomic, strong) UIRefreshControl * _Nonnull refreshControl;
        [Export("refreshControl", ArgumentSemantic.Strong)]
        UIRefreshControl RefreshControl { get; set; }

        // @property (nonatomic) BOOL allowsPullToRefresh;
        [Export("allowsPullToRefresh")]
        bool AllowsPullToRefresh { get; set; }

        // @property (readonly, nonatomic) BOOL isRefreshing;
        [Export("isRefreshing")]
        bool IsRefreshing { get; }

        // @property (nonatomic, strong) UIActivityIndicatorView * _Nonnull activityIndicatorView;
        [Export("activityIndicatorView", ArgumentSemantic.Strong)]
        UIActivityIndicatorView ActivityIndicatorView { get; set; }

        // -(void)showActivityIndicator;
        [Export("showActivityIndicator")]
        void ShowActivityIndicator();

        // -(void)hideActivityIndicator;
        [Export("hideActivityIndicator")]
        void HideActivityIndicator();

        // -(void)updateScreenshot;
        [Export("updateScreenshot")]
        void UpdateScreenshot();

        // -(void)showScreenshot;
        [Export("showScreenshot")]
        void ShowScreenshot();

        // -(void)hideScreenshot;
        [Export("hideScreenshot")]
        void HideScreenshot();

        // -(void)clearScreenshot;
        [Export("clearScreenshot")]
        void ClearScreenshot();

        // -(void)layoutSubviews;
        [Export("layoutSubviews")]
        void LayoutSubviews();
    }

    // @interface VisitableViewController : UIViewController
    [BaseType(typeof(UIViewController))]
    interface VisitableViewController
    {
        // @property (copy, nonatomic) NSURL * _Null_unspecified visitableURL;
        [Export("visitableURL", ArgumentSemantic.Copy)]
        NSUrl VisitableURL { get; set; }

        // -(instancetype _Nonnull)initWithUrl:(NSURL * _Nonnull)url;
        [Export("initWithUrl:")]
        IntPtr Constructor(NSUrl url);

        // @property (readonly, nonatomic, strong) VisitableView * _Null_unspecified visitableView;
        [Export("visitableView", ArgumentSemantic.Strong)]
        VisitableView VisitableView { get; }

        // -(void)visitableDidRender;
        [Export("visitableDidRender")]
        void VisitableDidRender();

        // -(void)viewDidLoad;
        [Export("viewDidLoad")]
        void ViewDidLoad();

        // -(void)viewWillAppear:(BOOL)animated;
        [Export("viewWillAppear:")]
        void ViewWillAppear(bool animated);

        // -(void)viewDidAppear:(BOOL)animated;
        [Export("viewDidAppear:")]
        void ViewDidAppear(bool animated);

        // -(instancetype _Nonnull)initWithNibName:(NSString * _Nullable)nibNameOrNil bundle:(NSBundle * _Nullable)nibBundleOrNil __attribute__((objc_designated_initializer));
        [Export("initWithNibName:bundle:")]
        [DesignatedInitializer]
        IntPtr Constructor([NullAllowed] string nibNameOrNil, [NullAllowed] NSBundle nibBundleOrNil);

        // -(instancetype _Nullable)initWithCoder:(NSCoder * _Nonnull)aDecoder __attribute__((objc_designated_initializer));
        [Export("initWithCoder:")]
        [DesignatedInitializer]
        IntPtr Constructor(NSCoder aDecoder);
    }
}