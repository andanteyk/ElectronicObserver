using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
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

namespace ElectronicObserver.Window.Dialog {
	public partial class DialogBaseAirCorpsSimulation : Form {


		private class SquadronUI {

			public readonly int BaseAirCorpsID;
			public readonly int SquadronID;

			public ComboBox AircraftCategory;
			public ComboBox Aircraft;

			public NumericUpDown AircraftCount;

			public Label AirSuperioritySortie;
			public Label AirSuperiorityAirDefense;
			public Label Distance;
			public Label Bomber;
			public Label Torpedo;
			public Label OrganizationCost;

			public event EventHandler Updated = delegate { };


			public SquadronUI( int baseAirCorpsID, int squadronID ) {

				BaseAirCorpsID = baseAirCorpsID;
				SquadronID = squadronID;

				AircraftCategory = new ComboBox();
				AircraftCategory.Size = new Size( 160, AircraftCategory.Height );
				AircraftCategory.Anchor = AnchorStyles.None;
				AircraftCategory.Margin = new Padding( 2, 0, 2, 0 );
				AircraftCategory.DropDownStyle = ComboBoxStyle.DropDownList;
				AircraftCategory.Items.AddRange( ComboBoxCategory.GetAllCategories().ToArray() );
				AircraftCategory.SelectedValueChanged += AircraftCategory_SelectedValueChanged;

				Aircraft = new ComboBox();
				Aircraft.Size = new Size( 240, Aircraft.Height );
				Aircraft.Anchor = AnchorStyles.None;
				Aircraft.Margin = new Padding( 2, 0, 2, 0 );
				Aircraft.DropDownStyle = ComboBoxStyle.DropDownList;
				Aircraft.SelectedValueChanged += Aircraft_SelectedValueChanged;

				AircraftCount = new NumericUpDown();
				AircraftCount.Size = new Size( 60, AircraftCount.Height );
				AircraftCount.Anchor = AnchorStyles.None;
				AircraftCount.Maximum = AircraftCount.Minimum = 0;
				AircraftCount.TextAlign = HorizontalAlignment.Right;
				AircraftCount.Margin = new Padding( 2, 0, 2, 0 );
				AircraftCount.ValueChanged += AircraftCount_ValueChanged;

				AirSuperioritySortie = NewLabel();
				AirSuperiorityAirDefense = NewLabel();
				Distance = NewLabel();
				Bomber = NewLabel();
				Torpedo = NewLabel();
				OrganizationCost = NewLabel();

				Update();
			}


			private Label NewLabel() {
				var label = new Label();
				label.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
				label.Padding = new Padding( 0, 1, 0, 1 );
				label.Margin = new Padding( 2, 1, 2, 1 );
				label.TextAlign = ContentAlignment.MiddleRight;

				return label;
			}

			public void AddToTable( TableLayoutPanel table, int row ) {
				table.Controls.Add( AircraftCategory, 0, row );
				table.Controls.Add( Aircraft, 1, row );
				table.Controls.Add( AircraftCount, 2, row );
				table.Controls.Add( AirSuperioritySortie, 3, row );
				table.Controls.Add( AirSuperiorityAirDefense, 4, row );
				table.Controls.Add( Distance, 5, row );
				table.Controls.Add( Bomber, 6, row );
				table.Controls.Add( Torpedo, 7, row );
				table.Controls.Add( OrganizationCost, 8, row );
			}

			void AircraftCategory_SelectedValueChanged( object sender, EventArgs e ) {

				// 指定されたカテゴリにおいて、利用可能な装備を列挙する

				var category = AircraftCategory.SelectedItem as ComboBoxCategory;

				IEnumerable<ComboBoxEquipment> list = new[] { new ComboBoxEquipment() };

				if ( category != null ) {
					list = list.Concat( KCDatabase.Instance.Equipments.Values
						.Where( eq => eq != null && eq.MasterEquipment.CategoryType == category.EquipmentType.TypeID )
						.OrderBy( eq => eq.EquipmentID )
						.ThenBy( eq => eq.Level )
						.ThenBy( eq => eq.AircraftLevel )
						.Select( eq => new ComboBoxEquipment( eq ) ) );
				}

				Aircraft.Items.Clear();
				Aircraft.Items.AddRange( list.ToArray() );
				Aircraft.SelectedIndex = 0;
			}

			void Aircraft_SelectedValueChanged( object sender, EventArgs e ) {

				var equipment = Aircraft.SelectedItem as ComboBoxEquipment;

				if ( equipment == null || equipment.EquipmentID == -1 ) {
					AircraftCount.Maximum = 0;

				} else {
					int aircraftCount = Calculator.IsAircraft( equipment.EquipmentID, false ) ? 18 : 4;
					AircraftCount.Value = AircraftCount.Maximum = aircraftCount;

				}

				Update();

			}

			void AircraftCount_ValueChanged( object sender, EventArgs e ) {
				Update();
			}

			private void Update() {
				var equipment = Aircraft.SelectedItem as ComboBoxEquipment;

				if ( equipment == null || equipment.EquipmentID == -1 ) {
					AirSuperioritySortie.Text = "0";
					AirSuperioritySortie.Tag = 0;
					AirSuperiorityAirDefense.Text = "0";
					AirSuperiorityAirDefense.Tag = 0;
					Distance.Text = "0";
					Bomber.Text = "0";
					Torpedo.Text = "0";
					OrganizationCost.Text = "0";
					OrganizationCost.Tag = 0;

				} else {

					var eq = equipment.EquipmentInstance;

					int aircraftCount = (int)AircraftCount.Value;

					int airSuperioritySortie = Calculator.GetAirSuperiority( equipment.EquipmentID, aircraftCount, equipment.AircraftLevel, equipment.Level, false );
					AirSuperioritySortie.Text = airSuperioritySortie.ToString();
					AirSuperioritySortie.Tag = airSuperioritySortie;

					int airSuperiorityAirDefense = Calculator.GetAirSuperiority( equipment.EquipmentID, aircraftCount, equipment.AircraftLevel, equipment.Level, true );
					AirSuperiorityAirDefense.Text = airSuperiorityAirDefense.ToString();
					AirSuperiorityAirDefense.Tag = airSuperiorityAirDefense;

					Distance.Text = eq.AircraftDistance.ToString();

					Torpedo.Text = eq.Torpedo.ToString();
					Bomber.Text = eq.Bomber.ToString();

					int organizationCost = aircraftCount * eq.AircraftCost;
					OrganizationCost.Text = organizationCost.ToString();
					OrganizationCost.Tag = organizationCost;

				}

				Updated( this, new EventArgs() );

			}

		}


		private class BaseAirCorpsUI {

			public readonly int BaseAirCorpsID;

			public Label TitleAircraftCategory;
			public Label TitleAircraft;
			public Label TitleAircraftCount;
			public Label TitleAirSuperioritySortie;
			public Label TitleAirSuperiorityAirDefense;
			public Label TitleDistance;
			public Label TitleBomber;
			public Label TitleTorpedo;
			public Label TitleOrganizationCost;

			public SquadronUI[] Squadrons;

			public Label TitleTotal;
			public Label DuplicateCheck;
			public Label TotalAirSuperioritySortie;
			public Label TotalAirSuperiorityAirDefense;
			public Label TotalDistance;
			public Label TotalOrganizationCost;

			public ToolTip ToolTipInternal;

			public event EventHandler Updated = delegate { };


			public BaseAirCorpsUI( int baseAirCorpsID, ToolTip tooltip ) {

				BaseAirCorpsID = baseAirCorpsID;

				TitleAircraftCategory = NewTitleLabel();
				TitleAircraft = NewTitleLabel();
				TitleAircraftCount = NewTitleLabel();
				TitleAirSuperioritySortie = NewTitleLabel();
				TitleAirSuperiorityAirDefense = NewTitleLabel();
				TitleDistance = NewTitleLabel();
				TitleBomber = NewTitleLabel();
				TitleTorpedo = NewTitleLabel();
				TitleOrganizationCost = NewTitleLabel();

				TitleAircraftCategory.Text = "カテゴリ";
				TitleAircraft.Text = "配備機";
				TitleAircraftCount.Text = "機数";
				TitleAirSuperioritySortie.Text = "出撃制空";
				TitleAirSuperiorityAirDefense.Text = "防空制空";
				TitleDistance.Text = "半径";
				TitleBomber.Text = "爆装";
				TitleTorpedo.Text = "雷装";
				TitleOrganizationCost.Text = "配備コスト";

				Squadrons = new SquadronUI[4];
				for ( int i = 0; i < Squadrons.Length; i++ ) {
					Squadrons[i] = new SquadronUI( baseAirCorpsID, i + 1 );
					Squadrons[i].Updated += BaseAirCorpsUI_Updated;
				}

				TitleTotal = NewTitleLabel();
				DuplicateCheck = NewTitleLabel();
				TotalAirSuperioritySortie = NewTotalLabel();
				TotalAirSuperiorityAirDefense = NewTotalLabel();
				TotalDistance = NewTotalLabel();
				TotalOrganizationCost = NewTotalLabel();

				TitleTotal.Text = "合計";
				DuplicateCheck.TextAlign = ContentAlignment.MiddleLeft;
				DuplicateCheck.ForeColor = Color.Red;

				ToolTipInternal = tooltip;

				BaseAirCorpsUI_Updated( null, new EventArgs() );
			}

			private Label NewTitleLabel() {
				var label = new Label();
				label.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
				label.Padding = new Padding( 0, 1, 0, 1 );
				label.Margin = new Padding( 2, 1, 2, 1 );
				label.TextAlign = ContentAlignment.MiddleCenter;

				return label;
			}

			private Label NewTotalLabel() {
				var label = new Label();
				label.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
				label.Padding = new Padding( 0, 1, 0, 1 );
				label.Margin = new Padding( 2, 1, 2, 1 );
				label.TextAlign = ContentAlignment.MiddleRight;

				return label;
			}

			public void AddToTable( TableLayoutPanel table ) {

				table.Controls.Add( TitleAircraftCategory, 0, 0 );
				table.Controls.Add( TitleAircraft, 1, 0 );
				table.Controls.Add( TitleAircraftCount, 2, 0 );
				table.Controls.Add( TitleAirSuperioritySortie, 3, 0 );
				table.Controls.Add( TitleAirSuperiorityAirDefense, 4, 0 );
				table.Controls.Add( TitleDistance, 5, 0 );
				table.Controls.Add( TitleBomber, 6, 0 );
				table.Controls.Add( TitleTorpedo, 7, 0 );
				table.Controls.Add( TitleOrganizationCost, 8, 0 );

				for ( int i  = 0; i < Squadrons.Length; i++ ) {
					Squadrons[i].AddToTable( table, i + 1 );
				}

				table.Controls.Add( TitleTotal, 0, Squadrons.Length + 1 );
				table.Controls.Add( DuplicateCheck, 1, Squadrons.Length + 1 );
				table.Controls.Add( TotalAirSuperioritySortie, 3, Squadrons.Length + 1 );
				table.Controls.Add( TotalAirSuperiorityAirDefense, 4, Squadrons.Length + 1 );
				table.Controls.Add( TotalDistance, 5, Squadrons.Length + 1 );
				table.Controls.Add( TotalOrganizationCost, 8, Squadrons.Length + 1 );

			}


			void BaseAirCorpsUI_Updated( object sender, EventArgs e ) {

				var squadrons = Squadrons.Select( sq => sq.Aircraft.SelectedItem as ComboBoxEquipment )
					.Where( eq => eq != null && eq.EquipmentInstance != null );


				int airSortie = Squadrons.Select( sq => sq.AirSuperioritySortie.Tag as int? ?? 0 ).Sum();

				TotalAirSuperioritySortie.Text = airSortie.ToString();
				ToolTipInternal.SetToolTip( TotalAirSuperioritySortie,
					string.Format( "確保: {0}\r\n優勢: {1}\r\n均衡: {2}\r\n劣勢: {3}\r\n",
						(int)( airSortie / 3.0 ),
						(int)( airSortie / 1.5 ),
						Math.Max( (int)( airSortie * 1.5 - 1 ), 0 ),
						Math.Max( (int)( airSortie * 3.0 - 1 ), 0 ) ) );


				int airDefense = Squadrons.Select( sq => sq.AirSuperiorityAirDefense.Tag as int? ?? 0 ).Sum();

				// 偵察機補正計算
				double reconRate = squadrons.Select( eq => {
					int losrate = Math.Min( Math.Max( eq.EquipmentInstance.LOS - 7, 0 ), 2 );
					switch ( eq.EquipmentInstance.CategoryType ) {
						case 10:	// 水上偵察機
						case 41:	// 大型飛行艇
							return 1.1 + losrate * 0.03;
						case 9:		// 艦上偵察機
						case 59:	// 噴式偵察機
							return 1.2 + losrate * 0.05;
						default:
							return 1;
					}
				} ).DefaultIfEmpty().Max();

				airDefense = (int)( airDefense * reconRate );

				TotalAirSuperiorityAirDefense.Text = airDefense.ToString();
				ToolTipInternal.SetToolTip( TotalAirSuperiorityAirDefense,
					string.Format( "確保: {0}\r\n優勢: {1}\r\n均衡: {2}\r\n劣勢: {3}\r\n",
						(int)( airDefense / 3.0 ),
						(int)( airDefense / 1.5 ),
						Math.Max( (int)( airDefense * 1.5 - 1 ), 0 ),
						Math.Max( (int)( airDefense * 3.0 - 1 ), 0 ) ) );


				// distance
				{
					int minDistance = squadrons
						.Select( eq => eq.EquipmentInstance.AircraftDistance )
						.DefaultIfEmpty()
						.Min();

					int maxReconDistance = 
						squadrons.Where( sq => {
							switch ( sq.EquipmentInstance.CategoryType ) {
								case 9:		// 艦上偵察機
								case 10:	// 水上偵察機
								case 41:	// 大型飛行艇
								case 59:	// 噴式偵察機
									return true;
								default:
									return false;
							}
						} )
						.Select( sq => sq.EquipmentInstance.AircraftDistance )
						.DefaultIfEmpty()
						.Max();

					int distance = minDistance;
					if ( maxReconDistance > minDistance )
						distance += Math.Min( (int)Math.Round( Math.Sqrt( maxReconDistance - minDistance ) ), 3 );

					TotalDistance.Text = distance.ToString();
				}

				TotalOrganizationCost.Text = Squadrons.Select( sq => sq.OrganizationCost.Tag as int? ?? 0 ).Sum().ToString();


				Updated( this, new EventArgs() );
			}
		}


		private class ComboBoxCategory {

			public readonly int ID;
			public readonly EquipmentType EquipmentType;

			public ComboBoxCategory( int id ) {
				ID = id;
				EquipmentType = KCDatabase.Instance.EquipmentTypes[id];
			}

			public override string ToString() {
				if ( EquipmentType == null )
					return "(不明)";
				else
					return EquipmentType.Name;
			}


			public static implicit operator int( ComboBoxCategory from ) {
				return from.ID;
			}

			public static implicit operator EquipmentType( ComboBoxCategory from ) {
				return from.EquipmentType;
			}


			public static IEnumerable<ComboBoxCategory> GetAllCategories() {
				foreach ( var category in KCDatabase.Instance.EquipmentTypes.Values ) {

					// オートジャイロ / 対潜哨戒機 は除外
					if ( category.TypeID == 25 || category.TypeID == 26 )
						continue;

					var first = KCDatabase.Instance.MasterEquipments.Values
						.Where( eq => !eq.IsAbyssalEquipment )
						.FirstOrDefault( eq => eq.CategoryType == category.TypeID );

					if ( first != null && Calculator.IsAircraft( first.EquipmentID, true ) )
						yield return new ComboBoxCategory( first.CategoryType );
				}
			}
		}

		private class ComboBoxEquipment {

			public readonly int EquipmentID;
			public readonly int Level;
			public readonly int AircraftLevel;
			public readonly EquipmentDataMaster EquipmentInstance;
			public readonly int UniqueID;

			public ComboBoxEquipment()
				: this( -1, 0, 0 ) { }

			public ComboBoxEquipment( int equipmentID, int level, int aircraftLevel ) {
				EquipmentID = equipmentID;
				Level = level;
				AircraftLevel = aircraftLevel;
				EquipmentInstance = KCDatabase.Instance.MasterEquipments[equipmentID];
				UniqueID = -1;
			}

			public ComboBoxEquipment( EquipmentData equipment ) {
				if ( equipment == null ) {
					EquipmentID = -1;
					Level = 0;
					AircraftLevel = 0;
					EquipmentInstance = null;
					UniqueID = -1;

				} else {
					EquipmentID = equipment.EquipmentID;
					Level = equipment.Level;
					AircraftLevel = equipment.AircraftLevel;
					EquipmentInstance = KCDatabase.Instance.MasterEquipments[equipment.EquipmentID];
					UniqueID = equipment.MasterID;
				}
			}

			private static readonly string[] AircraftLevelString = { 
				"",
				" |",
				" ||",
				" |||",
				" /",
				" //",
				" ///",
				" >>",
			};
			public override string ToString() {
				if ( EquipmentInstance != null ) {

					var sb = new StringBuilder( EquipmentInstance.Name );

					if ( Level > 0 )
						sb.Append( "+" ).Append( Level );
					if ( AircraftLevel > 0 )
						sb.Append( AircraftLevelString[AircraftLevel] );

					return sb.ToString();

				} else return "(なし)";
			}
		}




		private BaseAirCorpsUI[] BaseAirCorpsUIList;
		private TableLayoutPanel[] TableBaseAirCorpsList;

		public DialogBaseAirCorpsSimulation() {
			InitializeComponent();

			TableBaseAirCorpsList = new[] { 
				TableBaseAirCorps1,
				TableBaseAirCorps2,
				TableBaseAirCorps3,
			};


			BaseAirCorpsUIList = new BaseAirCorpsUI[TableBaseAirCorpsList.Length];
			for ( int i = 0; i < BaseAirCorpsUIList.Length; i++ ) {
				BaseAirCorpsUIList[i] = new BaseAirCorpsUI( i + 1, ToolTipInfo );

				TableBaseAirCorpsList[i].SuspendLayout();

				BaseAirCorpsUIList[i].AddToTable( TableBaseAirCorpsList[i] );
				BaseAirCorpsUIList[i].Updated += BaseAirCorpsUIList_Updated;

				TableBaseAirCorpsList[i].CellPaint += TableBaseAirCorps_CellPaint;
				ControlHelper.SetTableRowStyles( TableBaseAirCorpsList[i], new RowStyle( SizeType.Absolute, 32 ) );
				ControlHelper.SetTableColumnStyles( TableBaseAirCorpsList[i], new ColumnStyle( SizeType.Absolute, 72 ) );

				ControlHelper.SetTableColumnStyle( TableBaseAirCorpsList[i], 0, new ColumnStyle( SizeType.Absolute, 164 ) );
				ControlHelper.SetTableColumnStyle( TableBaseAirCorpsList[i], 1, new ColumnStyle( SizeType.Absolute, 244 ) );

				ControlHelper.SetDoubleBuffered( TableBaseAirCorpsList[i] );

				TableBaseAirCorpsList[i].ResumeLayout();
			}

		}

		private void DialogBaseAirCorpsSimulation_Load( object sender, EventArgs e ) {

			if ( !KCDatabase.Instance.BaseAirCorps.Any() ) {
				MessageBox.Show( "基地航空隊のデータがありません。\r\n一度出撃画面に移動してください。", "基地航空隊データ未受信",
					MessageBoxButtons.OK, MessageBoxIcon.Error );
				Close();
			}


			// 基地航空隊からのインポート; メニュー設定
			{
				var maps = KCDatabase.Instance.BaseAirCorps.Values
					.Select( b => b.MapAreaID )
					.Distinct()
					.OrderBy( i => i )
					.Select( i => KCDatabase.Instance.MapArea[i] )
					.Where( m => m != null );

				foreach ( var map in maps ) {
					int mapAreaID = map.MapAreaID;
					string name = map.Name;

					if ( string.IsNullOrWhiteSpace( map.Name ) || map.Name == "※" )
						name = "イベント海域";

					var tool = new ToolStripMenuItem( string.Format( "#{0} {1}", mapAreaID, name ), null,
						new EventHandler( ( ssender, ee ) => TopMenu_Edit_MapArea_Click( mapAreaID ) ) );

					TopMenu_Edit_ImportOrganization.DropDownItems.Add( tool );
				}
			}

			// 表示部初期化
			for ( int i  =0; i < BaseAirCorpsUIList.Length; i++ ) {
				var ui = BaseAirCorpsUIList[i];
				var table = TableBaseAirCorpsList[i];

				table.SuspendLayout();
				foreach ( var squi in ui.Squadrons ) {
					squi.AircraftCategory.SelectedItem = null;
				}
				table.ResumeLayout();
			}

			ClientSize = tableLayoutPanel2.PreferredSize;
			this.Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBaseAirCorps] );

		}

		void BaseAirCorpsUIList_Updated( object sender, EventArgs e ) {
			// 重複check	

			var sqs = BaseAirCorpsUIList.SelectMany( ui => ui.Squadrons.Select( squi => squi.Aircraft.SelectedItem ).OfType<ComboBoxEquipment>() );

			var sqids = sqs.Where( sq => sq != null && sq.UniqueID > 0 );
			var dupes = sqids.GroupBy( sq => sq.UniqueID ).Where( g => g.Count() > 1 ).Select( g => g.Key );

			for ( int i = 0; i < BaseAirCorpsUIList.Length; i++ ) {
				var ui = BaseAirCorpsUIList[i];
				var dupelist = new List<int>();

				for ( int x = 0; x < ui.Squadrons.Length; x++ ) {
					var squi = ui.Squadrons[x];
					var selected = squi.Aircraft.SelectedItem as ComboBoxEquipment;

					if ( selected != null && dupes.Contains( selected.UniqueID ) )
						dupelist.Add( x );
				}

				if ( dupelist.Any() ) {
					ui.DuplicateCheck.Text = "重複あり " + string.Join( ", ", dupelist.Select( d => "#" + ( d + 1 ) ) );
				} else {
					ui.DuplicateCheck.Text = "";
				}
			}
		}



		void TableBaseAirCorps_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}

		private void TopMenu_Edit_MapArea_Click( int mapAreaID ) {

			for ( int i = 0; i < BaseAirCorpsUIList.Length; i++ ) {

				var ui = BaseAirCorpsUIList[i];

				int id = mapAreaID * 10 + i + 1;
				var baseAirCorps = KCDatabase.Instance.BaseAirCorps[id];

				if ( baseAirCorps == null ) {
					for ( int x  =0; x < ui.Squadrons.Length; x++ ) {
						ui.Squadrons[x].AircraftCategory.SelectedItem = null;
						ui.Squadrons[x].Aircraft.SelectedItem = null;
					}
					continue;
				}

				for ( int x = 0; x < ui.Squadrons.Length; x++ ) {
					var sq = baseAirCorps[x + 1];

					if ( sq.State != 1 ) {
						ui.Squadrons[x].AircraftCategory.SelectedItem = null;
						ui.Squadrons[x].Aircraft.SelectedItem = null;
					} else {
						ui.Squadrons[x].AircraftCategory.SelectedItem = ui.Squadrons[x].AircraftCategory.Items.OfType<ComboBoxCategory>().FirstOrDefault( cat => cat == sq.EquipmentInstanceMaster.CategoryType );
						ui.Squadrons[x].Aircraft.SelectedItem = ui.Squadrons[x].Aircraft.Items.OfType<ComboBoxEquipment>().FirstOrDefault( eq => eq.UniqueID == sq.EquipmentMasterID );
						ui.Squadrons[x].AircraftCount.Value = sq.AircraftCurrent;
					}
				}
			}

		}

		private void TopMenu_Edit_Clear_Click( object sender, EventArgs e ) {
			if ( MessageBox.Show( "編成をすべてクリアします。\r\nよろしいですか？", "編成クリア", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1 )
				== System.Windows.Forms.DialogResult.Yes ) {

				for ( int i  =0; i < BaseAirCorpsUIList.Length; i++ ) {
					var ui = BaseAirCorpsUIList[i];
					var table = TableBaseAirCorpsList[i];

					table.SuspendLayout();
					foreach ( var squi in ui.Squadrons ) {
						squi.AircraftCategory.SelectedItem = null;
						squi.Aircraft.SelectedItem = null;
					}
					table.ResumeLayout();
				}
			}
		}

		private void DialogBaseAirCorpsSimulation_FormClosed( object sender, FormClosedEventArgs e ) {
			ResourceManager.DestroyIcon( Icon );
		}


	}
}
