using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICD.Connect.Panels;
using ICD.Connect.Panels.Mock;
using ICD.Connect.UI.Controls;
using ICD.Connect.UI.Controls.Lists;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls.Lists
{
    public sealed class VtProSubpageReferenceListTest : AbstractVtProListTest<VtProSubpageReferenceList>
    {
        protected override VtProSubpageReferenceList Instantiate(ushort smartObjectId, IPanelDevice panel, IVtProParent parent)
        {
            return new VtProSubpageReferenceList(smartObjectId, panel, parent);
        }

        protected override VtProSubpageReferenceList Instantiate(ushort smartObjectId, IPanelDevice panel)
        {
            return new VtProSubpageReferenceList(smartObjectId, panel, null);
        }

        /// <summary>
        /// Sets the visible state for the item at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="visible"></param>
        [TestCase((ushort)100, true)]
        public void SetItemVisible(ushort index, bool visible)
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(0, panel);

            instance.SetItemVisible(index, visible);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(visible, smartObject.BooleanInput.First().GetBoolValue());

            instance.SetItemVisible(index, !visible);

            Assert.AreEqual(!visible, smartObject.BooleanInput.First().GetBoolValue());
        }

        /// <summary>
        /// Gets the visibility of the item at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [TestCase((ushort)100)]
        public void GetItemVisible(ushort index)
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(0, panel);

            instance.SetItemVisible(index, true);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(true, instance.GetItemVisible(index));

            instance.SetItemVisible(index, false);

            Assert.AreEqual(false, instance.GetItemVisible(index));
        }

        /// <summary>
        /// Sets the enabled state for the item at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="enabled"></param>
        [TestCase((ushort)100, true)]
        public void SetItemEnabled(ushort index, bool enabled)
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(0, panel);

            instance.SetItemEnabled(index, enabled);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(enabled, smartObject.BooleanInput.First().GetBoolValue());

            instance.SetItemEnabled(index, !enabled);

            Assert.AreEqual(!enabled, smartObject.BooleanInput.First().GetBoolValue());
        }

        /// <summary>
        /// Gets the enabled state of the item at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [TestCase((ushort)100)]
        public void GetItemEnabled(ushort index)
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(0, panel);

            instance.SetItemEnabled(index, true);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(true, instance.GetItemEnabled(index));

            instance.SetItemEnabled(index, false);

            Assert.AreEqual(false, instance.GetItemEnabled(index));
        }

        /// <summary>
        /// Gets the digital join offset for the given control.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        [Test]
        public void GetDigitalJoinOffset()
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(0, panel);

            var output = instance.GetDigitalJoinOffset(instance);

            Assert.NotNull(output);

        }

        /// <summary>
        /// Gets the analog join offset for the given control.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        [Test]
        public void GetAnalogJoinOffset()
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(0, panel);

            var output = instance.GetAnalogJoinOffset(instance);

            Assert.NotNull(output);
        }

        /// <summary>
        /// Gets the serial join offset for the given control.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        [Test]
        public void GetSerialJoinOffset()
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(0, panel);

            var output = instance.GetSerialJoinOffset(instance);

            Assert.NotNull(output);
        }
    }
}
