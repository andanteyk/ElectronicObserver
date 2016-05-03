using ElectronicObserver.Data;
using ElectronicObserver.Data.ShipGroup;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Support;
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
	public partial class DialogShipGroupFilter : Form {

		private ShipGroupData _group;


		#region DataTable
		private DataTable _dtAndOr;
		private DataTable _dtLeftOperand;
		private DataTable _dtOperator;
		private DataTable _dtOperator_bool;
		private DataTable _dtOperator_number;
		private DataTable _dtOperator_string;
		private DataTable _dtOperator_array;
		private DataTable _dtRightOperand_bool;
		private DataTable _dtRightOperand_shipname;
		private DataTable _dtRightOperand_shiptype;
		private DataTable _dtRightOperand_range;
		private DataTable _dtRightOperand_speed;
		private DataTable _dtRightOperand_rarity;
		private DataTable _dtRightOperand_equipment;
		#endregion

		public DialogShipGroupFilter( ShipGroupData group ) {
			InitializeComponent();

			{
				// 一部の列ヘッダを中央揃えにする
				var headercenter = new DataGridViewCellStyle( ExpressionView_Enabled.HeaderCell.Style );
				headercenter.Alignment = DataGridViewContentAlignment.MiddleCenter;
				ExpressionView_Enabled.HeaderCell.Style =
				ExpressionView_InternalAndOr.HeaderCell.Style =
				ExpressionView_ExternalAndOr.HeaderCell.Style =
				ExpressionView_Inverse.HeaderCell.Style =
				ExpressionView_Up.HeaderCell.Style =
				ExpressionView_Down.HeaderCell.Style =
				ExpressionDetailView_Enabled.HeaderCell.Style =
				ConstFilterView_Up.HeaderCell.Style =
				ConstFilterView_Down.HeaderCell.Style =
				ConstFilterView_Delete.HeaderCell.Style =
				headercenter;
			}


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
				_dtRightOperand_shipname = new DataTable();
				_dtRightOperand_shipname.Columns.AddRange( new DataColumn[]{ 
					new DataColumn( "Value", typeof( int ) ), 
					new DataColumn( "Display", typeof( string ) ) } );
				foreach ( var s in KCDatabase.Instance.MasterShips.Values.Where( s => !s.IsAbyssalShip ).OrderBy( s => s.NameWithClass ).OrderBy( s => s.NameReading ) )
					_dtRightOperand_shipname.Rows.Add( s.ShipID, s.Name );
				_dtRightOperand_shipname.AcceptChanges();
			}
			{
				_dtRightOperand_shiptype = new DataTable();
				_dtRightOperand_shiptype.Columns.AddRange( new DataColumn[]{ 
					new DataColumn( "Value", typeof( int ) ), 
					new DataColumn( "Display", typeof( string ) ) } );
				foreach ( var st in KCDatabase.Instance.MasterShips.Values
					.Where( s => !s.IsAbyssalShip )
					.Select( s => s.ShipType )
					.Distinct()
					.OrderBy( i => i )
					.Select( i => KCDatabase.Instance.ShipTypes[i] ) )
					_dtRightOperand_shiptype.Rows.Add( st.TypeID, st.Name );
				_dtRightOperand_shiptype.AcceptChanges();
			}
			{
				_dtRightOperand_range = new DataTable();
				_dtRightOperand_range.Columns.AddRange( new DataColumn[]{ 
					new DataColumn( "Value", typeof( int ) ), 
					new DataColumn( "Display", typeof( string ) ) } );
				for ( int i = 0; i <= 4; i++ )
					_dtRightOperand_range.Rows.Add( i, Constants.GetRange( i ) );
				_dtRightOperand_range.AcceptChanges();
			}
			{
				_dtRightOperand_speed = new DataTable();
				_dtRightOperand_speed.Columns.AddRange( new DataColumn[]{ 
					new DataColumn( "Value", typeof( int ) ), 
					new DataColumn( "Display", typeof( string ) ) } );
				_dtRightOperand_speed.Rows.Add( 0, Constants.GetSpeed( 0 ) );
				_dtRightOperand_speed.Rows.Add( 5, Constants.GetSpeed( 5 ) );
				_dtRightOperand_speed.Rows.Add( 10, Constants.GetSpeed( 10 ) );
				_dtRightOperand_speed.AcceptChanges();
			}
			{
				_dtRightOperand_rarity = new DataTable();
				_dtRightOperand_rarity.Columns.AddRange( new DataColumn[]{ 
					new DataColumn( "Value", typeof( int ) ), 
					new DataColumn( "Display", typeof( string ) ) } );
				for ( int i = 1; i <= 8; i++ )
					_dtRightOperand_rarity.Rows.Add( i, Constants.GetShipRarity( i ) );
				_dtRightOperand_rarity.AcceptChanges();
			}
			{
				_dtRightOperand_equipment = new DataTable();
				_dtRightOperand_equipment.Columns.AddRange( new DataColumn[]{ 
					new DataColumn( "Value", typeof( int ) ), 
					new DataColumn( "Display", typeof( string ) ) } );
				_dtRightOperand_equipment.Rows.Add( -1, "(なし)" );
				foreach ( var eq in KCDatabase.Instance.MasterEquipments.Values.Where( eq => !eq.IsAbyssalEquipment ).OrderBy( eq => eq.CategoryType ) )
					_dtRightOperand_equipment.Rows.Add( eq.EquipmentID, eq.Name );
				_dtRightOperand_equipment.Rows.Add( 0, "(未開放)" );
				_dtRightOperand_equipment.AcceptChanges();
			}

			RightOperand_ComboBox.ValueMember = "Value";
			RightOperand_ComboBox.DisplayMember = "Display";
			RightOperand_ComboBox.DataSource = _dtRightOperand_bool;

			SetExpressionSetter( ExpressionData.LeftOperandNameTable.Keys.First() );

			#endregion


			ConstFilterSelector.SelectedIndex = 0;

			ImportGroupData( group );
		}

		private void DialogShipGroupFilter_Load( object sender, EventArgs e ) {
			if ( Owner != null )
				Icon = Owner.Icon;
		}



		/// <summary>
		/// グループデータをコピーし、UIを初期化します。
		/// </summary>
		/// <param name="group">対象となるグループ。コピーされるためこのインスタンスには変更は適用されません。</param>
		public void ImportGroupData( ShipGroupData group ) {

			_group = group.Clone();

			UpdateExpressionView();
			UpdateConstFilterView();
		}


		/// <summary>
		/// 編集したグループデータを出力します。
		/// </summary>
		public ShipGroupData ExportGroupData() {

			return _group;
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



		private void UpdateExpressionView() {

			ExpressionView.Rows.Clear();


			var rows = new DataGridViewRow[_group.Expressions.Expressions.Count];
			for ( int i = 0; i < rows.Length; i++ ) {
				rows[i] = GetExpressionViewRow( _group.Expressions.Expressions[i] );
			}

			ExpressionView.Rows.AddRange( rows.ToArray() );

			ExpressionDetailView.Rows.Clear();

			LabelResult.Tag = false;
			UpdateExpressionLabel();

		}


		/// <summary>
		/// 包含/除外フィルタの表示を更新します。
		/// </summary>
		private void UpdateConstFilterView() {

			List<int> values = ConstFilterSelector.SelectedIndex == 0 ? _group.InclusionFilter : _group.ExclusionFilter;

			ConstFilterView.Rows.Clear();

			var rows = new DataGridViewRow[values.Count];
			for ( int i = 0; i < rows.Length; i++ ) {
				rows[i] = new DataGridViewRow();
				rows[i].CreateCells( ConstFilterView );

				var ship = KCDatabase.Instance.Ships[values[i]];
				rows[i].SetValues( values[i], ship == null ? "(未在籍)" : ship.NameWithLevel );
			}

			ConstFilterView.Rows.AddRange( rows );

		}

		private List<int> GetConstFilterFromUI() {
			return ConstFilterSelector.SelectedIndex == 0 ? _group.InclusionFilter : _group.ExclusionFilter;
		}


		/// <summary>
		/// 指定された式から、式UIを初期化します。
		/// </summary>
		/// <param name="left">左辺値。</param>
		/// <param name="right">右辺値。指定しなければ null。</param>
		/// <param name="ope">演算子。指定しなければ null。</param>
		private void SetExpressionSetter( string left, object right = null, ExpressionData.ExpressionOperator? ope = null ) {

			Type lefttype = ExpressionData.GetLeftOperandType( left );

			bool isenumerable = lefttype != null && lefttype != typeof( string ) && lefttype.GetInterface( "IEnumerable" ) != null;
			if ( isenumerable )
				lefttype = lefttype.GetElementType() ?? lefttype.GetGenericArguments().First();

			Description.Text = "";

			LeftOperand.SelectedValue = left;

			// 特殊判定(決め打ち)シリーズ
			if ( left == ".MasterShip.NameWithClass" ) {
				RightOperand_ComboBox.Visible = true;
				RightOperand_ComboBox.Enabled = true;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_string;

				RightOperand_ComboBox.DataSource = _dtRightOperand_shipname;
				RightOperand_ComboBox.Text = (string)( right ?? _dtRightOperand_shipname.AsEnumerable().First()["Display"] );

			} else if ( left == ".MasterShip.ShipType" ) {
				RightOperand_ComboBox.Visible = true;
				RightOperand_ComboBox.Enabled = true;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
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
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_bool;

				RightOperand_ComboBox.DataSource = _dtRightOperand_equipment;
				RightOperand_ComboBox.SelectedValue = right == null ? 1 : right;

			} else if ( left == ".Range" ) {
				RightOperand_ComboBox.Visible = true;
				RightOperand_ComboBox.Enabled = true;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_number;

				RightOperand_ComboBox.DataSource = _dtRightOperand_range;
				RightOperand_ComboBox.SelectedValue = right == null ? 1 : right;

			} else if ( left == ".MasterShip.Speed" ) {
				RightOperand_ComboBox.Visible = true;
				RightOperand_ComboBox.Enabled = true;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_number;

				RightOperand_ComboBox.DataSource = _dtRightOperand_speed;
				RightOperand_ComboBox.SelectedValue = right == null ? 10 : right;

			} else if ( left == ".MasterShip.Rarity" ) {
				RightOperand_ComboBox.Visible = true;
				RightOperand_ComboBox.Enabled = true;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				RightOperand_NumericUpDown.Visible = false;
				RightOperand_NumericUpDown.Enabled = false;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_number;

				RightOperand_ComboBox.DataSource = _dtRightOperand_rarity;
				RightOperand_ComboBox.SelectedValue = right == null ? 1 : right;


				// 以下、汎用判定
			} else if ( lefttype == null ) {
				RightOperand_ComboBox.Visible = false;
				RightOperand_ComboBox.Enabled = false;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
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
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				RightOperand_NumericUpDown.Visible = true;
				RightOperand_NumericUpDown.Enabled = true;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_number;

				RightOperand_NumericUpDown.DecimalPlaces = 0;
				RightOperand_NumericUpDown.Increment = 1m;

				switch ( left ) {
					case ".MasterID":
						RightOperand_NumericUpDown.Minimum = 0;
						RightOperand_NumericUpDown.Maximum = 999999;
						break;
					case ".Level":
						RightOperand_NumericUpDown.Minimum = 1;
						RightOperand_NumericUpDown.Maximum = ExpTable.ShipMaximumLevel;
						break;
					case ".ExpTotal":
					case ".ExpNextRemodel":
						RightOperand_NumericUpDown.Minimum = 0;
						RightOperand_NumericUpDown.Maximum = 4360000;
						break;
					case ".ExpNext":
						RightOperand_NumericUpDown.Minimum = 0;
						RightOperand_NumericUpDown.Maximum = 195000;
						break;
					case ".HPCurrent":
					case ".HPMax":
						RightOperand_NumericUpDown.Minimum = 0;
						RightOperand_NumericUpDown.Maximum = 999;
						break;
					case ".Condition":
						RightOperand_NumericUpDown.Minimum = 0;
						RightOperand_NumericUpDown.Maximum = 100;
						break;
					case ".RepairingDockID":
						RightOperand_NumericUpDown.Minimum = -1;
						RightOperand_NumericUpDown.Maximum = 4;
						Description.Text = "-1=未入渠, 1～4=入渠中(ドック番号)";
						break;
					case ".RepairTime":
						RightOperand_NumericUpDown.Minimum = 0;
						RightOperand_NumericUpDown.Maximum = int.MaxValue;
						RightOperand_NumericUpDown.Increment = 60000;
						break;
					case ".SlotSize":
						RightOperand_NumericUpDown.Minimum = 0;
						RightOperand_NumericUpDown.Maximum = 5;
						break;
					default:
						RightOperand_NumericUpDown.Minimum = 0;
						RightOperand_NumericUpDown.Maximum = 9999;
						break;
				}
				RightOperand_NumericUpDown.Value = right == null ? RightOperand_NumericUpDown.Minimum : (int)right;
				UpdateDescriptionFromNumericUpDown();

			} else if ( lefttype == typeof( double ) ) {
				RightOperand_ComboBox.Visible = false;
				RightOperand_ComboBox.Enabled = false;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				RightOperand_NumericUpDown.Visible = true;
				RightOperand_NumericUpDown.Enabled = true;
				RightOperand_TextBox.Visible = false;
				RightOperand_TextBox.Enabled = false;
				Operator.Enabled = true;
				Operator.DataSource = _dtOperator_number;

				switch ( left ) {
					case ".HPRate":
					case ".AircraftRate[0]":
					case ".AircraftRate[1]":
					case ".AircraftRate[2]":
					case ".AircraftRate[3]":
					case ".AircraftRate[4]":
					case ".AircraftTotalRate":
					case ".FuelRate":
					case ".AmmoRate":
						RightOperand_NumericUpDown.Minimum = 0;
						RightOperand_NumericUpDown.Maximum = 1;
						RightOperand_NumericUpDown.DecimalPlaces = 2;
						RightOperand_NumericUpDown.Increment = 0.01m;
						break;
					default:
						RightOperand_NumericUpDown.Maximum = int.MaxValue;
						RightOperand_NumericUpDown.Minimum = int.MinValue;
						RightOperand_NumericUpDown.DecimalPlaces = 0;
						RightOperand_NumericUpDown.Increment = 1m;
						break;
				}
				RightOperand_NumericUpDown.Value = right == null ? RightOperand_NumericUpDown.Minimum : Convert.ToDecimal( right );
				UpdateDescriptionFromNumericUpDown();

			} else if ( lefttype == typeof( bool ) ) {
				RightOperand_ComboBox.Visible = true;
				RightOperand_ComboBox.Enabled = true;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
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
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
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
				dt.AcceptChanges();
				RightOperand_ComboBox.DataSource = dt;
				RightOperand_ComboBox.SelectedValue = right;

			} else {
				RightOperand_ComboBox.Visible = false;
				RightOperand_ComboBox.Enabled = false;
				RightOperand_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
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


			if ( Operator.DataSource as DataTable != null ) {
				if ( ope == null ) {
					Operator.SelectedValue = ( (DataTable)Operator.DataSource ).AsEnumerable().First()["Value"];
				} else {
					Operator.SelectedValue = (ExpressionData.ExpressionOperator)ope;
				}
			}
		}



		/// <summary>
		/// 選択された行をもとに、 ExpressionDetailView を更新します。
		/// </summary>
		/// <param name="index">対象となる行のインデックス。</param>
		private void UpdateExpressionDetailView( int index ) {

			if ( index < 0 || _group.Expressions.Expressions.Count <= index ) return;

			var ex = _group.Expressions.Expressions[index];


			// detail の更新と expression の初期化

			ExpressionDetailView.Rows.Clear();

			var rows = new DataGridViewRow[ex.Expressions.Count];
			for ( int i = 0; i < rows.Length; i++ ) {
				rows[i] = GetExpressionDetailViewRow( ex.Expressions[i] );
			}

			ExpressionDetailView.Rows.AddRange( rows );
		}


		// 選択を基にUIの更新
		private void ExpressionView_SelectionChanged( object sender, EventArgs e ) {

			UpdateExpressionDetailView( ExpressionView.SelectedRows.Count == 0 ? -1 : ExpressionView.SelectedRows[0].Index );

		}

		private void ExpressionDetailView_SelectionChanged( object sender, EventArgs e ) {

			int index = ExpressionView.SelectedRows.Count == 0 ? -1 : ExpressionView.SelectedRows[0].Index;
			int detailIndex = ExpressionDetailView.SelectedRows.Count == 0 ? -1 : ExpressionDetailView.SelectedRows[0].Index;

			if ( index < 0 || _group.Expressions.Expressions.Count <= index ||
				detailIndex < 0 || _group.Expressions[index].Expressions.Count <= detailIndex ) return;

			ExpressionData exp = _group.Expressions[index][detailIndex];

			SetExpressionSetter( exp.LeftOperand, exp.RightOperand, exp.Operator );

		}




		// Expression のボタン操作
		private void Expression_Add_Click( object sender, EventArgs e ) {

			int insertrow = GetSelectedRow( ExpressionView );
			if ( insertrow == -1 ) insertrow = ExpressionView.Rows.Count - 1;

			var exp = new ExpressionList();

			_group.Expressions.Expressions.Insert( insertrow + 1, exp );
			ExpressionView.Rows.Insert( insertrow + 1, GetExpressionViewRow( exp ) );

			ExpressionUpdated();
		}

		private void Expression_Delete_Click( object sender, EventArgs e ) {

			int selectedrow = GetSelectedRow( ExpressionView );

			if ( selectedrow == -1 ) {
				MessageBox.Show( "対象となる行を選択してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Asterisk );
				return;
			}

			ExpressionDetailView.Rows.Clear();

			_group.Expressions.Expressions.RemoveAt( selectedrow );
			ExpressionView.Rows.RemoveAt( selectedrow );


			ExpressionUpdated();
		}


		private void ButtonOK_Click( object sender, EventArgs e ) {

			DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void ButtonCancel_Click( object sender, EventArgs e ) {

			DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}




		/// <summary>
		/// UIの設定値からExpressionDataを構築します。
		/// </summary>
		private ExpressionData BuildExpressionDataFromUI() {

			var exp = new ExpressionData();
			exp.LeftOperand = (string)LeftOperand.SelectedValue ?? LeftOperand.Text;
			exp.Operator = (ExpressionData.ExpressionOperator)Operator.SelectedValue;

			Type type = exp.GetLeftOperandType();
			if ( type != null && type != typeof( string ) && type.GetInterface( "IEnumerable" ) != null )
				type = type.GetElementType() ?? type.GetGenericArguments().First();


			if ( RightOperand_ComboBox.Enabled ) {
				if ( RightOperand_ComboBox.DropDownStyle == ComboBoxStyle.DropDownList )
					exp.RightOperand = Convert.ChangeType( RightOperand_ComboBox.SelectedValue ?? RightOperand_ComboBox.Text, type );
				else
					exp.RightOperand = Convert.ChangeType( RightOperand_ComboBox.Text, type );

			} else if ( RightOperand_NumericUpDown.Enabled ) {
				exp.RightOperand = Convert.ChangeType( RightOperand_NumericUpDown.Value, type );

			} else if ( RightOperand_TextBox.Enabled ) {
				exp.RightOperand = Convert.ChangeType( RightOperand_TextBox.Text, type );

			} else {
				exp.RightOperand = null;
			}

			return exp;
		}



		// ExpressionDetail のボタン操作
		private void ExpressionDetail_Add_Click( object sender, EventArgs e ) {

			int procrow = GetSelectedRow( ExpressionView );
			if ( procrow == -1 ) {
				MessageBox.Show( "対象となる式(左側)の行を選択してください。\r\n行が存在しない場合は追加してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Asterisk );
				return;
			}

			var exp = BuildExpressionDataFromUI();

			_group.Expressions.Expressions[procrow].Expressions.Add( exp );
			ExpressionDetailView.Rows.Add( GetExpressionDetailViewRow( exp ) );

			UpdateExpressionViewRow( procrow );
		}


		private void ExpressionDetail_Edit_Click( object sender, EventArgs e ) {

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

			var exp = BuildExpressionDataFromUI();

			_group.Expressions.Expressions[procrow].Expressions[selectedrow] = exp;
			ExpressionDetailView.Rows.Insert( selectedrow, GetExpressionDetailViewRow( exp ) );
			ExpressionDetailView.Rows.RemoveAt( selectedrow + 1 );

			UpdateExpressionViewRow( procrow );
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

			_group.Expressions.Expressions[procrow].Expressions.RemoveAt( selectedrow );
			ExpressionDetailView.Rows.RemoveAt( selectedrow );

			UpdateExpressionViewRow( procrow );
		}


		// 左辺値変更時のUI変更
		private void LeftOperand_SelectedValueChanged( object sender, EventArgs e ) {
			SetExpressionSetter( (string)LeftOperand.SelectedValue ?? LeftOperand.Text );
		}




		// チェックボックスの更新を即時反映する
		// コンボボックスも可能だけど今回は省略
		private void ExpressionView_CurrentCellDirtyStateChanged( object sender, EventArgs e ) {

			if ( ExpressionView.Columns[ExpressionView.CurrentCellAddress.X] is DataGridViewCheckBoxColumn ) {
				if ( ExpressionView.IsCurrentCellDirty ) {
					ExpressionView.CommitEdit( DataGridViewDataErrorContexts.Commit );
				}
			}

		}

		private void ExpressionDetailView_CurrentCellDirtyStateChanged( object sender, EventArgs e ) {

			if ( ExpressionDetailView.Columns[ExpressionDetailView.CurrentCellAddress.X] is DataGridViewCheckBoxColumn ) {
				if ( ExpressionDetailView.IsCurrentCellDirty ) {
					ExpressionDetailView.CommitEdit( DataGridViewDataErrorContexts.Commit );
				}
			}
		}


		// UI 操作(チェックボックス/コンボボックス)の反映
		private void ExpressionView_CellValueChanged( object sender, DataGridViewCellEventArgs e ) {

			if ( e.RowIndex < 0 ) return;

			if ( e.ColumnIndex == ExpressionView_Enabled.Index ) {
				_group.Expressions[e.RowIndex].Enabled = (bool)ExpressionView[e.ColumnIndex, e.RowIndex].Value;

			} else if ( e.ColumnIndex == ExpressionView_ExternalAndOr.Index ) {
				_group.Expressions[e.RowIndex].ExternalAnd = (bool)ExpressionView[e.ColumnIndex, e.RowIndex].Value;

			} else if ( e.ColumnIndex == ExpressionView_Inverse.Index ) {
				_group.Expressions[e.RowIndex].Inverse = (bool)ExpressionView[e.ColumnIndex, e.RowIndex].Value;

			} else if ( e.ColumnIndex == ExpressionView_InternalAndOr.Index ) {
				_group.Expressions[e.RowIndex].InternalAnd = (bool)ExpressionView[e.ColumnIndex, e.RowIndex].Value;

			}

			UpdateExpressionViewRow( e.RowIndex );
		}

		private void ExpressionDetailView_CellValueChanged( object sender, DataGridViewCellEventArgs e ) {

			if ( e.RowIndex < 0 ) return;

			int procrow = GetSelectedRow( ExpressionView );
			if ( procrow == -1 ) {
				return;
			}

			if ( e.ColumnIndex == ExpressionDetailView_Enabled.Index ) {
				_group.Expressions[procrow].Expressions[e.RowIndex].Enabled = (bool)ExpressionDetailView[e.ColumnIndex, e.RowIndex].Value;
			}

			UpdateExpressionViewRow( procrow );
		}


		/// <summary>
		/// ExpressionView の指定された行の式表示を更新します。
		/// </summary>
		/// <param name="index">行インデックス。</param>
		private void UpdateExpressionViewRow( int index ) {
			ExpressionView[ExpressionView_Expression.Index, index].Value = _group.Expressions[index].ToString();
			ExpressionUpdated();
		}

		/// <summary>
		/// 式が更新されたときの動作を行います。
		/// </summary>
		private void ExpressionUpdated() {
			UpdateExpressionLabel();
		}

		private void UpdateExpressionLabel() {
			if ( LabelResult.Tag != null && (bool)LabelResult.Tag ) {
				_group.Expressions.Compile();
				LabelResult.Text = _group.Expressions.ToExpressionString();
			} else {
				LabelResult.Text = _group.Expressions.ToString();
			}
		}



		// ボタン処理
		private void ExpressionView_CellContentClick( object sender, DataGridViewCellEventArgs e ) {

			if ( e.RowIndex < 0 ) return;

			// fixme: 非選択セルで上下させると選択がちょっとちらつく  :(

			if ( e.ColumnIndex == ExpressionView_Up.Index && e.RowIndex > 0 ) {
				_group.Expressions.Expressions.Insert( e.RowIndex - 1, _group.Expressions[e.RowIndex] );
				_group.Expressions.Expressions.RemoveAt( e.RowIndex + 1 );

				ControlHelper.RowMoveUp( ExpressionView, e.RowIndex );
				ExpressionView.Rows[e.RowIndex - 1].Selected = true;

				ExpressionUpdated();


			} else if ( e.ColumnIndex == ExpressionView_Down.Index && e.RowIndex < ExpressionView.Rows.Count - 1 ) {
				_group.Expressions.Expressions.Insert( e.RowIndex + 2, _group.Expressions[e.RowIndex] );
				_group.Expressions.Expressions.RemoveAt( e.RowIndex );

				ControlHelper.RowMoveDown( ExpressionView, e.RowIndex );
				ExpressionView.Rows[e.RowIndex + 1].Selected = true;

				ExpressionUpdated();

			}


			if ( ExpressionView.SelectedRows.Count > 0 )
				UpdateExpressionDetailView( ExpressionView.SelectedRows[0].Index );
		}

		private void ConstFilterView_CellContentClick( object sender, DataGridViewCellEventArgs e ) {

			// 上下動に意味があるかはおいておいて
			if ( e.ColumnIndex == ConstFilterView_Up.Index && e.RowIndex > 0 ) {
				var list = GetConstFilterFromUI();
				list.Insert( e.RowIndex - 1, list[e.RowIndex] );
				list.RemoveAt( e.RowIndex + 1 );

				ControlHelper.RowMoveUp( ConstFilterView, e.RowIndex );

			} else if ( e.ColumnIndex == ConstFilterView_Down.Index && e.RowIndex < ConstFilterView.Rows.Count - 1 ) {
				var list = GetConstFilterFromUI();
				list.Insert( e.RowIndex + 2, list[e.RowIndex] );
				list.RemoveAt( e.RowIndex );

				ControlHelper.RowMoveDown( ConstFilterView, e.RowIndex );

			} else if ( e.ColumnIndex == ConstFilterView_Delete.Index && e.RowIndex >= 0 ) {
				var list = GetConstFilterFromUI();
				list.RemoveAt( e.RowIndex );

				ConstFilterView.Rows.RemoveAt( e.RowIndex );
			}

		}


		// コンボボックスの即選択
		private void ExpressionView_CellClick( object sender, DataGridViewCellEventArgs e ) {

			if ( e.RowIndex < 0 ) return;

			if ( ExpressionView.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn ) {
				ExpressionView.BeginEdit( false );
				var edit = ExpressionView.EditingControl as DataGridViewComboBoxEditingControl;
				edit.DroppedDown = true;
			}

		}


		private void ExpressionDetailView_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e ) {

			if ( e.RowIndex < 0 ) return;

			int procrow = GetSelectedRow( ExpressionView );
			if ( procrow < 0 || procrow >= _group.Expressions.Expressions.Count ||
				e.RowIndex >= _group.Expressions[procrow].Expressions.Count ) {
				return;
			}

			if ( e.ColumnIndex == ExpressionDetailView_LeftOperand.Index ) {
				e.Value = _group.Expressions[procrow].Expressions[e.RowIndex].LeftOperandToString();
				e.FormattingApplied = true;

			} else if ( e.ColumnIndex == ExpressionDetailView_Operator.Index ) {
				e.Value = _group.Expressions[procrow].Expressions[e.RowIndex].OperatorToString();
				e.FormattingApplied = true;

			} else if ( e.ColumnIndex == ExpressionDetailView_RightOperand.Index ) {
				e.Value = _group.Expressions[procrow].Expressions[e.RowIndex].RightOperandToString();
				e.FormattingApplied = true;
			}

		}



		// Description の変更
		private void RightOperand_NumericUpDown_ValueChanged( object sender, EventArgs e ) {

			UpdateDescriptionFromNumericUpDown();
		}


		private void UpdateDescriptionFromNumericUpDown() {

			string left =  ( (string)LeftOperand.SelectedValue ) ?? LeftOperand.Text;
			int intvalue = (int)RightOperand_NumericUpDown.Value;

			switch ( left ) {
				case ".MasterID": {
						var ship = KCDatabase.Instance.Ships[intvalue];
						if ( ship != null ) {
							Description.Text = ship.NameWithLevel;
						} else {
							Description.Text = "(未在籍)";
						}
					} break;

				case ".ShipID": {
						var ship = KCDatabase.Instance.MasterShips[intvalue];
						if ( ship != null ) {
							Description.Text = ship.ShipTypeName + " " + ship.NameWithClass;
						} else {
							Description.Text = "(存在せず)";
						}
					} break;

				case ".RepairTime": {
						Description.Text = string.Format( "(ミリ秒単位) {0}", DateTimeHelper.ToTimeRemainString( DateTimeHelper.FromAPITimeSpan( intvalue ) ) );
					} break;

				case ".MasterShip.AlbumNo": {
						var ship = KCDatabase.Instance.MasterShips.Values.FirstOrDefault( s => s.AlbumNo == intvalue );
						if ( ship == null )
							Description.Text = "(存在せず)";
						else
							Description.Text = ship.ShipTypeName + " " + ship.NameWithClass;

					} break;

				case ".MasterShip.RemodelBeforeShipID": {
						if ( intvalue == 0 ) {
							Description.Text = "(未改装)";
						} else {
							var ship = KCDatabase.Instance.MasterShips[intvalue];
							if ( ship == null )
								Description.Text = "(存在せず)";
							else {
								var before = ship.RemodelBeforeShip;
								Description.Text = ship.NameWithClass + " ← " + ( before == null ? "×" : before.NameWithClass );
							}
						}
					} break;

				case ".MasterShip.RemodelAfterShipID": {
						if ( intvalue == 0 ) {
							Description.Text = "(最終改装)";
						} else {
							var ship = KCDatabase.Instance.MasterShips[intvalue];
							if ( ship == null )
								Description.Text = "(存在せず)";
							else {
								var after = ship.RemodelAfterShip;
								Description.Text = ship.NameWithClass + " → " + ( after == null ? "×" : after.NameWithClass );
							}
						}
					} break;
			}

			if ( left.Contains( "Rate" ) ) {
				Description.Text = RightOperand_NumericUpDown.Value.ToString( "P0" );
			}

		}

		private void LabelResult_Click( object sender, EventArgs e ) {
			LabelResult.Tag = !(bool)LabelResult.Tag;
			UpdateExpressionLabel();
		}





		// ConstFilter 関連
		private void ConstFilterSelector_SelectedIndexChanged( object sender, EventArgs e ) {

			if ( _group != null ) {
				UpdateConstFilterView();
			}

		}

		private void OptimizeConstFilter_Click( object sender, EventArgs e ) {

			if ( ConstFilterSelector.SelectedIndex == 0 ) {

				_group.InclusionFilter = _group.InclusionFilter.Intersect( KCDatabase.Instance.Ships.Keys ).ToList();

			} else {

				_group.ExclusionFilter = _group.ExclusionFilter.Intersect( KCDatabase.Instance.Ships.Keys ).ToList();
			}

			UpdateConstFilterView();
		}

		private void ClearConstFilter_Click( object sender, EventArgs e ) {

			if ( MessageBox.Show( ConstFilterSelector.Text + " を初期化します。\r\nよろしいですか?", "初期化の確認",
				MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2 )
				== System.Windows.Forms.DialogResult.Yes ) {

				if ( ConstFilterSelector.SelectedIndex == 0 ) {
					_group.InclusionFilter.Clear();

				} else {
					_group.ExclusionFilter.Clear();
				}

				UpdateConstFilterView();
			}
		}

		private void ConvertToExpression_Click( object sender, EventArgs e ) {

			if ( MessageBox.Show( "現在の包含/除外リストを式に変換します。\r\n逆変換はできません。\r\nよろしいですか？", "確認",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1 )
					== System.Windows.Forms.DialogResult.Yes ) {

				if ( _group.InclusionFilter.Count > 0 ) {
					_group.Expressions.Expressions.Add( new ExpressionList( false, false, false ) );
					var exlist = _group.Expressions.Expressions.Last();
					foreach ( var id in _group.InclusionFilter ) {
						exlist.Expressions.Add( new ExpressionData( ".MasterID", ExpressionData.ExpressionOperator.Equal, id ) );
					}
					_group.InclusionFilter.Clear();
				}
				if ( _group.ExclusionFilter.Count > 0 ) {
					_group.Expressions.Expressions.Add( new ExpressionList( false, true, true ) );
					var exlist = _group.Expressions.Expressions.Last();

					foreach ( var id in _group.ExclusionFilter ) {
						exlist.Expressions.Add( new ExpressionData( ".MasterID", ExpressionData.ExpressionOperator.Equal, id ) );
					}
					_group.ExclusionFilter.Clear();
				}


				UpdateExpressionView();
				UpdateConstFilterView();

			}
		}


		private void ButtonMenu_Click( object sender, EventArgs e ) {
			SubMenu.Show( ButtonMenu, ButtonMenu.Width / 2, ButtonMenu.Height / 2 );
		}

		private void Menu_ImportFilter_Click( object sender, EventArgs e ) {


			if ( MessageBox.Show( "クリップボードからフィルタをインポートします。\r\n現在のフィルタは破棄されます。(包含/除外フィルタは維持されます)\r\nよろしいですか？\r\n",
					"フィルタのインポートの確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question )
				== System.Windows.Forms.DialogResult.No )
				return;

			string data = Clipboard.GetText();

			if ( string.IsNullOrEmpty( data ) ) {
				MessageBox.Show( "クリップボードが空です。\r\nフィルタデータをコピーしたうえで再度選択してください。\r\n",
					"インポートできません", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try {

				using ( var str = new StringReader( data ) ) {
					var exp = (ExpressionManager)_group.Expressions.Load( str );
					if ( exp == null )
						throw new ArgumentException( "インポートできないデータ形式です。" );
					else
						_group.Expressions = exp;
				}

				UpdateExpressionView();

			} catch ( Exception ex ) {

				MessageBox.Show( "フィルタのインポートに失敗しました。\r\n" + ex.Message, "インポートできません", MessageBoxButtons.OK, MessageBoxIcon.Error );
			}

		}

		private void Menu_ExportFilter_Click( object sender, EventArgs e ) {

			try {

				StringBuilder str = new StringBuilder();
				_group.Expressions.Save( str );

				Clipboard.SetText( str.ToString() );

				MessageBox.Show( "フィルタをクリップボードにエクスポートしました。\r\n「フィルタのインポート」で取り込んだり、\r\nメモ帳等に貼り付けて保存したりしてください。\r\n",
					"フィルタのエクスポート", MessageBoxButtons.OK, MessageBoxIcon.Information );

			} catch ( Exception ex ) {

				MessageBox.Show( "フィルタのエクスポートに失敗しました。\r\n" + ex.Message, "エクスポートできません", MessageBoxButtons.OK, MessageBoxIcon.Error );
			}

		}


	}
}
