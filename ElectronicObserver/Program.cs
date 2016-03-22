using ElectronicObserver.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver {
	public static class Program {

		public static System.Drawing.Font Window_Font;

		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			//Application.SetUnhandledExceptionMode( UnhandledExceptionMode.CatchException );
			Application.ThreadException += Application_ThreadException;

			Utility.Configuration.Instance.Load();
			Window_Font = ToolStripCustomizer.ToolStripRender.Window_Font = Utility.Configuration.Config.UI.MainFont.FontData;

			ToolStripCustomizer.ToolStripRender.RendererTheme = (ToolStripCustomizer.ToolStripRenderTheme)Utility.Configuration.Config.UI.ThemeID;


			var mutex = new System.Threading.Mutex( false, Application.ExecutablePath.Replace( '\\', '/' ) );

			if ( !mutex.WaitOne( 0, false ) ) {
				// 多重起動禁止
				MessageBox.Show( "既に起動しています。\r\n多重起動はできません。", "七四式電子観測儀", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new FormMain() );

			mutex.ReleaseMutex();
		}

		static void Application_ThreadException( object sender, System.Threading.ThreadExceptionEventArgs e )
		{
			MessageBox.Show( e.Exception.ToString(), "ElectronicObserver", MessageBoxButtons.OK, MessageBoxIcon.Error );
			Utility.ErrorReporter.SendErrorReport( e.Exception, "线程中错误：" + e.Exception.Message );
		}
	}
}
