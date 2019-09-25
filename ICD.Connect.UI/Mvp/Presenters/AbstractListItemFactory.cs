using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;

namespace ICD.Connect.UI.Mvp.Presenters
{
	/// <summary>
	/// Generates the given number of views.
	/// </summary>
	/// <typeparam name="TView"></typeparam>
	/// <param name="count"></param>
	/// <returns></returns>
	public delegate IEnumerable<TView> ListItemFactory<TView>(ushort count);

	/// <summary>
	/// Base class for list item factories.
	/// Takes a sequence of model items and generates the views and presenters, using a callback
	/// to bind the MVP triad.
	/// </summary>
	public abstract class AbstractListItemFactory<TModel, TPresenter, TView> : IEnumerable<TPresenter>, IDisposable
		where TPresenter : class, IPresenter
	{
		private readonly Dictionary<Type, Queue<TPresenter>> m_PresenterPool;

		private readonly List<TPresenter> m_Presenters;
		private readonly List<TModel> m_Models;

		private readonly SafeCriticalSection m_CacheSection;

		private readonly INavigationController m_NavigationController;
		private readonly ListItemFactory<TView> m_ViewFactory;

		private readonly Action<TPresenter> m_Subscribe;
		private readonly Action<TPresenter> m_Unsubscribe;

		// Organize the existing presenters for recycling in the least destructive way possible
		private readonly Dictionary<NullObject<TModel>, Queue<TPresenter>> m_ModelToPresenters;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="navigationController"></param>
		/// <param name="viewFactory"></param>
		/// <param name="subscribe">Called for each new presenter taken from the pool.</param>
		/// <param name="unsubscribe">Called for each presenter put back into the pool.</param>
		protected AbstractListItemFactory(INavigationController navigationController, ListItemFactory<TView> viewFactory,
		                                  Action<TPresenter> subscribe, Action<TPresenter> unsubscribe)
		{
			m_PresenterPool = new Dictionary<Type, Queue<TPresenter>>();

			m_Subscribe = subscribe;
			m_Unsubscribe = unsubscribe;

			m_Presenters = new List<TPresenter>();
			m_Models = new List<TModel>();

			m_CacheSection = new SafeCriticalSection();

			m_NavigationController = navigationController;
			m_ViewFactory = viewFactory;

			m_ModelToPresenters = new Dictionary<NullObject<TModel>, Queue<TPresenter>>();
		}

		#region Methods

		/// <summary>
		/// Release resources.
		/// </summary>
		public virtual void Dispose()
		{
			foreach (TPresenter presenter in m_PresenterPool.SelectMany(kvp => kvp.Value))
				presenter.Dispose();
			m_PresenterPool.Clear();

			foreach (TPresenter presenter in m_Presenters)
				presenter.Dispose();
			m_Presenters.Clear();

			m_Models.Clear();
		}

		/// <summary>
		/// Generates the presenters and views for the given sequence of models.
		/// </summary>
		/// <param name="models"></param>
		[PublicAPI]
		public IEnumerable<TPresenter> BuildChildren(IEnumerable<TModel> models)
		{
			if (models == null)
				throw new ArgumentNullException("models");

			m_CacheSection.Enter();

			try
			{
				// Update the model cache to match the given models
				if (!UpdateModelCache(models))
					return m_Presenters.ToArray();

				// Build the views (may be fewer than models due to list max size)
				IEnumerable<TView> views = m_ViewFactory((ushort)m_Models.Count);
				IList<TView> viewsArray = views as IList<TView> ?? views.ToArray();

				// Build the presenters
				m_Presenters.Clear();
				for (int index = 0; index < viewsArray.Count; index++)
				{
					TView view = viewsArray[index];

					// Get the model
					TModel model = m_Models[index];

					// Get the presenter
					TPresenter presenter = LazyLoadPresenterFromPool(model);
					m_Presenters.Add(presenter);

					// Bind
					BindMvpTriad(model, presenter, view);
				}

				// Return unused presenters back to the pool
				ReturnPresentersToPool();

				// Update the model -> presenter lookups for recycling
				UpdateModelToPresentersCache();

				return m_Presenters.ToArray();
			}
			finally
			{
				m_CacheSection.Leave();
			}
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Gets all of the presenters inside and outside of the pool.
		/// </summary>
		/// <returns></returns>
		protected IEnumerable<TPresenter> GetAllPresenters()
		{
			m_CacheSection.Enter();

			try
			{
				return m_PresenterPool.SelectMany(kvp => kvp.Value)
				                      .Concat(m_Presenters)
				                      .ToArray();
			}
			finally
			{
				m_CacheSection.Leave();
			}
		}

		/// <summary>
		/// Binds the model and view to the presenter.
		/// </summary>
		/// <param name="model"></param>
		/// <param name="presenter"></param>
		/// <param name="view"></param>
		protected abstract void BindMvpTriad(TModel model, TPresenter presenter, TView view);

		/// <summary>
		/// Gets the presenter type for the given model instance.
		/// Override to fill lists with different presenters.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[PublicAPI]
		protected virtual Type GetPresenterTypeForModel(TModel model)
		{
			return typeof(TPresenter);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Returns true if the cache changed.
		/// </summary>
		/// <param name="models"></param>
		/// <returns></returns>
		private bool UpdateModelCache(IEnumerable<TModel> models)
		{
			if (models == null)
				throw new ArgumentNullException("models");

			m_CacheSection.Enter();

			try
			{
				// Gather all of the models
				ICollection<TModel> modelsArray = models as ICollection<TModel> ?? models.ToArray();

				// Check if anything has actually changed
				if (modelsArray.Count == m_Models.Count && modelsArray.SequenceEqual(m_Models))
					return false;

				// Now update to the models
				m_Models.Clear();
				m_Models.AddRange(modelsArray);

				return true;
			}
			finally
			{
				m_CacheSection.Leave();
			}
		}

		/// <summary>
		/// Builds a cache of model to presenter to avoid destructive subsequent updates.
		/// </summary>
		private void UpdateModelToPresentersCache()
		{
			m_CacheSection.Enter();

			try
			{
				for (int index = 0; index < m_Presenters.Count; index++)
				{
					TModel model = m_Models[index];
					TPresenter presenter = m_Presenters[index];

// ReSharper disable RedundantCast - The 2008 compiler gets very unhappy without this cast
					m_ModelToPresenters.GetOrAddNew((NullObject<TModel>)model, () => new Queue<TPresenter>())
					                   .Enqueue(presenter);
// ReSharper restore RedundantCast
				}
			}
			finally
			{
				m_CacheSection.Leave();
			}
		}

		/// <summary>
		/// Retrieves or generates a presenter from the pool and adds it to the cache.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private TPresenter LazyLoadPresenterFromPool(TModel model)
		{
			m_CacheSection.Enter();

			try
			{
				TPresenter presenter;

				// Does the cache already contain a presenter for the given model?
				Queue<TPresenter> queue;
				if (m_ModelToPresenters.TryGetValue(model, out queue) && queue.Dequeue(out presenter))
					return presenter;

				// Get the pool for the given model
				Type presenterType = GetPresenterTypeForModel(model);
				
				// Create a new presenter if the pool is empty
				Queue<TPresenter> pool;
				presenter =
					m_PresenterPool.TryGetValue(presenterType, out pool) && pool.Dequeue(out presenter)
						? presenter
						: m_NavigationController.GetNewPresenter(presenterType) as TPresenter;

				// Call the subscription action
				if (m_Subscribe != null)
					m_Subscribe(presenter);

				return presenter;
			}
			finally
			{
				m_CacheSection.Leave();
			}
		}

		/// <summary>
		/// Loops over the unused presenters and moves them back into to the pool.
		/// </summary>
		private void ReturnPresentersToPool()
		{
			m_CacheSection.Enter();

			try
			{
				foreach (Queue<TPresenter> queue in m_ModelToPresenters.Values)
				{
					TPresenter presenter;
					while (queue.Dequeue(out presenter))
						ReturnPresenterToPool(presenter);
				}

				m_ModelToPresenters.Clear();
			}
			finally
			{
				m_CacheSection.Leave();
			}
		}

		/// <summary>
		/// Puts the presenter back into the pool.
		/// </summary>
		/// <param name="presenter"></param>
		private void ReturnPresenterToPool(TPresenter presenter)
		{
			if (presenter == null)
				throw new ArgumentNullException("presenter");

			if (m_Unsubscribe != null)
				m_Unsubscribe(presenter);

			// Clear the view first
			presenter.ClearView();
			// Now clear the model
			BindMvpTriad(default(TModel), presenter, default(TView));

			Type key = presenter.GetType();

			m_CacheSection.Enter();

			try
			{
				m_PresenterPool.GetOrAddNew(key, () => new Queue<TPresenter>())
				               .Enqueue(presenter);
			}
			finally
			{
				m_CacheSection.Leave();
			}
		}

		#endregion

		#region Enumerable

		public IEnumerator<TPresenter> GetEnumerator()
		{
			return m_CacheSection.Execute(() => m_Presenters.ToList()).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
