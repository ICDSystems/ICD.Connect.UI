using System;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.UI.Mvp.Views;

namespace ICD.Connect.UI.Mvp.Presenters
{
	public interface IPresenter : IDisposable
	{
		/// <summary>
		/// Raised when the view visibility changes.
		/// </summary>
		event EventHandler<BoolEventArgs> OnViewVisibilityChanged;

		/// <summary>
		/// Gets the state of the view visibility.
		/// </summary>
		bool IsViewVisible { get; }

		/// <summary>
		/// Sets the visibility of the view.
		/// </summary>
		/// <param name="visible"></param>
		void ShowView(bool visible);

		/// <summary>
		/// Sets the enabled state of the view.
		/// </summary>
		/// <param name="enabled"></param>
		void SetViewEnabled(bool enabled);

		/// <summary>
		/// Sets the current view to null.
		/// </summary>
		void ClearView();

		/// <summary>
		/// Updates the view.
		/// </summary>
		void Refresh();

		/// <summary>
		/// Refreshes the view asynchronously.
		/// </summary>
		void RefreshAsync();

		/// <summary>
		/// Asynchronously updates the view if it is currently visible.
		/// </summary>
		void RefreshIfVisible();

		/// <summary>
		/// Updates the view if it is currently visible.
		/// </summary>
		/// <param name="refreshAsync"></param>
		void RefreshIfVisible(bool refreshAsync);
	}

	public interface IPresenter<T> : IPresenter
		where T : IView
	{
		/// <summary>
		/// Sets the view.
		/// </summary>
		/// <param name="view"></param>
		void SetView(T view);
	}
}
