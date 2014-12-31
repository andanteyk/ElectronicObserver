using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility {

	public class ErrorReporter {

		private const string _basePath = "ErrorReport";


		public static void SaveErrorReport( Exception ex, string message, string connectionName = null, string connectionData = null ) {

			Utility.Logger.Add( 3, string.Format( "{0} : {1}", message, ex.Message ) );

			string path = _basePath;

			if ( !Directory.Exists( path ) )
				Directory.CreateDirectory( path );


			path = string.Format( "{0}\\{1}.txt", path, DateTimeHelper.GetTimeStamp() );

			try {
				using ( StreamWriter sw = new StreamWriter( path ) ) {

					sw.WriteLine( "エラーレポート : {0}", DateTime.Now );
					sw.WriteLine( "エラー : {0}", ex.GetType().Name );
					sw.WriteLine( ex.Message );
					sw.WriteLine( "追加情報 : {0}", message );
					sw.WriteLine( "スタックトレース：" );
					sw.WriteLine( ex.StackTrace );
					
					if ( connectionName != null && connectionData != null ) {
						sw.WriteLine();
						sw.WriteLine( "通信内容 : {0}", connectionName );
						sw.WriteLine( connectionData );
					}
				}

			} catch ( Exception ) {

				Utility.Logger.Add( 3, string.Format( "エラーレポートの書き込みに失敗しました。\r\n{0}\r\n{1}", ex.Message, ex.StackTrace ) );
			}

		}

	}
}
