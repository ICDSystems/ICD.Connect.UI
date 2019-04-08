using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Common.Utils.Collections;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.UI.Mvp.Presenters;

namespace ICD.Connect.UI.Mvp.VisibilityTree
{
	/// <summary>
	/// Base class for visibility nodes.
	/// </summary>
	public abstract class AbstractVisibilityNode : IVisibilityNode
	{
		/// <summary>
		/// Raised when a child's visibility is about to change.
		/// </summary>
		public event ChildVisibilityChangedCallback OnChildPreVisibilityChanged;

		/// <summary>
		/// Raised when a child's visibility changes.
		/// </summary>
		public event ChildVisibilityChangedCallback OnChildVisibilityChanged;

		private readonly IcdHashSet<IVisibilityNode> m_Nodes;
		private readonly IcdHashSet<IPresenter> m_Presenters;

		private readonly SafeCriticalSection m_NodesSection;
		private readonly SafeCriticalSection m_PresentersSection;

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractVisibilityNode()
		{
			m_Nodes = new IcdHashSet<IVisibilityNode>();
			m_Presenters = new IcdHashSet<IPresenter>();

			m_NodesSection = new SafeCriticalSection();
			m_PresentersSection = new SafeCriticalSection();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Returns true if any of the children are visible.
		/// </summary>
		public bool IsVisible
		{
			get
			{
				m_NodesSection.Enter();
				m_PresentersSection.Enter();

				try
				{
					return m_Presenters.Any(p => p.IsViewVisible) ||
						   m_Nodes.Any(n => n.IsVisible);
				}
				finally
				{
					m_NodesSection.Leave();
					m_PresentersSection.Leave();
				}
			}
		}

		/// <summary>
		/// Adds the node to the tree.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public void AddNode(IVisibilityNode node)
		{
			m_NodesSection.Enter();

			try
			{
				if (m_Nodes.Add(node))
					Subscribe(node);
			}
			finally
			{
				m_NodesSection.Leave();
			}
		}

		/// <summary>
		/// Adds the presenter to the tree.
		/// </summary>
		/// <param name="presenter"></param>
		/// <returns></returns>
		public void AddPresenter(IPresenter presenter)
		{
			m_PresentersSection.Enter();

			try
			{
				if (m_Presenters.Add(presenter))
					Subscribe(presenter);
			}
			finally
			{
				m_PresentersSection.Leave();
			}
		}

		/// <summary>
		/// Gets the immediate child nodes.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IVisibilityNode> GetNodes()
		{
			return m_NodesSection.Execute(() => m_Nodes.ToArray());
		}

		/// <summary>
		/// Gets the immediate child presenters.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IPresenter> GetPresenters()
		{
			return m_PresentersSection.Execute(() => m_Presenters.ToArray());
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Subscribe to the presenter events.
		/// </summary>
		/// <param name="presenter"></param>
		private void Subscribe(IPresenter presenter)
		{
			presenter.OnViewPreVisibilityChanged += PresenterOnPreVisibilityChanged;
			presenter.OnViewVisibilityChanged += PresenterOnVisibilityChanged;
		}

		/// <summary>
		/// Subscribe to the node events.
		/// </summary>
		/// <param name="node"></param>
		private void Subscribe(IVisibilityNode node)
		{
			node.OnChildPreVisibilityChanged += NodeOnChildPreVisibilityChanged;
			node.OnChildVisibilityChanged += NodeOnChildVisibilityChanged;
		}

		/// <summary>
		/// Called when a descendant presenter is about to change visibility.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="presenter"></param>
		/// <param name="visibility"></param>
		protected virtual void NodeOnChildPreVisibilityChanged(IVisibilityNode parent, IPresenter presenter, bool visibility)
		{
			ChildVisibilityChangedCallback handler = OnChildPreVisibilityChanged;
			if (handler != null)
				handler(parent, presenter, visibility);
		}

		/// <summary>
		/// Called when a descendant presenter changes visibility.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="presenter"></param>
		/// <param name="visibility"></param>
		protected virtual void NodeOnChildVisibilityChanged(IVisibilityNode parent, IPresenter presenter, bool visibility)
		{
			ChildVisibilityChangedCallback handler = OnChildVisibilityChanged;
			if (handler != null)
				handler(parent, presenter, visibility);
		}

		/// <summary>
		/// Called when a child presenter visibility is about to change.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		protected virtual void PresenterOnPreVisibilityChanged(object sender, BoolEventArgs args)
		{
			IPresenter presenter = sender as IPresenter;
			if (presenter == null)
				return;

			ChildVisibilityChangedCallback handler = OnChildPreVisibilityChanged;
			if (handler != null)
				handler(this, presenter, args.Data);
		}

		/// <summary>
		/// Called when a child presenter visibility changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		protected virtual void PresenterOnVisibilityChanged(object sender, BoolEventArgs args)
		{
			IPresenter presenter = sender as IPresenter;
			if (presenter == null)
				return;

			ChildVisibilityChangedCallback handler = OnChildVisibilityChanged;
			if (handler != null)
				handler(this, presenter, args.Data);
		}

		#endregion
	}
}
