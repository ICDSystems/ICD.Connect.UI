using System;
using ICD.Common.Utils.Xml;
using ICD.Connect.UI.ButtonActions.Attributes;

namespace ICD.Connect.UI.ButtonActions.Actions
{
	[ButtonActionSettings("TapAndHold", typeof(TapAndHoldButtonAction))]
	public sealed class TapAndHoldButtonActionSettings : AbstractButtonActionSettings
	{
		private const string HOLD_TIME_ELEMENT = "HoldTime";
		private const string TAP_EXCLUSIVE_ELEMENT = "TapExclusive";
		private const string TAP_BINDING = "TapAction";
		private const string HOLD_BINDING = "HoldAction";
		private const string BINDING_TYPE_ATTRIBUTE = "type";

		public long HoldTime { get; set; }
		public bool TapExclusive { get; set; }

		public string TapActionMapType { get; set; }
		public string TapActionMapXml { get; set; }
		public string HoldActionMapType { get; set; }
		public string HoldActionMapXml { get; set; }

		/// <summary>
		/// Parse the xml to the current state of the settings
		/// </summary>
		/// <param name="xml"></param>
		public override void ParseXml(string xml)
		{
			base.ParseXml(xml);

			HoldTime = XmlUtils.ReadChildElementContentAsLong(xml, HOLD_TIME_ELEMENT);
			TapExclusive = XmlUtils.ReadChildElementContentAsBoolean(xml, TAP_EXCLUSIVE_ELEMENT);

			string tapBindingXml;
			if (XmlUtils.TryGetChildElementAsString(xml, TAP_BINDING, out tapBindingXml))
			{
				string tapBindingType;
				if (!XmlUtils.TryGetAttribute(tapBindingXml, BINDING_TYPE_ATTRIBUTE, out tapBindingType))
					throw new InvalidOperationException("Can't build tap binding without type attribute");
				TapActionMapType = tapBindingType;
				TapActionMapXml = tapBindingXml;
			}

			string holdBindingXml;
			if (XmlUtils.TryGetChildElementAsString(xml, HOLD_BINDING, out holdBindingXml))
			{
				string holdBindingType;
				if (!XmlUtils.TryGetAttribute(holdBindingXml, BINDING_TYPE_ATTRIBUTE, out holdBindingType))
					throw new InvalidOperationException("Can't build hold binding without type attribute");
				HoldActionMapType = holdBindingType;
				HoldActionMapXml = HoldActionMapXml;
			}
		}

		/// <summary>
		/// Write the current state of the settings to XML
		/// </summary>
		/// <param name="writer"></param>
		public override void WriteElements(IcdXmlTextWriter writer)
		{
			base.WriteElements(writer);

			writer.WriteElementString(HOLD_TIME_ELEMENT, IcdXmlConvert.ToString(HoldTime));
			writer.WriteElementString(TAP_EXCLUSIVE_ELEMENT, IcdXmlConvert.ToString(TapExclusive));

			if (!string.IsNullOrEmpty(TapActionMapType))
			{
				writer.WriteStartElement(TAP_BINDING);
				writer.WriteAttributeString(BINDING_TYPE_ATTRIBUTE, TapActionMapType);
				writer.WriteValue(TapActionMapXml);
				writer.WriteEndElement();
			}

			if (!string.IsNullOrEmpty(HoldActionMapType))
			{
				writer.WriteStartElement(HOLD_BINDING);
				writer.WriteAttributeString(BINDING_TYPE_ATTRIBUTE,HoldActionMapType);
				writer.WriteValue(HoldActionMapXml);
				writer.WriteEndElement();
			}
		}
	}
}