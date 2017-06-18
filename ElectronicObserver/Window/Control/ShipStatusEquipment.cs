using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ElectronicObserver.Data;
using System.Drawing.Design;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Support;

namespace ElectronicObserver.Window.Control {

	public partial class ShipStatusEquipment : UserControl {

		private class SlotItem {

			/// <summary>
			/// 装備ID
			/// </summary>
			public int EquipmentID { get; set; }

			/// <summary>
			/// 装備インスタンス
			/// </summary>
			public EquipmentDataMaster Equipment {
				get {
					return KCDatabase.Instance.MasterEquipments[EquipmentID];
				}
			}

			/// <summary>
			/// 装備アイコンID
			/// </summary>
			public int EquipmentIconID {
				get {
					var eq = KCDatabase.Instance.MasterEquipments[EquipmentID];
					if ( eq != null )
						return eq.IconType;
					else
						return -1;
				}
			}

			/// <summary>
			/// 搭載機数
			/// </summary>
			public int AircraftCurrent { get; set; }

			/// <summary>
			/// 最大搭載機数
			/// </summary>
			public int AircraftMax { get; set; }


			/// <summary>
			/// 改修レベル
			/// </summary>
			public int Level { get; set; }

			/// <summary>
			/// 艦載機熟練度
			/// </summary>
			public int AircraftLevel { get; set; }


			public SlotItem() {
				EquipmentID = -1;
				AircraftCurrent = AircraftMax = 0;
				Level = AircraftLevel = 0;
			}
		}



		#region Properties


		private SlotItem[] SlotList;
		private int _slotSize;
		private int SlotSize {
			get { return _slotSize; }
			set {
				_slotSize = value;
				PropertyChanged();
			}
		}

		private bool _onMouse;


		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Font ), "Meiryo UI, 10px" )]
		public override Font Font {
			get { return base.Font; }
			set {
				base.Font = value;
				if ( LayoutParam != null )
					LayoutParam.ResetLayout();
				PropertyChanged();
			}
		}

		private Color _aircraftColorDisabled;
		/// <summary>
		/// 艦載機非搭載スロットの文字色
		/// </summary>
		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Color ), "170, 170, 170" )]
		[Description( "艦載機非搭載スロットの文字色を指定します。" )]
		public Color AircraftColorDisabled {
			get { return _aircraftColorDisabled; }
			set {
				_aircraftColorDisabled = value;
				PropertyChanged();
			}
		}

		private Color _aircraftColorLost;
		/// <summary>
		/// 艦載機全滅スロットの文字色
		/// </summary>
		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Color ), "255, 0, 255" )]
		[Description( "艦載機全滅スロットの文字色を指定します。" )]
		public Color AircraftColorLost {
			get { return _aircraftColorLost; }
			set {
				_aircraftColorLost = value;
				PropertyChanged();
			}
		}

		private Color _aircraftColorDamaged;
		/// <summary>
		/// 艦載機被撃墜スロットの文字色
		/// </summary>
		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Color ), "255, 0, 0" )]
		[Description( "艦載機被撃墜スロットの文字色を指定します。" )]
		public Color AircraftColorDamaged {
			get { return _aircraftColorDamaged; }
			set {
				_aircraftColorDamaged = value;
				PropertyChanged();
			}
		}

		private Color _aircraftColorFull;
		/// <summary>
		/// 艦載機満載スロットの文字色
		/// </summary>
		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Color ), "0, 0, 0" )]
		[Description( "艦載機満載スロットの文字色を指定します。" )]
		public Color AircraftColorFull {
			get { return _aircraftColorFull; }
			set {
				_aircraftColorFull = value;
				PropertyChanged();
			}
		}


		private Color _equipmentLevelColor;
		/// <summary>
		/// 改修レベルの色
		/// </summary>
		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Color ), "0, 102, 102" )]
		[Description( "改修レベルの文字色を指定します。" )]
		public Color EquipmentLevelColor {
			get { return _equipmentLevelColor; }
			set {
				_equipmentLevelColor = value;
				PropertyChanged();
			}
		}

		private Color _aircraftLevelColorLow;
		/// <summary>
		/// 艦載機熟練度の色 ( Lv. 1 ~ Lv. 3 )
		/// </summary>
		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Color ), "102, 153, 238" )]
		[Description( "艦載機熟練度の文字色( Lv. 1 ~ 3 )を指定します。" )]
		public Color AircraftLevelColorLow {
			get { return _aircraftLevelColorLow; }
			set {
				_aircraftLevelColorLow = value;
				PropertyChanged();
			}
		}

		private Color _aircraftLevelColorHigh;
		/// <summary>
		/// 艦載機熟練度の色 ( Lv. 4 ~ Lv. 7 )
		/// </summary>
		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Color ), "255, 170, 0" )]
		[Description( "艦載機熟練度の文字色( Lv. 4 ~ 7 )を指定します。" )]
		public Color AircraftLevelColorHigh {
			get { return _aircraftLevelColorHigh; }
			set {
				_aircraftLevelColorHigh = value;
				PropertyChanged();
			}
		}

		private Color _invalidSlotColor;
		private Brush _invalidSlotBrush;
		/// <summary>
		/// 不正スロットの背景色
		/// </summary>
		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Color ), "64, 255, 0, 0" )]
		[Description( "不正スロットの文字色を指定します。" )]
		public Color InvalidSlotColor {
			get { return _invalidSlotColor; }
			set {
				_invalidSlotColor = value;

				if ( _invalidSlotBrush != null )
					_invalidSlotBrush.Dispose();
				_invalidSlotBrush = new SolidBrush( _invalidSlotColor );

				PropertyChanged();
			}
		}


		private bool _showAircraft;
		/// <summary>
		/// 艦載機搭載数を表示するか
		/// </summary>
		[Browsable( true ), Category( "Behavior" ), DefaultValue( true )]
		[Description( "艦載機搭載数を表示するかを指定します。" )]
		public bool ShowAircraft {
			get { return _showAircraft; }
			set {
				_showAircraft = value;
				PropertyChanged();
			}
		}



		/// <summary>
		/// 装備改修レベル・艦載機熟練度の表示フラグ
		/// </summary>
		public enum LevelVisibilityFlag {

			/// <summary> 非表示 </summary>
			Invisible,

			/// <summary> 改修レベルのみ </summary>
			LevelOnly,

			/// <summary> 艦載機熟練度のみ </summary>
			AircraftLevelOnly,

			/// <summary> 改修レベル優先 </summary>
			LevelPriority,

			/// <summary> 艦載機熟練度優先 </summary>
			AircraftLevelPriority,

			/// <summary> 両方表示 </summary>
			Both,

			/// <summary> 両方表示(艦載機熟練度はアイコンにオーバーレイする) </summary>
			AircraftLevelOverlay,
		}

		private LevelVisibilityFlag _levelVisibility;
		/// <summary>
		/// 装備改修レベル・艦載機熟練度の表示フラグ
		/// </summary>
		[Browsable( true ), Category( "Behavior" ), DefaultValue( LevelVisibilityFlag.Both )]
		[Description( "装備改修レベル・艦載機熟練度の表示方法を指定します。" )]
		public LevelVisibilityFlag LevelVisibility {
			get { return _levelVisibility; }
			set {
				_levelVisibility = value;
				PropertyChanged();
			}
		}

		private bool _showAircraftLevelByNumber;
		/// <summary>
		/// 艦載機熟練度を数字で表示するか
		/// </summary>
		[Browsable( true ), Category( "Behavior" ), DefaultValue( false )]
		[Description( "艦載機熟練度を記号ではなく数値で表示するかを指定します。" )]
		public bool ShowAircraftLevelByNumber {
			get { return _showAircraftLevelByNumber; }
			set {
				_showAircraftLevelByNumber = value;
				PropertyChanged();
			}
		}

		private int _slotMargin;
		/// <summary>
		/// スロット間の空きスペース
		/// </summary>
		[Browsable( true ), Category( "Appearance" ), DefaultValue( 3 )]
		[Description( "スロット間の空きスペースを指定します。" )]
		public int SlotMargin {
			get { return _slotMargin; }
			set {
				_slotMargin = value;
				PropertyChanged();
			}
		}



		private Brush _overlayBrush = new SolidBrush( Color.FromArgb( 0xC0, 0xF0, 0xF0, 0xF0 ) );


		[System.Diagnostics.DebuggerDisplay( "[{PreferredSize.Width}, {PreferredSize.Height}]" )]
		private class LayoutParameter {
			private ShipStatusEquipment Parent;

			/// <summary> 数字2桁分のサイズキャッシュ </summary>
			public Size Digit2Size { get; private set; }

			/// <summary> 装備画像のサイズキャッシュ </summary>
			public Size ImageSize { get; private set; }

			/// <summary> 艦載機数・改修レベル等の表示エリアのサイズキャッシュ </summary>
			public Size InfoAreaSize { get; private set; }

			/// <summary> 1スロットあたりのサイズキャッシュ (マージン含む) </summary>
			public Size SlotUnitSize { get; private set; }

			/// <summary> コントロールの最適サイズのキャッシュ </summary>
			public Size PreferredSize { get; private set; }


			public bool IsAvailable { get; private set; }


			public LayoutParameter( ShipStatusEquipment parent ) {
				Parent = parent;
				ResetLayout();
			}


			public void ResetLayout() {
				Digit2Size = Size.Empty;
				ImageSize = Size.Empty;
				InfoAreaSize = Size.Empty;
				SlotUnitSize = Size.Empty;
				PreferredSize = Size.Empty;
				IsAvailable = false;
			}

			public void UpdateParameters( Graphics g, Size proposedSize, Font font ) {

				bool isGraphicsSpecified = g != null;

				if ( !IsAvailable ) {
					if ( !isGraphicsSpecified )
						g = Parent.CreateGraphics();
					Digit2Size = TextRenderer.MeasureText( g, "88", font, new Size( int.MaxValue, int.MaxValue ), GetBaseTextFormat() | TextFormatFlags.Top | TextFormatFlags.Right );
				}

				ImageList eqimages = ResourceManager.Instance.Equipments;


				int imageZoomRate = (int)Math.Max( Math.Ceiling( Math.Min( Digit2Size.Height * 2.0, ( proposedSize.Height > 0 ? proposedSize.Height : int.MaxValue ) ) / eqimages.ImageSize.Height - 1 ), 1 );
				ImageSize = new Size( eqimages.ImageSize.Width * imageZoomRate, eqimages.ImageSize.Height * imageZoomRate );

				// 情報エリア (機数とか熟練度とか) のサイズ
				InfoAreaSize = new Size(
					Math.Max( Digit2Size.Width, ImageSize.Width ),
					Math.Min( Digit2Size.Height + Math.Max( Digit2Size.Height, ImageSize.Height / 2 ), proposedSize.Height ) );


				// スロット1つ当たりのサイズ(右の余白含む)
				SlotUnitSize = new Size( ImageSize.Width + Parent.SlotMargin, ImageSize.Height );
				if ( Parent.ShowAircraft || Parent.LevelVisibility != LevelVisibilityFlag.Invisible ) {
					SlotUnitSize = new Size( SlotUnitSize.Width + InfoAreaSize.Width, Math.Max( SlotUnitSize.Height, InfoAreaSize.Height ) );
				}

				PreferredSize = new Size( SlotUnitSize.Width * Parent.SlotSize, SlotUnitSize.Height );


				if ( !IsAvailable && !isGraphicsSpecified )
					g.Dispose();

				IsAvailable = true;
			}
		}
		private LayoutParameter LayoutParam;

		#endregion


		private bool IsRefreshSuspended { get; set; }

		public void SuspendUpdate() {
			IsRefreshSuspended = true;
			SuspendLayout();
		}
		public void ResumeUpdate() {
			ResumeLayout();
			if ( IsRefreshSuspended ) {
				IsRefreshSuspended = false;
				PropertyChanged();
			}
		}



		public ShipStatusEquipment() {
			IsRefreshSuspended = true;
			InitializeComponent();

			SlotList = new SlotItem[6];
			for ( int i = 0; i < SlotList.Length; i++ ) {
				SlotList[i] = new SlotItem();
			}
			_slotSize = 0;

			_onMouse = false;

			base.Font = new Font( "Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel );

			_aircraftColorDisabled = Color.FromArgb( 0xAA, 0xAA, 0xAA );
			_aircraftColorLost = Color.FromArgb( 0xFF, 0x00, 0xFF );
			_aircraftColorDamaged = Color.FromArgb( 0xFF, 0x00, 0x00 );
			_aircraftColorFull = Color.FromArgb( 0x00, 0x00, 0x00 );

			_equipmentLevelColor = Color.FromArgb( 0x00, 0x66, 0x66 );
			_aircraftLevelColorLow = Color.FromArgb( 0x66, 0x99, 0xEE );
			_aircraftLevelColorHigh = Color.FromArgb( 0xFF, 0xAA, 0x00 );

			_invalidSlotColor = Color.FromArgb( 0x40, 0xFF, 0x00, 0x00 );
			_invalidSlotBrush = new SolidBrush( _invalidSlotColor );

			_showAircraft = true;

			_levelVisibility = LevelVisibilityFlag.Both;
			_showAircraftLevelByNumber = false;

			_slotMargin = 3;

			LayoutParam = new LayoutParameter( this );

			Disposed += ShipStatusEquipment_Disposed;

			IsRefreshSuspended = false;
		}

		/// <summary>
		/// スロット情報を設定します。主に味方艦用です。
		/// </summary>
		/// <param name="ship">当該艦船。</param>
		public void SetSlotList( ShipData ship ) {

			int slotCount = Math.Max( ship.SlotSize + ( ship.IsExpansionSlotAvailable ? 1 : 0 ), 4 );


			if ( SlotList.Length != slotCount ) {
				SlotList = new SlotItem[slotCount];
				for ( int i = 0; i < SlotList.Length; i++ ) {
					SlotList[i] = new SlotItem();
				}
			}

			for ( int i = 0; i < Math.Min( slotCount, 5 ); i++ ) {
				var eq = ship.SlotInstance[i];
				SlotList[i].EquipmentID = eq != null ? eq.EquipmentID : -1;
				SlotList[i].AircraftCurrent = ship.Aircraft[i];
				SlotList[i].AircraftMax = ship.MasterShip.Aircraft[i];
				SlotList[i].Level = eq != null ? eq.Level : 0;
				SlotList[i].AircraftLevel = eq != null ? eq.AircraftLevel : 0;
			}

			if ( ship.IsExpansionSlotAvailable ) {
				var eq = ship.ExpansionSlotInstance;
				SlotList[ship.SlotSize].EquipmentID = eq != null ? eq.EquipmentID : -1;
				SlotList[ship.SlotSize].AircraftCurrent =
				SlotList[ship.SlotSize].AircraftMax = 0;
				SlotList[ship.SlotSize].Level = eq != null ? eq.Level : 0;
				SlotList[ship.SlotSize].AircraftLevel = eq != null ? eq.AircraftLevel : 0;
			}


			_slotSize = ship.SlotSize + ( ship.IsExpansionSlotAvailable ? 1 : 0 );

			PropertyChanged();
		}

		/// <summary>
		/// スロット情報を設定します。主に敵艦用です。
		/// </summary>
		/// <param name="ship">当該艦船。</param>
		public void SetSlotList( ShipDataMaster ship ) {


			if ( SlotList.Length != ship.Aircraft.Count ) {
				SlotList = new SlotItem[ship.Aircraft.Count];
				for ( int i = 0; i < SlotList.Length; i++ ) {
					SlotList[i] = new SlotItem();
				}
			}

			for ( int i = 0; i < SlotList.Length; i++ ) {
				SlotList[i].EquipmentID = ship.DefaultSlot == null ? -1 : ( i < ship.DefaultSlot.Count ? ship.DefaultSlot[i] : -1 );
				SlotList[i].AircraftCurrent =
				SlotList[i].AircraftMax = ship.Aircraft[i];
				SlotList[i].Level =
				SlotList[i].AircraftLevel = 0;
			}

			_slotSize = ship.SlotSize;

			PropertyChanged();
		}

		/// <summary>
		/// スロット情報を設定します。主に演習の敵艦用です。
		/// </summary>
		/// <param name="shipID">艦船ID。</param>
		/// <param name="slot">装備スロット。</param>
		public void SetSlotList( int shipID, int[] slot ) {

			ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];

			int slotLength = slot != null ? slot.Length : 0;

			if ( SlotList.Length != slotLength ) {
				SlotList = new SlotItem[slotLength];
				for ( int i = 0; i < SlotList.Length; i++ ) {
					SlotList[i] = new SlotItem();
				}
			}

			for ( int i = 0; i < SlotList.Length; i++ ) {
				SlotList[i].EquipmentID = slot[i];
				SlotList[i].AircraftCurrent = ship.Aircraft[i];
				SlotList[i].AircraftMax = ship.Aircraft[i];
				SlotList[i].Level =
				SlotList[i].AircraftLevel = 0;
			}

			_slotSize = ship != null ? ship.SlotSize : 0;

			PropertyChanged();
		}

		public void SetSlotList( BaseAirCorpsData corps ) {

			int slotLength = corps != null ? corps.Squadrons.Count() : 0;

			if ( SlotList.Length != slotLength ) {
				SlotList = new SlotItem[slotLength];
				for ( int i = 0; i < SlotList.Length; i++ ) {
					SlotList[i] = new SlotItem();
				}
			}

			for ( int i = 0; i < SlotList.Length; i++ ) {
				var squadron = corps[i + 1];
				var eq = squadron.EquipmentInstance;

				switch ( squadron.State ) {
					case 0:		// 未配属
					case 2:		// 配置転換中
					default:
						SlotList[i].EquipmentID = -1;
						SlotList[i].AircraftCurrent =
						SlotList[i].AircraftMax =
						SlotList[i].Level =
						SlotList[i].AircraftLevel = 0;
						break;
					case 1:		// 配属済み
						if ( eq == null )
							goto case 0;
						SlotList[i].EquipmentID = eq.EquipmentID;
						SlotList[i].AircraftCurrent = squadron.AircraftCurrent;
						SlotList[i].AircraftMax = squadron.AircraftMax;
						SlotList[i].Level = eq.Level;
						SlotList[i].AircraftLevel = eq.AircraftLevel;
						break;
				}

			}

			_slotSize = slotLength;

			PropertyChanged();
		}


		private void PropertyChanged() {

			if ( IsRefreshSuspended )
				return;

			LayoutParam.ResetLayout();
			PerformLayout();

			Refresh();
		}


		private void ShipStatusEquipment_Paint( object sender, PaintEventArgs e ) {

			e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

			Rectangle basearea = new Rectangle( Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical );
			//e.Graphics.DrawRectangle( Pens.Magenta, basearea.X, basearea.Y, basearea.Width - 1, basearea.Height - 1 );

			ImageList eqimages = ResourceManager.Instance.Equipments;

			TextFormatFlags textformatBottomRight = GetBaseTextFormat() | TextFormatFlags.Bottom | TextFormatFlags.Right;
			TextFormatFlags textformatTopLeft = GetBaseTextFormat() | TextFormatFlags.Top | TextFormatFlags.Left;
			TextFormatFlags textformatTopRight =  GetBaseTextFormat() | TextFormatFlags.Top | TextFormatFlags.Right;


			LayoutParam.UpdateParameters( e.Graphics, basearea.Size, Font );


			for ( int slotindex = 0; slotindex < SlotList.Length; slotindex++ ) {

				SlotItem slot = SlotList[slotindex];

				Image image = null;

				var origin = new Point( basearea.X + LayoutParam.SlotUnitSize.Width * slotindex, basearea.Y );


				if ( slotindex >= SlotSize && slot.EquipmentID != -1 ) {
					//invalid!
					e.Graphics.FillRectangle( _invalidSlotBrush, new Rectangle( origin, LayoutParam.SlotUnitSize ) );
				}


				if ( slot.EquipmentID == -1 ) {
					if ( slotindex < SlotSize ) {
						//nothing
						image = eqimages.Images[(int)ResourceManager.EquipmentContent.Nothing];

					} else {
						//locked
						image = eqimages.Images[(int)ResourceManager.EquipmentContent.Locked];
					}

				} else {
					int imageID = slot.EquipmentIconID;
					if ( imageID <= 0 || (int)ResourceManager.EquipmentContent.Locked <= imageID )
						imageID = (int)ResourceManager.EquipmentContent.Unknown;

					image = eqimages.Images[imageID];
				}


				Rectangle imagearea = new Rectangle( origin.X, origin.Y + ( LayoutParam.SlotUnitSize.Height - LayoutParam.ImageSize.Height ) / 2, LayoutParam.ImageSize.Width, LayoutParam.ImageSize.Height );
				if ( image != null ) {

					e.Graphics.DrawImage( image, imagearea );
				}


				Color aircraftColor = AircraftColorDisabled;
				bool drawAircraftSlot = ShowAircraft;

				if ( slot.AircraftMax == 0 ) {
					if ( Calculator.IsAircraft( slot.EquipmentID, true ) ) {
						aircraftColor = AircraftColorDisabled;
					} else {
						drawAircraftSlot = false;
					}

				} else if ( slot.AircraftCurrent == 0 ) {
					aircraftColor = AircraftColorLost;

				} else if ( slot.AircraftCurrent < slot.AircraftMax ) {
					aircraftColor = AircraftColorDamaged;

				} else if ( !Calculator.IsAircraft( slot.EquipmentID, true ) ) {
					aircraftColor = AircraftColorDisabled;

				} else {
					aircraftColor = AircraftColorFull;
				}


				// 艦載機数描画
				if ( drawAircraftSlot ) {

					Rectangle textarea = new Rectangle( origin.X + LayoutParam.ImageSize.Width, origin.Y + LayoutParam.InfoAreaSize.Height * 3 / 4 - LayoutParam.Digit2Size.Height / 2,
						LayoutParam.InfoAreaSize.Width, LayoutParam.Digit2Size.Height );
					//e.Graphics.DrawRectangle( Pens.Cyan, textarea );

					if ( slot.AircraftCurrent < 10 ) {
						//1桁なら画像に近づける

						textarea.Width -= LayoutParam.Digit2Size.Width / 2;

					} else if ( slot.AircraftCurrent >= 100 ) {
						//3桁以上ならオーバーレイを入れる

						Size sz_realstr = TextRenderer.MeasureText( e.Graphics, slot.AircraftCurrent.ToString(), Font, new Size( int.MaxValue, int.MaxValue ), textformatBottomRight );

						textarea.X -= sz_realstr.Width - textarea.Width;
						textarea.Width = sz_realstr.Width;

						e.Graphics.FillRectangle( _overlayBrush, textarea );
					}

					TextRenderer.DrawText( e.Graphics, slot.AircraftCurrent.ToString(), Font, textarea, aircraftColor, textformatBottomRight );
				}

				// 改修レベル描画
				if ( slot.Level > 0 ) {

					if ( LevelVisibility == LevelVisibilityFlag.LevelOnly ||
						LevelVisibility == LevelVisibilityFlag.Both ||
						LevelVisibility == LevelVisibilityFlag.AircraftLevelOverlay ||
						( LevelVisibility == LevelVisibilityFlag.LevelPriority && ( !_onMouse || slot.AircraftLevel == 0 ) ) ||
						( LevelVisibility == LevelVisibilityFlag.AircraftLevelPriority && ( _onMouse || slot.AircraftLevel == 0 ) ) ) {

						TextRenderer.DrawText( e.Graphics, slot.Level >= 10 ? "★" : "+" + slot.Level, Font,
							new Rectangle( origin.X + LayoutParam.ImageSize.Width, origin.Y + LayoutParam.InfoAreaSize.Height * 1 / 4 - LayoutParam.Digit2Size.Height / 2,
								LayoutParam.InfoAreaSize.Width, LayoutParam.Digit2Size.Height ), EquipmentLevelColor, textformatTopRight );

					}

				}


				// 艦載機熟練度描画
				if ( slot.AircraftLevel > 0 ) {

					if ( LevelVisibility == LevelVisibilityFlag.AircraftLevelOnly ||
						LevelVisibility == LevelVisibilityFlag.Both ||
						( LevelVisibility == LevelVisibilityFlag.AircraftLevelPriority && ( !_onMouse || slot.Level == 0 ) ) ||
						( LevelVisibility == LevelVisibilityFlag.LevelPriority && ( _onMouse || slot.Level == 0 ) ) ) {
						// 右上に描画

						if ( ShowAircraftLevelByNumber ) {
							var area = new Rectangle( origin.X + LayoutParam.ImageSize.Width, origin.Y + LayoutParam.InfoAreaSize.Height * 1 / 4 - LayoutParam.Digit2Size.Height / 2,
								LayoutParam.InfoAreaSize.Width, LayoutParam.Digit2Size.Height );
							TextRenderer.DrawText( e.Graphics, slot.AircraftLevel.ToString(), Font, area, GetAircraftLevelColor( slot.AircraftLevel ), textformatTopRight );

						} else {
							var area = new Rectangle( origin.X + LayoutParam.ImageSize.Width, origin.Y,
								LayoutParam.ImageSize.Width, LayoutParam.ImageSize.Height );
							e.Graphics.DrawImage( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.AircraftLevelTop0 + slot.AircraftLevel], area );
						}


					} else if ( LevelVisibility == LevelVisibilityFlag.AircraftLevelOverlay ) {
						// 左上に描画

						if ( ShowAircraftLevelByNumber ) {
							var area = new Rectangle( origin.X, origin.Y, LayoutParam.Digit2Size.Width / 2, LayoutParam.Digit2Size.Height );
							e.Graphics.FillRectangle( _overlayBrush, area );
							TextRenderer.DrawText( e.Graphics, slot.AircraftLevel.ToString(), Font, area, GetAircraftLevelColor( slot.AircraftLevel ), textformatTopLeft );

						} else {
							e.Graphics.FillRectangle( _overlayBrush, new Rectangle( origin.X, origin.Y, LayoutParam.ImageSize.Width, LayoutParam.ImageSize.Height / 2 ) );
							e.Graphics.DrawImage( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.AircraftLevelTop0 + slot.AircraftLevel], new Rectangle( origin, LayoutParam.ImageSize ) );
						}
					}

				}

			}


		}


		public override Size GetPreferredSize( Size proposedSize ) {

			LayoutParam.UpdateParameters( null, proposedSize, Font );

			return LayoutParam.PreferredSize + Padding.Size;
		}


		private static TextFormatFlags GetBaseTextFormat() {
			return TextFormatFlags.NoPadding;
		}

		private Color GetAircraftLevelColor( int level ) {
			if ( level <= 3 )
				return AircraftLevelColorLow;
			else
				return AircraftLevelColorHigh;
		}



		private void ShipStatusEquipment_MouseEnter( object sender, EventArgs e ) {
			_onMouse = true;
			if ( LevelVisibility == LevelVisibilityFlag.LevelPriority || LevelVisibility == LevelVisibilityFlag.AircraftLevelPriority )
				PropertyChanged();
		}

		private void ShipStatusEquipment_MouseLeave( object sender, EventArgs e ) {
			_onMouse = false;
			if ( LevelVisibility == LevelVisibilityFlag.LevelPriority || LevelVisibility == LevelVisibilityFlag.AircraftLevelPriority )
				PropertyChanged();
		}


		void ShipStatusEquipment_Disposed( object sender, EventArgs e ) {
			_overlayBrush.Dispose();
			_invalidSlotBrush.Dispose();
		}

	}
}
