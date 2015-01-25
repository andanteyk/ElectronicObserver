using ElectronicObserver.Notifier;
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
	/// undone
	/// 通知システムの設定ダイアログを扱います。
	/// </summary>
	public partial class DialogConfigurationNotifier : Form {

		public DialogConfigurationNotifier( NotifierBase notifier ) {
			InitializeComponent();

			IsEnabled.Checked = notifier.IsEnabled;

			PlaysSound.Checked = notifier.PlaysSound;
			SoundPath.Text = notifier.SoundPath;
			
			DrawsImage.Checked = notifier.DialogData.DrawsImage;
			ImagePath.Text = notifier.DialogData.ImagePath;

			ShowsDialog.Checked = notifier.ShowsDialog;
			TopMostFlag.Checked = notifier.DialogData.TopMost;
			Alignment.SelectedIndex = (int)notifier.DialogData.Alignment;
			LocationX.Value = notifier.DialogData.Location.X;
			LocationY.Value = notifier.DialogData.Location.Y;
			DrawsMessage.Checked = notifier.DialogData.DrawsMessage;
			HasFormBorder.Checked = notifier.DialogData.HasFormBorder;
			AccelInterval.Value = notifier.AccelInterval;
			ClosingInterval.Value = notifier.DialogData.ClosingInterval;
			CloseOnMouseOver.Checked = notifier.DialogData.CloseOnMouseMove;
			ForeColorPreview.ForeColor = notifier.DialogData.ForeColor;
			BackColorPreview.ForeColor = notifier.DialogData.BackColor;

			NotifierDamage ndmg = notifier as NotifierDamage;
			if ( ndmg != null ) {
				NotifiesBefore.Checked = ndmg.NotifiesBefore;
				NotifiesNow.Checked = ndmg.NotifiesNow;
				NotifiesAfter.Checked = ndmg.NotifiesAfter;
				ContainsNotLockedShip.Checked = ndmg.ContainsNotLockedShip;
				ContainsSafeShip.Checked = ndmg.ContainsSafeShip;
				ContainsFlagship.Checked = ndmg.ContainsFlagship;
				LevelBorder.Value = ndmg.LevelBorder;

			} else {
				GroupDamage.Visible = false;
				GroupDamage.Enabled = false;
			}

		}

		private void DialogConfigurationNotifier_Load( object sender, EventArgs e ) {

		}
	}
}
