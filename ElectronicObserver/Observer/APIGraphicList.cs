using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer {

	public class APIGraphicList {

		private static APIGraphicList _instance;

		public static APIGraphicList Instance {
			get {
				if ( _instance == null ) {
					_instance = new APIGraphicList();
				}

				return _instance;
			}
		}

		private APIGraphicList() { }

		public void OutputGraphicList( string api_start2, string out_filename ) {

			var start2 = Codeplex.Data.DynamicJson.Parse( api_start2.Substring( 7 ) ).api_data;

			StringBuilder builder = new StringBuilder();

			using ( var writer = new StringWriter( builder ) ) {
				writer.WriteLine( "图鉴序号,ID,名称,文件名,文件版本,类型" );

				foreach ( var graphic in start2.api_mst_shipgraph ) {

					double id = graphic.api_id;
					double sortno = graphic.api_sortno;
					string filename = graphic.api_filename;

					Func<dynamic, bool> predicate = ship => ( ship.api_id == id && ship.api_sortno == sortno );

					var names = GetName( predicate,
						start2.api_mst_ship,
						start2.api_mst_stype );

					writer.WriteLine( "{0},{1},{2},{3},{4},{5}",
						graphic.api_sortno,
						graphic.api_id,
						names[0],
						filename,
						graphic.api_version,
						names[1]
						);
				}
			}

			File.WriteAllText( out_filename, builder.ToString(), Encoding.UTF8 );
		}

		private string[] GetName( Func<dynamic, bool> predicate, dynamic mst_ship, dynamic mst_stype ) {

			dynamic ele = FirstOrDefault( mst_ship, predicate );

			if ( ele == null ) {
				return new[] { "-", "-" };
			}

			double st = ele.api_stype;
			predicate = s => s.api_id == st;

			var stype = FirstOrDefault( mst_stype, predicate );
			if ( stype == null ) {
				return new string[] { ele.api_name, "-" };
			}

			return new string[] { ele.api_name, stype.api_name };
		}

		private dynamic FirstOrDefault( dynamic collection, Func<dynamic, bool> predicate ) {

			if ( collection == null || predicate == null || !collection.IsArray ) {
				return null;
			}

			foreach ( var c in collection ) {

				if ( predicate( c ) ) {
					return c;
				}
			}

			return null;
		}

	}

}
