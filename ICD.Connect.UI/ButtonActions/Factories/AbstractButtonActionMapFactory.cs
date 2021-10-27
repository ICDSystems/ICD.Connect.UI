using System;
using ICD.Common.Utils;
using ICD.Connect.Settings;
using ICD.Connect.UI.ButtonActions.Actions;
using ICD.Connect.UI.ButtonActions.HardButton;

namespace ICD.Connect.UI.ButtonActions.Factories
{
	public abstract class AbstractButtonActionMapFactory<TUi> : IButtonActionMapFactory<TUi>
		where TUi : IHardButtonUserInterface
	{
		private readonly TUi m_Ui;

		public TUi Ui { get { return m_Ui; } }

		protected AbstractButtonActionMapFactory(TUi ui)
		{
			m_Ui = ui;
		}

		IHardButtonUserInterface IButtonActionMapFactory.Ui { get { return Ui; } }

		public IButtonAction InstantiateButtonActionFromXml(IDeviceFactory factory, string mapType, string xml)
		{
			Type settings = ButtonActionRootFactory.GetSettingsTypeForFactoryName(mapType);
			return InstantiateButtonActionFromXml(factory, settings, xml);
		}

		private IButtonAction InstantiateButtonActionFromXml(IDeviceFactory factory, Type settingsType, string xml)
		{
			IButtonActionSettings settings = ReflectionUtils.CreateInstance<IButtonActionSettings>(settingsType);
			settings.ParseXml(xml);
			IButtonAction action = ReflectionUtils.CreateInstance<IButtonAction>(settings.ButtonActionType);
			action.LoadSettings(settings, this, factory);
			return action;
		}
	}
}