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
		[Browsable( true )]
		[DefaultValue( 0 )]
		public int Value {
			get { return _value; }
			set {
				_value = value;
				Refresh();
			}
		}

		private int _maximumValue;
		[Browsable( true )]
		[DefaultValue( 0 )]
		public int MaximumValue {
			get { return _maximumValue; }
			set {
				_maximumValue = value;
				Refresh();
			}
		}

		private int _valueNext;
		[Browsable( true )]
		[DefaultValue( 0 )]
		public int ValueNext {
			get { return _valueNext; }
			set {
				_valueNext = value;
				Refresh();
			}
		}

		private Color _mainFontColor;
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "0, 0, 0" )]
		public Color MainFontColor {
			get { return _mainFontColor; }
			set {
				_mainFontColor = value;
				Refresh();
			}
		}

		private Color _subFontColor;
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "68, 68, 68" )]
		public Color SubFontColor {
			get { return _subFontColor; }
			set {
				_subFontColor = value;
				Refresh();
			}
		}

		private Font _mainFont;
		[Browsable( true )]
		[DefaultValue( typeof( Font ), "Meiryo UI, 12px" )]
		public Font MainFont {
			get { return _mainFont; }
			set {
				_mainFont = value;
				Refresh();
			}
		}

		private Font _subFont;
		[Browsable( true )]
		[DefaultValue( typeof( Font ), "Meiryo UI, 10px" )]
		public Font SubFont {
			get { return _subFont; }
			set {
				_subFont = value;
				Refresh();
			}
		}


		private string _text;
		[Browsable( true )]
		[DefaultValue( "Lv." )]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Visible )]
		[EditorBrowsable( EditorBrowsableState.Always )]
		[Bindable( BindableSupport.Default ) ]
		public override string Text {
			get { return _text; }
			set {
				_text = value;
				Refresh();
			}
		}

		private string _textNext;
		[Browsable( true )]
		[DefaultValue( "next:" )]
		public string TextNext {
			get { return _textNext; }
			set {
				_textNext = value;
				Refresh();
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
			_mainFontColor = Color.FromArgb( 0x00, 0x00, 0x00 );
			_subFontColor = Color.FromArgb( 0x44, 0x44, 0x44 );
			_mainFont = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			_subFont = new Font( "Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel );
			_text = "Lv.";

			_valueNext = 0;
			_textNext = "next:";
		}




		private void ShipStatusLevel_Paint( object sender, PaintEventArgs e ) {


			Size maxsize = new Size( int.MaxValue, int.MaxValue );

			Rectangle basearea = new Rectangle( Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical );
			//e.Graphics.DrawRectangle( Pens.Magenta, new Rectangle( basearea.X, basearea.Y, basearea.Width - 1, basearea.Height - 1 ) );

			Size sz_value = TextRenderer.MeasureText( Math.Max( Value, MaximumValue ).ToString(), MainFont, maxsize, TextFormatValue );
			Size sz_text = TextRenderer.MeasureText( Text, SubFont, maxsize, TextFormatText );
			Size sz_textnext = TextRenderer.MeasureText( TextNext, SubFont, maxsize, TextFormatText );
			Size sz_valuenext = TextRenderer.MeasureText( ValueNext.ToString(), SubFont, maxsize, TextFormatText );


			sz_value.Width -= (int)( MainFont.Size / 2.0 );
			sz_text.Width -= (int)( SubFont.Size / 2.0 );
			sz_textnext.Width -= (int)( SubFont.Size / 2.0 );
			sz_valuenext.Width -= (int)( SubFont.Size / 2.0 );

			//*/
			
			//alignment.bottom 
			
			Point p = new Point( basearea.X, basearea.Bottom - sz_text.Height );
			TextRenderer.DrawText( e.Graphics, Text, SubFont, new Rectangle( p, sz_text ), SubFontColor, TextFormatText );
			//e.Graphics.DrawRectangle( Pens.Orange, new Rectangle( p, sz_text ) );

			p.X = basearea.Right - Math.Max( sz_textnext.Width, sz_valuenext.Width );
			p.Y = basearea.Bottom - sz_textnext.Height - sz_valuenext.Height + (int)( SubFont.Size / 2.0 );
			TextRenderer.DrawText( e.Graphics, TextNext, SubFont, new Rectangle( p, sz_textnext ), SubFontColor, TextFormatText );
			//e.Graphics.DrawRectangle( Pens.Orange, new Rectangle( p, sz_textnext ) );
			
	
			p.Y = basearea.Bottom - sz_valuenext.Height + 1;
			TextRenderer.DrawText( e.Graphics, ValueNext.ToString(), SubFont, new Rectangle( p, sz_valuenext ), SubFontColor, TextFormatText );
			//e.Graphics.DrawRectangle( Pens.Orange, new Rectangle( p, sz_valuenext ) );

			p.X -= sz_value.Width + InnerHorizontalMargin;
			p.Y = basearea.Bottom - sz_value.Height;
			TextRenderer.DrawText( e.Graphics, Value.ToString(), MainFont, new Rectangle( p, sz_value ), MainFontColor, TextFormatValue );
			//e.Graphics.DrawRectangle( Pens.Orange, new Rectangle( p, sz_value ) );

			
			/*/

			//alignment.top

			Point p = new Point( basearea.X, basearea.Y + sz_value.Height - sz_text.Height );
			TextRenderer.DrawText( e.Graphics, Text, SubFont, new Rectangle( p, sz_text ), SubFontColor, TextFormatText );
			//e.Graphics.DrawRectangle( Pens.Orange, new Rectangle( p, sz_text ) );

			p.X = basearea.Right - Math.Max( sz_textnext.Width, sz_valuenext.Width );
			p.Y = basearea.Bottom - sz_textnext.Height - sz_valuenext.Height + (int)( SubFont.Size / 2.0 );
			TextRenderer.DrawText( e.Graphics, TextNext, SubFont, new Rectangle( p, sz_textnext ), SubFontColor, TextFormatText );
			//e.Graphics.DrawRectangle( Pens.Orange, new Rectangle( p, sz_textnext ) );
		

			p.Y = basearea.Bottom - sz_valuenext.Height + 1;
			TextRenderer.DrawText( e.Graphics, ValueNext.ToString(), SubFont, new Rectangle( p, sz_valuenext ), SubFontColor, TextFormatText );
			//e.Graphics.DrawRectangle( Pens.Orange, new Rectangle( p, sz_valuenext ) );

			p.X -= sz_value.Width + InnerHorizontalMargin;
			p.Y = basearea.Y;
			TextRenderer.DrawText( e.Graphics, Value.ToString(), MainFont, new Rectangle( p, sz_value ), MainFontColor, TextFormatValue );
			//e.Graphics.DrawRectangle( Pens.Orange, new Rectangle( p, sz_value ) );

			//*/
			
		}



		public override Size GetPreferredSize( Size proposedSize ) {

			Size maxsize = new Size( 99999, 99999 );

			Size sz_value = TextRenderer.MeasureText( Math.Max( Value, MaximumValue ).ToString(), MainFont, maxsize, TextFormatValue );
			Size sz_text = TextRenderer.MeasureText( Text, SubFont, maxsize, TextFormatText );
			Size sz_textnext = TextRenderer.MeasureText( TextNext, SubFont, maxsize, TextFormatText );
			Size sz_valuenext = TextRenderer.MeasureText( ValueNext.ToString(), SubFont, maxsize, TextFormatText );


			sz_value.Width -= (int)( MainFont.Size / 2.0 );
			sz_text.Width -= (int)( SubFont.Size / 2.0 );
			sz_textnext.Width -= (int)( SubFont.Size / 2.0 );
			sz_valuenext.Width -= (int)( SubFont.Size / 2.0 );


			return new Size( sz_value.Width + sz_text.Width + Math.Max( sz_textnext.Width, sz_valuenext.Width ) + InnerHorizontalMargin + Padding.Horizontal,
				Math.Max( sz_value.Height, sz_textnext.Height + sz_valuenext.Height - (int)( SubFont.Size / 2.0 ) ) + Padding.Vertical - 1 );
			
		}

	}

}
