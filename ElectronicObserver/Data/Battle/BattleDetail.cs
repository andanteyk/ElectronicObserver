using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {
	/// <summary>
	/// 戦闘詳細のデータを保持します。
	/// </summary>
	public abstract class BattleDetail {

		public static readonly int AIR_ATTACKER = -999;

		protected int attackerIndex;
		protected int defenderIndex;
		public int[] Damages { get; protected set; }
		public CriticalType[] CriticalTypes { get; protected set; }
		public int AttackType { get; protected set; }

		public ShipDataMaster Attacker { get; protected set; }
		public ShipDataMaster Defender { get; protected set; }


		/// <summary>
		/// The first id in battle data's id list is always -1
		/// Skip it automatically
		/// so return attacker - 1
		/// The same as defender
		/// </summary>
		public int AttackerIndex { get { return attackerIndex - 1; } set { attackerIndex = value + 1; } }

		/// <summary>
		/// Though there mutiple defenders in API, but they are the same so it
		/// uses one defender
		/// </summary>
		public int DefenderIndex { get { return defenderIndex - 1; } set { defenderIndex = value + 1; } }


		public enum CriticalType {
			Miss = 0,
			Hit = 1,
			Critical = 2,
			Invalid = -1
		}


		/// <param name="bd">戦闘情報。</param>
		/// <param name="attackerIndex">攻撃側のインデックス。 1-18</param>
		/// <param name="defenderIndex">防御側のインデックス。 1-18</param>
		/// <param name="damages">ダメージの配列。</param>
		/// <param name="criticalTypes">命中判定の配列。</param>
		/// <param name="attackType">攻撃種別。</param>
		public BattleDetail( BattleData bd, int attackerIndex, int defenderIndex, int[] damages, int[] criticalTypes, int attackType ) {

			this.attackerIndex = attackerIndex;
			this.defenderIndex = defenderIndex;
			Damages = damages;
			CriticalTypes = criticalTypes.Select( i => (CriticalType)i ).ToArray();
			AttackType = attackType;


			int[] slots;

			if ( attackerIndex.Equals( AIR_ATTACKER ) ) {
				Attacker = null;
				slots = null;

			} else if ( AttackerIndex < 6 ) {
				var atk = bd.Initial.FriendFleet.MembersInstance[AttackerIndex];
				Attacker = atk.MasterShip;
				slots = atk.SlotMaster.ToArray();

			} else if ( AttackerIndex < 12 ) {
				Attacker = bd.Initial.EnemyMembersInstance[AttackerIndex - 6];
				slots = bd.Initial.EnemySlots[AttackerIndex - 6];

			} else {
				var atk = KCDatabase.Instance.Fleet[2].MembersInstance[AttackerIndex - 12];
				Attacker = atk.MasterShip;
				slots = atk.SlotMaster.ToArray();
			}


			if ( DefenderIndex < 6 )
				Defender = bd.Initial.FriendFleet.MembersInstance[DefenderIndex].MasterShip;
			else if ( DefenderIndex < 12 )
				Defender = bd.Initial.EnemyMembersInstance[DefenderIndex - 6];
			else
				Defender = KCDatabase.Instance.Fleet[2].MembersInstance[DefenderIndex - 12].MasterShip;


			if ( AttackType == 0 ) {
				AttackType = CaclulateAttackKind( slots, Attacker.ShipID, Defender.ShipID );
			}

		}

		/// <summary>
		/// 戦闘詳細の情報を出力します。
		/// </summary>
		public string BattleDescription() {

			StringBuilder builder = new StringBuilder();
			if ( Attacker == null ) {
				builder.Append( "敵航空隊 → " ).Append( Defender.NameWithClass );
				if ( 6 <= DefenderIndex && DefenderIndex < 12 )
					builder.Append( " #" ).Append( DefenderIndex - 6 + 1 );

			} else {
				builder.Append( Attacker.NameWithClass );
				if ( 6 <= AttackerIndex && AttackerIndex < 12 )
					builder.Append( " #" ).Append( AttackerIndex - 6 + 1 );

				builder.Append( " → " ).Append( Defender.NameWithClass );
				if ( 6 <= DefenderIndex && DefenderIndex < 12 )
					builder.Append( " #" ).Append( DefenderIndex - 6 + 1 );

			}
			builder.AppendLine();

			if ( AttackType >= 0 )
				builder.Append( "[" ).Append( GetAttackKind() ).Append( "] " );

			for ( int i = 0; i < Damages.Length; i++ ) {
				if ( CriticalTypes[i] ==  CriticalType.Invalid )	// カットイン(主砲/主砲)、カットイン(主砲/副砲)時に発生する
					continue;

				if ( i > 0 )
					builder.Append( " , " );

				switch ( CriticalTypes[i] ) {
					case CriticalType.Miss:
						builder.Append( "Miss" );
						break;
					case CriticalType.Hit:
						builder.Append( Damages[i] ).Append( " Dmg" );
						break;
					case CriticalType.Critical:
						builder.Append( Damages[i] ).Append( " Critical!" );
						break;
				}
			}

			builder.AppendLine();
			return builder.ToString();
		}


		protected abstract int CaclulateAttackKind( int[] slots, int attackerShipID, int defenderShipID );
		protected abstract string GetAttackKind();

	}


	/// <summary>
	/// 昼戦の戦闘詳細データを保持します。
	/// </summary>
	public class BattleDayDetail : BattleDetail {

		public BattleDayDetail( BattleData bd, int attackerId, int defenderId, int[] damages, int[] criticalTypes, int attackType )
			: base( bd, attackerId, defenderId, damages, criticalTypes, attackType ) {

		}

		protected override int CaclulateAttackKind( int[] slots, int attackerShipID, int defenderShipID ) {
			return Calculator.GetDayAttackKind( slots, attackerShipID, defenderShipID, false );
		}

		protected override string GetAttackKind() {
			return Constants.GetDayAttackKind( AttackType );
		}
	}

	/// <summary>
	/// 夜戦における戦闘詳細データを保持します。
	/// </summary>
	public class BattleNightDetail : BattleDetail {

		public BattleNightDetail( BattleData bd, int attackerId, int defenderId, int[] damages, int[] criticalTypes, int attackType )
			: base( bd, attackerId, defenderId, damages, criticalTypes, attackType ) {

		}

		protected override int CaclulateAttackKind( int[] slots, int attackerShipID, int defenderShipID ) {
			return Calculator.GetNightAttackKind( slots, attackerShipID, defenderShipID, false );
		}

		protected override string GetAttackKind() {
			return Constants.GetNightAttackKind( AttackType );
		}
	}

	/// <summary>
	/// 航空戦における戦闘詳細データを保持します。
	/// </summary>
	public class BattleAirDetail : BattleDayDetail {

		public BattleAirDetail( BattleData bd, int defenderId, int[] damages, int[] criticalTypes, int attackType )
			: base( bd, AIR_ATTACKER, defenderId, damages, criticalTypes, attackType ) {

		}

		protected override int CaclulateAttackKind( int[] slots, int attackerShipID, int defenderShipID ) {
			return -1;
		}

		protected override string GetAttackKind() {
			switch ( AttackType ) {
				case 1:
					return "雷撃";
				case 2:
					return "爆撃";
				case 3:
					return "雷撃+爆撃";
				default:
					return "不明";
			}
		}

	}
}
