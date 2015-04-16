﻿using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility {

	public class ErrorReporter {

		private const string _basePath = "ErrorReport";


		/// <summary>
		/// エラーレポートを作成します。
		/// </summary>
		/// <param name="ex">発生した例外。</param>
		/// <param name="message">追加メッセージ。</param>
		/// <param name="connectionName">エラーが発生したAPI名。省略可能です。</param>
		/// <param name="connectionData">エラーが発生したAPIの内容。省略可能です。</param>
		public static void SendErrorReport( Exception ex, string message, string connectionName = null, string connectionData = null ) {

			Utility.Logger.Add( 3, string.Format( "{0} : {1}", message, ex.Message ) );

			string path = _basePath;

			if ( !Directory.Exists( path ) )
				Directory.CreateDirectory( path );


			path = string.Format( "{0}\\{1}.txt", path, DateTimeHelper.GetTimeStamp() );

			try {
				using ( StreamWriter sw = new StreamWriter( path, false, new System.Text.UTF8Encoding( false ) ) ) {

                    sw.WriteLine(LoadResources.getter("ErrorReporter_1"), DateTime.Now);
                    sw.WriteLine(LoadResources.getter("ErrorReporter_2"), ex.GetType().Name);
					sw.WriteLine( ex.Message );
                    sw.WriteLine(LoadResources.getter("ErrorReporter_3"), message);
                    sw.WriteLine(LoadResources.getter("ErrorReporter_4"));
					sw.WriteLine( ex.StackTrace );
					
					if ( connectionName != null && connectionData != null ) {
						sw.WriteLine();
                        sw.WriteLine(LoadResources.getter("ErrorReporter_5"), connectionName);
						sw.WriteLine( connectionData );
					}
				}

			} catch ( Exception ) {

                Utility.Logger.Add(3, string.Format(LoadResources.getter("ErrorReporter_6"), ex.Message, ex.StackTrace));
			}

		}

	}
}
