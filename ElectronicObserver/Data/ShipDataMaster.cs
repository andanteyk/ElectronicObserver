using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 艦船のマスターデータを保持します。
	/// </summary>
	public class ShipDataMaster : IIdentifiable, IResponseLoader {

		/// <summary>
		/// 艦船ID
		/// </summary>
		public int ShipID { get; private set; }

		/// <summary>
		/// 並べ替え順
		/// </summary>
		public int SortID { get; private set; }

		/// <summary>
		/// 名前
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// 読み
		/// </summary>
		public string NameReading { get; private set; }

		//TODO:shiptype

		/// <summary>
		/// 改装Lv.
		/// </summary>
		public int RemodelAfterLevel { get; private set; }

		//参照にしてもいいかも
		/// <summary>
		/// 改装後の艦船ID
		/// </summary>
		public int RemodelAfterShipID { get; private set; }

		/// <summary>
		/// 改装前の艦船ID
		/// </summary>
		public int RemodelBeforeShipID { get; private set; }

		/// <summary>
		/// 改装に必要な燃料
		/// </summary>
		public int RemodelAmmo { get; private set; }

		/// <summary>
		/// 改装に必要な鋼材
		/// </summary>
		public int RemodelSteel { get; private set; }

		/// <summary>
		/// パラメータ
		/// </summary>
		public ParameterBase Param { get; private set; }

		/// <summary>
		/// 速力
		/// </summary>
		public ShipSpeed Speed { get; private set; }

		/// <summary>
		/// 装備スロットの数
		/// </summary>
		public int SlotSize { get; private set; }

		//TODO:propertize
		/// <summary>
		/// 各スロットの航空機搭載数
		/// </summary>
		public int[] Aircraft;

		/// <summary>
		/// 初期装備のID
		/// </summary>
		public int[] DefaultSlot;

		/// <summary>
		/// 建造時間(分)
		/// </summary>
		public int BuildingTime { get; private set; }

		//TODO:propertize
		/// <summary>
		/// 解体資材
		/// </summary>
		public int[] Material;

		/// <summary>
		/// 近代化改修の素材にしたとき上昇するパラメータの量
		/// </summary>
		public int[] PowerUp;

		/// <summary>
		/// レアリティ
		/// </summary>
		public ShipRarity Rarity { get; private set; }

		/// <summary>
		/// ドロップ/ログイン時のメッセージ
		/// </summary>
		public string MessageGet { get; private set; }

		/// <summary>
		/// 艦船名鑑でのメッセージ
		/// </summary>
		public string MessageDict { get; private set; }

		/// <summary>
		/// 搭載燃料
		/// </summary>
		public int Fuel { get; private set; }
		
		/// <summary>
		/// 搭載弾薬
		/// </summary>
		public int Ammo { get; private set; }



		public int ID {
			get { return ShipID; }
		}

		public bool LoadFromResponse( string apiname, dynamic data ) {
			throw new NotImplementedException();
		}

		
	}


}
