﻿using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Panels;
using ICD.Connect.UI.Controls;
using ICD.Connect.UI.Controls.Lists;
using ICD.Connect.UI.Controls.Pages;

namespace ICD.Connect.UI.Mvp.Views
{
	/// <summary>
	/// Base class for all of the views.
	/// </summary>
	public abstract class AbstractView : IView
	{
		/// <summary>
		/// Raised when the view is about to be shown or hidden.
		/// </summary>
		public event EventHandler<BoolEventArgs> OnPreVisibilityChanged;

		/// <summary>
		/// Raised when the view is shown or hidden.
		/// </summary>
		public event EventHandler<BoolEventArgs> OnVisibilityChanged;

		/// <summary>
		/// Raised when the view enabled state changes.
		/// </summary>
		public event EventHandler<BoolEventArgs> OnEnabledChanged;

		private readonly ISigInputOutput m_Panel;
		private readonly IVtProParent m_Parent;
		private readonly ushort m_Index;

		#region Properties

		/// <summary>
		/// Returns true if the view is visible.
		/// </summary>
		public bool IsVisible { get { return Page == null || Page.IsVisible; } }

		/// <summary>
		/// Returns true if the view is enabled.
		/// </summary>
		public bool IsEnabled { get { return Page == null || Page.IsEnabled; } }

		/// <summary>
		/// Gets the page control.
		/// </summary>
		[CanBeNull]
		public IVtProControl Page { get; private set; }

		/// <summary>
		/// Gets the wrapped panel.
		/// </summary>
		public ISigInputOutput Panel { get { return m_Panel; } }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		protected AbstractView(ISigInputOutput panel)
			: this(panel, null, 0)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		/// <param name="index"></param>
		protected AbstractView(ISigInputOutput panel, IVtProParent parent, ushort index)
		{
			m_Panel = panel;
			m_Parent = parent;
			m_Index = index;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Needs to be called after instantiation to create the child controls.
		/// </summary>
		public void Initialize()
		{
			InstantiateControls(m_Panel, m_Parent, m_Index);

			Page = GetChildren().FirstOrDefault(c => c is VtProSubpage || c is VtProPage);

			SubscribeControls();
		}

		/// <summary>
		/// Release resources.
		/// </summary>
		public virtual void Dispose()
		{
			OnVisibilityChanged = null;
			OnEnabledChanged = null;

			UnsubscribeControls();

			foreach (IVtProControl control in GetChildren())
				control.Dispose();
		}

		/// <summary>
		/// Sets the visibility of the view.
		/// </summary>
		/// <param name="visible"></param>
		public virtual void Show(bool visible)
		{
			if (Page == null)
				throw new ArgumentException("Unable to set visibility of view with no page");

			if (visible == IsVisible)
				return;

			OnPreVisibilityChanged.Raise(this, new BoolEventArgs(visible));

			try
			{
				Page.Show(visible);
			}
			catch (Exception e)
			{
				string error = string.Format("Unable to show {0} - {1}", GetType().Name, e.Message);
				throw new InvalidOperationException(error, e);
			}

			OnVisibilityChanged.Raise(this, new BoolEventArgs(visible));
		}

		/// <summary>
		/// Sets the enabled state of the view.
		/// </summary>
		/// <param name="enabled"></param>
		public void Enable(bool enabled)
		{
			if (Page == null)
				throw new ArgumentException("Unable to set enabled state of view with no page");

			if (enabled == IsEnabled)
				return;

			try
			{
				Page.Enable(enabled);
			}
			catch (Exception e)
			{
				string error = string.Format("Unable to enable {0} - {1}", GetType().Name, e.Message);
				throw new InvalidOperationException(error, e);
			}

			OnEnabledChanged.Raise(this, new BoolEventArgs(enabled));
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Instantiates the view controls.
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		/// <param name="index"></param>
		protected abstract void InstantiateControls(ISigInputOutput panel, IVtProParent parent, ushort index);

		/// <summary>
		/// Gets the child controls.
		/// </summary>
		/// <returns></returns>
		protected abstract IEnumerable<IVtProControl> GetChildren();

		/// <summary>
		/// Generates child views for the given subpage reference list. Sets the number of items in the SRL.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="factory"></param>
		/// <param name="subpageReferenceList"></param>
		/// <param name="viewList"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		protected static IEnumerable<T> GetChildViews<T>(IViewFactory factory, VtProSubpageReferenceList subpageReferenceList,
		                                                 List<T> viewList, ushort count)
			where T : class, IView
		{
			return factory.LazyLoadSrlViews(subpageReferenceList, viewList, count);
		}

		/// <summary>
		/// Subscribes to the view controls.
		/// </summary>
		protected virtual void SubscribeControls()
		{
		}

		/// <summary>
		/// Unsubscribes from the view controls.
		/// </summary>
		protected virtual void UnsubscribeControls()
		{
		}

		#endregion
	}
}
