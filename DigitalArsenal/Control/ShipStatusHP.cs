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
	public partial class ShipStatusHP : UserControl {


		private const TextFormatFlags TextFormatTime = TextFormatFlags.NoPadding | TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
		private const TextFormatFlags TextFormatText = TextFormatFlags.NoPadding | TextFormatFlags.Bottom | TextFormatFlags.Left;
		private const TextFormatFlags TextFormatHP = TextFormatFlags.NoPadding | TextFormatFlags.Bottom | TextFormatFlags.Right;

			

		#region Property

		private int _value;
		[Browsable( true )]
		[DefaultValue( 66 )]
		public int Value {
			get { return _value; }
			set {
				_value = value;
				_HPBar.Value = value;
				Refresh();
			}
		}

		private int _prevValue;
		[Browsable( true )]
		[DefaultValue( 88 )]
		public int PrevValue {
			get { return _prevValue; }
			set {
				_prevValue = value;
				_HPBar.PrevValue = value;
				Refresh();
			}
		}

		private int _maximumValue;
		[Browsable( true )]
		[DefaultValue( 100 )]
		public int MaximumValue {
			get { return _maximumValue; }
			set {
				_maximumValue = value;
				_HPBar.MaximumValue = value;
				Refresh();
			}
		}

		private DateTime? _repairTime;
		[Browsable( true )]
		[DefaultValue( null )]
		public DateTime? RepairTime {
			get { return _repairTime; }
			set {
				_repairTime = value;
				Refresh();
			}
		}



		private int _maximumDigit;
		[Browsable( true )]
		[DefaultValue( 999 )]
		public int MaximumDigit {
			get { return _maximumDigit; }
			set {
				_maximumDigit = value;
				Refresh();
			}
		}


		private Color _mainFontColor;
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "0, 0, 0" )]
		public Color MainFontColor {
			get {
				return _mainFontColor;
			}
			set {
				_mainFontColor = value;
				Refresh();
			}
		}

		private Color _subFontColor;
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "136, 136, 136" )]
		public Color SubFontColor {
			get {
				return _subFontColor;
			}
			set {
				_subFontColor = value;
				Refresh();
			}
		}


		private Font _mainFont;
		[Browsable( true )]
		[DefaultValue( typeof( Font ), "Meiryo UI, 12px" )]
		public Font MainFont {
			get {
				return _mainFont;
			}
			set {
				_mainFont = value;
				Refresh();
			}
		}

		private Font _subFont;
		[Browsable( true )]
		[DefaultValue( typeof( Font ), "Meiryo UI, 10px" )]
		public Font SubFont {
			get {
				return _subFont;
			}
			set {
				_subFont = value;
				Refresh();
			}
		}


		private string _text;
		[Browsable( true )]
		[DefaultValue( "HP:" )]
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


		[Browsable( true )]
		[DefaultValue( true )]
		public bool UsePrevValue {
			get { return HPBar.UsePrevValue; }
			set {
				HPBar.UsePrevValue = value;
				Refresh();
			}
		}

		[Browsable( true )]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Visible )]
		public StatusBar HPBar {
			get { return _HPBar; }
			set { 
				_HPBar = value;
				Refresh();
			}
		}
		
		#endregion

		


		public ShipStatusHP() {
			InitializeComponent();

			SetStyle( ControlStyles.ResizeRedraw, true );


			_value = _HPBar.Value;
			_prevValue = _HPBar.PrevValue;
			_maximumValue = _HPBar.MaximumValue;
			_repairTime = null;
			
			_maximumDigit = 999;

			_mainFont = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			_mainFontColor = FromArgb( 0xFF000000 );
			
			_subFont = new Font( "Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel );
			_subFontColor = FromArgb( 0xFF888888 );
			
			_text = "HP:";
		
		}





		private void ShipStatusHP_Paint( object sender, PaintEventArgs e ) {

			Size maxsize = new Size( 99999, 99999 );


			Graphics g = e.Graphics;
			Rectangle basearea = new Rectangle( Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical );




			if ( RepairTime != null ) {
				string timestr;
				TimeSpan rest = (DateTime)RepairTime - DateTime.Now;

				if ( rest.Ticks < 0 )
					timestr = "00:00:00";
				else
					timestr = string.Format( "{0:D2}:{1:D2}:{2:D2}", (int)rest.TotalHours, rest.Minutes, rest.Seconds );

				Size sz_time = TextRenderer.MeasureText( timestr, MainFont, maxsize, TextFormatTime );

				TextRenderer.DrawText( g, timestr, MainFont, new Rectangle( basearea.X, basearea.Y, basearea.Width, basearea.Height - _HPBar.Height ), MainFontColor, TextFormatTime );

				/*/
				g.DrawRectangle( Pens.Magenta, new Rectangle( basearea.X, basearea.Y, basearea.Width - 1, basearea.Height - BarThickness - BarBackgroundOffset - 1 ) );
				//*/


			} else {

				Size sz_text = TextRenderer.MeasureText( Text, SubFont, maxsize, TextFormatText );
				Size sz_hpmax = TextRenderer.MeasureText( Math.Max( MaximumValue, MaximumDigit ).ToString(), SubFont, maxsize, TextFormatHP );
				Size sz_slash = TextRenderer.MeasureText( " / ", SubFont, maxsize, TextFormatHP );
				Size sz_hpnow = TextRenderer.MeasureText( Math.Max( Value, MaximumDigit ).ToString(), MainFont, maxsize, TextFormatHP );

				sz_text.Width -= (int)( SubFont.Size / 2.0 );
				sz_hpmax.Width -= (int)( SubFont.Size / 2.0 );
				sz_slash.Width -= (int)( SubFont.Size / 2.0 );
				sz_hpnow.Width -= (int)( MainFont.Size / 2.0 );



				Point p = new Point( basearea.X, basearea.Bottom - _HPBar.Height - sz_text.Height + 1 );	
				TextRenderer.DrawText( g, Text, SubFont, new Rectangle( p, sz_text ), SubFontColor, TextFormatText );
				//g.DrawRectangle( Pens.Orange, new Rectangle( p, sz_text ) );

				p.X = basearea.Right - sz_hpmax.Width;
				TextRenderer.DrawText( g, MaximumValue.ToString(), SubFont, new Rectangle( p, sz_hpmax ), SubFontColor, TextFormatHP );
				//g.DrawRectangle( Pens.Orange, new Rectangle( p, sz_hpmax ) );

				p.X -= sz_slash.Width;
				TextRenderer.DrawText( g, " / ", SubFont, new Rectangle( p, sz_slash ), SubFontColor, TextFormatHP );
				//g.DrawRectangle( Pens.Orange, new Rectangle( p, sz_slash ) );

				p.X -= sz_hpnow.Width;
				p.Y = basearea.Bottom - _HPBar.Height - sz_hpnow.Height + 1;
				TextRenderer.DrawText( g, Value.ToString(), MainFont, new Rectangle( p, sz_hpnow ), MainFontColor, TextFormatHP );
				//g.DrawRectangle( Pens.Orange, new Rectangle( p, sz_hpnow ) );

			}

			_HPBar.Refresh();
		}



		public override Size GetPreferredSize( Size proposedSize ) {

			Size maxsize = new Size( 99999, 99999 );

			Size sz_text = TextRenderer.MeasureText( Text, SubFont, maxsize, TextFormatText );
			Size sz_hpmax = TextRenderer.MeasureText( Math.Max( MaximumValue, MaximumDigit ).ToString(), SubFont, maxsize, TextFormatHP );
			Size sz_slash = TextRenderer.MeasureText( " / ", SubFont, maxsize, TextFormatHP );
			Size sz_hpnow = TextRenderer.MeasureText( Math.Max( Value, MaximumDigit ).ToString(), MainFont, maxsize, TextFormatHP );

			sz_text.Width -= (int)( SubFont.Size / 2.0 );
			sz_hpmax.Width -= (int)( SubFont.Size / 2.0 );
			sz_slash.Width -= (int)( SubFont.Size / 2.0 );
			sz_hpnow.Width -= (int)( MainFont.Size / 2.0 );


			//checkme
			return new Size( sz_text.Width + sz_hpnow.Width + sz_slash.Width + sz_hpmax.Width + Padding.Horizontal,
				Math.Max( sz_text.Height, sz_hpnow.Height ) + _HPBar.Height );
		}



		private Color FromArgb( uint color ) {
			return Color.FromArgb( unchecked( (int)color ) );
		}


	
	}
}
