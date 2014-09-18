using DigitalArsenal.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalArsenal {

	//いわゆる実験場です。コントロール等の実装試験を行います。
	public partial class FormMain : Form {




		public FormMain() {
			InitializeComponent();

		}


		private void FormMain_Shown( object sender, EventArgs e ) {


			//page.2 table
			{
				Random rnd = new Random();

				Color colorMain = FromArgb( 0xFF000000 );
				Color colorSub = FromArgb( 0xFF888888 );
				Font fontMain = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
				Font fontSub = new Font( "Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel );
				Padding padding = new Padding( 0, 1, 0, 1 );
				Padding margin = new Padding( 2, 0, 2, 0 );

				for ( int i = 0; i < 6; i++ ) {
					//*/
					{
						var newLabel = new Label();
						Func<string> getName = () => {
							string[] str = new string[] { "雷", "電", "暁", "響", "潮", "島風", "浦風", "飛鷹", "飛龍", "伊168", "不知火", "阿賀野", "阿武隈", "あきつ丸", "夕立改二", "五十鈴改二", "千代田航改二", "Верный", "Bismarck zwei" };
							return str[rnd.Next( str.Length )];
						};
						newLabel.Text = getName();
						newLabel.Anchor = AnchorStyles.Left;
						newLabel.AutoSize = true;
						newLabel.Font = fontMain;
						newLabel.ForeColor = colorMain;
						//newLabel.BorderStyle = BorderStyle.FixedSingle;
						//newLabel.TextAlign = ContentAlignment.BottomCenter;
						newLabel.Padding = padding;
						newLabel.Margin = margin;
						TableTest.Controls.Add( newLabel, 0, i );
					}
					//*/
					{
						var newLabel = new ShipStatusLevel();
						newLabel.Anchor = AnchorStyles.Left;
						newLabel.Value = rnd.Next( 0, 50 ) + rnd.Next( 0, 50 ) + rnd.Next( 0, 50 );
						newLabel.MaximumValue = 150;
						newLabel.MainFont = fontMain;
						newLabel.SubFont = fontSub;
						newLabel.MainFontColor = colorMain;
						newLabel.SubFontColor = colorSub;
						newLabel.Text = "Lv.";
						newLabel.ValueNext = rnd.Next( 150000 );
						//newLabel.TextNext = "remodel:";
						//newLabel.BorderStyle = BorderStyle.FixedSingle;
						newLabel.BackColor = Color.MistyRose;
						newLabel.Padding = new Padding( 0 );
						newLabel.Margin = margin;
						newLabel.AutoSize = true;
						TableTest.Controls.Add( newLabel, 1, i );
					}
					{
						var newLabel = new ShipStatusHP();
						newLabel.Anchor = AnchorStyles.Left;
						newLabel.MaximumValue = rnd.Next( 4, 120 );
						newLabel.Value = newLabel.PrevValue = rnd.Next( 1, newLabel.MaximumValue );
						
						newLabel.MainFont = fontMain;
						newLabel.SubFont = fontSub;
						newLabel.MainFontColor = colorMain;
						newLabel.SubFontColor = colorSub;
						newLabel.Text = "HP:";
	
						//newLabel.BorderStyle = BorderStyle.FixedSingle;
						newLabel.BackColor = Color.MistyRose;
						newLabel.Padding = padding;
						newLabel.Margin = margin;
						newLabel.Size = new Size( 40, 20 );
						newLabel.AutoSize = true;
						TableTest.Controls.Add( newLabel, 2, i );
					}
					//*/
					RowStyle rs = new RowStyle( SizeType.Absolute, 21 );

					if ( TableTest.RowStyles.Count > i )
						TableTest.RowStyles[i] = rs;
					else {
						while ( TableTest.RowStyles.Count < i ) {
							TableTest.RowStyles.Add( rs /*new RowStyle( SizeType.AutoSize )*/ );
						}
						TableTest.RowStyles.Add( rs );
					}
					//*/
				}


				TableTest.BorderStyle = BorderStyle.FixedSingle;
			}

		}



		private void TableTest_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {

			//e.Graphics.DrawLine( Pens.Gainsboro, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Right - 1, e.CellBounds.Y );
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );

		}


		public static Color FromArgb( uint color ) {
			return Color.FromArgb( unchecked( (int)color ) );
		}



	}
}
