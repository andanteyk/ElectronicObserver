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
	public partial class DialogShipGroupColumnFilter : Form {

		public bool[] CheckedList {
			get {
				bool[] checkedList = new bool[FilterList.Items.Count];
				for ( int i = 0; i < checkedList.Length; i++ ) {
					checkedList[i] = FilterList.GetItemChecked( i );
				}

				return checkedList;
			}
		}

		public DialogShipGroupColumnFilter( DataGridView target ) {
            SuspendLayout();
			InitializeComponent();

			AllCheck.Tag = false;
			
			foreach ( DataGridViewColumn c in target.Columns ) {
				FilterList.Items.Add( c.HeaderText, c.Visible );
			}


			{
				int count = FilterList.CheckedItems.Count;
				if ( count == 0 )
					AllCheck.CheckState = CheckState.Unchecked;
				else if ( count == FilterList.Items.Count )
					AllCheck.CheckState = CheckState.Checked;
				else
					AllCheck.CheckState = CheckState.Indeterminate;

			}
			AllCheck.Tag = true;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96, 96);
            ResumeLayout();
        }



		private void ButtonOK_Click( object sender, EventArgs e ) {
			DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void ButtonCancel_Click( object sender, EventArgs e ) {
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}



		private void AllCheck_CheckedChanged( object sender, EventArgs e ) {

			if ( AllCheck.CheckState == CheckState.Indeterminate )
				return;

			bool check = AllCheck.Checked;
			AllCheck.Tag = false;

			for ( int i = 0; i < FilterList.Items.Count; i++ ) {
				FilterList.SetItemChecked( i, check );
			}
			AllCheck.Tag = true;

		}


		private void FilterList_ItemCheck( object sender, ItemCheckEventArgs e ) {

			if ( (bool)AllCheck.Tag )
				AllCheck.CheckState = CheckState.Indeterminate;

		}

	}
}
