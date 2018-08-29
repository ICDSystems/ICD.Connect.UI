using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICD.Connect.Panels;
using ICD.Connect.Panels.Devices;
using ICD.Connect.Panels.Mock;
using ICD.Connect.UI.Controls;
using ICD.Connect.UI.Controls.Buttons;
using ICD.Connect.UI.Controls.Lists;
using ICD.Connect.UI.Controls.Pages;
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
            var instance = Instantiate(1, panel);

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
            var instance = Instantiate(1, panel);

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
            var instance = Instantiate(1, panel);

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
            var instance = Instantiate(1, panel);

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
	    /// <param name="visible"></param>
	    /// <returns></returns>
	    [TestCase(true)]
        [TestCase(false)]
		public void GetDigitalJoinOffset(bool visible)
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(1, panel);

			VtProSubpage subpage1 = new VtProSubpage(panel, instance, 0);
			VtProButton button1 = new VtProButton(panel, subpage1)
			{
				DigitalVisibilityJoin = 1
			};

	        VtProSubpage subpage2 = new VtProSubpage(panel, instance, 1);
	        VtProButton button2 = new VtProButton(panel, subpage2)
	        {
		        DigitalVisibilityJoin = 1
	        };

	        VtProSubpage subpage3 = new VtProSubpage(panel, instance, 2);
	        VtProButton button3 = new VtProButton(panel, subpage3)
	        {
		        DigitalVisibilityJoin = 1
	        };

	        instance.DigitalJoinIncrement = 10;

			Assert.AreEqual(4010, instance.GetDigitalJoinOffset(subpage1));
	        Assert.AreEqual(4020, instance.GetDigitalJoinOffset(subpage2));
	        Assert.AreEqual(4030, instance.GetDigitalJoinOffset(subpage3));

	        var smartobj = panel.SmartObjects[1] as MockSmartObject;

	        Assert.NotNull(smartobj);

			button1.Show(visible);
	        button2.Show(visible);
	        button3.Show(visible);

			Assert.AreEqual(visible, panel.BooleanInput[4011].GetBoolValue());
	        Assert.AreEqual(visible, panel.BooleanInput[4021].GetBoolValue());
	        Assert.AreEqual(visible, panel.BooleanInput[4031].GetBoolValue());

		}

		/// <summary>
		/// Gets the analog join offset for the given control.
		/// </summary>
		/// <returns></returns>
		[Test]
		public void GetAnalogJoinOffset()
        {
	        var panel = new MockPanelDevice();
	        var instance = Instantiate(1, panel);

	        VtProSubpage subpage1 = new VtProSubpage(panel, instance, 0);
	        VtProButton button1 = new VtProButton(panel, subpage1);

	        VtProSubpage subpage2 = new VtProSubpage(panel, instance, 1);
	        VtProButton button2 = new VtProButton(panel, subpage2);

	        VtProSubpage subpage3 = new VtProSubpage(panel, instance, 2);
	        VtProButton button3 = new VtProButton(panel, subpage3);

	        instance.AnalogJoinIncrement = 10;

	        Assert.AreEqual(10, instance.GetAnalogJoinOffset(subpage1));
	        Assert.AreEqual(20, instance.GetAnalogJoinOffset(subpage2));
	        Assert.AreEqual(30, instance.GetAnalogJoinOffset(subpage3));

	        var smartobj = panel.SmartObjects[1] as MockSmartObject;

	        Assert.NotNull(smartobj);

	        button1.SetLabelTextAtJoin(1, "Text1");
	        button2.SetLabelTextAtJoin(11, "Text2");
	        button3.SetLabelTextAtJoin(21, "Text3");

	        Assert.AreEqual("Text1", panel.StringInput[11].GetStringValue());
	        Assert.AreEqual("Text2", panel.StringInput[21].GetStringValue());
	        Assert.AreEqual("Text3", panel.StringInput[31].GetStringValue());
		}

		/// <summary>
		/// Gets the serial join offset for the given control.
		/// </summary>
		/// <returns></returns>
		[Test]
		public void GetSerialJoinOffset()
		{
			var panel = new MockPanelDevice();
			var instance = Instantiate(1, panel);

			VtProSubpage subpage1 = new VtProSubpage(panel, instance, 0);
			VtProButton button1 = new VtProButton(panel, subpage1);

			VtProSubpage subpage2 = new VtProSubpage(panel, instance, 1);
			VtProButton button2 = new VtProButton(panel, subpage2);

			VtProSubpage subpage3 = new VtProSubpage(panel, instance, 2);
			VtProButton button3 = new VtProButton(panel, subpage3);

			instance.SerialJoinIncrement = 10;

			Assert.AreEqual(10, instance.GetSerialJoinOffset(subpage1));
			Assert.AreEqual(20, instance.GetSerialJoinOffset(subpage2));
			Assert.AreEqual(30, instance.GetSerialJoinOffset(subpage3));

			var smartobj = panel.SmartObjects[1] as MockSmartObject;

			Assert.NotNull(smartobj);

			button1.SetLabelTextAtJoin(1, 1);
			button2.SetLabelTextAtJoin(11, 50);
			button3.SetLabelTextAtJoin(21, 100);

			Assert.AreEqual(1, panel.UShortInput[11].GetUShortValue());
			Assert.AreEqual(50, panel.UShortInput[21].GetUShortValue());
			Assert.AreEqual(100, panel.UShortInput[31].GetUShortValue());
		}
    }
}
