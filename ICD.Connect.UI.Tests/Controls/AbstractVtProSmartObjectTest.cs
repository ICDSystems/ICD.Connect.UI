using ICD.Common.Properties;
using ICD.Connect.Panels;
using ICD.Connect.Panels.Mock;
using ICD.Connect.Panels.SmartObjects;
using ICD.Connect.UI.Controls;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls
{
    [TestFixture]
    public abstract class AbstractVtProSmartObjectTest<T> : AbstractVtProControlTest<T, IPanelDevice>
        where T : AbstractVtProSmartObject
    {
        [Test]
        public void VisibilityDigitalJoinTest()
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(0, panel, null);

            instance.VisibilityDigitalJoin = 100;

            Assert.AreEqual(100, instance.VisibilityDigitalJoin);
        }

        [Test]
        public void DisposeTest()
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(0, panel, null);

            instance.Dispose();

            Assert.Pass();
        }
    }
}
