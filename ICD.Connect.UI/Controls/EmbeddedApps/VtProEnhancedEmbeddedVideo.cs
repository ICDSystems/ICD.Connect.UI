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

		/// <summary>
		/// Start Joins decemented by 1, due to 1-based source indexing
		/// </summary>
		private const uint JOIN_ANALOG_INPUT_VIDEO_SOURCE_TYPE_START = 50 - 1;
		private const uint JOIN_ANALOG_INPUT_VIDEO_SNAPSHOT_REFRESH_TIME_START = 150 - 1;
		private const uint JOIN_SERIAL_INPUT_VIDEO_URL_START = 50 - 1;

		private const uint JOIN_SERIAL_INPUT_VIDEO_SNAPSHOT_URL_START = 150 - 1;

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
			IcdConsole.PrintLine(eConsoleColor.Magenta, "EEV: Setting OnOff State: {0}", state);
		}

		/// <summary>
		/// Sets the source for the control
		/// Sources are 1-based indexed
		/// </summary>
		/// <param name="source"></param>
		[PublicAPI]
		public void SetSource(ushort source)
		{
			if (m_SourceCache == source)
				return;

			// If video is currently playing, send on/off low momentarily
			// This is a workaround for the source sometimes not switching
			SmartObject.SendInputDigital(JOIN_DIGITAL_INPUT_ON_OFF, false);

			SmartObject.SendInputAnalog(JOIN_ANALOG_INPUT_SOURCE, source);

			m_SourceCache = source;

			IcdConsole.PrintLine(eConsoleColor.Magenta, "EEV: Setting Source to {0}", source);

			// If video is currently playing, send on/off low momentarily
			// This is a workaround for the source sometimes not switching
			if (!m_OnOffCache)
				return;

			SmartObject.SendInputDigital(JOIN_DIGITAL_INPUT_ON_OFF, true);

			IcdConsole.PrintLine(eConsoleColor.Magenta, "EEV: Toggling On/Off State after Source Select");
		}

		/// <summary>
		/// Sets the source type for the source at the specified index
		/// Sources are 1-based indexed
		/// </summary>
		/// <param name="source"></param>
		/// <param name="sourceType"></param>
		[PublicAPI]
		public void SetVideoSourceType(ushort source, ushort sourceType)
		{
			if (!IsSourceInRange(source))
				throw new IndexOutOfRangeException("index");

			m_VideoSourceTypeCacheSection.Enter();
			try
			{
				ushort currentValue;
				if (m_VideoSourceTypeCache.TryGetValue(source, out currentValue) && currentValue == sourceType)
					return;

				uint join = JOIN_ANALOG_INPUT_VIDEO_SOURCE_TYPE_START + source;
				SmartObject.SendInputAnalog(join, sourceType);

				m_VideoSourceTypeCache[source] = sourceType;
			}
			finally
			{
				m_VideoSourceTypeCacheSection.Leave();
			}

			IcdConsole.PrintLine(eConsoleColor.Magenta, "EEV: Setting Source {0} type to {1}", source, sourceType);
		}

		/// <summary>
		/// Sets the snapshot refresh interval for the source at the specified index
		/// Snapshot time is in seconds, 0 indicates no snapshot refresh
		/// Sources are 1-based indexed
		/// </summary>
		/// <param name="source"></param>
		/// <param name="snapshotRefreshTime"></param>
		[PublicAPI]
		public void SetVideoSnapshotRefreshTime(ushort source, ushort snapshotRefreshTime)
		{
			if (!IsSourceInRange(source))
				throw new IndexOutOfRangeException("index");

			m_VideoSnapshotRefreshTimeCacheSection.Enter();
			try
			{
				ushort currentValue;
				if (m_VideoSnapshotRefreshTimeCache.TryGetValue(source, out currentValue) && currentValue == snapshotRefreshTime)
					return;

				uint join = JOIN_ANALOG_INPUT_VIDEO_SNAPSHOT_REFRESH_TIME_START + source;
				SmartObject.SendInputAnalog(join, snapshotRefreshTime);

				m_VideoSnapshotRefreshTimeCache[source] = snapshotRefreshTime;
			}
			finally
			{
				m_VideoSnapshotRefreshTimeCacheSection.Leave();
			}
		}

		/// <summary>
		/// Sets the streaming URL for the source at the specified index
		/// Sources are 1-based indexed
		/// </summary>
		/// <param name="source"></param>
		/// <param name="url"></param>
		[PublicAPI]
		public void SetVideoUrl(ushort source, [CanBeNull] string url)
		{
			if (!IsSourceInRange(source))
				throw new IndexOutOfRangeException("index");

			url = url ?? String.Empty;

			m_VideoUrlCacheSection.Enter();
			try
			{
				string currentValue;
				if (m_VideoUrlCache.TryGetValue(source, out currentValue) && currentValue.Equals(url, StringComparison.Ordinal))
					return;

				bool turnBackOn = false;
				// If video is currently playing, send on/off low momentarily
				// This is a workaround for the source sometimes not switching
				if (m_OnOffCache && m_SourceCache == source)
				{
					SmartObject.SendInputDigital(JOIN_DIGITAL_INPUT_ON_OFF, false);
					turnBackOn = true;
				}

				uint join = JOIN_SERIAL_INPUT_VIDEO_URL_START + source;
				SmartObject.SendInputSerial(join, url);

				m_VideoUrlCache[source] = url;

				if (turnBackOn)
					SmartObject.SendInputDigital(JOIN_DIGITAL_INPUT_ON_OFF, true);
			}
			finally
			{
				m_VideoUrlCacheSection.Leave();
			}

			IcdConsole.PrintLine(eConsoleColor.Magenta, "EEV: Setting Source {0} url to {1}", source, url);
		}

		/// <summary>
		/// Sets the snapshot URL for the source at the specified index
		/// Sources are 1-based indexed
		/// </summary>
		/// <param name="source"></param>
		/// <param name="url"></param>
		[PublicAPI]
		public void SetVideoSnapshotUrl(ushort source, [CanBeNull] string url)
		{
			if (!IsSourceInRange(source))
				throw new IndexOutOfRangeException("index");

			url = url ?? String.Empty;

			m_VideoSnapshotUrlCacheSection.Enter();
			try
			{
				string currentValue;
				if (m_VideoSnapshotUrlCache.TryGetValue(source, out currentValue) && currentValue.Equals(url, StringComparison.Ordinal))
					return;

				uint join = JOIN_SERIAL_INPUT_VIDEO_SNAPSHOT_URL_START + source;
				SmartObject.SendInputSerial(join, url);

				m_VideoSnapshotUrlCache[source] = url;
			}
			finally
			{
				m_VideoSnapshotUrlCacheSection.Leave();
			}
		}

		/// <summary>
		/// Check if the source is in range of MaxSources
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		private bool IsSourceInRange(ushort source)
		{
			return source <= MaxSources;
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