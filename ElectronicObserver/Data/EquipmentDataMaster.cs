using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{


	/// <summary>
	/// 装備のマスターデータを保持します。
	/// </summary>
	public class EquipmentDataMaster : ResponseWrapper, IIdentifiable
	{

		/// <summary>
		/// 装備ID
		/// </summary>
		public int EquipmentID => (int)RawData.api_id;

		/// <summary>
		/// 図鑑番号
		/// </summary>
		public int AlbumNo => (int)RawData.api_sortno;

		/// <summary>
		/// 名前
		/// </summary>
		public string Name => RawData.api_name;


		/// <summary>
		/// 装備種別
		/// </summary>
		public ReadOnlyCollection<int> EquipmentType => Array.AsReadOnly((int[])RawData.api_type);



		#region Parameters

		/// <summary>
		/// 装甲
		/// </summary>
		public int Armor => (int)RawData.api_souk;

		/// <summary>
		/// 火力
		/// </summary>
		public int Firepower => (int)RawData.api_houg;

		/// <summary>
		/// 雷装
		/// </summary>
		public int Torpedo => (int)RawData.api_raig;

		/// <summary>
		/// 爆装
		/// </summary>
		public int Bomber => (int)RawData.api_baku;

		/// <summary>
		/// 対空
		/// </summary>
		public int AA => (int)RawData.api_tyku;

		/// <summary>
		/// 対潜
		/// </summary>
		public int ASW => (int)RawData.api_tais;

		/// <summary>
		/// 命中 / 対爆
		/// </summary>
		public int Accuracy => (int)RawData.api_houm;

		/// <summary>
		/// 回避 / 迎撃
		/// </summary>
		public int Evasion => (int)RawData.api_houk;

		/// <summary>
		/// 索敵
		/// </summary>
		public int LOS => (int)RawData.api_saku;

		/// <summary>
		/// 運
		/// </summary>
		public int Luck => (int)RawData.api_luck;

		/// <summary>
		/// 射程
		/// </summary>
		public int Range => (int)RawData.api_leng;

		#endregion


		/// <summary>
		/// レアリティ
		/// </summary>
		public int Rarity => (int)RawData.api_rare;

		/// <summary>
		/// 廃棄資材
		/// </summary>
		public ReadOnlyCollection<int> Material => Array.AsReadOnly((int[])RawData.api_broken);

		/// <summary>
		/// 図鑑説明
		/// </summary>
		public string Message => ((string)RawData.api_info).Replace("<br>", "\r\n");


		/// <summary>
		/// 基地航空隊：配置コスト
		/// </summary>
		public int AircraftCost => RawData.api_cost() ? (int)RawData.api_cost : 0;


		/// <summary>
		/// 基地航空隊：戦闘行動半径
		/// </summary>
		public int AircraftDistance => RawData.api_distance() ? (int)RawData.api_distance : 0;



		/// <summary>
		/// 深海棲艦専用装備かどうか
		/// </summary>
		public bool IsAbyssalEquipment => EquipmentID > 500;


		/// <summary>
		/// 図鑑に載っているか
		/// </summary>
		public bool IsListedInAlbum => AlbumNo > 0;


		/// <summary>
		/// 装備種別：小分類
		/// </summary>
		public int CardType => (int)RawData.api_type[1];

		/// <summary>
		/// 装備種別：カテゴリ
		/// </summary>
		public int CategoryType => (int)RawData.api_type[2];

		/// <summary>
		/// 装備種別：カテゴリ
		/// </summary>
		public EquipmentType CategoryTypeInstance => KCDatabase.Instance.EquipmentTypes[CategoryType];

		/// <summary>
		/// 装備種別：アイコン
		/// </summary>
		public int IconType => (int)RawData.api_type[3];


		//[Obsolete]
		//public string ResourceVersion { get; internal set; }


		public int ID => EquipmentID;

		public override string ToString()
		{
			return string.Format("[{0}] {1}", EquipmentID, Name);
		}
	}

}
