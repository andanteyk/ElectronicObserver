using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ElectronicObserver.Utility {

	/// <summary>
	/// Windows Media Player コントロールを利用して、音楽を再生するためのクラスです。
	/// </summary>
	public class MediaPlayer {

		private dynamic _wmp;

		public event Action<int> PlayStateChange = delegate { };
		public event Action MediaEnded = delegate { };

		private List<string> _playlist;
		private List<string> _realPlaylist;

		private Random _rand;


		/// <summary>
		/// 対応している拡張子リスト
		/// </summary>
		public static readonly ReadOnlyCollection<string> SupportedExtensions =
			new ReadOnlyCollection<string>( new List<string>() {
			"asf",
			"wma",
			"mp2",
			"mp3",
			"mid",
			"midi",
			"rmi",
			"aif",
			"aifc",
			"aiff",
			"au",
			"snd",
			"wav",
			"m4a",
			"aac",
			"flac",
			"mka",
		} );

		private static readonly Regex SupportedFileName = new Regex( ".*\\.(" + string.Join( "|", SupportedExtensions ) + ")", RegexOptions.Compiled );


		public MediaPlayer() {
			var type = Type.GetTypeFromProgID( "WMPlayer.OCX.7" );
			if ( type != null ) {
				_wmp = Activator.CreateInstance( type );
				_wmp.uiMode = "none";
				_wmp.settings.autoStart = false;
				_wmp.PlayStateChange += new Action<int>( wmp_PlayStateChange );
			} else {
				_wmp = null;
			}

			IsLoop = false;
			_isShuffle = false;
			IsMute = false;
			LoopHeadPosition = 0.0;
			AutoPlay = false;
			_playlist = new List<string>();
			_realPlaylist = new List<string>();
			_rand = new Random();

			MediaEnded += MediaPlayer_MediaEnded;
		}


		/// <summary>
		/// 利用可能かどうか
		/// false の場合全機能が使用不可能
		/// </summary>
		public bool IsAvailable {
			get { return _wmp != null; }
		}

		/// <summary>
		/// メディアファイルのパス。
		/// 再生中に変更された場合停止します。
		/// </summary>
		public string SourcePath {
			get { return !IsAvailable ? string.Empty : _wmp.URL; }
			set {
				if ( IsAvailable && _wmp.URL != value )
					_wmp.URL = value;
			}
		}

		/// <summary>
		/// 音量
		/// 0-100
		/// 注: システムの音量設定と連動しているようなので注意が必要
		/// </summary>
		public int Volume {
			get { return !IsAvailable ? 0 : _wmp.settings.volume; }
			set { if ( IsAvailable ) _wmp.settings.volume = value; }
		}

		/// <summary>
		/// ミュート
		/// </summary>
		public bool IsMute {
			get { return !IsAvailable ? false : _wmp.settings.mute; }
			set { if ( IsAvailable ) _wmp.settings.mute = value; }
		}


		private bool _isLoop;
		/// <summary>
		/// ループするか
		/// </summary>
		public bool IsLoop {
			get { return _isLoop; }
			set {
				_isLoop = value;
				if ( IsAvailable )
					_wmp.settings.setMode( "loop", _isLoop );
			}
		}

		/// <summary>
		/// ループ時の先頭 (秒単位)
		/// </summary>
		public double LoopHeadPosition { get; set; }


		/// <summary>
		/// 現在の再生地点 (秒単位)
		/// </summary>
		public double CurrentPosition {
			get { return !IsAvailable ? 0.0 : _wmp.controls.currentPosition; }
			set { if ( IsAvailable ) _wmp.controls.currentPosition = value; }
		}

		/// <summary>
		/// 再生状態
		/// </summary>
		public int PlayState {
			get { return !IsAvailable ? 0 : _wmp.playState; }
		}

		/// <summary>
		/// 現在のメディアの名前
		/// </summary>
		public string MediaName {
			get { return !IsAvailable ? string.Empty : _wmp.currentMedia != null ? _wmp.currentMedia.name : null; }
		}

		/// <summary>
		/// 現在のメディアの長さ(秒単位)
		/// なければ 0
		/// </summary>
		public double Duration {
			get { return !IsAvailable ? 0.0 : _wmp.currentMedia != null ? _wmp.currentMedia.duration : 0; }
		}



		/// <summary>
		/// プレイリストのコピーを取得します。
		/// </summary>
		/// <returns></returns>
		public List<string> GetPlaylist() {
			return new List<string>( _playlist );
		}

		/// <summary>
		/// プレイリストを設定します。
		/// </summary>
		/// <param name="list"></param>
		public void SetPlaylist( IEnumerable<string> list ) {
			if ( list == null )
				_playlist = new List<string>();
			else
				_playlist = list.Distinct().ToList();

			UpdateRealPlaylist();
		}


		public IEnumerable<string> SearchSupportedFiles( string path, System.IO.SearchOption option = System.IO.SearchOption.TopDirectoryOnly ) {
			return System.IO.Directory.EnumerateFiles( path, "*", option ).Where( s => SupportedFileName.IsMatch( s ) );
		}

		/// <summary>
		/// フォルダを検索し、音楽ファイルをプレイリストに設定します。
		/// </summary>
		/// <param name="path">フォルダへのパス。</param>
		/// <param name="option">検索オプション。既定ではサブディレクトリは検索されません。</param>
		public void SetPlaylistFromDirectory( string path, System.IO.SearchOption option = System.IO.SearchOption.TopDirectoryOnly ) {
			SetPlaylist( SearchSupportedFiles( path, option ) );
		}



		private int _playingIndex;
		/// <summary>
		/// 現在再生中の曲のプレイリスト中インデックス
		/// </summary>
		private int PlayingIndex {
			get { return _playingIndex; }
			set {
				if ( _playingIndex != value ) {

					if ( value < 0 || _realPlaylist.Count <= value )
						return;

					_playingIndex = value;
					SourcePath = _realPlaylist[_playingIndex];
					if ( AutoPlay )
						Play();
				}
			}
		}

		private bool _isShuffle;
		/// <summary>
		/// シャッフル再生するか
		/// </summary>
		public bool IsShuffle {
			get { return _isShuffle; }
			set {
				bool changed = _isShuffle != value;

				_isShuffle = value;

				if ( changed ) {
					UpdateRealPlaylist();
				}
			}
		}

		/// <summary>
		/// 曲が終了したとき自動で次の曲を再生するか
		/// </summary>
		public bool AutoPlay { get; set; }





		/// <summary>
		/// 再生
		/// </summary>
		public void Play() {
			if ( !IsAvailable ) return;
			
			if ( _realPlaylist.Count > 0 && SourcePath != _realPlaylist[_playingIndex] )
				SourcePath = _realPlaylist[_playingIndex];

			_wmp.controls.play();
		}

		/// <summary>
		/// ポーズ
		/// </summary>
		public void Pause() {
			if ( !IsAvailable ) return;

			_wmp.controls.pause();
		}

		/// <summary>
		/// 停止
		/// </summary>
		public void Stop() {
			if ( !IsAvailable ) return;

			_wmp.controls.stop();
		}

		/// <summary>
		/// ファイルを閉じる
		/// </summary>
		public void Close() {
			if ( !IsAvailable ) return;

			_wmp.close();
		}


		/// <summary>
		/// 次の曲へ
		/// </summary>
		public void Next() {
			if ( !IsAvailable ) return;

			int prevState = PlayState;

			if ( PlayingIndex >= _realPlaylist.Count - 1 ) {
				if ( IsShuffle )
					UpdateRealPlaylist();
				PlayingIndex = 0;
			} else {
				PlayingIndex++;
			}

			if ( prevState == 3 || AutoPlay )		// Playing
				Play();
		}

		/// <summary>
		/// 前の曲へ
		/// </summary>
		public void Prev() {
			if ( !IsAvailable ) return;

			if ( IsShuffle )
				return;

			int prevState = PlayState;

			if ( PlayingIndex == 0 )
				PlayingIndex = _realPlaylist.Count - 1;
			else
				PlayingIndex--;

			if ( prevState == 3 || AutoPlay )		// Playing
				Play();
		}

		private void UpdateRealPlaylist() {
			if ( !IsAvailable ) return;

			if ( !IsShuffle ) {
				_realPlaylist = new List<string>( _playlist );

			} else {
				// shuffle
				_realPlaylist = _playlist.OrderBy( s => Guid.NewGuid() ).ToList();

				// 同じ曲が連続で流れるのを防ぐ
				if ( _realPlaylist.Count > 1 && SourcePath == _realPlaylist[0] ) {
					_realPlaylist = _realPlaylist.Skip( 1 ).ToList();
					_realPlaylist.Insert( _rand.Next( 1, _realPlaylist.Count + 1 ), SourcePath );
				}
			}

			int index = _realPlaylist.IndexOf( SourcePath );
			PlayingIndex = index != -1 ? index : 0;
		}


		private bool _loopflag = false;
		void wmp_PlayStateChange( int NewState ) {

			// ループ用処理
			if ( IsLoop && LoopHeadPosition > 0.0 ) {
				switch ( NewState ) {
					case 8:		//MediaEnded
						_loopflag = true;
						break;

					case 3:		//playing
						if ( _loopflag ) {
							CurrentPosition = LoopHeadPosition;
							_loopflag = false;
						}
						break;
				}
			}

			if ( NewState == 8 )	//MediaEnded
				OnMediaEnded();

			PlayStateChange( NewState );
		}



		void MediaPlayer_MediaEnded() {
			// プレイリストの処理
			if ( !IsLoop && AutoPlay )
				Next();
		}


		// 即時変化させるとイベント終了直後に書き換えられて next が無視されるので苦肉の策
		private /*async*/ void OnMediaEnded() {
            //await Task.Run( () => Task.WaitAll( Task.Delay( 10 ) ) );
            System.Threading.Thread.Sleep( 10 );
			MediaEnded();
		}
	}
}
