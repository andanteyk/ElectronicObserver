using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 個別の艦娘データを保持します。
	/// </summary>
	[DebuggerDisplay( "[{ID}] {KCDatabase.Instance.MasterShips[ShipID].NameWithClass} Lv. {Level}" )]
	public class ShipData : APIWrapper, IIdentifiable {



		/// <summary>
		/// 艦娘を一意に識別するID
		/// </summary>
		public int MasterID {
			get { return (int)RawData.api_id; }
		}

		/// <summary>
		/// 並べ替えの順番
		/// </summary>
		public int SortID {
			get { return (int)RawData.api_sortno; }
		}

		/// <summary>
		/// 艦船ID
		/// </summary>
		public int ShipID {
			get { return (int)RawData.api_ship_id; }
		}

		/// <summary>
		/// レベル
		/// </summary>
		public int Level {
			get { return (int)RawData.api_lv; }
		}

		/// <summary>
		/// 累積経験値
		/// </summary>
		public int ExpTotal {
			get { return (int)RawData.api_exp[0]; }
		}

		/// <summary>
		/// 次のレベルに達するために必要な経験値
		/// </summary>
		public int ExpNext {
			get { return (int)RawData.api_exp[1]; }
		}


		/// <summary>
		/// 耐久現在値
		/// </summary>
		public int HPCurrent { get; internal set; }

		/// <summary>
		/// 耐久最大値
		/// </summary>
		public int HPMax {
			get { return (int)RawData.api_maxhp; }
		}

		/// <summary>
		/// 射程
		/// </summary>
		public int Range {
			get { return (int)RawData.api_leng; }
		}


		private int[] _slot;
		/// <summary>
		/// 装備スロット(ID)
		/// </summary>
		public ReadOnlyCollection<int> Slot {
			get { return Array.AsReadOnly<int>( _slot ); }
		}


		/// <summary>
		/// 装備スロット(マスターID)
		/// </summary>
		public ReadOnlyCollection<int> SlotMaster {
			get {
				if ( _slot == null ) return null;

				int[] s = new int[_slot.Length];

				for ( int i = 0; i < s.Length; i++ ) {
					EquipmentData eq = KCDatabase.Instance.Equipments[_slot[i]];
					if ( eq != null )
						s[i] = eq.EquipmentID;
					else
						s[i] = -1;
				}

				return Array.AsReadOnly<int>( s );
			}
		}

		/// <summary>
		/// 装備スロット(装備データ)
		/// </summary>
		public ReadOnlyCollection<EquipmentData> SlotInstance {
			get {
				if ( _slot == null ) return null;

				var s = new EquipmentData[_slot.Length];

				for ( int i = 0; i < s.Length; i++ ) {
					s[i] = KCDatabase.Instance.Equipments[_slot[i]];
				}

				return Array.AsReadOnly( s );
			}
		}

		/// <summary>
		/// 装備スロット(装備マスターデータ)
		/// </summary>
		public ReadOnlyCollection<EquipmentDataMaster> SlotInstanceMaster {
			get {
				if ( _slot == null ) return null;

				var s = new EquipmentDataMaster[_slot.Length];

				for ( int i = 0; i < s.Length; i++ ) {
					EquipmentData eq = KCDatabase.Instance.Equipments[_slot[i]];
					s[i] = eq != null ? eq.MasterEquipment : null;
				}

				return Array.AsReadOnly( s );
			}
		}


		/// <summary>
		/// 補強装備スロット(ID)
		/// 0=未開放, -1=装備なし 
		/// </summary>
		public int ExpansionSlot { get; private set; }

		/// <summary>
		/// 補強装備スロット(マスターID)
		/// </summary>
		public int ExpansionSlotMaster {
			get {
				if ( ExpansionSlot == 0 )
					return 0;

				EquipmentData eq = KCDatabase.Instance.Equipments[ExpansionSlot];
				if ( eq != null )
					return eq.EquipmentID;
				else
					return -1;
			}
		}

		/// <summary>
		/// 補強装備スロット(装備データ)
		/// </summary>
		public EquipmentData ExpansionSlotInstance {
			get { return KCDatabase.Instance.Equipments[ExpansionSlot]; }
		}

		/// <summary>
		/// 補強装備スロット(装備マスターデータ)
		/// </summary>
		public EquipmentDataMaster ExpansionSlotInstanceMaster {
			get {
				EquipmentData eq = ExpansionSlotInstance;
				return eq != null ? eq.MasterEquipment : null;
			}
		}


		/// <summary>
		/// 全てのスロット(ID)
		/// </summary>
		public ReadOnlyCollection<int> AllSlot {
			get {
				if ( _slot == null ) return null;

				int[] ret = new int[_slot.Length + 1];
				Array.Copy( _slot, ret, _slot.Length );
				ret[ret.Length - 1] = ExpansionSlot;
				return Array.AsReadOnly( ret );
			}
		}

		/// <summary>
		/// 全てのスロット(マスターID)
		/// </summary>
		public ReadOnlyCollection<int> AllSlotMaster {
			get {
				if ( _slot == null ) return null;

				var alls = AllSlot;
				int[] ret = new int[alls.Count];
				for ( int i = 0; i < ret.Length; i++ ) {
					var eq = KCDatabase.Instance.Equipments[alls[i]];
					if ( eq != null ) ret[i] = eq.EquipmentID;
					else ret[i] = -1;
				}

				return Array.AsReadOnly( ret );
			}
		}

		/// <summary>
		/// 全てのスロット(装備データ)
		/// </summary>
		public ReadOnlyCollection<EquipmentData> AllSlotInstance {
			get {
				if ( _slot == null ) return null;

				var alls = AllSlot;
				EquipmentData[] s = new EquipmentData[alls.Count];

				for ( int i = 0; i < s.Length; i++ ) {
					s[i] = KCDatabase.Instance.Equipments[alls[i]];
				}

				return Array.AsReadOnly( s );
			}
		}

		/// <summary>
		/// 全てのスロット(装備マスターデータ)
		/// </summary>
		public ReadOnlyCollection<EquipmentDataMaster> AllSlotInstanceMaster {
			get {
				if ( _slot == null ) return null;

				var alls = AllSlot;
				var s = new EquipmentDataMaster[alls.Count];

				for ( int i = 0; i < s.Length; i++ ) {
					EquipmentData eq = KCDatabase.Instance.Equipments[alls[i]];
					s[i] = eq != null ? eq.MasterEquipment : null;
				}

				return Array.AsReadOnly( s );
			}
		}



		private int[] _aircraft;
		/// <summary>
		/// 各スロットの航空機搭載量
		/// </summary>
		public ReadOnlyCollection<int> Aircraft {
			get { return Array.AsReadOnly<int>( _aircraft ); }
		}


		/// <summary>
		/// 現在の航空機搭載量
		/// </summary>
		public int AircraftTotal {
			get { return _aircraft.Sum( a => Math.Max( a, 0 ) ); }
		}


		/// <summary>
		/// 搭載燃料
		/// </summary>
		public int Fuel { get; internal set; }

		/// <summary>
		/// 搭載弾薬
		/// </summary>
		public int Ammo { get; internal set; }


		/// <summary>
		/// スロットのサイズ
		/// </summary>
		public int SlotSize {
			get { return !RawData.api_slotnum() ? 0 : (int)RawData.api_slotnum; }
		}

		/// <summary>
		/// 入渠にかかる時間(ミリ秒)
		/// </summary>
		public int RepairTime {
			get { return (int)RawData.api_ndock_time; }
		}

		/// <summary>
		/// 入渠にかかる鋼材
		/// </summary>
		public int RepairSteel {
			get { return (int)RawData.api_ndock_item[1]; }
		}

		/// <summary>
		/// 入渠にかかる燃料
		/// </summary>
		public int RepairFuel {
			get { return (int)RawData.api_ndock_item[0]; }
		}

		/// <summary>
		/// コンディション
		/// </summary>
		public int Condition { get; internal set; }


		#region Parameters

		/********************************************************
		 * 強化値：近代化改修・レベルアップによって上昇した数値
		 * 総合値：装備込みでのパラメータ
		 * 基本値：装備なしでのパラメータ(初期値+強化値)
		 ********************************************************/

		/// <summary>
		/// 火力強化値
		/// </summary>
		public int FirepowerModernized {
			get { return (int)RawData.api_kyouka[0]; }
		}

		/// <summary>
		/// 雷装強化値
		/// </summary>
		public int TorpedoModernized {
			get { return (int)RawData.api_kyouka[1]; }
		}

		/// <summary>
		/// 対空強化値
		/// </summary>
		public int AAModernized {
			get { return (int)RawData.api_kyouka[2]; }
		}

		/// <summary>
		/// 装甲強化値
		/// </summary>
		public int ArmorModernized {
			get { return (int)RawData.api_kyouka[3]; }
		}

		/// <summary>
		/// 運強化値
		/// </summary>
		public int LuckModernized {
			get { return (int)RawData.api_kyouka[4]; }
		}


		/// <summary>
		/// 火力改修残り
		/// </summary>
		public int FirepowerRemain {
			get { return ( MasterShip.FirepowerMax - MasterShip.FirepowerMin ) - FirepowerModernized; }
		}

		/// <summary>
		/// 雷装改修残り
		/// </summary>
		public int TorpedoRemain {
			get { return ( MasterShip.TorpedoMax - MasterShip.TorpedoMin ) - TorpedoModernized; }
		}

		/// <summary>
		/// 対空改修残り
		/// </summary>
		public int AARemain {
			get { return ( MasterShip.AAMax - MasterShip.AAMin ) - AAModernized; }
		}

		/// <summary>
		/// 装甲改修残り
		/// </summary>
		public int ArmorRemain {
			get { return ( MasterShip.ArmorMax - MasterShip.ArmorMin ) - ArmorModernized; }
		}

		/// <summary>
		/// 運改修残り
		/// </summary>
		public int LuckRemain {
			get { return ( MasterShip.LuckMax - MasterShip.LuckMin ) - LuckModernized; }
		}


		/// <summary>
		/// 火力総合値
		/// </summary>
		public int FirepowerTotal {
			get { return (int)RawData.api_karyoku[0]; }
		}

		/// <summary>
		/// 雷装総合値
		/// </summary>
		public int TorpedoTotal {
			get { return (int)RawData.api_raisou[0]; }
		}

		/// <summary>
		/// 対空総合値
		/// </summary>
		public int AATotal {
			get { return (int)RawData.api_taiku[0]; }
		}

		/// <summary>
		/// 装甲総合値
		/// </summary>
		public int ArmorTotal {
			get { return (int)RawData.api_soukou[0]; }
		}

		/// <summary>
		/// 回避総合値
		/// </summary>
		public int EvasionTotal {
			get { return (int)RawData.api_kaihi[0]; }
		}

		/// <summary>
		/// 対潜総合値
		/// </summary>
		public int ASWTotal {
			get { return (int)RawData.api_taisen[0]; }
		}

		/// <summary>
		/// 索敵総合値
		/// </summary>
		public int LOSTotal {
			get { return (int)RawData.api_sakuteki[0]; }
		}

		/// <summary>
		/// 運総合値
		/// </summary>
		public int LuckTotal {
			get { return (int)RawData.api_lucky[0]; }
		}

		/// <summary>
		/// 爆装総合値
		/// </summary>
		public int BomberTotal {
			get { return AllSlotInstanceMaster.Sum( s => s == null ? 0 : Math.Max( s.Bomber, 0 ) ); }
		}


		/// <summary>
		/// 火力基本値
		/// </summary>
		public int FirepowerBase {
			get {
				return MasterShip.FirepowerMin + FirepowerModernized;
			}
		}

		/// <summary>
		/// 雷装基本値
		/// </summary>
		public int TorpedoBase {
			get {
				return MasterShip.TorpedoMin + TorpedoModernized;
			}
		}

		/// <summary>
		/// 対空基本値
		/// </summary>
		public int AABase {
			get {
				return MasterShip.AAMin + AAModernized;
			}
		}

		/// <summary>
		/// 装甲基本値
		/// </summary>
		public int ArmorBase {
			get {
				return MasterShip.ArmorMin + ArmorModernized;
			}
		}

		/// <summary>
		/// 回避基本値
		/// </summary>
		public int EvasionBase {
			get {
				/*
				ShipDataMaster ship = KCDatabase.Instance.MasterShips[ShipID];
				return ship.EvasionMin + ( ship.EvasionMax - ship.EvasionMin ) * Level / 99;
				*/
				int param = EvasionTotal;
				foreach ( var eq in AllSlotInstance ) {
					if ( eq != null )
						param -= eq.MasterEquipment.Evasion;
				}
				return param;
			}
		}

		/// <summary>
		/// 対潜基本値
		/// </summary>
		public int ASWBase {
			get {
				int param = ASWTotal;
				foreach ( var eq in AllSlotInstance ) {
					if ( eq != null )
						param -= eq.MasterEquipment.ASW;
				}
				return param;
			}
		}

		/// <summary>
		/// 索敵基本値
		/// </summary>
		public int LOSBase {
			get {
				int param = LOSTotal;
				foreach ( var eq in AllSlotInstance ) {
					if ( eq != null )
						param -= eq.MasterEquipment.LOS;
				}
				return param;
			}
		}

		/// <summary>
		/// 運基本値
		/// </summary>
		public int LuckBase {
			get {
				return MasterShip.LuckMin + LuckModernized;
			}
		}


		/// <summary>
		/// 回避最大値
		/// </summary>
		public int EvasionMax {
			get { return (int)RawData.api_kaihi[1]; }
		}

		/// <summary>
		/// 対潜最大値
		/// </summary>
		public int ASWMax {
			get { return (int)RawData.api_taisen[1]; }
		}

		/// <summary>
		/// 索敵最大値
		/// </summary>
		public int LOSMax {
			get { return (int)RawData.api_sakuteki[1]; }
		}

		#endregion

		/// <summary>
		/// 爆装综合值
		/// </summary>
		public int BombTotal {
			get {
				int bomb = 0;
				for ( int i = 0; i < _slot.Length; i++ ) {
					if ( _slot[i] != -1 && KCDatabase.Instance.Equipments[_slot[i]] != null ) {
						bomb += KCDatabase.Instance.Equipments[_slot[i]].MasterEquipment.Bomber;
					}
				}
				return bomb;
			}
		}

		/// <summary>
		/// 保護ロックの有無
		/// </summary>
		public bool IsLocked {
			get { return (int)RawData.api_locked != 0; }
		}

		/// <summary>
		/// 装備による保護ロックの有無
		/// </summary>
		public bool IsLockedByEquipment {
			get { return (int)RawData.api_locked_equip != 0; }
		}


		//*/
		/// <summary>
		/// 出撃海域
		/// </summary>
		public int SallyArea {
			get {
				return RawData.api_sally_area() ? (int)RawData.api_sally_area : -1;
			}
		}
		//*/


		/// <summary>
		/// 艦船のマスターデータへの参照
		/// </summary>
		public ShipDataMaster MasterShip {
			get {
				return KCDatabase.Instance.MasterShips[ShipID];
			}
		}

		/// <summary>
		/// 入渠中のドックID　非入渠時は-1
		/// </summary>
		public int RepairingDockID {
			get {
				foreach ( var dock in KCDatabase.Instance.Docks.Values ) {
					if ( dock.ShipID == MasterID )
						return dock.DockID;
				}
				return -1;
			}
		}

		/// <summary>
		/// 所属艦隊　-1=なし
		/// </summary>
		public int Fleet {
			get {
				FleetManager fm = KCDatabase.Instance.Fleet;
				foreach ( var f in fm.Fleets.Values ) {
					if ( f.Members.Contains( MasterID ) )
						return f.FleetID;
				}
				return -1;
			}
		}


		/// <summary>
		/// 所属艦隊及びその位置
		/// ex. 1-3 (位置も1から始まる)
		/// 所属していなければ 空文字列
		/// </summary>
		public string FleetWithIndex {
			get {
				FleetManager fm = KCDatabase.Instance.Fleet;
				foreach ( var f in fm.Fleets.Values ) {
					int index = f.Members.IndexOf( MasterID );
					if ( index != -1 ) {
						return string.Format( "{0}-{1}", f.FleetID, index + 1 );
					}
				}
				return "";
			}

		}


		/// <summary>
		/// ケッコン済みかどうか
		/// </summary>
		public bool IsMarried {
			get { return Level > 99; }
		}


		/// <summary>
		/// 次の改装まで必要な経験値
		/// </summary>
		public int ExpNextRemodel {
			get {
				ShipDataMaster master = MasterShip;
				if ( master.RemodelAfterShipID <= 0 )
					return 0;
				return Math.Max( ExpTable.ShipExp[master.RemodelAfterLevel].Total - ExpTotal, 0 );
			}
		}


		/// <summary>
		/// 艦名
		/// </summary>
		public string Name {
			get { return MasterShip.Name; }
		}


		/// <summary>
		/// 艦名(レベルを含む)
		/// </summary>
		public string NameWithLevel {
			get { return string.Format( "{0} Lv. {1}", MasterShip.Name, Level ); }
		}


		/// <summary>
		/// HP/HPmax
		/// </summary>
		public double HPRate {
			get {
				if ( HPMax <= 0 ) return 0.0;
				return (double)HPCurrent / HPMax;
			}
		}


		/// <summary>
		/// 最大搭載燃料
		/// </summary>
		public int FuelMax {
			get { return MasterShip.Fuel; }
		}

		/// <summary>
		/// 最大搭載弾薬
		/// </summary>
		public int AmmoMax {
			get { return MasterShip.Ammo; }
		}


		/// <summary>
		/// 燃料残量割合
		/// </summary>
		public double FuelRate {
			get { return (double)Fuel / Math.Max( FuelMax, 1 ); }
		}

		/// <summary>
		/// 弾薬残量割合
		/// </summary>
		public double AmmoRate {
			get { return (double)Ammo / Math.Max( AmmoMax, 1 ); }
		}


		/// <summary>
		/// 搭載機残量割合
		/// </summary>
		public ReadOnlyCollection<double> AircraftRate {
			get {
				double[] airs = new double[_aircraft.Length];
				var airmax = MasterShip.Aircraft;

				for ( int i  = 0; i < airs.Length; i++ ) {
					airs[i] = (double)_aircraft[i] / Math.Max( airmax[i], 1 );
				}

				return Array.AsReadOnly( airs );
			}
		}

		/// <summary>
		/// 搭載機残量割合
		/// </summary>
		public double AircraftTotalRate {
			get { return (double)AircraftTotal / Math.Max( MasterShip.AircraftTotal, 1 ); }
		}





		/// <summary>
		/// 補強装備スロットが使用可能か
		/// </summary>
		public bool IsExpansionSlotAvailable {
			get { return ExpansionSlot != 0; }
		}



		#region ダメージ威力計算

		/// <summary>
		/// 航空戦威力
		/// 本来スロットごとのものであるが、ここでは最大火力を採用する
		/// </summary>
		public int AirBattlePower {
			get {
				double basepower = 0;
				var slots = SlotInstance;
				var aircrafts = Aircraft;
				for ( int i = 0; i < 5; i++ ) {
					double tmp = 0;
					if ( slots[i] == null )
						continue;

					switch ( slots[i].MasterEquipment.CategoryType ) {
						case 7:		//艦爆
						case 11:	//水爆
							tmp = slots[i].MasterEquipment.Bomber * Math.Sqrt( aircrafts[i] ) + 25;
							break;
						case 8:		//艦攻
							// 150% 補正を引いたとする
							tmp = ( slots[i].MasterEquipment.Torpedo * Math.Sqrt( aircrafts[i] ) + 25 ) * 1.5;
							break;
					}
					basepower = Math.Max( tmp, basepower );
				}

				//キャップ
				if ( basepower > 150 ) {
					basepower = 150 + Math.Sqrt( basepower - 150 );
				}
				basepower = Math.Floor( basepower );

				return (int)basepower;
			}
		}

		/// <summary>
		/// 砲撃威力
		/// </summary>
		public int ShellingPower {
			get {
				if ( Calculator.GetDayAttackKind( SlotMaster.ToArray(), ShipID, -1, false ) != 0 )
					return 0;		//砲撃以外は除外

				double basepower = FirepowerTotal + GetDayBattleEquipmentLevelBonus() + 5;

				basepower *= GetHPDamageBonus();

				basepower += GetLightCruiserDamageBonus();

				//キャップ
				if ( basepower > 150 ) {
					basepower = 150 + Math.Sqrt( basepower - 150 );
				}
				basepower = Math.Floor( basepower );


				//弾着
				switch ( Calculator.GetDayAttackKind( SlotMaster.ToArray(), ShipID, -1 ) ) {
					case 2:		//連撃
					case 4:		//主砲/電探
						basepower *= 1.2;
						break;
					case 3:		//主砲/副砲
						basepower *= 1.1;
						break;
					case 5:		//主砲/徹甲弾
						basepower *= 1.3;
						break;
					case 6:		//主砲/主砲
						basepower *= 1.5;
						break;
				}

				return (int)basepower;
			}
		}

		/// <summary>
		/// 空撃威力
		/// </summary>
		public int AircraftPower {
			get {
				if ( Calculator.GetDayAttackKind( SlotMaster.ToArray(), ShipID, -1, false ) != 7 )
					return 0;		//空撃以外は除外

				double basepower = Math.Floor( ( FirepowerTotal + TorpedoTotal + Math.Floor( BomberTotal * 1.3 ) + GetDayBattleEquipmentLevelBonus() ) * 1.5 ) + 55;

				basepower *= GetHPDamageBonus();

				//キャップ
				if ( basepower > 150 ) {
					basepower = 150 + Math.Sqrt( basepower - 150 );
				}
				basepower = Math.Floor( basepower );

				return (int)basepower;
			}
		}

		/// <summary>
		/// 対潜威力
		/// </summary>
		public int AntiSubmarinePower {
			get {

				if ( !Calculator.CanAttackSubmarine( this ) )
					return 0;

				double eqpower = 0;
				foreach ( var slot in SlotInstance ) {
					if ( slot == null )
						continue;

					switch ( slot.MasterEquipment.CategoryType ) {
						case 7:		//艦爆
						case 8:		//艦攻
						case 11:	//水爆
						case 14:	//ソナー
						case 15:	//爆雷
						case 25:	//オートジャイロ
						case 26:	//対潜哨戒機
						case 40:	//大型ソナー
							eqpower += slot.MasterEquipment.ASW;
							break;
					}
				}

				double basepower = Math.Sqrt( ASWBase ) * 2 + eqpower * 1.5 + GetAntiSubmarineEquipmentLevelBonus();
				if ( Calculator.GetDayAttackKind( SlotMaster.ToArray(), ShipID, 126, false ) == 7 ) {		//126=伊168; 対潜攻撃が空撃なら
					basepower += 8;
				} else {	//爆雷攻撃なら
					basepower += 13;
				}


				basepower *= GetHPDamageBonus();

				//対潜シナジー
				if ( SlotInstanceMaster.Where( s => s != null && ( s.CategoryType == 14 || s.CategoryType == 40 ) ).Any() &&		//ソナー or 大型ソナー
					 SlotInstanceMaster.Where( s => s != null && s.CategoryType == 15 ).Any() )			//爆雷
					basepower *= 1.15;

				//キャップ
				if ( basepower > 100 ) {
					basepower = 100 + Math.Sqrt( basepower - 100 );
				}
				basepower = Math.Floor( basepower );

				return (int)basepower;
			}
		}

		/// <summary>
		/// 雷撃威力
		/// </summary>
		public int TorpedoPower {
			get {
				if ( TorpedoBase == 0 )
					return 0;		//雷撃不能艦は除外

				double basepower = TorpedoTotal + GetTorpedoEquipmentLevelBonus() + 5;

				basepower *= GetHPDamageBonus();		//開幕雷撃は補正が違うが見なかったことに

				//キャップ
				if ( basepower > 150 ) {
					basepower = 150 + Math.Sqrt( basepower - 150 );
				}
				basepower = Math.Floor( basepower );

				return (int)basepower;
			}
		}

		/// <summary>
		/// 夜戦威力
		/// </summary>
		public int NightBattlePower {
			get {
				double basepower = FirepowerTotal + TorpedoTotal + GetNightBattleEquipmentLevelBonus();

				basepower *= GetHPDamageBonus();

				switch ( Calculator.GetNightAttackKind( SlotMaster.ToArray(), ShipID, -1 ) ) {
					case 1:	//連撃
						basepower *= 1.2;
						break;
					case 2:	//主砲/魚雷
						basepower *= 1.3;
						break;
					case 3:	//魚雷x2
						basepower *= 1.5;
						break;
					case 4:	//主砲x2/副砲
						basepower *= 1.75;
						break;
					case 5:	//主砲x3
						basepower *= 2.0;
						break;
				}

				basepower += GetLightCruiserDamageBonus();

				//キャップ
				if ( basepower > 300 ) {
					basepower = 300 + Math.Sqrt( basepower - 300 );
				}
				basepower = Math.Floor( basepower );

				return (int)basepower;
			}
		}


		/// <summary>
		/// 装備改修補正(砲撃戦)
		/// </summary>
		private double GetDayBattleEquipmentLevelBonus() {

			double basepower = 0;
			foreach ( var slot in SlotInstance ) {
				if ( slot == null )
					continue;

				switch ( slot.MasterEquipment.CategoryType ) {
					case 3:		//大口径主砲
					case 38:
						basepower += Math.Sqrt( slot.Level ) * 1.5;
						break;
					case 14:	//ソナー
					case 15:	//爆雷
						basepower += Math.Sqrt( slot.Level ) * 0.75;
						break;
					case 5:		//魚雷
					case 12:	//小型電探
					case 13:	//大型電探
					case 32:	//潜水艦魚雷
						break;	//無視
					default:
						basepower += Math.Sqrt( slot.Level );
						break;
				}
			}
			return basepower;
		}

		/// <summary>
		/// 装備改修補正(雷撃戦)
		/// </summary>
		private double GetTorpedoEquipmentLevelBonus() {
			double basepower = 0;
			foreach ( var slot in SlotInstance ) {
				if ( slot == null )
					continue;

				switch ( slot.MasterEquipment.CategoryType ) {
					case 5:		//魚雷
					case 32:	//潜水艦魚雷
						basepower += Math.Sqrt( slot.Level ) * 1.2;
						break;
					case 15:	//機銃
						basepower += Math.Sqrt( slot.Level );		// 2015/09/15 現在係数不明; とりあえず 1 とする
						break;
				}
			}
			return basepower;
		}

		/// <summary>
		/// 装備改修補正(対潜)
		/// </summary>
		private double GetAntiSubmarineEquipmentLevelBonus() {

			return SlotInstance.Sum( s => s == null ? 0.0 : ( s.MasterEquipment.CategoryType == 14 || s.MasterEquipment.CategoryType == 15 ) ? Math.Sqrt( s.Level ) : 0.0 );
		}

		/// <summary>
		/// 装備改修補正(夜戦)
		/// </summary>
		private double GetNightBattleEquipmentLevelBonus() {
			double basepower = 0;
			foreach ( var slot in SlotInstance ) {
				if ( slot == null )
					continue;

				switch ( slot.MasterEquipment.CategoryType ) {
					case 1:		//小口径主砲
					case 2:		//中口径主砲
					case 3:		//大口径主砲
					case 4:		//副砲
					case 5:		//魚雷
					case 19:	//徹甲弾
					case 29:	//探照灯
					case 32:	//潜水艦魚雷
					case 36:	//高射装置
					case 38:	//大口径主砲(II)
						basepower += Math.Sqrt( slot.Level );
						break;
				}
			}
			return basepower;
		}


		/// <summary>
		/// 耐久値による攻撃力補正
		/// </summary>
		private double GetHPDamageBonus() {
			if ( HPRate < 0.25 )
				return 0.4;
			else if ( HPRate < 0.5 )
				return 0.7;
			else
				return 1.0;
		}


		/// <summary>
		/// 軽巡軽量砲補正
		/// </summary>
		private double GetLightCruiserDamageBonus() {
			if ( MasterShip.ShipType == 3 ||
				MasterShip.ShipType == 4 ||
				MasterShip.ShipType == 21 ) {	//軽巡/雷巡/練巡

				int single = 0;
				int twin = 0;

				foreach ( var slot in SlotMaster ) {
					if ( slot == -1 ) continue;

					switch ( slot ) {
						case 4:		//14cm単装砲
						case 11:	//15.2cm単装砲
							single++;
							break;
						case 65:	//15.2cm連装砲
						case 119:	//14cm連装砲
						case 139:	//15.2cm連装砲改
							twin++;
							break;
					}
				}

				return Math.Sqrt( twin ) * 2.0 + Math.Sqrt( single );
			}

			return 0;
		}

		#endregion




		public int ID {
			get { return MasterID; }
		}


		public override void LoadFromResponse( string apiname, dynamic data ) {

			switch ( apiname ) {
				default:
					base.LoadFromResponse( apiname, (object)data );

					HPCurrent = (int)RawData.api_nowhp;
					Fuel = (int)RawData.api_fuel;
					Ammo = (int)RawData.api_bull;
					Condition = (int)RawData.api_cond;
					_slot = (int[])RawData.api_slot;
					ExpansionSlot = (int)RawData.api_slot_ex;
					_aircraft = (int[])RawData.api_onslot;
					break;

				case "api_req_hokyu/charge":
					Fuel = (int)data.api_fuel;
					Ammo = (int)data.api_bull;
					_aircraft = (int[])data.api_onslot;
					break;

				case "api_req_kaisou/slot_exchange_index":
					_slot = (int[])data.api_slot;
					break;
			}

		}


		public override void LoadFromRequest( string apiname, Dictionary<string, string> data ) {
			base.LoadFromRequest( apiname, data );

			KCDatabase db = KCDatabase.Instance;

			switch ( apiname ) {
				case "api_req_kousyou/destroyship": {

						for ( int i = 0; i < _slot.Length; i++ ) {
							if ( _slot[i] == -1 ) continue;
							db.Equipments.Remove( _slot[i] );
						}
					} break;

				case "api_req_kaisou/open_exslot":
					ExpansionSlot = -1;
					break;
			}
		}


		/// <summary>
		/// 入渠完了時の処理を行います。
		/// </summary>
		internal void Repair() {

			HPCurrent = HPMax;
			Condition = Math.Max( Condition, 40 );

			RawData.api_ndock_time = 0;
			RawData.api_ndock_item[0] = 0;
			RawData.api_ndock_item[1] = 0;

		}

	}

}

