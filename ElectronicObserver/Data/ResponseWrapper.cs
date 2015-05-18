using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {
	
	
	/// <summary>
	/// Responseを受信しデータを処理するクラスの基底です。
	/// </summary>
	public abstract class ResponseWrapper {

		/// <summary>
		/// 生の受信データ(api_data)
		/// </summary>
		public dynamic RawData { get; private set; }

		/// <summary>
		/// Responseを読み込みます。
		/// </summary>
		/// <param name="apiname">読み込むAPIの名前。</param>
		/// <param name="data">受信したデータ。</param>
		public virtual void LoadFromResponse( string apiname, dynamic data ) {
			RawData = data;
		}

		/// <summary>
		/// 現在のデータが有効かを取得します。
		/// </summary>
		public bool IsAvailable {
			get { return RawData != null; }
		}

		public ResponseWrapper() {
			RawData = null;
		}
	
	}

}
