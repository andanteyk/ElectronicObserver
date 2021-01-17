using ElectronicObserver.Data;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.Data
{

	/// <summary>
	/// 汎用計算クラス
	/// </summary>
	public static class Calculator
	{

		/// <summary>
		/// レベルに依存するパラメータ値を求めます。
		/// </summary>
		/// <param name="min">初期値。</param>
		/// <param name="max">最大値。</param>
		/// <param name="lv">レベル。</param>
		/// <returns></returns>
		public static int GetParameterFromLevel(int min, int max, int lv)
		{
			return min + (max - min) * lv / 99;
		}



		/// <summary>
		/// 各装備カテゴリにおける制空値の熟練度ボーナス
		/// </summary>
		private static readonly Dictionary<EquipmentTypes, int[]> AircraftLevelBonus = new Dictionary<EquipmentTypes, int[]>() {
			{ EquipmentTypes.CarrierBasedFighter,    new int[] { 0, 0, 2, 5, 9, 14, 14, 22, 22 } },
			{ EquipmentTypes.CarrierBasedBomber,     new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
			{ EquipmentTypes.CarrierBasedTorpedo,    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
			{ EquipmentTypes.SeaplaneBomber,         new int[] { 0, 1, 1, 1, 1, 3, 3, 6, 6 } },
			{ EquipmentTypes.SeaplaneFighter,        new int[] { 0, 0, 2, 5, 9, 14, 14, 22, 22 } },
			{ EquipmentTypes.LandBasedAttacker,      new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
			{ EquipmentTypes.Interceptor,            new int[] { 0, 0, 2, 5, 9, 14, 14, 22, 22 } },
			{ EquipmentTypes.JetFighter,             new int[] { 0, 0, 2, 5, 9, 14, 14, 22, 22 } },
			{ EquipmentTypes.JetBomber,              new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
			{ EquipmentTypes.JetTorpedo,             new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
		};

		/// <summary>
		/// 艦載機熟練度の内部値テーブル(仮)
		/// </summary>
		private static readonly List<int> AircraftExpTable = new List<int>() {
			0, 10, 25, 40, 55, 70, 85, 100, 120
		};

		/// <summary>
		/// 各装備カテゴリにおける制空値の改修ボーナス
		/// </summary>
		private static readonly Dictionary<EquipmentTypes, double> LevelBonus = new Dictionary<EquipmentTypes, double>() {
			{ EquipmentTypes.CarrierBasedFighter,   0.2 },
			{ EquipmentTypes.CarrierBasedBomber,    0.25 },
			{ EquipmentTypes.SeaplaneFighter,       0.2 },
			{ EquipmentTypes.Interceptor,           0.2 },
			{ EquipmentTypes.LandBasedAttacker,     0.5 },
		};



		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="equipmentID">装備ID。</param>
		/// <param name="count">搭載機数。</param>
		/// <param name="aircraftLevel">艦載機熟練度。既定値は 0 です。</param>
		/// <param name="level">改修レベル。既定値は 0 です。</param>
		/// <param name="baseAirCorpsActionKind">基地航空隊の状態。基地航空隊でなければ-1</param>
		/// <param name="isAircraftExpMaximum">艦載機の内部熟練度が当該レベルで最大値であるとして計算するか。falseなら最小値として計算します。</param>
		/// <returns></returns>
		public static int GetAirSuperiority(int equipmentID, int count, int aircraftLevel = 0, int level = 0, int baseAirCorpsActionKind = -1, bool isAircraftExpMaximum = false)
		{

			if (count <= 0)
				return 0;

			var eq = KCDatabase.Instance.MasterEquipments[equipmentID];
			if (eq == null)
				return 0;

			var category = eq.CategoryType;


			// 通常の艦隊の場合、偵察機等の制空値は計算しない
			if (baseAirCorpsActionKind == -1)
			{
				if (!AircraftLevelBonus.ContainsKey(category))
					return 0;
			}


			double levelBonus = LevelBonus.ContainsKey(category) ? LevelBonus[category] : 0;    // 改修レベル補正

			if (category == EquipmentTypes.LandBasedAttacker)
				levelBonus *= Math.Sqrt(level);
			else
				levelBonus *= level;


			double interceptorBonus = 0;    // 局地戦闘機の迎撃補正
			if (category == EquipmentTypes.Interceptor)
			{
				if (baseAirCorpsActionKind == 2)        // 防空の場合
					interceptorBonus = eq.Accuracy * 2 + eq.Evasion;
				else
					interceptorBonus = eq.Evasion * 1.5;
			}

			int aircraftExp;
			if (isAircraftExpMaximum)
			{
				if (aircraftLevel < 7)
					aircraftExp = AircraftExpTable[aircraftLevel + 1] - 1;
				else
					aircraftExp = AircraftExpTable.Last();
			}
			else
			{
				aircraftExp = AircraftExpTable[aircraftLevel];
			}

			return (int)((eq.AA + levelBonus + interceptorBonus) * Math.Sqrt(count)
				+ Math.Sqrt(aircraftExp / 10.0)
				+ (AircraftLevelBonus.ContainsKey(category) ? AircraftLevelBonus[category][aircraftLevel] : 0));
		}



		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="slot">装備スロット。</param>
		/// <param name="aircraft">搭載機数の配列。</param>
		public static int GetAirSuperiority(int[] slot, int[] aircraft)
		{
			return slot.Select((eq, i) => GetAirSuperiority(eq, aircraft[i])).Sum();
		}



		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="fleet">艦船IDの配列。</param>
		public static int GetAirSuperiority(int[] fleet)
		{
			return fleet.Select(id => KCDatabase.Instance.MasterShips[id]).Sum(ship => GetAirSuperiority(ship));
		}

		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="fleet">艦船IDの配列。</param>
		/// <param name="slot">各艦船の装備スロット。</param>
		public static int GetAirSuperiority(int[] fleet, int[][] slot)
		{

			int air = 0;
			int length = Math.Min(fleet.Length, slot.GetLength(0));

			for (int i = 0; i < length; i++)
			{
				ShipDataMaster ship = KCDatabase.Instance.MasterShips[fleet[i]];
				if (ship == null) continue;

				air += GetAirSuperiority(slot[i], ship.Aircraft.ToArray());

			}

			return air;
		}



		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="ship">対象の艦船。</param>
		public static int GetAirSuperiority(ShipData ship, bool isAircraftLevelMaximum = false)
		{

			if (ship == null) return 0;

			return ship.SlotInstance.Select((eq, i) => eq == null ? 0 :
			   GetAirSuperiority(eq.EquipmentID, ship.Aircraft[i], eq.AircraftLevel, eq.Level, -1, isAircraftLevelMaximum)).Sum();
		}

		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="ship">対象の艦船。</param>
		public static int GetAirSuperiority(ShipDataMaster ship)
		{
			if (ship?.DefaultSlot == null)
				return 0;
			return GetAirSuperiority(ship.DefaultSlot.ToArray(), ship.Aircraft.ToArray());
		}

		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		public static int GetAirSuperiority(FleetData fleet, bool isAircraftLevelMaximum = false)
		{
			if (fleet == null)
				return 0;
			return fleet.MembersWithoutEscaped.Select(ship => GetAirSuperiority(ship, isAircraftLevelMaximum)).Sum();
		}


		/// <summary>
		/// 基地航空隊の制空戦力を求めます。
		/// </summary>
		/// <param name="aircorps">対象の基地航空隊。</param>
		public static int GetAirSuperiority(BaseAirCorpsData aircorps, bool isAircraftLevelMaximum = false, bool isHighAltitude = false)
		{
			if (aircorps == null)
				return 0;

			int air = 0;
			double reconBonus = 1.0;

			foreach (var sq in aircorps.Squadrons.Values)
			{
				if (sq == null || sq.State != 1)
					continue;

				air += GetAirSuperiority(sq, aircorps.ActionKind, isAircraftLevelMaximum);


				// 偵察機補正計算
				switch (aircorps.ActionKind)
				{
					case 1:     // 出撃
						reconBonus = Math.Max(reconBonus, GetAirSuperioritySortieReconBonus(sq.EquipmentID));
						break;
					case 2:     // 防空
						reconBonus = Math.Max(reconBonus, GetAirSuperiorityAirDefenseReconBonus(sq.EquipmentID));
						break;
				}
			}

			double highAltitudeBonus = 1.0;
			if (isHighAltitude)
			{
				int highAltitudeFigherCount = KCDatabase.Instance.BaseAirCorps.Values
					.Where(corps => corps.MapAreaID == aircorps.MapAreaID && corps.ActionKind == aircorps.ActionKind)
					.SelectMany(corps => corps.Squadrons.Values)
					.Count(sq => sq?.State == 1 && sq.EquipmentInstanceMaster.IsHightAltitudeFighter);
				highAltitudeBonus = Math.Min(0.5 + 0.3 * highAltitudeFigherCount, 1.2);
			}

			return (int)(air * reconBonus * highAltitudeBonus);
		}


		/// <summary>
		/// 基地航空隊での出撃時における、偵察機による制空値ボーナス係数を求めます。
		/// </summary>
		public static double GetAirSuperioritySortieReconBonus(int equipmentID)
		{
			var eq = KCDatabase.Instance.MasterEquipments[equipmentID];
			if (eq == null) return 1;

			var category = eq.CategoryType;
			int losrate = Math.Min(Math.Max(eq.LOS - 7, 0), 2);     // ~7, 8, 9~

			switch (category)
			{
				case EquipmentTypes.LandBasedRecon:
					return 1.12 + losrate * 0.03;         // los=8 => 1.15, los=9 => 1.18

				default:
					return 1;
			}
		}

		/// <summary>
		/// 基地航空隊での防空戦における、偵察機による制空値ボーナス係数を求めます。
		/// </summary>
		public static double GetAirSuperiorityAirDefenseReconBonus(int equipmentID)
		{
			var eq = KCDatabase.Instance.MasterEquipments[equipmentID];
			if (eq == null) return 1;

			var category = eq.CategoryType;
			int losrate = Math.Min(Math.Max(eq.LOS - 7, 0), 2);     // ~7, 8, 9~

			switch (category)
			{
				case EquipmentTypes.SeaplaneRecon:
				case EquipmentTypes.FlyingBoat:
					return 1.1 + losrate * 0.03;

				case EquipmentTypes.LandBasedRecon:
					return 1.12 + losrate * 0.06;           // los=8 => 1.18, los=9 => 1.24

				case EquipmentTypes.CarrierBasedRecon:
				case EquipmentTypes.JetRecon:
					return 1.2 + losrate * 0.05;

				default:
					return 1;
			}
		}

		/// <summary>
		/// 基地航空中隊の制空戦力を求めます。
		/// </summary>
		/// <param name="squadron">対象の基地航空中隊。</param>
		public static int GetAirSuperiority(BaseAirCorpsSquadron squadron, int actionKind, bool isAircraftLevelMaximum = false)
		{
			if (squadron == null || squadron.State != 1)
				return 0;

			var eq = squadron.EquipmentInstance;
			if (eq == null)
				return 0;

			return GetAirSuperiority(eq.EquipmentID, squadron.AircraftCurrent, eq.AircraftLevel, eq.Level, actionKind, isAircraftLevelMaximum);
		}


		/// <summary>
		/// 最大練度の艦載機を搭載している場合の制空戦力を求めます。
		/// </summary>
		/// <param name="fleet">艦船IDリスト。</param>
		/// <param name="slot">各艦の装備IDリスト。</param>
		/// <returns></returns>
		public static int GetAirSuperiorityAtMaxLevel(int[] fleet, int[][] slot)
		{
			return fleet.Select(id => KCDatabase.Instance.MasterShips[id])
				.Select((ship, i) => ship == null ? 0 :
				   slot[i].Select((eqid, k) => GetAirSuperiority(eqid, ship.Aircraft[k], 7, 10, -1, true)).Sum()).Sum();
		}


		/// <summary>
		/// 艦載機熟練度・改修レベルを無視した制空戦力を求めます。
		/// </summary>
		/// <param name="ship">対象の艦船。</param>
		public static int GetAirSuperiorityIgnoreLevel(ShipData ship)
		{
			if (ship == null)
				return 0;
			return GetAirSuperiority(ship.SlotMaster.ToArray(), ship.Aircraft.ToArray());
		}

		/// <summary>
		/// 艦載機熟練度・改修レベルを無視した制空戦力を求めます。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		public static int GetAirSuperiorityIgnoreLevel(FleetData fleet)
		{
			if (fleet == null)
				return 0;
			return fleet.MembersWithoutEscaped.Select(ship => GetAirSuperiorityIgnoreLevel(ship)).Sum();
		}



		/// <summary>
		/// 索敵能力を求めます。「新判定式(33)」です。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		/// <param name="branchWeight">分岐点係数。2-5では1</param>
		public static double GetSearchingAbility_New33(FleetData fleet, int branchWeight)
		{

			double ret = 0;

			foreach (var ship in fleet.MembersWithoutEscaped)
			{
				if (ship == null)
				{
					ret += 2.0;
					continue;
				}

				// 装備シナジーがあるので合計値から引いていく
				int losBase = ship.LOSTotal;

				double equipmentBonus = 0;
				foreach (var eq in ship.AllSlotInstance.Where(eq => eq != null))
				{
					var category = eq.MasterEquipment.CategoryType;

					losBase -= eq.MasterEquipment.LOS;
					if (eq.EquipmentID == 315)                      // SG レーダー(初期型) だけ装備シナジー補正が乗らないので :(
					{
						switch (ship.MasterShip.ShipClass)
						{
							case 65:        // Iowa級
							case 69:        // Lexington級
							case 83:        // Casablanca級
							case 84:        // Essex級
							case 87:        // John C.Butler級
							case 91:        // Fletcher級
							case 93:        // Colorado級
							case 95:        // Northampton級
							case 99:        // Atlanta級
							case 102:       // South Dakota級
							case 105:       // Yorktown級
								losBase -= 4;
								break;
						}
					}

					double equipmentRate;
					switch (category)
					{
						case EquipmentTypes.CarrierBasedTorpedo:
						case EquipmentTypes.JetTorpedo:
							equipmentRate = 0.8;
							break;

						case EquipmentTypes.CarrierBasedRecon:
						case EquipmentTypes.JetRecon:
							equipmentRate = 1.0;
							break;

						case EquipmentTypes.SeaplaneRecon:
							equipmentRate = 1.2;
							break;

						case EquipmentTypes.SeaplaneBomber:
							equipmentRate = 1.1;
							break;

						default:
							equipmentRate = 0.6;
							break;
					}

					double levelRate;
					switch (category)
					{
						case EquipmentTypes.SeaplaneRecon:
						case EquipmentTypes.CarrierBasedRecon:
							levelRate = 1.2;
							break;

						case EquipmentTypes.SeaplaneBomber:
							levelRate = 1.15;
							break;

						case EquipmentTypes.RadarSmall:
							levelRate = 1.25;
							break;

						case EquipmentTypes.RadarLarge:
							levelRate = 1.4;
							break;

						default:
							levelRate = 0;
							break;
					}

					equipmentBonus += equipmentRate * (eq.MasterEquipment.LOS + levelRate * Math.Sqrt(eq.Level));
				}

				ret += Math.Sqrt(losBase) + equipmentBonus * branchWeight;
			}

			// 司令部Lv補正
			ret -= Math.Ceiling(KCDatabase.Instance.Admiral.Level * 0.4);

			return ret;
		}


		/// <summary>
		/// 艦隊の触接開始率を求めます。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		public static double GetContactProbability(FleetData fleet)
		{

			double successProb = 0.0;

			foreach (var ship in fleet.MembersWithoutEscaped)
			{
				if (ship == null) continue;

				var eqs = ship.SlotInstanceMaster;

				for (int i = 0; i < ship.Slot.Count; i++)
				{
					if (eqs[i] == null)
						continue;

					switch (eqs[i].CategoryType)
					{
						case EquipmentTypes.CarrierBasedRecon:
						case EquipmentTypes.SeaplaneRecon:
						case EquipmentTypes.FlyingBoat:
						case EquipmentTypes.JetRecon:
							successProb += 0.04 * eqs[i].LOS * Math.Sqrt(ship.Aircraft[i]);
							break;
					}

				}
			}

			return successProb;
		}

		/// <summary>
		/// 機体命中率別の触接選択率を求めます。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		/// <returns>機体の命中をキー, 触接選択率を値とした Dictionary 。</returns>
		public static Dictionary<int, double> GetContactSelectionProbability(FleetData fleet)
		{

			var probs = new Dictionary<int, double>();

			foreach (var ship in fleet.MembersWithoutEscaped)
			{
				if (ship == null)
					continue;

				foreach (var eq in ship.SlotInstanceMaster)
				{
					if (eq == null)
						continue;

					switch (eq.CategoryType)
					{
						case EquipmentTypes.CarrierBasedTorpedo:
						case EquipmentTypes.CarrierBasedRecon:
						case EquipmentTypes.SeaplaneRecon:
						case EquipmentTypes.FlyingBoat:
						case EquipmentTypes.JetTorpedo:
						case EquipmentTypes.JetRecon:
							if (!probs.ContainsKey(eq.Accuracy))
								probs.Add(eq.Accuracy, 1.0);

							probs[eq.Accuracy] *= 1.0 - (0.07 * eq.LOS);
							break;
					}
				}
			}

			foreach (int key in probs.Keys.ToArray())
			{       //列挙中の変更エラーを防ぐため 
				probs[key] = 1.0 - probs[key];
			}

			return probs;
		}


		/// <summary>
		/// 輸送作戦成功時の輸送量(減少TP)を求めます。
		/// (S勝利時のもの。A勝利時は int( value * 0.7 ) )
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		/// <returns>減少TP。</returns>
		public static int GetTPDamage(FleetData fleet)
		{

			int tp = 0;

			foreach (var ship in fleet.MembersWithoutEscaped.Where(s => s != null && s.HPRate > 0.25))
			{

				// 装備ボーナス
				foreach (var eq in ship.AllSlotInstanceMaster.Where(q => q != null))
				{

					switch (eq.CategoryType)
					{

						case EquipmentTypes.LandingCraft:
							//if ( eq.EquipmentID == 166 )	// 陸戦隊
							//	tp += 13;
							//else
							tp += 8;
							break;

						case EquipmentTypes.TransportContainer:
							tp += 5;
							break;

						case EquipmentTypes.Ration:
							tp += 1;
							break;

						case EquipmentTypes.SpecialAmphibiousTank:
							tp += 2;
							break;
					}
				}


				// 艦種ボーナス
				switch (ship.MasterShip.ShipType)
				{

					case ShipTypes.Destroyer:
						tp += 5;
						break;

					case ShipTypes.LightCruiser:
						tp += 2;
						if (ship.ShipID == 487) // 鬼怒改二
							tp += 8;
						break;

					case ShipTypes.AviationCruiser:
						tp += 4;
						break;

					case ShipTypes.AviationBattleship:
						tp += 7;
						break;

					case ShipTypes.SeaplaneTender:
						tp += 9;
						break;

					case ShipTypes.AmphibiousAssaultShip:
						tp += 12;
						break;

					case ShipTypes.SubmarineTender:
						tp += 7;
						break;

					case ShipTypes.TrainingCruiser:
						tp += 6;
						break;

					case ShipTypes.FleetOiler:
						tp += 15;
						break;
				}
			}


			return tp;
		}


		private static readonly Dictionary<int, double> EquipmentExpeditionBonus = new Dictionary<int, double>() {
			{ 68, 0.05 },	// 大発動艇
			{ 166, 0.02 },	// 大発戦車
			{ 167, 0.01 },	// 内火艇
			{ 193, 0.05 },	// 特大発動艇
		};
		/// <summary>
		/// 遠征資源の大発ボーナスを取得します。
		/// </summary>
		public static double GetExpeditionBonus(FleetData fleet)
		{
			var eqs = fleet.MembersInstance
				.Where(s => s != null)
				.SelectMany(s => s.SlotInstance)
				.Where(eq => eq != null && EquipmentExpeditionBonus.ContainsKey(eq.EquipmentID));

			double normalBonus = eqs.Sum(eq => EquipmentExpeditionBonus[eq.EquipmentID])
				+ fleet.MembersInstance.Count(s => s != null && s.ShipID == 487) * 0.05;        // 鬼怒改二

			normalBonus = Math.Min(normalBonus, 0.2);
			double levelBonus = eqs.Any() ? (0.01 * normalBonus * eqs.Average(eq => eq.Level)) : 0;

			int tokuCount = eqs.Count(eq => eq.EquipmentID == 193);
			int daihatsuCount = eqs.Count(eq => eq.EquipmentID == 68);
			double tokuBonus;

			if (tokuCount <= 2)
				tokuBonus = 0.02 * tokuCount;
			else if (tokuCount == 3)
				tokuBonus = 0.05 + 0.002 * Math.Min(Math.Max(daihatsuCount - 1, 0), 2);
			else
			{
				if (daihatsuCount <= 2)
					tokuBonus = 0.054 + 0.002 * daihatsuCount;
				else if (daihatsuCount == 3)
					tokuBonus = 0.059;
				else
					tokuBonus = 0.060;
			}

			// 厳密には tokuBonus は別の補正として扱われるが気にしないことにする

			return normalBonus + levelBonus + tokuBonus;
		}



		/// <summary>
		/// ハードスキン型陸上基地の名前リスト
		/// IDではなく名前なのは本家の処理に倣ったため
		/// </summary>
		private static readonly HashSet<string> HardInstallationNames = new HashSet<string>() {
			"離島棲姫",
			"砲台小鬼",
			"集積地棲姫",
			"集積地棲姫-壊",
		};



		/// <summary>
		/// 昼戦における攻撃種別を取得します。
		/// </summary>
		/// <param name="slot">攻撃艦のスロット(マスターID)。</param>
		/// <param name="attackerShipID">攻撃艦の艦船ID。</param>
		/// <param name="defenderShipID">防御艦の艦船ID。なければ-1</param>
		/// <param name="includeSpecialAttack">弾着観測砲撃を含むか。falseなら除外して計算</param>
		public static DayAttackKind GetDayAttackKind(int[] slot, int attackerShipID, int defenderShipID, bool includeSpecialAttack = true)
		{

			int reconCount = 0;
			int mainGunCount = 0;
			int subGunCount = 0;
			int apShellCount = 0;
			int radarCount = 0;
			int rocketCount = 0;
			int attackerCount = 0;
			int bomberCount = 0;
			int suisei634Count = 0;
			int zuiunCount = 0;

			if (slot == null)
				return DayAttackKind.Unknown;


			var slotmaster = slot.Select(s => KCDatabase.Instance.MasterEquipments[s]).Where(eq => eq != null).ToArray();


			foreach (var eq in slotmaster)
			{
				switch (eq.CategoryType)
				{
					case EquipmentTypes.MainGunSmall:
					case EquipmentTypes.MainGunMedium:
					case EquipmentTypes.MainGunLarge:
						mainGunCount++;
						break;

					case EquipmentTypes.SecondaryGun:
						subGunCount++;
						break;

					case EquipmentTypes.CarrierBasedBomber:
						bomberCount++;
						if (eq.Name.Contains("六三四空"))
							suisei634Count++;
						break;

					case EquipmentTypes.CarrierBasedTorpedo:
						attackerCount++;
						break;

					case EquipmentTypes.SeaplaneRecon:
					case EquipmentTypes.SeaplaneBomber:
						reconCount++;
						if (eq.Name.Contains("瑞雲"))
							zuiunCount++;
						break;

					case EquipmentTypes.RadarSmall:
					case EquipmentTypes.RadarLarge:
						radarCount++;
						break;

					case EquipmentTypes.APShell:
						apShellCount++;
						break;

					case EquipmentTypes.Rocket:
						rocketCount++;
						break;

				}
			}

			ShipDataMaster attacker = KCDatabase.Instance.MasterShips[attackerShipID];
			ShipDataMaster defender = KCDatabase.Instance.MasterShips[defenderShipID];

			if (includeSpecialAttack)
			{
				if (attackerShipID == 553 || attackerShipID == 554) // 伊勢改二・日向改二
				{
					if (mainGunCount >= 1 && zuiunCount >= 2)
						return DayAttackKind.ZuiunMultiAngle;

					else if (mainGunCount >= 1 && suisei634Count >= 2)
						return DayAttackKind.SeaAirMultiAngle;
				}

				if (reconCount > 0)
				{
					if (mainGunCount == 2 && apShellCount == 1)
						return DayAttackKind.CutinMainMain;

					else if (mainGunCount == 1 && subGunCount == 1 && apShellCount == 1)
						return DayAttackKind.CutinMainAP;

					else if (mainGunCount == 1 && subGunCount == 1 && radarCount == 1)
						return DayAttackKind.CutinMainRadar;

					else if (mainGunCount >= 1 && subGunCount >= 1)
						return DayAttackKind.CutinMainSub;

					else if (mainGunCount >= 2)
						return DayAttackKind.DoubleShelling;
				}

				if (bomberCount > 0 && attackerCount > 0)
					return DayAttackKind.CutinAirAttack;
			}


			if (attacker != null)
			{

				if (defender != null)
				{

					int landingID = GetLandingAttackKind(slot, attacker, defender);
					if (landingID > 0)
					{
						return (DayAttackKind)((int)DayAttackKind.LandingDaihatsu + landingID - 1);
					}

					if (rocketCount > 0 && defender.IsLandBase)
						return DayAttackKind.Rocket;
				}


				if (attackerShipID == 352)
				{   //速吸改

					if (defender != null && (defender.IsSubmarine))
					{
						// 対潜攻撃において、( 対潜 > 0 の艦上攻撃機 or 水上爆撃機 or オートジャイロ ) を装備している場合は空撃
						if (slotmaster.Any(eq =>
								(eq.CategoryType == EquipmentTypes.CarrierBasedTorpedo && eq.ASW > 0) ||
								eq.CategoryType == EquipmentTypes.SeaplaneBomber ||
								eq.CategoryType == EquipmentTypes.Autogyro))
							return DayAttackKind.AirAttack;
						else
							return DayAttackKind.DepthCharge;

					}
					else if (slotmaster.Any(eq => eq.CategoryType == EquipmentTypes.CarrierBasedTorpedo))
						return DayAttackKind.AirAttack;
					else
						return DayAttackKind.Shelling;

				}
				else if (attacker.IsAircraftCarrier)
				{
					return DayAttackKind.AirAttack;
				}
				else if (defender != null && defender.IsSubmarine)
				{
					switch (attacker.ShipType)
					{
						case ShipTypes.AviationCruiser:
						case ShipTypes.AviationBattleship:
						case ShipTypes.SeaplaneTender:
						case ShipTypes.AmphibiousAssaultShip:
							return DayAttackKind.AirAttack;

						default:
							return DayAttackKind.DepthCharge;
					}
				}
			}

			return DayAttackKind.Shelling;      //砲撃
		}


		/// <summary>
		/// 昼戦空母カットインの種別を取得します。
		/// </summary>
		public static DayAirAttackCutinKind GetDayAirAttackCutinKind(IEnumerable<EquipmentDataMaster> slot)
		{
			// note: 優先度は分からないが、とりあえず威力の高いものを優先して返す

			int fighterCount = 0;
			int bomberCount = 0;
			int torpedoCount = 0;

			foreach (var eq in slot.Where(s => s != null))
			{
				switch (eq.CategoryType)
				{
					case EquipmentTypes.CarrierBasedFighter:
						fighterCount++;
						break;

					case EquipmentTypes.CarrierBasedBomber:
						bomberCount++;
						break;

					case EquipmentTypes.CarrierBasedTorpedo:
						torpedoCount++;
						break;
				}
			}

			if (fighterCount >= 1 && bomberCount >= 1 && torpedoCount >= 1)
				return DayAirAttackCutinKind.FighterBomberAttacker;

			if (bomberCount >= 2 && torpedoCount >= 1)
				return DayAirAttackCutinKind.BomberBomberAttacker;

			if (bomberCount == 1 && torpedoCount >= 1)
				return DayAirAttackCutinKind.BomberAttacker;


			return DayAirAttackCutinKind.None;
		}


		/// <summary>
		/// 夜戦における攻撃種別を取得します。
		/// </summary>
		/// <param name="slot">攻撃艦のスロット(マスターID)。</param>
		/// <param name="attackerShipID">攻撃艦の艦船ID。</param>
		/// <param name="defenderShipID">防御艦の艦船ID。なければ-1</param>
		/// <param name="includeSpecialAttack">カットイン/連撃の判定を含むか。falseなら除外して計算</param>
		/// <param name="nightAirAttackFlag">夜戦空母攻撃フラグ</param>
		public static NightAttackKind GetNightAttackKind(int[] slot, int attackerShipID, int defenderShipID, bool includeSpecialAttack = true, bool nightAirAttackFlag = false)
		{

			int mainGunCount = 0;
			int subGunCount = 0;
			int torpedoCount = 0;
			int rocketCount = 0;
			int lateModelTorpedoCount = 0;
			int submarineEquipmentCount = 0;
			int nightFighterCount = 0;
			int nightAttackerCount = 0;
			int swordfishCount = 0;
			int nightCapableBomberCount = 0;
			int nightBomberCount = 0;
			int nightPersonnelCount = 0;
			int surfaceRadarCount = 0;
			int picketCrewCount = 0;

			if (slot == null)
				return NightAttackKind.Unknown;


			ShipDataMaster attacker = KCDatabase.Instance.MasterShips[attackerShipID];
			ShipDataMaster defender = KCDatabase.Instance.MasterShips[defenderShipID];


			var slotmaster = slot.Select(id => KCDatabase.Instance.MasterEquipments[id]).Where(eq => eq != null).ToArray();

			foreach (var eq in slotmaster)
			{
				switch (eq.CategoryType)
				{
					// 主砲系
					case EquipmentTypes.MainGunSmall:
					case EquipmentTypes.MainGunMedium:
					case EquipmentTypes.MainGunLarge:
					case EquipmentTypes.MainGunLarge2:
						mainGunCount++;
						break;

					// 副砲
					case EquipmentTypes.SecondaryGun:
						subGunCount++;
						break;

					// 魚雷系
					case EquipmentTypes.Torpedo:
					case EquipmentTypes.SubmarineTorpedo:
						torpedoCount++;

						if (eq.IsLateModelTorpedo)
							lateModelTorpedoCount++;
						break;

					// 夜間戦闘機
					case EquipmentTypes.CarrierBasedFighter:
						if (eq.IsNightFighter)
							nightFighterCount++;
						break;

					// (夜間)爆撃機
					case EquipmentTypes.CarrierBasedBomber:
						if (eq.EquipmentID == 154)      // 零戦62型(爆戦/岩井隊)
							nightCapableBomberCount++;
						else if (eq.EquipmentID == 320) // 彗星一二型(三一号光電管爆弾搭載機)
							nightBomberCount++;
						break;

					// 夜間攻撃機
					case EquipmentTypes.CarrierBasedTorpedo:
						if (eq.IsNightAttacker)
							nightAttackerCount++;

						if (eq.IsSwordfish)
							swordfishCount++;
						break;

					// 電探
					case EquipmentTypes.RadarSmall:
					case EquipmentTypes.RadarLarge:
						if (eq.IsSurfaceRadar)
							surfaceRadarCount++;
						break;

					// 見張員
					case EquipmentTypes.SurfaceShipPersonnel:
						picketCrewCount++;
						break;

					// 夜間作戦航空要員
					case EquipmentTypes.AviationPersonnel:
						if (eq.IsNightAviationPersonnel)
							nightPersonnelCount++;
						break;

					// 対地装備
					case EquipmentTypes.Rocket:
						rocketCount++;
						break;

					// 潜水艦装備
					case EquipmentTypes.SubmarineEquipment:
						submarineEquipmentCount++;
						break;
				}

			}

			if (attackerShipID == 545 || attackerShipID == 599 || attackerShipID == 610)      // Saratoga Mk.II/赤城改二戊/加賀改二戊
				nightPersonnelCount++;


			if (includeSpecialAttack)
			{

				// 駆逐艦カットイン
				if (attacker?.ShipType == ShipTypes.Destroyer)
				{
					if (mainGunCount >= 1 && torpedoCount >= 1 && surfaceRadarCount >= 1)
						return NightAttackKind.CutinTorpedoRadar;
					if (torpedoCount >= 1 && surfaceRadarCount >= 1 && picketCrewCount >= 1)
						return NightAttackKind.CutinTorpedoPicket;
				}

				// 潜水艦カットイン
				if (torpedoCount >= 2 || (lateModelTorpedoCount >= 1 && submarineEquipmentCount >= 1))
					return NightAttackKind.CutinTorpedoTorpedo;

				// 汎用カットイン
				else if (mainGunCount >= 3)
					return NightAttackKind.CutinMainMain;

				else if (mainGunCount == 2 && subGunCount > 0)
					return NightAttackKind.CutinMainSub;

				else if ((mainGunCount == 2 && subGunCount == 0 && torpedoCount == 1) || (mainGunCount == 1 && torpedoCount == 1))
					return NightAttackKind.CutinMainTorpedo;

				else if ((mainGunCount == 2 && subGunCount == 0 & torpedoCount == 0) ||
					(mainGunCount == 1 && subGunCount > 0) ||
					(subGunCount >= 2 && torpedoCount <= 1))
					return NightAttackKind.DoubleShelling;

				// 空母カットイン
				if (nightPersonnelCount > 0)
				{
					if (nightFighterCount > 0 &&
						(nightAttackerCount > 0 || (nightFighterCount + swordfishCount + nightCapableBomberCount) >= 3))
						return NightAttackKind.CutinAirAttack;
					else if (nightBomberCount > 0 && (nightFighterCount + nightAttackerCount > 0))
						return NightAttackKind.CutinAirAttack;
				}

				if (nightPersonnelCount > 0)
				{
					if (attackerShipID == 515 || attackerShipID == 393)     // Ark Royal(改)
						if (swordfishCount > 0)
							nightAirAttackFlag = true;

					if (nightFighterCount > 0 || nightAttackerCount > 0)
						nightAirAttackFlag = true;
				}
			}


			if (attacker != null)
			{

				// 対地攻撃系
				if (defender != null)
				{

					int landingID = GetLandingAttackKind(slot, attacker, defender);
					if (landingID > 0)
					{
						return (NightAttackKind)((int)NightAttackKind.LandingDaihatsu + landingID - 1);
					}

					if (rocketCount > 0 && defender.IsLandBase)
						return NightAttackKind.Rocket;
				}

				if (nightAirAttackFlag)
					return NightAttackKind.AirAttack;

				if (attacker.ShipType == ShipTypes.LightAircraftCarrier && defender != null && defender.IsSubmarine)
					return NightAttackKind.DepthCharge;

				if (attacker.IsAircraftCarrier)
				{

					if (attackerShipID == 432 || attackerShipID == 353 || attackerShipID == 433)        // Graf Zeppelin(改), Saratoga
						return NightAttackKind.Shelling;
					else if (attacker.Name == "リコリス棲姫" || attacker.Name == "深海海月姫")
						return NightAttackKind.Shelling;
					else
						return NightAttackKind.AirAttack;

				}
				else if (attacker.IsSubmarine)
				{
					return NightAttackKind.Torpedo;
				}
				else if (defender != null && defender.IsSubmarine)
				{
					switch (attacker.ShipType)
					{
						case ShipTypes.AviationCruiser:
						case ShipTypes.AviationBattleship:
						case ShipTypes.SeaplaneTender:
						case ShipTypes.AmphibiousAssaultShip:
							return NightAttackKind.AirAttack;

						default:
							return NightAttackKind.DepthCharge;
					}
				}
				else if (slot.Length > 0)
				{
					foreach (var eq in slotmaster)
					{
						if (eq.IsGun)
							return NightAttackKind.Shelling;
						if (eq.IsTorpedo)
							return NightAttackKind.Torpedo;
					}
				}

			}

			return NightAttackKind.Shelling;

		}


		/// <summary>
		/// 夜戦カットインにおける魚雷カットインの種別を取得します。
		/// </summary>
		/// <param name="slot">攻撃艦のスロット。</param>
		/// <param name="attackerShipID">攻撃艦の艦船ID。</param>
		/// <param name="defenerShipID">防御艦の艦船ID。なければ-1</param>
		/// <returns> 0=その他, 1=後期魚雷+潜水艦装備(x1.75), 2=後期魚雷x2(x1.6)</returns>
		public static NightTorpedoCutinKind GetNightTorpedoCutinKind(IEnumerable<EquipmentDataMaster> slot, int attackerShipID, int defenderShipID)
		{
			slot = slot.Where(eq => eq != null);

			// note: 発動優先度については要検証
			int latetorp = slot.Count(eq => eq.IsLateModelTorpedo);
			int subeq = slot.Count(eq => eq.CategoryType == EquipmentTypes.SubmarineEquipment);

			if (latetorp >= 1 && subeq >= 1)
				return NightTorpedoCutinKind.LateModelTorpedoSubmarineEquipment;       // x1.75
			else if (latetorp >= 2)
				return NightTorpedoCutinKind.LateModelTorpedo2;       // x1.6

			return NightTorpedoCutinKind.None;
		}



		/// <summary>
		/// 揚陸攻撃における攻撃種別を取得します。
		/// </summary>
		/// <param name="slot">攻撃艦のスロット(マスターID)。</param>
		/// <param name="attackerShipID">攻撃艦の艦船ID。</param>
		/// <param name="defenerShipID">防御艦の艦船ID。</param>
		public static int GetLandingAttackKind(IEnumerable<int> slot, ShipDataMaster attacker, ShipDataMaster defender)
		{

			if (defender == null)
				return 0;

			if (slot.Contains(230) && defender.IsLandBase)      // 特大発動艇+戦車第11連隊
				return 5;

			if (slot.Contains(167))
			{       // 特二式内火艇
				if (attacker.IsSubmarine)
				{       // 潜水系
					if (defender.IsLandBase)
						return 4;
				}
				else if (HardInstallationNames.Contains(defender.Name))
					return 4;
			}

			if (HardInstallationNames.Contains(defender.Name))
			{

				if (slot.Contains(166))     // 大発動艇(八九式中戦車&陸戦隊)
					return 3;

				if (slot.Contains(193))     // 特大発動艇
					return 2;

				if (slot.Contains(68))      // 大発動艇
					return 1;
			}

			return 0;
		}



		/// <summary>
		/// 対空カットイン種別を取得します。
		/// </summary>
		public static int GetAACutinKind(int shipID, int[] slot)
		{

			int highangle = 0;
			int highangle_director = 0;
			int director = 0;
			int radar = 0;
			int aaradar = 0;
			int maingunl = 0;
			int maingunl_fcr = 0;
			int aashell = 0;
			int aagun_total = 0;
			int aagun_medium = 0;
			int aagun_concentrated = 0;
			int aagun_pompom = 0;
			int aarocket_english = 0;
			int aarocket_mod = 0;
			int highangle_musashi = 0;
			int highangle_america = 0;
			int highangle_america_gfcs = 0;
			int radar_gfcs = 0;
			int highangle_atlanta = 0;
			int highangle_atlanta_gfcs = 0;

			var slotmaster = slot.Select(id => KCDatabase.Instance.MasterEquipments[id]).Where(eq => eq != null).ToArray();


			foreach (var eq in slotmaster)
			{
				if (eq.IsHighAngleGun)
				{
					highangle++;

					if (eq.IsHighAngleGunWithAADirector)
						highangle_director++;

					switch (eq.EquipmentID)
					{
						case 275:   // 10cm連装高角砲改+増設機銃
							highangle_musashi++;
							break;
						case 313:   // 5inch単装砲 Mk.30改
							highangle_america++;
							break;
						case 308:   // 5inch単装砲 Mk.30改+GFCS Mk.37
							highangle_america_gfcs++;
							break;
						case 362:   // 5inch連装両用砲(集中配備)
							highangle_atlanta++;
							break;
						case 363:   // GFCS Mk.37+5inch連装両用砲(集中配備)
							highangle_atlanta_gfcs++;
							break;
					}

				}
				else if (eq.CategoryType == EquipmentTypes.AADirector)
				{
					director++;
				}
				else if (eq.IsRadar)
				{
					radar++;

					if (eq.IsAirRadar)
						aaradar++;

					if (eq.EquipmentID == 307)   // GFCS Mk.37
						radar_gfcs++;
				}
				else if (eq.CategoryType == EquipmentTypes.MainGunLarge || eq.CategoryType == EquipmentTypes.MainGunLarge2)
				{
					maingunl++;

					if (eq.EquipmentID == 300)       // 16inch Mk.I三連装砲改+FCR type284
						maingunl_fcr++;
				}
				else if (eq.CategoryType == EquipmentTypes.AAShell)
				{
					aashell++;
				}
				else if (eq.CategoryType == EquipmentTypes.AAGun)
				{
					aagun_total++;

					if (eq.EquipmentID == 274)      // 12cm30連装噴進砲改二
						aarocket_mod++;
					if (eq.EquipmentID == 191)      // QF 2ポンド8連装ポンポン砲
						aagun_pompom++;
					if (eq.EquipmentID == 301)      // 20連装7inch UP Rocket Launchers
						aarocket_english++;

					if (eq.IsConcentratedAAGun)
						aagun_concentrated++;
					else if (eq.AA >= 3)
						aagun_medium++;
				}

			}



			// 固有カットイン

			switch (KCDatabase.Instance.MasterShips[shipID]?.ShipClass)
			{
				case 54:    // 秋月型
					if (highangle >= 2 && radar >= 1)
					{
						return 1;
					}
					if (highangle >= 1 && radar >= 1)
					{
						return 2;
					}
					if (highangle >= 2)
					{
						return 3;
					}
					break;

				case 91:    // Fletcher級
					if (highangle_america_gfcs >= 2)
						return 34;
					if (highangle_america_gfcs >= 1 && highangle_america >= 1)
						return 35;
					if (highangle_america >= 2)
					{
						if (radar_gfcs >= 1)
							return 36;
						return 37;
					}
					break;
			}


			switch (shipID)
			{

				case 428:   // 摩耶改二
					if (highangle >= 1 && aagun_concentrated >= 1)
					{
						if (aaradar >= 1)
							return 10;

						return 11;
					}
					break;

				case 141:   // 五十鈴改二
					if (highangle >= 1 && aagun_total >= 1)
					{
						if (aaradar >= 1)
							return 14;
						else
							return 15;
					}
					break;

				case 470:   // 霞改二乙
				case 622:   // 夕張改二
					if (highangle >= 1 && aagun_total >= 1)
					{
						if (aaradar >= 1)
							return 16;
						else
							return 17;
					}
					break;

				case 418:   // 皐月改二
					if (aagun_concentrated >= 1)
						return 18;
					break;

				case 487:   // 鬼怒改二
					if (aagun_concentrated >= 1)
					{
						if (highangle - highangle_director >= 1)
							return 19;
						return 20;
					}
					break;

				case 488:   // 由良改二
					if (highangle >= 1 && aaradar >= 1)
						return 21;
					break;

				case 548:   // 文月改二
					if (aagun_concentrated >= 1)
						return 22;
					break;

				case 539:   // UIT-25
				case 530:   // 伊504
					if (aagun_medium >= 1)
						return 23;
					break;

				case 477:   // 天龍改二
					if (highangle >= 3)
						return 30;
					if (highangle >= 2)
						return 31;
					break;

				case 478:   // 龍田改二
					if (highangle >= 1 && aagun_medium >= 1)
						return 24;
					break;

				case 82:    // 伊勢改
				case 88:    // 日向改
				case 553:   // 伊勢改二
				case 554:   // 日向改二
					if (aarocket_mod >= 1 && aaradar >= 1)
					{
						if (aashell >= 1)
							return 25;

						return 28;
					}
					break;

				case 148:   // 武蔵改
					if (aarocket_mod >= 1 && aaradar >= 1)
						return 28;
					break;

				case 546:   // 武蔵改二
					if (highangle_musashi >= 1 && aaradar >= 1)
						return 26;
					if (aarocket_mod >= 1 && aaradar >= 1)
						return 28;
					break;

				case 557:   // 磯風乙改
				case 558:   // 浜風乙改
					if (highangle >= 1 && aaradar >= 1)
						return 29;
					break;

				case 149:   // 金剛改二
				case 591:   // 金剛改二丙
				case 150:   // 比叡改二
				case 592:   // 比叡改二丙
				case 151:   // 榛名改二
				case 152:   // 霧島改二
				case 519:   // Jervis
				case 394:   // Jervis改
				case 571:   // Nelson
				case 576:   // Nelson改
				case 439:   // Warspite
				case 364:   // Warspite改
				case 515:   // Ark Royal
				case 393:   // Ark Royal改
				case 520:   // Janus
				case 893:   // Janus改
					if (aarocket_english >= 2)
						return 32;
					if (aagun_pompom >= 1 && (maingunl_fcr >= 1 || aarocket_english >= 1))
						return 32;
					break;

				case 579:   // Gotland改
				case 630:   // Gotland andra
					if (highangle >= 3)
						return 30;
					if (highangle >= 1 && aagun_medium >= 1)
						return 33;
					break;

				case 597:   // Atlanta
				case 696:   // Atlanta改
					if (highangle_atlanta_gfcs >= 1 && highangle_atlanta >= 1)
						return 39;
					if (highangle_atlanta_gfcs + highangle_atlanta >= 2)
					{
						if (radar_gfcs >= 1)
							return 40;
						return 41;
					}
					break;
			}



			if (maingunl >= 1 && aashell >= 1 && director >= 1 && aaradar >= 1)
			{
				return 4;
			}
			if (highangle_director >= 2 && aaradar >= 1)
			{
				return 5;
			}
			if (maingunl >= 1 && aashell >= 1 && director >= 1)
			{
				return 6;
			}
			if (highangle >= 1 && director >= 1 && aaradar >= 1)
			{
				return 7;
			}
			if (highangle_director >= 1 && aaradar >= 1)
			{
				return 8;
			}
			if (highangle >= 1 && director >= 1)
			{
				return 9;
			}

			if (aagun_concentrated >= 1 && (aagun_concentrated + aagun_medium) >= 2 && aaradar >= 1)
			{
				return 12;
			}

			return 0;
		}



		/// <summary>
		/// 加重対空値を求めます。
		/// </summary>
		public static double GetAdjustedAAValue(ShipData ship)
		{
			int equippedModifier = ship.SlotInstance.Any(s => s != null) ? 2 : 1;

			double x = ship.AABase;

			foreach (var eq in ship.AllSlotInstance)
			{
				if (eq == null)
					continue;

				var eqmaster = eq.MasterEquipment;

				double equipmentBonus;
				if (eqmaster.IsHighAngleGun || eqmaster.CategoryType == EquipmentTypes.AADirector)
					equipmentBonus = 4;

				else if (eqmaster.CategoryType == EquipmentTypes.AAGun)
					equipmentBonus = 6;

				else if (eqmaster.IsRadar)
					equipmentBonus = 3;

				else
					equipmentBonus = 0;


				double levelBonus;
				if (eqmaster.IsHighAngleGun)
				{
					if (eqmaster.IsHighAngleGunWithAADirector)
						levelBonus = 3;
					else
						levelBonus = 2;
				}
				else if (eqmaster.CategoryType == EquipmentTypes.AAGun)
				{
					if (eqmaster.AA >= 8)
						levelBonus = 6;
					else
						levelBonus = 4;
				}
				else if (eqmaster.CategoryType == EquipmentTypes.AADirector)
				{
					levelBonus = 2;
				}
				else
				{
					levelBonus = 0;
				}

				x += eqmaster.AA * equipmentBonus + Math.Sqrt(eq.Level) * levelBonus;
			}

			return equippedModifier * Math.Floor(x / equippedModifier);
		}


		/// <summary>
		/// 艦隊防空値を求めます。
		/// </summary>
		public static double GetAdjustedFleetAAValue(IEnumerable<ShipData> ships, int formation)
		{
			double formationBonus;
			switch (formation)
			{
				case 2:     // 複縦陣
					formationBonus = 1.2;
					break;
				case 3:     // 輪形陣
					formationBonus = 1.6;
					break;
				case 6:     // 警戒陣
					formationBonus = 1.1;
					break;
				case 11:    // 第一警戒航行序列
					formationBonus = 1.1;
					break;
				case 13:    // 第三警戒航行序列
					formationBonus = 1.5;
					break;
				default:
					formationBonus = 1.0;
					break;
			}

			double fleetAABonus = 0;
			foreach (var ship in ships)
			{
				if (ship == null)
					continue;

				double shipAABonus = 0;
				foreach (var eq in ship.AllSlotInstance)
				{
					if (eq == null)
						continue;

					var eqmaster = eq.MasterEquipment;


					double equipmentBonus;

					if (eqmaster.IsHighAngleGun || eqmaster.CategoryType == EquipmentTypes.AADirector)
						equipmentBonus = 0.35;

					else if (eqmaster.IsRadar)
						equipmentBonus = 0.4;

					else if (eqmaster.CategoryType == EquipmentTypes.AAShell)
						equipmentBonus = 0.6;

					else if (eqmaster.EquipmentID == 9) // 46cm三連装砲
						equipmentBonus = 0.25;

					else
						equipmentBonus = 0.2;


					double levelBonus;

					if (eqmaster.IsHighAngleGunWithAADirector)
						levelBonus = 3.0;

					else if (eqmaster.IsHighAngleGun || eqmaster.CategoryType == EquipmentTypes.AADirector)
						levelBonus = 2.0;

					else if (eqmaster.IsRadar)
						levelBonus = 1.5;

					else
						levelBonus = 0.0;


					shipAABonus += eqmaster.AA * equipmentBonus + Math.Sqrt(eq.Level) * levelBonus;
				}

				fleetAABonus += Math.Floor(shipAABonus);
			}

			return Math.Floor(formationBonus * fleetAABonus) * 2 / 1.3;
		}

		/// <summary>
		/// 艦隊防空値を求めます。
		/// </summary>
		public static double GetAdjustedFleetAAValue(FleetData fleet, int formation)
		{
			return GetAdjustedFleetAAValue(fleet.MembersWithoutEscaped, formation);
		}


		/// <summary>
		/// 対空砲火における連合艦隊補正を求めます。
		/// </summary>
		/// <param name="combinedFleetFlag">連合艦隊フラグ。 -1=連合艦隊でない, 1=連合艦隊主力艦隊, 2=連合艦隊随伴艦隊</param>
		public static double GetAirDefenseCombinedFleetCoefficient(int combinedFleetFlag)
		{
			switch (combinedFleetFlag)
			{
				case 1:
					return 0.72;
				case 2:
					return 0.48;
				default:
					return 1.0;
			}
		}


		/// <summary>
		/// 割合撃墜(の割合)を求めます。
		/// </summary>
		/// <param name="adjustedAAValue">加重対空値</param>
		/// <param name="combinedFleetFlag">連合艦隊フラグ。 -1=連合艦隊でない, 1=連合艦隊主力艦隊, 2=連合艦隊随伴艦隊</param>
		public static double GetProportionalAirDefense(double adjustedAAValue, int combinedFleetFlag = -1)
		{
			return adjustedAAValue * GetAirDefenseCombinedFleetCoefficient(combinedFleetFlag) / 400;
		}

		/// <summary>
		/// 固定撃墜を求めます。
		/// </summary>
		/// <param name="adjustedAAValue">加重対空値</param>
		/// <param name="adjustedFleetAAValue">艦隊防空値</param>
		/// <param name="cutinKind">対空カットイン種別</param>
		/// <param name="combinedFleetFlag">連合艦隊フラグ。 -1=連合艦隊でない, 1=連合艦隊主力艦隊, 2=連合艦隊随伴艦隊</param>
		public static int GetFixedAirDefense(double adjustedAAValue, double adjustedFleetAAValue, int cutinKind, int combinedFleetFlag = -1)
		{
			double cutinBonus = Calculator.AACutinVariableBonus.ContainsKey(cutinKind) ? Calculator.AACutinVariableBonus[cutinKind] : 1.0;

			return (int)Math.Floor((adjustedAAValue + adjustedFleetAAValue) * GetAirDefenseCombinedFleetCoefficient(combinedFleetFlag) * cutinBonus / 10);
		}




		/// <summary>
		/// 対空カットイン固定ボーナス
		/// </summary>
		public static readonly ReadOnlyDictionary<int, int> AACutinFixedBonus = new ReadOnlyDictionary<int, int>(new Dictionary<int, int>() {
			{  1, 7 },
			{  2, 6 },
			{  3, 4 },
			{  4, 6 },
			{  5, 4 },
			{  6, 4 },
			{  7, 3 },
			{  8, 4 },
			{  9, 2 },
			{ 10, 8 },
			{ 11, 6 },
			{ 12, 3 },
			{ 13, 4 },
			{ 14, 4 },
			{ 15, 3 },
			{ 16, 4 },
			{ 17, 2 },
			{ 18, 2 },
			{ 19, 5 },
			{ 20, 3 },
			{ 21, 5 },
			{ 22, 2 },
			{ 23, 1 },
			{ 24, 3 },
			{ 25, 7 },
			{ 26, 6 },
			{ 28, 4 },
			{ 29, 5 },
			{ 30, 3 },
			{ 31, 2 },
			{ 32, 3 },
			{ 33, 3 },
			{ 34, 7 },
			{ 35, 6 },
			{ 36, 6 },
			{ 37, 4 },
			{ 39, 10 },
			{ 40, 10 },
			{ 41, 9 },
		});


		/// <summary>
		/// 対空カットイン変動ボーナス
		/// </summary>
		public static readonly ReadOnlyDictionary<int, double> AACutinVariableBonus = new ReadOnlyDictionary<int, double>(new Dictionary<int, double>() {
			{  1, 1.7 },
			{  2, 1.7 },
			{  3, 1.6 },
			{  4, 1.5 },
			{  5, 1.5 },
			{  6, 1.45 },
			{  7, 1.35 },
			{  8, 1.4 },
			{  9, 1.3 },
			{ 10, 1.65 },
			{ 11, 1.5 },
			{ 12, 1.25 },
			{ 13, 1.35 },
			{ 14, 1.45 },
			{ 15, 1.3 },
			{ 16, 1.4 },
			{ 17, 1.25 },
			{ 18, 1.2 },
			{ 19, 1.45 },
			{ 20, 1.25 },
			{ 21, 1.45 },
			{ 22, 1.2 },
			{ 23, 1.05 },
			{ 24, 1.25 },
			{ 25, 1.55 },
			{ 26, 1.4 },
			{ 28, 1.4 },
			{ 29, 1.55 },
			{ 30, 1.3 },
			{ 31, 1.25 },
			{ 32, 1.2 },
			{ 33, 1.35 },
			{ 34, 1.6 },
			{ 35, 1.55 },
			{ 36, 1.55 },
			{ 37, 1.45 },
			{ 39, 1.7 },
			{ 40, 1.7 },
			{ 41, 1.65 },
		});


		/// <summary>
		/// 撃墜数の推定値を求めます。
		/// </summary>
		/// <param name="enemyAircraftCount">敵航空中隊の機数</param>
		/// <param name="proportionalAirDefense">割合撃墜の割合</param>
		/// <param name="fixedAirDefense">固定撃墜</param>
		/// <param name="aaCutinKind">発動した対空カットインの種類</param>
		public static int GetShootDownCount(int enemyAircraftCount, double proportionalAirDefense, int fixedAirDefense, int aaCutinKind)
		{
			return (int)Math.Floor(enemyAircraftCount * proportionalAirDefense) + fixedAirDefense + 1 + (AACutinFixedBonus.ContainsKey(aaCutinKind) ? AACutinFixedBonus[aaCutinKind] : 0);
		}


		/// <summary>
		/// 対空噴進弾幕の発動確率を求めます。
		/// </summary>
		/// <param name="ship">対象の艦船。</param>
		public static double GetAARocketBarrageProbability(ShipData ship)
		{
			if (ship == null)
				return 0;

			switch (ship.MasterShip.ShipType)
			{
				case ShipTypes.AviationBattleship:
				case ShipTypes.LightAircraftCarrier:
				case ShipTypes.AircraftCarrier:
				case ShipTypes.ArmoredAircraftCarrier:
				case ShipTypes.SeaplaneTender:
				case ShipTypes.AviationCruiser:
					{
						int rocketLauncherCount = ship.AllSlotInstanceMaster.Count(eq => eq?.IsAARocketLauncher ?? false);
						if (rocketLauncherCount == 0)
							return 0;

						double rocket = (0.9 * ship.LuckTotal + GetAdjustedAAValue(ship)) / 281.0 + ((rocketLauncherCount - 1) * 0.15);
						if (ship.MasterShip.ShipClass == 2) // 伊勢型
							rocket += 0.25;

						return rocket;
					}

				default:
					return 0;
			}
		}



		/// <summary>
		/// HP を 1 回復するために必要な入渠時間を求めます。
		/// </summary>
		public static TimeSpan CalculateDockingUnitTime(ShipData ship)
		{
			int damage = ship.HPMax - ship.HPCurrent;
			if (damage == 0)
				return TimeSpan.Zero;

			return new TimeSpan(DateTimeHelper.FromAPITimeSpan(ship.RepairTime).Add(TimeSpan.FromSeconds(-30)).Ticks / damage);
		}



		/// <summary>
		/// 泊地修理において、指定時間修理したときの回復量を求めます。
		/// </summary>
		/// <param name="ship">対象の艦船。</param>
		/// <param name="repairTime">泊地修理を実施した時間。</param>
		/// <returns></returns>
		public static int CalculateAnchorageRepairHealAmount(ShipData ship, TimeSpan repairTime)
		{
			return CalculateAnchorageRepairHealAmount(ship.HPMax - ship.HPCurrent, DateTimeHelper.FromAPITimeSpan(ship.RepairTime).TotalSeconds, repairTime);
		}

		/// <summary>
		/// 泊地修理において、指定時間修理したときの回復量を求めます。
		/// </summary>
		/// <param name="damage">被ダメージ。</param>
		/// <param name="dockingSeconds">入渠時間。</param>
		/// <param name="repairTime">泊地修理を実施した時間。</param>
		public static int CalculateAnchorageRepairHealAmount(int damage, double dockingSeconds, TimeSpan repairTime)
		{
			if (damage <= 0)
				return 0;

			int heal = (int)Math.Floor(Math.Floor(repairTime.TotalMinutes) * 60 / (dockingSeconds / damage));
			return Math.Min(Math.Max(heal, 1), damage);
		}


		/// <summary>
		/// 泊地修理において、指定した量の HP を回復するために必要な時間を求めます。
		/// </summary>
		/// <param name="ship">対象の艦船。</param>
		/// <param name="healAmount">回復したい HP 量。</param>
		public static TimeSpan CalculateAnchorageRepairTime(ShipData ship, int healAmount)
		{
			return CalculateAnchorageRepairTime(ship.HPMax - ship.HPCurrent, DateTimeHelper.FromAPITimeSpan(ship.RepairTime).TotalSeconds, healAmount);
		}

		/// <summary>
		/// 泊地修理において、指定した量の HP を回復するために必要な時間を求めます。
		/// </summary>
		/// <param name="damage">被ダメージ。</param>
		/// <param name="dockingSeconds">入渠時間。</param>
		/// <param name="healAmount">回復したい HP 量。</param>
		public static TimeSpan CalculateAnchorageRepairTime(int damage, double dockingSeconds, int healAmount)
		{

			if (healAmount <= 0)
				throw new ArgumentOutOfRangeException("healAmount must be greater than 0.");

			if (damage <= 0)
				return TimeSpan.Zero;

			healAmount = Math.Min(healAmount, damage);

			if (healAmount == 1)
			{
				return TimeSpan.FromMinutes(20);
			}
			else
			{
				var time = TimeSpan.FromMinutes(Math.Ceiling(healAmount * dockingSeconds / damage / 60));

				if (time.TotalMinutes < 20)
					return TimeSpan.FromMinutes(20);
				else
					return time;
			}
		}





		#region roma-to-hira table
		static Dictionary<string, string> RomaToHiraTable = new Dictionary<string, string> {
			{ "a", "あ" },
			{ "i", "い" },
			{ "u", "う" },
			{ "e", "え" },
			{ "o", "お" },

			{ "ba", "ば" },
			{ "bi", "び" },
			{ "bu", "ぶ" },
			{ "be", "べ" },
			{ "bo", "ぼ" },

			{ "bya", "びゃ" },
			{ "byi", "びぃ" },
			{ "byu", "びゅ" },
			{ "bye", "びぇ" },
			{ "byo", "びょ" },

			{ "ca", "か" },
			{ "ci", "し" },
			{ "cu", "く" },
			{ "ce", "せ" },
			{ "co", "こ" },

			{ "cha", "ちゃ" },
			{ "chi", "ち" },
			{ "chu", "ちゅ" },
			{ "che", "ちぇ" },
			{ "cho", "ちょ" },

			{ "cya", "ちゃ" },
			{ "cyi", "ちぃ" },
			{ "cyu", "ちゅ" },
			{ "cye", "ちぇ" },
			{ "cyo", "ちょ" },

			{ "da", "だ" },
			{ "di", "ぢ" },
			{ "du", "づ" },
			{ "de", "で" },
			{ "do", "ど" },

			{ "dha", "でゃ" },
			{ "dhi", "でぃ" },
			{ "dhu", "でゅ" },
			{ "dhe", "でぇ" },
			{ "dho", "でょ" },

			{ "dwa", "どぁ" },
			{ "dwi", "どぃ" },
			{ "dwu", "どぅ" },
			{ "dwe", "どぇ" },
			{ "dwo", "どぉ" },

			{ "dya", "ぢゃ" },
			{ "dyi", "ぢぃ" },
			{ "dyu", "ぢゅ" },
			{ "dye", "ぢぇ" },
			{ "dyo", "ぢょ" },

			{ "fa", "ふぁ" },
			{ "fi", "ふぃ" },
			{ "fu", "ふ" },
			{ "fe", "ふぇ" },
			{ "fo", "ふぉ" },

			{ "fwa", "ふぁ" },
			{ "fwi", "ふぃ" },
			{ "fwu", "ふぅ" },
			{ "fwe", "ふぇ" },
			{ "fwo", "ふぉ" },

			{ "fya", "ふゃ" },
			{ "fyi", "ふぃ" },
			{ "fyu", "ふゅ" },
			{ "fye", "ふぇ" },
			{ "fyo", "ふょ" },

			{ "ga", "が" },
			{ "gi", "ぎ" },
			{ "gu", "ぐ" },
			{ "ge", "げ" },
			{ "go", "ご" },

			{ "gwa", "ぐぁ" },
			{ "gwi", "ぐぃ" },
			{ "gwu", "ぐぅ" },
			{ "gwe", "ぐぇ" },
			{ "gwo", "ぐぉ" },

			{ "gya", "ぎゃ" },
			{ "gyi", "ぎぃ" },
			{ "gyu", "ぎゅ" },
			{ "gye", "ぎぇ" },
			{ "gyo", "ぎょ" },

			{ "ha", "は" },
			{ "hi", "ひ" },
			{ "hu", "ふ" },
			{ "he", "へ" },
			{ "ho", "ほ" },

			{ "hya", "ひゃ" },
			{ "hyi", "ひぃ" },
			{ "hyu", "ひゅ" },
			{ "hye", "ひぇ" },
			{ "hyo", "ひょ" },

			{ "ja", "じゃ" },
			{ "ji", "じ" },
			{ "ju", "じゅ" },
			{ "je", "じぇ" },
			{ "jo", "じょ" },

			{ "jya", "じゃ" },
			{ "jyi", "じぃ" },
			{ "jyu", "じゅ" },
			{ "jye", "じぇ" },
			{ "jyo", "じょ" },

			{ "ka", "か" },
			{ "ki", "き" },
			{ "ku", "く" },
			{ "ke", "け" },
			{ "ko", "こ" },

			{ "kwa", "くぁ" },

			{ "kya", "きゃ" },
			{ "kyi", "きぃ" },
			{ "kyu", "きゅ" },
			{ "kye", "きぇ" },
			{ "kyo", "きょ" },

			{ "la", "ぁ" },
			{ "li", "ぃ" },
			{ "lu", "ぅ" },
			{ "le", "ぇ" },
			{ "lo", "ぉ" },

			{ "lka", "ヵ" },
			{ "lke", "ヶ" },
			{ "ltu", "っ" },
			{ "ltsu", "っ" },
			{ "lwa", "ゎ" },

			{ "lya", "ゃ" },
			{ "lyi", "ぃ" },
			{ "lyu", "ゅ" },
			{ "lye", "ぇ" },
			{ "lyo", "ょ" },

			{ "ma", "ま" },
			{ "mi", "み" },
			{ "mu", "む" },
			{ "me", "め" },
			{ "mo", "も" },

			{ "mya", "みゃ" },
			{ "myi", "みぃ" },
			{ "myu", "みゅ" },
			{ "mye", "みぇ" },
			{ "myo", "みょ" },

			{ "na", "な" },
			{ "ni", "に" },
			{ "nu", "ぬ" },
			{ "ne", "ね" },
			{ "no", "の" },

			{ "nn", "ん" },

			{ "nya", "にゃ" },
			{ "nyi", "にぃ" },
			{ "nyu", "にゅ" },
			{ "nye", "にぇ" },
			{ "nyo", "にょ" },

			{ "pa", "ぱ" },
			{ "pi", "ぴ" },
			{ "pu", "ぷ" },
			{ "pe", "ぺ" },
			{ "po", "ぽ" },

			{ "pya", "ぴゃ" },
			{ "pyi", "ぴぃ" },
			{ "pyu", "ぴゅ" },
			{ "pye", "ぴぇ" },
			{ "pyo", "ぴょ" },

			{ "qa", "くぁ" },
			{ "qi", "くぃ" },
			{ "qu", "く" },
			{ "qe", "くぇ" },
			{ "qo", "くぉ" },

			{ "qwa", "くぁ" },
			{ "qwi", "くぃ" },
			{ "qwu", "くぅ" },
			{ "qwe", "くぇ" },
			{ "qwo", "くぉ" },

			{ "qya", "くゃ" },
			{ "qyi", "くぃ" },
			{ "qyu", "くゅ" },
			{ "qye", "くぇ" },
			{ "qyo", "くょ" },

			{ "ra", "ら" },
			{ "ri", "り" },
			{ "ru", "る" },
			{ "re", "れ" },
			{ "ro", "ろ" },

			{ "rya", "りゃ" },
			{ "ryi", "りぃ" },
			{ "ryu", "りゅ" },
			{ "rye", "りぇ" },
			{ "ryo", "りょ" },

			{ "sa", "さ" },
			{ "si", "し" },
			{ "su", "す" },
			{ "se", "せ" },
			{ "so", "そ" },

			{ "sha", "しゃ" },
			{ "shi", "し" },
			{ "shu", "しゅ" },
			{ "she", "しぇ" },
			{ "sho", "しょ" },

			{ "swa", "すぁ" },
			{ "swi", "すぃ" },
			{ "swu", "すぅ" },
			{ "swe", "すぇ" },
			{ "swo", "すぉ" },

			{ "sya", "しゃ" },
			{ "syi", "しぃ" },
			{ "syu", "しゅ" },
			{ "sye", "しぇ" },
			{ "syo", "しょ" },

			{ "ta", "た" },
			{ "ti", "ち" },
			{ "tu", "つ" },
			{ "te", "て" },
			{ "to", "と" },

			{ "tha", "てゃ" },
			{ "thi", "てぃ" },
			{ "thu", "てゅ" },
			{ "the", "てぇ" },
			{ "tho", "てょ" },

			{ "tsa", "つぁ" },
			{ "tsi", "つぃ" },
			{ "tsu", "つ" },
			{ "tse", "つぇ" },
			{ "tso", "つぉ" },

			{ "twa", "とぁ" },
			{ "twi", "とぃ" },
			{ "twu", "とぅ" },
			{ "twe", "とぇ" },
			{ "two", "とぉ" },

			{ "tya", "ちゃ" },
			{ "tyi", "ちぃ" },
			{ "tyu", "ちゅ" },
			{ "tye", "ちぇ" },
			{ "tyo", "ちょ" },

			{ "va", "ヴぁ" },
			{ "vi", "ヴぃ" },
			{ "vu", "ヴ" },
			{ "ve", "ヴぇ" },
			{ "vo", "ヴぉ" },

			{ "vya", "ヴゃ" },
			{ "vyi", "ヴぃ" },
			{ "vyu", "ヴゅ" },
			{ "vye", "ヴぇ" },
			{ "vyo", "ヴょ" },

			{ "wa", "わ" },
			{ "wi", "うぃ" },
			{ "wu", "う" },
			{ "we", "うぇ" },
			{ "wo", "を" },

			{ "wha", "うぁ" },
			{ "whi", "うぃ" },
			{ "whu", "う" },
			{ "whe", "うぇ" },
			{ "who", "うぉ" },

			{ "wyi", "ゐ" },
			{ "wye", "ゑ" },

			{ "xa", "ぁ" },
			{ "xi", "ぃ" },
			{ "xu", "ぅ" },
			{ "xe", "ぇ" },
			{ "xo", "ぉ" },

			{ "xka", "ヵ" },
			{ "xke", "ヶ" },
			{ "xtu", "っ" },
			{ "xtsu", "っ" },
			{ "xwa", "ゎ" },
			{ "xn", "ん" },

			{ "xya", "ゃ" },
			{ "xyi", "ぃ" },
			{ "xyu", "ゅ" },
			{ "xye", "ぇ" },
			{ "xyo", "ょ" },

			{ "ya", "や" },
			{ "yi", "い" },
			{ "yu", "ゆ" },
			{ "ye", "いぇ" },
			{ "yo", "よ" },

			{ "za", "ざ" },
			{ "zi", "じ" },
			{ "zu", "ず" },
			{ "ze", "ぜ" },
			{ "zo", "ぞ" },

			{ "zya", "じゃ" },
			{ "zyi", "じぃ" },
			{ "zyu", "じゅ" },
			{ "zye", "じぇ" },
			{ "zyo", "じょ" },

		};
		#endregion

		/// <summary>
		/// 文中のカタカナをひらがなに変換します。
		/// </summary>
		public static string ToHiragana(string str)
		{
			// あまり深いことは考えずにやる
			char hiraganaHead = '\x3041';
			char katakanaHead = '\x30a1';
			char katakanaTail = '\x30ff';

			char[] chars = str.ToCharArray();
			for (int i = 0; i < chars.Length; i++)
			{
				if (katakanaHead <= chars[i] && chars[i] <= katakanaTail)
				{       // is katakana
					chars[i] = (char)((int)chars[i] - (int)katakanaHead + (int)hiraganaHead);
				}
			}

			return new string(chars);
		}


		static bool IsVowel(char c)
		{
			return c == 'a' || c == 'i' || c == 'u' || c == 'e' || c == 'o' || c == 'n';
		}

		/// <summary>
		/// 文中のローマ字をひらがなに変換します。
		/// </summary>
		public static string RomaToHira(string roma)
		{
			var chars = roma.ToLower().ToCharArray();
			var sb = new StringBuilder();

			int prev = 0;

			for (int i = 0; i < roma.Length; i++)
			{
				if (IsVowel(chars[i]) || !char.IsLower(chars[i]) || i == roma.Length - 1)
				{

					// ひらがな以外の文字を除外
					for (int p = prev; p <= i; p++)
					{
						if (!char.IsLower(chars[p]))
						{
							sb.Append(chars[p]);
							prev++;
						}
						else
							break;
					}

					if (prev > i)
						continue;

					// n 単体はまだ処理しない
					if (chars[prev] == 'n' && i - prev == 0 && i < roma.Length - 1)
						continue;

					// っ
					for (int p = prev; p < i; p++)
					{
						if (chars[p] == chars[p + 1] && chars[p] != 'n')
						{
							sb.Append("っ");
							prev++;
						}
						else break;
					}

					var currentArray = new char[i - prev + 1];
					Array.Copy(chars, prev, currentArray, 0, currentArray.Length);
					var current = new string(currentArray);

					if (RomaToHiraTable.ContainsKey(current))
						sb.Append(RomaToHiraTable[current]);
					else if (chars[prev] == 'n' && RomaToHiraTable.ContainsKey(current.Substring(1)))
						sb.Append("ん").Append(RomaToHiraTable[current.Substring(1)]);
					else
						sb.Append(current);

					prev = i + 1;
				}
			}

			return sb.ToString();
		}


	}


	/// <summary>
	/// 昼戦攻撃種別を表します。
	/// </summary>
	public enum DayAttackKind
	{
		/// <summary> 不明 </summary>
		Unknown = -1,


		/// <summary> 通常攻撃 (API上でのみ使用されます) </summary>
		NormalAttack,

		/// <summary> レーザー攻撃 </summary>
		Laser,

		/// <summary> 連続射撃 </summary>
		DoubleShelling,

		/// <summary> カットイン(主砲/副砲) </summary>
		CutinMainSub,

		/// <summary> カットイン(主砲/電探) </summary>
		CutinMainRadar,

		/// <summary> カットイン(主砲/徹甲弾) </summary>
		CutinMainAP,

		/// <summary> カットイン(主砲/主砲) </summary>
		CutinMainMain,

		/// <summary> 空母カットイン </summary>
		CutinAirAttack,

		/// <summary> Nelson Touch </summary>
		SpecialNelson = 100,

		/// <summary> 一斉射かッ…胸が熱いな！ </summary>
		SpecialNagato = 101,

		/// <summary> 長門、いい？ いくわよ！ 主砲一斉射ッ！ </summary>
		SpecialMutsu = 102,

		/// <summary> Colorado Touch </summary>
		SpecialColorado = 103,

		/// <summary> 僚艦夜戦突撃 </summary>
		SpecialKongo = 104,

		/// <summary> 瑞雲立体攻撃 </summary>
		ZuiunMultiAngle = 200,

		/// <summary> 海空立体攻撃 </summary>
		SeaAirMultiAngle = 201,


		/// <summary> 砲撃 </summary>
		Shelling = 1000,

		/// <summary> 空撃 </summary>
		AirAttack,

		/// <summary> 爆雷攻撃 </summary>
		DepthCharge,

		/// <summary> 雷撃 </summary>
		Torpedo,


		/// <summary> ロケット攻撃 </summary>
		Rocket = 2000,


		/// <summary> 揚陸攻撃(大発動艇) </summary>
		LandingDaihatsu = 3000,

		/// <summary> 揚陸攻撃(特大発動艇) </summary>
		LandingTokuDaihatsu,

		/// <summary> 揚陸攻撃(大発動艇(八九式中戦車&陸戦隊)) </summary>
		LandingDaihatsuTank,

		/// <summary> 揚陸攻撃(特二式内火艇) </summary>
		LandingAmphibious,

		/// <summary> 揚陸攻撃(特大発動艇+戦車第11連隊) </summary>
		LandingTokuDaihatsuTank,

	}


	/// <summary>
	/// 夜戦攻撃種別を表します。
	/// </summary>
	public enum NightAttackKind
	{
		/// <summary> 不明 </summary>
		Unknown = -1,


		/// <summary> 通常攻撃 (API上でのみ使用されます) </summary>
		NormalAttack,

		/// <summary> 連続攻撃 </summary>
		DoubleShelling,

		/// <summary> カットイン(主砲/魚雷) </summary>
		CutinMainTorpedo,

		/// <summary> カットイン(魚雷/魚雷) </summary>
		CutinTorpedoTorpedo,

		/// <summary> カットイン(主砲/主砲/副砲) </summary>
		CutinMainSub,

		/// <summary> カットイン(主砲/主砲/主砲) </summary>
		CutinMainMain,

		/// <summary> 空母カットイン </summary>
		CutinAirAttack,

		/// <summary> 駆逐カットイン(主砲/魚雷/電探) </summary>
		CutinTorpedoRadar,

		/// <summary> 駆逐カットイン(魚雷/見張員/電探) </summary>
		CutinTorpedoPicket,

		/// <summary> Nelson Touch </summary>
		SpecialNelson = 100,

		/// <summary> 一斉射かッ…胸が熱いな！ </summary>
		SpecialNagato = 101,

		/// <summary> 長門、いい？ いくわよ！ 主砲一斉射ッ！ </summary>
		SpecialMutsu = 102,

		/// <summary> Colorado Touch </summary>
		SpecialColorado = 103,

		/// <summary> 僚艦夜戦突撃 </summary>
		SpecialKongo = 104,


		/// <summary> 砲撃 </summary>
		Shelling = 1000,

		/// <summary> 空撃 </summary>
		AirAttack,

		/// <summary> 爆雷攻撃 </summary>
		DepthCharge,

		/// <summary> 雷撃 </summary>
		Torpedo,


		/// <summary> ロケット攻撃 </summary>
		Rocket = 2000,


		/// <summary> 揚陸攻撃(大発動艇) </summary>
		LandingDaihatsu = 3000,

		/// <summary> 揚陸攻撃(特大発動艇) </summary>
		LandingTokuDaihatsu,

		/// <summary> 揚陸攻撃(大発動艇(八九式中戦車&陸戦隊)) </summary>
		LandingDaihatsuTank,

		/// <summary> 揚陸攻撃(特二式内火艇) </summary>
		LandingAmphibious,

		/// <summary> 揚陸攻撃(特大発動艇+戦車第11連隊) </summary>
		LandingTokuDaihatsuTank,

	}


	/// <summary>
	/// 昼戦空母カットインの種別を表します。
	/// </summary>
	public enum DayAirAttackCutinKind
	{
		None = 0,

		FighterBomberAttacker,
		BomberBomberAttacker,
		BomberAttacker,
	}


	/// <summary>
	/// 夜戦魚雷カットインの種別を表します。
	/// </summary>
	public enum NightTorpedoCutinKind
	{
		None = 0,

		LateModelTorpedoSubmarineEquipment,
		LateModelTorpedo2,
	}
}
