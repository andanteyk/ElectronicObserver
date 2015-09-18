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
	public partial class FormShipGroup : DockContent {


		/// <summary>タブ背景色(アクティブ)</summary>
		private readonly Color TabActiveColor = Color.FromArgb( 0xFF, 0xFF, 0xCC );

		/// <summary>タブ背景色(非アクティブ)</summary>
		private readonly Color TabInactiveColor = SystemColors.Control;



		//todo: セル　バリエーションを見直す
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



		public FormShipGroup( FormMain parent ) {
			InitializeComponent();

			ControlHelper.SetDoubleBuffered( ShipView );


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
			//checkme: もっとひつようかも
			#endregion


			SystemEvents.SystemShuttingDown += SystemShuttingDown;
		}


		private void FormShipGroup_Load( object sender, EventArgs e ) {

			ShipGroupManager groups = KCDatabase.Instance.ShipGroup;


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
		/// グループデータにGUIからの操作を適用します。
		/// </summary>
		private void ApplyGroupData( ImageLabel target ) {

#if false
			if ( target != null ) {

				//*//checkme: これいる?
				if ( KCDatabase.Instance.Ships.Count == 0 )
					return;
				//*/

				ShipGroupData g = KCDatabase.Instance.ShipGroup[(int)target.Tag];
				if ( g == null )
					return;


				g.ViewColumns = new IDDictionary<ShipGroupData.ViewColumnData>();
				for ( int i = 0; i < ShipView.Columns.Count; i++ ) {
					g.ViewColumns.Add( new ShipGroupData.ViewColumnData( ShipView.Columns[i] ) );
				}
				
				g.ScrollLockColumnCount = ShipView.Columns.OfType<DataGridViewColumn>().Count( c => c.Frozen );


				//*/
			}
#endif
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


			//undone: 新しい行への対応
			row.Cells[ShipView_Name.Index].Tag = ship.ShipID;
			row.Cells[ShipView_Level.Index].Tag = ship.ExpTotal;


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

			ShipView.SuspendLayout();
			ShipView.Rows.Clear();

			group.UpdateMembers();
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


			{
				int columnCount = ShipView.Columns.Count;
				if ( group.ViewColumns != null ) columnCount = Math.Min( columnCount, group.ViewColumns.Count );


				for ( int i = 0; i < columnCount; i++ ) {
					var view = group.ViewColumns[i];
					view.ToColumn( ShipView.Columns[view.Index] );
				}
			}


			ShipView.ResumeLayout();


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


			int groupID = (int)target.Tag;
			var group = KCDatabase.Instance.ShipGroup[groupID];


			ApplyGroupData( SelectedTab );


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

			//undone:新しい行への対応

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
				 e.ColumnIndex == ShipView_Aircraft5.Index ) {
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

			//undone: 新しい行への対応

			if ( e.Column.Index == ShipView_Name.Index ) {
				e.SortResult =
					KCDatabase.Instance.MasterShips[(int)ShipView.Rows[e.RowIndex1].Cells[e.Column.Index].Tag].AlbumNo -
					KCDatabase.Instance.MasterShips[(int)ShipView.Rows[e.RowIndex2].Cells[e.Column.Index].Tag].AlbumNo;

			} else if ( e.Column.Index == ShipView_Level.Index ) {
				e.SortResult = (int)ShipView.Rows[e.RowIndex1].Cells[e.Column.Index].Tag - (int)ShipView.Rows[e.RowIndex2].Cells[e.Column.Index].Tag;	//exptotal
				if ( e.SortResult == 0 )	//for Lv.99-100
					e.SortResult = (int)e.CellValue1 - (int)e.CellValue2;

			} else if (
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

			using ( var dialog = new DialogTextInput( "グループを追加", "グループ名を入力してください：" ) ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

					var group = KCDatabase.Instance.ShipGroup.Add();


					group.Name = dialog.InputtedText.Trim();

					for ( int i = 0; i < ShipView.Columns.Count; i++ ) {
						group.ViewColumns.Add( new ShipGroupData.ViewColumnData( ShipView.Columns[i] ) );
					}
					//group.ColumnAutoSize = MenuMember_ColumnAutoSize.Checked;
					//scrollrockcolumncount
					//filter, etc
					//undone: 各種設定の引継ぎ

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

				MenuMember_CSVOutput.Enabled = false;

			} else if ( KCDatabase.Instance.ShipGroup.ShipGroups.Count == 0 ) {

				MenuMember_CSVOutput.Enabled = false;

			} else {

				MenuMember_CSVOutput.Enabled = true;

			}

		}
		#endregion


		#region メニュー:メンバー操作


#if false
		private void MenuMember_AddToGroup_Click( object sender, EventArgs e ) {

			// fixme: 暫定的に封印
			return;

			using ( var dialog = new DialogTextSelect( "グループの選択", "追加するグループを選択してください：",
				KCDatabase.Instance.ShipGroup.ShipGroups.Values.Where( g => g.GroupID >= 0 ).ToArray() ) ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

					ShipGroupData group = (ShipGroupData)dialog.SelectedItem;
					if ( group != null && group.GroupID >= 0 ) {

						List<int> members = new List<int>( ShipView.Rows.GetRowCount( DataGridViewElementStates.Selected ) );

						foreach ( DataGridViewRow row in ShipView.SelectedRows.OfType<DataGridViewRow>().OrderBy( r => r.Tag ) ) {

							members.Add( (int)row.Cells[ShipView_ID.Index].Value );
						}

						group.Members.AddRange( members );
					}
				}
			}

		}


		private void MenuMember_CreateGroup_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogTextInput( "グループの追加", "追加するグループの名前を入力してください：" ) ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

					var group = KCDatabase.Instance.ShipGroup.Add();

					group.Name = dialog.InputtedText.Trim();
					foreach ( DataGridViewRow row in ShipView.SelectedRows.OfType<DataGridViewRow>().OrderBy( r => r.Tag ) ) {

						group.Members.Add( (int)row.Cells[ShipView_ID.Index].Value );
					}

					for ( int i = 0; i < ShipView.Columns.Count; i++ ) {
						group.ViewColumns.Add( new ShipGroupData.ViewColumnData( ShipView.Columns[i] ) );
					}

					//group.ColumnAutoSize = MenuMember_ColumnAutoSize.Checked;
					// fixme: 他のパラメータの設定

					ImageLabel il = CreateTabLabel( group.GroupID );
					TabPanel.Controls.Add( il );

					ChangeShipView( il );
				}
			}

		}


		private void MenuMember_Delete_Click( object sender, EventArgs e ) {

			ShipGroupData group = SelectedTab != null ? KCDatabase.Instance.ShipGroup[(int)SelectedTab.Tag] : null;


			if ( group == null ) {
				MessageBox.Show( "このグループは変更できません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Asterisk );
				return;
			}

			return;		//fixme: 一時的に封印

			List<int> list = new List<int>( ShipView.Rows.GetRowCount( DataGridViewElementStates.Selected ) );

			foreach ( DataGridViewRow row in ShipView.SelectedRows ) {
				list.Add( (int)row.Cells[ShipView_ID.Index].Value );
				ShipView.Rows.Remove( row );
			}

			/*// fixme /////////////////////////////////////////////////////////////////////////////////////////////////////////
			if ( group != null )
				group.Members = group.Members.Except( list ).ToList();
			//*/
		}
#endif

		private void MenuMember_ColumnFilter_Click( object sender, EventArgs e ) {

			ShipGroupData group = SelectedTab != null ? KCDatabase.Instance.ShipGroup[(int)SelectedTab.Tag] : null;

			if ( group == null ) {
				MessageBox.Show( "このグループは変更できません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Asterisk );
				return;
			}


			using ( var dialog = new DialogShipGroupColumnFilter( ShipView, group ) ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

					group.ViewColumns = new IDDictionary<ShipGroupData.ViewColumnData>( dialog.Result );
					group.ScrollLockColumnCount = dialog.ScrollLockColumnCount;

					ApplyViewData( group );
				}
			}

		}





		private void MenuMember_Filter_Click( object sender, EventArgs e ) {

			if ( SelectedTab != null ) {
				var group = KCDatabase.Instance.ShipGroup[(int)SelectedTab.Tag];
				var dialog = new DialogShipGroupFilter( group.Expressions );

				dialog.ImportExpressionData( group.Expressions );

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {

					group.Expressions = dialog.ExportExpressionData();

					group.Expressions.Compile();
					group.UpdateMembers();

					ChangeShipView( SelectedTab );
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
				data.ToColumn( ShipView.Columns[data.Index] );
			}

			int count = 0;
			foreach ( var column in ShipView.Columns.Cast<DataGridViewColumn>().OrderBy( c => c.DisplayIndex ) ) {
				column.Frozen = count < group.ScrollLockColumnCount;
				count++;
			}

		}






		// undone: 出力内容の更新

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
				ShipGroupData group = KCDatabase.Instance.ShipGroup[(int)senderLabel.Tag];
				if ( group != null && group.GroupID >= 0 ) {
					ships = group.MembersInstance;

				} else {
					ships = KCDatabase.Instance.Ships.Values;
				}

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
										GetEquipmentString( ship, 0 ),		//undone: IDにしたいけどよく考えたら強化値が反映されない
										GetEquipmentString( ship, 1 ),
										GetEquipmentString( ship, 2 ),
										GetEquipmentString( ship, 3 ),
										GetEquipmentString( ship, 4 ),
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

						Utility.ErrorReporter.SendErrorReport( ex, "艦船グループ CSVの出力に失敗しました。" );
						MessageBox.Show( "艦船グループ CSVの出力に失敗しました。\r\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );

					}



				}
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


			Utility.Configuration.Config.FormShipGroup.SplitterDistance = splitContainer1.SplitterDistance;
			Utility.Configuration.Config.FormShipGroup.AutoUpdate = MenuGroup_AutoUpdate.Checked;
			Utility.Configuration.Config.FormShipGroup.ShowStatusBar = MenuGroup_ShowStatusBar.Checked;


			//以下は実データがないと動作しないためなければスキップ
			//fixme: ほんとに?
			//if ( KCDatabase.Instance.Ships.Count == 0 ) return;

			ShipGroupManager groups = KCDatabase.Instance.ShipGroup;


			if ( SelectedTab != null )
				ApplyGroupData( SelectedTab );


			List<ImageLabel> list = TabPanel.Controls.OfType<ImageLabel>().OrderBy( c => TabPanel.Controls.GetChildIndex( c ) ).ToList();

			for ( int i = 0; i < list.Count; i++ ) {

				ShipGroupData group = groups[(int)list[i].Tag];
				if ( group != null )
					group.GroupID = i + 1;
			}

		}


		protected override string GetPersistString() {
			return "ShipGroup";
		}


	}
}
