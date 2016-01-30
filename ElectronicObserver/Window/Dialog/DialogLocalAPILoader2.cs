using ElectronicObserver.Observer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog {
	public partial class DialogLocalAPILoader2 : Form {


		private string CurrentPath { get; set; }


		public DialogLocalAPILoader2() {
			InitializeComponent();
		}

		private void DialogLocalAPILoader2_Load( object sender, EventArgs e ) {
			LoadFiles( Utility.Configuration.Config.Connection.SaveDataPath );
		}


		private void Menu_File_OpenFolder_Click( object sender, EventArgs e ) {

			FolderBrowser.SelectedPath = Utility.Configuration.Config.Connection.SaveDataPath;

			if ( FolderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
				LoadFiles( FolderBrowser.SelectedPath );
			}

		}

		private void Menu_File_Reload_Click( object sender, EventArgs e ) {
			if ( Directory.Exists( CurrentPath ) )
				LoadFiles( CurrentPath );
			else
				MessageBox.Show( "フォルダが指定されていないか存在しません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
		}

		private void ViewMenu_Execute_Click( object sender, EventArgs e ) {

			/*/
			var rows = APIView.SelectedRows.Cast<DataGridViewRow>().OrderBy( r => r.Cells[APIView_FileName.Index].Value );

			foreach ( DataGridViewRow row in rows ) {
				ExecuteAPI( (string)row.Cells[APIView_FileName.Index].Value );
			}
			/*/
			if ( !APICaller.IsBusy )
				APICaller.RunWorkerAsync( APIView.SelectedRows.Cast<DataGridViewRow>().Select( row => row.Cells[APIView_FileName.Index].Value as string ).OrderBy( s => s ) );
			else
				if ( MessageBox.Show( "既に実行中です。\n中断しますか?", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation )
					== System.Windows.Forms.DialogResult.Yes ) {
					APICaller.CancelAsync();
				}
			//*/
		}

		private void ViewMenu_Delete_Click( object sender, EventArgs e ) {

			foreach ( DataGridViewRow row in APIView.SelectedRows ) {
				APIView.Rows.Remove( row );
			}
		}


		private void ButtonExecuteNext_Click( object sender, EventArgs e ) {

			if ( APIView.SelectedRows.Count == 1 ) {

				var row = APIView.SelectedRows[0];
				int index = APIView.SelectedRows[0].Index;

				ExecuteAPI( (string)row.Cells[APIView_FileName.Index].Value );

				APIView.ClearSelection();
				if ( index < APIView.Rows.Count - 1 ) {
					APIView.Rows[index + 1].Selected = true;
					APIView.FirstDisplayedScrollingRowIndex = index + 1;
				}
			} else {
				MessageBox.Show( "単一行を選択してください。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information );

			}

		}



		private void LoadFiles( string path ) {

			if ( !Directory.Exists( path ) ) return;

			CurrentPath = path;

			APIView.Rows.Clear();

			var rows = new LinkedList<DataGridViewRow>();

			foreach ( string file in Directory.GetFiles( path, "*.json", SearchOption.TopDirectoryOnly ) ) {

				var row = new DataGridViewRow();
				row.CreateCells( APIView );

				row.SetValues( Path.GetFileName( file ) );
				rows.AddLast( row );

			}

			APIView.Rows.AddRange( rows.ToArray() );
			APIView.Sort( APIView_FileName, ListSortDirection.Ascending );

		}


		//filename format: yyyyMMdd_hhmmssff[Q|S]@apipath@apiname.json
		private string GetAPIName( string fileName ) {
			int indexa = fileName.IndexOf( '@' ) + 1, indexb =  fileName.LastIndexOf( '.' );
			return fileName.Substring( indexa ).Substring( 0, indexb - indexa ).Replace( '@', '/' );
		}

		private string GetAPITime( string filename ) {
			return filename.Substring( 0, filename.IndexOf( '@' ) - 1 );
		}


		private bool IsRequest( string filename ) {
			return char.ToLower( filename[filename.IndexOf( '@' ) - 1] ) == 'q';
		}

		private bool IsResponse( string filename ) {
			return char.ToLower( filename[filename.IndexOf( '@' ) - 1] ) == 's';
		}


		private void ExecuteAPI( string filename ) {

			if ( APIObserver.Instance.APIList.ContainsKey( GetAPIName( filename ) ) ) {

				string data;

				try {
					using ( var sr = new System.IO.StreamReader( CurrentPath + "\\" + filename ) ) {
						data = sr.ReadToEnd();
					}

				} catch ( Exception ex ) {
					Utility.Logger.Add( 3, string.Format( "APIファイル {0} の読み込みに失敗しました。{1}", filename, ex.Message ) );
					return;
				}


				if ( IsRequest( filename ) )
					APIObserver.Instance.LoadRequest( "/kcsapi/" + GetAPIName( filename ), data );

				if ( IsResponse( filename ) )
					APIObserver.Instance.LoadResponse( "/kcsapi/" + GetAPIName( filename ), data );
			}

		}



		private void APICaller_DoWork( object sender, DoWorkEventArgs e ) {

			var files = e.Argument as IOrderedEnumerable<string>;
			var act = new Action<string>( ExecuteAPI );

			foreach ( var file in files ) {
				Invoke( act, file );
				System.Threading.Thread.Sleep( 10 );		//ゆるして

				if ( APICaller.CancellationPending )
					break;
			}

		}

		private void ButtonSearch_Click( object sender, EventArgs e ) {

			int count = APIView.Rows.Count;
			int index;
			int result = -1;
			if ( APIView.SelectedRows.Count > 0 )
				index = APIView.SelectedRows[0].Index + 1;
			else
				index = 0;

			if ( index >= count )
				index = 0;

			for ( int i = index; i < count; i++ ) {
				if ( APIView[APIView_FileName.Index, i].Value.ToString().ToLower().Contains( TextFilter.Text.ToLower() ) ) {
					result = i;
					break;
				}
			}

			if ( result != -1 ) {
				APIView.ClearSelection();
				APIView.Rows[result].Selected = true;
				APIView.FirstDisplayedScrollingRowIndex = result;
			} else {
				System.Media.SystemSounds.Asterisk.Play();
			}
		}

		private void ButtonSearchLastStart2_Click( object sender, EventArgs e ) {
			for ( int i = APIView.Rows.Count - 1; i >= 0; i-- ) {
				if ( APIView[APIView_FileName.Index, i].Value.ToString().ToLower().Contains( "s@api_start2." ) ) {
					APIView.ClearSelection();
					APIView.Rows[i].Selected = true;
					APIView.FirstDisplayedScrollingRowIndex = i;
					return;
				}
			}

			//failed
			System.Media.SystemSounds.Asterisk.Play();
		}


		private void TextFilter_KeyDown( object sender, KeyEventArgs e ) {
			if ( e.KeyCode == Keys.Enter ) {
				e.SuppressKeyPress = true;
				ButtonSearch.PerformClick();
			}
		}

		

	}
}
