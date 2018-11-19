﻿using System;
using System.Collections.Generic;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;

namespace ICD.Connect.UI.Mvp.Presenters
{
	public abstract class AbstractNavigationController : INavigationController
	{
		private readonly Dictionary<Type, IPresenter> m_Cache;
		private readonly SafeCriticalSection m_CacheSection;

		#region Constructors

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
		/// Instantiates a new presenter of the given type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public abstract IPresenter GetNewPresenter(Type type);

		#endregion
	}
}