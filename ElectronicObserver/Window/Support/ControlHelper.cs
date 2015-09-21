using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

		public static int GetDpiHeight( this Form form, int height ) {

			if ( form == null ) {
				return height;
			}

			Graphics g = form.CreateGraphics();
			float dy;
			try {

				dy = g.DpiY;

			} catch {
				dy = 96f;
			} finally {

				g.Dispose();

			}
			return (int)Math.Round( dy / 96 * height );

		}

		public static void SuspendLayoutForDpiScale( this ContainerControl container ) {

			if ( Utility.Configuration.Config.UI.AutoScaleDpi ) {
				container.SuspendLayout();
			}

		}

		public static void ResumeLayoutForDpiScale( this ContainerControl container ) {

			if ( Utility.Configuration.Config.UI.AutoScaleDpi ) {
				container.AutoScaleMode = AutoScaleMode.Dpi;
				container.AutoScaleDimensions = new SizeF( 96f, 96f );

				container.ResumeLayout();
			}

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

	}

}
