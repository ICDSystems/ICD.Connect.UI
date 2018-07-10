using ICD.Connect.Panels;
using ICD.Connect.Panels.Mock;
using ICD.Connect.UI.Controls;
using ICD.Connect.UI.Controls.TextControls;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls.TextControls
{

    [TestFixture]
    public sealed class VtProSimpleLabelTest : AbstractVtProLabelTest<VtProSimpleLabel>
    {
        protected override VtProSimpleLabel Instantiate(ushort smartObjectId, ISigInputOutput panel, IVtProParent parent)
        {
            return new VtProSimpleLabel(panel, parent);
        }
        protected override VtProSimpleLabel Instantiate(ushort smartObjectId, ISigInputOutput panel)
        {
            return new VtProSimpleLabel(panel);
        }
    }
}
