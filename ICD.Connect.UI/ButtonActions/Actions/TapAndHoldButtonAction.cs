using ICD.Common.Properties;
using ICD.Common.Utils.Timers;
using ICD.Connect.Protocol.HardButtons;
using ICD.Connect.Settings;
using ICD.Connect.UI.ButtonActions.Factories;

namespace ICD.Connect.UI.ButtonActions.Actions
{
	public sealed class TapAndHoldButtonAction : AbstractButtonAction<TapAndHoldButtonActionSettings>
	{
		public long HoldTime { get; private set; }
		public bool TapExclusive { get; private set; }

		[CanBeNull]
		public IButtonAction TapAction { get; private set; }

		[CanBeNull]
		public IButtonAction HoldAction { get; private set; }

		private readonly SafeTimer m_PressTimer;
		private bool m_PressTimerRunning;

		public TapAndHoldButtonAction()
		{
		}

		public TapAndHoldButtonAction(string name, long holdTime, bool tapExclusive, IButtonAction tapAction, IButtonAction holdAction)
		{
			m_PressTimer = SafeTimer.Stopped(PressTimerExpired);
			Name = name;
			HoldTime = holdTime;
			TapExclusive = tapExclusive;
			TapAction = tapAction;
			HoldAction = holdAction;
		}

		private void PressTimerExpired()
		{
			m_PressTimerRunning = false;
			if (HoldAction != null)
				HoldAction.ButtonAction(eButtonAction.Hold);
		}

		public override bool ButtonAction(eButtonAction action)
		{
			switch (action)
			{
				case eButtonAction.Tap:
					if (TapAction != null)
						return TapAction.ButtonAction(eButtonAction.Tap);
					break;
				case eButtonAction.Hold:
					if (HoldAction != null)
						return HoldAction.ButtonAction(eButtonAction.Hold);
					break;
				case eButtonAction.HoldRelease:
					if (HoldAction != null)
						return HoldAction.ButtonAction(eButtonAction.HoldRelease);
					break;
				case eButtonAction.Press:
					StartPressTimer();
					return true;
				case eButtonAction.Release:
					ProcessRelease();
					return true;
			}

			return false;
		}

		private void StartPressTimer()
		{
			m_PressTimerRunning = true;
			m_PressTimer.Reset(HoldTime);
			if (!TapExclusive && TapAction != null)
				TapAction.ButtonAction(eButtonAction.Tap);
		}

		private void ProcessRelease()
		{
			if (m_PressTimerRunning)
			{
				m_PressTimer.Stop();
				m_PressTimerRunning = false;
				if (TapExclusive && TapAction != null)
					TapAction.ButtonAction(eButtonAction.Tap);
			}
			else
			{
				if (HoldAction != null)
					HoldAction.ButtonAction(eButtonAction.HoldRelease);
			}
		}

		public override void LoadSettings(TapAndHoldButtonActionSettings settings, IButtonActionMapFactory actionFactory, IDeviceFactory deviceFactory)
		{
			base.LoadSettings(settings, actionFactory, deviceFactory);

			HoldTime = settings.HoldTime;
			TapExclusive = settings.TapExclusive;

			if (!string.IsNullOrEmpty(settings.TapActionMapType))
				TapAction = actionFactory.InstantiateButtonActionFromXml(deviceFactory, settings.TapActionMapType, settings.TapActionMapXml);

			if (!string.IsNullOrEmpty(settings.HoldActionMapType))
				HoldAction = actionFactory.InstantiateButtonActionFromXml(deviceFactory, settings.HoldActionMapType, settings.HoldActionMapXml);
		}

		public override void CopySettings(TapAndHoldButtonActionSettings settings)
		{
			base.CopySettings(settings);

			if (TapAction != null)
			{
				IButtonActionSettings tapSettings = ButtonActionUtils.GetSettingsCopyForButtonAction(TapAction);

				settings.TapActionMapType = tapSettings.FactoryName;
				settings.TapActionMapXml = ButtonActionUtils.GetXmlContentForSettings(tapSettings);
			}

			if (HoldAction != null)
			{
				IButtonActionSettings holdSettings = ButtonActionUtils.GetSettingsCopyForButtonAction(HoldAction);

				settings.HoldActionMapType = holdSettings.FactoryName;
				settings.HoldActionMapXml = ButtonActionUtils.GetXmlContentForSettings(holdSettings);
			}

		}

	}
}