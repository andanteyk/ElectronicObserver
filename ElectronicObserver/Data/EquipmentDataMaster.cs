using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 装備のマスターデータを保持します。
	/// </summary>
	[DebuggerDisplay( "[{ID}] : {Name}" )]
	public class EquipmentDataMaster : ResponseWrapper, IIdentifiable {

		/// <summary>
		/// 装備ID
		/// </summary>
		public int EquipmentID {
			get { return (int)RawData.api_id; }
		}

		/// <summary>
		/// 図鑑番号
		/// </summary>
		public int AlbumNo {
			get { return (int)RawData.api_sortno; }
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
			get { return (int)RawData.api_souk; }
		}

		/// <summary>
		/// 火力
		/// </summary>
		public int Firepower {
			get { return (int)RawData.api_houg; }
		}

		/// <summary>
		/// 雷装
		/// </summary>
		public int Torpedo {
			get { return (int)RawData.api_raig; }
		}

		/// <summary>
		/// 爆装
		/// </summary>
		public int Bomber {
			get { return (int)RawData.api_baku; }
		}

		/// <summary>
		/// 対空
		/// </summary>
		public int AA {
			get { return (int)RawData.api_tyku; }
		}

		/// <summary>
		/// 対潜
		/// </summary>
		public int ASW {
			get { return (int)RawData.api_tais; }
		}

		/// <summary>
		/// 命中
		/// </summary>
		public int Accuracy {
			get { return (int)RawData.api_houm; }
		}

		/// <summary>
		/// 回避
		/// </summary>
		public int Evasion {
			get { return (int)RawData.api_houk; }
		}

		/// <summary>
		/// 索敵
		/// </summary>
		public int LOS {
			get { return (int)RawData.api_saku; }
		}

		/// <summary>
		/// 運
		/// </summary>
		public int Luck {
			get { return (int)RawData.api_luck; }
		}

		/// <summary>
		/// 射程
		/// </summary>
		public int Range {
			get { return (int)RawData.api_leng; }
		}

		#endregion


		/// <summary>
		/// レアリティ
		/// </summary>
		public int Rarity {
			get { return (int)RawData.api_rare; }
		}

		/// <summary>
		/// 廃棄資材
		/// </summary>
		public ReadOnlyCollection<int> Material {
			get { return Array.AsReadOnly<int>( (int[])RawData.api_broken ); }
		}

		/// <summary>
		/// 図鑑説明
		/// </summary>
		public string Message {
			get { return ( (string)RawData.api_info ).Replace( "<br>", "\n" ); }
		}


		/// <summary>
		/// 基地航空隊：配置コスト
		/// </summary>
		public int AircraftCost {
			get {
				return RawData.api_cost() ? (int)RawData.api_cost : 0;
			}
		}

		/// <summary>
		/// 基地航空隊：戦闘行動半径
		/// </summary>
		public int AircraftDistance {
			get {
				return RawData.api_distance() ? (int)RawData.api_distance : 0;
			}
		}


		/// <summary>
		/// 深海棲艦専用装備かどうか
		/// </summary>
		public bool IsAbyssalEquipment {
			get { return EquipmentID > 500; }
		}


		/// <summary>
		/// 図鑑に載っているか
		/// </summary>
		public bool IsListedInAlbum {
			get { return AlbumNo > 0; }
		}


		/// <summary>
		/// 装備種別：小分類
		/// </summary>
		public int CardType {
			get { return (int)RawData.api_type[1]; }
		}

		/// <summary>
		/// 装備種別：カテゴリ
		/// </summary>
		public int CategoryType {
			get { return (int)RawData.api_type[2]; }
		}

		/// <summary>
		/// 装備種別：カテゴリ
		/// </summary>
		public EquipmentType CategoryTypeInstance {
			get { return KCDatabase.Instance.EquipmentTypes[CategoryType]; }
		}

		/// <summary>
		/// 装備種別：アイコン
		/// </summary>
		public int IconType {
			get { return (int)RawData.api_type[3]; }
		}


		//[Obsolete]
		//public string ResourceVersion { get; internal set; }


		public int ID {
			get { return EquipmentID; }
		}

	}

}
