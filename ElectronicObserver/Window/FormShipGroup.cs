using ElectronicObserver.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Dialog;
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
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {
	public partial class FormShipGroup : DockContent {

		private readonly Color TabActiveColor = Color.White;
		private readonly Color TabInactiveColor = SystemColors.Control;

		private readonly Color CellColorRed = Color.FromArgb( 0xFF, 0xBB, 0xBB );
		private readonly Color CellColorOrange = Color.FromArgb( 0xFF, 0xDD, 0xBB );
		private readonly Color CellColorYellow = Color.FromArgb( 0xFF, 0xFF, 0xBB );
		private readonly Color CellColorGreen = Color.FromArgb( 0xBB, 0xFF, 0xBB );
		private readonly Color CellColorGray = Color.FromArgb( 0xBB, 0xBB, 0xBB );
		private readonly Color CellColorCherry = Color.FromArgb( 0xFF, 0xDD, 0xDD );

		private DataGridViewCellStyle CSDefaultLeft, CSDefaultCenter, CSDefaultRight,
			CSRedRight, CSOrangeRight, CSYellowRight, CSGreenRight, CSGrayRight, CSCherryRight,
			CSIsLocked;

		private ShipGroupData ShipGroupMaster;

		private ImageLabel SelectedTab = null;


		public FormShipGroup( FormMain parent ) {
			InitializeComponent();

			ControlHelper.SetDoubleBuffered( ShipView );


			ShipGroupMaster = new ShipGroupData( -1 );
			ShipGroupMaster.Name = "全所属艦";

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
			ShipView_Equipment1.DefaultCellStyle = CSDefaultLeft;
			ShipView_Equipment2.DefaultCellStyle = CSDefaultLeft;
			ShipView_Equipment3.DefaultCellStyle = CSDefaultLeft;
			ShipView_Equipment4.DefaultCellStyle = CSDefaultLeft;
			ShipView_Equipment5.DefaultCellStyle = CSDefaultLeft;

			#endregion

		}

		private void FormShipGroup_Load( object sender, EventArgs e ) {

			Font = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			ShipView.Font = Font;

			TabPanel.Controls.Add( CreateTabLabel( -1 ) );

			foreach ( var g in KCDatabase.Instance.ShipGroup.ShipGroups.Values ) {
				TabPanel.Controls.Add( CreateTabLabel( g.GroupID ) );
			}
		}


		private ImageLabel CreateTabLabel( int id ) {

			ImageLabel label = new ImageLabel();
			label.Text = KCDatabase.Instance.ShipGroup[id] != null ? KCDatabase.Instance.ShipGroup[id].Name : "全所属艦";
			label.Anchor = AnchorStyles.Left;
			label.Font = ShipView.Font;
			label.BackColor = TabInactiveColor;
			label.BorderStyle = BorderStyle.FixedSingle;
			label.Padding = new Padding( 1, 1, 3, 3 );
			label.Margin = new Padding( 2, 0, 2, 0 );
			label.ImageAlign = ContentAlignment.MiddleCenter;
			label.AutoSize = true;

			//イベントと固有IDの追加(内部データとの紐付)
			label.Click += TabLabel_Click;
			label.ContextMenuStrip = MenuGroup;
			label.Tag = id;

			return label;
		}


		void TabLabel_Click( object sender, EventArgs e ) {
			ChangeShipView( ( (ImageLabel)sender ) );
		}


		private void ChangeShipView( ImageLabel target ) {

			int groupID = (int)target.Tag;
			var group = KCDatabase.Instance.ShipGroup[groupID];


			if ( SelectedTab != null ) {
				SelectedTab.BackColor = TabInactiveColor;

				//ソート順の保持
				//checkme: なんかアレなのでもっといいのが思いついたら変更する
				//fixme: [全所属艦]はソート順が保持できない
				ShipGroupData g = KCDatabase.Instance.ShipGroup[(int)SelectedTab.Tag];
				if ( g == null )
					g = ShipGroupMaster;

				g.Members.Clear();
				g.Members.Capacity = ShipView.Rows.GetRowCount( DataGridViewElementStates.None );

				foreach ( DataGridViewRow row in ShipView.Rows ) {
					g.Members.Add( (int)row.Cells[ShipView_ID.Index].Value );
				}
				
			}


			if ( group == null ) {
				ShipGroupMaster.Members = ShipGroupMaster.Members.Intersect( KCDatabase.Instance.Ships.Keys ).Union( KCDatabase.Instance.Ships.Keys ).Distinct().ToList();
				group = ShipGroupMaster;
			}


			ShipView.SuspendLayout();

			ShipView.Rows.Clear();

			IEnumerable<ShipData> ships = group != null ? group.MembersInstance.AsEnumerable() : KCDatabase.Instance.Ships.Values;
			List<DataGridViewRow> rows = new List<DataGridViewRow>( ships.Count() );

			foreach ( ShipData ship in ships ) {

				if ( ship == null ) continue;
				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells( ShipView );
				row.SetValues(
					ship.MasterID,
					ship.MasterShip.ShipType,
					ship.MasterShip.Name,
					ship.Level,
					ship.ExpNext,
					ship.ExpNextRemodel,
					new Fraction( ship.HPCurrent, ship.HPMax ),
					ship.Condition,
					new Fraction( ship.Fuel, ship.MasterShip.Fuel ),
					new Fraction( ship.Ammo, ship.MasterShip.Ammo ),
					GetEquipmentString( ship, 0 ),
					GetEquipmentString( ship, 1 ),
					GetEquipmentString( ship, 2 ),
					GetEquipmentString( ship, 3 ),
					GetEquipmentString( ship, 4 ),
					ship.Fleet,
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
					ship.IsLocked,
					ship.SallyArea
					);

				row.Cells[ShipView_Name.Index].Tag = ship.ShipID;
				row.Cells[ShipView_Level.Index].Tag = ship.ExpTotal;

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
					else if ( ship.Condition < Utility.Configuration.Instance.Control.ConditionBorder )
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
				row.Cells[ShipView_FirepowerRemain.Index].Style = ( ship.MasterShip.FirepowerMax - ship.FirepowerBase ) == 0 ? CSGrayRight : CSDefaultRight;
				row.Cells[ShipView_TorpedoRemain.Index].Style = ( ship.MasterShip.TorpedoMax - ship.TorpedoBase ) == 0 ? CSGrayRight : CSDefaultRight;
				row.Cells[ShipView_AARemain.Index].Style = ( ship.MasterShip.AAMax - ship.AABase ) == 0 ? CSGrayRight : CSDefaultRight;
				row.Cells[ShipView_ArmorRemain.Index].Style = ( ship.MasterShip.ArmorMax - ship.ArmorBase ) == 0 ? CSGrayRight : CSDefaultRight;
				row.Cells[ShipView_LuckRemain.Index].Style = ( ship.MasterShip.LuckMax - ship.LuckBase ) == 0 ? CSGrayRight : CSDefaultRight;

				row.Cells[ShipView_Locked.Index].Style = ship.IsLocked ? CSIsLocked : CSDefaultCenter;


				rows.Add( row );

			}

			for ( int i = 0; i < rows.Count; i++ )
				rows[i].Tag = i;

			ShipView.Rows.AddRange( rows.ToArray() );
			ShipView.ResumeLayout();


			SelectedTab = target;
			SelectedTab.BackColor = TabActiveColor;

		}


		private string GetEquipmentString( ShipData ship, int index ) {

			int current = ship.Aircraft[index];
			int max = ship.MasterShip.Aircraft[index];
			string name = ship.SlotInstance[index] != null ? ship.SlotInstance[index].NameWithLevel : "(なし)";

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


		private void ShipView_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e ) {

			if ( e.ColumnIndex == ShipView_ShipType.Index ) {
				e.Value = KCDatabase.Instance.ShipTypes[(int)e.Value].Name;
				e.FormattingApplied = true;

			} else if ( e.ColumnIndex == ShipView_Fleet.Index && (int)e.Value == -1 ) {
				e.Value = "";
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


		private void ShipView_SortCompare( object sender, DataGridViewSortCompareEventArgs e ) {

			if ( e.Column.Index == ShipView_Name.Index ) {
				//e.SortResult = ((string)e.CellValue1).CompareTo( e.CellValue2 );		//checkme
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

				if ( rate > 0 )
					e.SortResult = 1;
				else if ( rate < 0 )
					e.SortResult = -1;
				else
					e.SortResult = frac1.Current - frac2.Current;

			} else if ( e.Column.Index == ShipView_Fleet.Index ) {
				int f1 = (int)e.CellValue1, f2 = (int)e.CellValue2;
				e.SortResult = ( f1 == -1 ? 99 : f1 ) - ( f2 == -1 ? 99 : f2 );

			} else if ( e.Column.Index == ShipView_RepairTime.Index ) {
				e.SortResult = ( (string)e.CellValue1 ).CompareTo( e.CellValue2 );

			} else if ( e.Column.Index == ShipView_Locked.Index ) {
				e.SortResult = ( (bool)e.CellValue1 ? 1 : 0 ) - ( (bool)e.CellValue2 ? 1 : 0 );

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




		private void MenuGroup_Add_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogTextInput( "グループを追加", "グループ名を入力してください：" ) ) {

				if ( dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

					var group = KCDatabase.Instance.ShipGroup.Add();

					group.Name = dialog.InputtedText;

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

				}
			}
		}



		private void MenuGroup_Opening( object sender, CancelEventArgs e ) {

			if ( MenuGroup.SourceControl == TabPanel ) {
				MenuGroup_Add.Enabled = true;
				MenuGroup_Delete.Enabled = false;
			} else {
				MenuGroup_Add.Enabled = true;
				MenuGroup_Delete.Enabled = true;
			}

		}




		private void MenuMember_Opening( object sender, CancelEventArgs e ) {

			if ( ShipView.Rows.GetRowCount( DataGridViewElementStates.Selected ) == 0 ) {

				e.Cancel = true;
			}
		}

		private void MenuMember_AddToGroup_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogTextSelect( "グループの選択", "追加するグループを選択してください：",
				KCDatabase.Instance.ShipGroup.ShipGroups.Values.ToArray() ) ) {

				if ( dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

					ShipGroupData group = (ShipGroupData)dialog.SelectedItem;
					if ( group != null ) {

						List<int> members = new List<int>( ShipView.Rows.GetRowCount( DataGridViewElementStates.Selected ) );

						foreach ( DataGridViewRow row in ShipView.SelectedRows ) {

							members.Add( (int)row.Cells[ShipView_ID.Index].Value );
						}

						group.Members.AddRange( members );
						group.CheckMembers();
					}
				}
			}

		}


		private void MenuMember_CreateGroup_Click( object sender, EventArgs e ) {

			using ( var dialog = new DialogTextInput( "グループの追加", "追加するグループの名前を入力してください：" ) ) {

				if ( dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

					var group = KCDatabase.Instance.ShipGroup.Add();

					group.Name = dialog.InputtedText;
					foreach ( DataGridViewRow row in ShipView.SelectedRows ) {

						group.Members.Add( (int)row.Cells[ShipView_ID.Index].Value );
					}

					ImageLabel il = CreateTabLabel( group.GroupID );
					TabPanel.Controls.Add( il );

					ChangeShipView( il );
				}
			}

		}


		private void MenuMember_Delete_Click( object sender, EventArgs e ) {

			List<int> list = new List<int>( ShipView.Rows.GetRowCount( DataGridViewElementStates.Selected ) );

			foreach ( DataGridViewRow row in ShipView.SelectedRows ) {
				list.Add( (int)row.Cells[ShipView_ID.Index].Value );
				ShipView.Rows.Remove( row );
			}

			ShipGroupData group = KCDatabase.Instance.ShipGroup[(int)SelectedTab.Tag];


			for ( int i = 0; i < list.Count; i++ ) {
				group.Members.Remove( list[i] );		//except...
			}

			
		}


		protected override string GetPersistString() {
			return "ShipGroup";
		}

	}
}
