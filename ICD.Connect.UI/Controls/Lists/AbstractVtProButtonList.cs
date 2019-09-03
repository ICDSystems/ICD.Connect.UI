using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Connect.Panels.Devices;

namespace ICD.Connect.UI.Controls.Lists
{
	/// <summary>
	/// Base class for button lists.
	/// </summary>
	public abstract class AbstractVtProButtonList : AbstractVtProList
	{
		private readonly SafeCriticalSection m_SetLabelsSection;
		private readonly SafeCriticalSection m_SetIconsSection;

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="smartObjectId"></param>
		/// <param name="panel"></param>
		protected AbstractVtProButtonList(uint smartObjectId, IPanelDevice panel)
			: this(smartObjectId, panel, null)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="smartObjectId"></param>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		protected AbstractVtProButtonList(uint smartObjectId, IPanelDevice panel, IVtProParent parent)
			: base(smartObjectId, panel, parent)
		{
			m_SetLabelsSection = new SafeCriticalSection();
			m_SetIconsSection = new SafeCriticalSection();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets the selected state for the button at the given index.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="selected"></param>
		[PublicAPI]
		public abstract void SetItemSelected(ushort index, bool selected);

		/// <summary>
		/// Sets the button labels.
		/// </summary>
		/// <param name="labels"></param>
		public void SetItemLabels(IList<string> labels)
		{
			if (labels == null)
				throw new ArgumentNullException("labels");

			m_SetLabelsSection.Enter();

			try
			{
				ushort min = (ushort)Math.Min(labels.Count, MaxSize);

				SetNumberOfItems(min);

				for (ushort index = 0; index < min; index++)
					SetItemLabel(index, labels[index]);
			}
			finally
			{
				m_SetLabelsSection.Leave();
			}
		}

		/// <summary>
		/// Sets the button label.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="text"></param>
		public abstract void SetItemLabel(ushort index, string text);

		/// <summary>
		/// Sets the button icons.
		/// </summary>
		/// <param name="icons"></param>
		[PublicAPI]
		public void SetItemIcons(IList<string> icons)
		{
			if (icons == null)
				throw new ArgumentNullException("icons");

			m_SetIconsSection.Enter();

			try
			{
				ushort min = (ushort)Math.Min(icons.Count, MaxSize);

				SetNumberOfItems(min);

				for (ushort index = 0; index < min; index++)
					SetItemIcon(index, icons[index]);
			}
			finally
			{
				m_SetIconsSection.Leave();
			}
		}

		/// <summary>
		/// Sets the button icon.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="icon"></param>
		public abstract void SetItemIcon(ushort index, string icon);

		/// <summary>
		/// Sets the button items.
		/// </summary>
		/// <param name="items"></param>
		public void SetItems(IList<ButtonListItem> items)
		{
			if (items == null)
				throw new ArgumentNullException("items");

			m_SetIconsSection.Enter();
			m_SetLabelsSection.Enter();

			try
			{
				ushort min = (ushort)Math.Min(items.Count, MaxSize);

				SetNumberOfItems(min);

				for (ushort index = 0; index < min; index++)
				{
					ButtonListItem item = items[index];

					SetItemLabel(index, item.Label);
					SetItemIcon(index, item.Icon);
				}
			}
			finally
			{
				m_SetIconsSection.Leave();
				m_SetLabelsSection.Leave();
			}
		}

		#endregion
	}
}
