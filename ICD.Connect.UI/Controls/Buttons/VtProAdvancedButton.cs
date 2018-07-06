using ICD.Connect.Panels;

namespace ICD.Connect.UI.Controls.Buttons
{
	public sealed class VtProAdvancedButton : AbstractVtProAdvancedButton
	{
		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		public VtProAdvancedButton(ISigInputOutput panel)
			: base(panel)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		public VtProAdvancedButton(ISigInputOutput panel, IVtProParent parent)
			: base(panel, parent)
		{
		}

		#endregion
	}
}
