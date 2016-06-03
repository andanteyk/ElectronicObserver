using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Phase {

	/// <summary>
	/// 戦闘開始フェーズの処理を行います。
	/// </summary>
	public class PhaseInitial : PhaseBase {


		public PhaseInitial( BattleData data )
			: base( data ) { }


		public override bool IsAvailable {
			get { return RawData != null; }
		}

		public override void EmulateBattle( int[] hps, int[] damages ) {
			throw new NotSupportedException();
		}

		/// <summary>
		/// 自軍艦隊ID
		/// </summary>
		public int FriendFleetID {
			get {
				dynamic id = RawData.api_dock_id() ? RawData.api_dock_id : RawData.api_deck_id;
				return id is string ? int.Parse( (string)id ) : (int)id;
			}
		}

		/// <summary>
		/// 自軍艦隊
		/// </summary>
		public FleetData FriendFleet { get { return KCDatabase.Instance.Fleet[FriendFleetID]; } }

		/// <summary>
		/// 伴随舰队
		/// </summary>
		public FleetData AccompanyFleet
		{
			get { return ( IsCombined && ( FriendFleetID == 1 ) ) ? KCDatabase.Instance.Fleet[2] : FriendFleet; }
		}


		/// <summary>
		/// 敵艦隊メンバ
		/// </summary>
		public int[] EnemyMembers {
			get { return ArraySkip( (int[])RawData.api_ship_ke ); }
		}

		/// <summary>
		/// 敵艦隊メンバ
		/// </summary>
		public ShipDataMaster[] EnemyMembersInstance {
			get {
				return ( (int[])RawData.api_ship_ke ).Skip( 1 ).Select( id => KCDatabase.Instance.MasterShips[id] ).ToArray();
			}
		}

		/// <summary>
		/// 敵艦のレベル
		/// </summary>
		public int[] EnemyLevels {
			get { return ArraySkip( (int[])RawData.api_ship_lv ); }
		}

		/// <summary>
		/// 戦闘開始時のHPリスト
		/// [0-5]=自軍, [6-11]=敵軍, [12-17]=(連合艦隊時)随伴
		/// </summary>
		public int[] InitialHPs {
			get {
				var list = (int[])RawData.api_nowhps;
				if ( list.Length < 13 )		//頭にきました。
					list = list.Concat( Enumerable.Repeat( -1, 13 - list.Length ) ).ToArray();

				if ( RawData.api_nowhps_combined() )
					return list.Skip( 1 ).Concat( ( (int[])RawData.api_nowhps_combined ).Skip( 1 ) ).ToArray();
				else
					return ArraySkip( list );
			}
		}

		/// <summary>
		/// 最大HPリスト
		/// [0-5]=自軍, [6-11]=敵軍, [12-17]=(連合艦隊時)随伴
		/// </summary>
		public int[] MaxHPs {
			get {
				if ( RawData.api_maxhps_combined() )
					return ( (int[])RawData.api_maxhps ).Skip( 1 ).Concat( ( (int[])RawData.api_maxhps_combined ).Skip( 1 ) ).ToArray();
				else
					return ArraySkip( (int[])RawData.api_maxhps );
			}
		}

		/// <summary>
		/// 敵艦のスロット
		/// </summary>
		public int[][] EnemySlots {
			get {
				return ( (dynamic[])RawData.api_eSlot ).Select( d => (int[])d ).ToArray();
			}
		}

		/// <summary>
		/// 敵艦のスロット
		/// </summary>
		public EquipmentDataMaster[][] EnemySlotsInstance {
			get {
				return ( (dynamic[])RawData.api_eSlot ).Select( d => ( (int[])d ).Select( id => KCDatabase.Instance.MasterEquipments[id] ).ToArray() ).ToArray();
			}
		}

		/// <summary>
		/// 敵艦のパラメータ
		/// </summary>
		public int[][] EnemyParameters {
			get {
				return ( (dynamic[])RawData.api_eParam ).Select( d => (int[])d ).ToArray();
			}
		}


		/// <summary>
		/// 装甲破壊されているか
		/// </summary>
		public bool IsBossDamaged {
			get {
				return RawData.api_boss_damaged() && (int)RawData.api_boss_damaged > 0;
			}
		}

	}
}
