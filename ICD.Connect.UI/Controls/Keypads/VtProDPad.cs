using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Timers;
using ICD.Connect.Panels.Devices;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.SmartObjects;
using ICD.Connect.Protocol.Sigs;
using ICD.Connect.UI.EventArguments;

namespace ICD.Connect.UI.Controls.Keypads
{
	public sealed class VtProDPad : AbstractVtProSmartControl
	{
		private const ushort UP = 1;
		private const ushort DOWN = 2;
		private const ushort LEFT = 3;
		private const ushort RIGHT = 4;
		private const ushort CENTER = 5;

		/// <summary>
		/// Raised when a button is pressed.
		/// </summary>
		public event EventHandler<DPadEventArgs> OnButtonPressed;

		/// <summary>
		/// Raised when a button is released.
		/// </summary>
		public event EventHandler<DPadEventArgs> OnButtonReleased;

		/// <summary>
		/// Raised when a button is held.
		/// </summary>
		public event EventHandler<DPadEventArgs> OnButtonHeld;

		private readonly Dictionary<DPadEventArgs.eDirection, SafeTimer> m_HoldTimers;

		/// <summary>
		/// Gets/sets the hold duration in milliseconds.
		/// </summary>
		[PublicAPI]
		public long HoldDuration { get; set; }

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="smartObjectId"></param>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		public VtProDPad(uint smartObjectId, IPanelDevice panel, IVtProParent parent)
			: base(smartObjectId, panel, parent)
		{
			m_HoldTimers = new Dictionary<DPadEventArgs.eDirection, SafeTimer>();

			foreach (DPadEventArgs.eDirection direction in EnumUtils.GetValues<DPadEventArgs.eDirection>())
			{
				DPadEventArgs.eDirection directionClosure = direction;
				m_HoldTimers.Add(direction, SafeTimer.Stopped(() => HoldCallback(directionClosure)));
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		public override void Dispose()
		{
			OnButtonPressed = null;
			OnButtonReleased = null;
			OnButtonHeld = null;

			base.Dispose();
		}

		/// <summary>
		/// Simulates a Press on the directional button.
		/// </summary>
		/// <param name="direction"></param>
		[PublicAPI]
		public void Press(DPadEventArgs.eDirection direction)
		{
			long holdDuration = HoldDuration;
			if (holdDuration > 0)
				m_HoldTimers[direction].Reset(holdDuration);

			if (IsVisibleRecursive)
				OnButtonPressed.Raise(this, new DPadEventArgs(direction));
		}

		/// <summary>
		/// Simulates a release on the directional button.
		/// </summary>
		/// <param name="direction"></param>
		[PublicAPI]
		public void Release(DPadEventArgs.eDirection direction)
		{
			m_HoldTimers[direction].Stop();

			if (IsVisibleRecursive)
				OnButtonReleased.Raise(this, new DPadEventArgs(direction));
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Called when a hold timer elapses.
		/// </summary>
		/// <param name="direction"></param>
		private void HoldCallback(DPadEventArgs.eDirection direction)
		{
			m_HoldTimers[direction].Stop();

			if (IsVisibleRecursive)
				OnButtonHeld.Raise(this, new DPadEventArgs(direction));
		}

		#endregion

		#region SmartObject Callbacks

		/// <summary>
		/// Subscribe to the smart object events.
		/// </summary>
		/// <param name="smartObject"></param>
		protected override void Subscribe(ISmartObject smartObject)
		{
			base.Subscribe(smartObject);

			smartObject.RegisterOutputSigChangeCallback(UP, eSigType.Digital, UpPressed);
			smartObject.RegisterOutputSigChangeCallback(DOWN, eSigType.Digital, DownPressed);
			smartObject.RegisterOutputSigChangeCallback(LEFT, eSigType.Digital, LeftPressed);
			smartObject.RegisterOutputSigChangeCallback(RIGHT, eSigType.Digital, RightPressed);
			smartObject.RegisterOutputSigChangeCallback(CENTER, eSigType.Digital, CenterPressed);
		}

		/// <summary>
		/// Unsubscribe from the smart object events.
		/// </summary>
		/// <param name="smartObject"></param>
		protected override void Unsubscribe(ISmartObject smartObject)
		{
			base.Unsubscribe(smartObject);

			smartObject.RegisterOutputSigChangeCallback(UP, eSigType.Digital, UpPressed);
			smartObject.RegisterOutputSigChangeCallback(DOWN, eSigType.Digital, DownPressed);
			smartObject.RegisterOutputSigChangeCallback(LEFT, eSigType.Digital, LeftPressed);
			smartObject.RegisterOutputSigChangeCallback(RIGHT, eSigType.Digital, RightPressed);
			smartObject.RegisterOutputSigChangeCallback(CENTER, eSigType.Digital, CenterPressed);
		}

		/// <summary>
		/// Called when the up button is pressed.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="args"></param>
		private void UpPressed(object parent, SigInfoEventArgs args)
		{
			Press(DPadEventArgs.eDirection.Up, args);
		}

		/// <summary>
		/// Called when the down button is pressed.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="args"></param>
		private void DownPressed(object parent, SigInfoEventArgs args)
		{
			Press(DPadEventArgs.eDirection.Down, args);
		}

		/// <summary>
		/// Called when the left button is pressed.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="args"></param>
		private void LeftPressed(object parent, SigInfoEventArgs args)
		{
			Press(DPadEventArgs.eDirection.Left, args);
		}

		/// <summary>
		/// Called when the right button is pressed.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="args"></param>
		private void RightPressed(object parent, SigInfoEventArgs args)
		{
			Press(DPadEventArgs.eDirection.Right, args);
		}

		/// <summary>
		/// Called when the center button is pressed.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="args"></param>
		private void CenterPressed(object parent, SigInfoEventArgs args)
		{
			Press(DPadEventArgs.eDirection.Center, args);
		}

		private void Press(DPadEventArgs.eDirection direction, SigInfoEventArgs args)
		{
			if (args.Data.GetBoolValue())
				Press(direction);
			else
				Release(direction);
		}

		#endregion
	}
}
