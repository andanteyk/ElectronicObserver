using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalArsenal.Control {
	public partial class ImageLabel : UserControl {


		#region Properties


		private Font _font;
		[Browsable( true )]
		[DefaultValue( typeof( Font ), "Meiryo UI, 12px" )]
		public override Font Font {		//checkme
			get { return _font; }
			set {
				_font = value;
				Refresh();
			}
		}

		private string _text;
		[Browsable( true )]
		[DefaultValue( "" )]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Visible )]
		[EditorBrowsable( EditorBrowsableState.Always )]
		[Bindable( BindableSupport.Default )]
		public override string Text {
			get { return _text; }
			set {
				_text = value;
				Refresh();
			}
		}

		private ContentAlignment _textAlign;
		[Browsable( true )]
		[DefaultValue( typeof( ContentAlignment ), "MiddleLeft" )]
		public ContentAlignment TextAlign {
			get { return _textAlign; }
			set {
				_textAlign = value;
				Refresh();
			}
		}


		private Image _image;
		[Browsable( true )]
		[DefaultValue( null )]
		public Image Image {
			get {
				if ( _imageList != null ) {
					if ( _imageKey != null && _imageKey != "" )
						return _imageList.Images[_imageKey];
					else if ( _imageIndex != -1 )
						return _imageList.Images[_imageIndex];
					else
						return _image;
				} else {
					return _image;
				}
			}
			set {
				_image = value;
				Refresh();
			}
		}

		private ContentAlignment _imageAlign;
		[Browsable( true )]
		[DefaultValue( typeof( ContentAlignment ), "MiddleLeft" )]
		public ContentAlignment ImageAlign {
			get { return _imageAlign; }
			set {
				_imageAlign = value;
				Refresh();
			}
		}


		private ImageList _imageList;
		[Browsable( true )]
		[DefaultValue( null )]
		public ImageList ImageList {
			get { return _imageList; }
			set {
				_imageList = value;
				Refresh();
			}
		}

		private string _imageKey;
		[Browsable( true )]
		[DefaultValue( null )]
		[RefreshProperties( System.ComponentModel.RefreshProperties.Repaint )]
		public string ImageKey {
			get { return _imageKey; }
			set {
				_imageKey = value;		//todo: update image data;
				Refresh();
			}
		}

		private int _imageIndex;
		[Browsable( true )]
		[DefaultValue( -1 )]
		[RefreshProperties( System.ComponentModel.RefreshProperties.Repaint )]
		public int ImageIndex {
			get { return _imageIndex; }
			set {
				_imageIndex = value;		//todo: update image data;
				Refresh();
			}
		}

		private int _imageMargin;
		[Browsable( true )]
		[DefaultValue( 0 )]
		public int ImageMargin {
			get { return _imageMargin; }
			set {
				_imageMargin = value;
				Refresh();
			}
		}

		//勝手に変わらないので注意
		private Size _imageSize;
		[Browsable( true )]
		[DefaultValue( typeof( Size ), "0, 0" )]
		public Size ImageSize {
			get { return _imageSize; }
			set {
				_imageSize = value;
				Refresh();
			}
		}
		

		#endregion



		public ImageLabel() {
			InitializeComponent();

			_text = "";
			_font = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			_textAlign = ContentAlignment.MiddleLeft;
			_imageAlign = ContentAlignment.MiddleLeft;
			_image = null;
			_imageList = null;
			_imageKey = null;
			_imageIndex = -1;
			_imageMargin = 0;

		}






		private void ImageLabel_Paint( object sender, PaintEventArgs e ) {

			//ImageAlign…？　いえ、知らない子ですね…

			Size maxsize = new Size( int.MaxValue, int.MaxValue );

			Rectangle basearea = new Rectangle( Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical );
			//e.Graphics.DrawRectangle( Pens.Magenta, basearea.X, basearea.Y, basearea.Width - 1, basearea.Height - 1 );

			Rectangle imagearea = new Rectangle( basearea.X, basearea.Y, ImageSize.Width, ImageSize.Height );

			switch ( ImageAlign ) {
				case ContentAlignment.TopLeft:
					imagearea.X += ImageMargin;
					imagearea.Y += ImageMargin;
					break;
				case ContentAlignment.MiddleLeft:
					imagearea.X += ImageMargin;
					imagearea.Y += basearea.Height / 2 - imagearea.Height / 2;
					break;
				case ContentAlignment.BottomLeft:
					imagearea.X += ImageMargin;
					imagearea.Y = basearea.Bottom - ( imagearea.Height + ImageMargin );
					break;

				case ContentAlignment.TopCenter:
					imagearea.X += basearea.Width / 2 - imagearea.Width / 2;
					imagearea.Y += ImageMargin;
					break;
				case ContentAlignment.MiddleCenter:
					imagearea.X += basearea.Width / 2 - imagearea.Width / 2;
					imagearea.Y += basearea.Height / 2 - imagearea.Height / 2;
					break;
				case ContentAlignment.BottomCenter:
					imagearea.X += basearea.Width / 2 - imagearea.Width / 2;
					imagearea.Y = basearea.Bottom - ( imagearea.Height + ImageMargin );
					break;		

				case ContentAlignment.TopRight:
					imagearea.X = basearea.Right - ( imagearea.Width + ImageMargin );
					imagearea.Y += ImageMargin;
					break;
				case ContentAlignment.MiddleRight:
					imagearea.X = basearea.Right - ( imagearea.Width + ImageMargin );
					imagearea.Y += basearea.Height / 2 - imagearea.Height / 2;
					break;
				case ContentAlignment.BottomRight:
					imagearea.X = basearea.Right - ( imagearea.Width + ImageMargin );
					imagearea.Y = basearea.Bottom - ( imagearea.Height + ImageMargin );
					break;
			}


			if ( Image != null ) {
				e.Graphics.DrawImage( Image, imagearea );
				//e.Graphics.DrawRectangle( Pens.Orange, imagearea.X, imagearea.Y, imagearea.Width - 1, imagearea.Height - 1 );
			}

			
			//text
			TextFormatFlags textformat = GetTextFormat( TextAlign );


			//Rectangle textarea = new Rectangle( imagearea.Right + ImageMargin, Padding.Top, basearea.Width - imagearea.Width - ImageMargin * 2, Height - Padding.Vertical );
			Rectangle textarea = ModifyTextArea( basearea, imagearea, ImageMargin, ImageAlign );	
			
			TextRenderer.DrawText( e.Graphics, Text, Font, textarea, ForeColor, textformat );
			//e.Graphics.DrawRectangle( Pens.Orange, textarea.X, textarea.Y, textarea.Width - 1, textarea.Height - 1 );

		}



		public override Size GetPreferredSize( Size proposedSize ) {

			Size ret = new Size( Padding.Horizontal, Padding.Vertical );

			TextFormatFlags textformat = GetTextFormat( TextAlign );
			Size sz_text = TextRenderer.MeasureText( Text, Font, new Size( int.MaxValue, int.MaxValue ), textformat );

			sz_text.Width -= (int)( Font.Size / 2 );

			switch ( ImageAlign ) { 
				case ContentAlignment.TopLeft:
				case ContentAlignment.MiddleLeft:
				case ContentAlignment.BottomLeft:
				case ContentAlignment.TopRight:
				case ContentAlignment.MiddleRight:
				case ContentAlignment.BottomRight:
					ret.Width += ImageSize.Width + ImageMargin * 2 + sz_text.Width;
					ret.Height += Math.Max( ImageSize.Height + ImageMargin * 2, sz_text.Height );
					break;

				case ContentAlignment.TopCenter:
				case ContentAlignment.BottomCenter:
					ret.Width += Math.Max( ImageSize.Width + ImageMargin * 2, sz_text.Width );
					ret.Height += ImageSize.Height + ImageMargin * 2 + sz_text.Height;
					break;

				case ContentAlignment.MiddleCenter:
					ret.Width += Math.Max( ImageSize.Width + ImageMargin * 2, sz_text.Width );
					ret.Height += Math.Max( ImageSize.Height + ImageMargin * 2, sz_text.Height );
					break;

			}
			
			return ret;
		}



		private static TextFormatFlags GetTextFormat( ContentAlignment align ) {

			TextFormatFlags textformat = TextFormatFlags.NoPadding;
			switch ( align ) {
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


		private static Rectangle ModifyTextArea( Rectangle textarea, Rectangle imagearea, int imageMargin, ContentAlignment align ) {

			switch ( align ) {
				case ContentAlignment.TopLeft:
				case ContentAlignment.MiddleLeft:
				case ContentAlignment.BottomLeft:
					textarea.X += imagearea.Width + imageMargin * 2;
					textarea.Width -= imagearea.Width + imageMargin * 2;
					break;

				case ContentAlignment.TopRight:
				case ContentAlignment.MiddleRight:
				case ContentAlignment.BottomRight:
					textarea.Width -= imagearea.Width + imageMargin * 2;
					break;

				case ContentAlignment.TopCenter:
					textarea.Y += imagearea.Height + imageMargin * 2;
					textarea.Height -= imagearea.Height + imageMargin * 2;
					break;
				case ContentAlignment.BottomCenter:
					textarea.Height -= imagearea.Height + imageMargin * 2;
					break;

				case ContentAlignment.MiddleCenter:
					break;		//どうしようもないので背景表示っぽく
			}

			return textarea;
		}


	}
}
