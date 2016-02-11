using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility {

	public sealed class SyncBGMPlayer {

		#region Singleton

		private static readonly SyncBGMPlayer instance = new SyncBGMPlayer();

		public static SyncBGMPlayer Instance {
			get { return instance; }
		}

		#endregion


		[DataContract( Name = "SoundHandle" )]
		public class SoundHandle : IIdentifiable, ICloneable {

			[DataMember]
			public SoundHandleID HandleID { get; set; }

			[DataMember]
			public bool Enabled { get; set; }

			[DataMember]
			public string Path { get; set; }

			[DataMember]
			public bool IsLoop { get; set; }

			[DataMember]
			public double LoopHeadPosition { get; set; }

			[DataMember]
			public int Volume { get; set; }

			public SoundHandle( SoundHandleID id ) {
				HandleID = id;
				Enabled = true;
				Path = "";
				IsLoop = true;
				LoopHeadPosition = 0.0;
				Volume = 100;
			}

			[IgnoreDataMember]
			public int ID {
				get { return (int)HandleID; }
			}

			public override string ToString() {
				return Enum.GetName( typeof( SoundHandleID ), HandleID ) + " : " + Path;
			}

			public SoundHandle Clone() {
				return (SoundHandle)MemberwiseClone();
			}

			object ICloneable.Clone() {
				return Clone();
			}
		}

		public enum SoundHandleID {
			Port = 1,
			Sortie = 101,
			BattleDay = 201,
			BattleNight,
			BattleAir,
			BattleBoss,
			BattlePracticeDay,
			BattlePracticeNight,
			ResultWin = 301,
			ResultLose,
			ResultBossWin,
			Record = 401,
			Item,
			Quest,
			Album,
			ImprovementArsenal,
		}

		public IDDictionary<SoundHandle> Handles { get; internal set; }
		public bool Enabled;
		public bool IsMute {
			get { return _mp.IsMute; }
			set { _mp.IsMute = value; }
		}

		private MediaPlayer _mp;
		private bool _isBoss;


		public SyncBGMPlayer() {

			_mp = new MediaPlayer();

			if ( !_mp.IsAvailable )
				Utility.Logger.Add( 3, "Windows Media Player のロードに失敗しました。音声の再生はできません。" );

			_mp.AutoPlay = false;

			_isBoss = false;


			Enabled = false;
			Handles = new IDDictionary<SoundHandle>();

			foreach ( SoundHandleID id in Enum.GetValues( typeof( SoundHandleID ) ) )
				Handles.Add( new SoundHandle( id ) );



			#region API register
			APIObserver o = APIObserver.Instance;

			o["api_port/port"].ResponseReceived += PlayPort;

			o["api_req_map/start"].ResponseReceived += PlaySortie;
			o["api_req_map/next"].ResponseReceived += PlaySortie;

			o["api_req_sortie/battle"].ResponseReceived += PlayBattleDay;
			o["api_req_combined_battle/battle"].ResponseReceived += PlayBattleDay;
			o["api_req_combined_battle/battle_water"].ResponseReceived += PlayBattleDay;

			o["api_req_battle_midnight/battle"].ResponseReceived += PlayBattleNight;
			o["api_req_battle_midnight/sp_midnight"].ResponseReceived += PlayBattleNight;
			o["api_req_combined_battle/midnight_battle"].ResponseReceived += PlayBattleNight;
			o["api_req_combined_battle/sp_midnight"].ResponseReceived += PlayBattleNight;

			o["api_req_sortie/airbattle"].ResponseReceived += PlayBattleAir;
			o["api_req_combined_battle/airbattle"].ResponseReceived += PlayBattleAir;
			o["api_req_sortie/ld_airbattle"].ResponseReceived += PlayBattleAir;
			o["api_req_combined_battle/ld_airbattle"].ResponseReceived += PlayBattleAir;

			o["api_req_practice/battle"].ResponseReceived += PlayPracticeDay;

			o["api_req_practice/midnight_battle"].ResponseReceived += PlayPracticeNight;

			o["api_req_sortie/battleresult"].ResponseReceived += PlayBattleResult;
			o["api_req_combined_battle/battleresult"].ResponseReceived += PlayBattleResult;
			o["api_req_practice/battle_result"].ResponseReceived += PlayBattleResult;

			o["api_get_member/record"].ResponseReceived += PlayRecord;

			o["api_get_member/payitem"].ResponseReceived += PlayItem;

			o["api_get_member/questlist"].ResponseReceived += PlayQuest;

			o["api_get_member/picture_book"].ResponseReceived += PlayAlbum;

			o["api_req_kousyou/remodel_slotlist"].ResponseReceived += PlayImprovementArsenal;

			#endregion

			Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
			SystemEvents.SystemShuttingDown += SystemEvents_SystemShuttingDown;
		}

		public void ConfigurationChanged() {
			var c = Utility.Configuration.Config.BGMPlayer;

			Enabled = c.Enabled;

			if ( c.Handles != null )
				Handles = new IDDictionary<SoundHandle>( c.Handles );

			if ( !c.SyncBrowserMute )
				IsMute = false;

			// 設定変更を適用するためいったん閉じる
			_mp.Close();
		}

		void SystemEvents_SystemShuttingDown() {
			var c = Utility.Configuration.Config.BGMPlayer;

			c.Enabled = Enabled;
			c.Handles = Handles.Values.ToList();
		}


		public void SetInitialVolume( int volume ) {
			_mp.Volume = volume;
		}



		void PlayPort( string apiname, dynamic data ) {
			_isBoss = false;
			Play( Handles[(int)SoundHandleID.Port] );
		}

		void PlaySortie( string apiname, dynamic data ) {
			Play( Handles[(int)SoundHandleID.Sortie] );
			_isBoss = (int)data.api_event_id == 5;
		}

		void PlayBattleDay( string apiname, dynamic data ) {
			if ( _isBoss )
				Play( Handles[(int)SoundHandleID.BattleBoss] );
			else
				Play( Handles[(int)SoundHandleID.BattleDay] );
		}

		void PlayBattleNight( string apiname, dynamic data ) {
			if ( _isBoss )
				Play( Handles[(int)SoundHandleID.BattleBoss] );
			else
				Play( Handles[(int)SoundHandleID.BattleNight] );
		}

		void PlayBattleAir( string apiname, dynamic data ) {
			if ( _isBoss )
				Play( Handles[(int)SoundHandleID.BattleBoss] );
			else
				Play( Handles[(int)SoundHandleID.BattleAir] );
		}

		void PlayPracticeDay( string apiname, dynamic data ) {
			Play( Handles[(int)SoundHandleID.BattlePracticeDay] );
		}

		void PlayPracticeNight( string apiname, dynamic data ) {
			Play( Handles[(int)SoundHandleID.BattlePracticeNight] );

		}

		void PlayBattleResult( string apiname, dynamic data ) {
			switch ( (string)data.api_win_rank ) {
				case "S":
				case "A":
				case "B":
					if ( _isBoss )
						Play( Handles[(int)SoundHandleID.ResultBossWin] );
					else
						Play( Handles[(int)SoundHandleID.ResultWin] );
					break;
				default:
					Play( Handles[(int)SoundHandleID.ResultLose] );
					break;
			}
		}


		void PlayRecord( string apiname, dynamic data ) {
			Play( Handles[(int)SoundHandleID.Record] );
		}

		void PlayItem( string apiname, dynamic data ) {
			Play( Handles[(int)SoundHandleID.Item] );
		}

		void PlayQuest( string apiname, dynamic data ) {
			Play( Handles[(int)SoundHandleID.Quest] );
		}

		void PlayAlbum( string apiname, dynamic data ) {
			Play( Handles[(int)SoundHandleID.Album] );
		}

		void PlayImprovementArsenal( string apiname, dynamic data ) {
			Play( Handles[(int)SoundHandleID.ImprovementArsenal] );
		}


		private bool Play( SoundHandle sh ) {
			if ( Enabled &&
				sh != null &&
				sh.Enabled &&
				!string.IsNullOrWhiteSpace( sh.Path ) &&
				_mp.SourcePath != sh.Path ) {

				_mp.Close();
				_mp.SourcePath = sh.Path;
				_mp.IsLoop = sh.IsLoop;
				_mp.LoopHeadPosition = sh.LoopHeadPosition;
				if ( !Utility.Configuration.Config.Control.UseSystemVolume )
					_mp.Volume = sh.Volume;
				_mp.Play();

				return true;
			}

			return false;
		}



		public static string SoundHandleIDToString( SoundHandleID id ) {
			switch ( id ) {
				case SoundHandleID.Port:
					return "母港";
				case SoundHandleID.Sortie:
					return "出撃中";
				case SoundHandleID.BattleDay:
					return "昼戦";
				case SoundHandleID.BattleNight:
					return "夜戦";
				case SoundHandleID.BattleAir:
					return "航空戦";
				case SoundHandleID.BattleBoss:
					return "ボス戦";
				case SoundHandleID.BattlePracticeDay:
					return "演習昼戦";
				case SoundHandleID.BattlePracticeNight:
					return "演習夜戦";
				case SoundHandleID.ResultWin:
					return "勝利";
				case SoundHandleID.ResultLose:
					return "敗北";
				case SoundHandleID.ResultBossWin:
					return "ボス勝利";
				case SoundHandleID.Record:
					return "戦績";
				case SoundHandleID.Item:
					return "アイテム";
				case SoundHandleID.Quest:
					return "任務";
				case SoundHandleID.Album:
					return "図鑑";
				case SoundHandleID.ImprovementArsenal:
					return "改修工廠";
				default:
					return "不明";
			}
		}
	}
}
