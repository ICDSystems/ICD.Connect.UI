using System;
using ICD.Common.Properties;
using ICD.Common.Utils.Extensions;

namespace ICD.Connect.UI.Attributes
{
	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public abstract class AbstractUiBindingAttribute<T> : AbstractUiAttribute
	{
		private readonly Type m_InterfaceBinding;

		/// <summary>
		/// Gets the interface type for the binding.
		/// </summary>
		public Type InterfaceBinding { get { return m_InterfaceBinding; } }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="interfaceBinding"></param>
		protected AbstractUiBindingAttribute(Type interfaceBinding)
		{
			if (interfaceBinding == null)
				throw new ArgumentNullException("interfaceBinding");

			if (!interfaceBinding.IsAssignableTo<T>())
				throw new ArgumentException("Unexpected type", "interfaceBinding");

			m_InterfaceBinding = interfaceBinding;
		}
	}
}
