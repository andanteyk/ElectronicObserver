using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Notifier {

	/// <summary>
	/// 大破進撃警告通知を扱います。
	/// </summary>
	public class NotifierDamage : NotifierBase {

		/// <summary>
		/// 事前通知(出撃前、戦闘結果判明直後)が有効かどうか
		/// </summary>
		public bool NotifiesBefore { get; set; }

		/// <summary>
		/// 事中通知(出撃前、戦績画面)が有効かどうか
		/// </summary>
		public bool NotifiesNow { get; set; }

		/// <summary>
		/// 事後通知(出撃直後、進撃中)が有効かどうか
		/// </summary>
		public bool NotifiesAfter { get; set; }

		/// <summary>
		/// 通知が有効な艦船のLv下限
		/// これよりLvが低い艦は除外されます
		/// </summary>
		public int LevelBorder { get; set; }

		/// <summary>
		/// 非ロック艦も含める
		/// </summary>
		public bool ContainsNotLockedShip { get; set; }

		/// <summary>
		/// ダメコン装備艦も含める
		/// </summary>
		public bool ContainsSafeShip { get; set; }

		/// <summary>
		/// 旗艦を含める
		/// </summary>
		public bool ContainsFlagship { get; set; }

		/// <summary>
		/// 終点でも通知する
		/// </summary>
		public bool NotifiesAtEndpoint { get; set; }


		public NotifierDamage()
			: base() {
			Initialize();
		}

		public NotifierDamage( Utility.Configuration.ConfigurationData.ConfigNotifierDamage config )
			: base( config ) {
			Initialize();

			NotifiesBefore = config.NotifiesBefore;
			NotifiesNow = config.NotifiesNow;
			NotifiesAfter = config.NotifiesAfter;
			LevelBorder = config.LevelBorder;
			ContainsNotLockedShip = config.ContainsNotLockedShip;
			ContainsSafeShip = config.ContainsSafeShip;
			ContainsFlagship = config.ContainsFlagship;
			NotifiesAtEndpoint = config.NotifiesAtEndpoint;
		}


		private void Initialize() {
			DialogData.Title = "！大破警告！";

			APIObserver o = APIObserver.Instance;

			o["api_port/port"].ResponseReceived += CloseAll;

			o["api_req_map/start"].ResponseReceived += InSortie;
			o["api_req_map/next"].ResponseReceived += InSortie;

			o["api_get_member/mapinfo"].ResponseReceived += BeforeSortie;

			o["api_req_sortie/battleresult"].ResponseReceived += BattleFinished;
			o["api_req_combined_battle/battleresult"].ResponseReceived += BattleFinished;

			o["api_req_sortie/battle"].ResponseReceived += BattleStarted;
			o["api_req_battle_midnight/battle"].ResponseReceived += BattleStarted;
			o["api_req_battle_midnight/sp_midnight"].ResponseReceived += BattleStarted;
			o["api_req_sortie/airbattle"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/battle"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/battle_water"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/airbattle"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/midnight_battle"].ResponseReceived += BattleStarted;
			o["api_req_combined_battle/sp_midnight"].ResponseReceived += BattleStarted;

		}

		void CloseAll( string apiname, dynamic data ) {
			DialogData.OnCloseAll();
		}




		private void BeforeSortie( string apiname, dynamic data ) {
			if ( NotifiesNow || NotifiesBefore ) {

				string[] array = GetDamagedShips(
					KCDatabase.Instance.Fleet.Fleets.Values
					.Where( f => f.ExpeditionState == 0 )
					.SelectMany( f => f.MembersWithoutEscaped.Skip( !ContainsFlagship ? 1 : 0 ) ) );

				if ( array != null && array.Length > 0 ) {
					Notify( array );
				}
			}
		}


		private void InSortie( string apiname, dynamic data ) {
			if ( NotifiesAfter ) {

				string[] array = GetDamagedShips( KCDatabase.Instance.Fleet.Fleets.Values
					.Where( f => f.IsInSortie )
					.SelectMany( f => f.MembersWithoutEscaped.Skip( !ContainsFlagship ? 1 : 0 ) ) );


				if ( array != null && array.Length > 0 ) {
					Notify( array );
				}
			}
		}


		private void BattleStarted( string apiname, dynamic data ) {
			if ( NotifiesBefore ) {
				CheckBattle();
			}
		}


		private void BattleFinished( string apiname, dynamic data ) {
			if ( NotifiesNow ) {
				CheckBattle();
			}
		}


		private void CheckBattle() {

			BattleManager bm = KCDatabase.Instance.Battle;

			if ( bm.Compass.IsEndPoint && !NotifiesAtEndpoint )
				return;


			List<string> list = new List<string>();

			switch ( bm.BattleMode & BattleManager.BattleModes.BattlePhaseMask ) {
				case BattleManager.BattleModes.Normal:
				case BattleManager.BattleModes.AirBattle:
					if ( bm.BattleNight != null ) {
						list.AddRange( GetDamagedShips( bm.BattleNight.Initial.FriendFleet, bm.BattleNight.ResultHPs.ToArray() ) );
					} else {
						list.AddRange( GetDamagedShips( bm.BattleDay.Initial.FriendFleet, bm.BattleDay.ResultHPs.ToArray() ) );
					}
					break;

				case BattleManager.BattleModes.NightDay:
				case BattleManager.BattleModes.NightOnly:
					if ( bm.BattleDay != null ) {
						list.AddRange( GetDamagedShips( bm.BattleDay.Initial.FriendFleet, bm.BattleDay.ResultHPs.ToArray() ) );
					} else {
						list.AddRange( GetDamagedShips( bm.BattleNight.Initial.FriendFleet, bm.BattleNight.ResultHPs.ToArray() ) );
					}
					break;
			}

			if ( ( bm.BattleMode & BattleManager.BattleModes.CombinedMask ) != 0 ) {
				switch ( bm.BattleMode & BattleManager.BattleModes.BattlePhaseMask ) {
					case BattleManager.BattleModes.Normal:
					case BattleManager.BattleModes.AirBattle:
						if ( bm.BattleNight != null ) {
							list.AddRange( GetDamagedShips( KCDatabase.Instance.Fleet[2], bm.BattleNight.ResultHPs.Skip( 12 ).ToArray() ) );
						} else {
							list.AddRange( GetDamagedShips( KCDatabase.Instance.Fleet[2], bm.BattleDay.ResultHPs.Skip( 12 ).ToArray() ) );
						}
						break;

					case BattleManager.BattleModes.NightDay:
					case BattleManager.BattleModes.NightOnly:
						if ( bm.BattleDay != null ) {
							list.AddRange( GetDamagedShips( KCDatabase.Instance.Fleet[2], bm.BattleDay.ResultHPs.Skip( 12 ).ToArray() ) );
						} else {
							list.AddRange( GetDamagedShips( KCDatabase.Instance.Fleet[2], bm.BattleNight.ResultHPs.Skip( 12 ).ToArray() ) );
						}
						break;
				}
			}


			if ( list.Count > 0 ) {
				Notify( list.ToArray() );
			}

		}


		// 注: 退避中かどうかまではチェックしない
		private bool IsShipDamaged( ShipData ship, int hp ) {
			return ship != null &&
				hp > 0 &&
				(double)hp / ship.HPMax <= 0.25 &&
				ship.RepairingDockID == -1 &&
				ship.Level >= LevelBorder &&
				( ContainsNotLockedShip ? true : ( ship.IsLocked || ship.SlotInstance.Count( q => q != null && q.IsLocked ) > 0 ) ) &&
				( ContainsSafeShip ? true : !ship.AllSlotInstanceMaster.Any( e => e != null ? e.CategoryType == 23 : false ) );
		}

		private string[] GetDamagedShips( IEnumerable<ShipData> ships ) {
			return ships.Where( s => IsShipDamaged( s, s != null ? s.HPCurrent : 0 ) ).Select( s => string.Format( "{0} ({1}/{2})", s.NameWithLevel, s.HPCurrent, s.HPMax ) ).ToArray();
		}

		private string[] GetDamagedShips( FleetData fleet, int[] hps ) {

			LinkedList<string> list = new LinkedList<string>();

			for ( int i = 0; i < fleet.Members.Count; i++ ) {

				if ( i == 0 && !ContainsFlagship ) continue;

				ShipData s = fleet.MembersInstance[i];

				if ( s != null && !fleet.EscapedShipList.Contains( s.MasterID ) &&
					IsShipDamaged( s, hps[i] ) ) {

					list.AddLast( string.Format( "{0} ({1}/{2})", s.NameWithLevel, hps[i], s.HPMax ) );
				}
			}

			return list.ToArray();
		}

		public void Notify( string[] messages ) {

			DialogData.Message = string.Format( "{0} が大破しています！",
				string.Join( ", ", messages ) );

			base.Notify();
		}


		public override void ApplyToConfiguration( Utility.Configuration.ConfigurationData.ConfigNotifierBase config ) {
			base.ApplyToConfiguration( config );

			var c = config as Utility.Configuration.ConfigurationData.ConfigNotifierDamage;

			if ( c != null ) {
				c.NotifiesBefore = NotifiesBefore;
				c.NotifiesNow = NotifiesNow;
				c.NotifiesAfter = NotifiesAfter;
				c.LevelBorder = LevelBorder;
				c.ContainsNotLockedShip = ContainsNotLockedShip;
				c.ContainsSafeShip = ContainsSafeShip;
				c.ContainsFlagship = ContainsFlagship;
				c.NotifiesAtEndpoint = NotifiesAtEndpoint;
			}
		}

	}
}
