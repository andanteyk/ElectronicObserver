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

		//共用されるので注意！
		//fixme: バグの温床になりそうなので別の仕様を考える
		public NotifierBase Notifier { get; set; }

		private string Message;

		
		public DialogNotifier( NotifierBase parent ) {
			InitializeComponent();

			Notifier = parent;

			Message = parent.Message;
			Text = Notifier.Title;
			Font = Utility.Configuration.Config.UI.MainFont;
			Icon = Resource.ResourceManager.Instance.AppIcon;
			Padding = new Padding( 4 );
		}


		private void DialogNotifier_Load( object sender, EventArgs e ) {

			if ( Notifier.Image != null ) {
				ClientSize = Notifier.Image.Size;
				FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			}

			

			StartPosition = FormStartPosition.WindowsDefaultLocation;		//undone: 定位置も設定できるようにする。

			if ( Notifier.AutoClosingInterval > 0 ) {
				CloseTimer.Interval = Notifier.AutoClosingInterval;
				CloseTimer.Start();
			}
		}


		private void DialogNotifier_Paint( object sender, PaintEventArgs e ) {

			Graphics g = e.Graphics;

			if ( Notifier.Image != null ) {

				//image mode

				g.DrawImage( Notifier.Image, 0, 0 );


			} else {

				//string mode
				TextRenderer.DrawText( g, Message, Font, new Rectangle( Padding.Left, Padding.Right, ClientSize.Width - Padding.Horizontal, ClientSize.Height - Padding.Vertical ), ForeColor, TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak );

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
			if ( false ) {		//undone: Configuration.xxx.CloseOnMouseMove == true then...
				Close();
			}
		}


		private void CloseTimer_Tick( object sender, EventArgs e ) {
			Close();
		}


	}
}
