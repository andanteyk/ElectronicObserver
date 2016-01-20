using CoreAudioApi;
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

		private static SimpleAudioVolume simpleAudioVolume;


		/// <summary>
		/// 音量操作のためのデータを取得します。
		/// </summary>
		/// <param name="processID">対象のプロセスID。</param>
		/// <returns>データ。取得に失敗した場合は null。</returns>
		private static SimpleAudioVolume GetVolumeObject( uint processID ) {

			if ( simpleAudioVolume != null ) {
				return simpleAudioVolume;
			}

			var deviceEnumerator = new MMDeviceEnumerator();
			MMDevice devices;
			try
			{
				devices = deviceEnumerator.GetDefaultAudioEndpoint( EDataFlow.eRender, ERole.eMultimedia );
			}
			catch
			{
				
				return null;
			}

			for ( int i = 0; i < devices.AudioSessionManager.Sessions.Count; i++ ) {
				var session = devices.AudioSessionManager.Sessions[i];
				if ( session.ProcessID == processID ) {
					simpleAudioVolume = session.SimpleAudioVolume;
					return simpleAudioVolume;
				}
			}

			return null;
		}


		private const string ErrorMessageNotFound = "指定したプロセスIDの音量オブジェクトは存在しません。";

		/// <summary>
		/// 音量を取得します。
		/// </summary>
		/// <param name="processID">対象のプロセスID。</param>
		/// <returns>音量( 0.0 - 1.0 )。</returns>
		public static float GetApplicationVolume( uint processID ) {
			var volume = GetVolumeObject( processID );
			if ( volume == null )
				throw new ArgumentException( ErrorMessageNotFound );

			return volume.MasterVolume;
		}

		/// <summary>
		/// ミュート状態を取得します。
		/// </summary>
		/// <param name="processID">対象のプロセスID。</param>
		/// <returns>ミュートされていれば true。</returns>
		public static bool GetApplicationMute( uint processID ) {
			var volume = GetVolumeObject( processID );
			if ( volume == null )
				throw new ArgumentException( ErrorMessageNotFound );

			return volume.Mute;
		}


		/// <summary>
		/// 音量を設定します。
		/// </summary>
		/// <param name="processID">対象のプロセスID。</param>
		/// <param name="level">音量( 0.0 - 1.0 )。</param>
		public static void SetApplicationVolume( uint processID, float level ) {
			var volume = GetVolumeObject( processID );
			if ( volume == null )
				throw new ArgumentException( ErrorMessageNotFound );

			volume.MasterVolume = level;
		}

		/// <summary>
		/// ミュート状態を設定します。
		/// </summary>
		/// <param name="processID">対象のプロセスID。</param>
		/// <param name="mute">ミュートするなら true。</param>
		public static void SetApplicationMute( uint processID, bool mute ) {
			var volume = GetVolumeObject( processID );
			if ( volume == null )
				throw new ArgumentException( ErrorMessageNotFound );

			volume.Mute = mute;
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



	}

}
