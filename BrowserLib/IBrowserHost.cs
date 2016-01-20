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

		[OperationContract]
		void ConfigurationUpdated( BrowserConfiguration config );

		[OperationContract]
		void GetIconResource();

		[OperationContract]
		void RequestNavigation( string baseurl );

	}

	/// <summary>
	/// ブラウザ側で必要な設定をまとめた
	/// 更新があったときにまとめて送る
	/// </summary>
	[DataContract]
	public class BrowserConfiguration {
		/// <summary>
		/// ブラウザの拡大率 10-1000(%)
		/// </summary>
		[DataMember]
		public int ZoomRate { get; set; }

		/// <summary>
		/// ブラウザをウィンドウサイズに合わせる
		/// </summary>
		[DataMember]
		public bool ZoomFit { get; set; }

		/// <summary>
		/// ログインページのURL
		/// </summary>
		[DataMember]
		public string LogInPageURL { get; set; }

		/// <summary>
		/// ブラウザを有効にするか
		/// </summary>
		[DataMember]
		public bool IsEnabled { get; set; }

		/// <summary>
		/// スクリーンショットの保存先フォルダ
		/// </summary>
		[DataMember]
		public string ScreenShotPath { get; set; }

		/// <summary>
		/// スクリーンショットのフォーマット
		/// 1=jpeg, 2=png
		/// </summary>
		[DataMember]
		public int ScreenShotFormat { get; set; }

		/// <summary>
		/// 適用するスタイルシート
		/// </summary>
		[DataMember]
		public string StyleSheet { get; set; }

		/// <summary>
		/// スクロール可能かどうか
		/// </summary>
		[DataMember]
		public bool IsScrollable { get; set; }

		/// <summary>
		/// スタイルシートを適用するか
		/// </summary>
		[DataMember]
		public bool AppliesStyleSheet { get; set; }

		/// <summary>
		/// ツールメニューの配置
		/// </summary>
		[DataMember]
		public int ToolMenuDockStyle { get; set; }

		/// <summary>
		/// ツールメニューの可視性
		/// </summary>
		[DataMember]
		public bool IsToolMenuVisible { get; set; }

		/// <summary>
		/// 再読み込み時に確認ダイアログを入れるか
		/// </summary>
		[DataMember]
		public bool ConfirmAtRefresh { get; set; }

		/// <summary>
		/// 直连swf时替换的embed元素
		/// </summary>
		[DataMember]
		public string EmbedHtml { get; set; }

		/// <summary>
		/// Flash模式
		/// </summary>
		[DataMember]
		public string FlashWmode { get; set; }

		/// <summary>
		/// Flash质量
		/// </summary>
		[DataMember]
		public string FlashQuality { get; set; }

		/// <summary>
		/// 主题ID
		/// </summary>
		[DataMember]
		public int ThemeID { get; set; }

		/// <summary>
		/// 是否显示URL地址
		/// </summary>
		[DataMember]
		public bool ShowURL { get; set; }

		/// <summary>
		/// 是否修改区域Cookie
		/// </summary>
		[DataMember]
		public bool ModifyCookieRegion { get; set; }

		/// <summary>
		/// 現在の音量
		/// </summary>
		[DataMember]
		public float Volume { get; set; }

		/// <summary>
		/// ミュートかどうか
		/// </summary>
		[DataMember]
		public bool IsMute { get; set; }

	}
}
