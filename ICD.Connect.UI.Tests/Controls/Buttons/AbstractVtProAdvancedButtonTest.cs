using ICD.Connect.Panels.Mock;
using ICD.Connect.Protocol.Sigs;
using ICD.Connect.UI.Controls.Buttons;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls.Buttons
{
    public abstract class AbstractVtProAdvancedButtonTest<T> : AbstractVtProButtonTest<T>
        where T : AbstractVtProAdvancedButton
    {
        /// <summary>
        /// Sets the button mode. Throws InvalidOperationException if there is no mode join.
        /// </summary>
        /// <param name="mode"></param>
        [TestCase((ushort) 100)]
        public void SetMode(ushort mode)
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(0, panel, null);
            instance.AnalogModeJoin = 10;

            instance.SetMode(mode);

            Assert.AreEqual(mode, panel.UShortInput[10].GetValue());
        }
    }
}
