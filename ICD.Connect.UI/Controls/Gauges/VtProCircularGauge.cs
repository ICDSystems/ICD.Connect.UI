using System;
using ICD.Common.Utils;
using ICD.Connect.Panels;

namespace ICD.Connect.UI.Controls.Gauges
{
	public sealed class VtProCircularGauge : AbstractVtProGauge
	{
		private ushort m_MinValueCache;
		private readonly SafeCriticalSection m_MinValueCacheSection;
		private ushort m_MaxValueCache;
		private readonly SafeCriticalSection m_MaxValueCacheSection;
		private ushort m_ChildPositionCache;
		private readonly SafeCriticalSection m_ChildPositionCacheSection;
		private string m_LeftLabelCache;
		private readonly SafeCriticalSection m_LeftLabelCacheSection;
		private string m_LeftChildLabelCache;
		private readonly SafeCriticalSection m_LeftChildLabelCacheSection;
		private string m_CenterLabelCache;
		private readonly SafeCriticalSection m_CenterLabelCacheSection;
		private string m_CenterChildLabelCache;
		private readonly SafeCriticalSection m_CenterChildLabelCacheSection;
		private string m_RightLabelCache;
		private readonly SafeCriticalSection m_RightLabelCacheSection;
		private string m_RightChildLabelCache;
		private readonly SafeCriticalSection m_RightChildLabelCacheSection;


		public ushort MinValueAnalogJoin { get; set; }
		public ushort MaxValueAnalogJoin { get; set; }
		public ushort ChildPositionAnalogJoin { get; set; }
		public ushort LeftLabelSerialJoin { get; set; }
		public ushort LeftChildLabelSerialJoin { get; set; }
		public ushort CenterLabelSerialJoin { get; set; }
		public ushort CenterChildLabelSerialJoin { get; set; }
		public ushort RightLabelSerialJoin { get; set; }
		public ushort RightChildLabelSerialJoin { get; set; }


		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		public VtProCircularGauge(ISigInputOutput panel) : base(panel)
		{
			m_MinValueCacheSection = new SafeCriticalSection();
			m_MaxValueCacheSection = new SafeCriticalSection();
			m_ChildPositionCacheSection = new SafeCriticalSection();
			m_LeftLabelCacheSection = new SafeCriticalSection();
			m_LeftChildLabelCacheSection = new SafeCriticalSection();
			m_CenterLabelCacheSection = new SafeCriticalSection();
			m_CenterChildLabelCacheSection = new SafeCriticalSection();
			m_RightLabelCacheSection = new SafeCriticalSection();
			m_RightChildLabelCacheSection = new SafeCriticalSection();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		public VtProCircularGauge(ISigInputOutput panel, IVtProParent parent) : base(panel, parent)
		{
			m_MinValueCacheSection = new SafeCriticalSection();
			m_MaxValueCacheSection = new SafeCriticalSection();
			m_ChildPositionCacheSection = new SafeCriticalSection();
			m_LeftLabelCacheSection = new SafeCriticalSection();
			m_LeftChildLabelCacheSection = new SafeCriticalSection();
			m_CenterLabelCacheSection = new SafeCriticalSection();
			m_CenterChildLabelCacheSection = new SafeCriticalSection();
			m_RightLabelCacheSection = new SafeCriticalSection();
			m_RightChildLabelCacheSection = new SafeCriticalSection();
		}

		#region Methods

		public void SetMinValue(ushort value)
		{
			m_MinValueCacheSection.Enter();
			try
			{
				if (MinValueAnalogJoin == 0)
					throw new InvalidOperationException("Unable to set value, join is 0");

				if (value == m_MinValueCache)
					return;

				ushort join = GetAnalogJoinWithParentOffset(MinValueAnalogJoin);

				m_MinValueCache = value;
				Panel.SendInputAnalog(join, value);
			}
			finally
			{
				m_MinValueCacheSection.Leave();
			}
		}

		public void SetMaxValue(ushort value)
		{
			m_MaxValueCacheSection.Enter();
			try
			{
				if (MaxValueAnalogJoin == 0)
					throw new InvalidOperationException("Unable to set value, join is 0");

				if (value == m_MaxValueCache)
					return;

				ushort join = GetAnalogJoinWithParentOffset(MaxValueAnalogJoin);

				m_MaxValueCache = value;
				Panel.SendInputAnalog(join, value);
			}
			finally
			{
				m_MaxValueCacheSection.Leave();
			}
		}

		public void SetChildPosition(ushort value)
		{
			m_ChildPositionCacheSection.Enter();
			try
			{
				if (ChildPositionAnalogJoin == 0)
					throw new InvalidOperationException("Unable to set value, join is 0");

				if (value == m_ChildPositionCache)
					return;

				ushort join = GetAnalogJoinWithParentOffset(ChildPositionAnalogJoin);

				m_ChildPositionCache = value;
				Panel.SendInputAnalog(join, value);
			}
			finally
			{
				m_ChildPositionCacheSection.Leave();
			}
		}

		public void SetLeftLabel(string value)
		{
			m_LeftLabelCacheSection.Enter();
			try
			{
				if (LeftLabelSerialJoin == 0)
					throw new InvalidOperationException("Unable to set value, join is 0");

				if (value == m_LeftLabelCache)
					return;

				ushort join = GetSerialJoinWithParentOffset(LeftLabelSerialJoin);

				m_LeftLabelCache = value;
				Panel.SendInputSerial(join, value);
			}
			finally
			{
				m_LeftLabelCacheSection.Leave();
			}
		}

		public void SetLeftChildLabel(string value)
		{
			m_LeftChildLabelCacheSection.Enter();
			try
			{
				if (LeftChildLabelSerialJoin == 0)
					throw new InvalidOperationException("Unable to set value, join is 0");

				if (value == m_LeftChildLabelCache)
					return;

				ushort join = GetSerialJoinWithParentOffset(LeftChildLabelSerialJoin);

				m_LeftChildLabelCache = value;
				Panel.SendInputSerial(join, value);
			}
			finally
			{
				m_LeftChildLabelCacheSection.Leave();
			}
		}

		public void SetCenterLabel(string value)
		{
			m_CenterLabelCacheSection.Enter();
			try
			{
				if (CenterLabelSerialJoin == 0)
					throw new InvalidOperationException("Unable to set value, join is 0");

				if (value == m_CenterLabelCache)
					return;

				ushort join = GetSerialJoinWithParentOffset(CenterLabelSerialJoin);

				m_CenterLabelCache = value;
				Panel.SendInputSerial(join, value);
			}
			finally
			{
				m_CenterLabelCacheSection.Leave();
			}
		}

		public void SetCenterChildLabel(string value)
		{
			m_CenterChildLabelCacheSection.Enter();
			try
			{
				if (CenterChildLabelSerialJoin == 0)
					throw new InvalidOperationException("Unable to set value, join is 0");

				if (value == m_CenterChildLabelCache)
					return;

				ushort join = GetSerialJoinWithParentOffset(CenterChildLabelSerialJoin);

				m_CenterChildLabelCache = value;
				Panel.SendInputSerial(join, value);
			}
			finally
			{
				m_CenterChildLabelCacheSection.Leave();
			}
		}

		public void SetRightLabel(string value)
		{
			m_RightLabelCacheSection.Enter();
			try
			{
				if (RightLabelSerialJoin == 0)
					throw new InvalidOperationException("Unable to set value, join is 0");

				if (value == m_RightLabelCache)
					return;

				ushort join = GetSerialJoinWithParentOffset(RightLabelSerialJoin);

				m_RightLabelCache = value;
				Panel.SendInputSerial(join, value);
			}
			finally
			{
				m_RightLabelCacheSection.Leave();
			}
		}

		public void SetRightChildLabel(string value)
		{
			m_RightChildLabelCacheSection.Enter();
			try
			{
				if (RightChildLabelSerialJoin == 0)
					throw new InvalidOperationException("Unable to set value, join is 0");

				if (value == m_RightChildLabelCache)
					return;

				ushort join = GetSerialJoinWithParentOffset(RightChildLabelSerialJoin);

				m_RightChildLabelCache = value;
				Panel.SendInputSerial(join, value);
			}
			finally
			{
				m_RightChildLabelCacheSection.Leave();
			}
		}



		#endregion

	}
}
