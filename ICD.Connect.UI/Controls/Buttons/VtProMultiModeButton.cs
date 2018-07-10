using ICD.Connect.Panels;

namespace ICD.Connect.UI.Controls.Buttons
{
	public sealed class VtProMultiModeButton : AbstractVtProAdvancedButton
	{
		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		public VtProMultiModeButton(ISigInputOutput panel)
			: base(panel)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		public VtProMultiModeButton(ISigInputOutput panel, IVtProParent parent)
			: base(panel, parent)
		{
		}

		#endregion
	}
}
