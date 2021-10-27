using System;
using ICD.Common.Properties;
using ICD.Common.Utils.Xml;

namespace ICD.Connect.UI.ButtonActions.Actions
{
	public interface IButtonActionSettings
	{
		/// <summary>
		/// FactoryName used in settings xml
		/// </summary>
		string FactoryName { get; }

		/// <summary>
		/// ButtonAction Type the settings are for
		/// </summary>
		Type ButtonActionType { get; }

		/// <summary>
		/// Name of the binding
		/// </summary>
		[CanBeNull]
		string Name { get; set; }

		/// <summary>
		/// Write the current state of the settings to XML
		/// </summary>
		/// <param name="writer"></param>
		void WriteElements(IcdXmlTextWriter writer);

		/// <summary>
		/// Parse the xml to the current state of the settings
		/// </summary>
		/// <param name="xml"></param>
		void ParseXml(string xml);
	}
}