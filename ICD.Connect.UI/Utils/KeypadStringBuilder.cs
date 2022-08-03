using System;
using ICD.Common.Properties;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;

namespace ICD.Connect.UI.Utils
{
	/// <summary>
	/// Convenience object for building strings one button-press at a time.
	/// </summary>
	public sealed class KeypadStringBuilder
	{
		/// <summary>
		/// Raised when the string value changes.
		/// </summary>
		[PublicAPI]
		public event EventHandler<StringEventArgs> OnStringChanged;

		[NotNull]
		private string m_Output;

		/// <summary>
		/// Constructor.
		/// </summary>
		public KeypadStringBuilder()
		{
			m_Output = string.Empty;
		}

		#region Methods

		/// <summary>
		/// Sets the current string in the builder.
		/// </summary>
		/// <param name="value"></param>
		[PublicAPI]
		public void SetString(string value)
		{
			SetString(value, false);
		}

		/// <summary>
		/// Sets the current string in the builder.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="suppressEvent">If true, OnStringChanges event will be suppressed - Use with care</param>
		[PublicAPI]
		public void SetString(string value, bool suppressEvent)
		{
			value = value ?? string.Empty;
			if (value == m_Output)
				return;

			m_Output = value;

			if(!suppressEvent)
				OnStringChanged.Raise(this, new StringEventArgs(m_Output));
		}

		/// <summary>
		/// Appends the given character to the string.
		/// </summary>
		/// <param name="character"></param>
		[PublicAPI]
		public void AppendCharacter(char character)
		{
			SetString(m_Output + character);
		}

		/// <summary>
		/// Removes the last character in the string.
		/// </summary>
		[PublicAPI]
		public void Backspace()
		{
			if (!string.IsNullOrEmpty(m_Output))
				SetString(m_Output.Substring(0, m_Output.Length - 1));
		}

		/// <summary>
		/// Clears the string.
		/// </summary>
		[PublicAPI]
		public void Clear()
		{
			Clear(false);
		}

		/// <summary>
		/// Clears the string
		/// </summary>
		/// <param name="suppressEvent">If true, OnStringChanges event will be suppressed - Use with care</param>
		[PublicAPI]
		public void Clear(bool suppressEvent)
		{
			SetString(string.Empty, suppressEvent);
		}

		/// <summary>
		/// Clears the builder and returns the contents.
		/// </summary>
		/// <returns></returns>
		[PublicAPI]
		[NotNull]
		public string Pop()
		{
			string output = ToString();
			Clear();
			return output;
		}

		/// <summary>
		/// Gets the resulting string.
		/// </summary>
		/// <returns></returns>
		[PublicAPI]
		[NotNull]
		public override string ToString()
		{
			return m_Output;
		}

		#endregion
	}
}
