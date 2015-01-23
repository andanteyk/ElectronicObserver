using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Notifier {
	
	public class NotifierDialogData {

		/// <summary>
		/// 通知用の画像
		/// </summary>
		public Bitmap Image { get; protected set; }

		/// <summary>
		/// 通知メッセージ
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// 通知のタイトル
		/// </summary>
		public string Title { get; set; }


		public bool DrawsImage { get; set; }
		/// <summary>
		/// 通知メッセージを描画するか
		/// </summary>
		public bool DrawsMessage { get; set; }


		/// <summary>
		/// 自動で閉じるまでの時間(ミリ秒, 0=閉じない)
		/// </summary>
		public int ClosingInterval { get; set; }

		/// <summary>
		/// マウスポインタがフォーム上を動いたとき自動的に閉じる
		/// </summary>
		public bool CloseOnMouseMove { get; set; }



		public NotifierDialogData() {

			Image = null;

		}

		public NotifierDialogData( Utility.Configuration.ConfigurationData.ConfigNotification config ) {

			Image = null;
			if ( config.DrawsImage && config.ImagePath != null && config.ImagePath != "" )
				LoadImage( config.ImagePath );
			DrawsImage = config.DrawsImage;
			DrawsMessage = config.DrawsMessage;
			ClosingInterval = config.ClosingInterval;
			CloseOnMouseMove = config.CloseOnMouseMove;

		}


		public NotifierDialogData Clone() {
			return (NotifierDialogData)MemberwiseClone();
		}


		#region 通知画像

		/// <summary>
		/// 通知画像を読み込みます。
		/// </summary>
		/// <param name="path"></param>
		public void LoadImage( string path ) {

			try {

				DisposeImage();
				Image = new Bitmap( path );

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, string.Format( "通知システム: 通知画像 {0} の読み込みに失敗しました。", path ) );
			}

		}

		/// <summary>
		/// 通知画像を破棄します。
		/// </summary>
		public void DisposeImage() {
			if ( Image != null ) {
				Image.Dispose();
				Image = null;
			}
		}

		#endregion
	}

}
