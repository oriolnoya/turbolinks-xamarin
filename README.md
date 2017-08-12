# Turbolinks for Xamarin
A C# implementation of [turbolinks-ios](https://github.com/turbolinks/turbolinks-ios) for using it with Xamarin.iOS apps.

### What is Turbolinks?
As they say on their [repo](https://github.com/turbolinks/turbolinks-ios):
> Turbolinks for iOS provides the tooling to wrap your Turbolinks 5-enabled web app in a native iOS shell. It manages a single WKWebView instance across multiple view controllers, giving you native navigation UI with all the client-side performance benefits of Turbolinks.

### How to run the demo?
Start the demo server by running `TurbolinksServer/demo-server` from your command line.

If you need more information please go to [their repo](https://github.com/turbolinks/turbolinks-ios#running-the-demo).


### Why did you ported to C# and not binded the library?
The first reason is that I want to try this hybrid approach on some app ideas I have. The second is that I tried binding the library but I had some problems so I gave up. And third and the main reason is that I want to learn Swift and I thought that porting it to C# would help me on this. I was correct :)
