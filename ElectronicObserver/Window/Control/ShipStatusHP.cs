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

		private static readonly Size MaxSize = new Size( int.MaxValue, int.MaxValue );
		private const string SlashText = " / ";


		private StatusBarModule _HPBar;

		private bool _onMouse;

		#region Property

		[Browsable( true ), Category( "Data" ), DefaultValue( 66 )]
		[Description( "HPの現在値です。" )]
		public int Value {
			get { return _HPBar.Value; }
			set {
				_HPBar.Value = value;
				_valueSizeCache = _preferredSizeCache = null;
				PropertyChanged();
			}
		}

		[Browsable( true ), Category( "Data" ), DefaultValue( 88 )]
		[Description( "以前のHPです。" )]
		public int PrevValue {
			get { return _HPBar.PrevValue; }
			set {
				_HPBar.PrevValue = value;
				PropertyChanged();
			}
		}

		[Browsable( true ), Category( "Data" ), DefaultValue( 100 )]
		[Description( "HPの最大値です。" )]
		public int MaximumValue {
			get { return _HPBar.MaximumValue; }
			set {
				_HPBar.MaximumValue = value;
				_maximumValueSizeCache = _preferredSizeCache = null;
				PropertyChanged();
			}
		}

		private DateTime _repairTime;
		[Browsable( true ), Category( "Data" )]
		[Description( "修復が完了する日時です。" )]
		public DateTime RepairTime {
			get { return _repairTime; }
			set {
				_repairTime = value;
				PropertyChanged();
			}
		}

		private ShipStatusHPRepairTimeShowMode _repairTimeShowMode;
		[Browsable( true ), Category( "Data" ), DefaultValue( null )]
		[Description( "修復完了日時の表示方法を指定します。。" )]
		public ShipStatusHPRepairTimeShowMode RepairTimeShowMode {
			get { return _repairTimeShowMode; }
			set {
				_repairTimeShowMode = value;
				PropertyChanged();
			}
		}


		private int _maximumDigit;
		[Browsable( true ), Category( "Data" ), DefaultValue( 999 )]
		[Description( "想定されるHPの最大値です。この値に応じてレイアウトされます。" )]
		public int MaximumDigit {
			get { return _maximumDigit; }
			set {
				_maximumDigit = value;
				_valueSizeCache =
				_maximumValueSizeCache =
				_preferredSizeCache = null;
				PropertyChanged();
			}
		}


		private Color _mainFontColor;
		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Color ), "0, 0, 0" )]
		[Description( "主要テキストの色を指定します。" )]
		public Color MainFontColor {
			get {
				return _mainFontColor;
			}
			set {
				_mainFontColor = value;
				PropertyChanged();
			}
		}

		private Color _subFontColor;
		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Color ), "136, 136, 136" )]
		[Description( "補助テキストの色を指定します。" )]
		public Color SubFontColor {
			get {
				return _subFontColor;
			}
			set {
				_subFontColor = value;
				PropertyChanged();
			}
		}

		private Color _repairFontColor;
		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Color ), "0, 0, 136" )]
		[Description( "修復時間テキストの色を指定します。" )]
		public Color RepairFontColor {
			get {
				return _repairFontColor;
			}
			set {
				_repairFontColor = value;
				PropertyChanged();
			}
		}


		private Font _mainFont;
		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Font ), "Meiryo UI, 12px" )]
		[Description( "主要テキストのフォントを指定します。" )]
		public Font MainFont {
			get {
				return _mainFont;
			}
			set {
				_mainFont = value;
				_valueSizeCache =
				_repairTimeSizeCache =
				_preferredSizeCache = null;
				PropertyChanged();
			}
		}

		private Font _subFont;
		[Browsable( true ), Category( "Appearance" ), DefaultValue( typeof( Font ), "Meiryo UI, 10px" )]
		[Description( "補助テキストのフォントを指定します。" )]
		public Font SubFont {
			get {
				return _subFont;
			}
			set {
				_subFont = value;
				_textSizeCache =
				_maximumValueSizeCache =
				_slashSizeCache =
				_preferredSizeCache = null;
				PropertyChanged();
			}
		}


		private string _text;
		[Browsable( true ), Category( "Appearance" ), DefaultValue( "HP:" )]
		[Description( "説明文となるテキストを指定します。" )]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Visible )]
		[EditorBrowsable( EditorBrowsableState.Always )]
		[Bindable( BindableSupport.Default )]
		public override string Text {
			get { return _text; }
			set {
				_text = value;
				_textSizeCache = _preferredSizeCache = null;
				PropertyChanged();
			}
		}


		[Browsable( true ), Category( "Appearance" ), DefaultValue( true )]
		[Description( "以前の値との差分を表示するかを指定します。" )]
		public bool UsePrevValue {
			get { return HPBar.UsePrevValue; }
			set {
				HPBar.UsePrevValue = value;
				PropertyChanged();
			}
		}


		private bool _showDifference;
		[Browsable( true ), Category( "Appearance" ), DefaultValue( false )]
		[Description( "最大値の代わりに以前の値を表示するかを指定します。" )]
		public bool ShowDifference {
			get { return _showDifference; }
			set {
				_showDifference = value;
				PropertyChanged();
			}
		}

		private bool _showHPBar;
		[Browsable( true ), Category( "Appearance" ), DefaultValue( true )]
		[Description( "HPバーを表示するかを指定します。" )]
		public bool ShowHPBar {
			get { return _showHPBar; }
			set {
				_showHPBar = value;
				_preferredSizeCache = null;
				PropertyChanged();
			}
		}


		[Browsable( true ), Category( "Appearance" )]
		[Description( "HPバーへの参照です。" )]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Visible )]
		public StatusBarModule HPBar {
			get { return _HPBar; }
			set { _HPBar = value; }
		}


		private bool IsRefreshSuspended { get; set; }

		public void SuspendUpdate() {
			IsRefreshSuspended = true;
		}
		public void ResumeUpdate() {
			IsRefreshSuspended = false;
			PropertyChanged();
		}


		// size cache

		private Size? _textSizeCache;
		private Size TextSizeCache {
			get {
				if ( _textSizeCache == null ) {
					_textSizeCache = TextRenderer.MeasureText( Text, SubFont, MaxSize, TextFormatText ) - new Size( !string.IsNullOrEmpty( Text ) ? (int)( SubFont.Size / 2.0 ) : 0, 0 );
					_preferredSizeCache = null;
				}
				return _textSizeCache.Value;
			}
		}

		private Size? _valueSizeCache;
		private Size ValueSizeCache {
			get {
				if ( _valueSizeCache == null ) {
					_valueSizeCache = TextRenderer.MeasureText( Math.Max( Value, MaximumDigit ).ToString(), MainFont, MaxSize, TextFormatHP ) - new Size( (int)( MainFont.Size / 2.0 ), 0 );
					_preferredSizeCache = null;
				}
				return _valueSizeCache.Value;
			}
		}

		private Size? _maximumValueSizeCache;
		private Size MaximumValueSizeCache {
			get {
				if ( _maximumValueSizeCache == null ) {
					_maximumValueSizeCache = TextRenderer.MeasureText( Math.Max( MaximumValue, MaximumDigit ).ToString(), SubFont, MaxSize, TextFormatHP ) - new Size( (int)( SubFont.Size / 2.0 ), 0 );
					_preferredSizeCache = null;
				}
				return _maximumValueSizeCache.Value;
			}
		}

		private Size? _slashSizeCache;
		private Size SlashSizeCache {
			get {
				if ( _slashSizeCache == null ) {
					_slashSizeCache = TextRenderer.MeasureText( SlashText, SubFont, MaxSize, TextFormatHP ) - new Size( (int)( SubFont.Size / 2.0 ), 0 );
					_preferredSizeCache = null;
				}
				return _slashSizeCache.Value;
			}
		}

		private Size? _repairTimeSizeCache;
		private Size RepairTimeSizeCache {
			get {
				if ( _repairTimeSizeCache == null ) {
					_repairTimeSizeCache = TextRenderer.MeasureText( DateTimeHelper.ToTimeRemainString( TimeSpan.Zero ), MainFont, MaxSize, TextFormatTime ) - new Size( (int)( MainFont.Size / 2.0 ), 0 );
				}
				return _repairTimeSizeCache.Value;
			}
		}

		private Size? _preferredSizeCache;

		#endregion




		public ShipStatusHP() {
			InitializeComponent();

			SetStyle( ControlStyles.ResizeRedraw, true );

			_HPBar = new StatusBarModule();
			_HPBar.Value = 66;
			_HPBar.PrevValue = 88;
			_HPBar.MaximumValue = 100;
			_repairTime = DateTime.Now;

			_maximumDigit = 999;

			_mainFont = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			_mainFontColor = FromArgb( 0xFF000000 );

			_subFont = new Font( "Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel );
			_subFontColor = FromArgb( 0xFF888888 );

			_repairFontColor = FromArgb( 0xFF000088 );
			_text = "HP:";

			_HPBar.UsePrevValue = true;
			_showDifference = false;
			_repairTimeShowMode = ShipStatusHPRepairTimeShowMode.Invisible;
			_showHPBar = true;
		}





		private void ShipStatusHP_Paint( object sender, PaintEventArgs e ) {

			Graphics g = e.Graphics;
			Rectangle basearea = new Rectangle( Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical );
			Size barSize = ShowHPBar ? _HPBar.GetPreferredSize( new Size( basearea.Width, 0 ) ) : Size.Empty;



			if ( RepairTimeShowMode == ShipStatusHPRepairTimeShowMode.Visible ||
				( RepairTimeShowMode == ShipStatusHPRepairTimeShowMode.MouseOver && _onMouse ) ) {
				string timestr = DateTimeHelper.ToTimeRemainString( (DateTime)RepairTime );

				var rect = new Rectangle( basearea.X, basearea.Y, basearea.Width, basearea.Height - barSize.Height );
				Font font;
				if ( rect.Width >= RepairTimeSizeCache.Width )
					font = MainFont;
				else
					font = SubFont;

				TextRenderer.DrawText( g, timestr, font, rect, RepairFontColor, TextFormatTime );

			} else {

				Point p = new Point( basearea.X, basearea.Bottom - barSize.Height - Math.Max( TextSizeCache.Height, MaximumValueSizeCache.Height ) + 1 );
				TextRenderer.DrawText( g, Text, SubFont, new Rectangle( p, TextSizeCache ), SubFontColor, TextFormatText );
				//g.DrawRectangle( Pens.Orange, new Rectangle( p, TextSizeCache ) );

				p.X = basearea.Right - MaximumValueSizeCache.Width;
				TextRenderer.DrawText( g, !ShowDifference ? MaximumValue.ToString() : GetDifferenceString(), SubFont, new Rectangle( p, MaximumValueSizeCache ), SubFontColor, TextFormatHP );
				//g.DrawRectangle( Pens.Orange, new Rectangle( p, MaximumValueSizeCache ) );

				p.X -= SlashSizeCache.Width;
				TextRenderer.DrawText( g, SlashText, SubFont, new Rectangle( p, SlashSizeCache ), SubFontColor, TextFormatHP );
				//g.DrawRectangle( Pens.Orange, new Rectangle( p, SlashSizeCache ) );

				p.X -= ValueSizeCache.Width;
				p.Y = basearea.Bottom - barSize.Height - ValueSizeCache.Height + 1;
				TextRenderer.DrawText( g, Math.Max( Value, 0 ).ToString(), MainFont, new Rectangle( p, ValueSizeCache ), MainFontColor, TextFormatHP );
				//g.DrawRectangle( Pens.Orange, new Rectangle( p, ValueSizeCache ) );

			}

			if ( ShowHPBar )
				_HPBar.Paint( g, new Rectangle( basearea.X, basearea.Bottom - barSize.Height, barSize.Width, barSize.Height ) );
		}



		public override Size GetPreferredSize( Size proposedSize ) {

			if ( _preferredSizeCache == null ) {

				Size barSize = ShowHPBar ? _HPBar.GetPreferredSize() : Size.Empty;

				_preferredSizeCache = new Size(
					TextSizeCache.Width + ValueSizeCache.Width + SlashSizeCache.Width + MaximumValueSizeCache.Width + Padding.Horizontal,
					Math.Max( TextSizeCache.Height, ValueSizeCache.Height ) + barSize.Height + Padding.Vertical );
			}

			return _preferredSizeCache.Value;
		}



		private void PropertyChanged() {
			if ( !IsRefreshSuspended ) {
				if ( AutoSize ) {
					var size = GetPreferredSize( MaxSize );

					if ( size != Size )
						Size = size;
				}
				Refresh();
			}
		}


		private static Color FromArgb( uint color ) {
			return Color.FromArgb( unchecked( (int)color ) );
		}

		private string GetDifferenceString() {

			return ( Value - PrevValue ).ToString( "+0;-0;-0" );
		}

		private void ShipStatusHP_MouseEnter( object sender, EventArgs e ) {
			_onMouse = true;
			if ( RepairTimeShowMode == ShipStatusHPRepairTimeShowMode.MouseOver )
				PropertyChanged();
		}

		private void ShipStatusHP_MouseLeave( object sender, EventArgs e ) {
			_onMouse = false;
			if ( RepairTimeShowMode == ShipStatusHPRepairTimeShowMode.MouseOver )
				PropertyChanged();
		}

	}


	public enum ShipStatusHPRepairTimeShowMode {
		Invisible,
		MouseOver,
		Visible,
	}
}
