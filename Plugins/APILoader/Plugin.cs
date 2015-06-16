using Codeplex.Data;

using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility;
using ElectronicObserver.Window;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Plugins;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APILoader
{
	public partial class Plugin : ServerPlugin
	{
		private FormMain MainForm;

		public override string MenuTitle
		{
			get
			{
				return "API加载器";
			}
		}

		public override async Task<bool> RunService( FormMain main )
		{
			MainForm = main;

			Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

			if ( !Configuration.Config.Debug.EnableDebugMenu )
			{
				Logger.Add( 1, "已关闭调试菜单。" );
				return true;
			}


			// デバッグ: 開始時にAPIリストを読み込む
			if ( Configuration.Config.Debug.LoadAPIListOnLoad )
			{

				try
				{

					await Task.Factory.StartNew( () => LoadAPIList( Configuration.Config.Debug.APIListPath ) );

				}
				catch ( Exception ex )
				{

					Logger.Add( 3, "API読み込みに失敗しました。" + ex.Message );
				}
			}


			InitToolStripMenuItem();

			MainForm.MainMenuStrip.Items.Insert( MainForm.MainMenuStrip.Items.Count - 1, StripMenu_Debug );

			return true;
		}

		private void ConfigurationChanged()
		{
			if ( !Configuration.Config.Debug.EnableDebugMenu )
			{
				if ( StripMenu_Debug != null )
					StripMenu_Debug.Visible = false;
				return;
			}

			if (StripMenu_Debug == null )
			{
				InitToolStripMenuItem();
				MainForm.MainMenuStrip.Items.Insert( MainForm.MainMenuStrip.Items.Count - 1, StripMenu_Debug );
			}

			StripMenu_Debug.Visible = true;
		}

		private void StripMenu_Debug_LoadAPIFromFile_Click( object sender, EventArgs e )
		{

			/*/
			using ( var dialog = new DialogLocalAPILoader() ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {
					if ( APIObserver.Instance.APIList.ContainsKey( dialog.APIName ) ) {

						if ( dialog.IsResponse ) {
							APIObserver.Instance.LoadResponse( dialog.APIPath, dialog.FileData );
						}
						if ( dialog.IsRequest ) {
							APIObserver.Instance.LoadRequest( dialog.APIPath, dialog.FileData );
						}

					}
				}
			}
			/*/
			new DialogLocalAPILoader2().Show( MainForm );
			//*/
		}

		private async void StripMenu_Debug_LoadInitialAPI_Click( object sender, EventArgs e )
		{

			using ( OpenFileDialog ofd = new OpenFileDialog() )
			{

				ofd.Title = "APIリストをロード";
				ofd.Filter = "API List|*.txt|File|*";
				ofd.InitialDirectory = Configuration.Config.Connection.SaveDataPath;

				if ( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
				{

					try
					{

						await Task.Factory.StartNew( () => LoadAPIList( ofd.FileName ) );

					}
					catch ( Exception ex )
					{

						MessageBox.Show( "API読み込みに失敗しました。\r\n" + ex.Message, "エラー",
							MessageBoxButtons.OK, MessageBoxIcon.Error );

					}

				}

			}

		}



		private void LoadAPIList( string path )
		{

			string parent = Path.GetDirectoryName( path );

			using ( StreamReader sr = new StreamReader( path ) )
			{
				string line;
				while ( ( line = sr.ReadLine() ) != null )
				{

					bool isRequest = false;
					{
						int slashindex = line.IndexOf( '/' );
						if ( slashindex != -1 )
						{

							switch ( line.Substring( 0, slashindex ).ToLower() )
							{
								case "q":
								case "request":
									isRequest = true;
									goto case "s";
								case "":
								case "s":
								case "response":
									line = line.Substring( Math.Min( slashindex + 1, line.Length ) );
									break;
							}

						}
					}

					if ( APIObserver.Instance.APIList.ContainsKey( line ) )
					{
						APIBase api = APIObserver.Instance.APIList[line];

						if ( isRequest ? api.IsRequestSupported : api.IsResponseSupported )
						{

							string[] files = Directory.GetFiles( parent, string.Format( "*{0}@{1}.json", isRequest ? "Q" : "S", line.Replace( '/', '@' ) ), SearchOption.TopDirectoryOnly );

							if ( files.Length == 0 )
								continue;

							Array.Sort( files );

							using ( StreamReader sr2 = new StreamReader( files[files.Length - 1] ) )
							{
								if ( isRequest )
								{
									MainForm.Invoke( (Action)( () => {
										APIObserver.Instance.LoadRequest( "/kcsapi/" + line, sr2.ReadToEnd() );
									} ) );
								}
								else
								{
									MainForm.Invoke( (Action)( () => {
										APIObserver.Instance.LoadResponse( "/kcsapi/" + line, sr2.ReadToEnd() );
									} ) );
								}
							}

							//System.Diagnostics.Debug.WriteLine( "APIList Loader: API " + line + " File " + files[files.Length-1] + " Loaded." );
						}
					}
				}

			}

		}





		private void StripMenu_Debug_LoadRecordFromOld_Click( object sender, EventArgs e )
		{

			if ( KCDatabase.Instance.MasterShips.Count == 0 )
			{
				MessageBox.Show( "先に通常の api_start2 を読み込んでください。", "大変ご迷惑をおかけしております", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}


			using ( OpenFileDialog ofd = new OpenFileDialog() )
			{

				ofd.Title = "旧 api_start2 からレコードを構築";
				ofd.Filter = "api_start2|*api_start2*.json|JSON|*.json|File|*";

				if ( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
				{

					try
					{

						using ( StreamReader sr = new StreamReader( ofd.FileName ) )
						{

							dynamic json = DynamicJson.Parse( sr.ReadToEnd().Remove( 0, 7 ) );

							foreach ( dynamic elem in json.api_data.api_mst_ship )
							{
								if ( elem.api_name != "なし" && KCDatabase.Instance.MasterShips.ContainsKey( (int)elem.api_id ) && KCDatabase.Instance.MasterShips[(int)elem.api_id].Name == elem.api_name )
								{
									RecordManager.Instance.ShipParameter.UpdateParameter( (int)elem.api_id, 1, (int)elem.api_tais[0], (int)elem.api_tais[1], (int)elem.api_kaih[0], (int)elem.api_kaih[1], (int)elem.api_saku[0], (int)elem.api_saku[1] );

									int[] defaultslot = Enumerable.Repeat( -1, 5 ).ToArray();
									( (int[])elem.api_defeq ).CopyTo( defaultslot, 0 );
									RecordManager.Instance.ShipParameter.UpdateDefaultSlot( (int)elem.api_id, defaultslot );
								}
							}
						}

					}
					catch ( Exception ex )
					{

						MessageBox.Show( "API読み込みに失敗しました。\r\n" + ex.Message, "エラー",
							MessageBoxButtons.OK, MessageBoxIcon.Error );
					}
				}
			}
		}

		private async void StripMenu_Debug_DeleteOldAPI_Click( object sender, EventArgs e )
		{

			if ( MessageBox.Show( "古いAPIデータを削除します。\r\n本当によろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 )
				== System.Windows.Forms.DialogResult.Yes )
			{

				try
				{

					int count = await Task.Factory.StartNew( () => DeleteOldAPI() );

					MessageBox.Show( "削除が完了しました。\r\n" + count + " 個のファイルを削除しました。", "削除成功", MessageBoxButtons.OK, MessageBoxIcon.Information );

				}
				catch ( Exception ex )
				{

					MessageBox.Show( "削除に失敗しました。\r\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				}


			}

		}

		private int DeleteOldAPI()
		{


			//適当極まりない
			int count = 0;

			var apilist = new Dictionary<string, List<KeyValuePair<string, string>>>();

			foreach ( string s in Directory.EnumerateFiles( Configuration.Config.Connection.SaveDataPath, "*.json", SearchOption.TopDirectoryOnly ) )
			{

				int start = s.IndexOf( '@' );
				int end = s.LastIndexOf( '.' );

				start--;
				string key = s.Substring( start, end - start + 1 );
				string date = s.Substring( 0, start );


				if ( !apilist.ContainsKey( key ) )
				{
					apilist.Add( key, new List<KeyValuePair<string, string>>() );
				}
				apilist[key].Add( new KeyValuePair<string, string>( date, s ) );
			}

			foreach ( var l in apilist.Values )
			{
				var l2 = l.OrderBy( el => el.Key ).ToList();
				for ( int i = 0; i < l2.Count - 1; i++ )
				{
					File.Delete( l2[i].Value );
					count++;
				}
			}

			return count;
		}

		private async void StripMenu_Debug_RenameShipResource_Click( object sender, EventArgs e )
		{

			if ( KCDatabase.Instance.MasterShips.Count == 0 )
			{
				MessageBox.Show( "艦船データが読み込まれていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			if ( MessageBox.Show( "通信から保存した艦船リソース名を持つファイル及びフォルダを、艦船名に置換します。\r\n" +
				"対象は指定されたフォルダ以下のすべてのファイル及びフォルダです。\r\n" +
				"続行しますか？", "艦船リソースをリネーム", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1 )
				== System.Windows.Forms.DialogResult.Yes )
			{

				string path = null;

				using ( FolderBrowserDialog dialog = new FolderBrowserDialog() )
				{
					dialog.SelectedPath = Configuration.Config.Connection.SaveDataPath;
					if ( dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK )
					{
						path = dialog.SelectedPath;
					}
				}

				if ( path == null ) return;



				try
				{

					int count = await Task.Factory.StartNew( () => RenameShipResource( path ) );

					MessageBox.Show( string.Format( "リネーム処理が完了しました。\r\n{0} 個のアイテムをリネームしました。", count ), "処理完了", MessageBoxButtons.OK, MessageBoxIcon.Information );


				}
				catch ( Exception ex )
				{

					ErrorReporter.SendErrorReport( ex, "艦船リソースのリネームに失敗しました。" );
					MessageBox.Show( "艦船リソースのリネームに失敗しました。\r\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );

				}



			}

		}


		private int RenameShipResource( string path )
		{

			int count = 0;

			foreach ( var p in Directory.EnumerateFiles( path, "*", SearchOption.AllDirectories ) )
			{

				string name = Path.GetFileName( p );

				foreach ( var ship in KCDatabase.Instance.MasterShips.Values )
				{

					if ( name.Contains( ship.ResourceName ) )
					{

						name = name.Replace( ship.ResourceName, ship.NameWithClass ).Replace( ' ', '_' );

						try
						{

							File.Move( p, Path.Combine( Path.GetDirectoryName( p ), name ) );
							count++;
							break;

						}
						catch ( IOException )
						{
							//ファイルが既に存在する：＊にぎりつぶす＊
						}

					}

				}

			}

			foreach ( var p in Directory.EnumerateDirectories( path, "*", SearchOption.AllDirectories ) )
			{

				string name = Path.GetFileName( p );        //GetDirectoryName だと親フォルダへのパスになってしまうため

				foreach ( var ship in KCDatabase.Instance.MasterShips.Values )
				{

					if ( name.Contains( ship.ResourceName ) )
					{

						name = name.Replace( ship.ResourceName, ship.NameWithClass ).Replace( ' ', '_' );

						try
						{

							Directory.Move( p, Path.Combine( Path.GetDirectoryName( p ), name ) );
							count++;
							break;

						}
						catch ( IOException )
						{
							//フォルダが既に存在する：＊にぎりつぶす＊
						}
					}

				}

			}


			return count;
		}
	}
}
