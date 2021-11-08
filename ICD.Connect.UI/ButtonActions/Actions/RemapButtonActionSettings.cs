using ICD.Common.Utils.Xml;
using ICD.Connect.Panels.HardButtons;
using ICD.Connect.UI.ButtonActions.Attributes;

namespace ICD.Connect.UI.ButtonActions.Actions
{
	[ButtonActionSettings("RemapButton", typeof(RemapButtonAction))]
	public sealed class RemapButtonActionSettings : AbstractButtonActionSettings
	{
		private const string REMAP_AS_BUTTON_ELEMENT = "RemapAsButton";

		public eHardButton RemapAsButton { get; set; }

		/// <summary>
		/// Parse the xml to the current state of the settings
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);
			
			RemapAsButton = XmlUtils.ReadChildElementContentAsEnum<eHardButton>(xml, REMAP_AS_BUTTON_ELEMENT, true);
		}

		/// <summary>
		/// Write the current state of the settings to XML
		/// </summary>
		/// <param name="writer"></param>
		public override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			writer.WriteElementString(REMAP_AS_BUTTON_ELEMENT, IcdXmlConvert.ToString(RemapAsButton));
		}
	}
}