using ElectronicObserver.Window;
using ElectronicObserver.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver {
	static class Program {
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main() {

			var mutex = new System.Threading.Mutex( false, Application.ExecutablePath.Replace( '\\', '/' ) );

			if ( !mutex.WaitOne( 0, false ) ) {
				// 多重起動禁止
				MessageBox.Show( "既に起動しています。\r\n多重起動はできません。", "七四式電子観測儀", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			try
			{
				MD5.Create();
			}
			catch (TargetInvocationException ex)
			{
				if (ex.InnerException is InvalidOperationException)
				{
					MessageBox.Show(
						"FIPS 有効の場合、このソフトは実行できません。\nレジストリの " +
						"HKLM\\System\\CurrentControlSet\\Control\\Lsa\\FIPSAlgorithmPolicy\\Enabled" +
						" キーの値を 0 に直してください。", "七四式電子観測儀", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else
				{
					MessageBox.Show("MD5 ハッシュを作成できません。このエラーを報告してください。", 
						"七四式電子観測儀", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				ErrorReporter.SendErrorReport(ex.InnerException, "MD5 ハッシュを作成できません。");
				return;
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new FormMain() );

			mutex.ReleaseMutex();
		}
	}
}
