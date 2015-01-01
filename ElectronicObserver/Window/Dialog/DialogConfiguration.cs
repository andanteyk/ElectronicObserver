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
	public partial class DialogConfiguration : Form {
		public DialogConfiguration() {
			InitializeComponent();
		}

		private void Connection_SaveReceivedData_CheckedChanged( object sender, EventArgs e ) {

			Connection_PanelSaveData.Enabled = Connection_SaveReceivedData.Checked;

		}


		private void Connection_SaveDataPath_TextChanged( object sender, EventArgs e ) {

			if ( Directory.Exists( Connection_SaveDataPath.Text ) ) {
				Connection_SaveDataPath.BackColor = SystemColors.Window;
				ToolTipInfo.SetToolTip( Connection_SaveDataPath, null );
			} else {
				Connection_SaveDataPath.BackColor = Color.MistyRose;
				ToolTipInfo.SetToolTip( Connection_SaveDataPath, "指定されたフォルダは存在しません。" );
			}
		}


		/// <summary>
		/// パラメータの更新をUIに適用します。
		/// </summary>
		internal void UpdateParameter() {

			Connection_SaveReceivedData_CheckedChanged( null, new EventArgs() );
			Connection_SaveDataPath_TextChanged( null, new EventArgs() );

		}



		private void Connection_SaveDataPathSearch_Click( object sender, EventArgs e ) {

			if ( Directory.Exists( Connection_SaveDataPath.Text ) ) {
				FolderBrowser.SelectedPath = Connection_SaveDataPath.Text;
			}

			if ( FolderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

				Connection_SaveDataPath.Text = FolderBrowser.SelectedPath;
			}

		}

		
	}
}
