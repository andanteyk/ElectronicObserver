using ElectronicObserver.Data;
using ElectronicObserver.Data.ShipGroup;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Dialog;
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
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {
	public partial class FormShipGroup : DockContent {


		/// <summary>タブ背景色(アクティブ)</summary>
		private readonly Color TabActiveColor = Color.FromArgb( 0xFF, 0xFF, 0xCC );

		/// <summary>タブ背景色(非アクティブ)</summary>
		private readonly Color TabInactiveColor = SystemColors.Control;



		// セル背景色
		private readonly Color CellColorRed = Color.FromArgb( 0xFF, 0xBB, 0xBB );
		private readonly Color CellColorOrange = Color.FromArgb( 0xFF, 0xDD, 0xBB );
		private readonly Color CellColorYellow = Color.FromArgb( 0xFF, 0xFF, 0xBB );
		private readonly Color CellColorGreen = Color.FromArgb( 0xBB, 0xFF, 0xBB );
		private readonly Color CellColorGray = Color.FromArgb( 0xBB, 0xBB, 0xBB );
		private readonly Color CellColorCherry = Color.FromArgb( 0xFF, 0xDD, 0xDD );

		private Brush BrushHighlight;
		private Brush BrushForeground = new SolidBrush( Color.Black );	// config.UI.ForeColor
		private Brush BrushSubForeground = new SolidBrush( Color.FromArgb( 0x88, 0x88, 0x88 ) );
		private readonly Brush[] Bs = new[]
		{
			//new SolidBrush( Color.FromArgb( 114, 55, 49 ) ),
			//new SolidBrush( Color.FromArgb( 46, 82, 113 ) ),
			//new SolidBrush( Color.FromArgb( 137, 79, 34 ) ),
			//new SolidBrush( Color.FromArgb( 131, 99, 37 ) ),
			//new SolidBrush( Color.FromArgb( 63, 63, 70 ) ),
			new SolidBrush( Color.FromArgb( 191, 115, 106 ) ),
			new SolidBrush( Color.FromArgb( 104, 151, 193 ) ),
			new SolidBrush( Color.FromArgb( 213, 139, 85 ) ),
			new SolidBrush( Color.FromArgb( 208, 167, 89 ) ),
			new SolidBrush( Color.FromArgb( 143, 143, 154 ) ),
		};

		private float subfontHeight = -1;

		//セルスタイル
		private DataGridViewCellStyle CSDefaultLeft, CSDefaultCenter, CSDefaultRight,
			CSRedRight, CSOrangeRight, CSYellowRight, CSGreenRight, CSGrayRight, CSCherryRight,
			CSIsLocked;

		/// <summary>選択中のタブ</summary>
		private ImageLabel SelectedTab = null;


		private bool IsLoadTypes = false;

		private Dictionary<string, int[]> CustomShipTypes = new Dictionary<string, int[]>
		{
			{ "駆逐艦",     new[] { 2 } },
			{ "軽巡·雷巡", new[] { 3, 4 } },
			{ "重巡·航巡", new[] { 5, 6 } },
			{ "戦艦",      new[] { 8, 9, 10, 12 } },
			{ "航空母艦",   new[] { 7, 11, 18 } },
			{ "潜水艦",     new[] { 13, 14 } },
			{ "航戦·航巡", new[] { 6, 10 } },
		};

		public FormShipGroup( FormMain parent ) {
			this.SuspendLayoutForDpiScale();
			InitializeComponent();

			ControlHelper.SetDoubleBuffered( ShipView );


			foreach ( DataGridViewColumn column in ShipView.Columns ) {
				column.MinimumWidth = 2;
			}

			foreach ( var ct in CustomShipTypes )
			{
				var label = new ImageLabel();
				label.Text = ct.Key;
				label.Font = ShipView.Font;
				label.BackColor = TabInactiveColor;
				label.BorderStyle = BorderStyle.FixedSingle;
				label.Padding = new Padding( 4, 4, 7, 7 );
				label.Margin = new Padding( 0, 0, 4, 0 );
				label.ImageAlign = ContentAlignment.MiddleCenter;
				label.AutoSize = true;
				label.Cursor = Cursors.Hand;

				label.Tag = ct.Value;
				label.Click += label_Click;

				FlowLayoutCustomShipTypes.Controls.Add( label );
			}


			#region set CellStyle

			CSDefaultLeft = new DataGridViewCellStyle();
			CSDefaultLeft.Alignment = DataGridViewContentAlignment.MiddleLeft;
			CSDefaultLeft.BackColor = SystemColors.Control;
			CSDefaultLeft.Font = Font;
			CSDefaultLeft.ForeColor = SystemColors.ControlText;
			CSDefaultLeft.SelectionBackColor = Color.FromArgb( 0xFF, 0xFF, 0xCC );
			CSDefaultLeft.SelectionForeColor = SystemColors.ControlText;
			CSDefaultLeft.WrapMode = DataGridViewTriState.False;

			CSDefaultCenter = new DataGridViewCellStyle( CSDefaultLeft );
			CSDefaultCenter.Alignment = DataGridViewContentAlignment.MiddleCenter;

			CSDefaultRight = new DataGridViewCellStyle( CSDefaultLeft );
			CSDefaultRight.Alignment = DataGridViewContentAlignment.MiddleRight;

			CSRedRight = new DataGridViewCellStyle( CSDefaultRight );
			CSRedRight.BackColor =
			CSRedRight.SelectionBackColor = CellColorRed;

			CSOrangeRight = new DataGridViewCellStyle( CSDefaultRight );
			CSOrangeRight.BackColor =
			CSOrangeRight.SelectionBackColor = CellColorOrange;

			CSYellowRight = new DataGridViewCellStyle( CSDefaultRight );
			CSYellowRight.BackColor =
			CSYellowRight.SelectionBackColor = CellColorYellow;

			CSGreenRight = new DataGridViewCellStyle( CSDefaultRight );
			CSGreenRight.BackColor =
			CSGreenRight.SelectionBackColor = CellColorGreen;

			CSGrayRight = new DataGridViewCellStyle( CSDefaultRight );
			CSGrayRight.ForeColor =
			CSGrayRight.SelectionForeColor = CellColorGray;

			CSCherryRight = new DataGridViewCellStyle( CSDefaultRight );
			CSCherryRight.BackColor =
			CSCherryRight.SelectionBackColor = CellColorCherry;

			CSIsLocked = new DataGridViewCellStyle( CSDefaultCenter );
			CSIsLocked.ForeColor =
			CSIsLocked.SelectionForeColor = Color.FromArgb( 0xFF, 0x88, 0x88 );


			ShipView.DefaultCellStyle = CSDefaultRight;
			ShipView_Name.DefaultCellStyle = CSDefaultLeft;
			ShipView_Slot1.DefaultCellStyle = CSDefaultLeft;
			ShipView_Slot2.DefaultCellStyle = CSDefaultLeft;
			ShipView_Slot3.DefaultCellStyle = CSDefaultLeft;
			ShipView_Slot4.DefaultCellStyle = CSDefaultLeft;
			ShipView_Slot5.DefaultCellStyle = CSDefaultLeft;
			ShipView_ExpansionSlot.DefaultCellStyle = CSDefaultLeft;

			ShipView.ColumnHeadersHeight = this.GetDpiHeight( 24 );

			#endregion


			SystemEvents.SystemShuttingDown += SystemShuttingDown;

			this.ResumeLayoutForDpiScale();
		}

		void label_Click( object sender, EventArgs e )
		{
			ImageLabel label = sender as ImageLabel;
			if ( label != null )
			{
				int[] ids = (int[])label.Tag;
				
				HangFilterChange = true;
				CheckShipTypeAll.Checked = false;
				foreach ( var type in FlowLayoutShipTypes.Controls.OfType<CheckBox>() )
				{
					type.Checked = ids.Contains( (int)type.Tag );
				}
				HangFilterChange = false;

				FilterCheckChanged( sender, e );
			}
		}


		private void FormShipGroup_Load( object sender, EventArgs e ) {

			ShipGroupManager groups = KCDatabase.Instance.ShipGroup;


			if ( !groups.ShipGroups.ContainsKey( -1 ) ) {
				var master = new ShipGroupData( -1 );
				master.Name = "全所属艦";
				//master.ColumnFilter = Enumerable.Repeat<bool>( true, ShipView.Columns.Count ).ToList();
				master.ColumnFilter = ShipView.Columns.OfType<DataGridViewColumn>().Select( c => c.Visible ).ToList();
				master.ColumnWidth = ShipView.Columns.OfType<DataGridViewColumn>().Select( c => c.Width ).ToList();

				groups.ShipGroups.Add( master );
			}


			foreach ( var g in groups.ShipGroups.Values ) {
				TabPanel.Controls.Add( CreateTabLabel( g.GroupID ) );
			}


			//*/
			{
				int columnCount = ShipView.Columns.Count;
				for ( int i = 0; i < columnCount; i++ ) {
					ShipView.Columns[i].Visible = false;
				}
			}
			//*/


			ConfigurationChanged();


			APIObserver o = APIObserver.Instance;

			o.APIList["api_port/port"].ResponseReceived += APIUpdated;
			o.APIList["api_get_member/ship2"].ResponseReceived += APIUpdated;
			o.APIList["api_get_member/ship_deck"].ResponseReceived += APIUpdated;


			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormShipGroup] );

		}


		void SetFilterShipTypes()
		{
			foreach ( var type in KCDatabase.Instance.ShipTypes.Values.OrderBy( t => t.SortID ) )
			{
				// 敌方补给舰不显示
				if ( type.TypeID == 15 )
					continue;

				var check = new CheckBox();
				check.AutoSize = true;
				check.Font = ShipView.Font;
				check.Text = type.Name;
				check.Checked = true;
				check.Tag = type.TypeID;
				check.UseVisualStyleBackColor = true;
				check.CheckedChanged += check_CheckedChanged;
				FlowLayoutShipTypes.Controls.Add( check );
			}
			IsLoadTypes = true;
		}

		void ConfigurationChanged() {

			var config = Utility.Configuration.Config;

			subfontHeight = -1;
			ShipView.Font = StatusBar.Font = Font = config.UI.MainFont;

			BrushHighlight = new SolidBrush( config.UI.HighlightForeColor );

			CSDefaultLeft.Font =
			CSDefaultCenter.Font =
			CSDefaultRight.Font =
			CSRedRight.Font =
			CSOrangeRight.Font =
			CSYellowRight.Font =
			CSGreenRight.Font =
			CSGrayRight.Font =
			CSCherryRight.Font =
			CSIsLocked.Font =
				config.UI.MainFont;

			splitContainer1.SplitterDistance = config.FormShipGroup.SplitterDistance;
			MenuGroup_AutoUpdate.Checked = config.FormShipGroup.AutoUpdate;
			MenuGroup_ShowStatusBar.Checked = config.FormShipGroup.ShowStatusBar;

		}



		/// <summary>
		/// 指定したグループIDに基づいてタブ ラベルを生成します。
		/// </summary>
		private ImageLabel CreateTabLabel( int id ) {

			ImageLabel label = new ImageLabel();
			label.Text = KCDatabase.Instance.ShipGroup[id].Name;
			label.Anchor = AnchorStyles.Left;
			label.Font = ShipView.Font;
			label.BackColor = TabInactiveColor;
			label.BorderStyle = BorderStyle.FixedSingle;
			label.Padding = new Padding( 4, 4, 7, 7 );
			label.Margin = new Padding( 0, 0, 0, 0 );
			label.ImageAlign = ContentAlignment.MiddleCenter;
			label.AutoSize = true;
			label.Cursor = Cursors.Hand;

			//イベントと固有IDの追加(内部データとの紐付)
			label.Click += TabLabel_Click;
			label.MouseDown += TabLabel_MouseDown;
			label.MouseMove += TabLabel_MouseMove;
			label.MouseUp += TabLabel_MouseUp;
			label.ContextMenuStrip = MenuGroup;
			label.Tag = id;

			this.ButtonFilter.Size = new System.Drawing.Size( 21, label.Height );

			return label;
		}




		void TabLabel_Click( object sender, EventArgs e ) {
			var label = sender as ImageLabel;

			if ( PanelFilter.Visible && (int)label.Tag != -1 )
				ButtonFilter.PerformClick();

			ChangeShipView( sender as ImageLabel );
		}

		private void APIUpdated( string apiname, dynamic data ) {

			if ( !IsLoadTypes )
				SetFilterShipTypes();

			if ( MenuGroup_AutoUpdate.Checked )
				ChangeShipView( SelectedTab );
		}






		/// <summary>
		/// ShipView用の新しい行のインスタンスを作成します。
		/// </summary>
		/// <param name="ship">追加する艦娘データ。</param>
		private DataGridViewRow CreateShipViewRow( ShipData ship ) {

			if ( ship == null ) return null;

			DataGridViewRow row = new DataGridViewRow();
			row.CreateCells( ShipView );

			row.SetValues(
				ship.MasterID,
				ship.MasterShip.ShipType,
				ship.MasterShip.Name,
				ship.Level,
				ship.ExpTotal,
				ship.ExpNext,
				ship.ExpNextRemodel,
				new Fraction( ship.HPCurrent, ship.HPMax ),
				ship.Condition,
				new Fraction( ship.Fuel, ship.FuelMax ),
				new Fraction( ship.Ammo, ship.AmmoMax ),
				GetEquipmentTypeID( ship, 0 ),
				GetEquipmentTypeID( ship, 1 ),
				GetEquipmentTypeID( ship, 2 ),
				GetEquipmentTypeID( ship, 3 ),
				GetEquipmentTypeID( ship, 4 ),
				GetEquipmentTypeID( ship, 5 ),
				GetEquipmentString( ship, 5 ),		//補強スロット
				new Fraction( ship.Aircraft[0], ship.MasterShip.Aircraft[0] ),
				new Fraction( ship.Aircraft[1], ship.MasterShip.Aircraft[1] ),
				new Fraction( ship.Aircraft[2], ship.MasterShip.Aircraft[2] ),
				new Fraction( ship.Aircraft[3], ship.MasterShip.Aircraft[3] ),
				new Fraction( ship.Aircraft[4], ship.MasterShip.Aircraft[4] ),
				new Fraction( ship.AircraftTotal, ship.MasterShip.AircraftTotal ),
				ship.FleetWithIndex,
				ship.RepairingDockID == -1 ? ship.RepairTime : -1000 + ship.RepairingDockID,
				ship.RepairSteel,
				ship.RepairFuel,
				ship.FirepowerBase,
				ship.FirepowerRemain,
				ship.FirepowerTotal,
				ship.TorpedoBase,
				ship.TorpedoRemain,
				ship.TorpedoTotal,
				ship.AABase,
				ship.AARemain,
				ship.AATotal,
				ship.ArmorBase,
				ship.ArmorRemain,
				ship.ArmorTotal,
				ship.ASWBase,
				ship.ASWTotal,
				ship.EvasionBase,
				ship.EvasionTotal,
				ship.LOSBase,
				ship.LOSTotal,
				ship.LuckBase,
				ship.LuckRemain,
				ship.LuckTotal,
				ship.BomberTotal,
				ship.MasterShip.Speed,
				ship.Range,
				ship.AirBattlePower,
				ship.ShellingPower,
				ship.AircraftPower,
				ship.AntiSubmarinePower,
				ship.TorpedoPower,
				ship.NightBattlePower,
				ship.IsLocked ? 1 : ship.IsLockedByEquipment ? 2 : 0,
				ship.SallyArea
				);

			row.Cells[ShipView_Slot1.Index].ToolTipText = GetEquipmentString( ship, 0 );
			row.Cells[ShipView_Slot2.Index].ToolTipText = GetEquipmentString( ship, 1 );
			row.Cells[ShipView_Slot3.Index].ToolTipText = GetEquipmentString( ship, 2 );
			row.Cells[ShipView_Slot4.Index].ToolTipText = GetEquipmentString( ship, 3 );
			row.Cells[ShipView_Slot5.Index].ToolTipText = GetEquipmentString( ship, 4 );
			var ex = ship.ExpansionSlotInstance;
			row.Cells[ShipView_ExpansionSlot.Index].ToolTipText = ( ex != null ? ex.NameWithLevel : ( ship.ExpansionSlot < 0 ? "(无)" : "" ) );

			row.Cells[ShipView_Firepower.Index].ToolTipText = ship.FirepowerRemain > 0 ? ( "剩余: " + ship.FirepowerRemain ) : null;
			row.Cells[ShipView_Torpedo.Index].ToolTipText = ship.TorpedoRemain > 0 ? ( "剩余: " + ship.TorpedoRemain ) : null;
			row.Cells[ShipView_AA.Index].ToolTipText = ship.AARemain > 0 ? ( "剩余: " + ship.AARemain ) : null;
			row.Cells[ShipView_Armor.Index].ToolTipText = ship.ArmorRemain > 0 ? ( "剩余: " + ship.ArmorRemain ) : null;
			row.Cells[ShipView_Luck.Index].ToolTipText = ship.LuckRemain > 0 ? ( "剩余: " + ship.LuckRemain ) : null;

			row.Cells[ShipView_Name.Index].Tag = ship.ShipID;
			//row.Cells[ShipView_Level.Index].Tag = ship.ExpTotal;


			{
				DataGridViewCellStyle cs;
				double hprate = ship.HPRate;
				if ( hprate <= 0.25 )
					cs = CSRedRight;
				else if ( hprate <= 0.50 )
					cs = CSOrangeRight;
				else if ( hprate <= 0.75 )
					cs = CSYellowRight;
				else if ( hprate < 1.00 )
					cs = CSGreenRight;
				else
					cs = CSDefaultRight;

				row.Cells[ShipView_HP.Index].Style = cs;
			}
			{
				DataGridViewCellStyle cs;
				if ( ship.Condition < 20 )
					cs = CSRedRight;
				else if ( ship.Condition < 30 )
					cs = CSOrangeRight;
				else if ( ship.Condition < Utility.Configuration.Config.Control.ConditionBorder )
					cs = CSYellowRight;
				else if ( ship.Condition < 50 )
					cs = CSDefaultRight;
				else
					cs = CSGreenRight;

				row.Cells[ShipView_Condition.Index].Style = cs;
			}
			row.Cells[ShipView_Fuel.Index].Style = ship.Fuel < ship.FuelMax ? CSYellowRight : CSDefaultRight;
			row.Cells[ShipView_Ammo.Index].Style = ship.Ammo < ship.AmmoMax ? CSYellowRight : CSDefaultRight;
			{
				var current = ship.Aircraft;
				var max = ship.MasterShip.Aircraft;
				row.Cells[ShipView_Aircraft1.Index].Style = ( max[0] > 0 && current[0] == 0 ) ? CSRedRight : ( current[0] < max[0] ) ? CSYellowRight : CSDefaultRight;
				row.Cells[ShipView_Aircraft2.Index].Style = ( max[1] > 0 && current[1] == 0 ) ? CSRedRight : ( current[1] < max[1] ) ? CSYellowRight : CSDefaultRight;
				row.Cells[ShipView_Aircraft3.Index].Style = ( max[2] > 0 && current[2] == 0 ) ? CSRedRight : ( current[2] < max[2] ) ? CSYellowRight : CSDefaultRight;
				row.Cells[ShipView_Aircraft4.Index].Style = ( max[3] > 0 && current[3] == 0 ) ? CSRedRight : ( current[3] < max[3] ) ? CSYellowRight : CSDefaultRight;
				row.Cells[ShipView_Aircraft5.Index].Style = ( max[4] > 0 && current[4] == 0 ) ? CSRedRight : ( current[4] < max[4] ) ? CSYellowRight : CSDefaultRight;
				row.Cells[ShipView_AircraftTotal.Index].Style = ( ship.MasterShip.AircraftTotal > 0 && ship.AircraftTotal == 0 ) ? CSRedRight : ( ship.AircraftTotal < ship.MasterShip.AircraftTotal ) ? CSYellowRight : CSDefaultRight;
			}
			{
				DataGridViewCellStyle cs;
				if ( ship.RepairTime == 0 )
					cs = CSDefaultRight;
				else if ( ship.RepairTime < 1000 * 60 * 60 )
					cs = CSYellowRight;
				else if ( ship.RepairTime < 1000 * 60 * 60 * 6 )
					cs = CSOrangeRight;
				else
					cs = CSRedRight;

				row.Cells[ShipView_RepairTime.Index].Style = cs;
			}
			row.Cells[ShipView_FirepowerRemain.Index].Style = ship.FirepowerRemain == 0 ? CSGrayRight : CSDefaultRight;
			row.Cells[ShipView_TorpedoRemain.Index].Style = ship.TorpedoRemain == 0 ? CSGrayRight : CSDefaultRight;
			row.Cells[ShipView_AARemain.Index].Style = ship.AARemain == 0 ? CSGrayRight : CSDefaultRight;
			row.Cells[ShipView_ArmorRemain.Index].Style = ship.ArmorRemain == 0 ? CSGrayRight : CSDefaultRight;
			row.Cells[ShipView_LuckRemain.Index].Style = ship.LuckRemain == 0 ? CSGrayRight : CSDefaultRight;

			row.Cells[ShipView_Locked.Index].Style = ship.IsLocked ? CSIsLocked : CSDefaultCenter;

			return row;
		}


		/// <summary>
		/// 指定したタブのグループのShipViewを作成します。
		/// </summary>
		/// <param name="target">作成するビューのグループデータ</param>
		private void BuildShipView( ImageLabel target ) {
			if ( target == null )
				return;

			int groupID = (int)target.Tag;

			ShipGroupData group = KCDatabase.Instance.ShipGroup[groupID];
			if ( groupID < 0 )
			{
				BuildShipView( GetFilteredShips(), group );
			}
			else
			{
				var ships = group.MembersInstance;

				BuildShipView( ships, group );
			}
		}

		private void BuildShipView( IEnumerable<ShipData> ships, ShipGroupData g )
		{
			ShipView.SuspendLayout();
			ShipView.Rows.Clear();

			group.UpdateMembers();
			var ships = group.MembersInstance;
			var rows = new List<DataGridViewRow>( ships.Count() );

			foreach ( ShipData ship in ships )
			{

				if ( ship == null ) continue;

				DataGridViewRow row = CreateShipViewRow( ship );
				rows.Add( row );

			}

			for ( int i = 0; i < rows.Count; i++ )
				rows[i].Tag = i;

			ShipView.Rows.AddRange( rows.ToArray() );

			if ( g != null )
			{

				// 設定に抜けがあった場合補充
				if ( group.ViewColumns == null ) {
					group.ViewColumns = new Dictionary<string, ShipGroupData.ViewColumnData>();
				}
				if ( ShipView.Columns.Count != group.ViewColumns.Count ) {
					foreach ( DataGridViewColumn column in ShipView.Columns ) {

						if ( !group.ViewColumns.ContainsKey( column.Name ) ) {
							var newdata = new ShipGroupData.ViewColumnData( column );
							newdata.Visible = true;		//初期状態でインビジだと不都合なので

							group.ViewColumns.Add( newdata.Name, newdata );
						}
					}


				// set sort
				SetSort( g.SortColumnName, (SortOrder)g.SortOrder );
			}
			else
			{
				DataGridViewColumn col = ShipView.SortedColumn;
				SortOrder order = ShipView.SortOrder;
				if ( order > SortOrder.None && col != null )
				{
					ShipView.Sort( col, ( order == SortOrder.Ascending ) ? ListSortDirection.Ascending : ListSortDirection.Descending );
				}
			}

			ApplyViewData( group );
			ApplyAutoSort( group );

			ShipView.ResumeLayout();
			if ( !ShipView.Focused )
				ShipView.Focus();


			//status bar
			if ( KCDatabase.Instance.Ships.Count > 0 )
			{
				try
				{
					Status_ShipCount.Text = string.Format( "所属: {0}隻", ships.Count() );
					Status_LevelTotal.Text = string.Format( "合計Lv: {0}", ships.Where( s => s != null ).Sum( s => s.Level ) );
					Status_LevelAverage.Text = string.Format( "平均Lv: {0:F2}", ships.Count() > 0 ? ships.Where( s => s != null ).Average( s => s.Level ) : 0 );
				}
				catch { }
			}

		}


		/// <summary>
		/// ShipViewを指定したタブに切り替えます。
		/// </summary>
		private void ChangeShipView( ImageLabel target ) {

			if ( target == null ) return;


			int groupID = (int)target.Tag;
			var group = KCDatabase.Instance.ShipGroup[groupID];


			if ( group == null ) {
				Utility.Logger.Add( 3, "エラー：存在しないグループを参照しようとしました。開発者に連絡してください" );
				return;
			}


			if ( SelectedTab != null )
				SelectedTab.BackColor = TabInactiveColor;

			SelectedTab = target;


			BuildShipView( SelectedTab );
			SelectedTab.BackColor = TabActiveColor;

		}


		private int GetEquipmentTypeID( ShipData ship, int index )
		{
			if ( index >= 5 )
			{
				if ( ship.ExpansionSlotInstanceMaster != null )
					return ship.ExpansionSlotInstanceMaster.IconType;

				else
					return -1 - ship.ExpansionSlot;

			}
			else if ( ship.SlotInstance[index] != null )
				return ship.SlotInstance[index].MasterEquipment.IconType;

			return ship.SlotSize > index ? 0 : -1;
		}

		private string GetEquipmentString( ShipData ship, int index ) {

			if ( index < 5 ) {
				return ( index >= ship.SlotSize && ship.Slot[index] == -1 ) ? "" :
					ship.SlotInstance[index] == null ? "(なし)" : ship.SlotInstance[index].NameWithLevel;
			} else {
				return ship.ExpansionSlot == 0 ? "" :
					ship.ExpansionSlotInstance == null ? "(なし)" : ship.ExpansionSlotInstance.NameWithLevel;
			}

		}


		private void ShipView_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e ) {

			if ( e.ColumnIndex == ShipView_ShipType.Index ) {
				e.Value = KCDatabase.Instance.ShipTypes[(int)e.Value].Name;
				e.FormattingApplied = true;

			} else if ( e.ColumnIndex == ShipView_Fleet.Index ) {
				if ( e.Value == null )
					e.Value = "";
				e.FormattingApplied = true;

			} else if ( e.ColumnIndex == ShipView_RepairTime.Index ) {

				if ( (int)e.Value < 0 ) {
					e.Value = "入渠 #" + ( (int)e.Value + 1000 );
				} else {
					e.Value = DateTimeHelper.ToTimeRemainString( DateTimeHelper.FromAPITimeSpan( (int)e.Value ) );
				}
				e.FormattingApplied = true;

			}
			else if (
				e.ColumnIndex == ShipView_Firepower.Index ||
				e.ColumnIndex == ShipView_Torpedo.Index ||
				e.ColumnIndex == ShipView_AA.Index ||
				e.ColumnIndex == ShipView_Armor.Index ||
				e.ColumnIndex == ShipView_Luck.Index
				) {
				e.Value += " MAX";
				e.FormattingApplied = true;

			} else if ( (
				e.ColumnIndex == ShipView_FirepowerRemain.Index ||
				e.ColumnIndex == ShipView_TorpedoRemain.Index ||
				e.ColumnIndex == ShipView_AARemain.Index ||
				e.ColumnIndex == ShipView_ArmorRemain.Index ||
				e.ColumnIndex == ShipView_LuckRemain.Index
				) && (int)e.Value == 0 ) {
				e.Value = "MAX";
				e.FormattingApplied = true;

			} else if ( e.ColumnIndex == ShipView_Aircraft1.Index ||
				 e.ColumnIndex == ShipView_Aircraft2.Index ||
				 e.ColumnIndex == ShipView_Aircraft3.Index ||
				 e.ColumnIndex == ShipView_Aircraft4.Index ||
				 e.ColumnIndex == ShipView_Aircraft5.Index ) {	// AircraftTotal は 0 でも表示する
				if ( ( (Fraction)e.Value ).Max == 0 ) {
					e.Value = "";
					e.FormattingApplied = true;
				}

			} else if ( e.ColumnIndex == ShipView_Locked.Index ) {
				e.Value = (int)e.Value == 1 ? "❤" : (int)e.Value == 2 ? "■" : "";
				e.FormattingApplied = true;

			} else if ( e.ColumnIndex == ShipView_SallyArea.Index && (int)e.Value == -1 ) {
				e.Value = "";
				e.FormattingApplied = true;

			} else if ( e.ColumnIndex == ShipView_Range.Index ) {
				e.Value = Constants.GetRange( (int)e.Value );
				e.FormattingApplied = true;

			} else if ( e.ColumnIndex == ShipView_Speed.Index ) {
				e.Value = Constants.GetSpeed( (int)e.Value );
				e.FormattingApplied = true;

			}

		}

		private void ShipView_CellPainting( object sender, DataGridViewCellPaintingEventArgs e )
		{
			if ( e.RowIndex < 0 )
				return;

			if ( e.ColumnIndex >= ShipView_Equipment1.Index && e.ColumnIndex <= ShipView_EquipmentEx.Index )
			{
				e.Paint( e.ClipBounds, e.PaintParts & ~DataGridViewPaintParts.ContentForeground );
				int id;
				var tag = e.Value;
				if ( tag is int && ( id = (int)tag ) >= 0 )
				{
					Image image = ResourceManager.Instance.Equipments[id];
					if ( image != null )
					{
						var rect = e.CellBounds;
						rect.Width = rect.Height = Math.Min( image.Height, rect.Height );
						rect.X += ( e.CellBounds.Width - rect.Width ) / 2;
						rect.Y += ( e.CellBounds.Height - rect.Height ) / 2;
						e.Graphics.DrawImage( image, rect );
					}
				}
				e.Handled = true;
			}

			else
			{
				int index = -1;
				if ( e.ColumnIndex == ShipView_Firepower.Index )
					index = 0;
				else if ( e.ColumnIndex == ShipView_Torpedo.Index )
					index = 1;
				else if ( e.ColumnIndex == ShipView_AA.Index )
					index = 2;
				else if ( e.ColumnIndex == ShipView_Armor.Index )
					index = 3;
				else if ( e.ColumnIndex == ShipView_Luck.Index )
					index = 4;

				if ( index >= 0 )
				{
					string value = ( (int)e.Value ).ToString();
					int remain = (int)ShipView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value;
					if ( remain <= 0 )
					{
						e.Graphics.FillRectangle( Bs[index], e.CellBounds );
						e.Paint( e.ClipBounds, e.PaintParts
							& ~DataGridViewPaintParts.ContentForeground
							& ~DataGridViewPaintParts.Background );
					}
					else
					{
						e.Paint( e.ClipBounds, e.PaintParts & ~DataGridViewPaintParts.ContentForeground );
					}
					StringFormat sf = new StringFormat { LineAlignment = StringAlignment.Center };
					e.Graphics.DrawString( value, ShipView.Font, remain > 0 ? BrushForeground : BrushHighlight, e.CellBounds, sf );
					float offset = e.Graphics.MeasureString( value, ShipView.Font ).Width;

					RectangleF rect = e.CellBounds;
					rect.X += offset + 3;
					rect.Width -= offset + 3;
					if ( rect.Width > 0 )
					{
						if ( subfontHeight < 0 )
						{
							subfontHeight = e.Graphics.MeasureString( "W", Utility.Configuration.Config.UI.SubFont ).Height;
						}
						rect.Y += ( rect.Height - subfontHeight ) * 2 / 3;
						rect.Height = subfontHeight;
						e.Graphics.DrawString( remain > 0 ? "+" + remain : "MAX", Utility.Configuration.Config.UI.SubFont, BrushSubForeground, rect );
					}

					e.Handled = true;
				}
			}
		}


		private void ShipView_SortCompare( object sender, DataGridViewSortCompareEventArgs e ) {

			if ( e.Column.Index == ShipView_Name.Index ) {
				e.SortResult =
					KCDatabase.Instance.MasterShips[(int)ShipView.Rows[e.RowIndex1].Cells[e.Column.Index].Tag].AlbumNo -
					KCDatabase.Instance.MasterShips[(int)ShipView.Rows[e.RowIndex2].Cells[e.Column.Index].Tag].AlbumNo;

			} else if ( e.Column.Index == ShipView_Exp.Index ) {
				e.SortResult = (int)e.CellValue1 - (int)e.CellValue2;
				if ( e.SortResult == 0 )	//for Lv.99-100
					e.SortResult = (int)ShipView[ShipView_Level.Index, e.RowIndex1].Value - (int)ShipView[ShipView_Level.Index, e.RowIndex2].Value;

			} else if (
				e.Column.Index == ShipView_HP.Index ||
				e.Column.Index == ShipView_Fuel.Index ||
				e.Column.Index == ShipView_Ammo.Index ||
				e.Column.Index == ShipView_Aircraft1.Index ||
				e.Column.Index == ShipView_Aircraft2.Index ||
				e.Column.Index == ShipView_Aircraft3.Index ||
				e.Column.Index == ShipView_Aircraft4.Index ||
				e.Column.Index == ShipView_Aircraft5.Index ||
				e.Column.Index == ShipView_AircraftTotal.Index
				) {
				Fraction frac1 = (Fraction)e.CellValue1, frac2 = (Fraction)e.CellValue2;

				double rate = frac1.Rate - frac2.Rate;

				if ( rate > 0.0 )
					e.SortResult = 1;
				else if ( rate < 0.0 )
					e.SortResult = -1;
				else
					e.SortResult = frac1.Current - frac2.Current;

			} else if ( e.Column.Index == ShipView_Fleet.Index ) {
				if ( e.CellValue1 == null ) {
					if ( e.CellValue2 == null )
						e.SortResult = 0;
					else
						e.SortResult = 1;
				} else {
					if ( e.CellValue2 == null )
						e.SortResult = -1;
					else
						e.SortResult = ( (string)e.CellValue1 ).CompareTo( e.CellValue2 );
				}

			} else if ( e.Column.Index == ShipView_EquipmentEx.Index ) {
				e.SortResult = (int)e.CellValue1 - (int)e.CellValue2;

			} else {
				e.SortResult = (int)e.CellValue1 - (int)e.CellValue2;
			}



			if ( e.SortResult == 0 ) {
				e.SortResult = (int)ShipView.Rows[e.RowIndex1].Tag - (int)ShipView.Rows[e.RowIndex2].Tag;
			}

			e.Handled = true;
		}


		private void ShipView_Sorted( object sender, EventArgs e ) {


			for ( int i = 0; i < ShipView.Rows.Count; i++ )
				ShipView.Rows[i].Tag = i;

		}





		#region メニュー:グループ操作

		private void MenuGroup_Add_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogTextInput( "添加分组", "请输入分组名：" ) ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

					var group = KCDatabase.Instance.ShipGroup.Add();


					group.Name = dialog.InputtedText.Trim();

					for ( int i = 0; i < ShipView.Columns.Count; i++ ) {
						var newdata = new ShipGroupData.ViewColumnData( ShipView.Columns[i] );
						if ( SelectedTab == null )
							newdata.Visible = true;		//初期状態では全行が非表示のため
						group.ViewColumns.Add( ShipView.Columns[i].Name, newdata );
					}

					TabPanel.Controls.Add( CreateTabLabel( group.GroupID ) );

				}

			}

		}

		private void MenuGroup_Copy_Click( object sender, EventArgs e ) {

			ImageLabel senderLabel = MenuGroup.SourceControl as ImageLabel;
			if ( senderLabel == null )
				return;		//想定外

			using ( var dialog = new DialogTextInput( "グループをコピー", "グループ名を入力してください：" ) ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

					var group = KCDatabase.Instance.ShipGroup[(int)senderLabel.Tag].Clone();

					group.GroupID = KCDatabase.Instance.ShipGroup.GetUniqueID();
					group.Name = dialog.InputtedText.Trim();

					KCDatabase.Instance.ShipGroup.ShipGroups.Add( group );

					TabPanel.Controls.Add( CreateTabLabel( group.GroupID ) );

				}
			}

		}

		private void MenuGroup_Delete_Click( object sender, EventArgs e ) {

			ImageLabel senderLabel = MenuGroup.SourceControl as ImageLabel;
			if ( senderLabel == null )
				return;		//想定外

			ShipGroupData group = KCDatabase.Instance.ShipGroup[(int)senderLabel.Tag];

			if ( group != null ) {
				if ( MessageBox.Show( string.Format( "要删除分组 [{0}] 吗？\r\n此操作无法撤销。", group.Name ), "确认",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 )
					== System.Windows.Forms.DialogResult.Yes ) {

					if ( SelectedTab == senderLabel ) {
						ShipView.Rows.Clear();
						SelectedTab = null;
					}
					KCDatabase.Instance.ShipGroup.ShipGroups.Remove( group );
					TabPanel.Controls.Remove( senderLabel );
					senderLabel.Dispose();
				}

			} else {
				MessageBox.Show( "此分组无法删除。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
			}
		}

		private void MenuGroup_Rename_Click( object sender, EventArgs e ) {

			ImageLabel senderLabel = MenuGroup.SourceControl as ImageLabel;
			if ( senderLabel == null ) return;

			ShipGroupData group = KCDatabase.Instance.ShipGroup[(int)senderLabel.Tag];

			if ( group != null ) {

				using ( var dialog = new DialogTextInput( "修改分组名", "请输入分组名：" ) ) {

					dialog.InputtedText = group.Name;

					if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

						group.Name = senderLabel.Text = dialog.InputtedText.Trim();

					}
				}

			} else {
				MessageBox.Show( "分组名修改失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
			}

		}


		private void TabPanel_DoubleClick( object sender, EventArgs e ) {

			MenuGroup_Add.PerformClick();

		}



		#endregion


		#region メニューON/OFF操作
		private void MenuGroup_Opening( object sender, CancelEventArgs e ) {

			if ( MenuGroup.SourceControl == TabPanel || SelectedTab == null ) {
				MenuGroup_Add.Enabled = true;
				MenuGroup_Copy.Enabled = false;
				MenuGroup_Rename.Enabled = false;
				MenuGroup_Delete.Enabled = false;
			} else {
				MenuGroup_Add.Enabled = true;
				MenuGroup_Copy.Enabled = true;
				MenuGroup_Rename.Enabled = true;
				MenuGroup_Delete.Enabled = true;
			}

		}

		private void MenuMember_Opening( object sender, CancelEventArgs e ) {

			if ( SelectedTab == null ) {

				e.Cancel = true;
				return;
			}

		}
		#endregion


		#region メニュー:メンバー操作

		private void MenuMember_ColumnFilter_Click( object sender, EventArgs e ) {

			ShipGroupData group = SelectedTab != null ? KCDatabase.Instance.ShipGroup[(int)SelectedTab.Tag] : null;

			if ( group == null ) {
				MessageBox.Show( "このグループは変更できません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Asterisk );
				return;
			}


			try {
				using ( var dialog = new DialogShipGroupColumnFilter( ShipView, group ) ) {

					if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

						group.ViewColumns = dialog.Result.ToDictionary( r => r.Name );
						group.ScrollLockColumnCount = dialog.ScrollLockColumnCount;

						ApplyViewData( group );
					}



				}
			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "ShipGroup: 列の設定ダイアログでエラーが発生しました。" );
			}
		}





		private void MenuMember_Filter_Click( object sender, EventArgs e ) {

			if ( SelectedTab != null ) {
				var group = KCDatabase.Instance.ShipGroup[(int)SelectedTab.Tag];

				try {
					using ( var dialog = new DialogShipGroupFilter( group.Expressions ) ) {

						dialog.ImportExpressionData( group.Expressions );

						if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

							group.Expressions = dialog.ExportExpressionData();

							group.Expressions.Compile();
							group.UpdateMembers();

							ChangeShipView( SelectedTab );
						}
					}
				} catch ( Exception ex ) {

					Utility.ErrorReporter.SendErrorReport( ex, "ShipGroup: フィルタダイアログでエラーが発生しました。" );
				}

			}
		}



		/// <summary>
		/// 表示設定を反映します。
		/// </summary>
		private void ApplyViewData( ShipGroupData group ) {

			// いったん解除しないと列入れ替え時にエラーが起きる
			foreach ( DataGridViewColumn column in ShipView.Columns ) {
				column.Frozen = false;
			}

			foreach ( var data in group.ViewColumns.Values ) {
				data.ToColumn( ShipView.Columns[data.Name] );
			}

			int count = 0;
			foreach ( var column in ShipView.Columns.Cast<DataGridViewColumn>().OrderBy( c => c.DisplayIndex ) ) {
				column.Frozen = count < group.ScrollLockColumnCount;
				count++;
			}

		}


		private void MenuMember_SortOrder_Click( object sender, EventArgs e ) {

			if ( SelectedTab != null ) {
				var group = KCDatabase.Instance.ShipGroup[(int)SelectedTab.Tag];

				try {
					using ( var dialog = new DialogShipGroupSortOrder( ShipView, group ) ) {

						if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

							group.AutoSortEnabled = dialog.AutoSortEnabled;
							group.SortOrder = dialog.Result;

							ApplyAutoSort( group );
						}

					}
				} catch ( Exception ex ) {

					Utility.ErrorReporter.SendErrorReport( ex, "ShipGroup: 自動ソート順設定ダイアログでエラーが発生しました。" );
				}
			}

		}


		private void ApplyAutoSort( ShipGroupData group ) {

			if ( !group.AutoSortEnabled || group.SortOrder == null )
				return;

			for ( int i = group.SortOrder.Count - 1; i >= 0; i-- ) {

				var order = group.SortOrder[i];
				ListSortDirection dir = order.Value;

				// ex. Desc -> Asc だった場合、見た目上 2番目が 1番目の Desc によって逆順に(Desc)になるため
				if ( group.SortOrder.Take( i ).Count( s => s.Value == ListSortDirection.Descending ) % 2 == 1 ) {
					dir = dir == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
				}

				if ( ShipView.Columns[order.Key].SortMode != DataGridViewColumnSortMode.NotSortable )
					ShipView.Sort( ShipView.Columns[order.Key], dir );
			}


		}



		private void MenuMember_CreateFromSelection_Click( object sender, EventArgs e ) {

			if ( ShipView.Rows.GetRowCount( DataGridViewElementStates.Selected ) == 0 )
				return;

			using ( var dialog = new DialogTextInput( "選択範囲から固定グループを作成", "グループ名を入力してください：" ) ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

					var group = KCDatabase.Instance.ShipGroup.Add();

					group.Name = dialog.InputtedText.Trim();

					for ( int i = 0; i < ShipView.Columns.Count; i++ ) {
						var newdata = new ShipGroupData.ViewColumnData( ShipView.Columns[i] );
						if ( SelectedTab == null )
							newdata.Visible = true;		//初期状態では全行が非表示のため
						group.ViewColumns.Add( ShipView.Columns[i].Name, newdata );
					}

					group.Expressions.Expressions.Add( new ExpressionList( false, true, false ) );

					var exp = group.Expressions.Expressions[0];
					foreach ( int id in ShipView.SelectedRows.Cast<DataGridViewRow>().Select( r => (int)r.Cells[ShipView_ID.Index].Value ) ) {
						exp.Expressions.Add( new ExpressionData( ".MasterID", ExpressionData.ExpressionOperator.Equal, id ) );
					}

					TabPanel.Controls.Add( CreateTabLabel( group.GroupID ) );

				}

			}

		}




		#region ColumnHeader
		private static readonly string[] ShipCSVHeaderUser = {
			"固有ID",
			"艦種",
			"艦名",
			"Lv",
			"Exp",
			"next",
			"改装まで",
			"耐久現在",
			"耐久最大",
			"Cond",
			"燃料",
			"弾薬",
			"装備1",
			"装備2",
			"装備3",
			"装備4",
			"装備5",
			"補強装備",
			"入渠",
			"火力",
			"火力改修",
			"火力合計",
			"雷装",
			"雷装改修",
			"雷装合計",
			"対空",
			"対空改修",
			"対空合計",
			"装甲",
			"装甲改修",
			"装甲合計",
			"対潜",
			"対潜合計",
			"回避",
			"回避合計",
			"索敵",
			"索敵合計",
			"運",
			"運改修",
			"運合計",
			"射程",
			"速力",
			"ロック",
			"出撃先",
			"航空威力",
			"砲撃威力",
			"空撃威力",
			"対潜威力",
			"雷撃威力",
			"夜戦威力",
			};

		private static readonly string[] ShipCSVHeaderData = {
			"固有ID",
			"艦種",
			"艦名",
			"艦船ID",
			"Lv",
			"Exp",
			"next",
			"改装まで",
			"耐久現在",
			"耐久最大",
			"Cond",
			"燃料",
			"弾薬",
			"装備1",
			"装備2",
			"装備3",
			"装備4",
			"装備5",
			"補強装備",
			"装備ID1",
			"装備ID2",
			"装備ID3",
			"装備ID4",
			"装備ID5",
			"補強装備ID",
			"艦載機1",
			"艦載機2",
			"艦載機3",
			"艦載機4",
			"艦載機5",
			"入渠",
			"入渠燃料",
			"入渠鋼材",
			"火力",
			"火力改修",
			"火力合計",
			"雷装",
			"雷装改修",
			"雷装合計",
			"対空",
			"対空改修",
			"対空合計",
			"装甲",
			"装甲改修",
			"装甲合計",
			"対潜",
			"対潜合計",
			"回避",
			"回避合計",
			"索敵",
			"索敵合計",
			"運",
			"運改修",
			"運合計",
			"射程",
			"速力",
			"ロック",
			"出撃先",
			"航空威力",
			"砲撃威力",
			"空撃威力",
			"対潜威力",
			"雷撃威力",
			"夜戦威力",
			};

		#endregion

		private void MenuMember_CSVOutput_Click( object sender, EventArgs e ) {

			IEnumerable<ShipData> ships;

			if ( SelectedTab == null ) {
				ships = KCDatabase.Instance.Ships.Values;

			} else {
				//*/
				ships = ShipView.Rows.Cast<DataGridViewRow>().Select( r => KCDatabase.Instance.Ships[(int)r.Cells[ShipView_ID.Index].Value] );
				/*/
				var group = KCDatabase.Instance.ShipGroup[(int)SelectedTab.Tag];
				if ( group == null )
					ships = KCDatabase.Instance.Ships.Values;
				else
					ships = group.MembersInstance;
				//*/
			}


			using ( var dialog = new DialogShipGroupCSVOutput() ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

					try {

						using ( StreamWriter sw = new StreamWriter( dialog.OutputPath, false, Utility.Configuration.Config.Log.FileEncoding ) ) {

							string[] header = dialog.OutputFormat == DialogShipGroupCSVOutput.OutputFormatConstants.User ? ShipCSVHeaderUser : ShipCSVHeaderData;

							sw.WriteLine( string.Join( ",", header ) );

							string arg = string.Format( "{{{0}}}", string.Join( "},{", Enumerable.Range( 0, header.Length ) ) );

							foreach ( ShipData ship in ships ) {

								if ( ship == null ) continue;


								if ( dialog.OutputFormat == DialogShipGroupCSVOutput.OutputFormatConstants.User ) {

									sw.WriteLine( arg,
										ship.MasterID,
										ship.MasterShip.ShipTypeName,
										ship.MasterShip.NameWithClass,
										ship.Level,
										ship.ExpTotal,
										ship.ExpNext,
										ship.ExpNextRemodel,
										ship.HPCurrent,
										ship.HPMax,
										ship.Condition,
										ship.Fuel,
										ship.Ammo,
										GetEquipmentString( ship, 0 ),
										GetEquipmentString( ship, 1 ),
										GetEquipmentString( ship, 2 ),
										GetEquipmentString( ship, 3 ),
										GetEquipmentString( ship, 4 ),
										GetEquipmentString( ship, 5 ),
										DateTimeHelper.ToTimeRemainString( DateTimeHelper.FromAPITimeSpan( ship.RepairTime ) ),
										ship.FirepowerBase,
										ship.FirepowerRemain,
										ship.FirepowerTotal,
										ship.TorpedoBase,
										ship.TorpedoRemain,
										ship.TorpedoTotal,
										ship.AABase,
										ship.AARemain,
										ship.AATotal,
										ship.ArmorBase,
										ship.ArmorRemain,
										ship.ArmorTotal,
										ship.ASWBase,
										ship.ASWTotal,
										ship.EvasionBase,
										ship.EvasionTotal,
										ship.LOSBase,
										ship.LOSTotal,
										ship.LuckBase,
										ship.LuckRemain,
										ship.LuckTotal,
										Constants.GetRange( ship.Range ),
										Constants.GetSpeed( ship.MasterShip.Speed ),
										ship.IsLocked ? "❤" : ship.IsLockedByEquipment ? "■" : "-",
										ship.SallyArea,
										ship.AirBattlePower,
										ship.ShellingPower,
										ship.AircraftPower,
										ship.AntiSubmarinePower,
										ship.TorpedoPower,
										ship.NightBattlePower );

								} else {		//data

									sw.WriteLine( arg,
										ship.MasterID,
										ship.MasterShip.ShipType,
										ship.MasterShip.NameWithClass,
										ship.ShipID,
										ship.Level,
										ship.ExpTotal,
										ship.ExpNext,
										ship.ExpNextRemodel,
										ship.HPCurrent,
										ship.HPMax,
										ship.Condition,
										ship.Fuel,
										ship.Ammo,
										GetEquipmentString( ship, 0 ),
										GetEquipmentString( ship, 1 ),
										GetEquipmentString( ship, 2 ),
										GetEquipmentString( ship, 3 ),
										GetEquipmentString( ship, 4 ),
										GetEquipmentString( ship, 5 ),
										ship.Slot[0],
										ship.Slot[1],
										ship.Slot[2],
										ship.Slot[3],
										ship.Slot[4],
										ship.ExpansionSlot,
										ship.Aircraft[0],
										ship.Aircraft[1],
										ship.Aircraft[2],
										ship.Aircraft[3],
										ship.Aircraft[4],
										ship.RepairTime,
										ship.RepairFuel,
										ship.RepairSteel,
										ship.FirepowerBase,
										ship.FirepowerRemain,
										ship.FirepowerTotal,
										ship.TorpedoBase,
										ship.TorpedoRemain,
										ship.TorpedoTotal,
										ship.AABase,
										ship.AARemain,
										ship.AATotal,
										ship.ArmorBase,
										ship.ArmorRemain,
										ship.ArmorTotal,
										ship.ASWBase,
										ship.ASWTotal,
										ship.EvasionBase,
										ship.EvasionTotal,
										ship.LOSBase,
										ship.LOSTotal,
										ship.LuckBase,
										ship.LuckRemain,
										ship.LuckTotal,
										ship.Range,
										ship.MasterShip.Speed,
										ship.IsLocked ? 1 : ship.IsLockedByEquipment ? 2 : 0,
										ship.SallyArea,
										ship.AirBattlePower,
										ship.ShellingPower,
										ship.AircraftPower,
										ship.AntiSubmarinePower,
										ship.TorpedoPower,
										ship.NightBattlePower );

								}

							}


						}

						Utility.Logger.Add( 2, "艦船グループ CSVを " + dialog.OutputPath + " に保存しました。" );

					} catch ( Exception ex ) {

						Utility.ErrorReporter.SendErrorReport( ex, "舰船分组输出CSV失败。" );
						MessageBox.Show( "舰船分组输出CSV失败。\r\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error );

					}



				}
			}

		}





		private void SetSort( string sortname, SortOrder order )
		{
			if ( order > SortOrder.None && !string.IsNullOrEmpty( sortname ) )
			{
				DataGridViewColumn col;
				try { col = ShipView.Columns[sortname]; }
				catch { return; }

				ShipView.Sort( col, ( order == SortOrder.Ascending ) ? ListSortDirection.Ascending : ListSortDirection.Descending );
			}
		}

		#endregion


		#region タブ操作系

		private Point? _tempMouse = null;
		void TabLabel_MouseDown( object sender, MouseEventArgs e ) {
			if ( e.Button == System.Windows.Forms.MouseButtons.Left ) {
				_tempMouse = TabPanel.PointToClient( e.Location );
			} else {
				_tempMouse = null;
			}
		}

		void TabLabel_MouseMove( object sender, MouseEventArgs e ) {

			if ( _tempMouse != null ) {

				Rectangle move = new Rectangle(
					_tempMouse.Value.X - SystemInformation.DragSize.Width / 2,
					_tempMouse.Value.Y - SystemInformation.DragSize.Height / 2,
					SystemInformation.DragSize.Width,
					SystemInformation.DragSize.Height
					);

				if ( !move.Contains( TabPanel.PointToClient( e.Location ) ) ) {
					TabPanel.DoDragDrop( sender, DragDropEffects.All );
					_tempMouse = null;
				}
			}

		}

		void TabLabel_MouseUp( object sender, MouseEventArgs e ) {
			_tempMouse = null;
		}


		private void TabPanel_DragEnter( object sender, DragEventArgs e ) {

			if ( e.Data.GetDataPresent( typeof( ImageLabel ) ) ) {
				e.Effect = DragDropEffects.Move;
			} else {
				e.Effect = DragDropEffects.None;
			}

		}

		private void TabPanel_QueryContinueDrag( object sender, QueryContinueDragEventArgs e ) {

			//右クリックでキャンセル
			if ( ( e.KeyState & 2 ) != 0 ) {
				e.Action = DragAction.Cancel;
			}

		}


		private void TabPanel_DragDrop( object sender, DragEventArgs e ) {

			//fixme:カッコカリ　範囲外にドロップすると端に行く

			Point mp = TabPanel.PointToClient( new Point( e.X, e.Y ) );

			var item = TabPanel.GetChildAtPoint( mp );

			int index = TabPanel.Controls.GetChildIndex( item, false );

			TabPanel.Controls.SetChildIndex( (System.Windows.Forms.Control)e.Data.GetData( typeof( ImageLabel ) ), index );

			TabPanel.Invalidate();

		}

		#endregion



		private void MenuGroup_ShowStatusBar_CheckedChanged( object sender, EventArgs e ) {

			StatusBar.Visible = MenuGroup_ShowStatusBar.Checked;

		}



		void SystemShuttingDown() {


			Utility.Configuration.Config.FormShipGroup.SplitterDistance = splitContainer1.SplitterDistance;
			Utility.Configuration.Config.FormShipGroup.AutoUpdate = MenuGroup_AutoUpdate.Checked;
			Utility.Configuration.Config.FormShipGroup.ShowStatusBar = MenuGroup_ShowStatusBar.Checked;



			ShipGroupManager groups = KCDatabase.Instance.ShipGroup;


			List<ImageLabel> list = TabPanel.Controls.OfType<ImageLabel>().OrderBy( c => TabPanel.Controls.GetChildIndex( c ) ).ToList();

			for ( int i = 0; i < list.Count; i++ ) {

				ShipGroupData group = groups[(int)list[i].Tag];
				if ( group != null )
					group.GroupID = i + 1;
			}

		}


		public override string GetPersistString()
		{
			return "ShipGroup";
		}

		private bool HangFilterChange = false;

		private void CheckShipTypeAll_CheckedChanged( object sender, EventArgs e )
		{
			if ( HangFilterChange )
				return;

			bool check = CheckShipTypeAll.Checked;
			HangFilterChange = true;
			foreach ( CheckBox ck in FlowLayoutShipTypes.Controls )
			{
				ck.Checked = check;
			}
			HangFilterChange = false;
			FilterCheckChanged( sender, e );
		}

		void check_CheckedChanged( object sender, EventArgs e )
		{
			if ( HangFilterChange )
				return;

			HangFilterChange = true;
			CheckShipTypeAll.Checked = FlowLayoutShipTypes.Controls.OfType<CheckBox>().All( c => c.Checked );
			HangFilterChange = false;

			FilterCheckChanged( sender, e );
		}

		private void ButtonFilter_Click( object sender, EventArgs e )
		{
			if ( PanelFilter.Visible || ( SelectedTab != null && (int)SelectedTab.Tag == -1 ) )
				PanelFilter.Visible = !PanelFilter.Visible;
		}

		private void FilterChanged( object sender, EventArgs e )
		{
			RadioButton rd = sender as RadioButton;
			if ( rd != null )
			{
				if ( rd.Checked )
					FilterCheckChanged( sender, e );
			}
		}

		private void FilterCheckChanged( object sender, EventArgs e )
		{
			if ( HangFilterChange )
				return;

			int id = ( SelectedTab == null ? 0 : (int)SelectedTab.Tag );
			if ( id < 0 )
				// 全部舰队
				BuildShipView( GetFilteredShips(), null );
		}

		private IEnumerable<ShipData> GetFilteredShips()
		{
			// filter
			var types = FlowLayoutShipTypes.Controls.OfType<CheckBox>().Where( c => c.Checked ).Select( c => (int)c.Tag );
			int level = 0;
			if ( RadioLevel1.Checked )
				level = 1;
			else if ( RadioLevel2Above.Checked )
				level = 2;
			int islock = 0;
			if ( RadioLock.Checked )
				islock = 1;
			else if ( RadioLockNone.Checked )
				islock = -1;
			bool exceptMission = checkBox1.Checked;

			IEnumerable<ShipData> ships;
			if ( level == 1 )
				ships = KCDatabase.Instance.Ships.Values.Where( s => ( s.Level == 1 ) && ( islock == 1 ? s.IsLocked : ( islock == 0 || !s.IsLocked ) ) && types.Contains( s.MasterShip.ShipType ) );
			else
				ships = KCDatabase.Instance.Ships.Values.Where( s => ( s.Level > level ) && ( islock == 1 ? s.IsLocked : ( islock == 0 || !s.IsLocked ) ) && types.Contains( s.MasterShip.ShipType ) );

			if ( exceptMission )
			{
				var missions = KCDatabase.Instance.Fleet.Fleets.Where( f => f.Value.ExpeditionState == 1 ).SelectMany( f => f.Value.Members );
				ships = ships.Where( s => !missions.Contains( s.MasterID ) );
			}

			return ships;
		}

	}
}
