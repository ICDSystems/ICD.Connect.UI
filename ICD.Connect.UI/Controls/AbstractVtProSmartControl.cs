using System;
using ICD.Connect.Panels.Devices;
using ICD.Connect.Panels.SmartObjects;

namespace ICD.Connect.UI.Controls
{
	public abstract class AbstractVtProSmartControl : AbstractVtProControl<IPanelDevice>
	{
		/// <summary>
		/// Gets the smart object.
		/// </summary>
		public ISmartObject SmartObject { get; private set; }

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="smartObjectId"></param>
		/// <param name="panel"></param>
		protected AbstractVtProSmartControl(uint smartObjectId, IPanelDevice panel)
			: this(smartObjectId, panel, null)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="smartObjectId"></param>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		protected AbstractVtProSmartControl(uint smartObjectId, IPanelDevice panel, IVtProParent parent)
			: base(panel, parent)
		{
			if (panel == null)
				throw new ArgumentNullException("panel");

			if (smartObjectId == 0)
				throw new ArgumentException("SmartObject Id 0 is invalid", "smartObjectId");

			SmartObject = panel.SmartObjects[smartObjectId];
			Subscribe(SmartObject);
		}

		#endregion

		/// <summary>
		/// Release resources.
		/// </summary>
		public override void Dispose()
		{
			base.Dispose();
			Unsubscribe(SmartObject);
		}

		/// <summary>
		/// Subscribe to the smart object events.
		/// </summary>
		/// <param name="smartObject"></param>
		protected virtual void Subscribe(ISmartObject smartObject)
		{
		}

		/// <summary>
		/// Unsubscribe from the smart object events.
		/// </summary>
		/// <param name="smartObject"></param>
		protected virtual void Unsubscribe(ISmartObject smartObject)
		{
		}
	}
}
