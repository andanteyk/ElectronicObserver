
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

			AppDomain.CurrentDomain.AssemblyResolve += Resolver;
			Application.Run(new FormBrowser(args[0]));
		}

		// Will attempt to load missing assembly from either x86 or x64 subdir
		private static Assembly Resolver(object sender, ResolveEventArgs args)
		{
			if (args.Name.StartsWith("CefSharp"))
			{
				string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
				string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
													   Environment.Is64BitProcess ? "x64" : "x86",
													   assemblyName);

				return File.Exists(archSpecificPath)
						   ? Assembly.LoadFile(archSpecificPath)
						   : null;
			}

			return null;
		}
	}
}
