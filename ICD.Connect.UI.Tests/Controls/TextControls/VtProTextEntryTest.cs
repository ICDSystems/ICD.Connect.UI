using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Panels;
using ICD.Connect.Panels.Mock;
using ICD.Connect.Protocol.Sigs;
using ICD.Connect.UI.Controls;
using ICD.Connect.UI.Controls.TextControls;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls.TextControls
{
    [TestFixture]
    public sealed class VtProTextEntryTest : AbstractVtProLabelTest<VtProTextEntry>
    {
        protected override VtProTextEntry Instantiate(ushort smartObjectId, ISigInputOutput panel, IVtProParent parent)
        {
            return new VtProTextEntry(panel, parent);
        }
        protected override VtProTextEntry Instantiate(ushort smartObjectId, ISigInputOutput panel)
        {
            return new VtProTextEntry(panel);
        }

        /// <summary>
        /// Release resources.
        /// </summary>
        [Test]
        public void DisposeTest()
        {
            MockPanelDevice panel = new MockPanelDevice();

            var instance = Instantiate(1, panel, null);

            instance.Dispose();

            Assert.Pass();
        }

        /// <summary>
        /// Simulates text input and raises the OnTextModified event.
        /// </summary>
        [TestCase("Test")]
        public void SetTextEntry(string text)
        {
            MockPanelDevice panel = new MockPanelDevice();

            var instance = Instantiate(1, panel, null);
            instance.AnalogModeJoin = 10;

            List<EventArgs> callbacks = new List<EventArgs>();

            instance.OnTextModified += (sender, args) => callbacks.Add(args);

            instance.SetTextEntry(text);

            Assert.AreEqual(1, callbacks.Count);
        }

        /// <summary>
        /// Sets the button mode. Throws InvalidOperationException if there is no mode join.
        /// </summary>
        /// <param name="mode"></param>
        [TestCase((ushort)100)]
        public void SetModeTest(ushort mode)
        {
            MockPanelDevice panel = new MockPanelDevice();

            var instance = Instantiate(1, panel, null);

            Assert.Throws<InvalidOperationException>(() => instance.SetMode(mode));

            instance.AnalogModeJoin = 10;

            instance.SetMode(mode);

            Assert.AreEqual(mode, panel.UShortInput[10].GetValue());

            instance.SetMode(mode);

            Assert.AreEqual(mode, panel.UShortInput[10].GetValue());
        }

        [TestCase((ushort)100)]
        public void EnterKeyPressJoinTest(ushort join)
        {
            MockPanelDevice panel = new MockPanelDevice();
            var instance = Instantiate(1, panel, null);

            instance.EnterKeyPressJoin = join;

            Assert.AreEqual(join, instance.EnterKeyPressJoin);
        }

        [TestCase((ushort)100)]
        public void EscKeyPressJoinTest(ushort join)
        {
            MockPanelDevice panel = new MockPanelDevice();
            var instance = Instantiate(1, panel, null);

            instance.EscKeyPressJoin = join;

            Assert.AreEqual(join, instance.EscKeyPressJoin);
        }

        [TestCase((ushort)100)]
        public void AnalogModeJoinTest(ushort join)
        {
            MockPanelDevice panel = new MockPanelDevice();
            var instance = Instantiate(1, panel, null);

            instance.AnalogModeJoin = join;

            Assert.AreEqual(join, instance.AnalogModeJoin);
        }

        [TestCase((ushort)100)]
        public void SetFocusJoinOnTest(ushort join)
        {
            MockPanelDevice panel = new MockPanelDevice();
            var instance = Instantiate(1, panel, null);

            instance.SetFocusJoinOn = join;

            Assert.AreEqual(join, instance.SetFocusJoinOn);
        }

        [TestCase((ushort)100)]
        public void SetFocusJoinOffTest(ushort join)
        {
            MockPanelDevice panel = new MockPanelDevice();
            var instance = Instantiate(1, panel, null);

            instance.SetFocusJoinOff = join;

            Assert.AreEqual(join, instance.SetFocusJoinOff);
        }

        [TestCase((ushort)100)]
        public void HasFocusJoinTest(ushort join)
        {
            MockPanelDevice panel = new MockPanelDevice();
            var instance = Instantiate(1, panel, null);

            instance.HasFocusJoin = join;

            Assert.AreEqual(join, instance.HasFocusJoin);
        }

        [TestCase((ushort)100)]
        public void SerialOutputJoinTest(ushort join)
        {
            MockPanelDevice panel = new MockPanelDevice();
            var instance = Instantiate(1, panel, null);

            instance.SerialOutputJoin = join;

            Assert.AreEqual(join, instance.SerialOutputJoin);

            instance.SerialOutputJoin = join;

            Assert.AreEqual(join, instance.SerialOutputJoin);

            instance.SerialOutputJoin = 0;

            Assert.AreEqual(0, instance.SerialOutputJoin);
        }
    }
}
