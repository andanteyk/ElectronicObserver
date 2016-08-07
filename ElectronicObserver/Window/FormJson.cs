using ElectronicObserver.Observer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {
	public partial class FormJson : DockContent {


		// yyyyMMdd_hhmmssff[S|Q]@api_path.json
		private static readonly Regex _filenamePattern = new Regex( @"\d{8}_\d{8}([SQ])@(.*)\.json$", RegexOptions.Compiled );

		private Regex _apiPattern;

		private string _currentAPIPath;
		private dynamic _currentData;


		public FormJson( FormMain parent ) {
			InitializeComponent();


			ConfigurationChanged();
		}

		private void FormJson_Load( object sender, EventArgs e ) {

			var o = APIObserver.Instance;

			o.RequestReceived += RequestReceived;
			o.ResponseReceived += ResponseReceived;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}


		void RequestReceived( string apiname, dynamic data ) {

			if ( !AutoUpdate.Checked )
				return;

			if ( _apiPattern != null && !_apiPattern.Match( apiname ).Success )
				return;


			LoadRequest( apiname, data );
		}

		void ResponseReceived( string apiname, dynamic data ) {

			if ( !AutoUpdate.Checked )
				return;

			if ( _apiPattern != null && !_apiPattern.Match( apiname ).Success )
				return;


			LoadResponse( apiname, data );
		}



		private void LoadRequest( string apiname, Dictionary<string, string> data ) {

			JsonRawData.Text = apiname + " : Request\r\n" + string.Join( "\r\n", data.Select( p => p.Key + "=" + p.Value ) );


			if ( !UpdatesTree.Checked )
				return;


			JsonTreeView.BeginUpdate();


			JsonTreeView.Nodes.Clear();
			JsonTreeView.Nodes.Add( apiname );

			TreeNode root = new TreeNode( "<Request> : {" + data.Count + "}" );
			root.Name = "<Request>";
			root.Nodes.AddRange( data.Select( e => new TreeNode( e.Key + " : " + e.Value ) ).ToArray() );

			JsonTreeView.Nodes.Add( root );


			JsonTreeView.EndUpdate();
			_currentAPIPath = apiname;
		}

		private void LoadResponse( string apiname, dynamic data ) {


			JsonRawData.Text = ( _currentAPIPath == apiname ? JsonRawData.Text + "\r\n\r\n" : "" ) + apiname + " : Response\r\n" + ( data == null ? "" : data.ToString() );
			_currentData = data;

			if ( !UpdatesTree.Checked )
				return;


			JsonTreeView.BeginUpdate();


			if ( JsonTreeView.Nodes.Count == 0 || JsonTreeView.Nodes[0].Text != apiname ) {
				JsonTreeView.Nodes.Clear();
				JsonTreeView.Nodes.Add( apiname );
			}

			TreeNode root = new TreeNode( "<root>" );
			AddNode( root, "<Response>", -1, data );

			JsonTreeView.Nodes.Add( root.FirstNode );


			JsonTreeView.EndUpdate();
			_currentAPIPath = apiname;
		}





		private void LoadFromFile( string path ) {


			var match = _filenamePattern.Match( path );

			if ( match.Success ) {

				try {

					using ( var sr = new StreamReader( path ) ) {

						string data = sr.ReadToEnd();


						if ( match.Groups[1].Value == "Q" ) {
							// request

							var parsedData = new Dictionary<string, string>();
							data = System.Web.HttpUtility.UrlDecode( data );

							foreach ( string unit in data.Split( "&".ToCharArray() ) ) {
								string[] pair = unit.Split( "=".ToCharArray() );
								parsedData.Add( pair[0], pair[1] );
							}

							LoadRequest( match.Groups[2].Value.Replace( '@', '/' ), parsedData );


						} else if ( match.Groups[1].Value == "S" ) {
							//response

							int head = data.IndexOfAny( "[{".ToCharArray() );
							if ( head == -1 )
								throw new ArgumentException( "JSON の開始文字を検出できませんでした。" );
							data = data.Substring( head );

							LoadResponse( match.Groups[2].Value.Replace( '@', '/' ), Codeplex.Data.DynamicJson.Parse( data ) );

						}

					}

				} catch ( Exception ex ) {

					Utility.ErrorReporter.SendErrorReport( ex, "JSON データの読み込みに失敗しました。" );
				}


			}
		}



		/// <summary>
		/// JSON データから、TreeNodeを構成します。
		/// </summary>
		/// <param name="root">親になるノード。</param>
		/// <param name="name">生成するノード名。</param>
		/// <param name="index">生成するノードが配列の要素だった場合、そのインデックス。そうでなければ -1</param>
		/// <param name="data">もととなる JSON データ。</param>
		private TreeNode AddNode( TreeNode root, string name, int index, dynamic data ) {

			TreeNode self = root.Nodes.Add( string.IsNullOrEmpty( name ) ? "" : ( name + " : " ) );
			self.Name = name + ( index >= 0 ? "[" + index + "]" : "" );

			if ( data == null ) {
				self.Text += "null";

			} else if ( data is string ) {
				self.Text += "\"" + data + "\"";

			} else if ( data is bool || data is double ) {
				self.Text += data.ToString();

			} else if ( data.IsArray ) {
				int count = 0;
				foreach ( var elem in data ) {
					AddNode( self, null, count, elem );
					count++;
				}
				self.Text += "[" + count + "]";
				self.Tag = 1;

			} else if ( data.IsObject ) {
				int count = 0;
				foreach ( KeyValuePair<string, dynamic> elem in data ) {
					AddNode( self, elem.Key, -1, elem.Value );
					count++;
				}
				self.Text += "{" + count + "}";
				self.Tag = 2;

			} else {
				throw new NotImplementedException();
			}

			return root;
		}




		void ConfigurationChanged() {

			var c = Utility.Configuration.Config;

			Font = tabControl1.Font = c.UI.MainFont;

			AutoUpdate.Checked = c.FormJson.AutoUpdate;
			UpdatesTree.Checked = c.FormJson.UpdatesTree;
			AutoUpdateFilter.Text = c.FormJson.AutoUpdateFilter;

			try {
				_apiPattern = new Regex( c.FormJson.AutoUpdateFilter );

			} catch ( Exception ) {
				Utility.Logger.Add( 3, "JSON ウィンドウ：フィルタが不正です。" );
				_apiPattern = null;
			}
		}


		private void tabControl1_DragEnter( object sender, DragEventArgs e ) {

			if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;

		}

		private void tabControl1_DragDrop( object sender, DragEventArgs e ) {

			foreach ( string path in ( (string[])e.Data.GetData( DataFormats.FileDrop ) ).OrderBy( s => s ) )
				LoadFromFile( path );

		}


		private void UpdatesTree_CheckedChanged( object sender, EventArgs e ) {

			JsonTreeView.Nodes.Clear();

			if ( !UpdatesTree.Checked ) {
				JsonTreeView.Nodes.Add( "<更新が無効になっています。Configから有効化してください。>" );

			}

			Utility.Configuration.Config.FormJson.UpdatesTree = UpdatesTree.Checked;

		}

		private void AutoUpdate_CheckedChanged( object sender, EventArgs e ) {
			Utility.Configuration.Config.FormJson.AutoUpdate = AutoUpdate.Checked;
		}

		private void AutoUpdateFilter_Validated( object sender, EventArgs e ) {
			var c = Utility.Configuration.Config.FormJson;
			c.AutoUpdateFilter = AutoUpdateFilter.Text;

			try {
				_apiPattern = new Regex( c.AutoUpdateFilter );

			} catch ( Exception ) {
				Utility.Logger.Add( 3, "JSON ウィンドウ：フィルタが不正です。フィルタはクリアされます。" );
				c.AutoUpdateFilter = AutoUpdateFilter.Text = "";
				_apiPattern = null;
			}
		}


		private void TreeContextMenu_Expand_Click( object sender, EventArgs e ) {
			JsonTreeView.SelectedNode.ExpandAll();
		}
		private void TreeContextMenu_Shrink_Click( object sender, EventArgs e ) {
			JsonTreeView.SelectedNode.Collapse();
		}
		private void TreeContextMenu_ShrinkParent_Click( object sender, EventArgs e ) {
			var node = JsonTreeView.SelectedNode.Parent;
			if ( node != null )
				node.Collapse();
		}


		private void TreeContextMenu_Opening( object sender, CancelEventArgs e ) {

			var root = JsonTreeView.SelectedNode;

			// root is array, children > 0, root[0](=child) is object or array
			if ( ( root.Tag as int? ?? 0 ) == 1 && root.Nodes.Count > 0 && ( root.FirstNode.Tag as int? ?? 0 ) != 0 ) {
				TreeContextMenu_OutputCSV.Enabled = true;

			} else {
				TreeContextMenu_OutputCSV.Enabled = false;
			}
		}

		private void TreeContextMenu_OutputCSV_Click( object sender, EventArgs e ) {

			if ( CSVSaver.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

				try {

					using ( var sw = new StreamWriter( CSVSaver.FileName, false, Utility.Configuration.Config.Log.FileEncoding ) ) {

						var root = JsonTreeView.SelectedNode;
						var data = _currentData;


						// root にあたる実データの取得
						List<string> revpath = new List<string>();
						var ptr = root;

						while ( ptr.Parent != null ) {
							revpath.Add( ptr.Name );
							ptr = ptr.Parent;
						}

						foreach ( var path in revpath.Reverse<string>() ) {
							int index = path.IndexOf( '[' );
							if ( index != -1 ) {
								int indexer = int.Parse( path.Substring( index + 1, path.Length - index - 2 ) );
								data = data[path.Substring( 0, index - 1 )][indexer];

							} else {
								data = data[path];
							}
						}


						// ヘッダーの取得
						var content = new StringBuilder();
						BuildCSVHeader( content, "", data[0] );
						content.Remove( content.Length - 1, 1 );

						sw.WriteLine( content.ToString() );


						foreach ( dynamic elem in data ) {
							content.Clear();
							BuildCSVContent( content, elem );
							content.Remove( content.Length - 1, 1 );

							sw.WriteLine( content.ToString() );
						}
					}


				} catch ( Exception ex ) {
					Utility.ErrorReporter.SendErrorReport( ex, "JSON: CSV 出力に失敗しました。" );
					MessageBox.Show( "CSV 出力に失敗しました。\r\n" + ex.Message, "出力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				}


			}

		}

		private void BuildCSVHeader( StringBuilder sb, string currentPath, dynamic data ) {

			if ( data is Codeplex.Data.DynamicJson ) {

				if ( data.IsObject ) {
					foreach ( string p in data.GetDynamicMemberNames() ) {
						BuildCSVHeader( sb, currentPath + "." + p, data[p] );
					}
					return;

				} else if ( data.IsArray ) {
					int index = 0;
					foreach ( dynamic elem in data ) {
						BuildCSVHeader( sb, currentPath + "[" + index + "]", elem );
						index++;
					}
					return;

				}
			}

			sb.Append( currentPath ).Append( "," );
		}

		private void BuildCSVContent( StringBuilder sb, dynamic data ) {

			if ( data is Codeplex.Data.DynamicJson ) {

				if ( data.IsObject ) {
					foreach ( string p in data.GetDynamicMemberNames() ) {
						BuildCSVContent( sb, data[p] );
					}
					return;

				} else if ( data.IsArray ) {
					foreach ( dynamic elem in data ) {
						BuildCSVContent( sb, elem );
					}
					return;

				}
			}

			sb.Append( data ).Append( "," );
		}


		// 右クリックでも選択するように
		private void JsonTreeView_MouseClick( object sender, MouseEventArgs e ) {
			var node = JsonTreeView.GetNodeAt( e.Location );
			if ( node != null ) {
				JsonTreeView.SelectedNode = node;
			}
		}





		protected override string GetPersistString() {
			return "Json";
		}




	}
}
