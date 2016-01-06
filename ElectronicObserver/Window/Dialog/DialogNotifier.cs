using ElectronicObserver.Notifier;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog {

	/// <summary>
	/// 通知ダイアログ
	/// </summary>
	public partial class DialogNotifier : Form {


		public NotifierDialogData DialogData { get; set; }


		private bool IsLayeredWindow { get { return DialogData != null ? !DialogData.HasFormBorder && DialogData.DrawsImage : false; } }

		protected override bool ShowWithoutActivation { get { return !DialogData.ShowWithActivation; } }



		public DialogNotifier( NotifierDialogData data ) {
			this.SuspendLayoutForDpiScale();

			InitializeComponent();

			DialogData = data.Clone();

			Text = DialogData.Title;
			Font = Utility.Configuration.Config.UI.MainFont;
			Icon = Resource.ResourceManager.Instance.AppIcon;
			Padding = new Padding( 4 );

			//SetStyle( ControlStyles.UserPaint, true );
			//SetStyle( ControlStyles.SupportsTransparentBackColor, true );
			ForeColor = DialogData.ForeColor;
			BackColor = DialogData.BackColor;

			if ( DialogData.DrawsImage && DialogData.Image != null ) {
				ClientSize = DialogData.Image.Size;
			}

			if ( !DialogData.HasFormBorder )
				FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

			data.CloseAll += data_CloseAll;
			this.ResumeLayoutForDpiScale();

		}




		private void DialogNotifier_Load( object sender, EventArgs e ) {


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

			if ( IsLayeredWindow ) {

				Size size = DialogData.Image != null ? DialogData.Image.Size : new Size( 300, 100 );

				// メッセージを書き込んだうえでレイヤードウィンドウ化する
				using ( var bmp = new Bitmap( size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb ) ) {

					using ( var g = Graphics.FromImage( bmp ) ) {

						g.Clear( Color.FromArgb( 0, 0, 0, 0 ) );
						g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
						g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

						if ( DialogData.Image != null )
							g.DrawImage( DialogData.Image, new Rectangle( 0, 0, bmp.Width, bmp.Height ) );
						else
							g.Clear( DialogData.BackColor );
						//DrawMessage( g );

						//*/
						if ( DialogData.DrawsMessage ) {

							// fixme: どうしても滑らかにフォントが描画できなかったので超絶苦肉の策

							using ( var path = new GraphicsPath() ) {

								path.AddString( DialogData.Message, Font.FontFamily, (int)Font.Style, Font.Size, new RectangleF( Padding.Left, Padding.Top, ClientSize.Width - Padding.Horizontal, ClientSize.Height - Padding.Vertical ), StringFormat.GenericDefault );

								using ( var brush = new SolidBrush( ForeColor ) ) {
									g.FillPath( brush, path );
								}
							}
						}
						//*/
					}

					SetLayeredWindow( bmp );

				}
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
				if ( IsLayeredWindow )
					cp.ExStyle |= 0x80000;	//set layered window flag
				return cp;
			}
		}


		private void DialogNotifier_Paint( object sender, PaintEventArgs e ) {

			if ( IsLayeredWindow ) return;

			Graphics g = e.Graphics;
			g.Clear( BackColor );

			try {

				if ( DialogData.DrawsImage && DialogData.Image != null ) {

					g.DrawImage( DialogData.Image, new Rectangle( 0, 0, DialogData.Image.Width, DialogData.Image.Height ) );
				}

				DrawMessage( g );

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "通知システム: ダイアログボックスでの画像の描画に失敗しました。" );
			}
		}


		private void DrawMessage( Graphics g ) {
			if ( DialogData.DrawsMessage ) {

				TextRenderer.DrawText( g, DialogData.Message, Font, new Rectangle( Padding.Left, Padding.Top, ClientSize.Width - Padding.Horizontal, ClientSize.Height - Padding.Vertical ), ForeColor, TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak );
			}
		}

		private void DialogNotifier_MouseClick( object sender, MouseEventArgs e ) {

			var flag = DialogData.ClickFlag;

			if ( ( e.Button & System.Windows.Forms.MouseButtons.Left ) != 0 ) {
				if ( ( flag & NotifierDialogClickFlags.Left ) != 0 ||
				   ( ( flag & NotifierDialogClickFlags.LeftDouble ) != 0 && e.Clicks > 1 ) ) {
					Close();
				}
			}

			if ( ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 ) {
				if ( ( flag & NotifierDialogClickFlags.Right ) != 0 ||
				   ( ( flag & NotifierDialogClickFlags.RightDouble ) != 0 && e.Clicks > 1 ) ) {
					Close();
				}
			}

			if ( ( e.Button & System.Windows.Forms.MouseButtons.Middle ) != 0 ) {
				if ( ( flag & NotifierDialogClickFlags.Middle ) != 0 ||
				   ( ( flag & NotifierDialogClickFlags.MiddleDouble ) != 0 && e.Clicks > 1 ) ) {
					Close();
				}
			}

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

		void data_CloseAll( object sender, EventArgs e ) {
			Close();
		}


		// 以下 レイヤードウィンドウ用の呪文

		[DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		public static extern IntPtr GetDC( IntPtr hWnd );

		[DllImport( "gdi32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		public static extern IntPtr CreateCompatibleDC( IntPtr hdc );

		[DllImport( "gdi32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		public static extern IntPtr SelectObject( IntPtr hdc, IntPtr hgdiobj );

		[DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		public static extern int ReleaseDC( IntPtr hWnd, IntPtr hDC );

		[DllImport( "gdi32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		public static extern int DeleteObject( IntPtr hobject );

		[DllImport( "gdi32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		public static extern int DeleteDC( IntPtr hdc );

		public const byte AC_SRC_OVER = 0;
		public const byte AC_SRC_ALPHA = 1;
		public const int ULW_ALPHA = 2;

		[StructLayout( LayoutKind.Sequential, Pack = 1 )]
		public struct BLENDFUNCTION {
			public byte BlendOp;
			public byte BlendFlags;
			public byte SourceConstantAlpha;
			public byte AlphaFormat;
		}

		[DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		public static extern int UpdateLayeredWindow(
			IntPtr hwnd,
			IntPtr hdcDst,
			[System.Runtime.InteropServices.In()]
            ref Point pptDst,
			[System.Runtime.InteropServices.In()]
            ref Size psize,
			IntPtr hdcSrc,
			[System.Runtime.InteropServices.In()]
            ref Point pptSrc,
			int crKey,
			[System.Runtime.InteropServices.In()]
            ref BLENDFUNCTION pblend,
			int dwFlags );

		/// <summary>
		/// レイヤードウィンドウを作成します。
		/// </summary>
		/// <param name="src">元になる画像。</param>
		public void SetLayeredWindow( Bitmap src ) {
			// GetDeviceContext
			IntPtr screenDc = IntPtr.Zero;
			IntPtr memDc = IntPtr.Zero;
			IntPtr hBitmap = IntPtr.Zero;
			IntPtr hOldBitmap = IntPtr.Zero;
			try {
				screenDc = GetDC( IntPtr.Zero );
				memDc = CreateCompatibleDC( screenDc );
				hBitmap = src.GetHbitmap( Color.FromArgb( 0 ) );
				hOldBitmap = SelectObject( memDc, hBitmap );

				BLENDFUNCTION blend = new BLENDFUNCTION();
				blend.BlendOp = AC_SRC_OVER;
				blend.BlendFlags = 0;
				blend.SourceConstantAlpha = 255;
				blend.AlphaFormat = AC_SRC_ALPHA;

				//Size = new Size( src.Width, src.Height );
				Point pptDst = new Point( this.Left, this.Top );
				Size psize = new Size( this.Width, this.Height );
				Point pptSrc = new Point( 0, 0 );
				UpdateLayeredWindow( this.Handle, screenDc, ref pptDst, ref psize, memDc,
				  ref pptSrc, 0, ref blend, ULW_ALPHA );

			} finally {
				if ( screenDc != IntPtr.Zero ) {
					ReleaseDC( IntPtr.Zero, screenDc );
				}
				if ( hBitmap != IntPtr.Zero ) {
					SelectObject( memDc, hOldBitmap );
					DeleteObject( hBitmap );
				}
				if ( memDc != IntPtr.Zero ) {
					DeleteDC( memDc );
				}
			}
		}


	}
}
