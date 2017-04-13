using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwfExtractor;
using System.Windows.Forms;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Resource;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ElectronicObserver.Utility {

	public class FleetImageGenerator {

		protected static readonly Size MaxValueSize = new Size( int.MaxValue, int.MaxValue );

		protected static readonly Size ShipBannerSize = new Size( 160, 40 );
		protected static readonly Size ShipCardSize = new Size( 218, 300 );
		protected static readonly Size ShipCutinSize = new Size( 665, 121 );
		protected static readonly Size ShipNameSize = new Size( 436, 63 );
		protected static readonly Size ShipSupplySize = new Size( 474, 47 );

		protected static readonly Size EquipmentCardSize = new Size( 260, 260 );
		protected static readonly Size EquipmentIconSize = new Size( 16, 16 );

		protected static readonly int ShipBannerNormalID = 1;
		protected static readonly int ShipBannerDamagedID = 3;
		protected static readonly int ShipCardNormalID = 5;
		protected static readonly int ShipCardDamagedID = 7;
		protected static readonly int ShipCutinNormalID = 21;
		protected static readonly int ShipCutinDamagedID = 23;
		protected static readonly int ShipNameID = 25;
		protected static readonly int ShipSupplyNormalID = 27;
		protected static readonly int ShipSupplyDamagedID = 29;




		public static Bitmap GenerateImageTest() {
			throw new NotImplementedException();
		}


		protected static Size RenderShipTest( Bitmap bmp, Graphics g, FleetImageArgument args, Point origin, ShipData ship ) {

			var formatMiddleLeft = GetTextFormat( ContentAlignment.MiddleLeft );
			var formatMiddleCenter = GetTextFormat( ContentAlignment.MiddleCenter );
			var formatMiddleRight = GetTextFormat( ContentAlignment.MiddleRight );

			Color mainColor = Color.FromArgb( 0xf0, 0xf0, 0xf0 );
			Color parameterNameColor = Color.Teal;
			Color aircraftEnableColor = mainColor;
			Color aircraftDisableColor = Color.Gray;

			Size shipNameSize = TextRenderer.MeasureText( g, "千代田航改二", args.LargeFont, MaxValueSize, formatMiddleLeft );
			Size equipmentNameSize = TextRenderer.MeasureText( g, "三式戦 飛燕一型丁", args.MediumFont, MaxValueSize, formatMiddleLeft );
			Size parameterSize = TextRenderer.MeasureText( g, "999", args.MediumFont, MaxValueSize, formatMiddleRight );
			Size levelSize = TextRenderer.MeasureText( g, "Lv", args.SmallFont, MaxValueSize, formatMiddleLeft );
			Size parameterNameSize = TextRenderer.MeasureText( g, "耐久", args.SmallFont, MaxValueSize, formatMiddleCenter );

			Size nameAreaSize = new Size( shipNameSize.Width + levelSize.Width + parameterSize.Width, Max( shipNameSize.Height, levelSize.Height, parameterSize.Height ) );
			Size equipmentAreaUnitSize = new Size( parameterSize.Width + EquipmentIconSize.Width + equipmentNameSize.Width, Max( parameterSize.Height, EquipmentIconSize.Height, equipmentNameSize.Height ) );
			Size equipmentAreaSize = new Size( equipmentAreaUnitSize.Width, equipmentAreaUnitSize.Height * 6 );
			Size parameterAreaUnitSize = new Size( EquipmentIconSize.Width + parameterNameSize.Width + parameterSize.Width, Max( EquipmentIconSize.Height + parameterNameSize.Height + parameterSize.Height ) );
			Size parameterAreaSize = new Size( parameterAreaUnitSize.Width * 2, parameterAreaUnitSize.Height * 6 );

			Size leftPaneSize = new Size( Max( nameAreaSize.Width, equipmentAreaSize.Width, parameterSize.Width ), nameAreaSize.Height + equipmentAreaSize.Height + parameterSize.Height );

			Size entireSize = new Size( leftPaneSize.Width + ShipCardSize.Width, Max( leftPaneSize.Height, ShipCardSize.Height ) );


			Point ptr = new Point( origin.X, origin.Y );
			TextRenderer.DrawText( g, ship.Name, args.LargeFont, new Rectangle( ptr.X, ptr.Y, shipNameSize.Width, nameAreaSize.Height ), mainColor, formatMiddleLeft );
			ptr.X += leftPaneSize.Width - levelSize.Width - parameterSize.Width;
			TextRenderer.DrawText( g, "Lv", args.SmallFont, new Rectangle( ptr.X, ptr.Y, levelSize.Width, nameAreaSize.Height ), parameterNameColor, formatMiddleLeft );
			ptr.X -= levelSize.Width;
			TextRenderer.DrawText( g, ship.Level.ToString(), args.SmallFont, new Rectangle( ptr.X, ptr.Y, parameterSize.Width, nameAreaSize.Height ), mainColor, formatMiddleRight );

			ptr.X = origin.X;
			ptr.Y += nameAreaSize.Height;


			for ( int i = 0; i < 6; i++ ) {

				int aircraftCurrent, aircraftMax;
				EquipmentData eq;
				bool isInvalid;

				if ( i < 5 ) {
					aircraftCurrent = ship.Aircraft[i];
					aircraftMax = ship.MasterShip.Aircraft[i];
					eq = ship.SlotInstance[i];
					isInvalid = i >= ship.SlotSize;
				} else {
					aircraftCurrent = aircraftMax = 0;
					eq = ship.ExpansionSlotInstance;
					isInvalid = ship.IsExpansionSlotAvailable;
				}

				ptr.X = origin.X;

				// aircraft slot
				if ( !isInvalid ) {
					if ( aircraftMax > 0 ) {
						Color col;
						if ( Calculator.IsAircraft( eq == null ? -1 : eq.EquipmentID, true, true ) ) {
							col = aircraftEnableColor;
						} else {
							col = aircraftDisableColor;
						}

						TextRenderer.DrawText( g, aircraftMax.ToString(), args.SmallFont, new Rectangle( ptr.X, ptr.Y, parameterSize.Width, equipmentAreaUnitSize.Height ), col, formatMiddleRight );
					}

					ptr.X += parameterSize.Width;
				}


				// icon
				{
					int index;

					if ( eq == null ) {
						if ( isInvalid )
							index = (int)ResourceManager.EquipmentContent.Locked;
						else
							index = (int)ResourceManager.EquipmentContent.Nothing;
					} else if ( (int)ResourceManager.EquipmentContent.Nothing < eq.MasterEquipment.IconType && eq.MasterEquipment.IconType < (int)ResourceManager.EquipmentContent.Locked ) {
						index = eq.MasterEquipment.IconType;
					} else {
						index = (int)ResourceManager.EquipmentContent.Unknown;
					}

					g.DrawImage( ResourceManager.Instance.Equipments.Images[index], ptr.X, ptr.Y + equipmentAreaUnitSize.Height / 2 - EquipmentIconSize.Height / 2 );

					ptr.X += EquipmentIconSize.Width;
				}

				// equipment name
				{
					TextRenderer.DrawText( g, eq.Name, args.MediumFont, new Rectangle( ptr.X, ptr.Y, equipmentNameSize.Width, equipmentAreaUnitSize.Height ), mainColor, formatMiddleRight );
				}

				ptr.Y += parameterAreaUnitSize.Height;
			}


			ptr.X = origin.X;


			var parameterList = new[] { 
				new {
					image = (int)ResourceManager.IconContent.ParameterHP,
					name = "耐久",
					value = ship.HPMax.ToString(),
				},
				new {
					image = (int)ResourceManager.IconContent.ParameterFirepower,
					name = "火力",
					value = ship.FirepowerTotal.ToString(),
				},
				new {
					image = (int)ResourceManager.IconContent.ParameterArmor,
					name = "装甲",
					value = ship.ArmorTotal.ToString(),
				},
				new {
					image = (int)ResourceManager.IconContent.ParameterTorpedo,
					name = "雷装",
					value = ship.TorpedoTotal.ToString(),
				},
				new {
					image = (int)ResourceManager.IconContent.ParameterEvasion,
					name = "回避",
					value = ship.EvasionTotal.ToString(),
				},
				new {
					image = (int)ResourceManager.IconContent.ParameterAA,
					name = "対空",
					value = ship.AATotal.ToString(),
				},
				new {
					image = (int)ResourceManager.IconContent.ParameterAircraft,
					name = "搭載",
					value = ship.MasterShip.AircraftTotal.ToString(),
				},
				new {
					image = (int)ResourceManager.IconContent.ParameterASW,
					name = "対潜",
					value = ship.ASWTotal.ToString(),
				},
				new {
					image = (int)ResourceManager.IconContent.ParameterSpeed,
					name = "速力",
					value = Constants.GetSpeed( ship.Speed ),
				},
				new {
					image = (int)ResourceManager.IconContent.ParameterLOS,
					name = "索敵",
					value = ship.LOSTotal.ToString(),
				},
				new {
					image = (int)ResourceManager.IconContent.ParameterRange,
					name = "射程",
					value = Constants.GetRange( ship.Range ),
				},
				new {
					image = (int)ResourceManager.IconContent.ParameterLuck,
					name = "運",
					value = ship.LuckTotal.ToString(),
				},
			};


			for ( int i = 0; i < parameterList.Length; i++ ) {
				Point pttr = new Point( ptr.X + parameterAreaUnitSize.Width * ( i % 2 ), ptr.Y + parameterAreaUnitSize.Height * ( i / 2 ) );

				g.DrawImage( ResourceManager.Instance.Icons.Images[parameterList[i].image], pttr.X, pttr.Y + parameterAreaUnitSize.Height / 2 - EquipmentIconSize.Height / 2 );
				pttr.X += EquipmentIconSize.Width;

				TextRenderer.DrawText( g, parameterList[i].name, args.SmallFont, new Rectangle( pttr.X, pttr.Y, parameterNameSize.Width, parameterAreaUnitSize.Height ), parameterNameColor, formatMiddleCenter );
				pttr.X += parameterNameSize.Width;

				TextRenderer.DrawText( g, parameterList[i].value, args.MediumFont, new Rectangle( pttr.X, pttr.Y, parameterSize.Width, parameterAreaUnitSize.Height ), mainColor, formatMiddleRight );
			}


			ptr.X = origin.X + leftPaneSize.Width;
			ptr.Y = origin.Y;

			try {
				string shipSwfPath = Utility.Configuration.Config.Connection.SaveDataPath + @"\resources\swf\ships\" + ship.MasterShip.ResourceName + ".swf";
				if ( System.IO.File.Exists( shipSwfPath ) ) {

					var shipSwf = new SwfParser();
					shipSwf.Parse( shipSwfPath );

					var imgtag = shipSwf.FindTags<SwfExtractor.Tags.ImageTag>().FirstOrDefault( t => t.CharacterID == ShipCardNormalID );
					using ( var shipImage = imgtag.ExtractImage() ) {
						g.DrawImage( shipImage, new Rectangle( ptr.X, ptr.Y, ShipCardSize.Width, ShipCardSize.Height ) );
					}
				}
			} catch ( Exception ) {
			}

			return entireSize;
		}




		public static Bitmap GenerateTestBitmap( FleetImageArgument args ) {

			var formatMiddleLeft = GetStringFormat( ContentAlignment.MiddleLeft );
			var formatMiddleCenter = GetStringFormat( ContentAlignment.MiddleCenter );
			var formatMiddleRight = GetStringFormat( ContentAlignment.MiddleRight );

			Color backgroundColor = Color.FromArgb( 0xff, 0xff, 0xff );
			Color mainTextColor = Color.FromArgb( 0x0f, 0x0f, 0x0f );
			Color subTextColor = Color.FromArgb( 0x00, 0x88, 0x88 );
			Color shadowColor = Color.FromArgb( 0x88, 0x88, 0x88 );
			Color disabledColor = Color.FromArgb( 0xaa, 0xaa, 0xaa );
			Color aircraftLevelLowColor = Color.FromArgb( 0x00, 0xff, 0xff );
			Color aircraftLevelHighColor = Color.FromArgb( 0xff, 0x88, 0x00 );

			var mainTextBrush = new SolidBrush( mainTextColor );
			var subTextBrush = new SolidBrush( subTextColor );
			var shadowBrush = new SolidBrush( shadowColor );
			var disabledBrush = new SolidBrush( disabledColor );
			var aircraftLevelLowBrush = new SolidBrush( aircraftLevelLowColor );
			var aircraftLevelHighBrush = new SolidBrush( aircraftLevelHighColor );

			var linePen = new Pen( subTextColor );


			string fleetAirSuperiorityTitle = "制空戦力";
			string fleetSearchingAbilityTitle = "索敵能力";

			// for measure space of strings
			Bitmap preimage = new Bitmap( 1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb );
			Graphics preg = Graphics.FromImage( preimage );

			// Size Calculation
			Size titleSize = string.IsNullOrWhiteSpace( args.Title ) ? Size.Empty : MeasureString( preg, args.Title, args.TitleFont, MaxValueSize, formatMiddleCenter );
			Size commentSize = string.IsNullOrWhiteSpace( args.Comment ) ? Size.Empty : MeasureString( preg, args.Comment, args.MediumFont, MaxValueSize, formatMiddleLeft );

			Size fleetNameSize = MeasureString( preg, "大正義日独伊三国褐色同盟", args.LargeFont, MaxValueSize, formatMiddleLeft );		// kanji 12 char
			Size fleetAirSuperiorityTitleSize = MeasureString( preg, fleetAirSuperiorityTitle, args.SmallFont, MaxValueSize, formatMiddleLeft );
			Size fleetAirSuperiorityValueEstimatedSize = MeasureString( preg, "8888", args.MediumDigitFont, MaxValueSize, formatMiddleRight );
			Size fleetSearchingAbilityTitleSize = MeasureString( preg, fleetSearchingAbilityTitle, args.SmallFont, MaxValueSize, formatMiddleLeft );
			Size fleetSearchingAbilityValueEstimatedSize = MeasureString( preg, "-888.88", args.MediumDigitFont, MaxValueSize, formatMiddleRight );

			Size shipIndexSize = MeasureString( preg, "#4:", args.MediumDigitFont, MaxValueSize, formatMiddleLeft );
			Size shipNameSize = MeasureString( preg, "千代田航改二", args.LargeFont, MaxValueSize, formatMiddleLeft );		// kanji 6 char
			Size equipmentNameSize = MeasureString( preg, "三式戦 飛燕一型丁", args.MediumFont, MaxValueSize, formatMiddleLeft );		// kanji 9 char
			Size mediumDigit3Size = MeasureString( preg, "888", args.MediumDigitFont, MaxValueSize, formatMiddleRight );
			Size smallDigit3Size = MeasureString( preg, "888", args.SmallDigitFont, MaxValueSize, formatMiddleRight );
			Size levelSize = MeasureString( preg, "Lv.", args.SmallDigitFont, MaxValueSize, formatMiddleLeft );
			Size parameterNameSize = MeasureString( preg, "耐久", args.SmallFont, MaxValueSize, formatMiddleCenter );
			Size parameterValueSize = Max( MeasureString( preg, "高速+", args.MediumFont, MaxValueSize, formatMiddleRight ), mediumDigit3Size );
			Size equipmentLevelSize = MeasureString( preg, "+10", args.SmallDigitFont, MaxValueSize, formatMiddleRight );

			Size parameterAreaInnerMargin = new Size( 16, 0 );
			Size fleetParameterAreaInnerMargin = new Size( 16, 0 );
			Padding shipNameAreaMargin = new Padding( 0, 0, 0, 2 );
			Padding equipmentAreaMargin = new Padding( 0, 0, 0, 2 );
			Padding parameterAreaMargin = new Padding( 0, 0, 0, 2 );
			Padding shipValuePaneMargin = new Padding();
			Padding shipImagePaneMargin = new Padding();
			Padding shipPaneUnitMargin = new Padding( 2 );
			Padding shipPaneMargin = new Padding();
			Padding fleetParameterAreaMargin = new Padding( 8, 0, 0, 4 );
			Padding fleetPaneUnitMargin = new Padding( 4 );
			Padding fleetPaneMargin = new Padding();
			Padding titleMargin = new Padding( 0, 0, 0, 2 );
			Padding commentMargin = new Padding( 2 );
			Padding commentPadding = new Padding( 2 );
			Padding entireMargin = new Padding();
			int lineMargin = 4;

			Size shipNameAreaSize = SumWidthMaxHeight( shipIndexSize, shipNameSize, levelSize, mediumDigit3Size );
			Size equipmentAreaUnitSize = SumWidthMaxHeight( smallDigit3Size, EquipmentIconSize, equipmentNameSize, EquipmentIconSize, equipmentLevelSize );
			Size equipmentAreaSize = new Size( equipmentAreaUnitSize.Width, equipmentAreaUnitSize.Height * 6 );
			Size parameterAreaUnitSize = SumWidthMaxHeight( EquipmentIconSize, parameterNameSize, parameterValueSize );
			Size parameterAreaSize = new Size( parameterAreaUnitSize.Width * 2, parameterAreaUnitSize.Height * 6 ) + parameterAreaInnerMargin;

			Size shipValuePaneSize = MaxWidthSumHeight( shipNameAreaSize + shipNameAreaMargin.Size, equipmentAreaSize + equipmentAreaMargin.Size, parameterAreaSize + parameterAreaMargin.Size );
			Size shipImagePaneSize = ShipCardSize;
			Size shipPaneUnitSize = SumWidthMaxHeight( shipValuePaneSize + shipValuePaneMargin.Size, shipImagePaneSize + shipImagePaneMargin.Size );
			Size shipPaneSize = new Size(
				( shipPaneUnitSize.Width + shipPaneUnitMargin.Horizontal ) * Math.Min( args.HorizontalShipCount, 6 ),
				( shipPaneUnitSize.Height + shipPaneUnitMargin.Vertical ) * (int)Math.Ceiling( 6.0 / args.HorizontalShipCount ) );

			Size fleetParameterAreaSize = SumWidthMaxHeight(
				EquipmentIconSize, fleetAirSuperiorityTitleSize, fleetAirSuperiorityValueEstimatedSize, fleetParameterAreaInnerMargin,
				EquipmentIconSize, fleetSearchingAbilityTitleSize, fleetSearchingAbilityValueEstimatedSize );

			Size fleetPaneUnitSize;
			bool isFleetNameAndParametersAreSameLine = fleetNameSize.Width + fleetParameterAreaSize.Width + fleetParameterAreaMargin.Horizontal < shipPaneSize.Width + shipPaneMargin.Horizontal;
			if ( isFleetNameAndParametersAreSameLine ) {
				fleetPaneUnitSize = MaxWidthSumHeight( SumWidthMaxHeight( fleetNameSize, fleetParameterAreaSize + fleetParameterAreaMargin.Size ), shipPaneSize + shipPaneMargin.Size );
			} else {
				fleetPaneUnitSize = MaxWidthSumHeight( fleetNameSize, fleetParameterAreaSize + fleetParameterAreaMargin.Size, shipPaneSize + shipPaneMargin.Size );
			}

			Size fleetPaneSize = new Size( ( fleetPaneUnitSize.Width + fleetPaneUnitMargin.Horizontal ) * Math.Min( args.HorizontalFleetCount, args.FleetIDs.Length ),
				( fleetPaneUnitSize.Height + fleetPaneUnitMargin.Vertical ) * (int)Math.Ceiling( (double)args.FleetIDs.Length / args.HorizontalFleetCount ) );

			Size commentAreaSize = commentSize + commentPadding.Size;

			Size entireSize = MaxWidthSumHeight( titleSize + titleMargin.Size, fleetPaneSize + fleetPaneMargin.Size, commentAreaSize + commentMargin.Size );

			// anchor
			shipNameSize.Width = shipValuePaneSize.Width - shipIndexSize.Width - levelSize.Width - mediumDigit3Size.Width;
			shipNameAreaSize.Width = shipValuePaneSize.Width;
			equipmentNameSize.Width = shipValuePaneSize.Width - smallDigit3Size.Width - EquipmentIconSize.Width - EquipmentIconSize.Width - equipmentLevelSize.Width;
			equipmentAreaUnitSize.Width = shipValuePaneSize.Width;
			Size equipmentNameSizeExtended = SumWidthMaxHeight( equipmentNameSize, EquipmentIconSize, equipmentLevelSize );

			var equipmentNameBrush = new LinearGradientBrush( new Rectangle( 0, 0, equipmentNameSize.Width * 2 + EquipmentIconSize.Width * 2 + equipmentLevelSize.Width, equipmentAreaUnitSize.Height ), Color.Black, Color.Black, LinearGradientMode.Horizontal );		// color is ignored
			{
				var blend = new ColorBlend();
				blend.Positions = new[] { 0f, (float)( equipmentNameSizeExtended.Width - EquipmentIconSize.Width - EquipmentIconSize.Width / 2 ) / equipmentNameBrush.Rectangle.Width, (float)( equipmentNameSizeExtended.Width - EquipmentIconSize.Width / 2 ) / equipmentNameBrush.Rectangle.Width, 1f };
				blend.Colors = new[] { mainTextColor, mainTextColor, Color.FromArgb( 0, mainTextColor ), Color.FromArgb( 0, mainTextColor ) };
				equipmentNameBrush.InterpolationColors = blend;
			}
			equipmentNameBrush.GammaCorrection = true;

			preg.Dispose();
			preimage.Dispose();


			var bitmap = new Bitmap( entireSize.Width + entireMargin.Horizontal, entireSize.Height + entireMargin.Vertical, System.Drawing.Imaging.PixelFormat.Format32bppArgb );
			using ( var g = Graphics.FromImage( bitmap ) ) {

				g.Clear( backgroundColor );
				if ( !string.IsNullOrEmpty( args.BackgroundImagePath ) && System.IO.File.Exists( args.BackgroundImagePath ) ) {
					try {
						using ( var backgroundImage = new Bitmap( args.BackgroundImagePath ) ) {
							using ( var backgroundBrush = new TextureBrush( backgroundImage, WrapMode.Tile ) ) {
								g.FillRectangle( backgroundBrush, new Rectangle( 0, 0, bitmap.Width, bitmap.Height ) );
							}
						}

					} catch ( Exception ) {
					}
				}


				Point masterPointer = new Point( entireMargin.Left, entireMargin.Top );

				// title
				if ( !titleSize.IsEmpty ) {
					g.DrawString( args.Title, args.TitleFont, shadowBrush, new Rectangle( masterPointer + GetAlignmentOffset( ContentAlignment.TopCenter, titleSize, entireSize ) + new Size( 2, 2 ), titleSize ), formatMiddleCenter );
					g.DrawString( args.Title, args.TitleFont, mainTextBrush, new Rectangle( masterPointer + GetAlignmentOffset( ContentAlignment.TopCenter, titleSize, entireSize ), titleSize ), formatMiddleCenter );
				}
				masterPointer.Y += titleSize.Height;


				for ( int fleetIndex = 0; fleetIndex < args.FleetIDs.Length; fleetIndex++ ) {
					int fleetID = args.FleetIDs[fleetIndex];
					FleetData fleet = KCDatabase.Instance.Fleet[fleetID];
					Point fleetPointerOrigin = masterPointer + new Size(
						( fleetPaneUnitSize.Width + fleetPaneUnitMargin.Horizontal ) * ( fleetIndex % args.HorizontalFleetCount ) + fleetPaneUnitMargin.Left,
						( fleetPaneUnitSize.Height + fleetPaneUnitMargin.Vertical ) * ( fleetIndex / args.HorizontalFleetCount ) + fleetPaneUnitMargin.Top );
					Point fleetPointer = fleetPointerOrigin;

					if ( fleet == null )
						continue;

					// fleet name
					g.DrawString( fleet.Name, args.LargeFont, mainTextBrush, new Rectangle( fleetPointer, fleetNameSize ), formatMiddleLeft );
					if ( isFleetNameAndParametersAreSameLine ) {
						fleetPointer.X += fleetNameSize.Width;
						fleetPointer.Y += fleetNameSize.Height - fleetParameterAreaSize.Height;
					} else {
						fleetPointer.Y += fleetNameSize.Height;
					}

					// fleet specs
					fleetPointer.X += fleetParameterAreaMargin.Left;
					fleetPointer.Y += fleetParameterAreaMargin.Top;
					{	// fighter power
						var iconpos = fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, EquipmentIconSize, fleetParameterAreaSize );
						g.DrawImage( ResourceManager.Instance.Equipments.Images[(int)ResourceManager.EquipmentContent.CarrierBasedFighter], iconpos.X, iconpos.Y, EquipmentIconSize.Width, EquipmentIconSize.Height );
						fleetPointer.X += EquipmentIconSize.Width;

						g.DrawString( fleetAirSuperiorityTitle, args.SmallFont, subTextBrush, new Rectangle( fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, fleetAirSuperiorityTitleSize, fleetParameterAreaSize ), fleetAirSuperiorityTitleSize ), formatMiddleLeft );
						fleetPointer.X += fleetAirSuperiorityTitleSize.Width;

						Size paramValueSize = MeasureString( g, fleet.GetAirSuperiority().ToString(), args.MediumDigitFont, MaxValueSize, formatMiddleLeft );
						g.DrawString( fleet.GetAirSuperiority().ToString(), args.MediumDigitFont, mainTextBrush, new Rectangle( fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, paramValueSize, fleetParameterAreaSize ), paramValueSize ), formatMiddleLeft );
						fleetPointer.X += paramValueSize.Width + fleetParameterAreaInnerMargin.Width;
					}
					{	// searching ability
						var iconpos = fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, EquipmentIconSize, fleetParameterAreaSize );
						g.DrawImage( ResourceManager.Instance.Equipments.Images[(int)ResourceManager.EquipmentContent.CarrierBasedRecon], iconpos.X, iconpos.Y, EquipmentIconSize.Width, EquipmentIconSize.Height );
						fleetPointer.X += EquipmentIconSize.Width;

						g.DrawString( fleetSearchingAbilityTitle, args.SmallFont, subTextBrush, new Rectangle( fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, fleetSearchingAbilityTitleSize, fleetParameterAreaSize ), fleetSearchingAbilityTitleSize ), formatMiddleLeft );
						fleetPointer.X += fleetAirSuperiorityTitleSize.Width;

						Size paramValueSize = MeasureString( g, fleet.GetSearchingAbilityString(), args.MediumDigitFont, MaxValueSize, formatMiddleLeft );
						g.DrawString( fleet.GetSearchingAbilityString(), args.MediumDigitFont, mainTextBrush, new Rectangle( fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, paramValueSize, fleetParameterAreaSize ), paramValueSize ), formatMiddleLeft );
						fleetPointer.X += paramValueSize.Width + fleetParameterAreaInnerMargin.Width;
					}

					fleetPointer.X = fleetPointerOrigin.X;
					fleetPointer.Y += fleetParameterAreaSize.Height;

					g.DrawLine( linePen, fleetPointer + new Size( lineMargin, 0 ), fleetPointer + new Size( shipPaneSize.Width - lineMargin, 0 ) );
					fleetPointer.Y += fleetParameterAreaMargin.Bottom;


					for ( int shipIndex = 0; shipIndex < 6; shipIndex++ ) {
						ShipData ship = fleet.MembersInstance[shipIndex];
						Point shipPointerOrigin = fleetPointer + new Size(
							( shipPaneUnitSize.Width + shipPaneUnitMargin.Horizontal ) * ( shipIndex % args.HorizontalShipCount ) + shipPaneUnitMargin.Left,
							( shipPaneUnitSize.Height + shipPaneUnitMargin.Vertical ) * ( shipIndex / args.HorizontalShipCount ) + shipPaneUnitMargin.Top );
						Point shipPointer = shipPointerOrigin;


						if ( ship == null )
							continue;

						g.DrawString( string.Format( "#{0}:", shipIndex + 1 ), args.MediumDigitFont, subTextBrush, new Rectangle( shipPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, shipIndexSize, shipNameAreaSize ), shipIndexSize ), formatMiddleLeft );
						shipPointer.X += shipIndexSize.Width;

						// ship name
						g.DrawString( ship.Name, args.LargeFont, mainTextBrush, new Rectangle( shipPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, shipNameSize, shipNameAreaSize ), shipNameSize ), formatMiddleLeft );
						shipPointer.X += shipNameSize.Width;

						// level
						g.DrawString( "Lv.", args.SmallDigitFont, subTextBrush, new Rectangle( shipPointer + GetAlignmentOffset( ContentAlignment.BottomLeft, levelSize, shipNameAreaSize ), levelSize ), formatMiddleLeft );
						shipPointer.X += levelSize.Width;
						g.DrawString( ship.Level.ToString(), args.MediumDigitFont, mainTextBrush, new Rectangle( shipPointer + GetAlignmentOffset( ContentAlignment.BottomLeft, mediumDigit3Size, shipNameAreaSize ), mediumDigit3Size ), formatMiddleRight );

						shipPointer.X = shipPointerOrigin.X;
						shipPointer.Y += shipNameAreaSize.Height;
						g.DrawLine( linePen, shipPointer + new Size( lineMargin, 0 ), shipPointer + new Size( shipValuePaneSize.Width - lineMargin, 0 ) );
						shipPointer.Y += shipNameAreaMargin.Bottom;

						// equipments
						for ( int equipmentIndex = 0; equipmentIndex < 6; equipmentIndex++ ) {
							EquipmentData eq = ship.AllSlotInstance[equipmentIndex];
							Point equipmentPointer = shipPointer + new Size( 0, equipmentAreaUnitSize.Height * equipmentIndex );


							int aircraftMax = equipmentIndex < 5 ? ship.MasterShip.Aircraft[equipmentIndex] : 0;

							if ( aircraftMax > 0 ) {
								Brush aircraftBrush;
								if ( eq != null && Calculator.IsAircraft( eq.EquipmentID, true, true ) ) {
									aircraftBrush = mainTextBrush;
								} else {
									aircraftBrush = disabledBrush;
								}

								g.DrawString( aircraftMax.ToString(), args.SmallDigitFont, aircraftBrush, new Rectangle( equipmentPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, smallDigit3Size, equipmentAreaUnitSize ), smallDigit3Size ), formatMiddleRight );
							}
							equipmentPointer.X += smallDigit3Size.Width;

							bool isOutOfSlot = equipmentIndex >= ship.SlotSize && !( equipmentIndex == 5 && ship.IsExpansionSlotAvailable );

							Size equipmentIconOffset = GetAlignmentOffset( ContentAlignment.MiddleLeft, EquipmentIconSize, equipmentAreaUnitSize );
							g.DrawImage( GetEquipmentIcon( eq != null ? eq.EquipmentID : -1, isOutOfSlot ),
								equipmentPointer.X + equipmentIconOffset.Width, equipmentPointer.Y + equipmentIconOffset.Height, EquipmentIconSize.Width, EquipmentIconSize.Height );
							equipmentPointer.X += EquipmentIconSize.Width;

							string equipmentName;
							if ( eq != null ) {
								equipmentName = eq.Name;
							} else if ( isOutOfSlot ) {
								equipmentName = "";
							} else {
								equipmentName = "(なし)";
							}
							equipmentNameBrush.ResetTransform();
							if ( eq != null && eq.AircraftLevel > 0 ) {
								equipmentNameBrush.TranslateTransform( equipmentPointer.X - EquipmentIconSize.Width - equipmentLevelSize.Width, 0 );
							} else if ( eq != null && eq.Level > 0 ) {
								equipmentNameBrush.TranslateTransform( equipmentPointer.X - EquipmentIconSize.Width, 0 );
							} else {
								equipmentNameBrush.TranslateTransform( equipmentPointer.X, 0 );
							}
							g.DrawString( equipmentName, args.MediumFont, equipmentNameBrush, new Rectangle( equipmentPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, equipmentNameSizeExtended, equipmentAreaUnitSize ), equipmentNameSizeExtended ), formatMiddleLeft );
							equipmentPointer.X += equipmentNameSize.Width;

							if ( eq != null ) {

								if ( 0 <= eq.AircraftLevel && eq.AircraftLevel <= 7 ) {
									var point = equipmentPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, EquipmentIconSize, equipmentAreaUnitSize );
									g.DrawImage( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.AircraftLevel0 + eq.AircraftLevel], point.X, point.Y, EquipmentIconSize.Width, EquipmentIconSize.Height );
								}
								equipmentPointer.X += EquipmentIconSize.Width;

								if ( eq.Level > 0 ) {
									g.DrawString( "+" + eq.Level, args.SmallDigitFont, subTextBrush, new Rectangle( equipmentPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, equipmentLevelSize, equipmentAreaUnitSize ), equipmentLevelSize ), formatMiddleRight );
								}

							}

						}
						shipPointer.Y += equipmentAreaSize.Height;
						g.DrawLine( linePen, shipPointer + new Size( lineMargin, 0 ), shipPointer + new Size( shipValuePaneSize.Width - lineMargin, 0 ) );
						shipPointer.Y += equipmentAreaMargin.Bottom;


						// parameters
						for ( int parameterIndex = 0; parameterIndex < ParameterDataList.Length; parameterIndex++ ) {
							Point parameterPointer = shipPointer + GetAlignmentOffset( ContentAlignment.TopCenter, parameterAreaSize, shipValuePaneSize ) + new Size( ( parameterAreaUnitSize.Width + parameterAreaInnerMargin.Width ) * ( parameterIndex % 2 ), ( parameterAreaUnitSize.Height + parameterAreaInnerMargin.Height ) * ( parameterIndex / 2 ) );
							var paramdata = ParameterDataList[parameterIndex];

							Size parameterIconOffset = GetAlignmentOffset( ContentAlignment.MiddleLeft, EquipmentIconSize, parameterAreaUnitSize );
							g.DrawImage( paramdata.Icon, parameterPointer.X + parameterIconOffset.Width, parameterPointer.Y + parameterIconOffset.Height,
								EquipmentIconSize.Width, EquipmentIconSize.Height );
							parameterPointer.X += EquipmentIconSize.Width;

							g.DrawString( paramdata.Name, args.SmallFont, subTextBrush, new Rectangle( parameterPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, parameterNameSize, parameterAreaUnitSize ), parameterNameSize ), formatMiddleCenter );
							parameterPointer.X += parameterNameSize.Width;

							g.DrawString( paramdata.ValueSelector( ship ), paramdata.IsCharacter ? args.MediumFont : args.MediumDigitFont, mainTextBrush,
								new Rectangle( parameterPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, parameterValueSize, parameterAreaUnitSize ), parameterValueSize ), formatMiddleRight );

						}
						shipPointer.Y += parameterAreaSize.Height;
						g.DrawLine( linePen, shipPointer + new Size( lineMargin, 0 ), shipPointer + new Size( shipValuePaneSize.Width - lineMargin, 0 ) );


						// ship image
						shipPointer.X = shipPointerOrigin.X + shipValuePaneSize.Width;
						shipPointer.Y = shipPointerOrigin.Y;
						DrawShipSwfImage( g, ship.MasterShip.ResourceName, args.ReflectDamageGraphic && ship.HPRate <= 0.5 ? ShipCardDamagedID : ShipCardNormalID, shipPointer.X, shipPointer.Y, ShipCardSize );
					}
				}

				masterPointer.Y += fleetPaneSize.Height;
				g.DrawLine( linePen, masterPointer + new Size( lineMargin, 0 ), masterPointer + new Size( fleetPaneSize.Width - commentMargin.Horizontal - lineMargin, 0 ) );


				if ( !commentSize.IsEmpty ) {
					var commentPointer = masterPointer + new Size( commentMargin.Left, commentMargin.Top );
					commentPointer += new Size( commentPadding.Left, commentPadding.Top );
					g.DrawString( args.Comment, args.MediumFont, mainTextBrush, new Rectangle( commentPointer, commentSize ), formatMiddleLeft );
				}
			}


			if ( args.AvoidTwitterDeterioration ) {
				// 不透明ピクセルのみだと jpeg 化されてしまうため、1px だけわずかに透明にする
				Color temp = bitmap.GetPixel( bitmap.Width - 1, bitmap.Height - 1 );
				bitmap.SetPixel( bitmap.Width - 1, bitmap.Height - 1, Color.FromArgb( 252, temp.R, temp.G, temp.B ) );
			}


			mainTextBrush.Dispose();
			shadowBrush.Dispose();
			disabledBrush.Dispose();
			subTextBrush.Dispose();
			aircraftLevelLowBrush.Dispose();
			aircraftLevelHighBrush.Dispose();

			equipmentNameBrush.Dispose();

			linePen.Dispose();

			return bitmap;
		}





		public static Bitmap GenerateCutinBitmap( FleetImageArgument args ) {

			var formatMiddleLeft = GetStringFormat( ContentAlignment.MiddleLeft );
			var formatMiddleCenter = GetStringFormat( ContentAlignment.MiddleCenter );
			var formatMiddleRight = GetStringFormat( ContentAlignment.MiddleRight );

			Color backgroundColor = Color.FromArgb( 0xff, 0xff, 0xff );
			Color mainTextColor = Color.FromArgb( 0x0f, 0x0f, 0x0f );
			Color subTextColor = Color.FromArgb( 0x00, 0x88, 0x88 );
			Color shadowColor = Color.FromArgb( 0x88, 0x88, 0x88 );
			Color disabledColor = Color.FromArgb( 0xaa, 0xaa, 0xaa );
			Color aircraftLevelLowColor = Color.FromArgb( 0x00, 0xff, 0xff );
			Color aircraftLevelHighColor = Color.FromArgb( 0xff, 0x88, 0x00 );

			var mainTextBrush = new SolidBrush( mainTextColor );
			var subTextBrush = new SolidBrush( subTextColor );
			var shadowBrush = new SolidBrush( shadowColor );
			var disabledBrush = new SolidBrush( disabledColor );
			var aircraftLevelLowBrush = new SolidBrush( aircraftLevelLowColor );
			var aircraftLevelHighBrush = new SolidBrush( aircraftLevelHighColor );

			var linePen = new Pen( subTextColor );


			string fleetAirSuperiorityTitle = "制空戦力";
			string fleetSearchingAbilityTitle = "索敵能力";

			// for measure space of strings
			Bitmap preimage = new Bitmap( 1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb );
			Graphics preg = Graphics.FromImage( preimage );

			// Size Calculation
			Size titleSize = string.IsNullOrWhiteSpace( args.Title ) ? Size.Empty : MeasureString( preg, args.Title, args.TitleFont, MaxValueSize, formatMiddleCenter );
			Size commentSize = string.IsNullOrWhiteSpace( args.Comment ) ? Size.Empty : MeasureString( preg, args.Comment, args.MediumFont, MaxValueSize, formatMiddleLeft );

			Size fleetNameSize = MeasureString( preg, "大正義日独伊三国褐色同盟", args.LargeFont, MaxValueSize, formatMiddleLeft );		// kanji 12 char
			Size fleetAirSuperiorityTitleSize = MeasureString( preg, fleetAirSuperiorityTitle, args.SmallFont, MaxValueSize, formatMiddleLeft );
			Size fleetAirSuperiorityValueEstimatedSize = MeasureString( preg, "8888", args.MediumDigitFont, MaxValueSize, formatMiddleRight );
			Size fleetSearchingAbilityTitleSize = MeasureString( preg, fleetSearchingAbilityTitle, args.SmallFont, MaxValueSize, formatMiddleLeft );
			Size fleetSearchingAbilityValueEstimatedSize = MeasureString( preg, "-888.88", args.MediumDigitFont, MaxValueSize, formatMiddleRight );

			Size shipIndexSize = MeasureString( preg, "#4:", args.MediumDigitFont, MaxValueSize, formatMiddleLeft );
			Size equipmentNameSize = MeasureString( preg, "61cm五連装(酸素)魚雷", args.MediumFont, MaxValueSize, formatMiddleLeft );		// kanji 9 char
			Size mediumDigit3Size = MeasureString( preg, "888", args.MediumDigitFont, MaxValueSize, formatMiddleRight );
			Size smallDigit3Size = MeasureString( preg, "888", args.SmallDigitFont, MaxValueSize, formatMiddleRight );
			Size levelSize = MeasureString( preg, "Lv.", args.SmallDigitFont, MaxValueSize, formatMiddleLeft );
			Size equipmentLevelSize = MeasureString( preg, "+10", args.SmallDigitFont, MaxValueSize, formatMiddleRight );
			Rectangle shipNameImageAvailableArea = new Rectangle( 100, 0, ShipNameSize.Width - 124, ShipNameSize.Height - 16 );

			Size fleetParameterAreaInnerMargin = new Size( 16, 0 );
			Padding shipNameAreaMargin = new Padding( 0, 0, 0, 2 );
			Padding equipmentAreaMargin = new Padding( 0, 0, 0, 2 );
			Padding shipPaneUnitMargin = new Padding( 2 );
			Padding shipPaneMargin = new Padding();
			Padding fleetParameterAreaMargin = new Padding( 8, 0, 0, 4 );
			Padding fleetPaneUnitMargin = new Padding( 4 );
			Padding fleetPaneMargin = new Padding();
			Padding titleMargin = new Padding( 0, 0, 0, 2 );
			Padding commentMargin = new Padding( 2 );
			Padding commentPadding = new Padding( 2 );
			Padding entireMargin = new Padding();
			int lineMargin = 4;

			Size shipNameSize = shipNameImageAvailableArea.Size;
			Size shipNameAreaSize = SumWidthMaxHeight( shipIndexSize, shipNameSize, levelSize, mediumDigit3Size );
			Size equipmentAreaUnitSize = SumWidthMaxHeight( smallDigit3Size, EquipmentIconSize, equipmentNameSize, equipmentLevelSize );
			Size equipmentAreaSize = new Size( equipmentAreaUnitSize.Width, equipmentAreaUnitSize.Height * 6 );

			Size shipPaneUnitSize = new Size( Max( shipNameAreaSize.Width + equipmentAreaSize.Width, ShipCutinSize.Width ),
				Max( shipNameAreaSize.Height + ShipCutinSize.Height, equipmentAreaSize.Height ) ); // SumWidthMaxHeight( MaxWidthSumHeight( shipNameAreaSize, ShipCutinSize ), equipmentAreaSize + equipmentAreaMargin.Size );
			Size shipPaneSize = new Size(
				( shipPaneUnitSize.Width + shipPaneUnitMargin.Horizontal ) * Math.Min( args.HorizontalShipCount, 6 ),
				( shipPaneUnitSize.Height + shipPaneUnitMargin.Vertical ) * (int)Math.Ceiling( 6.0 / args.HorizontalShipCount ) );

			Size fleetParameterAreaSize = SumWidthMaxHeight(
				EquipmentIconSize, fleetAirSuperiorityTitleSize, fleetAirSuperiorityValueEstimatedSize, fleetParameterAreaInnerMargin,
				EquipmentIconSize, fleetSearchingAbilityTitleSize, fleetSearchingAbilityValueEstimatedSize );

			Size fleetPaneUnitSize;
			bool isFleetNameAndParametersAreSameLine = fleetNameSize.Width + fleetParameterAreaSize.Width + fleetParameterAreaMargin.Horizontal < shipPaneSize.Width + shipPaneMargin.Horizontal;
			if ( isFleetNameAndParametersAreSameLine ) {
				fleetPaneUnitSize = MaxWidthSumHeight( SumWidthMaxHeight( fleetNameSize, fleetParameterAreaSize + fleetParameterAreaMargin.Size ), shipPaneSize + shipPaneMargin.Size );
			} else {
				fleetPaneUnitSize = MaxWidthSumHeight( fleetNameSize, fleetParameterAreaSize + fleetParameterAreaMargin.Size, shipPaneSize + shipPaneMargin.Size );
			}

			Size fleetPaneSize = new Size( ( fleetPaneUnitSize.Width + fleetPaneUnitMargin.Horizontal ) * Math.Min( args.HorizontalFleetCount, args.FleetIDs.Length ),
				( fleetPaneUnitSize.Height + fleetPaneUnitMargin.Vertical ) * (int)Math.Ceiling( (double)args.FleetIDs.Length / args.HorizontalFleetCount ) );

			Size commentAreaSize = commentSize + commentPadding.Size;

			Size entireSize = MaxWidthSumHeight( titleSize + titleMargin.Size, fleetPaneSize + fleetPaneMargin.Size, commentAreaSize + commentMargin.Size );


			Size equipmentNameSizeExtended = SumWidthMaxHeight( equipmentNameSize, equipmentLevelSize );

			var equipmentNameBrush = new LinearGradientBrush( new Rectangle( 0, 0, equipmentNameSize.Width * 2 + equipmentLevelSize.Width, equipmentAreaUnitSize.Height ), Color.Black, Color.Black, LinearGradientMode.Horizontal );		// color is ignored
			{
				var blend = new ColorBlend();
				blend.Positions = new[] { 0f, (float)( equipmentNameSizeExtended.Width - EquipmentIconSize.Width ) / equipmentNameBrush.Rectangle.Width, (float)( equipmentNameSizeExtended.Width ) / equipmentNameBrush.Rectangle.Width, 1f };
				blend.Colors = new[] { mainTextColor, mainTextColor, Color.FromArgb( 0, mainTextColor ), Color.FromArgb( 0, mainTextColor ) };
				equipmentNameBrush.InterpolationColors = blend;
			}
			equipmentNameBrush.GammaCorrection = true;

			var shipImageMaskBrush = new LinearGradientBrush( new Rectangle( 0, 0, ShipCutinSize.Width, ShipCutinSize.Height ), Color.Black, Color.Black, LinearGradientMode.Horizontal );
			{
				var blend = new ColorBlend();
				blend.Positions = new[] { 0f, Math.Min( (float)( shipPaneUnitSize.Width - equipmentAreaSize.Width - 16 ) / ShipCutinSize.Width, 1f ), Math.Min( (float)( shipPaneUnitSize.Width - equipmentAreaSize.Width + 16 ) / ShipCutinSize.Width, 1f ), 1f };
				blend.Colors = new[] { Color.White, Color.White, Color.FromArgb( 0xff, 0x11, 0x11, 0x11 ), Color.FromArgb( 0xff, 0x11, 0x11, 0x11 ) };
				shipImageMaskBrush.InterpolationColors = blend;
			}


			preg.Dispose();
			preimage.Dispose();


			var bitmap = new Bitmap( entireSize.Width + entireMargin.Horizontal, entireSize.Height + entireMargin.Vertical, System.Drawing.Imaging.PixelFormat.Format32bppArgb );
			using ( var g = Graphics.FromImage( bitmap ) ) {

				g.Clear( backgroundColor );
				if ( !string.IsNullOrEmpty( args.BackgroundImagePath ) && System.IO.File.Exists( args.BackgroundImagePath ) ) {
					try {
						using ( var backgroundImage = new Bitmap( args.BackgroundImagePath ) ) {
							using ( var backgroundBrush = new TextureBrush( backgroundImage, WrapMode.Tile ) ) {
								g.FillRectangle( backgroundBrush, new Rectangle( 0, 0, bitmap.Width, bitmap.Height ) );
							}
						}

					} catch ( Exception ) {
					}
				}


				Point masterPointer = new Point( entireMargin.Left, entireMargin.Top );

				// title
				if ( !titleSize.IsEmpty ) {
					g.DrawString( args.Title, args.TitleFont, shadowBrush, new Rectangle( masterPointer + GetAlignmentOffset( ContentAlignment.TopCenter, titleSize, entireSize ) + new Size( 2, 2 ), titleSize ), formatMiddleCenter );
					g.DrawString( args.Title, args.TitleFont, mainTextBrush, new Rectangle( masterPointer + GetAlignmentOffset( ContentAlignment.TopCenter, titleSize, entireSize ), titleSize ), formatMiddleCenter );
				}
				masterPointer.Y += titleSize.Height;


				for ( int fleetIndex = 0; fleetIndex < args.FleetIDs.Length; fleetIndex++ ) {
					int fleetID = args.FleetIDs[fleetIndex];
					FleetData fleet = KCDatabase.Instance.Fleet[fleetID];
					Point fleetPointerOrigin = masterPointer + new Size(
						( fleetPaneUnitSize.Width + fleetPaneUnitMargin.Horizontal ) * ( fleetIndex % args.HorizontalFleetCount ) + fleetPaneUnitMargin.Left,
						( fleetPaneUnitSize.Height + fleetPaneUnitMargin.Vertical ) * ( fleetIndex / args.HorizontalFleetCount ) + fleetPaneUnitMargin.Top );
					Point fleetPointer = fleetPointerOrigin;

					if ( fleet == null )
						continue;

					// fleet name
					g.DrawString( fleet.Name, args.LargeFont, mainTextBrush, new Rectangle( fleetPointer, fleetNameSize ), formatMiddleLeft );
					if ( isFleetNameAndParametersAreSameLine ) {
						fleetPointer.X += fleetNameSize.Width;
						fleetPointer.Y += fleetNameSize.Height - fleetParameterAreaSize.Height;
					} else {
						fleetPointer.Y += fleetNameSize.Height;
					}

					// fleet specs
					fleetPointer.X += fleetParameterAreaMargin.Left;
					fleetPointer.Y += fleetParameterAreaMargin.Top;
					{	// fighter power
						var iconpos = fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, EquipmentIconSize, fleetParameterAreaSize );
						g.DrawImage( ResourceManager.Instance.Equipments.Images[(int)ResourceManager.EquipmentContent.CarrierBasedFighter], iconpos.X, iconpos.Y, EquipmentIconSize.Width, EquipmentIconSize.Height );
						fleetPointer.X += EquipmentIconSize.Width;

						g.DrawString( fleetAirSuperiorityTitle, args.SmallFont, subTextBrush, new Rectangle( fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, fleetAirSuperiorityTitleSize, fleetParameterAreaSize ), fleetAirSuperiorityTitleSize ), formatMiddleLeft );
						fleetPointer.X += fleetAirSuperiorityTitleSize.Width;

						Size paramValueSize = MeasureString( g, fleet.GetAirSuperiority().ToString(), args.MediumDigitFont, MaxValueSize, formatMiddleLeft );
						g.DrawString( fleet.GetAirSuperiority().ToString(), args.MediumDigitFont, mainTextBrush, new Rectangle( fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, paramValueSize, fleetParameterAreaSize ), paramValueSize ), formatMiddleLeft );
						fleetPointer.X += paramValueSize.Width + fleetParameterAreaInnerMargin.Width;
					}
					{	// searching ability
						var iconpos = fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, EquipmentIconSize, fleetParameterAreaSize );
						g.DrawImage( ResourceManager.Instance.Equipments.Images[(int)ResourceManager.EquipmentContent.CarrierBasedRecon], iconpos.X, iconpos.Y, EquipmentIconSize.Width, EquipmentIconSize.Height );
						fleetPointer.X += EquipmentIconSize.Width;

						g.DrawString( fleetSearchingAbilityTitle, args.SmallFont, subTextBrush, new Rectangle( fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, fleetSearchingAbilityTitleSize, fleetParameterAreaSize ), fleetSearchingAbilityTitleSize ), formatMiddleLeft );
						fleetPointer.X += fleetAirSuperiorityTitleSize.Width;

						Size paramValueSize = MeasureString( g, fleet.GetSearchingAbilityString(), args.MediumDigitFont, MaxValueSize, formatMiddleLeft );
						g.DrawString( fleet.GetSearchingAbilityString(), args.MediumDigitFont, mainTextBrush, new Rectangle( fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, paramValueSize, fleetParameterAreaSize ), paramValueSize ), formatMiddleLeft );
						fleetPointer.X += paramValueSize.Width + fleetParameterAreaInnerMargin.Width;
					}

					fleetPointer.X = fleetPointerOrigin.X;
					fleetPointer.Y += fleetParameterAreaSize.Height;

					g.DrawLine( linePen, fleetPointer + new Size( lineMargin, 0 ), fleetPointer + new Size( shipPaneSize.Width - lineMargin, 0 ) );
					fleetPointer.Y += fleetParameterAreaMargin.Bottom;


					for ( int shipIndex = 0; shipIndex < 6; shipIndex++ ) {
						ShipData ship = fleet.MembersInstance[shipIndex];
						Point shipPointerOrigin = fleetPointer + new Size(
							( shipPaneUnitSize.Width + shipPaneUnitMargin.Horizontal ) * ( shipIndex % args.HorizontalShipCount ) + shipPaneUnitMargin.Left,
							( shipPaneUnitSize.Height + shipPaneUnitMargin.Vertical ) * ( shipIndex / args.HorizontalShipCount ) + shipPaneUnitMargin.Top );
						Point shipPointer = shipPointerOrigin;

						//g.DrawRectangle( Pens.Teal, new Rectangle( shipPointer, shipPaneUnitSize ) );

						if ( ship == null )
							continue;

						g.DrawString( string.Format( "#{0}:", shipIndex + 1 ), args.MediumDigitFont, subTextBrush, new Rectangle( shipPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, shipIndexSize, shipNameAreaSize ), shipIndexSize ), formatMiddleLeft );
						shipPointer.X += shipIndexSize.Width;

						using ( var shipNameImage = GetShipSwfImage( ship.MasterShip.ResourceName, ShipNameID ) ) {
							if ( shipNameImage != null ) {
								g.DrawImage( shipNameImage, new Rectangle( shipPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, shipNameSize, shipNameAreaSize ), shipNameSize ),
									shipNameImageAvailableArea, GraphicsUnit.Pixel );
							}
						}
						shipPointer.X += shipNameSize.Width;

						g.DrawString( "Lv.", args.SmallDigitFont, subTextBrush, new Rectangle( shipPointer + GetAlignmentOffset( ContentAlignment.BottomLeft, levelSize, shipNameAreaSize ), levelSize ), formatMiddleLeft );
						shipPointer.X += levelSize.Width;

						g.DrawString( ship.Level.ToString(), args.MediumDigitFont, mainTextBrush, new Rectangle( shipPointer + GetAlignmentOffset( ContentAlignment.BottomLeft, mediumDigit3Size, shipNameAreaSize ), mediumDigit3Size ), formatMiddleRight );

						shipPointer.X = shipPointerOrigin.X;


						using ( var shipImageOriginal = GetShipSwfImage( ship.MasterShip.ResourceName, args.ReflectDamageGraphic && ship.HPRate <= 0.5 ? ShipCutinDamagedID : ShipCutinNormalID ) ) {
							using ( var shipImage = shipImageOriginal.Clone( new Rectangle( 0, 0, shipImageOriginal.Width, shipImageOriginal.Height ), PixelFormat.Format32bppArgb ) ) {
								using ( var maskImage = new Bitmap( ShipCutinSize.Width, ShipCutinSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb ) ) {						// move to top
									using ( var maskg = Graphics.FromImage( maskImage ) ) {
										maskg.Clear( Color.Black );
										maskg.FillRectangle( shipImageMaskBrush, new Rectangle( 0, 0, maskImage.Width, maskImage.Height ) );
									}

									BitmapData imageData = shipImage.LockBits( new Rectangle( 0, 0, shipImage.Width, shipImage.Height ), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb );
									byte[] imageCanvas = new byte[imageData.Width * imageData.Height * 4];
									Marshal.Copy( imageData.Scan0, imageCanvas, 0, imageCanvas.Length );

									BitmapData maskData = maskImage.LockBits( new Rectangle( 0, 0, maskImage.Width, maskImage.Height ), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb );
									byte[] maskCanvas = new byte[maskData.Width * maskData.Height * 4];
									Marshal.Copy( maskData.Scan0, maskCanvas, 0, maskCanvas.Length );


									for ( int y = 0; y < imageData.Height; y++ ) {
										for ( int x = 0; x < imageData.Width; x++ ) {
											imageCanvas[( y * imageData.Width + x ) * 4 + 3] = (byte)( (int)imageCanvas[( y * imageData.Width + x ) * 4 + 3] * maskCanvas[( y * maskData.Width + x ) * 4 + 0] / 255 );
										}
									}

									Marshal.Copy( imageCanvas, 0, imageData.Scan0, imageCanvas.Length );
									shipImage.UnlockBits( imageData );
									maskImage.UnlockBits( maskData );

								}

								var shipOffset = GetAlignmentOffset( ContentAlignment.BottomLeft, ShipCutinSize, shipPaneUnitSize );
								g.DrawImage( shipImage, shipPointer.X + shipOffset.Width, shipPointer.Y + shipOffset.Height, ShipCutinSize.Width, ShipCutinSize.Height );

							}
						}



						// equipments
						Point equipmentPointerOrigin = shipPointer + GetAlignmentOffset( ContentAlignment.BottomRight, equipmentAreaSize, shipPaneUnitSize );
						for ( int equipmentIndex = 0; equipmentIndex < 6; equipmentIndex++ ) {
							EquipmentData eq = ship.AllSlotInstance[equipmentIndex];
							Point equipmentPointer = equipmentPointerOrigin + new Size( 0, equipmentAreaUnitSize.Height * equipmentIndex );


							int aircraftMax = equipmentIndex < 5 ? ship.MasterShip.Aircraft[equipmentIndex] : 0;

							if ( aircraftMax > 0 ) {
								Brush aircraftBrush;
								if ( eq != null && Calculator.IsAircraft( eq.EquipmentID, true, true ) ) {
									aircraftBrush = mainTextBrush;
								} else {
									aircraftBrush = disabledBrush;
								}

								g.DrawString( aircraftMax.ToString(), args.SmallDigitFont, aircraftBrush, new Rectangle( equipmentPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, smallDigit3Size, equipmentAreaUnitSize ), smallDigit3Size ), formatMiddleRight );
							}
							equipmentPointer.X += smallDigit3Size.Width;

							bool isOutOfSlot = equipmentIndex >= ship.SlotSize && !( equipmentIndex == 5 && ship.IsExpansionSlotAvailable );

							Size equipmentIconOffset = GetAlignmentOffset( ContentAlignment.MiddleLeft, EquipmentIconSize, equipmentAreaUnitSize );
							g.DrawImage( GetEquipmentIcon( eq != null ? eq.EquipmentID : -1, isOutOfSlot ),
								equipmentPointer.X + equipmentIconOffset.Width, equipmentPointer.Y + equipmentIconOffset.Height, EquipmentIconSize.Width, EquipmentIconSize.Height );
							equipmentPointer.X += EquipmentIconSize.Width;

							string equipmentName;
							if ( eq != null ) {
								equipmentName = eq.Name;
							} else if ( isOutOfSlot ) {
								equipmentName = "";
							} else {
								equipmentName = "(なし)";
							}
							equipmentNameBrush.ResetTransform();
							if ( eq != null && eq.Level > 0 ) {
								equipmentNameBrush.TranslateTransform( equipmentPointer.X - EquipmentIconSize.Width, 0 );
							} else {
								equipmentNameBrush.TranslateTransform( equipmentPointer.X, 0 );
							}
							g.DrawString( equipmentName, args.MediumFont, equipmentNameBrush, new Rectangle( equipmentPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, equipmentNameSizeExtended, equipmentAreaUnitSize ), equipmentNameSizeExtended ), formatMiddleLeft );
							equipmentPointer.X += equipmentNameSize.Width;

							if ( eq != null ) {

								if ( eq.Level > 0 ) {
									g.DrawString( "+" + eq.Level, args.SmallDigitFont, subTextBrush, new Rectangle( equipmentPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, equipmentLevelSize, equipmentAreaUnitSize ), equipmentLevelSize ), formatMiddleRight );
								}

							}

						}

					}

				}


				masterPointer.Y += fleetPaneSize.Height;
				g.DrawLine( linePen, masterPointer + new Size( lineMargin, 0 ), masterPointer + new Size( fleetPaneSize.Width - commentMargin.Horizontal - lineMargin, 0 ) );


				if ( !commentSize.IsEmpty ) {
					var commentPointer = masterPointer + new Size( commentMargin.Left, commentMargin.Top );
					commentPointer += new Size( commentPadding.Left, commentPadding.Top );
					g.DrawString( args.Comment, args.MediumFont, mainTextBrush, new Rectangle( commentPointer, commentSize ), formatMiddleLeft );
				}
			}

			if ( args.AvoidTwitterDeterioration ) {
				// 不透明ピクセルのみだと jpeg 化されてしまうため、1px だけわずかに透明にする
				Color temp = bitmap.GetPixel( bitmap.Width - 1, bitmap.Height - 1 );
				bitmap.SetPixel( bitmap.Width - 1, bitmap.Height - 1, Color.FromArgb( 252, temp.R, temp.G, temp.B ) );
			}


			mainTextBrush.Dispose();
			shadowBrush.Dispose();
			disabledBrush.Dispose();
			subTextBrush.Dispose();
			aircraftLevelLowBrush.Dispose();
			aircraftLevelHighBrush.Dispose();

			equipmentNameBrush.Dispose();
			shipImageMaskBrush.Dispose();

			linePen.Dispose();

			return bitmap;
		}





		public static Bitmap GenerateBannerBitmap( FleetImageArgument args ) {

			var formatMiddleLeft = GetStringFormat( ContentAlignment.MiddleLeft );
			var formatMiddleCenter = GetStringFormat( ContentAlignment.MiddleCenter );
			var formatMiddleRight = GetStringFormat( ContentAlignment.MiddleRight );

			Color backgroundColor = Color.FromArgb( 0xff, 0xff, 0xff );
			Color mainTextColor = Color.FromArgb( 0x0f, 0x0f, 0x0f );
			Color subTextColor = Color.FromArgb( 0x00, 0x88, 0x88 );
			Color shadowColor = Color.FromArgb( 0x88, 0x88, 0x88 );
			Color disabledColor = Color.FromArgb( 0xaa, 0xaa, 0xaa );
			Color aircraftLevelLowColor = Color.FromArgb( 0x00, 0xff, 0xff );
			Color aircraftLevelHighColor = Color.FromArgb( 0xff, 0x88, 0x00 );

			var mainTextBrush = new SolidBrush( mainTextColor );
			var subTextBrush = new SolidBrush( subTextColor );
			var shadowBrush = new SolidBrush( shadowColor );
			var disabledBrush = new SolidBrush( disabledColor );
			var aircraftLevelLowBrush = new SolidBrush( aircraftLevelLowColor );
			var aircraftLevelHighBrush = new SolidBrush( aircraftLevelHighColor );

			var linePen = new Pen( subTextColor );


			string fleetAirSuperiorityTitle = "制空戦力";
			string fleetSearchingAbilityTitle = "索敵能力";

			// for measure space of strings
			Bitmap preimage = new Bitmap( 1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb );
			Graphics preg = Graphics.FromImage( preimage );

			// Size Calculation
			Size titleSize = string.IsNullOrWhiteSpace( args.Title ) ? Size.Empty : MeasureString( preg, args.Title, args.TitleFont, MaxValueSize, formatMiddleCenter );
			Size commentSize = string.IsNullOrWhiteSpace( args.Comment ) ? Size.Empty : MeasureString( preg, args.Comment, args.MediumFont, MaxValueSize, formatMiddleLeft );

			Size fleetNameSize = MeasureString( preg, "大正義日独伊三国褐色同盟", args.LargeFont, MaxValueSize, formatMiddleLeft );		// kanji 12 char
			Size fleetAirSuperiorityTitleSize = MeasureString( preg, fleetAirSuperiorityTitle, args.SmallFont, MaxValueSize, formatMiddleLeft );
			Size fleetAirSuperiorityValueEstimatedSize = MeasureString( preg, "8888", args.MediumDigitFont, MaxValueSize, formatMiddleRight );
			Size fleetSearchingAbilityTitleSize = MeasureString( preg, fleetSearchingAbilityTitle, args.SmallFont, MaxValueSize, formatMiddleLeft );
			Size fleetSearchingAbilityValueEstimatedSize = MeasureString( preg, "-888.88", args.MediumDigitFont, MaxValueSize, formatMiddleRight );

			Size shipIndexSize = MeasureString( preg, "#4:", args.MediumDigitFont, MaxValueSize, formatMiddleLeft );
			Size equipmentNameSize = MeasureString( preg, "61cm五連装(酸素)魚雷", args.SmallFont, MaxValueSize, formatMiddleLeft );		// kanji 9 char
			Size mediumDigit3Size = MeasureString( preg, "888", args.MediumDigitFont, MaxValueSize, formatMiddleRight );
			Size smallDigit3Size = MeasureString( preg, "888", args.SmallDigitFont, MaxValueSize, formatMiddleRight );
			Size levelSize = MeasureString( preg, "Lv.", args.SmallDigitFont, MaxValueSize, formatMiddleLeft );
			Size equipmentLevelSize = MeasureString( preg, "+10", args.SmallDigitFont, MaxValueSize, formatMiddleRight );

			Size fleetParameterAreaInnerMargin = new Size( 16, 0 );
			Padding shipNameAreaMargin = new Padding( 0, 0, 0, 2 );
			Padding equipmentAreaMargin = new Padding( 0, 0, 0, 2 );
			Padding shipPaneUnitMargin = new Padding( 2 );
			Padding shipPaneMargin = new Padding();
			Padding fleetParameterAreaMargin = new Padding( 8, 0, 0, 4 );
			Padding fleetPaneUnitMargin = new Padding( 4 );
			Padding fleetPaneMargin = new Padding();
			Padding titleMargin = new Padding( 0, 0, 0, 2 );
			Padding commentMargin = new Padding( 2 );
			Padding commentPadding = new Padding( 2 );
			Padding entireMargin = new Padding();
			int lineMargin = 4;

			Size shipBannerSize = ShipBannerSize;
			Size shipNameAreaSize = SumWidthMaxHeight( ShipBannerSize, smallDigit3Size );
			Size equipmentAreaUnitSize = SumWidthMaxHeight( EquipmentIconSize, equipmentNameSize, equipmentLevelSize );
			Size equipmentAreaSize = new Size( equipmentAreaUnitSize.Width, equipmentAreaUnitSize.Height * 6 );

			Size shipPaneUnitSize = MaxWidthSumHeight( shipNameAreaSize, equipmentAreaSize );
			Size shipPaneSize = new Size(
				( shipPaneUnitSize.Width + shipPaneUnitMargin.Horizontal ) * Math.Min( args.HorizontalShipCount, 6 ),
				( shipPaneUnitSize.Height + shipPaneUnitMargin.Vertical ) * (int)Math.Ceiling( 6.0 / args.HorizontalShipCount ) );

			Size fleetParameterAreaSize = SumWidthMaxHeight(
				EquipmentIconSize, fleetAirSuperiorityTitleSize, fleetAirSuperiorityValueEstimatedSize, fleetParameterAreaInnerMargin,
				EquipmentIconSize, fleetSearchingAbilityTitleSize, fleetSearchingAbilityValueEstimatedSize );

			Size fleetPaneUnitSize;
			bool isFleetNameAndParametersAreSameLine = fleetNameSize.Width + fleetParameterAreaSize.Width + fleetParameterAreaMargin.Horizontal < shipPaneSize.Width + shipPaneMargin.Horizontal;
			if ( isFleetNameAndParametersAreSameLine ) {
				fleetPaneUnitSize = MaxWidthSumHeight( SumWidthMaxHeight( fleetNameSize, fleetParameterAreaSize + fleetParameterAreaMargin.Size ), shipPaneSize + shipPaneMargin.Size );
			} else {
				fleetPaneUnitSize = MaxWidthSumHeight( fleetNameSize, fleetParameterAreaSize + fleetParameterAreaMargin.Size, shipPaneSize + shipPaneMargin.Size );
			}

			Size fleetPaneSize = new Size( ( fleetPaneUnitSize.Width + fleetPaneUnitMargin.Horizontal ) * Math.Min( args.HorizontalFleetCount, args.FleetIDs.Length ),
				( fleetPaneUnitSize.Height + fleetPaneUnitMargin.Vertical ) * (int)Math.Ceiling( (double)args.FleetIDs.Length / args.HorizontalFleetCount ) );

			Size commentAreaSize = commentSize + commentPadding.Size;

			Size entireSize = MaxWidthSumHeight( titleSize + titleMargin.Size, fleetPaneSize + fleetPaneMargin.Size, commentAreaSize + commentMargin.Size );


			Size equipmentNameSizeExtended = SumWidthMaxHeight( equipmentNameSize, equipmentLevelSize );

			var equipmentNameBrush = new LinearGradientBrush( new Rectangle( 0, 0, equipmentNameSize.Width * 2 + equipmentLevelSize.Width, equipmentAreaUnitSize.Height ), Color.Black, Color.Black, LinearGradientMode.Horizontal );		// color is ignored
			{
				var blend = new ColorBlend();
				blend.Positions = new[] { 0f, (float)( equipmentNameSizeExtended.Width - EquipmentIconSize.Width ) / equipmentNameBrush.Rectangle.Width, (float)( equipmentNameSizeExtended.Width ) / equipmentNameBrush.Rectangle.Width, 1f };
				blend.Colors = new[] { mainTextColor, mainTextColor, Color.FromArgb( 0, mainTextColor ), Color.FromArgb( 0, mainTextColor ) };
				equipmentNameBrush.InterpolationColors = blend;
			}
			equipmentNameBrush.GammaCorrection = true;


			preg.Dispose();
			preimage.Dispose();


			var bitmap = new Bitmap( entireSize.Width + entireMargin.Horizontal, entireSize.Height + entireMargin.Vertical, System.Drawing.Imaging.PixelFormat.Format32bppArgb );
			using ( var g = Graphics.FromImage( bitmap ) ) {

				g.Clear( backgroundColor );
				if ( !string.IsNullOrEmpty( args.BackgroundImagePath ) && System.IO.File.Exists( args.BackgroundImagePath ) ) {
					try {
						using ( var backgroundImage = new Bitmap( args.BackgroundImagePath ) ) {
							using ( var backgroundBrush = new TextureBrush( backgroundImage, WrapMode.Tile ) ) {
								g.FillRectangle( backgroundBrush, new Rectangle( 0, 0, bitmap.Width, bitmap.Height ) );
							}
						}

					} catch ( Exception ) {
					}
				}


				Point masterPointer = new Point( entireMargin.Left, entireMargin.Top );

				// title
				if ( !titleSize.IsEmpty ) {
					g.DrawString( args.Title, args.TitleFont, shadowBrush, new Rectangle( masterPointer + GetAlignmentOffset( ContentAlignment.TopCenter, titleSize, entireSize ) + new Size( 2, 2 ), titleSize ), formatMiddleCenter );
					g.DrawString( args.Title, args.TitleFont, mainTextBrush, new Rectangle( masterPointer + GetAlignmentOffset( ContentAlignment.TopCenter, titleSize, entireSize ), titleSize ), formatMiddleCenter );
				}
				masterPointer.Y += titleSize.Height;


				for ( int fleetIndex = 0; fleetIndex < args.FleetIDs.Length; fleetIndex++ ) {
					int fleetID = args.FleetIDs[fleetIndex];
					FleetData fleet = KCDatabase.Instance.Fleet[fleetID];
					Point fleetPointerOrigin = masterPointer + new Size(
						( fleetPaneUnitSize.Width + fleetPaneUnitMargin.Horizontal ) * ( fleetIndex % args.HorizontalFleetCount ) + fleetPaneUnitMargin.Left,
						( fleetPaneUnitSize.Height + fleetPaneUnitMargin.Vertical ) * ( fleetIndex / args.HorizontalFleetCount ) + fleetPaneUnitMargin.Top );
					Point fleetPointer = fleetPointerOrigin;

					if ( fleet == null )
						continue;

					// fleet name
					g.DrawString( fleet.Name, args.LargeFont, mainTextBrush, new Rectangle( fleetPointer, fleetNameSize ), formatMiddleLeft );
					if ( isFleetNameAndParametersAreSameLine ) {
						fleetPointer.X += fleetNameSize.Width;
						fleetPointer.Y += fleetNameSize.Height - fleetParameterAreaSize.Height;
					} else {
						fleetPointer.Y += fleetNameSize.Height;
					}

					// fleet specs
					fleetPointer.X += fleetParameterAreaMargin.Left;
					fleetPointer.Y += fleetParameterAreaMargin.Top;
					{	// fighter power
						var iconpos = fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, EquipmentIconSize, fleetParameterAreaSize );
						g.DrawImage( ResourceManager.Instance.Equipments.Images[(int)ResourceManager.EquipmentContent.CarrierBasedFighter], iconpos.X, iconpos.Y, EquipmentIconSize.Width, EquipmentIconSize.Height );
						fleetPointer.X += EquipmentIconSize.Width;

						g.DrawString( fleetAirSuperiorityTitle, args.SmallFont, subTextBrush, new Rectangle( fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, fleetAirSuperiorityTitleSize, fleetParameterAreaSize ), fleetAirSuperiorityTitleSize ), formatMiddleLeft );
						fleetPointer.X += fleetAirSuperiorityTitleSize.Width;

						Size paramValueSize = MeasureString( g, fleet.GetAirSuperiority().ToString(), args.MediumDigitFont, MaxValueSize, formatMiddleLeft );
						g.DrawString( fleet.GetAirSuperiority().ToString(), args.MediumDigitFont, mainTextBrush, new Rectangle( fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, paramValueSize, fleetParameterAreaSize ), paramValueSize ), formatMiddleLeft );
						fleetPointer.X += paramValueSize.Width + fleetParameterAreaInnerMargin.Width;
					}
					{	// searching ability
						var iconpos = fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, EquipmentIconSize, fleetParameterAreaSize );
						g.DrawImage( ResourceManager.Instance.Equipments.Images[(int)ResourceManager.EquipmentContent.CarrierBasedRecon], iconpos.X, iconpos.Y, EquipmentIconSize.Width, EquipmentIconSize.Height );
						fleetPointer.X += EquipmentIconSize.Width;

						g.DrawString( fleetSearchingAbilityTitle, args.SmallFont, subTextBrush, new Rectangle( fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, fleetSearchingAbilityTitleSize, fleetParameterAreaSize ), fleetSearchingAbilityTitleSize ), formatMiddleLeft );
						fleetPointer.X += fleetAirSuperiorityTitleSize.Width;

						Size paramValueSize = MeasureString( g, fleet.GetSearchingAbilityString(), args.MediumDigitFont, MaxValueSize, formatMiddleLeft );
						g.DrawString( fleet.GetSearchingAbilityString(), args.MediumDigitFont, mainTextBrush, new Rectangle( fleetPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, paramValueSize, fleetParameterAreaSize ), paramValueSize ), formatMiddleLeft );
						fleetPointer.X += paramValueSize.Width + fleetParameterAreaInnerMargin.Width;
					}

					fleetPointer.X = fleetPointerOrigin.X;
					fleetPointer.Y += fleetParameterAreaSize.Height;

					g.DrawLine( linePen, fleetPointer + new Size( lineMargin, 0 ), fleetPointer + new Size( shipPaneSize.Width - lineMargin, 0 ) );
					fleetPointer.Y += fleetParameterAreaMargin.Bottom;


					for ( int shipIndex = 0; shipIndex < 6; shipIndex++ ) {
						ShipData ship = fleet.MembersInstance[shipIndex];
						Point shipPointerOrigin = fleetPointer + new Size(
							( shipPaneUnitSize.Width + shipPaneUnitMargin.Horizontal ) * ( shipIndex % args.HorizontalShipCount ) + shipPaneUnitMargin.Left,
							( shipPaneUnitSize.Height + shipPaneUnitMargin.Vertical ) * ( shipIndex / args.HorizontalShipCount ) + shipPaneUnitMargin.Top );
						Point shipPointer = shipPointerOrigin;

						//g.DrawRectangle( Pens.Teal, new Rectangle( shipPointer, shipPaneUnitSize ) );

						if ( ship == null )
							continue;

						//g.DrawString( string.Format( "#{0}:", shipIndex + 1 ), args.MediumDigitFont, subTextBrush, new Rectangle( shipPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, shipIndexSize, shipNameAreaSize ), shipIndexSize ), formatMiddleLeft );
						//shipPointer.X += shipIndexSize.Width;

						DrawShipSwfImage( g, ship.MasterShip.ResourceName, args.ReflectDamageGraphic && ship.HPRate <= 0.5 ? ShipBannerDamagedID : ShipBannerNormalID, shipPointer.X, shipPointer.Y, ShipBannerSize );
						shipPointer.X += shipBannerSize.Width;

						g.DrawString( ship.Level.ToString(), args.SmallDigitFont, subTextBrush, new Rectangle( shipPointer, smallDigit3Size ), formatMiddleLeft );

						shipPointer.X = shipPointerOrigin.X;
						shipPointer.Y += shipNameAreaSize.Height;


						// equipments
						Point equipmentPointerOrigin = shipPointer;
						for ( int equipmentIndex = 0; equipmentIndex < 6; equipmentIndex++ ) {
							EquipmentData eq = ship.AllSlotInstance[equipmentIndex];
							Point equipmentPointer = equipmentPointerOrigin + new Size( 0, equipmentAreaUnitSize.Height * equipmentIndex );

							
							bool isOutOfSlot = equipmentIndex >= ship.SlotSize && !( equipmentIndex == 5 && ship.IsExpansionSlotAvailable );

							Size equipmentIconOffset = GetAlignmentOffset( ContentAlignment.MiddleLeft, EquipmentIconSize, equipmentAreaUnitSize );
							g.DrawImage( GetEquipmentIcon( eq != null ? eq.EquipmentID : -1, isOutOfSlot ),
								equipmentPointer.X + equipmentIconOffset.Width, equipmentPointer.Y + equipmentIconOffset.Height, EquipmentIconSize.Width, EquipmentIconSize.Height );
							equipmentPointer.X += EquipmentIconSize.Width;

							string equipmentName;
							if ( eq != null ) {
								equipmentName = eq.Name;
							} else if ( isOutOfSlot ) {
								equipmentName = "";
							} else {
								equipmentName = "(なし)";
							}
							equipmentNameBrush.ResetTransform();
							if ( eq != null && eq.Level > 0 ) {
								equipmentNameBrush.TranslateTransform( equipmentPointer.X - EquipmentIconSize.Width, 0 );
							} else {
								equipmentNameBrush.TranslateTransform( equipmentPointer.X, 0 );
							}
							g.DrawString( equipmentName, args.SmallFont, equipmentNameBrush, new Rectangle( equipmentPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, equipmentNameSizeExtended, equipmentAreaUnitSize ), equipmentNameSizeExtended ), formatMiddleLeft );
							equipmentPointer.X += equipmentNameSize.Width;

							if ( eq != null ) {

								if ( eq.Level > 0 ) {
									g.DrawString( "+" + eq.Level, args.SmallDigitFont, subTextBrush, new Rectangle( equipmentPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, equipmentLevelSize, equipmentAreaUnitSize ), equipmentLevelSize ), formatMiddleRight );
								}

							}

						}

					}

				}


				masterPointer.Y += fleetPaneSize.Height;
				g.DrawLine( linePen, masterPointer + new Size( lineMargin, 0 ), masterPointer + new Size( fleetPaneSize.Width - commentMargin.Horizontal - lineMargin, 0 ) );


				if ( !commentSize.IsEmpty ) {
					var commentPointer = masterPointer + new Size( commentMargin.Left, commentMargin.Top );
					commentPointer += new Size( commentPadding.Left, commentPadding.Top );
					g.DrawString( args.Comment, args.MediumFont, mainTextBrush, new Rectangle( commentPointer, commentSize ), formatMiddleLeft );
				}
			}

			if ( args.AvoidTwitterDeterioration ) {
				// 不透明ピクセルのみだと jpeg 化されてしまうため、1px だけわずかに透明にする
				Color temp = bitmap.GetPixel( bitmap.Width - 1, bitmap.Height - 1 );
				bitmap.SetPixel( bitmap.Width - 1, bitmap.Height - 1, Color.FromArgb( 252, temp.R, temp.G, temp.B ) );
			}


			mainTextBrush.Dispose();
			shadowBrush.Dispose();
			disabledBrush.Dispose();
			subTextBrush.Dispose();
			aircraftLevelLowBrush.Dispose();
			aircraftLevelHighBrush.Dispose();

			equipmentNameBrush.Dispose();

			linePen.Dispose();

			return bitmap;
		}








		// management

		protected static Size MeasureString( Graphics g, string text, Font font, Size areaSize, StringFormat format ) {
			return Size.Round( g.MeasureString( text, font, new SizeF( areaSize.Width, areaSize.Height ), format ) );
			/*
			var f = new StringFormat( format );
			f.SetMeasurableCharacterRanges( new[] { new CharacterRange( 0, text.Length ) } );
			RectangleF rect = new RectangleF( 0, 0, areaSize.Width, areaSize.Height );
			var regions = g.MeasureCharacterRanges( text, font, rect, f );
			return Rectangle.Round( regions[0].GetBounds( g ) ).Size;
			//*/
		}

		protected static int Max( params int[] values ) {
			return values.Max();
		}

		protected static Size Max( params Size[] values ) {
			return new Size( values.Max( s => s.Width ), values.Max( s => s.Height ) );
		}

		protected static int MaxWidth( params Size[] values ) {
			return values.Max( s => s.Width );
		}
		protected static int MaxHeight( params Size[] values ) {
			return values.Max( s => s.Height );
		}
		protected static int SumWidth( params Size[] values ) {
			return values.Sum( s => s.Width );
		}
		protected static int SumHeight( params Size[] values ) {
			return values.Sum( s => s.Height );
		}
		protected static Size SumWidthMaxHeight( params Size[] values ) {
			return new Size( SumWidth( values ), MaxHeight( values ) );
		}
		protected static Size MaxWidthSumHeight( params Size[] values ) {
			return new Size( MaxWidth( values ), SumHeight( values ) );
		}

		protected static Image GetEquipmentIcon( int equipmentID, bool isOutOfSlot ) {
			var eq = KCDatabase.Instance.MasterEquipments[equipmentID];

			if ( eq == null ) {
				if ( isOutOfSlot )
					return ResourceManager.Instance.Equipments.Images[(int)ResourceManager.EquipmentContent.Locked];
				else
					return ResourceManager.Instance.Equipments.Images[(int)ResourceManager.EquipmentContent.Nothing];

			} else {
				if ( 0 < eq.IconType && eq.IconType < (int)ResourceManager.EquipmentContent.Locked )
					return ResourceManager.Instance.Equipments.Images[eq.IconType];
				else
					return ResourceManager.Instance.Equipments.Images[(int)ResourceManager.EquipmentContent.Unknown];
			}
		}


		protected class ShipParameterData {
			public readonly Image Icon;
			public readonly string Name;
			public readonly Func<ShipData, string> ValueSelector;
			public readonly bool IsCharacter;

			public ShipParameterData( ResourceManager.IconContent iconIndex, string name, Func<ShipData, string> selector, bool isCharacter = false ) {
				Icon = ResourceManager.Instance.Icons.Images[(int)iconIndex];
				Name = name;
				ValueSelector = selector;
				IsCharacter = isCharacter;
			}
		}

		protected static readonly ShipParameterData[] ParameterDataList = new[] { 
			new ShipParameterData( ResourceManager.IconContent.ParameterHP, "耐久", ship => ship.HPMax.ToString() ),
			new ShipParameterData( ResourceManager.IconContent.ParameterFirepower, "火力", ship => ship.FirepowerTotal.ToString() ),
			new ShipParameterData( ResourceManager.IconContent.ParameterArmor, "装甲", ship => ship.ArmorTotal.ToString() ),
			new ShipParameterData( ResourceManager.IconContent.ParameterTorpedo, "雷装", ship => ship.TorpedoTotal.ToString() ),
			new ShipParameterData( ResourceManager.IconContent.ParameterEvasion, "回避", ship => ship.EvasionTotal.ToString() ),
			new ShipParameterData( ResourceManager.IconContent.ParameterAA, "対空", ship => ship.AATotal.ToString() ),
			new ShipParameterData( ResourceManager.IconContent.ParameterAircraft, "搭載", ship => ship.MasterShip.AircraftTotal.ToString() ),
			new ShipParameterData( ResourceManager.IconContent.ParameterASW, "対潜", ship => ship.ASWTotal.ToString() ),
			new ShipParameterData( ResourceManager.IconContent.ParameterSpeed, "速力", ship => Constants.GetSpeed( ship.Speed ), true ),
			new ShipParameterData( ResourceManager.IconContent.ParameterLOS, "索敵", ship => ship.LOSTotal.ToString() ),
			new ShipParameterData( ResourceManager.IconContent.ParameterRange, "射程", ship => Constants.GetRange( ship.Range ), true ),
			new ShipParameterData( ResourceManager.IconContent.ParameterLuck, "運", ship => ship.LuckTotal.ToString() ),
		};


		protected static bool DrawShipSwfImage( Graphics g, string resourceID, int characterID, int x, int y, Size size ) {
			try {
				using ( var shipImage = GetShipSwfImage( resourceID, characterID ) ) {
					if ( shipImage == null )
						return false;

					g.DrawImage( shipImage, new Rectangle( x, y, size.Width, size.Height ) );
				}

				return true;
			} catch ( Exception ) {
				return false;
			}
		}

		protected static Bitmap GetShipSwfImage( string resourceID, int characterID ) {
			try {
				string shipSwfPath = Utility.Configuration.Config.Connection.SaveDataPath + @"\resources\swf\ships\" + resourceID + ".swf";
				if ( System.IO.File.Exists( shipSwfPath ) ) {

					var shipSwf = new SwfParser();
					shipSwf.Parse( shipSwfPath );

					var imgtag = shipSwf.FindTags<SwfExtractor.Tags.ImageTag>().FirstOrDefault( t => t.CharacterID == characterID );
					return imgtag.ExtractImage();
				}

				return null;
			} catch ( Exception ) {
				return null;
			}
		}

		protected static Size GetAlignmentOffset( ContentAlignment alignment, Size content, Size container ) {
			switch ( alignment ) {
				case ContentAlignment.TopLeft:
				default:
					return new Size( 0, 0 );
				case ContentAlignment.TopCenter:
					return new Size( ( container.Width - content.Width ) / 2, 0 );
				case ContentAlignment.TopRight:
					return new Size( container.Width - content.Width, 0 );
				case ContentAlignment.MiddleLeft:
					return new Size( 0, ( container.Height - content.Height ) / 2 );
				case ContentAlignment.MiddleCenter:
					return new Size( ( container.Width - content.Width ) / 2, ( container.Height - content.Height ) / 2 );
				case ContentAlignment.MiddleRight:
					return new Size( container.Width - content.Width, ( container.Height - content.Height ) / 2 );
				case ContentAlignment.BottomLeft:
					return new Size( 0, container.Height - content.Height );
				case ContentAlignment.BottomCenter:
					return new Size( ( container.Width - content.Width ) / 2, container.Height - content.Height );
				case ContentAlignment.BottomRight:
					return new Size( container.Width - content.Width, container.Height - content.Height );
			}
		}



		protected static TextFormatFlags GetTextFormat( ContentAlignment alignment ) {

			TextFormatFlags textformat = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix;

			switch ( alignment ) {
				case ContentAlignment.TopLeft:
					textformat |= TextFormatFlags.Top | TextFormatFlags.Left; break;
				case ContentAlignment.TopCenter:
					textformat |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter; break;
				case ContentAlignment.TopRight:
					textformat |= TextFormatFlags.Top | TextFormatFlags.Right; break;
				case ContentAlignment.MiddleLeft:
					textformat |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left; break;
				case ContentAlignment.MiddleCenter:
					textformat |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter; break;
				case ContentAlignment.MiddleRight:
					textformat |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right; break;
				case ContentAlignment.BottomLeft:
					textformat |= TextFormatFlags.Bottom | TextFormatFlags.Left; break;
				case ContentAlignment.BottomCenter:
					textformat |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter; break;
				case ContentAlignment.BottomRight:
					textformat |= TextFormatFlags.Bottom | TextFormatFlags.Right; break;
			}

			return textformat;

		}

		protected static StringFormat GetStringFormat( ContentAlignment alignment ) {

			var format = new StringFormat( StringFormat.GenericDefault );
			format.Trimming = StringTrimming.None;
			format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoWrap;

			switch ( alignment ) {
				case ContentAlignment.TopLeft:
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Near;
					break;
				case ContentAlignment.TopCenter:
					format.Alignment = StringAlignment.Center;
					format.LineAlignment = StringAlignment.Near;
					break;
				case ContentAlignment.TopRight:
					format.Alignment = StringAlignment.Far;
					format.LineAlignment = StringAlignment.Near;
					break;
				case ContentAlignment.MiddleLeft:
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Center;
					break;
				case ContentAlignment.MiddleCenter:
					format.Alignment = StringAlignment.Center;
					format.LineAlignment = StringAlignment.Center;
					break;
				case ContentAlignment.MiddleRight:
					format.Alignment = StringAlignment.Far;
					format.LineAlignment = StringAlignment.Center;
					break;
				case ContentAlignment.BottomLeft:
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Far;
					break;
				case ContentAlignment.BottomCenter:
					format.Alignment = StringAlignment.Center;
					format.LineAlignment = StringAlignment.Far;
					break;
				case ContentAlignment.BottomRight:
					format.Alignment = StringAlignment.Far;
					format.LineAlignment = StringAlignment.Far;
					break;
			}

			return format;
		}
	}

	public class FleetImageArgument {

		public int[] FleetIDs;
		public int HorizontalFleetCount;
		public int HorizontalShipCount;

		public bool ReflectDamageGraphic;
		public bool AvoidTwitterDeterioration;

		public Font TitleFont;
		public Font LargeFont;
		public Font MediumFont;
		public Font SmallFont;
		public Font MediumDigitFont;
		public Font SmallDigitFont;

		public string BackgroundImagePath;

		public string Title;
		public string Comment;
	}

}
