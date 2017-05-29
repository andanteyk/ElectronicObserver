using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Support {

	/// <summary>
	/// コントロールの操作を補助する機能を提供します。
	/// </summary>
	public static class ControlHelper {

		/// <summary>
		/// コントロールの DoubleBuffered を設定します。
		/// </summary>
		/// <param name="control">対象となるコントロール。</param>
		/// <param name="flag">設定するフラグ。既定では true です。</param>
		public static void SetDoubleBuffered( System.Windows.Forms.Control control, bool flag = true ) {

			System.Reflection.PropertyInfo prop = control.GetType().GetProperty( "DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic );
			prop.SetValue( control, flag, null );

		}


		/// <summary>
		/// DataGridView の指定行を 1 つ上に移動します。
		/// </summary>
		/// <param name="dgv">対象となる DataGridView 。</param>
		/// <param name="rowIndex">行のインデックス。</param>
		/// <returns>成功すれば true 。</returns>
		public static bool RowMoveUp( DataGridView dgv, int rowIndex ) {

			if ( rowIndex <= 0 ) return false;

			var row = dgv.Rows[rowIndex - 1];
			dgv.Rows.Remove( row );
			dgv.Rows.Insert( rowIndex, row );

			return true;
		}

		/// <summary>
		/// DataGridView の指定行を 1 つ下に移動します。
		/// </summary>
		/// <param name="dgv">対象となる DataGridView 。</param>
		/// <param name="rowIndex">行のインデックス。</param>
		/// <returns>成功すれば true 。</returns>
		public static bool RowMoveDown( DataGridView dgv, int rowIndex ) {

			if ( rowIndex >= dgv.Rows.Count - 1 ) return false;

			var row = dgv.Rows[rowIndex + 1];
			dgv.Rows.Remove( row );
			dgv.Rows.Insert( rowIndex, row );

			return true;
		}


		/// <summary>
		/// 文字列のサイズを計測します。
		/// ピクセル単位で正確である保証がありますが、処理は非常に重いため呼び出しには注意してください。
		/// </summary>
		/// <param name="baseGraphics">描画先の Graphics 。null を指定した場合はデフォルトの Graphics が利用されます。</param>
		/// <param name="text">測定する文字列。</param>
		/// <param name="font">測定する文字列のフォント。</param>
		/// <param name="format">書式設定。</param>
		/// <returns>文字列が描画された範囲。</returns>
		public static Rectangle MeasureStringStrict( Graphics baseGraphics, string text, Font font, StringFormat format ) {

			bool isBaseGraphicsNull = baseGraphics == null;
			Bitmap tempImg = null;
			Rectangle ret;
			int left = -1, top = -1, right = -1, bottom = -1;

			if ( isBaseGraphicsNull ) {
				tempImg = new Bitmap( 1, 1 );
				baseGraphics = Graphics.FromImage( tempImg );
			}

			var baseRectangle = baseGraphics.MeasureString( text, font, new SizeF( int.MaxValue, int.MaxValue ), format );

			using ( var img = new Bitmap( (int)Math.Ceiling( baseRectangle.Width ), (int)Math.Ceiling( baseRectangle.Height ), PixelFormat.Format24bppRgb ) ) {
				using ( var g = Graphics.FromImage( img ) ) {

					g.TextRenderingHint = baseGraphics.TextRenderingHint;
					g.TextContrast = baseGraphics.TextContrast;
					g.PixelOffsetMode = baseGraphics.PixelOffsetMode;

					g.Clear( Color.White );

					g.DrawString( text, font, Brushes.Black, new Point( 0, 0 ), format );


					BitmapData bmpdata = img.LockBits( new Rectangle( 0, 0, img.Width, img.Height ), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb );
					byte[] canvas = new byte[bmpdata.Stride * bmpdata.Height];
					Marshal.Copy( bmpdata.Scan0, canvas, 0, canvas.Length );

					Func<int, int, bool> pred = ( xp, yp ) =>
						canvas[yp * bmpdata.Stride + xp * 3 + 2] != 0xff ||
						canvas[yp * bmpdata.Stride + xp * 3 + 1] != 0xff ||
						canvas[yp * bmpdata.Stride + xp * 3 + 0] != 0xff;


					// left
					for ( int x = 0; x < img.Width; x++ ) {
						for ( int y = 0; y < img.Height; y++ ) {
							if ( pred( x, y ) ) {
								left = x;
								break;
							}
							if ( left != -1 )
								break;
						}
					}

					// top
					for ( int y = 0; y < img.Height; y++ ) {
						for ( int x = 0; x < img.Width; x++ ) {
							if ( pred( x, y ) ) {
								top = y;
								break;
							}
							if ( top != -1 )
								break;
						}
					}

					// right
					for ( int x = img.Width - 1; x >= 0; x-- ) {
						for ( int y = 0; y < img.Height; y++ ) {
							if ( pred( x, y ) ) {
								right = x;
								break;
							}
							if ( right != -1 )
								break;
						}
					}

					// bottom
					for ( int y = img.Height - 1; y >= 0; y-- ) {
						for ( int x = 0; x < img.Width; x++ ) {
							if ( pred( x, y ) ) {
								bottom = y;
								break;
							}
							if ( bottom != -1 )
								break;
						}
					}


					if ( left != -1 &&
						top != -1 &&
						bottom != -1 &&
						right != -1 ) {
						ret = new Rectangle( left, top, right - left + 1, bottom - top + 1 );
					} else {
						ret = Rectangle.Empty;
					}

					img.UnlockBits( bmpdata );

					/*// debug
					using ( var p = new Pen( Color.Orange, 1 ) ) {
						p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
						g.DrawRectangle( p, ret );
						if ( ret.Width <= 0 || ret.Height <= 0 )
							g.DrawEllipse( p, new Rectangle( 0, 0, 3, 3 ) );
					}
					img.Save( "temp_" + Utility.Storage.SerializableFont.FontToString( font, true ) +  ".png", ImageFormat.Png );
					//*/
				}
			}

			if ( isBaseGraphicsNull ) {
				baseGraphics.Dispose();
				tempImg.Dispose();
			}

			return ret;
		}
	}

}
