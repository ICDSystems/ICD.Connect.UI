using ICD.Connect.Panels;
using ICD.Connect.Panels.Mock;
using ICD.Connect.UI.Controls;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls
{
    public abstract class AbstractVtProControlTest<TControl,TPanel>
        where TControl : AbstractVtProControl<TPanel>
        where TPanel : class, ISigInputOutput
    {
        /// <summary>
        /// Creates a new control of the generic type.
        /// </summary>
        /// <param name="smartObjectId"></param>
        /// <param name="panel"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected abstract TControl Instantiate(ushort smartObjectId, TPanel panel, IVtProParent parent);

        protected abstract TControl Instantiate(ushort smartObjectId, TPanel panel);

        [Test]
        public void InstanceWithParentTest()
        {
            MockPanelDevice panel = new MockPanelDevice();
            TControl instance = Instantiate(0, panel as TPanel, null);

            Assert.NotNull(instance);
        }

        [Test]
        public void InstanceTest()
        {
            MockPanelDevice panel = new MockPanelDevice();
            TControl instance = Instantiate(0, panel as TPanel);

            Assert.NotNull(instance);
        }

        [TestCase((ushort) 100)]
        public void DigitalVisibilityJoinTest(ushort join)
        {
            MockPanelDevice panel = new MockPanelDevice();
            TControl instance = Instantiate(0, panel as TPanel, null);

            instance.DigitalVisibilityJoin = join;

            Assert.AreEqual(join, instance.DigitalVisibilityJoin);
        }

        [TestCase((ushort)100)]
        public void DigitalEnableJoinTest(ushort join)
        {
            MockPanelDevice panel = new MockPanelDevice();
            TControl instance = Instantiate(0, panel as TPanel, null);

            instance.DigitalEnableJoin = join;

            Assert.AreEqual(join, instance.DigitalEnableJoin);
        }

        /// <summary>
        /// Enables/disables the button. Throws InvalidOperationException if there is no enable join.
        /// </summary>
        /// <param name="state"></param>
        [TestCase(true)]
        [TestCase(false)]
        public void EnableTest(bool state)
        {
            MockPanelDevice panel = new MockPanelDevice();
            TControl instance = Instantiate(0, panel as TPanel, null);

            Assert.AreEqual(true, instance.IsEnabled);

            instance.DigitalEnableJoin = 100;

            instance.Enable(state);

            Assert.AreEqual(state, instance.IsEnabled);
        }

        /// <summary>
        /// Shows/hides the control. Throws InvalidOperationException if there is no visibility join.
        /// </summary>
        /// <param name="state"></param>
        [TestCase(true)]
        [TestCase(false)]
        public void ShowTest(bool state)
        {
            MockPanelDevice panel = new MockPanelDevice();
            TControl instance = Instantiate(0, panel as TPanel, null);

            Assert.AreEqual(true, instance.IsVisible);

            instance.DigitalVisibilityJoin = 100;

            instance.Show(state);

            Assert.AreEqual(state, instance.IsVisible);
        }
    }
}
