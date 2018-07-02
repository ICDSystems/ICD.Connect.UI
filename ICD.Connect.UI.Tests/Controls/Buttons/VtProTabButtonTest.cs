using System;
using System.Collections.Generic;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Panels;
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

        [Test]
        public void OnButtonPressed()
        {
            MockPanelDevice panel = new MockPanelDevice();

            VtProTabButton instance = Instantiate(0, panel, null);

            List<EventArgs> callbacks = new List<EventArgs>();

            instance.OnButtonPressed += (sender, args) => callbacks.Add(args);

            instance.Press(100);
            instance.Press(200);
            instance.Press(300);

            Assert.AreEqual(3, callbacks.Count);
        }

        [Test]
        public void OnButtonReleased()
        {
            MockPanelDevice panel = new MockPanelDevice();

            VtProTabButton instance = Instantiate(0, panel, null);

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
        public void Press(ushort index)
        {
            //if (IsVisibleRecursive)
            //    OnButtonPressed.Raise(this, new UShortEventArgs(index));
            Assert.Inconclusive();
        }

        /// <summary>
        /// Simulates a release on the button at the given index.
        /// </summary>
        /// <param name="index"></param>
        [TestCase((ushort)100)]
        public void Release(ushort index)
        {
            //if (IsVisibleRecursive)
            //    OnButtonReleased.Raise(this, new UShortEventArgs(index));
            Assert.Inconclusive();
        }

        /// <summary>
        /// Sets the selected state for the button at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="selected"></param>
        [TestCase((ushort)100, true)]
        [TestCase((ushort)100, false)]
        public void SetItemSelected(ushort index, bool selected)
        {
            //m_SelectionCacheSection.Enter();

            //try
            //{
            //    if (selected == m_SelectionCache.GetDefault(index, false))
            //        return;

            //    m_SelectionCache[index] = selected;

            //    ushort join = (ushort)((index * DIGITAL_INCREMENT) + DIGITAL_SELECT_START_JOIN);
            //    SmartObject.SendInputDigital(join, selected);
            //}
            //finally
            //{
            //    m_SelectionCacheSection.Leave();
            //}
            Assert.Inconclusive();
        }
    }
}
