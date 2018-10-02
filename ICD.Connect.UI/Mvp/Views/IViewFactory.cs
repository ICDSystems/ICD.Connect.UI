using System.Collections.Generic;
using ICD.Connect.Panels.Devices;
using ICD.Connect.Panels.SmartObjects;
using ICD.Connect.UI.Controls;
using ICD.Connect.UI.Controls.Lists;

namespace ICD.Connect.UI.Mvp.Views
{
	/// <summary>
	/// IViewFactory provides functionality for a presenter to obtain its view.
	/// </summary>
	public interface IViewFactory
	{
		/// <summary>
		/// Gets the panel for this view factory.
		/// </summary>
		IPanelDevice Panel { get; }

		/// <summary>
		/// Instantiates a new view of the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T GetNewView<T>() where T : class, IView;

		/// <summary>
		/// Instantiates a view of the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="smartObject"></param>
		/// <param name="parent"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		T GetNewView<T>(ISmartObject smartObject, IVtProParent parent, ushort index) where T : class, IView;

		/// <summary>
		/// Creates views for the given subpage reference list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="childViews"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		IEnumerable<T> GetNewSrlViews<T>(VtProSubpageReferenceList list, List<T> childViews, ushort count) where T : class, IView;
	}
}
