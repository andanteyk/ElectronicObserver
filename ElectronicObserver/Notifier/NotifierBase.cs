using ElectronicObserver.Utility;
using ElectronicObserver.Window;
using ElectronicObserver.Window.Dialog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
		public MediaPlayer Sound { get; protected set; }

		/// <summary>
		/// 通知音のパス
		/// </summary>
		public string SoundPath { get; set; }

		/// <summary>
		/// 通知音を再生するか
		/// </summary>
		public bool PlaysSound { get; set; }


		private bool _loopsSound;
		/// <summary>
		/// 通知音をループさせるか
		/// </summary>
		public bool LoopsSound {
			get { return _loopsSound; }
			set {
				_loopsSound = value;
				SetIsLoop();
			}
		}

		private int _soundVolume;
		/// <summary>
		/// 通知音の音量 (0-100)
		/// </summary>
		public int SoundVolume {
			get { return _soundVolume; }
			set {
				_soundVolume = value;
				if ( !Utility.Configuration.Config.Control.UseSystemVolume )
					Sound.Volume = _soundVolume;
			}
		}

		private bool _showsDialog;
		/// <summary>
		/// 通知ダイアログを表示するか
		/// </summary>
		public bool ShowsDialog {
			get { return _showsDialog; }
			set {
				_showsDialog = value;
				SetIsLoop();
			}
		}

		private void SetIsLoop() {
			Sound.IsLoop = LoopsSound && ShowsDialog;
		}


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
			SoundVolume = config.SoundVolume;
			LoopsSound = config.LoopsSound;
			ShowsDialog = config.ShowsDialog;
			AccelInterval = config.AccelInterval;

		}

		private void Initialize() {

			SystemEvents.UpdateTimerTick += UpdateTimerTick;
			Sound = new MediaPlayer();
			Sound.IsShuffle = true;
			Sound.MediaEnded += Sound_MediaEnded;
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

				if ( File.Exists( path ) ) {
					Sound.SetPlaylist( null );
					Sound.SourcePath = path;

				} else if ( Directory.Exists( path ) ) {
					Sound.SetPlaylistFromDirectory( path );

				} else {
					throw new FileNotFoundException( "指定されたファイルまたはディレクトリが見つかりませんでした。" );
				}

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
					if ( Sound.PlayState == 3 ) {		//playing
						if ( Sound.GetPlaylist().Any() )
							Sound.Next();

						Sound.Stop();
					}

					//音量の再設定(システム側の音量変更によって設定が変わることがあるので)
					SoundVolume = _soundVolume;
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
			Sound.Close();
			Sound.SourcePath = SoundPath = "";
		}


		void Sound_MediaEnded() {
			if ( Sound.GetPlaylist().Any() && !LoopsSound )
				Sound.Next();
		}


		#endregion



		/// <summary>
		/// 通知ダイアログを表示します。
		/// </summary>
		public void ShowDialog() {

			if ( ShowsDialog ) {
				var dialog = new DialogNotifier( DialogData );
				dialog.FormClosing += dialog_FormClosing;
				NotifierManager.Instance.ShowNotifier( dialog );
			}
		}

		void dialog_FormClosing( object sender, System.Windows.Forms.FormClosingEventArgs e ) {
			if ( LoopsSound ) {
				Sound.Stop();
				Sound.Next();
			}
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
			config.SoundVolume = SoundVolume;
			config.LoopsSound = LoopsSound;
			config.IsEnabled = IsEnabled;
			config.ShowsDialog = ShowsDialog;
			config.AccelInterval = AccelInterval;

		}

	}
}
