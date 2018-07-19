using System.Linq;
using ICD.Connect.Panels.Mock;
using ICD.Connect.UI.Controls.Lists;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls.Lists
{
    public abstract class AbstractVtProButtonListTest<T> : AbstractVtProListTest<T>
    where T : AbstractVtProButtonList
    {
        /// <summary>
        /// Sets the button labels.
        /// </summary>
        /// <param name="labels"></param>
        [TestCase("Test1", "Test2", "Test3")]
        public void SetItemLabels(params string[] labels)
        {
            var panel = new MockPanelDevice();
            T instance = Instantiate(1, panel);

            instance.SetItemLabels(labels);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            CollectionAssert.AreEqual(labels, smartObject.StringInput.Select(s => s.GetStringValue()).ToArray());
        }

        /// <summary>
        /// Sets the button icons.
        /// </summary>
        /// <param name="icons"></param>
        [TestCase("Test1", "Test2", "Test3")]
        public void SetItemIcons(params string[] icons)
        {
            var panel = new MockPanelDevice();
            T instance = Instantiate(1, panel);

            instance.SetItemIcons(icons);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            CollectionAssert.AreEqual(icons, smartObject.StringInput.Select(s => s.GetStringValue()).ToArray());
        }
    }
}
