using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Data;
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

	public partial class FormCompass : DockContent {


		private class TableEnemyMemberControl {

			public ImageLabel ShipName;
			public ShipStatusEquipment Equipments;

			public FormCompass Parent;
			public ToolTip ToolTipInfo;


			public TableEnemyMemberControl( FormCompass parent ) {

				#region Initialize

				Parent = parent;
				ToolTipInfo = parent.ToolTipInfo;


				ShipName = new ImageLabel();
				ShipName.Anchor = AnchorStyles.Left;
				ShipName.ForeColor = parent.MainFontColor;
				ShipName.ImageAlign = ContentAlignment.MiddleCenter;
				ShipName.Padding = new Padding( 0, 1, 0, 1 );
				ShipName.Margin = new Padding( 2, 0, 2, 0 );
				//ShipName.MaximumSize = new Size( 60, 20 );
				ShipName.AutoEllipsis = true;
				ShipName.AutoSize = true;
				ShipName.Cursor = Cursors.Help;
				ShipName.MouseClick += ShipName_MouseClick;

				Equipments = new ShipStatusEquipment();
				Equipments.SuspendLayout();
				Equipments.Anchor = AnchorStyles.Left;
				Equipments.Padding = new Padding( 0, 2, 0, 1 );
				Equipments.Margin = new Padding( 2, 0, 2, 0 );
				Equipments.Size = new Size( 40, 20 );	//checkme: 要る？
				Equipments.AutoSize = true;
				Equipments.ResumeLayout();

				ConfigurationChanged();

				#endregion

			}


			public TableEnemyMemberControl( FormCompass parent, TableLayoutPanel table, int row )
				: this( parent ) {

				AddToTable( table, row );
			}

			public void AddToTable( TableLayoutPanel table, int row ) {

				table.Controls.Add( ShipName, 0, row );
				table.Controls.Add( Equipments, 1, row );

				#region set RowStyle
				RowStyle rs = new RowStyle( SizeType.AutoSize );

				if ( table.RowStyles.Count > row )
					table.RowStyles[row] = rs;
				else
					while ( table.RowStyles.Count <= row )
						table.RowStyles.Add( rs );
				#endregion
			}



			public void Update( int shipID ) {
				var slot = shipID != -1 ? KCDatabase.Instance.MasterShips[shipID].DefaultSlot : null;
				Update( shipID, slot != null ? slot.ToArray() : null );
			}


			public void Update( int shipID, int[] slot ) {

				ShipName.Tag = shipID;

				if ( shipID == -1 ) {
					//なし
					ShipName.Text = "-";
					ShipName.ForeColor = Utility.Configuration.Config.UI.ForeColor;
					Equipments.Visible = false;
					ToolTipInfo.SetToolTip( ShipName, null );
					ToolTipInfo.SetToolTip( Equipments, null );

				} else {

					ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];


					ShipName.Text = ship.Name;
					ShipName.ForeColor = GetShipNameColor( ship );
					ToolTipInfo.SetToolTip( ShipName, GetShipString( shipID, slot ) );

					Equipments.SetSlotList( shipID, slot );
					Equipments.Visible = true;
					ToolTipInfo.SetToolTip( Equipments, GetEquipmentString( shipID, slot ) );
				}

			}

			public void UpdateEquipmentToolTip( int shipID, int[] slot, int level, int hp, int firepower, int torpedo, int aa, int armor ) {

				ToolTipInfo.SetToolTip( ShipName, GetShipString( shipID, slot, level, hp, firepower, torpedo, aa, armor ) );
			}


			void ShipName_MouseClick( object sender, MouseEventArgs e ) {

				if ( ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 ) {
					int shipID = ShipName.Tag as int? ?? -1;

					if ( shipID != -1 )
						new DialogAlbumMasterShip( shipID ).Show( Parent );
				}

			}


			public void ConfigurationChanged() {
				ShipName.Font = Parent.MainFont;
				Equipments.Font = Parent.SubFont;

			}

		}


		private class TableEnemyCandidateControl {

			public ImageLabel[] ShipNames;
			public ImageLabel Formation;
			public ImageLabel AirSuperiority;

			public FormCompass Parent;
			public ToolTip ToolTipInfo;


			public TableEnemyCandidateControl( FormCompass parent ) {

				#region Initialize

				Parent = parent;
				ToolTipInfo = parent.ToolTipInfo;


				ShipNames = new ImageLabel[6];
				for ( int i = 0; i < ShipNames.Length; i++ ) {
					ShipNames[i] = InitializeImageLabel();
					ShipNames[i].Cursor = Cursors.Help;
					ShipNames[i].MouseClick += TableEnemyCandidateControl_MouseClick;
				}

				Formation = InitializeImageLabel();
				Formation.Anchor = AnchorStyles.None;

				Formation.ImageAlign = ContentAlignment.MiddleLeft;
				Formation.ImageList = ResourceManager.Instance.Icons;
				Formation.ImageIndex = -1;


				AirSuperiority = InitializeImageLabel();
				AirSuperiority.Anchor = AnchorStyles.Right;
				AirSuperiority.ImageAlign = ContentAlignment.MiddleLeft;
				AirSuperiority.ImageList = ResourceManager.Instance.Equipments;
				AirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;


				ConfigurationChanged();

				#endregion

			}

			private ImageLabel InitializeImageLabel() {
				var label = new ImageLabel();
				label.Anchor = AnchorStyles.Left;
				label.ForeColor = Parent.MainFontColor;
				label.ImageAlign = ContentAlignment.MiddleCenter;
				label.Padding = new Padding( 0, 1, 0, 1 );
				label.Margin = new Padding( 4, 0, 4, 0 );
				label.MaximumSize = new Size( 60, 20 );
				label.AutoEllipsis = true;
				label.AutoSize = true;

				return label;
			}



			public TableEnemyCandidateControl( FormCompass parent, TableLayoutPanel table, int column )
				: this( parent ) {

				AddToTable( table, column );
			}

			public void AddToTable( TableLayoutPanel table, int column ) {

				table.ColumnCount = Math.Max( table.ColumnCount, column + 1 );
				table.RowCount = Math.Max( table.RowCount, 8 );

				for ( int i = 0; i < 6; i++ )
					table.Controls.Add( ShipNames[i], column, i );
				table.Controls.Add( Formation, column, 6 );
				table.Controls.Add( AirSuperiority, column, 7 );


			}


			public void ConfigurationChanged() {
				for ( int i = 0; i < ShipNames.Length; i++ )
					ShipNames[i].Font = Parent.MainFont;
				Formation.Font = AirSuperiority.Font = Parent.MainFont;
			}

			public void Update( EnemyFleetRecord.EnemyFleetElement fleet ) {

				if ( fleet == null ) {
					for ( int i = 0; i < 6; i++ )
						ShipNames[i].Visible = false;
					Formation.Visible = false;
					AirSuperiority.Visible = false;
					ToolTipInfo.SetToolTip( AirSuperiority, null );

					return;
				}

				for ( int i = 0; i < 6; i++ ) {

					var ship = KCDatabase.Instance.MasterShips[fleet.FleetMember[i]];

					// カッコカリ 上のとマージするといいかもしれない

					if ( ship == null ) {
						// nothing
						ShipNames[i].Text = "-";
						ShipNames[i].ForeColor = Utility.Configuration.Config.UI.ForeColor;
						ShipNames[i].Tag = -1;
						ShipNames[i].Cursor = Cursors.Default;
						ToolTipInfo.SetToolTip( ShipNames[i], null );

					} else {

						ShipNames[i].Text = ship.Name;
						ShipNames[i].ForeColor = GetShipNameColor( ship );
						ShipNames[i].Tag = ship.ShipID;
						ShipNames[i].Cursor = Cursors.Help;
						ToolTipInfo.SetToolTip( ShipNames[i], GetShipString( ship.ShipID, ship.DefaultSlot.ToArray() ) );
					}

					ShipNames[i].Visible = true;

				}

				//Formation.Text = Constants.GetFormationShort( fleet.Formation );
				Formation.ImageIndex = (int)ResourceManager.IconContent.BattleFormationEnemyLineAhead + fleet.Formation - 1;
				ToolTipInfo.SetToolTip( Formation, Constants.GetFormationShort( fleet.Formation ) );
				Formation.Visible = true;

				{
					int air = Calculator.GetAirSuperiority( fleet.FleetMember );
					AirSuperiority.Text = air.ToString();
					ToolTipInfo.SetToolTip( AirSuperiority, GetAirSuperiorityString( air ) );
					AirSuperiority.Visible = true;
				}

			}


			void TableEnemyCandidateControl_MouseClick( object sender, MouseEventArgs e ) {

				if ( ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 ) {
					int shipID = ( (ImageLabel)sender ).Tag as int? ?? -1;

					if ( shipID != -1 )
						new DialogAlbumMasterShip( shipID ).Show( Parent );
				}
			}

		}



		#region ***Control method

		private static Color GetShipNameColor( ShipDataMaster ship ) {
			switch ( ship.AbyssalShipClass ) {
				case 0:
				case 1:		//normal
				default:
					return Utility.Configuration.Config.UI.ForeColor; // Color.FromArgb( 0x00, 0x00, 0x00 );
				case 2:		//elite
					return Utility.Configuration.Config.UI.EliteColor; // Color.FromArgb( 0xFF, 0x00, 0x00 );
				case 3:		//flagship
					return Utility.Configuration.Config.UI.FlagshipColor; // Color.FromArgb( 0xFF, 0x88, 0x00 );
				case 4:		//latemodel / flagship kai
					return Utility.Configuration.Config.UI.LateModelColor; // Color.FromArgb( 0x00, 0x88, 0xFF );
				case 5:		//latemodel elite
					return Utility.Configuration.Config.UI.LateModelEliteColor; // Color.FromArgb( 0x88, 0x00, 0x00 );
				case 6:		//latemodel flagship
					return Utility.Configuration.Config.UI.LateModelFlagshipColor; // Color.FromArgb( 0x88, 0x44, 0x00 );
			}
		}


		private static string GetShipString( int shipID, int[] slot ) {

			ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
			if ( ship == null ) return null;

			return GetShipString( shipID, slot, -1, ship.HPMin, ship.FirepowerMax, ship.TorpedoMax, ship.AAMax, ship.ArmorMax,
				 ship.ASW != null && !ship.ASW.IsMaximumDefault ? ship.ASW.Maximum : -1,
				 ship.Evasion != null && !ship.Evasion.IsMaximumDefault ? ship.Evasion.Maximum : -1,
				 ship.LOS != null && !ship.LOS.IsMaximumDefault ? ship.LOS.Maximum : -1,
				 ship.LuckMin );
		}

		private static string GetShipString( int shipID, int[] slot, int level, int hp, int firepower, int torpedo, int aa, int armor ) {
			ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
			if ( ship == null ) return null;

			return GetShipString( shipID, slot, level, hp, firepower, torpedo, aa, armor,
				ship.ASW != null && ship.ASW.IsAvailable ? ship.ASW.GetParameter( level ) : -1,
				ship.Evasion != null && ship.Evasion.IsAvailable ? ship.Evasion.GetParameter( level ) : -1,
				ship.LOS != null && ship.LOS.IsAvailable ? ship.LOS.GetParameter( level ) : -1,
				level > 99 ? Math.Min( ship.LuckMin + 3, ship.LuckMax ) : ship.LuckMin );
		}

		private static string GetShipString( int shipID, int[] slot, int level, int hp, int firepower, int torpedo, int aa, int armor, int asw, int evasion, int los, int luck ) {

			ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
			if ( ship == null ) return null;

			int firepower_c = firepower;
			int torpedo_c = torpedo;
			int aa_c = aa;
			int armor_c = armor;
			int asw_c = asw;
			int evasion_c = evasion;
			int los_c = los;
			int luck_c = luck;
			int range = ship.Range;

			asw = Math.Max( asw, 0 );
			evasion = Math.Max( evasion, 0 );
			los = Math.Max( los, 0 );

			if ( slot != null ) {
				int count = slot.Length;
				for ( int i = 0; i < count; i++ ) {
					EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[slot[i]];
					if ( eq == null ) continue;

					firepower += eq.Firepower;
					torpedo += eq.Torpedo;
					aa += eq.AA;
					armor += eq.Armor;
					asw += eq.ASW;
					evasion += eq.Evasion;
					los += eq.LOS;
					luck += eq.Luck;
					range = Math.Max( range, eq.Range );
				}
			}

			/*
			return string.Format(
						"{0} {1}{2}\n耐久: {3}\n火力: {4}/{5}\n雷装: {6}/{7}\n対空: {8}/{9}\n装甲: {10}/{11}\n対潜: {12}/{13}\n回避: {14}/{15}\n索敵: {16}/{17}\n運: {18}/{19}\n射程: {20} / 速力: {21}\n(右クリックで図鑑)\n",
						ship.ShipTypeName, ship.NameWithClass, level < 1 ? "" : string.Format( " Lv. {0}", level ),
						hp,
						firepower_c, firepower,
						torpedo_c, torpedo,
						aa_c, aa,
						armor_c, armor,
						asw_c == -1 ? "???" : asw_c.ToString(), asw,
						evasion_c == -1 ? "???" : evasion_c.ToString(), evasion,
						los_c == -1 ? "???" : los_c.ToString(), los,
						luck_c, luck,
						Constants.GetRange( range ),
						Constants.GetSpeed( ship.Speed )
						);
			*/

			var sb = new StringBuilder();

			sb.Append( ship.ShipTypeName ).Append( " " ).Append( ship.NameWithClass );
			if ( level > 0 )
				sb.Append( " Lv. " ).Append( level );
			sb.AppendLine();

			sb.Append( "耐久: " ).Append( hp ).AppendLine();

			sb.Append( "火力: " ).Append( firepower_c );
			if ( firepower_c != firepower )
				sb.Append( "/" ).Append( firepower );
			/*
            if ( ship.ShipType == 7 ||	// 轻空母
				 ship.ShipType == 11 ||	// 正规空母
                 ship.IsLandBase) {
                sb.Append( "/ 空母火力: " ).Append( CalculatorEx.CalculateFireEnemy( shipID, slot, firepower_c, torpedo_c ) );
            }
			*/
			sb.AppendLine();

			sb.Append( "雷装: " ).Append( torpedo_c );
			if ( torpedo_c != torpedo )
				sb.Append( "/" ).Append( torpedo );
			sb.AppendLine();

			sb.Append( "対空: " ).Append( aa_c );
			if ( aa_c != aa )
				sb.Append( "/" ).Append( aa );
			sb.AppendLine();

            sb.Append( "加权对空: " ).Append( CalculatorEx.CalculateWeightingAAEnemy( shipID, slot, aa_c ) ).AppendLine();

			sb.Append( "装甲: " ).Append( armor_c );
			if ( armor_c != armor )
				sb.Append( "/" ).Append( armor );
			sb.AppendLine();

			sb.Append( "対潜: " );
			if ( asw_c < 0 ) sb.Append( "???" );
			else sb.Append( asw_c );
			if ( asw_c != asw )
				sb.Append( "/" ).Append( asw );
			sb.AppendLine();

			sb.Append( "回避: " );
			if ( evasion_c < 0 ) sb.Append( "???" );
			else sb.Append( evasion_c );
			if ( evasion_c != evasion )
				sb.Append( "/" ).Append( evasion );
			sb.AppendLine();

			sb.Append( "索敵: " );
			if ( los_c < 0 ) sb.Append( "???" );
			else sb.Append( los_c );
			if ( los_c != los )
				sb.Append( "/" ).Append( los );
			sb.AppendLine();

			sb.Append( "運: " ).Append( luck_c );
			if ( luck_c != luck )
				sb.Append( "/" ).Append( luck );
			sb.AppendLine();

			sb.AppendFormat( "射程: {0} / 速力: {1}\r\n(右クリックで図鑑)\r\n",
				Constants.GetRange( range ),
				Constants.GetSpeed( ship.Speed ) );

			return sb.ToString();

		}

		private static string GetEquipmentString( int shipID, int[] slot ) {
			StringBuilder sb = new StringBuilder();
			ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];

			if ( ship == null || slot == null ) return null;

			for ( int i = 0; i < slot.Length; i++ ) {
				if ( slot[i] != -1 )
					sb.AppendFormat( "[{0}] {1}\r\n", ship.Aircraft[i], KCDatabase.Instance.MasterEquipments[slot[i]].Name );
			}

			sb.AppendFormat( "\r\n昼戦: {0}\r\n夜戦: {1}\r\n",
				Constants.GetDayAttackKind( Calculator.GetDayAttackKind( slot, ship.ShipID, -1 ) ),
				Constants.GetNightAttackKind( Calculator.GetNightAttackKind( slot, ship.ShipID, -1 ) ) );

			{
				int aacutin = Calculator.GetAACutinKind( shipID, slot );
				if ( aacutin != 0 ) {
					sb.AppendFormat( "対空: {0}\r\n", Constants.GetAACutinKind( aacutin ) );
				}
			}
			{
				int airsup = Calculator.GetAirSuperiority( slot, ship.Aircraft.ToArray() );
				if ( airsup > 0 ) {
					sb.AppendFormat( "制空戦力: {0}\r\n", airsup );
				}
			}

			return sb.ToString();
		}

		private static string GetAirSuperiorityString( int air ) {
			if ( air > 0 ) {
				return string.Format( "確保: {0}\r\n優勢: {1}\r\n均衡: {2}\r\n劣勢: {3}\r\n",
							(int)( air * 3.0 ),
							(int)Math.Ceiling( air * 1.5 ),
							(int)( air / 1.5 + 1 ),
							(int)( air / 3.0 + 1 ) );
			}
			return null;
		}

		#endregion




		public Font MainFont { get; set; }
		public Font SubFont { get; set; }
		public Color MainFontColor { get; set; }
		public Color SubFontColor { get; set; }

		private Pen LinePen = Pens.Silver;

		private TableEnemyMemberControl[] ControlMembers;
		private TableEnemyCandidateControl[] ControlCandidates;

		private int _candidatesDisplayCount;


		/// <summary>
		/// 次に遭遇する敵艦隊候補
		/// </summary>
		private List<EnemyFleetRecord.EnemyFleetElement> _enemyFleetCandidate = null;

		/// <summary>
		/// 表示中の敵艦隊候補のインデックス
		/// </summary>
		private int _enemyFleetCandidateIndex = 0;




		public FormCompass( FormMain parent ) {
			this.SuspendLayoutForDpiScale();
			InitializeComponent();




			ControlHelper.SetDoubleBuffered( BasePanel );
			ControlHelper.SetDoubleBuffered( TableEnemyMember );


			TableEnemyMember.SuspendLayout();
			ControlMembers = new TableEnemyMemberControl[6];
			for ( int i = 0; i < ControlMembers.Length; i++ ) {
				ControlMembers[i] = new TableEnemyMemberControl( this, TableEnemyMember, i );
			}
			TableEnemyMember.ResumeLayout();

			TableEnemyCandidate.SuspendLayout();
			ControlCandidates = new TableEnemyCandidateControl[6];
			for ( int i  = 0; i < ControlCandidates.Length; i++ ) {
				ControlCandidates[i] = new TableEnemyCandidateControl( this, TableEnemyCandidate, i );
			}
			//row/column style init
			for ( int y = 0; y < TableEnemyCandidate.RowCount; y++ ) {
				var rs = new RowStyle( SizeType.AutoSize );
				if ( TableEnemyCandidate.RowStyles.Count <= y )
					TableEnemyCandidate.RowStyles.Add( rs );
				else
					TableEnemyCandidate.RowStyles[y] = rs;
			}
			for ( int x = 0; x < TableEnemyCandidate.ColumnCount; x++ ) {
				var cs = new ColumnStyle( SizeType.AutoSize );
				if ( TableEnemyCandidate.ColumnStyles.Count <= x )
					TableEnemyCandidate.ColumnStyles.Add( cs );
				else
					TableEnemyCandidate.ColumnStyles[x] = cs;
			}
			TableEnemyCandidate.ResumeLayout();


			//BasePanel.SetFlowBreak( TextMapArea, true );
			BasePanel.SetFlowBreak( TextDestination, true );
			//BasePanel.SetFlowBreak( TextEventKind, true );
			BasePanel.SetFlowBreak( TextEventDetail, true );
			BasePanel.SetFlowBreak( TextAA, true );


			TextDestination.ImageList = ResourceManager.Instance.Equipments;
			TextEventDetail.ImageList = ResourceManager.Instance.Equipments;
			TextFormation.ImageList = ResourceManager.Instance.Icons;
			TextAirSuperiority.ImageList = ResourceManager.Instance.Equipments;
			TextAirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;



			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormCompass] );

			this.ResumeLayoutForDpiScale();
		}


		private void FormCompass_Load( object sender, EventArgs e ) {

			BasePanel.Visible = false;
			TextAA.ImageList = TextAirSuperiority.ImageList = ResourceManager.Instance.Equipments;
			TextAirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;
			TextAA.ImageIndex = (int)ResourceManager.EquipmentContent.AADirector;


			APIObserver o = APIObserver.Instance;

			o.APIList["api_port/port"].ResponseReceived += Updated;
			o.APIList["api_req_map/start"].ResponseReceived += Updated;
			o.APIList["api_req_map/next"].ResponseReceived += Updated;
			o.APIList["api_req_member/get_practice_enemyinfo"].ResponseReceived += Updated;

			o.APIList["api_req_sortie/battle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_battle_midnight/sp_midnight"].ResponseReceived += BattleStarted;
			o.APIList["api_req_sortie/airbattle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_sortie/ld_airbattle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_combined_battle/battle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_combined_battle/sp_midnight"].ResponseReceived += BattleStarted;
			o.APIList["api_req_combined_battle/airbattle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_combined_battle/battle_water"].ResponseReceived += BattleStarted;
			o.APIList["api_req_combined_battle/ld_airbattle"].ResponseReceived += BattleStarted;
			o.APIList["api_req_practice/battle"].ResponseReceived += BattleStarted;


			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}


		private void Updated( string apiname, dynamic data ) {

			Func<int, Color> getColorFromEventKind = ( int kind ) => {
				switch ( kind ) {
					case 0:
					case 1:
					default:	//昼夜戦・その他
						return Utility.Configuration.Config.UI.ForeColor;
					case 2:
					case 3:		//夜戦・夜昼戦
						return Color.Navy;
					case 4:		//航空戦
					case 6:		//長距離空襲戦
						return Color.DarkGreen;
				}
			};

			if ( apiname == "api_port/port" ) {

				BasePanel.Visible = false;

			} else if ( apiname == "api_req_member/get_practice_enemyinfo" ) {

				TextMapArea.Text = "演習";
				TextDestination.Text = string.Format( "{0} {1}", data.api_nickname, Constants.GetAdmiralRank( (int)data.api_rank ) );
				TextDestination.ImageAlign = ContentAlignment.MiddleCenter;
				TextDestination.ImageIndex = -1;
				ToolTipInfo.SetToolTip( TextDestination, null );
				TextEventKind.Text = data.api_cmt;
				TextEventKind.ForeColor = getColorFromEventKind( 0 );
				TextEventDetail.Text = string.Format( "Lv. {0} / {1} exp.", data.api_level, data.api_experience[0] );
				TextEventDetail.ImageAlign = ContentAlignment.MiddleCenter;
				TextEventDetail.ImageIndex = -1;
				ToolTipInfo.SetToolTip( TextEventDetail, null );
				TextEnemyFleetName.Text = data.api_deckname;

			} else {

				CompassData compass = KCDatabase.Instance.Battle.Compass;


				BasePanel.SuspendLayout();
				PanelEnemyFleet.Visible = false;
				PanelEnemyCandidate.Visible = false;
				TextEnemyFleetName.Visible =
				TextFormation.Visible =
				TextAirSuperiority.Visible = false;
				TextAA.Visible = false;

				_enemyFleetCandidate = null;
				_enemyFleetCandidateIndex = -1;


				TextMapArea.Text = string.Format( "出撃海域 : {0}-{1}{2}", compass.MapAreaID, compass.MapInfoID,
					compass.MapInfo.EventDifficulty > 0 ? " [" + Constants.GetDifficulty( compass.MapInfo.EventDifficulty ) + "]" : "" );
				{
					var mapinfo = compass.MapInfo;

					if ( mapinfo.IsCleared ) {
						ToolTipInfo.SetToolTip( TextMapArea, null );

					} else if ( mapinfo.RequiredDefeatedCount != -1 ) {
						ToolTipInfo.SetToolTip( TextMapArea, string.Format( "撃破: {0} / {1} 回", mapinfo.CurrentDefeatedCount, mapinfo.RequiredDefeatedCount ) );

					} else if ( mapinfo.MapHPMax > 0 ) {
						ToolTipInfo.SetToolTip( TextMapArea, string.Format( "{0}: {1} / {2}", mapinfo.GaugeType == 3 ? "TP" : "HP", mapinfo.MapHPCurrent, mapinfo.MapHPMax ) );

					} else {
						ToolTipInfo.SetToolTip( TextMapArea, null );
					}
				}


				TextDestination.Text = string.Format( "次のセル : {0}{1}", compass.Destination, ( compass.IsEndPoint ? " (終点)" : "" ) );
				if ( compass.LaunchedRecon != 0 ) {
					TextDestination.ImageAlign = ContentAlignment.MiddleRight;
					TextDestination.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;

					string tiptext;
					switch ( compass.CommentID ) {
						case 1:
							tiptext = "敵艦隊発見！";
							break;
						case 2:
							tiptext = "攻撃目標発見！";
							break;
						case 3:
							tiptext = "針路哨戒！";
							break;
						default:
							tiptext = "索敵機発艦！";
							break;
					}
					ToolTipInfo.SetToolTip( TextDestination, tiptext );

				} else {
					TextDestination.ImageAlign = ContentAlignment.MiddleCenter;
					TextDestination.ImageIndex = -1;
					ToolTipInfo.SetToolTip( TextDestination, null );
				}

				//とりあえずリセット
				TextEventDetail.ImageAlign = ContentAlignment.MiddleCenter;
				TextEventDetail.ImageIndex = -1;
				ToolTipInfo.SetToolTip( TextEventDetail, null );


				TextEventKind.ForeColor = getColorFromEventKind( 0 );

				{
					string eventkind = Constants.GetMapEventID( compass.EventID );

					switch ( compass.EventID ) {

						case 0:		//初期位置
						case 1:		//不明
							TextEventDetail.Text = "どうしてこうなった";
							break;

						case 2:		//資源
						case 8:		//船団護衛成功
							TextEventDetail.Text = GetMaterialName( compass ) + " x " + compass.GetItemAmount;
							break;

						case 3:		//渦潮
							{
								int materialmax = KCDatabase.Instance.Fleet.Fleets.Values
									.Where( f => f != null && f.IsInSortie )
									.SelectMany( f => f.MembersWithoutEscaped )
									.Max( s => {
										if ( s == null ) return 0;
										switch ( compass.WhirlpoolItemID ) {
											case 1:
												return s.Fuel;
											case 2:
												return s.Ammo;
											default:
												return 0;
										}
									} );

								TextEventDetail.Text = string.Format( "{0} x {1} ({2:p0})",
									Constants.GetMaterialName( compass.WhirlpoolItemID ),
									compass.WhirlpoolItemAmount,
									(double)compass.WhirlpoolItemAmount / Math.Max( materialmax, 1 ) );

							}
							break;

						case 4:		//通常戦闘
							if ( compass.EventKind >= 2 ) {
								eventkind += "/" + Constants.GetMapEventKind( compass.EventKind );

								TextEventKind.ForeColor = getColorFromEventKind( compass.EventKind );
							}
							UpdateEnemyFleet();
							break;

						case 5:		//ボス戦闘
							TextEventKind.ForeColor = Color.Red;
							goto case 4;

						case 6:		//気のせいだった
							switch ( compass.EventKind ) {

								case 0:		//気のせいだった
								default:
									TextEventDetail.Text = "";
									break;
								case 1:		//敵影を見ず
									eventkind = "敵影を見ず";
									TextEventDetail.Text = "";
									break;
								case 2:		//能動分岐
									eventkind = "能動分岐";
									TextEventDetail.Text = string.Join( "/", compass.RouteChoices );
									break;
							}
							break;

						case 7:		//航空戦or航空偵察
							TextEventKind.ForeColor = getColorFromEventKind( compass.EventKind );

							switch ( compass.EventKind ) {
								case 0:		//航空偵察
									eventkind = "航空偵察";

									switch ( compass.AirReconnaissanceResult ) {
										case 0:
										default:
											TextEventDetail.Text = "失敗";
											break;
										case 1:
											TextEventDetail.Text = "成功";
											break;
										case 2:
											TextEventDetail.Text = "大成功";
											break;
									}

									switch ( compass.AirReconnaissancePlane ) {
										case 0:
										default:
											TextEventDetail.ImageAlign = ContentAlignment.MiddleCenter;
											TextEventDetail.ImageIndex = -1;
											break;
										case 1:
											TextEventDetail.ImageAlign = ContentAlignment.MiddleLeft;
											TextEventDetail.ImageIndex = (int)ResourceManager.EquipmentContent.FlyingBoat;
											break;
										case 2:
											TextEventDetail.ImageAlign = ContentAlignment.MiddleLeft;
											TextEventDetail.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
											break;
									}

									if ( compass.GetItemID != -1 ) {
										TextEventDetail.Text += string.Format( "　{0} x {1}", GetMaterialName( compass ), compass.GetItemAmount );
									}

									break;

								case 4:		//航空戦
								default:
									UpdateEnemyFleet();
									break;
							}
							break;

						case 9:		//揚陸地点
							TextEventDetail.Text = "";
							break;

						default:
							TextEventDetail.Text = "";
							break;

					}
					TextEventKind.Text = eventkind;
				}

				BasePanel.ResumeLayout();

				BasePanel.Visible = true;
			}


		}


		private string GetMaterialName( CompassData compass ) {

			if ( compass.GetItemID == 4 ) {		//"※"　大方資源専用ID

				return Constants.GetMaterialName( compass.GetItemIDMetadata );

			} else {
				UseItemMaster item =  KCDatabase.Instance.MasterUseItems[compass.GetItemIDMetadata];
				if ( item != null )
					return item.Name;
				else
					return "謎のアイテム";
			}
		}



		private void BattleStarted( string apiname, dynamic data ) {
			UpdateEnemyFleetInstant( apiname.Contains( "practice" ) );
		}





		private void UpdateEnemyFleet() {

			CompassData compass = KCDatabase.Instance.Battle.Compass;

			_enemyFleetCandidate = RecordManager.Instance.EnemyFleet.Record.Values.Where(
				r =>
					r.MapAreaID == compass.MapAreaID &&
					r.MapInfoID == compass.MapInfoID &&
					r.CellID == compass.Destination &&
					r.Difficulty == compass.MapInfo.EventDifficulty
				).ToList();
			_enemyFleetCandidateIndex = 0;


			if ( _enemyFleetCandidate.Count == 0 ) {
				TextEventDetail.Text = "(敵艦隊候補なし)";
				TextEnemyFleetName.Text = string.Empty;	//			"(敵艦隊情報不明)";

				TextFormation.Visible = false;
				TextAirSuperiority.Visible = false;
				TextAA.Visible = false;
				TableEnemyMember.Visible = false;

				TableEnemyCandidate.Visible = false;

			} else {
				_enemyFleetCandidate.Sort( ( a, b ) => {
					for ( int i = 0; i < a.FleetMember.Length; i++ ) {
						int diff = a.FleetMember[i] - b.FleetMember[i];
						if ( diff != 0 )
							return diff;
					}
					return a.Formation - b.Formation;
				} );

				NextEnemyFleetCandidate( 0 );
			}


			PanelEnemyFleet.Visible = false;

		}


		private void UpdateEnemyFleetInstant( bool isPractice = false ) {

			BattleManager bm = KCDatabase.Instance.Battle;
			BattleData bd;

			switch ( bm.BattleMode & BattleManager.BattleModes.BattlePhaseMask ) {
				case BattleManager.BattleModes.NightOnly:
				case BattleManager.BattleModes.NightDay:
					bd = bm.BattleNight;
					break;
				default:
					bd = bm.BattleDay;
					break;
			}

			int[] enemies = bd.Initial.EnemyMembers;
			int[][] slots = bd.Initial.EnemySlots;
			int[] levels = bd.Initial.EnemyLevels;
			int[][] parameters = bd.Initial.EnemyParameters;
			int[] hps = bd.Initial.MaxHPs;


			_enemyFleetCandidate = null;
			_enemyFleetCandidateIndex = -1;



			if ( ( bm.BattleMode & BattleManager.BattleModes.BattlePhaseMask ) != BattleManager.BattleModes.Practice ) {
				var efcurrent = EnemyFleetRecord.EnemyFleetElement.CreateFromCurrentState();
				var efrecord = RecordManager.Instance.EnemyFleet[efcurrent.FleetID];
				if ( efrecord != null ) {
					TextEnemyFleetName.Text = efrecord.FleetName;
				}
				TextEventDetail.Text = "敵艦隊ID: " + efcurrent.FleetID.ToString( "x8" );
				ToolTipInfo.SetToolTip( TextEventDetail, null );
			}

			//TextFormation.Text = Constants.GetFormationShort( (int)bd.Searching.FormationEnemy );
			TextFormation.Text = string.Empty;
			TextFormation.ImageIndex = (int)ResourceManager.IconContent.BattleFormationEnemyLineAhead + bd.Searching.FormationEnemy - 1;
			ToolTipInfo.SetToolTip( TextFormation, Constants.GetFormationShort( (int)bd.Searching.FormationEnemy ) );
			TextFormation.Visible = true;
			{
				int air = Calculator.GetAirSuperiority( enemies, slots );
				TextAirSuperiority.Text = isPractice ?
					air.ToString() + " ～ " + Calculator.GetAirSuperiorityAtMaxLevel( enemies, slots ).ToString() :
					air.ToString();
				ToolTipInfo.SetToolTip( TextAirSuperiority, GetAirSuperiorityString( isPractice ? 0 : air ) );
                TextAirSuperiority.Visible = true;
			}
			TextAA.Text = CalculatorEx.GetEnemyFleetAAValue( enemies, bd.Searching.FormationEnemy ).ToString();

			TableEnemyMember.SuspendLayout();
			for ( int i = 0; i < ControlMembers.Length; i++ ) {
				int shipID = enemies[i];
				ControlMembers[i].Update( shipID, shipID != -1 ? slots[i] : null );

				if ( shipID != -1 )
					ControlMembers[i].UpdateEquipmentToolTip( shipID, slots[i], levels[i], hps[i + 6], parameters[i][0], parameters[i][1], parameters[i][2], parameters[i][3] );
			}
			TableEnemyMember.ResumeLayout();
			TableEnemyMember.Visible = true;

			PanelEnemyFleet.Visible = true;
			PanelEnemyCandidate.Visible = false;
			TextEnemyFleetName.Visible =
			TextFormation.Visible =
			TextAirSuperiority.Visible = true;
			TextAA.Visible = true;
			BasePanel.Visible = true;			//checkme

		}



		private void TextEnemyFleetName_MouseDown( object sender, MouseEventArgs e ) {

			if ( e.Button == System.Windows.Forms.MouseButtons.Left )
				NextEnemyFleetCandidate();
			else if ( e.Button == System.Windows.Forms.MouseButtons.Right )
				NextEnemyFleetCandidate( -_candidatesDisplayCount );
		}


		private void NextEnemyFleetCandidate() {
			NextEnemyFleetCandidate( _candidatesDisplayCount );
		}

		private void NextEnemyFleetCandidate( int offset ) {

			if ( _enemyFleetCandidate != null && _enemyFleetCandidate.Count != 0 ) {

				_enemyFleetCandidateIndex += offset;
				if ( _enemyFleetCandidateIndex < 0 )
					_enemyFleetCandidateIndex = ( _enemyFleetCandidate.Count - 1 ) - ( _enemyFleetCandidate.Count - 1 ) % _candidatesDisplayCount;
				else if ( _enemyFleetCandidateIndex >= _enemyFleetCandidate.Count )
					_enemyFleetCandidateIndex = 0;


				var candidate = _enemyFleetCandidate[_enemyFleetCandidateIndex];


				TextEventDetail.Text = TextEnemyFleetName.Text = candidate.FleetName;

				//TextEnemyFleetName.Text = candidate.FleetName;
				//TextFormation.Text = Constants.GetFormationShort( candidate.Formation );
				//TextAirSuperiority.Text = Calculator.GetAirSuperiority( candidate.FleetMember ).ToString();
				//TextAA.Text = CalculatorEx.GetEnemyFleetAAValue( candidate.FleetMember, candidate.Formation ).ToString();

				if ( _enemyFleetCandidate.Count > _candidatesDisplayCount ) {
					TextEventDetail.Text += " ▼";
					ToolTipInfo.SetToolTip( TextEventDetail, string.Format( "候補: {0} / {1}\r\n(左右クリックでページめくり)\r\n", _enemyFleetCandidateIndex + 1, _enemyFleetCandidate.Count ) );
				} else {
					ToolTipInfo.SetToolTip( TextEventDetail, string.Format( "候補: {0}\r\n", _enemyFleetCandidate.Count ) );
				}

				//TextFormation.Visible = true;
				//TextAirSuperiority.Visible = true;
				//TextAA.Visible = true;
				//TableEnemyMember.Visible = true;
				TableEnemyCandidate.SuspendLayout();
				for ( int i = 0; i < ControlCandidates.Length; i++ ) {
					if ( i + _enemyFleetCandidateIndex >= _enemyFleetCandidate.Count || i >= _candidatesDisplayCount ) {
						ControlCandidates[i].Update( null );
						continue;
					}

					ControlCandidates[i].Update( _enemyFleetCandidate[i + _enemyFleetCandidateIndex] );
				}
				TableEnemyCandidate.ResumeLayout();
				TableEnemyCandidate.Visible = true;

				PanelEnemyCandidate.Visible = true;

			}
		}


		void ConfigurationChanged() {

			Font = PanelEnemyFleet.Font = MainFont = Utility.Configuration.Config.UI.MainFont;
			SubFont = Utility.Configuration.Config.UI.SubFont;

			TextMapArea.Font =
			TextDestination.Font =
			TextEventKind.Font =
			TextEventDetail.Font = Font;

			_candidatesDisplayCount = Utility.Configuration.Config.FormCompass.CandidateDisplayCount;
			_enemyFleetCandidateIndex = 0;
			if ( PanelEnemyCandidate.Visible )
				NextEnemyFleetCandidate( 0 );

			if ( ControlMembers != null ) {
				bool flag = Utility.Configuration.Config.FormFleet.ShowAircraft;
				for ( int i = 0; i < ControlMembers.Length; i++ ) {
					ControlMembers[i].Equipments.ShowAircraft = flag;
					ControlMembers[i].ConfigurationChanged();
				}
			}


			if ( ControlCandidates != null ) {
				for ( int i = 0; i < ControlCandidates.Length; i++ )
					ControlCandidates[i].ConfigurationChanged();
			}
		}



		public override string GetPersistString()
		{
			return "Compass";
		}

		private void TableEnemyMember_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( LinePen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}

		private void TableEnemyCandidateMember_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {

			if ( _enemyFleetCandidate == null || _enemyFleetCandidateIndex + e.Column >= _enemyFleetCandidate.Count )
				return;


			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );

			if ( e.Row == 5 || e.Row == 7 ) {
				e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
			}
		}





	}

}
