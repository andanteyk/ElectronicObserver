using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog {
	public partial class DialogDropRecordViewer : Form {

		private ShipDropRecord _record;

		private const string NameAny = "(全て)";
		private const string NameNotExist = "(なし)";
		private const string NameFullPort = "(満員)";
		private const string NameExist = "(ドロップ)";

		private const string MapAny = "*";

		private Dictionary<int, DataTable> MapCellTable;


		private class SearchArgument {
			public string ShipName;
			public string ItemName;
			public string EquipmentName;
			public DateTime DateBegin;
			public DateTime DateEnd;
			public int MapAreaID;
			public int MapInfoID;
			public int MapCellID;
			public int MapDifficulty;
			public CheckState IsBossOnly;
			public bool RankS;
			public bool RankA;
			public bool RankB;
			public bool RankX;
			public bool MergeRows;
			public DataGridViewRow BaseRow;
		}


		public DialogDropRecordViewer() {
			InitializeComponent();

			_record = RecordManager.Instance.ShipDrop;
		}

		private void DialogDropRecordViewer_Load( object sender, EventArgs e ) {

			var includedShipNames = _record.Record
				.Select( r => r.ShipName )
				.Distinct()
				.Except( new[] { NameNotExist, NameFullPort } );

			var includedShipObjects = includedShipNames
				.Select( name => KCDatabase.Instance.MasterShips.Values.FirstOrDefault( ship => ship.NameWithClass == name ) )
				.Where( s => s != null );

			var removedShipNames = includedShipNames.Except( includedShipObjects.Select( s => s.NameWithClass ) );


			var includedItemNames = _record.Record
				.Select( r => r.ItemName )
				.Distinct()
				.Except( new[] { NameNotExist } );

			var includedItemObjects = includedItemNames
				.Select( name => KCDatabase.Instance.MasterUseItems.Values.FirstOrDefault( item => item.Name == name ) )
				.Where( s => s != null );

			var removedItemNames = includedItemNames.Except( includedItemObjects.Select( item => item.Name ) );

			var dtbase = new DataTable();
			dtbase.Columns.AddRange( new DataColumn[] {
				new DataColumn( "Value", typeof( int ) ),
				new DataColumn( "Display", typeof( string ) ),
			} );



			MapCellTable = new Dictionary<int, DataTable>();
			{
				var dict = new Dictionary<int, HashSet<int>>();

				foreach ( var r in _record.Record ) {
					int id = r.MapAreaID * 10 + r.MapInfoID;

					if ( !dict.ContainsKey( id ) ) {
						dict.Add( id, new HashSet<int>() );
					}

					dict[id].Add( r.CellID );
				}

				foreach ( var p in dict ) {
					MapCellTable.Add( p.Key, dtbase.Clone() );
					MapCellTable[p.Key].Rows.Add( -1, MapAny );
					foreach ( var c in p.Value.OrderBy( k => k ) )
						MapCellTable[p.Key].Rows.Add( c, c.ToString() );
					MapCellTable[p.Key].AcceptChanges();
				}
			}



			ShipName.Items.Add( NameAny );
			ShipName.Items.Add( NameExist );
			ShipName.Items.Add( NameNotExist );
			ShipName.Items.Add( NameFullPort );
			ShipName.Items.AddRange( includedShipObjects
				.OrderBy( s => s.NameReading )
				.OrderBy( s => s.ShipType )
				.Select( s => s.NameWithClass )
				.Union( removedShipNames.OrderBy( s => s ) )
				.ToArray()
				);
			ShipName.SelectedIndex = 0;

			ItemName.Items.Add( NameAny );
			ItemName.Items.Add( NameExist );
			ItemName.Items.Add( NameNotExist );
			ItemName.Items.AddRange( includedItemObjects
				.OrderBy( i => i.ItemID )
				.Select( i => i.Name )
				.Union( removedItemNames.OrderBy( i => i ) )
				.ToArray()
				);
			ItemName.SelectedIndex = 0;

			// not implemented: eq


			DateBegin.Value = DateBegin.MinDate = DateEnd.MinDate = _record.Record.First().Date.Date;
			DateEnd.Value = DateBegin.MaxDate = DateEnd.MaxDate = DateTime.Now.AddDays( 1 ).Date;

			{
				DataTable dt = dtbase.Clone();
				dt.Rows.Add( -1, MapAny );
				foreach ( var i in _record.Record
					.Select( r => r.MapAreaID )
					.Distinct()
					.OrderBy( i => i ) )
					dt.Rows.Add( i, i.ToString() );
				dt.AcceptChanges();
				MapAreaID.DisplayMember = "Display";
				MapAreaID.ValueMember = "Value";
				MapAreaID.DataSource = dt;
				MapAreaID.SelectedIndex = 0;
			}

			{
				DataTable dt = dtbase.Clone();
				dt.Rows.Add( -1, MapAny );
				foreach ( var i in _record.Record
					.Select( r => r.MapInfoID )
					.Distinct()
					.OrderBy( i => i ) )
					dt.Rows.Add( i, i.ToString() );
				dt.AcceptChanges();
				MapInfoID.DisplayMember = "Display";
				MapInfoID.ValueMember = "Value";
				MapInfoID.DataSource = dt;
				MapInfoID.SelectedIndex = 0;
			}

			{
				DataTable dt = dtbase.Clone();
				dt.Rows.Add( -1, MapAny );
				// 残りは都度生成する
				dt.AcceptChanges();
				MapCellID.DisplayMember = "Display";
				MapCellID.ValueMember = "Value";
				MapCellID.DataSource = dt;
				MapCellID.SelectedIndex = 0;
			}

			{
				DataTable dt = dtbase.Clone();
				dt.Rows.Add( 0, MapAny );
				foreach ( var diff in _record.Record
					.Select( r => r.Difficulty )
					.Distinct()
					.Except( new[] { 0 } )
					.OrderBy( i => i ) )
					dt.Rows.Add( diff, Constants.GetDifficulty( diff ) );
				dt.AcceptChanges();
				MapDifficulty.DisplayMember = "Display";
				MapDifficulty.ValueMember = "Value";
				MapDifficulty.DataSource = dt;
				MapDifficulty.SelectedIndex = 0;
			}


			foreach ( DataGridViewColumn column in RecordView.Columns )
				column.Width = 20;

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

		private string GetContentStringForSorting( ShipDropRecord.ShipDropElement elem, bool ignoreShip = false, bool ignoreItem = false, bool ignoreEquipment = false ) {

			var ship = KCDatabase.Instance.MasterShips[elem.ShipID];
			var item = KCDatabase.Instance.MasterUseItems[elem.ItemID];
			var eq = KCDatabase.Instance.MasterEquipments[elem.EquipmentID];

			if ( ship != null && ship.Name != elem.ShipName ) ship = null;
			if ( item != null && item.Name != elem.ItemName ) item = null;
			if ( eq != null && eq.Name != elem.EquipmentName ) eq = null;

			StringBuilder sb = new StringBuilder();


			if ( elem.ShipID > 0 && !ignoreShip ) {
				sb.AppendFormat( "0{0:D4}{1}/{2}", ship != null ? ship.ShipType : 0, ship != null ? ship.NameReading : elem.ShipName, elem.ShipName );
			}

			if ( elem.ItemID > 0 && !ignoreItem ) {
				if ( sb.Length > 0 ) sb.Append( "," );
				sb.AppendFormat( "1{0:D4}{1}", item != null ? item.ItemID : 0, elem.ItemName );
			}

			if ( elem.EquipmentID > 0 && !ignoreEquipment ) {
				if ( sb.Length > 0 ) sb.Append( "," );
				sb.AppendFormat( "2{0:D4}{1}", eq != null ? eq.EquipmentID : 0, elem.EquipmentName );
			}

			return sb.ToString();
		}


		private string ConvertContentString( string str ) {

			if ( str.Length == 0 )
				return NameNotExist;

			StringBuilder sb = new StringBuilder();

			foreach ( var s in str.Split( ",".ToCharArray() ) ) {

				if ( sb.Length > 0 )
					sb.Append( " + " );

				switch ( s[0] ) {
					case '0':
						sb.Append( s.Substring( s.IndexOf( "/" ) + 1 ) );
						break;
					case '1':
					case '2':
						sb.Append( s.Substring( 5 ) );
						break;
				}
			}

			return sb.ToString();
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


		private void MapAreaID_SelectedIndexChanged( object sender, EventArgs e ) {

			int maparea = (int)( MapAreaID.SelectedValue ?? -1 );
			int mapinfo = (int)( MapInfoID.SelectedValue ?? -1 );

			if ( maparea == -1 || mapinfo == -1 ) {
				MapCellID.Enabled = false;
				if ( MapCellID.Items.Count > 0 )
					MapCellID.SelectedIndex = 0;

			} else {
				MapCellID.Enabled = true;
				if ( MapCellTable.ContainsKey( maparea * 10 + mapinfo ) ) {
					MapCellID.DataSource = MapCellTable[maparea * 10 + mapinfo];
				} else {
					MapCellID.Enabled = false;
				}
				MapCellID.SelectedIndex = 0;
			}
		}


		private void ButtonRun_Click( object sender, EventArgs e ) {

			if ( Searcher.IsBusy ) {
				if ( MessageBox.Show( "検索を中止しますか?", "検索中です", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2 )
					== System.Windows.Forms.DialogResult.Yes ) {
					Searcher.CancelAsync();
				}
				return;
			}

			RecordView.Rows.Clear();

			var row = new DataGridViewRow();
			row.CreateCells( RecordView );

			var args = new SearchArgument();
			args.ShipName = ShipName.Text;
			args.ItemName = (string)ItemName.SelectedItem;
			args.EquipmentName = (string)EquipmentName.SelectedItem;
			args.DateBegin = DateBegin.Value;
			args.DateEnd = DateEnd.Value;
			args.MapAreaID = (int)MapAreaID.SelectedValue;
			args.MapInfoID = (int)MapInfoID.SelectedValue;
			args.MapCellID = (int)MapCellID.SelectedValue;
			args.MapDifficulty = (int)MapDifficulty.SelectedValue;
			args.IsBossOnly = IsBossOnly.CheckState;
			args.RankS = RankS.Checked;
			args.RankA = RankA.Checked;
			args.RankB = RankB.Checked;
			args.RankX = RankX.Checked;
			args.MergeRows = MergeRows.Checked;
			args.BaseRow = row;

			RecordView.Tag = args;


			// column initialize
			if ( MergeRows.Checked ) {
				RecordView_Name.DisplayIndex = 0;
				RecordView_Header.HeaderText = "回数";
				RecordView_Header.Width = 100;
				RecordView_Header.DisplayIndex = 1;
				RecordView_RankS.Width = 100;
				RecordView_RankS.Visible = true;
				RecordView_RankA.Width = 100;
				RecordView_RankA.Visible = true;
				RecordView_RankB.Width = 100;
				RecordView_RankB.Visible = true;

				RecordView_Date.Visible = false;
				RecordView_Map.Visible = false;
				RecordView_Rank.Visible = false;

			} else {
				RecordView_Header.HeaderText = "";
				RecordView_Header.Width = 50;
				RecordView_Header.DisplayIndex = 0;
				RecordView_Date.Width = 150;
				RecordView_Date.Visible = true;
				RecordView_Map.Width = 240;
				RecordView_Map.Visible = true;
				RecordView_Rank.Width = 40;
				RecordView_Rank.Visible = true;

				RecordView_RankS.Visible = false;
				RecordView_RankA.Visible = false;
				RecordView_RankB.Visible = false;

			}
			RecordView.ColumnHeadersVisible = true;


			StatusInfo.Text = "検索中です...";
			StatusInfo.Tag = DateTime.Now;

			Searcher.RunWorkerAsync( args );
		}


		private void Searcher_DoWork( object sender, DoWorkEventArgs e ) {

			SearchArgument args = (SearchArgument)e.Argument;


			int priorityShip = 
				args.ShipName == NameAny ? 0 :
				args.ShipName == NameExist ? 1 : 2;
			int priorityItem =
				args.ItemName == NameAny ? 0 :
				args.ItemName == NameExist ? 1 : 2;
			int priorityContent = Math.Max( priorityShip, priorityItem );

			var records = RecordManager.Instance.ShipDrop.Record;
			var rows = new LinkedList<DataGridViewRow>();


			//lock ( records ) 
			{
				int i = 0;
				var counts = new Dictionary<string, int[]>();
				var allcounts = new Dictionary<string, int[]>();


				foreach ( var r in records ) {

					#region Filtering

					if ( r.Date < args.DateBegin || args.DateEnd < r.Date )
						continue;

					if ( ( ( r.Rank == "SS" || r.Rank == "S" ) && !args.RankS ) ||
						 ( ( r.Rank == "A" ) && !args.RankA ) ||
						 ( ( r.Rank == "B" ) && !args.RankB ) ||
						 ( ( Constants.GetWinRank( r.Rank ) <= 3 ) && !args.RankX ) )
						continue;


					if ( args.MapAreaID != -1 && args.MapAreaID != r.MapAreaID )
						continue;
					if ( args.MapInfoID != -1 && args.MapInfoID != r.MapInfoID )
						continue;
					if ( args.MapCellID != -1 && args.MapCellID != r.CellID )
						continue;
					switch ( args.IsBossOnly ) {
						case CheckState.Unchecked:
							if ( r.IsBossNode )
								continue;
							break;
						case CheckState.Checked:
							if ( !r.IsBossNode )
								continue;
							break;
					}
					if ( args.MapDifficulty != 0 && args.MapDifficulty != r.Difficulty )
						continue;



					if ( args.MergeRows ) {
						string key;

						if ( priorityContent == 2 ) {
							key = GetMapSerialID( r.MapAreaID, r.MapInfoID, r.CellID, r.IsBossNode, args.MapDifficulty == 0 ? -1 : r.Difficulty ).ToString( "X8" );

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



					switch ( args.ShipName ) {
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
							if ( r.ShipName != args.ShipName )
								continue;
							break;
					}

					switch ( args.ItemName ) {
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
							if ( r.ItemName != args.ItemName )
								continue;
							break;
					}

					#endregion


					if ( !args.MergeRows ) {
						var row = (DataGridViewRow)args.BaseRow.Clone();

						row.SetValues(
							i + 1,
							GetContentString( r ),
							r.Date,
							GetMapString( r.MapAreaID, r.MapInfoID, r.CellID, r.IsBossNode, r.Difficulty ),
							Constants.GetWinRank( r.Rank ),
							null,
							null,
							null
							);

						row.Cells[1].Tag = GetContentStringForSorting( r );
						row.Cells[3].Tag = GetMapSerialID( r.MapAreaID, r.MapInfoID, r.CellID, r.IsBossNode, r.Difficulty );

						rows.AddLast( row );


					} else {
						//merged

						string key;

						if ( priorityContent == 2 ) {
							key = GetMapSerialID( r.MapAreaID, r.MapInfoID, r.CellID, r.IsBossNode, args.MapDifficulty == 0 ? -1 : r.Difficulty ).ToString( "X8" );

						} else {
							key = GetContentStringForSorting( r, priorityShip < priorityItem && priorityShip < 2, priorityShip >= priorityItem && priorityItem < 2 );
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


				if ( args.MergeRows ) {

					int[] allcountssum = Enumerable.Range( 0, 4 ).Select( k => allcounts.Values.Sum( a => a[k] ) ).ToArray();

					foreach ( var c in counts ) {
						var row = (DataGridViewRow)args.BaseRow.Clone();

						string name = c.Key;

						int serialID = 0;
						if ( int.TryParse( name, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out serialID ) )
							name = GetMapString( serialID );

						// fixme: name != map だった時にソートキーが入れられない

						row.SetValues(
							c.Value[0],
							serialID != 0 ? name : ConvertContentString( name ),
							null,
							null,
							null,
							c.Value[1],
							c.Value[2],
							c.Value[3]
							);


						if ( priorityContent == 2 ) {
							row.Cells[0].Tag = allcounts[c.Key][0];
							if ( serialID != 0 )
								row.Cells[1].Tag = serialID;
							else
								row.Cells[1].Tag = name;
							row.Cells[5].Tag = allcounts[c.Key][1];
							row.Cells[6].Tag = allcounts[c.Key][2];
							row.Cells[7].Tag = allcounts[c.Key][3];

						} else {
							row.Cells[0].Tag = ( (double)c.Value[0] / Math.Max( allcountssum[0], 1 ) );
							if ( serialID != 0 )
								row.Cells[1].Tag = serialID;
							else
								row.Cells[1].Tag = name;
							row.Cells[5].Tag = ( (double)c.Value[1] / Math.Max( allcountssum[1], 1 ) );
							row.Cells[6].Tag = ( (double)c.Value[2] / Math.Max( allcountssum[2], 1 ) );
							row.Cells[7].Tag = ( (double)c.Value[3] / Math.Max( allcountssum[3], 1 ) );

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

				RecordView.Rows.AddRange( rows );

				RecordView.Sort( RecordView.SortedColumn ?? RecordView_Header,
					RecordView.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending );

				StatusInfo.Text = "検索が完了しました。(" + (int)( DateTime.Now - (DateTime)StatusInfo.Tag ).TotalMilliseconds + " ms)";

			} else {

				StatusInfo.Text = "検索がキャンセルされました。";
			}

		}


		private void RecordView_SortCompare( object sender, DataGridViewSortCompareEventArgs e ) {

			object tag1 = RecordView[e.Column.Index, e.RowIndex1].Tag;
			object tag2 = RecordView[e.Column.Index, e.RowIndex2].Tag;

			if ( tag1 != null && ( tag1 is double || tag1 is int ) && e.CellValue1 is int ) {
				double c1 = 0 , c2 = 0;

				if ( tag1 is double ) {
					c1 = (double)tag1;
					c2 = (double)tag2;
				} else if ( tag1 is int ) {
					c1 = (double)(int)e.CellValue1 / Math.Max( (int)tag1, 1 );
					c2 = (double)(int)e.CellValue2 / Math.Max( (int)tag2, 1 );
				}


				if ( Math.Abs( c1 - c2 ) < 0.000001 )
					e.SortResult = (int)e.CellValue1 - (int)e.CellValue2;
				else if ( c1 < c2 )
					e.SortResult = -1;
				else
					e.SortResult = 1;
				e.Handled = true;

			} else if ( tag1 is string ) {
				e.SortResult = ( (IComparable)tag1 ).CompareTo( tag2 );
				e.Handled = true;
			} else if ( tag1 is int ) {
				e.SortResult = (int)tag1 - (int)tag2;
				e.Handled = true;
			}


			if ( !e.Handled ) {
				e.SortResult = ( (IComparable)e.CellValue1 ?? 0 ).CompareTo( e.CellValue2 ?? 0 );
				e.Handled = true;
			}

			if ( e.SortResult == 0 ) {
				e.SortResult = (int)( RecordView.Rows[e.RowIndex1].Tag ?? 0 ) - (int)( RecordView.Rows[e.RowIndex2].Tag ?? 0 );
			}

		}

		private void RecordView_Sorted( object sender, EventArgs e ) {

			for ( int i = 0; i < RecordView.Rows.Count; i++ ) {
				RecordView.Rows[i].Tag = i;
			}

		}


		private void RecordView_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e ) {

			SearchArgument args = (SearchArgument)RecordView.Tag;

			if ( args.MergeRows ) {
				// merged

				if ( e.ColumnIndex == RecordView_Header.Index ||
					 e.ColumnIndex == RecordView_RankS.Index ||
					 e.ColumnIndex == RecordView_RankA.Index ||
					 e.ColumnIndex == RecordView_RankB.Index ) {

					if ( RecordView[e.ColumnIndex, e.RowIndex].Tag is double ) {
						e.Value = string.Format( "{0} ({1:p1})", e.Value, (double)RecordView[e.ColumnIndex, e.RowIndex].Tag );
					} else {
						int max = (int)RecordView[e.ColumnIndex, e.RowIndex].Tag;
						e.Value = string.Format( "{0}/{1} ({2:p1})", e.Value, max, (double)( (int)e.Value ) / Math.Max( max, 1 ) );
					}
					e.FormattingApplied = true;
				}

			} else {
				//not merged

				if ( e.ColumnIndex == RecordView_Date.Index ) {
					e.Value = DateTimeHelper.TimeToCSVString( (DateTime)e.Value );
					e.FormattingApplied = true;

				} else if ( e.ColumnIndex == RecordView_Rank.Index ) {
					e.Value = Constants.GetWinRank( (int)e.Value );
					e.FormattingApplied = true;
				}
			}

		}


		private void RecordView_CellDoubleClick( object sender, DataGridViewCellEventArgs e ) {

			SearchArgument args = (SearchArgument)RecordView.Tag;
			if ( args == null || args.MergeRows )
				return;

			try {

				DateTime time = Convert.ToDateTime( RecordView[RecordView_Date.Index, e.RowIndex].Value );


				if ( !Directory.Exists( Data.Battle.BattleManager.BattleLogPath ) ) {
					StatusInfo.Text = "戦闘ログが見つかりませんでした。";
					return;
				}

				StatusInfo.Text = "戦闘ログを検索しています…";
				string battleLogFile = Directory.EnumerateFiles( Data.Battle.BattleManager.BattleLogPath,
					time.ToString( "yyyyMMdd_HHmmss", System.Globalization.CultureInfo.InvariantCulture ) + "*.txt",
					SearchOption.TopDirectoryOnly )
					.FirstOrDefault();

				if ( battleLogFile == null ) {
					StatusInfo.Text = "戦闘ログが見つかりませんでした。";
					return;
				}

				StatusInfo.Text = string.Format( "戦闘ログ {0} を開きます。", Path.GetFileName( battleLogFile ) );
				System.Diagnostics.Process.Start( battleLogFile );


			} catch ( Exception ) {
				StatusInfo.Text = "戦闘ログを開けませんでした。";
			}

		}

	}
}
