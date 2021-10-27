using System;
using ICD.Common.Properties;
using ICD.Connect.Settings;
using ICD.Connect.UI.ButtonActions.Actions;
using ICD.Connect.UI.ButtonActions.HardButton;

namespace ICD.Connect.UI.ButtonActions.Factories
{
	public interface IButtonActionMapFactory<TUi> : IButtonActionMapFactory
		where TUi : IHardButtonUserInterface
	{
		new TUi Ui { get; }
	}

	public interface IButtonActionMapFactory
	{
		IHardButtonUserInterface Ui { get; }

		IButtonAction InstantiateButtonActionFromXml(IDeviceFactory factory, string bindingType, string xml);
	}
}