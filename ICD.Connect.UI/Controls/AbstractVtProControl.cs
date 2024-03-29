﻿using System;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Connect.Panels;

namespace ICD.Connect.UI.Controls
{
	/// <summary>
	/// AbstractVtProControl is the base class for all VTPro controls.
	/// </summary>
	public abstract class AbstractVtProControl<T> : IVtProControl
		where T : class, ISigInputOutput
	{
		private readonly T m_Panel;
		private readonly IVtProParent m_Parent;
		private readonly ushort m_Index;

		private readonly SafeCriticalSection m_VisibilitySection;
		private readonly SafeCriticalSection m_EnabledSection;

		private bool? m_CachedVisibility;
		private bool? m_CachedEnabled;

		#region Properties

		/// <summary>
		/// Gets the panel this control is a part of.
		/// </summary>
		protected T Panel { get { return m_Panel; } }

		/// <summary>
		/// Gets/sets the digital visibility join.
		/// </summary>
		[PublicAPI]
		public ushort DigitalVisibilityJoin { get; set; }

		/// <summary>
		/// Gets/sets the digital enable join.
		/// </summary>
		[PublicAPI]
		public ushort DigitalEnableJoin { get; set; }

		/// <summary>
		/// Gets the visibility state of the control.
		/// </summary>
		public virtual bool IsVisible { get { return DigitalVisibilityJoin == 0 || (m_CachedVisibility ?? false); } }

		/// <summary>
		/// Returns true if this control, and all parent controls are visible.
		/// </summary>
		public bool IsVisibleRecursive { get { return IsVisible && (m_Parent == null || m_Parent.IsVisibleRecursive); } }

		/// <summary>
		/// Gets the enabled state of the control.
		/// </summary>
		public virtual bool IsEnabled { get { return DigitalEnableJoin == 0 || (m_CachedEnabled ?? false); } }

		/// <summary>
		/// Gets the index of the control in a list.
		/// </summary>
		public ushort Index { get { return m_Index; } }

		/// <summary>
		/// Gets/sets the parent for cumulative join offsets.
		/// </summary>
		public IJoinOffsets Parent { get { return m_Parent; } }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractVtProControl(T panel)
			: this(panel, null)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		protected AbstractVtProControl(T panel, IVtProParent parent)
			: this(panel, parent, 0)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		/// <param name="index"></param>
		protected AbstractVtProControl(T panel, IVtProParent parent, ushort index)
		{
			if (panel == null)
				throw new InvalidOperationException(string.Format("Creating {0} with null panel", GetType().Name));

			m_Panel = panel;
			m_Parent = parent;
			m_Index = index;

			m_VisibilitySection = new SafeCriticalSection();
			m_EnabledSection = new SafeCriticalSection();

			SubscribePanelFeedback();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		public virtual void Dispose()
		{
			UnsubscribePanelFeedback();
		}

		/// <summary>
		/// Enables/disables the button. Throws InvalidOperationException if there is no enable join.
		/// </summary>
		public virtual void Enable(bool state)
		{
			m_EnabledSection.Enter();

			try
			{
				if (!state && DigitalEnableJoin == 0)
					throw new InvalidOperationException(string.Format("Unable to disable {0}, join is 0", GetType().Name));

				if (state == IsEnabled)
					return;

				ushort join = GetDigitalJoinWithParentOffset(DigitalEnableJoin);

				m_CachedEnabled = state;
				Panel.SendInputDigital(join, state);
			}
			finally
			{
				m_EnabledSection.Leave();
			}
		}

		/// <summary>
		/// Shows/hides the control. Throws InvalidOperationException if there is no visibility join.
		/// </summary>
		/// <param name="state"></param>
		public virtual void Show(bool state)
		{
			m_VisibilitySection.Enter();

			try
			{
				if (!state && DigitalVisibilityJoin == 0)
					throw new InvalidOperationException(string.Format("Unable to hide {0}, join is 0", GetType().Name));

				if (state == IsVisible)
					return;

				ushort join = GetDigitalJoinWithParentOffset(DigitalVisibilityJoin);

				m_CachedVisibility = state;
				Panel.SendInputDigital(join, state);
			}
			finally
			{
				m_VisibilitySection.Leave();
			}
		}

		protected ushort GetSerialJoinWithParentOffset(ushort join)
		{
			return Parent == null ? join : Parent.GetSerialJoinWithOffset(join, this);
		}

		protected ushort GetAnalogJoinWithParentOffset(ushort join)
		{
			return Parent == null ? join : Parent.GetAnalogJoinWithOffset(join, this);
		}

		protected ushort GetDigitalJoinWithParentOffset(ushort join)
		{
			return Parent == null ? join : Parent.GetDigitalJoinWithOffset(join, this);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Subscribe to the panel events.
		/// </summary>
		protected virtual void SubscribePanelFeedback()
		{
		}

		/// <summary>
		/// Unsubscribe from the panel events.
		/// </summary>
		protected virtual void UnsubscribePanelFeedback()
		{
		}

		#endregion
	}
}
