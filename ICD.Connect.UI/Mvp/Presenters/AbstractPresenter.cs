using System;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.UI.Mvp.Views;

namespace ICD.Connect.UI.Mvp.Presenters
{
	/// <summary>
	/// Base class for all presenters.
	/// </summary>
	public abstract class AbstractPresenter<T> : IPresenter<T>
		where T : class, IView
	{
		/// <summary>
		/// Raised when the view visibility is about to change.
		/// </summary>
		public event EventHandler<BoolEventArgs> OnViewPreVisibilityChanged;

		/// <summary>
		/// Raised when the view visibility changes.
		/// </summary>
		public event EventHandler<BoolEventArgs> OnViewVisibilityChanged;

		private readonly INavigationController m_Navigation;
		private readonly IViewFactory m_ViewFactory;
		private readonly SafeCriticalSection m_ViewSection;

		private T m_View;

		#region Properties

		/// <summary>
		/// Gets the navigation controller.
		/// </summary>
		protected INavigationController Navigation { get { return m_Navigation; } }

		/// <summary>
		/// Gets the view factory.
		/// </summary>
		protected IViewFactory ViewFactory { get { return m_ViewFactory; } }

		/// <summary>
		/// Gets the state of the view visibility.
		/// </summary>
		public bool IsViewVisible { get { return m_ViewSection.Execute(() => m_View != null && m_View.IsVisible); } }

		/// <summary>
		/// Returns true if this presenter is part of a collection of components.
		/// </summary>
		private bool IsComponent { get { return this is IComponentPresenter; } }

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="nav"></param>
		/// <param name="views"></param>
		protected AbstractPresenter(INavigationController nav, IViewFactory views)
		{
			if (nav == null)
				throw new ArgumentNullException("nav");

			if (views == null)
				throw new ArgumentNullException("views");

			m_Navigation = nav;
			m_ViewFactory = views;
			m_ViewSection = new SafeCriticalSection();
		}

		#region Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		public virtual void Dispose()
		{
			OnViewPreVisibilityChanged = null;
			OnViewVisibilityChanged = null;

			m_ViewSection.Enter();

			try
			{
				if (m_View == null)
					return;

				Unsubscribe(m_View);
				m_View.Dispose();
			}
			finally
			{
				m_ViewSection.Leave();
			}
		}

		/// <summary>
		/// Gets the view for this presenter.
		/// </summary>
		/// <returns></returns>
		[CanBeNull]
		public T GetView()
		{
			return GetView(!IsComponent);
		}

		/// <summary>
		/// Sets the current view to null.
		/// </summary>
		public void ClearView()
		{
			SetView(null);
		}

		/// <summary>
		/// Disposes the old view and sets the new view.
		/// </summary>
		/// <param name="view"></param>
		public virtual void SetView(T view)
		{
			bool hide = false;

			m_ViewSection.Enter();

			try
			{
				if (view == m_View)
					return;

				// Special case - Can't set an SRL count to 0 (yay) so hide when cleaning up items.
				if (view == null && m_View != null)
					hide = true;

				if (m_View != null)
					Unsubscribe(m_View);

				m_View = view;

				if (m_View != null)
					Subscribe(m_View);
			}
			finally
			{
				m_ViewSection.Leave();
			}

			if (hide)
				ShowView(false);

			RefreshIfVisible();
		}

		/// <summary>
		/// Sets the visibility of the presenters view.
		/// </summary>
		/// <param name="visible"></param>
		public void ShowView(bool visible)
		{
			T view;

			m_ViewSection.Enter();

			try
			{
				// Don't bother creating the view just to hide it.
				if (m_View == null && !visible)
					return;

				view = GetView();
				if (view == null)
					throw new InvalidOperationException("Presenter has no view");
			}
			finally
			{
				m_ViewSection.Leave();
			}

			view.Show(visible);
		}

		/// <summary>
		/// Sets the enabled state of the view.
		/// </summary>
		/// <param name="enabled"></param>
		public void SetViewEnabled(bool enabled)
		{
			T view;

			m_ViewSection.Enter();

			try
			{
				// Don't bother creating the view just to disable it.
				if (m_View == null && !enabled)
					return;

				view = GetView();
				if (view == null)
					throw new InvalidOperationException("Presenter has no view");
			}
			finally
			{
				m_ViewSection.Leave();
			}

			view.Enable(enabled);
		}

		/// <summary>
		/// Updates the view.
		/// </summary>
		[PublicAPI]
		public virtual void Refresh()
		{
			T view = GetView();

			// Don't refresh if we currently have no view.
			if (view == null)
				return;

			try
			{
				Refresh(view);
			}
			catch (Exception e)
			{
				ServiceProvider.GetService<ILoggerService>()
				               .AddEntry(eSeverity.Error, e, "{0} Failed to Refresh - {1}", GetType().Name, e.Message);
			}
		}

		/// <summary>
		/// Refreshes the view asynchronously.
		/// </summary>
		[PublicAPI]
		public void RefreshAsync()
		{
			ThreadingUtils.SafeInvoke(Refresh);
		}

		/// <summary>
		/// Asynchronously updates the view if it is currently visible.
		/// </summary>
		[PublicAPI]
		public void RefreshIfVisible()
		{
			RefreshIfVisible(false);
		}

		/// <summary>
		/// Updates the view if it is currently visible.
		/// </summary>
		/// <param name="refreshAsync"></param>
		[PublicAPI]
		public void RefreshIfVisible(bool refreshAsync)
		{
			if (!IsViewVisible)
				return;

			if (refreshAsync)
				RefreshAsync();
			else
				Refresh();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Updates the view.
		/// </summary>
		/// <param name="view"></param>
		protected virtual void Refresh(T view)
		{
		}

		/// <summary>
		/// Gets the view for the presenter.
		/// </summary>
		/// <param name="instantiate">When true instantiates a new view if the current view is null.</param>
		/// <returns></returns>
		[CanBeNull]
		private T GetView(bool instantiate)
		{
			T view;

			m_ViewSection.Enter();

			try
			{
				if (m_View == null && instantiate)
					view = InstantiateView();
				else
					view = m_View;
			}
			finally
			{
				m_ViewSection.Leave();
			}

			SetView(view);
			return view;
		}

		/// <summary>
		/// Override to control how views are instantiated.
		/// </summary>
		/// <returns></returns>
		[NotNull]
		private T InstantiateView()
		{
            if (IsComponent)
				throw new InvalidOperationException(string.Format("{0} can not create its own view.", GetType().Name));

			T view = m_ViewFactory.GetNewView<T>();
			return view;
		}

		#endregion

		#region View Callbacks

		/// <summary>
		/// Subscribe to the view events.
		/// </summary>
		/// <param name="view"></param>
		protected virtual void Subscribe(T view)
		{
			view.OnPreVisibilityChanged += ViewOnPreVisibilityChangedInternal;
			view.OnVisibilityChanged += ViewOnVisibilityChanged;
		}

		/// <summary>
		/// Unsubscribe from the view events.
		/// </summary>
		/// <param name="view"></param>
		protected virtual void Unsubscribe(T view)
		{
			view.OnPreVisibilityChanged -= ViewOnPreVisibilityChangedInternal;
			view.OnVisibilityChanged -= ViewOnVisibilityChanged;
		}

		/// <summary>
		/// Called when the view visibility is about to change.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void ViewOnPreVisibilityChangedInternal(object sender, BoolEventArgs args)
		{
			OnViewPreVisibilityChanged.Raise(this, new BoolEventArgs(args.Data));

			ViewOnPreVisibilityChanged(sender, args);

			// Refresh after OnViewPreVisibilityChanged event triggers updates
			if (args.Data)
				Refresh();
		}

		/// <summary>
		/// Called when the view visibility is about to change.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		protected virtual void ViewOnPreVisibilityChanged(object sender, BoolEventArgs args)
		{
		}

		/// <summary>
		/// Called when the view visibility changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		protected virtual void ViewOnVisibilityChanged(object sender, BoolEventArgs args)
		{
			OnViewVisibilityChanged.Raise(this, new BoolEventArgs(args.Data));
		}

		#endregion
	}
}
