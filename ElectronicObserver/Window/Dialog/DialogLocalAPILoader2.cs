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
	public partial class DialogLocalAPILoader2 : Form {


		private string CurrentPath { get; set; }


		public DialogLocalAPILoader2() {
			InitializeComponent();
		}

		private void LoadFiles( string path ) {

			CurrentPath = path;

			APIView.Rows.Clear();

			var rows = new LinkedList<DataGridViewRow>();

			foreach ( string file in Directory.GetFiles( path, "*.json", SearchOption.TopDirectoryOnly ) {

				var row = new DataGridViewRow();
				row.CreateCells( APIView );

				row.SetValues( Path.GetFileName( file ) );

			}

			APIView.Rows.AddRange(rows.ToArray());
		}



		private void Menu_File_OpenFolder_Click( object sender, EventArgs e ) {

		}
	}
}
