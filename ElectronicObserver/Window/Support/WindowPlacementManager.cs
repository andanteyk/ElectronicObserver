using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Window.Support {



	public static class WindowPlacementManager {

		public static string WindowPlacementConfigPath {
			get { return @"Settings\WindowPlacement.cfg"; }
		}


		public static void LoadWindowPlacement( FormMain form, string path ) {

			try {

				if ( File.Exists( path ) ) {

					string settings;

					using ( StreamReader sr = new StreamReader( path ) ) {
						settings = sr.ReadToEnd();
					}

					Rectangle rect = DynamicJson.Parse( settings );

					form.Location = rect.Location;
					form.ClientSize = rect.Size;

				}

			} catch ( Exception e ) {

				Utility.Logger.Add( 3, "ウィンドウ状態の復元に失敗しました。\r\n" + e.Message );
				
			}

		}


		public static void SaveWindowPlacement( FormMain form, string path ) {


			try {

				string parent = Directory.GetParent( path ).FullName;
				if ( !Directory.Exists( parent ) ) {
					Directory.CreateDirectory( parent );
				}


				string settings = DynamicJson.Serialize( new Rectangle( form.Location, form.ClientSize ) );

				using ( StreamWriter sw = new StreamWriter( path ) ) {

					sw.Write( settings );
				
				}


			} catch ( Exception e ) {

				Utility.Logger.Add( 3, "ウィンドウ状態の保存に失敗しました。\r\n" + e.Message );
			}
		}


		/*
		public static void LoadSubWindowLayout( FormMain form, string path ) {



		}
		*/
		

	}



}
