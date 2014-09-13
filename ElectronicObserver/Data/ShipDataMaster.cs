using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 艦船のマスターデータを保持します。
	/// </summary>
	public class ShipDataMaster : ResponseWrapper, IIdentifiable {

		/// <summary>
		/// 艦船ID
		/// </summary>
		public int ShipID {
			get { return RawData.api_id; }
		}

		/// <summary>
		/// 並べ替え順
		/// </summary>
		public int SortID {
			get { return RawData.api_sortno; }
		}

		/// <summary>
		/// 名前
		/// </summary>
		public string Name {
			get { return RawData.api_name; }
		}

		/// <summary>
		/// 読み
		/// </summary>
		public string NameReading {
			get { return RawData.api_yomi; }
		}

		/// <summary>
		/// 艦種
		/// </summary>
		public int ShipType {
			get { return RawData.api_stype; }
		}


		/// <summary>
		/// 改装Lv.
		/// </summary>
		public int RemodelAfterLevel {
			get { return RawData.api_afterlv; }
		}

		//参照にしてもいいかも
		/// <summary>
		/// 改装後の艦船ID
		/// </summary>
		public int RemodelAfterShipID {
			get { return int.Parse( (string)RawData.api_aftershipid ); }
		}

		//FIXME
		/// <summary>
		/// 改装前の艦船ID
		/// </summary>
		public int RemodelBeforeShipID { get; internal set; }

		/// <summary>
		/// 改装に必要な燃料
		/// </summary>
		public int RemodelAmmo {
			get { return RawData.api_afterfuel; }
		}

		/// <summary>
		/// 改装に必要な鋼材
		/// </summary>
		public int RemodelSteel {
			get { return RawData.api_afterbull; }
		}

		/// <summary>
		/// 改装に改装設計図が必要かどうか
		/// </summary>
		public int NeedBlueprint { get; internal set; }


		#region Parameters
		
		/// <summary>
		/// 耐久初期値
		/// </summary>
		public int HPMin {
			get { return RawData.api_taik[0]; }
		}

		/// <summary>
		/// 耐久最大値
		/// </summary>
		public int HPMax {
			get { return RawData.api_taik[1]; }
		}

		/// <summary>
		/// 装甲初期値
		/// </summary>
		public int ArmorMin {
			get { return RawData.api_souk[0]; }
		}

		/// <summary>
		/// 装甲最大値
		/// </summary>
		public int ArmorMax {
			get { return RawData.api_souk[1]; }
		}

		/// <summary>
		/// 火力初期値
		/// </summary>
		public int FirepowerMin {
			get { return RawData.api_houg[0]; }
		}

		/// <summary>
		/// 火力最大値
		/// </summary>
		public int FirepowerMax {
			get { return RawData.api_houg[1]; }
		}

		/// <summary>
		/// 雷装初期値
		/// </summary>
		public int TorpedoMin {
			get { return RawData.api_raig[0]; }
		}

		/// <summary>
		/// 雷装最大値
		/// </summary>
		public int TorpedoMax {
			get { return RawData.api_raig[1]; }
		}

		/// <summary>
		/// 対空初期値
		/// </summary>
		public int AAMin {
			get { return RawData.api_tyku[0]; }
		}

		/// <summary>
		/// 対空最大値
		/// </summary>
		public int AAMax {
			get { return RawData.api_tyku[1]; }
		}

		/// <summary>
		/// 対潜初期値
		/// </summary>
		public int ASWMin {
			get { return RawData.api_tais[0]; }
		}

		/// <summary>
		/// 対潜最大値
		/// </summary>
		public int ASWMax {
			get { return RawData.api_tais[1]; }
		}

		/// <summary>
		/// 回避初期値
		/// </summary>
		public int EvasionMin {
			get { return RawData.api_kaih[0]; }
		}

		/// <summary>
		/// 回避最大値
		/// </summary>
		public int EvasionMax {
			get { return RawData.api_kaih[1]; }
		}

		/// <summary>
		/// 索敵初期値
		/// </summary>
		public int LOSMin {
			get { return RawData.api_saku[0]; }
		}

		/// <summary>
		/// 索敵最大値
		/// </summary>
		public int LOSMax {
			get { return RawData.api_saku[1]; }
		}

		/// <summary>
		/// 運初期値
		/// </summary>
		public int LuckMin {
			get { return RawData.api_luck[0]; }
		}

		/// <summary>
		/// 運最大値
		/// </summary>
		public int LuckMax {
			get { return RawData.api_luck[1]; }
		}

		/// <summary>
		/// 速力
		/// </summary>
		public int Speed {
			get { return RawData.api_sokuh; }
		}

		/// <summary>
		/// 射程
		/// </summary>
		public int Range {
			get { return RawData.api_leng; }
		}
		#endregion


		/// <summary>
		/// 装備スロットの数
		/// </summary>
		public int SlotSize {
			get { return RawData.api_slot_num; }
		}

		/// <summary>
		/// 各スロットの航空機搭載数
		/// </summary>
		public ReadOnlyCollection<int> Aircraft {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_maxeq ); }
		}

		/// <summary>
		/// 初期装備のID
		/// </summary>
		public ReadOnlyCollection<int> DefaultSlot {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_defeq ); }
		}


		/// <summary>
		/// 建造時間(分)
		/// </summary>
		public int BuildingTime {
			get { return RawData.api_buildtime; }
		}


		/// <summary>
		/// 解体資材
		/// </summary>
		public ReadOnlyCollection<int> Material {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_broken ); }
		}

		/// <summary>
		/// 近代化改修の素材にしたとき上昇するパラメータの量
		/// </summary>
		public ReadOnlyCollection<int> PowerUp {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_powup ); }
		}

		/// <summary>
		/// レアリティ
		/// </summary>
		public int Rarity {
			get { return RawData.api_backs; }
		}

		/// <summary>
		/// ドロップ/ログイン時のメッセージ
		/// </summary>
		public string MessageGet {
			get { return RawData.api_getmes; }
		}

		/// <summary>
		/// 艦船名鑑でのメッセージ
		/// </summary>
		public string MessageDict {
			get { return RawData.api_sinfo; }
		}

		/// <summary>
		/// 搭載燃料
		/// </summary>
		public int Fuel {
			get { return RawData.api_fuel_max; }
		}
		
		/// <summary>
		/// 搭載弾薬
		/// </summary>
		public int Ammo {
			get { return RawData.api_bull_max; }
		}



		public ShipDataMaster() {
			RemodelBeforeShipID = 0;
		}



		public int ID {
			get { return ShipID; }
		}

		
	}

}
