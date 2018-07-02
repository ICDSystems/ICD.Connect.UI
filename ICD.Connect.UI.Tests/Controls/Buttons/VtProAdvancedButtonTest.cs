using ICD.Connect.Panels;
using ICD.Connect.Panels.Mock;
using ICD.Connect.UI.Controls;
using ICD.Connect.UI.Controls.Buttons;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls.Buttons
{
    [TestFixture]
    public sealed class VtProAdvancedButtonTest : AbstractVtProAdvancedButtonTest<VtProAdvancedButton>
    {
        protected override VtProAdvancedButton Instantiate(ushort smartObjectId, ISigInputOutput panel, IVtProParent parent)
        {
            return new VtProAdvancedButton(panel, parent);
        }
    }
}
