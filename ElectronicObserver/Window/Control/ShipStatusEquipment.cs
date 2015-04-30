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

namespace ElectronicObserver.Window.Control {

	public partial class ShipStatusEquipment : UserControl {

		private class SlotItem {

			public int EquipmentID { get; set; }

			public EquipmentDataMaster Equipment {
				get {
					return KCDatabase.Instance.MasterEquipments[EquipmentID];
				}
			}

			public int EquipmentIconID {
				get {
					var eq = KCDatabase.Instance.MasterEquipments[EquipmentID];
					if ( eq != null )
						return eq.IconType;
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
		private int SlotSize { get; set; }


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


		#endregion



		public ShipStatusEquipment() {
			InitializeComponent();

			SlotList = new SlotItem[5];
			for ( int i = 0; i < SlotList.Length; i++ ) {
				SlotList[i] = new SlotItem();
			}
			SlotSize = 0;


			base.Font = new Font( "Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel );

			_aircraftColorDisabled = Color.FromArgb( 0xAA, 0xAA, 0xAA );
			_aircraftColorLost = Color.FromArgb( 0xFF, 0x00, 0xFF );
			_aircraftColorDamaged = Color.FromArgb( 0xFF, 0x00, 0x00 );
			_aircraftColorFull = Color.FromArgb( 0x00, 0x00, 0x00 );

			_invalidSlotColor = Color.FromArgb( 0x40, 0xFF, 0x00, 0x00 );

			_showAircraft = true;
			_overlayAircraft = false;

			_slotMargin = 3;

		}


		/// <summary>
		/// スロット情報を設定します。主に味方艦用です。
		/// </summary>
		/// <param name="ship">当該艦船。</param>
		public void SetSlotList( ShipData ship ) {

			int slotCount = Math.Max( ship.MasterShip.SlotSize, 4 );

			if ( SlotList.Length != slotCount ) {
				SlotList = new SlotItem[slotCount];
				for ( int i = 0; i < SlotList.Length; i++ ) {
					SlotList[i] = new SlotItem();
				}
			}

			for ( int i = 0; i < SlotList.Length; i++ ) {
				var eq = ship.SlotInstance[i];
				SlotList[i].EquipmentID = eq != null ? eq.EquipmentID : -1;
				SlotList[i].AircraftCurrent = ship.Aircraft[i];
				SlotList[i].AircraftMax = ship.MasterShip.Aircraft[i];
			}

			SlotSize = ship.MasterShip.SlotSize;

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

			ImageList eqimages = ResourceManager.Instance.Equipments;

			TextFormatFlags textformat = TextFormatFlags.NoPadding;
			if ( !OverlayAircraft ) {
				textformat |= TextFormatFlags.Bottom | TextFormatFlags.Right;
			} else {
				textformat |= TextFormatFlags.Top | TextFormatFlags.Left;
			}

			// 艦載機スロット表示の予測サイズ(2桁)
			Size sz_eststr = TextRenderer.MeasureText( "99", Font, new Size( int.MaxValue, int.MaxValue ), textformat );
			sz_eststr.Width -= (int)( Font.Size / 2.0 );

			// スロット1つ当たりのサイズ(右の余白含む)
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

					if ( Calculator.IsAircraft( slot.EquipmentID, true ) ) {

						if ( slot.AircraftMax == 0 ) {
							aircraftColor = AircraftColorDisabled;
						} else if ( slot.AircraftCurrent == 0 ) {
							aircraftColor = AircraftColorLost;
						} else if ( slot.AircraftCurrent < slot.AircraftMax ) {
							aircraftColor = AircraftColorDamaged;
						} else {
							aircraftColor = AircraftColorFull;
						}

					} else {
						if ( slot.AircraftMax == 0 )
							drawAircraftSlot = false;
					}

				} else if ( slot.AircraftMax == 0 ) {
					drawAircraftSlot = false;
				}


				if ( drawAircraftSlot ) {
					Rectangle textarea = new Rectangle( basearea.X + sz_unit.Width * slotindex, basearea.Y, sz_unit.Width - SlotMargin, sz_unit.Height );

					if ( OverlayAircraft ) {
						using ( SolidBrush b = new SolidBrush( Color.FromArgb( 0x80, 0xFF, 0xFF, 0xFF ) ) ) {
							e.Graphics.FillRectangle( b, new Rectangle( textarea.X, textarea.Y, sz_eststr.Width, sz_eststr.Height ) );
						}

					} else {

						if ( slot.AircraftCurrent < 10 ) {
							//1桁なら画像に近づける

							textarea.Width -= sz_eststr.Width / 2 - 1;

						} else if ( slot.AircraftCurrent >= 100 ) {
							//3桁以上ならオーバーレイを入れる

							Size sz_realstr = TextRenderer.MeasureText( slot.AircraftCurrent.ToString(), Font, new Size( int.MaxValue, int.MaxValue ), textformat );
							sz_realstr.Width -= (int)( Font.Size / 2.0 );

							using ( SolidBrush b = new SolidBrush( Color.FromArgb( 0x80, 0xFF, 0xFF, 0xFF ) ) ) {
								e.Graphics.FillRectangle( b, new Rectangle(
									textarea.X + sz_unit.Width - sz_realstr.Width,
									textarea.Y + sz_unit.Height - sz_realstr.Height,
									sz_realstr.Width, sz_realstr.Height ) );
							}

						}

					}


					TextRenderer.DrawText( e.Graphics, slot.AircraftCurrent.ToString(), Font, textarea, aircraftColor, textformat );
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
			sz_eststr.Width -= (int)( Font.Size / 2.0 );

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
