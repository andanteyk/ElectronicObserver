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
using System.Runtime.Serialization;
using ElectronicObserver.Utility.Storage;

namespace ElectronicObserver.Utility {

	public class FleetImageGenerator {

		/// <summary> 幅・高さともに int.MaxValue の Size </summary>
		protected static readonly Size MaxValueSize = new Size( int.MaxValue, int.MaxValue );

		// 各種画像リソースのサイズ
		protected static readonly Size EquipmentCardSize = new Size( 260, 260 );
		protected static readonly Size EquipmentIconSize = new Size( 16, 16 );




		private FleetImageGenerator() { }



		/// <summary>
		/// 詳細な情報を表示する、カード式の編成画像を出力します。
		/// </summary>
		public static Bitmap GenerateCardBitmap( FleetImageArgument args ) {

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
			Padding shipPaneUnitMargin = new Padding( 2 );
			Padding fleetParameterAreaMargin = new Padding( 8, 0, 0, 4 );
			Padding fleetPaneUnitMargin = new Padding( 4 );
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
			Size shipImagePaneSize = SwfHelper.ShipCardSize;
			Size shipPaneUnitSize = SumWidthMaxHeight( shipValuePaneSize, shipImagePaneSize );
			Size shipPaneSize = new Size(
				( shipPaneUnitSize.Width + shipPaneUnitMargin.Horizontal ) * Math.Min( args.HorizontalShipCount, 6 ),
				( shipPaneUnitSize.Height + shipPaneUnitMargin.Vertical ) * (int)Math.Ceiling( 6.0 / args.HorizontalShipCount ) );

			Size fleetParameterAreaSize = SumWidthMaxHeight(
				EquipmentIconSize, fleetAirSuperiorityTitleSize, fleetAirSuperiorityValueEstimatedSize, fleetParameterAreaInnerMargin,
				EquipmentIconSize, fleetSearchingAbilityTitleSize, fleetSearchingAbilityValueEstimatedSize );

			Size fleetPaneUnitSize;
			bool isFleetNameAndParametersAreSameLine = fleetNameSize.Width + fleetParameterAreaSize.Width + fleetParameterAreaMargin.Horizontal < shipPaneSize.Width;
			if ( isFleetNameAndParametersAreSameLine ) {
				fleetPaneUnitSize = MaxWidthSumHeight( SumWidthMaxHeight( fleetNameSize, fleetParameterAreaSize + fleetParameterAreaMargin.Size ), shipPaneSize );
			} else {
				fleetPaneUnitSize = MaxWidthSumHeight( fleetNameSize, fleetParameterAreaSize + fleetParameterAreaMargin.Size, shipPaneSize );
			}

			Size fleetPaneSize = new Size( ( fleetPaneUnitSize.Width + fleetPaneUnitMargin.Horizontal ) * Math.Min( args.HorizontalFleetCount, args.FleetIDs.Length ),
				( fleetPaneUnitSize.Height + fleetPaneUnitMargin.Vertical ) * (int)Math.Ceiling( (double)args.FleetIDs.Length / args.HorizontalFleetCount ) );

			Size commentAreaSize = commentSize + commentPadding.Size;

			Size entireSize = MaxWidthSumHeight( titleSize + titleMargin.Size, fleetPaneSize, commentAreaSize + commentMargin.Size );

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
						DrawShipSwfImage( g, ship.MasterShip.ResourceName, args.ReflectDamageGraphic && ship.HPRate <= 0.5 ? SwfHelper.ShipResourceCharacterID.CardDamaged : SwfHelper.ShipResourceCharacterID.CardNormal, shipPointer.X, shipPointer.Y, SwfHelper.ShipCardSize );
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




		/// <summary>
		/// カットイン式の編成画像を出力します。
		/// </summary>
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
			var subLinePen = new Pen( subTextColor );
			subLinePen.DashStyle = DashStyle.Dash;


			string fleetAirSuperiorityTitle = "制空戦力";
			string fleetSearchingAbilityTitle = "索敵能力";

			// for measure space of strings
			Bitmap preimage = new Bitmap( 1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb );
			Graphics preg = Graphics.FromImage( preimage );

			bool has5thSlot = args.FleetIDs
				.Select( id => KCDatabase.Instance.Fleet[id] )
				.Where( f => f != null )
				.SelectMany( f => f.MembersInstance )
				.Any( s => s != null && s.SlotSize >= 5 );
			int slotCount = has5thSlot ? 6 : 5;

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
			Rectangle shipNameImageAvailableArea = new Rectangle( 100, 0, SwfHelper.ShipNameSize.Width - 124, SwfHelper.ShipNameSize.Height - 16 );

			Size fleetParameterAreaInnerMargin = new Size( 16, 0 );
			Padding shipNameAreaMargin = new Padding( 0, 0, 0, 2 );
			Padding equipmentAreaUnitMargin = new Padding( 2, 1, 2, 0 );
			Padding equipmentAreaMargin = new Padding( 0, 0, 0, 2 );
			Padding shipPaneUnitMargin = new Padding( 2 );
			Padding fleetParameterAreaMargin = new Padding( 8, 0, 0, 4 );
			Padding fleetPaneUnitMargin = new Padding( 4 );
			Padding titleMargin = new Padding( 0, 0, 0, 2 );
			Padding commentMargin = new Padding( 2 );
			Padding commentPadding = new Padding( 2 );
			Padding entireMargin = new Padding();
			int lineMargin = 4;

			Size shipNameSize = shipNameImageAvailableArea.Size;
			Size shipParameterSize = new Size( Max( levelSize.Width, EquipmentIconSize.Width ) + mediumDigit3Size.Width, Max( levelSize.Height, EquipmentIconSize.Height, mediumDigit3Size.Height ) * 2 );
			Size shipNameAreaSize = SumWidthMaxHeight( shipIndexSize, shipNameSize, shipParameterSize );
			Size equipmentAreaUnitSize = SumWidthMaxHeight( smallDigit3Size, EquipmentIconSize, equipmentNameSize, equipmentLevelSize );
			Size equipmentAreaSize = new Size( ( equipmentAreaUnitSize.Width + equipmentAreaUnitMargin.Horizontal ), ( equipmentAreaUnitSize.Height + equipmentAreaUnitMargin.Vertical ) * slotCount );

			Size shipPaneUnitSize = new Size( Max( shipNameAreaSize.Width + equipmentAreaSize.Width, SwfHelper.ShipCutinSize.Width ),
				Max( shipNameAreaSize.Height + SwfHelper.ShipCutinSize.Height, equipmentAreaSize.Height ) ); // SumWidthMaxHeight( MaxWidthSumHeight( shipNameAreaSize, ShipCutinSize ), equipmentAreaSize + equipmentAreaMargin.Size );
			Size shipPaneSize = new Size(
				( shipPaneUnitSize.Width + shipPaneUnitMargin.Horizontal ) * Math.Min( args.HorizontalShipCount, 6 ),
				( shipPaneUnitSize.Height + shipPaneUnitMargin.Vertical ) * (int)Math.Ceiling( 6.0 / args.HorizontalShipCount ) );

			Size fleetParameterAreaSize = SumWidthMaxHeight(
				EquipmentIconSize, fleetAirSuperiorityTitleSize, fleetAirSuperiorityValueEstimatedSize, fleetParameterAreaInnerMargin,
				EquipmentIconSize, fleetSearchingAbilityTitleSize, fleetSearchingAbilityValueEstimatedSize );

			Size fleetPaneUnitSize;
			bool isFleetNameAndParametersAreSameLine = fleetNameSize.Width + fleetParameterAreaSize.Width + fleetParameterAreaMargin.Horizontal < shipPaneSize.Width;
			if ( isFleetNameAndParametersAreSameLine ) {
				fleetPaneUnitSize = MaxWidthSumHeight( SumWidthMaxHeight( fleetNameSize, fleetParameterAreaSize + fleetParameterAreaMargin.Size ), shipPaneSize );
			} else {
				fleetPaneUnitSize = MaxWidthSumHeight( fleetNameSize, fleetParameterAreaSize + fleetParameterAreaMargin.Size, shipPaneSize );
			}

			Size fleetPaneSize = new Size( ( fleetPaneUnitSize.Width + fleetPaneUnitMargin.Horizontal ) * Math.Min( args.HorizontalFleetCount, args.FleetIDs.Length ),
				( fleetPaneUnitSize.Height + fleetPaneUnitMargin.Vertical ) * (int)Math.Ceiling( (double)args.FleetIDs.Length / args.HorizontalFleetCount ) );

			Size commentAreaSize = commentSize + commentPadding.Size;

			Size entireSize = MaxWidthSumHeight( titleSize + titleMargin.Size, fleetPaneSize, commentAreaSize + commentMargin.Size );


			Size equipmentNameSizeExtended = SumWidthMaxHeight( equipmentNameSize, equipmentLevelSize );

			var equipmentNameBrush = new LinearGradientBrush( new Rectangle( 0, 0, equipmentNameSize.Width * 2 + equipmentLevelSize.Width, equipmentAreaUnitSize.Height ), Color.Black, Color.Black, LinearGradientMode.Horizontal );		// color is ignored
			{
				var blend = new ColorBlend();
				blend.Positions = new[] { 0f, (float)( equipmentNameSizeExtended.Width - EquipmentIconSize.Width ) / equipmentNameBrush.Rectangle.Width, (float)( equipmentNameSizeExtended.Width ) / equipmentNameBrush.Rectangle.Width, 1f };
				blend.Colors = new[] { mainTextColor, mainTextColor, Color.FromArgb( 0, mainTextColor ), Color.FromArgb( 0, mainTextColor ) };
				equipmentNameBrush.InterpolationColors = blend;
			}
			equipmentNameBrush.GammaCorrection = true;

			var shipImageMaskBrush = new LinearGradientBrush( new Rectangle( 0, 0, SwfHelper.ShipCutinSize.Width, SwfHelper.ShipCutinSize.Height ), Color.Black, Color.Black, LinearGradientMode.Horizontal );
			{
				var blend = new ColorBlend();
				blend.Positions = new[] { 0f, Math.Min( (float)( shipPaneUnitSize.Width - equipmentAreaSize.Width - 16 ) / SwfHelper.ShipCutinSize.Width, 1f ), Math.Min( (float)( shipPaneUnitSize.Width - equipmentAreaSize.Width + 16 ) / SwfHelper.ShipCutinSize.Width, 1f ), 1f };
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

						using ( var shipNameImage = SwfHelper.GetShipSwfImage( ship.MasterShip.ResourceName, SwfHelper.ShipResourceCharacterID.Name ) ) {
							if ( shipNameImage != null ) {
								g.DrawImage( shipNameImage, new Rectangle( shipPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, shipNameSize, shipNameAreaSize ), shipNameSize ),
									shipNameImageAvailableArea, GraphicsUnit.Pixel );
							} else {
								// 画像がなければ文字列で艦名を描画する
								g.DrawString( ship.Name, args.LargeFont, mainTextBrush, new Rectangle( shipPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, shipNameSize, shipNameAreaSize ), shipNameSize ), formatMiddleLeft );
							}
						}
						shipPointer.X += shipNameSize.Width;


						{
							Size paramNameSize = new Size( Max( levelSize.Width, EquipmentIconSize.Width ), Max( levelSize.Height, EquipmentIconSize.Height, mediumDigit3Size.Height ) );

							g.DrawString( "Lv.", args.SmallDigitFont, subTextBrush, new Rectangle( shipPointer + GetAlignmentOffset( ContentAlignment.BottomLeft, levelSize, paramNameSize ), levelSize ), formatMiddleLeft );
							g.DrawString( ship.Level.ToString(), args.MediumDigitFont, mainTextBrush, new Rectangle( shipPointer + new Size( paramNameSize.Width, 0 ), mediumDigit3Size ), formatMiddleRight );

							var luckRect = new Rectangle( shipPointer + new Size( 0, paramNameSize.Height ) + GetAlignmentOffset( ContentAlignment.MiddleCenter, EquipmentIconSize, paramNameSize ), EquipmentIconSize );
							g.DrawImage( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.ParameterLuck],
								luckRect );
							g.DrawString( ship.LuckTotal.ToString(), args.MediumDigitFont, mainTextBrush, new Rectangle( shipPointer + new Size( paramNameSize.Width, paramNameSize.Height ), mediumDigit3Size ), formatMiddleRight );

						}
						shipPointer.X = shipPointerOrigin.X;


						using ( var shipImageOriginal = SwfHelper.GetShipSwfImage( ship.MasterShip.ResourceName, args.ReflectDamageGraphic && ship.HPRate <= 0.5 ? SwfHelper.ShipResourceCharacterID.CutinDamaged : SwfHelper.ShipResourceCharacterID.CutinNormal ) ) {
							if ( shipImageOriginal != null ) {
								using ( var shipImage = shipImageOriginal.Clone( new Rectangle( 0, 0, shipImageOriginal.Width, shipImageOriginal.Height ), PixelFormat.Format32bppArgb ) ) {
									using ( var maskImage = new Bitmap( SwfHelper.ShipCutinSize.Width, SwfHelper.ShipCutinSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb ) ) {						// move to top
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

									var shipOffset = GetAlignmentOffset( ContentAlignment.BottomLeft, SwfHelper.ShipCutinSize, shipPaneUnitSize );
									g.DrawImage( shipImage, shipPointer.X + shipOffset.Width, shipPointer.Y + shipOffset.Height, SwfHelper.ShipCutinSize.Width, SwfHelper.ShipCutinSize.Height );

								}
							}
						}



						// equipments
						Point equipmentPointerOrigin = shipPointer + GetAlignmentOffset( ContentAlignment.BottomRight, equipmentAreaSize, shipPaneUnitSize );
						for ( int equipmentIndex = 0; equipmentIndex < 6; equipmentIndex++ ) {
							EquipmentData eq = ship.AllSlotInstance[equipmentIndex];

							int yIndex = equipmentIndex;
							if ( !has5thSlot && equipmentIndex >= 5 )		// 5スロ目がないとき、そのスペースを省略
								yIndex--;

							Point equipmentPointer = equipmentPointerOrigin + new Size( equipmentAreaUnitMargin.Left, equipmentAreaUnitMargin.Top + ( equipmentAreaUnitSize.Height + equipmentAreaUnitMargin.Vertical ) * yIndex );

							if ( equipmentIndex == 5 && ship.IsExpansionSlotAvailable ) {
								g.DrawLine( subLinePen, equipmentPointer.X + lineMargin, equipmentPointer.Y - 1, equipmentPointer.X + equipmentAreaUnitSize.Width - lineMargin, equipmentPointer.Y - 1 );
							}


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


						g.DrawLine( linePen, shipPointerOrigin.X + lineMargin, shipPointerOrigin.Y + shipPaneUnitSize.Height, shipPointerOrigin.X + shipPaneUnitSize.Width - lineMargin, shipPointerOrigin.Y + shipPaneUnitSize.Height );

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
			subLinePen.Dispose();

			return bitmap;
		}




		/// <summary>
		/// コンパクトにまとめた、バナー式の編成画像を出力します。
		/// </summary>
		public static Bitmap GenerateBannerBitmap( FleetImageArgument args ) {

			var formatTopLeft = GetStringFormat( ContentAlignment.TopLeft );
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
			var subLinePen = new Pen( subTextColor );
			subLinePen.DashStyle = DashStyle.Dash;

			string fleetAirSuperiorityTitle = "制空戦力";
			string fleetSearchingAbilityTitle = "索敵能力";

			// for measure space of strings
			Bitmap preimage = new Bitmap( 1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb );
			Graphics preg = Graphics.FromImage( preimage );

			bool has5thSlot = args.FleetIDs
				.Select( id => KCDatabase.Instance.Fleet[id] )
				.Where( f => f != null )
				.SelectMany( f => f.MembersInstance )
				.Any( s => s != null && s.SlotSize >= 5 );
			int slotCount = has5thSlot ? 6 : 5;

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
			Padding equipmentAreaUnitMargin = new Padding( 2, 1, 2, 0 );
			Padding shipPaneUnitMargin = new Padding( 2 );
			Padding fleetParameterAreaMargin = new Padding( 8, 0, 0, 4 );
			Padding fleetPaneUnitMargin = new Padding( 4 );
			Padding titleMargin = new Padding( 0, 0, 0, 2 );
			Padding commentMargin = new Padding( 2 );
			Padding commentPadding = new Padding( 2 );
			Padding entireMargin = new Padding();
			int lineMargin = 4;

			Size shipBannerSize = SwfHelper.ShipBannerSize;
			Size shipNameAreaSize = SumWidthMaxHeight( SwfHelper.ShipBannerSize, MaxWidthSumHeight( SumWidthMaxHeight( levelSize, smallDigit3Size ), SumWidthMaxHeight( EquipmentIconSize, smallDigit3Size ) ) );
			Size equipmentAreaUnitSize = SumWidthMaxHeight( EquipmentIconSize, equipmentNameSize, equipmentLevelSize );
			Size equipmentAreaSize = new Size( equipmentAreaUnitSize.Width + equipmentAreaUnitMargin.Horizontal, ( equipmentAreaUnitSize.Height + equipmentAreaUnitMargin.Vertical ) * slotCount );

			Size shipPaneUnitSize = MaxWidthSumHeight( shipNameAreaSize, equipmentAreaSize );
			Size shipPaneSize = new Size(
				( shipPaneUnitSize.Width + shipPaneUnitMargin.Horizontal ) * Math.Min( args.HorizontalShipCount, 6 ),
				( shipPaneUnitSize.Height + shipPaneUnitMargin.Vertical ) * (int)Math.Ceiling( 6.0 / args.HorizontalShipCount ) );

			Size fleetParameterAreaSize = SumWidthMaxHeight(
				EquipmentIconSize, fleetAirSuperiorityTitleSize, fleetAirSuperiorityValueEstimatedSize, fleetParameterAreaInnerMargin,
				EquipmentIconSize, fleetSearchingAbilityTitleSize, fleetSearchingAbilityValueEstimatedSize );

			Size fleetPaneUnitSize;
			bool isFleetNameAndParametersAreSameLine = fleetNameSize.Width + fleetParameterAreaSize.Width + fleetParameterAreaMargin.Horizontal < shipPaneSize.Width;
			if ( isFleetNameAndParametersAreSameLine ) {
				fleetPaneUnitSize = MaxWidthSumHeight( SumWidthMaxHeight( fleetNameSize, fleetParameterAreaSize + fleetParameterAreaMargin.Size ), shipPaneSize );
			} else {
				fleetPaneUnitSize = MaxWidthSumHeight( fleetNameSize, fleetParameterAreaSize + fleetParameterAreaMargin.Size, shipPaneSize );
			}

			Size fleetPaneSize = new Size( ( fleetPaneUnitSize.Width + fleetPaneUnitMargin.Horizontal ) * Math.Min( args.HorizontalFleetCount, args.FleetIDs.Length ),
				( fleetPaneUnitSize.Height + fleetPaneUnitMargin.Vertical ) * (int)Math.Ceiling( (double)args.FleetIDs.Length / args.HorizontalFleetCount ) );

			Size commentAreaSize = commentSize + commentPadding.Size;

			Size entireSize = MaxWidthSumHeight( titleSize + titleMargin.Size, fleetPaneSize, commentAreaSize + commentMargin.Size );


			// anchor
			equipmentNameSize.Width = shipPaneUnitSize.Width - EquipmentIconSize.Width - equipmentLevelSize.Width;
			equipmentAreaUnitSize.Width = shipPaneUnitSize.Width - equipmentAreaUnitMargin.Horizontal;

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


						if ( ship == null )
							continue;

						//g.DrawString( string.Format( "#{0}:", shipIndex + 1 ), args.MediumDigitFont, subTextBrush, new Rectangle( shipPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, shipIndexSize, shipNameAreaSize ), shipIndexSize ), formatMiddleLeft );
						//shipPointer.X += shipIndexSize.Width;

						if ( !DrawShipSwfImage( g, ship.MasterShip.ResourceName, args.ReflectDamageGraphic && ship.HPRate <= 0.5 ? SwfHelper.ShipResourceCharacterID.BannerDamaged : SwfHelper.ShipResourceCharacterID.BannerNormal, shipPointer.X, shipPointer.Y, SwfHelper.ShipBannerSize ) ) {
							// alternate drawing
							g.DrawString( ship.Name, args.MediumFont, mainTextBrush, new RectangleF( shipPointer.X, shipPointer.Y, SwfHelper.ShipBannerSize.Width, SwfHelper.ShipBannerSize.Height ), formatTopLeft );
						}
						shipPointer.X += shipBannerSize.Width;

						{
							var shipParameterPointer = shipPointer;
							Size paramNameSize = new Size( Max( levelSize, EquipmentIconSize ).Width, 0 );
							//lv.
							g.DrawString( "Lv.", args.SmallDigitFont, subTextBrush, new Rectangle( shipParameterPointer, smallDigit3Size ), formatMiddleLeft );
							g.DrawString( ship.Level.ToString(), args.SmallDigitFont, mainTextBrush, new Rectangle( shipParameterPointer + paramNameSize, smallDigit3Size ), formatMiddleRight );

							shipParameterPointer.Y += levelSize.Height;

							//luck
							g.DrawImage( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.ParameterLuck],
								shipParameterPointer.X, shipParameterPointer.Y, EquipmentIconSize.Width, EquipmentIconSize.Height );
							g.DrawString( ship.LuckTotal.ToString(), args.SmallDigitFont, mainTextBrush, new Rectangle( shipParameterPointer + paramNameSize, smallDigit3Size ), formatMiddleRight );
						}


						shipPointer.X = shipPointerOrigin.X;
						shipPointer.Y += shipNameAreaSize.Height;


						// equipments
						Point equipmentPointerOrigin = shipPointer;
						for ( int equipmentIndex = 0; equipmentIndex < 6; equipmentIndex++ ) {
							EquipmentData eq = ship.AllSlotInstance[equipmentIndex];

							int yIndex = equipmentIndex;
							if ( !has5thSlot && equipmentIndex >= 5 )		// 5スロ目がないとき、そのスペースを省略
								yIndex--;

							Point equipmentPointer = equipmentPointerOrigin + new Size( equipmentAreaUnitMargin.Left, equipmentAreaUnitMargin.Top + ( equipmentAreaUnitSize.Height + equipmentAreaUnitMargin.Vertical ) * yIndex );

							if ( equipmentIndex == 5 && ship.IsExpansionSlotAvailable ) {
								g.DrawLine( subLinePen, equipmentPointer.X + lineMargin, equipmentPointer.Y - 1, equipmentPointer.X + equipmentAreaUnitSize.Width - lineMargin, equipmentPointer.Y - 1 );
							}

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
							//equipmentPointer.X += equipmentNameSize.Width;

							if ( eq != null ) {
								if ( eq.Level > 0 ) {
									var currentEquipmentNameSize = MeasureString( g, equipmentName, args.SmallFont, MaxValueSize, formatMiddleLeft );
									equipmentPointer.X += Math.Min( currentEquipmentNameSize.Width, equipmentNameSize.Width );
									g.DrawString( "+" + eq.Level, args.SmallDigitFont, subTextBrush, new Rectangle( equipmentPointer + GetAlignmentOffset( ContentAlignment.MiddleLeft, equipmentLevelSize, equipmentAreaUnitSize ), equipmentLevelSize ), formatMiddleRight );
								}

							}

						}
						shipPointer.Y += equipmentAreaSize.Height;

						g.DrawLine( linePen, shipPointer.X + lineMargin, shipPointer.Y, shipPointer.X + shipPaneUnitSize.Width - lineMargin, shipPointer.Y );
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
			subLinePen.Dispose();

			return bitmap;
		}





		public static bool HasShipSwfImage( int[] fleets ) {

			try {

				var swfFiles = System.IO.Directory.GetFiles( Utility.Configuration.Config.Connection.SaveDataPath + @"\resources\swf\ships\", "*.swf", System.IO.SearchOption.TopDirectoryOnly )
					.Select( path => System.IO.Path.GetFileNameWithoutExtension( path ) )
					.Select( path => {
						int index = path.IndexOf( '_' );
						if ( index != -1 )
							path = path.Remove( index );
						return path;
					} );

				var resourceIDs = fleets.Select( f => KCDatabase.Instance.Fleet[f] )
					.Where( f => f != null )
					.SelectMany( f => f.MembersInstance )
					.Where( s => s != null )
					.Select( s => s.MasterShip.ResourceName )
					.Distinct();

				return !resourceIDs.Except( swfFiles ).Any();

			} catch ( Exception ) {

				return false;
			}
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



		/// <summary>
		/// 艦船画像を swf ファイルから読み込んで描画します。
		/// </summary>
		/// <param name="g">対象となる Graphics。</param>
		/// <param name="resourceID">艦船のリソース名(ファイル名)。</param>
		/// <param name="characterID">描画する画像の CharacterID 。</param>
		/// <param name="x">描画先の X 座標。</param>
		/// <param name="y">描画先の Y 座標。</param>
		/// <param name="size">描画する画像のサイズ。</param>
		/// <returns>描画に成功したかを返します。ファイルが存在しない場合などには false が返ります。</returns>
		protected static bool DrawShipSwfImage( Graphics g, string resourceID, SwfHelper.ShipResourceCharacterID characterID, int x, int y, Size size ) {
			try {
				using ( var shipImage = SwfHelper.GetShipSwfImage( resourceID, characterID ) ) {
					if ( shipImage == null )
						return false;

					g.DrawImage( shipImage, new Rectangle( x, y, size.Width, size.Height ) );
				}

				return true;
			} catch ( Exception ) {
				return false;
			}
		}


		/// <summary>
		/// 矩形範囲にコントロールを配置した際の座標を求めます。
		/// </summary>
		/// <param name="alignment">配置方法。</param>
		/// <param name="content">配置するコントロールのサイズ。</param>
		/// <param name="container">配置されるコンテナのサイズ。</param>
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


		/// <summary>
		/// 配置に合わせた文字列のフォーマットを返します。
		/// </summary>
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


	/// <summary>
	/// FleetImageGenerator クラスのメソッドに与えるパラメータ群を保持します。
	/// </summary>
	[DataContract( Name = "FleetImageArgument" )]
	public class FleetImageArgument {

		/// <summary> 対象となる艦隊IDのリスト </summary>
		[DataMember]
		public int[] FleetIDs;

		/// <summary> 艦隊を横に並べる最大数 </summary>
		[DataMember]
		public int HorizontalFleetCount;

		/// <summary> 艦船を横に並べる最大数 </summary>
		[DataMember]
		public int HorizontalShipCount;


		/// <summary> HP に応じて中破グラフィックを適用するか </summary>
		[DataMember]
		public bool ReflectDamageGraphic;

		/// <summary> Twitter の画像圧縮を回避する情報を埋め込むか </summary>
		[DataMember]
		public bool AvoidTwitterDeterioration;



		/// <summary> タイトルのフォント </summary>
		[IgnoreDataMember]
		public Font TitleFont;

		/// <summary> 大きい文字のフォント(艦隊名など) </summary>
		[IgnoreDataMember]
		public Font LargeFont;

		/// <summary> 通常の文字のフォント(艦船・装備など) </summary>
		[IgnoreDataMember]
		public Font MediumFont;

		/// <summary> 小さな文字のフォント() </summary>
		[IgnoreDataMember]
		public Font SmallFont;

		/// <summary> 通常の英数字フォント(Lvなど) </summary>	
		[IgnoreDataMember]
		public Font MediumDigitFont;

		/// <summary> 小さな英数字フォント(搭載機数など) </summary>
		[IgnoreDataMember]
		public Font SmallDigitFont;


		[DataMember]
		public SerializableFont SerializedTitleFont {
			get { return TitleFont; }
			set { TitleFont = value; }
		}
		[DataMember]
		public SerializableFont SerializedLargeFont {
			get { return LargeFont; }
			set { LargeFont = value; }
		}
		[DataMember]
		public SerializableFont SerializedMediumFont {
			get { return MediumFont; }
			set { MediumFont = value; }
		}
		[DataMember]
		public SerializableFont SerializedSmallFont {
			get { return SmallFont; }
			set { SmallFont = value; }
		}
		[DataMember]
		public SerializableFont SerializedMediumDigitFont {
			get { return MediumDigitFont; }
			set { MediumDigitFont = value; }
		}
		[DataMember]
		public SerializableFont SerializedSmallDigitFont {
			get { return SmallDigitFont; }
			set { SmallDigitFont = value; }
		}


		/// <summary> 背景画像ファイルへのパス(空白の場合描画されません) </summary>
		[DataMember]
		public string BackgroundImagePath;


		/// <summary> ユーザ指定のタイトル </summary>
		[DataMember]
		public string Title;

		/// <summary> ユーザ指定のコメント </summary>
		[DataMember]
		public string Comment;



		public FleetImageArgument() {
			BackgroundImagePath = "";
			Title = "";
			Comment = "";
		}


		public static FleetImageArgument GetDefaultInstance() {
			var ret = new FleetImageArgument();
			ret.FleetIDs = new int[0];
			ret.HorizontalFleetCount = 2;
			ret.HorizontalShipCount = 2;
			ret.AvoidTwitterDeterioration = true;

			ret.Fonts = GetDefaultFonts();

			return ret;
		}

		public static readonly string DefaultFontFamily = "Meiryo UI";
		public static readonly float[] DefaultFontPixels = new float[] { 32, 24, 16, 12, 16, 12 };

		public static Font[] GetDefaultFonts() {
			var fonts = new Font[DefaultFontPixels.Length];
			for ( int i = 0; i < fonts.Length; i++ ) {
				fonts[i] = new Font( DefaultFontFamily, DefaultFontPixels[i], i == 0 ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Pixel );
			}
			return fonts;
		}

		public FleetImageArgument Clone() {

			var clone = (FleetImageArgument)MemberwiseClone();

			clone.FleetIDs = FleetIDs.ToArray();

			clone.Fonts = Fonts.Select( f => f != null ? (Font)f.Clone() : null ).ToArray();

			return clone;
		}


		public void DisposeResources() {
			foreach ( var font in Fonts ) {
				if ( font != null )
					font.Dispose();
			}
		}


		public Font[] Fonts {
			get {
				return new Font[] { 
					TitleFont,
					LargeFont,
					MediumFont,
					SmallFont,
					MediumDigitFont,
					SmallDigitFont,
				};
			}
			set {
				TitleFont = value[0];
				LargeFont = value[1];
				MediumFont = value[2];
				SmallFont = value[3];
				MediumDigitFont = value[4];
				SmallDigitFont = value[5];
			}

		}

	}

}
