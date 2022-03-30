using System;
using ICD.Common.Properties;
using ICD.Connect.Panels.HardButtons;
using ICD.Connect.Protocol.HardButtons;
using ICD.Connect.Settings;
using ICD.Connect.UI.ButtonActions.Factories;
using ICD.Connect.UI.ButtonActions.HardButton;

namespace ICD.Connect.UI.ButtonActions.Actions
{
	public abstract class AbstractButtonAction<TUi, TSettings> : AbstractButtonAction<TSettings>, IButtonAction<TUi, TSettings>
		where TUi : class, IHardButtonUserInterface where TSettings : class, IButtonActionSettings
	{
		private TUi m_Ui;

		/// <summary>
		/// Ui this action map is attached to
		/// </summary>
		public TUi Ui
		{
			get { return m_Ui; }
			protected set
			{
				if (m_Ui == value)
					return;

				Unsubscribe(m_Ui);
				m_Ui = value;
				Subscribe(m_Ui);

			}
		}

		/// <summary>
		/// Load the settings into the instace
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="actionFactory"></param>
		/// <param name="deviceFactory"></param>
		public override sealed void LoadSettings(TSettings settings, IButtonActionMapFactory actionFactory, IDeviceFactory deviceFactory)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");
			if (actionFactory == null)
				throw new ArgumentNullException("actionFactory");
			if (deviceFactory == null)
				throw new ArgumentNullException("deviceFactory");

			base.LoadSettings(settings, actionFactory, deviceFactory);

			var actionFactoryUi = actionFactory as IButtonActionMapFactory<TUi>;
			if (actionFactoryUi == null)
				throw new InvalidOperationException("Can't convert actionFactory into correct Ui action factory");

			Ui = actionFactoryUi.Ui;

			LoadSettingsFinal(settings, actionFactoryUi, deviceFactory);
		}

		/// <summary>
		/// Load the settings into this instance
		/// Final method to override using the generic IButtonActionFactory
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="actionFactory"></param>
		/// <param name="deviceFactory"></param>
		protected abstract void LoadSettingsFinal([NotNull] TSettings settings,
		                                          [NotNull] IButtonActionMapFactory<TUi> actionFactory,
		                                          [NotNull] IDeviceFactory deviceFactory);

		#region Ui Callbacks

		/// <summary>
		/// Subscribe to the UI
		/// </summary>
		/// <param name="ui"></param>
		protected virtual void Subscribe(TUi ui)
		{
		}

		/// <summary>
		/// Unsubscribe to the UI
		/// </summary>
		/// <param name="ui"></param>
		protected virtual void Unsubscribe(TUi ui)
		{
		}

		#endregion
	}

	public abstract class AbstractButtonAction<TSettings>: IButtonAction<TSettings>
		where TSettings : class, IButtonActionSettings
	{
		/// <summary>
		/// Name for the ActionMap
		/// </summary>
		public string Name { get; protected set; }

		/// <summary>
		/// If this action map should be serialized out to settings
		/// False for default button actions added automatically
		/// </summary>
		public bool Serialize { get; set; }

		/// <summary>
		/// Settings class the action map uses
		/// </summary>
		public Type SettingsClass { get { return typeof(TSettings); } }

		/// <summary>
		/// Handle a button press/release/etc
		/// This is what the ActionMap "does"
		/// </summary>
		/// <param name="action"></param>
		public abstract bool ButtonAction(eButtonAction action);

		/// <summary>
		/// Load the settings into the instace
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="actionFactory"></param>
		/// <param name="deviceFactory"></param>
		public virtual void LoadSettings(TSettings settings, IButtonActionMapFactory actionFactory, IDeviceFactory deviceFactory)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");
			if (actionFactory == null)
				throw new ArgumentNullException("actionFactory");
			if (deviceFactory == null)
				throw new ArgumentNullException("deviceFactory");

			Name = settings.Name;
		}

		/// <summary>
		/// Copy the instance to the settings
		/// </summary>
		/// <param name="settings"></param>
		public virtual void CopySettings(TSettings settings)
		{
			settings.Name = Name;
		}

		/// <summary>
		/// Load the settings into the instace
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="actionFactory"></param>
		/// <param name="deviceFactory"></param>
		void IButtonAction.LoadSettings(IButtonActionSettings settings, IButtonActionMapFactory actionFactory, IDeviceFactory deviceFactory)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");
			if (actionFactory == null)
				throw new ArgumentNullException("actionFactory");
			if (deviceFactory == null)
				throw new ArgumentNullException("deviceFactory");

			var castSettings = settings as TSettings;
			if (castSettings == null)
				throw new ArgumentException("Settings could not be converted to correct settings class for this device", "settings");
			LoadSettings(castSettings, actionFactory, deviceFactory);
		}

		/// <summary>
		/// Copy the instance to the settings
		/// </summary>
		/// <param name="settings"></param>
		void IButtonAction.CopySettings(IButtonActionSettings settings)
		{
			var castSettings = settings as TSettings;
			if (castSettings == null)
				throw new ArgumentException("Settings could not be converted to correct settings class for this device", "settings");
			CopySettings(castSettings);
		}
	}
}