using System;
using ICD.Common.Properties;
using ICD.Common.Utils.Attributes;

namespace ICD.Connect.UI.ButtonActions.Attributes
{
	[MeansImplicitUse]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class ButtonActionSettingsAttribute : AbstractIcdAttribute
	{
		private readonly string m_FactoryName;
		private readonly Type m_ActionMapType;

		[PublicAPI]
		public string FactoryName { get { return m_FactoryName; } }

		[PublicAPI]
		public Type ActionMapType { get { return m_ActionMapType; } }

		public ButtonActionSettingsAttribute(string factoryName, Type actionMapType)
		{
			m_FactoryName = factoryName;
			m_ActionMapType = actionMapType;
		}
	}
}