using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer {

	public delegate void APIReceivedEventHandler( string apiname, dynamic data );

	/// <summary>
	/// API処理部の基底となるクラスです。
	/// </summary>
	public abstract class APIBase {

		/// <summary>
		/// Requestの処理が完了した時に発生します。
		/// </summary>
		public event APIReceivedEventHandler RequestReceived = delegate { };
		
		/// <summary>
		/// Responseの処理が完了した時に発生します。
		/// </summary>
		public event APIReceivedEventHandler ResponseReceived = delegate { };


		/// <summary>
		/// Requestを処理し、RequestReceivedを起動します。
		/// 継承時は最後に呼ぶようにして下さい。
		/// </summary>
		/// <param name="data">処理するデータ。</param>
		public virtual void OnRequestReceived( Dictionary<string, string> data ) {
			RequestReceived( APIName, data );
		}

		/// <summary>
		/// Responseを処理し、ResponseReceivedを起動します。
		/// 継承時は最後に呼ぶようにして下さい。
		/// </summary>
		/// <param name="data">処理するデータ。</param>
		public virtual void OnResponseReceived( dynamic data ) {
			ResponseReceived( APIName, data );
		}


		/// <summary>
		/// Requestの処理をサポートしているかを取得します。
		/// </summary>
		public virtual bool IsRequestSupported { get { return false; } }

		/// <summary>
		/// Responseの処理をサポートしているかを取得します。
		/// </summary>
		public virtual bool IsResponseSupported { get { return true; } }



		/// <summary>
		/// API名を取得します。
		/// </summary>
		public abstract string APIName { get; }
	
	}

}
