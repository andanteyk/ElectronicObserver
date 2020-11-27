using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Utility.Storage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;

namespace ElectronicObserver.Utility
{


	public sealed class Configuration
	{


		private static readonly Configuration instance = new Configuration();

		public static Configuration Instance => instance;


		private const string SaveFileName = @"Settings\Configuration.xml";


		public delegate void ConfigurationChangedEventHandler();
		public event ConfigurationChangedEventHandler ConfigurationChanged = delegate { };


		[DataContract(Name = "Configuration")]
		public sealed class ConfigurationData : DataStorage
		{

			public class ConfigPartBase
			{
				//reserved
			}


			/// <summary>
			/// 通信の設定を扱います。
			/// </summary>
			public class ConfigConnection : ConfigPartBase
			{

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

				public string JoinApi { get; set; }


				public ConfigConnection()
				{

					Port = 40620;
					SaveReceivedData = false;
					SaveDataPath = @"KCAPI";
					SaveRequest = false;
					SaveResponse = true;
					SaveOtherFile = false;
					ApplyVersion = false;
					RegisterAsSystemProxy = false;
					UseUpstreamProxy = false;
					UpstreamProxyPort = 0;
					UpstreamProxyAddress = "127.0.0.1";
					UseSystemProxy = false;
					DownstreamProxy = "";
					JoinApi = "";
				}

			}
			/// <summary>通信</summary>
			[DataMember]
			public ConfigConnection Connection { get; private set; }


			public class ConfigUI : ConfigPartBase
			{

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
				public bool BarColorMorphing
				{
					get { return _barColorMorphing; }
					set
					{
						_barColorMorphing = value;

						if (!_barColorMorphing)
							BarColorScheme = new List<SerializableColor>(DefaultBarColorScheme[0]);
						else
							BarColorScheme = new List<SerializableColor>(DefaultBarColorScheme[1]);
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

				/// <summary>
				/// 固定レイアウト(フォントに依存しないレイアウト)を利用するか
				/// </summary>
				public bool IsLayoutFixed;


				public ConfigUI()
				{
					MainFont = new Font("Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel);
					SubFont = new Font("Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel);
					BarColorMorphing = false;
					IsLayoutFixed = true;
				}
			}
			/// <summary>UI</summary>
			[DataMember]
			public ConfigUI UI { get; private set; }


			/// <summary>
			/// ログの設定を扱います。
			/// </summary>
			public class ConfigLog : ConfigPartBase
			{

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
				public Encoding FileEncoding
				{
					get
					{
						switch (FileEncodingID)
						{
							case 0:
								return new System.Text.UTF8Encoding(false);
							case 1:
								return new System.Text.UTF8Encoding(true);
							case 2:
								return new System.Text.UnicodeEncoding(false, false);
							case 3:
								return new System.Text.UnicodeEncoding(false, true);
							case 4:
								return Encoding.GetEncoding(932);
							default:
								return new System.Text.UTF8Encoding(false);

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

				/// <summary>
				/// ログを即時保存するか
				/// </summary>
				public bool SaveLogImmediately { get; set; }


				public ConfigLog()
				{
					LogLevel = 2;
					SaveLogFlag = true;
					SaveErrorReport = true;
					FileEncodingID = 4;
					ShowSpoiler = true;
					PlayTime = 0;
					PlayTimeIgnoreInterval = 10 * 60;
					SaveBattleLog = false;
					SaveLogImmediately = false;
				}

			}
			/// <summary>ログ</summary>
			[DataMember]
			public ConfigLog Log { get; private set; }


			/// <summary>
			/// 動作の設定を扱います。
			/// </summary>
			public class ConfigControl : ConfigPartBase
			{

				/// <summary>
				/// 疲労度ボーダー
				/// </summary>
				public int ConditionBorder { get; set; }

				/// <summary>
				/// レコードを自動保存するか
				/// 0=しない、1=1時間ごと、2=1日ごと, 3=即時
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

				/// <summary>
				/// 出撃札がない艦娘が出撃したときに警告ダイアログを表示するか
				/// </summary>
				public bool ShowSallyAreaAlertDialog { get; set; }

				/// <summary>
				/// 必要経験値計算：出撃当たりの経験値
				/// </summary>
				public int ExpCheckerExpUnit { get; set; }

				/// <summary>
				/// 遠征に失敗する可能性があるとき警告ダイアログを表示するか
				/// </summary>
				public bool ShowExpeditionAlertDialog { get; set; }

				public ConfigControl()
				{
					ConditionBorder = 40;
					RecordAutoSaving = 1;
					UseSystemVolume = true;
					LastVolume = 0.8f;
					LastIsMute = false;
					PowerEngagementForm = 1;
					ShowSallyAreaAlertDialog = true;
					ExpCheckerExpUnit = 2268;
					ShowExpeditionAlertDialog = true;
				}
			}
			/// <summary>動作</summary>
			[DataMember]
			public ConfigControl Control { get; private set; }


			/// <summary>
			/// デバッグの設定を扱います。
			/// </summary>
			public class ConfigDebug : ConfigPartBase
			{

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

				public ConfigDebug()
				{
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
			public class ConfigLife : ConfigPartBase
			{

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

				public ConfigLife()
				{
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
			public class ConfigFormArsenal : ConfigPartBase
			{

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

				public ConfigFormArsenal()
				{
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
			public class ConfigFormDock : ConfigPartBase
			{

				/// <summary>
				/// 完了時に点滅させるか
				/// </summary>
				public bool BlinkAtCompletion { get; set; }

				/// <summary>
				/// 艦名表示の最大幅
				/// </summary>
				public int MaxShipNameWidth { get; set; }

				public ConfigFormDock()
				{
					BlinkAtCompletion = true;
					MaxShipNameWidth = 64;
				}
			}
			/// <summary>[入渠]ウィンドウ</summary>
			[DataMember]
			public ConfigFormDock FormDock { get; private set; }


			/// <summary>
			/// [司令部]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormHeadquarters : ConfigPartBase
			{

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

				public ConfigFormHeadquarters()
				{
					BlinkAtMaximum = true;
					Visibility = null;      // フォーム側で設定します
					DisplayUseItemID = 68;  // 秋刀魚
				}
			}
			/// <summary>[司令部]ウィンドウ</summary>
			[DataMember]
			public ConfigFormHeadquarters FormHeadquarters { get; private set; }


			/// <summary>
			/// [艦隊]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormFleet : ConfigPartBase
			{

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

				/// <summary>
				/// 泊地修理によるHP回復を表示に反映するか
				/// </summary>
				public bool ReflectAnchorageRepairHealing { get; set; }

				/// <summary>
				/// 遠征艦隊が母港にいるとき強調表示
				/// </summary>
				public bool EmphasizesSubFleetInPort { get; set; }

				/// <summary>
				/// 大破時に点滅させる
				/// </summary>
				public bool BlinkAtDamaged { get; set; }

				/// <summary>
				/// 艦隊状態の表示方法
				/// </summary>
				public int FleetStateDisplayMode { get; set; }

				/// <summary>
				/// 出撃海域によって色分けするか
				/// </summary>
				public bool AppliesSallyAreaColor { get; set; }

				/// <summary>
				/// 出撃海域による色分けのテーブル
				/// </summary>
				public List<SerializableColor> SallyAreaColorScheme { get; set; }

				[IgnoreDataMember]
				internal readonly List<SerializableColor> DefaultSallyAreaColorScheme = new List<SerializableColor>()
				{
					SerializableColor.UIntToColor(0xfff0f0f0),
					SerializableColor.UIntToColor(0xffffdddd),
					SerializableColor.UIntToColor(0xffddffdd),
					SerializableColor.UIntToColor(0xffddddff),
					SerializableColor.UIntToColor(0xffffffcc),
					SerializableColor.UIntToColor(0xffccffff),
					SerializableColor.UIntToColor(0xffffccff),
					SerializableColor.UIntToColor(0xffffffff),
					SerializableColor.UIntToColor(0xffffead5),
					SerializableColor.UIntToColor(0xffe7c8c8),
					SerializableColor.UIntToColor(0xffe7e7b8),
					SerializableColor.UIntToColor(0xffc8e7c8),
					SerializableColor.UIntToColor(0xffb8e7e7),
					SerializableColor.UIntToColor(0xffc8c8e7),
					SerializableColor.UIntToColor(0xffe7b8e7),
				};

				public ConfigFormFleet()
				{
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
					ReflectAnchorageRepairHealing = true;
					EmphasizesSubFleetInPort = false;
					BlinkAtDamaged = true;
					FleetStateDisplayMode = 2;
					AppliesSallyAreaColor = false;
					SallyAreaColorScheme = DefaultSallyAreaColorScheme.ToList();
				}
			}
			/// <summary>[艦隊]ウィンドウ</summary>
			[DataMember]
			public ConfigFormFleet FormFleet { get; private set; }


			/// <summary>
			/// [任務]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormQuest : ConfigPartBase
			{

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

				public ConfigFormQuest()
				{
					ShowRunningOnly = false;
					ShowOnce = true;
					ShowDaily = true;
					ShowWeekly = true;
					ShowMonthly = true;
					ShowOther = true;
					ColumnFilter = null;        //実際の初期化は FormQuest で行う
					ColumnWidth = null;         //上に同じ
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
			public class ConfigFormShipGroup : ConfigPartBase
			{

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

				public ConfigFormShipGroup()
				{
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
			public class ConfigFormBrowser : ConfigPartBase
			{

				/// <summary>
				/// ブラウザの拡大率 10-1000(%)
				/// </summary>
				public double ZoomRate { get; set; }

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
				/// スクリーンショットの保存モード
				/// 1=ファイル, 2=クリップボード, 3=両方
				/// </summary>
				public int ScreenShotSaveMode { get; set; }

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
				/// Twitter の画像圧縮を回避するか
				/// </summary>
				public bool AvoidTwitterDeterioration { get; set; }

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
				/// ハードウェアアクセラレーションを有効にするか
				/// </summary>
				public bool HardwareAccelerationEnabled { get; set; }

				/// <summary>
				/// 描画バッファを保持するか
				/// </summary>
				public bool PreserveDrawingBuffer { get; set; }

				/// <summary>
				/// カラープロファイルを sRGB に固定するか
				/// </summary>
				public bool ForceColorProfile { get; set; }

				/// <summary>
				/// ブラウザのログを保存するか
				/// </summary>
				public bool SavesBrowserLog { get; set; }

				public ConfigFormBrowser()
				{
					ZoomRate = 1;
					ZoomFit = false;
					LogInPageURL = @"http://www.dmm.com/netgame_s/kancolle/";
					IsEnabled = true;
					ScreenShotPath = "ScreenShot";
					ScreenShotFormat = 2;
					ScreenShotSaveMode = 1;
					StyleSheet = "\r\nbody {\r\n	margin:0;\r\n	overflow:hidden\r\n}\r\n\r\n#game_frame {\r\n	position:fixed;\r\n	left:50%;\r\n	top:-16px;\r\n	margin-left:-450px;\r\n	z-index:1\r\n}\r\n";
					IsScrollable = false;
					AppliesStyleSheet = true;
					IsDMMreloadDialogDestroyable = false;
					AvoidTwitterDeterioration = true;
					ToolMenuDockStyle = DockStyle.Top;
					IsToolMenuVisible = true;
					ConfirmAtRefresh = true;
					HardwareAccelerationEnabled = true;
					PreserveDrawingBuffer = true;
					ForceColorProfile = false;
					SavesBrowserLog = false;
				}
			}
			/// <summary>[ブラウザ]ウィンドウ</summary>
			[DataMember]
			public ConfigFormBrowser FormBrowser { get; private set; }


			/// <summary>
			/// [羅針盤]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormCompass : ConfigPartBase
			{

				/// <summary>
				/// 一度に表示する敵艦隊候補数
				/// </summary>
				public int CandidateDisplayCount { get; set; }

				/// <summary>
				/// スクロール可能か
				/// </summary>
				public bool IsScrollable { get; set; }

				/// <summary>
				/// 艦名表示の最大幅
				/// </summary>
				public int MaxShipNameWidth { get; set; }


				public ConfigFormCompass()
				{
					CandidateDisplayCount = 4;
					IsScrollable = false;
					MaxShipNameWidth = 60;
				}
			}
			/// <summary>[羅針盤]ウィンドウ</summary>
			[DataMember]
			public ConfigFormCompass FormCompass { get; private set; }


			/// <summary>
			/// [JSON]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormJson : ConfigPartBase
			{

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


				public ConfigFormJson()
				{
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
			public class ConfigFormBattle : ConfigPartBase
			{

				/// <summary>
				/// スクロール可能か
				/// </summary>
				public bool IsScrollable { get; set; }

				/// <summary>
				/// 戦闘中は表示を隠し、戦闘後のみ表示する
				/// </summary>
				public bool HideDuringBattle { get; set; }

				/// <summary>
				/// HP バーを表示するか
				/// </summary>
				public bool ShowHPBar { get; set; }

				/// <summary>
				/// HP バーに艦種を表示するか
				/// </summary>
				public bool ShowShipTypeInHPBar { get; set; }

				/// <summary>
				/// 7隻目を主力艦隊と同じ行に表示するか
				/// </summary>
				public bool Display7thAsSingleLine { get; set; }


				public ConfigFormBattle()
				{
					IsScrollable = false;
					HideDuringBattle = false;
					ShowHPBar = true;
					ShowShipTypeInHPBar = false;
					Display7thAsSingleLine = true;
				}
			}

			/// <summary>
			/// [戦闘]ウィンドウ
			/// </summary>
			[DataMember]
			public ConfigFormBattle FormBattle { get; private set; }


			/// <summary>
			/// [基地航空隊]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigFormBaseAirCorps : ConfigPartBase
			{

				/// <summary>
				/// イベント海域のもののみ表示するか
				/// </summary>
				public bool ShowEventMapOnly { get; set; }

				public ConfigFormBaseAirCorps()
				{
					ShowEventMapOnly = false;
				}
			}

			/// <summary>
			/// [基地航空隊]ウィンドウ
			/// </summary>
			[DataMember]
			public ConfigFormBaseAirCorps FormBaseAirCorps { get; private set; }



			/// <summary>
			/// 各[通知]ウィンドウの設定を扱います。
			/// </summary>
			public class ConfigNotifierBase : ConfigPartBase
			{

				public bool IsEnabled { get; set; }

				public bool IsSilenced { get; set; }

				public string JoinApi { get; set; }

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


				public ConfigNotifierBase()
				{
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
					Location = new Point(0, 0);
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
			public class ConfigNotifierDamage : ConfigNotifierBase
			{

				public bool NotifiesBefore { get; set; }
				public bool NotifiesNow { get; set; }
				public bool NotifiesAfter { get; set; }
				public int LevelBorder { get; set; }
				public bool ContainsNotLockedShip { get; set; }
				public bool ContainsSafeShip { get; set; }
				public bool ContainsFlagship { get; set; }
				public bool NotifiesAtEndpoint { get; set; }
				public ConfigNotifierDamage()
					: base()
				{
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
			public class ConfigNotifierAnchorageRepair : ConfigNotifierBase
			{

				public int NotificationLevel { get; set; }

				public ConfigNotifierAnchorageRepair()
					: base()
				{
					NotificationLevel = 2;
				}
			}

			/// <summary>
			/// [基地航空隊通知]の設定を扱います。
			/// </summary>
			public class ConfigNotifierBaseAirCorps : ConfigNotifierBase
			{
				/// <summary>
				/// 未補給時に通知する
				/// </summary>
				public bool NotifiesNotSupplied { get; set; }

				/// <summary>
				/// 疲労時に通知する
				/// </summary>
				public bool NotifiesTired { get; set; }

				/// <summary>
				/// 編成されていないときに通知する
				/// </summary>
				public bool NotifiesNotOrganized { get; set; }


				/// <summary>
				/// 待機のとき通知する
				/// </summary>
				public bool NotifiesStandby { get; set; }

				/// <summary>
				/// 退避の時通知する
				/// </summary>
				public bool NotifiesRetreat { get; set; }

				/// <summary>
				/// 休息の時通知する
				/// </summary>
				public bool NotifiesRest { get; set; }


				/// <summary>
				/// 通常海域で通知する
				/// </summary>
				public bool NotifiesNormalMap { get; set; }

				/// <summary>
				/// イベント海域で通知する
				/// </summary>
				public bool NotifiesEventMap { get; set; }


				/// <summary>
				/// 基地枠の配置転換完了時に通知する
				/// </summary>
				public bool NotifiesSquadronRelocation { get; set; }

				/// <summary>
				/// 装備の配置転換完了時に通知する
				/// </summary>
				public bool NotifiesEquipmentRelocation { get; set; }


				public ConfigNotifierBaseAirCorps()
					: base()
				{
					NotifiesNotSupplied = true;
					NotifiesTired = false;
					NotifiesNotOrganized = false;

					NotifiesStandby = false;
					NotifiesRetreat = true;
					NotifiesRest = true;

					NotifiesNormalMap = false;
					NotifiesEventMap = true;

					NotifiesSquadronRelocation = true;
					NotifiesEquipmentRelocation = false;
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

			/// <summary>[基地航空隊通知]</summary>
			[DataMember]
			public ConfigNotifierBaseAirCorps NotifierBaseAirCorps { get; private set; }


			/// <summary>
			/// SyncBGMPlayer の設定を扱います。
			/// </summary>
			public class ConfigBGMPlayer : ConfigPartBase
			{

				public bool Enabled { get; set; }
				public List<SyncBGMPlayer.SoundHandle> Handles { get; set; }
				public bool SyncBrowserMute { get; set; }

				public ConfigBGMPlayer()
					: base()
				{
					// 初期値定義は SyncBGMPlayer 内でも
					Enabled = false;
					Handles = new List<SyncBGMPlayer.SoundHandle>();
					foreach (SyncBGMPlayer.SoundHandleID id in Enum.GetValues(typeof(SyncBGMPlayer.SoundHandleID)))
						Handles.Add(new SyncBGMPlayer.SoundHandle(id));
					SyncBrowserMute = false;
				}
			}
			[DataMember]
			public ConfigBGMPlayer BGMPlayer { get; private set; }


			/// <summary>
			/// 編成画像出力の設定を扱います。
			/// </summary>
			public class ConfigFleetImageGenerator : ConfigPartBase
			{

				public FleetImageArgument Argument { get; set; }
				public int ImageType { get; set; }
				public int OutputType { get; set; }
				public bool OpenImageAfterOutput { get; set; }
				public string LastOutputPath { get; set; }
				public bool DisableOverwritePrompt { get; set; }
				public bool AutoSetFileNameToDate { get; set; }
				public bool SyncronizeTitleAndFileName { get; set; }

				public ConfigFleetImageGenerator()
					: base()
				{
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



			public class ConfigWhitecap : ConfigPartBase
			{

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
					: base()
				{
					ShowInTaskbar = true;
					TopMost = false;
					BoardWidth = 200;
					BoardHeight = 150;
					ZoomRate = 2;
					UpdateInterval = 100;
					ColorTheme = 0;
					BirthRule = (1 << 3);
					AliveRule = (1 << 2) | (1 << 3);
				}
			}
			[DataMember]
			public ConfigWhitecap Whitecap { get; private set; }



			[DataMember]
			public string Version
			{
				get { return SoftwareInformation.VersionEnglish; }
				set { } //readonly
			}


			[DataMember]
			public string VersionUpdateTime { get; set; }



			public ConfigurationData()
			{
				Initialize();
			}

			public override void Initialize()
			{

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
				FormBaseAirCorps = new ConfigFormBaseAirCorps();

				NotifierExpedition = new ConfigNotifierBase();
				NotifierConstruction = new ConfigNotifierBase();
				NotifierRepair = new ConfigNotifierBase();
				NotifierCondition = new ConfigNotifierBase();
				NotifierDamage = new ConfigNotifierDamage();
				NotifierAnchorageRepair = new ConfigNotifierAnchorageRepair();
				NotifierBaseAirCorps = new ConfigNotifierBaseAirCorps();

				BGMPlayer = new ConfigBGMPlayer();
				FleetImageGenerator = new ConfigFleetImageGenerator();
				Whitecap = new ConfigWhitecap();

				VersionUpdateTime = DateTimeHelper.TimeToCSVString(SoftwareInformation.UpdateTime);

			}
		}
		private static ConfigurationData _config;

		public static ConfigurationData Config => _config;



		private Configuration()
			: base()
		{

			_config = new ConfigurationData();
		}


		internal void OnConfigurationChanged()
		{
			ConfigurationChanged();
		}


		public void Load(Form mainForm)
		{
			var temp = (ConfigurationData)_config.Load(SaveFileName);
			if (temp != null)
			{
				_config = temp;
				CheckUpdate(mainForm);
				OnConfigurationChanged();
			}
			else
			{
				MessageBox.Show(SoftwareInformation.SoftwareNameJapanese + " をご利用いただきありがとうございます。\r\n設定や使用方法については「ヘルプ」→「オンラインヘルプ」を参照してください。\r\nご使用の前に必ずご一読ください。",
					"初回起動メッセージ", MessageBoxButtons.OK, MessageBoxIcon.Information);

			}
		}

		public void Save()
		{
			_config.Save(SaveFileName);
		}



		private void CheckUpdate(Form mainForm)
		{
			DateTime dt = Config.VersionUpdateTime == null ? new DateTime(0) : DateTimeHelper.CSVStringToTime(Config.VersionUpdateTime);


			// version 2.5.5.1 or earlier
			if (dt <= DateTimeHelper.CSVStringToTime("2017/03/30 00:00:00"))
			{

				if (MessageBox.Show("艦これ本体の仕様変更に伴い、レコードデータを変換する必要があります。\r\n変換を実行しますか？\r\n(変換しない場合、動作に問題が発生する可能性があります。)", "バージョンアップに伴う確認(～2.5.5.1)",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
				{

					// 敵編成レコードの敵編成ID再計算とドロップレコードの敵編成ID振りなおし
					// ~ver. 2.8.2 更新で内部処理が変わったので要確認
					try
					{
						var enemyFleetRecord = new EnemyFleetRecord();
						var convertPair = new Dictionary<ulong, ulong>();

						enemyFleetRecord.Load(RecordManager.Instance.MasterPath);

						foreach (var record in enemyFleetRecord.Record.Values)
						{
							ulong key = record.FleetID;
							for (int i = 0; i < record.FleetMember.Length; i++)
							{
								int id = record.FleetMember[i];
								record.FleetMember[i] = 500 < id && id < 1000 ? id + 1000 : id;
							}
							convertPair.Add(key, record.FleetID);
						}

						enemyFleetRecord.SaveAll(RecordManager.Instance.MasterPath);

						var shipDropRecord = new ShipDropRecord();
						shipDropRecord.Load(RecordManager.Instance.MasterPath);

						foreach (var record in shipDropRecord.Record)
						{
							if (convertPair.ContainsKey(record.EnemyFleetID))
								record.EnemyFleetID = convertPair[record.EnemyFleetID];
						}

						shipDropRecord.SaveAll(RecordManager.Instance.MasterPath);

					}
					catch (Exception ex)
					{
						ErrorReporter.SendErrorReport(ex, "CheckUpdate: ドロップレコードのID振りなおしに失敗しました。");
					}


					// パラメータレコードの移動と破損データのダウンロード
					try
					{

						var currentRecord = new ShipParameterRecord();
						currentRecord.Load(RecordManager.Instance.MasterPath);

						foreach (var record in currentRecord.Record.Values)
						{
							if (500 < record.ShipID && record.ShipID <= 1000)
							{
								record.ShipID += 1000;
							}
						}

						string defaultRecordPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
						while (Directory.Exists(defaultRecordPath))
							defaultRecordPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

						Directory.CreateDirectory(defaultRecordPath);

						Resource.ResourceManager.CopyDocumentFromArchive("Record/" + currentRecord.FileName, Path.Combine(defaultRecordPath, currentRecord.FileName));

						var defaultRecord = new ShipParameterRecord();
						defaultRecord.Load(defaultRecordPath);
						var changed = new List<int>();

						foreach (var pair in defaultRecord.Record.Keys.GroupJoin(currentRecord.Record.Keys, i => i, i => i, (id, list) => new { id, list }))
						{
							if (defaultRecord[pair.id].HPMin > 0 && (pair.list == null || defaultRecord[pair.id].SaveLine() != currentRecord[pair.id].SaveLine()))
								changed.Add(pair.id);
						}

						foreach (var id in changed)
						{
							if (currentRecord[id] == null)
								currentRecord.Update(new ShipParameterRecord.ShipParameterElement());
							currentRecord[id].LoadLine(defaultRecord.Record[id].SaveLine());
						}

						currentRecord.SaveAll(RecordManager.Instance.MasterPath);

						Directory.Delete(defaultRecordPath, true);


					}
					catch (Exception ex)
					{
						ErrorReporter.SendErrorReport(ex, "パラメータレコードの再編に失敗しました。");
					}

				}


			}


			// version 2.6.2 or earlier
			if (dt <= DateTimeHelper.CSVStringToTime("2017/05/07 23:00:00"))
			{

				// 開発レコードを重複記録してしまう不具合があったため、重複行の削除を行う

				try
				{

					var dev = new DevelopmentRecord();
					string path = RecordManager.Instance.MasterPath + "\\" + dev.FileName;


					string backupPath = RecordManager.Instance.MasterPath + "\\Backup_" + DateTimeHelper.GetTimeStamp();
					Directory.CreateDirectory(backupPath);
					File.Copy(path, backupPath + "\\" + dev.FileName);


					if (File.Exists(path))
					{

						var lines = new List<string>();
						using (StreamReader sr = new StreamReader(path, Utility.Configuration.Config.Log.FileEncoding))
						{
							sr.ReadLine();      // skip header row
							while (!sr.EndOfStream)
								lines.Add(sr.ReadLine());
						}

						int beforeCount = lines.Count;
						lines = lines.Distinct().ToList();
						int afterCount = lines.Count;

						using (StreamWriter sw = new StreamWriter(path, false, Utility.Configuration.Config.Log.FileEncoding))
						{
							sw.WriteLine(dev.RecordHeader);
							foreach (var line in lines)
							{
								sw.WriteLine(line);
							}
						}

						Utility.Logger.Add(2, "<= ver. 2.6.2 開発レコード重複不具合対応: 正常に完了しました。 " + (beforeCount - afterCount) + " 件の重複を削除しました。");

					}

				}
				catch (Exception ex)
				{
					ErrorReporter.SendErrorReport(ex, "<= ver. 2.6.2 開発レコード重複不具合対応: 失敗しました。");
				}
			}


			// version 2.8.2 or earlier
			if (dt <= DateTimeHelper.CSVStringToTime("2017/10/17 20:30:00"))
				Update282_ConvertRecord();

			if (dt <= DateTimeHelper.CSVStringToTime("2018/02/11 23:00:00"))
				Update307_ConvertRecord();

			if (dt <= DateTimeHelper.CSVStringToTime("2018/08/17 23:00:00"))
				Update312_RemoveObsoleteRegistry();

			if (dt <= DateTimeHelper.CSVStringToTime("2020/06/07 23:00:00"))
				Update460_AddSallyAreaColorScheme();


			Config.VersionUpdateTime = DateTimeHelper.TimeToCSVString(SoftwareInformation.UpdateTime);
		}


		private void Update282_ConvertRecord()
		{
			// 敵編成レコード：ハッシュ計算が変わり、項目が増えたため引き継ぎ不能、バックアップを取っておく
			// ドロップ記録レコード：〃　編成IDを 0x0 で初期化する


			// for retry
			do
			{
				try
				{
					var fleet = new EnemyFleetRecord();
					string fleetPath = RecordManager.Instance.MasterPath + "\\" + fleet.FileName;

					var drop = new ShipDropRecord();
					string dropPath = RecordManager.Instance.MasterPath + "\\" + drop.FileName;


					string backupDirectoryPath = RecordManager.Instance.MasterPath + "\\Backup_" + DateTimeHelper.GetTimeStamp();


					Directory.CreateDirectory(backupDirectoryPath);


					// enemy fleet record
					if (File.Exists(fleetPath))
					{
						bool isNewVersion;
						try
						{
							using (var reader = new StreamReader(fleetPath, Utility.Configuration.Config.Log.FileEncoding))
								isNewVersion = reader.ReadLine() == fleet.RecordHeader;
						}
						catch (Exception)
						{
							isNewVersion = false;
						}


						if (!isNewVersion)
						{
							File.Move(fleetPath, backupDirectoryPath + "\\" + fleet.FileName);
						}
						else
						{
							Utility.Logger.Add(1, "~2.8.2 レコード変換処理：敵艦隊レコードは既に新しいフォーマットです。処理をスキップします。");
						}
					}


					// copy default record
					if (!File.Exists(fleetPath))
						Resource.ResourceManager.CopyDocumentFromArchive("Record/" + fleet.FileName, fleetPath);


					// drop record
					if (File.Exists(dropPath))
					{
						bool isNewVersion;
						try
						{
							using (var reader = new StreamReader(dropPath, Utility.Configuration.Config.Log.FileEncoding))
							{
								reader.ReadLine();
								isNewVersion = reader.ReadLine().Split(",".ToCharArray())[12].Length == 16;
							}
						}
						catch (Exception)
						{
							isNewVersion = false;
						}


						if (!isNewVersion)
						{
							File.Copy(dropPath, backupDirectoryPath + "\\" + drop.FileName);


							drop.Load(RecordManager.Instance.MasterPath);
							foreach (var r in drop.Record)
								r.EnemyFleetID = 0;

							drop.SaveAll(RecordManager.Instance.MasterPath);
						}
						else
						{
							Utility.Logger.Add(1, "~2.8.2 レコード変換処理：ドロップレコードは既に新しいフォーマットです。処理をスキップします。");
						}
					}


					// 何もバックアップしなくてよかった時
					if (!Directory.EnumerateFiles(backupDirectoryPath).Any())
						Directory.Delete(backupDirectoryPath);


					Utility.Logger.Add(2, "~2.8.2 レコード変換処理：正常に完了しました。");

				}
				catch (Exception ex)
				{
					Utility.ErrorReporter.SendErrorReport(ex, "~2.8.2 レコード変換処理：失敗しました。");

					if (MessageBox.Show($"互換性維持のためのレコード変換処理中にエラーが発生しました。\r\n\r\n{ex.Message}\r\n\r\n再試行しますか？\r\n（「いいえ」を選択した場合、一部の記録データが消失する可能性があります。）",
						"~2.8.2 レコード変換処理：" + ex.GetType().Name, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
						== DialogResult.Yes)
						continue;
					else
						break;
				}
			} while (false);


		}


		/// <summary>
		/// 難易度設定がずれたため、ハッシュ値が変更された
		/// それに対する対応・移行処理を行う
		/// </summary>
		private void Update307_ConvertRecord()
		{
			try
			{

				var fleet = new EnemyFleetRecord();
				string fleetPath = RecordManager.Instance.MasterPath + "\\" + fleet.FileName;

				var drop = new ShipDropRecord();
				string dropPath = RecordManager.Instance.MasterPath + "\\" + drop.FileName;


				string backupDirectoryPath = RecordManager.Instance.MasterPath + "\\Backup_" + DateTimeHelper.GetTimeStamp();



				Directory.CreateDirectory(backupDirectoryPath);
				File.Copy(fleetPath, backupDirectoryPath + "\\" + fleet.FileName);
				File.Copy(dropPath, backupDirectoryPath + "\\" + drop.FileName);


				var hashremap = new Dictionary<ulong, ulong>();

				using (var reader = new StreamReader(RecordManager.Instance.MasterPath + "\\" + fleet.FileName, Config.Log.FileEncoding))
				{
					reader.ReadLine();      // skip header

					while (!reader.EndOfStream)
					{
						string line = reader.ReadLine();

						var data = new EnemyFleetRecord.EnemyFleetElement(line);

						ulong oldhash = Convert.ToUInt64(line.Substring(0, 16), 16);
						ulong newhash = data.FleetID;

						if (oldhash != newhash)
						{
							hashremap.Add(oldhash, newhash);
							int diff = data.Difficulty;

							switch (diff)
							{
								case 2: diff = 1; break;        // 1(丁)が誤って "丙" と記録されている → ロードすると 2 になるので、1 に再設定
								case 3: diff = 2; break;
								case 4: diff = 3; break;
								case -1: diff = 4; break;       // 4(甲)は "不明" → ロードすると -1 になるので、 4 に
							}

							data = new EnemyFleetRecord.EnemyFleetElement(data.FleetName, data.MapAreaID, data.MapInfoID, data.CellID, diff, data.Formation, data.FleetMember, data.FleetMemberLevel, data.ExpShip);
						}

						// 敵連合艦隊データのフォーマットが誤っていた([6] == -1になって、[7]から随伴艦隊が始まる)ので、捨てなければならない :(
						bool rotten = data.FleetMember[6] == -1 && data.FleetMember[7] != -1;

						if (!rotten)
							fleet.Record.Add(data.FleetID, data);

					}
				}

				fleet.SaveAll(RecordManager.Instance.MasterPath);


				drop.Load(RecordManager.Instance.MasterPath);

				foreach (var d in drop.Record)
				{
					if (hashremap.ContainsKey(d.EnemyFleetID))
					{
						d.EnemyFleetID = hashremap[d.EnemyFleetID];

						int diff = d.Difficulty;
						switch (diff)
						{
							case 2: diff = 1; break;
							case 3: diff = 2; break;
							case 4: diff = 3; break;
							case -1: diff = 4; break;
						}

						d.Difficulty = diff;
					}
				}

				drop.SaveAll(RecordManager.Instance.MasterPath);


				Utility.Logger.Add(2, "<= ver. 3.0.7 難易度変更に伴うレコードファイルの修正: 正常に完了しました。");

			}
			catch (Exception ex)
			{
				ErrorReporter.SendErrorReport(ex, "<= ver. 3.0.7 難易度変更に伴うレコードファイルの修正: 失敗しました。");
			}

		}


		private void Update312_RemoveObsoleteRegistry()
		{
			// ;)
			Config.FormBrowser.ZoomRate = 1;


			string RegistryPathMaster = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\";
			string RegistryPathBrowserVersion = @"FEATURE_BROWSER_EMULATION\";
			string RegistryPathGPURendering = @"FEATURE_GPU_RENDERING\";


			try
			{
				using (var reg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegistryPathMaster + RegistryPathBrowserVersion, true))
					reg.DeleteValue(Window.FormBrowserHost.BrowserExeName);

				using (var reg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegistryPathMaster + RegistryPathGPURendering, true))
					reg.DeleteValue(Window.FormBrowserHost.BrowserExeName);

			}
			catch (Exception ex)
			{
				Utility.ErrorReporter.SendErrorReport(ex, "<= ver. 3.1.2 移行処理: 古いレジストリ値の削除に失敗しました。");
			}
		}

		private void Update460_AddSallyAreaColorScheme()
		{
			if (Config.FormFleet.SallyAreaColorScheme.SequenceEqual(Config.FormFleet.DefaultSallyAreaColorScheme.Take(8)))
			{
				Config.FormFleet.SallyAreaColorScheme = Config.FormFleet.DefaultSallyAreaColorScheme.ToList();
				Utility.Logger.Add(1, "<= ver. 4.6.0 移行処理: カラースキームの追加が完了しました。");
			}
		}
	}


}
