using ICD.Common.Utils;
using ICD.Connect.Protocol.HardButtons;

namespace ICD.Connect.UI.ButtonActions.Actions
{
	/// <summary>
	/// This class takes no action when the ButtonAction is called
	/// This can be used to override default bindings
	/// </summary>
	public sealed class NoActionButtonAction : AbstractButtonAction<NoActionButtonActionSettings>
	{
		/// <summary>
		/// Handle a button press/release/etc
		/// This is what the ActionMap "does"
		/// </summary>
		/// <param name="action"></param>
		public override bool ButtonAction(eButtonAction action)
		{
			IcdConsole.PrintLine(eConsoleColor.Magenta, "NoButtonAction activated with action {0}", action);
			// This class purposfully does nothing, so it's always successful
			return true;
		}
	}
}