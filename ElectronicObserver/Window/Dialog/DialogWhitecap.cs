using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog {
	public partial class DialogWhitecap : Form {

		private int[,,] board;
		private Size boardSize;
		private int currentDim;

		private int zoomrate;
		private int colortheme;
		private Bitmap imagebuf;

		private Random rand;

		private int clock;


		public DialogWhitecap() {
			InitializeComponent();

			zoomrate = Utility.Configuration.Config.Whitecap.ZoomRate;

			colortheme = Utility.Configuration.Config.Whitecap.ColorTheme;
			imagebuf = null;

			rand = new Random();

			SetSize( Utility.Configuration.Config.Whitecap.BoardWidth, Utility.Configuration.Config.Whitecap.BoardHeight );
			ShowInTaskbar = Utility.Configuration.Config.Whitecap.ShowInTaskbar;
			TopMost = Utility.Configuration.Config.Whitecap.ShowInTaskbar;
		}

		private void DialogWhitecap_Load( object sender, EventArgs e ) {

			UpdateTimer.Interval = Utility.Configuration.Config.Whitecap.UpdateInterval;

			Start();
		}

		private void Start() {

			InitBoard();

			ClientSize = new Size( boardSize.Width * zoomrate, boardSize.Height * zoomrate );
			if ( imagebuf != null ) {
				imagebuf.Dispose();
			}
			imagebuf = new Bitmap( boardSize.Width, boardSize.Height, PixelFormat.Format24bppRgb );

			clock = 0;
			UpdateTimer.Start();
		}

		private void SetSize( int width, int height ) {
			boardSize = new Size( width, height );
			board = new int[2, height, width];
		}

		private void InitBoard( bool isRand = true ) {

			for ( int dim = 0; dim < 2; dim++ ) {
				for ( int y = 0; y < boardSize.Height; y++ ) {
					for ( int x = 0; x < boardSize.Width; x++ ) {
						board[dim, y, x] = isRand ? rand.Next( 2 ) : 0;
					}
				}
			}

			currentDim = 0;
		}

		private void SetCell( int dim, int x, int y, int value ) {

			x = x % boardSize.Width;
			if ( x < 0 ) x += boardSize.Width;

			y = y % boardSize.Height;
			if ( y < 0 ) y += boardSize.Height;

			board[dim, y, x] = value;
		}

		private int GetCell( int dim, int x, int y ) {

			x = x % boardSize.Width;
			if ( x < 0 ) x += boardSize.Width;

			y = y % boardSize.Height;
			if ( y < 0 ) y += boardSize.Height;

			return board[dim, y, x];
		}



		private void UpdateTimer_Tick( object sender, EventArgs e ) {

			for ( int y = 0; y < boardSize.Height; y++ ) {
				for ( int x = 0; x < boardSize.Width; x++ ) {

					int alive = 0;

					for ( int dy = -1; dy <= 1; dy++ ) {
						for ( int dx = -1; dx <= 1; dx++ ) {
							if ( ( dx != 0 || dy != 0 ) && GetCell( currentDim, x + dx, y + dy ) != 0 )
								alive++;
						}
					}

					if ( GetCell( currentDim, x, y ) != 0 ) {
						SetCell( 1 - currentDim, x, y, ( alive == 2 || alive == 3 ) ? 1 : 0 );

					} else {

						SetCell( 1 - currentDim, x, y, alive == 3 ? 1 : 0 );

					}
				}
			}

			currentDim = 1 - currentDim;
			clock++;

			Refresh();
		}


		private void DialogWhitecap_Paint( object sender, PaintEventArgs e ) {

			e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

			e.Graphics.Clear( Color.Black );

			BitmapData bmpdata = imagebuf.LockBits( new Rectangle( 0, 0, imagebuf.Width, imagebuf.Height ), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb );
			byte[] canvas = new byte[imagebuf.Width * imagebuf.Height * 3];
			Marshal.Copy( bmpdata.Scan0, canvas, 0, canvas.Length );
			
			for ( int y = 0; y < boardSize.Height; y++ ) {
				for ( int x = 0; x < boardSize.Width; x++ ) {

					Color col;
					Color prev = Color.FromArgb(
						canvas[( ( y * boardSize.Width + x ) * 3 + 2 )],
						canvas[( ( y * boardSize.Width + x ) * 3 + 1 )],
						canvas[( ( y * boardSize.Width + x ) * 3 + 0 )] );
					int value = GetCell( currentDim, x, y );


					switch ( colortheme ) {

						case 1:
							col = value != 0 ?
								BlendColor( FromRgb( 0x000000 ), FromRgb( 0xFF0000 ), (double)y / boardSize.Height ) :
								BlendColor( FromRgb( 0xFF0000 ), FromRgb( 0xFFFF00 ), (double)y / boardSize.Height );
							break;

						case 2:
							col = value != 0 ?
								BlendColor( FromRgb( 0xFFFFFF ), FromRgb( 0x00FFFF ), (double)y / boardSize.Height ) :
								BlendColor( FromRgb( 0x0044FF ), FromRgb( 0x000000 ), (double)y / boardSize.Height );
							break;

						case 3:
							col = value != 0 ?
								BlendColor( FromRgb( 0xFFFFFF ), FromRgb( 0xFFDDBB ), (double)y / boardSize.Height ) :
								BlendColor( FromRgb( 0x00FFFF ), FromRgb( 0xFFDDBB ), (double)y / boardSize.Height );
							break;

						case 4:
							col = value != 0 ?
								BlendColor( FromRgb( 0xFFFFFF ), FromRgb( 0xCCCCFF ), (double)y / boardSize.Height ) :
								BlendColor( FromRgb( 0x000000 ), FromRgb( 0x000088 ), (double)y / boardSize.Height );
							break;

						case 5:
							col = value != 0 ?
								BlendColor( FromRgb( 0xDDDDDD ), FromRgb( 0xFFFFFF ), (double)( x + y ) / ( boardSize.Width + boardSize.Height ) ) :
								BlendColor( FromRgb( 0x778888 ), FromRgb( 0x99AAAA ), (double)( x + y ) / ( boardSize.Width + boardSize.Height ) );
							break;

						case 6:
							col = value != 0 ?
								BlendColor( FromRgb( 0xFF66FF ), FromRgb( 0xFFAAFF ), Math.Pow( x + y, 2 ) / Math.Pow( boardSize.Width + boardSize.Height, 2 ) ) :
								BlendColor( FromRgb( 0xFFCCCC ), FromRgb( 0xFFFFFF ), Math.Pow( x + y, 2 ) / Math.Pow( boardSize.Width + boardSize.Height, 2 ) );
							break;

						case 7:
							col = value != 0 ?
								BlendColor( FromRgb( 0x008800 ), FromRgb( 0x44FF44 ), Math.Pow( x + y, 2 ) / Math.Pow( boardSize.Width + boardSize.Height, 2 ) ) :
								BlendColor( FromRgb( 0x88FF88 ), FromRgb( 0xCCFF88 ), Math.Pow( x + y, 2 ) / Math.Pow( boardSize.Width + boardSize.Height, 2 ) );
							break;

						case 8:
							col = value != 0 ?
								FromRgb( 0xFFFFFF ) :
								BlendColor( FromRgb( 0x000000 ), prev, 0.5 );
									
							break;

						case 9:
							col = value != 0 ?
								FromHsv( x + y + clock * 3, 1.0, 1.0 ) :
								FromRgb( 0x000000 );
							break;

						default:
							col = value != 0 ? FromRgb( 0xFFFFFF ) : FromRgb( 0x000000 );
							break;

					}


					canvas[( ( y * boardSize.Width + x ) * 3 + 0 )] = col.B;
					canvas[( ( y * boardSize.Width + x ) * 3 + 1 )] = col.G;
					canvas[( ( y * boardSize.Width + x ) * 3 + 2 )] = col.R;
					
				}
			}

			Marshal.Copy( canvas, 0, bmpdata.Scan0, canvas.Length );
			imagebuf.UnlockBits( bmpdata );

			e.Graphics.DrawImage( imagebuf, 0, 0, imagebuf.Width * zoomrate, imagebuf.Height * zoomrate );

		}


		private Color FromRgb( int rgb ) {
			return Color.FromArgb( ( rgb >> 16 ) & 0xFF, ( rgb >> 8 ) & 0xFF, rgb & 0xFF );
		}


		/// <summary>
		/// generate Color from hsv
		/// </summary>
		/// <param name="hue">0-360</param>
		/// <param name="saturation">0-1</param>
		/// <param name="brightness">0-1</param>
		/// <returns></returns>
		private Color FromHsv( double hue, double saturation, double brightness ) {
			hue = hue % 360.0;
			if ( hue < 0.0 ) hue += 360.0;
			if ( saturation < 0.0 ) saturation = 0.0;

			double r = 255 * brightness, g = 255 * brightness, b = 255 * brightness;
			int mode = (int)( hue / 60.0 );
			double weight = ( ( hue / 60.0 ) - (int)( hue / 60.0 ) );

			switch ( mode ) {
				default:
				case 0:
					g *= 1.0 - saturation * ( 1.0 - weight );
					b *= 1.0 - saturation;
					break;
				case 1:
					r *= 1.0 - saturation * weight;
					b *= 1.0 - saturation;
					break;
				case 2:
					b *= 1.0 - saturation * ( 1.0 - weight );
					r *= 1.0 - saturation;
					break;
				case 3:
					g *= 1.0 - saturation * weight;
					r *= 1.0 - saturation;
					break;
				case 4:
					r *= 1.0 - saturation * ( 1.0 - weight );
					g *= 1.0 - saturation;
					break;
				case 5:
					b *= 1.0 - saturation * weight;
					g *= 1.0 - saturation;
					break;
			}

			return Color.FromArgb( (int)r, (int)g, (int)b );
		}

		private Color AddColor( Color a, Color b, double weight = 1.0 ) {
			return Color.FromArgb(
				Math.Min( a.R + (int)( b.R * weight ), 255 ),
				Math.Min( a.G + (int)( b.G * weight ), 255 ),
				Math.Min( a.B + (int)( b.B * weight ), 255 ) );
		}

		private Color BlendColor( Color a, Color b, double weight = 0.5 ) {
			return Color.FromArgb(
				Math.Min( (int)( a.R * ( 1 - weight ) + b.R * weight ), 255 ),
				Math.Min( (int)( a.G * ( 1 - weight ) + b.G * weight ), 255 ),
				Math.Min( (int)( a.B * ( 1 - weight ) + b.B * weight ), 255 ) );
		}

		private void DialogWhitecap_MouseClick( object sender, MouseEventArgs e ) {

			if ( e.Button == System.Windows.Forms.MouseButtons.Right ) {
				UpdateTimer.Stop();
				InitBoard();
				UpdateTimer.Start();
			}
		}

		private void DialogWhitecap_DoubleClick( object sender, EventArgs e ) {

			UpdateTimer.Stop();
			colortheme = rand.Next( 64 );
			Start();
		}

	}
}
