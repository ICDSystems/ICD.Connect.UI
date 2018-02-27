using ICD.Common.Utils.EventArguments;
using ICD.Connect.UI.Utils;

namespace ICD.Connect.UI.EventArguments
{
	public sealed class KeyboardKeyEventArgs : GenericEventArgs<KeyboardKey>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="data"></param>
		public KeyboardKeyEventArgs(KeyboardKey data)
			: base(data)
		{
		}
	}
}
