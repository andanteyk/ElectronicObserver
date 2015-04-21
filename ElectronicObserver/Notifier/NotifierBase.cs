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
		/// 通知ダイアログに渡す設定データ
		/// </summary>
		public NotifierDialogData DialogData { get; protected set; }

		/// <summary>
		/// 有効かどうか
		/// </summary>
		public bool IsEnabled { get; set; }


		/// <summary>
		/// 通知音
		/// </summary>
		public SoundPlayer Sound { get; protected set; }

		/// <summary>
		/// 通知音のパス
		/// </summary>
		public string SoundPath { get; set; }

		/// <summary>
		/// 通知音を再生するか
		/// </summary>
		public bool PlaysSound { get; set; }

		/// <summary>
		/// undone
		/// 通知音をループさせるか
		/// </summary>
		public bool LoopsSound { get; set; }

		/// <summary>
		/// 通知ダイアログを表示するか
		/// </summary>
		public bool ShowsDialog { get; set; }


		/// <summary>
		/// 通知を早める時間(ミリ秒)
		/// </summary>
		public int AccelInterval { get; set; }


		public NotifierBase() {

			Initialize();
			DialogData = new NotifierDialogData();

		}

		public NotifierBase( Utility.Configuration.ConfigurationData.ConfigNotifierBase config ) {

			Initialize();
			DialogData = new NotifierDialogData( config );
			if ( config.PlaysSound && config.SoundPath != null && config.SoundPath != "" )
				LoadSound( config.SoundPath );

			IsEnabled = config.IsEnabled;
			PlaysSound = config.PlaysSound;
			ShowsDialog = config.ShowsDialog;
			AccelInterval = config.AccelInterval;

		}

		private void Initialize() {

			SystemEvents.UpdateTimerTick += UpdateTimerTick;
			Sound = null;
			SoundPath = "";
		}


		protected virtual void UpdateTimerTick() { }


		#region 通知音

		/// <summary>
		/// 通知音を読み込みます。
		/// </summary>
		/// <param name="path">音声ファイルへのパス。</param>
		/// <returns>成功すれば true 、失敗すれば false を返します。</returns>
		public bool LoadSound( string path ) {
			try {

				DisposeSound();
				Sound = new SoundPlayer( path );
				SoundPath = path;

				return true;

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, string.Format( "通知システム: 通知音 {0} のロードに失敗しました。", path ) );
				DisposeSound();

			}

			return false;
		}

		/// <summary>
		/// 通知音を再生します。
		/// </summary>
		public void PlaySound() {
			try {

				if ( Sound != null && PlaysSound ) {
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
			SoundPath = "";
		}

		#endregion



		/// <summary>
		/// 通知ダイアログを表示します。
		/// </summary>
		public void ShowDialog() {

			if ( ShowsDialog )
				NotifierManager.Instance.ShowNotifier( new DialogNotifier( DialogData ) );

		}

		/// <summary>
		/// 通知を行います。
		/// </summary>
		public virtual void Notify() {

			if ( !IsEnabled ) return;

			ShowDialog();
			PlaySound();

		}


		public virtual void ApplyToConfiguration( Utility.Configuration.ConfigurationData.ConfigNotifierBase config ) {

			DialogData.ApplyToConfiguration( config );
			config.PlaysSound = PlaysSound;
			config.SoundPath = SoundPath;
			config.IsEnabled = IsEnabled;
			config.ShowsDialog = ShowsDialog;
			config.AccelInterval = AccelInterval;

		}

	}
}
