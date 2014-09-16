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


		#region Parameters

		private int _value;
		[Browsable( true )]
		[DefaultValue( 0 )]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public int Value {
			get { return _value; }
			set {
				_value = value;
				Refresh();
			}
		}

		private Color _mainColor;
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "0, 0, 0" )]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public Color MainColor {
			get { return _mainColor; }
			set {
				_mainColor = value;
				Refresh();
			}
		}

		private Color _subColor;
		[Browsable( true )]
		[DefaultValue( typeof( Color ), "68, 68, 68" )]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public Color SubColor {
			get { return _subColor; }
			set {
				_subColor = value;
				Refresh();
			}
		}

		private Font _mainFont;
		[Browsable( true )]
		[DefaultValue( typeof( Font ), "Meiryo UI, 12px" )]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
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
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public Font SubFont {
			get { return _subFont; }
			set {
				_subFont = value;
				Refresh();
			}
		}


		[Browsable( true )]
		[DefaultValue( "Lv:" )]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
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
			_mainColor = Color.FromArgb( 0x00, 0x00, 0x00 );
			_subColor = Color.FromArgb( 0x44, 0x44, 0x44 );
			_mainFont = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			_subFont = new Font( "Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel );
			Text = "Lv.";

		}




		private void ShipStatusLevel_Paint( object sender, PaintEventArgs e ) {
			//WIP
		}

	}
}
