using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility {


	public delegate void LogAddedEventHandler( Logger.LogData data );

	
	public sealed class Logger {

		#region Singleton

		private static readonly Logger instance = new Logger();

		public static Logger Instance {
			get { return instance; }
		}

		#endregion


		/// <summary>
		/// ログが追加された時に発生します。
		/// </summary>
		public event LogAddedEventHandler LogAdded = delegate { };


		public class LogData {

			/// <summary>
			/// 書き込み時刻
			/// </summary>
			public readonly DateTime Time;

			/// <summary>
			/// 優先度
			/// 基本的には 0=非表示(ログファイルにのみ記載, など), 1=内部情報(動作ログ, API受信情報など), 2=重要な情報(入渠完了など, ユーザーに表示する必要があるもの), 3=緊急の情報(エラー等)
			/// </summary>
			public readonly int Priority;

			/// <summary>
			/// ログ内容
			/// </summary>
			public readonly string Message;
			
			public LogData( DateTime time, int priority, string message ) {
				Time = time;
				Priority = priority;
				Message = message;
			}

		}



		private List<LogData> log;
		private bool toDebugConsole;
		private int priorityBorder;

		private Logger() {
			log = new List<LogData>();
			toDebugConsole = true;
			priorityBorder = 1;
		}


		public static IReadOnlyList<LogData> Log {
			get {
				return Logger.Instance.log.AsReadOnly();
			}
		}


		/// <summary>
		/// ログを追加します。
		/// </summary>
		/// <param name="priority">優先度。</param>
		/// <param name="message">ログ内容。</param>
		public static void Add( int priority, string message ) {
			if ( Logger.Instance.priorityBorder <= priority ) {

				LogData data = new LogData( DateTime.Now, priority, message );

				Logger.Instance.log.Add( data );
				if ( Logger.Instance.toDebugConsole ) {
					System.Diagnostics.Debug.WriteLine( string.Format( "[{0}][{1}] : {2}", DateTime.Now, priority, message ) );
				}

				Logger.Instance.LogAdded( data );
			}
		}

		/// <summary>
		/// ログをすべて消去します。
		/// </summary>
		public static void Clear() {
			Logger.instance.log.Clear();
		}

	}
}
