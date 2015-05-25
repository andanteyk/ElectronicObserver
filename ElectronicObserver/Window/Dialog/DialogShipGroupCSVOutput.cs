using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog {
	public partial class DialogShipGroupCSVOutput : Form {

		/// <summary>
		/// 出力フィルタを指定します。
		/// </summary>
		public enum FilterModeConstants {

			/// <summary>全て出力</summary>
			All,
			
			/// <summary>表示されている行のみ出力</summary>
			VisibleColumnOnly,
		}

		/// <summary>
		/// 出力フォーマットを指定します。
		/// </summary>
		public enum OutputFormatConstants {

			/// <summary>閲覧用</summary>
			User,

			/// <summary>データ用</summary>
			Data,
		}


		/// <summary>
		/// 出力ファイルのパス
		/// </summary>
		public string OutputPath {
			get { return TextOutputPath.Text; }
			set { TextOutputPath.Text = value; }
		}

		/// <summary>
		/// 出力フィルタ
		/// </summary>
		public FilterModeConstants FilterMode {
			get {
				if ( RadioOutput_All.Checked )
					return FilterModeConstants.All;
				else
					return FilterModeConstants.VisibleColumnOnly;
			}
			set {
				switch ( value ) {
					case FilterModeConstants.All:
						RadioOutput_All.Checked = true; break;
					
					case FilterModeConstants.VisibleColumnOnly:
						RadioOutput_VisibleColumnOnly.Checked = true; break;
				}
			}
		}

		/// <summary>
		/// 出力フォーマット
		/// </summary>
		public OutputFormatConstants OutputFormat {
			get {
				if ( RadioFormat_User.Checked )
					return OutputFormatConstants.User;
				else
					return OutputFormatConstants.Data;
			}
			set {
				switch ( value ) {
					case OutputFormatConstants.User:
						RadioFormat_User.Checked = true; break;

					case OutputFormatConstants.Data:
						RadioFormat_Data.Checked = true; break;
				}
			}
		}



		public DialogShipGroupCSVOutput() {
            SuspendLayout();

			InitializeComponent();

			DialogSaveCSV.InitialDirectory = Utility.Configuration.Config.Connection.SaveDataPath;

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96, 96);
            ResumeLayout();

        }

		private void DialogShipGroupCSVOutput_Load( object sender, EventArgs e ) {

			
		}

		private void ButtonOutputPathSearch_Click( object sender, EventArgs e ) {

			if ( DialogSaveCSV.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

				TextOutputPath.Text = DialogSaveCSV.FileName;

			}

			DialogSaveCSV.InitialDirectory = null;

		}

		private void ButtonOK_Click( object sender, EventArgs e ) {
			DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void ButtonCancel_Click( object sender, EventArgs e ) {
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}

	}
}
