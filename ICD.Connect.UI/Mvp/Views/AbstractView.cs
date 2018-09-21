using System;
using System.Collections.Generic;
using System.Linq;
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
		public event EventHandler<BoolEventArgs> OnVisibilityChanged;
		public event EventHandler<BoolEventArgs> OnEnabledChanged;

		private readonly ISigInputOutput m_Panel;

		private IVtProControl m_CachedPage;

		/// <summary>
		/// Returns true if the view is visible.
		/// </summary>
		public bool IsVisible { get { return Page.IsVisible; } }

		/// <summary>
		/// Returns true if the view is enabled.
		/// </summary>
		public bool IsEnabled { get { return Page.IsEnabled; } }

		/// <summary>
		/// Gets the page control.
		/// </summary>
		public IVtProControl Page
		{
			get
			{
				return m_CachedPage ?? (m_CachedPage = GetChildren().FirstOrDefault(c => c is VtProSubpage || c is VtProPage));
			}
		}

		public ISigInputOutput Panel { get { return m_Panel; } }

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

			InstantiateControls(panel, parent, index);
			SubscribeControls();
		}

		#endregion

		#region Methods

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
			if (visible == IsVisible)
				return;

			try
			{
				Page.Show(visible);
			}
			catch (Exception e)
			{
				string error = string.Format("Unable to show {0} - {1}", GetType().Name, e.Message);
				throw new Exception(error, e);
			}

			OnVisibilityChanged.Raise(this, new BoolEventArgs(visible));
		}

		/// <summary>
		/// Sets the enabled state of the view.
		/// </summary>
		/// <param name="enabled"></param>
		public void Enable(bool enabled)
		{
			if (enabled == IsEnabled)
				return;

			try
			{
				Page.Enable(enabled);
			}
			catch (Exception e)
			{
				string error = string.Format("Unable to enable {0} - {1}", GetType().Name, e.Message);
				throw new Exception(error, e);
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
			return factory.GetNewSrlViews(subpageReferenceList, viewList, count);
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
