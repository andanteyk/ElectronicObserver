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


		private DataGridViewCellStyle CSDefaultLeft, CSDefaultRight, 
			CSUnselectableLeft, CSUnselectableRight;


		public DialogEquipmentList() {
			this.SuspendLayoutForDpiScale();

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
			CSDefaultLeft.WrapMode = DataGridViewTriState.False;

			CSDefaultRight = new DataGridViewCellStyle( CSDefaultLeft );
			CSDefaultRight.Alignment = DataGridViewContentAlignment.MiddleRight;

			CSUnselectableLeft = new DataGridViewCellStyle( CSDefaultLeft );
			CSUnselectableLeft.SelectionForeColor = CSUnselectableLeft.ForeColor;
			CSUnselectableLeft.SelectionBackColor = CSUnselectableLeft.BackColor;

			CSUnselectableRight = new DataGridViewCellStyle( CSDefaultRight );
			CSUnselectableRight.SelectionForeColor = CSUnselectableRight.ForeColor;
			CSUnselectableRight.SelectionBackColor = CSUnselectableRight.BackColor;


			EquipmentView.DefaultCellStyle = CSDefaultRight;
			EquipmentView_Name.DefaultCellStyle = CSDefaultLeft;
			EquipmentView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

			DetailView.DefaultCellStyle = CSUnselectableRight;
			DetailView_EquippedShip.DefaultCellStyle = CSUnselectableLeft;
			DetailView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			int headerHeight = this.GetDpiHeight( 23 );
			DetailView.ColumnHeadersHeight = headerHeight;
			EquipmentView.ColumnHeadersHeight = headerHeight;
			DetailView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			EquipmentView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

			#endregion

			this.ResumeLayoutForDpiScale();
		}

		private void DialogEquipmentList_Load( object sender, EventArgs e ) {

			UpdateView();

			this.Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormEquipmentList] );

		}


		private void EquipmentView_SortCompare( object sender, DataGridViewSortCompareEventArgs e ) {

			if ( e.Column.Index == EquipmentView_Name.Index ) {

				int id1 = (int)EquipmentView[EquipmentView_ID.Index, e.RowIndex1].Value;
				int id2 = (int)EquipmentView[EquipmentView_ID.Index, e.RowIndex2].Value;

				e.SortResult =
					KCDatabase.Instance.MasterEquipments[id1].CategoryType -
					KCDatabase.Instance.MasterEquipments[id2].CategoryType;

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

			if ( e.ColumnIndex == EquipmentView_Icon.Index ) {
				e.Value = ResourceManager.GetEquipmentImage( (int)e.Value );
				e.FormattingApplied = true;
			}

		}

		private void EquipmentView_SelectionChanged( object sender, EventArgs e ) {

			if ( EquipmentView.Enabled && EquipmentView.SelectedRows != null && EquipmentView.SelectedRows.Count > 0 )
				UpdateDetailView( (int)EquipmentView[EquipmentView_ID.Index, EquipmentView.SelectedRows[0].Index].Value );

		}


		private void DetailView_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e ) {

			if ( e.ColumnIndex != DetailView_EquippedShip.Index ) {
				if ( AreEqual( e.RowIndex, e.RowIndex - 1 ) ) {

					e.Value = "";
					e.FormattingApplied = true;
					return;
				}
			}

			if ( e.ColumnIndex == DetailView_Level.Index ) {

				e.Value = "+" + e.Value;
				e.FormattingApplied = true;

			}

		}

		private void DetailView_CellPainting( object sender, DataGridViewCellPaintingEventArgs e ) {

			e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;

			if ( AreEqual( e.RowIndex, e.RowIndex + 1 ) ) {
				e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
			} else {
				e.AdvancedBorderStyle.Bottom = DetailView.AdvancedCellBorderStyle.Bottom;
			}

		}

		/// <summary>
		/// DetailView 内の行が同じレベルであるかを判定します。
		/// </summary>
		private bool AreEqual( int rowIndex1, int rowIndex2 ) {

			if ( rowIndex1 < 0 ||
				rowIndex1 >= DetailView.Rows.Count ||
				rowIndex2 < 0 ||
				rowIndex2 >= DetailView.Rows.Count )
				return false;

			return ( (IComparable)DetailView[DetailView_Level.Index, rowIndex1].Value )
				.CompareTo( DetailView[DetailView_Level.Index, rowIndex2].Value ) == 0 &&
				( (IComparable)DetailView[DetailView_AircraftLevel.Index, rowIndex1].Value )
				.CompareTo( DetailView[DetailView_AircraftLevel.Index, rowIndex2].Value ) == 0;
		}


		/// <summary>
		/// 一覧ビューを更新します。
		/// </summary>
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


			//剰余数計算
			foreach ( var ship in ships ) {

				foreach ( var eq in ship.AllSlotInstanceMaster ) {
					if ( eq == null ) continue;
					remainCount[eq.EquipmentID]--;
				}

			}


			//表示処理
			EquipmentView.SuspendLayout();

			EquipmentView.Enabled = false;
			EquipmentView.Rows.Clear();


			var rows = new List<DataGridViewRow>( allCount.Count );
			var ids = allCount.Keys;

			foreach ( int id in ids ) {

				var row = new DataGridViewRow();
				row.CreateCells( EquipmentView );
				row.SetValues(
					id,
					masterEquipments[id].IconType,
					masterEquipments[id].Name,
					allCount[id],
					remainCount[id]
					);

				rows.Add( row );
			}

			for ( int i = 0; i < rows.Count; i++ )
				rows[i].Tag = i;

			EquipmentView.Rows.AddRange( rows.ToArray() );

			EquipmentView.Sort( EquipmentView_Name, ListSortDirection.Ascending );


			EquipmentView.Enabled = true;
			EquipmentView.ResumeLayout();

			if ( EquipmentView.Rows.Count > 0 )
				EquipmentView.CurrentCell = EquipmentView[0, 0];

		}


		private class DetailCounter : IIdentifiable {

			public int level;
			public int aircraftLevel;
			public int countAll;
			public int countRemain;
			public int countRemainPrev;

			public List<string> equippedShips;

			public DetailCounter( int lv, int aircraftLv ) {
				level = lv;
				aircraftLevel = aircraftLv;
				countAll = 0;
				countRemainPrev = 0;
				countRemain = 0;
				equippedShips = new List<string>();
			}

			public static int CalculateID( int level, int aircraftLevel ) {
				return level + aircraftLevel * 100;
			}

			public static int CalculateID( EquipmentData eq ) {
				return CalculateID( eq.Level, eq.AircraftLevel );
			}

			public int ID { get { return CalculateID( level, aircraftLevel ); } }
		}


		/// <summary>
		/// 詳細ビューを更新します。
		/// </summary>
		private void UpdateDetailView( int equipmentID ) {

			DetailView.SuspendLayout();

			DetailView.Rows.Clear();

			//装備数カウント
			var eqs = KCDatabase.Instance.Equipments.Values.Where( eq => eq.EquipmentID == equipmentID );
			var countlist = new IDDictionary<DetailCounter>();

			foreach ( var eq in eqs ) {
				var c = countlist[DetailCounter.CalculateID( eq )];
				if ( c == null ) {
					countlist.Add( new DetailCounter( eq.Level, eq.AircraftLevel ) );
					c = countlist[DetailCounter.CalculateID( eq )];
				}
				c.countAll++;
				c.countRemain++;
				c.countRemainPrev++;
			}

			//装備艦集計
			foreach ( var ship in KCDatabase.Instance.Ships.Values ) {

				foreach ( var eq in ship.AllSlotInstance.Where( s => s != null && s.EquipmentID == equipmentID ) ) {

					countlist[DetailCounter.CalculateID( eq )].countRemain--;

				}

				foreach ( var c in countlist.Values ) {
					if ( c.countRemain != c.countRemainPrev ) {

						int diff = c.countRemainPrev - c.countRemain;

						c.equippedShips.Add( ship.NameWithLevel + ( diff > 1 ? ( " x" + diff ) : "" ) );

						c.countRemainPrev = c.countRemain;
					}
				}

			}


			//行に反映
			var rows = new List<DataGridViewRow>( eqs.Count() );

			foreach ( var c in countlist.Values ) {

				if ( c.equippedShips.Count() == 0 ) {
					c.equippedShips.Add( "" );
				}

				foreach ( var s in c.equippedShips ) {

					var row = new DataGridViewRow();
					row.CreateCells( DetailView );
					row.SetValues( c.level, c.aircraftLevel, c.countAll, c.countRemain, s );
					rows.Add( row );
				}

			}

			DetailView.Rows.AddRange( rows.ToArray() );
			DetailView.Sort( DetailView_AircraftLevel, ListSortDirection.Ascending );
			DetailView.Sort( DetailView_Level, ListSortDirection.Ascending );

			DetailView.ResumeLayout();

			Text = "装備一覧 - " + KCDatabase.Instance.MasterEquipments[equipmentID].Name;
		}



		private void Menu_File_CSVOutput_Click( object sender, EventArgs e ) {

			if ( SaveCSVDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

				try {

					using ( StreamWriter sw = new StreamWriter( SaveCSVDialog.FileName, false, Utility.Configuration.Config.Log.FileEncoding ) ) {

						sw.WriteLine( "固有ID,装備ID,装備名,改修Lv,艦載機Lv,ロック,装備艦ID,装備艦" );
						string arg = string.Format( "{{{0}}}", string.Join( "},{", Enumerable.Range( 0, 8 ) ) );

						foreach ( var eq in KCDatabase.Instance.Equipments.Values ) {

							if ( eq.Name == "なし" ) continue;

							ShipData equippedShip = KCDatabase.Instance.Ships.Values.FirstOrDefault( s => s.Slot.Contains( eq.MasterID ) );


							sw.WriteLine( arg,
								eq.MasterID,
								eq.EquipmentID,
								eq.Name,
								eq.Level,
								eq.AircraftLevel,
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
