using Codeplex.Data;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Utility.Storage;
using ElectronicObserver.Window.Dialog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Utility {


	public sealed class Configuration {


		private static readonly Configuration instance = new Configuration();

		public static Configuration Instance {
			get { return instance; }
		}


		private const string SaveFileName = @"Settings\Configuration.xml";


		public delegate void ConfigurationChangedEventHandler();
		public event ConfigurationChangedEventHandler ConfigurationChanged = delegate { };


		[DataContract( Name = "Configuration" )]
		public class ConfigurationData : DataStorage {

			public class ConfigPartBase {
				//reserved
			}


			/// <summary>
			/// 通信の設定を扱います。
			/// </summary>
			public class ConfigConnection : ConfigPartBase {

				/// <summary>
				/// ポート番号
				/// </summary>
				public ushort Port { get; set; }

				/// <summary>
				/// 通信内容を保存するか
				/// </summary>
				public bool SaveReceivedData { get; set; }

				/// <summary>
				/// 通信内容保存：保存先
				/// </summary>
				public string SaveDataPath { get; set; }

				/// <summary>
				/// 通信内容保存：Requestを保存するか
				/// </summary>
				public bool SaveRequest { get; set; }

				/// <summary>
				/// 通信内容保存：Responseを保存するか
				/// </summary>
				public bool SaveResponse { get; set; }

				/// <summary>
				/// 通信内容保存：SWFを保存するか
				/// </summary>
				public bool SaveSWF { get; set; }

				/// <summary>
				/// 通信内容保存：その他ファイルを保存するか
				/// </summary>
				public bool SaveOtherFile { get; set; }

				/// <summary>
				/// 通信内容保存：バージョンを追加するか
				/// </summary>
				public bool ApplyVersion { get; set; }


				/// <summary>
				/// システムプロキシに登録するか
				/// </summary>
				public bool RegisterAsSystemProxy { get; set; }

				/// <summary>
				/// 上流プロキシを利用するか
				/// </summary>
				public bool UseUpstreamProxy { get; set; }

				/// <summary>
				/// 上流プロキシのポート番号
				/// </summary>
				public ushort UpstreamProxyPort { get; set; }

				/// <summary>
				/// 上流プロキシのアドレス
				/// </summary>
				public string UpstreamProxyAddress { get; set; }

				/// <summary>
				/// システムプロキシを利用するか
				/// </summary>
				public bool UseSystemProxy { get; set; }

				/// <summary>
				/// 下流プロキシ設定
				/// 空なら他の設定から自動生成する
				/// </summary>
				public string DownstreamProxy { get; set; }


				/// <summary>
				/// kancolle-db.netに送信する
				/// </summary>
				public bool SendDataToKancolleDB { get; set; }

				/// <summary>
				/// kancolle-db.netのOAuth認証
				/// </summary>
				public string SendKancolleOAuth { get; set; }


				public ConfigConnection() {

					Port = 40620;
					SaveReceivedData = false;
					SaveDataPath = @"KCAPI";
					SaveRequest = false;
					SaveResponse = true;
					SaveSWF = false;
					SaveOtherFile = false;
					ApplyVersion = false;
					RegisterAsSystemProxy = false;
					UseUpstreamProxy = false;
					UpstreamProxyPort = 0;
					UpstreamProxyAddress = "127.0.0.1";
					UseSystemProxy = false;
					DownstreamProxy = "";
					SendDataToKancolleDB = false;
					SendKancolleOAuth = "";
				}

			}
			/// <summary>通信</summary>
			[DataMember]
			public ConfigConnection Connection { get; private set; }


			public class ConfigUI : ConfigPartBase {

				/// <summary>
				/// メインフォント
				/// </summary>
				public SerializableFont MainFont { get; set; }

				/// <summary>
				/// サブフォント
				/// </summary>
				public SerializableFont SubFont { get; set; }

				[IgnoreDataMember]
				private bool _barColorMorphing;

				/// <summary>
				/// HPバーの色を滑らかに変化させるか
				/// </summary>
				public bool BarColorMorphing {
					get { return _barColorMorphing; }
					set {
						_barColorMorphing = value;

						if ( !_barColorMorphing )
							BarColorScheme = new List<SerializableColor>( DefaultBarColorScheme[0] );
						else
							BarColorScheme = new List<SerializableColor>( DefaultBarColorScheme[1] );
					}
				}

				/// <summary>
				/// HPバーのカラーリング
				/// </summary>
				public List<SerializableColor> BarColorScheme { get; set; }


				[IgnoreDataMember]
				private readonly List<SerializableColor>[] DefaultBarColorScheme = new List<SerializableColor>[] {
					new List<SerializableColor>() {
						SerializableColor.UIntToColor( 0xFFFF0000 ),
						SerializableColor.UIntToColor( 0xFFFF0000 ),
						SerializableColor.UIntToColor( 0xFFFF8800 ),
						SerializableColor.UIntToColor( 0xFFFF8800 ),
						SerializableColor.UIntToColor( 0xFFFFCC00 ),
						SerializableColor.UIntToColor( 0xFFFFCC00 ),
						SerializableColor.UIntToColor( 0xFF00CC00 ),
						SerializableColor.UIntToColor( 0xFF00CC00 ),
						SerializableColor.UIntToColor( 0xFF0044CC ),
						SerializableColor.UIntToColor( 0xFF44FF00 ),
						SerializableColor.UIntToColor( 0xFF882222 ),
						SerializableColor.UIntToColor( 0xFF888888 ),
					},
					/*/// recognize
					new List<SerializableColor>() {
						SerializableColor.UIntToColor( 0xFFFF0000 ),
						SerializableColor.UIntToColor( 0xFFFF0000 ),
						SerializableColor.UIntToColor( 0xFFFF6600 ),
						SerializableColor.UIntToColor( 0xFFFF9900 ),
						SerializableColor.UIntToColor( 0xFFFFCC00 ),
						SerializableColor.UIntToColor( 0xFFEEEE00 ),
						SerializableColor.UIntToColor( 0xFFAAEE00 ),
						SerializableColor.UIntToColor( 0xFF00CC00 ),
						SerializableColor.UIntToColor( 0xFF0044CC ),
						SerializableColor.UIntToColor( 0xFF00FF44 ),
						SerializableColor.UIntToColor( 0xFF882222 ),
						SerializableColor.UIntToColor( 0xFF888888 ),
					},
					/*/// gradation
					new List<SerializableColor>() {
						SerializableColor.UIntToColor( 0xFFFF0000 ),
						SerializableColor.UIntToColor( 0xFFFF0000 ),
						SerializableColor.UIntToColor( 0xFFFF4400 ),
						SerializableColor.UIntToColor( 0xFFFF8800 ),
						SerializableColor.UIntToColor( 0xFFFFAA00 ),
						SerializableColor.UIntToColor( 0xFFEEEE00 ),
						SerializableColor.UIntToColor( 0xFFCCEE00 ),
						SerializableColor.UIntToColor( 0xFF00CC00 ),
						SerializableColor.UIntToColor( 0xFF0044CC ),
						SerializableColor.UIntToColor( 0xFF00FF44 ),
						SerializableColor.UIntToColor( 0xFF882222 ),
						SerializableColor.UIntToColor( 0xFF888888 ),
					},
					//*/
				};

				public ConfigUI() {
					MainFont = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
					SubFont = new Font( "Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel );
					BarColorMorphing = false;
				}
			}
			/// <summary>UI</summary>
			[DataMember]
			public ConfigUI UI { get; private set; }


			/// <summary>
			/// ログの設定を扱います。
			/// </summary>
			public class ConfigLog : ConfigPartBase {

				/// <summary>
				/// ログのレベル
				/// </summary>
				public int LogLevel { get; set; }

				/// <summary>
				/// ログを保存するか
				/// </summary>
				public bool SaveLogFlag { get; set; }

				/// <summary>
				/// エラーレポートを保存するか
				/// </summary>
				public bool SaveErrorReport { get; set; }

				/// <summary>
				/// ファイル エンコーディングのID
				/// </summary>
				public int FileEncodingID { get; set; }

				/// <summary>
				/// ファイル エンコーディング
				/// </summary>
				[IgnoreDataMember]
				public Encoding FileEncoding {
					get {
						switch ( FileEncodingID ) {
							case 0:
								return new System.Text.UTF8Encoding( false );
							case 1:
								return new System.Text.UTF8Encoding( true );
							case 2:
								return new System.Text.UnicodeEncoding( false, false );
							case 3:
								return new System.Text.UnicodeEncoding( false, true );
							case 4:
								return Encoding.GetEncoding( 932 );
							default:
								return new System.Text.UTF8Encoding( false );

						}
					}
				}

				/// <summary>
				/// ネタバレを許可するか
				/// </summary>
				public bool ShowSpoiler { get; set; }

				/// <summary>
				/// プレイ時間
				/// </summary>
				public double PlayTime { get; set; }

				/// <summary>
				/// これ以上の無通信時間があったときプレイ時間にカウントしない
				/// </summary>
				public double PlayTimeIgnoreInterval { get; set; }

				/// <summary>
				/// 戦闘ログを保存するか
				/// </summary>
				public bool SaveBattleLog { get; set; }

				public ConfigLog() {
					LogLevel = 2;
					SaveLogFlag = true;
					SaveErrorReport = true;
					FileEncodingID = 4;
					ShowSpoiler = true;
					PlayTime = 0;
					PlayTimeIgnoreInterval = 10 * 60;
					SaveBattleLog = false;
				}

			}
			/// <summary>ログ</summary>
			[DataMember]
			public ConfigLog Log { get; private set; }


			/// <summary>
			/// 動作の設定を扱います。
			/// </summary>
			public class ConfigControl : ConfigPartBase {

				/// <summary>
				/// 疲労度ボーダー
				/// </summary>
				public int ConditionBorder { get; set; }

				/// <summary>
				/// レコードを自動保存するか
				/// 0=しない、1=1時間ごと、2=1日ごと
				/// </summary>
				public int RecordAutoSaving { get; set; }

				/// <summary>
				/// システムの音量設定を利用するか
				/// </summary>
				public bool UseSystemVolume { get; set; }

				/// <summary>
				/// 前回終了時の音量
				/// </summary>
				public float LastVolume { get; set; }

				/// <summary>
				/// 前回終了時にミュート状態だったか
				/// </summary>
				public bool LastIsMute { get; set; }

				/// <summary>
				/// 威力表示の基準となる交戦形態
				/// </summary>
				public int PowerEngagementForm { get; set; }


				public ConfigControl() {
					ConditionBorder = 40;
					RecordAutoSaving = 1;
					UseSystemVolume = true;
					LastVolume = 0.8f;
					LastIsMute = false;
					PowerEngagementForm = 1;
				}
			}
			/// <summary>動作</summary>
			[DataMember]
			public ConfigControl Control { get; private set; }


			/// <summary>
			/// デバッグの設定を扱います。
			/// </summary>
			public class ConfigDebug : ConfigPartBase {

				/// <summary>
				/// デバッグメニューを有効にするか
				/// </summary>
				public bool EnableDebugMenu { get; set; }

				/// <summary>
				/// 起動時にAPIリストをロードするか
				/// </summary>
				public bool LoadAPIListOnLoad { get; set; }

				/// <summary>
				/// APIリストのパス
				/// </summary>
				public string APIListPath { get; set; }

				/// <summary>
				/// エラー発生時に警告音を鳴らすか
				/// </summary>
				public bool AlertOnError { get; set; }

				public ConfigDebug() {
					EnableDebugMenu = false;
					LoadAPIListOnLoad = false;
					APIListPath = "";
					AlertOnError = false;
				}
			}
			/// <summary>デバッグ</summary>
			[DataMember]
			public ConfigDebug Debug { get; private set; }


			/// <summary>
			/// 起動と終了の設定を扱います。
			/// </summary>
			public class ConfigLife : ConfigPartBase {

				/// <summary>
				/// 終了時に確認するか
				/// </summary>
				public bool ConfirmOnClosing { get; set; }

				/// <summary>
				/// 最前面に表示するか
				/// </summary>
				public bool TopMost { get; set; }

				/// <summary>
				/// レイアウトファイルのパス
				/// </summary>
				public string LayoutFilePath { get; set; }

				/// <summary>
				/// 更新情報を取得するか
				/// </summary>
				public bool CheckUpdateInformation { get; set; }

				/// <summary>
				/// ステータスバーを表示するか
				/// </summary>
				public bool ShowStatusBar { get; set; }

				/// <summary>
				/// 時計表示のフォーマット
				/// </summary>
				public int ClockFormat { get; set; }

				/// <summary>
				/// レイアウトをロックするか
				/// </summary>
				public bool LockLayout { get; set; }

				/// <summary>
				/// レイアウトロック中でもフロートウィンドウを閉じられるようにするか
				/// </summary>
				public bool CanCloseFloatWindowInLock { get; set; }

				public ConfigLife() {
					ConfirmOnClosing = true;
					TopMost = false;
					LayoutFilePath = @"Settings\WindowLayout.zip";
					CheckUpdateInformation = true;
					ShowStatusBar = true;
					ClockFormat = 0;
					LockLayout = false;
					CanCloseFloatWindowInLock = false;
				}
			}
			/// <summary>起動と終了</summary>
			[DataMember]
			public ConfigLife Life { get; private set; }


			/// <summary>
			/// [工廠]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormArsenal : ConfigPartBase {

				/// <summary>
				/// 艦名を表示するか
				/// </summary>
				public bool ShowShipName { get; set; }

				/// <summary>
				/// 完了時に点滅させるか
				/// </summary>
				public bool BlinkAtCompletion { get; set; }

				/// <summary>
				/// 艦名表示の最大幅
				/// </summary>
				public int MaxShipNameWidth { get; set; }

				public ConfigFormArsenal() {
					ShowShipName = true;
					BlinkAtCompletion = true;
					MaxShipNameWidth = 60;
				}
			}
			/// <summary>[工廠]ウィンドウ</summary>
			[DataMember]
			public ConfigFormArsenal FormArsenal { get; private set; }


			/// <summary>
			/// [入渠]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormDock : ConfigPartBase {

				/// <summary>
				/// 完了時に点滅させるか
				/// </summary>
				public bool BlinkAtCompletion { get; set; }

				/// <summary>
				/// 艦名表示の最大幅
				/// </summary>
				public int MaxShipNameWidth { get; set; }

				public ConfigFormDock() {
					BlinkAtCompletion = true;
					MaxShipNameWidth = 60;
				}
			}
			/// <summary>[入渠]ウィンドウ</summary>
			[DataMember]
			public ConfigFormDock FormDock { get; private set; }


			/// <summary>
			/// [司令部]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormHeadquarters : ConfigPartBase {

				/// <summary>
				/// 艦船/装備が満タンの時点滅するか
				/// </summary>
				public bool BlinkAtMaximum { get; set; }


				/// <summary>
				/// 項目の可視/不可視設定
				/// </summary>
				public SerializableList<bool> Visibility { get; set; }

				/// <summary>
				/// 任意アイテム表示のアイテムID
				/// </summary>
				public int DisplayUseItemID { get; set; }

				public ConfigFormHeadquarters() {
					BlinkAtMaximum = true;
					Visibility = null;		// フォーム側で設定します
					DisplayUseItemID = 68;	// 秋刀魚
				}
			}
			/// <summary>[司令部]ウィンドウ</summary>
			[DataMember]
			public ConfigFormHeadquarters FormHeadquarters { get; private set; }


			/// <summary>
			/// [艦隊]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormFleet : ConfigPartBase {

				/// <summary>
				/// 艦載機を表示するか
				/// </summary>
				public bool ShowAircraft { get; set; }

				/// <summary>
				/// 索敵式の計算方法
				/// </summary>
				public int SearchingAbilityMethod { get; set; }

				/// <summary>
				/// スクロール可能か
				/// </summary>
				public bool IsScrollable { get; set; }

				/// <summary>
				/// 艦名表示の幅を固定するか
				/// </summary>
				public bool FixShipNameWidth { get; set; }

				/// <summary>
				/// HPバーを短縮するか
				/// </summary>
				public bool ShortenHPBar { get; set; }

				/// <summary>
				/// next lv. を表示するか
				/// </summary>
				public bool ShowNextExp { get; set; }

				/// <summary>
				/// 装備の改修レベル・艦載機熟練度の表示フラグ
				/// </summary>
				public Window.Control.ShipStatusEquipment.LevelVisibilityFlag EquipmentLevelVisibility { get; set; }

				/// <summary>
				/// 艦載機熟練度を数字で表示するフラグ
				/// </summary>
				public bool ShowAircraftLevelByNumber { get; set; }

				/// <summary>
				/// 制空戦力の計算方法
				/// </summary>
				public int AirSuperiorityMethod { get; set; }

				/// <summary>
				/// 泊地修理タイマを表示するか
				/// </summary>
				public bool ShowAnchorageRepairingTimer { get; set; }

				/// <summary>
				/// タイマー完了時に点滅させるか
				/// </summary>
				public bool BlinkAtCompletion { get; set; }

				/// <summary>
				/// 疲労度アイコンを表示するか
				/// </summary>
				public bool ShowConditionIcon { get; set; }

				/// <summary>
				/// 艦名表示幅固定時の幅
				/// </summary>
				public int FixedShipNameWidth { get; set; }

				/// <summary>
				/// 制空戦力を範囲表示するか
				/// </summary>
				public bool ShowAirSuperiorityRange { get; set; }

				public ConfigFormFleet() {
					ShowAircraft = true;
					SearchingAbilityMethod = 4;
					IsScrollable = true;
					FixShipNameWidth = false;
					ShortenHPBar = false;
					ShowNextExp = true;
					EquipmentLevelVisibility = Window.Control.ShipStatusEquipment.LevelVisibilityFlag.Both;
					ShowAircraftLevelByNumber = false;
					AirSuperiorityMethod = 1;
					ShowAnchorageRepairingTimer = true;
					BlinkAtCompletion = true;
					ShowConditionIcon = true;
					FixedShipNameWidth = 40;
					ShowAirSuperiorityRange = false;
				}
			}
			/// <summary>[艦隊]ウィンドウ</summary>
			[DataMember]
			public ConfigFormFleet FormFleet { get; private set; }


			/// <summary>
			/// [任務]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormQuest : ConfigPartBase {

				/// <summary>
				/// 遂行中の任務のみ表示するか
				/// </summary>
				public bool ShowRunningOnly { get; set; }


				/// <summary>
				/// 単発を表示
				/// </summary>
				public bool ShowOnce { get; set; }

				/// <summary>
				/// デイリーを表示
				/// </summary>
				public bool ShowDaily { get; set; }

				/// <summary>
				/// ウィークリーを表示
				/// </summary>
				public bool ShowWeekly { get; set; }

				/// <summary>
				/// マンスリーを表示
				/// </summary>
				public bool ShowMonthly { get; set; }

				/// <summary>
				/// その他を表示
				/// </summary>
				public bool ShowOther { get; set; }

				/// <summary>
				/// 列の可視性
				/// </summary>
				public SerializableList<bool> ColumnFilter { get; set; }

				/// <summary>
				/// 列の幅
				/// </summary>
				public SerializableList<int> ColumnWidth { get; set; }

				/// <summary>
				/// どの行をソートしていたか
				/// </summary>
				public int SortParameter { get; set; }

				/// <summary>
				/// 進捗を自動保存するか
				/// 0 = しない、1 = 一時間ごと、2 = 一日ごと
				/// </summary>
				public int ProgressAutoSaving { get; set; }

				public bool AllowUserToSortRows { get; set; }

				public ConfigFormQuest() {
					ShowRunningOnly = false;
					ShowOnce = true;
					ShowDaily = true;
					ShowWeekly = true;
					ShowMonthly = true;
					ShowOther = true;
					ColumnFilter = null;		//実際の初期化は FormQuest で行う
					ColumnWidth = null;			//上に同じ
					SortParameter = 3 << 1 | 0;
					ProgressAutoSaving = 1;
					AllowUserToSortRows = true;
				}
			}
			/// <summary>[任務]ウィンドウ</summary>
			[DataMember]
			public ConfigFormQuest FormQuest { get; private set; }


			/// <summary>
			/// [艦船グループ]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormShipGroup : ConfigPartBase {

				/// <summary>
				/// 自動更新するか
				/// </summary>
				public bool AutoUpdate { get; set; }

				/// <summary>
				/// ステータスバーを表示するか
				/// </summary>
				public bool ShowStatusBar { get; set; }


				/// <summary>
				/// 艦名列のソート方法
				/// 0 = 図鑑番号順, 1 = あいうえお順
				/// </summary>
				public int ShipNameSortMethod { get; set; }

				public ConfigFormShipGroup() {
					AutoUpdate = true;
					ShowStatusBar = true;
					ShipNameSortMethod = 0;
				}
			}
			/// <summary>[艦船グループ]ウィンドウ</summary>
			[DataMember]
			public ConfigFormShipGroup FormShipGroup { get; private set; }


			/// <summary>
			/// [ブラウザ]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormBrowser : ConfigPartBase {

				/// <summary>
				/// ブラウザの拡大率 10-1000(%)
				/// </summary>
				public int ZoomRate { get; set; }

				/// <summary>
				/// ブラウザをウィンドウサイズに合わせる
				/// </summary>
				[DataMember]
				public bool ZoomFit { get; set; }

				/// <summary>
				/// ログインページのURL
				/// </summary>
				public string LogInPageURL { get; set; }

				/// <summary>
				/// ブラウザを有効にするか
				/// </summary>
				public bool IsEnabled { get; set; }

				/// <summary>
				/// スクリーンショットの保存先フォルダ
				/// </summary>
				public string ScreenShotPath { get; set; }

				/// <summary>
				/// スクリーンショットのフォーマット
				/// 1=jpeg, 2=png
				/// </summary>
				public int ScreenShotFormat { get; set; }

				/// <summary>
				/// 適用するスタイルシート
				/// </summary>
				public string StyleSheet { get; set; }

				/// <summary>
				/// スクロール可能かどうか
				/// </summary>
				public bool IsScrollable { get; set; }

				/// <summary>
				/// スタイルシートを適用するか
				/// </summary>
				public bool AppliesStyleSheet { get; set; }

				/// <summary>
				/// DMMによるページ更新ダイアログを非表示にするか
				/// </summary>
				public bool IsDMMreloadDialogDestroyable { get; set; }

				/// <summary>
				/// ツールメニューの配置
				/// </summary>
				public DockStyle ToolMenuDockStyle { get; set; }

				/// <summary>
				/// ツールメニューの可視性
				/// </summary>
				public bool IsToolMenuVisible { get; set; }

				/// <summary>
				/// 再読み込み時に確認ダイアログを入れるか
				/// </summary>
				public bool ConfirmAtRefresh { get; set; }

				/// <summary>
				/// flashのパラメータ指定 'wmode'
				/// </summary>
				public string FlashWMode { get; set; }

				/// <summary>
				/// flashのパラメータ指定 'quality'
				/// </summary>
				public string FlashQuality { get; set; }


				public ConfigFormBrowser() {
					ZoomRate = 100;
					ZoomFit = false;
					LogInPageURL = @"http://www.dmm.com/netgame_s/kancolle/";
					IsEnabled = true;
					ScreenShotPath = "ScreenShot";
					ScreenShotFormat = 2;
					StyleSheet = "\r\nbody {\r\n	margin:0;\r\n	overflow:hidden\r\n}\r\n\r\n#game_frame {\r\n	position:fixed;\r\n	left:50%;\r\n	top:-16px;\r\n	margin-left:-450px;\r\n	z-index:1\r\n}\r\n";
					IsScrollable = false;
					AppliesStyleSheet = true;
					IsDMMreloadDialogDestroyable = false;
					ToolMenuDockStyle = DockStyle.Top;
					IsToolMenuVisible = true;
					ConfirmAtRefresh = true;
					FlashWMode = "opaque";
					FlashQuality = "high";
				}
			}
			/// <summary>[ブラウザ]ウィンドウ</summary>
			[DataMember]
			public ConfigFormBrowser FormBrowser { get; private set; }


			/// <summary>
			/// [羅針盤]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormCompass : ConfigPartBase {

				/// <summary>
				/// 一度に表示する敵艦隊候補数
				/// </summary>
				public int CandidateDisplayCount { get; set; }

				/// <summary>
				/// スクロール可能か
				/// </summary>
				public bool IsScrollable { get; set; }

				public ConfigFormCompass() {
					CandidateDisplayCount = 4;
					IsScrollable = false;
				}
			}
			/// <summary>[羅針盤]ウィンドウ</summary>
			[DataMember]
			public ConfigFormCompass FormCompass { get; private set; }


			/// <summary>
			/// [JSON]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormJson : ConfigPartBase {

				/// <summary>
				/// 自動更新するか
				/// </summary>
				public bool AutoUpdate { get; set; }

				/// <summary>
				/// TreeView を更新するか
				/// </summary>
				public bool UpdatesTree { get; set; }

				/// <summary>
				/// 自動更新時のフィルタ
				/// </summary>
				public string AutoUpdateFilter { get; set; }


				public ConfigFormJson() {
					AutoUpdate = false;
					UpdatesTree = true;
					AutoUpdateFilter = "";
				}
			}
			/// <summary>[JSON]ウィンドウ</summary>
			[DataMember]
			public ConfigFormJson FormJson { get; private set; }


			/// <summary>
			/// [戦闘]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormBattle : ConfigPartBase {

				/// <summary>
				/// スクロール可能か
				/// </summary>
				public bool IsScrollable { get; set; }

				/// <summary>
				/// 戦闘中は表示を隠し、戦闘後のみ表示する
				/// </summary>
				public bool HideDuringBattle { get; set; }

				public ConfigFormBattle() {
					IsScrollable = false;
					HideDuringBattle = false;
				}
			}

			/// <summary>
			/// [戦闘]ウィンドウ
			/// </summary>
			[DataMember]
			public ConfigFormBattle FormBattle { get; private set; }


			/// <summary>
			/// 各[通知]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigNotifierBase : ConfigPartBase {

				public bool IsEnabled { get; set; }

				public bool IsSilenced { get; set; }

				public bool ShowsDialog { get; set; }

				public string ImagePath { get; set; }

				public bool DrawsImage { get; set; }

				public string SoundPath { get; set; }

				public bool PlaysSound { get; set; }

				public int SoundVolume { get; set; }

				public bool LoopsSound { get; set; }

				public bool DrawsMessage { get; set; }

				public int ClosingInterval { get; set; }

				public int AccelInterval { get; set; }

				public bool CloseOnMouseMove { get; set; }

				public Notifier.NotifierDialogClickFlags ClickFlag { get; set; }

				public Notifier.NotifierDialogAlignment Alignment { get; set; }

				public Point Location { get; set; }

				public bool HasFormBorder { get; set; }

				public bool TopMost { get; set; }

				public bool ShowWithActivation { get; set; }

				public SerializableColor ForeColor { get; set; }

				public SerializableColor BackColor { get; set; }


				public ConfigNotifierBase() {
					IsEnabled = true;
					IsSilenced = false;
					ShowsDialog = true;
					ImagePath = "";
					DrawsImage = false;
					SoundPath = "";
					PlaysSound = false;
					SoundVolume = 100;
					LoopsSound = false;
					DrawsMessage = true;
					ClosingInterval = 10000;
					AccelInterval = 0;
					CloseOnMouseMove = false;
					ClickFlag = Notifier.NotifierDialogClickFlags.Left;
					Alignment = Notifier.NotifierDialogAlignment.BottomRight;
					Location = new Point( 0, 0 );
					HasFormBorder = true;
					TopMost = true;
					ShowWithActivation = true;
					ForeColor = SystemColors.ControlText;
					BackColor = SystemColors.Control;
				}

			}


			/// <summary>
			/// [大破進撃通知]の設定を扱います。
			/// </summary>
			public class ConfigNotifierDamage : ConfigNotifierBase {

				public bool NotifiesBefore { get; set; }
				public bool NotifiesNow { get; set; }
				public bool NotifiesAfter { get; set; }
				public int LevelBorder { get; set; }
				public bool ContainsNotLockedShip { get; set; }
				public bool ContainsSafeShip { get; set; }
				public bool ContainsFlagship { get; set; }
				public bool NotifiesAtEndpoint { get; set; }
				public ConfigNotifierDamage()
					: base() {
					NotifiesBefore = false;
					NotifiesNow = true;
					NotifiesAfter = true;
					LevelBorder = 1;
					ContainsNotLockedShip = true;
					ContainsSafeShip = true;
					ContainsFlagship = true;
					NotifiesAtEndpoint = false;
				}
			}


			/// <summary>
			/// [泊地修理通知]の設定を扱います。
			/// </summary>
			public class ConfigNotifierAnchorageRepair : ConfigNotifierBase {

				public int NotificationLevel { get; set; }

				public ConfigNotifierAnchorageRepair()
					: base() {
					NotificationLevel = 2;
				}
			}


			/// <summary>[遠征帰投通知]</summary>
			[DataMember]
			public ConfigNotifierBase NotifierExpedition { get; private set; }

			/// <summary>[建造完了通知]</summary>
			[DataMember]
			public ConfigNotifierBase NotifierConstruction { get; private set; }

			/// <summary>[入渠完了通知]</summary>
			[DataMember]
			public ConfigNotifierBase NotifierRepair { get; private set; }

			/// <summary>[疲労回復通知]</summary>
			[DataMember]
			public ConfigNotifierBase NotifierCondition { get; private set; }

			/// <summary>[大破進撃通知]</summary>
			[DataMember]
			public ConfigNotifierDamage NotifierDamage { get; private set; }

			/// <summary>[泊地修理通知]</summary>
			[DataMember]
			public ConfigNotifierAnchorageRepair NotifierAnchorageRepair { get; private set; }



			/// <summary>
			/// SyncBGMPlayer の設定を扱います。
			/// </summary>
			public class ConfigBGMPlayer : ConfigPartBase {

				public bool Enabled { get; set; }
				public List<SyncBGMPlayer.SoundHandle> Handles { get; set; }
				public bool SyncBrowserMute { get; set; }

				public ConfigBGMPlayer()
					: base() {
					// 初期値定義は SyncBGMPlayer 内でも
					Enabled = false;
					Handles = new List<SyncBGMPlayer.SoundHandle>();
					foreach ( SyncBGMPlayer.SoundHandleID id in Enum.GetValues( typeof( SyncBGMPlayer.SoundHandleID ) ) )
						Handles.Add( new SyncBGMPlayer.SoundHandle( id ) );
					SyncBrowserMute = false;
				}
			}
			[DataMember]
			public ConfigBGMPlayer BGMPlayer { get; private set; }


			/// <summary>
			/// 編成画像出力の設定を扱います。
			/// </summary>
			public class ConfigFleetImageGenerator : ConfigPartBase {

				public FleetImageArgument Argument { get; set; }
				public int ImageType { get; set; }
				public int OutputType { get; set; }
				public bool OpenImageAfterOutput { get; set; }
				public string LastOutputPath { get; set; }
				public bool DisableOverwritePrompt { get; set; }
				public bool AutoSetFileNameToDate { get; set; }
				public bool SyncronizeTitleAndFileName { get; set; }

				public ConfigFleetImageGenerator()
					: base() {
					Argument = FleetImageArgument.GetDefaultInstance();
					ImageType = 0;
					OutputType = 0;
					OpenImageAfterOutput = false;
					LastOutputPath = "";
					DisableOverwritePrompt = false;
					AutoSetFileNameToDate = false;
					SyncronizeTitleAndFileName = false;
				}
			}
			[DataMember]
			public ConfigFleetImageGenerator FleetImageGenerator { get; private set; }



			public class ConfigWhitecap : ConfigPartBase {

				public bool ShowInTaskbar { get; set; }
				public bool TopMost { get; set; }
				public int BoardWidth { get; set; }
				public int BoardHeight { get; set; }
				public int ZoomRate { get; set; }
				public int UpdateInterval { get; set; }
				public int ColorTheme { get; set; }
				public int BirthRule { get; set; }
				public int AliveRule { get; set; }

				public ConfigWhitecap()
					: base() {
					ShowInTaskbar = true;
					TopMost = false;
					BoardWidth = 200;
					BoardHeight = 150;
					ZoomRate = 2;
					UpdateInterval = 100;
					ColorTheme = 0;
					BirthRule = ( 1 << 3 );
					AliveRule = ( 1 << 2 ) | ( 1 << 3 );
				}
			}
			[DataMember]
			public ConfigWhitecap Whitecap { get; private set; }



			[DataMember]
			public string Version {
				get { return SoftwareInformation.VersionEnglish; }
				set { }	//readonly
			}


			[DataMember]
			public string VersionUpdateTime { get; set; }


			public override void Initialize() {

				Connection = new ConfigConnection();
				UI = new ConfigUI();
				Log = new ConfigLog();
				Control = new ConfigControl();
				Debug = new ConfigDebug();
				Life = new ConfigLife();

				FormArsenal = new ConfigFormArsenal();
				FormDock = new ConfigFormDock();
				FormFleet = new ConfigFormFleet();
				FormHeadquarters = new ConfigFormHeadquarters();
				FormQuest = new ConfigFormQuest();
				FormShipGroup = new ConfigFormShipGroup();
				FormBattle = new ConfigFormBattle();
				FormBrowser = new ConfigFormBrowser();
				FormCompass = new ConfigFormCompass();
				FormJson = new ConfigFormJson();

				NotifierExpedition = new ConfigNotifierBase();
				NotifierConstruction = new ConfigNotifierBase();
				NotifierRepair = new ConfigNotifierBase();
				NotifierCondition = new ConfigNotifierBase();
				NotifierDamage = new ConfigNotifierDamage();
				NotifierAnchorageRepair = new ConfigNotifierAnchorageRepair();

				BGMPlayer = new ConfigBGMPlayer();
				FleetImageGenerator = new ConfigFleetImageGenerator();
				Whitecap = new ConfigWhitecap();

				VersionUpdateTime = DateTimeHelper.TimeToCSVString( SoftwareInformation.UpdateTime );

			}
		}
		private static ConfigurationData _config;

		public static ConfigurationData Config {
			get { return _config; }
		}



		private Configuration()
			: base() {

			_config = new ConfigurationData();
		}


		internal void OnConfigurationChanged() {
			ConfigurationChanged();
		}


		public void Load( Form mainForm ) {
			var temp = (ConfigurationData)_config.Load( SaveFileName );
			if ( temp != null ) {
				_config = temp;
				CheckUpdate( mainForm );
				OnConfigurationChanged();
			} else {
				MessageBox.Show( SoftwareInformation.SoftwareNameJapanese + " をご利用いただきありがとうございます。\r\n設定や使用方法については「ヘルプ」→「オンラインヘルプ」を参照してください。\r\nご使用の前に必ずご一読ください。",
					"初回起動メッセージ", MessageBoxButtons.OK, MessageBoxIcon.Information );


				// そのままだと正常に動作しなくなった(らしい)ので、ブラウザバージョンの書き込み
				Microsoft.Win32.RegistryKey reg = null;
				try {

					reg = Microsoft.Win32.Registry.CurrentUser.CreateSubKey( DialogConfiguration.RegistryPathMaster + DialogConfiguration.RegistryPathBrowserVersion );
					reg.SetValue( Window.FormBrowserHost.BrowserExeName, DialogConfiguration.DefaultBrowserVersion, Microsoft.Win32.RegistryValueKind.DWord );
					reg.Close();

					reg = Microsoft.Win32.Registry.CurrentUser.CreateSubKey( DialogConfiguration.RegistryPathMaster + DialogConfiguration.RegistryPathGPURendering );
					reg.SetValue( Window.FormBrowserHost.BrowserExeName, DialogConfiguration.DefaultGPURendering ? 1 : 0, Microsoft.Win32.RegistryValueKind.DWord );

					Utility.Logger.Add( 2, "ブラウザバージョンをレジストリに書き込みました。削除したい場合は「設定→サブウィンドウ→ブラウザ2→削除」を押してください。" );


				} catch ( Exception ex ) {
					Utility.ErrorReporter.SendErrorReport( ex, "ブラウザバージョンをレジストリに書き込めませんでした。" );

				} finally {
					if ( reg != null )
						reg.Close();
				}

			}
		}

		public void Save() {
			_config.Save( SaveFileName );
		}



		private void CheckUpdate( Form mainForm ) {
			DateTime dt = Config.VersionUpdateTime == null ? new DateTime( 0 ) : DateTimeHelper.CSVStringToTime( Config.VersionUpdateTime );

			// version 1.4.6 or earlier
			if ( dt <= DateTimeHelper.CSVStringToTime( "2015/08/27 21:00:00" ) ) {

				if ( MessageBox.Show(
					"バージョンアップが検出されました。\r\n古いレコードファイルを新しいフォーマットにコンバートします。\r\n(元のファイルは Record_Backup フォルダに残されます。)\r\nよろしいですか？\r\n(コンバートせずに続行した場合、読み込めなくなる可能性があります。)\r\n",
					"バージョンアップに伴う確認(～1.4.6)",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1 )
					 == DialogResult.Yes ) {

					try {

						Directory.CreateDirectory( "Record_Backup" );

						if ( File.Exists( RecordManager.Instance.MasterPath + "\\EnemyFleetRecord.csv" ) ) {
							File.Copy( RecordManager.Instance.MasterPath + "\\EnemyFleetRecord.csv", "Record_Backup\\EnemyFleetRecord.csv", false );

							//ヒャッハー！！
							using ( var writer = new StreamWriter( RecordManager.Instance.MasterPath + "\\EnemyFleetRecord.csv", false, Config.Log.FileEncoding ) ) {
								writer.WriteLine();
							}
						}


						if ( File.Exists( RecordManager.Instance.MasterPath + "\\ShipDropRecord.csv" ) ) {
							File.Copy( RecordManager.Instance.MasterPath + "\\ShipDropRecord.csv", "Record_Backup\\ShipDropRecord.csv", false );

							using ( var reader = new StreamReader( "Record_Backup\\ShipDropRecord.csv", Config.Log.FileEncoding ) ) {
								using ( var writer = new StreamWriter( RecordManager.Instance.MasterPath + "\\ShipDropRecord.csv", false, Config.Log.FileEncoding ) ) {

									while ( !reader.EndOfStream ) {
										string line = reader.ReadLine();
										var elem = line.Split( ",".ToCharArray() ).ToList();

										elem.Insert( 6, Constants.GetDifficulty( -1 ) );	//difficulty
										elem[8] = "0";		//EnemyFleetID


										writer.WriteLine( string.Join( ",", elem ) );
									}
								}
							}
						}


						if ( File.Exists( RecordManager.Instance.MasterPath + "\\ShipParameterRecord.csv" ) ) {
							File.Copy( RecordManager.Instance.MasterPath + "\\ShipParameterRecord.csv", "Record_Backup\\ShipParameterRecord.csv", false );

							using ( var reader = new StreamReader( "Record_Backup\\ShipParameterRecord.csv", Config.Log.FileEncoding ) ) {
								using ( var writer = new StreamWriter( RecordManager.Instance.MasterPath + "\\ShipParameterRecord.csv", false, Config.Log.FileEncoding ) ) {

									while ( !reader.EndOfStream ) {
										string line = reader.ReadLine();
										var elem = line.Split( ",".ToCharArray() ).ToList();

										elem.InsertRange( 2, Enumerable.Repeat( "0", 10 ) );
										elem.InsertRange( 21, Enumerable.Repeat( "0", 3 ) );
										elem.InsertRange( 29, Enumerable.Repeat( "null", 5 ) );
										elem.Insert( 34, "null" );

										writer.WriteLine( string.Join( ",", elem ) );
									}
								}
							}
						}



						if ( File.Exists( RecordManager.Instance.MasterPath + "\\ConstructionRecord.csv" ) ) {
							File.Copy( RecordManager.Instance.MasterPath + "\\ConstructionRecord.csv", "Record_Backup\\ConstructionRecord.csv", false );

							using ( var reader = new StreamReader( "Record_Backup\\ConstructionRecord.csv", Config.Log.FileEncoding ) ) {
								using ( var writer = new StreamWriter( RecordManager.Instance.MasterPath + "\\ConstructionRecord.csv", false, Config.Log.FileEncoding ) ) {

									string[] prev = null;

									while ( !reader.EndOfStream ) {
										string line = reader.ReadLine();
										var elem = line.Split( ",".ToCharArray() );

										// 以前のバージョンのバグによる無効行・重複行の削除
										if ( prev != null ) {
											if ( elem[0] == "0" || (	//invalid id
												elem[0] == prev[0] &&	//id
												elem[1] == prev[1] &&	//name
												elem[3] == prev[3] &&	//fuel
												elem[4] == prev[4] &&	//ammo
												elem[5] == prev[5] &&	//steel
												elem[6] == prev[6] &&	//bauxite
												elem[7] == prev[7] &&	//dev.mat
												elem[8] == prev[8] &&	//islarge
												elem[9] == prev[9]		//emptydock
												) ) {

												prev = elem;
												continue;
											}
										}

										writer.WriteLine( string.Join( ",", elem ) );
										prev = elem;
									}
								}
							}
						}


						// 読み書き方式が変わったので念のため
						if ( File.Exists( RecordManager.Instance.MasterPath + "\\DevelopmentRecord.csv" ) ) {
							File.Copy( RecordManager.Instance.MasterPath + "\\DevelopmentRecord.csv", "Record_Backup\\DevelopmentRecord.csv", false );
						}


					} catch ( Exception ex ) {

						Utility.ErrorReporter.SendErrorReport( ex, "バージョンアップに伴うレコードのコンバートに失敗しました。" );

						if ( MessageBox.Show( "コンバートに失敗しました。\r\n" + ex.Message + "\r\n起動処理を続行しますか？\r\n(データが破壊される可能性があります)\r\n",
							"エラー", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2 )
							== DialogResult.No )
							Environment.Exit( -1 );

					}
				}


			}

			// version 1.5.0 or earlier
			if ( dt <= DateTimeHelper.CSVStringToTime( "2015/09/04 21:00:00" ) ) {

				if ( MessageBox.Show(
					"バージョンアップが検出されました。\r\n艦船グループデータの互換性がなくなったため、当該データを初期化します。\r\n(古いファイルは Settings_Backup フォルダに退避されます。)\r\nよろしいですか？\r\n(初期化せずに続行した場合、エラーが発生します。)\r\n",
					"バージョンアップに伴う確認(～1.5.0)",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1 )
					 == DialogResult.Yes ) {

					try {

						Directory.CreateDirectory( "Settings_Backup" );
						File.Move( "Settings\\ShipGroups.xml", "Settings_Backup\\ShipGroups.xml" );

					} catch ( Exception ex ) {

						Utility.ErrorReporter.SendErrorReport( ex, "バージョンアップに伴うグループデータの削除に失敗しました。" );

						// エラーが出るだけなのでシャットダウンは不要
						MessageBox.Show( "削除に失敗しました。\r\n" + ex.Message,
							"エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );

					}
				}
			}


			// version 1.6.3 or earlier
			if ( dt <= DateTimeHelper.CSVStringToTime( "2015/10/03 22:00:00" ) ) {

				if ( MessageBox.Show(
					"バージョンアップが検出されました。\r\nアイテムドロップ仕様の変更に伴い、艦船ドロップレコードのフォーマットを変更します。\r\n(古いファイルは Record_Backup フォルダに退避されます。)\r\nよろしいですか？\r\n(初期化せずに続行した場合、エラーが発生します。)\r\n",
					"バージョンアップに伴う確認(～1.6.3)",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1 )
					 == DialogResult.Yes ) {

					try {

						if ( File.Exists( RecordManager.Instance.MasterPath + "\\ShipDropRecord.csv" ) ) {

							Directory.CreateDirectory( "Record_Backup" );

							if ( File.Exists( "Record_Backup\\ShipDropRecord.csv" ) ) {
								var result = MessageBox.Show( "バックアップ先に既にファイルが存在します。\r\n上書きしますか？\r\n(キャンセルした場合、コンバート処理を中止します。)",
									"バックアップの上書き確認", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );

								switch ( result ) {
									case DialogResult.Yes:
										File.Copy( RecordManager.Instance.MasterPath + "\\ShipDropRecord.csv", "Record_Backup\\ShipDropRecord.csv", true );
										break;
									case DialogResult.No:
										break;
									case DialogResult.Cancel:
										throw new InvalidOperationException( "バックアップ処理がキャンセルされました。" );
								}
							} else {
								File.Copy( RecordManager.Instance.MasterPath + "\\ShipDropRecord.csv", "Record_Backup\\ShipDropRecord.csv", false );
							}


							using ( var reader = new StreamReader( "Record_Backup\\ShipDropRecord.csv", Config.Log.FileEncoding ) ) {
								using ( var writer = new StreamWriter( RecordManager.Instance.MasterPath + "\\ShipDropRecord.csv", false, Config.Log.FileEncoding ) ) {

									while ( !reader.EndOfStream ) {
										string line = reader.ReadLine();
										var elem = line.Split( ",".ToCharArray() ).ToList();

										// 旧IDの変換
										int oldID;
										if ( !int.TryParse( elem[0], out oldID ) )
											oldID = -1;

										if ( oldID > 2000 ) {
											elem[0] = "-1";
											elem[1] = "(なし)";
											elem.InsertRange( 2, new string[] { "-1", "(なし)", ( oldID - 2000 ).ToString(), "???" } );

										} else if ( oldID > 1000 ) {
											elem[0] = "-1";
											elem[1] = "(なし)";
											elem.InsertRange( 2, new string[] { ( oldID - 1000 ).ToString(), "???", "-1", "(なし)" } );

										} else {
											elem.InsertRange( 2, new string[] { "-1", "(なし)", "-1", "(なし)" } );

										}


										writer.WriteLine( string.Join( ",", elem ) );
									}
								}
							}
						}


					} catch ( Exception ex ) {

						Utility.ErrorReporter.SendErrorReport( ex, "バージョンアップに伴うレコードのコンバートに失敗しました。" );

						if ( MessageBox.Show( "コンバートに失敗しました。\r\n" + ex.Message + "\r\n起動処理を続行しますか？\r\n(データが破壊される可能性があります)\r\n",
							"エラー", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2 )
							== DialogResult.No )
							Environment.Exit( -1 );

					}
				}
			}


			// version 2.5.5.1 or earlier
			if ( dt <= DateTimeHelper.CSVStringToTime( "2017/03/30 00:00:00" ) ) {

				if ( MessageBox.Show( "艦これ本体の仕様変更に伴い、レコードデータを変換する必要があります。\r\n変換を実行しますか？\r\n(変換しない場合、動作に問題が発生する可能性があります。)", "バージョンアップに伴う確認(～2.5.5.1)",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1 ) == DialogResult.Yes ) {

					// 敵編成レコードの敵編成ID再計算とドロップレコードの敵編成ID振りなおし
					try {
						var enemyFleetRecord = new EnemyFleetRecord();
						var convertPair = new Dictionary<uint, uint>();

						enemyFleetRecord.Load( RecordManager.Instance.MasterPath );

						foreach ( var record in enemyFleetRecord.Record.Values ) {
							uint key = record.FleetID;
							record.FleetMember = record.FleetMember.Select( id => 500 < id && id < 1000 ? id + 1000 : id ).ToArray();
							convertPair.Add( key, record.FleetID );
						}

						enemyFleetRecord.Save( RecordManager.Instance.MasterPath );

						var shipDropRecord = new ShipDropRecord();
						shipDropRecord.Load( RecordManager.Instance.MasterPath );

						foreach ( var record in shipDropRecord.Record ) {
							if ( convertPair.ContainsKey( record.EnemyFleetID ) )
								record.EnemyFleetID = convertPair[record.EnemyFleetID];
						}

						shipDropRecord.Save( RecordManager.Instance.MasterPath );

					} catch ( Exception ex ) {
						ErrorReporter.SendErrorReport( ex, "CheckUpdate: ドロップレコードのID振りなおしに失敗しました。" );
					}


					// パラメータレコードの移動と破損データのダウンロード
					try {

						var currentRecord = new ShipParameterRecord();
						currentRecord.Load( RecordManager.Instance.MasterPath );

						foreach ( var record in currentRecord.Record.Values ) {
							if ( 500 < record.ShipID && record.ShipID <= 1000 ) {
								record.ShipID += 1000;
							}
						}

						string defaultRecordPath = Path.Combine( Path.GetTempPath(), Path.GetRandomFileName() );
						while ( Directory.Exists( defaultRecordPath ) )
							defaultRecordPath = Path.Combine( Path.GetTempPath(), Path.GetRandomFileName() );

						Directory.CreateDirectory( defaultRecordPath );

						ElectronicObserver.Resource.ResourceManager.CopyDocumentFromArchive( "Record/" + currentRecord.FileName, Path.Combine( defaultRecordPath, currentRecord.FileName ) );

						var defaultRecord = new ShipParameterRecord();
						defaultRecord.Load( defaultRecordPath );
						var changed = new List<int>();

						foreach ( var pair in defaultRecord.Record.Keys.GroupJoin( currentRecord.Record.Keys, i => i, i => i, ( id, list ) => new { id, list } ) ) {
							if ( defaultRecord[pair.id].HPMin > 0 && ( pair.list == null || defaultRecord[pair.id].SaveLine() != currentRecord[pair.id].SaveLine() ) )
								changed.Add( pair.id );
					}

						foreach ( var id in changed ) {
							if ( currentRecord[id] == null )
								currentRecord.Update( new ShipParameterRecord.ShipParameterElement() );
							currentRecord[id].LoadLine( defaultRecord.Record[id].SaveLine() );
				}

						currentRecord.Save( RecordManager.Instance.MasterPath );

						Directory.Delete( defaultRecordPath, true );


					} catch ( Exception ex ) {
						ErrorReporter.SendErrorReport( ex, "パラメータレコードの再編に失敗しました。" );
					}

				}


			}




			Config.VersionUpdateTime = DateTimeHelper.TimeToCSVString( SoftwareInformation.UpdateTime );
		}

	}


}
