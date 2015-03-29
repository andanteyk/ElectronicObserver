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

namespace ElectronicObserver.Window.Control {

	public partial class ShipStatusEquipment : UserControl {

		private class SlotItem {

			public int EquipmentID { get; set; }

			public EquipmentDataMaster Equipment {
				get {
					if ( EquipmentID != -1 )
						return KCDatabase.Instance.MasterEquipments[EquipmentID];
					else
						return null;
				}
			}

			public int EquipmentIconID {
				get {
					if ( EquipmentID != -1 )
						return KCDatabase.Instance.MasterEquipments[EquipmentID].EquipmentType[3];
					else
						return -1;
				}
			}
			public int AircraftCurrent { get; set; }
			public int AircraftMax { get; set; }


			public SlotItem() {
				EquipmentID = -1;
				AircraftCurrent = AircraftMax = 0;
			}
		}



		#region Properties


		private SlotItem[] SlotList;
		

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
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "170, 170, 170" )]
		public Color AircraftColorDisabled {
			get { return _aircraftColorDisabled; }
			set {
				_aircraftColorDisabled = value;
				PropertyChanged();
			}
		}

		private Color _aircraftColorLost;
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
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "0, 0, 0" )]
		public Color AircraftColorFull {
			get { return _aircraftColorFull; }
			set {
				_aircraftColorFull = value;
				PropertyChanged();
			}
		}


		private Color _invalidSlotColor;
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
		[Browsable( true )]
		[DefaultValue( true )]
		public bool OverlayAircraft {
			get { return _overlayAircraft; }
			set {
				_overlayAircraft = value;
				PropertyChanged();
			}
		}


		private int _slotMargin;
		[Browsable( true )]
		[DefaultValue( 0 )]
		[Description( "スロット間のスペースを指定します。" )]
		[Category( "表示" )]
		public int SlotMargin {
			get { return _slotMargin; }
			set {
				_slotMargin = value;
				PropertyChanged();
			}
		}


		private int SlotSize { get; set; }

		#endregion



		public ShipStatusEquipment() {
			InitializeComponent();

			SlotList = new SlotItem[5];
			for ( int i = 0; i < SlotList.Length; i++ ) {
				SlotList[i] = new SlotItem();
			}


			base.Font = new Font( "Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel );
			
			_aircraftColorDisabled = Color.FromArgb( 0xAA, 0xAA, 0xAA );
			_aircraftColorLost = Color.FromArgb( 0xFF, 0x00, 0xFF );
			_aircraftColorDamaged = Color.FromArgb( 0xFF, 0x00, 0x00 );
			_aircraftColorFull = Color.FromArgb( 0x00, 0x00, 0x00 );

			_invalidSlotColor = Color.FromArgb( 0x40, 0xFF, 0x00, 0x00 );

			_showAircraft = true;
			_overlayAircraft = false;

			_slotMargin = 3;
			SlotSize = 0;

		}


		/// <summary>
		/// スロット情報を設定します。主に味方艦用です。
		/// </summary>
		/// <param name="ship">当該艦船。</param>
		public void SetSlotList( ShipData ship ) {

			int SlotCount = Math.Min( 4, ship.Slot.Count ); // 味方艦は装備4つまでなので4つに制限
			if ( SlotList.Length != SlotCount ) {
				SlotList = new SlotItem[SlotCount];
				for ( int i = 0; i < SlotList.Length; i++ ) {
					SlotList[i] = new SlotItem();
				}
			}

			for ( int i = 0; i < SlotList.Length; i++ ) {
				SlotList[i].EquipmentID = ship.Slot[i] != -1 ? KCDatabase.Instance.Equipments[ship.Slot[i]].EquipmentID : -1;
				SlotList[i].AircraftCurrent = ship.Aircraft[i];
				SlotList[i].AircraftMax = KCDatabase.Instance.MasterShips[ship.ShipID].Aircraft[i];
			}

			SlotSize = KCDatabase.Instance.MasterShips[ship.ShipID].SlotSize;

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

			if ( SlotList.Length != slot.Length ) {
				SlotList = new SlotItem[slot.Length];
				for ( int i = 0; i < SlotList.Length; i++ ) {
					SlotList[i] = new SlotItem();
				}
			}

			for ( int i = 0; i < SlotList.Length; i++ ) {
				SlotList[i].EquipmentID = slot[i];
				SlotList[i].AircraftCurrent = ship.Aircraft[i];
				SlotList[i].AircraftMax = ship.Aircraft[i];
			}

			SlotSize = ship.SlotSize;

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

			ImageList eqimages = ResourceManager.Instance.Equipments;

			TextFormatFlags textformat = TextFormatFlags.NoPadding;
			if ( !OverlayAircraft ) {
				textformat |= TextFormatFlags.Bottom | TextFormatFlags.Right;
			} else {
				textformat |= TextFormatFlags.Top | TextFormatFlags.Left;
			}

			Size sz_eststr = TextRenderer.MeasureText( "99", Font, new Size( int.MaxValue, int.MaxValue ), textformat );
			sz_eststr.Width -= (int)( Font.Size / 1.5 );
			
			Size sz_unit = new Size( eqimages.ImageSize.Width + SlotMargin, eqimages.ImageSize.Height );
			if ( ShowAircraft ) {
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
					Rectangle imagearea = new Rectangle( basearea.X + sz_unit.Width * slotindex, basearea.Y, eqimages.ImageSize.Width, eqimages.ImageSize.Height );

					e.Graphics.DrawImage( image, imagearea );
				}



				Color aircraftColor = AircraftColorDisabled;
				bool drawAircraftSlot = ShowAircraft;

				if ( slot.EquipmentID != -1 ) {

					switch ( slot.Equipment.EquipmentType[2] ) {
						case 6:		//艦戦
						case 7:		//艦爆
						case 8:		//艦攻
						case 9:		//艦偵
						case 10:	//水偵
						case 11:	//水爆
						case 25:	//オートジャイロ
						case 26:	//対潜哨戒機

							if ( slot.AircraftMax == 0 ) {
								aircraftColor = AircraftColorDisabled;
							} else if ( slot.AircraftCurrent == 0 ) {
								aircraftColor = AircraftColorLost;
							} else if ( slot.AircraftCurrent < slot.AircraftMax ) {
								aircraftColor = AircraftColorDamaged;
							} else {
								aircraftColor = AircraftColorFull;
							}
							break;

						default:
							if ( slot.AircraftMax == 0 )
								drawAircraftSlot = false;
							break;
					}

				} else if ( slotindex >= SlotSize && slot.AircraftMax == 0 ) {
					drawAircraftSlot = false;

				} else if ( slot.AircraftMax == 0 ) {
					drawAircraftSlot = false;
				}


				if ( drawAircraftSlot ) {
					Rectangle textarea = new Rectangle( basearea.X + sz_unit.Width * slotindex, basearea.Y, sz_unit.Width - SlotMargin, sz_unit.Height );
					String aircraftSlotText = slot.AircraftCurrent.ToString();

					if ( OverlayAircraft ) {
						using ( SolidBrush b = new SolidBrush( Color.FromArgb( 0x80, 0xFF, 0xFF, 0xFF ) ) ) {
							e.Graphics.FillRectangle( b, new Rectangle( textarea.X, textarea.Y, sz_eststr.Width, sz_eststr.Height ) );
						}
					} else {
						if ( aircraftSlotText.Length == 1 ) {
							// 1文字の場合は画像に近づける
							textarea.Width -= sz_eststr.Width / 2;
						}
						if ( aircraftSlotText.Length > 2 ) { // 3文字以上の時はかぶるので背景を薄くする
							Size sz_realstr = TextRenderer.MeasureText( aircraftSlotText, Font, new Size( int.MaxValue, int.MaxValue ), textformat );
							sz_realstr.Width -= (int)( Font.Size / 2.0 );
							using ( SolidBrush b = new SolidBrush( Color.FromArgb( 0xC0, 0xFF, 0xFF, 0xFF ) ) ) {
								e.Graphics.FillRectangle( b, new Rectangle(
									textarea.X + sz_unit.Width - sz_realstr.Width,
									textarea.Y + sz_unit.Height - sz_realstr.Height,
									sz_realstr.Width, sz_realstr.Height ) );
							}
						}
					}


					TextRenderer.DrawText( e.Graphics, aircraftSlotText, Font, textarea, aircraftColor, textformat );
				}

			}
		
		}


		public override Size GetPreferredSize( Size proposedSize ) {

			Rectangle basearea = new Rectangle( Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical );
			//e.Graphics.DrawRectangle( Pens.Magenta, basearea.X, basearea.Y, basearea.Width - 1, basearea.Height - 1 );

			ImageList eqimages = ResourceManager.Instance.Equipments;

			TextFormatFlags textformat = TextFormatFlags.NoPadding;
			if ( !OverlayAircraft ) {
				textformat |= TextFormatFlags.Bottom | TextFormatFlags.Right;
			} else {
				textformat |= TextFormatFlags.Top | TextFormatFlags.Left;
			}

			Size sz_eststr = TextRenderer.MeasureText( "99", Font, new Size( int.MaxValue, int.MaxValue ), textformat );
			sz_eststr.Width -= (int)( Font.Size / 1.5 );

			Size sz_unit = new Size( eqimages.ImageSize.Width + SlotMargin, eqimages.ImageSize.Height );
			if ( ShowAircraft ) {
				if ( !OverlayAircraft )
					sz_unit.Width += sz_eststr.Width;
				sz_unit.Height = Math.Max( sz_unit.Height, sz_eststr.Height );
			}
			

			return new Size( Padding.Horizontal + sz_unit.Width * SlotList.Length, Padding.Vertical + Math.Max( eqimages.ImageSize.Height, sz_unit.Height ) );

		}

	}
}
