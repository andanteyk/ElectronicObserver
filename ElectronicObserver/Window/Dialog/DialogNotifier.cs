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
	public partial class DialogNotifier : Form {

		
		public NotifierDialogData DialogData { get; set; }

	
		public DialogNotifier( NotifierDialogData data ) {
			InitializeComponent();

			DialogData = data.Clone();

			Text = DialogData.Title;
			Font = Utility.Configuration.Config.UI.MainFont;
			Icon = Resource.ResourceManager.Instance.AppIcon;
			Padding = new Padding( 4 );
		}


		private void DialogNotifier_Load( object sender, EventArgs e ) {

			if ( DialogData.Image != null ) {
				ClientSize = DialogData.Image.Size;
				FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			}

			

			StartPosition = FormStartPosition.WindowsDefaultLocation;		//undone: 定位置も設定できるようにする。

			if ( DialogData.ClosingInterval > 0 ) {
				CloseTimer.Interval = DialogData.ClosingInterval;
				CloseTimer.Start();
			}
		}


		private void DialogNotifier_Paint( object sender, PaintEventArgs e ) {

			Graphics g = e.Graphics;

			try {
	
				if ( DialogData.DrawsImage && DialogData.Image != null ) {

					g.DrawImage( DialogData.Image, 0, 0 );
				} 
			
				if ( DialogData.DrawsMessage ) {

					TextRenderer.DrawText( g, DialogData.Message, Font, new Rectangle( Padding.Left, Padding.Right, ClientSize.Width - Padding.Horizontal, ClientSize.Height - Padding.Vertical ), ForeColor, TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak );
				}

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "通知システム: ダイアログボックスでの画像の描画に失敗しました。" );
			}
		}


		private void DialogNotifier_Click( object sender, EventArgs e ) {
			Close();
		}

		private void DialogNotifier_KeyDown( object sender, KeyEventArgs e ) {
			if ( e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape )
				Close();
		}

		private void DialogNotifier_MouseMove( object sender, MouseEventArgs e ) {
			if ( DialogData.CloseOnMouseMove ) {
				Close();
			}
		}


		private void CloseTimer_Tick( object sender, EventArgs e ) {
			Close();
		}


	}
}
