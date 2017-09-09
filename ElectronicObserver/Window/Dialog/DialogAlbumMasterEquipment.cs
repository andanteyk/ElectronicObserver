using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
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
	public partial class DialogAlbumMasterEquipment : Form {


		public DialogAlbumMasterEquipment() {
			InitializeComponent();

			TitleFirepower.ImageList =
			TitleTorpedo.ImageList =
			TitleAA.ImageList =
			TitleArmor.ImageList =
			TitleASW.ImageList =
			TitleEvasion.ImageList =
			TitleLOS.ImageList =
			TitleAccuracy.ImageList =
			TitleBomber.ImageList =
			TitleSpeed.ImageList =
			TitleRange.ImageList =
			TitleAircraftCost.ImageList =
			TitleAircraftDistance.ImageList =
			Rarity.ImageList =
			MaterialFuel.ImageList =
			MaterialAmmo.ImageList =
			MaterialSteel.ImageList =
			MaterialBauxite.ImageList =
				ResourceManager.Instance.Icons;

			EquipmentType.ImageList = ResourceManager.Instance.Equipments;

			TitleFirepower.ImageIndex = (int)ResourceManager.IconContent.ParameterFirepower;
			TitleTorpedo.ImageIndex = (int)ResourceManager.IconContent.ParameterTorpedo;
			TitleAA.ImageIndex = (int)ResourceManager.IconContent.ParameterAA;
			TitleArmor.ImageIndex = (int)ResourceManager.IconContent.ParameterArmor;
			TitleASW.ImageIndex = (int)ResourceManager.IconContent.ParameterASW;
			TitleEvasion.ImageIndex = (int)ResourceManager.IconContent.ParameterEvasion;
			TitleLOS.ImageIndex = (int)ResourceManager.IconContent.ParameterLOS;
			TitleAccuracy.ImageIndex = (int)ResourceManager.IconContent.ParameterAccuracy;
			TitleBomber.ImageIndex = (int)ResourceManager.IconContent.ParameterBomber;
			TitleSpeed.ImageIndex = (int)ResourceManager.IconContent.ParameterSpeed;
			TitleRange.ImageIndex = (int)ResourceManager.IconContent.ParameterRange;
			TitleAircraftCost.ImageIndex = (int)ResourceManager.IconContent.ParameterAircraftCost;
			TitleAircraftDistance.ImageIndex = (int)ResourceManager.IconContent.ParameterAircraftDistance;
			MaterialFuel.ImageIndex = (int)ResourceManager.IconContent.ResourceFuel;
			MaterialAmmo.ImageIndex = (int)ResourceManager.IconContent.ResourceAmmo;
			MaterialSteel.ImageIndex = (int)ResourceManager.IconContent.ResourceSteel;
			MaterialBauxite.ImageIndex = (int)ResourceManager.IconContent.ResourceBauxite;


			BasePanelEquipment.Visible = false;


			ControlHelper.SetDoubleBuffered( TableEquipmentName );
			ControlHelper.SetDoubleBuffered( TableParameterMain );
			ControlHelper.SetDoubleBuffered( TableParameterSub );
			ControlHelper.SetDoubleBuffered( TableArsenal );

			ControlHelper.SetDoubleBuffered( EquipmentView );


			//Initialize EquipmentView
			EquipmentView.SuspendLayout();

			EquipmentView_ID.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			EquipmentView_Icon.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			//EquipmentView_Type.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;


			EquipmentView.Rows.Clear();

			List<DataGridViewRow> rows = new List<DataGridViewRow>( KCDatabase.Instance.MasterEquipments.Values.Count( s => s.Name != "なし" ) );

			foreach ( var eq in KCDatabase.Instance.MasterEquipments.Values ) {

				if ( eq.Name == "なし" ) continue;

				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells( EquipmentView );
				row.SetValues( eq.EquipmentID, eq.IconType, eq.CategoryTypeInstance.Name, eq.Name );
				rows.Add( row );

			}
			EquipmentView.Rows.AddRange( rows.ToArray() );

			EquipmentView_ID.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
			EquipmentView_Icon.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
			//EquipmentView_Type.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;

			EquipmentView.Sort( EquipmentView_ID, ListSortDirection.Ascending );
			EquipmentView.ResumeLayout();

		}

		public DialogAlbumMasterEquipment( int equipmentID )
			: this() {

			UpdateAlbumPage( equipmentID );


			if ( KCDatabase.Instance.MasterEquipments.ContainsKey( equipmentID ) ) {
				var row = EquipmentView.Rows.OfType<DataGridViewRow>().First( r => (int)r.Cells[EquipmentView_ID.Index].Value == equipmentID );
				if ( row != null )
					EquipmentView.FirstDisplayedScrollingRowIndex = row.Index;
			}
		}



		private void DialogAlbumMasterEquipment_Load( object sender, EventArgs e ) {

			this.Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormAlbumEquipment] );

		}




		private void EquipmentView_SortCompare( object sender, DataGridViewSortCompareEventArgs e ) {

			if ( e.Column.Name == EquipmentView_Type.Name ) {
				e.SortResult =
					KCDatabase.Instance.MasterEquipments[(int)EquipmentView.Rows[e.RowIndex1].Cells[0].Value].EquipmentType[2] -
					KCDatabase.Instance.MasterEquipments[(int)EquipmentView.Rows[e.RowIndex2].Cells[0].Value].EquipmentType[2];
			} else {
				e.SortResult = ( (IComparable)e.CellValue1 ).CompareTo( e.CellValue2 );
			}

			if ( e.SortResult == 0 ) {
				e.SortResult = (int)( EquipmentView.Rows[e.RowIndex1].Tag ?? 0 ) - (int)( EquipmentView.Rows[e.RowIndex2].Tag ?? 0 );
			}

			e.Handled = true;
		}

		private void EquipmentView_Sorted( object sender, EventArgs e ) {

			for ( int i = 0; i < EquipmentView.Rows.Count; i++ ) {
				EquipmentView.Rows[i].Tag = i;
			}
		}


		private void EquipmentView_CellFormatting( object sender, DataGridViewCellFormattingEventArgs e ) {

			if ( e.ColumnIndex == EquipmentView_Icon.Index ) {
				e.Value = ResourceManager.GetEquipmentImage( (int)e.Value );
				e.FormattingApplied = true;
			}

		}



		private void EquipmentView_CellMouseClick( object sender, DataGridViewCellMouseEventArgs e ) {

			if ( e.RowIndex >= 0 ) {
				int equipmentID = (int)EquipmentView.Rows[e.RowIndex].Cells[0].Value;

				if ( ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 ) {
					Cursor = Cursors.AppStarting;
					new DialogAlbumMasterEquipment( equipmentID ).Show( Owner );
					Cursor = Cursors.Default;

				} else if ( ( e.Button & System.Windows.Forms.MouseButtons.Left ) != 0 ) {
					UpdateAlbumPage( equipmentID );
				}
			}

		}




		private void UpdateAlbumPage( int equipmentID ) {

			KCDatabase db = KCDatabase.Instance;
			EquipmentDataMaster eq = db.MasterEquipments[equipmentID];

			if ( eq == null ) return;


			BasePanelEquipment.SuspendLayout();


			//header
			EquipmentID.Tag = equipmentID;
			EquipmentID.Text = eq.EquipmentID.ToString();
			ToolTipInfo.SetToolTip( EquipmentID, string.Format( "Type: [ {0} ]", string.Join( ", ", eq.EquipmentType ) ) );
			AlbumNo.Text = eq.AlbumNo.ToString();


			TableEquipmentName.SuspendLayout();

			EquipmentType.Text = eq.CategoryTypeInstance.Name;

			{
				int eqicon = eq.IconType;
				if ( eqicon >= (int)ResourceManager.EquipmentContent.Locked )
					eqicon = (int)ResourceManager.EquipmentContent.Unknown;
				EquipmentType.ImageIndex = eqicon;

				StringBuilder sb = new StringBuilder();
				sb.AppendLine( "装備可能艦種:" );
				foreach ( var stype in KCDatabase.Instance.ShipTypes.Values ) {
					if ( stype.EquipmentType.Contains( eq.CategoryType ) )
						sb.AppendLine( stype.Name );
				}
				ToolTipInfo.SetToolTip( EquipmentType, sb.ToString() );
			}
			EquipmentName.Text = eq.Name;
			ToolTipInfo.SetToolTip( EquipmentName, "(右クリックでコピー)" );

			TableEquipmentName.ResumeLayout();


			//main parameter
			TableParameterMain.SuspendLayout();

			SetParameterText( Firepower, eq.Firepower );
			SetParameterText( Torpedo, eq.Torpedo );
			SetParameterText( AA, eq.AA );
			SetParameterText( Armor, eq.Armor );
			SetParameterText( ASW, eq.ASW );
			SetParameterText( Evasion, eq.Evasion );
			SetParameterText( LOS, eq.LOS );
			SetParameterText( Accuracy, eq.Accuracy );
			SetParameterText( Bomber, eq.Bomber );

			if ( eq.CategoryType == 48 ) {
				TitleAccuracy.Text = "対爆";
				TitleEvasion.Text = "迎撃";
			} else {
				TitleAccuracy.Text = "命中";
				TitleEvasion.Text = "回避";
			}

			TableParameterMain.ResumeLayout();


			//sub parameter
			TableParameterSub.SuspendLayout();

			Speed.Text = "なし"; //Constants.GetSpeed( eq.Speed );
			Range.Text = Constants.GetRange( eq.Range );
			Rarity.Text = Constants.GetEquipmentRarity( eq.Rarity );
			Rarity.ImageIndex = (int)ResourceManager.IconContent.RarityRed + Constants.GetEquipmentRarityID( eq.Rarity );		//checkme

			TableParameterSub.ResumeLayout();


			// aircraft
			if ( Calculator.IsAircraft( equipmentID, true ) ) {
				TableAircraft.SuspendLayout();
				AircraftCost.Text = eq.AircraftCost.ToString();
				ToolTipInfo.SetToolTip( AircraftCost, "配備時のボーキ消費：" + ( ( Calculator.IsAircraft( equipmentID, false ) ? 18 : 4 ) * eq.AircraftCost ) );
				AircraftDistance.Text = eq.AircraftDistance.ToString();
				TableAircraft.ResumeLayout();
				TableAircraft.Visible = true;
			} else {
				TableAircraft.Visible = false;
			}


			//default equipment
			DefaultSlots.BeginUpdate();
			DefaultSlots.Items.Clear();
			foreach ( var ship in KCDatabase.Instance.MasterShips.Values ) {
				if ( ship.DefaultSlot != null && ship.DefaultSlot.Contains( equipmentID ) ) {
					DefaultSlots.Items.Add( ship );
				}
			}
			DefaultSlots.EndUpdate();


			Description.Text = eq.Message;


			//arsenal
			TableArsenal.SuspendLayout();

			MaterialFuel.Text = eq.Material[0].ToString();
			MaterialAmmo.Text = eq.Material[1].ToString();
			MaterialSteel.Text = eq.Material[2].ToString();
			MaterialBauxite.Text = eq.Material[3].ToString();

			TableArsenal.ResumeLayout();



			//装備画像を読み込んでみる
			{
				string path = string.Format( @"{0}\\resources\\image\\slotitem\\card\\{1:D3}.png", Utility.Configuration.Config.Connection.SaveDataPath, equipmentID );
				if ( File.Exists( path ) ) {
					try {

						EquipmentImage.Image = new Bitmap( path );

					} catch ( Exception ) {
						if ( EquipmentImage.Image != null )
							EquipmentImage.Image.Dispose();
						EquipmentImage.Image = null;
					}
				} else {
					if ( EquipmentImage.Image != null )
						EquipmentImage.Image.Dispose();
					EquipmentImage.Image = null;
				}
			}


			BasePanelEquipment.ResumeLayout();
			BasePanelEquipment.Visible = true;


			this.Text = "装備図鑑 - " + eq.Name;

		}


		private void SetParameterText( ImageLabel label, int value ) {

			if ( value > 0 ) {
				label.ForeColor = SystemColors.ControlText;
				label.Text = "+" + value.ToString();
			} else if ( value == 0 ) {
				label.ForeColor = Color.Silver;
				label.Text = "0";
			} else {
				label.ForeColor = Color.Red;
				label.Text = value.ToString();
			}

		}


		private void DefaultSlots_MouseDown( object sender, MouseEventArgs e ) {

			if ( e.Button == System.Windows.Forms.MouseButtons.Right ) {
				int index = DefaultSlots.IndexFromPoint( e.Location );
				if ( index >= 0 ) {
					Cursor = Cursors.AppStarting;
					new DialogAlbumMasterShip( ( (ShipDataMaster)DefaultSlots.Items[index] ).ShipID ).Show( Owner );
					Cursor = Cursors.Default;
				}
			}
		}



		private void TableParameterMain_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
			/*/
			if ( e.Column == 0 )
				e.Graphics.DrawLine( Pens.Silver, e.CellBounds.Right - 1, e.CellBounds.Y, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
			//*/
		}

		private void TableParameterSub_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}



		private void TableArsenal_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}

		private void TableAircraft_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}



		private void StripMenu_File_OutputCSVUser_Click( object sender, EventArgs e ) {

			if ( SaveCSVDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

				try {

					using ( StreamWriter sw = new StreamWriter( SaveCSVDialog.FileName, false, Utility.Configuration.Config.Log.FileEncoding ) ) {

						sw.WriteLine( "装備ID,図鑑番号,装備種,装備名,大分類,図鑑カテゴリID,カテゴリID,アイコンID,航空機グラフィックID,火力,雷装,対空,装甲,対潜,回避,索敵,運,命中,爆装,射程,レア,廃棄燃料,廃棄弾薬,廃棄鋼材,廃棄ボーキ,図鑑文章,戦闘行動半径,配置コスト" );

						foreach ( EquipmentDataMaster eq in KCDatabase.Instance.MasterEquipments.Values ) {

							sw.WriteLine( string.Join( ",",
								eq.EquipmentID,
								eq.AlbumNo,
								KCDatabase.Instance.EquipmentTypes[eq.EquipmentType[2]].Name,
								eq.Name,
								eq.EquipmentType[0],
								eq.EquipmentType[1],
								eq.EquipmentType[2],
								eq.EquipmentType[3],
								eq.EquipmentType[4],
								eq.Firepower,
								eq.Torpedo,
								eq.AA,
								eq.Armor,
								eq.ASW,
								eq.Evasion,
								eq.LOS,
								eq.Luck,
								eq.Accuracy,
								eq.Bomber,
								Constants.GetRange( eq.Range ),
								Constants.GetEquipmentRarity( eq.Rarity ),
								eq.Material[0],
								eq.Material[1],
								eq.Material[2],
								eq.Material[3],
								eq.Message.Replace( "\r\n", "<br>" ),
								eq.AircraftDistance,
								eq.AircraftCost
								) );

						}

					}

				} catch ( Exception ex ) {

					Utility.ErrorReporter.SendErrorReport( ex, "装備図鑑 CSVの出力に失敗しました。" );
					MessageBox.Show( "装備図鑑 CSVの出力に失敗しました。\r\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				}

			}


		}


		private void StripMenu_File_OutputCSVData_Click( object sender, EventArgs e ) {

			if ( SaveCSVDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

				try {

					using ( StreamWriter sw = new StreamWriter( SaveCSVDialog.FileName, false, Utility.Configuration.Config.Log.FileEncoding ) ) {

						sw.WriteLine( "装備ID,図鑑番号,装備名,装備種1,装備種2,装備種3,装備種4,装備種5,火力,雷装,対空,装甲,対潜,回避,索敵,運,命中,爆装,射程,レア,廃棄燃料,廃棄弾薬,廃棄鋼材,廃棄ボーキ,図鑑文章,戦闘行動半径,配置コスト" );

						foreach ( EquipmentDataMaster eq in KCDatabase.Instance.MasterEquipments.Values ) {

							sw.WriteLine( string.Join( ",",
								eq.EquipmentID,
								eq.AlbumNo,
								eq.Name,
								eq.EquipmentType[0],
								eq.EquipmentType[1],
								eq.EquipmentType[2],
								eq.EquipmentType[3],
								eq.EquipmentType[4],
								eq.Firepower,
								eq.Torpedo,
								eq.AA,
								eq.Armor,
								eq.ASW,
								eq.Evasion,
								eq.LOS,
								eq.Luck,
								eq.Accuracy,
								eq.Bomber,
								eq.Range,
								eq.Rarity,
								eq.Material[0],
								eq.Material[1],
								eq.Material[2],
								eq.Material[3],
								eq.Message.Replace( "\r\n", "<br>" ),
								eq.AircraftDistance,
								eq.AircraftCost
								) );

						}

					}

				} catch ( Exception ex ) {

					Utility.ErrorReporter.SendErrorReport( ex, "装備図鑑 CSVの出力に失敗しました。" );
					MessageBox.Show( "装備図鑑 CSVの出力に失敗しました。\r\n" + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				}

			}

		}


		private void TextSearch_TextChanged( object sender, EventArgs e ) {
			if ( string.IsNullOrWhiteSpace( TextSearch.Text ) )
				return;

			string searchWord = DialogAlbumMasterShip.ToHiragana( TextSearch.Text.ToLower() );
			var target =
				EquipmentView.Rows.OfType<DataGridViewRow>()
				.Select( r => KCDatabase.Instance.MasterEquipments[(int)r.Cells[EquipmentView_ID.Index].Value] )
				.FirstOrDefault(
					eq => DialogAlbumMasterShip.ToHiragana( eq.Name.ToLower() ).Contains( searchWord ) );

			if ( target != null ) {
				EquipmentView.FirstDisplayedScrollingRowIndex = EquipmentView.Rows.OfType<DataGridViewRow>().First( r => (int)r.Cells[EquipmentView_ID.Index].Value == target.EquipmentID ).Index;
			}
		}

		private void TextSearch_KeyDown( object sender, KeyEventArgs e ) {
			if ( e.KeyCode == Keys.Enter ) {
				TextSearch_TextChanged( sender, e );
				e.SuppressKeyPress = true;
				e.Handled = true;
			}
		}




		private void DialogAlbumMasterEquipment_FormClosed( object sender, FormClosedEventArgs e ) {

			ResourceManager.DestroyIcon( Icon );

		}

		private void StripMenu_Edit_CopyEquipmentName_Click( object sender, EventArgs e ) {
			var eq = KCDatabase.Instance.MasterEquipments[EquipmentID.Tag as int? ?? -1];
			if ( eq != null )
				Clipboard.SetText( eq.Name );
			else
				System.Media.SystemSounds.Exclamation.Play();
		}

		private void EquipmentName_MouseClick( object sender, MouseEventArgs e ) {
			if ( e.Button == System.Windows.Forms.MouseButtons.Right ) {
				var eq = KCDatabase.Instance.MasterEquipments[EquipmentID.Tag as int? ?? -1];
				if ( eq != null )
					Clipboard.SetText( eq.Name );
				else
					System.Media.SystemSounds.Exclamation.Play();
			}
		}

		private void StripMenu_Edit_CopyEquipmentData_Click( object sender, EventArgs e ) {
			var eq = KCDatabase.Instance.MasterEquipments[EquipmentID.Tag as int? ?? -1];
			if ( eq == null ) {
				System.Media.SystemSounds.Exclamation.Play();
				return;
			}

			var sb = new StringBuilder();

			sb.AppendFormat( "{0} {1}\r\n", eq.CategoryTypeInstance.Name, eq.Name );
			sb.AppendFormat( "ID: {0} / 図鑑番号: {1} / カテゴリID: [{2}]\r\n", eq.EquipmentID, eq.AlbumNo, string.Join( ", ", eq.EquipmentType ) );

			sb.AppendLine();

			if ( eq.Firepower != 0 )
				sb.AppendFormat( "火力: {0:+0;-0;0}\r\n", eq.Firepower );
			if ( eq.Torpedo != 0 )
				sb.AppendFormat( "雷装: {0:+0;-0;0}\r\n", eq.Torpedo );
			if ( eq.AA != 0 )
				sb.AppendFormat( "対空: {0:+0;-0;0}\r\n", eq.AA );
			if ( eq.Armor != 0 )
				sb.AppendFormat( "装甲: {0:+0;-0;0}\r\n", eq.Armor );
			if ( eq.ASW != 0 )
				sb.AppendFormat( "対潜: {0:+0;-0;0}\r\n", eq.ASW );
			if ( eq.Evasion != 0 )
				sb.AppendFormat( "{0}: {1:+0;-0;0}\r\n", eq.CategoryType == 48 ? "迎撃" : "回避", eq.Evasion );
			if ( eq.LOS != 0 )
				sb.AppendFormat( "索敵: {0:+0;-0;0}\r\n", eq.LOS );
			if ( eq.Accuracy != 0 )
				sb.AppendFormat( "{0}: {1:+0;-0;0}\r\n", eq.CategoryType == 48 ? "対爆" : "命中", eq.Accuracy );
			if ( eq.Bomber != 0 )
				sb.AppendFormat( "爆装: {0:+0;-0;0}\r\n", eq.Bomber );
			if ( eq.Luck != 0 )
				sb.AppendFormat( "運: {0:+0;-0;0}\r\n", eq.Luck );

			if ( eq.Range > 0 )
				sb.Append( "射程: " ).AppendLine( Constants.GetRange( eq.Range ) );

			if ( eq.AircraftCost > 0 )
				sb.AppendFormat( "配備コスト: {0}\r\n", eq.AircraftCost );
			if ( eq.AircraftDistance > 0 )
				sb.AppendFormat( "戦闘行動半径: {0}\r\n", eq.AircraftDistance );

			sb.AppendLine();

			sb.AppendFormat( "レアリティ: {0}\r\n", Constants.GetEquipmentRarity( eq.Rarity ) );
			sb.AppendFormat( "廃棄資材: {0}\r\n", string.Join( " / ", eq.Material ) );

			sb.AppendLine();

			sb.AppendFormat( "図鑑説明: \r\n{0}\r\n",
				!string.IsNullOrWhiteSpace( eq.Message ) ? eq.Message : "(不明)" );

			sb.AppendLine();

			sb.AppendLine( "初期装備/開発:" );
			string result = GetAppearingArea( eq.EquipmentID );
			if ( string.IsNullOrWhiteSpace( result ) )
				result = "(不明)\r\n";
			sb.AppendLine( result );


			Clipboard.SetText( sb.ToString() );
		}


		private string GetAppearingArea( int equipmentID ) {
			var sb = new StringBuilder();

			foreach ( var ship in KCDatabase.Instance.MasterShips.Values
				.Where( s => s.DefaultSlot != null && s.DefaultSlot.Contains( equipmentID ) ) ) {
				sb.AppendLine( ship.NameWithClass );
			}

			foreach ( var record in RecordManager.Instance.Development.Record
				.Where( r => r.EquipmentID == equipmentID )
				.Select( r => new {
					r.Fuel, r.Ammo, r.Steel, r.Bauxite
				} )
				.Distinct()
				.OrderBy( r => r.Fuel )
				.ThenBy( r => r.Ammo )
				.ThenBy( r => r.Steel )
				.ThenBy( r => r.Bauxite )
				) {
				sb.AppendFormat( "開発 {0} / {1} / {2} / {3}\r\n",
					record.Fuel, record.Ammo, record.Steel, record.Bauxite );
			}

			return sb.ToString();
		}

		private void StripMenu_View_ShowAppearingArea_Click( object sender, EventArgs e ) {

			int eqID = EquipmentID.Tag as int? ?? -1;
			var eq = KCDatabase.Instance.MasterEquipments[eqID];

			if ( eq == null ) {
				System.Media.SystemSounds.Exclamation.Play();
				return;
			}

			string result = GetAppearingArea( eqID );

			if ( string.IsNullOrWhiteSpace( result ) ) {
				result = eq.Name + " の初期装備艦・開発レシピは不明です。";
			}

			MessageBox.Show( result, "入手手段表示", MessageBoxButtons.OK, MessageBoxIcon.Information );
		}


		private void StripMenu_Edit_GoogleEquipmentName_Click( object sender, EventArgs e ) {
			var eq = KCDatabase.Instance.MasterEquipments[EquipmentID.Tag as int? ?? -1];
			if ( eq == null ) {
				System.Media.SystemSounds.Exclamation.Play();
				return;
			}

			try {

				// google <装備名> 艦これ
				System.Diagnostics.Process.Start( @"https://www.google.co.jp/search?q=" + Uri.EscapeDataString( eq.Name ) + "+%E8%89%A6%E3%81%93%E3%82%8C" );

			} catch ( Exception ex ) {
				Utility.ErrorReporter.SendErrorReport( ex, "艦船名の Google 検索に失敗しました。" );
			}
		}


	}
}
