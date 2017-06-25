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



		/// <summary>
		/// TableLayoutPanel における、現在の設定を反映した汎用的な RowStyle を取得します。
		/// </summary>
		public static RowStyle GetDefaultRowStyle() {
			return Utility.Configuration.Config.UI.IsLayoutFixed ?
				new RowStyle( SizeType.Absolute, 21 ) :
				new RowStyle( SizeType.AutoSize );
		}

		/// <summary>
		/// TableLayoutPanel において、実際に利用されている行数を取得します。
		/// デフォルトの RowCount 及び RowStyles.Count は実際と異なる値を返すことがあるためです。
		/// </summary>
		public static int GetActualRowCount( TableLayoutPanel panel ) {
			int count = panel.RowCount;
			foreach ( System.Windows.Forms.Control c in panel.Controls ) {
				count = Math.Max( panel.GetRow( c ) + 1, count );
			}
			return count;
		}

		/// <summary>
		/// 指定した行に対して、RowStyle を適用します。
		/// </summary>
		public static void SetTableRowStyle( TableLayoutPanel panel, int row, RowStyle style ) {
			if ( panel.RowStyles.Count > row )
				panel.RowStyles[row] = new RowStyle( style.SizeType, style.Height );
			else
				while ( panel.RowStyles.Count <= row )
					panel.RowStyles.Add( new RowStyle( style.SizeType, style.Height ) );
		}

		/// <summary>
		/// 全ての行に対して、RowStyle を適用します。
		/// </summary>
		public static void SetTableRowStyles( TableLayoutPanel panel, RowStyle style ) {
			int rowCount = GetActualRowCount( panel );
			for ( int i = 0; i < rowCount; i++ ) {
				if ( panel.RowStyles.Count > i )
					panel.RowStyles[i] = new RowStyle( style.SizeType, style.Height );
				else
					panel.RowStyles.Add( new RowStyle( style.SizeType, style.Height ) );
			}
		}


		/// <summary>
		/// TableLayoutPanel における、現在の設定を反映した汎用的な ColumnStyle を取得します。
		/// </summary>
		public static ColumnStyle GetDefaultColumnStyle() {
			return new ColumnStyle( SizeType.AutoSize );
		}

		/// <summary>
		/// TableLayoutPanel において、実際に利用されている列数を取得します。
		/// デフォルトの ColumnCount 及び ColumnStyles.Count は実際と異なる値を返すことがあるためです。
		/// </summary>
		public static int GetActualColumnCount( TableLayoutPanel panel ) {
			int count = panel.ColumnCount;
			foreach ( System.Windows.Forms.Control c in panel.Controls ) {
				count = Math.Max( panel.GetColumn( c ) + 1, count );
			}
			return count;
		}

		/// <summary>
		/// 指定した列に対して、ColumnStyle を適用します。
		/// </summary>
		public static void SetTableColumnStyle( TableLayoutPanel panel, int column, ColumnStyle style ) {
			if ( panel.ColumnStyles.Count > column )
				panel.ColumnStyles[column] = new ColumnStyle( style.SizeType, style.Width );
			else
				while ( panel.ColumnStyles.Count <= column )
					panel.ColumnStyles.Add( new ColumnStyle( style.SizeType, style.Width ) );
		}

		/// <summary>
		/// 全ての列に対して、ColumnStyle を適用します。
		/// </summary>
		public static void SetTableColumnStyles( TableLayoutPanel panel, ColumnStyle style ) {
			int columnCount = GetActualColumnCount( panel );
			for ( int i = 0; i < columnCount; i++ ) {
				if ( panel.ColumnStyles.Count > i )
					panel.ColumnStyles[i] = new ColumnStyle( style.SizeType, style.Width );
				else
					panel.ColumnStyles.Add( new ColumnStyle( style.SizeType, style.Width ) );
			}
		}


	}

}
