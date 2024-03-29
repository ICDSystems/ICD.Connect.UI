﻿using System;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Panels.Devices;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.SmartObjects;
using ICD.Connect.Protocol.Sigs;

namespace ICD.Connect.UI.Controls.Lists
{
	public abstract class AbstractVtProList : AbstractVtProSmartControl
	{
		/// <summary>
		/// Raised when the user starts or stops scrolling the list.
		/// </summary>
		public event EventHandler<BoolEventArgs> OnIsMovingChanged;

		private readonly SafeCriticalSection m_NumberOfItemsSection;

		private ushort? m_ItemCountCache;

		#region Properties

		/// <summary>
		/// The maximum number of items in the list.
		/// </summary>
		public ushort MaxSize { get; set; }

		/// <summary>
		/// Gets the join number for setting the number of items.
		/// </summary>
		protected abstract ushort AnalogNumberOfItemsJoin { get; }

		/// <summary>
		/// Gets the join number for scrolling to an item in the list.
		/// </summary>
		protected abstract ushort AnalogScrollToItemJoin { get; }

		/// <summary>
		/// Gets the join number for getting the moving state of the list.
		/// </summary>
		protected abstract ushort DigitalIsMovingOutputJoin { get; }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="smartObjectId"></param>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		protected AbstractVtProList(uint smartObjectId, IPanelDevice panel, IVtProParent parent)
			: base(smartObjectId, panel, parent)
		{
			m_NumberOfItemsSection = new SafeCriticalSection();
		}

		#region Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		public override void Dispose()
		{
			OnIsMovingChanged = null;

			base.Dispose();
		}

		/// <summary>
		/// Scrolls to the given item in the list.
		/// </summary>
		/// <param name="item"></param>
		public void ScrollToItem(ushort item)
		{
			if (AnalogScrollToItemJoin == 0)
				throw new InvalidOperationException("Unable to scroll to item, join is 0");

			ushort join = GetAnalogJoinWithParentOffset(AnalogScrollToItemJoin);

			// Hack to bypass the cache
			SmartObject.SendInputAnalog(join, (ushort)(item + 2));
			SmartObject.SendInputAnalog(join, (ushort)(item + 1));
		}

		/// <summary>
		/// Sets the number of items in the list.
		/// </summary>
		/// <param name="count"></param>
		public void SetNumberOfItems(ushort count)
		{
			m_NumberOfItemsSection.Enter();

			try
			{
				if (AnalogNumberOfItemsJoin == 0)
					throw new InvalidOperationException("Unable to set number of items, join is 0");

				if (count == m_ItemCountCache)
					return;

				ushort join = GetAnalogJoinWithParentOffset(AnalogNumberOfItemsJoin);

				m_ItemCountCache = count;
				SmartObject.SendInputAnalog(join, count);

				// Fixes scrolling issues (list appearing empty) after clearing and then repopulating list
				if (m_ItemCountCache == 0)
					ScrollToItem(0);
			}
			finally
			{
				m_NumberOfItemsSection.Leave();
			}
		}

		/// <summary>
		/// Simulates the user starting or stopping scrolling the list.
		/// </summary>
		/// <param name="isMoving"></param>
		[PublicAPI]
		public void SetIsMoving(bool isMoving)
		{
			if (IsVisibleRecursive)
				OnIsMovingChanged.Raise(this, new BoolEventArgs(isMoving));
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

			if (DigitalIsMovingOutputJoin != 0)
				SmartObject.RegisterOutputSigChangeCallback(DigitalIsMovingOutputJoin, eSigType.Digital, IsMovingChange);
		}

		/// <summary>
		/// Unsubscribe from the smart object events.
		/// </summary>
		/// <param name="smartObject"></param>
		protected override void Unsubscribe(ISmartObject smartObject)
		{
			base.Unsubscribe(smartObject);

			if (DigitalIsMovingOutputJoin != 0)
				SmartObject.UnregisterOutputSigChangeCallback(DigitalIsMovingOutputJoin, eSigType.Digital, IsMovingChange);
		}

		/// <summary>
		/// Called when the press join state changes.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="args"></param>
		private void IsMovingChange(object parent, SigInfoEventArgs args)
		{
			if (DigitalIsMovingOutputJoin == 0)
				return;

			SetIsMoving(args.Data.GetBoolValue());
		}

		#endregion
	}
}
