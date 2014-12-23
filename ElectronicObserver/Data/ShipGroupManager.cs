using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 艦船グループのデータを管理します。
	/// </summary>
	public class ShipGroupManager {

		public const string DefaultFilePath = @"Settings\\ShipGroups.csv";


		/// <summary>
		/// 艦船グループリスト
		/// </summary>
		public IDDictionary<ShipGroupData> ShipGroups { get; private set; }


		public ShipGroupManager() {
			ShipGroups = new IDDictionary<ShipGroupData>();
		}


		public ShipGroupData this[int index] {
			get {
				return ShipGroups[index];
			}
		}


		public void Load( string path = DefaultFilePath ) {

			try {

				using ( var sr = new StreamReader( path, Encoding.Default ) ) {

					string line;
					int id = 1;
					while ( ( line = sr.ReadLine() ) != null ) {

						string[] data = line.Split( ",".ToArray() );
						if ( data.Length == 0 )
							continue;

						var group = new ShipGroupData( id );

						group.Name = data[0];

						for ( int i = 1; i < data.Length; i++ ) {
							group.Members.Add( int.Parse( data[i] ) );
						}

						id++;
					}

				}

				
			} catch ( FileNotFoundException ) {

				Utility.Logger.Add( 3, string.Format( "艦船グループデータ {0} が存在しません。新規作成します", path ) );

			} catch ( Exception ex ) {

				Utility.Logger.Add( 3, "艦船グループデータの読み込み時にエラーが発生しました。\r\n" + ex.Message );

			}

		}


		public void Save( string path = DefaultFilePath ) {

			if ( KCDatabase.Instance.Ships.Count == 0 )		//未ロード時はセーブしない
				return;

			try {

				using ( StreamWriter sw = new StreamWriter( path, false, Encoding.Default ) ) {

					foreach ( var g in ShipGroups.Values ) {

						//g.CheckMembers();		//checkme:　不用意なデータの破壊を防ぐため；必要と感じたら復活させて

						sw.Write( g.Name );

						foreach ( int i in g.Members ) {
							sw.Write( "," );
							sw.Write( i );
						}

						sw.WriteLine();

					}
					
				}	

			} catch ( Exception ex ) {

				Utility.Logger.Add( 3, "艦船グループデータの書き込み時にエラーが発生しました。\r\n" + ex.Message );

			}
		}


		public ShipGroupData Add() {

			int key = ShipGroups.Count > 0 ? ShipGroups.Keys.Max() + 1 : 1;
			var group = new ShipGroupData( key );
			ShipGroups.Add( group );
			return group;

		}

	}

}
