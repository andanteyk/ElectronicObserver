using System;
using System.Collections.Generic;
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
		
	}

}
