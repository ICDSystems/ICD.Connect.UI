using System;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Timers;
using ICD.Connect.Panels;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Protocol.Sigs;
using ICD.Connect.UI.Controls.TextControls;

namespace ICD.Connect.UI.Controls.Buttons
{
	public abstract class AbstractVtProButton : AbstractVtProLabel
	{
		/// <summary>
		/// Raised when the user presses the button.
		/// </summary>
		public event EventHandler OnPressed;

		/// <summary>
		/// Raised when the user releases the button.
		/// </summary>
		public event EventHandler OnReleased;

		/// <summary>
		/// Raised when the user holds the button.
		/// </summary>
		public event EventHandler OnHeld;

		private readonly SafeTimer m_HoldTimer;
		private readonly SafeCriticalSection m_SelectSection;

		private ushort m_DigitalPressJoin;
		private ushort m_SubscribedDigitalPressJoin;

		private bool m_SelectedCache;

		#region Properties

		/// <summary>
		/// Gets the digital press join.
		/// </summary>
		[PublicAPI]
		public ushort DigitalPressJoin
		{
			get { return m_DigitalPressJoin; }
			set
			{
				if (value == m_DigitalPressJoin)
					return;

				UnsubscribeDigitalFeedback();
				m_DigitalPressJoin = value;
				SubscribeDigitalFeedback();
			}
		}

		/// <summary>
		/// Gets/sets the hold duration in milliseconds.
		/// </summary>
		[PublicAPI]
		public long HoldDuration { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		protected AbstractVtProButton(ISigInputOutput panel)
			: this(panel, null)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		protected AbstractVtProButton(ISigInputOutput panel, IVtProParent parent)
			: base(panel, parent)
		{
			m_HoldTimer = SafeTimer.Stopped(Hold);
			m_SelectSection = new SafeCriticalSection();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		public override void Dispose()
		{
			OnPressed = null;
			OnReleased = null;
			OnHeld = null;

			m_HoldTimer.Dispose();

			base.Dispose();
		}

		/// <summary>
		/// Simulates a button press.
		/// </summary>
		[PublicAPI]
		public void Press()
		{
			long holdDuration = HoldDuration;
			if (holdDuration > 0)
				m_HoldTimer.Reset(holdDuration);

			if (IsVisibleRecursive)
				OnPressed.Raise(this);
		}

		/// <summary>
		/// Simulates a button release.
		/// </summary>
		[PublicAPI]
		public void Release()
		{
			m_HoldTimer.Stop();

			if (IsVisibleRecursive)
				OnReleased.Raise(this);
		}

		/// <summary>
		/// Simulates the button being held.
		/// </summary>
		[PublicAPI]
		public void Hold()
		{
			m_HoldTimer.Stop();

			if (IsVisibleRecursive)
				OnHeld.Raise(this);
		}

		/// <summary>
		/// Sets the selected state of the button.
		/// </summary>
		/// <param name="state"></param>
		public void SetSelected(bool state)
		{
			m_SelectSection.Enter();

			try
			{
				if (DigitalPressJoin == 0)
					throw new InvalidOperationException("Unable to set selected state, join is 0");

				if (state == m_SelectedCache)
					return;

				ushort join = GetDigitalJoinWithParentOffset(DigitalPressJoin);

				m_SelectedCache = state;
				Panel.SendInputDigital(join, m_SelectedCache);
			}
			finally
			{
				m_SelectSection.Leave();
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Subscribe to the panel events.
		/// </summary>
		protected override void SubscribePanelFeedback()
		{
			base.SubscribePanelFeedback();

			SubscribeDigitalFeedback();
		}

		/// <summary>
		/// Unsubscribe from the panel events.
		/// </summary>
		protected override void UnsubscribePanelFeedback()
		{
			base.UnsubscribePanelFeedback();

			UnsubscribeDigitalFeedback();
		}

		/// <summary>
		/// Unsubscribes from the previous subscribed join.
		/// </summary>
		private void UnsubscribeDigitalFeedback()
		{
			if (m_SubscribedDigitalPressJoin != 0)
				Panel.UnregisterOutputSigChangeCallback(m_SubscribedDigitalPressJoin, eSigType.Digital, ButtonPressChange);
			m_SubscribedDigitalPressJoin = 0;
		}

		/// <summary>
		/// Unsubscribes from the previous subscribed join and subscribes to the current join.
		/// </summary>
		private void SubscribeDigitalFeedback()
		{
			UnsubscribeDigitalFeedback();

			m_SubscribedDigitalPressJoin = GetDigitalJoinWithParentOffset(DigitalPressJoin);
			if (m_SubscribedDigitalPressJoin != 0)
				Panel.RegisterOutputSigChangeCallback(m_SubscribedDigitalPressJoin, eSigType.Digital, ButtonPressChange);
		}

		/// <summary>
		/// Called when the press join state changes.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="args"></param>
		private void ButtonPressChange(object parent, SigInfoEventArgs args)
		{
			if (DigitalPressJoin == 0)
				return;

			if (args.Data.GetBoolValue())
				Press();
			else
				Release();
		}

		#endregion
	}
}
