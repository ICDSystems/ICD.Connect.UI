using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Panels;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.Mock;
using ICD.Connect.Protocol.Sigs;
using ICD.Connect.UI.Controls.Lists;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls.Lists
{
    [TestFixture]
    public abstract class AbstractVtProListTest<T> : AbstractVtProSmartObjectTest<T>
    where T : AbstractVtProList
    {
        /// <summary>
        /// Release resources.
        /// </summary>
        [Test]
        private void Dispose()
        {
            var panel = new MockPanelDevice();
            T instance = Instantiate(0, panel);
            instance.Dispose();

            Assert.Pass();
        }

        /// <summary>
        /// Scrolls to the given item in the list.
        /// </summary>
        /// <param name="item"></param>
        [TestCase((ushort)100)]
        public void ScrollToItem(ushort item)
        {
            var panel = new MockPanelDevice();
            T instance = Instantiate(0, panel);

            instance.ScrollToItem(item);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(item + 1, smartObject.UShortInput.First().GetValue());
        }

        /// <summary>
        /// Sets the number of items in the list.
        /// </summary>
        /// <param name="count"></param>
        [TestCase((ushort)100)]
        public void SetNumberOfItems(ushort count)
        {
            var panel = new MockPanelDevice();
            T instance = Instantiate(0, panel);

            instance.SetNumberOfItems(count);

            var smartObject = instance.SmartObject as MockSmartObject;

            Assert.NotNull(smartObject);

            Assert.AreEqual(count, smartObject.UShortInput.First().GetValue());
        }

        /// <summary>
        /// Simulates the user starting or stopping scrolling the list.
        /// </summary>
        /// <param name="isMoving"></param>
        [TestCase(true)]
        [TestCase(false)]
        public void SetIsMoving(bool isMoving)
        {
            var callbackArgs = new List<BoolEventArgs>();

            var panel = new MockPanelDevice();
            T instance = Instantiate(0, panel);

            instance.OnIsMovingChanged += (sender, args) => callbackArgs.Add(args);

            instance.SetIsMoving(isMoving);

            Assert.AreEqual(isMoving, callbackArgs.Select(a => a.Data).First());
        }
    }
}
