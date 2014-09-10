using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 個別の艦娘データを保持します。
	/// </summary>
	public class ShipData : ResponseWrapper, IIdentifiable {

		//マスターデータか何かに定義しておくといいかも
		//public static const int SlotMax = 5;


		/// <summary>
		/// 艦娘を一意に識別するID
		/// </summary>
		public int MasterID {
			get { return RawData.api_id; }
		}

		/// <summary>
		/// 並べ替えの順番
		/// </summary>
		public int SortID {
			get { return RawData.api_sortno; }
		}

		/// <summary>
		/// 艦船ID
		/// </summary>
		public int ShipID {
			get { return RawData.api_ship_id; }
		}

		/// <summary>
		/// レベル
		/// </summary>
		public int Level {
			get { return RawData.api_lv; }
		}

		/// <summary>
		/// 累積経験値
		/// </summary>
		public int ExpTotal {
			get { return RawData.api_exp[0]; }
		}

		/// <summary>
		/// 次のレベルに達するために必要な経験値
		/// </summary>
		public int ExpNext {
			get { return RawData.api_exp[1]; }
		}


		public int HPCurrent {
			get { return RawData.api_nowhp; }
		}

		public int HPMax {
			get { return RawData.api_maxhp; }
		}

		public int Range {
			get { return RawData.api_leng; }
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


		public int FirepowerModernized {
			get { return RawData.api_kyouka[0]; }
		}

		public int TorpedoModernized {
			get { return RawData.api_kyouka[1]; }
		}

		public int AAModernized {
			get { return RawData.api_kyouka[2]; }
		}

		public int ArmorModernized {
			get { return RawData.api_kyouka[3]; }
		}

		public int LuckModernized {
			get { return RawData.api_kyouka[4]; }
		}


		/// <summary>
		/// 搭載燃料
		/// </summary>
		public Fraction Fuel {
			get { return RawData.api_fuel; }
		}

		/// <summary>
		/// 搭載弾薬
		/// </summary>
		public Fraction Ammo {
			get { return RawData.api_bull; }
		}

		/// <summary>
		/// 入渠にかかる時間
		/// </summary>
		public int RepairTime {
			get { return RawData.api_ndock_time; }
		}

		/// <summary>
		/// 入渠にかかる鋼材
		/// </summary>
		public int RepairSteel {
			get { return RawData.api_ndock_item[0]; }
		}
		
		/// <summary>
		/// 入渠にかかる燃料
		/// </summary>
		public int RepairFuel {
			get { return RawData.api_ndock_item[1]; }
		}

		/// <summary>
		/// コンディション
		/// </summary>
		public int Condition {
			get { return RawData.api_cond; }
		}

		public int FirepowerTotal {
			get { return RawData.api_karyoku[0]; }
		}

		public int TorpedoTotal {
			get { return RawData.api_raisou[0]; }
		}

		public int AATotal {
			get { return RawData.api_taiku[0]; }
		}

		public int ArmorTotal {
			get { return RawData.api_soukou[0]; }
		}

		public int EvasionTotal {
			get { return RawData.api_kaihi[0]; }
		}

		public int ASWTotal {
			get { return RawData.api_taisen[0]; }
		}

		public int LOSTotal {
			get { return RawData.api_sakuteki[0]; }
		}

		public int LuckyTotal {
			get { return RawData.api_lucky[0]; }
		}

		/// <summary>
		/// 保護ロックの有無
		/// </summary>
		public bool IsLocked { get; private set; }

		//*/
		/// <summary>
		/// 出撃海域
		/// 備考：2014/09/02現在、この値は削除されています
		/// </summary>
		public int SallyArea {
			get {
				if ( RawData.api_sally_area() )
					return RawData.api_sally_area;
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

