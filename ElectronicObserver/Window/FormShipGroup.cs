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

		//セルスタイル
		private DataGridViewCellStyle CSDefaultLeft, CSDefaultCenter, CSDefaultRight,
			CSRedRight, CSOrangeRight, CSYellowRight, CSGreenRight, CSGrayRight, CSCherryRight,
			CSIsLocked;

		/// <summary>選択中のタブ</summary>
		private ImageLabel SelectedTab = null;

		/// <summary>選択中のグループ</summary>
		private ShipGroupData CurrentGroup {
			get {
				return SelectedTab == null ? null : KCDatabase.Instance.ShipGroup[(int)SelectedTab.Tag];
			}
		}

		private bool IsRowsUpdating;
		private int _splitterDistance;


		public FormShipGroup( FormMain parent ) {
			InitializeComponent();

			ControlHelper.SetDoubleBuffered( ShipView );

			IsRowsUpdating = true;
			_splitterDistance = -1;

			foreach ( DataGridViewColumn column in ShipView.Columns ) {
				column.MinimumWidth = 2;
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

			#endregion


			SystemEvents.SystemShuttingDown += SystemShuttingDown;
		}


		private void FormShipGroup_Load( object sender, EventArgs e ) {

			ShipGroupManager groups = KCDatabase.Instance.ShipGroup;


			// 空(≒初期状態)の時、おなじみ全所属艦を追加
			if ( groups.ShipGroups.Count == 0 ) {

				var group = KCDatabase.Instance.ShipGroup.Add();
				group.Name = "全所属艦";

				for ( int i = 0; i < ShipView.Columns.Count; i++ ) {
					var newdata = new ShipGroupData.ViewColumnData( ShipView.Columns[i] );
					if ( SelectedTab == null )
						newdata.Visible = true;		//初期状態では全行が非表示のため
					group.ViewColumns.Add( ShipView.Columns[i].Name, newdata );
				}
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

			IsRowsUpdating = false;
			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormShipGroup] );

		}


		void ConfigurationChanged() {

			var config = Utility.Configuration.Config;

			ShipView.Font = StatusBar.Font = Font = config.UI.MainFont;

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

			foreach ( System.Windows.Forms.Control c in TabPanel.Controls )
				c.Font = Font;

			MenuGroup_AutoUpdate.Checked = config.FormShipGroup.AutoUpdate;
			MenuGroup_ShowStatusBar.Checked = config.FormShipGroup.ShowStatusBar;

		}


		// レイアウトロード時に呼ばれる
		public void ConfigureFromPersistString( string persistString ) {

			string[] args = persistString.Split( "?=&".ToCharArray() );

			for ( int i = 1; i < args.Length - 1; i += 2 ) {
				switch ( args[i] ) {
					case "SplitterDistance":
						// 直接変えるとサイズが足りないか何かで変更が適用されないことがあるため、 Resize イベント中に変更する(ために値を記録する)
						// しかし Resize イベントだけだと呼ばれないことがあるため、直接変えてもおく
						// つらい
						splitContainer1.SplitterDistance = _splitterDistance = int.Parse( args[i + 1] );
						break;
				}
			}
		}

		public override string GetPersistString() {
			return "ShipGroup?SplitterDistance=" + splitContainer1.SplitterDistance;
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

			return label;
		}




		void TabLabel_Click( object sender, EventArgs e ) {
			ChangeShipView( sender as ImageLabel );
		}

		private void APIUpdated( string apiname, dynamic data ) {
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
				GetEquipmentString( ship, 0 ),
				GetEquipmentString( ship, 1 ),
				GetEquipmentString( ship, 2 ),
				GetEquipmentString( ship, 3 ),
				GetEquipmentString( ship, 4 ),
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

			ShipGroupData group = KCDatabase.Instance.ShipGroup[(int)target.Tag];

			IsRowsUpdating = true;
			ShipView.SuspendLayout();

			UpdateMembers( group );

			ShipView.Rows.Clear();

			var ships = group.MembersInstance;
			var rows = new List<DataGridViewRow>( ships.Count() );

			foreach ( ShipData ship in ships ) {

				if ( ship == null ) continue;

				DataGridViewRow row = CreateShipViewRow( ship );
				rows.Add( row );

			}

			for ( int i = 0; i < rows.Count; i++ )
				rows[i].Tag = i;

			ShipView.Rows.AddRange( rows.ToArray() );



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
			}


			ApplyViewData( group );
			ApplyAutoSort( group );

			ShipView.ResumeLayout();
			IsRowsUpdating = false;

			//status bar
			if ( KCDatabase.Instance.Ships.Count > 0 ) {
				Status_ShipCount.Text = string.Format( "所属: {0}隻", group.Members.Count );
				Status_LevelTotal.Text = string.Format( "合計Lv: {0}", group.MembersInstance.Where( s => s != null ).Sum( s => s.Level ) );
				Status_LevelAverage.Text = string.Format( "平均Lv: {0:F2}", group.Members.Count > 0 ? group.MembersInstance.Where( s => s != null ).Average( s => s.Level ) : 0 );
			}
		}


		/// <summary>
		/// ShipViewを指定したタブに切り替えます。
		/// </summary>
		private void ChangeShipView( ImageLabel target ) {

			if ( target == null ) return;


			var group = KCDatabase.Instance.ShipGroup[(int)target.Tag];
			var currentGroup = CurrentGroup;

			int headIndex = 0;
			List<int> selectedIDList = new List<int>();

			if ( group == null ) {
				Utility.Logger.Add( 3, "エラー：存在しないグループを参照しようとしました。開発者に連絡してください" );
				return;
			}

			if ( currentGroup != null ) {

				UpdateMembers( currentGroup );

				if ( CurrentGroup.GroupID != group.GroupID ) {
					ShipView.Rows.Clear();		//別グループの行の並び順を引き継がせないようにする

				} else {
					headIndex = ShipView.FirstDisplayedScrollingRowIndex;
					selectedIDList = ShipView.SelectedRows.Cast<DataGridViewRow>().Select( r => (int)r.Cells[ShipView_ID.Index].Value ).ToList();
				}
			}


			if ( SelectedTab != null )
				SelectedTab.BackColor = TabInactiveColor;

			SelectedTab = target;


			BuildShipView( SelectedTab );
			SelectedTab.BackColor = TabActiveColor;


			if ( 0 <= headIndex && headIndex < ShipView.Rows.Count )
				ShipView.FirstDisplayedScrollingRowIndex = headIndex;

			if ( selectedIDList.Count > 0 ) {
				ShipView.ClearSelection();
				for ( int i = 0; i < ShipView.Rows.Count; i++ ) {
					var row = ShipView.Rows[i];
					if ( selectedIDList.Contains( (int)row.Cells[ShipView_ID.Index].Value ) ) {
						row.Selected = true;
					}
				}
			}

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


		/// <summary>
		/// 現在選択している艦船のIDリストを求めます。
		/// </summary>
		private IEnumerable<int> GetSelectedShipID() {
			return ShipView.SelectedRows.Cast<DataGridViewRow>().OrderBy( r => r.Index ).Select( r => (int)r.Cells[ShipView_ID.Index].Value );
		}


		/// <summary>
		/// 現在の表を基に、グループメンバーを更新します。
		/// </summary>
		private void UpdateMembers( ShipGroupData group ) {
			group.UpdateMembers( ShipView.Rows.Cast<DataGridViewRow>().Select( r => (int)r.Cells[ShipView_ID.Index].Value ) );
		}


		private void ShipView_SelectionChanged( object sender, EventArgs e ) {

			var group = CurrentGroup;
			if ( KCDatabase.Instance.Ships.Count > 0 && group != null ) {
				if ( ShipView.Rows.GetRowCount( DataGridViewElementStates.Selected ) >= 2 ) {
					var levels = ShipView.SelectedRows.Cast<DataGridViewRow>().Select( r => (int)r.Cells[ShipView_Level.Index].Value );
					Status_ShipCount.Text = string.Format( "選択: {0} / {1}隻", ShipView.Rows.GetRowCount( DataGridViewElementStates.Selected ), group.Members.Count );
					Status_LevelTotal.Text = string.Format( "合計Lv: {0}", levels.Sum() );
					Status_LevelAverage.Text = string.Format( "平均Lv: {0:F2}", levels.Average() );


				} else {
					Status_ShipCount.Text = string.Format( "所属: {0}隻", group.Members.Count );
					Status_LevelTotal.Text = string.Format( "合計Lv: {0}", group.MembersInstance.Where( s => s != null ).Sum( s => s.Level ) );
					Status_LevelAverage.Text = string.Format( "平均Lv: {0:F2}", group.Members.Count > 0 ? group.MembersInstance.Where( s => s != null ).Average( s => s.Level ) : 0 );
				}

			} else {
				Status_ShipCount.Text =
				Status_LevelTotal.Text =
				Status_LevelAverage.Text = "";
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
				if ( (string)e.CellValue1 == "" ) {
					if ( (string)e.CellValue2 == "" )
						e.SortResult = 0;
					else
						e.SortResult = 1;
				} else {
					if ( (string)e.CellValue2 == "" )
						e.SortResult = -1;
					else
						e.SortResult = ( (string)e.CellValue1 ).CompareTo( e.CellValue2 );
				}

			} else {
				e.SortResult = (int)e.CellValue1 - (int)e.CellValue2;
			}



			if ( e.SortResult == 0 ) {
				e.SortResult = (int)ShipView.Rows[e.RowIndex1].Tag - (int)ShipView.Rows[e.RowIndex2].Tag;

				if ( ShipView.SortOrder == SortOrder.Descending )
					e.SortResult = -e.SortResult;
			}

			e.Handled = true;
		}



		private void ShipView_Sorted( object sender, EventArgs e ) {

			int count = ShipView.Rows.Count;
			var direction = ShipView.SortOrder;

			for ( int i = 0; i < count; i++ )
				ShipView.Rows[i].Tag = i;

		}





		// 列のサイズ変更関連
		private void ShipView_ColumnWidthChanged( object sender, DataGridViewColumnEventArgs e ) {

			if ( IsRowsUpdating )
				return;

			var group = CurrentGroup;
			if ( group != null ) {

				if ( !group.ViewColumns[e.Column.Name].AutoSize ) {
					group.ViewColumns[e.Column.Name].Width = e.Column.Width;
				}
			}

		}

		private void ShipView_ColumnDisplayIndexChanged( object sender, DataGridViewColumnEventArgs e ) {

			if ( IsRowsUpdating )
				return;

			var group = CurrentGroup;
			if ( group != null ) {

				foreach ( DataGridViewColumn column in ShipView.Columns ) {
					group.ViewColumns[column.Name].DisplayIndex = column.DisplayIndex;
				}
			}

		}




		#region メニュー:グループ操作

		private void MenuGroup_Add_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogTextInput( "グループを追加", "グループ名を入力してください：" ) ) {

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
				if ( MessageBox.Show( string.Format( "グループ [{0}] を削除しますか？\r\nこの操作は元に戻せません。", group.Name ), "確認",
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
				MessageBox.Show( "このグループは削除できません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
			}
		}

		private void MenuGroup_Rename_Click( object sender, EventArgs e ) {

			ImageLabel senderLabel = MenuGroup.SourceControl as ImageLabel;
			if ( senderLabel == null ) return;

			ShipGroupData group = KCDatabase.Instance.ShipGroup[(int)senderLabel.Tag];

			if ( group != null ) {

				using ( var dialog = new DialogTextInput( "グループ名の変更", "グループ名を入力してください：" ) ) {

					dialog.InputtedText = group.Name;

					if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

						group.Name = senderLabel.Text = dialog.InputtedText.Trim();

					}
				}

			} else {
				MessageBox.Show( "このグループの名前を変更することはできません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
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

			if ( KCDatabase.Instance.Ships.Count == 0 ) {
				MenuMember_Filter.Enabled = false;
				MenuMember_CSVOutput.Enabled = false;

			} else {
				MenuMember_Filter.Enabled = true;
				MenuMember_CSVOutput.Enabled = true;
			}

			if ( ShipView.Rows.GetRowCount( DataGridViewElementStates.Selected ) == 0 ) {
				MenuMember_AddToGroup.Enabled = false;
				MenuMember_CreateGroup.Enabled = false;
				MenuMember_Exclude.Enabled = false;

			} else {
				MenuMember_AddToGroup.Enabled = true;
				MenuMember_CreateGroup.Enabled = true;
				MenuMember_Exclude.Enabled = true;

			}

		}
		#endregion


		#region メニュー:メンバー操作

		private void MenuMember_ColumnFilter_Click( object sender, EventArgs e ) {

			ShipGroupData group = CurrentGroup;

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

			var group = CurrentGroup;
			if ( group != null ) {

				try {
					if ( group.Expressions == null )
						group.Expressions = new ExpressionManager();

					using ( var dialog = new DialogShipGroupFilter( group ) ) {

						if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

							// replace
							int id = group.GroupID;
							group = dialog.ExportGroupData();
							group.GroupID = id;
							group.Expressions.Compile();

							KCDatabase.Instance.ShipGroup.ShipGroups.Remove( id );
							KCDatabase.Instance.ShipGroup.ShipGroups.Add( group );

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

			IsRowsUpdating = true;

			// いったん解除しないと列入れ替え時にエラーが起きる
			foreach ( DataGridViewColumn column in ShipView.Columns ) {
				column.Frozen = false;
			}

			foreach ( var data in group.ViewColumns.Values.OrderBy( g => g.DisplayIndex ) ) {
				data.ToColumn( ShipView.Columns[data.Name] );
			}

			int count = 0;
			foreach ( var column in ShipView.Columns.Cast<DataGridViewColumn>().OrderBy( c => c.DisplayIndex ) ) {
				column.Frozen = count < group.ScrollLockColumnCount;
				count++;
			}

			IsRowsUpdating = false;
		}


		private void MenuMember_SortOrder_Click( object sender, EventArgs e ) {

			var group = CurrentGroup;

			if ( group != null ) {

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

			// 一番上/最後に実行したほうが優先度が高くなるので逆順で
			for ( int i = group.SortOrder.Count - 1; i >= 0; i-- ) {

				var order = group.SortOrder[i];
				ListSortDirection dir = order.Value;

				if ( ShipView.Columns[order.Key].SortMode != DataGridViewColumnSortMode.NotSortable )
					ShipView.Sort( ShipView.Columns[order.Key], dir );
			}


		}





		private void MenuMember_AddToGroup_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogTextSelect( "グループの選択", "追加するグループを選択してください：",
				KCDatabase.Instance.ShipGroup.ShipGroups.Values.ToArray() ) ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

					var group = (ShipGroupData)dialog.SelectedItem;
					if ( group != null ) {
						group.AddInclusionFilter( GetSelectedShipID() );

						if ( group.ID == CurrentGroup.ID )
							ChangeShipView( SelectedTab );
					}

				}
			}

		}

		private void MenuMember_CreateGroup_Click( object sender, EventArgs e ) {

			var ships = GetSelectedShipID();
			if ( ships.Count() == 0 )
				return;

			using ( var dialog = new DialogTextInput( "グループの追加", "グループ名を入力してください：" ) ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

					var group = KCDatabase.Instance.ShipGroup.Add();

					group.Name = dialog.InputtedText.Trim();

					for ( int i = 0; i < ShipView.Columns.Count; i++ ) {
						var newdata = new ShipGroupData.ViewColumnData( ShipView.Columns[i] );
						if ( SelectedTab == null )
							newdata.Visible = true;		//初期状態では全行が非表示のため
						group.ViewColumns.Add( ShipView.Columns[i].Name, newdata );
					}

					// 艦船ID == 0 を作成(空リストを作る)
					group.Expressions.Expressions.Add( new ExpressionList( false, true, false ) );
					group.Expressions.Expressions[0].Expressions.Add( new ExpressionData( ".MasterID", ExpressionData.ExpressionOperator.Equal, 0 ) );
					group.Expressions.Compile();

					group.AddInclusionFilter( ships );

					TabPanel.Controls.Add( CreateTabLabel( group.GroupID ) );

				}

			}
		}

		private void MenuMember_Exclude_Click( object sender, EventArgs e ) {

			var group = CurrentGroup;
			if ( group != null ) {

				group.AddExclusionFilter( GetSelectedShipID() );

				ChangeShipView( SelectedTab );
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
										ship.IsLocked ? "●" : ship.IsLockedByEquipment ? "■" : "-",
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

						Utility.ErrorReporter.SendErrorReport( ex, "艦船グループ CSVの出力に失敗しました。" );
						MessageBox.Show( "艦船グループ CSVの出力に失敗しました。\r\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );

					}



				}
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


		private void FormShipGroup_Resize( object sender, EventArgs e ) {
			if ( _splitterDistance != -1 && splitContainer1.Height > 0 ) {
				try {
					splitContainer1.SplitterDistance = _splitterDistance;
					_splitterDistance = -1;

				} catch ( Exception ) {
					// *ぷちっ*
				}
			}
		}


	}
}
