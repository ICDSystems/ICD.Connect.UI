using System;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Connect.Panels;

namespace ICD.Connect.UI.Controls.Buttons
{
	public abstract class AbstractVtProAdvancedButton : AbstractVtProButton
	{
		private readonly SafeCriticalSection m_ModeSection;

		private ushort m_ModeCache;

		[PublicAPI]
		public ushort AnalogModeJoin { get; set; }

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		protected AbstractVtProAdvancedButton(ISigInputOutput panel)
			: this(panel, null)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		protected AbstractVtProAdvancedButton(ISigInputOutput panel, IVtProParent parent)
			: base(panel, parent)
		{
			m_ModeSection = new SafeCriticalSection();
		}

		#endregion

		/// <summary>
		/// Sets the button mode. Throws InvalidOperationException if there is no mode join.
		/// </summary>
		/// <param name="mode"></param>
		[PublicAPI]
		public void SetMode(ushort mode)
		{
			m_ModeSection.Enter();

			try
			{
				if (AnalogModeJoin == 0)
					throw new InvalidOperationException("Unable to set mode, join is 0");

				if (mode == m_ModeCache)
					return;

				ushort join = GetAnalogJoinWithParentOffset(AnalogModeJoin);

				m_ModeCache = mode;
				Panel.SendInputAnalog(join, m_ModeCache);
			}
			finally
			{
				m_ModeSection.Leave();
			}
		}
	}
}
