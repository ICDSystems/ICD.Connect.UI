using System.Collections.Generic;
using ICD.Connect.UI.Mvp.Presenters;

namespace ICD.Connect.UI.Mvp.VisibilityTree
{
	public delegate void ChildVisibilityChangedCallback(IVisibilityNode parent, IPresenter presenter, bool visibility);

	public interface IVisibilityNode
	{
		/// <summary>
		/// Raised when a child's visibility is about to change.
		/// </summary>
		event ChildVisibilityChangedCallback OnChildPreVisibilityChanged;

		/// <summary>
		/// Raised when a child's visibility changes.
		/// </summary>
		event ChildVisibilityChangedCallback OnChildVisibilityChanged;

		/// <summary>
		/// Returns true if any of the children are visible.
		/// </summary>
		bool IsVisible { get; }

		/// <summary>
		/// Adds the node to the tree.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		void AddNode(IVisibilityNode node);

		/// <summary>
		/// Adds the presenter to the tree.
		/// </summary>
		/// <param name="presenter"></param>
		/// <returns></returns>
		void AddPresenter(IPresenter presenter);

		/// <summary>
		/// Gets the immediate child nodes.
		/// </summary>
		/// <returns></returns>
		IEnumerable<IVisibilityNode> GetNodes();

		/// <summary>
		/// Gets the immediate child presenters.
		/// </summary>
		/// <returns></returns>
		IEnumerable<IPresenter> GetPresenters();

		/// <summary>
		/// Returns true if this node contains the given child node recursively.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		bool ContainsNodeRecursive(IVisibilityNode node);
	}

	/// <summary>
	/// Extension methods for IVisibilityNodes.
	/// </summary>
	public static class VisibilityNodeExtensions
	{
		/// <summary>
		/// Recursively hides all children of the node.
		/// </summary>
		/// <param name="extends"></param>
		public static void Hide(this IVisibilityNode extends)
		{
			foreach (IVisibilityNode node in extends.GetNodes())
				node.Hide();
			foreach (IPresenter presenter in extends.GetPresenters())
				presenter.ShowView(false);
		}
	}
}
