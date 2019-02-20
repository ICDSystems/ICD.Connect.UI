using System;
using ICD.Common.Properties;

namespace ICD.Connect.UI.Attributes
{
	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public abstract class AbstractUiBindingAttribute : AbstractUiAttribute, IUiBindingAttribute
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

			m_InterfaceBinding = interfaceBinding;
		}
	}
}
