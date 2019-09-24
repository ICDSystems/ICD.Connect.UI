using ICD.Common.Properties;
using ICD.Connect.Panels;

namespace ICD.Connect.UI.Controls.Guages
{
	public sealed class VtProGauge : AbstractVtProGauge
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		[PublicAPI]
		public VtProGauge(ISigInputOutput panel)
			: base(panel)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		public VtProGauge(ISigInputOutput panel, IVtProParent parent)
			: base(panel, parent)
		{
		}
	}
}
