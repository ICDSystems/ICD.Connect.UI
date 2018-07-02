using ICD.Connect.Panels;
using ICD.Connect.Panels.Mock;
using ICD.Connect.UI.Controls;
using ICD.Connect.UI.Controls.TextControls;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls.TextControls
{

    [TestFixture]
    public sealed class VtProFormattedTextTest : AbstractVtProLabelTest<VtProFormattedText>
    {
        protected override VtProFormattedText Instantiate(ushort smartObjectId, ISigInputOutput panel, IVtProParent parent)
        {
            return new VtProFormattedText(panel, parent);
        }
    }
}
