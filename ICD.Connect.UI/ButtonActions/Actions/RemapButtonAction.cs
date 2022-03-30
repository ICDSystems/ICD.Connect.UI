using ICD.Connect.Protocol.HardButtons;
using ICD.Connect.Settings;
using ICD.Connect.UI.ButtonActions.Factories;
using ICD.Connect.UI.ButtonActions.HardButton;

namespace ICD.Connect.UI.ButtonActions.Actions
{
	public sealed class RemapButtonAction : AbstractButtonAction<IHardButtonUserInterface, RemapButtonActionSettings>
	{
		public eHardButton RemapAsButton { get; private set; }

		public RemapButtonAction()
		{
		}

		public RemapButtonAction(IHardButtonUserInterface ui, string name, eHardButton remapAsButton)
		{
			Ui = ui;
			Name = name;
			RemapAsButton = remapAsButton;
		}

		public override bool ButtonAction(eButtonAction action)
		{
			if (Ui == null)
				return false;

			return Ui.ButtonAction(RemapAsButton, action);
		}

		protected override void LoadSettingsFinal(RemapButtonActionSettings settings, IButtonActionMapFactory<IHardButtonUserInterface> actionFactory,
		                                          IDeviceFactory deviceFactory)
		{
			RemapAsButton = settings.RemapAsButton;
		}

		public override void CopySettings(RemapButtonActionSettings settings)
		{
			base.CopySettings(settings);

			settings.RemapAsButton = RemapAsButton;
		}
	}
}