using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils.EventArguments;
using ICD.Connect.Panels;
using ICD.Connect.Panels.Mock;
using ICD.Connect.Protocol.Sigs;
using ICD.Connect.UI.Controls;
using ICD.Connect.UI.Controls.Lists;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls.Lists
{
    [TestFixture]
    public sealed class VtProDynamicButtonListTest : AbstractVtProButtonListTest<VtProDynamicButtonList>
    {
        protected override VtProDynamicButtonList Instantiate(ushort smartObjectId, IPanelDevice panel, IVtProParent parent)
        {
            return new VtProDynamicButtonList(smartObjectId, panel, parent);
        }

        protected override VtProDynamicButtonList Instantiate(ushort smartObjectId, IPanelDevice panel)
        {
            return new VtProDynamicButtonList(smartObjectId, panel);
        }

        /// <summary>
		/// Release resources.
		/// </summary>
		[Test]
		public void Dispose()
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(1, panel);

            instance.Dispose();

            Assert.Pass();
        }

        /// <summary>
        /// Simulates a release on the button at the given index.
        /// </summary>
        /// <param name="index"></param>
        [TestCase((ushort)100)]
        public void Release(ushort index)
        {
            var callbacks = new List<UShortEventArgs>();
            var panel = new MockPanelDevice();
            var instance = Instantiate(1, panel);

            instance.OnButtonReleased += (sender, args) => callbacks.Add(args);

            instance.Release(index);

            Assert.AreEqual(1, callbacks.Count);
            Assert.AreEqual(index, callbacks.Select(a => a.Data).First());

            instance.Click(index);
            instance.Hold(index);

            Assert.AreEqual(1, callbacks.Count);
        }

        /// <summary>
        /// Simulates a click on the button at the given index.
        /// </summary>
        /// <param name="index"></param>
        [TestCase((ushort)100)]
        public void Click(ushort index)
        {
            var callbacks = new List<UShortEventArgs>();
            var panel = new MockPanelDevice();
            var instance = Instantiate(1, panel);

            instance.OnButtonClicked += (sender, args) => callbacks.Add(args);

            instance.Click(index);

            Assert.AreEqual(1, callbacks.Count);
            Assert.AreEqual(index, callbacks.Select(a => a.Data).First());

            instance.Hold(index);
            instance.Release(index);

            Assert.AreEqual(1, callbacks.Count);
        }

        /// <summary>
        /// Simulates the button at the given index being held.
        /// </summary>
        /// <param name="index"></param>
        [TestCase((ushort)100)]
        public void Hold(ushort index)
        {
            var callbacks = new List<UShortEventArgs>();
            var panel = new MockPanelDevice();
            var instance = Instantiate(1, panel);

            instance.OnButtonHeld += (sender, args) => callbacks.Add(args);

            instance.Hold(index);

            Assert.AreEqual(1, callbacks.Count);
            Assert.AreEqual(index, callbacks.Select(a => a.Data).First());

            instance.Click(index);
            instance.Release(index);

            Assert.AreEqual(1, callbacks.Count);
        }

        /// <summary>
        /// Sets the visible state for the button at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="visible"></param>
        [TestCase((ushort)100, true)]
        [TestCase((ushort)100, false)]
        public void SetItemVisible(ushort index, bool visible)
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(1, panel);

            instance.SetItemVisible(index, visible);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(visible, smartObject.BooleanInput.First().GetBoolValue());
        }

        /// <summary>
        /// Sets the enabled state for the button at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="enabled"></param>
        [TestCase((ushort)100, true)]
        [TestCase((ushort)100, false)]
        public void SetItemEnabled(ushort index, bool enabled)
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(1, panel);

            instance.SetItemEnabled(index, enabled);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(enabled, smartObject.BooleanInput.First().GetBoolValue());
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
            var panel = new MockPanelDevice();
            var instance = Instantiate(1, panel);

            instance.SetItemSelected(index, selected);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(selected, smartObject.BooleanInput.First().GetBoolValue());
        }

        /// <summary>
        /// Sets the button label.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="text"></param>
        [TestCase((ushort)100, "Test")]
        public void SetItemLabel(ushort index, string text)
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(1, panel);

            instance.SetItemIcon(index, text);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(text, smartObject.StringInput.First().GetStringValue());
        }

        /// <summary>
        /// Sets the button icon.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="icon"></param>
        [TestCase((ushort)100, "Test")]
        public void SetItemIcon(ushort index, string icon)
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(1, panel);

            instance.SetItemIcon(index, icon);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(icon, smartObject.StringInput.First().GetStringValue());
        }
    }
}
