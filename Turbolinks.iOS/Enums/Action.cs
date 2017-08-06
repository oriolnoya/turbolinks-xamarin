namespace Turbolinks.iOS.Enums
{    
    using System;

    public enum Action
    {
		[ValueToServer("none")]
		None = 0,
        [ValueToServer("advance")]
        Advance = 1,
        [ValueToServer("replace")]
        Replace = 2,
        [ValueToServer("restore")]
        Restore = 3
    }

	public class ValueToServerAttribute : Attribute
	{
		public ValueToServerAttribute(string valueToServer)
		{
			this.ValueToServer = valueToServer;
		}

		public string ValueToServer { get; private set; }
	}
}
