using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Support;
using SwfExtractor;
using SwfExtractor.Tags;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog {
	public partial class DialogShipGraphicViewer : Form {

		Dictionary<string, SwfParser> Parsers;
		List<ImageTag> ImageTags;
		int CurrentIndex;
		Bitmap CurrentImage;

		Point ImageOffset = new Point();
		Point PreviousMouseLocation = new Point();
		double ZoomRate = 1;
		InterpolationMode Interpolation = InterpolationMode.NearestNeighbor;
		bool AdvMode = false;
		bool RestrictedPatch = false;
		bool DrawsInformation = true;


		private DialogShipGraphicViewer() {
			InitializeComponent();
			Parsers = new Dictionary<string, SwfParser>();
			ImageTags = new List<ImageTag>();

			MouseWheel += DialogShipGraphicViewer_MouseWheel;

			SetStyle( ControlStyles.ResizeRedraw, true );
			ControlHelper.SetDoubleBuffered( DrawingPanel );

			OpenSwfDialog.InitialDirectory = Utility.Configuration.Config.Connection.SaveDataPath + @"\resources\swf\ships";


			// 背景画像生成
			var back = new Bitmap( 16, 16, PixelFormat.Format24bppRgb );
			using ( var g = Graphics.FromImage( back ) ) {
				g.Clear( Color.White );
				g.FillRectangle( Brushes.LightGray, new Rectangle( 0, 0, 8, 8 ) );
				g.FillRectangle( Brushes.LightGray, new Rectangle( 8, 8, 8, 8 ) );
			}
			DrawingPanel.BackgroundImage = back;

		}

		public DialogShipGraphicViewer( string path )
			: this() {

			OpenSwf( path );
		}

		public DialogShipGraphicViewer( string[] pathlist )
			: this() {
			OpenSwf( pathlist );
		}


		private void DialogShipGraphicViewer_Load( object sender, EventArgs e ) {

			this.Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormAlbumShip] );

			RefreshImage();
		}


		private void DialogShipGraphicViewer_FormClosed( object sender, FormClosedEventArgs e ) {
			if ( DrawingPanel.BackgroundImage != null )
				DrawingPanel.BackgroundImage.Dispose();
			ResourceManager.DestroyIcon( Icon );
		}



		#region Input Controls

		private void DialogShipGraphicViewer_KeyDown( object sender, KeyEventArgs e ) {

			if ( ImageTags.Count == 0 )
				return;

			bool fileChanged = false;

			if ( e.KeyCode == Keys.Left ) {
				CurrentIndex--;
				fileChanged = true;
			} else if ( e.KeyCode == Keys.Right ) {
				CurrentIndex++;
				fileChanged = true;

			} else if ( e.KeyCode == Keys.Up ) {
				ZoomRate += 0.1;
			} else if ( e.KeyCode == Keys.Down ) {
				ZoomRate -= 0.1;

			} else if ( e.KeyCode == Keys.Oemtilde ) {	// "@"
				AdvMode = !AdvMode;
				RestrictedPatch = AdvMode && e.Shift;

			} else return;


			if ( fileChanged ) {
				CurrentIndex %= ImageTags.Count;
				if ( CurrentIndex < 0 )
					CurrentIndex += ImageTags.Count;

				if ( CurrentImage != null )
					CurrentImage.Dispose();
				CurrentImage = ImageTags[CurrentIndex].ExtractImage();

				ImageOffset = new Point();
				ZoomRate = 1;
			}

			ValidateParameters();
			RefreshImage();
		}

		void DialogShipGraphicViewer_MouseWheel( object sender, MouseEventArgs e ) {
			double wheel_delta = 120;
			ZoomRate += ( e.Delta / wheel_delta ) * 0.1;
			ValidateParameters();
			RefreshImage();
		}

		private void DrawingPanel_MouseMove( object sender, MouseEventArgs e ) {
			if ( e.Button == System.Windows.Forms.MouseButtons.Left ) {
				ImageOffset.X += e.Location.X - PreviousMouseLocation.X;
				ImageOffset.Y += e.Location.Y - PreviousMouseLocation.Y;
				RefreshImage();
			}
			PreviousMouseLocation = e.Location;
		}

		private void DrawingPanel_MouseClick( object sender, MouseEventArgs e ) {
			PreviousMouseLocation = e.Location;
		}


		#endregion


		#region View Control


		private void TopMenu_View_InterpolationMode_Sharp_Click( object sender, EventArgs e ) {

			foreach ( ToolStripMenuItem  item in TopMenu_View_InterpolationMode.DropDownItems ) {
				if ( object.ReferenceEquals( item, sender ) )
					item.CheckState = CheckState.Indeterminate;
				else
					item.CheckState = CheckState.Unchecked;
			}

			if ( TopMenu_View_InterpolationMode_Sharp.Checked )
				Interpolation = InterpolationMode.NearestNeighbor;
			else if ( TopMenu_View_InterpolationMode_Smooth.Checked )
				Interpolation = InterpolationMode.HighQualityBicubic;

			RefreshImage();
		}


		private void TopMenu_View_Zoom_In_Click( object sender, EventArgs e ) {
			ZoomRate += 0.1;
			ValidateParameters();
		}

		private void TopMenu_View_Zoom_Out_Click( object sender, EventArgs e ) {
			ZoomRate -= 0.1;
			ValidateParameters();
		}

		private void TopMenu_View_Zoom_100_Click( object sender, EventArgs e ) {
			ZoomRate = 1;
			RefreshImage();
		}

		private void TopMenu_View_Zoom_Fit_Click( object sender, EventArgs e ) {
			RefreshImage();
		}

		private void ValidateParameters() {
			ZoomRate = Math.Min( Math.Max( ZoomRate, 0.1 ), 16 );
		}

		#endregion


		#region File Control

		private void OpenSwf( string path ) {
			OpenSwf( new string[] { path } );
		}

		private void OpenSwf( string[] pathlist ) {		//undone: null check at args
			try {
				Parsers.Clear();
				for ( int i = 0; i < pathlist.Length; i++ ) {
					if ( File.Exists( pathlist[i] ) ) {
						Parsers.Add( pathlist[i], new SwfParser() );
						Parsers[pathlist[i]].Parse( pathlist[i] );
					}

				}

				ImageTags = Parsers.Values.SelectMany( p => p.FindTags<ImageTag>() ).ToList();

				if ( ImageTags.Count == 0 ) {
					throw new InvalidOperationException( "展開しましたが、画像が見つかりませんでした。" );
				}

				CurrentIndex = 0;
				if ( CurrentImage != null )
					CurrentImage.Dispose();
				CurrentImage = ImageTags[CurrentIndex].ExtractImage();

			} catch ( Exception ex ) {
				MessageBox.Show( string.Join( "\r\n", pathlist ) + "を開けませんでした。\r\n" + ex.GetType().Name + "\r\n" + ex.Message );
				Parsers.Clear();
				ImageTags.Clear();
				if ( CurrentImage != null ) {
					CurrentImage.Dispose();
					CurrentImage = null;
				}

			} finally {
				RefreshImage();
			}

		}


		private void TopMenu_File_Open_Click( object sender, EventArgs e ) {
			if ( OpenSwfDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
				OpenSwf( OpenSwfDialog.FileNames );
			}
		}

		private void TopMenu_File_SaveImage_Click( object sender, EventArgs e ) {
			if ( CurrentImage == null ) {
				System.Media.SystemSounds.Exclamation.Play();
				return;
			}

			if ( SaveImageDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
				try {

					CurrentImage.Save( SaveImageDialog.FileName, ImageFormat.Png );

				} catch ( Exception ex ) {

					MessageBox.Show( SaveImageDialog.FileName + "\r\nへの保存に失敗しました。\r\n" + ex.GetType().Name + "\r\n" + ex.Message );

				}
			}
		}

		private void TopMenu_File_SaveAllImage_Click( object sender, EventArgs e ) {

			if ( !ImageTags.Any() ) {
				System.Media.SystemSounds.Exclamation.Play();
				return;
			}

			if ( SaveFolderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

				try {
					foreach ( var parser in Parsers ) {

						string dirname = Path.GetFileNameWithoutExtension( parser.Key );

						foreach ( var tag in parser.Value.FindTags<ImageTag>() ) {
							using ( var img = tag.ExtractImage() ) {

								img.Save( SaveFolderDialog.SelectedPath.TrimEnd( '\\' ) + "\\" + dirname + "_" + tag.CharacterID + ".png", ImageFormat.Png );

							}
						}
					}

				} catch ( Exception ex ) {

					MessageBox.Show( SaveFolderDialog.SelectedPath + "\r\nへの保存に失敗しました。\r\n" + ex.GetType().Name + "\r\n" + ex.Message );
				}

			}

		}


		private void TopMenu_File_CopyToClipboard_Click( object sender, EventArgs e ) {
			if ( CurrentImage == null ) {
				System.Media.SystemSounds.Exclamation.Play();
				return;
			}

			try {

				Clipboard.SetImage( CurrentImage );

			} catch ( Exception ) {

				System.Media.SystemSounds.Exclamation.Play();
			}
		}


		private void DialogShipGraphicViewer_DragEnter( object sender, DragEventArgs e ) {
			if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;
		}

		private void DialogShipGraphicViewer_DragDrop( object sender, DragEventArgs e ) {
			OpenSwf( (string[])e.Data.GetData( DataFormats.FileDrop ) );
		}

		#endregion


		#region Drawing

		private void DrawingPanel_Paint( object sender, PaintEventArgs e ) {
			if ( CurrentImage == null )
				return;

			e.Graphics.InterpolationMode = Interpolation;
			e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			var panelSize = DrawingPanel.Size;

			double zoomRate = ZoomRate;
			if ( TopMenu_View_Zoom_Fit.Checked ) {
				zoomRate = Math.Min( (double)panelSize.Width / CurrentImage.Width, (double)panelSize.Height / CurrentImage.Height );
			}

			var imgSize = new SizeF( (float)( CurrentImage.Width * zoomRate ), (float)( CurrentImage.Height * zoomRate ) );


			if ( DrawsInformation ) {
				var parentParser = Parsers.First( p => p.Value.Tags.Any( t => object.ReferenceEquals( t, ImageTags[CurrentIndex] ) ) );
				var ship = GetShipFromPath( parentParser.Key );

				e.Graphics.DrawString(
					string.Format( "{0} / {1}\r\n{2} ({3}) CID: {4}\r\nZoom {5:p1}\r\n(←/→キーでページめくり)", CurrentIndex + 1, ImageTags.Count, Path.GetFileName( parentParser.Key ), ship != null ? ship.NameWithClass : "???", ImageTags[CurrentIndex].CharacterID, zoomRate ),
					Font, Brushes.DimGray, new PointF( 0, 0 ) );
			}

			e.Graphics.DrawImage( CurrentImage, new RectangleF( ( panelSize.Width - imgSize.Width ) / 2 + ImageOffset.X, ( panelSize.Height - imgSize.Height ) / 2 + ImageOffset.Y, imgSize.Width, imgSize.Height ) );


			if ( AdvMode ) {
				DrawAdvMode( e.Graphics );
			}

		}

		// かん☆これ!!
		private void DrawAdvMode( Graphics g ) {

			var parentParser = Parsers.First( p => p.Value.Tags.Any( t => object.ReferenceEquals( t, ImageTags[CurrentIndex] ) ) );
			var ship = GetShipFromPath( parentParser.Key );
			if ( ship == null )
				return;

			var panelSize = DrawingPanel.Size;

			int mainHeight = 200;
			int nameHeight = 64;
			var mainBound = new Rectangle( 16, panelSize.Height - mainHeight - 16, panelSize.Width - 32, mainHeight );
			var innerBound = new Rectangle( mainBound.X + 16, mainBound.Y + 16, mainBound.Width - 32, mainBound.Height - 32 );
			var nameBound = new Rectangle( 16, mainBound.Y - nameHeight - 16, 256, nameHeight );
			var nameInnerBound = new Rectangle( nameBound.X + 16, nameBound.Y + 16, nameBound.Width - 32, nameBound.Height - 32 );

			var rand = new Random( 401168 );		// for fixed drawing



			// window shadow
			using ( var pen = new Pen( Color.RoyalBlue, 8 ) ) {
				pen.LineJoin = LineJoin.Round;
				g.DrawRectangle( pen, new Rectangle( mainBound.X + 2, mainBound.Y + 2, mainBound.Width, mainBound.Height ) );
				g.DrawRectangle( pen, new Rectangle( nameBound.X + 2, nameBound.Y + 2, nameBound.Width, nameBound.Height ) );
			}

			// window base
			using ( var grad = new LinearGradientBrush( mainBound, Color.FromArgb( 0xcc, 0x44, 0xcc, 0xff ), Color.FromArgb( 0xcc, 0x44, 0x44, 0xff ), 90 ) ) {
				g.FillRectangle( grad, mainBound );
			}
			using ( var grad = new LinearGradientBrush( nameBound, Color.FromArgb( 0xcc, 0x44, 0xcc, 0xff ), Color.FromArgb( 0xcc, 0x44, 0x44, 0xff ), 90 ) ) {
				g.FillRectangle( grad, nameBound );
			}


			// decorations
			g.SetClip( mainBound );

			using ( var pen = new Pen( Color.FromArgb( 0x44, 0xcc, 0xee, 0xff ), 4 ) ) {
				Point center = new Point( mainBound.X + mainHeight * 3 / 4, mainBound.Y + mainHeight * 3 / 4 );

				// compass
				g.DrawEllipse( pen, new Rectangle( center.X - 150, center.Y - 150, 300, 300 ) );
				g.DrawEllipse( pen, new Rectangle( center.X - 140, center.Y - 140, 280, 280 ) );
				g.DrawEllipse( pen, new Rectangle( center.X - 16, center.Y - 16, 32, 32 ) );

				int direct = (int)( 120 * Math.Sin( Math.PI / 4 ) );

				var path = new GraphicsPath();
				path.AddLines( new Point[] { 
						new Point( center.X + direct, center.Y - direct ),
						new Point( center.X + 16, center.Y + 16 ),
						new Point( center.X - direct, center.Y + direct ),
						new Point( center.X - 16, center.Y - 16 )
					} );
				path.CloseFigure();
				g.DrawPath( pen, path );


				// bubbles
				for ( int i = 0; i < 16; i++ ) {
					int r = (int)( 8 + rand.NextDouble() * 24 * i / 8 );
					g.DrawEllipse( pen, new Rectangle(
						mainBound.Right - 96 + (int)( ( rand.NextDouble() * 128 - 64 ) * ( i + 1 ) / 16 ),
						mainBound.Bottom - 16 - ( i * mainBound.Height ) / 16,
						r, r
						) );
				}
			}

			g.ResetClip();


			// window frame
			using ( var pen = new Pen( Color.White, 8 ) ) {
				pen.LineJoin = LineJoin.Round;
				g.DrawRectangle( pen, mainBound );
				g.DrawRectangle( pen, nameBound );
			}


			// text
			// don't think (about env that font is not installed), feel
			using ( var font = new Font( "にゃしぃフォント改二", 32, FontStyle.Regular, GraphicsUnit.Pixel ) ) {
				var rec = Resource.Record.RecordManager.Instance.ShipParameter[ship.ShipID];
				string mes = null;

				if ( rec == null ) {
					mes = "……";

				} else {
					var processedShips = new LinkedList<ShipDataMaster>();
					processedShips.AddLast( ship );

					while ( processedShips.Last.Value.RemodelBeforeShip != null && !processedShips.Any( s2 => processedShips.Last.Value.RemodelBeforeShipID == s2.ShipID ) ) {
						processedShips.AddLast( processedShips.Last.Value.RemodelBeforeShip );
					}

					foreach ( var s in processedShips ) {
						string mescan = s.MessageGet.Replace( "<br>", "\r\n" );
						if ( !string.IsNullOrWhiteSpace( mescan ) ) {
							mes = mescan;
							break;
						}
					}

					if ( mes == null ) {
						foreach ( var s in processedShips ) {
							string mescan = s.MessageAlbum.Replace( "<br>", "\r\n" );
							if ( !string.IsNullOrWhiteSpace( mescan ) ) {
								mes = mescan;
								break;
							}
						}
					}

					if ( mes == null )
						mes = "……";
				}

				if ( RestrictedPatch ) {
					mes = mes
						.Replace( "。", "♥" )
						.Replace( ".\r\n", "♥\r\n" )
						.Replace( "……", "…" )
						.Replace( "…", "…♥" )
						.Replace( "！", "！♥" )
						.Replace( "？", "？♥" )
						.Replace( "!", "!♥" )
						.Replace( "?", "?♥" )
						;
				}

				g.DrawString( mes, font, Brushes.RoyalBlue, new Rectangle( innerBound.Location + new Size( 2, 2 ), innerBound.Size ) );
				g.DrawString( mes, font, Brushes.White, innerBound );

				g.DrawString( ship.NameWithClass, font, Brushes.RoyalBlue, new Rectangle( nameInnerBound.Location + new Size( 2, 2 ), nameInnerBound.Size ) );
				g.DrawString( ship.NameWithClass, font, Brushes.White, nameInnerBound );
			}
		}

		private void DrawingPanel_Resize( object sender, EventArgs e ) {
			RefreshImage();
		}


		private void RefreshImage() {
			DrawingPanel.Refresh();
		}

		#endregion



		private ShipDataMaster GetShipFromPath( string path ) {
			path = Path.GetFileNameWithoutExtension( path );
			int verindex = path.LastIndexOf( '_' );
			if ( verindex != -1 )
				path = path.Substring( 0, verindex );

			var rec = Resource.Record.RecordManager.Instance.ShipParameter.Record.Values.FirstOrDefault( r => r.ResourceName != null && r.ResourceName.Contains( path ) );

			if ( rec != null ) {
				var ship = KCDatabase.Instance.MasterShips[rec.ShipID];
				return ship ?? KCDatabase.Instance.MasterShips[rec.OriginalCostumeShipID];
			} else {
				return null;
			}
		}


	}
}
