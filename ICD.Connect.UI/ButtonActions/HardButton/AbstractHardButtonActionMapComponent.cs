using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Panels.HardButtons;
using ICD.Connect.Settings;
using ICD.Connect.UI.ButtonActions.Actions;
using ICD.Connect.UI.ButtonActions.Factories;

namespace ICD.Connect.UI.ButtonActions.HardButton
{
	public abstract class AbstractHardButtonActionMapComponent<TUi>
		where TUi : IHardButtonUserInterface
	{
		private readonly Dictionary<eHardButton, IButtonAction> m_ButtonBindings;
		private readonly SafeCriticalSection m_ButtonBindingsSection;
		private readonly IButtonActionMapFactory<TUi> m_Factory;

		public IButtonActionMapFactory<TUi> Factory { get { return m_Factory; } }

		protected AbstractHardButtonActionMapComponent(IButtonActionMapFactory<TUi> factory)
		{
			m_ButtonBindings = new Dictionary<eHardButton, IButtonAction>();
			m_ButtonBindingsSection = new SafeCriticalSection();

			m_Factory = factory;
		}

		public bool ButtonAction(eHardButton button, eButtonAction action)
		{
			IButtonAction binding = null;

			if (!TryGetButtonActionMap(button, out binding))
				return false;

			binding.ButtonAction(action);

			return true;
		}

		#region ButtonActionMap

		public bool ButtonHasActionMap(eHardButton button)
		{
			return m_ButtonBindingsSection.Execute(() => m_ButtonBindings.ContainsKey(button));
		}

		public bool TryGetButtonActionMap(eHardButton button, out IButtonAction action)
		{
			action = null;
			m_ButtonBindingsSection.Enter();
			try
			{
				return m_ButtonBindings.TryGetValue(button, out action);
			}
			finally
			{
				m_ButtonBindingsSection.Leave();
			}
		}

		public void SetButtonActionMap(eHardButton button, IButtonAction action)
		{
			m_ButtonBindingsSection.Enter();
			try
			{
				m_ButtonBindings[button] = action;
			}
			finally
			{
				m_ButtonBindingsSection.Leave();
			}
		}


		/// <summary>
		/// Adds button action maps only if there isn't an existing action map for the hard button
		/// </summary>
		/// <param name="button"></param>
		/// <param name="action"></param>
		/// <returns>True if added, false if not</returns>
		public bool TryAddButtonActionMap(eHardButton button, IButtonAction action)
		{
			m_ButtonBindingsSection.Enter();
			try
			{
				if (m_ButtonBindings.ContainsKey(button))
					return false;
				m_ButtonBindings.Add(button, action);
				return true;
			}
			finally
			{
				m_ButtonBindingsSection.Leave();
			}
		}

		/// <summary>
		/// Adds button action maps only if there isn't an existing action map for the hard button
		/// </summary>
		/// <param name="range"></param>
		public void TryAddButtonActionMapRange(IEnumerable<KeyValuePair<eHardButton, IButtonAction>> range)
		{
			m_ButtonBindingsSection.Enter();
			try
			{
				foreach (KeyValuePair<eHardButton, IButtonAction> kvp in range)
					TryAddButtonActionMap(kvp.Key, kvp.Value);
			}
			finally
			{
				m_ButtonBindingsSection.Leave();
			}
		}

		public void ClearActionMaps()
		{
			m_ButtonBindingsSection.Execute(() => m_ButtonBindings.Clear());
		}

		public IEnumerable<KeyValuePair<eHardButton, IButtonAction>> GetButtonActionMaps()
		{
			m_ButtonBindingsSection.Enter();
			try
			{
				return m_ButtonBindings.ToArray(m_ButtonBindings.Count);
			}
			finally
			{
				m_ButtonBindingsSection.Leave();
			}
		}

		#endregion

		#region DefaultActionMaps

		protected void AddDefaultActionMaps(IDeviceFactory deviceFactory)
		{
			TryAddButtonActionMapRange(GetDefaultActionMaps(deviceFactory));
		}

		protected abstract IEnumerable<KeyValuePair<eHardButton, IButtonAction>> GetDefaultActionMaps(IDeviceFactory deviceFactory);

		#endregion

		#region Settings

		public virtual void LoadSettings(IHardButtonActionMapComponentSettings settings, IDeviceFactory deviceFactory)
		{
			var actionMaps = settings.GetActionMapSettings();
			foreach (var kvp in actionMaps)
			{
				eHardButton button = kvp.Key;

				IButtonActionSettings buttonActionSetting = kvp.Value;

				IButtonAction buttonAction = ReflectionUtils.CreateInstance<IButtonAction>(buttonActionSetting.ButtonActionType);

				buttonAction.LoadSettings(buttonActionSetting, Factory, deviceFactory);

				SetButtonActionMap(button, buttonAction);
			}
		}

		public virtual void CopySettings([NotNull] IHardButtonActionMapComponentSettings settings)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");

			IEnumerable<KeyValuePair<eHardButton, IButtonActionSettings>> actionMapSettings =
				GetButtonActionMaps()
					.Select(
					        kvp =>
					        new KeyValuePair<eHardButton, IButtonActionSettings>(kvp.Key,
					                                                             ButtonActionMapUtils.GetSettingsCopyForButtonActionMap
						                                                             (kvp.Value)));

			settings.SetActionMapSettings(actionMapSettings);
		}

		public virtual void ClearSettings()
		{
			ClearActionMaps();
		}

		#endregion
	}
}