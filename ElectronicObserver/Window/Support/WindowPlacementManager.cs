using Codeplex.Data;
using ElectronicObserver.Utility.Storage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Window.Support {



	public static class WindowPlacementManager {

		#region 各種宣言

		[DllImport( "user32.dll" )]
		public static extern bool SetWindowPlacement(
			IntPtr hWnd,
			[In] ref WINDOWPLACEMENT lpwndpl );

		[DllImport( "user32.dll" )]
		public static extern bool GetWindowPlacement(
			IntPtr hWnd,
			out WINDOWPLACEMENT lpwndpl );


		[Serializable]
		[StructLayout( LayoutKind.Sequential )]
		public struct WINDOWPLACEMENT {
			public int length;
			public int flags;
			public SW showCmd;
			public POINT minPosition;
			public POINT maxPosition;
			public RECT normalPosition;
		}

		[Serializable]
		[StructLayout( LayoutKind.Sequential )]
		public struct POINT {
			public int X;
			public int Y;

			public POINT( int x, int y ) {
				this.X = x;
				this.Y = y;
			}
		}

		[Serializable]
		[StructLayout( LayoutKind.Sequential )]
		public struct RECT {
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;

			public RECT( int left, int top, int right, int bottom ) {
				this.Left = left;
				this.Top = top;
				this.Right = right;
				this.Bottom = bottom;
			}
		}

		public enum SW {
			HIDE = 0,
			SHOWNORMAL = 1,
			SHOWMINIMIZED = 2,
			SHOWMAXIMIZED = 3,
			SHOWNOACTIVATE = 4,
			SHOW = 5,
			MINIMIZE = 6,
			SHOWMINNOACTIVE = 7,
			SHOWNA = 8,
			RESTORE = 9,
			SHOWDEFAULT = 10,
		}



		[DataContract( Name = "WindowPlacementWrapper" )]
		public class WindowPlacementWrapper : DataStorage {

			[IgnoreDataMember]
			public WINDOWPLACEMENT RawData;

			[DataMember]
			public int flags {
				get { return RawData.flags; }
				set { RawData.flags = value; }
			}

			[DataMember]
			public int showCmd {
				get { return (int)RawData.showCmd; }
				set { RawData.showCmd = (SW)value; }
			}

			[DataMember]
			public int minPositionX {
				get { return RawData.minPosition.X; }
				set { RawData.minPosition.X = value; }
			}

			[DataMember]
			public int minPositionY {
				get { return RawData.minPosition.Y; }
				set { RawData.minPosition.Y = value; }
			}

			[DataMember]
			public int maxPositionX {
				get { return RawData.maxPosition.X; }
				set { RawData.maxPosition.X = value; }
			}

			[DataMember]
			public int maxPositionY {
				get { return RawData.maxPosition.Y; }
				set { RawData.maxPosition.Y = value; }
			}

			[DataMember]
			public int normalPositionLeft {
				get { return RawData.normalPosition.Left; }
				set { RawData.normalPosition.Left = value; }
			}

			[DataMember]
			public int normalPositionTop {
				get { return RawData.normalPosition.Top; }
				set { RawData.normalPosition.Top = value; }
			}

			[DataMember]
			public int normalPositionRight {
				get { return RawData.normalPosition.Right; }
				set { RawData.normalPosition.Right = value; }
			}

			[DataMember]
			public int normalPositionBottom {
				get { return RawData.normalPosition.Bottom; }
				set { RawData.normalPosition.Bottom = value; }
			}


			public WindowPlacementWrapper() {
				Initialize();
			}

			public override void Initialize() {
				RawData = new WINDOWPLACEMENT();
				RawData.length = Marshal.SizeOf( RawData );
				RawData.flags = 0;
			}
		}
		#endregion



		public static string WindowPlacementConfigPath {
			get { return @"Settings\WindowPlacement.json"; }
		}


		[Obsolete]
		public static void LoadWindowPlacement( FormMain form, string path ) {

			try {

				if ( File.Exists( path ) ) {

					string settings;

					using ( StreamReader sr = new StreamReader( path ) ) {
						settings = sr.ReadToEnd();
					}


					WindowPlacementWrapper wp = DynamicJson.Parse( settings );

					if ( wp.RawData.showCmd == SW.SHOWMINIMIZED )
						wp.RawData.showCmd = SW.SHOWNORMAL;

					SetWindowPlacement( form.Handle, ref wp.RawData );

				}

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "ウィンドウ状態の復元に失敗しました。" );
				
			}

		}


		public static void LoadWindowPlacement( FormMain form, Stream stream ) {

			try {
				var wp = new WindowPlacementWrapper();
				wp = (WindowPlacementWrapper)wp.Load( stream );

				if ( wp.RawData.showCmd == SW.SHOWMINIMIZED )
					wp.RawData.showCmd = SW.SHOWNORMAL;

				SetWindowPlacement( form.Handle, ref wp.RawData );


			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "ウィンドウ状態の復元に失敗しました。" );
			}

		}


		[Obsolete]
		public static void SaveWindowPlacement( FormMain form, string path ) {


			try {

				string parent = Directory.GetParent( path ).FullName;
				if ( !Directory.Exists( parent ) ) {
					Directory.CreateDirectory( parent );
				}


				var wp = new WindowPlacementWrapper();
				


				GetWindowPlacement( form.Handle, out wp.RawData );


				string settings = DynamicJson.Serialize( wp );

				using ( StreamWriter sw = new StreamWriter( path ) ) {

					sw.Write( settings );
				
				}


			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "ウィンドウ状態の保存に失敗しました。" );
			}
		}



		public static void SaveWindowPlacement( FormMain form, Stream stream ) {

			try {
				var wp = new WindowPlacementWrapper();

				GetWindowPlacement( form.Handle, out wp.RawData );

				wp.Save( stream );

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "ウィンドウ状態の保存に失敗しました。" );
			}
		}

	}



}
