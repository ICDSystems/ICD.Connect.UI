using System;
using System.Collections.Generic;
using ICD.Common.Utils;
using ICD.Connect.Panels.Mock;
using ICD.Connect.Protocol.Sigs;
using ICD.Connect.UI.Controls.Buttons;
using ICD.Connect.UI.Tests.Controls.TextControls;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls.Buttons
{
    public abstract class AbstractVtProButtonTest<T> : AbstractVtProLabelTest<T>
        where T : AbstractVtProButton
    {
        [Test]
        public void OnPressedFeedbackTest()
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);
            instance.DigitalPressJoin = 100;

            List<EventArgs> callbacks = new List<EventArgs>();

            instance.OnPressed += (sender, args) => callbacks.Add(args);

            panel.RaiseOutputSigChange(new SigInfo(100, 0, true));
            panel.RaiseOutputSigChange(new SigInfo(100, 0, false));
            panel.RaiseOutputSigChange(new SigInfo(100, 0, true));
            panel.RaiseOutputSigChange(new SigInfo(100, 0, false));

            Assert.AreEqual(2, callbacks.Count);
        }

        [Test]
        public void OnReleasedFeedbackTest()
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);
            instance.DigitalPressJoin = 100;

            List<EventArgs> callbacks = new List<EventArgs>();

            instance.OnReleased += (sender, args) => callbacks.Add(args);

            panel.RaiseOutputSigChange(new SigInfo(100, 0, true));
            panel.RaiseOutputSigChange(new SigInfo(100, 0, false));
            panel.RaiseOutputSigChange(new SigInfo(100, 0, true));
            panel.RaiseOutputSigChange(new SigInfo(100, 0, false));

            Assert.AreEqual(2, callbacks.Count);
        }

        [Test]
        public void OnHeldFeedbackTest()
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);
            instance.DigitalPressJoin = 100;
            instance.HoldDuration = 500;

            List<EventArgs> callbacks = new List<EventArgs>();

            instance.OnHeld += (sender, args) => callbacks.Add(args);

            panel.RaiseOutputSigChange(new SigInfo(100, 0, true));

            Assert.AreEqual(0, callbacks.Count);

            ThreadingUtils.Sleep(1000);

            panel.RaiseOutputSigChange(new SigInfo(100, 0, false));
            panel.RaiseOutputSigChange(new SigInfo(100, 0, true));

            ThreadingUtils.Sleep(100);

            panel.RaiseOutputSigChange(new SigInfo(100, 0, false));

            Assert.AreEqual(1, callbacks.Count);
        }

        [TestCase((ushort) 100)]
        public void DigitalPressJoinTest(ushort join)
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);

            instance.DigitalPressJoin = 1;

            Assert.AreEqual(instance.DigitalPressJoin, 1);

            instance.DigitalPressJoin = join;

            Assert.AreEqual(instance.DigitalPressJoin, join);

            List<EventArgs> callbacks = new List<EventArgs>();

            instance.OnPressed += (sender, args) => callbacks.Add(args);
            instance.OnReleased += (sender, args) => callbacks.Add(args);

            panel.RaiseOutputSigChange(new SigInfo(100, 0, true));
            panel.RaiseOutputSigChange(new SigInfo(100, 0, false));
            panel.RaiseOutputSigChange(new SigInfo(100, 0, true));
            panel.RaiseOutputSigChange(new SigInfo(100, 0, false));

            Assert.AreEqual(4, callbacks.Count);

            instance.DigitalPressJoin = join;

            Assert.AreEqual(instance.DigitalPressJoin, join);
        }

        [Test]
        public void PressTest()
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);

            List<EventArgs> callbacks = new List<EventArgs>();

            instance.OnPressed += (sender, args) => callbacks.Add(args);

            instance.Press();

            Assert.AreEqual(1, callbacks.Count);
        }

        [Test]
        public void ReleaseTest()
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);

            List<EventArgs> callbacks = new List<EventArgs>();

            instance.OnReleased += (sender, args) => callbacks.Add(args);

            instance.Release();

            Assert.AreEqual(1, callbacks.Count);
        }

        [Test]
        public void HoldTest()
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);

            List<EventArgs> callbacks = new List<EventArgs>();

            instance.OnHeld += (sender, args) => callbacks.Add(args);

            instance.Hold();

            Assert.AreEqual(1, callbacks.Count);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SetSelectedTest(bool state)
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);
            instance.DigitalPressJoin = 100;

            instance.SetSelected(state);

            Assert.AreEqual(state, panel.BooleanInput[100].GetBoolValue());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SetSelectedTestException(bool state)
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);

            Assert.Throws<InvalidOperationException>(() => instance.SetSelected(state));
        }

        [Test]
        public void DisposeTest()
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);

            instance.Dispose();

            Assert.Pass();
        }
    }
}
