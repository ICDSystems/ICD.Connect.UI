using System;

namespace ICD.Connect.UI.Attributes
{
	public sealed class PresenterBindingAttribute : AbstractUiBindingAttribute
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="interfaceBinding"></param>
		public PresenterBindingAttribute(Type interfaceBinding)
			: base(interfaceBinding)
		{
		}
	}
}
