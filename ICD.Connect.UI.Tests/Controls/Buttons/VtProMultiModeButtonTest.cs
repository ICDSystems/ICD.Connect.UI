using ICD.Connect.Panels;
using ICD.Connect.Panels.Mock;
using ICD.Connect.UI.Controls;
using ICD.Connect.UI.Controls.Buttons;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls.Buttons
{
    [TestFixture]
    public sealed class VtProMultiModeButtonTest : AbstractVtProAdvancedButtonTest<VtProMultiModeButton>
    {
        protected override VtProMultiModeButton Instantiate(ushort smartObjectId, ISigInputOutput panel, IVtProParent parent)
        {
            return new VtProMultiModeButton(panel, parent);
        }
    }
}
