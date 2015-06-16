//#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Browser {
	static class Program {
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main( string[] args ) {
			//Application.SetUnhandledExceptionMode( UnhandledExceptionMode.CatchException );
			Application.ThreadException += Application_ThreadException;

			// FormBrowserHostから起動された時は引数に通信用URLが渡される
			if ( args.Length == 0 ) {
				MessageBox.Show( "これは七四式電子観測儀のサブプログラムであり、単体では起動できません。\r\n本体から起動してください。", 
					"情報", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			try
			{
				Application.Run( new FormBrowser( args[0] ) );
			}
			catch ( Exception ex )
			{
				Application_ThreadException( null, new System.Threading.ThreadExceptionEventArgs( ex ) );
			}
		}

		static void Application_ThreadException( object sender, System.Threading.ThreadExceptionEventArgs e )
		{
			MessageBox.Show( e.Exception.ToString(), "EOBrowser", MessageBoxButtons.OK, MessageBoxIcon.Error );
		}
	}
}
