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
			get { return (int)RawData.api_ndock_item[0]; }
		}

		/// <summary>
		/// 入渠にかかる燃料
		/// </summary>
		public int RepairFuel {
			get { return (int)RawData.api_ndock_item[1]; }
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
		/// 火力基本値
		/// </summary>
		public int FirepowerBase {
			get {	//該当IDが存在しなければぬるぽするだろうけど、そんな状況では公式蔵も動かないだろうから問題なし（？）
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
		/// 所属していなければ null
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
				return null;
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
		/// 補強装備スロットが使用可能か
		/// </summary>
		public bool IsExpansionSlotAvailable {
			get { return ExpansionSlot != 0; }
		}


		public int ID {
			get { return MasterID; }
		}


		public override void LoadFromResponse( string apiname, dynamic data ) {

			switch ( apiname ) {
				case "api_port/port":
				case "api_get_member/ship2":
				case "api_get_member/ship3":
				case "api_req_kousyou/getship":
				case "api_get_member/ship_deck":
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

