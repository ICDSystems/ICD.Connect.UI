using System;
using ICD.Common.Properties;
using ICD.Connect.Panels.HardButtons;
using ICD.Connect.Settings;
using ICD.Connect.UI.ButtonActions.Factories;
using ICD.Connect.UI.ButtonActions.HardButton;

namespace ICD.Connect.UI.ButtonActions.Actions
{
	public interface IButtonAction<TUi, TSettings> : IButtonAction<TSettings>
		where TUi : IHardButtonUserInterface where TSettings : IButtonActionSettings
	{
		/// <summary>
		/// Ui this action map is attached to
		/// </summary>
		[CanBeNull]
		TUi Ui { get; }
	}

	public interface IButtonAction<TSettings> : IButtonAction
		where TSettings:IButtonActionSettings
	{
		/// <summary>
		/// Load the settings into the instace
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="actionFactory"></param>
		/// <param name="deviceFactory"></param>
		void LoadSettings([NotNull]TSettings settings, [NotNull]IButtonActionMapFactory actionFactory, [NotNull]IDeviceFactory deviceFactory);

		/// <summary>
		/// Copy the instance to the settings
		/// </summary>
		/// <param name="settings"></param>
		void CopySettings([NotNull]TSettings settings);
	}

	public interface IButtonAction
	{
		/// <summary>
		/// Name for the ActionMap
		/// </summary>
		[CanBeNull]
		string Name { get; }

		/// <summary>
		/// Settings class the action map uses
		/// </summary>
		Type SettingsClass { get; }

		/// <summary>
		/// Handle a button press/release/etc
		/// This is what the ActionMap "does"
		/// </summary>
		/// <param name="action"></param>
		/// <returns>true if the action was executed, false if not</returns>
		bool ButtonAction(eButtonAction action);

		/// <summary>
		/// Load the settings into the instace
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="actionFactory"></param>
		/// <param name="deviceFactory"></param>
		void LoadSettings([NotNull]IButtonActionSettings settings, [NotNull]IButtonActionMapFactory actionFactory, [NotNull]IDeviceFactory deviceFactory);

		/// <summary>
		/// Copy the instance to the settings
		/// </summary>
		/// <param name="settings"></param>
		void CopySettings(IButtonActionSettings settings);
	}
}