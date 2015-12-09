using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Window.Control {

	/// <summary>
	/// 現在値と最大値を視覚的に表現するバー
	/// </summary>
	[TypeConverter( typeof( ExpandableObjectConverter ) )]
	public class StatusBarModule {


		public event EventHandler PropertyChanged = delegate { };


		#region Property

		private int _value = 66;
		/// <summary>
		/// 現在値
		/// </summary>
		[Browsable( true )]
		[Description( "現在値を指定します。" )]
		[DefaultValue( 66 )]
		public int Value {
			get { return _value; }
			set {
				_value = value;
				if ( !_usePrevValue )
					_prevValue = _value;
				Refresh();
			}
		}

		private int _prevValue = 88;
		/// <summary>
		/// 直前の現在値
		/// </summary>
		[Browsable( true )]
		[Description( "直前の値を指定します。" )]
		[DefaultValue( 88 )]
		public int PrevValue {
			get { return _prevValue; }
			set {
				if ( _usePrevValue )
					_prevValue = value;
				Refresh();
			}
		}

		private int _maximumValue = 100;
		/// <summary>
		/// 最大値
		/// </summary>
		[Browsable( true )]
		[Description( "最大値を指定します。" )]
		[DefaultValue( 100 )]
		public int MaximumValue {
			get { return _maximumValue; }
			set {
				_maximumValue = value;
				Refresh();
			}
		}

		private bool _usePrevValue = true;
		/// <summary>
		/// 直前の値を利用するかどうか
		/// </summary>
		[Browsable( true )]
		[Description( "直前の値を利用するかを指定します。" )]
		[DefaultValue( true )]
		public bool UsePrevValue {
			get { return _usePrevValue; }
			set {
				_usePrevValue = value;
				if ( !_usePrevValue )
					_prevValue = _value;
				Refresh();
			}
		}


		private Color _barColor0 = FromArgb( 0xFFFF0000 );
		/// <summary>
		/// バーの色(0%～25%)
		/// </summary>
		[Browsable( true )]
		[Description( "0%～25%の時のバーの色を指定します。" )]
		[DefaultValue( typeof( Color ), "255, 0, 0" )]
		public Color BarColor0 {
			get { return _barColor0; }
			set {
				_barColor0 = value;
				Refresh();
			}
		}

		private Color _barColor1 = FromArgb( 0xFFFF8800 );
		/// <summary>
		/// バーの色(25%～50%)
		/// </summary>
		[Browsable( true )]
		[Description( "25%～50%の時のバーの色を指定します。" )]
		[DefaultValue( typeof( Color ), "255, 136, 0" )]
		public Color BarColor1 {
			get { return _barColor1; }
			set {
				_barColor1 = value;
				Refresh();
			}
		}

		private Color _barColor2 = FromArgb( 0xFFFFCC00 );
		/// <summary>
		/// バーの色(50%～75%)
		/// </summary>
		[Browsable( true )]
		[Description( "50%～75%の時のバーの色を指定します。" )]
		[DefaultValue( typeof( Color ), "255, 204, 0" )]
		public Color BarColor2 {
			get { return _barColor2; }
			set {
				_barColor2 = value;
				Refresh();
			}
		}

		private Color _barColor3 = FromArgb( 0xFF00CC00 );
		/// <summary>
		/// バーの色(75%～100%)
		/// </summary>
		[Browsable( true )]
		[Description( "75%～100%の時のバーの色を指定します。" )]
		[DefaultValue( typeof( Color ), "0, 204, 0" )]
		public Color BarColor3 {
			get { return _barColor3; }
			set {
				_barColor3 = value;
				Refresh();
			}
		}

		private Color _barColor4 = FromArgb( 0xFF0044CC );
		/// <summary>
		/// バーの色(100%)
		/// </summary>
		[Browsable( true )]
		[Description( "100%の時のバーの色を指定します。" )]
		[DefaultValue( typeof( Color ), "0, 68, 204" )]
		public Color BarColor4 {
			get { return _barColor4; }
			set {
				_barColor4 = value;
				Refresh();
			}
		}

		private Color _barColorIncrement =  FromArgb( 0xFF44FF00 );
		/// <summary>
		/// バーの色(増加分)
		/// </summary>
		[Browsable( true )]
		[Description( "現在値が増加した時のバーの色を指定します。" )]
		[DefaultValue( typeof( Color ), "68, 255, 0" )]
		public Color BarColorIncrement {
			get { return _barColorIncrement; }
			set {
				_barColorIncrement = value;
				Refresh();
			}
		}

		private Color _barColorDecrement = FromArgb( 0xFF882222 );
		/// <summary>
		/// バーの色(減少分)
		/// </summary>
		[Browsable( true )]
		[Description( "現在値が減少した時のバーの色を指定します。" )]
		[DefaultValue( typeof( Color ), "136, 34, 34" )]
		public Color BarColorDecrement {
			get { return _barColorDecrement; }
			set {
				_barColorDecrement = value;
				Refresh();
			}
		}

		private Color _barColorBackground = FromArgb( 0xFF888888 );
		/// <summary>
		/// バーの背景色
		/// </summary>
		[Browsable( true )]
		[Description( "バーの背景色を指定します。" )]
		[DefaultValue( typeof( Color ), "68, 68, 68" )]
		public Color BarColorBackground {
			get { return _barColorBackground; }
			set {
				_barColorBackground = value;
				Refresh();
			}
		}


		private int _barThickness = 4;
		/// <summary>
		/// バーの太さ
		/// </summary>
		[Browsable( true )]
		[Description( "バーの太さ(高さ)を指定します。" )]
		[DefaultValue( 4 )]
		public int BarThickness {
			get { return _barThickness; }
			set {
				_barThickness = value;
				Refresh();
			}
		}

		private int _barBackgroundOffset = 1;
		/// <summary>
		/// バーの前景と背景とのずれの大きさ
		/// </summary>
		[Browsable( true )]
		[Description( "バーの前景と背景のずれの大きさを指定します。影のような表現に利用します。" )]
		[DefaultValue( 1 )]
		public int BarBackgroundOffset {
			get { return _barBackgroundOffset; }
			set {
				_barBackgroundOffset = value;
				Refresh();
			}
		}

		private bool _colorMorphing = false;
		[Browsable( true )]
		[Description( "バーの色を割合に応じて滑らかに変化させるかを指定します。" )]
		[DefaultValue( false )]
		/// <summary>
		/// 色を滑らかに変化させるか
		/// </summary>
		public bool ColorMorphing {
			get { return _colorMorphing; }
			set {
				_colorMorphing = value;
				Refresh();
			}
		}



		#endregion



		public StatusBarModule() {

		}



		public void SetBarColor( IEnumerable<Color> colors ) {

			int length = colors.Count();

			if ( length < 5 )
				return;

			_barColor0 = colors.ElementAt( 0 );
			_barColor1 = colors.ElementAt( 1 );
			_barColor2 = colors.ElementAt( 2 );
			_barColor3 = colors.ElementAt( 3 );
			_barColor4 = colors.ElementAt( 4 );

			if ( length >= 7 ) {
				_barColorIncrement = colors.ElementAt( 5 );
				_barColorDecrement = colors.ElementAt( 6 ); ;
			}

			if ( length >= 8 )
				_barColorBackground = colors.ElementAt( 7 );


			Refresh();
		}


		private static double GetPercentage( int value, int max ) {
			if ( max <= 0 || value <= 0 )
				return 0.0;
			else if ( value > max )
				return 1.0;
			else
				return (double)value / max;
		}

		private Color GetBarColor() {
			if ( Value <= MaximumValue * 0.25 )
				return BarColor0;
			else if ( Value <= MaximumValue * 0.50 )
				return BarColor1;
			else if ( Value <= MaximumValue * 0.75 )
				return BarColor2;
			else if ( Value < MaximumValue )
				return BarColor3;
			else
				return BarColor4;
		}



		private static Color FromArgb( uint color ) {
			return Color.FromArgb( unchecked( (int)color ) );
		}

		private static Color BlendColor( Color a, Color b, double weight ) {

			if ( weight < 0.0 || 1.0 < weight )
				throw new ArgumentOutOfRangeException( "weight は 0.0 - 1.0 の範囲内でなければなりません。指定値: " + weight );

			return Color.FromArgb(
				(int)( a.A * weight + b.A * ( 1 - weight ) ),
				(int)( a.R * weight + b.R * ( 1 - weight ) ),
				(int)( a.G * weight + b.G * ( 1 - weight ) ),
				(int)( a.B * weight + b.B * ( 1 - weight ) ) );
		}


		/// <summary>
		/// バーを描画します。
		/// </summary>
		/// <param name="g">描画するための Graphics。</param>
		/// <param name="rect">描画する領域。</param>
		public void Paint( Graphics g, Rectangle rect ) {

			using ( var b = new SolidBrush( BarColorBackground ) ) {
				g.FillRectangle( b, new Rectangle( rect.X + BarBackgroundOffset, rect.Bottom - BarThickness, rect.Width - BarBackgroundOffset, BarThickness ) );
			}
			using ( var b = new SolidBrush( Value > PrevValue ? BarColorIncrement : BarColorDecrement ) ) {
				g.FillRectangle( b, new Rectangle( rect.X, rect.Bottom - BarThickness - BarBackgroundOffset,
					(int)Math.Ceiling( ( rect.Width - BarBackgroundOffset ) * GetPercentage( Math.Max( Value, PrevValue ), MaximumValue ) ), BarThickness ) );
			}

			Color barColor;
			if ( !ColorMorphing ) {
				barColor = GetBarColor();

			} else {
				double p = GetPercentage( Value, MaximumValue );

				/*/
				if ( p <= 0.25 ) {
					barColor = BarColor0;
				} else if ( p <= 0.5 ) {
					barColor = BlendColor( BarColor0, BarColor1, ( 0.5 - p ) * 4.0 );
				} else if ( p <= 0.75 ) {
					barColor = BlendColor( BarColor1, BarColor2, ( 0.75 - p ) * 4.0 );
				} else if ( p < 1.0 ) {
					barColor = BlendColor( BarColor2, BarColor3, ( 1.0 - p ) * 4.0 );
				} else {
					barColor = BarColor4;
				}
				/*/
				if ( p <= 1.0 / 3.0 ) {
					barColor = BlendColor( BarColor0, BarColor1, ( 1.0 / 3.0 - p ) * 3.0 );
				} else if ( p <= 2.0 / 3.0 ) {
					barColor = BlendColor( BarColor1, BarColor2, ( 2.0 / 3.0 - p ) * 3.0 );
				} else if ( p < 1.0 ) {
					barColor = BlendColor( BarColor2, BarColor3, ( 1.0 - p ) * 3.0 );
				} else {
					barColor = BarColor4;
				}
				//*/
			}

			using ( var b = new SolidBrush( barColor ) ) {
				g.FillRectangle( b, new Rectangle( rect.X, rect.Bottom - BarThickness - BarBackgroundOffset,
					(int)Math.Ceiling( ( rect.Width - BarBackgroundOffset ) * GetPercentage( Math.Min( Value, PrevValue ), MaximumValue ) ), BarThickness ) );
			}

		}



		/// <summary>
		/// このモジュールを描画するために適切なサイズを取得します。
		/// </summary>
		public Size GetPreferredSize( Size proposedSize ) {
			return new Size( Math.Max( proposedSize.Width, BarThickness + BarBackgroundOffset ),
				Math.Max( proposedSize.Height, BarThickness + BarBackgroundOffset ) );
		}

		/// <summary>
		/// このモジュールを描画するために適切なサイズを取得します。
		/// </summary>
		public Size GetPreferredSize() {
			return GetPreferredSize( Size.Empty );
		}


		private void Refresh() {
			PropertyChanged( this, new EventArgs() );
		}

	}

}
