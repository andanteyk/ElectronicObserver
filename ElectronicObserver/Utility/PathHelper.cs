using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Utility {

	public static class PathHelper {


		/// <summary>
		/// OpenFileDialog を、指定されたパスで初期化します。
		/// </summary>
		public static void InitOpenFileDialog( string path, OpenFileDialog dialog ) {

			if ( path == null || path.Trim().Length == 0 ) return;

			string parent = Path.GetDirectoryName( Path.GetFullPath( path ) );

			if ( File.Exists( path ) ) {
				dialog.InitialDirectory = parent;
				dialog.FileName = Path.GetFileName( path );

			} else if ( Directory.Exists( path ) ) {
				dialog.InitialDirectory = path;
				dialog.FileName = "";

			} else if ( Directory.Exists( parent ) ) {
				dialog.InitialDirectory = parent;
				dialog.FileName = "";

			}

		}


		/// <summary>
		/// OpenFileDialog からパスを取得します。
		/// </summary>
		public static string GetPathFromOpenFileDialog( OpenFileDialog dialog ) {

			string path = dialog.FileName;

			// カレントディレクトリ以下にあるなら相対パスとして記録する
			string currentDir = Directory.GetCurrentDirectory() + @"\";
			if ( path != null && path.IndexOf( currentDir ) == 0 ) {
				path = path.Remove( 0, currentDir.Length );
			}

			return path;
		}


		/// <summary>
		/// OpenFileDialog を利用し、パスを取得します。
		/// </summary>
		public static string ProcessOpenFileDialog( string path, OpenFileDialog dialog ) {

			InitOpenFileDialog( path, dialog );

			if ( dialog.ShowDialog() == DialogResult.OK ) {
				return GetPathFromOpenFileDialog( dialog );
			}

			return path;
		}



		/// <summary>
		/// SaveFileDialog を、指定されたパスで初期化します。
		/// </summary>
		public static void InitSaveFileDialog( string path, SaveFileDialog dialog ) {

			if ( path == null || path.Trim().Length == 0 ) return;

			string parent = Path.GetDirectoryName( Path.GetFullPath( path ) );

			if ( File.Exists( path ) ) {
				dialog.InitialDirectory = parent;
				dialog.FileName = Path.GetFileName( path );

			} else if ( Directory.Exists( path ) ) {
				dialog.InitialDirectory = path;
				dialog.FileName = "";

			} else if ( Directory.Exists( parent ) ) {
				dialog.InitialDirectory = parent;
				dialog.FileName = "";

			}

		}


		/// <summary>
		/// SaveFileDialog からパスを取得します。
		/// </summary>
		public static string GetPathFromSaveFileDialog( SaveFileDialog dialog ) {

			string path = dialog.FileName;

			// カレントディレクトリ以下にあるなら相対パスとして記録する
			string currentDir = Directory.GetCurrentDirectory() + @"\";
			if ( path != null && path.IndexOf( currentDir ) == 0 ) {
				path = path.Remove( 0, currentDir.Length );
			}

			return path;
		}


		/// <summary>
		/// SaveFileDialog を利用し、パスを取得します。
		/// </summary>
		/// <returns>指定されたファイル名。キャンセルされた場合は null を返します。</returns>
		public static string ProcessSaveFileDialog( string path, SaveFileDialog dialog ) {

			InitSaveFileDialog( path, dialog );

			if ( dialog.ShowDialog() == DialogResult.OK ) {
				return GetPathFromSaveFileDialog( dialog );
			}

			return null;
		}




		/// <summary>
		/// FolderBrowserDialog を、指定されたパスで初期化します。
		/// </summary>
		public static void InitFolderBrowserDialog( string path, FolderBrowserDialog dialog ) {

			if ( path == null || path.Trim().Length == 0 ) return;

			path = Path.GetFullPath( path );

			if ( Directory.Exists( path ) ) {
				dialog.SelectedPath = path;
			}

		}


		/// <summary>
		/// FolderBrowserDialog からパスを取得します。
		/// </summary>
		public static string GetPathFromFolderBrowserDialog( FolderBrowserDialog dialog ) {

			string path = dialog.SelectedPath;

			// カレントディレクトリ以下にあるなら相対パスとして記録する
			string currentDir = Directory.GetCurrentDirectory() + @"\";
			if ( path != null && path.IndexOf( currentDir ) == 0 ) {
				path = path.Remove( 0, currentDir.Length );
			}

			return path;
		}


		/// <summary>
		/// FolderBrowserDialog を利用し、パスを取得します。
		/// </summary>
		public static string ProcessFolderBrowserDialog( string path, FolderBrowserDialog dialog ) {

			InitFolderBrowserDialog( path, dialog );

			if ( dialog.ShowDialog() == DialogResult.OK ) {

				return GetPathFromFolderBrowserDialog( dialog );

			}

			return path;
		}

	}

}
