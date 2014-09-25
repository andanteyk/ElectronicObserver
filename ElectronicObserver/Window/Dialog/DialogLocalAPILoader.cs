using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ElectronicObserver.Window.Dialog {
	public partial class DialogLocalAPILoader : Form {

		public string FilePath {
			get { return TextFilePath.Text; }
		}

		public string FileData {
			get {
				try {

					using ( var sr = new StreamReader( TextFilePath.Text ) ) {
						return sr.ReadToEnd();
					}

				} catch ( Exception ) {

					return null;
				}
			}
		}

		public string APIPath {
			get {
				if ( APIList.SelectedIndex != -1 )
					return "kcsapi/" + APIList.SelectedItem.ToString();
				else
					return null;
			}
		}

		public bool IsRequest {
			get { return APICategory.SelectedIndex == 0; }
		}

		public bool IsResponse {
			get { return APICategory.SelectedIndex == 1; }
		}


		public DialogLocalAPILoader() {
			InitializeComponent();
		}


		private void DialogLocalAPILoader_Load( object sender, EventArgs e ) {

			Icon iconWarning = SystemIcons.Warning;
			Bitmap bmp = new Bitmap( PictureWarning.Width, PictureWarning.Height );
			Graphics g = Graphics.FromImage( bmp );

			g.DrawIcon( iconWarning, 0, 0 );
			PictureWarning.Image = bmp;

			g.Dispose();


			APICategory.SelectedIndex = 1;
		}


		private void APICategory_SelectedIndexChanged( object sender, EventArgs e ) {

			APIList.Items.Clear();
			if ( APICategory.SelectedIndex == 0 ) {
				//request
				APIList.Items.Add( "api_req_quest/clearitemget" );
				APIList.Items.Add( "api_req_nyukyo/start" );
				APIList.Items.Add( "api_req_kousyou/createship_speedchange" );

			} else {
				//response

				APIList.Items.Add( "api_start2" );
				APIList.Items.Add( "api_get_member/basic" );
				APIList.Items.Add( "api_get_member/slot_item" );
				APIList.Items.Add( "api_get_member/useitem" );
				APIList.Items.Add( "api_get_member/kdock" );
				APIList.Items.Add( "api_port/port" );
				APIList.Items.Add( "api_get_member/ship2" );
				APIList.Items.Add( "api_get_member/questlist" );
				APIList.Items.Add( "api_get_member/ndock" );
				APIList.Items.Add( "api_req_kousyou/getship" );
			}
			
			APIList.SelectedIndex = 0;

		}


		private void ButtonSearchFilePath_Click( object sender, EventArgs e ) {

			if ( File.Exists( TextFilePath.Text ) )
				FileOpener.FileName = TextFilePath.Text;

			FileOpener.Filter = APIList.SelectedItem.ToString() + "|*" + ( APICategory.SelectedIndex == 0 ? "Q" : "S" ) + "@" + APIList.SelectedItem.ToString().Replace( '/', '@' ) + ".json|JSON|*.json;*.js|File|*";

			if ( FileOpener.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
				TextFilePath.Text = FileOpener.FileName;
			}

		}



		private void ButtonOpen_Click( object sender, EventArgs e ) {
			DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void ButtonCancel_Click( object sender, EventArgs e ) {
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}


	}
}
