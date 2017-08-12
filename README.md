# Turbolinks for Xamarin
A C# implementation of [turbolinks-ios](https://github.com/turbolinks/turbolinks-ios) for using it with Xamarin.iOS apps.

### What is Turbolinks?
As they say on their [repo](https://github.com/turbolinks/turbolinks-ios):
> Turbolinks for iOS provides the tooling to wrap your Turbolinks 5-enabled web app in a native iOS shell. It manages a single WKWebView instance across multiple view controllers, giving you native navigation UI with all the client-side performance benefits of Turbolinks.

### How to run the demo?
Start the demo server by running `TurbolinksServer/demo-server` from your command line.

If you need more information please go to [their repo](https://github.com/turbolinks/turbolinks-ios#running-the-demo).


### Why porting to C# and not binding the native library?
The first reason is that I wanted to try this hybrid approach on some app ideas I have. The second is that I tried binding the library but I had some problems so I gave up. And the third reason is that I want to learn Swift and I thought that porting it to C# would help me on this. I was correct :)

### What is my roadmap from now on?
1. Fix a known bug that makes the transition between views less smooth than it should be. I have noticed that the turbolinks.js of the server raises an error when manipulating the browser history and this makes that the appearing screen doesn't show the content until it has appeared completely. This is not the correct behaviour so I want to solve it asap. All the help would be welcome.

2. Build a real personal project using the library.

3. Port [xamarin-android](https://github.com/turbolinks/turbolinks-android). This is not a priority for me at this moment, so I don't know when I'll be able to do it. If somebody is interested in doing I would be glad to welcome him/her.
