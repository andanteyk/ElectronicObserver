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


		protected override bool ShowWithoutActivation { get { return !DialogData.ShowWithActivation; } }


		public DialogNotifier( NotifierDialogData data ) {

			InitializeComponent();

			DialogData = data.Clone();

			Text = DialogData.Title;
			Font = Utility.Configuration.Config.UI.MainFont;
			Icon = Resource.ResourceManager.Instance.AppIcon;
			Padding = new Padding( 4 );

			SetStyle( ControlStyles.UserPaint, true );
			SetStyle( ControlStyles.SupportsTransparentBackColor, true );
			ForeColor = DialogData.ForeColor;
			BackColor = DialogData.BackColor;

			if ( DialogData.DrawsImage && DialogData.Image != null ) {
				ClientSize = DialogData.Image.Size;
			}

			if ( !DialogData.HasFormBorder )
				FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

		}


		private void DialogNotifier_Load( object sender, EventArgs e ) {

			
			//TopMost = DialogData.TopMost;


			Rectangle screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
			switch ( DialogData.Alignment ) {

				case NotifierDialogAlignment.TopLeft:
					Location = new Point( screen.X, screen.Y );
					break;
				case NotifierDialogAlignment.TopCenter:
					Location = new Point( screen.X + ( screen.Width - Width ) / 2, screen.Y );
					break;
				case NotifierDialogAlignment.TopRight:
					Location = new Point( screen.Right - Width, screen.Y );
					break;
				case NotifierDialogAlignment.MiddleLeft:
					Location = new Point( screen.X, screen.Y + ( screen.Height - Height ) / 2 );
					break;
				case NotifierDialogAlignment.MiddleCenter:
					Location = new Point( screen.X + ( screen.Width - Width ) / 2, screen.Y + ( screen.Height - Height ) / 2 );
					break;
				case NotifierDialogAlignment.MiddleRight:
					Location = new Point( screen.Right - Width, screen.Y + ( screen.Height - Height ) / 2 );
					break;
				case NotifierDialogAlignment.BottomLeft:
					Location = new Point( screen.X, screen.Bottom - Height );
					break;
				case NotifierDialogAlignment.BottomCenter:
					Location = new Point( screen.X + ( screen.Width - Width ) / 2, screen.Bottom - Height );
					break;
				case NotifierDialogAlignment.BottomRight:
					Location = new Point( screen.Right - Width, screen.Bottom - Height );
					break;
				case NotifierDialogAlignment.Custom:
				case NotifierDialogAlignment.CustomRelative:
					Location = new Point( DialogData.Location.X, DialogData.Location.Y );
					break;

			}

			if ( DialogData.ClosingInterval > 0 ) {
				CloseTimer.Interval = DialogData.ClosingInterval;
				CloseTimer.Start();
			}
		}


		protected override CreateParams CreateParams {
			get {
				var cp = base.CreateParams;
				if ( DialogData != null && DialogData.TopMost )
					cp.ExStyle |= 0x8;		//set topmost flag
				return cp;
			}
		}


		private void DialogNotifier_Paint( object sender, PaintEventArgs e ) {


			Graphics g = e.Graphics;
			g.Clear( BackColor );

			try {

				if ( DialogData.DrawsImage && DialogData.Image != null ) {

					g.DrawImage( DialogData.Image, new Rectangle( 0, 0, DialogData.Image.Width, DialogData.Image.Height ) );
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
