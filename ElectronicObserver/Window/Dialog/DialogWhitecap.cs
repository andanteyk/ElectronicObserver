using ElectronicObserver.Utility.Mathematics;
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

		private int birthRule;
		private int aliveRule;

		private int zoomrate;
		private int colortheme;
		private Bitmap imagebuf;

		private Random rand;

		private int clock;


		public DialogWhitecap() {
			InitializeComponent();

			birthRule = Utility.Configuration.Config.Whitecap.BirthRule;
			aliveRule = Utility.Configuration.Config.Whitecap.AliveRule;

			zoomrate = Utility.Configuration.Config.Whitecap.ZoomRate;

			colortheme = Utility.Configuration.Config.Whitecap.ColorTheme;
			imagebuf = null;

			rand = new Random();

			SetSize( Utility.Configuration.Config.Whitecap.BoardWidth, Utility.Configuration.Config.Whitecap.BoardHeight );
			ShowInTaskbar = Utility.Configuration.Config.Whitecap.ShowInTaskbar;
			TopMost = Utility.Configuration.Config.Whitecap.TopMost;
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
						SetCell( 1 - currentDim, x, y, ( ( 1 << alive ) & aliveRule ) != 0 ? 1 : 0 );

					} else {

						SetCell( 1 - currentDim, x, y, ( ( 1 << alive ) & birthRule ) != 0 ? 1 : 0 );

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
							/*
							col = value != 0 ?
								BlendColor( FromRgb( 0xFFFFFF ), FromRgb( 0xFFDDBB ), (double)y / boardSize.Height ) :
								BlendColor( FromRgb( 0x00FFFF ), FromRgb( 0xFFDDBB ), (double)y / boardSize.Height );
							*/
							col = BlendColor( GetCell( currentDim, x, y + (int)( ( Math.Sin( clock / 100.0 * 2.0 * Math.PI ) + 1 ) * boardSize.Height / 8.0 ) ) != 0 ?
								BlendColor( FromRgb( 0xFFFFFF ), FromRgb( 0xFFDDBB ), Math.Max( Math.Min( ( y + ( ( Math.Sin( clock / 100.0 * 2.0 * Math.PI ) ) * boardSize.Height / 8.0 ) ) / boardSize.Height, 1.0 ), 0.0 ) ) :
								BlendColor( FromRgb( 0x00FFFF ), FromRgb( 0xFFDDBB ), Math.Min( ( y + ( ( Math.Sin( clock / 100.0 * 2.0 * Math.PI ) + 1 ) * boardSize.Height / 8.0 ) ) / boardSize.Height, 1.0 ) ),
								prev, 0.8 );
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

						case 10:
							col = GetCell( currentDim, x, y + clock ) != 0 ?
								BlendColor( FromRgb( 0xFFFFFF ), FromRgb( 0x00FFFF ), (double)y / boardSize.Height ) :
								BlendColor( prev, BlendColor( FromRgb( 0x0044FF ), FromRgb( 0x000000 ), (double)y / boardSize.Height ), 0.2 );
							break;

						case 11:
							col = value != 0 ? FromRgb( 0x00FF00 ) : FromRgb( 0x111111 );
							break;

						case 12:
							col = value != 0 ?
								FromRgb( 0x0044FF ) :
								BlendColor( FromRgb( 0xFFFFFF ), prev, 0.9 );
							break;

						case 13:
							col = value != 0 ?
								FromRgb( 0xFF0000 ) :
								AddColor( prev, FromRgb( 0xFF4422 ), 0.1 );
							break;

						case 14:
							col = GetCell( currentDim, x, y + clock ) != 0 ?
								BlendColor( FromRgb( 0xFFFFFF ), FromRgb( 0xFFFFCC ), (double)y / boardSize.Height ) :
								BlendColor( prev, BlendColor( FromRgb( 0x88FFFF ), FromRgb( 0x0000FF ), (double)y / boardSize.Height ), 0.05 );
							break;

						case 15:
							col = FromHsv( x * x + 2 * x * y + y * y + 98 * x + 168 * y, value != 0 ? 1.0 : 0.2, value != 0 ? 1.0 : 1.0 );
							break;

						case 16:
							col = value != 0 ? FromRgb( 0x000000 ) : FromRgb( 0xFFFFFF );
							break;

						case 17:
							col = BlendColor( prev, GetCell( currentDim, x + clock / 4, y ) != 0 ?
								FromRgb( 0xFFFFFF ) : BlendColor( FromRgb( 0x0088FF ), FromRgb( 0x88FFFF ), (double)y / boardSize.Height ),
								0.08 );
							break;

						case 18:
							col = AddColor( value != 0 ? FromRgb( 0xFF0000 ) : FromRgb( 0x000000 ),
								AddColor( GetCell( currentDim, x, boardSize.Height - 1 - y ) != 0 ? FromRgb( 0x00FF00 ) : FromRgb( 0x000000 ),
								GetCell( currentDim, boardSize.Width - 1 - x, y ) != 0 ? FromRgb( 0x0000FF ) : FromRgb( 0x000000 ) ) );
							break;

						case 19:
							//*/
							if ( value != 0 ||
								GetCell( currentDim, x + 1, y ) != 0 ||
								GetCell( currentDim, x, y + 1 ) != 0 ||
								GetCell( currentDim, x + 1, y + 1 ) != 0 )
								col = FromRgb( 0xFFFFFF );
							else if (
								GetCell( currentDim, x - 1, y ) != 0 ||
								GetCell( currentDim, x, y - 1 ) != 0 ||
								GetCell( currentDim, x - 1, y - 1 ) != 0 )
								col = FromRgb( 0x888888 );
							else
								col = FromRgb( 0x000000 );
							/*/
							if ( value != 0 )
								col = FromRgb( 0xFFFFFF );
							else if ( GetCell( currentDim, x - 1, y - 1 ) != 0 )
								col = FromRgb( 0x888888 );
							else
								col = FromRgb( 0x000000 );			
							//*/
							break;

						case 20:
							if ( value != 0 ) {
								const int incr = 1;
								col = Color.FromArgb( Math.Min( prev.R + incr, 255 ), Math.Min( prev.G + incr, 255 ), Math.Min( prev.B + incr, 255 ) );
							} else {
								const int decr = 0;
								col = Color.FromArgb( Math.Max( prev.R - decr, 0 ), Math.Max( prev.G - decr, 0 ), Math.Max( prev.B - decr, 0 ) );
							} break;

						case 21:
							if ( value != 0 ) {
								col = BlendColor( prev, FromRgb( 0x000000 ), 0.2 );
							} else {
								col = BlendColor( prev, BlendColor( FromRgb( 0x111188 ), FromRgb( 0x111111 ), (double)y / boardSize.Height ), 0.2 );
							}

							if ( value != 0 ) {
								const int blocksize = 32;
								const int frequency = 16;

								int ux = (int)( x / blocksize ) * blocksize;
								int uy = (int)( y / blocksize ) * blocksize;

								int seedx = clock / frequency * 16829 + ux / blocksize * 81953 + uy / blocksize * 40123;
								int seedy = clock / frequency * 81041 + ux / blocksize * 11471 + uy / blocksize * 51419;
								int seedz = clock / frequency * 39503 + ux / blocksize * 46133 + uy / blocksize * 15241;


								int rx = ( seedx >> 4 ) % blocksize;
								int ry = ( seedy >> 4 ) % blocksize;
								int rz = ( seedz >> 4 ) % 256;

								if ( ux + rx == x && uy + ry == y ) {
									Color eye;
									if ( rz < 160 )
										eye = FromRgb( 0x00FFFF );
									else if ( rz < 224 )
										eye = FromRgb( 0xFF0000 );
									else
										eye = FromRgb( 0xFFCC00 );


									col = BlendColor( col, eye, (double)y / boardSize.Height );
								}

							}

							break;

						case 22:
							if ( value == 0 )
								col = FromRgb( 0x000000 );
							else {
								if ( GetCell( 1 - currentDim, x, y ) != 0 ) {
									col = prev;
								} else {
									col = FromHsv( (int)( rand.NextDouble() * 12 ) * 30, 0.75, 1 );
								}
							}
							break;

						case 23:
							if ( value != 0 )
								col = FromHsv( 0, 0, rand.NextDouble() * 0.5 + 0.5 );
							else
								col = FromHsv( 0, 0, rand.NextDouble() * 0.5 );
							break;

						case 24: {
								int prevcell = GetCell( 1 - currentDim, x, y );
								if ( value != 0 ) {
									if ( prevcell != 0 )
										col = FromRgb( 0xFFFFFF );
									else
										col = FromRgb( 0x00FF00 );
								} else {
									if ( prevcell != 0 )
										col = FromRgb( 0xFF0000 );
									else
										col = FromRgb( 0x000000 );
								}
							} break;

						case 25: {
								int prevcell = GetCell( 1 - currentDim, x, y );
								if ( value != 0 ) {
									if ( prevcell != 0 )
										col = FromRgb( 0x222222 );
									else
										col = FromRgb( 0x00FF00 );
								} else {
									if ( prevcell != 0 )
										col = FromRgb( 0xFF0000 );
									else
										col = FromRgb( 0x000000 );
								}
							} break;

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

			} else if ( e.Button == System.Windows.Forms.MouseButtons.Middle ) {

				UpdateTimer.Stop();

				try {
					imagebuf.Save( string.Format( "SS@{0}.png", DateTimeHelper.GetTimeStamp() ), ImageFormat.Png );

				} catch ( Exception ) {
					System.Media.SystemSounds.Exclamation.Play();

				} finally {
					UpdateTimer.Start();
				}
			}
		}

		private void DialogWhitecap_DoubleClick( object sender, EventArgs e ) {

			UpdateTimer.Stop();
			colortheme = rand.Next( 64 );
			//colortheme = 24;
			Start();
		}

	}
}
