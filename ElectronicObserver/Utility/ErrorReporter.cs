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

					sw.WriteLine( "错误报告日期 : {0}", DateTime.Now );
					sw.WriteLine( "错误 : {0}", ex.GetType().Name );
					sw.WriteLine( ex.Message );
					sw.WriteLine( "追加信息 : {0}", message );
					sw.WriteLine( "堆栈信息：" );
					sw.WriteLine( ex.StackTrace );

                    var exInner = ex;
                    while ((exInner = exInner.InnerException) != null)
                    {
                        sw.WriteLine("Inner Error : {0}", exInner.GetType().Name);
                        sw.WriteLine(exInner.Message);
                        sw.WriteLine("Stack Trace：");
                        sw.WriteLine(exInner.StackTrace);
                    }
					
					if ( connectionName != null && connectionData != null ) {
						sw.WriteLine();
						sw.WriteLine( "通信内容 : {0}", connectionName );
						sw.WriteLine( connectionData );
					}
				}

			} catch ( Exception ) {

				Utility.Logger.Add( 3, string.Format( "错误报告文件写入失败。\r\n{0}\r\n{1}", ex.Message, ex.StackTrace ) );
			}

		}

		public static void SendLoadErrorReport( System.Reflection.ReflectionTypeLoadException ex, string message )
		{

			Utility.Logger.Add( 3, string.Format( "{0} : {1}", message, ex.Message ) );

			string path = _basePath;

			if ( !Directory.Exists( path ) )
				Directory.CreateDirectory( path );


			path = string.Format( "{0}\\{1}.txt", path, DateTimeHelper.GetTimeStamp() );

			try
			{
				using ( StreamWriter sw = new StreamWriter( path, false, new System.Text.UTF8Encoding( false ) ) )
				{

					sw.WriteLine( "错误报告日期 : {0}", DateTime.Now );
					sw.WriteLine( "错误 : {0}", ex.GetType().Name );
					sw.WriteLine( ex.Message );
					sw.WriteLine( "追加信息 : {0}", message );
					sw.WriteLine( "堆栈信息：" );
					sw.WriteLine( ex.StackTrace );

					foreach ( var loaderErr in ex.LoaderExceptions )
					{
						sw.WriteLine( "Loader Error : {0}", loaderErr.GetType().Name );
						sw.WriteLine( loaderErr.Message );
						sw.WriteLine( "Stack Trace：" );
						sw.WriteLine( loaderErr.StackTrace );
					}
				}

			}
			catch ( Exception )
			{

				Utility.Logger.Add( 3, string.Format( "错误报告文件写入失败。\r\n{0}\r\n{1}", ex.Message, ex.StackTrace ) );
			}


			if ( Utility.Configuration.Config.Debug.AlertOnError )
				System.Media.SystemSounds.Hand.Play();
		}

	}
}
