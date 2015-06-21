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
	public partial class ColorPicker : UserControl {

		[Browsable( true )]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Visible )]
		[EditorBrowsable( EditorBrowsableState.Always )]
		public override string Text {
			get { return label1.Text; }
			set { label1.Text = value; }
		}

		[DefaultValue( typeof( Color ), "0, 0, 0" )]
		public Color SelectedColor {
			get { return label2.BackColor; }
			set {
				label2.BackColor = value;
				textBox1.Text = ToColorString( value );
			}
		}

		[DefaultValue( typeof( Color ), "Transparent" )]
		public override Color BackColor {
			get { return base.BackColor; }
			set { base.BackColor = value; }
		}

		[DefaultValue( typeof( Font ), "Microsoft JhengHei UI, 12px" )]
		public override Font Font {
			get { return base.Font; }
			set { base.Font = value; }
		}

		public ColorPicker() {
			InitializeComponent();
			textBox1.LostFocus += textBox1_LostFocus;
		}

		private void textBox1_LostFocus( object sender, EventArgs e ) {

			textBox1.Text = ToColorString( label2.BackColor );
		}

		private void textBox1_TextChanged( object sender, EventArgs e ) {

			Color c = ParseColor( textBox1.Text );
			if ( c != Color.Empty ) {

				label2.BackColor = c;
			}
		}

		private static Color ParseColor( string str ) {

			if ( string.IsNullOrEmpty( str ) || str.Length <= 1 ) {
				return Color.Empty;
			}

			int color;
			if ( str[0] == '#' ) {

				try {

					color = int.Parse( str.Substring( 1 ), System.Globalization.NumberStyles.HexNumber );
					return Color.FromArgb( color );
				} catch {

					return Color.Empty;
				}

			} else {

				if ( int.TryParse( str, out color ) ) {

					return Color.FromArgb( color );
				}
			}

			return Color.Empty;
		}

		private static string ToColorString( Color color ) {

			return string.Format( "#{0:X8}", color.ToArgb() );

		}

		private void label2_Click( object sender, EventArgs e ) {

			Color_Dialog.Color = label2.BackColor;

			if ( Color_Dialog.ShowDialog( this.ParentForm ) == DialogResult.OK ) {

				this.SelectedColor = Color_Dialog.Color;

			}
		}

		private static ColorDialog _Color_Dialog;

		public static ColorDialog Color_Dialog {
			get {
				if ( _Color_Dialog == null ) {

					_Color_Dialog = new ColorDialog();
					_Color_Dialog.FullOpen = true;
				}

				return _Color_Dialog;
			}
		}
	}
}
