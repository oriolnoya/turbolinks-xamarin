namespace Turbolinks.iOS
{
    using System;
    using Foundation;
    using Turbolinks.iOS.Enums;

    public class NSError : Foundation.NSError
    {
        const string ErrorDomain = "com.basecamp.Turbolinks";

        public NSError(ErrorCode code, string localizedDescription) : 
            base(new NSString(ErrorDomain), 
                 new nint((int)code), 
                 NSDictionary.FromObjectAndKey(new NSString(localizedDescription), LocalizedDescriptionKey))
        {
        }

		public NSError(ErrorCode code, int statusCode) :
			base(new NSString(ErrorDomain),
				 new nint((int)code),
                 NSDictionary.FromObjectsAndKeys(
                     new NSObject[] {new NSNumber(statusCode), new NSString($"HTTP Error: ({statusCode})")}, 
                     new NSObject[] {new NSString("statusCode"), LocalizedDescriptionKey }))
		{
		}

        public NSError(ErrorCode code, NSError error) :
			base(new NSString(ErrorDomain),
				 new nint((int)code),
				 NSDictionary.FromObjectsAndKeys(
                     new NSObject[] { error, new NSString(error.LocalizedDescription) },
					 new NSObject[] { new NSString("error"), LocalizedDescriptionKey }))
		{
		}
    }
}