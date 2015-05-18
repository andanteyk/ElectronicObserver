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
			// FormBrowserHostから起動された時は引数に通信用URLが渡される
			if ( args.Length == 0 ) {
				MessageBox.Show( "これは七四式電子観測儀のサブプログラムであり、単体では起動できません。\r\n本体から起動してください。", 
					"情報", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}
#if DEBUG
			// debug
			System.Diagnostics.Debug.WriteLine( string.Format( "volume: {0}.", VolumeManager.GetApplicationVolume( (uint)System.Diagnostics.Process.GetCurrentProcess().Id ) ) );

			return;
#endif
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new FormBrowser( args[0] ) );
		}
	}
}
