using ElectronicObserver.Utility;
using ElectronicObserver.Window;
using ElectronicObserver.Window.Dialog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Notifier {
	
	/// <summary>
	/// 通知を扱います。
	/// </summary>
	public abstract class NotifierBase {

		/// <summary>
		/// 通知音
		/// </summary>
		public SoundPlayer Sound { get; protected set; }

		/// <summary>
		/// 通知用の画像
		/// </summary>
		public Bitmap Image { get; protected set; }

		/// <summary>
		/// 通知メッセージ
		/// </summary>
		public string Message { get; protected set; }


		/// <summary>
		/// 通知のタイトル
		/// </summary>
		public string Title { get; protected set; }


		/// <summary>
		/// 通知音を再生するか
		/// </summary>
		public bool PlaysNotificationSound { get; set; }

		/// <summary>
		/// 通知ダイアログを表示するか
		/// </summary>
		public bool ShowsNotificationDialog { get; set; }


		/// <summary>
		/// 自動で閉じるまでの時間(ミリ秒, 0=閉じない)
		/// </summary>
		public int AutoClosingInterval { get; set; }


		public NotifierBase() {

			Sound = null;
			Image = null;
			
			SystemEvents.UpdateTimerTick += UpdateTimerTick;

		}

		protected virtual void UpdateTimerTick() {}


		#region 通知音

		/// <summary>
		/// 通知音を読み込みます。
		/// </summary>
		public void LoadSound( string path ) {
			try {

				DisposeSound();
				Sound = new SoundPlayer( path );
			
			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, string.Format( "通知システム: 通知音 {0} のロードに失敗しました。", path ) );

			}
		}

		/// <summary>
		/// 通知音を再生します。
		/// </summary>
		public void PlaySound() {
			try {

				if ( Sound != null && PlaysNotificationSound ) {
					Sound.Play();
				}

			} catch ( Exception ex ) {

				Utility.Logger.Add( 3, "通知システム: 通知音の再生に失敗しました。" + ex.Message );
			}
		}

		/// <summary>
		/// 通知音を破棄します。
		/// </summary>
		public void DisposeSound() {
			if ( Sound != null ) {
				Sound.Dispose();
				Sound = null;
			}
		}

		#endregion


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


		/// <summary>
		/// 通知ダイアログを表示します。
		/// </summary>
		public void ShowDialog() {

			if ( ShowsNotificationDialog )
				new DialogNotifier( this ).Show();

		}

		/// <summary>
		/// 通知を行います。
		/// </summary>
		public virtual void Notify() {

			ShowDialog();
			PlaySound();

		}

	}
}
