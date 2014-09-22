using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.Mathematics {

	/// <summary>
	/// APIに含まれている日時データを<see cref="DateTime"/>に変換します。
	/// </summary>
	public static class DateConverter {

		/// <summary>
		/// 起点となる日時。日本基準です。
		/// </summary>
		private static readonly DateTime origin = new DateTime( 1970, 1, 1, 9, 0, 0 );

		/// <summary>
		/// APIに含まれている日時データから<see cref="System.DateTime"/>を生成します。
		/// </summary>
		/// <param name="time">日時データ</param>
		/// <returns>変換された<see cref="DateTime"/>。</returns>
		public static DateTime FromAPITime( long time ) {
			return new DateTime( time * 10000 + origin.Ticks );
		}


		/// <summary>
		/// 残り時間を標準的書式の文字列に変換します。
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public static string ToTimeRemainString( DateTime time ) {
			var span = time - DateTime.Now;
			if ( span.Ticks < 0 )
				return "00:00:00";
			else
				return string.Format( "{0:D2}:{1:D2}:{2:D2}", (int)span.TotalHours, span.Minutes, span.Seconds );
		}
	}

}
