using ElectronicObserver.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog {
	public partial class DialogHalloween : Form {

		private Bitmap canvas;
		private Bitmap[] fairies;
		private Graphics paintbrush;
		private int zoomscale;
		private Rectangle trickButton;
		private Rectangle treatButton;
		private Random rand;
		private Point cursorPosition;
		private RectangleF[] fairiesVector;
		private int state;
		private int tick;

		public DialogHalloween() {
			InitializeComponent();
		}

		private void DialogHalloween_Load( object sender, EventArgs e ) {

			canvas = new Bitmap( 320, 240, PixelFormat.Format32bppArgb );
			paintbrush = Graphics.FromImage( canvas );
			rand = new Random();
			fairies = new Bitmap[4];
			fairiesVector = new RectangleF[4];

			for ( int i = 0; i < 4; i++ ) {
				try {
					fairies[i] = new Bitmap( ResourceManager.GetStreamFromArchive( "Fairy/Fairy" + ( i + 1 ) + ".png" ) );
				} catch ( Exception ex ) {
					Utility.Logger.Add( 1, "[PumpkinHead] Failed to gather fairies. Boo... " + ex.Message );
					Close();
					return;
				}
			}

			zoomscale = 2;
			trickButton = new Rectangle( canvas.Width * 1 / 4 - 80 / 2, canvas.Height * 3 / 4 - 32 / 2, 80, 32 );
			treatButton = new Rectangle( canvas.Width * 3 / 4 - 80 / 2, canvas.Height * 3 / 4 - 32 / 2, 80, 32 );

			cursorPosition = new Point( -1, -1 );
			state = 1;
			tick = 0;

			ClientSize = new Size( canvas.Width * zoomscale, canvas.Height * zoomscale );
			Icon = ResourceManager.Instance.AppIcon;
			Updater.Start();

		}

		private void Updater_Tick( object sender, EventArgs e ) {

			paintbrush.ResetTransform();

			switch ( state ) {
				case 1:
					// initial phase

					DrawBackground();

					//
					{
						string mes = "T r i c k   o r   T r e a t !";
						paintbrush.DrawString( mes, Font, Brushes.White, (int)( canvas.Width / 2 - mes.Length * Font.Size / 4 ) + 1, canvas.Height * 1 / 8 + 1 );
						paintbrush.DrawString( mes, Font, Brushes.Red, (int)( canvas.Width / 2 - mes.Length * Font.Size / 4 ), canvas.Height * 1 / 8 );
					}

					paintbrush.DrawImage( fairies[3],
						canvas.Width / 2 - fairies[3].Width / 2 + ( cursorPosition.X >= canvas.Width / 2 ? fairies[3].Width : 0 ),
						canvas.Height * 4 / 8 - fairies[3].Height / 2 + (int)( Math.Sin( 1.0 / 4.0 * tick / GetFPS() * 2 * Math.PI ) * fairies[3].Height / 4 ),
						fairies[3].Width * ( cursorPosition.X >= canvas.Width / 2 ? -1 : 1 ),
						fairies[3].Height );

					DrawButton( "Trick!", trickButton, Brushes.Orange, Pens.White, trickButton.Contains( cursorPosition ) ? Brushes.Red : Brushes.Maroon );
					DrawButton( "Treat!", treatButton, Brushes.Orange, Pens.White, treatButton.Contains( cursorPosition ) ? Brushes.Red : Brushes.Maroon );

					break;

				case 2:
					// Tricked

					DrawBackground();

					//

					{
						string mes = "B o o o o o o o o o o o o ! !";
						paintbrush.DrawString( mes, Font, Brushes.White, (int)( canvas.Width / 2 - mes.Length * Font.Size / 4 ) + 1, canvas.Height * 1 / 8 + 1 );
						paintbrush.DrawString( mes, Font, Brushes.Red, (int)( canvas.Width / 2 - mes.Length * Font.Size / 4 ), canvas.Height * 1 / 8 );
					}

					{
						string mes = "* Open config and press OK to restore  ";
						paintbrush.DrawString( mes, Font, Brushes.White, (int)( canvas.Width - mes.Length * Font.Size / 2 ) + 1, canvas.Height * 15 / 16 + 1 );
						paintbrush.DrawString( mes, Font, Brushes.Brown, (int)( canvas.Width - mes.Length * Font.Size / 2 ), canvas.Height * 15 / 16 );
					}

					for ( int i = 0; i < fairiesVector.Length; i++ ) {
						fairiesVector[i].X += fairiesVector[i].Width;
						fairiesVector[i].Y += fairiesVector[i].Height;

						if ( ( fairiesVector[i].X < 0 && fairiesVector[i].Width < 0 ) ||
							 ( fairiesVector[i].X >= canvas.Width - fairies[i].Width && fairiesVector[i].Width > 0 ) )
							fairiesVector[i].Width *= -1;
						if ( ( fairiesVector[i].Y < 0 && fairiesVector[i].Height < 0 ) ||
							 ( fairiesVector[i].Y >= canvas.Height - fairies[i].Height && fairiesVector[i].Height > 0 ) )
							fairiesVector[i].Height *= -1;

						paintbrush.DrawImage( fairies[i], new Rectangle(
							( fairiesVector[i].Width <= 0 ? (int)fairiesVector[i].X : ( (int)fairiesVector[i].X + fairies[i].Width ) ) + (int)Math.Round( ( rand.NextDouble() * 2.0 - 1.0 ) * 4.0 ),
							(int)fairiesVector[i].Y + (int)Math.Round( ( rand.NextDouble() * 2.0 - 1.0 ) * 4.0 ),
							( fairiesVector[i].Width > 0 ? -1 : 1 ) * fairies[i].Width,
							fairies[i].Height ) );
					}

					break;


				case 3:
					// Treated

					DrawBackground();

					//
					{
						string mes = "T h a n k   y o u ! !";
						paintbrush.DrawString( mes, Font, Brushes.White, (int)( canvas.Width / 2 - mes.Length * Font.Size / 4 ) + 1, canvas.Height * 1 / 8 + 1 );
						paintbrush.DrawString( mes, Font, Brushes.Red, (int)( canvas.Width / 2 - mes.Length * Font.Size / 4 ), canvas.Height * 1 / 8 );
					}

					{
						string mes = "* Set comment \"jackolantern\"  ";
						paintbrush.DrawString( mes, Font, Brushes.White, (int)( canvas.Width - mes.Length * Font.Size / 2 ) + 1, canvas.Height * 15 / 16 + 1 );
						paintbrush.DrawString( mes, Font, Brushes.Brown, (int)( canvas.Width - mes.Length * Font.Size / 2 ), canvas.Height * 15 / 16 );
					}


					// green girl
					{
						int w = fairies[0].Width;
						int h = fairies[0].Height;
						Point org = new Point( 8 + w / 2, canvas.Height / 2 - h / 2 );
						switch ( tick * 2 / GetFPS() % 4 ) {
							case 0:
								paintbrush.DrawImage( fairies[0], new Point[] { 
									new Point( org.X, org.Y ),
									new Point( org.X + w, org.Y ),
									new Point( org.X, org.Y + h ),
								} );
								break;
							case 1:
								paintbrush.DrawImage( fairies[0], new Point[] { 
									new Point( org.X + w, org.Y ),
									new Point( org.X + w, org.Y + h ),
									new Point( org.X, org.Y ),
								} );
								break;
							case 2:
								paintbrush.DrawImage( fairies[0], new Point[] { 
									new Point( org.X + w, org.Y + h ),
									new Point( org.X, org.Y + h ),
									new Point( org.X + w, org.Y ),
								} );
								break;
							case 3:
								paintbrush.DrawImage( fairies[0], new Point[] { 
									new Point( org.X, org.Y + h ),
									new Point( org.X, org.Y ),
									new Point( org.X + w, org.Y + h ),
								} );
								break;
						}
					}


					//peach girl
					{
						int beattick = 8;
						int phase = (int)( tick / 0.4 / GetFPS() ) % beattick;
						bool isInverted = phase >= beattick / 2;
						paintbrush.ResetTransform();
						paintbrush.DrawImage( fairies[1], new Rectangle(
							64 + 8 + 32 + ( isInverted ? fairies[1].Width : 0 ),
							canvas.Height / 2 - fairies[1].Height / 2 + ( phase % 4 == 0 ? ( (int)( Math.Sin( tick % 4 / 2.0 * Math.PI ) * 8 ) ) : 0 ),
							fairies[1].Width * ( isInverted ? -1 : 1 ),
							fairies[1].Height ) );
					}

					//bird girl
					{
						int beattick = 1 * GetFPS();
						int phase = tick / beattick % 4;
						bool horizontalInverted = phase == 1 || phase == 2;
						bool verticalInverted = phase == 2 || phase == 3;
						paintbrush.DrawImage( fairies[2], new Rectangle(
							128 + 8 + 32 + ( horizontalInverted ? fairies[2].Width : 0 ) + (int)Math.Round( ( rand.NextDouble() * 2.0 - 1.0 ) * 8 ),
							canvas.Height / 2 - fairies[2].Height / 2 + ( verticalInverted ? fairies[2].Height : 0 ) + (int)Math.Round( ( rand.NextDouble() * 2.0 - 1.0 ) * 8 ),
							fairies[2].Width * ( horizontalInverted ? -1 : 1 ),
							fairies[2].Height * ( verticalInverted ? -1 : 1 ) ) );
					}

					//witch girl
					{
						double rad = 16;
						double angle = (double)tick / GetFPS() * Math.PI % ( 2 * Math.PI );
						paintbrush.DrawImage( fairies[3], new Rectangle(
							192 + 8 + 32 + (int)( Math.Cos( angle ) * rad ) + ( Math.Cos( angle ) >= 0 ? fairies[3].Width : 0 ),
							canvas.Height / 2 - fairies[3].Height / 2 + (int)( Math.Sin( angle ) * rad ),
							fairies[3].Width * ( Math.Cos( angle ) >= 0 ? -1 : 1 ),
							fairies[3].Height ) );
					}

					break;
			}

			paintbrush.ResetTransform();
			//paintbrush.DrawString( tick.ToString(), Font, Brushes.Red, 0, 0 );

			tick++;
			Refresh();
		}

		private void DialogHalloween_MouseClick( object sender, MouseEventArgs e ) {
			cursorPosition = new Point( e.X / zoomscale, e.Y / zoomscale );

			switch ( state ) {
				case 1:

					if ( trickButton.Contains( cursorPosition ) ) {

						state = 2;


						// initialize movement
						for ( int i = 0; i < 4; i++ ) {
							fairiesVector[i] = new RectangleF( rand.Next( canvas.Width - fairies[i].Width ), rand.Next( canvas.Height - fairies[i].Height ),
								(float)( ( rand.NextDouble() * 2.0 - 1.0 ) * 8.0 ), (float)( ( rand.NextDouble() * 2.0 - 1.0 ) * 8.0 ) );
						}


						// system font override
						var c = Utility.Configuration.Config;
						Font preservedfont_main = c.UI.MainFont;
						Font preservedfont_sub = c.UI.SubFont;

						string[] candidates = {
							"HGP創英角ﾎﾟｯﾌﾟ体",
							"ふい字Ｐ",
							"Segoe Script",
							"MS UI Gothic",
						  };
						string fontname = null;

						var fonts = new System.Drawing.Text.InstalledFontCollection();
						for ( int i = 0; i < candidates.Length; i++ ) {
							if ( fonts.Families.Count( f => f.Name == candidates[i] ) > 0 ) {
								fontname = candidates[i];
								break;
							}

						}
						if ( fontname == null )
							break;

						c.UI.MainFont = new Font( fontname, 12, FontStyle.Regular, GraphicsUnit.Pixel );
						c.UI.SubFont = new Font( fontname, 10, FontStyle.Regular, GraphicsUnit.Pixel );

						Utility.Configuration.Instance.OnConfigurationChanged();

						c.UI.MainFont = preservedfont_main;
						c.UI.SubFont = preservedfont_sub;

					} else if ( treatButton.Contains( cursorPosition ) ) {
						state = 3;
					}

					break;

			}
		}

		private void DialogHalloween_MouseMove( object sender, MouseEventArgs e ) {
			cursorPosition = new Point( e.X / zoomscale, e.Y / zoomscale );
		}

		private void DialogHalloween_Paint( object sender, PaintEventArgs e ) {

			e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

			e.Graphics.DrawImage( canvas, 0, 0, canvas.Width * zoomscale, canvas.Height * zoomscale );

		}



		private int GetFPS() {
			return 1000 / Updater.Interval;
		}

		private void DrawBackground() {

			const int squaresize = 32;
			const int framerate = 4;

			BitmapData bitmapdata = canvas.LockBits( new Rectangle( 0, 0, canvas.Width, canvas.Height ), ImageLockMode.WriteOnly, canvas.PixelFormat );
			byte[] buffer = new byte[canvas.Width * canvas.Height * 4];

			int shift = ( tick * framerate / GetFPS() ) % squaresize;

			for ( int i = 0; i < buffer.Length; i += 4 ) {
				int colflag = ( ( ( i / 4 % canvas.Width + shift ) / squaresize ) & 1 ) ^ ( ( ( i / 4 / canvas.Width + shift ) / squaresize ) & 1 );

				if ( colflag == 0 ) {
					//orange
					buffer[i + 3] = 0xFF;
					buffer[i + 2] = 0xFF;
					buffer[i + 1] = 0xCC;
					buffer[i + 0] = 0x88;

				} else {
					//black
					buffer[i + 3] = 0xFF;
					buffer[i + 2] = 0x88;
					buffer[i + 1] = 0x88;
					buffer[i + 0] = 0x88;

				}
			}

			Marshal.Copy( buffer, 0, bitmapdata.Scan0, buffer.Length );
			canvas.UnlockBits( bitmapdata );
		}

		private void DrawButton( string message, Rectangle rect, Brush foregroundBrush, Pen backgroundPen, Brush backgroundBrush ) {
			paintbrush.FillRectangle( backgroundBrush, rect );
			paintbrush.DrawRectangle( backgroundPen, rect );
			paintbrush.DrawRectangle( backgroundPen, new Rectangle( rect.X + 2, rect.Y + 2, rect.Width - 4, rect.Height - 4 ) );
			paintbrush.DrawString( message, Font, Brushes.White, rect.X + rect.Width / 2 - (int)( message.Length * Font.Size / 2 ) / 2 + 1, rect.Y + rect.Height / 2 - (int)( Font.Size / 2 ) + 1 );
			paintbrush.DrawString( message, Font, foregroundBrush, rect.X + rect.Width / 2 - (int)( message.Length * Font.Size / 2 ) / 2, rect.Y + rect.Height / 2 - (int)( Font.Size / 2 ) );

		}


		private void DialogHalloween_FormClosed( object sender, FormClosedEventArgs e ) {

			Updater.Stop();

			paintbrush.Dispose();
			canvas.Dispose();
			for ( int i = 0; i < fairies.Length; i++ )
				if ( fairies[i] != null )
					fairies[i].Dispose();

		}




	}
}
