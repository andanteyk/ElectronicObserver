using ElectronicObserver.Resource.Record;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
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
			get { return (int)RawData.api_id; }
		}

		/// <summary>
		/// 図鑑番号
		/// </summary>
		public int AlbumNo {
			get { return !RawData.api_sortno() ? 0 : (int)RawData.api_sortno; }
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
			get { return (int)RawData.api_stype; }
		}


		/// <summary>
		/// 改装Lv.
		/// </summary>
		public int RemodelAfterLevel {
			get { return !RawData.api_afterlv() ? 0 : (int)RawData.api_afterlv; }
		}

		/// <summary>
		/// 改装後の艦船ID
		/// 0=なし
		/// </summary>
		public int RemodelAfterShipID {
			get { return !RawData.api_aftershipid() ? 0 : int.Parse( (string)RawData.api_aftershipid ); }
		}

		/// <summary>
		/// 改装後の艦船
		/// </summary>
		public ShipDataMaster RemodelAfterShip {
			get { return RemodelAfterShipID > 0 ? KCDatabase.Instance.MasterShips[RemodelAfterShipID] : null; }
		}


		/// <summary>
		/// 改装前の艦船ID
		/// 0=なし
		/// </summary>
		public int RemodelBeforeShipID { get; internal set; }

		/// <summary>
		/// 改装前の艦船
		/// </summary>
		public ShipDataMaster RemodelBeforeShip {
			get { return RemodelBeforeShipID > 0 ? KCDatabase.Instance.MasterShips[RemodelBeforeShipID] : null; }
		}


		/// <summary>
		/// 改装に必要な弾薬
		/// </summary>
		public int RemodelAmmo {
			get { return !RawData.api_afterbull() ? 0 : (int)RawData.api_afterbull; }
		}

		/// <summary>
		/// 改装に必要な鋼材
		/// </summary>
		public int RemodelSteel {
			get { return !RawData.api_afterfuel() ? 0 : (int)RawData.api_afterfuel; }
		}

		/// <summary>
		/// 改装に改装設計図が必要かどうか
		/// </summary>
		public int NeedBlueprint { get; internal set; }

		/// <summary>
		/// 改装に試製甲板カタパルトが必要かどうか
		/// </summary>
		public int NeedCatapult { get; internal set; }


		#region Parameters

		/// <summary>
		/// 耐久初期値
		/// </summary>
		public int HPMin {
			get {
				if ( RawData.api_taik() ) {
					return (int)RawData.api_taik[0];
				} else {
					var p = GetParameterElement();
					if ( p != null )
						return p.HPMin;
					else
						return 0;
				}
			}
		}

		/// <summary>
		/// 耐久最大値
		/// </summary>
		public int HPMax {
			get {
				if ( RawData.api_taik() ) {
					return (int)RawData.api_taik[1];
				} else {
					var p = GetParameterElement();
					if ( p != null )
						return p.HPMax;
					else
						return 0;
				}
			}
		}

		/// <summary>
		/// 装甲初期値
		/// </summary>
		public int ArmorMin {
			get {
				if ( RawData.api_souk() ) {
					return (int)RawData.api_souk[0];
				} else {
					var p = GetParameterElement();
					if ( p != null )
						return p.ArmorMin;
					else
						return 0;
				}
			}
		}

		/// <summary>
		/// 装甲最大値
		/// </summary>
		public int ArmorMax {
			get {
				if ( RawData.api_souk() ) {
					return (int)RawData.api_souk[1];
				} else {
					var p = GetParameterElement();
					if ( p != null )
						return p.ArmorMax;
					else
						return 0;
				}
			}
		}

		/// <summary>
		/// 火力初期値
		/// </summary>
		public int FirepowerMin {
			get {
				if ( RawData.api_houg() ) {
					return (int)RawData.api_houg[0];
				} else {
					var p = GetParameterElement();
					if ( p != null )
						return p.FirepowerMin;
					else
						return 0;
				}
			}
		}

		/// <summary>
		/// 火力最大値
		/// </summary>
		public int FirepowerMax {
			get {
				if ( RawData.api_houg() ) {
					return (int)RawData.api_houg[1];
				} else {
					var p = GetParameterElement();
					if ( p != null )
						return p.FirepowerMax;
					else
						return 0;
				}
			}
		}

		/// <summary>
		/// 雷装初期値
		/// </summary>
		public int TorpedoMin {
			get {
				if ( RawData.api_raig() ) {
					return (int)RawData.api_raig[0];
				} else {
					var p = GetParameterElement();
					if ( p != null )
						return p.TorpedoMin;
					else
						return 0;
				}
			}
		}

		/// <summary>
		/// 雷装最大値
		/// </summary>
		public int TorpedoMax {
			get {
				if ( RawData.api_raig() ) {
					return (int)RawData.api_raig[1];
				} else {
					var p = GetParameterElement();
					if ( p != null )
						return p.TorpedoMax;
					else
						return 0;
				}
			}
		}

		/// <summary>
		/// 対空初期値
		/// </summary>
		public int AAMin {
			get {
				if ( RawData.api_tyku() ) {
					return (int)RawData.api_tyku[0];
				} else {
					var p = GetParameterElement();
					if ( p != null )
						return p.AAMin;
					else
						return 0;
				}
			}
		}

		/// <summary>
		/// 対空最大値
		/// </summary>
		public int AAMax {
			get {
				if ( RawData.api_tyku() ) {
					return (int)RawData.api_tyku[1];
				} else {
					var p = GetParameterElement();
					if ( p != null )
						return p.AAMax;
					else
						return 0;
				}
			}
		}


		/// <summary>
		/// 対潜
		/// </summary>
		public ShipParameterRecord.Parameter ASW {
			get {
				var p = GetParameterElement();
				if ( p != null )
					return p.ASW;
				else
					return null;
			}
		}

		/// <summary>
		/// 回避
		/// </summary>
		public ShipParameterRecord.Parameter Evasion {
			get {
				var p = GetParameterElement();
				if ( p != null )
					return p.Evasion;
				else
					return null;
			}
		}

		/// <summary>
		/// 索敵
		/// </summary>
		public ShipParameterRecord.Parameter LOS {
			get {
				var p = GetParameterElement();
				if ( p != null )
					return p.LOS;
				else
					return null;
			}
		}


		/// <summary>
		/// 運初期値
		/// </summary>
		public int LuckMin {
			get {
				if ( RawData.api_luck() ) {
					return (int)RawData.api_luck[0];
				} else {
					var p = GetParameterElement();
					if ( p != null )
						return p.LuckMin;
					else
						return 0;
				}
			}
		}

		/// <summary>
		/// 運最大値
		/// </summary>
		public int LuckMax {
			get {
				if ( RawData.api_luck() ) {
					return (int)RawData.api_luck[1];
				} else {
					var p = GetParameterElement();
					if ( p != null )
						return p.LuckMax;
					else
						return 0;
				}
			}
		}

		/// <summary>
		/// 速力
		/// 0=陸上基地, 5=低速, 10=高速
		/// </summary>
		public int Speed {
			get { return (int)RawData.api_soku; }
		}

		/// <summary>
		/// 射程
		/// </summary>
		public int Range {
			get {
				if ( RawData.api_leng() ) {
					return (int)RawData.api_leng;
				} else {
					var p = GetParameterElement();
					if ( p != null )
						return p.Range;
					else
						return 0;
				}
			}
		}
		#endregion


		/// <summary>
		/// 装備スロットの数
		/// </summary>
		public int SlotSize {
			get { return (int)RawData.api_slot_num; }
		}

		/// <summary>
		/// 各スロットの航空機搭載数
		/// </summary>
		public ReadOnlyCollection<int> Aircraft {
			get {
				if ( RawData.api_maxeq() ) {
					return Array.AsReadOnly( (int[])RawData.api_maxeq );
				} else {
					var p = GetParameterElement();
					if ( p != null && p.Aircraft != null )
						return Array.AsReadOnly( p.Aircraft );
					else
						return Array.AsReadOnly( new[] { 0, 0, 0, 0, 0 } );
				}
			}
		}

		/// <summary>
		/// 搭載
		/// </summary>
		public int AircraftTotal {
			get { return Aircraft.Sum( a => Math.Max( a, 0 ) ); }
		}


		/// <summary>
		/// 初期装備のID
		/// </summary>
		public ReadOnlyCollection<int> DefaultSlot {
			get {
				var p = GetParameterElement();
				if ( p != null && p.DefaultSlot != null )
					return Array.AsReadOnly<int>( p.DefaultSlot );
				else
					return null;
			}
		}


		/// <summary>
		/// 建造時間(分)
		/// </summary>
		public int BuildingTime {
			get { return !RawData.api_buildtime() ? 0 : (int)RawData.api_buildtime; }
		}


		/// <summary>
		/// 解体資材
		/// </summary>
		public ReadOnlyCollection<int> Material {
			get { return Array.AsReadOnly<int>( !RawData.api_broken() ? new[] { 0, 0, 0, 0 } : (int[])RawData.api_broken ); }
		}

		/// <summary>
		/// 近代化改修の素材にしたとき上昇するパラメータの量
		/// </summary>
		public ReadOnlyCollection<int> PowerUp {
			get { return Array.AsReadOnly<int>( !RawData.api_powup() ? new[] { 0, 0, 0, 0 } : (int[])RawData.api_powup ); }
		}

		/// <summary>
		/// レアリティ
		/// </summary>
		public int Rarity {
			get { return !RawData.api_backs() ? 0 : (int)RawData.api_backs; }
		}

		/// <summary>
		/// ドロップ/ログイン時のメッセージ
		/// </summary>
		public string MessageGet {
			get {
				var p = GetParameterElement();
				if ( p != null && p.MessageGet != null )
					return p.MessageGet.Replace( "<br>", "\r\n" );
				else
					return "";
			}
		}

		/// <summary>
		/// 艦船名鑑でのメッセージ
		/// </summary>
		public string MessageAlbum {
			get {
				var p = GetParameterElement();
				if ( p != null && p.MessageAlbum != null )
					return p.MessageAlbum.Replace( "<br>", "\r\n" );
				else
					return "";
			}
		}


		/// <summary>
		/// 搭載燃料
		/// </summary>
		public int Fuel {
			get { return !RawData.api_fuel_max() ? 0 : (int)RawData.api_fuel_max; }
		}

		/// <summary>
		/// 搭載弾薬
		/// </summary>
		public int Ammo {
			get { return !RawData.api_bull_max() ? 0 : (int)RawData.api_bull_max; }
		}


		/// <summary>
		/// ボイス再生フラグ
		/// </summary>
		public int VoiceFlag {
			get { return !RawData.api_voicef() ? 0 : (int)RawData.api_voicef; }
		}


		/// <summary>
		/// リソースのファイル/フォルダ名
		/// </summary>
		public string ResourceName {
			get {
				var p = GetParameterElement();
				if ( p != null && p.ResourceName != null )
					return p.ResourceName;
				else
					return "";
			}
		}

		/// <summary>
		/// 画像リソースのバージョン
		/// </summary>
		public string ResourceGraphicVersion {
			get {
				var p = GetParameterElement();
				if ( p != null && p.ResourceGraphicVersion != null )
					return p.ResourceGraphicVersion;
				else
					return "";
			}
		}

		/// <summary>
		/// ボイスリソースのバージョン
		/// </summary>
		public string ResourceVoiceVersion {
			get {
				var p = GetParameterElement();
				if ( p != null && p.ResourceVoiceVersion != null )
					return p.ResourceVoiceVersion;
				else
					return "";
			}
		}

		/// <summary>
		/// 母港ボイスリソースのバージョン
		/// </summary>
		public string ResourcePortVoiceVersion {
			get {
				var p = GetParameterElement();
				if ( p != null && p.ResourcePortVoiceVersion != null )
					return p.ResourcePortVoiceVersion;
				else
					return "";
			}
		}

		/// <summary>
		/// 衣替え艦：ベースとなる艦船ID
		/// </summary>
		public int OriginalCostumeShipID {
			get {
				var p = GetParameterElement();
				if ( p != null )
					return p.OriginalCostumeShipID;
				else
					return -1;
			}
		}



		//以下、自作計算プロパティ群

		public static readonly int HPModernizableLimit = 2;
		public static readonly int ASWModernizableLimit = 9;


		/// <summary>
		/// ケッコンカッコカリ後のHP
		/// </summary>
		public int HPMaxMarried {
			get {
				int incr;
				if ( HPMin < 30 ) incr = 4;
				else if ( HPMin < 40 ) incr = 5;
				else if ( HPMin < 50 ) incr = 6;
				else if ( HPMin < 70 ) incr = 7;
				else if ( HPMin < 90 ) incr = 8;
				else incr = 9;

				return Math.Min( HPMin + incr, HPMax );
			}
		}

		/// <summary>
		/// HP改修可能値(未婚時)
		/// </summary>
		public int HPMaxModernizable {
			get { return Math.Min( HPMax - HPMin, HPModernizableLimit ); }
		}

		/// <summary>
		/// HP改修可能値(既婚時)
		/// </summary>
		public int HPMaxMarriedModernizable {
			get { return Math.Min( HPMax - HPMaxMarried, HPModernizableLimit ); }
		}

		/// <summary>
		/// 近代化改修後のHP(未婚時)
		/// </summary>
		public int HPMaxModernized {
			get {
				return Math.Min( HPMin + HPMaxModernizable, HPMax );
			}
		}

		/// <summary>
		/// 近代化改修後のHP(既婚時)
		/// </summary>
		public int HPMaxMarriedModernized {
			get {
				return Math.Min( HPMaxMarried + HPMaxModernizable, HPMax );
			}
		}


		/// <summary>
		/// 対潜改修可能値
		/// </summary>
		public int ASWModernizable {
			get { return ASW == null || ASW.Maximum == 0 ? 0 : ASWModernizableLimit; }
		}


		/// <summary>
		/// 深海棲艦かどうか
		/// </summary>
		public bool IsAbyssalShip {
			get { return ShipID > 1500; }
		}


		/// <summary>
		/// クラスも含めた艦名
		/// </summary>
		public string NameWithClass {
			get {
				if ( !IsAbyssalShip || NameReading == "" || NameReading == "-" )
					return Name;
				else
					return string.Format( "{0} {1}", Name, NameReading );
			}
		}

		/// <summary>
		/// 陸上基地かどうか
		/// </summary>
		public bool IsLandBase {
			get {
				return Speed == 0;
			}
		}


		/// <summary>
		/// 図鑑に載っているか
		/// </summary>
		public bool IsListedInAlbum {
			get { return 0 < AlbumNo && AlbumNo <= 350; }
		}


		/// <summary>
		/// 艦種名
		/// </summary>
		public string ShipTypeName {
			get { return KCDatabase.Instance.ShipTypes[ShipType].Name; }
		}


		/// <summary>
		/// 潜水艦系か
		/// </summary>
		public bool IsSubmarine {
			get { return ShipType == 13 || ShipType == 14; }
		}


		/// <summary>
		/// 自身のパラメータレコードを取得します。
		/// </summary>
		/// <returns></returns>
		private ShipParameterRecord.ShipParameterElement GetParameterElement() {
			return RecordManager.Instance.ShipParameter[ShipID];
		}



		private static readonly Color[] ShipNameColors = new Color[] { 
			Color.FromArgb( 0x00, 0x00, 0x00 ),
			Color.FromArgb( 0xFF, 0x00, 0x00 ),
			Color.FromArgb( 0xFF, 0x88, 0x00 ),
			Color.FromArgb( 0x00, 0x66, 0x00 ),
			Color.FromArgb( 0x88, 0x00, 0x00 ),
			Color.FromArgb( 0x00, 0x88, 0xFF ),
			Color.FromArgb( 0x00, 0x00, 0xFF ),
		};

		public Color GetShipNameColor() {

			if ( !IsAbyssalShip ) {
				return SystemColors.ControlText;
			}

			bool isLateModel = Name.Contains( "後期型" );
			bool isRemodeled = Name.Contains( "改" );
			bool isDestroyed = Name.EndsWith( "-壊" );
			bool isDemon = Name.EndsWith( "鬼" );
			bool isPrincess = Name.EndsWith( "姫" );
			bool isWaterDemon = Name.EndsWith( "水鬼" );
			bool isWaterPrincess = Name.EndsWith( "水姫" );
			bool isElite = NameReading == "elite";
			bool isFlagship = NameReading == "flagship";


			if ( isDestroyed )
				return Color.FromArgb( 0xFF, 0x00, 0xFF );

			else if ( isWaterPrincess )
				return ShipNameColors[6];
			else if ( isWaterDemon )
				return ShipNameColors[5];
			else if ( isPrincess )
				return ShipNameColors[4];
			else if ( isDemon )
				return ShipNameColors[3];
			else {

				int tier;

				if ( isFlagship )
					tier = 2;
				else if ( isElite )
					tier = 1;
				else
					tier = 0;

				if ( isLateModel || isRemodeled )
					tier += 3;

				return ShipNameColors[tier];
			}
		}


		public ShipDataMaster() {
			RemodelBeforeShipID = 0;
		}



		public int ID {
			get { return ShipID; }
		}


		public override string ToString() {
			return string.Format( "[{0}] {1}", ShipID, NameWithClass );
		}

	}

}
