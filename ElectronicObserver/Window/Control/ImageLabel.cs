using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Design;
using ElectronicObserver.Resource;

namespace ElectronicObserver.Window.Control {
	public partial class ImageLabel : UserControl {


		#region Properties


		#region DefaultValues

		[DefaultValue( true )]
		public override bool AutoSize {
			get {
				return base.AutoSize;
			}
			set {
				base.AutoSize = value;
			}
		}

		
		[DefaultValue( typeof( AutoSizeMode ), "GrowAndShrink" )]
		public new AutoSizeMode AutoSizeMode {
			get {
				return base.AutoSizeMode;
			}
			set {
				base.AutoSizeMode = value;
			}
		}
		
		#endregion


		/*
		[Browsable( true )]
		[DefaultValue( typeof( Font ), "Meiryo UI, 12px" )]
		public override Font Font {		//checkme
			get { return base.Font; }
			set {
				base.Font = value;
				PropertyChanged();
			}
		}
		*/


		[Browsable( true )]
		[DefaultValue( "" )]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Visible )]
		[EditorBrowsable( EditorBrowsableState.Always )]
		[Bindable( BindableSupport.Default )]
		[Editor( "System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof( System.Drawing.Design.UITypeEditor ) )]
		public override string Text {
			get { return base.Text; }
			set {
				base.Text = value;
				PropertyChanged();
			}
		}

		private ContentAlignment _textAlign;
		[Browsable( true )]
		[DefaultValue( typeof( ContentAlignment ), "MiddleLeft" )]
		[Description( "ラベル内のテキストの位置を決定します。" )]
		[Category( "表示" )]
		public ContentAlignment TextAlign {
			get { return _textAlign; }
			set {
				_textAlign = value;
				PropertyChanged();
			}
		}


		private Image _image;
		[Browsable( true )]
		[DefaultValue( null )]
		[Description( "コントロールに表示されるイメージです。" )]
		[Category( "表示" )]
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
				PropertyChanged();
			}
		}

		private ContentAlignment _imageAlign;
		[Browsable( true )]
		[DefaultValue( typeof( ContentAlignment ), "MiddleLeft" )]
		[Description( "コントロールに表示されるイメージの配置です。" )]
		[Category( "表示" )]
		public ContentAlignment ImageAlign {
			get { return _imageAlign; }
			set {
				_imageAlign = value;
				PropertyChanged();
			}
		}


		private ImageCollection _imageList;
		[Browsable( true )]
		[DefaultValue( null )]
		[Description( "コントロールに表示するイメージを取得するための ImageList です。" )]
		[Category( "表示" )]
		public ImageCollection ImageList {
			get { return _imageList; }
			set {
				_imageList = value;
				PropertyChanged();
			}
		}

		private string _imageKey;
		[Browsable( true )]
		[DefaultValue( null )]
		[RefreshProperties( System.ComponentModel.RefreshProperties.Repaint )]
		[Editor( "System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof( UITypeEditor ) )]
		[TypeConverter( typeof( ImageKeyConverter ) )]
		[Description( "コントロールに表示される ImageList の中のイメージのインデックスです。" )]
		[Category( "表示" )]
		public string ImageKey {
			get { return _imageKey; }
			set {
				_imageKey = value;
				PropertyChanged();
			}
		}

		private int _imageIndex;
		[Browsable( true )]
		[DefaultValue( -1 )]
		[RefreshProperties( System.ComponentModel.RefreshProperties.Repaint )]
		[Editor( "System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof( UITypeEditor ) )]
		[TypeConverter( typeof( ImageIndexConverter ) )]
		[Description( "コントロールに表示される ImageList の中のイメージのインデックスです。" )]
		[Category( "表示" )]
		public int ImageIndex {
			get { return _imageIndex; }
			set {
				_imageIndex = value;
				PropertyChanged();
			}
		}

		private int _imageMargin;
		[Browsable( true )]
		[DefaultValue( 3 )]
		[Description( "イメージとラベルの間のスペースを指定します。" )]
		[Category( "表示" )]
		public int ImageMargin {
			get { return _imageMargin; }
			set {
				_imageMargin = value;
				PropertyChanged();
			}
		}

		
		private Size _imageSize;
		[Browsable( true )]
		[DefaultValue( typeof( Size ), "16, 16" )]
		[Description( "イメージのサイズを指定します。" )]
		[Category( "表示" )]
		public Size ImageSize {
			get {
				if ( Image != null )
					return Image.Size;
				else
					return _imageSize;
			}
			set {
				_imageSize = value;
				PropertyChanged();
			}
		}


		private bool _autoWrap;
		[Browsable( true )]
		[DefaultValue( false )]
		[Description( "ラベル コントロールの幅を超えるテキストの自動処理を有効にします。" )]
		[Category( "動作" )]
		public bool AutoWrap {
			get { return _autoWrap; }
			set {
				_autoWrap = value;
				PropertyChanged();
			}
		}


		private bool _autoEllipsis;
		[Browsable( true )]
		[DefaultValue( false )]
		[Description( "ラベル コントロールの幅を超えるテキストの自動処理を有効にします。" )]
		[Category( "動作" )]
		public bool AutoEllipsis {
			get { return _autoEllipsis; }
			set {
				_autoEllipsis = value;
				PropertyChanged();
			}
		}


		private bool _showText;
		[Browsable( true )]
		[DefaultValue( true )]
		[Description( "设置是否显示文字。" )]
		[Category( "動作" )]
		public bool ShowText {
			get { return _showText; }
			set {
				_showText = value;
				PropertyChanged();
			}
		}
		

		#endregion



		public ImageLabel() {
			InitializeComponent();

			Text = "";
			//Font = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			_textAlign = ContentAlignment.MiddleLeft;
			_imageAlign = ContentAlignment.MiddleLeft;
			_image = null;
			_imageList = null;
			_imageKey = null;
			_imageIndex = -1;
			_imageMargin = 3;
			_imageSize = new Size( 16, 16 );
			_autoWrap = false;
			_autoEllipsis = false;
			_showText = true;

		}






		private void ImageLabel_Paint( object sender, PaintEventArgs e ) {

			Rectangle basearea = new Rectangle( Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical );
			//e.Graphics.DrawRectangle( Pens.Magenta, basearea.X, basearea.Y, basearea.Width - 1, basearea.Height - 1 );

			Rectangle imagearea = new Rectangle( basearea.X, basearea.Y, ImageSize.Width, ImageSize.Height );

			switch ( ImageAlign ) {
				case ContentAlignment.TopLeft:
					break;
				case ContentAlignment.MiddleLeft:
					imagearea.Y += basearea.Height / 2 - imagearea.Height / 2;
					break;
				case ContentAlignment.BottomLeft:
					imagearea.Y = basearea.Bottom - imagearea.Height;
					break;

				case ContentAlignment.TopCenter:
					imagearea.X += basearea.Width / 2 - imagearea.Width / 2;
					break;
				case ContentAlignment.MiddleCenter:
					imagearea.X += basearea.Width / 2 - imagearea.Width / 2;
					imagearea.Y += basearea.Height / 2 - imagearea.Height / 2;
					break;
				case ContentAlignment.BottomCenter:
					imagearea.X += basearea.Width / 2 - imagearea.Width / 2;
					imagearea.Y = basearea.Bottom - imagearea.Height;
					break;		

				case ContentAlignment.TopRight:
					imagearea.X = basearea.Right - imagearea.Width;
					break;
				case ContentAlignment.MiddleRight:
					imagearea.X = basearea.Right - imagearea.Width;
					imagearea.Y += basearea.Height / 2 - imagearea.Height / 2;
					break;
				case ContentAlignment.BottomRight:
					imagearea.X = basearea.Right - imagearea.Width;
					imagearea.Y = basearea.Bottom - imagearea.Height;
					break;
			}


			if ( Image != null ) {
				e.Graphics.DrawImage( Image, imagearea );
				//e.Graphics.DrawRectangle( Pens.Orange, imagearea.X, imagearea.Y, imagearea.Width - 1, imagearea.Height - 1 );
			}

			
			//text
			if ( ShowText ) {
				TextFormatFlags textformat = GetTextFormat( TextAlign, AutoWrap, AutoEllipsis );


				Rectangle textarea = ModifyTextArea( basearea, imagearea, ImageMargin, ImageAlign );

				TextRenderer.DrawText( e.Graphics, Text, Font, textarea, ForeColor, textformat );
				//e.Graphics.DrawRectangle( Pens.Orange, textarea.X, textarea.Y, textarea.Width - 1, textarea.Height - 1 );
			}
		}



		public Size GetPreferredSize() {

			Size ret = new Size( Padding.Horizontal, Padding.Vertical );

			TextFormatFlags textformat = GetTextFormat( TextAlign, AutoWrap, AutoEllipsis );
			Size sz_text;

			if ( ShowText ) {
				sz_text = TextRenderer.MeasureText( Text, Font, new Size( int.MaxValue, int.MaxValue ), textformat );

				if ( Text.Length > 0 )
					sz_text.Width -= (int)( Font.Size / 2 );
			} else {

				sz_text = Size.Empty;
			}

			switch ( ImageAlign ) { 
				case ContentAlignment.TopLeft:
				case ContentAlignment.MiddleLeft:
				case ContentAlignment.BottomLeft:
				case ContentAlignment.TopRight:
				case ContentAlignment.MiddleRight:
				case ContentAlignment.BottomRight:
					ret.Width += ImageSize.Width + ImageMargin + sz_text.Width;
					ret.Height += Math.Max( ImageSize.Height, sz_text.Height );
					break;

				case ContentAlignment.TopCenter:
				case ContentAlignment.BottomCenter:
					ret.Width += Math.Max( ImageSize.Width, sz_text.Width );
					ret.Height += ImageSize.Height + ImageMargin + sz_text.Height;
					break;

				case ContentAlignment.MiddleCenter:
					ret.Width += Math.Max( ImageSize.Width, sz_text.Width );
					ret.Height += Math.Max( ImageSize.Height, sz_text.Height );
					break;

			}
			
			return ret;
		}


		public override Size GetPreferredSize( Size proposedSize ) {
			var size = GetPreferredSize();
			if ( !MaximumSize.IsEmpty ) {
				size.Width = Math.Min( MaximumSize.Width, size.Width );
				size.Height = Math.Min( MaximumSize.Height, size.Height );
			}
			if ( !MinimumSize.IsEmpty ) {
				size.Width = Math.Max( MinimumSize.Width, size.Width );
				size.Height = Math.Max( MinimumSize.Height, size.Height );
			}
			return size;
		}


		private static TextFormatFlags GetTextFormat( ContentAlignment align, bool autowrap, bool autoellipsis ) {

			TextFormatFlags textformat = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix;

			if ( autowrap )
				textformat |= TextFormatFlags.WordBreak;
			if ( autoellipsis )
				textformat |= TextFormatFlags.WordEllipsis;

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
					textarea.X += imagearea.Width + imageMargin;
					textarea.Width -= imagearea.Width + imageMargin;
					break;

				case ContentAlignment.TopRight:
				case ContentAlignment.MiddleRight:
				case ContentAlignment.BottomRight:
					textarea.Width -= imagearea.Width + imageMargin;
					break;

				case ContentAlignment.TopCenter:
					textarea.Y += imagearea.Height + imageMargin;
					textarea.Height -= imagearea.Height + imageMargin;
					break;
				case ContentAlignment.BottomCenter:
					textarea.Height -= imagearea.Height + imageMargin;
					break;

				case ContentAlignment.MiddleCenter:
					break;		//どうしようもないので背景表示っぽく
			}

			return textarea;
		}


		private void PropertyChanged() {
			if ( AutoSize )
				Size = GetPreferredSize( Size );
			
			//Refresh();
			Invalidate();			//checkme
		}


		private void ImageLabel_SizeChanged( object sender, EventArgs e ) {
			//Refresh();
			Invalidate();
		}


	}
}
