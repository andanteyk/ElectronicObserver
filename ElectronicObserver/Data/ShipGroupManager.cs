using Microsoft.VisualBasic.FileIO;
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

		public const string DefaultFilePath = @"Settings\\ShipGroups.dat";


		/// <summary>
		/// 艦船グループリスト
		/// </summary>
		public IDDictionary<ShipGroupData> ShipGroups { get; private set; }

		/// <summary>
		/// フィルタリスト
		/// </summary>
		public List<bool> FilterList { get; private set; }

		/// <summary>
		/// 自動更新フラグ
		/// </summary>
		public bool AutoUpdateFlag { get; private set; }



		public ShipGroupManager() {
			ShipGroups = new IDDictionary<ShipGroupData>();
			FilterList = new List<bool>();
			AutoUpdateFlag = true;							//undone:初期値は常に true で保存されない、明日実装する
		}


		public ShipGroupData this[int index] {
			get {
				return ShipGroups[index];
			}
		}


		public void Load( string path = DefaultFilePath ) {

			try {

				using ( var parser = new TextFieldParser( path, Encoding.UTF8 ) ) {
					parser.Delimiters = new string[] { "," };
					
					{
						//filter
						string[] data = parser.ReadFields();
						FilterList = new List<bool>();

						for ( int i = 1; i < data.Length; i++ ) {
							int flag;
							if ( int.TryParse( data[i], out flag ) )
								FilterList.Add( flag != 0 );
						}
					}

					int id = 1;
					while ( !parser.EndOfData ) {
						string[] data = parser.ReadFields();

						if ( data.Length < 1 )
							continue;

						var group = new ShipGroupData( id );

						group.Name = data[0];
						group.Members.Capacity = data.Length - 1;

						for ( int i = 1; i < data.Length; i++ ) {
							int value;
							if ( int.TryParse( data[i], out value ) )
								group.Members.Add( value );
						}


						ShipGroups.Add( group );

						id++;
					}
				}

				
			} catch ( FileNotFoundException ) {

				Utility.Logger.Add( 3, string.Format( "艦船グループデータ {0} が存在しません。新規作成します", path ) );

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SaveErrorReport( ex, "艦船グループデータの読み込み時にエラーが発生しました。" );

			}

		}


		public void Save( string path = DefaultFilePath ) {

			if ( KCDatabase.Instance.Ships.Count == 0 )		//未ロード時はセーブしない
				return;

			try {

				using ( StreamWriter sw = new StreamWriter( path, false, Encoding.UTF8 ) ) {

					sw.WriteLine( "Filter,{0}", string.Join( ",", FilterList.Select( b => b ? 1 : 0 ) ) );

					foreach ( var g in ShipGroups.Values.OrderBy( g => g.GroupID ) ) {

						//g.CheckMembers();		//checkme:　不用意なデータの破壊を防ぐため；必要と感じたら復活させて

						string name = g.Name;
						if ( name.Contains( '\"' ) || name.Contains( ',' ) ) {
							name = string.Format( "\"{0}\"", name.Replace( "\"", "\"\"" ) );
						}

						sw.WriteLine( "{0},{1}", name, string.Join( ",", g.Members ) );

					}
					
				}	

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SaveErrorReport( ex, "艦船グループデータの書き込み時にエラーが発生しました。" );
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
