namespace Turbolinks.iOS.Enums
{
    public enum ScriptMessageName
    {
        [ValueToServer("pageLoaded")]
        PageLoaded,

        [ValueToServer("errorRaised")]
        ErrorRaised,

		[ValueToServer("visitProposed")]
        VisitProposed,

		[ValueToServer("visitStarted")]
        VisitStarted,

		[ValueToServer("visitRequestStarted")]
        VisitRequestStarted,

		[ValueToServer("visitRequestCompleted")]
        VisitRequestCompleted,

		[ValueToServer("visitRequestFailed")]
        VisitRequestFailed,

		[ValueToServer("visitRequestFinished")]
        VisitRequestFinished,

		[ValueToServer("visitRendered")]
        VisitRendered,

		[ValueToServer("visitCompleted")]
        VisitCompleted,

		[ValueToServer("pageInvalidated")]
        PageInvalidated
    }
}
