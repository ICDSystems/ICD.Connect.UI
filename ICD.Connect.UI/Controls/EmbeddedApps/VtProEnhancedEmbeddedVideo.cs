using System;
using System.Collections.Generic;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.EventArguments;
using ICD.Common.Utils.Extensions;
using ICD.Connect.Panels.Devices;
using ICD.Connect.Panels.EventArguments;
using ICD.Connect.Panels.SmartObjects;
using ICD.Connect.Protocol.Sigs;

namespace ICD.Connect.UI.Controls.EmbeddedApps
{
	/// <summary>
	/// Control of the VTPro Enhanced Embedded Video object via Smart Object Id
	/// </summary>
	/// <remarks>
	/// Note: On/Off control should be enabled on the VTPro side, and utilized here
	/// to work around Crestron issues when switching soruces
	/// </remarks>
	public sealed class VtProEnhancedEmbeddedVideo : AbstractVtProSmartControl
	{
		#region Constants

		private const int MAX_SOURCES_LIMIT = 100;

		private const uint JOIN_DIGITAL_INPUT_ON_OFF = 1;
		private const uint JOIN_DIGITAL_OUTPUT_PRESS = 2;
		private const uint JOIN_DIGITAL_OUTPUT_VIDEO_PLAYING = 3;
		private const uint JOIN_DIGITAL_OUTPUT_SNAPSHOT_SHOWING = 4;
		private const uint JOIN_ANALOG_INPUT_SOURCE = 1;
		private const uint JOIN_ANALOG_INPUT_VIDEO_SOURCE_TYPE_START = 50;
		private const uint JOIN_ANALOG_INPUT_VIDEO_SNAPSHOT_REFRESH_TIME_START = 150;
		private const uint JOIN_SERIAL_INPUT_VIDEO_URL_START = 50;

		private const uint JOIN_SERIAL_INPUT_VIDEO_SNAPSHOT_URL_START = 150;

		#endregion

		#region Events

		/// <summary>
		/// Raised when the user presses/releases the video window
		/// </summary>
		[PublicAPI]
		public event EventHandler<BoolEventArgs> OnPressedChanged;

		/// <summary>
		/// Raised when video starts/stops playing
		/// </summary>
		[PublicAPI]
		public event EventHandler<BoolEventArgs> OnVideoPlayingChanged;

		/// <summary>
		/// Raised when the snapshot shows/hides
		/// </summary>
		[PublicAPI]
		public event EventHandler<BoolEventArgs> OnSnapshotShowingChanged; 

		#endregion

		#region Fields

		private int m_MaxSources;

		private bool m_IsPressed;

		private bool m_IsVideoPlaying;

		private bool m_IsSnapshotShowing;

		#region Cache

		private bool m_OnOffCache;
		private ushort m_SourceCache;
		private readonly Dictionary<ushort, ushort> m_VideoSourceTypeCache;
		private readonly Dictionary<ushort, ushort> m_VideoSnapshotRefreshTimeCache;
		private readonly Dictionary<ushort, string> m_VideoUrlCache;
		private readonly Dictionary<ushort, string> m_VideoSnapshotUrlCache;

		private readonly SafeCriticalSection m_VideoSourceTypeCacheSection;
		private readonly SafeCriticalSection m_VideoSnapshotRefreshTimeCacheSection;
		private readonly SafeCriticalSection m_VideoUrlCacheSection;
		private readonly SafeCriticalSection m_VideoSnapshotUrlCacheSection;

		#endregion

		#endregion

		#region Properties

		/// <summary>
		/// Maximum number of sources the control supports
		/// </summary>
		[PublicAPI]
		public int MaxSources
		{
			get { return m_MaxSources; }
			set { m_MaxSources = MathUtils.Clamp(value, 0, MAX_SOURCES_LIMIT); }
		}

		/// <summary>
		/// If the video window is pressed or not
		/// </summary>
		[PublicAPI]
		public bool IsPressed
		{
			get { return m_IsPressed; }
			private set
			{
				if (value == m_IsPressed)
					return;

				m_IsPressed = value;

				OnPressedChanged.Raise(this, new BoolEventArgs(value));
			}
		}

		/// <summary>
		/// If the video is playing or now
		/// </summary>
		[PublicAPI]
		public bool IsVideoPlaying
		{
			get { return m_IsVideoPlaying; }
			private set
			{
				if (value == m_IsVideoPlaying)
					return;

				m_IsVideoPlaying = value;

				OnVideoPlayingChanged.Raise(this, new BoolEventArgs(value));
			}
		}

		/// <summary>
		/// If the snapshot is showing or not
		/// </summary>
		[PublicAPI]
		public bool IsSnapshotShowing
		{
			get { return m_IsSnapshotShowing; }
			private set
			{
				if (value == m_IsSnapshotShowing)
					return;

				m_IsSnapshotShowing = value;

				OnSnapshotShowingChanged.Raise(this, new BoolEventArgs(value));
			}
		}

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="smartObjectId"></param>
		/// <param name="panel"></param>
		public VtProEnhancedEmbeddedVideo(uint smartObjectId, IPanelDevice panel) : this(smartObjectId, panel, null)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="smartObjectId"></param>
		/// <param name="panel"></param>
		/// <param name="parent"></param>
		public VtProEnhancedEmbeddedVideo(uint smartObjectId, IPanelDevice panel, IVtProParent parent) : base(smartObjectId, panel, parent)
		{
			m_VideoSourceTypeCache = new Dictionary<ushort, ushort>();
			m_VideoSnapshotRefreshTimeCache = new Dictionary<ushort, ushort>();
			m_VideoUrlCache = new Dictionary<ushort, string>();
			m_VideoSnapshotUrlCache = new Dictionary<ushort, string>();

			m_VideoSourceTypeCacheSection = new SafeCriticalSection();
			m_VideoSnapshotRefreshTimeCacheSection = new SafeCriticalSection();
			m_VideoUrlCacheSection = new SafeCriticalSection();
			m_VideoSnapshotUrlCacheSection = new SafeCriticalSection();
		}

		#region Methods

		/// <summary>
		/// Sets the On/Off state of the control
		/// </summary>
		/// <param name="state"></param>
		[PublicAPI]
		public void SetOnOff(bool state)
		{
			if (m_OnOffCache == state)
				return;

			SmartObject.SendInputDigital(JOIN_DIGITAL_INPUT_ON_OFF, state);

			m_OnOffCache = state;
		}

		/// <summary>
		/// Sets the source for the control
		/// </summary>
		/// <param name="source"></param>
		[PublicAPI]
		public void SetSource(ushort source)
		{
			if (m_SourceCache == source)
				return;

			SmartObject.SendInputAnalog(JOIN_ANALOG_INPUT_SOURCE, source);

			m_SourceCache = source;

			// If video is currently playing, send on/off low momentarily
			// This is a workaround for the source sometimes not switching
			if (!m_OnOffCache)
				return;
			SmartObject.SendInputDigital(JOIN_DIGITAL_INPUT_ON_OFF, false);
			SmartObject.SendInputDigital(JOIN_DIGITAL_INPUT_ON_OFF, true);
		}

		/// <summary>
		/// Sets the source type for the source at the specified index
		/// </summary>
		/// <param name="index"></param>
		/// <param name="sourceType"></param>
		[PublicAPI]
		public void SetVideoSourceType(ushort index, ushort sourceType)
		{
			if (!IsIndexInRange(index))
				throw new IndexOutOfRangeException("index");

			m_VideoSourceTypeCacheSection.Enter();
			try
			{
				ushort currentValue;
				if (m_VideoSourceTypeCache.TryGetValue(index, out currentValue) && currentValue == sourceType)
					return;

				uint join = JOIN_ANALOG_INPUT_VIDEO_SOURCE_TYPE_START + index;
				SmartObject.SendInputAnalog(join, sourceType);

				m_VideoSourceTypeCache[index] = sourceType;
			}
			finally
			{
				m_VideoSourceTypeCacheSection.Leave();
			}
		}

		/// <summary>
		/// Sets the snapshot refresh interval for the source at the specified index
		/// Snapshot time is in seconds, 0 indicates no snapshot refresh
		/// </summary>
		/// <param name="index"></param>
		/// <param name="snapshotRefreshTime"></param>
		[PublicAPI]
		public void SetVideoSnapshotRefreshTime(ushort index, ushort snapshotRefreshTime)
		{
			if (!IsIndexInRange(index))
				throw new IndexOutOfRangeException("index");

			m_VideoSnapshotRefreshTimeCacheSection.Enter();
			try
			{
				ushort currentValue;
				if (m_VideoSnapshotRefreshTimeCache.TryGetValue(index, out currentValue) && currentValue == snapshotRefreshTime)
					return;

				uint join = JOIN_ANALOG_INPUT_VIDEO_SNAPSHOT_REFRESH_TIME_START + index;
				SmartObject.SendInputAnalog(join, snapshotRefreshTime);

				m_VideoSnapshotRefreshTimeCache[index] = snapshotRefreshTime;
			}
			finally
			{
				m_VideoSnapshotRefreshTimeCacheSection.Leave();
			}
		}

		/// <summary>
		/// Sets the streaming URL for the source at the specified index
		/// </summary>
		/// <param name="index"></param>
		/// <param name="url"></param>
		[PublicAPI]
		public void SetVideoUrl(ushort index, [CanBeNull] string url)
		{
			if (!IsIndexInRange(index))
				throw new IndexOutOfRangeException("index");

			url = url ?? String.Empty;

			m_VideoUrlCacheSection.Enter();
			try
			{
				string currentValue;
				if (m_VideoUrlCache.TryGetValue(index, out currentValue) && currentValue.Equals(url, StringComparison.Ordinal))
					return;

				uint join = JOIN_SERIAL_INPUT_VIDEO_URL_START + index;
				SmartObject.SendInputSerial(join, url);

				m_VideoUrlCache[index] = url;
			}
			finally
			{
				m_VideoUrlCacheSection.Leave();
			}
		}

		/// <summary>
		/// Sets the snapshot URL for the source at the specified index
		/// </summary>
		/// <param name="index"></param>
		/// <param name="url"></param>
		[PublicAPI]
		public void SetVideoSnapshotUrl(ushort index, [CanBeNull] string url)
		{
			if (!IsIndexInRange(index))
				throw new IndexOutOfRangeException("index");

			url = url ?? String.Empty;

			m_VideoSnapshotUrlCacheSection.Enter();
			try
			{
				string currentValue;
				if (m_VideoSnapshotUrlCache.TryGetValue(index, out currentValue) && currentValue.Equals(url, StringComparison.Ordinal))
					return;

				uint join = JOIN_SERIAL_INPUT_VIDEO_SNAPSHOT_URL_START + index;
				SmartObject.SendInputSerial(join, url);

				m_VideoSnapshotUrlCache[index] = url;
			}
			finally
			{
				m_VideoSnapshotUrlCacheSection.Leave();
			}
		}

		private bool IsIndexInRange(ushort index)
		{
			return index < MaxSources;
		}

		#endregion

		#region SO Callbacks

		/// <summary>
		/// Subscribe to the smart object events.
		/// </summary>
		/// <param name="smartObject"></param>
		protected override void Subscribe(ISmartObject smartObject)
		{
			base.Subscribe(smartObject);

			if (smartObject == null)
				return;

			smartObject.RegisterOutputSigChangeCallback(JOIN_DIGITAL_OUTPUT_PRESS, eSigType.Digital, PressedCallback);
			smartObject.RegisterOutputSigChangeCallback(JOIN_DIGITAL_OUTPUT_VIDEO_PLAYING, eSigType.Digital, VideoPlayingCallback);
			smartObject.RegisterOutputSigChangeCallback(JOIN_DIGITAL_OUTPUT_SNAPSHOT_SHOWING, eSigType.Digital, SnapshotShowingCallback);
		}

		/// <summary>
		/// Unsubscribe from the smart object events.
		/// </summary>
		/// <param name="smartObject"></param>
		protected override void Unsubscribe(ISmartObject smartObject)
		{
			base.Unsubscribe(smartObject);


			if (smartObject == null)
				return;

			smartObject.UnregisterOutputSigChangeCallback(JOIN_DIGITAL_OUTPUT_PRESS, eSigType.Digital, PressedCallback);
			smartObject.UnregisterOutputSigChangeCallback(JOIN_DIGITAL_OUTPUT_VIDEO_PLAYING, eSigType.Digital, VideoPlayingCallback);
			smartObject.UnregisterOutputSigChangeCallback(JOIN_DIGITAL_OUTPUT_SNAPSHOT_SHOWING, eSigType.Digital, SnapshotShowingCallback);
		}

		private void PressedCallback(object parent, [NotNull] SigInfoEventArgs args)
		{
			IsPressed = args.Data.GetBoolValue();
		}

		private void VideoPlayingCallback(object parent, [NotNull] SigInfoEventArgs args)
		{
			IsVideoPlaying = args.Data.GetBoolValue();
		}

		private void SnapshotShowingCallback(object parent, [NotNull] SigInfoEventArgs args)
		{
			IsSnapshotShowing = args.Data.GetBoolValue();
		}

		#endregion
	}
}