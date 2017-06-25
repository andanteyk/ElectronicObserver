using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Data.Battle.Detail;
using ElectronicObserver.Data.Battle.Phase;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Control;
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

	public partial class FormBattle : DockContent {

		private readonly Color WinRankColor_Win = SystemColors.ControlText;
		private readonly Color WinRankColor_Lose = Color.Red;

		private readonly Size DefaultBarSize = new Size( 80, 20 );
		private readonly Size SmallBarSize = new Size( 60, 20 );

		private List<ShipStatusHP> HPBars;

		public Font MainFont { get; set; }
		public Font SubFont { get; set; }



		public FormBattle( FormMain parent ) {
			InitializeComponent();

			ControlHelper.SetDoubleBuffered( TableTop );
			ControlHelper.SetDoubleBuffered( TableBottom );


			HPBars = new List<ShipStatusHP>( 24 );


			TableBottom.SuspendLayout();
			for ( int i = 0; i < 24; i++ ) {
				HPBars.Add( new ShipStatusHP() );
				HPBars[i].Size = DefaultBarSize;
				HPBars[i].AutoSize = false;
				HPBars[i].AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
				HPBars[i].Margin = new Padding( 2, 0, 2, 0 );
				HPBars[i].Anchor = AnchorStyles.None;
				HPBars[i].MainFont = MainFont;
				HPBars[i].SubFont = SubFont;
				HPBars[i].UsePrevValue = true;
				HPBars[i].ShowDifference = true;
				HPBars[i].MaximumDigit = 9999;

				if ( i < 6 ) {
					TableBottom.Controls.Add( HPBars[i], 0, i + 1 );
				} else if ( i < 12 ) {
					TableBottom.Controls.Add( HPBars[i], 3, i - 5 );
				} else if ( i < 18 ) {
					TableBottom.Controls.Add( HPBars[i], 1, i - 11 );
				} else {
					TableBottom.Controls.Add( HPBars[i], 2, i - 17 );
				}
			}
			TableBottom.ResumeLayout();


			Searching.ImageList =
			SearchingFriend.ImageList =
			SearchingEnemy.ImageList =
			AACutin.ImageList =
			AirStage1Friend.ImageList =
			AirStage1Enemy.ImageList =
			AirStage2Friend.ImageList =
			AirStage2Enemy.ImageList =
				ResourceManager.Instance.Equipments;


			ConfigurationChanged();

			BaseLayoutPanel.Visible = false;


			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBattle] );

		}



		private void FormBattle_Load( object sender, EventArgs e ) {

			APIObserver o = APIObserver.Instance;

			o.APIList["api_port/port"].ResponseReceived += Updated;
			o.APIList["api_req_map/start"].ResponseReceived += Updated;
			o.APIList["api_req_map/next"].ResponseReceived += Updated;
			o.APIList["api_req_sortie/battle"].ResponseReceived += Updated;
			o.APIList["api_req_sortie/battleresult"].ResponseReceived += Updated;
			o.APIList["api_req_battle_midnight/battle"].ResponseReceived += Updated;
			o.APIList["api_req_battle_midnight/sp_midnight"].ResponseReceived += Updated;
			o.APIList["api_req_sortie/airbattle"].ResponseReceived += Updated;
			o.APIList["api_req_sortie/ld_airbattle"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/battle"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/midnight_battle"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/sp_midnight"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/airbattle"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/battle_water"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/ld_airbattle"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/ec_battle"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/ec_midnight_battle"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/each_battle"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/each_battle_water"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/battleresult"].ResponseReceived += Updated;
			o.APIList["api_req_practice/battle"].ResponseReceived += Updated;
			o.APIList["api_req_practice/midnight_battle"].ResponseReceived += Updated;
			o.APIList["api_req_practice/battle_result"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		}


		private void Updated( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;
			BattleManager bm = db.Battle;
			bool hideDuringBattle = Utility.Configuration.Config.FormBattle.HideDuringBattle;

			BaseLayoutPanel.SuspendLayout();
			TableTop.SuspendLayout();
			TableBottom.SuspendLayout();
			switch ( apiname ) {

				case "api_port/port":
					BaseLayoutPanel.Visible = false;
					ToolTipInfo.RemoveAll();
					break;

				case "api_req_map/start":
				case "api_req_map/next":
					if ( !bm.Compass.HasAirRaid )
						goto case "api_port/port";

					SetFormation( bm );
					ClearSearchingResult();
					ClearBaseAirAttack();
					SetAerialWarfare( ( (BattleBaseAirRaid)bm.BattleDay ).BaseAirRaid );
					SetHPBar( bm.BattleDay );
					SetDamageRate( bm );

					BaseLayoutPanel.Visible = !hideDuringBattle;
					break;


				case "api_req_sortie/battle":
				case "api_req_practice/battle":
				case "api_req_sortie/ld_airbattle": {

						SetFormation( bm );
						SetSearchingResult( bm.BattleDay );
						SetBaseAirAttack( bm.BattleDay.BaseAirAttack );
						SetAerialWarfare( bm.BattleDay.AirBattle );
						SetHPBar( bm.BattleDay );
						SetDamageRate( bm );

						BaseLayoutPanel.Visible = !hideDuringBattle;
					} break;

				case "api_req_battle_midnight/battle":
				case "api_req_practice/midnight_battle": {

						SetNightBattleEvent( bm.BattleNight.NightBattle );
						SetHPBar( bm.BattleNight );
						SetDamageRate( bm );

						BaseLayoutPanel.Visible = !hideDuringBattle;
					} break;

				case "api_req_battle_midnight/sp_midnight": {

						SetFormation( bm );
						ClearBaseAirAttack();
						ClearAerialWarfare();
						ClearSearchingResult();
						SetNightBattleEvent( bm.BattleNight.NightBattle );
						SetHPBar( bm.BattleNight );
						SetDamageRate( bm );

						BaseLayoutPanel.Visible = !hideDuringBattle;
					} break;

				case "api_req_sortie/airbattle": {

						SetFormation( bm );
						SetSearchingResult( bm.BattleDay );
						SetBaseAirAttack( bm.BattleDay.BaseAirAttack );
						SetAerialWarfareAirBattle( bm.BattleDay.AirBattle, ( (BattleAirBattle)bm.BattleDay ).AirBattle2 );
						SetHPBar( bm.BattleDay );
						SetDamageRate( bm );

						BaseLayoutPanel.Visible = !hideDuringBattle;
					} break;

				case "api_req_combined_battle/battle":
				case "api_req_combined_battle/battle_water":
				case "api_req_combined_battle/ld_airbattle":
				case "api_req_combined_battle/ec_battle":
				case "api_req_combined_battle/each_battle":
				case "api_req_combined_battle/each_battle_water": {

						SetFormation( bm );
						SetSearchingResult( bm.BattleDay );
						SetBaseAirAttack( bm.BattleDay.BaseAirAttack );
						SetAerialWarfare( bm.BattleDay.AirBattle );
						SetHPBar( bm.BattleDay );
						SetDamageRate( bm );

						BaseLayoutPanel.Visible = !hideDuringBattle;
					} break;

				case "api_req_combined_battle/airbattle": {

						SetFormation( bm );
						SetSearchingResult( bm.BattleDay );
						SetBaseAirAttack( bm.BattleDay.BaseAirAttack );
						SetAerialWarfareAirBattle( bm.BattleDay.AirBattle, ( (BattleCombinedAirBattle)bm.BattleDay ).AirBattle2 );
						SetHPBar( bm.BattleDay );
						SetDamageRate( bm );

						BaseLayoutPanel.Visible = !hideDuringBattle;
					} break;

				case "api_req_combined_battle/midnight_battle":
				case "api_req_combined_battle/ec_midnight_battle": {

						SetNightBattleEvent( bm.BattleNight.NightBattle );
						SetHPBar( bm.BattleNight );
						SetDamageRate( bm );

						BaseLayoutPanel.Visible = !hideDuringBattle;
					} break;

				case "api_req_combined_battle/sp_midnight": {

						SetFormation( bm );
						ClearAerialWarfare();
						ClearSearchingResult();
						ClearBaseAirAttack();
						SetNightBattleEvent( bm.BattleNight.NightBattle );
						SetHPBar( bm.BattleNight );
						SetDamageRate( bm );

						BaseLayoutPanel.Visible = !hideDuringBattle;
					} break;


				case "api_req_sortie/battleresult":
				case "api_req_combined_battle/battleresult":
				case "api_req_practice/battle_result": {

						SetMVPShip( bm );

						BaseLayoutPanel.Visible = true;
					} break;

			}

			TableTop.ResumeLayout();
			TableBottom.ResumeLayout();

			BaseLayoutPanel.ResumeLayout();


			if ( Utility.Configuration.Config.UI.IsLayoutFixed )
				TableTop.Width = TableTop.GetPreferredSize( BaseLayoutPanel.Size ).Width;
			else
				TableTop.Width = TableBottom.ClientSize.Width;
			TableTop.Height = TableTop.GetPreferredSize( BaseLayoutPanel.Size ).Height;

		}


		/// <summary>
		/// 陣形・交戦形態を設定します。
		/// </summary>
		private void SetFormation( BattleManager bm ) {

			FormationFriend.Text = Constants.GetFormationShort( bm.FirstBattle.Searching.FormationFriend );
			FormationEnemy.Text = Constants.GetFormationShort( bm.FirstBattle.Searching.FormationEnemy );
			Formation.Text = Constants.GetEngagementForm( bm.FirstBattle.Searching.EngagementForm );

			if ( bm.Compass != null && bm.Compass.EventID == 5 ) {
				FleetEnemy.ForeColor = Color.Red;
			} else {
				FleetEnemy.ForeColor = SystemColors.ControlText;
			}
		}

		/// <summary>
		/// 索敵結果を設定します。
		/// </summary>
		private void SetSearchingResult( BattleData bd ) {

			int searchFriend = bd.Searching.SearchingFriend;
			SearchingFriend.Text = Constants.GetSearchingResultShort( searchFriend );
			SearchingFriend.ImageAlign = searchFriend > 0 ? ContentAlignment.MiddleLeft : ContentAlignment.MiddleCenter;
			SearchingFriend.ImageIndex = searchFriend > 0 ? (int)( searchFriend < 4 ? ResourceManager.EquipmentContent.Seaplane : ResourceManager.EquipmentContent.Radar ) : -1;
			ToolTipInfo.SetToolTip( SearchingFriend, null );

			int searchEnemy = bd.Searching.SearchingEnemy;
			SearchingEnemy.Text = Constants.GetSearchingResultShort( searchEnemy );
			SearchingEnemy.ImageAlign = searchEnemy > 0 ? ContentAlignment.MiddleLeft : ContentAlignment.MiddleCenter;
			SearchingEnemy.ImageIndex = searchEnemy > 0 ? (int)( searchEnemy < 4 ? ResourceManager.EquipmentContent.Seaplane : ResourceManager.EquipmentContent.Radar ) : -1;
			ToolTipInfo.SetToolTip( SearchingEnemy, null );

		}

		/// <summary>
		/// 索敵結果をクリアします。
		/// 索敵フェーズが発生しなかった場合にこれを設定します。
		/// </summary>
		private void ClearSearchingResult() {

			SearchingFriend.Text = "-";
			SearchingFriend.ImageAlign = ContentAlignment.MiddleCenter;
			SearchingFriend.ImageIndex = -1;
			ToolTipInfo.SetToolTip( SearchingFriend, null );

			SearchingEnemy.Text = "-";
			SearchingEnemy.ImageAlign = ContentAlignment.MiddleCenter;
			SearchingEnemy.ImageIndex = -1;
			ToolTipInfo.SetToolTip( SearchingEnemy, null );

		}

		/// <summary>
		/// 基地航空隊フェーズの結果を設定します。
		/// </summary>
		private void SetBaseAirAttack( PhaseBaseAirAttack pd ) {

			if ( pd != null && pd.IsAvailable ) {

				Searching.Text = "基地航空隊";
				Searching.ImageAlign = ContentAlignment.MiddleLeft;
				Searching.ImageIndex = (int)ResourceManager.EquipmentContent.LandAttacker;

				var sb = new StringBuilder();
				int index = 1;

				foreach ( var phase in pd.AirAttackUnits ) {

					sb.AppendFormat( "{0} 回目 - #{1} :\r\n",
						index, phase.AirUnitID );

					if ( phase.IsStage1Available ) {
						sb.AppendFormat( "　St1: 自軍 -{0}/{1} | 敵軍 -{2}/{3} | {4}\r\n",
							phase.AircraftLostStage1Friend, phase.AircraftTotalStage1Friend,
							phase.AircraftLostStage1Enemy, phase.AircraftTotalStage1Enemy,
							Constants.GetAirSuperiority( phase.AirSuperiority ) );
					}
					if ( phase.IsStage2Available ) {
						sb.AppendFormat( "　St2: 自軍 -{0}/{1} | 敵軍 -{2}/{3}\r\n",
							phase.AircraftLostStage2Friend, phase.AircraftTotalStage2Friend,
							phase.AircraftLostStage2Enemy, phase.AircraftTotalStage2Enemy );
					}

					index++;
				}

				ToolTipInfo.SetToolTip( Searching, sb.ToString() );


			} else {
				ClearBaseAirAttack();
			}

		}

		/// <summary>
		/// 基地航空隊フェーズの結果をクリアします。
		/// </summary>
		private void ClearBaseAirAttack() {
			Searching.Text = "索敵";
			Searching.ImageAlign = ContentAlignment.MiddleCenter;
			Searching.ImageIndex = -1;
			ToolTipInfo.SetToolTip( Searching, null );
		}


		/// <summary>
		/// 航空戦情報を設定します。
		/// </summary>
		private void SetAerialWarfare( PhaseAirBattleBase pd ) {

			//空対空戦闘
			if ( pd.IsStage1Available ) {

				AirSuperiority.Text = Constants.GetAirSuperiority( pd.AirSuperiority );

				int[] planeFriend = { pd.AircraftLostStage1Friend, pd.AircraftTotalStage1Friend };
				AirStage1Friend.Text = string.Format( "-{0}/{1}", planeFriend[0], planeFriend[1] );

				if ( planeFriend[1] > 0 && planeFriend[0] == planeFriend[1] )
					AirStage1Friend.ForeColor = Color.Red;
				else
					AirStage1Friend.ForeColor = SystemColors.ControlText;

				int[] planeEnemy = { pd.AircraftLostStage1Enemy, pd.AircraftTotalStage1Enemy };
				AirStage1Enemy.Text = string.Format( "-{0}/{1}", planeEnemy[0], planeEnemy[1] );

				if ( planeEnemy[1] > 0 && planeEnemy[0] == planeEnemy[1] )
					AirStage1Enemy.ForeColor = Color.Red;
				else
					AirStage1Enemy.ForeColor = SystemColors.ControlText;


				//触接
				int touchFriend = pd.TouchAircraftFriend;
				if ( touchFriend != -1 ) {
					AirStage1Friend.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage1Friend.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
					ToolTipInfo.SetToolTip( AirStage1Friend, "触接中: " + KCDatabase.Instance.MasterEquipments[touchFriend].Name );
				} else {
					AirStage1Friend.ImageAlign = ContentAlignment.MiddleCenter;
					AirStage1Friend.ImageIndex = -1;
					ToolTipInfo.SetToolTip( AirStage1Friend, null );
				}

				int touchEnemy = pd.TouchAircraftEnemy;
				if ( touchEnemy != -1 ) {
					AirStage1Enemy.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage1Enemy.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
					ToolTipInfo.SetToolTip( AirStage1Enemy, "触接中: " + KCDatabase.Instance.MasterEquipments[touchEnemy].Name );
				} else {
					AirStage1Enemy.ImageAlign = ContentAlignment.MiddleCenter;
					AirStage1Enemy.ImageIndex = -1;
					ToolTipInfo.SetToolTip( AirStage1Enemy, null );
				}

			} else {		//空対空戦闘発生せず

				AirSuperiority.Text = Constants.GetAirSuperiority( -1 );

				AirStage1Friend.Text = "-";
				AirStage1Friend.ForeColor = SystemColors.ControlText;
				AirStage1Friend.ImageAlign = ContentAlignment.MiddleCenter;
				AirStage1Friend.ImageIndex = -1;
				ToolTipInfo.SetToolTip( AirStage1Friend, null );

				AirStage1Enemy.Text = "-";
				AirStage1Enemy.ForeColor = SystemColors.ControlText;
				AirStage1Enemy.ImageAlign = ContentAlignment.MiddleCenter;
				AirStage1Enemy.ImageIndex = -1;
				ToolTipInfo.SetToolTip( AirStage1Enemy, null );
			}

			//艦対空戦闘
			if ( pd.IsStage2Available ) {

				int[] planeFriend = { pd.AircraftLostStage2Friend, pd.AircraftTotalStage2Friend };
				AirStage2Friend.Text = string.Format( "-{0}/{1}", planeFriend[0], planeFriend[1] );

				if ( planeFriend[1] > 0 && planeFriend[0] == planeFriend[1] )
					AirStage2Friend.ForeColor = Color.Red;
				else
					AirStage2Friend.ForeColor = SystemColors.ControlText;

				int[] planeEnemy = { pd.AircraftLostStage2Enemy, pd.AircraftTotalStage2Enemy };
				AirStage2Enemy.Text = string.Format( "-{0}/{1}", planeEnemy[0], planeEnemy[1] );

				if ( planeEnemy[1] > 0 && planeEnemy[0] == planeEnemy[1] )
					AirStage2Enemy.ForeColor = Color.Red;
				else
					AirStage2Enemy.ForeColor = SystemColors.ControlText;


				//対空カットイン
				if ( pd.IsAACutinAvailable ) {
					int cutinID = pd.AACutInKind;
					int cutinIndex = pd.AACutInIndex;

					AACutin.Text = "#" + ( cutinIndex + 1 );
					AACutin.ImageAlign = ContentAlignment.MiddleLeft;
					AACutin.ImageIndex = (int)ResourceManager.EquipmentContent.HighAngleGun;
					ToolTipInfo.SetToolTip( AACutin, string.Format(
						"対空カットイン: {0}\r\nカットイン種別: {1} ({2})",
						pd.AACutInShip.NameWithLevel,
						cutinID,
						Constants.GetAACutinKind( cutinID ) ) );

				} else {
					AACutin.Text = "対空砲火";
					AACutin.ImageAlign = ContentAlignment.MiddleCenter;
					AACutin.ImageIndex = -1;
					ToolTipInfo.SetToolTip( AACutin, null );
				}

			} else {	//艦対空戦闘発生せず
				AirStage2Friend.Text = "-";
				AirStage2Friend.ForeColor = SystemColors.ControlText;
				AirStage2Enemy.Text = "-";
				AirStage2Enemy.ForeColor = SystemColors.ControlText;
				AACutin.Text = "対空砲火";
				AACutin.ImageAlign = ContentAlignment.MiddleCenter;
				AACutin.ImageIndex = -1;
				ToolTipInfo.SetToolTip( AACutin, null );
			}


			AirStage2Friend.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage2Friend.ImageIndex = -1;
			ToolTipInfo.SetToolTip( AirStage2Friend, null );
			AirStage2Enemy.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage2Enemy.ImageIndex = -1;
			ToolTipInfo.SetToolTip( AirStage2Enemy, null );

		}


		/// <summary>
		/// 航空戦情報(航空戦)を設定します。
		/// 通常艦隊・連合艦隊両用です。
		/// </summary>
		private void SetAerialWarfareAirBattle( PhaseAirBattle pd1, PhaseAirBattle pd2 ) {

			//空対空戦闘
			if ( pd1.IsStage1Available ) {

				//二回目の空戦が存在するか
				bool isBattle2Enabled = pd2.IsStage1Available;

				AirSuperiority.Text = Constants.GetAirSuperiority( pd1.AirSuperiority );
				if ( isBattle2Enabled ) {
					ToolTipInfo.SetToolTip( AirSuperiority, "第2次: " + Constants.GetAirSuperiority( pd2.AirSuperiority ) );
				} else {
					ToolTipInfo.SetToolTip( AirSuperiority, null );
				}


				int[] planeFriend = {
					pd1.AircraftLostStage1Friend,
					pd1.AircraftTotalStage1Friend,
					( isBattle2Enabled ? pd2.AircraftLostStage1Friend : 0 ),
					( isBattle2Enabled ? pd2.AircraftTotalStage1Friend : 0 ),
				};
				AirStage1Friend.Text = string.Format( "-{0}/{1}", planeFriend[0] + planeFriend[2], planeFriend[1] );
				ToolTipInfo.SetToolTip( AirStage1Friend, string.Format( "第1次: -{0}/{1}\r\n第2次: -{2}/{3}\r\n",
					planeFriend[0], planeFriend[1], planeFriend[2], planeFriend[3] ) );

				if ( ( planeFriend[1] > 0 && planeFriend[0] == planeFriend[1] ) ||
					 ( planeFriend[3] > 0 && planeFriend[2] == planeFriend[3] ) )
					AirStage1Friend.ForeColor = Color.Red;
				else
					AirStage1Friend.ForeColor = SystemColors.ControlText;


				int[] planeEnemy = { 
					pd1.AircraftLostStage1Enemy,
					pd1.AircraftTotalStage1Enemy,
					( isBattle2Enabled ? pd2.AircraftLostStage1Enemy : 0 ),
					( isBattle2Enabled ? pd2.AircraftTotalStage1Enemy : 0 ),
				};
				AirStage1Enemy.Text = string.Format( "-{0}/{1}", planeEnemy[0] + planeEnemy[2], planeEnemy[1] );
				ToolTipInfo.SetToolTip( AirStage1Enemy, string.Format( "第1次: -{0}/{1}\r\n第2次: -{2}/{3}\r\n",
					planeEnemy[0], planeEnemy[1], planeEnemy[2], planeEnemy[3] ) );

				if ( ( planeEnemy[1] > 0 && planeEnemy[0] == planeEnemy[1] ) ||
					 ( planeEnemy[3] > 0 && planeEnemy[2] == planeEnemy[3] ) )
					AirStage1Enemy.ForeColor = Color.Red;
				else
					AirStage1Enemy.ForeColor = SystemColors.ControlText;


				//触接
				int[] touchFriend = { 
					pd1.TouchAircraftFriend,
					isBattle2Enabled ? pd2.TouchAircraftFriend : -1
					};
				if ( touchFriend[0] != -1 || touchFriend[1] != -1 ) {
					AirStage1Friend.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage1Friend.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;

					EquipmentDataMaster[] planes = { KCDatabase.Instance.MasterEquipments[touchFriend[0]], KCDatabase.Instance.MasterEquipments[touchFriend[1]] };
					ToolTipInfo.SetToolTip( AirStage1Friend, string.Format(
						"{0}触接中\r\n第1次: {1}\r\n第2次: {2}",
						ToolTipInfo.GetToolTip( AirStage1Friend ) ?? "",
						planes[0] != null ? planes[0].Name : "(なし)",
						planes[1] != null ? planes[1].Name : "(なし)"
						) );
				} else {
					AirStage1Friend.ImageAlign = ContentAlignment.MiddleCenter;
					AirStage1Friend.ImageIndex = -1;
					//ToolTipInfo.SetToolTip( AirStage1Friend, null );
				}

				int[] touchEnemy = {
					pd1.TouchAircraftEnemy,
					isBattle2Enabled ? pd2.TouchAircraftEnemy : -1
					};
				if ( touchEnemy[0] != -1 || touchEnemy[1] != -1 ) {
					AirStage1Enemy.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage1Enemy.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;

					EquipmentDataMaster[] planes = { KCDatabase.Instance.MasterEquipments[touchEnemy[0]], KCDatabase.Instance.MasterEquipments[touchEnemy[1]] };
					ToolTipInfo.SetToolTip( AirStage1Enemy, string.Format(
						"{0}触接中\r\n第1次: {1}\r\n第2次: {2}",
						ToolTipInfo.GetToolTip( AirStage1Enemy ) ?? "",
						planes[0] != null ? planes[0].Name : "(なし)",
						planes[1] != null ? planes[1].Name : "(なし)"
						) );
				} else {
					AirStage1Enemy.ImageAlign = ContentAlignment.MiddleCenter;
					AirStage1Enemy.ImageIndex = -1;
					//ToolTipInfo.SetToolTip( AirStage1Enemy, null );
				}

			} else {	//空対空戦闘発生せず(!?)
				AirSuperiority.Text = Constants.GetAirSuperiority( -1 );
				ToolTipInfo.SetToolTip( AirSuperiority, null );
				AirStage1Friend.Text = "-";
				AirStage1Friend.ForeColor = SystemColors.ControlText;
				ToolTipInfo.SetToolTip( AirStage1Friend, null );
				AirStage1Enemy.Text = "-";
				AirStage1Enemy.ForeColor = SystemColors.ControlText;
				ToolTipInfo.SetToolTip( AirStage1Enemy, null );
			}

			//艦対空戦闘
			if ( pd1.IsStage2Available ) {

				//二回目の空戦が存在するか
				bool isBattle2Enabled = pd2.IsStage2Available;


				int[] planeFriend = { 
					pd1.AircraftLostStage2Friend,
					pd1.AircraftTotalStage2Friend,
					( isBattle2Enabled ? pd2.AircraftLostStage2Friend : 0 ),
					( isBattle2Enabled ? pd2.AircraftTotalStage2Friend : 0 ),
				};
				AirStage2Friend.Text = string.Format( "-{0}/{1}", planeFriend[0] + planeFriend[2], planeFriend[1] );
				ToolTipInfo.SetToolTip( AirStage2Friend, string.Format( "第1次: -{0}/{1}\r\n第2次: -{2}/{3}\r\n",
					planeFriend[0], planeFriend[1], planeFriend[2], planeFriend[3] ) );

				if ( ( planeFriend[1] > 0 && planeFriend[0] == planeFriend[1] ) ||
					 ( planeFriend[3] > 0 && planeFriend[2] == planeFriend[3] ) )
					AirStage2Friend.ForeColor = Color.Red;
				else
					AirStage2Friend.ForeColor = SystemColors.ControlText;


				int[] planeEnemy = { 
					pd1.AircraftLostStage2Enemy,
					pd1.AircraftTotalStage2Enemy,
					( isBattle2Enabled ? pd2.AircraftLostStage2Enemy : 0 ),
					( isBattle2Enabled ? pd2.AircraftTotalStage2Enemy : 0 ),
				};
				AirStage2Enemy.Text = string.Format( "-{0}/{1}", planeEnemy[0] + planeEnemy[2], planeEnemy[1] );
				ToolTipInfo.SetToolTip( AirStage2Enemy, string.Format( "第1次: -{0}/{1}\r\n第2次: -{2}/{3}\r\n{4}",
					planeEnemy[0], planeEnemy[1], planeEnemy[2], planeEnemy[3],
					isBattle2Enabled ? "" : "(第二次戦発生せず)" ) );			//DEBUG

				if ( ( planeEnemy[1] > 0 && planeEnemy[0] == planeEnemy[1] ) ||
					 ( planeEnemy[3] > 0 && planeEnemy[2] == planeEnemy[3] ) )
					AirStage2Enemy.ForeColor = Color.Red;
				else
					AirStage2Enemy.ForeColor = SystemColors.ControlText;


				//対空カットイン
				{
					bool[] fire = new bool[] { pd1.IsAACutinAvailable, isBattle2Enabled && pd2.IsAACutinAvailable };
					int[] cutinID = new int[] {
						fire[0] ? pd1.AACutInKind : -1,
						fire[1] ? pd2.AACutInKind : -1,
					};
					int[] cutinIndex = new int[] {
						fire[0] ? pd1.AACutInIndex : -1,
						fire[1] ? pd2.AACutInIndex : -1,
					};

					if ( fire[0] || fire[1] ) {

						AACutin.Text = string.Format( "#{0}/{1}", fire[0] ? ( cutinIndex[0] + 1 ).ToString() : "-", fire[1] ? ( cutinIndex[1] + 1 ).ToString() : "-" );
						AACutin.ImageAlign = ContentAlignment.MiddleLeft;
						AACutin.ImageIndex = (int)ResourceManager.EquipmentContent.HighAngleGun;

						StringBuilder sb = new StringBuilder();
						sb.AppendLine( "対空カットイン" );
						for ( int i = 0; i < 2; i++ ) {
							if ( fire[i] ) {
								sb.AppendFormat( "第{0}次: {1}\r\nカットイン種別: {2} ({3})\r\n",
									i + 1,
									( i == 0 ? pd1 : pd2 ).AACutInShip.NameWithLevel,
									cutinID[i],
									Constants.GetAACutinKind( cutinID[i] ) );
							} else {
								sb.AppendFormat( "第{0}次: (発動せず)\r\n",
									i + 1 );
							}
						}
						ToolTipInfo.SetToolTip( AACutin, sb.ToString() );

					} else {
						AACutin.Text = "対空砲火";
						AACutin.ImageAlign = ContentAlignment.MiddleCenter;
						AACutin.ImageIndex = -1;
						ToolTipInfo.SetToolTip( AACutin, null );
					}
				}

			} else {	//艦対空戦闘発生せず
				AirStage2Friend.Text = "-";
				AirStage2Friend.ForeColor = SystemColors.ControlText;
				ToolTipInfo.SetToolTip( AirStage2Friend, null );
				AirStage2Enemy.Text = "-";
				AirStage2Enemy.ForeColor = SystemColors.ControlText;
				ToolTipInfo.SetToolTip( AirStage2Enemy, null );
				AACutin.Text = "対空砲火";
				AACutin.ImageAlign = ContentAlignment.MiddleCenter;
				AACutin.ImageIndex = -1;
				ToolTipInfo.SetToolTip( AACutin, null );
			}

			AirStage2Friend.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage2Friend.ImageIndex = -1;
			AirStage2Enemy.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage2Enemy.ImageIndex = -1;

		}


		/// <summary>
		/// 航空戦情報をクリアします。
		/// </summary>
		private void ClearAerialWarfare() {
			AirSuperiority.Text = "-";
			ToolTipInfo.SetToolTip( AirSuperiority, null );

			AirStage1Friend.Text = "-";
			AirStage1Friend.ForeColor = SystemColors.ControlText;
			AirStage1Friend.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage1Friend.ImageIndex = -1;
			ToolTipInfo.SetToolTip( AirStage1Friend, null );

			AirStage1Enemy.Text = "-";
			AirStage1Enemy.ForeColor = SystemColors.ControlText;
			AirStage1Enemy.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage1Enemy.ImageIndex = -1;
			ToolTipInfo.SetToolTip( AirStage1Enemy, null );

			AirStage2Friend.Text = "-";
			AirStage2Friend.ForeColor = SystemColors.ControlText;
			AirStage2Friend.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage2Friend.ImageIndex = -1;
			ToolTipInfo.SetToolTip( AirStage2Friend, null );

			AirStage2Enemy.Text = "-";
			AirStage2Enemy.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage2Enemy.ForeColor = SystemColors.ControlText;
			AirStage2Enemy.ImageIndex = -1;
			ToolTipInfo.SetToolTip( AirStage2Enemy, null );

			AACutin.Text = "-";
			AACutin.ImageAlign = ContentAlignment.MiddleCenter;
			AACutin.ImageIndex = -1;
			ToolTipInfo.SetToolTip( AACutin, null );
		}


		/// <summary>
		/// 両軍のHPゲージを設定します。
		/// </summary>
		private void SetHPBar( BattleData bd ) {

			KCDatabase db = KCDatabase.Instance;
			bool isPractice = ( bd.BattleType & BattleData.BattleTypeFlag.Practice ) != 0;
			bool isCombined = ( bd.BattleType & BattleData.BattleTypeFlag.Combined ) != 0;
			bool isEnemyCombined = ( bd.BattleType & BattleData.BattleTypeFlag.EnemyCombined ) != 0;
			bool isBaseAirRaid = ( bd.BattleType & BattleData.BattleTypeFlag.BaseAirRaid ) != 0;

			var initialHPs = bd.Initial.InitialHPs;
			var maxHPs = bd.Initial.MaxHPs;
			var resultHPs = bd.ResultHPs;
			var attackDamages = bd.AttackDamages;


			foreach ( var bar in HPBars )
				bar.SuspendUpdate();

			for ( int i = 0; i < 24; i++ ) {

				if ( initialHPs[i] != -1 ) {
					HPBars[i].Value = resultHPs[i];
					HPBars[i].PrevValue = initialHPs[i];
					HPBars[i].MaximumValue = maxHPs[i];
					HPBars[i].BackColor = SystemColors.Control;
					HPBars[i].Visible = true;
				} else {
					HPBars[i].Visible = false;
				}
			}


			// friend main
			for ( int i = 0; i < 6; i++ ) {
				if ( initialHPs[i] != -1 ) {
					string name;
					bool isEscaped;
					bool isLandBase;

					if ( isBaseAirRaid ) {
						name = string.Format( "第{0}基地", i + 1 );
						isEscaped = false;
						isLandBase = true;
					} else {
						ShipData ship = bd.Initial.FriendFleet.MembersInstance[i];
						name = string.Format( "{0} Lv. {1}", ship.MasterShip.NameWithClass, ship.Level );
						isEscaped = bd.Initial.FriendFleet.EscapedShipList.Contains( ship.MasterID );
						isLandBase = ship.MasterShip.IsLandBase;
					}

					ToolTipInfo.SetToolTip( HPBars[i], string.Format
						( "{0}\r\nHP: ({1} → {2})/{3} ({4}) [{5}]\r\n与ダメージ: {6}\r\n\r\n{7}",
						name,
						Math.Max( HPBars[i].PrevValue, 0 ),
						Math.Max( HPBars[i].Value, 0 ),
						HPBars[i].MaximumValue,
						HPBars[i].Value - HPBars[i].PrevValue,
						Constants.GetDamageState( (double)HPBars[i].Value / HPBars[i].MaximumValue, isPractice, isLandBase, isEscaped ),
						attackDamages[i],
						bd.GetBattleDetail( i )
						) );

					if ( isEscaped ) HPBars[i].BackColor = Color.Silver;
					else HPBars[i].BackColor = SystemColors.Control;
				}
			}


			// enemy main
			for ( int i = 0; i < 6; i++ ) {
				if ( initialHPs[i + 6] != -1 ) {
					ShipDataMaster ship = bd.Initial.EnemyMembersInstance[i];

					ToolTipInfo.SetToolTip( HPBars[i + 6],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]\r\n\r\n{7}",
							ship.NameWithClass,
							bd.Initial.EnemyLevels[i],
							Math.Max( HPBars[i + 6].PrevValue, 0 ),
							Math.Max( HPBars[i + 6].Value, 0 ),
							HPBars[i + 6].MaximumValue,
							HPBars[i + 6].Value - HPBars[i + 6].PrevValue,
							Constants.GetDamageState( (double)HPBars[i + 6].Value / HPBars[i + 6].MaximumValue, isPractice, ship.IsLandBase ),
							bd.GetBattleDetail( i + 6 )
							)
						);
				}
			}


			// friend escort
			if ( isCombined ) {
				FleetFriendEscort.Visible = true;

				for ( int i = 0; i < 6; i++ ) {
					if ( initialHPs[i + 12] != -1 ) {
						ShipData ship = bd.Initial.FriendFleetEscort.MembersInstance[i];
						bool isEscaped = bd.Initial.FriendFleetEscort.EscapedShipList.Contains( ship.MasterID );

						ToolTipInfo.SetToolTip( HPBars[i + 12], string.Format(
							"{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]\r\n与ダメージ: {7}\r\n\r\n{8}",
							ship.MasterShip.NameWithClass,
							ship.Level,
							Math.Max( HPBars[i + 12].PrevValue, 0 ),
							Math.Max( HPBars[i + 12].Value, 0 ),
							HPBars[i + 12].MaximumValue,
							HPBars[i + 12].Value - HPBars[i + 12].PrevValue,
							Constants.GetDamageState( (double)HPBars[i + 12].Value / HPBars[i + 12].MaximumValue, isPractice, ship.MasterShip.IsLandBase, isEscaped ),
							attackDamages[i + 12],
							bd.GetBattleDetail( i + 12 )
							) );

						if ( isEscaped ) HPBars[i + 12].BackColor = Color.Silver;
						else HPBars[i + 12].BackColor = SystemColors.Control;
					}
				}

			} else {
				FleetFriendEscort.Visible = false;
			}


			// enemy escort
			if ( isEnemyCombined ) {
				FleetEnemyEscort.Visible = true;

				for ( int i = 0; i < 6; i++ ) {
					if ( initialHPs[i + 18] != -1 ) {
						ShipDataMaster ship = bd.Initial.EnemyMembersEscortInstance[i];

						var bar = HPBars[i + 18];

						ToolTipInfo.SetToolTip( bar,
							string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]\r\n\r\n{7}",
								ship.NameWithClass,
								bd.Initial.EnemyLevelsEscort[i],
								Math.Max( bar.PrevValue, 0 ),
								Math.Max( bar.Value, 0 ),
								bar.MaximumValue,
								bar.Value - bar.PrevValue,
								Constants.GetDamageState( (double)bar.Value / bar.MaximumValue, isPractice, ship.IsLandBase ),
								bd.GetBattleDetail( i + 18 )
								)
							);
					}
				}

			} else {
				FleetEnemyEscort.Visible = false;
			}


			//*/
			if ( isCombined && isEnemyCombined ) {
				foreach ( var bar in HPBars ) {
					bar.Size = SmallBarSize;
					bar.Text = null;
				}
			} else {
				foreach ( var bar in HPBars ) {
					bar.Size = DefaultBarSize;
					bar.Text = "HP:";
				}
			}
			//*/


			if ( bd.Initial.IsBossDamaged )
				HPBars[6].BackColor = Color.MistyRose;

			if ( !isBaseAirRaid ) {
				foreach ( int i in bd.MVPShipIndexes )
					HPBars[i].BackColor = Color.Moccasin;
				foreach ( int i in bd.MVPShipCombinedIndexes )
					HPBars[12 + i].BackColor = Color.Moccasin;
			}

			foreach ( var bar in HPBars )
				bar.ResumeUpdate();
		}



		/// <summary>
		/// 損害率と戦績予測を設定します。
		/// </summary>
		private void SetDamageRate( BattleManager bm ) {

			double friendrate, enemyrate;
			int rank = bm.PredictWinRank( out friendrate, out enemyrate );

			DamageFriend.Text = friendrate.ToString( "p1" );
			DamageEnemy.Text = enemyrate.ToString( "p1" );

			if ( bm.IsBaseAirRaid ) {
				int kind = bm.Compass.AirRaidDamageKind;
				WinRank.Text = Constants.GetAirRaidDamageShort( kind );
				WinRank.ForeColor = ( 1 <= kind && kind <= 3 ) ? WinRankColor_Lose : WinRankColor_Win;
			} else {
				WinRank.Text = Constants.GetWinRank( rank );
				WinRank.ForeColor = rank >= 4 ? WinRankColor_Win : WinRankColor_Lose;
			}

			WinRank.MinimumSize = Utility.Configuration.Config.UI.IsLayoutFixed ? new Size( DefaultBarSize.Width, 0 ) : new Size( HPBars[0].Width, 0 );
		}


		/// <summary>
		/// 夜戦における各種表示を設定します。
		/// </summary>
		/// <param name="hp">戦闘開始前のHP。</param>
		/// <param name="isCombined">連合艦隊かどうか。</param>
		/// <param name="bd">戦闘データ。</param>
		private void SetNightBattleEvent( PhaseNightBattle pd ) {

			FleetData fleet = pd.FriendFleet;

			//味方探照灯判定
			{
				int index = pd.SearchlightIndexFriend;

				if ( index != -1 ) {
					ShipData ship = fleet.MembersInstance[index];

					AirStage1Friend.Text = "#" + ( index + 1 );
					AirStage1Friend.ForeColor = SystemColors.ControlText;
					AirStage1Friend.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage1Friend.ImageIndex = (int)ResourceManager.EquipmentContent.Searchlight;
					ToolTipInfo.SetToolTip( AirStage1Friend, "探照灯照射: " + ship.NameWithLevel );
				} else {
					ToolTipInfo.SetToolTip( AirStage1Friend, null );
				}
			}

			//敵探照灯判定
			{
				int index = pd.SearchlightIndexEnemy;
				if ( index != -1 ) {
					AirStage1Enemy.Text = "#" + ( index + 1 );
					AirStage1Enemy.ForeColor = SystemColors.ControlText;
					AirStage1Enemy.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage1Enemy.ImageIndex = (int)ResourceManager.EquipmentContent.Searchlight;
					ToolTipInfo.SetToolTip( AirStage1Enemy, "探照灯照射: " + pd.SearchlightEnemyInstance.NameWithClass );
				} else {
					ToolTipInfo.SetToolTip( AirStage1Enemy, null );
				}
			}


			//夜間触接判定
			if ( pd.TouchAircraftFriend != -1 ) {
				SearchingFriend.Text = "夜間触接";
				SearchingFriend.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
				SearchingFriend.ImageAlign = ContentAlignment.MiddleLeft;
				ToolTipInfo.SetToolTip( SearchingFriend, "夜間触接中: " + KCDatabase.Instance.MasterEquipments[pd.TouchAircraftFriend].Name );
			} else {
				ToolTipInfo.SetToolTip( SearchingFriend, null );
			}

			if ( pd.TouchAircraftEnemy != -1 ) {
				SearchingEnemy.Text = "夜間触接";
				SearchingEnemy.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
				SearchingFriend.ImageAlign = ContentAlignment.MiddleLeft;
				ToolTipInfo.SetToolTip( SearchingEnemy, "夜間触接中: " + KCDatabase.Instance.MasterEquipments[pd.TouchAircraftEnemy].Name );
			} else {
				ToolTipInfo.SetToolTip( SearchingEnemy, null );
			}

			//照明弾投射判定
			{
				int index = pd.FlareIndexFriend;

				if ( index != -1 ) {
					AirStage2Friend.Text = "#" + ( index + 1 );
					AirStage2Friend.ForeColor = SystemColors.ControlText;
					AirStage2Friend.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage2Friend.ImageIndex = (int)ResourceManager.EquipmentContent.Flare;
					ToolTipInfo.SetToolTip( AirStage2Friend, "照明弾投射: " + fleet.MembersInstance[index].NameWithLevel );

				} else {
					ToolTipInfo.SetToolTip( AirStage2Friend, null );
				}
			}

			{
				int index = pd.FlareIndexEnemy;

				if ( index != -1 ) {
					AirStage2Enemy.Text = "#" + ( index + 1 );
					AirStage2Enemy.ForeColor = SystemColors.ControlText;
					AirStage2Enemy.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage2Enemy.ImageIndex = (int)ResourceManager.EquipmentContent.Flare;
					ToolTipInfo.SetToolTip( AirStage2Enemy, "照明弾投射: " + pd.FlareEnemyInstance.NameWithClass );
				} else {
					ToolTipInfo.SetToolTip( AirStage2Enemy, null );
				}
			}
		}


		/// <summary>
		/// 戦闘終了後に、MVP艦の表示を更新します。
		/// </summary>
		/// <param name="bm">戦闘データ。</param>
		private void SetMVPShip( BattleManager bm ) {

			bool isCombined = bm.IsCombinedBattle;

			var bd = bm.StartsFromDayBattle ? (BattleData)bm.BattleDay : (BattleData)bm.BattleNight;
			var br = bm.Result;

			var friend = bd.Initial.FriendFleet;
			var escort = !isCombined ? null : bd.Initial.FriendFleetEscort;


			/*// DEBUG
			{
				BattleData lastbattle = bm.StartsFromDayBattle ? (BattleData)bm.BattleNight ?? bm.BattleDay : (BattleData)bm.BattleDay ?? bm.BattleNight;
				if ( lastbattle.MVPShipIndexes.Count() > 1 || !lastbattle.MVPShipIndexes.Contains( br.MVPIndex - 1 ) ) {
					Utility.Logger.Add( 1, "MVP is wrong : [" + string.Join( ",", lastbattle.MVPShipIndexes ) + "] => " + ( br.MVPIndex - 1 ) );
				}
				if ( isCombined && ( lastbattle.MVPShipCombinedIndexes.Count() > 1 || !lastbattle.MVPShipCombinedIndexes.Contains( br.MVPIndexCombined - 1 ) ) ) {
					Utility.Logger.Add( 1, "MVP is wrong (escort) : [" + string.Join( ",", lastbattle.MVPShipCombinedIndexes ) + "] => " + ( br.MVPIndexCombined - 1 ) );
				}
			}
			//*/


			for ( int i = 0; i < 6; i++ ) {
				if ( friend.EscapedShipList.Contains( friend.Members[i] ) ) {
					HPBars[i].BackColor = Color.Silver;

				} else if ( br.MVPIndex == i + 1 ) {
					HPBars[i].BackColor = Color.Moccasin;

				} else {
					HPBars[i].BackColor = SystemColors.Control;
				}

				if ( escort != null ) {
					if ( escort.EscapedShipList.Contains( escort.Members[i] ) ) {
						HPBars[i + 12].BackColor = Color.Silver;

					} else if ( br.MVPIndexCombined == i + 1 ) {
						HPBars[i + 12].BackColor = Color.Moccasin;

					} else {
						HPBars[i + 12].BackColor = SystemColors.Control;
					}
				}
			}

			/*// debug
			if ( WinRank.Text.First().ToString() != bm.Result.Rank ) {
				Utility.Logger.Add( 1, string.Format( "戦闘評価予測が誤っています。(予測: {0}, 実際: {1})", WinRank.Text.First().ToString(), bm.Result.Rank ) );
			}
			//*/

		}


		private void RightClickMenu_Opening( object sender, CancelEventArgs e ) {

			var bm = KCDatabase.Instance.Battle;

			if ( bm == null || bm.BattleMode == BattleManager.BattleModes.Undefined )
				e.Cancel = true;

			RightClickMenu_ShowBattleResult.Enabled = !BaseLayoutPanel.Visible;
		}

		private void RightClickMenu_ShowBattleDetail_Click( object sender, EventArgs e ) {
			var bm = KCDatabase.Instance.Battle;

			if ( bm == null || bm.BattleMode == BattleManager.BattleModes.Undefined )
				return;

			var dialog = new Dialog.DialogBattleDetail();

			dialog.BattleDetailText = BattleDetailDescriptor.GetBattleDetail( bm );
			dialog.Location = RightClickMenu.Location;
			dialog.Show( this );

		}

		private void RightClickMenu_ShowBattleResult_Click( object sender, EventArgs e ) {
			BaseLayoutPanel.Visible = true;
		}




		void ConfigurationChanged() {

			var config = Utility.Configuration.Config;

			MainFont = TableTop.Font = TableBottom.Font = Font = config.UI.MainFont;
			SubFont = config.UI.SubFont;

			BaseLayoutPanel.AutoScroll = config.FormBattle.IsScrollable;


			bool fixSize = config.UI.IsLayoutFixed;
			bool showHPBar = config.FormBattle.ShowHPBar;

			TableBottom.SuspendLayout();
			if ( fixSize ) {
				ControlHelper.SetTableColumnStyles( TableBottom, new ColumnStyle( SizeType.AutoSize ) );
				ControlHelper.SetTableRowStyle( TableBottom, 0, new RowStyle( SizeType.Absolute, 21 ) );
				for ( int i = 1; i <= 6; i++ )
					ControlHelper.SetTableRowStyle( TableBottom, i, new RowStyle( SizeType.Absolute, showHPBar ? 21 : 16 ) );
				ControlHelper.SetTableRowStyle( TableBottom, 7, new RowStyle( SizeType.Absolute, 21 ) );
			} else {
				ControlHelper.SetTableColumnStyles( TableBottom, new ColumnStyle( SizeType.AutoSize ) );
				ControlHelper.SetTableRowStyles( TableBottom, new RowStyle( SizeType.AutoSize ) );
			}
			if ( HPBars != null ) {
				foreach ( var b in HPBars ) {
					b.MainFont = MainFont;
					b.SubFont = SubFont;
					b.AutoSize = !fixSize;
					if ( !b.AutoSize ) {
						b.Size = ( HPBars[12].Visible && HPBars[18].Visible ) ? SmallBarSize : DefaultBarSize;
					}
					b.HPBar.ColorMorphing = config.UI.BarColorMorphing;
					b.HPBar.SetBarColorScheme( config.UI.BarColorScheme.Select( col => col.ColorData ).ToArray() );
					b.ShowHPBar = showHPBar;
				}
			}
			FleetFriend.MaximumSize =
			FleetFriendEscort.MaximumSize =
			FleetEnemy.MaximumSize =
			FleetEnemyEscort.MaximumSize =
			DamageFriend.MaximumSize =
			DamageEnemy.MaximumSize =
			WinRank.MaximumSize =
				fixSize ? DefaultBarSize : Size.Empty;

			WinRank.MinimumSize = fixSize ? new Size( 80, 0 ) : new Size( HPBars[0].Width, 0 );

			TableBottom.ResumeLayout();

			TableTop.SuspendLayout();
			if ( fixSize ) {
				ControlHelper.SetTableColumnStyles( TableTop, new ColumnStyle( SizeType.Absolute, 21 * 4 ) );
				ControlHelper.SetTableRowStyles( TableTop, new RowStyle( SizeType.Absolute, 21 ) );
				TableTop.Width = TableTop.GetPreferredSize( BaseLayoutPanel.Size ).Width;
			} else {
				ControlHelper.SetTableColumnStyles( TableTop, new ColumnStyle( SizeType.Percent, 100 ) );
				ControlHelper.SetTableRowStyles( TableTop, new RowStyle( SizeType.AutoSize ) );
				TableTop.Width = TableBottom.ClientSize.Width;
			}
			TableTop.Height = TableTop.GetPreferredSize( BaseLayoutPanel.Size ).Height;
			TableTop.ResumeLayout();

		}



		private void TableTop_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			if ( e.Row == 1 || e.Row == 3 )
				e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}

		private void TableBottom_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			if ( e.Row == 7 )
				e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}


		protected override string GetPersistString() {
			return "Battle";
		}


	}

}
