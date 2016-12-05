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


		public override bool IsAvailable {
			get { return RawData != null; }
		}

		public override void EmulateBattle( int[] hps, int[] damages ) {
			throw new NotSupportedException();
		}

		/// <summary>
		/// 自軍艦隊ID
		/// </summary>
		public int FriendFleetID { get; private set; }

		/// <summary>
		/// 自軍艦隊
		/// </summary>
		public FleetData FriendFleet { get { return KCDatabase.Instance.Fleet[FriendFleetID]; } }

		/// <summary>
		/// 自軍随伴艦隊
		/// </summary>
		public FleetData FriendFleetEscort { get { return IsCombined ? KCDatabase.Instance.Fleet[2] : null; } }


		/// <summary>
		/// 敵艦隊メンバ
		/// </summary>
		public int[] EnemyMembers { get; private set; }

		/// <summary>
		/// 敵艦隊メンバ
		/// </summary>
		public ShipDataMaster[] EnemyMembersInstance { get; private set; }


		/// <summary>
		/// 敵艦隊メンバ(随伴艦隊)
		/// </summary>
		public int[] EnemyMembersEscort { get; private set; }

		/// <summary>
		/// 敵艦隊メンバ(随伴艦隊)
		/// </summary>
		public ShipDataMaster[] EnemyMembersEscortInstance { get; private set; }


		/// <summary>
		/// 敵艦隊メンバ [0-5]=主力艦隊 [6-11]=随伴艦隊
		/// </summary>
		public int[] AllEnemyMembers { get; private set; }

		/// <summary>
		/// 敵艦隊メンバ [0-5]=主力艦隊 [6-11]=随伴艦隊
		/// </summary>
		public ShipDataMaster[] AllEnemyMembersInstance { get; private set; }


		/// <summary>
		/// 敵艦のレベル
		/// </summary>
		public int[] EnemyLevels { get; private set; }

		/// <summary>
		/// 敵艦のレベル(随伴艦隊)
		/// </summary>
		public int[] EnemyLevelsEscort { get; private set; }


		/// <summary>
		/// 戦闘開始時のHPリスト
		/// [0-5]=自軍, [6-11]=敵軍, [12-17]=(連合艦隊時)自軍随伴, [18-23]=(敵連合艦隊時)敵軍随伴
		/// </summary>
		public int[] InitialHPs { get; private set; }

		/// <summary>
		/// 最大HPリスト
		/// [0-5]=自軍, [6-11]=敵軍, [12-17]=(連合艦隊時)随伴, [18-23]=(敵連合艦隊時)敵軍随伴
		/// </summary>
		public int[] MaxHPs { get; private set; }


		/// <summary>
		/// 敵艦のスロット
		/// </summary>
		public int[][] EnemySlots { get; private set; }

		/// <summary>
		/// 敵艦のスロット
		/// </summary>
		public EquipmentDataMaster[][] EnemySlotsInstance { get; private set; }


		/// <summary>
		/// 敵艦のスロット(随伴艦隊)
		/// </summary>
		public int[][] EnemySlotsEscort { get; private set; }

		/// <summary>
		/// 敵艦のスロット(随伴艦隊)
		/// </summary>
		public EquipmentDataMaster[][] EnemySlotsEscortInstance { get; private set; }


		/// <summary>
		/// 敵艦のパラメータ
		/// </summary>
		public int[][] EnemyParameters { get; private set; }

		/// <summary>
		/// 敵艦のパラメータ(随伴艦隊)
		/// </summary>
		public int[][] EnemyParametersEscort { get; private set; }


		/// <summary>
		/// 装甲破壊されているか
		/// </summary>
		public bool IsBossDamaged {
			get {
				return RawData.api_xal01() && (int)RawData.api_xal01 > 0;
			}
		}

		/// <summary>
		/// 戦闘糧食を食べた艦娘のインデックス [0-11]
		/// </summary>
		public int[] RationIndexes { get; private set; }




		public PhaseInitial( BattleData data, string title )
			: base( data, title ) {

			{
				dynamic id = RawData.api_dock_id() ? RawData.api_dock_id :
					RawData.api_deck_id() ? RawData.api_deck_id : 1;
				FriendFleetID = id is string ? int.Parse( (string)id ) : (int)id;
			}
			if ( FriendFleetID <= 0 )
				FriendFleetID = 1;

			EnemyMembers = ArraySkip( (int[])RawData.api_ship_ke );
			EnemyMembersInstance = EnemyMembers.Select( id => KCDatabase.Instance.MasterShips[id] ).ToArray();

			EnemyMembersEscort = !RawData.api_ship_ke_combined() ? null : ArraySkip( (int[])RawData.api_ship_ke_combined );
			EnemyMembersEscortInstance = EnemyMembersEscort == null ? null : EnemyMembersEscort.Select( id => KCDatabase.Instance.MasterShips[id] ).ToArray();

			AllEnemyMembers = EnemyMembers.Concat( EnemyMembersEscort ?? Enumerable.Repeat( -1, 6 ) ).ToArray();
			AllEnemyMembersInstance = EnemyMembersInstance.Concat( EnemyMembersEscortInstance ?? Enumerable.Repeat<ShipDataMaster>( null, 6 ) ).ToArray();

			EnemyLevels = ArraySkip( (int[])RawData.api_ship_lv );
			EnemyLevelsEscort = !RawData.api_ship_lv_combined() ? null : ArraySkip( (int[])RawData.api_ship_lv_combined );

			InitialHPs = GetHPArray( (int[])RawData.api_nowhps, !RawData.api_nowhps_combined() ? null : (int[])RawData.api_nowhps_combined );
			MaxHPs = GetHPArray( (int[])RawData.api_maxhps, !RawData.api_maxhps_combined() ? null : (int[])RawData.api_maxhps_combined );

			EnemySlots = ( (dynamic[])RawData.api_eSlot ).Select( d => (int[])d ).ToArray();
			EnemySlotsInstance = EnemySlots.Select( part => part.Select( id => KCDatabase.Instance.MasterEquipments[id] ).ToArray() ).ToArray();

			EnemySlotsEscort = !RawData.api_eSlot_combined() ? null : ( (dynamic[])RawData.api_eSlot_combined ).Select( d => (int[])d ).ToArray();
			EnemySlotsEscortInstance = EnemySlotsEscort == null ? null : EnemySlotsEscort.Select( part => part.Select( id => KCDatabase.Instance.MasterEquipments[id] ).ToArray() ).ToArray();

			EnemyParameters = !RawData.api_eParam() ? null : ( (dynamic[])RawData.api_eParam ).Select( d => (int[])d ).ToArray();
			EnemyParametersEscort = !RawData.api_eParam_combined() ? null : ( (dynamic[])RawData.api_eParam_combined ).Select( d => (int[])d ).ToArray();

			{
				var rations = new List<int>();
				if ( RawData.api_combat_ration() ) {
					rations.AddRange( ( (int[])RawData.api_combat_ration ).Select( i => FriendFleet.Members.IndexOf( i ) ) );
				}
				if ( RawData.api_combat_ration_combined() ) {
					rations.AddRange( ( (int[])RawData.api_combat_ration_combined ).Select( i => FriendFleetEscort.Members.IndexOf( i ) + 6 ) );
				}
				RationIndexes = rations.ToArray();
			}
		}


		private int[] GetHPArray( int[] mainhp, int[] escorthp ) {
			var main = mainhp.Skip( 1 );
			var escort = escorthp == null ? null : escorthp.Skip( 1 );

			// 稀に参加艦が6隻以下だと配列長が短くなることがあるので :(
			if ( main.Count() < 12 )
				main = main.Concat( Enumerable.Repeat( -1, 12 - main.Count() ) );
			if ( escort != null && escort.Count() < 12 )
				escort = escort.Concat( Enumerable.Repeat( -1, 12 - escort.Count() ) );

			if ( escort != null )
				return main.Concat( escort ).ToArray();
			else
				return main.Concat( Enumerable.Repeat( -1, 12 ) ).ToArray();
		}


		/// <summary>
		/// 2016/11/19 現在、連合艦隊夜戦において味方随伴艦隊が 最大HP = 現在HP となる不具合が存在するため、
		/// 昼戦データから最大HPを取得する
		/// </summary>
		public void TakeOverMaxHPs( BattleData bd ) {
			Array.Copy( bd.Initial.MaxHPs, 12, MaxHPs, 12, 6 );
		}

	}
}
