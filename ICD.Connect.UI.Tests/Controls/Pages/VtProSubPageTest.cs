using ICD.Connect.Panels;
using ICD.Connect.Panels.Mock;
using ICD.Connect.UI.Controls;
using ICD.Connect.UI.Controls.Lists;
using ICD.Connect.UI.Controls.Pages;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls.Pages
{
	public sealed class VtProSubpageTest : AbstractVtProPageTest<VtProSubpage, ISigInputOutput>
	{
		protected override VtProSubpage Instantiate(ushort index, ISigInputOutput panel, IVtProParent parent)
		{
			return new VtProSubpage(panel, parent, index);
		}

		protected override VtProSubpage Instantiate(ushort index, ISigInputOutput panel)
		{
			return new VtProSubpage(panel);
		}

		/// <summary>
		/// Shows/hides the control. Throws InvalidOperationException if there is no visibility join.
		/// </summary>
		/// <param name="state"></param>
		[TestCase(true)]
		[TestCase(false)]
		public void Show(bool state)
		{
			var panel = new MockPanelDevice();
			var instance = Instantiate(0, panel);

			instance.DigitalVisibilityJoin = 100;
			instance.Show(state);

			Assert.AreEqual(state, instance.IsVisible);

			var parent = new VtProSubpageReferenceList(1, panel);
			var instance2 = Instantiate(0, panel, parent);

			instance2.Show(state);

			Assert.AreEqual(state, instance2.IsVisible);
		}

		/// <summary>
		/// Enables/disables the button. Throws InvalidOperationException if there is no enable join.
		/// </summary>
		/// /// <param name="state"></param>
		[TestCase(true)]
		[TestCase(false)]
		public void Enable(bool state)
		{
			var panel = new MockPanelDevice();
			var instance = Instantiate(0, panel);

			instance.DigitalEnableJoin = 100;
			instance.Enable(state);

			Assert.AreEqual(state, instance.IsEnabled);

			var parent = new VtProSubpageReferenceList(1, panel);
			var instance2 = Instantiate(0, panel, parent);

			instance2.Enable(state);

			Assert.AreEqual(state, instance2.IsEnabled);
		}
	}
}
