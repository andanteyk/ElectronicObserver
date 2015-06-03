using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog {

	public partial class DialogBattleReport : Form {

		private string html;

		public DialogBattleReport(string html) {
			InitializeComponent();

			this.html = html;
		}

		private void DialogBattleReport_Load( object sender, EventArgs e ) {
			webBrowser1.DocumentText = html;
		}

		private void MenuFile_Save_Click( object sender, EventArgs e ) {

			if ( saveFileDialog1.ShowDialog( this ) == DialogResult.OK ) {

				// 保存
				try {
					File.WriteAllText( saveFileDialog1.FileName, html, Encoding.UTF8 );
				} catch ( Exception ex ) {
					Utility.ErrorReporter.SendErrorReport( ex, "保存战斗日志时写入文件失败。" );
				}
			}
		}
	}
}
