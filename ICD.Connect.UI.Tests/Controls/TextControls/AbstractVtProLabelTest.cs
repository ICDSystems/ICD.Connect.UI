using System;
using System.Linq;
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
        [TestCase((ushort)100)]
        public void SerialLabelJoinsTest(ushort join)
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);

            instance.SerialLabelJoins.Add(100);

            Assert.AreEqual(join, instance.SerialLabelJoins.First());
        }

        [TestCase((ushort)100)]
        public void AnalogLabelJoinsTest(ushort join)
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);

            instance.AnalogLabelJoins.Add(100);

            Assert.AreEqual(join, instance.AnalogLabelJoins.First());
        }

        [TestCase((ushort)100)]
        public void DigitalLabelJoinsTest(ushort join)
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);

            instance.DigitalLabelJoins.Add(100);

            Assert.AreEqual(join, instance.DigitalLabelJoins.First());
        }

        [TestCase("Test")]
        public void SetLabelTextTest(string text)
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);

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
        public void SetLabelTextAtJoinTest(ushort join, string text)
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);

            instance.SetLabelTextAtJoin(join, text);

            Assert.AreEqual(text, panel.StringInput[100].GetValue());

            Assert.DoesNotThrow(() => instance.SetLabelTextAtJoin(join, text));

            Assert.Throws<InvalidOperationException>(() => instance.SetLabelTextAtJoin(0, text));
        }

        /// <summary>
        /// Sets the label text at the given join.
        /// </summary>
        /// <param name="join"></param>
        /// <param name="value"></param>
        [TestCase((ushort) 100, (ushort) 100)]
        public void SetLabelTextAtJoinTest(ushort join, ushort value)
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);

            instance.SetLabelTextAtJoin(join, value);

            Assert.AreEqual(value, panel.UShortInput[100].GetValue());

            Assert.DoesNotThrow(() => instance.SetLabelTextAtJoin(join, value));

            Assert.Throws<InvalidOperationException>(() => instance.SetLabelTextAtJoin(0, value));
        }

        /// <summary>
        /// Sets the label text at the given join.
        /// </summary>
        /// <param name="join"></param>
        /// <param name="value"></param>
        [TestCase((ushort) 100, true)]
        [TestCase((ushort) 100, false)]
        public void SetLabelTextAtJoinTest(ushort join, bool value)
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);

            instance.SetLabelTextAtJoin(join, value);

            Assert.AreEqual(value, panel.BooleanInput[100].GetValue());

            Assert.DoesNotThrow(() => instance.SetLabelTextAtJoin(join, value));

            Assert.Throws<InvalidOperationException>(() => instance.SetLabelTextAtJoin(0, value));
        }
    }
}
