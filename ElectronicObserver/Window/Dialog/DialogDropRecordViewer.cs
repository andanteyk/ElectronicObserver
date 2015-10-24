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

		private string GetMapString( int maparea, int mapinfo, int cell = -1, bool isboss = false, int difficulty = -1, bool insertEnemyFleetName = true ) {
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

			if ( insertEnemyFleetName ) {
				var enemy = RecordManager.Instance.EnemyFleet.Record.Values.FirstOrDefault( r => r.MapAreaID == maparea && r.MapInfoID == mapinfo && r.CellID == cell && r.Difficulty == difficulty );
				if ( enemy != null )
					sb.AppendFormat( " ({0})", enemy.FleetName );
			}

			return sb.ToString();
		}

		private string GetMapString( int serialID, bool insertEnemyFleetName = true ) {
			return GetMapString( serialID >> 24 & 0xFF, serialID >> 16 & 0xFF, serialID >> 8 & 0xFF, ( serialID & 1 ) != 0, (sbyte)( ( serialID >> 1 & 0x7F ) << 1 ) >> 1, insertEnemyFleetName );
		}

		private int GetMapSerialID( int maparea, int mapinfo, int cell, bool isboss, int difficulty = -1 ) {
			return ( maparea & 0xFF ) << 24 | ( mapinfo & 0xFF ) << 16 | ( cell & 0xFF ) << 8 | ( difficulty & 0x7F ) << 1 | ( isboss ? 1 : 0 );
		}


		private void ButtonRun_Click( object sender, EventArgs e ) {

			if ( Searcher.IsBusy ) {
				if ( MessageBox.Show( "検索を中止しますか?", "検索中です", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2 )
					== System.Windows.Forms.DialogResult.Yes ) {
					Searcher.CancelAsync();
				}
				return;
			}

			DropView.Rows.Clear();

			var row = new DataGridViewRow();
			row.CreateCells( DropView );

			DropView.Tag = MergeRows.Checked;

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
				DropView_Map.Width = 240;
				DropView_Map.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
				DropView_Rank.HeaderText = "ランク";
				DropView_Rank.Width = 40;
				DropView_Rank.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

			}

			Searcher.RunWorkerAsync( new object[] { 
				ShipName.SelectedItem,
				ItemName.SelectedItem,
				EquipmentName.SelectedItem,
				DateBegin.Value,
				DateEnd.Value,
				RankS.Checked,
				RankA.Checked,
				RankB.Checked,
				RankX.Checked,
				MapAreaID.SelectedItem,
				MapInfoID.SelectedItem,
				MapCellID.SelectedItem,
				IsBossOnly.CheckState,
				MapDifficulty.SelectedItem,
				MergeRows.Checked,
				row
				} );
		}


		private void DropView_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e ) {

			if ( (bool)DropView.Tag ) {
				// merged

				if ( e.ColumnIndex == DropView_Header.Index ||
					 e.ColumnIndex == DropView_Date.Index ||
					 e.ColumnIndex == DropView_Map.Index ||
					 e.ColumnIndex == DropView_Rank.Index ) {

					if ( DropView[e.ColumnIndex, e.RowIndex].Tag is double ) {
						e.Value = string.Format( "{0} ({1:p1})", e.Value, (double)DropView[e.ColumnIndex, e.RowIndex].Tag );
					} else {
						int max = (int)DropView[e.ColumnIndex, e.RowIndex].Tag;
						e.Value = string.Format( "{0}/{1} ({2:p1})", e.Value, max, (double)( (int)e.Value ) / Math.Max( max, 1 ) );
					}
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





		private void Searcher_DoWork( object sender, DoWorkEventArgs e ) {

			object[] args = (object[])e.Argument;

			string shipName = (string)args[0];
			string itemName = (string)args[1];
			string equipmentName = (string)args[2];
			DateTime dateBegin = (DateTime)args[3];
			DateTime dateEnd = (DateTime)args[4];
			bool rankS = (bool)args[5];
			bool rankA = (bool)args[6];
			bool rankB = (bool)args[7];
			bool rankX = (bool)args[8];
			string mapArea = (string)args[9];
			string mapInfo = (string)args[10];
			string mapCell = (string)args[11];
			CheckState isBoss = (CheckState)args[12];
			string difficulty = (string)args[13];
			bool mergeRows = (bool)args[14];
			DataGridViewRow origin = (DataGridViewRow)args[15];


			int priorityShip = 
				shipName == NameAny ? 0 :
				shipName == NameExist ? 1 : 2;
			int priorityItem =
				itemName == NameAny ? 0 :
				itemName == NameExist ? 1 : 2;
			int priorityContent = Math.Max( priorityShip, priorityItem );

			var records = RecordManager.Instance.ShipDrop.Record;
			var rows = new LinkedList<DataGridViewRow>();


			//lock ( records ) 
			{
				int i = 0;
				var allcounts = new Dictionary<string, int[]>();
				var counts = new Dictionary<string, int[]>();


				foreach ( var r in records ) {

					#region Filtering



					if ( r.Date < dateBegin || dateEnd < r.Date )
						continue;

					if ( ( ( r.Rank == "SS" || r.Rank == "S" ) && !rankS ) ||
						 ( ( r.Rank == "A" ) && !rankA ) ||
						 ( ( r.Rank == "B" ) && !rankB ) ||
						 ( ( Constants.GetWinRank( r.Rank ) <= 3 ) && !rankX ) )
						continue;


					if ( mapArea != MapAny && int.Parse( mapArea ) != r.MapAreaID )
						continue;
					if ( mapInfo != MapAny && int.Parse( mapInfo ) != r.MapInfoID )
						continue;
					if ( mapCell != MapAny && int.Parse( mapCell ) != r.CellID )
						continue;
					switch ( isBoss ) {
						case CheckState.Unchecked:
							if ( r.IsBossNode )
								continue;
							break;
						case CheckState.Checked:
							if ( !r.IsBossNode )
								continue;
							break;
					}
					if ( difficulty != MapAny && difficulty != Constants.GetDifficulty( r.Difficulty ) )
						continue;



					if ( mergeRows ) {
						string key;

						if ( priorityContent == 2 ) {
							key = GetMapSerialID( r.MapAreaID, r.MapInfoID, r.CellID, r.IsBossNode, difficulty == MapAny ? -1 : r.Difficulty ).ToString( "X8" );

						} else {
							key = GetContentString( r, priorityShip < priorityItem && priorityShip < 2, priorityShip >= priorityItem && priorityItem < 2 );
						}


						if ( !allcounts.ContainsKey( key ) ) {
							allcounts.Add( key, new int[4] );
						}

						switch ( r.Rank ) {
							case "B":
								allcounts[key][3]++;
								break;
							case "A":
								allcounts[key][2]++;
								break;
							case "S":
							case "SS":
								allcounts[key][1]++;
								break;
						}
						allcounts[key][0]++;
					}



					switch ( shipName ) {
						case NameAny:
							break;
						case NameExist:
							if ( r.ShipID < 0 )
								continue;
							break;
						case NameNotExist:
							if ( r.ShipID != -1 )
								continue;
							break;
						case NameFullPort:
							if ( r.ShipID != -2 )
								continue;
							break;
						default:
							if ( r.ShipName != shipName )
								continue;
							break;
					}

					switch ( itemName ) {
						case NameAny:
							break;
						case NameExist:
							if ( r.ItemID < 0 )
								continue;
							break;
						case NameNotExist:
							if ( r.ItemID != -1 )
								continue;
							break;
						default:
							if ( r.ItemName != itemName )
								continue;
							break;
					}

					#endregion


					if ( !mergeRows ) {
						var row = (DataGridViewRow)origin.Clone();

						row.SetValues(
							i + 1,
							GetContentString( r ),
							r.Date,
							GetMapString( r.MapAreaID, r.MapInfoID, r.CellID, r.IsBossNode, r.Difficulty ),
							Constants.GetWinRank( r.Rank )
							);

						row.Cells[3].Tag = GetMapSerialID( r.MapAreaID, r.MapInfoID, r.CellID, r.IsBossNode, r.Difficulty );

						rows.AddLast( row );


					} else {
						//merged

						string key;

						if ( priorityContent == 2 ) {
							key = GetMapSerialID( r.MapAreaID, r.MapInfoID, r.CellID, r.IsBossNode, difficulty == MapAny ? -1 : r.Difficulty ).ToString( "X8" );

						} else {
							key = GetContentString( r, priorityShip < priorityItem && priorityShip < 2, priorityShip >= priorityItem && priorityItem < 2 );
						}


						if ( !counts.ContainsKey( key ) ) {
							counts.Add( key, new int[4] );
						}

						switch ( r.Rank ) {
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



					if ( Searcher.CancellationPending )
						break;

					i++;
				}


				if ( mergeRows ) {

					int[] allcountssum = Enumerable.Range( 0, 4 ).Select( k => allcounts.Values.Sum( a => a[k] ) ).ToArray();

					foreach ( var c in counts ) {
						var row = (DataGridViewRow)origin.Clone();

						string name = c.Key;

						int serialID = 0;
						if ( int.TryParse( name, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out serialID ) )
							name = GetMapString( serialID );


						row.SetValues(
							c.Value[0],
							name,
							c.Value[1],
							c.Value[2],
							c.Value[3]
							);


						if ( priorityContent == 2 ) {
							row.Cells[0].Tag = allcounts[c.Key][0];
							row.Cells[1].Tag = serialID;
							row.Cells[2].Tag = allcounts[c.Key][1];
							row.Cells[3].Tag = allcounts[c.Key][2];
							row.Cells[4].Tag = allcounts[c.Key][3];

						} else {
							row.Cells[0].Tag = ( (double)c.Value[0] / Math.Max( allcountssum[0], 1 ) );
							row.Cells[1].Tag = serialID;
							row.Cells[2].Tag = ( (double)c.Value[1] / Math.Max( allcountssum[1], 1 ) );
							row.Cells[3].Tag = ( (double)c.Value[2] / Math.Max( allcountssum[2], 1 ) );
							row.Cells[4].Tag = ( (double)c.Value[3] / Math.Max( allcountssum[3], 1 ) );

						}

						rows.AddLast( row );
					}

				}

			}

			e.Result = rows.ToArray();
		}



		private void Searcher_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e ) {

			if ( !e.Cancelled ) {
				var rows = (DataGridViewRow[])e.Result;

				DropView.Rows.AddRange( rows );

				DropView.Sort( DropView.SortedColumn ?? DropView_Header, DropView.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending );
			}

		}


		private void DropView_SortCompare( object sender, DataGridViewSortCompareEventArgs e ) {

			if ( (bool)DropView.Tag ) {
				if ( e.Column.Index == DropView_Header.Index ||
					 e.Column.Index == DropView_Date.Index ||
					 e.Column.Index == DropView_Map.Index ||
					 e.Column.Index == DropView_Rank.Index ) {
					var cell1 = DropView[e.Column.Index, e.RowIndex1];
					var cell2 = DropView[e.Column.Index, e.RowIndex2];

					double c1, c2;
					if ( cell1.Tag is double ) {
						c1 = (double)cell1.Tag;
						c2 = (double)cell2.Tag;
					} else {
						c1 = (double)(int)cell1.Value / Math.Max( (int)cell1.Tag, 1 );
						c2 = (double)(int)cell2.Value / Math.Max( (int)cell2.Tag, 1 );
					}

					if ( Math.Abs( c1 - c2 ) < 0.000001 )		// 誤差がこれ以下なら一致とみなして分子で比較
						e.SortResult = (int)cell1.Value - (int)cell2.Value;
					else if ( c1 < c2 )
						e.SortResult = -1;
					else
						e.SortResult = 1;

					e.Handled = true;

				} else if ( e.Column.Index == DropView_Name.Index ) {
					var cell1 = DropView[e.Column.Index, e.RowIndex1];
					var cell2 = DropView[e.Column.Index, e.RowIndex2];

					if ( cell1.Tag is int ) {		//serialID
						e.SortResult = (int)cell1.Tag - (int)cell2.Tag;
						e.Handled = true;
					}
				}

			} else {

				if ( e.Column.Index == DropView_Map.Index ) {
					var cell1 = DropView[e.Column.Index, e.RowIndex1];
					var cell2 = DropView[e.Column.Index, e.RowIndex2];

					if ( cell1.Tag is int ) {		//serialID
						e.SortResult = (int)cell1.Tag - (int)cell2.Tag;
						e.Handled = true;
					}
				}
			}

			if ( !e.Handled ) {
				e.SortResult = ( (IComparable)e.CellValue1 ).CompareTo( e.CellValue2 );
				e.Handled = true;
			}

			if ( e.SortResult == 0 ) {
				e.SortResult = (int)( DropView.Rows[e.RowIndex1].Tag ?? 0 ) - (int)( DropView.Rows[e.RowIndex2].Tag ?? 0 );
			}

		}

		private void DropView_Sorted( object sender, EventArgs e ) {

			for ( int i = 0; i < DropView.Rows.Count; i++ ) {
				DropView.Rows[i].Tag = i;
			}

		}


	}
}
