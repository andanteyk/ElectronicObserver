using ElectronicObserver.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog {
	public partial class DialogConfigurationBGMPlayer : Form {

		public SyncBGMPlayer.SoundHandle ResultHandle { get; private set; }

		public DialogConfigurationBGMPlayer( SyncBGMPlayer.SoundHandle handle ) {
			InitializeComponent();

			FilePath.Text = handle.Path;
			IsLoop.Checked = handle.IsLoop;
			LoopHeadPosition.Value = (decimal)handle.LoopHeadPosition;
			Volume.Value = handle.Volume;

			Text = "BGM设定 - " + SyncBGMPlayer.SoundHandleIDToString( handle.HandleID );
			ResultHandle = handle.Clone();
		}

		private void DialogConfigurationBGMPlayer_Load( object sender, EventArgs e ) {
			OpenMusicDialog.Filter = "音乐文件|" + string.Join( ";", MediaPlayer.SupportedExtensions.Select( s => "*." + s ) ) + "|所有文件|*";
		}

		private void ButtonAccept_Click( object sender, EventArgs e ) {
			ResultHandle.Path = FilePath.Text;
			ResultHandle.IsLoop = IsLoop.Checked;
			ResultHandle.LoopHeadPosition = (double)LoopHeadPosition.Value;
			ResultHandle.Volume = (int)Volume.Value;

			DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void ButtonCancel_Click( object sender, EventArgs e ) {
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}

		private void FilePath_DragEnter( object sender, DragEventArgs e ) {
			if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;
		}

		private void FilePath_DragDrop( object sender, DragEventArgs e ) {
			FilePath.Text = ( (string[])e.Data.GetData( DataFormats.FileDrop ) )[0];
		}

		private void FilePathSearch_Click( object sender, EventArgs e ) {
			if ( OpenMusicDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
				FilePath.Text = OpenMusicDialog.FileName;
			}
		}
	}
}
