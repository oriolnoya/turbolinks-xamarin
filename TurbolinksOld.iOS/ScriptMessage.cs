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

        public Enums.Action Action => (_data["action"] != null) ? Enum.GetValues(typeof(Enums.Action)).Cast<Enums.Action>().First(a => a.ToString() == _data["action"].ToString()) : Enums.Action.None;

        public static ScriptMessage Parse(WKScriptMessage message)
        {
            var body = message.Body as NSDictionary;

            if (body == null || body["name"] == null || body["data"] == null) return null;

            var scriptMessageName = Enum.GetValues(typeof(ScriptMessageName)).Cast<ScriptMessageName>().First(smn => smn.ToString() == body["name"].ToString());

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
    }
}
