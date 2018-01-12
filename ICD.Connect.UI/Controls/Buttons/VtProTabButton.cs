using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Panels;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.SmartObjects;
using ICD.Connect.Protocol.Sigs;

namespace ICD.Connect.UI.Controls.Buttons
{
	public sealed class VtProTabButton : AbstractVtProSmartObject
	{
		public event EventHandler<UShortEventArgs> OnButtonPressed;
		public event EventHandler<UShortEventArgs> OnButtonReleased;

		// Bool input
		private const ushort DIGITAL_SELECT_START_JOIN = 2;

		// Bool output
		private const ushort DIGITAL_PRESS_START_JOIN = 1;

		private const ushort DIGITAL_INCREMENT = 2;

		private readonly Dictionary<ushort, bool> m_SelectionCache;
		private readonly SafeCriticalSection m_SelectionCacheSection;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="smartObjectId"></param>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		public VtProTabButton(uint smartObjectId, IPanelDevice panel, IVtProParent parent)
			: base(smartObjectId, panel, parent)
		{
			m_SelectionCache = new Dictionary<ushort, bool>();
			m_SelectionCacheSection = new SafeCriticalSection();
		}

		/// <summary>
		/// Release resources.
		/// </summary>
		public override void Dispose()
		{
			OnButtonPressed = null;
			OnButtonReleased = null;

			base.Dispose();
		}

		#region Methods

		/// <summary>
		/// Simulates a Press on the button at the given index.
		/// </summary>
		/// <param name="index"></param>
		[PublicAPI]
		public void Press(ushort index)
		{
			if (IsVisibleRecursive)
				OnButtonPressed.Raise(this, new UShortEventArgs(index));
		}

		/// <summary>
		/// Simulates a release on the button at the given index.
		/// </summary>
		/// <param name="index"></param>
		[PublicAPI]
		public void Release(ushort index)
		{
			if (IsVisibleRecursive)
				OnButtonReleased.Raise(this, new UShortEventArgs(index));
		}

		/// <summary>
		/// Sets the selected state for the button at the given index.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="selected"></param>
		[PublicAPI]
		public void SetItemSelected(ushort index, bool selected)
		{
			m_SelectionCacheSection.Enter();

			try
			{
				if (selected == m_SelectionCache.GetDefault(index, false))
					return;

				m_SelectionCache[index] = selected;

				ushort join = (ushort)((index * DIGITAL_INCREMENT) + DIGITAL_SELECT_START_JOIN);
				SmartObject.SendInputDigital(join, selected);
			}
			finally
			{
				m_SelectionCacheSection.Leave();
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Subscribe to the smart object events.
		/// </summary>
		/// <param name="smartObject"></param>
		protected override void Subscribe(ISmartObject smartObject)
		{
			base.Subscribe(smartObject);

			smartObject.OnAnyOutput += SmartObjectOnAnyOutput;
		}

		/// <summary>
		/// Unsubscribe from the smart object events.
		/// </summary>
		/// <param name="smartObject"></param>
		protected override void Unsubscribe(ISmartObject smartObject)
		{
			base.Unsubscribe(smartObject);

			smartObject.OnAnyOutput -= SmartObjectOnAnyOutput;
		}

		/// <summary>
		/// Called when the SmartObject raises an output event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void SmartObjectOnAnyOutput(object sender, SigInfoEventArgs args)
		{
			SigInfo sigInfo = args.Data;
			if (sigInfo.Type != eSigType.Digital)
				return;

			ushort pressIndex = (ushort)((sigInfo.Number - DIGITAL_PRESS_START_JOIN) / DIGITAL_INCREMENT);
			bool press = sigInfo.GetBoolValue();

			if (press)
				Press(pressIndex);
			else
				Release(pressIndex);
		}

		#endregion
	}
}
