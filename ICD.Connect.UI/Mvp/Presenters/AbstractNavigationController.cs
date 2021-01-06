using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Connect.UI.Attributes;

namespace ICD.Connect.UI.Mvp.Presenters
{
	public abstract class AbstractNavigationController : INavigationController
	{
		private static readonly BindingMap<PresenterBindingAttribute> s_InterfaceToConcrete;

		private readonly Dictionary<Type, IPresenter> m_Cache;
		private readonly SafeCriticalSection m_CacheSection;

		#region Constructors

		/// <summary>
		/// Static constructor.
		/// </summary>
		static AbstractNavigationController()
		{
			s_InterfaceToConcrete = new BindingMap<PresenterBindingAttribute>();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractNavigationController()
		{
			m_Cache = new Dictionary<Type, IPresenter>();
			m_CacheSection = new SafeCriticalSection();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		public virtual void Dispose()
		{
			m_Cache.Values.ForEach(p => p.Dispose());
			m_Cache.Clear();
		}

		/// <summary>
		/// Gets the instantiated presenters.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IPresenter> GetPresenters()
		{
			return m_CacheSection.Execute(() => m_Cache.Values.ToArray(m_Cache.Count));
		}

		/// <summary>
		/// Gets the concrete presenter type for the given interface type.
		/// </summary>
		/// <param name="presenterInterface"></param>
		/// <returns></returns>
		public Type GetPresenterType(Type presenterInterface)
		{
			return s_InterfaceToConcrete.GetConcreteType(presenterInterface);
		}

		/// <summary>
		/// Gets the interface presenter type for the given concrete type.
		/// </summary>
		/// <param name="presenterConcrete"></param>
		/// <returns></returns>
		public Type GetInterfaceType(Type presenterConcrete)
		{
			return s_InterfaceToConcrete.GetInterfaceType(presenterConcrete);
		}

		/// <summary>
		/// Instantiates a new presenter of the given type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public abstract IPresenter GetNewPresenter(Type type);

		/// <summary>
		/// Instantiates a new presenter of the given type.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		protected IPresenter GetNewPresenter(Type type, params object[] parameters)
		{
			return (IPresenter)s_InterfaceToConcrete.CreateInstance(type, parameters);
		}

		/// <summary>
		/// Instantiates or returns an existing presenter of the given type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public IPresenter LazyLoadPresenter(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			m_CacheSection.Enter();

			try
			{
				IPresenter presenter;

				if (!m_Cache.TryGetValue(type, out presenter))
				{
					presenter = GetNewPresenter(type);
					m_Cache[type] = presenter;
				}

				return presenter;
			}
			finally
			{
				m_CacheSection.Leave();
			}
		}

		/// <summary>
		/// Instantiates or returns an existing presenter for every presenter that can be assigned to the given type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public IEnumerable<IPresenter> LazyLoadPresenters(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			return s_InterfaceToConcrete.GetAssignableInterfaces(type)
			                            .Select(presenterType => LazyLoadPresenter(presenterType));
		}

		/// <summary>
		/// Instantiates or returns an existing presenter for every presenter that can be assigned to the given type.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<T> LazyLoadPresenters<T>()
			where T : IPresenter
		{
			return LazyLoadPresenters(typeof(T)).Cast<T>();
		}

		#endregion
	}
}
