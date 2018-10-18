using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Connect.Panels.Devices;
using ICD.Connect.Panels.SmartObjects;
using ICD.Connect.UI.Controls;
using ICD.Connect.UI.Controls.Lists;

namespace ICD.Connect.UI.Mvp.Views
{
	public abstract class AbstractViewFactory : IViewFactory
	{
		private readonly IPanelDevice m_Panel;

		/// <summary>
		/// Gets the panel for this view factory.
		/// </summary>
		public IPanelDevice Panel { get { return m_Panel; } }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		protected AbstractViewFactory(IPanelDevice panel)
		{
			m_Panel = panel;
		}

		/// <summary>
		/// Instantiates a new view of the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public abstract T GetNewView<T>() where T : class, IView;

		/// <summary>
		/// Instantiates a view of the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="smartObject"></param>
		/// <param name="parent"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public abstract T GetNewView<T>(ISmartObject smartObject, IVtProParent parent, ushort index) where T : class, IView;

		/// <summary>
		/// Creates views for the given subpage reference list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="childViews"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public IEnumerable<T> GetNewSrlViews<T>(VtProSubpageReferenceList list, List<T> childViews, ushort count)
			where T : class, IView
		{
			count = Math.Min(count, list.MaxSize);
			list.SetNumberOfItems(count);

			ISmartObject smartObject = list.SmartObject;

			for (ushort index = (ushort)childViews.Count; index < count; index++)
			{
				T view = GetNewView<T>(smartObject, list, index);
				childViews.Add(view);
			}

			return childViews.Take(count);
		}
	}
}
