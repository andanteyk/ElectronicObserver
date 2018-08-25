using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Browser
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			// FormBrowserHostから起動された時は引数に通信用URLが渡される
			if (args.Length == 0)
			{
				MessageBox.Show("これは七四式電子観測儀のサブプログラムであり、単体では起動できません。\r\n本体から起動してください。",
					"情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
			Application.Run(new FormBrowser(args[0]));
		}

		private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			if (args.Name.StartsWith("CefSharp"))
			{
				string asmname = args.Name.Split(",".ToCharArray(), 2)[0] + ".dll";
				string arch = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, Environment.Is64BitProcess ? "x64" : "x86", asmname);
				return System.IO.File.Exists(arch) ? System.Reflection.Assembly.LoadFile(arch) : null;
			}
			return null;
		}
	}
}
