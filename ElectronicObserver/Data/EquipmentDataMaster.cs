using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 装備のマスターデータを保持します。
	/// </summary>
	public class EquipmentDataMaster : ResponseWrapper, IIdentifiable {

		/// <summary>
		/// 装備ID
		/// </summary>
		public int EquipmentID {
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
		/// 装備種別
		/// </summary>
		public ReadOnlyCollection<int> EquipmentType {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_type ); }
		}



		#region Parameters

		/// <summary>
		/// 装甲
		/// </summary>
		public int Armor {
			get { return RawData.api_souk; }
		}

		/// <summary>
		/// 火力
		/// </summary>
		public int Firepower {
			get { return RawData.api_houg; }
		}

		/// <summary>
		/// 雷装
		/// </summary>
		public int Torpedo {
			get { return RawData.api_raig; }
		}

		/// <summary>
		/// 爆装
		/// </summary>
		public int Bomber {
			get { return RawData.api_baku; }
		}

		/// <summary>
		/// 対空
		/// </summary>
		public int AA {
			get { return RawData.api_tyku; }
		}

		/// <summary>
		/// 対潜
		/// </summary>
		public int ASW {
			get { return RawData.api_tais; }
		}

		/// <summary>
		/// 命中
		/// </summary>
		public int Accuracy {
			get { return RawData.api_houm; }
		}

		/// <summary>
		/// 回避
		/// </summary>
		public int Evasion {
			get { return RawData.api_houk; }
		}

		/// <summary>
		/// 索敵
		/// </summary>
		public int LOS {
			get { return RawData.api_saku; }
		}

		/// <summary>
		/// 運
		/// </summary>
		public int Luck {
			get { return RawData.api_luck; }
		}

		/// <summary>
		/// 射程
		/// </summary>
		public int Range {
			get { return RawData.api_leng; }
		}

		#endregion


		/// <summary>
		/// レアリティ
		/// </summary>
		public int Rarity {
			get { return RawData.api_rare; }
		}
		
		/// <summary>
		/// 廃棄資材
		/// </summary>
		public ReadOnlyCollection<int> Material {
			get { return Array.AsReadOnly<int>((int[])RawData.api_broken); }
		}
		
		/// <summary>
		/// 図鑑説明
		/// </summary>
		public string Message {
			get { return RawData.api_info; }
		}





		public int ID {
			get { return EquipmentID; }
		}

	}

}
