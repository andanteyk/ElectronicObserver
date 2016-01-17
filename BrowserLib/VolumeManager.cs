using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BrowserLib {

	/// <summary>
	/// 音量やミュートを管理します。
	/// </summary>
	public class VolumeManager {

		public uint ProcessID { get; private set; }

		public VolumeManager( uint processID ) {
			ProcessID = processID;
		}


		/// <summary>
		/// 音量操作のためのデータを取得します。
		/// </summary>
		/// <param name="processID">対象のプロセスID。</param>
		/// <returns>データ。取得に失敗した場合は null。</returns>
		private static ISimpleAudioVolume GetVolumeObject( uint processID ) {

			ISimpleAudioVolume ret = null;

			// スピーカーデバイスの取得
			IMMDeviceEnumerator deviceEnumerator = (IMMDeviceEnumerator)( new MMDeviceEnumerator() );
			IMMDevice speakers;
			deviceEnumerator.GetDefaultAudioEndpoint( EDataFlow.eRender, ERole.eMultimedia, out speakers );

			// 列挙のためにセッションマネージャをアクティベート
			Guid IID_IAudioSessionManager2 = typeof( IAudioSessionManager2 ).GUID;
			object o;
			speakers.Activate( ref IID_IAudioSessionManager2, 0, IntPtr.Zero, out o );
			IAudioSessionManager2 mgr = (IAudioSessionManager2)o;

			// セッションの列挙
			IAudioSessionEnumerator sessionEnumerator;
			mgr.GetSessionEnumerator( out sessionEnumerator );
			int count;
			sessionEnumerator.GetCount( out count );

			for ( int i = 0; i < count; i++ ) {
				IAudioSessionControl ctl;
				IAudioSessionControl2 ctl2;

				sessionEnumerator.GetSession( i, out ctl );

				ctl2 = ctl as IAudioSessionControl2;

				uint pid = uint.MaxValue;

				if ( ctl2 != null ) {
					ctl2.GetProcessId( out pid );
				}

				if ( pid == processID ) {
					ret = ctl2 as ISimpleAudioVolume;
					break;
				}


				if ( ctl != null )
					Marshal.ReleaseComObject( ctl );

				if ( ctl2 != null )
					Marshal.ReleaseComObject( ctl2 );
			}

			Marshal.ReleaseComObject( sessionEnumerator );
			Marshal.ReleaseComObject( mgr );
			Marshal.ReleaseComObject( speakers );
			Marshal.ReleaseComObject( deviceEnumerator );


			return ret;
		}


		private const string ErrorMessageNotFound = "指定したプロセスIDの音量オブジェクトは存在しません。";

		/// <summary>
		/// 音量を取得します。
		/// </summary>
		/// <param name="processID">対象のプロセスID。</param>
		/// <returns>音量( 0.0 - 1.0 )。</returns>
		public static float GetApplicationVolume( uint processID ) {
			ISimpleAudioVolume volume = GetVolumeObject( processID );
			if ( volume == null )
				throw new ArgumentException( ErrorMessageNotFound );

			float level;
			volume.GetMasterVolume( out level );

			Marshal.ReleaseComObject( volume );
			return level;
		}

		/// <summary>
		/// ミュート状態を取得します。
		/// </summary>
		/// <param name="processID">対象のプロセスID。</param>
		/// <returns>ミュートされていれば true。</returns>
		public static bool GetApplicationMute( uint processID ) {
			ISimpleAudioVolume volume = GetVolumeObject( processID );
			if ( volume == null )
				throw new ArgumentException( ErrorMessageNotFound );

			bool mute;
			volume.GetMute( out mute );

			Marshal.ReleaseComObject( volume );
			return mute;
		}


		/// <summary>
		/// 音量を設定します。
		/// </summary>
		/// <param name="processID">対象のプロセスID。</param>
		/// <param name="level">音量( 0.0 - 1.0 )。</param>
		public static void SetApplicationVolume( uint processID, float level ) {
			ISimpleAudioVolume volume = GetVolumeObject( processID );
			if ( volume == null )
				throw new ArgumentException( ErrorMessageNotFound );

			Guid guid = Guid.Empty;
			volume.SetMasterVolume( level, ref guid );

			Marshal.ReleaseComObject( volume );
		}

		/// <summary>
		/// ミュート状態を設定します。
		/// </summary>
		/// <param name="processID">対象のプロセスID。</param>
		/// <param name="mute">ミュートするなら true。</param>
		public static void SetApplicationMute( uint processID, bool mute ) {
			ISimpleAudioVolume volume = GetVolumeObject( processID );
			if ( volume == null )
				throw new ArgumentException( ErrorMessageNotFound );

			Guid guid = Guid.Empty;
			volume.SetMute( mute, ref guid );

			Marshal.ReleaseComObject( volume );
		}

		/// <summary>
		/// ミュート状態をトグルします。
		/// </summary>
		/// <param name="processID">対象のプロセスID。</param>
		/// <returns>トグル後のミュート状態。</returns>
		public static bool ToggleMute( uint processID ) {

			bool mute = !GetApplicationMute( processID );
			SetApplicationMute( processID, mute );
			return mute;
		}



		//インスタンス用

		public float Volume {
			get { return GetApplicationVolume( ProcessID ); }
			set { SetApplicationVolume( ProcessID, value ); }
		}

		public bool IsMute {
			get { return GetApplicationMute( ProcessID ); }
			set { SetApplicationMute( ProcessID, value ); }
		}

		/// <summary>
		/// ミュート状態をトグルします。
		/// </summary>
		/// <returns>トグル後のミュート状態。</returns>
		public bool ToggleMute() {
			return ToggleMute( ProcessID );
		}



		#region 呪文

		[ComImport]
		[Guid( "BCDE0395-E52F-467C-8E3D-C4579291692E" )]
		internal class MMDeviceEnumerator {
		}

		internal enum EDataFlow {
			eRender,
			eCapture,
			eAll,
			EDataFlow_enum_count
		}

		internal enum ERole {
			eConsole,
			eMultimedia,
			eCommunications,
			ERole_enum_count
		}

		[Guid( "A95664D2-9614-4F35-A746-DE8DB63617E6" ), InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
		internal interface IMMDeviceEnumerator {
			int NotImpl1();

			[PreserveSig]
			int GetDefaultAudioEndpoint( EDataFlow dataFlow, ERole role, out IMMDevice ppDevice );

			// the rest is not implemented
		}

		[Guid( "D666063F-1587-4E43-81F1-B948E807363F" ), InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
		internal interface IMMDevice {
			[PreserveSig]
			int Activate( ref Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs( UnmanagedType.IUnknown )] out object ppInterface );

			// the rest is not implemented
		}

		[Guid( "77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F" ), InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
		internal interface IAudioSessionManager2 {
			int NotImpl1();
			int NotImpl2();

			[PreserveSig]
			int GetSessionEnumerator( out IAudioSessionEnumerator SessionEnum );

			// the rest is not implemented
		}

		[Guid( "E2F5BB11-0570-40CA-ACDD-3AA01277DEE8" ), InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
		internal interface IAudioSessionEnumerator {
			[PreserveSig]
			int GetCount( out int SessionCount );

			[PreserveSig]
			int GetSession( int SessionCount, out IAudioSessionControl Session );
		}

		[Guid( "F4B1A599-7266-4319-A8CA-E70ACB11E8CD" ), InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
		internal interface IAudioSessionControl {
			int NotImpl1();

			[PreserveSig]
			int GetDisplayName( [MarshalAs( UnmanagedType.LPWStr )] out string pRetVal );

			// the rest is not implemented
		}

		[Guid( "87CE5498-68D6-44E5-9215-6DA47EF883D8" ), InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
		internal interface ISimpleAudioVolume {
			[PreserveSig]
			int SetMasterVolume( float fLevel, ref Guid EventContext );

			[PreserveSig]
			int GetMasterVolume( out float pfLevel );

			[PreserveSig]
			int SetMute( bool bMute, ref Guid EventContext );

			[PreserveSig]
			int GetMute( out bool pbMute );
		}

		public enum AudioSessionState {
			AudioSessionStateInactive = 0,
			AudioSessionStateActive = 1,
			AudioSessionStateExpired = 2
		}

		public enum AudioSessionDisconnectReason {
			DisconnectReasonDeviceRemoval = 0,
			DisconnectReasonServerShutdown = ( DisconnectReasonDeviceRemoval + 1 ),
			DisconnectReasonFormatChanged = ( DisconnectReasonServerShutdown + 1 ),
			DisconnectReasonSessionLogoff = ( DisconnectReasonFormatChanged + 1 ),
			DisconnectReasonSessionDisconnected = ( DisconnectReasonSessionLogoff + 1 ),
			DisconnectReasonExclusiveModeOverride = ( DisconnectReasonSessionDisconnected + 1 )
		}

		[Guid( "24918ACC-64B3-37C1-8CA9-74A66E9957A8" ), InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
		public interface IAudioSessionEvents {
			[PreserveSig]
			int OnDisplayNameChanged( [MarshalAs( UnmanagedType.LPWStr )] string NewDisplayName, Guid EventContext );
			[PreserveSig]
			int OnIconPathChanged( [MarshalAs( UnmanagedType.LPWStr )] string NewIconPath, Guid EventContext );
			[PreserveSig]
			int OnSimpleVolumeChanged( float NewVolume, bool newMute, Guid EventContext );
			[PreserveSig]
			int OnChannelVolumeChanged( UInt32 ChannelCount, IntPtr NewChannelVolumeArray, UInt32 ChangedChannel, Guid EventContext );
			[PreserveSig]
			int OnGroupingParamChanged( Guid NewGroupingParam, Guid EventContext );
			[PreserveSig]
			int OnStateChanged( AudioSessionState NewState );
			[PreserveSig]
			int OnSessionDisconnected( AudioSessionDisconnectReason DisconnectReason );
		}

		[Guid( "BFB7FF88-7239-4FC9-8FA2-07C950BE9C6D" ), InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
		public interface IAudioSessionControl2 {
			[PreserveSig]
			int GetState( out AudioSessionState state );
			[PreserveSig]
			int GetDisplayName( [Out(), MarshalAs( UnmanagedType.LPWStr )] out string name );
			[PreserveSig]
			int SetDisplayName( [MarshalAs( UnmanagedType.LPWStr )] string value, Guid EventContext );
			[PreserveSig]
			int GetIconPath( [Out(), MarshalAs( UnmanagedType.LPWStr )] out string Path );
			[PreserveSig]
			int SetIconPath( [MarshalAs( UnmanagedType.LPWStr )] string Value, Guid EventContext );
			[PreserveSig]
			int GetGroupingParam( out Guid GroupingParam );
			[PreserveSig]
			int SetGroupingParam( Guid Override, Guid Eventcontext );
			[PreserveSig]
			int RegisterAudioSessionNotification( IAudioSessionEvents NewNotifications );
			[PreserveSig]
			int UnregisterAudioSessionNotification( IAudioSessionEvents NewNotifications );
			[PreserveSig]
			int GetSessionIdentifier( [Out(), MarshalAs( UnmanagedType.LPWStr )] out string retVal );
			[PreserveSig]
			int GetSessionInstanceIdentifier( [Out(), MarshalAs( UnmanagedType.LPWStr )] out string retVal );
			[PreserveSig]
			int GetProcessId( out UInt32 retvVal );
			[PreserveSig]
			int IsSystemSoundsSession();
			[PreserveSig]
			int SetDuckingPreference( bool optOut );
		}

		#endregion

	}

}
