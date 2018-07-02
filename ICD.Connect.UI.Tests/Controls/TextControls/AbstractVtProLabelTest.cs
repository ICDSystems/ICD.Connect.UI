using ICD.Connect.Panels;
using ICD.Connect.Panels.Mock;
using ICD.Connect.Protocol.Sigs;
using ICD.Connect.UI.Controls.TextControls;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls.TextControls
{
    public abstract class AbstractVtProLabelTest<T> : AbstractVtProControlTest<T, ISigInputOutput>
        where T : AbstractVtProLabel
    {
        [TestCase("Test")]
        public void SetLabelText(string text)
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(0, panel, null);

            instance.IndirectTextJoin = 100;

            instance.SetLabelText(text);

            Assert.AreEqual(text, panel.StringInput[100].GetValue());
        }

        /// <summary>
        /// Sets the label text at the given join.
        /// </summary>
        /// <param name="join"></param>
        /// <param name="text"></param>
        [TestCase((ushort) 100, "Test")]
        public void SetLabelTextAtJoin(ushort join, string text)
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(0, panel, null);

            instance.SetLabelTextAtJoin(join, text);

            Assert.AreEqual(text, panel.StringInput[100].GetValue());
        }

        /// <summary>
        /// Sets the label text at the given join.
        /// </summary>
        /// <param name="join"></param>
        /// <param name="value"></param>
        [TestCase((ushort) 100, (ushort) 100)]
        public void SetLabelTextAtJoin(ushort join, ushort value)
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(0, panel, null);

            instance.SetLabelTextAtJoin(join, value);

            Assert.AreEqual(value, panel.UShortInput[100].GetValue());
        }

        /// <summary>
        /// Sets the label text at the given join.
        /// </summary>
        /// <param name="join"></param>
        /// <param name="value"></param>
        [TestCase((ushort) 100, true)]
        [TestCase((ushort) 100, false)]
        public void SetLabelTextAtJoin(ushort join, bool value)
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(0, panel, null);

            instance.SetLabelTextAtJoin(join, value);

            Assert.AreEqual(value, panel.BooleanInput[100].GetValue());
        }
    }
}
