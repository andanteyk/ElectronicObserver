using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog {
	public partial class DialogFleetImageGenerator : Form {

		private FleetImageArgument CurrentArgument;
		private Font GeneralFont;

		private readonly TextBox[] TextFontList;
		private Regex LFtoCRLF = new Regex( @"\n|\r\n", RegexOptions.Multiline | RegexOptions.Compiled );


		public DialogFleetImageGenerator() {
			InitializeComponent();

			TextFontList = new TextBox[]{
				TextTitleFont,
				TextLargeFont,
				TextMediumFont,
				TextSmallFont,
				TextMediumDigitFont,
				TextSmallDigitFont,
			};

			for ( int i = 0; i < TextFontList.Length; i++ ) {
				int x  = i;
				this.Controls.Find( "Select" + TextFontList[i].Name.Remove( 0, 4 ), true ).First().Click += ( sender, e ) => SelectFont_Click( sender, e, x );
			}

			LoadConfiguration();

		}

		public DialogFleetImageGenerator( int fleetID )
			: this() {

			CurrentArgument.FleetIDs = new int[] { fleetID };
		}



		private void DialogFleetImageGenerator_Load( object sender, EventArgs e ) {

			ApplyToUI( CurrentArgument );

			UpdateButtonAlert();
		}



		private void LoadConfiguration() {
			var config = Utility.Configuration.Config.FleetImageGenerator;

			CurrentArgument = config.Argument.Clone();


			switch ( config.ImageType ) {
				case 0:
				default:
					ImageTypeCard.Checked = true;
					break;
				case 1:
					ImageTypeCutin.Checked = true;
					break;
				case 2:
					ImageTypeBanner.Checked = true;
					break;
			}

			switch ( config.OutputType ) {
				case 0:
				default:
					OutputTypeFile.Checked = true;
					break;
				case 1:
					OutputTypeClipboard.Checked = true;
					break;
			}

			OpenImageAfterOutput.Checked = config.OpenImageAfterOutput;

			SaveImageDialog.FileName = System.IO.Path.GetFileName( config.LastOutputPath );
			SaveImageDialog.InitialDirectory = string.IsNullOrWhiteSpace( config.LastOutputPath ) ? "" : System.IO.Path.GetDirectoryName( config.LastOutputPath );
		}

		private void SaveConfiguration() {
			var config = Utility.Configuration.Config.FleetImageGenerator;

			if ( config.Argument != null )
				config.Argument.DisposeResources();

			config.Argument = CurrentArgument.Clone();

			if ( ImageTypeCard.Checked )
				config.ImageType = 0;
			else if ( ImageTypeCutin.Checked )
				config.ImageType = 1;
			else if ( ImageTypeBanner.Checked )
				config.ImageType = 2;

			if ( OutputTypeFile.Checked )
				config.OutputType = 0;
			else if ( OutputTypeClipboard.Checked )
				config.OutputType = 1;

			config.OpenImageAfterOutput = OpenImageAfterOutput.Checked;

			config.LastOutputPath = SaveImageDialog.FileName;
		}



		private void ApplyToUI( FleetImageArgument args ) {

			// undone: プリセットはなしで

			int[] fleetIDs = args.FleetIDs ?? new int[0];

			TargetFleet1.Checked = fleetIDs.Contains( 1 );
			TargetFleet2.Checked = fleetIDs.Contains( 2 );
			TargetFleet3.Checked = fleetIDs.Contains( 3 );
			TargetFleet4.Checked = fleetIDs.Contains( 4 );

			Title.Text = args.Title;
			Comment.Text = string.IsNullOrWhiteSpace( args.Comment ) ? "" : LFtoCRLF.Replace( args.Comment, "\r\n" );		// 保存データからのロード時に \n に変換されてしまっているため


			HorizontalFleetCount.Value = args.HorizontalFleetCount;
			HorizontalShipCount.Value = args.HorizontalShipCount;

			ReflectDamageGraphic.Checked = args.ReflectDamageGraphic;
			AvoidTwitterDeterioration.Checked = args.AvoidTwitterDeterioration;

			BackgroundImagePath.Text = args.BackgroundImagePath;

			for ( int i = 0; i < TextFontList.Length; i++ ) {
				TextFontList[i].Text = SerializableFont.FontToString( args.Fonts[i], true );
			}
		}

		private FleetImageArgument ApplyToArgument( FleetImageArgument defaultValue = null ) {

			var ret = defaultValue == null ? new FleetImageArgument() : defaultValue.Clone();

			ret.FleetIDs = new[]{
				TargetFleet1.Checked ? 1 : 0,
				TargetFleet2.Checked ? 2 : 0,
				TargetFleet3.Checked ? 3 : 0,
				TargetFleet4.Checked ? 4 : 0 
			}.Where( i => i > 0 ).ToArray();

			ret.HorizontalFleetCount = (int)HorizontalFleetCount.Value;
			ret.HorizontalShipCount = (int)HorizontalShipCount.Value;

			ret.ReflectDamageGraphic = ReflectDamageGraphic.Checked;
			ret.AvoidTwitterDeterioration = AvoidTwitterDeterioration.Checked;

			var fonts = ret.Fonts;
			for ( int i = 0; i < fonts.Length; i++ ) {
				if ( fonts[i] != null )
					fonts[i].Dispose();
				fonts[i] = SerializableFont.StringToFont( TextFontList[i].Text, true );
			}
			ret.Fonts = fonts;

			ret.BackgroundImagePath = BackgroundImagePath.Text;

			ret.Title = Title.Text;
			ret.Comment = Comment.Text;

			return ret;
		}

		private int[] ToFleetIDs() {
			return new[]{
				TargetFleet1.Checked ? 1 : 0,
				TargetFleet2.Checked ? 2 : 0,
				TargetFleet3.Checked ? 3 : 0,
				TargetFleet4.Checked ? 4 : 0 
			}.Where( i => i > 0 ).ToArray();
		}


		private void ApplyGeneralFont_Click( object sender, EventArgs e ) {

			if ( GeneralFont != null ) {
				GeneralFont.Dispose();
			}
			GeneralFont = SerializableFont.StringToFont( TextGeneralFont.Text, true );

			if ( GeneralFont == null ) {
				MessageBox.Show( "フォント名が正しくありません。", "フォント変換失敗", MessageBoxButtons.OK, MessageBoxIcon.Error );
				TextGeneralFont.Text = "";
				return;
			}


			for ( int i = 0; i < TextFontList.Length; i++ ) {
				float size = FleetImageArgument.DefaultFontPixels[i];
				var unit = GraphicsUnit.Pixel;
				var style = FontStyle.Regular;

				var font = SerializableFont.StringToFont( TextFontList[i].Text, true );
				if ( font != null ) {
					size = font.Size;
					unit = font.Unit;
					style = font.Style;
					font.Dispose();
				}

				font = new Font( GeneralFont.FontFamily, size, style, unit );
				TextFontList[i].Text = SerializableFont.FontToString( font );
				font.Dispose();
			}

		}


		private void SelectGeneralFont_Click( object sender, EventArgs e ) {
			fontDialog1.Font = GeneralFont;
			if ( fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
				GeneralFont = fontDialog1.Font;
				TextGeneralFont.Text = SerializableFont.FontToString( GeneralFont, true );
			}
		}

		private void SelectFont_Click( object sender, EventArgs e, int index ) {
			fontDialog1.Font = SerializableFont.StringToFont( TextFontList[index].Text, true );
			if ( fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
				TextFontList[index].Text = SerializableFont.FontToString( fontDialog1.Font, true );
			}
		}


		private void SearchBackgroundImagePath_Click( object sender, EventArgs e ) {
			OpenImageDialog.FileName = BackgroundImagePath.Text;
			if ( OpenImageDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
				BackgroundImagePath.Text = OpenImageDialog.FileName;
			}
		}

		private void ClearBackgroundPath_Click( object sender, EventArgs e ) {
			BackgroundImagePath.Text = "";
		}




		private void ButtonOK_Click( object sender, EventArgs e ) {

			var args = ApplyToArgument();

			// validation
			if ( args.FleetIDs == null || args.FleetIDs.Length == 0 ) {
				MessageBox.Show( "出力する艦隊が指定されていません。", "入力値エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				args.DisposeResources();
				return;
			}

			if ( args.HorizontalFleetCount <= 0 || args.HorizontalShipCount <= 0 ) {
				MessageBox.Show( "艦隊・艦船の横幅は 1 以上にしてください。", "入力値エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				args.DisposeResources();
				return;
			}

			if ( args.Fonts.Any( f => f == null ) ) {
				MessageBox.Show( "未入力・不正なフォントが存在します。", "入力値エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				args.DisposeResources();
				return;
			}


			int mode;
			if ( ImageTypeCard.Checked )
				mode = 0;
			else if ( ImageTypeCutin.Checked )
				mode = 1;
			else if ( ImageTypeBanner.Checked )
				mode = 2;
			else
				mode = 0;


			try {

				if ( OutputTypeFile.Checked ) {

					if ( SaveImageDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

						using ( var image = GenerateFleetImage( args, mode ) ) {


							switch ( SaveImageDialog.FilterIndex ) {
								case 1:
								default:
									image.Save( SaveImageDialog.FileName, System.Drawing.Imaging.ImageFormat.Png );
									break;
								case 2:
									image.Save( SaveImageDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg );
									break;
							}

							if ( OpenImageAfterOutput.Checked )
								System.Diagnostics.Process.Start( SaveImageDialog.FileName );


						}

					} else return;

				} else if ( OutputTypeClipboard.Checked ) {

					using ( var image = GenerateFleetImage( args, mode ) ) {

						Clipboard.SetImage( image );
					}
				}



				if ( CurrentArgument != null )
					CurrentArgument.DisposeResources();
				CurrentArgument = args;
				SaveConfiguration();

			} catch ( Exception ex ) {

				MessageBox.Show( "画像の出力に失敗しました。\r\n" + ex.GetType().Name + "\r\n" + ex.Message, "編成画像出力失敗", MessageBoxButtons.OK, MessageBoxIcon.Error );

			} finally {
				args.DisposeResources();
			}


			Close();

		}

		private Bitmap GenerateFleetImage( FleetImageArgument args, int mode ) {
			switch ( mode ) {
				case 0:
				default:
					return FleetImageGenerator.GenerateCardBitmap( args );
				case 1:
					return FleetImageGenerator.GenerateCutinBitmap( args );
				case 2:
					return FleetImageGenerator.GenerateBannerBitmap( args );
			}
		}


		private void ButtonCancel_Click( object sender, EventArgs e ) {
			Close();
		}


		private void ImageTypeCard_CheckedChanged( object sender, EventArgs e ) {
			if ( ImageTypeCard.Checked )
				HorizontalShipCount.Value = 2;
		}

		private void ImageTypeCutin_CheckedChanged( object sender, EventArgs e ) {
			if ( ImageTypeCutin.Checked )
				HorizontalShipCount.Value = 1;
		}

		private void ImageTypeBanner_CheckedChanged( object sender, EventArgs e ) {
			if ( ImageTypeBanner.Checked )
				HorizontalShipCount.Value = 2;
		}



		private void UpdateButtonAlert() {

			bool visibility = false;

			if ( !Utility.Configuration.Config.Connection.SaveReceivedData || !Utility.Configuration.Config.Connection.SaveSWF ) {

				visibility = true;
				ButtonAlert.Text = "艦船画像保存設定が無効です(詳細表示...)";

			}

			if ( !FleetImageGenerator.HasShipSwfImage( ToFleetIDs() ) ) {

				visibility = true;
				ButtonAlert.Text = "艦船画像が足りません(詳細表示...)";

			}

			ButtonAlert.Visible = visibility;

		}


		private void ButtonAlert_Click( object sender, EventArgs e ) {

			if ( !Utility.Configuration.Config.Connection.SaveReceivedData || !Utility.Configuration.Config.Connection.SaveSWF ) {

				if ( MessageBox.Show( "編成画像を出力するためには、艦船画像を保存する設定を有効にする必要があります。\r\n有効にしますか？",
					"艦船画像保存設定が無効です", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1 )
					== System.Windows.Forms.DialogResult.Yes ) {

					if ( !Utility.Configuration.Config.Connection.SaveReceivedData ) {
						Utility.Configuration.Config.Connection.SaveReceivedData = true;
						Utility.Configuration.Config.Connection.SaveResponse = false;		// もともと不要にしていたユーザーには res は邪魔なだけだと思うので
					}
					Utility.Configuration.Config.Connection.SaveSWF = true;

					UpdateButtonAlert();
				}

			}

			if ( !FleetImageGenerator.HasShipSwfImage( ToFleetIDs() ) ) {

				MessageBox.Show( "現在の艦隊を出力するための艦船画像データが不足しています。\r\n\r\nキャッシュを削除したのち再読み込みを行い、\r\n艦これ本体側で出力したい艦隊の編成ページを開くと\r\n艦船画像データが保存されます。",
					"艦船画像データ不足", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );

				UpdateButtonAlert();
			}

		}



		private void TargetFleet1_CheckedChanged( object sender, EventArgs e ) {
			UpdateButtonAlert();
		}


		private void ButtonClearFont_Click( object sender, EventArgs e ) {

			if ( MessageBox.Show( "フォントをデフォルト設定に戻します。\r\nよろしいですか？", "クリア確認",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 )
				 == System.Windows.Forms.DialogResult.Yes ) {

				if ( GeneralFont != null )
					GeneralFont.Dispose();
				GeneralFont = null;
				TextGeneralFont.Text = "";

				for ( int i = 0; i < TextFontList.Length; i++ ) {
					using ( var font = new Font( FleetImageArgument.DefaultFontFamily, FleetImageArgument.DefaultFontPixels[i], FontStyle.Regular, GraphicsUnit.Pixel ) ) {
						TextFontList[i].Text = SerializableFont.FontToString( font );
					}
				}
			}
		}



		private void DialogFleetImageGenerator_FormClosing( object sender, FormClosingEventArgs e ) {
			CurrentArgument.DisposeResources();
		}


	}
}
