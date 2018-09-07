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
		public EquipmentTypes CategoryType => (EquipmentTypes)(int)RawData.api_type[2];

		/// <summary>
		/// 装備種別：カテゴリ
		/// </summary>
		public EquipmentType CategoryTypeInstance => KCDatabase.Instance.EquipmentTypes[(int)CategoryType];

		/// <summary>
		/// 装備種別：アイコン
		/// </summary>
		public int IconType => (int)RawData.api_type[3];


		internal int[] equippableShipsAtExpansion = new int[0];
		/// <summary>
		/// 拡張スロットに装備可能な艦船IDのリスト
		/// </summary>
		public IEnumerable<int> EquippableShipsAtExpansion => equippableShipsAtExpansion;



		// 以降自作判定
		// note: icontype の扱いについては再考の余地あり

		/// <summary> 砲系かどうか </summary>
		public bool IsGun =>
			CategoryType == EquipmentTypes.MainGunSmall ||
			CategoryType == EquipmentTypes.MainGunMedium ||
			CategoryType == EquipmentTypes.MainGunLarge ||
			CategoryType == EquipmentTypes.MainGunLarge2 ||
			CategoryType == EquipmentTypes.SecondaryGun;

		/// <summary> 主砲系かどうか </summary>
		public bool IsMainGun =>
			CategoryType == EquipmentTypes.MainGunSmall ||
			CategoryType == EquipmentTypes.MainGunMedium ||
			CategoryType == EquipmentTypes.MainGunLarge ||
			CategoryType == EquipmentTypes.MainGunLarge2;

		/// <summary> 副砲系かどうか </summary>
		public bool IsSecondaryGun => CategoryType == EquipmentTypes.SecondaryGun;

		/// <summary> 魚雷系かどうか </summary>
		public bool IsTorpedo => CategoryType == EquipmentTypes.Torpedo || CategoryType == EquipmentTypes.SubmarineTorpedo;

		/// <summary> 後期型魚雷かどうか </summary>
		public bool IsLateModelTorpedo =>
			EquipmentID == 213 ||   // 後期型艦首魚雷(6門)
			EquipmentID == 214;     // 熟練聴音員+後期型艦首魚雷(6門)


		/// <summary> 高角砲かどうか </summary>
		public bool IsHighAngleGun => IconType == 16;

		/// <summary> 高角砲+高射装置かどうか </summary>
		public bool IsHighAngleGunWithAADirector => IsHighAngleGun && AA >= 8;

		/// <summary> 集中配備機銃かどうか </summary>
		public bool IsConcentratedAAGun => CategoryType == EquipmentTypes.AAGun && AA >= 9;


		/// <summary> 航空機かどうか </summary>
		public bool IsAircraft
		{
			get
			{
				switch (CategoryType)
				{
					case EquipmentTypes.CarrierBasedFighter:
					case EquipmentTypes.CarrierBasedBomber:
					case EquipmentTypes.CarrierBasedTorpedo:
					case EquipmentTypes.SeaplaneBomber:
					case EquipmentTypes.Autogyro:
					case EquipmentTypes.ASPatrol:
					case EquipmentTypes.SeaplaneFighter:
					case EquipmentTypes.LandBasedAttacker:
					case EquipmentTypes.Interceptor:
					case EquipmentTypes.JetFighter:
					case EquipmentTypes.JetBomber:
					case EquipmentTypes.JetTorpedo:

					case EquipmentTypes.CarrierBasedRecon:
					case EquipmentTypes.SeaplaneRecon:
					case EquipmentTypes.FlyingBoat:
					case EquipmentTypes.JetRecon:
						return true;

					default:
						return false;
				}
			}
		}

		/// <summary> 戦闘に参加する航空機かどうか </summary>
		public bool IsCombatAircraft
		{
			get
			{
				switch (CategoryType)
				{
					case EquipmentTypes.CarrierBasedFighter:
					case EquipmentTypes.CarrierBasedBomber:
					case EquipmentTypes.CarrierBasedTorpedo:
					case EquipmentTypes.SeaplaneBomber:
					case EquipmentTypes.Autogyro:
					case EquipmentTypes.ASPatrol:
					case EquipmentTypes.SeaplaneFighter:
					case EquipmentTypes.LandBasedAttacker:
					case EquipmentTypes.Interceptor:
					case EquipmentTypes.JetFighter:
					case EquipmentTypes.JetBomber:
					case EquipmentTypes.JetTorpedo:
						return true;

					default:
						return false;
				}
			}
		}

		/// <summary> 偵察機かどうか </summary>
		public bool IsReconAircraft
		{
			get
			{
				switch (CategoryType)
				{
					case EquipmentTypes.CarrierBasedRecon:
					case EquipmentTypes.SeaplaneRecon:
					case EquipmentTypes.FlyingBoat:
					case EquipmentTypes.JetRecon:
						return true;

					default:
						return false;
				}
			}
		}

		/// <summary> 対潜攻撃可能な航空機かどうか </summary>
		public bool IsAntiSubmarineAircraft
		{
			get
			{
				switch (CategoryType)
				{
					case EquipmentTypes.CarrierBasedBomber:
					case EquipmentTypes.CarrierBasedTorpedo:
					case EquipmentTypes.SeaplaneBomber:
					case EquipmentTypes.Autogyro:
					case EquipmentTypes.ASPatrol:
					case EquipmentTypes.FlyingBoat:
					case EquipmentTypes.LandBasedAttacker:
					case EquipmentTypes.JetBomber:
					case EquipmentTypes.JetTorpedo:
						return ASW > 0;

					default:
						return false;
				}
			}
		}

		/// <summary> 夜間行動可能な航空機かどうか </summary>
		public bool IsNightAircraft => IsNightFighter || IsNightAttacker;

		/// <summary> 夜間戦闘機かどうか </summary>
		public bool IsNightFighter => IconType == 45;

		/// <summary> 夜間攻撃機かどうか </summary>
		public bool IsNightAttacker => IconType == 46;

		/// <summary> Swordfish 系艦上攻撃機かどうか </summary>
		public bool IsSwordfish => CategoryType == EquipmentTypes.CarrierBasedTorpedo && Name.Contains("Swordfish");


		/// <summary> 電探かどうか </summary>
		public bool IsRadar => CategoryType == EquipmentTypes.RadarSmall || CategoryType == EquipmentTypes.RadarLarge || CategoryType == EquipmentTypes.RadarLarge2;

		/// <summary> 対空電探かどうか </summary>
		public bool IsAirRadar => IsRadar && AA >= 2;

		/// <summary> 水上電探かどうか </summary>
		public bool IsSurfaceRadar => IsRadar && Accuracy >= 3;


		/// <summary> ソナーかどうか </summary>
		public bool IsSonar => CategoryType == EquipmentTypes.Sonar || CategoryType == EquipmentTypes.SonarLarge;

		/// <summary> 爆雷かどうか(投射機は含まない) </summary>
		public bool IsDepthCharge =>
			EquipmentID == 226 ||       // 九五式爆雷 
			EquipmentID == 227;         // 二式爆雷

		/// <summary> 爆雷投射機かどうか(爆雷は含まない) </summary>
		public bool IsDepthChargeProjector => CategoryType == EquipmentTypes.DepthCharge && !IsDepthCharge;


		/// <summary> 夜間作戦航空要員かどうか </summary>
		public bool IsNightAviationPersonnel =>
			EquipmentID == 258 ||       // 夜間作戦航空要員
			EquipmentID == 259;         // 夜間作戦航空要員+熟練甲板員







		public int ID => EquipmentID;

		public override string ToString() => $"[{EquipmentID}] {Name}";

	}

}
