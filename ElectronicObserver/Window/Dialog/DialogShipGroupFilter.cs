using ElectronicObserver.Data;
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


		#region DataTable
		private DataTable _dtAndOr;
		private DataTable _dtLeftOperand;
		private DataTable _dtOperator;
		private DataTable _dtOperator_bool;
		private DataTable _dtOperator_number;
		private DataTable _dtOperator_string;
		private DataTable _dtOperator_array;
		private DataTable _dtRightOperand_bool;
		private DataTable _dtRightOperand_shiptype;
		private DataTable _dtRightOperand_equipment;
		#endregion

		public DialogShipGroupFilter( ExpressionManager exp ) {
			InitializeComponent();


			#region init DataTable
			{
				_dtAndOr = new DataTable();
				_dtAndOr.Columns.AddRange( new DataColumn[]{ 
					new DataColumn( "Value", typeof( bool ) ), 
					new DataColumn( "Display", typeof( string ) ) } );
				_dtAndOr.Rows.Add( true, "And" );
				_dtAndOr.Rows.Add( false, "Or" );
				_dtAndOr.AcceptChanges();

				ExpressionView_InternalAndOr.ValueMember = "Value";
				ExpressionView_InternalAndOr.DisplayMember = "Display";
				ExpressionView_InternalAndOr.DataSource = _dtAndOr;

				ExpressionView_ExternalAndOr.ValueMember = "Value";
				ExpressionView_ExternalAndOr.DisplayMember = "Display";
				ExpressionView_ExternalAndOr.DataSource = _dtAndOr;
			}
			{
				_dtLeftOperand = new DataTable();
				_dtLeftOperand.Columns.AddRange( new DataColumn[]{ 
					new DataColumn( "Value", typeof( string ) ), 
					new DataColumn( "Display", typeof( string ) ) } );
				foreach ( var lont in ExpressionData.LeftOperandNameTable )
					_dtLeftOperand.Rows.Add( lont.Key, lont.Value );
				_dtLeftOperand.AcceptChanges();

				LeftOperand.ValueMember = "Value";
				LeftOperand.DisplayMember = "Display";
				LeftOperand.DataSource = _dtLeftOperand;

				ExpressionDetailView_LeftOperand.ValueMember = "Value";
				ExpressionDetailView_LeftOperand.DisplayMember = "Display";
				ExpressionDetailView_LeftOperand.DataSource = _dtLeftOperand;
			}
			{
				_dtOperator = new DataTable();
				_dtOperator.Columns.AddRange( new DataColumn[]{ 
					new DataColumn( "Value", typeof( ExpressionData.ExpressionOperator ) ), 
					new DataColumn( "Display", typeof( string ) ) } );
				foreach ( var ont in ExpressionData.OperatorNameTable )
					_dtOperator.Rows.Add( ont.Key, ont.Value );
				_dtOperator.AcceptChanges();

				Operator.ValueMember = "Value";
				Operator.DisplayMember = "Display";
				Operator.DataSource = _dtOperator;

				ExpressionDetailView_Operator.ValueMember = "Value";
				ExpressionDetailView_Operator.DisplayMember = "Display";
				ExpressionDetailView_Operator.DataSource = _dtOperator;
			}
			{
				_dtOperator_bool = new DataTable();
				_dtOperator_bool.Columns.AddRange( new DataColumn[]{ 
					new DataColumn( "Value", typeof( ExpressionData.ExpressionOperator ) ), 
					new DataColumn( "Display", typeof( string ) ) } );
				_dtOperator_bool.Rows.Add( ExpressionData.ExpressionOperator.Equal, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.Equal] );
				_dtOperator_bool.Rows.Add( ExpressionData.ExpressionOperator.NotEqual, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.NotEqual] );
				_dtOperator_bool.AcceptChanges();
			}
			{
				_dtOperator_number = new DataTable();
				_dtOperator_number.Columns.AddRange( new DataColumn[]{ 
					new DataColumn( "Value", typeof( ExpressionData.ExpressionOperator ) ), 
					new DataColumn( "Display", typeof( string ) ) } );
				_dtOperator_number.Rows.Add( ExpressionData.ExpressionOperator.Equal, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.Equal] );
				_dtOperator_number.Rows.Add( ExpressionData.ExpressionOperator.NotEqual, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.NotEqual] );
				_dtOperator_number.Rows.Add( ExpressionData.ExpressionOperator.LessThan, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.LessThan] );
				_dtOperator_number.Rows.Add( ExpressionData.ExpressionOperator.LessEqual, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.LessEqual] );
				_dtOperator_number.Rows.Add( ExpressionData.ExpressionOperator.GreaterThan, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.GreaterThan] );
				_dtOperator_number.Rows.Add( ExpressionData.ExpressionOperator.GreaterEqual, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.GreaterEqual] );
				_dtOperator_number.AcceptChanges();
			}
			{
				_dtOperator_string = new DataTable();
				_dtOperator_string.Columns.AddRange( new DataColumn[]{ 
					new DataColumn( "Value", typeof( ExpressionData.ExpressionOperator ) ), 
					new DataColumn( "Display", typeof( string ) ) } );
				_dtOperator_string.Rows.Add( ExpressionData.ExpressionOperator.Equal, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.Equal] );
				_dtOperator_string.Rows.Add( ExpressionData.ExpressionOperator.NotEqual, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.NotEqual] );
				_dtOperator_string.Rows.Add( ExpressionData.ExpressionOperator.Contains, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.Contains] );
				_dtOperator_string.Rows.Add( ExpressionData.ExpressionOperator.NotContains, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.NotContains] );
				_dtOperator_string.Rows.Add( ExpressionData.ExpressionOperator.BeginWith, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.BeginWith] );
				_dtOperator_string.Rows.Add( ExpressionData.ExpressionOperator.NotBeginWith, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.NotBeginWith] );
				_dtOperator_string.Rows.Add( ExpressionData.ExpressionOperator.EndWith, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.EndWith] );
				_dtOperator_string.Rows.Add( ExpressionData.ExpressionOperator.NotEndWith, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.NotEndWith] );
				_dtOperator_string.AcceptChanges();
			}
			{
				_dtOperator_array = new DataTable();
				_dtOperator_array.Columns.AddRange( new DataColumn[]{ 
					new DataColumn( "Value", typeof( ExpressionData.ExpressionOperator ) ), 
					new DataColumn( "Display", typeof( string ) ) } );
				_dtOperator_array.Rows.Add( ExpressionData.ExpressionOperator.ArrayContains, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.ArrayContains] );
				_dtOperator_array.Rows.Add( ExpressionData.ExpressionOperator.ArrayNotContains, ExpressionData.OperatorNameTable[ExpressionData.ExpressionOperator.ArrayNotContains] );
				_dtOperator_array.AcceptChanges();
			}
			{
				_dtRightOperand_bool = new DataTable();
				_dtRightOperand_bool.Columns.AddRange( new DataColumn[]{ 
					new DataColumn( "Value", typeof( bool ) ), 
					new DataColumn( "Display", typeof( string ) ) } );
				_dtRightOperand_bool.Rows.Add( true, "○" );
				_dtRightOperand_bool.Rows.Add( false, "×" );
				_dtRightOperand_bool.AcceptChanges();
			}
			{
				_dtRightOperand_shiptype = new DataTable();
				_dtRightOperand_shiptype.Columns.AddRange( new DataColumn[]{ 
					new DataColumn( "Value", typeof( ExpressionData.ExpressionOperator ) ), 
					new DataColumn( "Display", typeof( string ) ) } );
				foreach ( var st in KCDatabase.Instance.ShipTypes.Values )
					_dtRightOperand_shiptype.Rows.Add( st.TypeID, st.Name );
				_dtRightOperand_shiptype.AcceptChanges();
			}
			{
				_dtRightOperand_equipment = new DataTable();
				_dtRightOperand_equipment.Columns.AddRange( new DataColumn[]{ 
					new DataColumn( "Value", typeof( ExpressionData.ExpressionOperator ) ), 
					new DataColumn( "Display", typeof( string ) ) } );
				foreach ( var eq in KCDatabase.Instance.MasterEquipments.Values )
					_dtRightOperand_equipment.Rows.Add( eq.EquipmentID, eq.Name );
				_dtRightOperand_equipment.AcceptChanges();
			}

			RightOperand_ComboBox.ValueMember = "Value";
			RightOperand_ComboBox.DisplayMember = "Display";
			RightOperand_ComboBox.DataSource = _dtRightOperand_bool;

			#endregion


			_target = exp.Clone();
		}


		public void ImportExpressionData( ExpressionManager exm ) {

			_target = exm.Clone();

			ExpressionView.Rows.Clear();


			var rows = new DataGridViewRow[exm.Expressions.Count];
			for ( int i = 0; i < rows.Length; i++ ) {
				rows[i] = GetExpressionViewRow( exm.Expressions[i] );
			}

			ExpressionView.Rows.AddRange( rows.ToArray() );


			ExpressionDetailView.Rows.Clear();

		}



		public ExpressionManager ExportExpressionData() {

			//undone: gui 動作の適用
			return _target;
		}



		private DataGridViewRow GetExpressionViewRow( ExpressionList exp ) {
			var row = new DataGridViewRow();
			row.CreateCells( ExpressionView );

			row.SetValues(
				exp.Enabled,
				exp.ExternalAnd,
				exp.Inverse,
				exp.InternalAnd,
				exp.ToString()
				);

			return row;
		}

		private DataGridViewRow GetExpressionDetailViewRow( ExpressionData exp ) {
			var row = new DataGridViewRow();
			row.CreateCells( ExpressionDetailView );

			row.SetValues(
				exp.Enabled,
				exp.LeftOperand,
				exp.RightOperand,
				exp.Operator
				);

			return row;
		}


		private int GetSelectedRow( DataGridView dgv ) {
			return dgv.SelectedRows.Count == 0 ? -1 : dgv.SelectedRows[0].Index;
		}



		private void SetExpressionSetter( string left, object right = null ) {

			Type lefttype = ExpressionData.GetLeftOperandType( left );

			bool isenumerable = lefttype != null && lefttype != typeof( string ) && lefttype.GetInterface( "IEnumerable" ) != null;
			if ( isenumerable )
				lefttype = lefttype.GetElementType() ?? lefttype.GetGenericArguments().First();

			// 特殊判定(決め打ち)シリーズ
			if ( left == ".MasterShip.ShipType" ) {
				RightOperand_ComboBox.Visible = true;
				RightOperand_ComboBox.Enabled = true;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_bool;

				RightOperand_ComboBox.DataSource = _dtRightOperand_shiptype;
				RightOperand_ComboBox.SelectedValue = right == null ? 2 : right;

			} else if ( left.Contains( "SlotMaster" ) ) {
				RightOperand_ComboBox.Visible = true;
				RightOperand_ComboBox.Enabled = true;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_bool;

				RightOperand_ComboBox.DataSource = _dtRightOperand_equipment;
				RightOperand_ComboBox.SelectedValue = right == null ? 1 : right;


			// 以下、汎用判定
			} else if ( lefttype == null ) {
				RightOperand_ComboBox.Visible = false;
				RightOperand_ComboBox.Enabled = false;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = true;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = false;
				Operator.DataSource = _dtOperator;

				RightOperand_TextBox.Text = right == null ? "" : right.ToString();

			} else if ( lefttype == typeof( int ) ) {
				RightOperand_ComboBox.Visible = false;
				RightOperand_ComboBox.Enabled = false;
				RightOperand_NumericUpDown.Visible = true;
				RightOperand_NumericUpDown.Enabled = true;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_number;

				RightOperand_NumericUpDown.Maximum = int.MaxValue;
				RightOperand_NumericUpDown.Minimum = int.MinValue;
				RightOperand_NumericUpDown.Value = right == null ? 0 : (int)right;

			} else if ( lefttype == typeof( double ) ) {
				RightOperand_ComboBox.Visible = false;
				RightOperand_ComboBox.Enabled = false;
				RightOperand_NumericUpDown.Visible = true;
				RightOperand_NumericUpDown.Enabled = true;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_number;

				RightOperand_NumericUpDown.Maximum = int.MaxValue;		// int_min - int_max もあれば十分では
				RightOperand_NumericUpDown.Minimum = int.MinValue;		//fixme: 左辺値によってその有効な範囲は変化する、というか大体 0-1 で済む
				RightOperand_NumericUpDown.Value = right == null ? 0 : (decimal)right;

			} else if ( lefttype == typeof( bool ) ) {
				RightOperand_ComboBox.Visible = true;
				RightOperand_ComboBox.Enabled = true;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_bool;

				RightOperand_ComboBox.DataSource = _dtRightOperand_bool;
				RightOperand_ComboBox.SelectedValue = right == null ? true : right;

			} else if ( lefttype.IsEnum ) {
				RightOperand_ComboBox.Visible = true;
				RightOperand_ComboBox.Enabled = true;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_bool;

				DataTable dt = new DataTable();
				dt.Columns.AddRange( new DataColumn[]{ 
					new DataColumn( "Value", typeof( string ) ), 
					new DataColumn( "Display", typeof( string ) ) } );
				var names = lefttype.GetEnumNames();
				var values = lefttype.GetEnumValues();
				for ( int i = 0; i < names.Length; i++ )
					dt.Rows.Add( values.GetValue( i ), names[i] );
				RightOperand_ComboBox.DataSource = dt;
				RightOperand_ComboBox.SelectedValue = right;

			} else {
				RightOperand_ComboBox.Visible = false;
				RightOperand_ComboBox.Enabled = false;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = true;
				RightOperand_TextBox.Enabled = true;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_string;

				RightOperand_TextBox.Text = right == null ? "" : right.ToString();

			}


			if ( isenumerable ) {
				Operator.DataSource = _dtOperator_array;
			}

		}




		private void ExpressionView_SelectionChanged( object sender, EventArgs e ) {

			int index = ExpressionView.SelectedRows.Count == 0 ? -1 : ExpressionView.SelectedRows[0].Index;

			if ( index < 0 || _target.Expressions.Count <= index ) return;

			var ex = _target.Expressions[index];



			// detail の更新と expression の初期化

			ExpressionDetailView.Rows.Clear();

			var rows = new DataGridViewRow[ex.Expressions.Count];
			for ( int i = 0; i < rows.Length; i++ ) {
				rows[i] = GetExpressionDetailViewRow( ex.Expressions[i] );
			}
			ExpressionDetailView.Rows.AddRange( rows );

		}



		private void ExpressionDetailView_CellMouseClick( object sender, DataGridViewCellMouseEventArgs e ) {

			if ( e.RowIndex < 0 || e.ColumnIndex == ExpressionDetailView_Enabled.Index ) return;
			if ( ExpressionView.SelectedRows.Count == 0 || ExpressionView.SelectedRows[0].Index == -1 ) return;

			ExpressionData exp = _target[ExpressionView.SelectedRows[0].Index][e.RowIndex];
			Type lefttype = ExpressionData.GetLeftOperandType( exp.LeftOperand );

			SetExpressionSetter( exp.LeftOperand, exp.RightOperand );

		}






		private void Expression_Add_Click( object sender, EventArgs e ) {

			int insertrow = GetSelectedRow( ExpressionView );
			if ( insertrow == -1 ) insertrow = ExpressionView.Rows.Count;

			var exp = new ExpressionList();

			_target.Expressions.Insert( insertrow, exp );
			ExpressionView.Rows.Insert( insertrow, GetExpressionViewRow( exp ) );

		}

		private void Expression_Delete_Click( object sender, EventArgs e ) {

			int selectedrow = GetSelectedRow( ExpressionView );

			if ( selectedrow == -1 ) {
				MessageBox.Show( "対象となる行を選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Asterisk );
				return;
			}

			_target.Expressions.RemoveAt( selectedrow );
			ExpressionView.Rows.RemoveAt( selectedrow );

		}

		private void ExpressionDetail_Add_Click( object sender, EventArgs e ) {

			int procrow = GetSelectedRow( ExpressionView );
			if ( procrow == -1 ) {
				MessageBox.Show( "対象となる式列(左側)を選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Asterisk );
				return;
			}

			var exp = new ExpressionData();
			exp.LeftOperand = (string)LeftOperand.SelectedValue;
			exp.Operator = (ExpressionData.ExpressionOperator)Operator.SelectedValue;

			Type type = exp.GetLeftOperandType();
			if ( type != null && type != typeof ( string ) && type.GetInterface( "IEnumerable" ) != null )
				type = type.GetElementType() ?? type.GetGenericArguments().First();


			if ( RightOperand_ComboBox.Enabled ) {
				exp.RightOperand = Convert.ChangeType( RightOperand_ComboBox.SelectedValue, type );
			} else if ( RightOperand_NumericUpDown.Enabled ) {
				exp.RightOperand = Convert.ChangeType( RightOperand_NumericUpDown.Value, type );
			} else if ( RightOperand_TextBox.Enabled ) {
				exp.RightOperand = Convert.ChangeType( RightOperand_TextBox.Text, type );
			} else {
				exp.RightOperand = null;
			}


			_target.Expressions[procrow].Expressions.Add( exp );
			ExpressionDetailView.Rows.Add( GetExpressionDetailViewRow( exp ) );
		}


		private void ExpressionDetail_Delete_Click( object sender, EventArgs e ) {

			int procrow = GetSelectedRow( ExpressionView );
			if ( procrow == -1 ) {
				MessageBox.Show( "対象となる式列(左側)を選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Asterisk );
				return;
			}

			int selectedrow = GetSelectedRow( ExpressionDetailView );
			if ( selectedrow == -1 ) {
				MessageBox.Show( "対象となる行を選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Asterisk );
				return;
			}

			_target.Expressions[procrow].Expressions.RemoveAt( selectedrow );
			ExpressionDetailView.Rows.RemoveAt( selectedrow );

		}


		private void LeftOperand_SelectedValueChanged( object sender, EventArgs e ) {
			if ( LeftOperand.SelectedValue != null )
				SetExpressionSetter( (string)LeftOperand.SelectedValue );
		}




		// チェックボックスの更新を即時反映する
		private void ExpressionView_CurrentCellDirtyStateChanged( object sender, EventArgs e ) {

			if ( ExpressionView.Columns[ExpressionView.CurrentCellAddress.X] is DataGridViewCheckBoxColumn ) {
				if ( ExpressionView.IsCurrentCellDirty ) {
					ExpressionView.CommitEdit( DataGridViewDataErrorContexts.Commit );
				}
			}

		}


		private void ExpressionView_CellValueChanged( object sender, DataGridViewCellEventArgs e ) {

			if ( e.RowIndex < 0 ) return;

			if ( e.ColumnIndex == ExpressionView_Enabled.Index ) {
				_target[e.RowIndex].Enabled = (bool)ExpressionView[e.ColumnIndex, e.RowIndex].Value;

			} else if ( e.ColumnIndex == ExpressionView_ExternalAndOr.Index ) {
				_target[e.RowIndex].ExternalAnd = (bool)ExpressionView[e.ColumnIndex, e.RowIndex].Value;

			} else if ( e.ColumnIndex == ExpressionView_Inverse.Index ) {
				_target[e.RowIndex].Inverse = (bool)ExpressionView[e.ColumnIndex, e.RowIndex].Value;

			} else if ( e.ColumnIndex == ExpressionView_InternalAndOr.Index ) {
				_target[e.RowIndex].InternalAnd = (bool)ExpressionView[e.ColumnIndex, e.RowIndex].Value;

			}
		}

		private void ExpressionView_CellContentClick( object sender, DataGridViewCellEventArgs e ) {

			if ( e.RowIndex < 0 ) return;

			if ( e.ColumnIndex == ExpressionView_Up.Index && e.RowIndex > 0 ) {
				_target.Expressions.Insert( e.RowIndex - 1, _target[e.RowIndex] );
				_target.Expressions.RemoveAt( e.RowIndex + 1 );
				ExpressionView.Rows.Insert( e.RowIndex + 1, GetExpressionViewRow( _target[e.RowIndex] ) );
				ExpressionView.Rows.RemoveAt( e.RowIndex - 1 );

			} else if ( e.ColumnIndex == ExpressionView_Down.Index && e.RowIndex < ExpressionView.Rows.Count - 1 ) {
				_target.Expressions.Insert( e.RowIndex + 2, _target[e.RowIndex] );
				_target.Expressions.RemoveAt( e.RowIndex );
				ExpressionView.Rows.Insert( e.RowIndex, GetExpressionViewRow( _target[e.RowIndex] ) );
				ExpressionView.Rows.RemoveAt( e.RowIndex + 2 );
			}

		}






		private void ButtonOK_Click( object sender, EventArgs e ) {

			DialogResult = System.Windows.Forms.DialogResult.OK;

		}

		private void ButtonCancel_Click( object sender, EventArgs e ) {

			DialogResult = System.Windows.Forms.DialogResult.Cancel;

		}

	




	}
}
