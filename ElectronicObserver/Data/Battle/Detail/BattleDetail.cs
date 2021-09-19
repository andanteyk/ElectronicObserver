using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Detail
{
	/// <summary>
	/// 戦闘詳細のデータを保持します。
	/// </summary>
	public abstract class BattleDetail
	{

		public double[] RawDamages { get; protected set; }
		public int[] Damages { get; protected set; }
		public bool[] GuardsFlagship { get; protected set; }
		public CriticalType[] CriticalTypes { get; protected set; }
		public int AttackType { get; protected set; }
		public int[] EquipmentIDs { get; protected set; }
		public int DefenderHP { get; protected set; }

		public ShipDataMaster Attacker { get; protected set; }
		public ShipDataMaster Defender { get; protected set; }


		/// <summary> 攻撃側インデックス </summary>
		public BattleIndex AttackerIndex { get; protected set; }

		/// <summary> 防御側インデックス </summary>
		public BattleIndex DefenderIndex { get; protected set; }


		protected readonly BattleData Battle;


		public enum CriticalType
		{
			Miss = 0,
			Hit = 1,
			Critical = 2,
			Invalid = -1
		}


		/// <param name="bd">戦闘情報。</param>
		/// <param name="attackerIndex">攻撃側のインデックス。</param>
		/// <param name="defenderIndex">防御側のインデックス。</param>
		/// <param name="damages">ダメージの配列。</param>
		/// <param name="criticalTypes">命中判定の配列。</param>
		/// <param name="attackType">攻撃種別。</param>
		/// <param name="defenderHP">防御側の攻撃を受ける直前のHP。</param>
		public BattleDetail(BattleData bd, BattleIndex attackerIndex, BattleIndex defenderIndex, double[] damages, int[] criticalTypes, int attackType, int[] equipmentIDs, int defenderHP)
		{
			Battle = bd;

			AttackerIndex = attackerIndex;
			DefenderIndex = defenderIndex;
			RawDamages = damages;
			Damages = damages.Select(dmg => (int)dmg).ToArray();
			GuardsFlagship = damages.Select(dmg => dmg != Math.Floor(dmg)).ToArray();
			CriticalTypes = criticalTypes.Select(i => (CriticalType)i).ToArray();
			AttackType = attackType;
			EquipmentIDs = equipmentIDs;
			DefenderHP = defenderHP;

		}


		protected int[] SetAttacker()
		{
			if (AttackerIndex < 0)
			{
				Attacker = null;
				return null;
			}
			else
			{
				switch (AttackerIndex.Side)
				{
					case BattleSides.FriendMain:
						{
							var atk = Battle.Initial.FriendFleet.MembersInstance[AttackerIndex.Index];
							Attacker = atk.MasterShip;
							return atk.AllSlotMaster.ToArray();
						}

					case BattleSides.FriendEscort:
						{
							var atk = Battle.Initial.FriendFleetEscort.MembersInstance[AttackerIndex.Index];
							Attacker = atk.MasterShip;
							return atk.AllSlotMaster.ToArray();
						}

					case BattleSides.EnemyMain:
						Attacker = Battle.Initial.EnemyMembersInstance[AttackerIndex.Index];
						return Battle.Initial.EnemySlots[AttackerIndex.Index];

					case BattleSides.EnemyEscort:
						Attacker = Battle.Initial.EnemyMembersEscortInstance[AttackerIndex.Index];
						return Battle.Initial.EnemySlotsEscort[AttackerIndex.Index];

					default:
						throw new NotSupportedException();
				}
			}
		}

		protected void SetDefender()
		{
			if (Battle.IsBaseAirRaid)
			{
				Defender = null;
			}
			else
			{
				switch (DefenderIndex.Side)
				{
					case BattleSides.FriendMain:
						Defender = Battle.Initial.FriendFleet.MembersInstance[DefenderIndex.Index].MasterShip;
						break;

					case BattleSides.FriendEscort:
						Defender = Battle.Initial.FriendFleetEscort.MembersInstance[DefenderIndex.Index].MasterShip;
						break;

					case BattleSides.EnemyMain:
						Defender = Battle.Initial.EnemyMembersInstance[DefenderIndex.Index];
						break;

					case BattleSides.EnemyEscort:
						Defender = Battle.Initial.EnemyMembersEscortInstance[DefenderIndex.Index];
						break;
				}
			}

		}



		/// <summary>
		/// 戦闘詳細の情報を出力します。
		/// </summary>
		public override string ToString()
		{

			StringBuilder builder = new StringBuilder();


			if (Battle.IsPractice)
				builder.AppendFormat("{0}{1} → {2}{3}",
					Attacker == null ? "" : AttackerIndex.IsFriend ? "自軍 " : "敵軍 ", GetAttackerName(),
					DefenderIndex.IsFriend ? "自軍 " : "敵軍 ", GetDefenderName()
					).AppendLine();
			else
				builder.AppendFormat("{0} → {1}", GetAttackerName(), GetDefenderName()).AppendLine();


			if (AttackType >= 0)
				builder.Append("[").Append(GetAttackKind()).Append("] ");

			/*// 
			if ( EquipmentIDs != null ) {
				var eqs = EquipmentIDs.Select( id => KCDatabase.Instance.MasterEquipments[id] ).Where( eq => eq != null ).Select( eq => eq.Name );
				if ( eqs.Any() )
					builder.Append( "(" ).Append( string.Join( ", ", eqs ) ).Append( ") " );
			}
			//*/

			for (int i = 0; i < Damages.Length; i++)
			{
				if (CriticalTypes[i] == CriticalType.Invalid)   // カットイン(主砲/主砲)、カットイン(主砲/副砲)時に発生する
					continue;

				if (i > 0)
					builder.Append(" , ");

				if (GuardsFlagship[i])
					builder.Append("<かばう> ");

				switch (CriticalTypes[i])
				{
					case CriticalType.Miss:
						builder.Append("Miss");
						break;
					case CriticalType.Hit:
						builder.Append(Damages[i]).Append(" Dmg");
						break;
					case CriticalType.Critical:
						builder.Append(Damages[i]).Append(" Critical!");
						break;
				}

			}


			int beforeHP = Math.Max(DefenderHP, 0);
			int afterHP = Math.Max(DefenderHP - Damages.Sum(), 0);
			if (beforeHP != afterHP)
				builder.AppendFormat(" ( {0} → {1} )", beforeHP, afterHP);



			builder.AppendLine();


			// damage control
			if (beforeHP > 0 && afterHP <= 0 && DefenderIndex.IsFriend && !Battle.IsPractice && !Battle.IsBaseAirRaid)
			{				
				var defender = (DefenderIndex.Side == BattleSides.FriendEscort ? Battle.Initial.FriendFleetEscort : Battle.Initial.FriendFleet)
					?.MembersInstance?.ElementAtOrDefault(DefenderIndex.Index);
				if (defender != null)
				{
					int id = defender.DamageControlID;

					if (id == 42)
						builder.AppendFormat("　応急修理要員発動　HP{0}", (int)(defender.HPMax * 0.2)).AppendLine();

					else if (id == 43)
						builder.AppendFormat("　応急修理女神発動　HP{0}", defender.HPMax).AppendLine();

				}
			}
			return builder.ToString();
		}


		protected virtual string GetAttackerName()
		{
			int index = AttackerIndex.Index + 1 + (AttackerIndex.IsEscort ? 6 : 0);

			if (Attacker == null)
				return "#" + index;

			return Attacker.NameWithClass + " #" + index;
		}

		protected virtual string GetDefenderName()
		{
			int index = DefenderIndex.Index + 1 + (DefenderIndex.IsEscort ? 6 : 0);

			if (Defender == null)
				return "#" + index;
			return Defender.NameWithClass + " #" + index;
		}

		protected abstract int CaclulateAttackKind(int[] slots, int attackerShipID, int defenderShipID);
		protected abstract string GetAttackKind();

	}


	/// <summary>
	/// 昼戦の戦闘詳細データを保持します。
	/// </summary>
	public class BattleDayDetail : BattleDetail
	{

		public BattleDayDetail(BattleData bd, BattleIndex attackerId, BattleIndex defenderId, double[] damages, int[] criticalTypes, int attackType, int[] equipmentIDs, int defenderHP)
			: base(bd, attackerId, defenderId, damages, criticalTypes, attackType, equipmentIDs, defenderHP)
		{
			var attackerSlots = SetAttacker();
			SetDefender();

			if (AttackType == 0 && Attacker != null)
				AttackType = CaclulateAttackKind(attackerSlots, Attacker.ShipID, Defender.ShipID);

		}

		protected override int CaclulateAttackKind(int[] slots, int attackerShipID, int defenderShipID)
		{
			return (int)Calculator.GetDayAttackKind(slots, attackerShipID, defenderShipID, false);
		}

		protected override string GetAttackKind()
		{
			return Constants.GetDayAttackKind((DayAttackKind)AttackType);
		}
	}

	/// <summary>
	/// 支援攻撃の戦闘詳細データを保持します。
	/// </summary>
	public class BattleSupportDetail : BattleDetail
	{

		public BattleSupportDetail(BattleData bd, BattleIndex defenderId, double damage, int criticalType, int attackType, int defenderHP)
			: base(bd, BattleIndex.Invalid, defenderId, new double[] { damage }, new int[] { criticalType }, attackType, null, defenderHP)
		{
			var attackerSlots = SetAttacker();
			SetDefender();

			if (AttackType == 0 && Attacker != null)
				AttackType = CaclulateAttackKind(attackerSlots, Attacker.ShipID, Defender.ShipID);
		}

		protected override string GetAttackerName()
		{
			return "支援艦隊";
		}

		protected override int CaclulateAttackKind(int[] slots, int attackerShipID, int defenderShipID)
		{
			return -1;
		}

		protected override string GetAttackKind()
		{
			switch (AttackType)
			{
				case 1:
					return "空撃";
				case 2:
					return "砲撃";
				case 3:
					return "雷撃";
				case 4:
					return "爆撃";
				default:
					return "不明";
			}
		}

	}

	/// <summary>
	/// 夜戦における戦闘詳細データを保持します。
	/// </summary>
	public class BattleNightDetail : BattleDetail
	{

		public bool NightAirAttackFlag { get; protected set; }

		public BattleNightDetail(BattleData bd, BattleIndex attackerId, BattleIndex defenderId, double[] damages, int[] criticalTypes, int attackType, int[] equipmentIDs, bool nightAirAttackFlag, int defenderHP)
			: base(bd, attackerId, defenderId, damages, criticalTypes, attackType, equipmentIDs, defenderHP)
		{
			NightAirAttackFlag = nightAirAttackFlag;

			var attackerSlots = SetAttacker();
			SetDefender();

			if (AttackType == 0 && Attacker != null)
				AttackType = CaclulateAttackKind(attackerSlots, Attacker.ShipID, Defender.ShipID);
		}

		protected override int CaclulateAttackKind(int[] slots, int attackerShipID, int defenderShipID)
		{
			return (int)Calculator.GetNightAttackKind(slots, attackerShipID, defenderShipID, false, NightAirAttackFlag);
		}

		protected override string GetAttackKind()
		{
			return Constants.GetNightAttackKind((NightAttackKind)AttackType);
		}
	}

	/// <summary>
	/// 航空戦における戦闘詳細データを保持します。
	/// </summary>
	public class BattleAirDetail : BattleDayDetail
	{
		public int WaveIndex { get; protected set; }

		public BattleAirDetail(BattleData bd, int waveIndex, BattleIndex defenderId, double damage, int criticalType, int attackType, int defenderHP)
			: base(bd, BattleIndex.Invalid, defenderId, new double[] { damage }, new int[] { criticalType }, attackType, null, defenderHP)
		{
			WaveIndex = waveIndex;
		}

		protected override string GetAttackerName()
		{
			if (WaveIndex <= 0)
			{
				if (DefenderIndex.Side == BattleSides.FriendMain || DefenderIndex.Side == BattleSides.FriendEscort)
					return "敵軍航空隊";
				else
					return "自軍航空隊";

			}
			else
			{
				return string.Format("基地航空隊 第{0}波", WaveIndex);
			}
		}

		protected override string GetDefenderName()
		{
			if (WaveIndex < 0 && DefenderIndex.Side == BattleSides.FriendMain)
				return string.Format("第{0}基地", DefenderIndex.Index + 1);

			return base.GetDefenderName();
		}

		protected override int CaclulateAttackKind(int[] slots, int attackerShipID, int defenderShipID)
		{
			return -1;
		}

		protected override string GetAttackKind()
		{
			switch (AttackType)
			{
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


	/// <summary>
	/// 友軍艦隊砲撃における戦闘詳細データを保持します。
	/// </summary>
	public class BattleFriendlyShellingDetail : BattleDetail
	{

		public bool NightAirAttackFlag { get; protected set; }

		public BattleFriendlyShellingDetail(BattleNight bd, BattleIndex attackerId, BattleIndex defenderId, double[] damages, int[] criticalTypes, int attackType, int[] equipmentIDs, bool nightAirAttackFlag, int defenderHP)
			: base(bd, attackerId, defenderId, damages, criticalTypes, attackType, equipmentIDs, defenderHP)
		{
			NightAirAttackFlag = nightAirAttackFlag;

			int[] attackerSlots;

			if (attackerId.IsFriend)
			{
				Attacker = bd.FriendlySupportInfo.FriendlyMembersInstance[attackerId.Index];
				attackerSlots = bd.FriendlySupportInfo.FriendlySlots[attackerId.Index];
			}
			else
			{
				attackerSlots = SetAttacker();
			}

			if (defenderId.IsFriend)
				Defender = bd.FriendlySupportInfo.FriendlyMembersInstance[defenderId.Index];
			else
				SetDefender();


			if (AttackType == 0 && Attacker != null)
				AttackType = CaclulateAttackKind(attackerSlots, Attacker.ShipID, Defender.ShipID);
		}

		protected override int CaclulateAttackKind(int[] slots, int attackerShipID, int defenderShipID)
		{
			return (int)Calculator.GetNightAttackKind(slots, attackerShipID, defenderShipID, false, NightAirAttackFlag);
		}

		protected override string GetAttackKind()
		{
			return Constants.GetNightAttackKind((NightAttackKind)AttackType);
		}
	}

	/// <summary>
	/// 友軍艦隊航空攻撃における戦闘詳細データを保持します。
	/// </summary>
	public class BattleFriendlyAirDetail : BattleAirDetail
	{
		public BattleFriendlyAirDetail(BattleData bd, BattleIndex defenderId, double damage, int criticalType, int attackType, int defenderHP)
			: base(bd, 0, defenderId, damage, criticalType, attackType, defenderHP)
		{
			if (defenderId.IsFriend)
				Defender = bd.FriendlySupportInfo.FriendlyMembersInstance[defenderId.Index];
			else
				SetDefender();
		}

		protected override string GetDefenderName()
		{
			if (DefenderIndex.IsFriend)
			{
				return Battle.FriendlySupportInfo.FriendlyMembersInstance[DefenderIndex.Index].NameWithClass + " #" + (DefenderIndex.Index + 1);
			}

			return base.GetDefenderName();
		}
	}

}
