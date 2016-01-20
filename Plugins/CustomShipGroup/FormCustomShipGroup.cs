using CustomShipGroup;
using CustomShipGroup.Model;
using ElectronicObserver.Data;
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
	public partial class FormCustomShipGroup : DockContent {


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

		/// <summary>艦これ起動前にタブを選択した場合はtrue</summary>
		private bool IsTabSelectedBeforeBoot = false;

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

		public FormCustomShipGroup( FormMain parent ) {
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
                label.ForeColor = SystemColors.ControlText;
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
            CSDefaultRight.BackColor = SystemColors.Control;
            CSDefaultRight.Font = Font;
            CSDefaultRight.ForeColor = SystemColors.ControlText;
            CSDefaultRight.SelectionBackColor = Color.FromArgb(0xFF, 0xFF, 0xCC);
            CSDefaultRight.SelectionForeColor = SystemColors.ControlText;

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
			ShipView_Equipment1.DefaultCellStyle = CSDefaultLeft;
			ShipView_Equipment2.DefaultCellStyle = CSDefaultLeft;
			ShipView_Equipment3.DefaultCellStyle = CSDefaultLeft;
			ShipView_Equipment4.DefaultCellStyle = CSDefaultLeft;
			ShipView_Equipment5.DefaultCellStyle = CSDefaultLeft;

            ShipView.ForeColor = SystemColors.ControlText;

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

			CustomShipGroupManager groups = Plugin.ShipGroupManager;


			if ( !groups.ShipGroups.ContainsKey( -1 ) ) {
				var master = new CustomShipGroupData( -1 );
				master.Name = "全所属艦";
				//master.ColumnFilter = Enumerable.Repeat<bool>( true, ShipView.Columns.Count ).ToList();
				master.ColumnFilter = ShipView.Columns.OfType<DataGridViewColumn>().Select( c => c.Visible ).ToList();
				master.ColumnWidth = ShipView.Columns.OfType<DataGridViewColumn>().Select( c => c.Width ).ToList();

				groups.ShipGroups.Add( master );
			}


			foreach ( var g in groups.ShipGroups.Values ) {
				TabPanel.Controls.Add( CreateTabLabel( g.GroupID ) );
			}


			{
				int columnCount = ShipView.Columns.Count;
				for ( int i = 0; i < columnCount; i++ ) {
					ShipView.Columns[i].Visible = false;
				}
			}


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
				// 敌方补给舰不显示 海防舰  超弩级 也不显示
				if ( type.TypeID == 1 )
					continue;
                if (type.TypeID == 12)
                    continue;
                if (type.TypeID == 15)
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

			//splitContainer1.SplitterDistance = config.FormShipGroup.SplitterDistance;
			MenuGroup_AutoUpdate.Checked = config.FormShipGroup.AutoUpdate;
			MenuGroup_ShowStatusBar.Checked = config.FormShipGroup.ShowStatusBar;

		}



		/// <summary>
		/// 指定したグループIDに基づいてタブ ラベルを生成します。
		/// </summary>
		private ImageLabel CreateTabLabel( int id ) {

			ImageLabel label = new ImageLabel();
			label.Text = Plugin.ShipGroupManager[id] != null ? Plugin.ShipGroupManager[id].Name : "全所属艦";
			label.Anchor = AnchorStyles.Left;
			label.Font = ShipView.Font;
			label.BackColor = TabInactiveColor;
            label.ForeColor = SystemColors.ControlText;
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

			if ( IsTabSelectedBeforeBoot )
			{
				// 空のShipViewでKCDatabase.Instance.ShipGroupを上書きしてしまうのを防ぐため、
				// 艦これ起動前にタブを選択した後の最初の艦船データ受信時は、ShipViewの構築を行う
				BuildShipView( SelectedTab );
				IsTabSelectedBeforeBoot = false;

			} else if ( MenuGroup_AutoUpdate.Checked )
				ChangeShipView( SelectedTab );
		}





		/// <summary>
		/// グループデータにGUIからの操作を適用します。
		/// </summary>
		private void ApplyGroupData( ImageLabel target ) {

			if ( target != null ) {

				//ソート順の保持
				if ( KCDatabase.Instance.Ships.Count == 0 )
					return;

				CustomShipGroupData g = Plugin.ShipGroupManager[(int)target.Tag];
				if ( g == null )
					return;

				g.Members.Clear();
				g.Members.Capacity = ShipView.Rows.GetRowCount( DataGridViewElementStates.None );

				foreach ( DataGridViewRow row in ShipView.Rows ) {
					g.Members.Add( (int)row.Cells[ShipView_ID.Index].Value );
				}


				g.ColumnFilter = ShipView.Columns.OfType<DataGridViewColumn>().Select( c => c.Visible ).ToList();
				g.ColumnWidth = ShipView.Columns.OfType<DataGridViewColumn>().Select( c => c.Width ).ToList();
				g.ColumnAutoSize = MenuMember_ColumnAutoSize.Checked;
				g.LockShipNameScroll = MenuMember_LockShipNameScroll.Checked;

				// sort save
				if ( ShipView.SortedColumn != null )
				{
					g.SortColumnName = ShipView.SortedColumn.Name;
					g.SortOrder = (int)ShipView.SortOrder;
				}
			}
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
				new Fraction( ship.Fuel, ship.MasterShip.Fuel ),
				new Fraction( ship.Ammo, ship.MasterShip.Ammo ),
				GetEquipmentTypeID( ship, 0 ),
				GetEquipmentTypeID( ship, 1 ),
				GetEquipmentTypeID( ship, 2 ),
				GetEquipmentTypeID( ship, 3 ),
				GetEquipmentTypeID( ship, 4 ),
				GetEquipmentTypeID( ship, 5 ),
				ship.FleetWithIndex,
				ship.RepairingDockID == -1 ? ship.RepairTime : -1000 + ship.RepairingDockID,
				ship.FirepowerBase,
				ship.FirepowerRemain,
				ship.TorpedoBase,
				ship.TorpedoRemain,
				ship.AABase,
				ship.AARemain,
				ship.ArmorBase,
				ship.ArmorRemain,
				ship.ASWBase,
				ship.EvasionBase,
				ship.LOSBase,
				ship.LuckBase,
				ship.LuckRemain,
				ship.IsLocked,
				ship.SallyArea
				);

			row.Cells[ShipView_Equipment1.Index].ToolTipText = GetEquipmentString( ship, 0 );
			row.Cells[ShipView_Equipment2.Index].ToolTipText = GetEquipmentString( ship, 1 );
			row.Cells[ShipView_Equipment3.Index].ToolTipText = GetEquipmentString( ship, 2 );
			row.Cells[ShipView_Equipment4.Index].ToolTipText = GetEquipmentString( ship, 3 );
			row.Cells[ShipView_Equipment5.Index].ToolTipText = GetEquipmentString( ship, 4 );
			var ex = ship.ExpansionSlotInstance;
			row.Cells[ShipView_EquipmentEx.Index].ToolTipText = ( ex != null ? ex.NameWithLevel : ( ship.ExpansionSlot < 0 ? "(无)" : "" ) );

			row.Cells[ShipView_Firepower.Index].ToolTipText = ship.FirepowerRemain > 0 ? ( "剩余: " + ship.FirepowerRemain ) : null;
			row.Cells[ShipView_Torpedo.Index].ToolTipText = ship.TorpedoRemain > 0 ? ( "剩余: " + ship.TorpedoRemain ) : null;
			row.Cells[ShipView_AA.Index].ToolTipText = ship.AARemain > 0 ? ( "剩余: " + ship.AARemain ) : null;
			row.Cells[ShipView_Armor.Index].ToolTipText = ship.ArmorRemain > 0 ? ( "剩余: " + ship.ArmorRemain ) : null;
			row.Cells[ShipView_Luck.Index].ToolTipText = ship.LuckRemain > 0 ? ( "剩余: " + ship.LuckRemain ) : null;

			row.Cells[ShipView_Name.Index].Tag = ship.ShipID;
			row.Cells[ShipView_Level.Index].Tag = ship.ExpTotal;

            //foreach(var cell in row.Cells)
            //{
            //    ((DataGridViewCell)cell).Style = CSDefaultRight;
            //}
			{
				DataGridViewCellStyle cs;
				double hprate = (double)ship.HPCurrent / Math.Max( ship.HPMax, 1 );
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
			row.Cells[ShipView_Fuel.Index].Style = ship.Fuel < ship.MasterShip.Fuel ? CSYellowRight : CSDefaultRight;
			row.Cells[ShipView_Ammo.Index].Style = ship.Fuel < ship.MasterShip.Fuel ? CSYellowRight : CSDefaultRight;
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

			CustomShipGroupData group = Plugin.ShipGroupManager[groupID];
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

		private void BuildShipView( IEnumerable<ShipData> ships, CustomShipGroupData g )
		{
			ShipView.SuspendLayout();

			ShipView.Rows.Clear();

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
				{
					int columnCount = ShipView.Columns.Count;
					if ( g.ColumnFilter != null ) columnCount = Math.Min( columnCount, g.ColumnFilter.Count );
					if ( g.ColumnWidth != null ) columnCount = Math.Min( columnCount, g.ColumnWidth.Count );


					for ( int i = 0; i < columnCount; i++ )
					{
						ShipView.Columns[i].Visible = g.ColumnFilter[i];
						ShipView.Columns[i].Width = g.ColumnWidth[i];
					}
				}

				SetColumnAutoSize( g.ColumnAutoSize );
				SetLockShipNameScroll( g.LockShipNameScroll );

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
			var group = Plugin.ShipGroupManager[groupID];


			ApplyGroupData( SelectedTab );


			if ( group == null ) {
				Utility.Logger.Add( 3, "エラー：存在しないグループを参照しようとしました。開発者に連絡してください" );
				return;
			}
			if ( group.GroupID < 0 ) {
				group.Members = group.Members.Intersect( KCDatabase.Instance.Ships.Keys ).Union( KCDatabase.Instance.Ships.Keys ).Distinct().ToList();
			}

			if ( SelectedTab != null )
				SelectedTab.BackColor = TabInactiveColor;

			SelectedTab = target;
			// 艦これ起動前にタブを選択した場合はフラグを立てておく(APIUpdatedで使う)
			if ( KCDatabase.Instance.Ships.Count == 0 )
				IsTabSelectedBeforeBoot = true;

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

			int current = ship.Aircraft[index];
			int max = ship.MasterShip.Aircraft[index];
			string name = ship.SlotInstance[index] != null ? ship.SlotInstance[index].NameWithLevel : "(无)";

			if ( index >= ship.MasterShip.SlotSize && ship.Slot[index] == -1 ) {
				return "";

			} else if ( max == 0 ) {
				return name;

			} else if ( current == max ) {
				return string.Format( "[{0}] {1}", current, name );

			} else {
				return string.Format( "[{0}/{1}] {2}", current, max, name );

			}

		}

		private string GetEquipmentOnlyString( ShipData ship, int index ) {

			string name = ship.SlotInstance[index] != null ? ship.SlotInstance[index].NameWithLevel : "(无)";

			if ( index >= ship.MasterShip.SlotSize && ship.Slot[index] == -1 ) {
				return "";

			} else {
				return name;

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

			} else if ( e.ColumnIndex == ShipView_Locked.Index ) {
				e.Value = (bool)e.Value ? "❤" : "";
				e.FormattingApplied = true;

			} else if ( e.ColumnIndex == ShipView_SallyArea.Index && (int)e.Value == -1 ) {
				e.Value = "";
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

			} else if ( e.Column.Index == ShipView_Level.Index ) {
				e.SortResult = (int)ShipView.Rows[e.RowIndex1].Cells[e.Column.Index].Tag - (int)ShipView.Rows[e.RowIndex2].Cells[e.Column.Index].Tag;	//exptotal
				if ( e.SortResult == 0 )	//for Lv.99-100
					e.SortResult = (int)e.CellValue1 - (int)e.CellValue2;

			} else if (
				e.Column.Index == ShipView_HP.Index ||
				e.Column.Index == ShipView_Fuel.Index ||
				e.Column.Index == ShipView_Ammo.Index
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

			} else if ( e.Column.Index == ShipView_Locked.Index ) {
				e.SortResult = ( (bool)e.CellValue1 ? 1 : 0 ) - ( (bool)e.CellValue2 ? 1 : 0 );

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

					var group = Plugin.ShipGroupManager.Add();

					group.Name = dialog.InputtedText.Trim();
					group.ColumnFilter = ShipView.Columns.OfType<DataGridViewColumn>().Select( c => c.Visible ).ToList();
					if ( group.ColumnFilter.All( f => !f ) )
						group.ColumnFilter = Enumerable.Repeat<bool>( true, ShipView.Columns.Count ).ToList();
					group.ColumnWidth = ShipView.Columns.OfType<DataGridViewColumn>().Select( c => c.Width ).ToList();
					group.ColumnAutoSize = MenuMember_ColumnAutoSize.Checked;

					TabPanel.Controls.Add( CreateTabLabel( group.GroupID ) );

				}

			}

		}

		private void MenuGroup_Delete_Click( object sender, EventArgs e ) {

			ImageLabel senderLabel = MenuGroup.SourceControl as ImageLabel;
			if ( senderLabel == null )
				return;		//想定外

			CustomShipGroupData group = Plugin.ShipGroupManager[(int)senderLabel.Tag];

			if ( group != null && group.GroupID >= 0 ) {
				if ( MessageBox.Show( string.Format( "要删除分组 [{0}] 吗？\r\n此操作无法撤销。", group.Name ), "确认",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 )
					== System.Windows.Forms.DialogResult.Yes ) {

					if ( SelectedTab == senderLabel ) {
						ShipView.Rows.Clear();
						SelectedTab = null;
					}
					Plugin.ShipGroupManager.ShipGroups.Remove( group );
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

			CustomShipGroupData group = Plugin.ShipGroupManager[(int)senderLabel.Tag];

			if ( group != null && group.GroupID >= 0 ) {

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
				MenuGroup_Rename.Enabled = false;
				MenuGroup_Delete.Enabled = false;
			} else {
				MenuGroup_Add.Enabled = true;
				MenuGroup_Rename.Enabled = true;
				MenuGroup_Delete.Enabled = true;
			}

		}

		private void MenuMember_Opening( object sender, CancelEventArgs e ) {

			if ( SelectedTab == null ) {

				e.Cancel = true;
				return;

			} else if ( ShipView.Rows.GetRowCount( DataGridViewElementStates.Selected ) == 0 ) {

				MenuMember_AddToGroup.Enabled = false;
				MenuMember_CreateGroup.Enabled = false;
				MenuMember_Delete.Enabled = false;
				MenuMember_CSVOutput.Enabled = false;

			} else if ( Plugin.ShipGroupManager.ShipGroups.Count == 0 ) {

				MenuMember_AddToGroup.Enabled = false;
				MenuMember_CreateGroup.Enabled = true;
				MenuMember_Delete.Enabled = true;
				MenuMember_CSVOutput.Enabled = false;

			} else {

				MenuMember_AddToGroup.Enabled = true;
				MenuMember_CreateGroup.Enabled = true;
				MenuMember_Delete.Enabled = true;
				MenuMember_CSVOutput.Enabled = true;

			}

			// 「現在の艦隊を追加」コンテキストメニュー
			{
				int groupID = (int)SelectedTab.Tag;

				// 艦隊がロード済み && 選択中のタブが全所属艦以外の場合にEnabled
				MenuMember_AddCurrentFleet_Group.Enabled = ( KCDatabase.Instance.Fleet.Fleets.Count > 0 ) && ( groupID >= 0 );

				if ( MenuMember_AddCurrentFleet_Group.Enabled ) {

					MenuMember_AddCurrentFleet_Group.DropDownItems.Clear();
					foreach ( FleetData fleet in KCDatabase.Instance.Fleet.Fleets.Values ) {

						if ( fleet.MembersInstance.Count( s => s != null ) == 0 ) continue;

						var newItem = new ToolStripMenuItem();
						newItem.Name = "MenuMember_AddCurrentFleetChild_" + fleet.FleetID;
						newItem.Text = string.Format( "#&{0} {1}", fleet.FleetID, fleet.Name );
						newItem.Tag = fleet.FleetID;
						newItem.Click += MenuMember_AddCurrentFleetChild_Click;
						MenuMember_AddCurrentFleet_Group.DropDownItems.Add( newItem );
					}
				}
			}
		}
		#endregion


		#region メニュー:メンバー操作

		private void MenuMember_AddToGroup_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogTextSelect( "分组选择", "请选择要添加到的分组：",
				Plugin.ShipGroupManager.ShipGroups.Values.Where( g => g.GroupID >= 0 ).ToArray() ) ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

					CustomShipGroupData group = (CustomShipGroupData)dialog.SelectedItem;
					if ( group != null && group.GroupID >= 0 ) {

						List<int> members = new List<int>( ShipView.Rows.GetRowCount( DataGridViewElementStates.Selected ) );

						foreach ( DataGridViewRow row in ShipView.SelectedRows.OfType<DataGridViewRow>().OrderBy( r => r.Tag ) ) {

							members.Add( (int)row.Cells[ShipView_ID.Index].Value );
						}

						group.Members.AddRange( members );
						group.CheckMembers();
					}
				}
			}

		}


		private void MenuMember_CreateGroup_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogTextInput( "添加分组", "请输入分组名：" ) ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

					var group = Plugin.ShipGroupManager.Add();

					group.Name = dialog.InputtedText.Trim();
					foreach ( DataGridViewRow row in ShipView.SelectedRows.OfType<DataGridViewRow>().OrderBy( r => r.Tag ) ) {

						group.Members.Add( (int)row.Cells[ShipView_ID.Index].Value );
					}
					group.ColumnFilter = ShipView.Columns.OfType<DataGridViewColumn>().Select( c => c.Visible ).ToList();
					group.ColumnWidth = ShipView.Columns.OfType<DataGridViewColumn>().Select( c => c.Width ).ToList();
					group.ColumnAutoSize = MenuMember_ColumnAutoSize.Checked;

					ImageLabel il = CreateTabLabel( group.GroupID );
					TabPanel.Controls.Add( il );

					ChangeShipView( il );
				}
			}

		}


		private void MenuMember_Delete_Click( object sender, EventArgs e ) {

			CustomShipGroupData group = SelectedTab != null ? Plugin.ShipGroupManager[(int)SelectedTab.Tag] : null;


			if ( group == null || group.GroupID < 0 ) {
				MessageBox.Show( "此分组无法修改。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk );
				return;
			}

			List<int> list = new List<int>( ShipView.Rows.GetRowCount( DataGridViewElementStates.Selected ) );

			foreach ( DataGridViewRow row in ShipView.SelectedRows ) {
				list.Add( (int)row.Cells[ShipView_ID.Index].Value );
				ShipView.Rows.Remove( row );
			}

			if ( group != null )
				group.Members = group.Members.Except( list ).ToList();

		}


		private void MenuMember_ColumnFilter_Click( object sender, EventArgs e ) {

			CustomShipGroupData group = SelectedTab != null ? Plugin.ShipGroupManager[(int)SelectedTab.Tag] : null;

			if ( group == null ) {
				MessageBox.Show( "此分组无法修改。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk );
				return;
			}


			using ( var dialog = new CustomDialogShipGroupColumnFilter( ShipView ) ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {



					bool[] checkedList = dialog.CheckedList;

					group.ColumnFilter = checkedList.ToList();
					for ( int i = 0; i < checkedList.Length; i++ ) {
						ShipView.Columns[i].Visible = checkedList[i];
					}

				}
			}

		}





		#region ColumnHeader
		private readonly string[] ShipCSVHeaderUser = {
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
			"入渠",
			"火力",
			"火力改修",
			"雷装",
			"雷装改修",
			"対空",
			"対空改修",
			"装甲",
			"装甲改修",
			"対潜",
			"回避",
			"索敵",
			"運",
			"運改修",
			"射程",
			"ロック",
			"出撃先"
			};

		private readonly string[] ShipCSVHeaderData = {
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
			"艦載機1",
			"艦載機2",
			"艦載機3",
			"艦載機4",
			"艦載機5",
			"入渠",
			"火力",
			"火力改修",
			"雷装",
			"雷装改修",
			"対空",
			"対空改修",
			"装甲",
			"装甲改修",
			"対潜",
			"回避",
			"索敵",
			"運",
			"運改修",
			"射程",
			"ロック",
			"出撃先"
			};

		#endregion

		private void MenuMember_CSVOutput_Click( object sender, EventArgs e ) {

			IEnumerable<ShipData> ships;
			ImageLabel senderLabel = MenuGroup.SourceControl as ImageLabel;
			if ( senderLabel == null ) {
				ships = KCDatabase.Instance.Ships.Values;

			} else {
				CustomShipGroupData group = Plugin.ShipGroupManager[(int)senderLabel.Tag];
				if ( group != null && group.GroupID >= 0 ) {
					ships = group.MembersInstance;

				} else {
					ships = KCDatabase.Instance.Ships.Values;
				}

			}



			using ( var dialog = new CustomDialogShipGroupCSVOutput() ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

					try {

						using ( StreamWriter sw = new StreamWriter( dialog.OutputPath, false, Utility.Configuration.Config.Log.FileEncoding ) ) {

							string[] header = dialog.OutputFormat == CustomDialogShipGroupCSVOutput.OutputFormatConstants.User ? ShipCSVHeaderUser : ShipCSVHeaderData;

							sw.WriteLine( string.Join( ",", header ) );

							string arg = string.Format( "{{{0}}}", string.Join( "},{", Enumerable.Range( 0, header.Length ) ) );

							foreach ( ShipData ship in ships ) {

								if ( ship == null ) continue;


								if ( dialog.OutputFormat == CustomDialogShipGroupCSVOutput.OutputFormatConstants.User ) {

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
										DateTimeHelper.ToTimeRemainString( DateTimeHelper.FromAPITimeSpan( ship.RepairTime ) ),
										ship.FirepowerBase,
										ship.FirepowerRemain,
										ship.TorpedoBase,
										ship.TorpedoRemain,
										ship.AABase,
										ship.AARemain,
										ship.ArmorBase,
										ship.ArmorRemain,
										ship.ASWBase,
										ship.EvasionBase,
										ship.LOSBase,
										ship.LuckBase,
										ship.LuckRemain,
										Constants.GetRange( ship.Range ),
										ship.IsLocked ? "❤" : "-",
										ship.SallyArea );

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
										GetEquipmentOnlyString( ship, 0 ),		//undone: IDにしたいけどよく考えたら強化値が反映されない
										GetEquipmentOnlyString( ship, 1 ),
										GetEquipmentOnlyString( ship, 2 ),
										GetEquipmentOnlyString( ship, 3 ),
										GetEquipmentOnlyString( ship, 4 ),
										ship.Aircraft[0],
										ship.Aircraft[1],
										ship.Aircraft[2],
										ship.Aircraft[3],
										ship.Aircraft[4],
										ship.RepairTime * 10000,
										ship.FirepowerBase,
										ship.FirepowerRemain,
										ship.TorpedoBase,
										ship.TorpedoRemain,
										ship.AABase,
										ship.AARemain,
										ship.ArmorBase,
										ship.ArmorRemain,
										ship.ASWBase,
										ship.EvasionBase,
										ship.LOSBase,
										ship.LuckBase,
										ship.LuckRemain,
										ship.Range,
										ship.IsLocked ? 1 : 0,
										ship.SallyArea
										);

								}

							}


						}

					} catch ( Exception ex ) {

						Utility.ErrorReporter.SendErrorReport( ex, "舰船分组输出CSV失败。" );
						MessageBox.Show( "舰船分组输出CSV失败。\r\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error );

					}



				}
			}

		}


		private void MenuMember_ColumnAutoSize_Click( object sender, EventArgs e ) {

			SetColumnAutoSize();

		}


		/// <summary>
		/// 列の自動調整設定を適用します。
		/// </summary>
		/// <param name="flag">設定。nullなら既定値を、そうでなければその値を設定します。</param>
		private void SetColumnAutoSize( bool? flag = null ) {

			if ( flag == null )
				flag = MenuMember_ColumnAutoSize.Checked;
			else
				MenuMember_ColumnAutoSize.Checked = (bool)flag;

			if ( flag == true ) {
				ShipView.AutoResizeColumns( DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader );
			}

			foreach ( DataGridViewColumn column in ShipView.Columns ) {
				column.AutoSizeMode = flag == true ? DataGridViewAutoSizeColumnMode.AllCellsExceptHeader : DataGridViewAutoSizeColumnMode.NotSet;
			}
		}


		private void MenuMember_LockShipNameScroll_Click( object sender, EventArgs e ) {

			SetLockShipNameScroll();
		}


		/// <summary>
		/// 艦名をスクロールロックするかの設定を適用します。
		/// </summary>
		/// <param name="flag">設定。nullなら既定値を、そうでなければその値を設定します。</param>
		private void SetLockShipNameScroll( bool? flag = null ) {

			if ( flag == null )
				flag = MenuMember_LockShipNameScroll.Checked;
			else
				MenuMember_LockShipNameScroll.Checked = (bool)flag;

			ShipView_ID.Frozen = flag == true;
			ShipView_ShipType.Frozen = flag == true;
			ShipView_Name.Frozen = flag == true;

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


		/// <summary>
		/// 現在の艦隊を表示中のグループに追加する。
		/// 「このグループに現在の艦隊を追加」の子項目をクリックした時に実行。
		/// </summary>
		/// <param name="sender">追加する艦隊。senderのTagに艦隊IDを格納すること。</param>
		/// <param name="e"></param>
		private void MenuMember_AddCurrentFleetChild_Click( object sender, EventArgs e ) {
			if ( SelectedTab == null )
				return;

			FleetData fleet = KCDatabase.Instance.Fleet[(int)( (ToolStripItem)sender ).Tag];
			if ( fleet == null ) return;

			// ShipViewに追加
			var members = fleet.MembersInstance.Where( s => s != null ).ToList();
			var rows = new List<DataGridViewRow>( members.Count );
			foreach ( var ship in members ) {
				rows.Add( CreateShipViewRow( ship ) );
			}
			ShipView.Rows.AddRange( rows.ToArray() );
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


			//Utility.Configuration.Config.FormShipGroup.SplitterDistance = splitContainer1.SplitterDistance;
			Utility.Configuration.Config.FormShipGroup.AutoUpdate = MenuGroup_AutoUpdate.Checked;
			Utility.Configuration.Config.FormShipGroup.ShowStatusBar = MenuGroup_ShowStatusBar.Checked;


			//以下は実データがないと動作しないためなければスキップ
			if ( KCDatabase.Instance.Ships.Count == 0 ) return;

			CustomShipGroupManager groups = Plugin.ShipGroupManager;


			if ( SelectedTab != null )
				ApplyGroupData( SelectedTab );


			List<ImageLabel> list = TabPanel.Controls.OfType<ImageLabel>().OrderBy( c => TabPanel.Controls.GetChildIndex( c ) ).ToList();

			for ( int i = 0; i < list.Count; i++ ) {

				CustomShipGroupData group = groups[(int)list[i].Tag];
				if ( group != null && group.GroupID >= 0 )
					group.GroupID = i + 1;
			}

		}


		public override string GetPersistString()
		{
			return "CustomShipGroup";
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