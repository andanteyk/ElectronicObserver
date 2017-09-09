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

		private static readonly int[] SquadronAircraftCategories = { 
			6,		// 艦上戦闘機
			7,		// 艦上爆撃機
			8,		// 艦上攻撃機
			9,		// 艦上偵察機
			10,		// 水上偵察機
			11,		// 水上爆撃機
			41,		// 大型飛行艇
  			45,		// 水上戦闘機
			47,		// 陸上攻撃機
			48,		// 局地戦闘機
			56,		// 噴式戦闘機
			57,		// 噴式戦闘爆撃機	
			58,		// 噴式攻撃機
			59,		// 噴式偵察機
		};

		private static readonly int[] SquadronAttackerCategories = { 
			7,		// 艦上爆撃機
			8,		// 艦上攻撃機
			11,		// 水上爆撃機
			47,		// 陸上攻撃機
			57,		// 噴式戦闘爆撃機	
			58,		// 噴式攻撃機
		};

		private static readonly int[] SquadronFighterCategories = {
			6,		// 艦上戦闘機
			45,		// 水上戦闘機
			48,		// 局地戦闘機
			56,		// 噴式戦闘機
		};

		private static readonly int[] SquadronReconCategories = { 
			9,		// 艦上偵察機
			10,		// 水上偵察機
			41,		// 大型飛行艇
  			59,		// 噴式偵察機
		};


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

			public DialogBaseAirCorpsSimulation Parent;
			public ToolTip ToolTipInternal;


			public event EventHandler Updated = delegate { };


			public SquadronUI( int baseAirCorpsID, int squadronID, DialogBaseAirCorpsSimulation parent ) {

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

				Parent = parent;
				ToolTipInternal = parent.ToolTipInfo;

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

					ToolTipInternal.SetToolTip( Aircraft, null );
				} else {
					int aircraftCount = Calculator.IsAircraft( equipment.EquipmentID, false ) ? 18 : 4;
					AircraftCount.Value = AircraftCount.Maximum = aircraftCount;

					ToolTipInternal.SetToolTip( Aircraft, GetAircraftParameters( equipment.EquipmentInstance ) );
				}

				Update();

			}

			void AircraftCount_ValueChanged( object sender, EventArgs e ) {
				Update();
			}

			private static string GetAircraftParameters( EquipmentDataMaster eq ) {

				if ( eq == null )
					return "";

				var sb = new StringBuilder();

				Action<string, int> Add = ( name, value ) => {
					if ( value != 0 )
						sb.Append( name ).Append( ": " ).AppendLine( value.ToString( "+0;-0;0" ) );
				};

				Action<string, int> AddNoSign = ( name, value ) => {
					if ( value != 0 )
						sb.Append( name ).Append( ": " ).AppendLine( value.ToString() );
				};

				bool isLand = eq.CategoryType == 48;

				Add( "火力", eq.Firepower );
				Add( "雷装", eq.Torpedo );
				Add( "爆装", eq.Bomber );
				Add( "対空", eq.AA );
				Add( "装甲", eq.Armor );
				Add( "対潜", eq.ASW );
				Add( isLand ? "迎撃" : "回避", eq.Evasion );
				Add( "索敵", eq.LOS );
				Add( isLand ? "対爆" : "命中", eq.Accuracy );
				AddNoSign( "コスト", eq.AircraftCost );
				AddNoSign( "半径", eq.AircraftDistance );

				return sb.ToString();
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

			public Label TitleAutoAirSuperiority;
			public Label TitleAutoDistance;
			public ComboBox AutoAirSuperiorityMode;
			public NumericUpDown AutoAirSuperiority;
			public NumericUpDown AutoDistance;
			public Button AutoOrganizeSortie;
			public Button AutoOrganizeAirDefense;

			public DialogBaseAirCorpsSimulation Parent;
			public ToolTip ToolTipInternal;

			public event EventHandler Updated = delegate { };


			public BaseAirCorpsUI( int baseAirCorpsID, DialogBaseAirCorpsSimulation parent ) {

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
				TitleAutoAirSuperiority = NewTitleLabel();
				TitleAutoDistance = NewTitleLabel();

				TitleAircraftCategory.Text = "カテゴリ";
				TitleAircraft.Text = "配備機";
				TitleAircraftCount.Text = "機数";
				TitleAirSuperioritySortie.Text = "出撃制空";
				TitleAirSuperiorityAirDefense.Text = "防空制空";
				TitleDistance.Text = "半径";
				TitleBomber.Text = "爆装";
				TitleTorpedo.Text = "雷装";
				TitleOrganizationCost.Text = "配備コスト";
				TitleAutoAirSuperiority.Text = "目標制空";
				TitleAutoDistance.Text = "目標半径";

				AutoAirSuperiority = new NumericUpDown();
				AutoAirSuperiority.Size = new Size( 60, AutoAirSuperiority.Height );
				AutoAirSuperiority.Anchor = AnchorStyles.None;
				AutoAirSuperiority.Maximum = 9999;
				AutoAirSuperiority.TextAlign = HorizontalAlignment.Right;
				AutoAirSuperiority.Margin = new Padding( 2, 0, 2, 0 );

				AutoDistance = new NumericUpDown();
				AutoDistance.Size = new Size( 60, AutoDistance.Height );
				AutoDistance.Anchor = AnchorStyles.None;
				AutoDistance.Maximum = 20;
				AutoDistance.TextAlign = HorizontalAlignment.Right;
				AutoDistance.Margin = new Padding( 2, 0, 2, 0 );

				AutoAirSuperiorityMode = new ComboBox();
				AutoAirSuperiorityMode.Size = new Size( 160, AutoAirSuperiorityMode.Height );
				AutoAirSuperiorityMode.Anchor = AnchorStyles.None;
				AutoAirSuperiorityMode.Margin = new Padding( 2, 0, 2, 0 );
				AutoAirSuperiorityMode.DropDownStyle = ComboBoxStyle.DropDownList;
				AutoAirSuperiorityMode.Items.Add( -1 );
				AutoAirSuperiorityMode.Items.Add( 1 );
				AutoAirSuperiorityMode.Items.Add( 2 );
				AutoAirSuperiorityMode.Items.Add( 0 );
				AutoAirSuperiorityMode.Items.Add( 3 );
				AutoAirSuperiorityMode.Items.Add( 4 );
				AutoAirSuperiorityMode.FormattingEnabled = true;
				AutoAirSuperiorityMode.Format += AutoAirSuperiorityMode_Format;
				AutoAirSuperiorityMode.SelectedIndex = 0;

				AutoOrganizeSortie = new Button();
				AutoOrganizeSortie.Size = new Size( 60, AutoOrganizeSortie.Height );
				AutoOrganizeSortie.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				AutoOrganizeSortie.Margin = new Padding( 2, 0, 2, 0 );
				AutoOrganizeSortie.Text = "出撃編成";
				AutoOrganizeSortie.Click += AutoOrganize_Click;

				AutoOrganizeAirDefense = new Button();
				AutoOrganizeAirDefense.Size = new Size( 60, AutoOrganizeSortie.Height );
				AutoOrganizeAirDefense.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				AutoOrganizeAirDefense.Margin = new Padding( 2, 0, 2, 0 );
				AutoOrganizeAirDefense.Text = "防空編成";
				AutoOrganizeAirDefense.Click += AutoOrganize_Click;

				Squadrons = new SquadronUI[4];
				for ( int i = 0; i < Squadrons.Length; i++ ) {
					Squadrons[i] = new SquadronUI( baseAirCorpsID, i + 1, parent );
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

				Parent = parent;
				ToolTipInternal = parent.ToolTipInfo;

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

				int autocolumn = 9;
				table.Controls.Add( TitleAutoAirSuperiority, autocolumn + 0, 0 );
				table.Controls.Add( TitleAutoDistance, autocolumn + 1, 0 );
				table.Controls.Add( AutoAirSuperiority, autocolumn + 0, 1 );
				table.Controls.Add( AutoDistance, autocolumn + 1, 1 );
				table.Controls.Add( AutoAirSuperiorityMode, autocolumn + 0, 2 );
				table.Controls.Add( AutoOrganizeSortie, autocolumn + 0, 5 );
				table.Controls.Add( AutoOrganizeAirDefense, autocolumn + 1, 5 );

				table.SetColumnSpan( AutoAirSuperiorityMode, 2 );
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


			void AutoAirSuperiorityMode_Format( object sender, ListControlConvertEventArgs e ) {
				if ( e.DesiredType == typeof( string ) ) {
					int val = (int)e.Value;

					if ( val == -1 )
						e.Value = "ちょうど";
					else
						e.Value = Constants.GetAirSuperiority( val );
				}
			}

			void AutoOrganize_Click( object sender, EventArgs e ) {

				bool isAirDefense = sender == AutoOrganizeAirDefense;
				int airSuperiority = (int)AutoAirSuperiority.Value;
				switch ( AutoAirSuperiorityMode.SelectedItem as int? ?? 0 ) {
					case -1:
					default:
						break;
					case 1:
						airSuperiority = airSuperiority * 3;
						break;
					case 2:
						airSuperiority = (int)Math.Ceiling( airSuperiority * 1.5 );
						break;
					case 0:
						airSuperiority = (int)Math.Ceiling( airSuperiority / 1.5 );
						break;
					case 3:
						airSuperiority = (int)Math.Ceiling( airSuperiority / 3.0 );
						break;
					case 4:
						airSuperiority = 0;
						break;
				}
				int distance = (int)AutoDistance.Value;


				// 装備済み・ほかの航空隊に配備されている機体以外で編成
				var orgs = AutoOrganize( isAirDefense, airSuperiority, distance,
					Parent.GetUsingEquipments( new int[] { BaseAirCorpsID - 1 } ).Concat( KCDatabase.Instance.Ships.Values.SelectMany( s => s.AllSlot ) ) );

				if ( orgs == null || orgs.All( o => o == null ) ) {
					MessageBox.Show( "自動編成に失敗しました。\r\n条件が厳しすぎるか、航空機が不足しています。\r\n",
						"自動編成失敗", MessageBoxButtons.OK, MessageBoxIcon.Error );
					return;
				}

				for ( int i = 0; i < Squadrons.Length; i++ ) {
					var squi = Squadrons[i];

					squi.AircraftCategory.SelectedItem = squi.AircraftCategory.Items.OfType<ComboBoxCategory>().FirstOrDefault( c => c == ( orgs[i] == null ? -1 : orgs[i].MasterEquipment.CategoryType ) );
					squi.Aircraft.SelectedItem = squi.Aircraft.Items.OfType<ComboBoxEquipment>().FirstOrDefault( q => q.UniqueID == ( orgs[i] == null ? -1 : orgs[i].MasterID ) );
				}

				System.Media.SystemSounds.Asterisk.Play();
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

			public override string ToString() {
				if ( EquipmentInstance != null ) {

					var sb = new StringBuilder( EquipmentInstance.Name );

					if ( Level > 0 )
						sb.Append( "+" ).Append( Level );
					if ( AircraftLevel > 0 )
						sb.Append( " " ).Append( EquipmentData.AircraftLevelString[AircraftLevel] );

					sb.Append( " :" ).Append( EquipmentInstance.AircraftDistance );
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
				BaseAirCorpsUIList[i] = new BaseAirCorpsUI( i + 1, this );

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
			if ( !( e.Column == 9 && e.Row == 2 ) )
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



		/// <summary>
		/// 自動編成を行います。
		/// </summary>
		/// <param name="isAirDefense">防空かどうか。false なら出撃</param>
		/// <param name="minimumFigherPower">目標制空値。</param>
		/// <param name="minimumDistance">目標戦闘行動半径。</param>
		/// <param name="excludeEquipments">使用しない装備IDのリスト。</param>
		/// <returns>編成結果のリスト[4]。要素に null を含む可能性があります。編成不可能だった場合は null を返します。</returns>
		public static List<EquipmentData> AutoOrganize( bool isAirDefense, int minimumFigherPower, int minimumDistance, IEnumerable<int> excludeEquipments ) {

			var ret = new List<EquipmentData>( 4 );

			var available = KCDatabase.Instance.Equipments.Values
				.Where( eq => !excludeEquipments.Contains( eq.MasterID ) )
				.Select( eq => new { eq, master = eq.MasterEquipment } )
				.Where( eqp => SquadronAircraftCategories.Contains( eqp.master.CategoryType ) );

			var fighter = available
					.Where( eqp => SquadronFighterCategories.Contains( eqp.master.CategoryType ) );


			if ( !isAirDefense ) {

				// 戦闘機に割くスロット数
				int fighterSlot = -1;

				// 射程拡張が必要か、必要ならいくつ伸ばすか
				int extendedDistance;


				// 攻撃力(仮想的に 雷装+爆装)の高いのを詰め込む
				// 射程拡張も考慮して、 min - 3 まで確保しておく
				var attackerfp = available
					.Where( eq => SquadronAttackerCategories.Contains( eq.master.CategoryType ) && eq.master.AircraftDistance >= minimumDistance - 3 )
					.Select( eqp => new { eqp.eq, eqp.master, fp = Calculator.GetAirSuperiority( eqp.master.EquipmentID, 18, eqp.eq.AircraftLevel, eqp.eq.Level, false ) } )
					.OrderByDescending( eq => eq.master.Torpedo + eq.master.Bomber )
					.ThenBy( f => f.master.AircraftCost )
					.AsEnumerable();


				var fighterfp = fighter.Select( eqp => new { eqp.eq, eqp.master, fp = Calculator.GetAirSuperiority( eqp.master.EquipmentID, 18, eqp.eq.AircraftLevel, eqp.eq.Level, false ) } )
					.OrderByDescending( f => f.fp )
					.ThenBy( f => f.master.AircraftCost );

				// 最強の戦闘機を編成すると仮定して、最低何スロット必要かを調べる
				for ( extendedDistance = 0; extendedDistance <= 3; extendedDistance++ ) {

					var availfighterfp = fighterfp
						.Where( f => f.master.AircraftDistance + extendedDistance >= minimumDistance );

					for ( int i = 0; i <= ( extendedDistance > 0 ? 3 : 4 ); i++ ) {
						if ( availfighterfp.Take( i ).Sum( f => f.fp ) + attackerfp.Take( 4 - i - ( extendedDistance > 0 ? 1 : 0 ) ).Sum( f => f.fp ) >= minimumFigherPower ) {
							fighterSlot = i;
							break;
						}
					}

					if ( fighterSlot != -1 )
						break;
				}

				if ( fighterSlot == -1 )
					return null;		// 編成不可能


				// 攻撃隊の射程調整
				while ( attackerfp.Count( f => f.master.AircraftDistance + extendedDistance >= minimumDistance ) < ( 4 - ( extendedDistance > 0 ? 1 : 0 ) - fighterSlot ) &&
					extendedDistance < 3 )
					extendedDistance++;


				// 射程拡張が必要なら適切な偵察機を載せる
				if ( extendedDistance > 0 ) {
					// 延長距離 = sqrt( ( 偵察機距離 - その他距離 ) )
					// 偵察機距離 = 延長距離^2 + その他距離

					int reconDistance = extendedDistance * extendedDistance + ( minimumDistance - extendedDistance );

					var recon = available.Where( eqp => SquadronReconCategories.Contains( eqp.master.CategoryType ) &&
						eqp.master.AircraftDistance >= reconDistance )
						.OrderBy( eqp => eqp.master.AircraftCost )
						.FirstOrDefault();

					if ( recon == null )
						return null;	// 編成不可能

					ret.Add( recon.eq );
				}


				attackerfp = attackerfp
					.Where( f => f.master.AircraftDistance + extendedDistance >= minimumDistance )
					.Take( 4 - ret.Count - fighterSlot );
				minimumFigherPower -= attackerfp.Sum( f => f.fp );


				if ( fighterSlot > 0 ) {
					// 射程が足りている戦闘機
					var fighterfpdist = fighterfp.Where( f => f.master.AircraftDistance + extendedDistance >= minimumDistance );
					int estimatedIndex = fighterfpdist.TakeWhile( f => f.fp >= minimumFigherPower / fighterSlot ).Count();

					// fighterfpdist は 制空値が高い順 に並んでいるので、
					// 下から窓をずらしていけばいい感じのが出る（はず）
					// 少なくとも先頭(制空値最高)が 目標 / スロット 以下だと絶対に満たせないので、そこから始める
					for ( int i = Math.Min( estimatedIndex, fighterfpdist.Count() - fighterSlot ); i >= 0; i-- ) {

						var org = fighterfpdist.Skip( i ).Take( fighterSlot );
						if ( org.Sum( f => f.fp ) >= minimumFigherPower ) {
							ret.AddRange( org.Select( f => f.eq ) );
							break;
						}
					}
				}

				ret.AddRange( attackerfp.Select( f => f.eq ) );


			} else {
				// 防空

				// とりあえず最大補正の偵察機を突っ込む
				var recons = available
					.Where( eq => SquadronReconCategories.Contains( eq.master.CategoryType ) )
					.Select( eq => new { eq.eq, eq.master, bonus = Calculator.GetAirSuperiorityAirDefenseReconBonus( eq.master.EquipmentID ) } )
					.OrderByDescending( f => f.bonus )
					.ThenBy( eq => eq.master.AircraftCost );

				if ( recons.Any() ) {
					ret.Add( recons.First().eq );
					minimumFigherPower = (int)Math.Ceiling( minimumFigherPower / recons.First().bonus );
				}

				var fighterfp = fighter
					.Select( eqp => new { eqp.eq, eqp.master, fp = Calculator.GetAirSuperiority( eqp.master.EquipmentID, 18, eqp.eq.AircraftLevel, eqp.eq.Level, true ) } )
					.OrderByDescending( f => f.fp )
					.ThenBy( f => f.master.AircraftCost );

				int estimatedIndex = fighterfp.TakeWhile( f => f.fp >= minimumFigherPower / ( 4 - ret.Count ) ).Count();

				// fighterfp は 制空値が高い順 に並んでいるので、
				// 下から窓をずらしていけばいい感じのが出る（はず）
				for ( int i = Math.Min( estimatedIndex, fighterfp.Count() - ( 4 - ret.Count ) ); i >= 0; i-- ) {

					var org = fighterfp.Skip( i ).Take( 4 - ret.Count );
					if ( org.Sum( f => f.fp ) >= minimumFigherPower ) {
						ret.AddRange( org.Select( f => f.eq ) );
						break;
					}
				}

				if ( ret.Count == ( recons.Any() ? 1 : 0 ) )		// 戦闘機の配備に失敗
					return null;
			}

			while ( ret.Count < 4 )
				ret.Add( null );
			return ret;
		}


		/// <summary>
		/// 現在UI上に配備されている装備ID群を求めます。
		/// </summary>
		/// <param name="except">除外する航空隊のインデックス。</param>
		private IEnumerable<int> GetUsingEquipments( IEnumerable<int> except ) {

			foreach ( var corpsui in BaseAirCorpsUIList.Where( ( b, i ) => !except.Contains( i ) ) ) {
				foreach ( var squi in corpsui.Squadrons ) {

					var eq =  squi.Aircraft.SelectedItem as ComboBoxEquipment;

					if ( eq != null && eq.UniqueID != -1 ) {
						yield return eq.UniqueID;
					}
				}
			}
		}


	}
}
