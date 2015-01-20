using ElectronicObserver.Data;
using ElectronicObserver.Resource;
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
	public partial class DialogEquipmentList : Form {


		private DataGridViewCellStyle CSDefaultLeft, CSDefaultRight, CSEquippedShips;


		public DialogEquipmentList() {

			InitializeComponent();

			ControlHelper.SetDoubleBuffered( EquipmentView );

			Font = Utility.Configuration.Config.UI.MainFont;

			foreach ( DataGridViewColumn column in EquipmentView.Columns ) {
				column.MinimumWidth = 2;
			}



			#region CellStyle

			CSDefaultLeft = new DataGridViewCellStyle();
			CSDefaultLeft.Alignment = DataGridViewContentAlignment.MiddleLeft;
			CSDefaultLeft.BackColor = SystemColors.Control;
			CSDefaultLeft.Font = Font;
			CSDefaultLeft.ForeColor = SystemColors.ControlText;
			CSDefaultLeft.SelectionBackColor = Color.FromArgb( 0xFF, 0xFF, 0xCC );
			CSDefaultLeft.SelectionForeColor = SystemColors.ControlText;

			CSDefaultRight = new DataGridViewCellStyle( CSDefaultLeft );
			CSDefaultRight.Alignment = DataGridViewContentAlignment.MiddleRight;
			CSDefaultRight.WrapMode = DataGridViewTriState.False;

			CSEquippedShips = new DataGridViewCellStyle( CSDefaultLeft );
			CSEquippedShips.WrapMode = DataGridViewTriState.True;

			EquipmentView.DefaultCellStyle = CSDefaultRight;
			EquipmentView_Name.DefaultCellStyle = CSDefaultLeft;
			EquipmentView_EquippedShip.DefaultCellStyle = CSEquippedShips;
			EquipmentView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

			#endregion

		}

		private void DialogEquipmentList_Load( object sender, EventArgs e ) {

			UpdateView();

			this.Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormEquipmentList] );

		}


		private void UpdateView() {

			var ships = KCDatabase.Instance.Ships.Values;
			var equipments = KCDatabase.Instance.Equipments.Values;
			var masterEquipments = KCDatabase.Instance.MasterEquipments;
			int masterCount = masterEquipments.Values.Count( eq => !eq.IsAbyssalEquipment );

			var allCount = new Dictionary<int, int>( masterCount );

			//全個数計算
			foreach ( var e in equipments ) {

				if ( !allCount.ContainsKey( e.EquipmentID ) ) {
					allCount.Add( e.EquipmentID, 1 );

				} else {
					allCount[e.EquipmentID]++;
				}

			}


			var remainCount = new Dictionary<int, int>( allCount );
			var equippedShips = new Dictionary<int, StringBuilder>( allCount.Count );

			//剰余数計算&装備艦
			foreach ( var ship in ships ) {

				var owneqs = ship.SlotInstance;
				int[] eqids = ship.SlotInstance.Select( i => i != null ? i.EquipmentID : -1 ).ToArray();


				for ( int i = 0; i < eqids.Length; i++ ) {

					if ( eqids[i] == -1 ) continue;

					int cid = eqids[i];

					int count = 0;
					for ( int j = 0; j < eqids.Length; j++ ) {
						if ( eqids[j] == cid ) {
							count++;
							eqids[j] = -1;
						}
					}

					remainCount[cid] -= count;

					if ( !equippedShips.ContainsKey( cid ) ) {
						equippedShips.Add( cid, new StringBuilder( GetShipInformation( ship, count ) ) );

					} else {
						equippedShips[cid].AppendFormat( ", {0}", GetShipInformation( ship, count ) );
					}

				}

			}


			//表示処理
			EquipmentView.SuspendLayout();

			EquipmentView.Rows.Clear();


			var rows = new List<DataGridViewRow>( allCount.Count );
			var ids = allCount.Keys;

			foreach ( int id in ids ) {

				var row = new DataGridViewRow();
				row.CreateCells( EquipmentView );
				row.SetValues(
					id,
					masterEquipments[id].Name,
					allCount[id],
					remainCount[id],
					equippedShips.ContainsKey( id ) ? equippedShips[id].ToString() : null
					);

				rows.Add( row );
			}

			for ( int i = 0; i < rows.Count; i++ )
				rows[i].Tag = i;

			EquipmentView.Rows.AddRange( rows.ToArray() );

			EquipmentView.Sort( EquipmentView_Name, ListSortDirection.Ascending );

			EquipmentView.ResumeLayout();

		}


		private string GetShipInformation( ShipData ship, int count ) {

			return string.Format( "{0} Lv.{1}{2}", ship.MasterShip.Name, ship.Level, count > 1 ? " x" + count : "" );

		}


		private void EquipmentView_SortCompare( object sender, DataGridViewSortCompareEventArgs e ) {

			if ( e.Column.Index == EquipmentView_Name.Index ) {

				int id1 = (int)EquipmentView.Rows[e.RowIndex1].Cells[EquipmentView_ID.Index].Value;
				int id2 = (int)EquipmentView.Rows[e.RowIndex2].Cells[EquipmentView_ID.Index].Value;

				e.SortResult =
					KCDatabase.Instance.MasterEquipments[id1].EquipmentType[2] -
					KCDatabase.Instance.MasterEquipments[id2].EquipmentType[2];

				if ( e.SortResult == 0 ) {
					e.SortResult = id1 - id2;
				}

			} else {

				e.SortResult = ( (IComparable)e.CellValue1 ).CompareTo( e.CellValue2 );

			}


			if ( e.SortResult == 0 ) {
				e.SortResult = ( EquipmentView.Rows[e.RowIndex1].Tag as int? ?? 0 ) - ( EquipmentView.Rows[e.RowIndex2].Tag as int? ?? 0 );
			}

			e.Handled = true;

		}

		private void EquipmentView_Sorted( object sender, EventArgs e ) {

			for ( int i = 0; i < EquipmentView.Rows.Count; i++ )
				EquipmentView.Rows[i].Tag = i;

		}

		private void EquipmentView_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e ) {

			//いまはなにもないです

		}


		private void Menu_File_CSVOutput_Click( object sender, EventArgs e ) {

			if ( SaveCSVDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

				try {

					using ( StreamWriter sw = new StreamWriter( SaveCSVDialog.FileName, false, Encoding.UTF8 ) ) {

						sw.WriteLine( "固有ID,装備ID,装備名,改修Lv,ロック,装備艦ID,装備艦" );
						string arg = string.Format( "{{{0}}}", string.Join( "},{", Enumerable.Range( 0, 7 ) ) );

						foreach ( var eq in KCDatabase.Instance.Equipments.Values ) {

							if ( eq.Name == "なし" ) continue;

							ShipData equippedShip = KCDatabase.Instance.Ships.Values.FirstOrDefault( s => s.Slot.Contains( eq.MasterID ) );


							sw.WriteLine( arg,
								eq.MasterID,
								eq.EquipmentID,
								eq.Name,
								eq.Level,
								eq.IsLocked ? 1 : 0,
								equippedShip != null ? equippedShip.MasterID : -1,
								equippedShip != null ? equippedShip.NameWithLevel : ""
								);

						}

					}

				} catch ( Exception ex ) {

					Utility.ErrorReporter.SendErrorReport( ex, "装備一覧 CSVの出力に失敗しました。" );
					MessageBox.Show( "装備一覧 CSVの出力に失敗しました。\r\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );

				}

			}


		}



		private void DialogEquipmentList_FormClosed( object sender, FormClosedEventArgs e ) {

			ResourceManager.DestroyIcon( Icon );

		}

		private void TopMenu_File_Update_Click( object sender, EventArgs e ) {

			UpdateView();
		}

	}
}
