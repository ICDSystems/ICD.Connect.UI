using ICD.Connect.Panels.Devices;
using ICD.Connect.Panels.Mock;
using ICD.Connect.UI.Controls;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls
{
    public abstract class AbstractVtProSmartControlTest<T> : AbstractVtProControlTest<T, IPanelDevice>
        where T : AbstractVtProSmartControl
    {
        [Test]
        public void VisibilityDigitalJoinTest()
        {
            MockPanelDevice panel = new MockPanelDevice();

            T instance = Instantiate(1, panel, null);

            instance.DigitalVisibilityJoin = 100;

            Assert.AreEqual(100, instance.DigitalVisibilityJoin);
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
