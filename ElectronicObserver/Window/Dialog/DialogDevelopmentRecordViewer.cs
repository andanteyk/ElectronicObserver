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
	public partial class DialogDevelopmentRecordViewer : Form {

		private DevelopmentRecord _record;

		private const string NameAny = "(全て)";
		private const string NameNotExist = "(失敗)";
		private const string NameExist = "(成功)";



		public DialogDevelopmentRecordViewer() {
			InitializeComponent();

			_record = RecordManager.Instance.Development;
		}

		private void DialogDevelopmentRecordViewer_Load( object sender, EventArgs e ) {

			EquipmentName.Items.Add( NameAny );
			EquipmentName.Items.Add( NameExist );
			EquipmentName.Items.Add( NameNotExist );
			EquipmentName.Items.AddRange( _record.Record
					.Where( r => r.EquipmentID != -1 )
					.GroupBy( r => r.EquipmentID, ( key, r ) => r.First() )
					.OrderBy( r => r.EquipmentID )
					.OrderBy( r => KCDatabase.Instance.MasterEquipments[r.EquipmentID].CategoryType )
					.Select( r => r.EquipmentName )
					.ToArray() );
			EquipmentName.SelectedIndex = 0;

			{
				DataTable dt = new DataTable();
				dt.Columns.AddRange( new DataColumn[] {
					new DataColumn( "Value", typeof( int ) ),
					new DataColumn( "Display", typeof( string ) ),
				} );
				dt.Rows.Add( -1, NameAny );
				foreach ( var record in _record.Record
					.Where( r => r.EquipmentID != -1 )
					.Select( r => KCDatabase.Instance.MasterEquipments[r.EquipmentID] )
					.GroupBy( r => r.CategoryType, ( key, r ) => r.First() )
					.OrderBy( r => r.CategoryType )
					) {
					dt.Rows.Add( record.CategoryType, record.CategoryTypeInstance.Name );
				}
				dt.AcceptChanges();
				EquipmentCategory.DisplayMember = "Display";
				EquipmentCategory.ValueMember = "Value";
				EquipmentCategory.DataSource = dt;
				EquipmentCategory.SelectedIndex = 0;
			}

			{
				DataTable dt = new DataTable();
				dt.Columns.AddRange( new DataColumn[] {
					new DataColumn( "Value", typeof( int ) ),
					new DataColumn( "Display", typeof( string ) ),
				} );
				dt.Rows.Add( -1, NameAny );
				foreach ( var category in _record.Record
					.Select( r => r.FlagshipType )
					.Distinct()
					.OrderBy( i => i ) ) {
					dt.Rows.Add( category, KCDatabase.Instance.ShipTypes[category].Name );
				}
				dt.AcceptChanges();
				SecretaryCategory.DisplayMember = "Display";
				SecretaryCategory.ValueMember = "Value";
				SecretaryCategory.DataSource = dt;
				SecretaryCategory.SelectedIndex = 0;
			}

			SecretaryShipName.Items.Add( NameAny );
			SecretaryShipName.Items.AddRange(
				_record.Record.Select( r => r.FlagshipName )
				.Distinct().OrderBy( s => s ).ToArray() );
			SecretaryShipName.SelectedIndex = 0;

			DateBegin.Value = DateBegin.MinDate = DateEnd.MinDate = _record.Record.First().Date.Date;
			DateEnd.Value = DateBegin.MaxDate = DateEnd.MaxDate = DateTime.Now.AddDays( 1 ).Date;

			Recipe.Items.Add( NameAny );
			Recipe.Items.AddRange( _record.Record
				.Select( r => new[] { r.Fuel, r.Ammo, r.Steel, r.Bauxite } )
				.OrderBy( r => r[3] )
				.OrderBy( r => r[2] )
				.OrderBy( r => r[1] )
				.OrderBy( r => r[0] )
				.Select( a => GetRecipeString( a ) ).Distinct().ToArray() );
			Recipe.SelectedIndex = 0;

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.ItemDevelopmentMaterial] );
		}


		private string GetRecipeString( int[] resources ) {
			return string.Join( "/", resources );
		}

		private string GetRecipeString( int fuel, int ammo, int steel, int bauxite ) {
			return GetRecipeString( new int[] { fuel, ammo, steel, bauxite } );
		}

		private int[] GetResources( string recipe ) {
			return recipe.Split( "/".ToCharArray() ).Select( s => int.Parse( s ) ).ToArray();
		}

		private void DialogDevelopmentRecordViewer_FormClosed( object sender, FormClosedEventArgs e ) {
			ResourceManager.DestroyIcon( Icon );
		}


		private void ButtonRun_Click( object sender, EventArgs e ) {

			if ( Searcher.IsBusy ) {
				if ( MessageBox.Show( "検索を中止しますか?", "検索中です", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2 )
					== System.Windows.Forms.DialogResult.Yes ) {
					Searcher.CancelAsync();
				}
				return;
			}

			DevelopmentView.Rows.Clear();

			var row = new DataGridViewRow();
			row.CreateCells( DevelopmentView );

			
			if ( !MergeRows.Checked ) {
				DevelopmentView_Header.Width = 50;
				DevelopmentView_Header.HeaderText = "";
				DevelopmentView_Name.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				DevelopmentView_Name.HeaderText = "装備名";
				DevelopmentView_Date.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
				DevelopmentView_Date.HeaderText = "日付";
				DevelopmentView_Date.Width = 140;
				DevelopmentView_Recipe.Width = 120;
				DevelopmentView_Recipe.Visible = true;
				DevelopmentView_FlagshipType.Width = 60;
				DevelopmentView_FlagshipType.Visible = true;
				DevelopmentView_Flagship.Width = 60;
				DevelopmentView_Flagship.Visible = true;
			} else {
				DevelopmentView_Header.Width = 150;
				DevelopmentView_Header.HeaderText = "回数";
				DevelopmentView_Name.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
				DevelopmentView_Name.Width = 160;
				DevelopmentView_Name.HeaderText = ( ( EquipmentName.Text != NameAny && EquipmentName.Text != NameExist ) || (int)EquipmentCategory.SelectedValue != -1 ) ? "レシピ" : "装備";
				DevelopmentView_Date.HeaderText = ( SecretaryShipName.Text != NameAny || (int)SecretaryCategory.SelectedValue != -1 ) ? "レシピ別回数" : "艦種別回数";
				DevelopmentView_Date.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				DevelopmentView_Recipe.Visible = false;
				DevelopmentView_FlagshipType.Visible = false;
				DevelopmentView_Flagship.Visible = false;
			}
			DevelopmentView.ColumnHeadersVisible = true;

			Searcher.RunWorkerAsync( new object[] { 
				EquipmentName.SelectedItem,
				EquipmentCategory.SelectedValue,
				SecretaryCategory.SelectedValue,
				SecretaryShipName.SelectedItem,
				DateBegin.Value,
				DateEnd.Value,
				Recipe.SelectedItem,
				MergeRows.Checked,
				row
				} );

		}

		private void EquipmentCategory_SelectedIndexChanged( object sender, EventArgs e ) {
			string name = EquipmentName.Text;
			int category = (int)EquipmentCategory.SelectedValue;

			if ( category != -1 && name != NameAny && name != NameExist ) {
				var eq = KCDatabase.Instance.MasterEquipments.Values.FirstOrDefault( eqm => eqm.Name == name );
				if ( eq != null && eq.CategoryType != category ) {
					EquipmentName.SelectedIndex = 0;
				}
			}
		}

		private void SecretaryCategory_SelectedIndexChanged( object sender, EventArgs e ) {
			string name = SecretaryShipName.Text;
			int category = (int)SecretaryCategory.SelectedValue;

			if ( category != -1 && name != NameAny ) {
				var ship = KCDatabase.Instance.MasterShips.Values.FirstOrDefault( s => s.Name == name );
				if ( ship != null && ship.ShipType != category ) {
					SecretaryShipName.SelectedIndex = 0;
				}
			}
		}

		private void EquipmentName_TextChanged( object sender, EventArgs e ) {

			var eq = KCDatabase.Instance.MasterEquipments.Values.FirstOrDefault( eqm => eqm.Name == EquipmentName.Text );

			if ( eq != null && (int)EquipmentCategory.SelectedValue != -1 && eq.CategoryType != (int)EquipmentCategory.SelectedValue ) {
				EquipmentCategory.SelectedValue = eq.CategoryType;
			}

		}

		private void SecretaryShipName_TextChanged( object sender, EventArgs e ) {

			var ship = KCDatabase.Instance.MasterShips.Values.FirstOrDefault( sm => sm.Name == SecretaryShipName.Text );

			if ( ship != null && (int)SecretaryCategory.SelectedValue != -1 && ship.ShipType != (int)SecretaryCategory.SelectedValue ) {
				SecretaryCategory.SelectedValue = ship.ShipType;
			}

		}

		private void Searcher_DoWork( object sender, DoWorkEventArgs e ) {

			object[] args = (object[])e.Argument;

			string equipmentName = (string)args[0];
			int equipmentCategory = (int)args[1];
			int secretaryCategory = (int)args[2];
			string secretaryName = (string)args[3];
			DateTime dateBegin = (DateTime)args[4];
			DateTime dateEnd = (DateTime)args[5];
			string recipe = (string)args[6];
			bool mergeRows = (bool)args[7];
			DataGridViewRow origin = (DataGridViewRow)args[8];

			int prioritySecretary =
				secretaryName != NameAny ? 2 :
				secretaryCategory != -1 ? 1 : 0;

			int priorityEquipment =
				equipmentName != NameAny && equipmentName != NameExist ? 2 :
				equipmentCategory != -1 ? 1 : 0;


			var records = RecordManager.Instance.Development.Record;
			var rows = new LinkedList<DataGridViewRow>();


			//

			{
				int i = 0;
				var counts = new Dictionary<string, int>();
				var allcounts = new Dictionary<string, int>();
				var countsdetail = new Dictionary<string, Dictionary<string, int>>();

				foreach ( var r in records ) {

					#region Filtering

					var eq = KCDatabase.Instance.MasterEquipments[r.EquipmentID];
					var secretary = KCDatabase.Instance.MasterShips[r.FlagshipID];
					string currentRecipe = GetRecipeString( r.Fuel, r.Ammo, r.Steel, r.Bauxite );

					if ( r.Date < dateBegin || dateEnd < r.Date )
						continue;

					if ( secretaryCategory != -1 && secretary != null && secretaryCategory != secretary.ShipType )
						continue;

					if ( secretaryName != NameAny && secretaryName != r.FlagshipName )
						continue;



					if ( mergeRows ) {

						string key;

						if ( priorityEquipment > 0 )
							key = currentRecipe;
						else
							key = r.EquipmentName;

						if ( !allcounts.ContainsKey( key ) ) {
							allcounts.Add( key, 1 );

						} else {
							allcounts[key]++;
						}

					}


					if ( equipmentCategory != -1 && ( eq == null || equipmentCategory != eq.CategoryType ) )
						continue;

					switch ( equipmentName ) {
						case NameAny:
							break;
						case NameExist:
							if ( r.EquipmentID == -1 )
								continue;
							break;
						case NameNotExist:
							if ( r.EquipmentID != -1 )
								continue;
							break;
						default:
							if ( r.EquipmentName != equipmentName )
								continue;
							break;
					}



					if ( recipe != NameAny && recipe != currentRecipe )
						continue;


					#endregion


					if ( !mergeRows ) {
						var row = (DataGridViewRow)origin.Clone();

						row.SetValues(
							i + 1,
							r.EquipmentName,
							r.Date,
							GetRecipeString( r.Fuel, r.Ammo, r.Steel, r.Bauxite ),
							secretary.ShipTypeName,
							secretary.NameWithClass
							);

						rows.AddLast( row );

					} else {

						string key;
						if ( priorityEquipment > 0 )
							key = currentRecipe;
						else
							key = r.EquipmentName;

						if ( !counts.ContainsKey( key ) ) {
							counts.Add( key, 1 );

						} else {
							counts[key]++;
						}



						if ( priorityEquipment > 0 )
							key = currentRecipe;
						else
							key = r.EquipmentName;

						string key2;
						if ( prioritySecretary > 0 )
							key2 = currentRecipe;
						else
							key2 = secretary.ShipTypeName;

						if ( !countsdetail.ContainsKey( key ) ) {
							countsdetail.Add( key, new Dictionary<string, int>() );
						}
						if ( !countsdetail[key].ContainsKey( key2 ) ) {
							countsdetail[key].Add( key2, 1 );
						} else {
							countsdetail[key][key2]++;
						}

					}

					if ( Searcher.CancellationPending )
						break;

					i++;
				}


				if ( mergeRows ) {

					int sum = counts.Values.Sum();

					foreach ( var c in counts ) {
						var row = (DataGridViewRow)origin.Clone();

						if ( priorityEquipment > 0 ) {

							row.SetValues(
								c.Value,
								c.Key,
								string.Join( ", ", countsdetail[c.Key].OrderByDescending( p => p.Value ).Select( d => string.Format( "{0}({1})", d.Key, d.Value ) ) ),
								"*",
								"*",
								"*"
								);

							row.Cells[0].Tag = allcounts[c.Key];

						} else {

							row.SetValues(
								c.Value,
								c.Key,
								string.Join( ", ", countsdetail[c.Key].OrderByDescending( p => p.Value ).Select( d => string.Format( "{0}({1})", d.Key, d.Value ) ) ),
								"*",
								"*",
								"*"
								);

							row.Cells[0].Tag = (double)c.Value / sum;
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

				DevelopmentView.Rows.AddRange( rows );
				DevelopmentView.Sort( DevelopmentView.SortedColumn ?? DevelopmentView_Header,
					DevelopmentView.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending );
			}

		}

		private void DevelopmentView_SortCompare( object sender, DataGridViewSortCompareEventArgs e ) {

			if ( e.Column.Index == DevelopmentView_Header.Index ) {
				object tag1 = DevelopmentView[e.Column.Index, e.RowIndex1].Tag;
				object tag2 = DevelopmentView[e.Column.Index, e.RowIndex2].Tag;

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
			}

			if ( !e.Handled ) {
				e.SortResult = ( (IComparable)e.CellValue1 ).CompareTo( e.CellValue2 );
				e.Handled = true;
			}

			if ( e.SortResult == 0 ) {
				e.SortResult = (int)( DevelopmentView.Rows[e.RowIndex1].Tag ?? 0 ) - (int)( DevelopmentView.Rows[e.RowIndex2].Tag ?? 0 );
			}
		}

		private void DevelopmentView_Sorted( object sender, EventArgs e ) {

			for ( int i = 0; i < DevelopmentView.Rows.Count; i++ ) {
				DevelopmentView.Rows[i].Tag = i;
			}
		}

		private void DevelopmentView_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e ) {

			if ( e.ColumnIndex == DevelopmentView_Header.Index ) {
				object tag = DevelopmentView[e.ColumnIndex, e.RowIndex].Tag;

				if ( tag != null ) {
					if ( tag is double ) {
						e.Value = string.Format( "{0} ({1:p1})", e.Value, (double)tag );
						e.FormattingApplied = true;
					} else if ( tag is int ) {
						e.Value = string.Format( "{0}/{1} ({2:p1})", e.Value, (int)tag, (double)(int)e.Value / (int)tag );
						e.FormattingApplied = true;
					}
				}

			} else if ( e.ColumnIndex == DevelopmentView_Date.Index ) {

				if ( e.Value is DateTime ) {
					e.Value = DateTimeHelper.TimeToCSVString( (DateTime)e.Value );
					e.FormattingApplied = true;
				}
			}

		}


	}
}
