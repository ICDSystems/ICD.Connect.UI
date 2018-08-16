using System;

namespace ICD.Connect.UI.Utils
{
	/// <summary>
	/// HTML formatting utils for .
	/// </summary>
	public static class HtmlUtils
	{
		/// <summary>
		/// Newline HTML for labels.
		/// </summary>
		public const string LINE_BREAK = "<br />";

		/// <summary>
		/// Wraps the text with a font element with a color attribute.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		public static string FormatColoredText(string text, string color)
		{
			if (string.IsNullOrEmpty("color"))
				throw new ArgumentException("Color is null or empty", "color");

			return string.IsNullOrEmpty(text) ? text : string.Format("<font color=\"{0}\">{1}</font>", color, text);
		}
	}
}
