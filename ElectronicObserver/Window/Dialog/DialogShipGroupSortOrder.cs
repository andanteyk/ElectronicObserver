using ElectronicObserver.Data;
using ElectronicObserver.Window.Support;
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
	public partial class DialogShipGroupSortOrder : Form {

		public List<KeyValuePair<string, ListSortDirection>> Result { get; private set; }
		public bool AutoSortEnabled { get { return AutoSortFlag.Checked; } }



		public DialogShipGroupSortOrder( DataGridView target, ShipGroupData group ) {
			InitializeComponent();

			var rows_enabled = new LinkedList<DataGridViewRow>();
			var rows_disabled = new LinkedList<DataGridViewRow>();

			var columns = target.Columns.Cast<DataGridViewColumn>();
			var names = columns.Select( c => c.Name );


			if ( group.SortOrder == null )
				group.SortOrder = new List<KeyValuePair<string, ListSortDirection>>();

			foreach ( var sort in group.SortOrder.Where( s => names.Contains( s.Key ) ) ) {

				var row = new DataGridViewRow();

				row.CreateCells( EnabledView );
				row.SetValues( target.Columns[sort.Key].HeaderText, sort.Value );
				row.Cells[EnabledView_Name.Index].Tag = sort.Key;
				row.Tag = columns.FirstOrDefault( c => c.Name == sort.Key ).DisplayIndex;

				rows_enabled.AddLast( row );
			}

			foreach ( var name in names.Where( n => group.SortOrder.Count( s => n == s.Key ) == 0 ) ) {

				var row = new DataGridViewRow();

				row.CreateCells( DisabledView );
				row.SetValues( target.Columns[name].HeaderText );
				row.Cells[DisabledView_Name.Index].Tag = name;
				row.Tag = columns.FirstOrDefault( c => c.Name == name ).DisplayIndex;

				rows_disabled.AddLast( row );
			}

			EnabledView.Rows.AddRange( rows_enabled.ToArray() );
			DisabledView.Rows.AddRange( rows_disabled.ToArray() );


			AutoSortFlag.Checked = group.AutoSortEnabled;
		}

		private void DialogShipGroupSortOrder_Load( object sender, EventArgs e ) {
			if ( Owner != null )
				Icon = Owner.Icon;
		}



		// ボタン操作
		private void EnabledView_CellContentClick( object sender, DataGridViewCellEventArgs e ) {

			if ( e.ColumnIndex == EnabledView_SortDirection.Index ) {

				EnabledView[e.ColumnIndex, e.RowIndex].Value = ( (ListSortDirection)EnabledView[e.ColumnIndex, e.RowIndex].Value ) == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

			} else if ( e.ColumnIndex == EnabledView_Up.Index ) {

				if ( !ControlHelper.RowMoveUp( EnabledView, e.RowIndex ) ) {
					System.Media.SystemSounds.Exclamation.Play();
				}

			} else if ( e.ColumnIndex == EnabledView_Down.Index ) {

				if ( !ControlHelper.RowMoveDown( EnabledView, e.RowIndex ) ) {
					System.Media.SystemSounds.Exclamation.Play();
				}

			}
		}


		private void EnabledView_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e ) {

			if ( e.RowIndex >= 0 && e.ColumnIndex == EnabledView_SortDirection.Index ) {

				switch ( (ListSortDirection)e.Value ) {
					case ListSortDirection.Ascending:
						e.Value = "▲";
						e.FormattingApplied = true;
						break;
					case ListSortDirection.Descending:
						e.Value = "▼";
						e.FormattingApplied = true;
						break;
				}

			}

		}



		private void DisabledView_SortCompare( object sender, DataGridViewSortCompareEventArgs e ) {

			e.SortResult = (int)DisabledView.Rows[e.RowIndex1].Tag -
				(int)DisabledView.Rows[e.RowIndex2].Tag;
			e.Handled = true;

		}



		private void ButtonUp_Click( object sender, EventArgs e ) {

			if ( EnabledView.SelectedRows.Count == 0 || !ControlHelper.RowMoveUp( EnabledView, EnabledView.SelectedRows[0].Index ) ) {
				System.Media.SystemSounds.Exclamation.Play();
			}
		}


		private void ButtonDown_Click( object sender, EventArgs e ) {

			if ( EnabledView.SelectedRows.Count == 0 || !ControlHelper.RowMoveDown( EnabledView, EnabledView.SelectedRows[0].Index ) ) {
				System.Media.SystemSounds.Exclamation.Play();
			}
		}



		private void ButtonLeft_Click( object sender, EventArgs e ) {

			var selectedRows = DisabledView.SelectedRows;

			if ( selectedRows.Count == 0 ) {
				System.Media.SystemSounds.Asterisk.Play();
				return;
			}

			var addrows = new DataGridViewRow[selectedRows.Count];
			int i = 0;

			foreach ( DataGridViewRow src in selectedRows ) {
				addrows[i] = new DataGridViewRow();
				addrows[i].CreateCells( EnabledView );
				addrows[i].SetValues( src.Cells[DisabledView_Name.Index].Value, ListSortDirection.Ascending );
				addrows[i].Cells[EnabledView_Name.Index].Tag = src.Cells[DisabledView_Name.Index].Tag;
				addrows[i].Tag = src.Tag;
				DisabledView.Rows.Remove( src );
				i++;
			}

			EnabledView.Rows.AddRange( addrows );
			DisabledView.Sort( DisabledView_Name, ListSortDirection.Ascending );
		}

		private void ButtonRight_Click( object sender, EventArgs e ) {

			var selectedRows = EnabledView.SelectedRows;

			if ( selectedRows.Count == 0 ) {
				System.Media.SystemSounds.Asterisk.Play();
				return;
			}

			var addrows = new DataGridViewRow[selectedRows.Count];
			int i = 0;

			foreach ( DataGridViewRow src in selectedRows ) {
				addrows[i] = new DataGridViewRow();
				addrows[i].CreateCells( DisabledView );
				addrows[i].SetValues( src.Cells[DisabledView_Name.Index].Value );
				addrows[i].Cells[DisabledView_Name.Index].Tag = src.Cells[EnabledView_Name.Index].Tag;
				addrows[i].Tag = src.Tag;
				EnabledView.Rows.Remove( src );
				i++;
			}

			DisabledView.Rows.AddRange( addrows );
			DisabledView.Sort( DisabledView_Name, ListSortDirection.Ascending );
		}


		private void ButtonLeftAll_Click( object sender, EventArgs e ) {

			var addrows = new DataGridViewRow[DisabledView.Rows.Count];
			int i = 0;

			foreach ( DataGridViewRow src in DisabledView.Rows ) {
				addrows[i] = new DataGridViewRow();
				addrows[i].CreateCells( EnabledView );
				addrows[i].SetValues( src.Cells[DisabledView_Name.Index].Value, ListSortDirection.Ascending );
				addrows[i].Cells[EnabledView_Name.Index].Tag = src.Cells[DisabledView_Name.Index].Tag;
				addrows[i].Tag = src.Tag;
				i++;
			}

			DisabledView.Rows.Clear();
			EnabledView.Rows.AddRange( addrows );
			DisabledView.Sort( DisabledView_Name, ListSortDirection.Ascending );

		}

		private void ButtonRightAll_Click( object sender, EventArgs e ) {

			var addrows = new DataGridViewRow[EnabledView.Rows.Count];
			int i = 0;

			foreach ( DataGridViewRow src in EnabledView.Rows ) {
				addrows[i] = new DataGridViewRow();
				addrows[i].CreateCells( DisabledView );
				addrows[i].SetValues( src.Cells[DisabledView_Name.Index].Value );
				addrows[i].Cells[DisabledView_Name.Index].Tag = src.Cells[EnabledView_Name.Index].Tag;
				addrows[i].Tag = src.Tag;
				i++;
			}

			EnabledView.Rows.Clear();
			DisabledView.Rows.AddRange( addrows );
			DisabledView.Sort( DisabledView_Name, ListSortDirection.Ascending );

		}




		private void ButtonOK_Click( object sender, EventArgs e ) {

			Result = new List<KeyValuePair<string, ListSortDirection>>( EnabledView.Rows.Count );

			foreach ( DataGridViewRow row in EnabledView.Rows ) {
				Result.Add( new KeyValuePair<string, ListSortDirection>( (string)row.Cells[EnabledView_Name.Index].Tag, (ListSortDirection)row.Cells[EnabledView_SortDirection.Index].Value ) );
			}


			DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void ButtonCancel_Click( object sender, EventArgs e ) {

			DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}


	}
}
