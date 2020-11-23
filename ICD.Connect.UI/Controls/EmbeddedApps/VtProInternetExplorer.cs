using System;
using ICD.Common.Properties;
using ICD.Connect.Panels;

namespace ICD.Connect.UI.Controls.EmbeddedApps
{
	public sealed class VtProInternetExplorer : AbstractVtProControl<ISigInputOutput>
	{
		/// <summary>
		/// Serial join to set the URL link.
		/// </summary>
		[PublicAPI]
		public ushort SerialFilePathJoin { get; set; }

		/// <summary>
		/// Digital join to open the file dialog window when the incoming signal is high.
		/// </summary>
		[PublicAPI]
		public ushort DigitalOpenFileDialogJoin { get; set; }

		/// <summary>
		/// Digital join to resize the application to the designated
		/// display mode when the incoming signal is high.
		/// </summary>
		[PublicAPI]
		public ushort DigitalDisplayModeJoin { get; set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public VtProInternetExplorer(ISigInputOutput panel) : this(panel, null)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		public VtProInternetExplorer(ISigInputOutput panel, IVtProParent parent) : this(panel, parent,0)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		/// <param name="index"></param>
		public VtProInternetExplorer(ISigInputOutput panel, IVtProParent parent, ushort index) : base(panel, parent, index)
		{
		}

		[PublicAPI]
		public void SetFilePath(string filePath)
		{
			if (SerialFilePathJoin == 0)
				throw new InvalidOperationException("Unable to set file path, join is 0");

			Panel.SendInputSerial(SerialFilePathJoin, filePath);
		}

		[PublicAPI]
		public void OpenFileDialog(bool open)
		{
			if (DigitalOpenFileDialogJoin == 0)
				throw new InvalidOperationException("Unable to open file dialog, join is 0");

			Panel.SendInputDigital(DigitalOpenFileDialogJoin, open);
		}

		[PublicAPI]
		public void SetDisplayMode(bool displayMode)
		{
			if (DigitalDisplayModeJoin == 0)
				throw new InvalidOperationException("Unable to set display mode, join is 0");

			Panel.SendInputDigital(DigitalDisplayModeJoin, displayMode);
		}
	}
}