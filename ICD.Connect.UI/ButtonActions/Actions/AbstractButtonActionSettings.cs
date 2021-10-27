using System;
using ICD.Common.Utils;
using ICD.Common.Utils.Xml;
using ICD.Connect.UI.ButtonActions.Attributes;

namespace ICD.Connect.UI.ButtonActions.Actions
{
	public class AbstractButtonActionSettings : IButtonActionSettings
	{
		private const string NAME_ELEMENT = "Name";

		/// <summary>
		/// FactoryName used in settings xml
		/// </summary>
		public string FactoryName 
		{
			get
			{
				ButtonActionSettingsAttribute attribute = AttributeUtils.GetClassAttribute<ButtonActionSettingsAttribute>(GetType());
				if (attribute == null)
					throw new InvalidOperationException(string.Format("{0} has no Settings Attribute", GetType().Name));

				return attribute.FactoryName;
			} 
		}

		/// <summary>
		/// ButtonAction Type the settings are for
		/// </summary>
		public Type ButtonActionType
		{
			get
			{
				ButtonActionSettingsAttribute attribute = AttributeUtils.GetClassAttribute<ButtonActionSettingsAttribute>(GetType());
				if (attribute == null)
					throw new InvalidOperationException(string.Format("{0} has no Settings Attribute", GetType().Name));

				return attribute.ActionMapType;
			}
		}

		/// <summary>
		/// Name of the binding
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Write the current state of the settings to XML
		/// </summary>
		/// <param name="writer"></param>
		public virtual void WriteElements(IcdXmlTextWriter writer)
		{
			writer.WriteElementString(NAME_ELEMENT, IcdXmlConvert.ToString(Name));
		}

		/// <summary>
		/// Parse the xml to the current state of the settings
		/// </summary>
		/// <param name="xml"></param>
		public virtual void ParseXml(string xml)
		{
			Name = XmlUtils.ReadChildElementContentAsString(xml, NAME_ELEMENT);
		}
	}
}