using System;

namespace ICD.Connect.UI.Attributes
{
	public interface IUiBindingAttribute
	{
		/// <summary>
		/// Gets the interface type for the binding.
		/// </summary>
		Type InterfaceBinding { get; }
	}
}