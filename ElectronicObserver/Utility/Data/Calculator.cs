using ElectronicObserver.Data;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.Data {

	/// <summary>
	/// 汎用計算クラス
	/// </summary>
	public static class Calculator {

		/// <summary>
		/// レベルに依存するパラメータ値を求めます。
		/// </summary>
		/// <param name="min">初期値。</param>
		/// <param name="max">最大値。</param>
		/// <param name="lv">レベル。</param>
		/// <returns></returns>
		public static int GetParameterFromLevel( int min, int max, int lv ) {
			return min + ( max - min ) * lv / 99;
		}



		/// <summary>
		/// 各装備カテゴリにおける制空値の熟練度ボーナス
		/// </summary>
		private static readonly Dictionary<int, int[]> AircraftLevelBonus = new Dictionary<int, int[]>() {
			{ 6, new int[] { 0, 0, 2, 5, 9, 14, 14, 22, 22 } },		// 艦上戦闘機
			{ 7, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 } },			// 艦上爆撃機
			{ 8, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 } },			// 艦上攻撃機
			{ 11, new int[] { 0, 1, 1, 1, 1, 3, 3, 6, 6 } },		// 水上爆撃機
			{ 45, new int[] { 0, 0, 2, 5, 9, 14, 14, 22, 22 } },	// 水上戦闘機
			{ 47, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 } },		// 陸上攻撃機
			{ 48, new int[] { 0, 0, 2, 5, 9, 14, 14, 22, 22 } },	// 局地戦闘機
			{ 56, new int[] { 0, 0, 2, 5, 9, 14, 14, 22, 22 } },	// 噴式戦闘機
			{ 57, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 } },		// 噴式戦闘爆撃機
			{ 58, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 } },		// 噴式攻撃機
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
		private static readonly Dictionary<int, double> LevelBonus = new Dictionary<int, double>() { 
			{ 6, 0.2 },		// 艦上戦闘機
			{ 7, 0.25 },	// 艦上爆撃機
			{ 45, 0.2 },	// 水上戦闘機
		};



		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="equipmentID">装備ID。</param>
		/// <param name="count">搭載機数。</param>
		/// <param name="aircraftLevel">艦載機熟練度。既定値は 0 です。</param>
		/// <param name="level">改修レベル。既定値は 0 です。</param>
		/// <param name="isAirDefense">基地航空隊による防空戦かどうか。</param>
		/// <param name="isAircraftExpMaximum">艦載機の内部熟練度が当該レベルで最大値であるとして計算するか。falseなら最小値として計算します。</param>
		/// <returns></returns>
		public static int GetAirSuperiority( int equipmentID, int count, int aircraftLevel = 0, int level = 0, bool isAirDefense = false, bool isAircraftExpMaximum = false ) {

			if ( count <= 0 )
				return 0;

			var eq = KCDatabase.Instance.MasterEquipments[equipmentID];
			if ( eq == null )
				return 0;

			int category = eq.CategoryType;
			if ( !isAirDefense && !AircraftLevelBonus.ContainsKey( category ) )		// 防空の場合は全航空機が参加する
				return 0;

			double levelBonus = LevelBonus.ContainsKey( category ) ? LevelBonus[category] : 0;	// 改修レベル補正
			double interceptorBonus = 0;	// 局地戦闘機の迎撃補正
			if ( category == 48 ) {
				if ( isAirDefense )
					interceptorBonus = eq.Accuracy * 2 + eq.Evasion;
				else
					interceptorBonus = eq.Evasion * 1.5;
			}

			int aircraftExp;
			if ( isAircraftExpMaximum ) {
				if ( aircraftLevel < 7 )
					aircraftExp = AircraftExpTable[aircraftLevel + 1] - 1;
				else
					aircraftExp = AircraftExpTable.Last();
			} else {
				aircraftExp = AircraftExpTable[aircraftLevel];
			}

			return (int)( ( eq.AA + levelBonus * level + interceptorBonus ) * Math.Sqrt( count )
				+ Math.Sqrt( aircraftExp / 10.0 )
				+ ( AircraftLevelBonus.ContainsKey( category ) ? AircraftLevelBonus[category][aircraftLevel] : 0 ) );
		}



		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="slot">装備スロット。</param>
		/// <param name="aircraft">搭載機数の配列。</param>
		public static int GetAirSuperiority( int[] slot, int[] aircraft ) {

			return slot.Select( ( eq, i ) => GetAirSuperiority( eq, aircraft[i] ) ).Sum();
		}



		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="fleet">艦船IDの配列。</param>
		public static int GetAirSuperiority( int[] fleet ) {

			return fleet.Select( id => KCDatabase.Instance.MasterShips[id] ).Sum( ship => GetAirSuperiority( ship ) );
		}

		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="fleet">艦船IDの配列。</param>
		/// <param name="slot">各艦船の装備スロット。</param>
		public static int GetAirSuperiority( int[] fleet, int[][] slot ) {

			int air = 0;
			int length = Math.Min( fleet.Length, slot.GetLength( 0 ) );

			for ( int i = 0; i < length; i++ ) {
				ShipDataMaster ship = KCDatabase.Instance.MasterShips[fleet[i]];
				if ( ship == null ) continue;

				air += GetAirSuperiority( slot[i], ship.Aircraft.ToArray() );

			}

			return air;
		}



		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="ship">対象の艦船。</param>
		public static int GetAirSuperiority( ShipData ship, bool isAircraftLevelMaximum = false ) {

			if ( ship == null ) return 0;

			return ship.SlotInstance.Select( ( eq, i ) => eq == null ? 0 :
				GetAirSuperiority( eq.EquipmentID, ship.Aircraft[i], eq.AircraftLevel, eq.Level, false, isAircraftLevelMaximum ) ).Sum();
		}

		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="ship">対象の艦船。</param>
		public static int GetAirSuperiority( ShipDataMaster ship ) {

			if ( ship == null || ship.DefaultSlot == null ) return 0;
			return GetAirSuperiority( ship.DefaultSlot.ToArray(), ship.Aircraft.ToArray() );

		}

		/// <summary>
		/// 制空戦力を求めます。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		public static int GetAirSuperiority( FleetData fleet, bool isAircraftLevelMaximum = false ) {
			if ( fleet == null )
				return 0;
			return fleet.MembersWithoutEscaped.Select( ship => GetAirSuperiority( ship, isAircraftLevelMaximum ) ).Sum();
		}


		/// <summary>
		/// 基地航空隊の制空戦力を求めます。
		/// </summary>
		/// <param name="aircorps">対象の基地航空隊。</param>
		public static int GetAirSuperiority( BaseAirCorpsData aircorps, bool isAircraftLevelMaximum = false ) {
			if ( aircorps == null )
				return 0;

			int air = 0;
			double rate = 1.0;

			foreach ( var sq in aircorps.Squadrons.Values ) {
				if ( sq == null || sq.State != 1 )
					continue;

				air += GetAirSuperiority( sq, aircorps.ActionKind == 2, isAircraftLevelMaximum );

				if ( aircorps.ActionKind != 2 )
					continue;

				// 偵察機補正計算
				int category = sq.EquipmentInstanceMaster.CategoryType;
				int losrate = Math.Min( Math.Max( sq.EquipmentInstanceMaster.LOS - 7, 0 ), 2 );		// ~7, 8, 9~

				switch ( category ) {
					case 10:	// 水上偵察機
					case 41:	// 大型飛行艇
						rate = Math.Max( rate, 1.1 + losrate * 0.03 );
						break;
					case 9:		// 艦上偵察機
					case 59:	// 噴式偵察機
						rate = Math.Max( rate, 1.2 + losrate * 0.05 );
						break;
				}
			}

			return (int)( air * rate );
		}

		/// <summary>
		/// 基地航空中隊の制空戦力を求めます。
		/// </summary>
		/// <param name="squadron">対象の基地航空中隊。</param>
		public static int GetAirSuperiority( BaseAirCorpsSquadron squadron, bool isAirDefense = false, bool isAircraftLevelMaximum = false ) {
			if ( squadron == null || squadron.State != 1 )
				return 0;

			var eq = squadron.EquipmentInstance;
			if ( eq == null )
				return 0;

			return GetAirSuperiority( eq.EquipmentID, squadron.AircraftCurrent, eq.AircraftLevel, eq.Level, isAirDefense, isAircraftLevelMaximum );
		}


		/// <summary>
		/// 最大練度の艦載機を搭載している場合の制空戦力を求めます。
		/// </summary>
		/// <param name="fleet">艦船IDリスト。</param>
		/// <param name="slot">各艦の装備IDリスト。</param>
		/// <returns></returns>
		public static int GetAirSuperiorityAtMaxLevel( int[] fleet, int[][] slot ) {
			return fleet.Select( id => KCDatabase.Instance.MasterShips[id] )
				.Select( ( ship, i ) => ship == null ? 0 :
					slot[i].Select( ( eqid, k ) => GetAirSuperiority( eqid, ship.Aircraft[k], 7, 10, false, true ) ).Sum() ).Sum();
		}


		/// <summary>
		/// 艦載機熟練度・改修レベルを無視した制空戦力を求めます。
		/// </summary>
		/// <param name="ship">対象の艦船。</param>
		public static int GetAirSuperiorityIgnoreLevel( ShipData ship ) {
			if ( ship == null )
				return 0;
			return GetAirSuperiority( ship.SlotMaster.ToArray(), ship.Aircraft.ToArray() );
		}

		/// <summary>
		/// 艦載機熟練度・改修レベルを無視した制空戦力を求めます。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		public static int GetAirSuperiorityIgnoreLevel( FleetData fleet ) {
			if ( fleet == null )
				return 0;
			return fleet.MembersWithoutEscaped.Select( ship => GetAirSuperiorityIgnoreLevel( ship ) ).Sum();
		}



		/// <summary>
		/// 索敵能力を求めます。「2-5式」です。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		public static int GetSearchingAbility_Old( FleetData fleet ) {

			KCDatabase db = KCDatabase.Instance;

			int los_reconplane = 0;
			int los_radar = 0;
			int los_other = 0;

			foreach ( var ship in fleet.MembersWithoutEscaped ) {

				if ( ship == null )
					continue;

				los_other += ship.LOSBase;

				var slot = ship.SlotInstanceMaster;

				for ( int j = 0; j < slot.Count; j++ ) {

					if ( slot[j] == null ) continue;

					switch ( slot[j].EquipmentType[2] ) {
						case 9:		//艦偵
						case 10:	//水偵
						case 11:	//水爆
							if ( ship.Aircraft[j] > 0 )
								los_reconplane += slot[j].LOS * 2;
							break;

						case 12:	//小型電探
						case 13:	//大型電探
							los_radar += slot[j].LOS;
							break;

						default:
							los_other += slot[j].LOS;
							break;
					}
				}
			}


			return (int)Math.Sqrt( los_other ) + los_radar + los_reconplane;
		}


		/// <summary>
		/// 索敵能力を求めます。「2-5式(秋)」です。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		public static double GetSearchingAbility_Autumn( FleetData fleet ) {

			double ret = 0.0;

			foreach ( var ship in fleet.MembersWithoutEscaped ) {
				if ( ship == null ) continue;

				ret += Math.Sqrt( ship.LOSBase ) * 1.6841056;

				foreach ( var eq in ship.SlotInstanceMaster ) {
					if ( eq == null ) continue;

					switch ( eq.CategoryType ) {

						case 7:		//艦爆
							ret += eq.LOS * 1.0376255; break;

						case 8:		//艦攻
							ret += eq.LOS * 1.3677954; break;

						case 9:		//艦偵
							ret += eq.LOS * 1.6592780; break;

						case 10:	//水偵
							ret += eq.LOS * 2.0000000; break;

						case 11:	//水爆
							ret += eq.LOS * 1.7787282; break;

						case 12:	//小型電探
							ret += eq.LOS * 1.0045358; break;

						case 13:	//大型電探
							ret += eq.LOS * 0.9906638; break;

						case 29:	//探照灯
							ret += eq.LOS * 0.9067950; break;

					}
				}
			}

			ret -= Math.Ceiling( KCDatabase.Instance.Admiral.Level / 5.0 ) * 5.0 * 0.6142467;

			return Math.Round( ret, 1 );
		}


		/// <summary>
		/// 索敵能力を求めます。「2-5式(秋)簡易式」です。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		public static double GetSearchingAbility_TinyAutumn( FleetData fleet ) {

			double ret = 0.0;

			foreach ( var ship in fleet.MembersWithoutEscaped ) {
				if ( ship == null ) continue;

				double cur = Math.Sqrt( ship.LOSBase );

				foreach ( var eq in ship.SlotInstanceMaster ) {
					if ( eq == null ) continue;

					switch ( eq.CategoryType ) {

						case 7:		//艦爆
							cur += eq.LOS * 0.6; break;

						case 8:		//艦攻
							cur += eq.LOS * 0.8; break;

						case 9:		//艦偵
							cur += eq.LOS * 1.0; break;

						case 10:	//水偵
							cur += eq.LOS * 1.2; break;

						case 11:	//水爆
							cur += eq.LOS * 1.0; break;

						case 12:	//小型電探
							cur += eq.LOS * 0.6; break;

						case 13:	//大型電探
							cur += eq.LOS * 0.6; break;

						case 29:	//探照灯
							cur += eq.LOS * 0.5; break;

						default:	//その他
							cur += eq.LOS * 0.5; break;
					}
				}

				ret += Math.Floor( cur );
			}

			ret -= Math.Floor( KCDatabase.Instance.Admiral.Level * 0.4 );

			return Math.Round( ret, 1 );
		}


		/// <summary>
		/// 索敵能力を求めます。「判定式(33)」です。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		public static double GetSearchingAbility_33( FleetData fleet ) {

			double ret = 0.0;

			foreach ( var ship in fleet.MembersWithoutEscaped ) {
				if ( ship == null ) {
					ret += 2.0;
					continue;
				}

				//equipments
				foreach ( var slot in ship.SlotInstance ) {

					if ( slot == null )
						continue;

					switch ( slot.MasterEquipment.CategoryType ) {

						case 8:		//艦上攻撃機
							ret += 0.8 * slot.MasterEquipment.LOS;
							break;

						case 9:		//艦上偵察機
						case 94:	//艦上偵察機(II) 存在しないが念のため
							ret += 1.0 * slot.MasterEquipment.LOS;
							break;

						case 10:	//水上偵察機
							ret += 1.2 * ( slot.MasterEquipment.LOS + 1.2 * Math.Sqrt( slot.Level ) );
							break;

						case 11:	//水上爆撃機
							ret += 1.1 * slot.MasterEquipment.LOS;
							break;

						case 12:	//小型電探
						case 13:	//大型電探
							ret += 0.6 * ( slot.MasterEquipment.LOS + 1.25 * Math.Sqrt( slot.Level ) );
							break;

						default:
							ret += 0.6 * slot.MasterEquipment.LOS;
							break;
					}
				}

				ret += Math.Sqrt( ship.LOSBase );

			}

			ret -= Math.Ceiling( 0.4 * KCDatabase.Instance.Admiral.Level );

			return ret;
		}


		/// <summary>
		/// 索敵能力を求めます。「新判定式(33)」です。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		/// <param name="branchWeight">分岐点係数。2-5では1</param>
		public static double GetSearchingAbility_New33( FleetData fleet, int branchWeight ) {

			double ret = 0;

			foreach ( var ship in fleet.MembersWithoutEscaped ) {
				if ( ship == null ) {
					ret += 2.0;
					continue;
				}

				ret += Math.Sqrt( ship.LOSBase );

				double equipmentBonus = 0;
				foreach ( var eq in ship.AllSlotInstance.Where( eq => eq != null ) ) {

					int category = eq.MasterEquipment.CategoryType;

					double equipmentRate;
					if ( category == 8 || category == 58 )		// 艦上攻撃機・噴式攻撃機
						equipmentRate = 0.8;
					else if ( category == 9 || category == 59 )	// 艦上偵察機・噴式偵察機
						equipmentRate = 1.0;
					else if ( category == 10 )	// 水上偵察機
						equipmentRate = 1.2;
					else if ( category == 11 )	// 水上爆撃機
						equipmentRate = 1.1;
					else
						equipmentRate = 0.6;

					double levelRate;
					if ( category == 10 )		// 水上偵察機
						levelRate = 1.2;
					else if ( category == 12 )	// 小型電探
						levelRate = 1.25;
					else if ( category == 13 )	// 大型電探
						levelRate = 1.4;
					else
						levelRate = 0.0;

					equipmentBonus += equipmentRate * ( eq.MasterEquipment.LOS + levelRate * Math.Sqrt( eq.Level ) );
				}

				ret += equipmentBonus * branchWeight;
			}

			// 司令部Lv補正
			ret -= Math.Ceiling( KCDatabase.Instance.Admiral.Level * 0.4 );

			return ret;
		}


		/// <summary>
		/// 艦隊の触接開始率を求めます。
		/// </summary>
		/// <param name="fleet">対象の艦隊。</param>
		public static double GetContactProbability( FleetData fleet ) {

			double successProb = 0.0;

			foreach ( var ship in fleet.MembersWithoutEscaped ) {
				if ( ship == null ) continue;

				var eqs = ship.SlotInstanceMaster;

				for ( int i = 0; i < ship.Slot.Count; i++ ) {
					if ( eqs[i] == null )
						continue;

					if ( eqs[i].CategoryType == 9 ||	// 艦上偵察機
						eqs[i].CategoryType == 10 ||	// 水上偵察機
						eqs[i].CategoryType == 41 ||	// 大型飛行艇
						eqs[i].CategoryType == 59 ) {	// 噴式偵察機

						successProb += 0.04 * eqs[i].LOS * Math.Sqrt( ship.Aircraft[i] );
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
		public static Dictionary<int, double> GetContactSelectionProbability( FleetData fleet ) {

			var probs = new Dictionary<int, double>();

			foreach ( var ship in fleet.MembersWithoutEscaped ) {
				if ( ship == null )
					continue;

				foreach ( var eq in ship.SlotInstanceMaster ) {
					if ( eq == null )
						continue;

					switch ( eq.CategoryType ) {
						case 8:		// 艦上攻撃機
						case 9:		// 艦上偵察機
						case 10:	// 水上偵察機
						case 41:	// 大型飛行艇
						case 58:	// 噴式攻撃機
						case 59:	// 噴式偵察機
							if ( !probs.ContainsKey( eq.Accuracy ) )
								probs.Add( eq.Accuracy, 1.0 );

							probs[eq.Accuracy] *= 1.0 - ( 0.07 * eq.LOS );
							break;
					}
				}
			}

			foreach ( int key in probs.Keys.ToArray() ) {		//列挙中の変更エラーを防ぐため 
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
		public static int GetTPDamage( FleetData fleet ) {

			int tp = 0;

			foreach ( var ship in fleet.MembersWithoutEscaped.Where( s => s != null && s.HPRate > 0.25 ) ) {

				// 装備ボーナス
				foreach ( var eq in ship.AllSlotInstanceMaster.Where( q => q != null ) ) {

					switch ( eq.CategoryType ) {

						case 24:	// 上陸用舟艇
							//if ( eq.EquipmentID == 166 )	// 陸戦隊
							//	tp += 13;
							//else
							tp += 8;
							break;
						case 30:	// 簡易輸送部材
							tp += 5;
							break;
						case 43:	// 戦闘糧食
							tp += 1;
							break;
						case 46:	// 特型内火艇
							tp += 2;
							break;
					}
				}


				// 艦種ボーナス
				switch ( ship.MasterShip.ShipType ) {

					case 2:		// 駆逐艦
						tp += 5;
						break;
					case 3:		// 軽巡洋艦
						tp += 2;
						if ( ship.ShipID == 487 )	// 鬼怒改二
							tp += 8;
						break;
					case 5:		// 重巡洋艦
						tp += 0;
						break;
					case 6:		// 航空巡洋艦
						tp += 4;
						break;
					case 10:	// 航空戦艦
						tp += 7;
						break;
					case 16:	// 水上機母艦
						tp += 9;
						break;
					case 17:	// 揚陸艦
						tp += 12;
						break;
					case 20:	// 潜水母艦
						tp += 7;
						break;
					case 21:	// 練習巡洋艦
						tp += 6;
						break;
					case 22:	// 補給艦
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
		public static double GetExpeditionBonus( FleetData fleet ) {
			var eqs = fleet.MembersInstance
				.Where( s => s != null )
				.SelectMany( s => s.SlotInstance )
				.Where( eq => eq != null && EquipmentExpeditionBonus.ContainsKey( eq.EquipmentID ) );

			double normalBonus = eqs.Sum( eq => EquipmentExpeditionBonus[eq.EquipmentID] )
				+ fleet.MembersInstance.Count( s => s != null && s.ShipID == 487 ) * 0.05;		// 鬼怒改二

			normalBonus = Math.Min( normalBonus, 0.2 );
			double levelBonus = eqs.Any() ? ( 0.01 * normalBonus * eqs.Average( eq => eq.Level ) ) : 0;

			int tokuCount = eqs.Count( eq => eq.EquipmentID == 193 );
			int daihatsuCount = eqs.Count( eq => eq.EquipmentID == 68 );
			double tokuBonus;

			if ( tokuCount <= 2 )
				tokuBonus = 0.02 * tokuCount;
			else if ( tokuCount == 3 )
				tokuBonus = 0.05 + 0.002 * Math.Min( Math.Max( daihatsuCount - 1, 0 ), 2 );
			else {
				if ( daihatsuCount <= 2 )
					tokuBonus = 0.054 + 0.002 * daihatsuCount;
				else if ( daihatsuCount == 3 )
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
		public static DayAttackKind GetDayAttackKind( int[] slot, int attackerShipID, int defenderShipID, bool includeSpecialAttack = true ) {

			int reconcnt = 0;
			int mainguncnt = 0;
			int subguncnt = 0;
			int apshellcnt = 0;
			int radarcnt = 0;
			int rocketcnt = 0;

			if ( slot == null ) return DayAttackKind.Unknown;

			for ( int i = 0; i < slot.Length; i++ ) {

				EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[slot[i]];
				if ( eq == null ) continue;

				int eqtype = eq.CategoryType;

				switch ( eqtype ) {
					case 1:		// 小口径主砲
					case 2:		// 中口径主砲
					case 3:		// 大口径主砲
						mainguncnt++;
						break;
					case 4:		// 副砲
						subguncnt++;
						break;
					case 10:	// 水上偵察機
					case 11:	// 水上爆撃機
						reconcnt++;
						break;
					case 12:	// 小型電探
					case 13:	// 大型電探
						radarcnt++;
						break;
					case 19:	// 対艦強化弾
						apshellcnt++;
						break;
					case 37:	// 対地装備
						rocketcnt++;
						break;

				}
			}

			if ( reconcnt > 0 && includeSpecialAttack ) {
				if ( mainguncnt == 2 && apshellcnt == 1 )
					return DayAttackKind.CutinMainMain;
				else if ( mainguncnt == 1 && subguncnt == 1 && apshellcnt == 1 )
					return DayAttackKind.CutinMainAP;
				else if ( mainguncnt == 1 && subguncnt == 1 && radarcnt == 1 )
					return DayAttackKind.CutinMainLadar;
				else if ( mainguncnt >= 1 && subguncnt >= 1 )
					return DayAttackKind.CutinMainSub;
				else if ( mainguncnt >= 2 )
					return DayAttackKind.DoubleShelling;
			}


			ShipDataMaster atkship = KCDatabase.Instance.MasterShips[attackerShipID];
			ShipDataMaster defship = KCDatabase.Instance.MasterShips[defenderShipID];

			if ( atkship != null ) {

				if ( defship != null ) {

					int landingID = GetLandingAttackKind( slot, attackerShipID, defenderShipID );
					if ( landingID > 0 ) {
						return (DayAttackKind)( (int)DayAttackKind.LandingDaihatsu + landingID - 1 );
					}

					if ( rocketcnt > 0 && defship.IsLandBase )
						return DayAttackKind.Rocket;
				}


				if ( attackerShipID == 352 ) {	//速吸改

					if ( defship != null && ( defship.IsSubmarine ) ) {
						if ( slot.Select( id => KCDatabase.Instance.MasterEquipments[id] )
							.Count( eq => eq != null && ( ( eq.CategoryType == 8 && eq.ASW > 0 ) || eq.CategoryType == 11 || eq.CategoryType == 25 ) ) > 0 )
							return DayAttackKind.AirAttack;		// 対潜攻撃において、( 対潜>0の艦上攻撃機 or 水上爆撃機 or オートジャイロ ) を装備している場合
						else
							return DayAttackKind.DepthCharge;

					} else if ( slot.Select( id => KCDatabase.Instance.MasterEquipments[id] ).Count( eq => eq != null && eq.CategoryType == 8 ) > 0 )
						return DayAttackKind.AirAttack;
					else
						return DayAttackKind.Shelling;

				} else if ( atkship.ShipType == 7 || atkship.ShipType == 11 || atkship.ShipType == 18 )		//軽空母/正規空母/装甲空母
					return DayAttackKind.AirAttack;

				else if ( defship != null && defship.IsSubmarine )
					if ( atkship.ShipType == 6 || atkship.ShipType == 10 ||
						 atkship.ShipType == 16 || atkship.ShipType == 17 )			//航空巡洋艦/航空戦艦/水上機母艦/揚陸艦
						return DayAttackKind.AirAttack;
					else
						return DayAttackKind.DepthCharge;

				//本来の雷撃は発生しない
				else if ( atkship.IsSubmarine )
					return DayAttackKind.Torpedo;			//(特例措置, 本来のコード中には存在しない)

			}

			return DayAttackKind.Shelling;		//砲撃
		}




		/// <summary>
		/// 夜戦における攻撃種別を取得します。
		/// </summary>
		/// <param name="slot">攻撃艦のスロット(マスターID)。</param>
		/// <param name="attackerShipID">攻撃艦の艦船ID。</param>
		/// <param name="defenderShipID">防御艦の艦船ID。なければ-1</param>
		/// <param name="includeSpecialAttack">カットイン/連撃の判定を含むか。falseなら除外して計算</param>
		public static NightAttackKind GetNightAttackKind( int[] slot, int attackerShipID, int defenderShipID, bool includeSpecialAttack = true ) {

			int mainguncnt = 0;
			int subguncnt = 0;
			int torpcnt = 0;
			int rocketcnt = 0;
			int latetorpcnt = 0;
			int subeqcnt = 0;

			if ( slot == null ) return NightAttackKind.Unknown;

			for ( int i = 0; i < slot.Length; i++ ) {
				EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[slot[i]];
				if ( eq == null ) continue;

				int eqtype = eq.EquipmentType[2];

				switch ( eqtype ) {
					case 1:
					case 2:
					case 3:		//主砲
						mainguncnt++;
						break;
					case 4:		//副砲
						subguncnt++;
						break;
					case 5:
					case 32:	//魚雷
						torpcnt++;
						if ( LateModelTorpedoIDs.Contains( eq.EquipmentID ) )	// 後期魚雷
							latetorpcnt++;
						break;
					case 37:	// 対地装備
						rocketcnt++;
						break;
					case 51:	// 潜水艦装備
						subeqcnt++;
						break;
				}

			}


			if ( includeSpecialAttack ) {

				if ( torpcnt >= 2 || ( latetorpcnt >= 1 && subeqcnt >= 1 ) )
					return NightAttackKind.CutinTorpedoTorpedo;
				else if ( mainguncnt >= 3 )
					return NightAttackKind.CutinMainMain;
				else if ( mainguncnt == 2 && subguncnt > 0 )
					return NightAttackKind.CutinMainSub;
				else if ( ( mainguncnt == 2 && subguncnt == 0 && torpcnt == 1 ) || ( mainguncnt == 1 && torpcnt == 1 ) )
					return NightAttackKind.CutinMainTorpedo;
				else if ( ( mainguncnt == 2 && subguncnt == 0 & torpcnt == 0 ) ||
					( mainguncnt == 1 && subguncnt > 0 ) ||
					( subguncnt >= 2 && torpcnt <= 1 ) ) {
					return NightAttackKind.DoubleShelling;
				}

			}


			ShipDataMaster atkship = KCDatabase.Instance.MasterShips[attackerShipID];
			ShipDataMaster defship = KCDatabase.Instance.MasterShips[defenderShipID];

			if ( atkship != null ) {

				if ( defship != null ) {

					int landingID = GetLandingAttackKind( slot, attackerShipID, defenderShipID );
					if ( landingID > 0 ) {
						return (NightAttackKind)( (int)NightAttackKind.LandingDaihatsu + landingID - 1 );
					}

					if ( rocketcnt > 0 && defship.IsLandBase )
						return NightAttackKind.Rocket;
				}

				if ( atkship.ShipType == 7 && defship != null && defship.IsSubmarine )
					return NightAttackKind.DepthCharge;

				if ( atkship.ShipType == 7 || atkship.ShipType == 11 || atkship.ShipType == 18 ) {		//軽空母/正規空母/装甲空母

					if ( attackerShipID == 432 || attackerShipID == 353 || attackerShipID == 433 )		//Graf Zeppelin(改), Saratoga
						return NightAttackKind.Shelling;
					else if ( atkship.Name == "リコリス棲姫" || atkship.Name == "深海海月姫" )
						return NightAttackKind.Shelling;
					else
						return NightAttackKind.AirAttack;

				} else if ( atkship.IsSubmarine )
					return NightAttackKind.Torpedo;

				else if ( defship != null && ( defship.IsSubmarine ) )			//潜水艦/潜水空母
					if ( atkship.ShipType == 6 || atkship.ShipType == 10 ||
						 atkship.ShipType == 16 || atkship.ShipType == 17 )			//航空巡洋艦/航空戦艦/水上機母艦/揚陸艦
						return NightAttackKind.AirAttack;
					else
						return NightAttackKind.DepthCharge;

				else if ( slot.Length > 0 ) {
					EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[slot[0]];
					if ( eq != null && ( eq.CategoryType == 5 || eq.CategoryType == 32 ) ) {		//最初のスロット==魚雷		(本来の判定とは微妙に異なるが無問題)
						return NightAttackKind.Torpedo;
					}
				}

			}

			return NightAttackKind.Shelling;

		}


		/// <summary>
		/// 夜戦魚雷カットインにおいて後期魚雷として扱われる装備のID群
		/// </summary>
		public static readonly int[] LateModelTorpedoIDs = new int[] { 
			213,		// 後期型艦首魚雷(6門)
			214,		// 熟練聴音員+後期型艦首魚雷(6門)
		};


		/// <summary>
		/// 夜戦カットインにおける魚雷カットインの種別を取得します。
		/// </summary>
		/// <param name="slot">攻撃艦のスロット(マスターID)。</param>
		/// <param name="attackerShipID">攻撃艦の艦船ID。</param>
		/// <param name="defenerShipID">防御艦の艦船ID。なければ-1</param>
		/// <returns> 0=その他, 1=後期魚雷+潜水艦装備(x1.75), 2=後期魚雷x2(x1.6)</returns>
		public static int GetNightTorpedoCutinKind( int[] slot, int attackerShipID, int defenderShipID ) {

			// note: 発動優先度については要検証
			int latetorp = slot.Intersect( LateModelTorpedoIDs ).Count();
			int subeq = slot.Select( id => KCDatabase.Instance.MasterEquipments[id] ).Count( eq => eq != null && eq.CategoryType == 51 );

			if ( latetorp >= 1 && subeq >= 1 )
				return 1;		// x1.75
			else if ( latetorp >= 2 )
				return 2;		// x1.6

			return 0;
		}


		/// <summary>
		/// 揚陸攻撃における攻撃種別を取得します。
		/// </summary>
		/// <param name="slot">攻撃艦のスロット(マスターID)。</param>
		/// <param name="attackerShipID">攻撃艦の艦船ID。</param>
		/// <param name="defenerShipID">防御艦の艦船ID。なければ-1</param>
		public static int GetLandingAttackKind( int[] slot, int attackerShipID, int defenderShipID ) {
			var attacker = KCDatabase.Instance.MasterShips[attackerShipID];
			var defender = KCDatabase.Instance.MasterShips[defenderShipID];

			if ( slot.Contains( 230 ) && defender.IsLandBase )		// 特大発動艇+戦車第11連隊
				return 5;

			if ( slot.Contains( 167 ) ) {		// 特二式内火艇
				if ( attacker.IsSubmarine ) {		// 潜水系
					if ( defender.IsLandBase )
						return 4;
				} else if ( HardInstallationNames.Contains( defender.Name ) )
					return 4;
			}

			if ( HardInstallationNames.Contains( defender.Name ) ) {

				if ( slot.Contains( 166 ) )		// 大発動艇(八九式中戦車&陸戦隊)
					return 3;

				if ( slot.Contains( 193 ) )		// 特大発動艇
					return 2;

				if ( slot.Contains( 68 ) )		// 大発動艇
					return 1;
			}

			return 0;
		}



		/// <summary>
		/// 対空カットイン種別を取得します。
		/// </summary>
		public static int GetAACutinKind( int shipID, int[] slot ) {

			int highangle = 0;
			int highangle_director = 0;
			int director = 0;
			int radar = 0;
			int aaradar = 0;
			int maingunl = 0;
			int aashell = 0;
			int aagun = 0;
			int aagun_concentrated = 0;


			foreach ( int eid in slot ) {

				EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[eid];
				if ( eq == null ) continue;

				if ( eq.IconType == 16 ) {	//高角砲
					if ( eq.AA >= 8 )
						highangle_director++;

					highangle++;

				} else if ( eq.CategoryType == 36 ) {	//高射装置
					director++;

				} else if ( eq.CardType == 8 ) {	//電探
					if ( eq.AA >= 2 ) {
						aaradar++;
					}
					radar++;

				} else if ( eq.CategoryType == 3 ) {	//大口径主砲
					maingunl++;

				} else if ( eq.CategoryType == 18 ) {	//対空強化弾
					aashell++;

				} else if ( eq.CategoryType == 21 ) {	//対空機銃
					if ( eq.AA >= 9 )
						aagun_concentrated++;

					aagun++;

				}

			}


			// 固有カットイン
			switch ( shipID ) {

				case 421:	//秋月
				case 330:	//秋月改
				case 422:	//照月
				case 346:	//照月改
				case 423:	//初月
				case 357:	//初月改
					if ( highangle >= 2 && radar >= 1 ) {
						return 1;
					}
					if ( highangle >= 1 && radar >= 1 ) {
						return 2;
					}
					if ( highangle >= 2 ) {
						return 3;
					}
					break;

				case 428:	//摩耶改二
					if ( highangle >= 1 && aagun_concentrated >= 1 ) {
						if ( aaradar >= 1 )
							return 10;

						return 11;
					}
					break;

				case 141:	//五十鈴改二
					if ( highangle >= 1 && aagun >= 1 ) {
						if ( aaradar >= 1 )
							return 14;
						else
							return 15;
					}
					break;

				case 470:	//霞改二乙
					if ( highangle >= 1 && aagun >= 1 ) {
						if ( aaradar >= 1 )
							return 16;
						else
							return 17;
					}
					break;

				case 418:	//皐月改二
					if ( aagun_concentrated >= 1 )
						return 18;
					break;

				case 487:	//鬼怒改二
					if ( aagun_concentrated >= 1 ) {
						if ( highangle - highangle_director >= 1 )
							return 19;
						return 20;
					}
					break;
			}



			if ( maingunl >= 1 && aashell >= 1 && director >= 1 && aaradar >= 1 ) {
				return 4;
			}
			if ( highangle_director >= 2 && aaradar >= 1 ) {
				return 5;
			}
			if ( maingunl >= 1 && aashell >= 1 && director >= 1 ) {
				return 6;
			}
			if ( highangle >= 1 && director >= 1 && aaradar >= 1 ) {
				return 7;
			}
			if ( highangle_director >= 1 && aaradar >= 1 ) {
				return 8;
			}
			if ( highangle >= 1 && director >= 1 ) {
				return 9;
			}

			if ( aagun_concentrated >= 1 && aagun >= 2 && aaradar >= 1 ) {	//注: 機銃2なのは集中機銃がダブるため
				return 12;
			}

			return 0;
		}



		/// <summary>
		/// 加重対空値を求めます。
		/// </summary>
		public static double GetAdjustedAAValue( ShipData ship ) {
			int equippedModifier = ship.SlotInstance.Any( s => s != null ) ? 2 : 1;

			double x = ship.AABase;

			foreach ( var eq in ship.AllSlotInstance ) {
				if ( eq == null )
					continue;

				var eqmaster = eq.MasterEquipment;

				double equipmentBonus;
				if ( eqmaster.IconType == 16 || eqmaster.CategoryType == 36 )	// 高角砲・高射装置
					equipmentBonus = 4;
				else if ( eqmaster.CategoryType == 21 )		// 機銃
					equipmentBonus = 6;
				else if ( eqmaster.CategoryType == 12 || eqmaster.CategoryType == 13 )		// 小型電探・大型電探
					equipmentBonus = 3;
				else
					equipmentBonus = 0;

				double levelBonus;
				if ( eqmaster.IconType == 16 )	// 高角砲
					levelBonus = 3;
				else if ( eqmaster.CategoryType == 21 )		// 機銃
					levelBonus = 4;
				else
					levelBonus = 0;

				x += eqmaster.AA * equipmentBonus + Math.Sqrt( eq.Level ) * levelBonus;

			}

			return equippedModifier * Math.Floor( x / equippedModifier );
		}


		/// <summary>
		/// 艦隊防空値を求めます。
		/// </summary>
		public static double GetAdjustedFleetAAValue( IEnumerable<ShipData> ships, int formation ) {
			double formationBonus;
			switch ( formation ) {
				case 2:		// 複縦陣
					formationBonus = 1.2;
					break;
				case 3:		// 輪形陣
					formationBonus = 1.6;
					break;
				case 11:	// 第一警戒航行序列
					formationBonus = 1.1;
					break;
				case 13:	// 第三警戒航行序列
					formationBonus = 1.5;
					break;
				default:
					formationBonus = 1.0;
					break;
			}

			double fleetAABonus = 0;
			foreach ( var ship in ships ) {
				if ( ship == null )
					continue;

				double shipAABonus = 0;
				foreach ( var eq in ship.AllSlotInstance ) {
					if ( eq == null )
						continue;

					var eqmaster = eq.MasterEquipment;
					double equipmentBonus;
					if ( eqmaster.IconType == 16 || eqmaster.CategoryType == 36 )		// 高角砲・高射装置
						equipmentBonus = 0.35;
					else if ( eqmaster.CategoryType == 12 || eqmaster.CategoryType == 13 )	// 小型電探・大型電探
						equipmentBonus = 0.4;
					else if ( eqmaster.CategoryType == 18 )		// 対空強化弾
						equipmentBonus = 0.6;
					else
						equipmentBonus = 0.2;

					double levelBonus;
					if ( eqmaster.IconType == 16 )		// 高角砲
						levelBonus = 3.0;
					else if ( eqmaster.CategoryType == 36 )		// 高射装置
						levelBonus = 2.0;
					else if ( eqmaster.CategoryType == 12 || eqmaster.CategoryType == 13 )	// 小型電探・大型電探
						levelBonus = 1.5;
					else
						levelBonus = 0.0;

					shipAABonus += eqmaster.AA * equipmentBonus + Math.Sqrt( eq.Level ) * levelBonus;
				}

				fleetAABonus += Math.Floor( shipAABonus );
			}

			return Math.Floor( formationBonus * fleetAABonus ) * 2 / 1.3;
		}

		/// <summary>
		/// 艦隊防空値を求めます。
		/// </summary>
		public static double GetAdjustedFleetAAValue( FleetData fleet, int formation ) {
			return GetAdjustedFleetAAValue( fleet.MembersWithoutEscaped, formation );
		}


		/// <summary>
		/// 対空砲火における連合艦隊補正を求めます。
		/// </summary>
		/// <param name="combinedFleetFlag">連合艦隊フラグ。 -1=連合艦隊でない, 1=連合艦隊主力艦隊, 2=連合艦隊随伴艦隊</param>
		public static double GetAirDefenseCombinedFleetCoefficient( int combinedFleetFlag ) {
			switch ( combinedFleetFlag ) {
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
		public static double GetProportionalAirDefense( double adjustedAAValue, int combinedFleetFlag = -1 ) {
			return adjustedAAValue * GetAirDefenseCombinedFleetCoefficient( combinedFleetFlag ) / 400;
		}

		/// <summary>
		/// 固定撃墜を求めます。
		/// </summary>
		/// <param name="adjustedAAValue">加重対空値</param>
		/// <param name="adjustedFleetAAValue">艦隊防空値</param>
		/// <param name="cutinKind">対空カットイン種別</param>
		/// <param name="combinedFleetFlag">連合艦隊フラグ。 -1=連合艦隊でない, 1=連合艦隊主力艦隊, 2=連合艦隊随伴艦隊</param>
		public static int GetFixedAirDefense( double adjustedAAValue, double adjustedFleetAAValue, int cutinKind, int combinedFleetFlag = -1 ) {
			double cutinBonus = Calculator.AACutinVariableBonus.ContainsKey( cutinKind ) ? Calculator.AACutinVariableBonus[cutinKind] : 1.0;

			return (int)Math.Floor( ( adjustedAAValue + adjustedFleetAAValue ) * GetAirDefenseCombinedFleetCoefficient( combinedFleetFlag ) * cutinBonus / 10 );
		}




		/// <summary>
		/// 対空カットイン固定ボーナス
		/// </summary>
		public static readonly ReadOnlyDictionary<int, int> AACutinFixedBonus = new ReadOnlyDictionary<int, int>( new Dictionary<int, int>() { 
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
		} );


		/// <summary>
		/// 対空カットイン変動ボーナス
		/// </summary>
		public static readonly ReadOnlyDictionary<int, double> AACutinVariableBonus = new ReadOnlyDictionary<int, double>( new Dictionary<int, double>() {
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
		} );


		/// <summary>
		/// 撃墜数の推定値を求めます。
		/// </summary>
		/// <param name="enemyAircraftCount">敵航空中隊の機数</param>
		/// <param name="proportionalAirDefense">割合撃墜の割合</param>
		/// <param name="fixedAirDefense">固定撃墜</param>
		/// <param name="aaCutinKind">発動した対空カットインの種類</param>
		public static int GetShootDownCount( int enemyAircraftCount, double proportionalAirDefense, int fixedAirDefense, int aaCutinKind ) {
			return (int)Math.Floor( enemyAircraftCount * proportionalAirDefense ) + fixedAirDefense + 1 + ( AACutinFixedBonus.ContainsKey( aaCutinKind ) ? AACutinFixedBonus[aaCutinKind] : 0 );
		}




		/// <summary>
		/// 装備が艦載機であるかを取得します。
		/// </summary>
		/// <param name="equipmentID">装備ID。</param>
		/// <param name="containsRecon">偵察機(非攻撃機)を含めるか。</param>
		/// <param name="containsASWAircraft">対潜可能機を含めるか。</param>
		public static bool IsAircraft( int equipmentID, bool containsRecon, bool containsASWAircraft = false ) {

			var eq = KCDatabase.Instance.MasterEquipments[equipmentID];

			if ( eq == null ) return false;

			switch ( eq.CategoryType ) {
				case 6:		// 艦上戦闘機
				case 7:		// 艦上爆撃機
				case 8:		// 艦上攻撃機
				case 11:	// 水上爆撃機
				case 25:	// オートジャイロ
				case 26:	// 対潜哨戒機
				case 45:	// 水上戦闘機
				case 47:	// 陸上攻撃機
				case 48:	// 局地戦闘機
				case 56:	// 噴式戦闘機
				case 57:	// 噴式戦闘爆撃機
				case 58:	// 噴式攻撃機
					return true;

				case 9:		// 艦上偵察機
				case 10:	// 水上偵察機
				case 59:	// 噴式偵察機
					return containsRecon;

				case 41:	//大型飛行艇
					return containsRecon || containsASWAircraft;
				default:
					return false;
			}

		}



		/// <summary>
		/// 対潜攻撃可能であるかを取得します。
		/// </summary>
		/// <param name="ship">対象の艦船データ。</param>
		public static bool CanAttackSubmarine( ShipData ship ) {

			switch ( ship.MasterShip.ShipType ) {
				case 1:		//海防
				case 2:		//駆逐
				case 3:		//軽巡
				case 4:		//雷巡
				case 21:	//練巡
				case 22:	//補給
					return ship.ASWBase > 0;

				case 6:		//航巡
				case 7:		//軽空母
				case 10:	//航戦
				case 16:	//水母
				case 17:	//揚陸
					return ship.SlotInstanceMaster.Count( eq => eq != null && IsAircraft( eq.EquipmentID, false, true ) && eq.ASW > 0 ) > 0;

				default:
					return false;
			}

		}


		public static TimeSpan CalculateDockingUnitTime( ShipData ship ) {
			return new TimeSpan( DateTimeHelper.FromAPITimeSpan( ship.RepairTime ).Add( TimeSpan.FromSeconds( -30 ) ).Ticks / ( ship.HPMax - ship.HPCurrent ) );
		}

	}


	public enum DayAttackKind {
		Unknown = -1,

		Shelling,
		Laser,
		DoubleShelling,
		CutinMainSub,
		CutinMainLadar,
		CutinMainAP,
		CutinMainMain,
		AirAttack,
		DepthCharge,
		Torpedo,

		Rocket,

		LandingDaihatsu,
		LandingTokuDaihatsu,
		LandingDaihatsuTank,
		LandingAmphibious,
		LandingTokuDaihatsuTank,
	}

	public enum NightAttackKind {
		Unknown = -1,

		Shelling,
		DoubleShelling,
		CutinMainTorpedo,
		CutinTorpedoTorpedo,
		CutinMainSub,
		CutinMainMain,
		Reserved,
		AirAttack,
		DepthCharge,
		Torpedo,

		Rocket,

		LandingDaihatsu,
		LandingTokuDaihatsu,
		LandingDaihatsuTank,
		LandingAmphibious,
		LandingTokuDaihatsuTank,
	}

}
