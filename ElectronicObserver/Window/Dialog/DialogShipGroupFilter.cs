using ElectronicObserver.Data.ShipGroup;
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
	public partial class DialogShipGroupFilter : Form {

		private ExpressionManager _target;



		public DialogShipGroupFilter() {
			InitializeComponent();

			_target = null;
		}



		public void ImportExpressionData( ExpressionManager exm ) {

			_target = exm.Clone();

			ExpressionView.Rows.Clear();


			var rows = new DataGridViewRow[exm.Expressions.Count];
			for ( int i = 0; i < rows.Length; i++ ) {
				var ex = exm.Expressions[i];
				var row = new DataGridViewRow();
				row.CreateCells( ExpressionView );

				row.SetValues(
					ex.Enabled,
					ex.ExternalAnd ? "And" : "Or",
					ex.Inverse,
					ex.ToString()
					);

				rows[i] = row;
			}

			ExpressionView.Rows.AddRange( rows.ToArray() );


			ExpressionDetailView.Rows.Clear();

		}



		public ExpressionManager ExportExpressionData() {

			//undone: gui 動作の適用
			return _target;
		}


		private void InitGroupExpression() {
			GroupExpression.Enabled = false;

			RightOperand_ComboBox.Visible = false;
			RightOperand_ComboBox.Items.Clear();

			RightOperand_NumericUpDown.Visible = false;
			RightOperand_NumericUpDown.Value = RightOperand_NumericUpDown.Maximum = RightOperand_NumericUpDown.Minimum = 0;

			RightOperand_TextBox.Visible = true;
			RightOperand_TextBox.Text = "";

			ButtonAdd.Enabled = false;
			ButtonEdit.Enabled = false;
			ButtonRemove.Enabled = false;
		}





		private void ExpressionView_CellMouseClick( object sender, DataGridViewCellMouseEventArgs e ) {

			if ( e.RowIndex < 0 || _target.Expressions.Count <= e.RowIndex ) return;

			var ex = _target.Expressions[e.RowIndex];



			if ( e.ColumnIndex == ExpressionView_Up.Index ) {
				//undone

			} else if ( e.ColumnIndex == ExpressionView_Down.Index ) {
				//undone

			} else {
				// detail の更新と expression の初期化

				ExpressionDetailView.Rows.Clear();

				var rows = new DataGridViewRow[ex.Expressions.Count];
				for ( int i = 0; i < rows.Length; i++ ) {
					var row = new DataGridViewRow();
					row.CreateCells( ExpressionDetailView );

					row.SetValues(
						ex.Expressions[i].Enabled,
						ex.Expressions[i].LeftOperand,
						ex.Expressions[i].RightOperand,
						ex.Expressions[i].Operator
						);

					rows[i] = row;
				}
				ExpressionDetailView.Rows.AddRange( rows );

				InitGroupExpression();
			}
		}




		private void LeftOperand_SelectedValueChanged( object sender, EventArgs e ) {



		}



	}
}
