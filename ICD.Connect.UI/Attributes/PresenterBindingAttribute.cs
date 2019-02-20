using System;
using ICD.Connect.UI.Mvp.Presenters;

namespace ICD.Connect.UI.Attributes
{
	public sealed class PresenterBindingAttribute : AbstractUiBindingAttribute<IPresenter>
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
