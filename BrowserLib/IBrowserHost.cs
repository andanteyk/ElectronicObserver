using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BrowserLib {
	/// <summary>
	/// FormBrowserHostのインターフェス
	/// WCFでプロセス間通信用
	/// </summary>
	[ServiceContract]
	public interface IBrowserHost {
		/// <summary>
		/// ブラウザ側で必要な設定情報
		/// </summary>
		BrowserConfiguration Configuration {
			[OperationContract]
			get;
		}

		/// <summary>
		/// キーメッセージの送り先
		/// </summary>
		IntPtr HWND {
			[OperationContract]
			get;
		}

		/// <summary>
		/// ブラウザのウィンドウハンドルが作成されたあとホスト側で処理するために呼び出す
		/// </summary>
		[OperationContract]
		void ConnectToBrowser( IntPtr hwnd );

		/// <summary>
		/// エラー報告
		/// 例外は送れるのかよく分からなかったら例外の名前だけ送るようにした
		/// </summary>
		[OperationContract]
		void SendErrorReport( string exceptionName, string message );

		/// <summary>
		/// ログ追加
		/// </summary>
		[OperationContract]
		void AddLog( int priority, string message );
	}

	/// <summary>
	/// ブラウザ側で必要な設定をまとめた
	/// 更新があったときにまとめて送る
	/// </summary>
	[DataContract]
	public class BrowserConfiguration {
		[DataMember]
		public bool IsScrollable;

		[DataMember]
		public string StyleSheet;

		[DataMember]
		public string LogInPageURL;

		[DataMember]
		public int ZoomRate;
	}
}
