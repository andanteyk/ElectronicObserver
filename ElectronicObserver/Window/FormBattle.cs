using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
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


		private List<ShipStatusHP> HPBars;

		public Font MainFont { get; set; }
		public Font SubFont { get; set; }



		public FormBattle( FormMain parent ) {
			InitializeComponent();

			ControlHelper.SetDoubleBuffered( TableTop );
			ControlHelper.SetDoubleBuffered( TableBottom );


			ConfigurationChanged();

			HPBars = new List<ShipStatusHP>( 18 );


			TableBottom.SuspendLayout();
			for ( int i = 0; i < 18; i++ ) {
				HPBars.Add( new ShipStatusHP() );
				HPBars[i].Size = new Size( 80, 20 );
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
					TableBottom.Controls.Add( HPBars[i], 2, i - 5 );
				} else {
					TableBottom.Controls.Add( HPBars[i], 1, i - 11 );
				}
			}
			TableBottom.ResumeLayout();


			SearchingFriend.ImageList =
			SearchingEnemy.ImageList =
			AACutin.ImageList =
			AirStage1Friend.ImageList =
			AirStage1Enemy.ImageList =
			AirStage2Friend.ImageList =
			AirStage2Enemy.ImageList =
				ResourceManager.Instance.Equipments;

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
			o.APIList["api_req_combined_battle/battle"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/midnight_battle"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/sp_midnight"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/airbattle"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/battle_water"].ResponseReceived += Updated;
			o.APIList["api_req_combined_battle/battleresult"].ResponseReceived += Updated;
			o.APIList["api_req_practice/battle"].ResponseReceived += Updated;
			o.APIList["api_req_practice/midnight_battle"].ResponseReceived += Updated;
			o.APIList["api_req_practice/battle_result"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		}


		private void Updated( string apiname, dynamic data ) {

			KCDatabase db = KCDatabase.Instance;
			BattleManager bm = db.Battle;

			BaseLayoutPanel.SuspendLayout();
			TableTop.SuspendLayout();
			TableBottom.SuspendLayout();
			switch ( apiname ) {

				case "api_req_map/start":
				case "api_req_map/next":
				case "api_port/port":
					BaseLayoutPanel.Visible = false;
					ToolTipInfo.RemoveAll();
					break;


				case "api_req_sortie/battle":
				case "api_req_practice/battle": {
						int[] hp = bm.BattleDay.EmulateBattle();

						SetFormation( bm.BattleDay );
						SetSearchingResult( bm.BattleDay );
						SetAerialWarfare( bm.BattleDay );
						SetHPNormal( hp, bm.BattleDay );
						SetDamageRateNormal( hp, bm.BattleDay );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_battle_midnight/battle":
				case "api_req_practice/midnight_battle": {
						int[] hp = bm.BattleNight.EmulateBattle();

						SetNightBattleEvent( bm.BattleNight.InitialHP.Skip( 1 ).ToArray(), false, bm.BattleNight );
						SetHPNormal( hp, bm.BattleNight );
						SetDamageRateNormal( hp, bm.BattleDay );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_battle_midnight/sp_midnight": {
						int[] hp = bm.BattleNight.EmulateBattle();

						SetFormation( bm.BattleNight );
						ClearAerialWarfare();
						ClearSearchingResult();
						SetNightBattleEvent( bm.BattleNight.InitialHP.Skip( 1 ).ToArray(), false, bm.BattleNight );
						SetHPNormal( hp, bm.BattleNight );
						SetDamageRateNormal( hp, bm.BattleNight );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_sortie/airbattle": {
						int[] hp = bm.BattleDay.EmulateBattle();

						SetFormation( bm.BattleDay );
						SetSearchingResult( bm.BattleDay );
						SetAerialWarfareAirBattle( bm.BattleDay );
						SetHPNormal( hp, bm.BattleDay );
						SetDamageRateNormal( hp, bm.BattleDay );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_combined_battle/battle":
				case "api_req_combined_battle/battle_water": {
						int[] hp = bm.BattleDay.EmulateBattle();

						SetFormation( bm.BattleDay );
						SetSearchingResult( bm.BattleDay );
						SetAerialWarfare( bm.BattleDay );
						SetHPCombined( hp, bm.BattleDay );
						SetDamageRateCombined( hp, bm.BattleDay );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_combined_battle/airbattle": {
						int[] hp = bm.BattleDay.EmulateBattle();

						SetFormation( bm.BattleDay );
						SetSearchingResult( bm.BattleDay );
						SetAerialWarfareAirBattle( bm.BattleDay );
						SetHPCombined( hp, bm.BattleDay );
						SetDamageRateCombined( hp, bm.BattleDay );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_combined_battle/midnight_battle": {
						int[] hp = bm.BattleNight.EmulateBattle();

						SetNightBattleEvent( bm.BattleNight.InitialHP.Skip( 1 ).ToArray(), true, bm.BattleNight );
						SetHPCombined( hp, bm.BattleNight );
						SetDamageRateCombined( hp, bm.BattleDay );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_combined_battle/sp_midnight": {
						int[] hp = bm.BattleNight.EmulateBattle();

						SetFormation( bm.BattleNight );
						ClearAerialWarfare();
						ClearSearchingResult();
						SetNightBattleEvent( bm.BattleNight.InitialHP.Skip( 1 ).ToArray(), true, bm.BattleNight );
						SetHPCombined( hp, bm.BattleNight );
						SetDamageRateCombined( hp, bm.BattleNight );

						BaseLayoutPanel.Visible = true;
					} break;


			}
			TableTop.ResumeLayout();
			TableBottom.ResumeLayout();
			BaseLayoutPanel.ResumeLayout();

		}


		/// <summary>
		/// 陣形・交戦形態を設定します。
		/// </summary>
		private void SetFormation( BattleData bd ) {

			FormationFriend.Text = Constants.GetFormationShort( bd.Data.api_formation[0] is string ? int.Parse( bd.Data.api_formation[0] ) : (int)bd.Data.api_formation[0] );
			FormationEnemy.Text = Constants.GetFormationShort( bd.Data.api_formation[1] is string ? int.Parse( bd.Data.api_formation[1] ) : (int)bd.Data.api_formation[1] );
			Formation.Text = Constants.GetEngagementForm( (int)bd.Data.api_formation[2] );

		}

		/// <summary>
		/// 索敵結果を設定します。
		/// </summary>
		private void SetSearchingResult( BattleData bd ) {

			int searchFriend = (int)bd.Data.api_search[0];
			SearchingFriend.Text = Constants.GetSearchingResultShort( searchFriend );
			SearchingFriend.ImageAlign = ContentAlignment.MiddleLeft;
			SearchingFriend.ImageIndex = (int)( searchFriend < 4 ? ResourceManager.EquipmentContent.Seaplane : ResourceManager.EquipmentContent.Radar );
			ToolTipInfo.SetToolTip( SearchingFriend, null );

			int searchEnemy = (int)bd.Data.api_search[1];
			SearchingEnemy.Text = Constants.GetSearchingResultShort( searchEnemy );
			SearchingEnemy.ImageAlign = ContentAlignment.MiddleLeft;
			SearchingEnemy.ImageIndex = (int)( searchEnemy < 4 ? ResourceManager.EquipmentContent.Seaplane : ResourceManager.EquipmentContent.Radar );
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
		/// 航空戦情報を設定します。
		/// </summary>
		private void SetAerialWarfare( BattleData bd ) {

			//空対空戦闘
			if ( (int)bd.Data.api_stage_flag[0] != 0 ) {

				AirSuperiority.Text = Constants.GetAirSuperiority( (int)bd.Data.api_kouku.api_stage1.api_disp_seiku );

				int[] planeFriend = { (int)bd.Data.api_kouku.api_stage1.api_f_lostcount, (int)bd.Data.api_kouku.api_stage1.api_f_count };
				AirStage1Friend.Text = string.Format( "-{0}/{1}", planeFriend[0], planeFriend[1] );

				if ( planeFriend[1] > 0 && planeFriend[0] == planeFriend[1] )
					AirStage1Friend.ForeColor = Color.Red;
				else
					AirStage1Friend.ForeColor = SystemColors.ControlText;

				int[] planeEnemy = { (int)bd.Data.api_kouku.api_stage1.api_e_lostcount, (int)bd.Data.api_kouku.api_stage1.api_e_count };
				AirStage1Enemy.Text = string.Format( "-{0}/{1}", planeEnemy[0], planeEnemy[1] );

				if ( planeEnemy[1] > 0 && planeEnemy[0] == planeEnemy[1] )
					AirStage1Enemy.ForeColor = Color.Red;
				else
					AirStage1Enemy.ForeColor = SystemColors.ControlText;


				//触接
				int touchFriend = (int)bd.Data.api_kouku.api_stage1.api_touch_plane[0];
				if ( touchFriend != -1 ) {
					AirStage1Friend.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage1Friend.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
					ToolTipInfo.SetToolTip( AirStage1Friend, "触接中: " + KCDatabase.Instance.MasterEquipments[touchFriend].Name );
				} else {
					AirStage1Friend.ImageAlign = ContentAlignment.MiddleCenter;
					AirStage1Friend.ImageIndex = -1;
					ToolTipInfo.SetToolTip( AirStage1Friend, null );
				}

				int touchEnemy = (int)bd.Data.api_kouku.api_stage1.api_touch_plane[1];
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
			if ( (int)bd.Data.api_stage_flag[1] != 0 ) {

				int[] planeFriend = { (int)bd.Data.api_kouku.api_stage2.api_f_lostcount, (int)bd.Data.api_kouku.api_stage2.api_f_count };
				AirStage2Friend.Text = string.Format( "-{0}/{1}", planeFriend[0], planeFriend[1] );

				if ( planeFriend[1] > 0 && planeFriend[0] == planeFriend[1] )
					AirStage2Friend.ForeColor = Color.Red;
				else
					AirStage2Friend.ForeColor = SystemColors.ControlText;

				int[] planeEnemy = { (int)bd.Data.api_kouku.api_stage2.api_e_lostcount, (int)bd.Data.api_kouku.api_stage2.api_e_count };
				AirStage2Enemy.Text = string.Format( "-{0}/{1}", planeEnemy[0], planeEnemy[1] );

				if ( planeEnemy[1] > 0 && planeEnemy[0] == planeEnemy[1] )
					AirStage2Enemy.ForeColor = Color.Red;
				else
					AirStage2Enemy.ForeColor = SystemColors.ControlText;


				//対空カットイン
				if ( bd.Data.api_kouku.api_stage2.api_air_fire() ) {
					int cutinID = (int)bd.Data.api_kouku.api_stage2.api_air_fire.api_kind;
					int cutinIndex = (int)bd.Data.api_kouku.api_stage2.api_air_fire.api_idx;

					AACutin.Text = "#" + ( cutinIndex + 1 );
					AACutin.ImageAlign = ContentAlignment.MiddleLeft;
					AACutin.ImageIndex = (int)ResourceManager.EquipmentContent.HighAngleGun;
					ToolTipInfo.SetToolTip( AACutin, string.Format(
						"対空カットイン: {0}\r\nカットイン種別: {1} ({2})",
						KCDatabase.Instance.Fleet[cutinIndex >= 6 ? 2 : bd.FleetIDFriend].MembersInstance[cutinIndex % 6].NameWithLevel,
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
		private void SetAerialWarfareAirBattle( BattleData bd ) {

			//空対空戦闘
			if ( (int)bd.Data.api_stage_flag[0] != 0 ) {

				//二回目の空戦が存在するか
				bool isBattle2Enabled = (int)bd.Data.api_stage_flag2[0] != 0;

				AirSuperiority.Text = Constants.GetAirSuperiority( (int)bd.Data.api_kouku.api_stage1.api_disp_seiku );
				if ( isBattle2Enabled ) {
					ToolTipInfo.SetToolTip( AirSuperiority, "第2次: " + Constants.GetAirSuperiority( (int)bd.Data.api_kouku2.api_stage1.api_disp_seiku ) );
				} else {
					ToolTipInfo.SetToolTip( AirSuperiority, null );
				}


				/*
				int[] planeFriend = { 
					(int)bd.Data.api_kouku.api_stage1.api_f_lostcount + ( isBattle2Enabled ? (int)bd.Data.api_kouku2.api_stage1.api_f_lostcount : 0 ), 
					(int)bd.Data.api_kouku.api_stage1.api_f_count };
				*/
				int[] planeFriend = {
					(int)bd.Data.api_kouku.api_stage1.api_f_lostcount,
					(int)bd.Data.api_kouku.api_stage1.api_f_count,
					( isBattle2Enabled ? (int)bd.Data.api_kouku2.api_stage1.api_f_lostcount : 0 ),
					( isBattle2Enabled ? (int)bd.Data.api_kouku2.api_stage1.api_f_count : 0 ),
				};
				AirStage1Friend.Text = string.Format( "-{0}/{1}", planeFriend[0] + planeFriend[2], planeFriend[1] );
				ToolTipInfo.SetToolTip( AirStage1Friend, string.Format( "第1次: -{0}/{1}\r\n第2次: -{2}/{3}\r\n",
					planeFriend[0], planeFriend[1], planeFriend[2], planeFriend[3] ) );

				if ( planeFriend[1] > 0 && ( planeFriend[0] == planeFriend[1] || planeFriend[2] == planeFriend[3] ) )
					AirStage1Friend.ForeColor = Color.Red;
				else
					AirStage1Friend.ForeColor = SystemColors.ControlText;


				int[] planeEnemy = { 
					(int)bd.Data.api_kouku.api_stage1.api_e_lostcount,
					(int)bd.Data.api_kouku.api_stage1.api_e_count,
					( isBattle2Enabled ? (int)bd.Data.api_kouku2.api_stage1.api_e_lostcount : 0 ),
					( isBattle2Enabled ? (int)bd.Data.api_kouku2.api_stage1.api_e_count : 0 ),
				};
				AirStage1Enemy.Text = string.Format( "-{0}/{1}", planeEnemy[0] + planeEnemy[2], planeEnemy[1] );
				ToolTipInfo.SetToolTip( AirStage1Enemy, string.Format( "第1次: -{0}/{1}\r\n第2次: -{2}/{3}\r\n",
					planeEnemy[0], planeEnemy[1], planeEnemy[2], planeEnemy[3] ) );

				if ( planeEnemy[1] > 0 && ( planeEnemy[0] == planeEnemy[1] || planeEnemy[2] == planeEnemy[3] ) )
					AirStage1Enemy.ForeColor = Color.Red;
				else
					AirStage1Enemy.ForeColor = SystemColors.ControlText;


				//触接
				int[] touchFriend = { 
					(int)bd.Data.api_kouku.api_stage1.api_touch_plane[0],
					isBattle2Enabled ? (int)bd.Data.api_kouku2.api_stage1.api_touch_plane[0] : -1
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
					(int)bd.Data.api_kouku.api_stage1.api_touch_plane[1],
					isBattle2Enabled ? (int)bd.Data.api_kouku2.api_stage1.api_touch_plane[1] : -1
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
			if ( (int)bd.Data.api_stage_flag[1] != 0 ) {

				//二回目の空戦が存在するか
				bool isBattle2Enabled = (int)bd.Data.api_stage_flag2[1] != 0;


				int[] planeFriend = { 
					(int)bd.Data.api_kouku.api_stage2.api_f_lostcount,
					(int)bd.Data.api_kouku.api_stage2.api_f_count,
					( isBattle2Enabled ? (int)bd.Data.api_kouku2.api_stage2.api_f_lostcount : 0 ),
					( isBattle2Enabled ? (int)bd.Data.api_kouku2.api_stage2.api_f_count : 0 ),
				};
				AirStage2Friend.Text = string.Format( "-{0}/{1}", planeFriend[0] + planeFriend[2], planeFriend[1] );
				ToolTipInfo.SetToolTip( AirStage2Friend, string.Format( "第1次: -{0}/{1}\r\n第2次: -{2}/{3}\r\n",
					planeFriend[0], planeFriend[1], planeFriend[2], planeFriend[3] ) );

				if ( planeFriend[1] > 0 && ( planeFriend[0] == planeFriend[1] || planeFriend[2] == planeFriend[3] ) )
					AirStage2Friend.ForeColor = Color.Red;
				else
					AirStage2Friend.ForeColor = SystemColors.ControlText;


				int[] planeEnemy = { 
					(int)bd.Data.api_kouku.api_stage2.api_e_lostcount,
					(int)bd.Data.api_kouku.api_stage2.api_e_count,
					( isBattle2Enabled ? (int)bd.Data.api_kouku2.api_stage2.api_e_lostcount : 0 ),
					( isBattle2Enabled ? (int)bd.Data.api_kouku2.api_stage2.api_e_count : 0 ),
				};
				AirStage2Enemy.Text = string.Format( "-{0}/{1}", planeEnemy[0] + planeEnemy[2], planeEnemy[1] );
				ToolTipInfo.SetToolTip( AirStage2Enemy, string.Format( "第1次: -{0}/{1}\r\n第2次: -{2}/{3}\r\n",
					planeEnemy[0], planeEnemy[1], planeEnemy[2], planeEnemy[3] ) );

				if ( planeEnemy[1] > 0 && ( planeEnemy[0] == planeEnemy[1] || planeEnemy[2] == planeEnemy[3] ) )
					AirStage2Enemy.ForeColor = Color.Red;
				else
					AirStage2Enemy.ForeColor = SystemColors.ControlText;


				//対空カットイン
				{
					bool[] fire = new bool[] { bd.Data.api_kouku.api_stage2.api_air_fire(), isBattle2Enabled && bd.Data.api_kouku2.api_stage2.api_air_fire() };
					int[] cutinID = new int[] {
						fire[0] ? (int)bd.Data.api_kouku.api_stage2.api_air_fire.api_kind : -1,
						fire[1] ? (int)bd.Data.api_kouku2.api_stage2.api_air_fire.api_kind : -1,
					};
					int[] cutinIndex = new int[] {
						fire[0] ? (int)bd.Data.api_kouku.api_stage2.api_air_fire.api_idx : -1,
						fire[1] ? (int)bd.Data.api_kouku2.api_stage2.api_air_fire.api_idx : -1,
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
									KCDatabase.Instance.Fleet[cutinIndex[i] >= 6 ? 2 : bd.FleetIDFriend].MembersInstance[cutinIndex[i] % 6].NameWithLevel,
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
			AirStage1Friend.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage1Friend.ImageIndex = -1;
			ToolTipInfo.SetToolTip( AirStage1Friend, null );

			AirStage1Enemy.Text = "-";
			AirStage1Enemy.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage1Enemy.ImageIndex = -1;
			ToolTipInfo.SetToolTip( AirStage1Enemy, null );

			AirStage2Friend.Text = "-";
			AirStage2Friend.ImageAlign = ContentAlignment.MiddleCenter;
			AirStage2Friend.ImageIndex = -1;
			ToolTipInfo.SetToolTip( AirStage2Friend, null );

			AirStage2Enemy.Text = "-";
			AirStage2Enemy.ImageAlign = ContentAlignment.MiddleCenter;
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
		private void SetHPNormal( int[] hp, BattleData bd ) {

			KCDatabase db = KCDatabase.Instance;
			bool isPractice = ( bd.BattleType & BattleData.BattleTypeFlag.Practice ) != 0;

			for ( int i = 0; i < 12; i++ ) {
				if ( (int)bd.Data.api_nowhps[i + 1] != -1 ) {
					HPBars[i].Value = hp[i];
					HPBars[i].PrevValue = (int)bd.Data.api_nowhps[i + 1];
					HPBars[i].MaximumValue = (int)bd.Data.api_maxhps[i + 1];
					HPBars[i].Visible = true;
				} else {
					HPBars[i].Visible = false;
				}
			}


			for ( int i = 0; i < 6; i++ ) {
				if ( (int)bd.Data.api_nowhps[i + 1] != -1 ) {
					ShipData ship = db.Ships[db.Fleet[bd.FleetIDFriend].Members[i]];

					ToolTipInfo.SetToolTip( HPBars[i],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]",
							ship.MasterShip.NameWithClass,
							ship.Level,
							Math.Max( HPBars[i].PrevValue, 0 ),
							Math.Max( HPBars[i].Value, 0 ),
							HPBars[i].MaximumValue,
							HPBars[i].Value - HPBars[i].PrevValue,
							Constants.GetDamageState( (double)HPBars[i].Value / HPBars[i].MaximumValue, isPractice, ship.MasterShip.IsLandBase )
							)
						);
				}
			}

			for ( int i = 0; i < 6; i++ ) {
				if ( (int)bd.Data.api_nowhps[i + 7] != -1 ) {
					ShipDataMaster ship = db.MasterShips[bd.EnemyFleetMembers[i + 1]];

					ToolTipInfo.SetToolTip( HPBars[i + 6],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]",
							ship.NameWithClass,
							bd.EnemyLevels[i + 1],
							Math.Max( HPBars[i + 6].PrevValue, 0 ),
							Math.Max( HPBars[i + 6].Value, 0 ),
							HPBars[i + 6].MaximumValue,
							HPBars[i + 6].Value - HPBars[i + 6].PrevValue,
							Constants.GetDamageState( (double)HPBars[i + 6].Value / HPBars[i + 6].MaximumValue, isPractice, ship.IsLandBase )
							)
						);
				}
			}


			FleetCombined.Visible = false;
			for ( int i = 12; i < 18; i++ ) {
				HPBars[i].Visible = false;
			}

		}

		/// <summary>
		/// 両軍のHPゲージを設定します。(連合艦隊用)
		/// </summary>
		private void SetHPCombined( int[] hp, BattleData bd ) {

			KCDatabase db = KCDatabase.Instance;
			bool isPractice = ( bd.BattleType & BattleData.BattleTypeFlag.Practice ) != 0;

			for ( int i = 0; i < 12; i++ ) {
				if ( (int)bd.Data.api_nowhps[i + 1] != -1 ) {
					HPBars[i].Value = hp[i];
					HPBars[i].PrevValue = (int)bd.Data.api_nowhps[i + 1];
					HPBars[i].MaximumValue = (int)bd.Data.api_maxhps[i + 1];
					HPBars[i].Visible = true;
				} else {
					HPBars[i].Visible = false;
				}
			}

			FleetCombined.Visible = true;
			for ( int i = 0; i < 6; i++ ) {
				if ( (int)bd.Data.api_nowhps_combined[i + 1] != -1 ) {
					HPBars[i + 12].Value = hp[i + 12];
					HPBars[i + 12].PrevValue = (int)bd.Data.api_nowhps_combined[i + 1];
					HPBars[i + 12].MaximumValue = (int)bd.Data.api_maxhps_combined[i + 1];
					HPBars[i + 12].Visible = true;
				} else {
					HPBars[i + 12].Visible = false;
				}
			}


			for ( int i = 0; i < 6; i++ ) {
				if ( (int)bd.Data.api_nowhps[i + 1] != -1 ) {
					ShipData ship = db.Ships[db.Fleet[bd.FleetIDFriend].Members[i]];
					bool isEscaped = db.Fleet[bd.FleetIDFriend].EscapedShipList.Contains( ship.MasterID );

					ToolTipInfo.SetToolTip( HPBars[i],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]",
							ship.MasterShip.NameWithClass,
							ship.Level,
							Math.Max( HPBars[i].PrevValue, 0 ),
							Math.Max( HPBars[i].Value, 0 ),
							HPBars[i].MaximumValue,
							HPBars[i].Value - HPBars[i].PrevValue,
							Constants.GetDamageState( (double)HPBars[i].Value / HPBars[i].MaximumValue, isPractice, ship.MasterShip.IsLandBase, isEscaped )
							)
						);

					if ( isEscaped ) HPBars[i].BackColor = Color.Silver;
					else HPBars[i].BackColor = SystemColors.Control;
				}
			}

			for ( int i = 0; i < 6; i++ ) {
				if ( (int)bd.Data.api_nowhps[i + 7] != -1 ) {
					ShipDataMaster ship = db.MasterShips[bd.EnemyFleetMembers[i + 1]];

					ToolTipInfo.SetToolTip( HPBars[i + 6],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]",
							ship.NameWithClass,
							bd.EnemyLevels[i + 1],
							Math.Max( HPBars[i + 6].PrevValue, 0 ),
							Math.Max( HPBars[i + 6].Value, 0 ),
							HPBars[i + 6].MaximumValue,
							HPBars[i + 6].Value - HPBars[i + 6].PrevValue,
							Constants.GetDamageState( (double)HPBars[i + 6].Value / HPBars[i + 6].MaximumValue, isPractice, ship.IsLandBase )
							)
						);
				}
			}

			for ( int i = 0; i < 6; i++ ) {
				if ( (int)bd.Data.api_nowhps_combined[i + 1] != -1 ) {
					ShipData ship = db.Ships[db.Fleet[2].Members[i]];
					bool isEscaped = db.Fleet[2].EscapedShipList.Contains( ship.MasterID );

					ToolTipInfo.SetToolTip( HPBars[i + 12],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]",
							ship.MasterShip.NameWithClass,
							ship.Level,
							Math.Max( HPBars[i + 12].PrevValue, 0 ),
							Math.Max( HPBars[i + 12].Value, 0 ),
							HPBars[i + 12].MaximumValue,
							HPBars[i + 12].Value - HPBars[i + 12].PrevValue,
							Constants.GetDamageState( (double)HPBars[i + 12].Value / HPBars[i + 12].MaximumValue, ship.MasterShip.IsLandBase, isEscaped )
							)
						);

					if ( isEscaped ) HPBars[i + 12].BackColor = Color.Silver;
					else HPBars[i + 12].BackColor = SystemColors.Control;
				}
			}
		}


		/// <summary>
		/// 勝利ランクを計算します。連合艦隊は情報が少ないので正確ではありません。
		/// </summary>
		/// <param name="countFriend">戦闘に参加した自軍艦数。</param>
		/// <param name="countEnemy">戦闘に参加した敵軍艦数。</param>
		/// <param name="sunkFriend">撃沈された自軍艦数。</param>
		/// <param name="sunkEnemy">撃沈した敵軍艦数。</param>
		/// <param name="friendrate">自軍損害率。</param>
		/// <param name="enemyrate">敵軍損害率。</param>
		/// <param name="defeatFlagship">敵旗艦を撃沈しているか。</param>
		/// <remarks>thanks: nekopanda</remarks>
		private static int GetWinRank(
			int countFriend, int countEnemy,
			int sunkFriend, int sunkEnemy,
			double friendrate, double enemyrate,
			bool defeatFlagship ) {

			int rifriend = (int)( friendrate * 100 );
			int rienemy = (int)( enemyrate * 100 );

			bool borderC = rienemy > ( 0.9 * rifriend );
			bool borderB = rienemy > ( 2.5 * rifriend );

			if ( sunkFriend == 0 ) {	// 味方轟沈数ゼロ
				if ( enemyrate >= 1.0 ) {	// 敵を殲滅した
					if ( friendrate <= 0.0 ) {	// 味方ダメージゼロ
						return 7;	// SS
					}
					return 6;	// S

				} else if ( sunkEnemy >= (int)Math.Round( countEnemy * 0.6 ) ) {	// 半数以上撃破
					return 5;	// A

				} else if ( defeatFlagship || borderB ) {	// 敵旗艦を撃沈 or 戦果ゲージが2.5倍以上
					return 4;	// B
				}

			} else {
				if ( enemyrate >= 1.0 ) {	// 敵を殲滅した
					return 4;	// B
				}
				// 敵旗艦を撃沈 and 味方轟沈数 < 敵撃沈数
				if ( defeatFlagship && ( sunkFriend < sunkEnemy ) ) {
					return 4;	// B
				}
				// 戦果ゲージが2.5倍以上
				if ( borderB ) {
					return 4;	// B
				}
				// 敵旗艦を撃沈
				// TODO: 味方の轟沈艦が２隻以上ある場合、敵旗艦を撃沈してもDになる場合がある
				if ( defeatFlagship ) {
					return 3;	// C
				}
			}

			// 戦果ゲージが0.9倍以上
			if ( borderC ) {
				return 3;	// C
			}
			// 轟沈艦があり かつ 残った艦が１隻のみ
			if ( ( sunkFriend > 0 ) && ( ( countFriend - sunkFriend ) == 1 ) ) {
				return 1;	// E
			}

			// 残りはD
			return 2;	// D
		}


		/// <summary>
		/// 損害率と戦績予測を設定します。
		/// </summary>
		private void SetDamageRateNormal( int[] hp, BattleData bd ) {

			int friendbefore = 0;
			int friendafter = 0;
			double friendrate;
			int enemybefore = 0;
			int enemyafter = 0;
			double enemyrate;

			for ( int i = 0; i < 6; i++ ) {
				friendbefore += Math.Max( (int)bd.Data.api_nowhps[i + 1], 0 );
				friendafter += Math.Max( hp[i], 0 );
				enemybefore += Math.Max( (int)bd.Data.api_nowhps[i + 7], 0 );
				enemyafter += Math.Max( hp[i + 6], 0 );
			}

			friendrate = ( (double)( friendbefore - friendafter ) / friendbefore );
			DamageFriend.Text = string.Format( "{0:p1}", friendrate );
			enemyrate = ( (double)( enemybefore - enemyafter ) / enemybefore );
			DamageEnemy.Text = string.Format( "{0:p1}", enemyrate );


			//戦績判定
			{
				int countFriend = KCDatabase.Instance.Fleet[(int)bd.FleetIDFriend].Members.Count( v => v != -1 );
				int countEnemy = ( bd.EnemyFleetMembers.Skip( 1 ).Count( v => v != -1 ) );
				int sunkFriend = hp.Take( countFriend ).Count( v => v <= 0 );
				int sunkEnemy = hp.Skip( 6 ).Take( countEnemy ).Count( v => v <= 0 );

				int rank = GetWinRank( countFriend, countEnemy, sunkFriend, sunkEnemy, friendrate, enemyrate, hp[6] <= 0 );


				WinRank.Text = Constants.GetWinRank( rank );
				WinRank.ForeColor = rank >= 4 ? WinRankColor_Win : WinRankColor_Lose;
			}
		}


		/// <summary>
		/// 損害率と戦績予測を設定します(連合艦隊用)。
		/// </summary>
		private void SetDamageRateCombined( int[] hp, BattleData bd ) {

			int friendbefore = 0;
			int friendafter = 0;
			double friendrate;
			int enemybefore = 0;
			int enemyafter = 0;
			double enemyrate;

			BattleDataCombined bdc = bd as BattleDataCombined;

			for ( int i = 0; i < 6; i++ ) {
				friendbefore += Math.Max( (int)bdc.Data.api_nowhps[i + 1], 0 );
				friendafter += Math.Max( hp[i], 0 );
				friendbefore += Math.Max( (int)bdc.Data.api_nowhps_combined[i + 1], 0 );
				friendafter += Math.Max( hp[i + 12], 0 );
				enemybefore += Math.Max( (int)bdc.Data.api_nowhps[i + 7], 0 );
				enemyafter += Math.Max( hp[i + 6], 0 );
			}

			friendrate = ( (double)( friendbefore - friendafter ) / friendbefore );
			DamageFriend.Text = string.Format( "{0:p1}", friendrate );
			enemyrate = ( (double)( enemybefore - enemyafter ) / enemybefore );
			DamageEnemy.Text = string.Format( "{0:p1}", enemyrate );


			//戦績判定
			{
				int countFriend = KCDatabase.Instance.Fleet[bdc.FleetIDFriend].Members.Count( v => v != -1 );
				int countFriendCombined = KCDatabase.Instance.Fleet[bdc.FleetIDFriendCombined].Members.Count( v => v != -1 );
				int countEnemy = ( bdc.EnemyFleetMembers.Skip( 1 ).Count( v => v != -1 ) );
				int sunkFriend = hp.Take( countFriend ).Count( v => v <= 0 ) + hp.Skip( 12 ).Take( countFriendCombined ).Count( v => v <= 0 );
				int sunkEnemy = hp.Skip( 6 ).Take( countEnemy ).Count( v => v <= 0 );

				int rank = GetWinRank( countFriend + countFriendCombined, countEnemy, sunkFriend, sunkEnemy, friendrate, enemyrate, hp[6] <= 0 );


				WinRank.Text = Constants.GetWinRank( rank );
				WinRank.ForeColor = rank >= 4 ? WinRankColor_Win : WinRankColor_Lose;
			}
		}


		/// <summary>
		/// 夜戦における各種表示を設定します。
		/// </summary>
		/// <param name="hp">戦闘開始前のHP。</param>
		/// <param name="isCombined">連合艦隊かどうか。</param>
		/// <param name="bd">戦闘データ。</param>
		private void SetNightBattleEvent( int[] hp, bool isCombined, BattleData bd ) {

			FleetData fleet = KCDatabase.Instance.Fleet[isCombined ? 2 : bd.FleetIDFriend];

			//味方探照灯判定
			{
				ShipData ship = null;
				int index = -1;
				for ( int i = 0; i < 6; i++ ) {
					ShipData s = fleet.MembersWithoutEscaped[i];
					if ( s != null &&
						s.SlotInstanceMaster.Count( e => e != null && e.CategoryType == 29 ) > 0 &&
						hp[isCombined ? 12 + i : i] > 1 ) {
						ship = s;
						index = i;
						break;
					}
				}

				if ( index != -1 ) {
					AirStage1Friend.Text = "#" + ( index + 1 );
					AirStage1Friend.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage1Friend.ImageIndex = (int)ResourceManager.EquipmentContent.Searchlight;
					ToolTipInfo.SetToolTip( AirStage1Friend, "探照灯照射: " + ship.NameWithLevel );
				} else {
					ToolTipInfo.SetToolTip( AirStage1Friend, null );
				}
			}

			//敵探照灯判定
			{
				int index = -1;
				for ( int i = 1; i < bd.EnemyFleetMembers.Count; i++ ) {
					if ( bd.EnemyFleetMembers[i] == -1 ) continue;
					if ( hp[i + 6 - 1] <= 1 ) continue;

					if ( ( (int[])bd.Data.api_eSlot[i - 1] ).Count(
						id => KCDatabase.Instance.MasterEquipments.ContainsKey( id ) &&
							KCDatabase.Instance.MasterEquipments[id].CategoryType == 29
							) > 0 ) {
						index = i;
						break;
					}
				}

				if ( index != -1 ) {
					AirStage1Enemy.Text = "#" + ( index );
					AirStage1Enemy.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage1Enemy.ImageIndex = (int)ResourceManager.EquipmentContent.Searchlight;
					ToolTipInfo.SetToolTip( AirStage1Enemy, "探照灯照射: " + KCDatabase.Instance.MasterShips[bd.EnemyFleetMembers[index]].NameWithClass );
				} else {
					ToolTipInfo.SetToolTip( AirStage1Enemy, null );
				}
			}


			//夜間触接判定
			if ( (int)bd.Data.api_touch_plane[0] != -1 ) {
				SearchingFriend.Text = "夜間触接";
				SearchingFriend.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
				SearchingFriend.ImageAlign = ContentAlignment.MiddleLeft;
				ToolTipInfo.SetToolTip( SearchingFriend, "夜間触接中: " + KCDatabase.Instance.MasterEquipments[(int)bd.Data.api_touch_plane[0]].Name );
			} else {
				ToolTipInfo.SetToolTip( SearchingFriend, null );
			}

			if ( (int)bd.Data.api_touch_plane[1] != -1 ) {
				SearchingEnemy.Text = "夜間触接";
				SearchingEnemy.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
				SearchingFriend.ImageAlign = ContentAlignment.MiddleLeft;
				ToolTipInfo.SetToolTip( SearchingEnemy, "夜間触接中: " + KCDatabase.Instance.MasterEquipments[(int)bd.Data.api_touch_plane[1]].Name );
			} else {
				ToolTipInfo.SetToolTip( SearchingEnemy, null );
			}

			//照明弾投射判定
			{
				int index = (int)bd.Data.api_flare_pos[0];

				if ( index != -1 ) {
					AirStage2Friend.Text = "#" + index;
					AirStage2Friend.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage2Friend.ImageIndex = (int)ResourceManager.EquipmentContent.Flare;
					ToolTipInfo.SetToolTip( AirStage2Friend, "照明弾投射: " + fleet.MembersInstance[index - 1].NameWithLevel );

				} else {
					ToolTipInfo.SetToolTip( AirStage2Friend, null );
				}
			}

			{
				int index = (int)bd.Data.api_flare_pos[1];

				if ( index != -1 ) {
					AirStage2Enemy.Text = "#" + index;
					AirStage2Enemy.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage2Enemy.ImageIndex = (int)ResourceManager.EquipmentContent.Flare;
					ToolTipInfo.SetToolTip( AirStage2Enemy, "照明弾投射: " + KCDatabase.Instance.MasterShips[bd.EnemyFleetMembers[index]].NameWithClass );
				} else {
					ToolTipInfo.SetToolTip( AirStage2Enemy, null );
				}
			}
		}



		void ConfigurationChanged() {

			MainFont = TableTop.Font = TableBottom.Font = Font = Utility.Configuration.Config.UI.MainFont;
			SubFont = Utility.Configuration.Config.UI.SubFont;

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
