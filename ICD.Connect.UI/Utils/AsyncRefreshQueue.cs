using System;
using ICD.Common.Properties;
using ICD.Common.Utils;

namespace ICD.Connect.UI.Utils
{
	/// <summary>
	/// Blindly triggering async refreshes can result in a lot of unnecessary work being performed.
	/// We're only interested in completing the current refresh, and performing the final refresh to
	/// catch any changes that may have happened during the previous refresh. Anything inbetween is
	/// wasted work.
	/// 
	/// This class acts as a mechanism to reduce the number of refreshes being performed in seqeuence.
	/// </summary>
	public sealed class AsyncRefreshQueue
	{
		private readonly SafeCriticalSection m_StateSection;

		private bool m_IsRefreshing;
		private bool m_QueuedRefresh;

		[UsedImplicitly]
		private object m_RefreshHandle;

		/// <summary>
		/// Gets/sets the refresh action.
		/// </summary>
		[PublicAPI]
		public Action RefreshCallback { get; set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public AsyncRefreshQueue()
		{
			m_StateSection = new SafeCriticalSection();
		}

		/// <summary>
		/// Enqueues a new refresh callback.
		/// This will be dropped if there are already two refreshes enqueued.
		/// </summary>
		public void EnqueueRefresh()
		{
			m_StateSection.Enter();

			try
			{
				if (m_IsRefreshing)
					m_QueuedRefresh = true;
				else
					PerformNextRefresh();
			}
			finally
			{
				m_StateSection.Leave();
			}
		}

		private void PerformNextRefresh()
		{
			m_StateSection.Enter();

			try
			{
				m_IsRefreshing = true;
				m_QueuedRefresh = false;
			}
			finally
			{
				m_StateSection.Leave();
			}

			m_RefreshHandle = ThreadingUtils.SafeInvoke(Refresh);
		}

		private void Refresh()
		{
			// Perform the refresh
			Action refreshCallback = RefreshCallback;
			if (refreshCallback != null)
				refreshCallback();

			// We're done if there is no queued subsequent refresh
			m_StateSection.Enter();

			try
			{
				if (!m_QueuedRefresh)
				{
					m_IsRefreshing = false;
					return;
				}
			}
			finally
			{
				m_StateSection.Leave();
			}

			PerformNextRefresh();
		}
	}
}
