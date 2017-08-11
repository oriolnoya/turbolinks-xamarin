namespace TurbolinksDemo.iOS
{
    using System;

    public class Error
    {
        public string Title;
        public string Message;

        public Error(string title, string message)
        {
            Title = title;
            Message = message;
        }

        public Error(int httpStatusCode)
        {
            Title = "Server error";
            Message = $"The server returned an HTTP {httpStatusCode} response.";
        }

        public static Error HTTPNotFoundError = new Error("Page not found", "There doesn’t seem to be anything here.");

        public static Error NetworkError = new Error("Can’t Connect", "TurbolinksDemo can’t connect to the server. Did you remember to start it?\\nSee README.md for more instructions.");

        public static Error UnknownError = new Error("Unknown Error", "An unknown error occurred.");
    }
}
