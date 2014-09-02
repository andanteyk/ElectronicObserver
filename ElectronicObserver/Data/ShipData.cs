using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 個別の艦娘データを保持します。
	/// </summary>
	public class ShipData : IIdentifiable, IResponseLoader {

		//マスターデータか何かに定義しておくといいかも
		//public static const int SlotMax = 5;


		/// <summary>
		/// 艦娘を一意に識別するID
		/// </summary>
		public int MasterID { get; private set; }

		/// <summary>
		/// 並べ替えの順番
		/// </summary>
		public int SortID { get; private set; }

		/// <summary>
		/// 艦船ID
		/// </summary>
		public int ShipID { get; private set; }

		/// <summary>
		/// レベル
		/// </summary>
		public int Level { get; private set; }

		/// <summary>
		/// 累積経験値
		/// </summary>
		public int ExpTotal { get; private set; }

		/// <summary>
		/// 次のレベルに達するために必要な経験値
		/// </summary>
		public int ExpNext { get; private set; }

		/// <summary>
		/// パラメータ
		/// </summary>
		public ParameterBase Param { get; private set; }

		//TODO: propertize
		/// <summary>
		/// 装備スロット
		/// </summary>
		public EquipmentData[] Slot;

		//TODO: propertize
		/// <summary>
		/// 各スロットの航空機搭載量
		/// </summary>
		public Fraction[] Aircraft;

		/// <summary>
		/// 搭載燃料
		/// </summary>
		public Fraction Fuel { get; private set; }

		/// <summary>
		/// 搭載弾薬
		/// </summary>
		public Fraction Ammo { get; private set; }

		/// <summary>
		/// 入渠にかかる時間
		/// </summary>
		public int RepairTime { get; private set; }

		/// <summary>
		/// 入渠にかかる鋼材
		/// </summary>
		public int RepairSteel { get; private set; }
		
		/// <summary>
		/// 入渠にかかる燃料
		/// </summary>
		public int RepairFuel { get; private set; }

		/// <summary>
		/// コンディション
		/// </summary>
		public int Condition { get; private set; }

		/// <summary>
		/// 保護ロックの有無
		/// </summary>
		public bool IsLocked { get; private set; }

		/// <summary>
		/// 出撃海域
		/// 備考：2014/09/02現在、この値は削除されています
		/// </summary>
		public int SallyArea { get; private set; }



		public int ID {
			get { return MasterID; }
		}

		public bool LoadFromResponse( string apiname, dynamic data ) {
			throw new NotImplementedException();
		}

		
	}


}

