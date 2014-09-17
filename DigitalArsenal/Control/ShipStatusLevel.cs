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

	public partial class ShipStatusLevel : UserControl {

		private const TextFormatFlags TextFormatValue = TextFormatFlags.NoPadding | TextFormatFlags.Bottom | TextFormatFlags.Right;
		private const TextFormatFlags TextFormatText = TextFormatFlags.NoPadding | TextFormatFlags.Bottom | TextFormatFlags.Left;
		//private const TextFormatFlags TextFormatNext = TextFormatFlags.NoPadding | TextFormatFlags.Bottom | TextFormatFlags.Left;
		//private const TextFormatFlags TextFormatNextValue = TextFormatFlags.NoPadding | TextFormatFlags.Bottom | TextFormatFlags.Right;



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


		[Browsable( true )]
		[DefaultValue( "Lv:" )]
		public override string Text {
			get { return base.Text; }
			set {
				base.Text = value;
				Refresh();
			}
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
			Text = "Lv.";

		}




		private void ShipStatusLevel_Paint( object sender, PaintEventArgs e ) {


			Size maxsize = new Size( 99999, 99999 );

#if false
			Size tmainsize = TextRenderer.MeasureText( Math.Max( Value, MaximumValue ).ToString(), MainFont, maxsize, TextFormatMain );
			Size tsubsize = TextRenderer.MeasureText( Text, SubFont, maxsize, TextFormatSub );

			tmainsize.Width += MarginOffset;
			tsubsize.Width += MarginOffset;


			TextRenderer.DrawText( e.Graphics, Text, SubFont, new Rectangle( Padding.Left, Padding.Top, Size.Width - Padding.Horizontal, Size.Height - Padding.Vertical ), SubFontColor, TextFormatSub );
			TextRenderer.DrawText( e.Graphics, Value.ToString(), MainFont, new Rectangle( Padding.Left, Padding.Top, Size.Width - Padding.Horizontal, Size.Height - Padding.Vertical ), MainFontColor, TextFormatMain );

#endif

			Rectangle basearea = new Rectangle( Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical );
			//e.Graphics.DrawRectangle( Pens.Magenta, new Rectangle( basearea.X, basearea.Y, basearea.Width - 1, basearea.Height - 1 ) );

			//Font NextFont = new Font( "Meiryo UI", 9, FontStyle.Regular, GraphicsUnit.Pixel );

			Size sz_value = TextRenderer.MeasureText( Math.Max( Value, MaximumValue ).ToString(), MainFont, maxsize, TextFormatValue );
			Size sz_text = TextRenderer.MeasureText( Text, SubFont, maxsize, TextFormatText );
			//Size sz_next = TextRenderer.MeasureText( "next.", NextFont, maxsize, TextFormatNext );
			//Size sz_nval = TextRenderer.MeasureText( "123456", NextFont, maxsize, TextFormatNextValue );

			sz_value.Width -= (int)( MainFont.Size / 2.0 );
			sz_text.Width -= (int)( SubFont.Size / 2.0 );
			//sz_next.Width -= (int)( SubFont.Size / 2.0 );
			//sz_nval.Width -= (int)( SubFont.Size / 2.0 );

			Point p = new Point( basearea.X, basearea.Y + sz_value.Height - sz_text.Height );
			TextRenderer.DrawText( e.Graphics, Text, SubFont, new Rectangle( p, sz_text ), SubFontColor, TextFormatText );

			p.X = basearea.Right - sz_value.Width - 1;
			p.Y = basearea.Y;
			TextRenderer.DrawText( e.Graphics, Value.ToString(), MainFont, new Rectangle( p, sz_value ), MainFontColor, TextFormatValue );

			/*
			p.X = basearea.X;
			p.Y = basearea.Bottom - sz_next.Height + 1;
			TextRenderer.DrawText( e.Graphics, "next.", NextFont, new Rectangle( p, sz_next ), SubFontColor, TextFormatNext );

			p.X = basearea.Right - sz_nval.Width - 1;
			p.Y = basearea.Bottom - sz_nval.Height + 1;
			TextRenderer.DrawText( e.Graphics, "123456", NextFont, new Rectangle( p, sz_nval ), SubFontColor, TextFormatNextValue );
			*/

			/*/
			e.Graphics.DrawRectangle( Pens.Magenta, new Rectangle( 0, 0, Size.Width - 1, Size.Height - 1 ) );
			e.Graphics.DrawRectangle( Pens.Red, new Rectangle( Padding.Left, Padding.Top, Size.Width - Padding.Horizontal - 1, Size.Height - Padding.Vertical - 1 ) );
			e.Graphics.DrawRectangle( Pens.Orange, new Rectangle( Padding.Left, Size.Height - Padding.Bottom - tsubsize.Height, tsubsize.Width - 1, tsubsize.Height - 1 ) );
			e.Graphics.DrawRectangle( Pens.Orange, new Rectangle( Size.Width - Padding.Right - tmainsize.Width, Size.Height - Padding.Bottom - tmainsize.Height, tmainsize.Width - 1, tmainsize.Height - 1 ) );
			//*/
		}



		public override Size GetPreferredSize( Size proposedSize ) {

			Size maxsize = new Size( 99999, 99999 );

			Size sz_value = TextRenderer.MeasureText( Math.Max( Value, MaximumValue ).ToString(), MainFont, maxsize, TextFormatValue );
			Size sz_text = TextRenderer.MeasureText( Text, SubFont, maxsize, TextFormatText );
			
			sz_value.Width -= (int)( MainFont.Size / 2.0 );
			sz_text.Width -= (int)( SubFont.Size / 2.0 );


			return new Size( sz_value.Width + sz_text.Width + Padding.Horizontal, Math.Max( sz_value.Height, sz_text.Height ) + Padding.Vertical );
			
		}

	}

}
