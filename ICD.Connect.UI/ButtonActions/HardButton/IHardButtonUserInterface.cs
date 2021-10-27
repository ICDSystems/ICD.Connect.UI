using ICD.Connect.Panels.HardButtons;

namespace ICD.Connect.UI.ButtonActions.HardButton
{
	public interface IHardButtonUserInterface
	{
		bool ButtonAction(eHardButton button, eButtonAction action);
	}
}