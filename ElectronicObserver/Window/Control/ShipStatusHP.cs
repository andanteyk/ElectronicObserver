using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ElectronicObserver.Utility.Mathematics;

namespace ElectronicObserver.Window.Control {
	public partial class ShipStatusHP : UserControl {


		private const TextFormatFlags TextFormatTime = TextFormatFlags.NoPadding | TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
		private const TextFormatFlags TextFormatText = TextFormatFlags.NoPadding | TextFormatFlags.Bottom | TextFormatFlags.Left;
		private const TextFormatFlags TextFormatHP = TextFormatFlags.NoPadding | TextFormatFlags.Bottom | TextFormatFlags.Right;

		private StatusBarModule _HPBar;
			

		#region Property

		[Browsable( true )]
		[Description( "HPの現在値です。" )]
		[Category( "データ" )]
		[DefaultValue( 66 )]
		public int Value {
			get { return _HPBar.Value; }
			set {
				_HPBar.Value = value;
				Refresh();
			}
		}

		[Browsable( true )]
		[Description( "以前のHPです。" )]
		[Category( "データ" )]
		[DefaultValue( 88 )]
		public int PrevValue {
			get { return _HPBar.PrevValue; }
			set {
				_HPBar.PrevValue = value;
				Refresh();
			}
		}

		[Browsable( true )]
		[Description( "HPの最大値です。" )]
		[Category( "データ" )]
		[DefaultValue( 100 )]
		public int MaximumValue {
			get { return _HPBar.MaximumValue; }
			set {
				_HPBar.MaximumValue = value;
				Refresh();
			}
		}

		private DateTime? _repairTime;
		[Browsable( true )]
		[Description( "修復が完了する日時です。" )]
		[Category( "データ" )]
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
		[Description( "想定されるHPの最大値です。この値に応じてレイアウトされます。" )]
		[Category( "データ" )]
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
		[Description( "主要テキストの色を指定します。" )]
		[Category( "表示" )]
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
		[Description( "補助テキストの色を指定します。" )]
		[Category( "表示" )]
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

		private Color _repairFontColor;
		[Browsable( true )]
		[Description( "修復時間テキストの色を指定します。" )]
		[Category( "表示" )]
		[DefaultValue( typeof( Color ), "0, 0, 136" )]
		public Color RepairFontColor {
			get {
				return _repairFontColor;
			}
			set {
				_repairFontColor = value;
				Refresh();
			}
		}


		private Font _mainFont;
		[Browsable( true )]
		[Description( "主要テキストのフォントを指定します。" )]
		[Category( "表示" )]
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
		[Description( "補助テキストの色を指定します。" )]
		[Category( "表示" )]
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
		[Description( "説明文となるテキストを指定します。" )]
		[Category( "表示" )]
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
		[Description( "以前の値との差分を表示するかを指定します。" )]
		[Category( "表示" )]
		[DefaultValue( true )]
		public bool UsePrevValue {
			get { return HPBar.UsePrevValue; }
			set {
				HPBar.UsePrevValue = value;
				Refresh();
			}
		}


		private bool _showDifference;
		[Browsable( true )]
		[Description( "最大値の代わりに以前の値を表示するかを指定します。" )]
		[Category( "表示" )]
		[DefaultValue( false )]
		public bool ShowDifference {
			get { return _showDifference; }
			set {
				_showDifference = value;
				Refresh();
			}
		}


		[Browsable( true )]
		[Description( "HPバーへの参照です。" )]
		[Category( "表示" )]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Visible )]
		public StatusBarModule HPBar {
			get { return _HPBar; }
			set { _HPBar = value; }
		}
		



		#endregion

		


		public ShipStatusHP() {
			InitializeComponent();

			SetStyle( ControlStyles.ResizeRedraw, true );

			_HPBar = new StatusBarModule();
			_HPBar.Value = 66;
			_HPBar.PrevValue = 88;
			_HPBar.MaximumValue = 100;
			_repairTime = null;
			
			_maximumDigit = 999;

			_mainFont = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			_mainFontColor = FromArgb( 0xFF000000 );
			
			_subFont = new Font( "Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel );
			_subFontColor = FromArgb( 0xFF888888 );

			_repairFontColor = FromArgb( 0xFF000088 );
			_text = "HP:";

			_HPBar.UsePrevValue = true;
			_showDifference = false;
		}





		private void ShipStatusHP_Paint( object sender, PaintEventArgs e ) {

			Size maxsize = new Size( 99999, 99999 );


			Graphics g = e.Graphics;
			Rectangle basearea = new Rectangle( Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical );
			Size barSize = _HPBar.GetPreferredSize( new Size( basearea.Width, 0 ) );



			if ( RepairTime != null ) {
				string timestr = DateTimeHelper.ToTimeRemainString( (DateTime)RepairTime );
				
				Size sz_time = TextRenderer.MeasureText( timestr, MainFont, maxsize, TextFormatTime );

				TextRenderer.DrawText( g, timestr, MainFont, new Rectangle( basearea.X, basearea.Y, basearea.Width, basearea.Height - barSize.Height ), RepairFontColor, TextFormatTime );

				/*/
				g.DrawRectangle( Pens.Magenta, new Rectangle( basearea.X, basearea.Y, basearea.Width - 1, basearea.Height - BarThickness - BarBackgroundOffset - 1 ) );
				//*/


			} else {

				Size sz_text = TextRenderer.MeasureText( Text, SubFont, maxsize, TextFormatText );
				Size sz_hpmax = TextRenderer.MeasureText( Math.Max( MaximumValue, MaximumDigit ).ToString(), SubFont, maxsize, TextFormatHP );
				Size sz_slash = TextRenderer.MeasureText( " / ", SubFont, maxsize, TextFormatHP );
				Size sz_hpnow = TextRenderer.MeasureText( Math.Max( Value, MaximumDigit ).ToString(), MainFont, maxsize, TextFormatHP );

				if ( Text.Length > 0 )
					sz_text.Width -= (int)( SubFont.Size / 2.0 );
				sz_hpmax.Width -= (int)( SubFont.Size / 2.0 );
				sz_slash.Width -= (int)( SubFont.Size / 2.0 );
				sz_hpnow.Width -= (int)( MainFont.Size / 2.0 );



				Point p = new Point( basearea.X, basearea.Bottom - barSize.Height - Math.Max( sz_text.Height, sz_hpmax.Height ) + 1 );
				TextRenderer.DrawText( g, Text, SubFont, new Rectangle( p, sz_text ), SubFontColor, TextFormatText );
				//g.DrawRectangle( Pens.Orange, new Rectangle( p, sz_text ) );

				p.X = basearea.Right - sz_hpmax.Width;
				TextRenderer.DrawText( g, !ShowDifference ? MaximumValue.ToString() : GetDifferenceString(), SubFont, new Rectangle( p, sz_hpmax ), SubFontColor, TextFormatHP );
				//g.DrawRectangle( Pens.Orange, new Rectangle( p, sz_hpmax ) );

				p.X -= sz_slash.Width;
				TextRenderer.DrawText( g, " / ", SubFont, new Rectangle( p, sz_slash ), SubFontColor, TextFormatHP );
				//g.DrawRectangle( Pens.Orange, new Rectangle( p, sz_slash ) );

				p.X -= sz_hpnow.Width;
				p.Y = basearea.Bottom - barSize.Height - sz_hpnow.Height + 1;
				TextRenderer.DrawText( g, Math.Max( Value, 0 ).ToString(), MainFont, new Rectangle( p, sz_hpnow ), MainFontColor, TextFormatHP );
				//g.DrawRectangle( Pens.Orange, new Rectangle( p, sz_hpnow ) );

			}

			_HPBar.Paint( g, new Rectangle( basearea.X, basearea.Bottom - barSize.Height, barSize.Width, barSize.Height ) );
		}



		public override Size GetPreferredSize( Size proposedSize ) {

			Size maxsize = new Size( int.MaxValue, int.MaxValue );

			Size barSize = _HPBar.GetPreferredSize();

			Size sz_text = TextRenderer.MeasureText( Text, SubFont, maxsize, TextFormatText );
			Size sz_hpmax = TextRenderer.MeasureText( Math.Max( MaximumValue, MaximumDigit ).ToString(), SubFont, maxsize, TextFormatHP );
			Size sz_slash = TextRenderer.MeasureText( " / ", SubFont, maxsize, TextFormatHP );
			Size sz_hpnow = TextRenderer.MeasureText( Math.Max( Value, MaximumDigit ).ToString(), MainFont, maxsize, TextFormatHP );

			if ( Text.Length > 0 )
				sz_text.Width -= (int)( SubFont.Size / 2.0 );
			sz_hpmax.Width -= (int)( SubFont.Size / 2.0 );
			sz_slash.Width -= (int)( SubFont.Size / 2.0 );
			sz_hpnow.Width -= (int)( MainFont.Size / 2.0 );

			return new Size( sz_text.Width + sz_hpnow.Width + sz_slash.Width + sz_hpmax.Width + Padding.Horizontal,
				Math.Max( sz_text.Height, sz_hpnow.Height ) + barSize.Height + Padding.Vertical );
		}



		private Color FromArgb( uint color ) {
			return Color.FromArgb( unchecked( (int)color ) );
		}



		private string GetDifferenceString() {

			int diff = Value - PrevValue;

			if ( diff < 0 )
				return diff.ToString();
			else if ( diff == 0 )
				return "-0";
			else
				return "+" + diff.ToString();
		}

	}
}
