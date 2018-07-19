using System.Runtime.CompilerServices;
using ICD.Connect.Panels;
using ICD.Connect.UI.Controls;
using ICD.Connect.UI.Controls.Pages;

namespace ICD.Connect.UI.Tests.Controls.Pages
{
    public sealed class VtProPageTest : AbstractVtProPageTest<VtProPage, IPanelDevice>
    {
	    protected override VtProPage Instantiate(ushort smartObjectId, IPanelDevice panel, IVtProParent parent)
	    {
		    return new VtProPage(panel);
	    }

	    protected override VtProPage Instantiate(ushort smartObjectId, IPanelDevice panel)
	    {
		    return Instantiate(smartObjectId, panel, null);
	    }
	}
}
