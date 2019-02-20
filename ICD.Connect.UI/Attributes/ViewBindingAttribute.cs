using System;

namespace ICD.Connect.UI.Attributes
{
	public sealed class ViewBindingAttribute : AbstractUiBindingAttribute
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="interfaceBinding"></param>
		public ViewBindingAttribute(Type interfaceBinding)
			: base(interfaceBinding)
		{
		}
	}
}
