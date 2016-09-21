using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Control {

	public partial class ShipStatusLevel : UserControl {

		private const TextFormatFlags TextFormatValue = TextFormatFlags.NoPadding | TextFormatFlags.Bottom | TextFormatFlags.Right;
		private const TextFormatFlags TextFormatText = TextFormatFlags.NoPadding | TextFormatFlags.Bottom | TextFormatFlags.Left;



		#region Property

		private int _value;
		[Browsable( true ), Category( "Data" ), DefaultValue( 0 )]
		[Description( "現在のレベルです。" )]
		public int Value {
			get { return _value; }
			set {
				_value = value;
				_valueSizeCache = null;
				PropertyChanged();
			}
		}

		private int _maximumValue;
		[Browsable( true ), Category( "Data" ), DefaultValue( 0 )]
		[Description( "レベルの最大値です。" )]
		public int MaximumValue {
			get { return _maximumValue; }
			set {
				_maximumValue = value;
				_valueSizeCache = null;
				PropertyChanged();
			}
		}

		private int _valueNext;
		[Browsable( true ), Category( "Data" ), DefaultValue( 0 )]
		[Description( "次のレベルアップに必要な経験値です。" )]
		public int ValueNext {
			get { return _valueNext; }
			set {
				_valueNext = value;
				_valueNextSizeCache = null;
				PropertyChanged();
			}
		}

		private int _maximumValueNext;
		[Browsable( true ), Category( "Data" ), DefaultValue( 0 )]
		[Description( "次のレベルアップに必要な経験値の最大値です。" )]
		public int MaximumValueNext {
			get { return _maximumValueNext; }
			set {
				_maximumValueNext = value;
				_valueNextSizeCache = null;
				PropertyChanged();
			}
		}

		private Color _mainFontColor;
		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Color ), "0, 0, 0" )]
		[Description( "主要テキストの色を指定します。" )]
		public Color MainFontColor {
			get { return _mainFontColor; }
			set {
				_mainFontColor = value;
				PropertyChanged();
			}
		}

		private Color _subFontColor;
		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Color ), "68, 68, 68" )]
		[Description( "補助テキストの色を指定します。" )]
		public Color SubFontColor {
			get { return _subFontColor; }
			set {
				_subFontColor = value;
				PropertyChanged();
			}
		}

		private Font _mainFont;
		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Font ), "Meiryo UI, 12px" )]
		[Description( "主要テキストのフォントを指定します。" )]
		public Font MainFont {
			get { return _mainFont; }
			set {
				_mainFont = value;
				_valueSizeCache = null;
				PropertyChanged();
			}
		}

		private Font _subFont;
		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Font ), "Meiryo UI, 10px" )]
		[Description( "補助テキストの色を指定します。" )]
		public Font SubFont {
			get { return _subFont; }
			set {
				_subFont = value;
				_textSizeCache =
				_valueNextSizeCache =
				_textNextSizeCache = null;
				PropertyChanged();
			}
		}


		private string _text;
		[Browsable( true ), Category( "Appearance" ), DefaultValue( "Lv." )]
		[Description( "レベルの説明文となるテキストを指定します。" )]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Visible )]
		[EditorBrowsable( EditorBrowsableState.Always )]
		[Bindable( BindableSupport.Default )]
		public override string Text {
			get { return _text; }
			set {
				_text = value;
				_textSizeCache = null;
				PropertyChanged();
			}
		}

		private string _textNext;
		[Browsable( true ), Category( "Appearance" ), DefaultValue( "next:" )]
		[Description( "次のレベルアップに必要な経験値の説明文となるテキストを指定します。" )]
		public string TextNext {
			get { return _textNext; }
			set {
				_textNext = value;
				_textNextSizeCache = null;
				_valueNextSizeCache = null;
				PropertyChanged();
			}
		}


		// size cache

		private static readonly Size MaxSize = new Size( int.MaxValue, int.MaxValue );

		private Size? _textSizeCache;
		private Size TextSizeCache {
			get {
				return _textSizeCache ??
					( _textSizeCache = TextRenderer.MeasureText( Text, SubFont, MaxSize, TextFormatText ) - new Size( !string.IsNullOrEmpty( Text ) ? (int)( SubFont.Size / 2.0 ) : 0, 0 ) ).Value;
			}
		}

		private Size? _valueSizeCache;
		private Size ValueSizeCache {
			get {
				return _valueSizeCache ??
					( _valueSizeCache = TextRenderer.MeasureText( Math.Max( Value, MaximumValue ).ToString(), MainFont, MaxSize, TextFormatValue ) - new Size( (int)( MainFont.Size / 2.0 ), 0 ) ).Value;
			}
		}

		private Size? _textNextSizeCache;
		private Size TextNextSizeCache {
			get {
				return _textNextSizeCache ??
					( _textNextSizeCache = TextNext == null ?
						Size.Empty :
						( TextRenderer.MeasureText( TextNext, SubFont, MaxSize, TextFormatText ) - new Size( !string.IsNullOrEmpty( TextNext ) ? (int)( SubFont.Size / 2.0 ) : 0, 0 ) ) ).Value;
			}
		}

		private Size? _valueNextSizeCache;
		private Size ValueNextSizeCache {
			get {
				return _valueNextSizeCache ??
					( _valueNextSizeCache = TextNext == null ?
						Size.Empty :
						( TextRenderer.MeasureText( Math.Max( ValueNext, MaximumValueNext ).ToString(), SubFont, MaxSize, TextFormatText ) - new Size( (int)( MainFont.Size / 2.0 ), 0 ) ) ).Value;
			}
		}


		private int InnerHorizontalMargin {
			get { return 4; }
		}
		#endregion




		public ShipStatusLevel() {
			InitializeComponent();

			SetStyle( ControlStyles.ResizeRedraw, true );

			_value = 0;
			_maximumValue = 0;

			_mainFontColor = Color.FromArgb( 0x00, 0x00, 0x00 );
			_subFontColor = Color.FromArgb( 0x44, 0x44, 0x44 );
			_mainFont = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			_subFont = new Font( "Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel );
			_text = "Lv.";

			_valueNext = 0;
			_maximumValueNext = 0;
			_textNext = "next:";
		}




		private void ShipStatusLevel_Paint( object sender, PaintEventArgs e ) {

			Rectangle basearea = new Rectangle( Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical );
			//e.Graphics.DrawRectangle( Pens.Magenta, new Rectangle( basearea.X, basearea.Y, basearea.Width - 1, basearea.Height - 1 ) );


			//*/

			//alignment.bottom 

			Point p = new Point( basearea.X, basearea.Bottom - TextSizeCache.Height );
			TextRenderer.DrawText( e.Graphics, Text, SubFont, new Rectangle( p, TextSizeCache ), SubFontColor, TextFormatText );
			//e.Graphics.DrawRectangle( Pens.Orange, new Rectangle( p, TextSizeCache ) );

			p.X += TextSizeCache.Width;
			p.Y = basearea.Bottom - ValueSizeCache.Height;
			TextRenderer.DrawText( e.Graphics, Value.ToString(), MainFont, new Rectangle( p, ValueSizeCache ), MainFontColor, TextFormatValue );
			//e.Graphics.DrawRectangle( Pens.Orange, new Rectangle( p, ValueSizeCache ) );


			p.X = basearea.Right - Math.Max( TextNextSizeCache.Width, ValueNextSizeCache.Width );
			p.Y = basearea.Bottom - TextNextSizeCache.Height - ValueNextSizeCache.Height + (int)( SubFont.Size / 2.0 );
			if ( TextNext != null ) {
				TextRenderer.DrawText( e.Graphics, TextNext, SubFont, new Rectangle( p, TextNextSizeCache ), SubFontColor, TextFormatText );
				//e.Graphics.DrawRectangle( Pens.Orange, new Rectangle( p, TextNextSizeCache ) );
			}

			p.Y = basearea.Bottom - ValueNextSizeCache.Height + 1;
			if ( TextNext != null ) {
				TextRenderer.DrawText( e.Graphics, ValueNext.ToString(), SubFont, new Rectangle( p, ValueNextSizeCache ), SubFontColor, TextFormatText );
				//e.Graphics.DrawRectangle( Pens.Orange, new Rectangle( p, ValueNextSizeCache ) );
			}

		}



		public override Size GetPreferredSize( Size proposedSize ) {

			return new Size( ValueSizeCache.Width + TextSizeCache.Width + ( TextNext == null ? 0 : InnerHorizontalMargin ) + Math.Max( TextNextSizeCache.Width, ValueNextSizeCache.Width ) + Padding.Horizontal,
				Math.Max( ValueSizeCache.Height, TextNextSizeCache.Height + ValueNextSizeCache.Height - (int)( SubFont.Size / 2.0 ) ) + Padding.Vertical - 1 );

		}


		private void PropertyChanged() {
			if ( AutoSize )
				Size = GetPreferredSize( Size );

			Refresh();
		}

	}

}
