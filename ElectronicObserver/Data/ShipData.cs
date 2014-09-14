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
	[DebuggerDisplay( "[{ID}] : {KCDatabase.Instance.MasterShips[ShipID].Name} Lv. {Level}" )]
	public class ShipData : ResponseWrapper, IIdentifiable {

		

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
		public int HPCurrent {
			get { return (int)RawData.api_nowhp; }
		}

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


		/// <summary>
		/// 装備スロット
		/// </summary>
		public ReadOnlyCollection<int> Slot {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_slot ); }
		}
		
		/// <summary>
		/// 各スロットの航空機搭載量
		/// </summary>
		public ReadOnlyCollection<int> Aircraft {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_onslot ); }
		}



		/// <summary>
		/// 搭載燃料
		/// </summary>
		public int Fuel {
			get { return (int)RawData.api_fuel; }
		}

		/// <summary>
		/// 搭載弾薬
		/// </summary>
		public int Ammo {
			get { return (int)RawData.api_bull; }
		}

		/// <summary>
		/// 入渠にかかる時間
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
		public int Condition {
			get { return (int)RawData.api_cond; }
		}


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
		public int LuckyTotal {
			get { return (int)RawData.api_lucky[0]; }
		}


		/// <summary>
		/// 火力基本値
		/// </summary>
		public int FirepowerBase {
			get {	//該当IDが存在しなければぬるぽするだろうけど、そんな状況では公式蔵も動かないだろうから問題なし（？）
				return KCDatabase.Instance.MasterShips[ShipID].FirepowerMin + FirepowerModernized;
			}
		}

		/// <summary>
		/// 雷装基本値
		/// </summary>
		public int TorpedoBase {
			get {
				return KCDatabase.Instance.MasterShips[ShipID].TorpedoMin + TorpedoModernized;
			}
		}

		/// <summary>
		/// 対空基本値
		/// </summary>
		public int AABase {
			get {
				return KCDatabase.Instance.MasterShips[ShipID].AAMin + AAModernized;
			}
		}

		/// <summary>
		/// 装甲基本値
		/// </summary>
		public int ArmorBase {
			get {
				return KCDatabase.Instance.MasterShips[ShipID].ArmorMin + ArmorModernized;
			}
		}

		/// <summary>
		/// 回避基本値
		/// </summary>
		public int EvasionBase {
			get {
				ShipDataMaster ship = KCDatabase.Instance.MasterShips[ShipID];
				return ship.EvasionMin + ( ship.EvasionMax - ship.EvasionMin ) * Level / 99;
			}
		}

		/// <summary>
		/// 対潜基本値
		/// </summary>
		public int ASWBase {
			get {
				ShipDataMaster ship = KCDatabase.Instance.MasterShips[ShipID];
				return ship.ASWMin + ( ship.ASWMax - ship.ASWMin ) * Level / 99;
			}
		}

		/// <summary>
		/// 索敵基本値
		/// </summary>
		public int LOSBase {
			get {
				ShipDataMaster ship = KCDatabase.Instance.MasterShips[ShipID];
				return ship.LOSMin + ( ship.LOSMax - ship.LOSMin ) * Level / 99;
			}
		}

		/// <summary>
		/// 運基本値
		/// </summary>
		public int LuckBase {
			get {
				return KCDatabase.Instance.MasterShips[ShipID].LuckMin + LuckModernized;
			}
		}

		#endregion


		/// <summary>
		/// 保護ロックの有無
		/// </summary>
		public bool IsLocked {
			get { return RawData.api_locked != 0; }
		}

		//*/
		/// <summary>
		/// 出撃海域
		/// 備考：2014/09/02現在、この値は削除されています
		/// </summary>
		public int SallyArea {
			get {
				if ( RawData.api_sally_area() )
					return (int)RawData.api_sally_area;
				else
					return 0;
			}
		}
		//*/



		
		public int ID {
			get { return MasterID; }
		}



	}

}

