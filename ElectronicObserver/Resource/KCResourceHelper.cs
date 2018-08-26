using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource
{
	public static class KCResourceHelper
	{

		public static readonly Size ShipNameSize = new Size(654, 95);
		public static readonly Size ShipBannerSize = new Size(240, 60);
		public static readonly Size ShipCardSize = new Size(327, 450);
		public static readonly Size ShipAlbumSize = new Size(431, 645);     // note: w=430の画像も存在する
		public static readonly Size ShipCutinSize = new Size(998, 182);
		public static readonly Size ShipSupplySize = new Size(711, 71);

		public static readonly Size EquipmentCardSize = new Size(390, 390);
		public static readonly Size EquipmentCardSmallSize = new Size(160, 160);
		public static readonly Size EquipmentAlbumSize = new Size(430, 645);
		public static readonly Size EquipmentSpecSize = new Size(301, 183);
		public static readonly Size EquipmentNameSize = new Size(654, 94);


		public static readonly string ResourceTypeShipName = "album_status";
		public static readonly string ResourceTypeShipBanner = "banner";
		public static readonly string ResourceTypeShipCard = "card";
		public static readonly string ResourceTypeShipAlbumFull = "character_full";
		public static readonly string ResourceTypeShipAlbumZoom = "character_up";
		public static readonly string ResourceTypeShipFull = "full";
		public static readonly string ResourceTypeShipCutin = "remodel";
		public static readonly string ResourceTypeShipSupply = "supply_character";

		public static readonly string ResourceTypeEquipmentText = "btxt_flat";
		public static readonly string ResourceTypeEquipmentCard = "card";
		public static readonly string ResourceTypeEquipmentCardSmall = "card_t";
		public static readonly string ResourceTypeEquipmentFairy = "item_character";
		public static readonly string ResourceTypeEquipmentAlbum = "item_on";
		public static readonly string ResourceTypeEquipmentWeapon = "item_up";
		public static readonly string ResourceTypeEquipmentSpec = "remodel";
		public static readonly string ResourceTypeEquipmentName = "statustop_item";



		/// <summary>
		/// 艦船画像リソースへのパスを取得します。
		/// </summary>
		/// <param name="shipID">艦船ID。</param>
		/// <param name="isDamaged">中破グラフィックかどうか。</param>
		/// <param name="resourceType">画像種別。["album_status", "banner", "card", "character_full", "character_up", "full", "remodel", "supply_character"]</param>
		/// <returns></returns>
		private static string GetShipResourcePath(int shipID, bool isDamaged, string resourceType)
		{
			if (resourceType == "album_status")
				isDamaged = false;

			if (isDamaged)
				resourceType += "_dmg";

			return $@"kcs2\resources\ship\{resourceType}\{shipID:d4}";
		}

		/// <summary>
		/// 装備画像リソースへのパスを取得します。
		/// </summary>
		/// <param name="equipmentID">装備ID。</param>
		/// <param name="resourceType">画像種別。["btxt_flat", "card", "card_t", "item_character", "item_on", "item_up", "remodel", "statustop_item"]</param>
		/// <returns></returns>
		private static string GetEquipmentResourcePath(int equipmentID, string resourceType)
		{
			return $@"kcs2\resources\slot\{resourceType}\{equipmentID:d3}";
		}




		/// <summary>
		/// 最新のリソースへのパスを取得します。
		/// </summary>
		public static string GetLatestResourcePath(string resourcePath)
		{
			string root = Utility.Configuration.Config.Connection.SaveDataPath + "\\" + Path.GetDirectoryName(resourcePath);

			try
			{
				if (!Directory.Exists(root))
					return null;

				return
					Directory.EnumerateFiles(root, Path.GetFileNameWithoutExtension(resourcePath) + "*.png", SearchOption.TopDirectoryOnly)
					.OrderBy(path => File.GetCreationTime(path))
					.LastOrDefault();
			}
			catch (Exception)
			{
				return null;
			}
		}


		public static string GetShipImagePath(int shipID, bool isDamaged, string resourceType)
			=> GetLatestResourcePath(GetShipResourcePath(shipID, isDamaged, resourceType));

		public static string GetEquipmentImagePath(int equipmentID, string resourceType)
			=> GetLatestResourcePath(GetEquipmentResourcePath(equipmentID, resourceType));

		public static bool HasShipImage(int shipID, bool isDamaged, string resourceType)
			=> GetShipImagePath(shipID, isDamaged, resourceType) != null;

		public static bool HasEquipmentImage(int equipmentID, string resourceType)
			=> GetEquipmentImagePath(equipmentID, resourceType) != null;


		/// <summary>
		/// 艦船の画像を読み込みます。
		/// </summary>
		/// <param name="shipID">艦船ID。</param>
		/// <param name="isDamaged">中破しているかどうか。</param>
		/// <param name="resourceType">画像種別。同クラスの定数を使用します。</param>
		/// <returns>成功した場合は艦船画像。失敗した場合は null。</returns>
		public static Bitmap LoadShipImage(int shipID, bool isDamaged, string resourceType)
		{
			string resourcepath = GetShipResourcePath(shipID, isDamaged, resourceType);
			string realpath = GetLatestResourcePath(resourcepath);

			if (realpath == null)
				return null;

			return new Bitmap(realpath);
		}

		/// <summary>
		/// 装備の画像を読み込みます。
		/// </summary>
		/// <param name="equipmentID">装備ID。</param>
		/// <param name="resourceType">画像種別。同クラスの定数を使用します。</param>
		/// <returns>成功した場合は装備画像。失敗した場合は null。</returns>
		public static Bitmap LoadEquipmentImage(int equipmentID, string resourceType)
		{
			string resourcepath = GetEquipmentResourcePath(equipmentID, resourceType);
			string realpath = GetLatestResourcePath(resourcepath);

			if (realpath == null)
				return null;

			return new Bitmap(realpath);
		}
	}
}
