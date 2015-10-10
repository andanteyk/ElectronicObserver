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

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new FormMain() );
		}

		static void Application_ThreadException( object sender, System.Threading.ThreadExceptionEventArgs e )
		{
			MessageBox.Show( e.Exception.ToString(), "ElectronicObserver", MessageBoxButtons.OK, MessageBoxIcon.Error );
			Utility.ErrorReporter.SendErrorReport( e.Exception, "线程中错误：" + e.Exception.Message );
		}
	}
}
