using SwfExtractor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource {
	
	
	public static class SwfHelper {

		// 各種画像リソースのサイズ
		public static readonly Size ShipBannerSize = new Size( 160, 40 );
		public static readonly Size ShipCardSize = new Size( 218, 300 );
		public static readonly Size ShipCutinSize = new Size( 665, 121 );
		public static readonly Size ShipNameSize = new Size( 436, 63 );
		public static readonly Size ShipSupplySize = new Size( 474, 47 );

		/// <summary>
		///  各種画像リソースの CharacterID
		/// </summary>
		public enum ShipResourceCharacterID {
			BannerNormal = 1,
			BannerDamaged = 3,
			CardNormal = 5,
			CardDamaged = 7,
			CutinNormal = 21,
			CutinDamaged = 23,
			Name = 25,
			SupplyNormal = 27,
			SupplyDamaged = 29,
		}


		/// <summary>
		/// 現在使用可能な画像リソースファイルへのパスを取得します。
		/// </summary>
		public static string GetResourcePath( string relativePath, string resourceName ) {
			return Directory.GetFiles( Utility.Configuration.Config.Connection.SaveDataPath + @"\" + relativePath, "*.swf", System.IO.SearchOption.TopDirectoryOnly )
				.Where( path => path.Contains( resourceName ) ).LastOrDefault();
		}

		/// <summary>
		/// 現在使用可能な艦船画像リソースファイルへのパスを取得します。
		/// </summary>
		public static string GetShipResourcePath( string resourceName ) {
			return GetResourcePath( @"resources\swf\ships\", resourceName );
		}


		/// <summary>
		/// 艦船画像リソースファイルが存在するかを調べます。
		/// </summary>
		public static bool HasShipSwfImage( string resourceName ) {
			return GetShipResourcePath( resourceName ) != null;
		}


		/// <summary>
		/// 艦船画像を swf ファイルから読み込みます。
		/// </summary>
		/// <param name="resourceName">艦船のリソース名(ファイル名)。</param>
		/// <param name="characterID">描画する画像の CharacterID 。</param>
		/// <returns>成功した場合は対象の Bitmap オブジェクト、失敗した場合は null を返します。</returns>
		public static Bitmap GetShipSwfImage( string resourceName, int characterID ) {
			try {

				string shipSwfPath = GetShipResourcePath( resourceName );

				if ( shipSwfPath != null ) {

					var shipSwf = new SwfParser();
					shipSwf.Parse( shipSwfPath );

					var imgtag = shipSwf.FindTags<SwfExtractor.Tags.ImageTag>().FirstOrDefault( t => t.CharacterID == characterID );
					return imgtag.ExtractImage();
				}

				return null;
			} catch ( Exception ) {
				return null;
			}
		}

		/// <summary>
		/// 艦船画像を swf ファイルから読み込みます。
		/// </summary>
		/// <param name="resourceName">艦船のリソース名(ファイル名)。</param>
		/// <param name="characterID">描画する画像の CharacterID 。</param>
		/// <returns>成功した場合は対象の Bitmap オブジェクト、失敗した場合は null を返します。</returns>
		public static Bitmap GetShipSwfImage( string resourceName, ShipResourceCharacterID characterID ) {
			return GetShipSwfImage( resourceName, (int)characterID );
		}

	
	}

}
