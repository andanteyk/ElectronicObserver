using ElectronicObserver.Notifier;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Support;
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

	/// <summary>
	/// 通知システムの設定ダイアログを扱います。
	/// </summary>
	public partial class DialogConfigurationNotifier : Form {

		private NotifierBase _notifier;
		private bool _soundChanged;
		private bool _imageChanged;

		public DialogConfigurationNotifier( NotifierBase notifier ) {
			this.SuspendLayoutForDpiScale();
			InitializeComponent();

			_notifier = notifier;

			//init base
			_soundChanged = false;
			_imageChanged = false;

			GroupSound.AllowDrop = true;
			GroupImage.AllowDrop = true;



			//init from data

			IsEnabled.Checked = notifier.IsEnabled;

			PlaysSound.Checked = notifier.PlaysSound;
			SoundPath.Text = notifier.SoundPath;
			SoundVolume.Value = notifier.SoundVolume;
			LoopsSound.Checked = notifier.LoopsSound;

			DrawsImage.Checked = notifier.DialogData.DrawsImage;
			ImagePath.Text = notifier.DialogData.ImagePath;

			ShowsDialog.Checked = notifier.ShowsDialog;
			TopMostFlag.Checked = notifier.DialogData.TopMost;
			Alignment.SelectedIndex = (int)notifier.DialogData.Alignment;
			LocationX.Value = notifier.DialogData.Location.X;
			LocationY.Value = notifier.DialogData.Location.Y;
			DrawsMessage.Checked = notifier.DialogData.DrawsMessage;
			HasFormBorder.Checked = notifier.DialogData.HasFormBorder;
			AccelInterval.Value = notifier.AccelInterval / 1000;
			ClosingInterval.Value = notifier.DialogData.ClosingInterval / 1000;
			for ( int i = 0; i < (int)NotifierDialogClickFlags.HighestBit; i++ )
				CloseList.SetItemChecked( i, ( (int)notifier.DialogData.ClickFlag & ( 1 << i ) ) != 0 );
			CloseList.SetItemChecked( (int)NotifierDialogClickFlags.HighestBit, notifier.DialogData.CloseOnMouseMove );
			ShowWithActivation.Checked = notifier.DialogData.ShowWithActivation;
			ForeColorPreview.ForeColor = notifier.DialogData.ForeColor;
			BackColorPreview.ForeColor = notifier.DialogData.BackColor;
			LevelBorder.Maximum = ExpTable.ShipMaximumLevel;

			NotifierDamage ndmg = notifier as NotifierDamage;
			if ( ndmg != null ) {
				NotifiesBefore.Checked = ndmg.NotifiesBefore;
				NotifiesNow.Checked = ndmg.NotifiesNow;
				NotifiesAfter.Checked = ndmg.NotifiesAfter;
				ContainsNotLockedShip.Checked = ndmg.ContainsNotLockedShip;
				ContainsSafeShip.Checked = ndmg.ContainsSafeShip;
				ContainsFlagship.Checked = ndmg.ContainsFlagship;
				LevelBorder.Value = ndmg.LevelBorder;
				NotifiesAtEndpoint.Checked = ndmg.NotifiesAtEndpoint;

			} else {
				GroupDamage.Visible = false;
				GroupDamage.Enabled = false;
			}


			DialogOpenSound.Filter = "音楽ファイル|" + string.Join( ";", Utility.MediaPlayer.SupportedExtensions.Select( s => "*." + s ) ) + "|File|*";

			this.ResumeLayoutForDpiScale();
		}

		private void DialogConfigurationNotifier_Load( object sender, EventArgs e ) {

		}



		private void GroupSound_DragEnter( object sender, DragEventArgs e ) {

			if ( e.Data.GetDataPresent( DataFormats.FileDrop ) ) {
				e.Effect = DragDropEffects.Copy;
			} else {
				e.Effect = DragDropEffects.None;
			}

		}

		private void GroupSound_DragDrop( object sender, DragEventArgs e ) {

			SoundPath.Text = ( (string[])e.Data.GetData( DataFormats.FileDrop ) )[0];
		}

		private void GroupImage_DragEnter( object sender, DragEventArgs e ) {

			if ( e.Data.GetDataPresent( DataFormats.FileDrop ) ) {
				e.Effect = DragDropEffects.Copy;
			} else {
				e.Effect = DragDropEffects.None;
			}

		}

		private void GroupImage_DragDrop( object sender, DragEventArgs e ) {

			ImagePath.Text = ( (string[])e.Data.GetData( DataFormats.FileDrop ) )[0];
		}



		private void SoundPath_TextChanged( object sender, EventArgs e ) {

			_soundChanged = true;
		}

		private void ImagePath_TextChanged( object sender, EventArgs e ) {

			_imageChanged = true;
		}

		private void SoundPathSearch_Click( object sender, EventArgs e ) {

			if ( SoundPath.Text != "" ) {
				try {
					DialogOpenSound.InitialDirectory = System.IO.Path.GetDirectoryName( SoundPath.Text );

				} catch ( Exception ) { }
			}

			if ( DialogOpenSound.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
				SoundPath.Text = DialogOpenSound.FileName;
			}

		}

		private void ImagePathSearch_Click( object sender, EventArgs e ) {

			if ( ImagePath.Text != "" ) {
				try {
					DialogOpenImage.InitialDirectory = System.IO.Path.GetDirectoryName( ImagePath.Text );

				} catch ( Exception ) { }
			}

			if ( DialogOpenImage.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
				ImagePath.Text = DialogOpenImage.FileName;
			}
		}


		private void ForeColorSelect_Click( object sender, EventArgs e ) {

			DialogColor.Color = ForeColorPreview.ForeColor;
			if ( DialogColor.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
				ForeColorPreview.ForeColor = DialogColor.Color;
			}
		}

		private void BackColorSelect_Click( object sender, EventArgs e ) {

			DialogColor.Color = BackColorPreview.ForeColor;
			if ( DialogColor.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
				BackColorPreview.ForeColor = DialogColor.Color;
			}
		}


		private void ForeColorPreview_ForeColorChanged( object sender, EventArgs e ) {

			if ( ForeColorPreview.ForeColor.GetBrightness() >= 0.5 ) {
				ForeColorPreview.BackColor = Color.Black;
			} else {
				ForeColorPreview.BackColor = Color.White;
			}
		}

		private void BackColorPreview_ForeColorChanged( object sender, EventArgs e ) {

			if ( BackColorPreview.ForeColor.GetBrightness() >= 0.5 ) {
				BackColorPreview.BackColor = Color.Black;
			} else {
				BackColorPreview.BackColor = Color.White;
			}
		}



		private void ButtonOK_Click( object sender, EventArgs e ) {

			if ( !SetConfiguration() ) return;

			DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void ButtonCancel_Click( object sender, EventArgs e ) {

			DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}


		private bool SetConfiguration() {


			if ( _soundChanged ) {
				if ( !_notifier.LoadSound( SoundPath.Text ) && PlaysSound.Checked ) {
					MessageBox.Show( "声音文件读取失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error );
					return false;
				}
			}
			if ( _imageChanged ) {
				if ( !_notifier.DialogData.LoadImage( ImagePath.Text ) && DrawsImage.Checked ) {
					MessageBox.Show( "图像文件读取失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error );
					return false;
				}
			}


			//set configuration
			_notifier.IsEnabled = IsEnabled.Checked;

			_notifier.PlaysSound = PlaysSound.Checked;
			_notifier.DialogData.DrawsImage = DrawsImage.Checked;
			_notifier.SoundVolume = (int)SoundVolume.Value;
			_notifier.LoopsSound = LoopsSound.Checked;

			_notifier.ShowsDialog = ShowsDialog.Checked;
			_notifier.DialogData.TopMost = TopMostFlag.Checked;
			_notifier.DialogData.Alignment = (NotifierDialogAlignment)Alignment.SelectedIndex;
			_notifier.DialogData.Location = new Point( (int)LocationX.Value, (int)LocationY.Value );
			_notifier.DialogData.DrawsMessage = DrawsMessage.Checked;
			_notifier.DialogData.HasFormBorder = HasFormBorder.Checked;
			_notifier.AccelInterval = (int)( AccelInterval.Value * 1000 );
			_notifier.DialogData.ClosingInterval = (int)( ClosingInterval.Value * 1000 );
			{
				int flag = 0;
				for ( int i = 0; i < (int)NotifierDialogClickFlags.HighestBit; i++ )
					flag |= ( CloseList.GetItemChecked( i ) ? 1 : 0 ) << i;
				_notifier.DialogData.ClickFlag = (NotifierDialogClickFlags)flag;
			}
			_notifier.DialogData.CloseOnMouseMove = CloseList.GetItemChecked( (int)NotifierDialogClickFlags.HighestBit );
			_notifier.DialogData.ForeColor = ForeColorPreview.ForeColor;
			_notifier.DialogData.BackColor = BackColorPreview.ForeColor;
			_notifier.DialogData.ShowWithActivation = ShowWithActivation.Checked;

			NotifierDamage ndmg = _notifier as NotifierDamage;
			if ( ndmg != null ) {
				ndmg.NotifiesBefore = NotifiesBefore.Checked;
				ndmg.NotifiesNow = NotifiesNow.Checked;
				ndmg.NotifiesAfter = NotifiesAfter.Checked;
				ndmg.ContainsNotLockedShip = ContainsNotLockedShip.Checked;
				ndmg.ContainsSafeShip = ContainsSafeShip.Checked;
				ndmg.ContainsFlagship = ContainsFlagship.Checked;
				ndmg.LevelBorder = (int)LevelBorder.Value;
				ndmg.NotifiesAtEndpoint = NotifiesAtEndpoint.Checked;
			}

			return true;
		}


		private void ButtonTest_Click( object sender, EventArgs e ) {

			if ( !SetConfiguration() ) return;
			_notifier.DialogData.Message = "测试通知。";
			_notifier.Notify();

		}

		private void SoundPathDirectorize_Click( object sender, EventArgs e ) {
			if ( !string.IsNullOrWhiteSpace( SoundPath.Text ) ) {
				try {
					SoundPath.Text = System.IO.Path.GetDirectoryName( SoundPath.Text );
				} catch ( Exception ) {
					// *ぷちっ*
				}
			}
		}


	}
}
