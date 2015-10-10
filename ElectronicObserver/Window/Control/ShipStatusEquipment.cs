﻿using System;
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

			public bool IsExpansionSlot { get; set; }

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
				Level =
				AircraftLevel = 0;
			}
		}



		#region Properties


		private SlotItem[] SlotList;
		private int SlotSize { get; set; }
		private bool IsExpansionSlotAvailable { get; set; }


		[Browsable( true )]
		[DefaultValue( typeof( Font ), "Meiryo UI, 10px" )]
		public override Font Font {
			get { return base.Font; }
			set {
				base.Font = value;
				PropertyChanged();
			}
		}

		private Color _aircraftColorDisabled;
		/// <summary>
		/// 艦載機非搭載スロットの文字色
		/// </summary>
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "170, 170, 170" )]
		public Color AircraftColorDisabled {
			get { return _aircraftColorDisabled; }
			set {
				_aircraftColorDisabled = value;
				PropertyChanged();
			}
		}

		private Pen _aircraftOne = new Pen( Color.FromArgb( 154, 181, 208 ), 1 );
		private Pen _aircraftTwo = new Pen( Color.FromArgb( 213, 157, 18 ), 1 );
		private Pen _aircraftThree = new Pen( Color.FromArgb( 38, 181, 50 ), 1 );

		private Color _aircraftColorLost;
		/// <summary>
		/// 艦載機全滅スロットの文字色
		/// </summary>
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "255, 0, 255" )]
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
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "255, 0, 0" )]
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
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "0, 0, 0" )]
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
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "0, 102, 102" )]
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
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "102, 153, 238" )]
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
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "255, 170, 0" )]
		public Color AircraftLevelColorHigh {
			get { return _aircraftLevelColorHigh; }
			set {
				_aircraftLevelColorHigh = value;
				PropertyChanged();
			}
		}

		private Color _invalidSlotColor;
		/// <summary>
		/// 不正スロットの背景色
		/// </summary>
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "64, 255, 0, 0" )]
		public Color InvalidSlotColor {
			get { return _invalidSlotColor; }
			set {
				_invalidSlotColor = value;
				PropertyChanged();
			}
		}


		private bool _showAircraft;
		/// <summary>
		/// 艦載機搭載数を表示するか
		/// </summary>
		[Browsable( true )]
		[DefaultValue( true )]
		public bool ShowAircraft {
			get { return _showAircraft; }
			set {
				_showAircraft = value;
				PropertyChanged();
			}
		}


		private bool _overlayAircraft;
		/// <summary>
		/// 艦載機搭載数をアイコンの上に表示するか
		/// </summary>
		[Browsable( true )]
		[DefaultValue( false )]
		public bool OverlayAircraft {
			get { return _overlayAircraft; }
			set {
				_overlayAircraft = value;
				PropertyChanged();
			}
		}

		private bool _textProficiency;
		[Browsable( true )]
		[DefaultValue( false )]
		public bool TextProficiency
		{
			get { return _textProficiency; }
			set {
				_textProficiency = value;
				PropertyChanged();
			}
		}

		private bool _showEquipmentLevel = true;
		/// <summary>
		/// 装備改修レベル・艦載機熟練度を表示するか
		/// </summary>
		[Browsable( true )]
		[DefaultValue( true )]
		public bool ShowEquipmentLevel {
			get { return _showEquipmentLevel; }
			set {
				_showEquipmentLevel = value;
				PropertyChanged();
			}
		}


		private int _slotMargin;
		/// <summary>
		/// スロット間の空きスペース
		/// </summary>
		[Browsable( true )]
		[DefaultValue( 3 )]
		[Description( "スロット間のスペースを指定します。" )]
		[Category( "表示" )]
		public int SlotMargin {
			get { return _slotMargin; }
			set {
				_slotMargin = value;
				PropertyChanged();
			}
		}

		private int _aircraftMargin;
		/// <summary>
		/// 搭載数表示位置のスペース
		/// </summary>
		[Browsable( true )]
		[DefaultValue( 3 )]
		[Description( "搭載数表示位置のスペースを指定します。" )]
		[Category( "表示" )]
		public int AircraftMargin {
			get { return _aircraftMargin; }
			set {
				_aircraftMargin = value;
				PropertyChanged();
			}
		}

		#endregion



		public ShipStatusEquipment() {
			InitializeComponent();

			SlotList = new SlotItem[6];
			for ( int i = 0; i < SlotList.Length; i++ ) {
				SlotList[i] = new SlotItem();
			}
			SlotSize = 0;


			base.Font = new Font( "Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel );

			_aircraftColorDisabled = Color.FromArgb( 0xAA, 0xAA, 0xAA );
			_aircraftColorLost = Color.FromArgb( 0xFF, 0x00, 0xFF );
			_aircraftColorDamaged = Color.FromArgb( 0xFF, 0x00, 0x00 );
			_aircraftColorFull = Utility.Configuration.Config.UI.ForeColor;

			_equipmentLevelColor = Color.FromArgb( 0x00, 0x66, 0x66 );
			_aircraftLevelColorLow = Color.FromArgb( 0x66, 0x99, 0xEE );
			_aircraftLevelColorHigh = Color.FromArgb( 0xFF, 0xAA, 0x00 );

			_invalidSlotColor = Color.FromArgb( 0x40, 0xFF, 0x00, 0x00 );

			_showAircraft = true;
			_overlayAircraft = false;

			_slotMargin = 3;
			_aircraftMargin = 3;

		}


		/// <summary>
		/// スロット情報を設定します。主に味方艦用です。
		/// </summary>
		/// <param name="ship">当該艦船。</param>
		public void SetSlotList( ShipData ship ) {

			int slotCount = Math.Max( ship.SlotSize + ( ship.IsExpansionSlotAvailable ? 1 : 0 ), 4 );

			IsExpansionSlotAvailable = ship.IsExpansionSlotAvailable;

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
				SlotList[i].IsExpansionSlot = false;
			}

			if ( ship.IsExpansionSlotAvailable ) {
				var eq = ship.ExpansionSlotInstance;
				SlotList[ship.SlotSize].EquipmentID = eq != null ? eq.EquipmentID : -1;
				SlotList[ship.SlotSize].AircraftCurrent =
				SlotList[ship.SlotSize].AircraftMax =
				SlotList[ship.SlotSize].Level =
				SlotList[ship.SlotSize].AircraftLevel = 0;
				SlotList[ship.SlotSize].IsExpansionSlot = true;
			}


			SlotSize = ship.SlotSize + ( ship.IsExpansionSlotAvailable ? 1 : 0 );

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
				SlotList[i].EquipmentID = ship.DefaultSlot == null ? -1 : ( ship.DefaultSlot.Count < i ? ship.DefaultSlot[i] : -1 );
				SlotList[i].AircraftCurrent =
				SlotList[i].AircraftMax = ship.Aircraft[i];
				SlotList[i].Level =
				SlotList[i].AircraftLevel = 0;
			}

			SlotSize = ship.SlotSize;

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

			SlotSize = ship != null ? ship.SlotSize : 0;

			PropertyChanged();
		}


		private void PropertyChanged() {

			if ( AutoSize )
				Size = GetPreferredSize( Size );

			Refresh();
		}


		private void ShipStatusEquipment_Paint( object sender, PaintEventArgs e ) {

			Rectangle basearea = new Rectangle( Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical );
			//e.Graphics.DrawRectangle( Pens.Magenta, basearea.X, basearea.Y, basearea.Width - 1, basearea.Height - 1 );

			ImageCollection eqimages = ResourceManager.Instance.Equipments;

			TextFormatFlags textformat = TextFormatFlags.NoPadding;
			if ( !OverlayAircraft ) {
				textformat |= TextFormatFlags.Bottom | TextFormatFlags.Right;
			} else {
				textformat |= TextFormatFlags.Bottom | TextFormatFlags.Left;
			}

			TextFormatFlags textformatLevel = TextFormatFlags.NoPadding;
			if ( !OverlayAircraft ) {
				textformatLevel |= TextFormatFlags.Top | TextFormatFlags.Right;
			} else {
				textformatLevel |= TextFormatFlags.Top | TextFormatFlags.Left;
			}

			// 艦載機スロット表示の予測サイズ(2桁)
			Size sz_eststr = TextRenderer.MeasureText( "99", Font, new Size( int.MaxValue, int.MaxValue ), textformat );
			sz_eststr.Width -= (int)( Font.Size / 2.0 );

			// スロット1つ当たりのサイズ(右の余白含む)
			Size sz_unit = new Size( eqimages.ImageSize.Width + SlotMargin, eqimages.ImageSize.Height );
			if ( ShowAircraft || ShowEquipmentLevel ) {
				if ( !OverlayAircraft )
					sz_unit.Width += sz_eststr.Width;
				sz_unit.Height = Math.Max( sz_unit.Height, sz_eststr.Height );
			}


			for ( int slotindex = 0; slotindex < SlotList.Length; slotindex++ ) {

				SlotItem slot = SlotList[slotindex];

				Image image = null;


				if ( slotindex >= SlotSize && slot.EquipmentID != -1 ) {
					//invalid!
					using ( SolidBrush b = new SolidBrush( InvalidSlotColor ) ) {
						e.Graphics.FillRectangle( b, new Rectangle( basearea.X + sz_unit.Width * slotindex, basearea.Y, sz_unit.Width, sz_unit.Height ) );
					}
				}

				if ( slot.IsExpansionSlot )
				{
					// expansion slot spliter
					e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
					using ( var b = new SolidBrush( AircraftColorDisabled ) )
					{
						e.Graphics.FillRectangle( b, new Rectangle( basearea.X + sz_unit.Width * 4, basearea.Y, 1, sz_unit.Height ) );
					}
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


				if ( image != null ) {
					Rectangle imagearea;
					if ( slot.IsExpansionSlot )
						imagearea = new Rectangle( basearea.X + sz_unit.Width * 4 + 3, basearea.Y, eqimages.ImageSize.Width, eqimages.ImageSize.Height );
					else
						imagearea = new Rectangle( basearea.X + sz_unit.Width * slotindex, basearea.Y, eqimages.ImageSize.Width, eqimages.ImageSize.Height );

					e.Graphics.DrawImage( image, imagearea );
					//e.Graphics.DrawRectangle( Pens.Magenta, basearea.X + sz_unit.Width * slotindex, basearea.Y, eqimages.ImageSize.Width, eqimages.ImageSize.Height );
				}


				Color aircraftColor = AircraftColorDisabled;
				bool drawEquipmentLevel = ShowEquipmentLevel;
				bool drawAircraftSlot = ShowAircraft;
				bool drawAircraftProficiency = ( slot.AircraftLevel > 0 );

				if ( slot.EquipmentID != -1 ) { //装備有

					if ( Calculator.IsAircraft( slot.EquipmentID, true ) ) { //装備有り、艦載機の場合

						if ( slot.AircraftMax < 0 )
							drawAircraftSlot = false;
						else if ( slot.AircraftMax == 0 ) {
							aircraftColor = AircraftColorDisabled;

						} else if ( slot.AircraftCurrent == 0 ) {
							aircraftColor = AircraftColorLost;

						} else if ( slot.AircraftCurrent < slot.AircraftMax ) {
							aircraftColor = AircraftColorDamaged;

						} else {
							aircraftColor = AircraftColorFull;
						}

					} else {

						if ( slot.AircraftMax <= 0 )
							drawAircraftSlot = false;
					}

				} else if ( slot.AircraftMax <= 0 ) {
					drawAircraftSlot = false;
				}


				if ( slot.AircraftLevel == 0 && slot.Level == 0 ) {
					drawEquipmentLevel = false;
				}




				if ( drawAircraftSlot ) {

					Rectangle textarea = new Rectangle( basearea.X + sz_unit.Width * slotindex, basearea.Y - AircraftMargin - 1, sz_unit.Width - SlotMargin, sz_unit.Height + AircraftMargin * 2 );
					//e.Graphics.DrawRectangle( Pens.Cyan, textarea );


					if ( OverlayAircraft ) {
						using ( SolidBrush b = new SolidBrush( Color.FromArgb( 0xC0, 0xF0, 0xF0, 0xF0 ) ) ) {
							//e.Graphics.FillRectangle( b, new Rectangle( textarea.X, textarea.Y, sz_eststr.Width, sz_eststr.Height ) );
							e.Graphics.FillRectangle( b, textarea );
						}

					} else {

						if ( slot.AircraftCurrent < 10 ) {
							//1桁なら画像に近づける

							textarea.Width -= sz_eststr.Width / 2 - 1;

						} else if ( slot.AircraftCurrent >= 100 ) {
							//3桁以上ならオーバーレイを入れる

							Size sz_realstr = TextRenderer.MeasureText( slot.AircraftCurrent.ToString(), Font, new Size( int.MaxValue, int.MaxValue ), textformat );
							sz_realstr.Width -= (int)( Font.Size / 2.0 );

							using ( SolidBrush b = new SolidBrush( Color.FromArgb( 0xC0, 0xF0, 0xF0, 0xF0 ) ) ) {
								e.Graphics.FillRectangle( b, new Rectangle(
									textarea.X + sz_unit.Width - sz_realstr.Width - SlotMargin,
									textarea.Bottom - sz_realstr.Height + AircraftMargin,
									sz_realstr.Width, sz_realstr.Height ) );
							}

						}

					}

					textarea.Height = Math.Max( textarea.Height, Height - 1 );
					TextRenderer.DrawText( e.Graphics, slot.AircraftCurrent.ToString(), Font, textarea, aircraftColor, textformat );
				}

				if ( drawAircraftProficiency )
				{
					Rectangle textarea = new Rectangle( basearea.X + sz_unit.Width * slotindex, basearea.Y, sz_unit.Width - SlotMargin, sz_unit.Height );
					if ( TextProficiency )
					{
						textarea.Y -= 5;
						TextFormatFlags format = TextFormatFlags.NoPadding | TextFormatFlags.Top | TextFormatFlags.Right;
						Color color = AircraftColorDisabled;
						//if ( slot.AircraftProficiency < 4 )
						//	color = _aircraftPenOne.Color;
						//else if ( slot.AircraftProficiency < 7 )
						//	color = _aircraftPenTwo.Color;
						//else
						//	color = _aircraftPenThree.Color;
						TextRenderer.DrawText( e.Graphics, slot.AircraftLevel.ToString(), Font, textarea, color, format );
					}
					else
					{
						e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
						int h = sz_unit.Height / 2;

						int x = textarea.Right - 3;
						int y = textarea.Y;
						int pro = slot.AircraftLevel;
						if ( pro < 4 )
						{
							for ( int i = 0; i < pro; i++ )
							{
								e.Graphics.DrawLine( _aircraftOne, x, 0, x, h );
								x -= 3;
							}
						}
						else if ( pro < 7 )
						{
							for ( int i = 0; i < pro - 3; i++ )
							{
								e.Graphics.DrawLine( _aircraftTwo, x - 2, 0, x + 2, h );
								x -= 4;
							}
						}
						else
						{
							for ( int i = 0; i < 2; i++ )
							{
								e.Graphics.DrawLine( _aircraftThree, x - 2, 0, x + 1, h / 2 );
								e.Graphics.DrawLine( _aircraftThree, x - 2, h, x + 1, h / 2 );
								x -= 4;
							}
						}
						//Brush brush;
						//if ( pro < 4 )
						//	brush = _aircraftBrushOne;
						//else if ( pro < 7 )
						//	brush = _aircraftBrushTwo;
						//else
						//	brush = _aircraftBrushThree;
						//for ( int i = 0; i < pro; i++ )
						//{
						//	e.Graphics.FillRectangle( brush, x + 2, y, 1, 5 );
						//	x -= 2;
						//}
					}
				}

				if ( drawEquipmentLevel ) {

					//Rectangle textarea = new Rectangle( basearea.X + sz_unit.Width * slotindex, basearea.Y - ( AircraftMargin + SlotMargin ), sz_unit.Width - AircraftMargin, sz_unit.Height + 7 );
					Rectangle textarea = new Rectangle( basearea.X + sz_unit.Width * slotindex, basearea.Y - AircraftMargin - 1, sz_unit.Width - SlotMargin, sz_unit.Height + AircraftMargin * 2 );
					//e.Graphics.DrawRectangle( Pens.Cyan, textarea );


					/*
					if ( slot.AircraftLevel > 0 ) {

						string leveltext;
						Color levelcol;

						if ( slot.AircraftLevel <= 3 )
							levelcol = AircraftLevelColorLow;
						else
							levelcol = AircraftLevelColorHigh;

						switch ( slot.AircraftLevel ) {
							case 1: leveltext = "|"; break;
							case 2: leveltext = "||"; break;
							case 3: leveltext = "|||"; break;
							case 4: leveltext = "/"; break;
							case 5: leveltext = "//"; break;
							case 6: leveltext = "///"; break;
							case 7: leveltext = ">>"; break;
							default: leveltext = "x"; break;
						}

						TextRenderer.DrawText( e.Graphics, leveltext, Font, textarea, levelcol, textformatLevel );
					}
					//*/


					if ( slot.Level > 0 ) {
						TextRenderer.DrawText( e.Graphics, slot.Level >= 10 ? "★" : "+" + slot.Level, Font, textarea, EquipmentLevelColor, textformatLevel );
					}

				}


			}

		}


		public override Size GetPreferredSize( Size proposedSize ) {

			Rectangle basearea = new Rectangle( Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical );
			//e.Graphics.DrawRectangle( Pens.Magenta, basearea.X, basearea.Y, basearea.Width - 1, basearea.Height - 1 );

			ImageCollection eqimages = ResourceManager.Instance.Equipments;

			TextFormatFlags textformat = TextFormatFlags.NoPadding;
			if ( !OverlayAircraft ) {
				textformat |= TextFormatFlags.Bottom | TextFormatFlags.Right;
			} else {
				textformat |= TextFormatFlags.Top | TextFormatFlags.Left;
			}

			Size sz_eststr = TextRenderer.MeasureText( "99", Font, new Size( int.MaxValue, int.MaxValue ), textformat );
			sz_eststr.Width -= (int)( Font.Size / 2.0 );

			Size sz_unit = new Size( eqimages.ImageSize.Width + SlotMargin, eqimages.ImageSize.Height );
			if ( ShowAircraft || ShowEquipmentLevel ) {
				if ( !OverlayAircraft )
					sz_unit.Width += sz_eststr.Width;
				sz_unit.Height = Math.Max( sz_unit.Height, sz_eststr.Height );
			}

			int width;
			if ( IsExpansionSlotAvailable )
				width = Padding.Horizontal + sz_unit.Width * 5 + 3;
			else
				width = Padding.Horizontal + sz_unit.Width * SlotList.Length;

			return new Size( width, Padding.Vertical + Math.Max( eqimages.ImageSize.Height, sz_unit.Height ) );

		}

	}
}
