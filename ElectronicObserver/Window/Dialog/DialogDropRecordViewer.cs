using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Mathematics;
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
	public partial class DialogDropRecordViewer : Form {

		private ShipDropRecord _record;

		private const string NameAny = "(全て)";
		private const string NameNotExist = "(なし)";
		private const string NameFullPort = "(満員)";
		private const string NameExist = "(ドロップ)";

		private const string MapAny = "*";

		public DialogDropRecordViewer() {
			InitializeComponent();

			_record = RecordManager.Instance.ShipDrop;
		}

		private void DialogDropRecordViewer_Load( object sender, EventArgs e ) {

			ShipName.Items.Add( NameAny );
			ShipName.Items.Add( NameExist );
			ShipName.Items.Add( NameNotExist );
			ShipName.Items.Add( NameFullPort );
			ShipName.Items.AddRange( KCDatabase.Instance.MasterShips.Values
				.Where( s => !s.IsAbyssalShip && s.RemodelBeforeShipID == 0 )
				.OrderBy( s => s.Name )
				.OrderBy( s => s.NameReading )
				.Select( s => s.NameWithClass ).ToArray() );
			ShipName.SelectedIndex = 0;

			// アイテムは殆どがドロップしないのでレコードにあるやつだけ
			ItemName.Items.Add( NameAny );
			ItemName.Items.Add( NameExist );
			ItemName.Items.Add( NameNotExist );
			ItemName.Items.AddRange( _record.Record
				.Where( r => r.ItemID != -1 )
				.OrderBy( r => r.ItemID )
				.Select( r => r.ItemName )
				.Distinct().ToArray() );
			ItemName.SelectedIndex = 0;

			// not implemented: eq


			DateBegin.Value = DateBegin.MinDate = DateEnd.MinDate = _record.Record.First().Date.Date;
			DateEnd.Value = DateBegin.MaxDate = DateEnd.MaxDate = DateTime.Now.AddDays( 1 ).Date;


			MapAreaID.Items.Add( MapAny );
			MapAreaID.Items.AddRange( _record.Record.Select( r => r.MapAreaID ).Distinct().OrderBy( i => i ).Select( i => i.ToString() ).ToArray() );
			MapAreaID.SelectedIndex = 0;

			MapInfoID.Items.Add( MapAny );
			MapInfoID.Items.AddRange( _record.Record.Select( r => r.MapInfoID ).Distinct().OrderBy( i => i ).Select( i => i.ToString() ).ToArray() );
			MapInfoID.SelectedIndex = 0;

			// fixme: 都度生成のほうがよさげ
			MapCellID.Items.Add( MapAny );
			//MapCellID.Items.AddRange( _record.Record.Select( r => r.CellID ).Distinct().OrderBy( i => i ).Select( i => i.ToString() ).ToArray() );
			MapCellID.SelectedIndex = 0;
			MapCellID.Enabled = false;

			MapDifficulty.Items.Add( MapAny );
			MapDifficulty.Items.AddRange( _record.Record.Select( r => r.Difficulty ).Distinct().OrderBy( i => i ).Select( i => Constants.GetDifficulty( i ) ).ToArray() );
			MapDifficulty.SelectedIndex = 0;


			LabelShipName.ImageList = ResourceManager.Instance.Icons;
			LabelShipName.ImageIndex = (int)ResourceManager.IconContent.HeadQuartersShip;
			LabelItemName.ImageList = ResourceManager.Instance.Icons;
			LabelItemName.ImageIndex = (int)ResourceManager.IconContent.ItemPresentBox;
			LabelEquipmentName.ImageList = ResourceManager.Instance.Equipments;
			LabelEquipmentName.ImageIndex = (int)ResourceManager.EquipmentContent.MainGunL;

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.ItemPresentBox] );
		}

		private void DialogDropRecordViewer_FormClosed( object sender, FormClosedEventArgs e ) {
			ResourceManager.DestroyIcon( Icon );
		}




		private IEnumerable<ShipDropRecord.ShipDropElement> GetFilteredRecords() {

			IEnumerable<ShipDropRecord.ShipDropElement> records = _record.Record;


			switch ( (string)ShipName.SelectedItem ) {
				case NameAny:
					//do nothing
					break;
				case NameNotExist:
					records = records.Where( r => r.ShipID == -1 );
					break;
				case NameFullPort:
					records = records.Where( r => r.ShipID == -2 );
					break;
				case NameExist:
					records = records.Where( r => r.ShipID > 0 );
					break;
				default:
					records = records.Where( r => r.ShipName == (string)ShipName.SelectedItem );
					break;
			}

			switch ( (string)ItemName.SelectedItem ) {
				case NameAny:
					//do nothing
					break;
				case NameNotExist:
					records = records.Where( r => r.ItemID == -1 );
					break;
				case NameExist:
					records = records.Where( r => r.ItemID > 0 );
					break;
				default:
					records = records.Where( r => r.ItemName == (string)ItemName.SelectedItem );
					break;
			}


			records = records.Where( r => DateBegin.Value <= r.Date && r.Date <= DateEnd.Value );

			records = records.Where( r =>
				( ( r.Rank == "S" || r.Rank == "SS" ) && RankS.Checked ) ||
				( ( r.Rank == "A" ) && RankA.Checked ) ||
				( ( r.Rank == "B" ) && RankB.Checked ) ||
				( ( Constants.GetWinRank( r.Rank ) <= 3 ) && RankX.Checked ) );


			if ( (string)MapAreaID.SelectedItem != MapAny ) {
				records = records.Where( r => r.MapAreaID == int.Parse( (string)MapAreaID.SelectedItem ) );
			}

			if ( (string)MapInfoID.SelectedItem != MapAny ) {
				records = records.Where( r => r.MapInfoID == int.Parse( (string)MapInfoID.SelectedItem ) );
			}

			if ( (string)MapCellID.SelectedItem != MapAny ) {
				records = records.Where( r => r.CellID == int.Parse( (string)MapCellID.SelectedItem ) );
			}

			if ( (string)MapDifficulty.SelectedItem != MapAny ) {
				records = records.Where( r => r.Difficulty == int.Parse( (string)MapDifficulty.SelectedItem ) );
			}

			switch ( IsBossOnly.CheckState ) {
				case CheckState.Checked:
					records = records.Where( r => r.IsBossNode );
					break;
				case CheckState.Unchecked:
					records = records.Where( r => !r.IsBossNode );
					break;
				case CheckState.Indeterminate:
					//do nothing
					break;
			}


			return records;
		}


		private void UpdateDropView() {

			var records = GetFilteredRecords();

			DropView.Rows.Clear();


			if ( !MergeRows.Checked ) {
				var rows = new DataGridViewRow[records.Count()];

				{
					int i = 0;
					foreach ( var r in records ) {
						var row = new DataGridViewRow();
						row.CreateCells( DropView );
						row.SetValues(
							i + 1,
							GetContentString( r ),
							r.Date,
							GetMapString( r.MapAreaID, r.MapInfoID, r.CellID, r.IsBossNode, r.Difficulty ),
							Constants.GetWinRank( r.Rank )
							);

						rows[i] = row;

						i++;
					}
				}

				DropView.Rows.AddRange( rows );


			} else {

				var counts = new Dictionary<string, int[]>();
				string shipName = (string)ShipName.SelectedItem;
				string itemName = (string)ItemName.SelectedItem;
				string mapareaID = (string)MapAreaID.SelectedItem;
				string mapinfoID = (string)MapInfoID.SelectedItem;
				string mapcellID = (string)MapCellID.SelectedItem;
				string mapDifficulty = (string)MapDifficulty.SelectedItem;

				int priorityShip = 
					shipName == NameAny ? 0 :
					shipName == NameExist ? 1 : 2;
				int priorityItem =
					itemName == NameAny ? 0 :
					itemName == NameExist ? 1 : 2;
				int priorityContent = Math.Max( priorityShip, priorityItem );

				foreach ( var r in records ) {
					string key;

					if ( priorityContent == 2 ) {
						key = GetMapString( r.MapAreaID, r.MapInfoID, r.CellID, r.IsBossNode, (string)MapDifficulty.SelectedItem == MapAny ? -1 : r.Difficulty );

					} else {
						key = GetContentString( r, priorityShip < priorityItem && priorityShip < 2, priorityShip >= priorityItem && priorityItem < 2 );
					}


					if ( !counts.ContainsKey( key ) ) {
						counts.Add( key, new int[4] );
					}

					switch ( r.Rank ) {
						case "E":
						case "D":
						case "C":
							break;
						case "B":
							counts[key][3]++;
							break;
						case "A":
							counts[key][2]++;
							break;
						case "S":
						case "SS":
							counts[key][1]++;
							break;
					}
					counts[key][0]++;

				}


				var rows = new DataGridViewRow[counts.Count()];
				{
					int i = 0;
					foreach ( var c in counts ) {
						var row = new DataGridViewRow();
						row.CreateCells( DropView );
						row.SetValues(
							c.Value[0],
							c.Key,
							c.Value[1],
							c.Value[2],
							c.Value[3]
							);

						if ( priorityContent == 2 ) {
							var samearea = records.Where( r => 
								( mapareaID == MapAny || int.Parse( mapareaID ) == r.MapAreaID ) &&
								( mapinfoID == MapAny || int.Parse( mapinfoID ) == r.MapInfoID ) &&
								( mapcellID == MapAny || int.Parse( mapcellID ) == r.CellID ) &&
								( mapDifficulty == MapAny || int.Parse( mapDifficulty ) == r.Difficulty ) 
								);

							row.Cells[DropView_Header.Index].Tag = samearea.Count();
							row.Cells[DropView_Date.Index].Tag = samearea.Where( r => r.Rank == "S" || r.Rank == "SS" ).Count();
							row.Cells[DropView_Map.Index].Tag = samearea.Where( r => r.Rank == "A" ).Count();
							row.Cells[DropView_Rank.Index].Tag = samearea.Where( r => r.Rank == "B" ).Count();

						} else {
							var sum = Enumerable.Range( 0, 4 ).Select( j => counts.Values.Sum( v => v[j] ) ).ToArray();

							row.Cells[DropView_Header.Index].Tag = sum[0];
							row.Cells[DropView_Date.Index].Tag = sum[1];
							row.Cells[DropView_Map.Index].Tag = sum[2];
							row.Cells[DropView_Rank.Index].Tag = sum[3];

						}

						rows[i] = row;

						i++;
					}
				}

				DropView.Rows.AddRange( rows );

			}

			DropView.Tag = MergeRows.Checked;
			DropView.Sort( DropView.SortedColumn ?? DropView_Header, DropView.SortOrder == SortOrder.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending );

			if ( MergeRows.Checked ) {
				DropView_Header.HeaderText = "回数";
				DropView_Header.Width = 100;
				DropView_Header.DisplayIndex = 1;
				DropView_Date.HeaderText = "S勝利";
				DropView_Date.Width = 100;
				DropView_Date.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				DropView_Map.HeaderText = "A勝利";
				DropView_Map.Width = 100;
				DropView_Map.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				DropView_Rank.HeaderText = "B勝利";
				DropView_Rank.Width = 100;
				DropView_Rank.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

			} else {
				DropView_Header.HeaderText = "";
				DropView_Header.Width = 50;
				DropView_Header.DisplayIndex = 0;
				DropView_Date.HeaderText = "日付";
				DropView_Date.Width = 150;
				DropView_Date.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
				DropView_Map.HeaderText = "海域";
				DropView_Map.Width = 120;
				DropView_Map.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
				DropView_Rank.HeaderText = "ランク";
				DropView_Rank.Width = 40;
				DropView_Rank.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

			}


			StripLabel_RecordCount.Text = string.Format( "{0} / {1} 件のレコード", records.Count(), _record.Record.Count );
			StripLabel_RecordCount.Tag = records.Count();
		}


		private string GetContentString( ShipDropRecord.ShipDropElement elem, bool ignoreShip = false, bool ignoreItem = false, bool ignoreEquipment = false ) {

			if ( elem.ShipID > 0 && !ignoreShip ) {

				if ( elem.ItemID > 0 && !ignoreItem ) {
					if ( elem.EquipmentID > 0 && !ignoreEquipment )
						return elem.ShipName + " + " + elem.ItemName + " + " + elem.EquipmentName;
					else
						return elem.ShipName + " + " + elem.ItemName;
				} else {
					if ( elem.EquipmentID > 0 && !ignoreEquipment )
						return elem.ShipName + " + " + elem.EquipmentName;
					else
						return elem.ShipName;
				}

			} else {
				if ( elem.ItemID > 0 && !ignoreItem ) {
					if ( elem.EquipmentID > 0 && !ignoreEquipment )
						return elem.ItemName + " + " + elem.EquipmentName;
					else
						return elem.ItemName;
				} else {
					if ( elem.EquipmentID > 0 && !ignoreEquipment )
						return elem.EquipmentName;
					else
						return elem.ShipName;
				}
			}

		}

		private string GetMapString( int maparea, int mapinfo, int cell = -1, bool isboss = false, int difficulty = -1 ) {
			var sb = new StringBuilder();
			sb.Append( maparea );
			sb.Append( "-" );
			sb.Append( mapinfo );
			if ( difficulty != -1 )
				sb.AppendFormat( "[{0}]", Constants.GetDifficulty( difficulty ) );
			if ( cell != -1 ) {
				sb.Append( "-" );
				sb.Append( cell );
			}
			if ( isboss )
				sb.Append( " [ボス]" );

			var enemy = RecordManager.Instance.EnemyFleet.Record.Values.FirstOrDefault( r => r.MapAreaID == maparea && r.MapInfoID == mapinfo && r.CellID == cell && r.Difficulty == difficulty );
			if ( enemy != null )
				sb.AppendFormat( " ({0})", enemy.FleetName );

			return sb.ToString();
		}


		private void ButtonRun_Click( object sender, EventArgs e ) {

			UpdateDropView();
		}


		private void DropView_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e ) {

			if ( (bool)DropView.Tag ) {
				// merged

				if ( e.ColumnIndex == DropView_Header.Index ||
					 e.ColumnIndex == DropView_Date.Index ||
					 e.ColumnIndex == DropView_Map.Index ||
					 e.ColumnIndex == DropView_Rank.Index ) {
					int max = (int)DropView[e.ColumnIndex, e.RowIndex].Tag;
					e.Value = string.Format( "{0} ({1:p2})", e.Value, (double)( (int)e.Value ) / Math.Max( max, 1 ) );
					e.FormattingApplied = true;
				}

			} else {
				//not merged

				if ( e.ColumnIndex == DropView_Date.Index ) {
					e.Value = DateTimeHelper.TimeToCSVString( (DateTime)e.Value );
					e.FormattingApplied = true;

				} else if ( e.ColumnIndex == DropView_Rank.Index ) {
					e.Value = Constants.GetWinRank( (int)e.Value );
					e.FormattingApplied = true;
				}
			}
		}


		private void MapAreaID_SelectedIndexChanged( object sender, EventArgs e ) {
			if ( (string)MapAreaID.SelectedItem == MapAny || (string)MapInfoID.SelectedItem == MapAny ) {
				MapCellID.Enabled = false;
				if ( MapCellID.Items.Count > 0 )
					MapCellID.SelectedIndex = 0;

			} else {
				MapCellID.Enabled = true;
				MapCellID.Items.Clear();
				MapCellID.Items.Add( MapAny );
				MapCellID.Items.AddRange( _record.Record
					.Where( r => r.MapAreaID == int.Parse( (string)MapAreaID.SelectedItem ) && r.MapInfoID == int.Parse( (string)MapInfoID.SelectedItem ) )
					.Select( r => r.CellID )
					.Distinct()
					.OrderBy( i => i )
					.Select( i => i.ToString() ).ToArray() );
				MapCellID.SelectedIndex = 0;
			}
		}


	}
}
