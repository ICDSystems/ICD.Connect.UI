using System.Linq;
using ICD.Connect.Panels;
using ICD.Connect.Panels.Mock;
using ICD.Connect.UI.Controls;
using ICD.Connect.UI.Controls.Lists;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls.Lists
{
    [TestFixture]
    public sealed class VtProSpinnerListTest : AbstractVtProSmartObjectTest<VtProSpinnerList>
    {
        protected override VtProSpinnerList Instantiate(ushort smartObjectId, IPanelDevice panel, IVtProParent parent)
        {
            return new VtProSpinnerList(smartObjectId, panel, parent);
        }

        protected override VtProSpinnerList Instantiate(ushort smartObjectId, IPanelDevice panel)
        {
            return new VtProSpinnerList(smartObjectId, panel, null);
        }

        /// <summary>
		/// Sets the number of items in the list.
		/// </summary>
		/// <param name="count"></param>
        [TestCase((ushort)100)]
        public void SetNumberOfItems(ushort count)
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(0, panel);

            instance.SetNumberOfItems(count);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(count, smartObject.UShortInput.First().GetUShortValue());
        }

        /// <summary>
        /// Selects the item at the given index.
        /// </summary>
        /// <param name="item"></param>
        [TestCase((ushort)100)]
        public void SelectItem(ushort item)
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(0, panel);

            instance.SelectItem(item);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(item + 1, smartObject.UShortInput.First().GetUShortValue());
        }

        /// <summary>
        /// Scrolls to the next item.
        /// </summary>
        [Test]
        public void NextItem()
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(0, panel);

            instance.NextItem();

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(true, smartObject.BooleanInput.First().GetBoolValue());
        }

        /// <summary>
        /// Scrolls to the previous item
        /// </summary>
        [Test]
        public void PreviousItem()
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(0, panel);

            instance.PreviousItem();

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(true, smartObject.BooleanInput.First().GetBoolValue());
        }

        /// <summary>
        /// Sets the visibility of the item at the given index.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="visible"></param>
        [TestCase((ushort)100, true)]
        [TestCase((ushort)100, false)]
        public void SetItemVisible(ushort item, bool visible)
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(0, panel);

            instance.SetItemVisible(item, visible);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(visible, smartObject.BooleanInput.First().GetBoolValue());
        }

        /// <summary>
        /// Sets the label for the item at the given index.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="label"></param>
        [TestCase((ushort)100, "Test")]
        public void SetItemLabel(ushort item, string label)
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(0, panel);

            instance.SetItemLabel(item, label);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(label, smartObject.StringInput.First().GetStringValue());
        }

        /// <summary>
        /// Sets the number of items and the item labels.
        /// </summary>
        /// <param name="labels"></param>
        [TestCase("Test1","Test2","Test3")]
        public void SetItemLabels(params string[] labels)
        {
            var panel = new MockPanelDevice();
            var instance = Instantiate(0, panel);

            instance.SetItemLabels(labels);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(labels, smartObject.StringInput.Select(s => s.GetStringValue()).ToArray());
        }
    }
}
