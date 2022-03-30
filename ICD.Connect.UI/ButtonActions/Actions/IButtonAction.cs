using System;
using ICD.Common.Properties;
using ICD.Connect.Panels.HardButtons;
using ICD.Connect.Protocol.HardButtons;
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

	[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
	public interface IButtonAction
	{
		/// <summary>
		/// Name for the Button Action
		/// </summary>
		[CanBeNull]
		string Name { get; }

		/// <summary>
		/// If this action map should be serialized out to settings
		/// False for default button actions added automatically
		/// </summary>
		bool Serialize { get; set; }

		/// <summary>
		/// Settings class the button action uses
		/// </summary>
		Type SettingsClass { get; }

		/// <summary>
		/// Handle a button press/release/etc
		/// This is what the button action "does"
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