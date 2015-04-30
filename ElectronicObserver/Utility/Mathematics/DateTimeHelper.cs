using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.Mathematics {

	/// <summary>
	/// 日時データを扱うためのメソッド群です。
	/// </summary>
	public static class DateTimeHelper {

		/// <summary>
		/// 起点となる日時。
		/// </summary>
		private static readonly long origin = new DateTime( 1970, 1, 1, 0, 0, 0 ).Ticks;


		/// <summary>
		/// APIに含まれている日時データから<see cref="System.DateTime"/>を生成します。
		/// </summary>
		/// <param name="time">日時データ。</param>
		/// <returns>変換された<see cref="DateTime"/>。</returns>
		public static DateTime FromAPITime( long time ) {
			return new DateTime( time * 10000 + origin, DateTimeKind.Utc ).ToLocalTime();
		}

		/// <summary>
		/// <see cref="System.DateTime"/>からAPIに含まれている日時データを生成します。
		/// </summary>
		/// <param name="time">日時データ。</param>
		/// <returns>変換された日時のAPIデータ。</returns>
		public static long ToAPITime( DateTime time ) {
			return ( time.ToUniversalTime().Ticks - origin ) / 10000;
		}


		/// <summary>
		/// APIに含まれている時間データからTimeSpanを生成します。
		/// </summary>
		/// <param name="time">時間データ。</param>
		/// <returns>変換されたTimeSpan。</returns>
		public static TimeSpan FromAPITimeSpan( long time ) {
			return new TimeSpan( time * 10000 );
		}


		/// <summary>
		/// 残り時間を標準的書式の文字列に変換します。
		/// </summary>
		/// <param name="time">完了時刻。</param>
		/// <returns>書式に則った時間を表す文字列。</returns>
		public static string ToTimeRemainString( DateTime time ) {
			return ToTimeRemainString( time - DateTime.Now );
		}

		/// <summary>
		/// 残り時間を標準的書式の文字列に変換します。
		/// </summary>
		/// <param name="span">残り時間。</param>
		/// <returns>書式に則った時間を表す文字列。</returns>
		public static string ToTimeRemainString( TimeSpan span ) {
			if ( span.Ticks < 0 )
				return "00:00:00";
			else
				return string.Format( "{0:D2}:{1:D2}:{2:D2}", (int)span.TotalHours, span.Minutes, span.Seconds );
		}


		/// <summary>
		/// 経過時間を標準的書式の文字列に変換します。
		/// </summary>
		/// <param name="time">起点時間。</param>
		/// <returns>書式に則った時間を表す文字列。</returns>
		public static string ToTimeElapsedString( DateTime time ) {
			return ToTimeElapsedString( DateTime.Now - time );
		}

		/// <summary>
		/// 経過時間を標準的書式の文字列に変換します。
		/// </summary>
		/// <param name="span">経過時間。</param>
		/// <returns>書式に則った時間を表す文字列。</returns>
		public static string ToTimeElapsedString( TimeSpan span ) {
			return ToTimeRemainString( span );
		}


		/// <summary>
		/// 指定した日時をまたいでいるかを取得します。
		/// </summary>
		/// <param name="prev">前回処理した時の日時。</param>
		/// <param name="border">指定した日時。</param>
		/// <returns></returns>
		public static bool IsCrossed( DateTime prev, DateTime border ) {
			return prev < border;
		}


		public static bool IsCrossedHour( DateTime prev ) {

			DateTime nexthour = prev.Date.AddHours( prev.Hour + 1 );
			return nexthour <= DateTime.Now;
		}


		/// <summary>
		/// 指定した日時をまたいでいるかを取得します。日単位で処理されます。
		/// </summary>
		/// <param name="prev">前回処理した時の日時。</param>
		/// <param name="hours">指定した日時の時間。</param>
		/// <param name="minutes">指定した日時の分。</param>
		/// <param name="seconds">指定した日時の秒。</param>
		/// <returns></returns>
		public static bool IsCrossedDay( DateTime prev, int hours, int minutes, int seconds ) {

			DateTime now = DateTime.Now;

			TimeSpan nowtime = now.TimeOfDay;
			TimeSpan bordertime = new TimeSpan( hours, minutes, seconds ) + GetTimeDifference();

			return IsCrossed( prev, now.Subtract( new TimeSpan( nowtime < bordertime ? 1 : 0, nowtime.Hours, nowtime.Minutes, nowtime.Seconds ) ).Add( bordertime ) );	
		}


		/// <summary>
		/// 指定した日時をまたいでいるかを取得します。週単位で処理されます。
		/// </summary>
		/// <param name="prev">前回処理した時の日時。</param>
		/// <param name="dayOfWeek">指定した日時の曜日。</param>
		/// <param name="hours">指定した日時の時間。</param>
		/// <param name="minutes">指定した日時の分。</param>
		/// <param name="seconds">指定した日時の秒。</param>
		/// <returns></returns>
		public static bool IsCrossedWeek( DateTime prev, DayOfWeek dayOfWeek, int hours, int minutes, int seconds ) {

			DateTime now = DateTime.Now;

			TimeSpan nowtime = now.TimeOfDay;
			TimeSpan bordertime = new TimeSpan( hours, minutes, seconds ) + GetTimeDifference();

			int dayshift = now.DayOfWeek - dayOfWeek;
			if ( dayshift < 0 )
				dayshift += 7;
			else if ( dayshift == 0 && nowtime < bordertime )
				dayshift += 7;

			DateTime border = now.Subtract( new TimeSpan( dayshift, nowtime.Hours, nowtime.Minutes, nowtime.Seconds ) ).Add( bordertime );

			return IsCrossed( prev, border );
		}


		/// <summary>
		/// 指定した日時をまたいでいるかを取得します。月単位で処理されます。
		/// </summary>
		/// <param name="prev">前回処理した時の日時。</param>
		/// <param name="days">指定した日時の日付。</param>
		/// <param name="hours">指定した日時の時間。</param>
		/// <param name="minutes">指定した日時の分。</param>
		/// <param name="seconds">指定した日時の秒。</param>
		/// <returns></returns>
		public static bool IsCrossedMonth( DateTime prev, int days, int hours, int minutes, int seconds ) {

			DateTime now = DateTime.Now;

			DateTime border = now.Subtract( new TimeSpan( now.Day, now.Hour, now.Minute, now.Second ) ).Add( new TimeSpan( days, hours, minutes, seconds ) + GetTimeDifference() );
			if ( now < border )
				border = border.AddMonths( -1 );

			return IsCrossed( prev, border );
		}


		/// <summary>
		/// ファイル名の一部として利用できるフォーマットの現在日時文字列を取得します。
		/// </summary>
		/// <returns>変換結果の文字列。</returns>
		public static string GetTimeStamp() {
			return GetTimeStamp( DateTime.Now );
		}

		/// <summary>
		/// ファイル名の一部として利用できるフォーマットの日時文字列を取得します。
		/// </summary>
		/// <param name="time">指定する日時。</param>
		/// <returns>変換結果の文字列。</returns>
		public static string GetTimeStamp( DateTime time ) {

			return time.ToString( "yyyyMMdd_HHmmssff" );
		}



		public static string TimeToCSVString( DateTime time ) {
			return time.ToString( "yyyy/MM/dd HH:mm:ss" );
		}

		public static DateTime CSVStringToTime( string str ) {
			string[] elem = str.Split( "/ :".ToCharArray() );
			return new DateTime( int.Parse( elem[0] ), int.Parse( elem[1] ), int.Parse( elem[2] ), int.Parse( elem[3] ), int.Parse( elem[4] ), int.Parse( elem[5] ) );
		}


		/// <summary>
		/// 現在地点と東京標準時(艦これ時間)との時差を取得します。
		/// </summary>
		public static TimeSpan GetTimeDifference() {
			return TimeZoneInfo.Local.BaseUtcOffset - TimeZoneInfo.FindSystemTimeZoneById( "Tokyo Standard Time" ).BaseUtcOffset;
		}


	}


}
