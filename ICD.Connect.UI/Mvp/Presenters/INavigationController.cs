using System;
using System.Collections.Generic;
using ICD.Common.Properties;

namespace ICD.Connect.UI.Mvp.Presenters
{
	/// <summary>
	/// INavigationController provides a way for presenters to access each other.
	/// </summary>
	public interface INavigationController : IDisposable
	{
		/// <summary>
		/// Instantiates a new presenter of the given type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		[NotNull]
		IPresenter GetNewPresenter(Type type);

		/// <summary>
		/// Instantiates or returns an existing presenter of the given type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		[NotNull]
		IPresenter LazyLoadPresenter(Type type);

		/// <summary>
		/// Instantiates or returns an existing presenter for every presenter that can be assigned to the given type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		IEnumerable<IPresenter> LazyLoadPresenters(Type type);

		/// <summary>
		/// Instantiates or returns an existing presenter for every presenter that can be assigned to the given type.
		/// </summary>
		/// <returns></returns>
		IEnumerable<T> LazyLoadPresenters<T>() where T : IPresenter;
	}

	/// <summary>
	/// Extension methods for INavigationControllers.
	/// </summary>
	public static class NavigationControllerExtensions
	{
		/// <summary>
		/// Instantiates or returns an existing presenter of the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		[NotNull]
		public static T LazyLoadPresenter<T>(this INavigationController extends)
			where T : class, IPresenter
		{
			return extends.LazyLoadPresenter<T>(typeof(T));
		}

		/// <summary>
		/// Instantiates or returns an existing presenter of the given type.
		/// </summary>
		/// <param name="extends"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		[NotNull]
		public static T LazyLoadPresenter<T>(this INavigationController extends, Type type)
			where T : class, IPresenter
		{
			IPresenter presenter = extends.LazyLoadPresenter(type);
			T output = presenter as T;

			if (output == null)
				throw new InvalidCastException(string.Format("Failed to cast {0} to {1}", presenter, typeof(T).Name));

			return output;
		}

		/// <summary>
		/// Shows the presenter of the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="extends"></param>
		/// <returns></returns>
		[NotNull]
		public static T NavigateTo<T>(this INavigationController extends)
			where T : class, IPresenter
		{
			T output = extends.LazyLoadPresenter<T>();
			output.ShowView(true);
			return output;
		}
	}
}
