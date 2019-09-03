namespace ICD.Connect.UI.Controls.Lists
{
	public struct ButtonListItem
	{
		private readonly string m_Label;
		private readonly string m_Icon;

		/// <summary>
		/// Gets the label text.
		/// </summary>
		public string Label { get { return m_Label; } }

		/// <summary>
		/// Gets the icon name.
		/// </summary>
		public string Icon { get { return m_Icon; } }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="label"></param>
		/// <param name="icon"></param>
		public ButtonListItem(string label, string icon)
		{
			m_Label = label;
			m_Icon = icon;
		}
	}
}
