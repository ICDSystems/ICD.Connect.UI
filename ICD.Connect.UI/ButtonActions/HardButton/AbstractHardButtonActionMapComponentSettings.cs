using System;
using System.Collections.Generic;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Xml;
using ICD.Connect.Panels.HardButtons;
using ICD.Connect.UI.ButtonActions.Actions;
using ICD.Connect.UI.ButtonActions.Factories;

namespace ICD.Connect.UI.ButtonActions.HardButton
{
	public abstract class AbstractHardButtonActionMapComponentSettings<TUi> : IHardButtonActionMapComponentSettings where TUi : IHardButtonUserInterface
	{
		private const string ACTION_MAPS_ELEMENT = "ButtonActionMaps";
		private const string ACTION_MAP_ELEMENT = "ButtonActionMap";
		private const string BUTTON_ATTRIBUTE = "button";
		private const string FACTORY_NAME_ATTRIBUTE = "type";


		private readonly Dictionary<eHardButton, IButtonActionSettings> m_ButtonActionMaps;
		private readonly SafeCriticalSection m_ButtonActionMapsSection;

		protected AbstractHardButtonActionMapComponentSettings()
		{
			m_ButtonActionMaps = new Dictionary<eHardButton, IButtonActionSettings>();
			m_ButtonActionMapsSection = new SafeCriticalSection();
		}

		public IEnumerable<KeyValuePair<eHardButton, IButtonActionSettings>> GetActionMapSettings()
		{
			m_ButtonActionMapsSection.Enter();
			try
			{
				return m_ButtonActionMaps.ToArray(m_ButtonActionMaps.Count);
			}
			finally
			{
				m_ButtonActionMapsSection.Leave();
			}
		}

		public void SetActionMapSettings(IEnumerable<KeyValuePair<eHardButton, IButtonActionSettings>> actionMaps)
		{
			m_ButtonActionMapsSection.Enter();
			try
			{
				m_ButtonActionMaps.Clear();
				m_ButtonActionMaps.AddRange(actionMaps);
			}
			finally
			{
				m_ButtonActionMapsSection.Leave();
			}
		}

		

		public void WriteXml(IcdXmlTextWriter writer)
		{
			m_ButtonActionMapsSection.Enter();
			try
			{
				writer.WriteStartElement(ACTION_MAPS_ELEMENT);
				{
					foreach (var actionMap in m_ButtonActionMaps)
					{
						string factoryName = actionMap.Value.FactoryName;
						writer.WriteStartElement(ACTION_MAP_ELEMENT);
						writer.WriteAttributeString(BUTTON_ATTRIBUTE, IcdXmlConvert.ToString(actionMap.Key));
						writer.WriteAttributeString(FACTORY_NAME_ATTRIBUTE, factoryName);
						{
							actionMap.Value.WriteElements(writer);
						}
						writer.WriteEndElement();
					}
				}
				writer.WriteEndElement();
			}
			finally
			{
				m_ButtonActionMapsSection.Leave();
			}
		}

		public void LoadFromXml(string xml)
		{
			var actionMaps = XmlUtils.ReadListFromXml(xml, ACTION_MAP_ELEMENT, s => BuildControlFromXml(s));

			SetActionMapSettings(actionMaps);
		}

		private KeyValuePair<eHardButton, IButtonActionSettings> BuildControlFromXml(string xml)
		{
			eHardButton button = XmlUtils.GetAttributeAsEnum<eHardButton>(xml, BUTTON_ATTRIBUTE, true);
			string factoryName = XmlUtils.GetAttributeAsString(xml, FACTORY_NAME_ATTRIBUTE);

			Type settingsType = ButtonActionRootFactory.GetSettingsTypeForFactoryName(factoryName);

			IButtonActionSettings settings = ReflectionUtils.CreateInstance<IButtonActionSettings>(settingsType);
			settings.ParseXml(XmlUtils.ReadElementContent(xml));

			return new KeyValuePair<eHardButton, IButtonActionSettings>(button, settings);
			
		}
	}
}