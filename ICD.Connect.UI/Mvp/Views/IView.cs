using System;
using ICD.Common.Utils.EventArguments;

namespace ICD.Connect.UI.Mvp.Views
{
	/// <summary>
	/// IView holds simple logic for controlling the user-facing output of the program.
	/// </summary>
	public interface IView : IDisposable
	{
		/// <summary>
		/// Raised when the view is about to be shown or hidden.
		/// </summary>
		event EventHandler<BoolEventArgs> OnPreVisibilityChanged;

		/// <summary>
		/// Raised when the view is shown or hidden.
		/// </summary>
		event EventHandler<BoolEventArgs> OnVisibilityChanged;

		/// <summary>
		/// Returns true if the view is visible.
		/// </summary>
		bool IsVisible { get; }

		/// <summary>
		/// Needs to be called after instantiation to create the child controls.
		/// </summary>
		void Initialize();

		/// <summary>
		/// Sets the visibility of the view.
		/// </summary>
		/// <param name="visible"></param>
		void Show(bool visible);

		/// <summary>
		/// Sets the enabled state of the view.
		/// </summary>
		/// <param name="enabled"></param>
		void Enable(bool enabled);
	}
}
