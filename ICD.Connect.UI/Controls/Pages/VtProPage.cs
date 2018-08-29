using ICD.Common.Properties;
using ICD.Connect.Panels;
using ICD.Connect.Panels.Devices;

namespace ICD.Connect.UI.Controls.Pages
{
	[PublicAPI]
	public sealed class VtProPage : AbstractVtProPage<IPanelDevice>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public VtProPage(IPanelDevice panel)
			: base(panel)
		{
		}
	}
}
