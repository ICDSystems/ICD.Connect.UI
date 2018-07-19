using ICD.Connect.Panels;
using ICD.Connect.Panels.Mock;
using ICD.Connect.UI.Controls.Pages;
using NUnit.Framework;

namespace ICD.Connect.UI.Tests.Controls.Pages
{
	public abstract class AbstractVtProPageTest<TControl, TPanel> : AbstractVtProControlTest<TControl, TPanel>
		where TControl : AbstractVtProPage<TPanel>
		where TPanel : class, ISigInputOutput
	{
		/// <summary>
		/// The digital join offset is added to all child control digital joins.
		/// </summary>
		[TestCase((ushort)100)]
		public void DigitalJoinOffset(ushort offset)
		{
			MockPanelDevice panel = new MockPanelDevice();
			TControl instance = Instantiate(1, panel as TPanel, null);

			instance.DigitalJoinOffset = offset;

			Assert.AreEqual(offset, instance.DigitalJoinOffset);

			instance.DigitalJoinOffset = offset += 1;

			Assert.AreEqual(offset, instance.DigitalJoinOffset);
		}

		/// <summary>
		/// The analog join offset is added to all child control analog joins.
		/// </summary>
		[TestCase((ushort)100)]
		public void AnalogJoinOffset(ushort offset)
		{
			MockPanelDevice panel = new MockPanelDevice();
			TControl instance = Instantiate(1, panel as TPanel, null);

			instance.AnalogJoinOffset = offset;

			Assert.AreEqual(offset, instance.AnalogJoinOffset);

			instance.AnalogJoinOffset = offset += 1;

			Assert.AreEqual(offset, instance.AnalogJoinOffset);
		}

		/// <summary>
		/// The serial join offset is added to all child control serial joins.
		/// </summary>
		[TestCase((ushort)100)]
		public void SerialJoinOffset(ushort offset)
		{
			MockPanelDevice panel = new MockPanelDevice();
			TControl instance = Instantiate(1, panel as TPanel, null);

			instance.SerialJoinOffset = offset;

			Assert.AreEqual(offset, instance.SerialJoinOffset);

			instance.SerialJoinOffset = offset += 1;

			Assert.AreEqual(offset, instance.SerialJoinOffset);
		}

		/// <summary>
		/// Gets the digital join offset for the given control.
		/// </summary>
		/// <param name="control"></param>
		/// <returns></returns>
		[TestCase((ushort)100)]
		public void GetDigitalJoinOffset(ushort offset)
		{
			MockPanelDevice panel = new MockPanelDevice();
			TControl instance = Instantiate(1, panel as TPanel, null);

			instance.DigitalJoinOffset = offset;

			Assert.AreEqual(offset, instance.GetDigitalJoinOffset(instance));

			instance.DigitalJoinOffset = offset += 1;

			Assert.AreEqual(offset, instance.GetDigitalJoinOffset(instance));
		}

		/// <summary>
		/// Gets the analog join offset for the given control.
		/// </summary>
		/// <param name="control"></param>
		/// <returns></returns>
		[TestCase((ushort)100)]
		public void GetAnalogJoinOffset(ushort offset)
		{
			MockPanelDevice panel = new MockPanelDevice();
			TControl instance = Instantiate(1, panel as TPanel, null);

			instance.AnalogJoinOffset = offset;

			Assert.AreEqual(offset, instance.GetAnalogJoinOffset(instance));

			instance.AnalogJoinOffset = offset += 1;

			Assert.AreEqual(offset, instance.GetAnalogJoinOffset(instance));
		}

		/// <summary>
		/// Gets the serial join offset for the given control.
		/// </summary>
		/// <param name="control"></param>
		/// <returns></returns>
		[TestCase((ushort)100)]
		public void GetSerialJoinOffset(ushort offset)
		{
			MockPanelDevice panel = new MockPanelDevice();
			TControl instance = Instantiate(1, panel as TPanel, null);

			instance.SerialJoinOffset = offset;

			Assert.AreEqual(offset, instance.GetSerialJoinOffset(instance));

			instance.SerialJoinOffset = offset += 1;

			Assert.AreEqual(offset, instance.GetSerialJoinOffset(instance));
		}
	}
}