using System.Collections.Generic;
using ICD.Common.Utils.Xml;
using ICD.Connect.Panels.HardButtons;
using ICD.Connect.UI.ButtonActions.Actions;

namespace ICD.Connect.UI.ButtonActions.HardButton
{
	public interface IHardButtonActionMapComponentSettings
	{
		void WriteXml(IcdXmlTextWriter writer);
		void LoadFromXml(string xml);

		IEnumerable<KeyValuePair<eHardButton, IButtonActionSettings>> GetActionMapSettings();

		void SetActionMapSettings(IEnumerable<KeyValuePair<eHardButton, IButtonActionSettings>> actionMaps);
	}
}