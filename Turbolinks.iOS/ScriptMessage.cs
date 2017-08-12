namespace Turbolinks.iOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Foundation;
    using Turbolinks.iOS.Enums;
    using WebKit;

    public class ScriptMessage
    {
        ScriptMessageName _name;
        Dictionary<string, object> _data;

        public ScriptMessage(ScriptMessageName name, Dictionary<string, object> data)
        {
            _name = name;
            _data = data;
        }

        public ScriptMessageName Name => _name;
        public Dictionary<string, object> Data => _data;

        public string Identifier => _data["identifier"]?.ToString() ?? string.Empty;

        public string RestorarionIdentifier => _data["restorationIdentifier"]?.ToString() ?? string.Empty;

        public NSUrl Location => (_data["location"] != null) ? new NSUrl(_data["location"].ToString()) : null;

        public Enums.Action Action => (_data["action"] != null) ? GetAction(_data["action"].ToString()) : Enums.Action.None;

        public static ScriptMessage Parse(WKScriptMessage message)
        {
            var body = message.Body as NSDictionary;
            if (body == null) return null;

            if (body["name"] == null || body["name"] as NSString == null) return null;

            var scriptMessageName = GetScriptMessageName(body["name"].ToString());
            if (scriptMessageName == ScriptMessageName.None) return null;

            if (body["data"] == null || body["data"] as NSDictionary == null) return null;
            var data = body["data"] as NSDictionary;

            return new ScriptMessage(scriptMessageName, ConvertToDictionary(data));
        }

        static Dictionary<string, object> ConvertToDictionary(NSDictionary nativeDict)
        {
            var dict = new Dictionary<string, object>();

			foreach (var item in nativeDict)
                dict.Add((NSString)item.Key, item.Value);

			return dict;
        }

        static ScriptMessageName GetScriptMessageName(string name)
        {
            switch(name)
            {
                case "pageLoaded":
                    return ScriptMessageName.PageLoaded;
                case "errorRaised":
                    return ScriptMessageName.ErrorRaised;
                case "visitProposed":
                    return ScriptMessageName.VisitProposed;
                case "visitStarted":
                    return ScriptMessageName.VisitStarted;
                case "visitRequestStarted":
                    return ScriptMessageName.VisitRequestStarted;
                case "visitRequestCompleted":
                    return ScriptMessageName.VisitRequestCompleted;
                case "visitRequestFailed":
                    return ScriptMessageName.VisitRequestFailed;
                case "visitRequestFinished":
                    return ScriptMessageName.VisitRequestFinished;
                case "visitRendered":
                    return ScriptMessageName.VisitRendered;
                case "visitCompleted":
                    return ScriptMessageName.VisitCompleted;
                case "pageInvalidated":
                    return ScriptMessageName.PageInvalidated;
                default:
                    return ScriptMessageName.None;
            }
        }

        static Enums.Action GetAction(string actionName)
        {
            switch(actionName)
            {
                case "advance":
                    return Enums.Action.Advance;
                case "replace":
                    return Enums.Action.Replace;
                case "restore":
                    return Enums.Action.Restore;
                default:
                    return Enums.Action.None;
            }
        }

		
    }
}
