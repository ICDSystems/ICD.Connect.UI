using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Panels;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.Mock;
using ICD.Connect.Protocol.Sigs;
using ICD.Connect.UI.Controls;
using ICD.Connect.UI.Controls.Buttons;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls.Buttons
{
    [TestFixture]
    public sealed class VtProTabButtonTest : AbstractVtProSmartObjectTest<VtProTabButton>
    {
        protected override VtProTabButton Instantiate(ushort smartObjectId, IPanelDevice panel, IVtProParent parent)
        {
            return new VtProTabButton(smartObjectId, panel, parent);
        }

        protected override VtProTabButton Instantiate(ushort smartObjectId, IPanelDevice panel)
        {
            return new VtProTabButton(smartObjectId, panel, null);
        }

        [Test]
        public void OnButtonPressedTest()
        {
            MockPanelDevice panel = new MockPanelDevice();

            VtProTabButton instance = Instantiate(1, panel, null);

            List<EventArgs> callbacks = new List<EventArgs>();

            instance.OnButtonPressed += (sender, args) => callbacks.Add(args);

            instance.Press(100);
            instance.Press(200);
            instance.Press(300);

            Assert.AreEqual(3, callbacks.Count);
        }

        [Test]
        public void OnButtonReleasedTest()
        {
            MockPanelDevice panel = new MockPanelDevice();

            VtProTabButton instance = Instantiate(1, panel, null);

            List<EventArgs> callbacks = new List<EventArgs>();

            instance.OnButtonReleased += (sender, args) => callbacks.Add(args);

            instance.Release(100);
            instance.Release(200);
            instance.Release(300);

            Assert.AreEqual(3, callbacks.Count);
        }

        /// <summary>
        /// Simulates a Press on the button at the given index.
        /// </summary>
        /// <param name="index"></param>
        [TestCase((ushort)100)]
        public void PressTest(ushort index)
        {
            MockPanelDevice panel = new MockPanelDevice();

            VtProTabButton instance = Instantiate(1, panel, null);

            List<EventArgs> callbacks = new List<EventArgs>();

            instance.OnButtonPressed += (sender, args) => callbacks.Add(args);

            instance.Press(100);

            instance.Press(101);

            Assert.AreEqual(2, callbacks.Count);
        }

        /// <summary>
        /// Simulates a release on the button at the given index.
        /// </summary>
        /// <param name="index"></param>
        [TestCase((ushort)100)]
        public void ReleaseTest(ushort index)
        {
            MockPanelDevice panel = new MockPanelDevice();

            VtProTabButton instance = Instantiate(1, panel, null);

            List<EventArgs> callbacks = new List<EventArgs>();

            instance.OnButtonReleased += (sender, args) => callbacks.Add(args);

            instance.Release(100);

            instance.Release(101);

            Assert.AreEqual(2, callbacks.Count);
        }

        /// <summary>
        /// Sets the selected state for the button at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="selected"></param>
        [TestCase((ushort)100, true)]
        [TestCase((ushort)100, false)]
        public void SetItemSelectedTest(ushort index, bool selected)
        {
            Assert.Inconclusive();

            MockPanelDevice panel = new MockPanelDevice();

            VtProTabButton instance = Instantiate(1, panel, null);

            instance.SetItemSelected(index, selected);

            Assert.Inconclusive();
        }

        /// <summary>
        /// Called when the SmartObject raises an output event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [Test]
        public void SmartObjectOnAnyOutputTest()
        {
            Assert.Inconclusive();

            MockPanelDevice panel = new MockPanelDevice();

            VtProTabButton instance = Instantiate(1, panel, null);

            List<SigInfoEventArgs> callbackArgs = new List<SigInfoEventArgs>();

            instance.SmartObject.OnAnyOutput += (sender, args) => callbackArgs.Add(args);

            Assert.AreEqual(1, callbackArgs.Count);
        }

    }
}
