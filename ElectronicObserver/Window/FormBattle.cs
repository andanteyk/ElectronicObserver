using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
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

		private List<ShipStatusHP> HPBars;
		private List<ImageLabel> DamageLabels;

		public Font MainFont { get; set; }
		public Font SubFont { get; set; }

		private Pen LinePen = Pens.Silver;

		public FormBattle( FormMain parent ) {
            SuspendLayout();
			InitializeComponent();

			ControlHelper.SetDoubleBuffered( TableTop );
			ControlHelper.SetDoubleBuffered( TableBottom );

			ConfigurationChanged();


			HPBars = new List<ShipStatusHP>( 18 );
			DamageLabels = new List<ImageLabel>( 12 );


			TableBottom.SuspendLayout();
			for ( int i = 0; i < 12; i++ ) {
				var lbl = new ImageLabel();
				DamageLabels.Add( lbl );
				{
					lbl.ImageList = ResourceManager.Instance.Icons;
					lbl.ImageIndex = (int)ResourceManager.IconContent.ConditionNormal;
					lbl.ImageAlign = ContentAlignment.MiddleLeft;
					lbl.AutoSize = false;
					lbl.ShowText = !Utility.Configuration.Config.FormBattle.IsShortDamage;
					lbl.Size = new Size( 56, 20 );
					lbl.Margin = new Padding( 2, 0, 2, 0 );
					lbl.Anchor = AnchorStyles.None;
					lbl.Font = MainFont;
				}
				if ( i < 6 ) {
					TableBottom.Controls.Add( lbl, 1, i + 1 );
				} else {
					TableBottom.Controls.Add( lbl, 3, i - 5 );
				}
			}

			for ( int i = 0; i < 18; i++ ) {
				HPBars.Add( new ShipStatusHP() );
				HPBars[i].Size = new Size( 80, 20 );
				HPBars[i].Margin = new Padding( 2, 0, 2, 0 );
				HPBars[i].Anchor = AnchorStyles.None;
				HPBars[i].MainFont = MainFont;
				HPBars[i].SubFont = SubFont;
				HPBars[i].MainFontColor = Utility.Configuration.Config.UI.ForeColor;
				HPBars[i].SubFontColor = Utility.Configuration.Config.UI.SubForeColor;
				HPBars[i].UsePrevValue = true;
				HPBars[i].ShowDifference = true;
				HPBars[i].MaximumDigit = 9999;

				if ( i < 6 ) {
					TableBottom.Controls.Add( HPBars[i], 0, i + 1 );
				} else if ( i < 12 ) {
					TableBottom.Controls.Add( HPBars[i], 4, i - 5 );
				} else {
					TableBottom.Controls.Add( HPBars[i], 2, i - 11 );
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
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96, 96);
            ResumeLayout();
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

			/*
			#region - Debug -

			dynamic data = Codeplex.Data.DynamicJson.Parse( System.IO.File.OpenRead( "api_start2.txt" ) ).api_data;
			o.APIList["api_start2"].OnResponseReceived( data );

			data = Codeplex.Data.DynamicJson.Parse( System.IO.File.OpenRead( "port.txt" ) ).api_data;
			o.APIList["api_port/port"].OnResponseReceived( data );

			data = Codeplex.Data.DynamicJson.Parse( System.IO.File.OpenRead( "battle.txt" ) ).api_data;
			string apiname = "api_req_sortie/battle";
			KCDatabase.Instance.Battle.LoadFromResponse( apiname, data );

			//data = Codeplex.Data.DynamicJson.Parse( System.IO.File.OpenRead( "practice.txt" ) ).api_data;
			//string apiname = "api_req_practice/battle";
			//KCDatabase.Instance.Battle.LoadFromResponse( apiname, data );

			//data = Codeplex.Data.DynamicJson.Parse( System.IO.File.OpenRead( "practice_midnight.txt" ) ).api_data;
			//apiname = "api_req_practice/midnight_battle";
			//KCDatabase.Instance.Battle.LoadFromResponse( apiname, data );
			Updated( apiname, data );

			#endregion
			//*/
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

						SetFormation( bm.BattleDay );
						SetSearchingResult( bm.BattleDay );
						SetAerialWarfare( bm.BattleDay.AirBattle );
						SetHPNormal( bm.BattleDay );
						SetDamageRateNormal( bm.BattleDay, bm.BattleDay.Initial.InitialHPs );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_battle_midnight/battle":
				case "api_req_practice/midnight_battle": {

						SetNightBattleEvent( bm.BattleNight.NightBattle );
						SetHPNormal( bm.BattleNight );
						SetDamageRateNormal( bm.BattleNight, bm.BattleDay.Initial.InitialHPs );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_battle_midnight/sp_midnight": {

						SetFormation( bm.BattleNight );
						ClearAerialWarfare();
						ClearSearchingResult();
						SetNightBattleEvent( bm.BattleNight.NightBattle );
						SetHPNormal( bm.BattleNight );
						SetDamageRateNormal( bm.BattleNight, bm.BattleNight.Initial.InitialHPs );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_sortie/airbattle": {

						SetFormation( bm.BattleDay );
						SetSearchingResult( bm.BattleDay );
						SetAerialWarfareAirBattle( bm.BattleDay.AirBattle, ( (BattleAirBattle)bm.BattleDay ).AirBattle2 );
						SetHPNormal( bm.BattleDay );
						SetDamageRateNormal( bm.BattleDay, bm.BattleDay.Initial.InitialHPs );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_combined_battle/battle":
				case "api_req_combined_battle/battle_water": {

						SetFormation( bm.BattleDay );
						SetSearchingResult( bm.BattleDay );
						SetAerialWarfare( bm.BattleDay.AirBattle );
						SetHPCombined( bm.BattleDay );
						SetDamageRateCombined( bm.BattleDay, bm.BattleDay.Initial.InitialHPs );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_combined_battle/airbattle": {

						SetFormation( bm.BattleDay );
						SetSearchingResult( bm.BattleDay );
						SetAerialWarfareAirBattle( bm.BattleDay.AirBattle, ( (BattleCombinedAirBattle)bm.BattleDay ).AirBattle2 );
						SetHPCombined( bm.BattleDay );
						SetDamageRateCombined( bm.BattleDay, bm.BattleDay.Initial.InitialHPs );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_combined_battle/midnight_battle": {

						SetNightBattleEvent( bm.BattleNight.NightBattle );
						SetHPCombined( bm.BattleNight );
						SetDamageRateCombined( bm.BattleNight, bm.BattleDay.Initial.InitialHPs );

						BaseLayoutPanel.Visible = true;
					} break;

				case "api_req_combined_battle/sp_midnight": {

						SetFormation( bm.BattleNight );
						ClearAerialWarfare();
						ClearSearchingResult();
						SetNightBattleEvent( bm.BattleNight.NightBattle );
						SetHPCombined( bm.BattleNight );
						SetDamageRateCombined( bm.BattleNight, bm.BattleNight.Initial.InitialHPs );

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

			FormationFriend.Text = Constants.GetFormationShort( bd.Searching.FormationFriend );
			FormationEnemy.Text = Constants.GetFormationShort( bd.Searching.FormationEnemy );
			Formation.Text = Constants.GetEngagementForm( bd.Searching.EngagementForm );

		}

		/// <summary>
		/// 索敵結果を設定します。
		/// </summary>
		private void SetSearchingResult( BattleData bd ) {

			int searchFriend = bd.Searching.SearchingFriend;
			SearchingFriend.Text = Constants.GetSearchingResultShort( searchFriend );
			SearchingFriend.ImageAlign = ContentAlignment.MiddleLeft;
			SearchingFriend.ImageIndex = (int)( searchFriend < 4 ? ResourceManager.EquipmentContent.Seaplane : ResourceManager.EquipmentContent.Radar );
			ToolTipInfo.SetToolTip( SearchingFriend, null );

			int searchEnemy = bd.Searching.SearchingEnemy;
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
		private void SetAerialWarfare( PhaseAirBattle pd ) {

			//空対空戦闘
			if ( pd.IsStage1Available ) {

				AirSuperiority.Text = Constants.GetAirSuperiority( pd.AirSuperiority );
				ToolTipInfo.SetToolTip( AirSuperiority, string.Format( "航空伤害: {0}", pd.TotalDamage ) );

				int[] planeFriend = { pd.AircraftLostStage1Friend, pd.AircraftTotalStage1Friend };
				AirStage1Friend.Text = string.Format( "-{0}/{1}", planeFriend[0], planeFriend[1] );

				if ( planeFriend[1] > 0 && planeFriend[0] == planeFriend[1] )
					AirStage1Friend.ForeColor = Utility.Configuration.Config.UI.FailedColor;
				else
					AirStage1Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;

				int[] planeEnemy = { pd.AircraftLostStage1Enemy, pd.AircraftTotalStage1Enemy };
				AirStage1Enemy.Text = string.Format( "-{0}/{1}", planeEnemy[0], planeEnemy[1] );

				if ( planeEnemy[1] > 0 && planeEnemy[0] == planeEnemy[1] )
					AirStage1Enemy.ForeColor = Utility.Configuration.Config.UI.FailedColor;
				else
					AirStage1Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;


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
				ToolTipInfo.SetToolTip( AirSuperiority, null );

				AirStage1Friend.Text = "-";
				AirStage1Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				AirStage1Friend.ImageAlign = ContentAlignment.MiddleCenter;
				AirStage1Friend.ImageIndex = -1;
				ToolTipInfo.SetToolTip( AirStage1Friend, null );

				AirStage1Enemy.Text = "-";
				AirStage1Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				AirStage1Enemy.ImageAlign = ContentAlignment.MiddleCenter;
				AirStage1Enemy.ImageIndex = -1;
				ToolTipInfo.SetToolTip( AirStage1Enemy, null );
			}

			//艦対空戦闘
			if ( pd.IsStage2Available ) {

				int[] planeFriend = { pd.AircraftLostStage2Friend, pd.AircraftTotalStage2Friend };
				AirStage2Friend.Text = string.Format( "-{0}/{1}", planeFriend[0], planeFriend[1] );

				if ( planeFriend[1] > 0 && planeFriend[0] == planeFriend[1] )
					AirStage2Friend.ForeColor = Utility.Configuration.Config.UI.FailedColor;
				else
					AirStage2Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;

				int[] planeEnemy = { pd.AircraftLostStage2Enemy, pd.AircraftTotalStage2Enemy };
				AirStage2Enemy.Text = string.Format( "-{0}/{1}", planeEnemy[0], planeEnemy[1] );

				if ( planeEnemy[1] > 0 && planeEnemy[0] == planeEnemy[1] )
					AirStage2Enemy.ForeColor = Utility.Configuration.Config.UI.FailedColor;
				else
					AirStage2Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;


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
				AirStage2Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				AirStage2Enemy.Text = "-";
				AirStage2Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;
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
					ToolTipInfo.SetToolTip( AirSuperiority, string.Format( "第2次: {0}\r\n航空伤害: {1}",
						Constants.GetAirSuperiority( pd2.AirSuperiority ),
						pd1.TotalDamage + pd2.TotalDamage ) );
				} else {
					ToolTipInfo.SetToolTip( AirSuperiority, string.Format( "航空伤害: {0}", pd1.TotalDamage ) );
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

				if ( planeFriend[1] > 0 && ( planeFriend[0] == planeFriend[1] || planeFriend[2] == planeFriend[3] ) )
					AirStage1Friend.ForeColor = Utility.Configuration.Config.UI.FailedColor;
				else
					AirStage1Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;


				int[] planeEnemy = { 
					pd1.AircraftLostStage1Enemy,
					pd1.AircraftTotalStage1Enemy,
					( isBattle2Enabled ? pd2.AircraftLostStage1Enemy : 0 ),
					( isBattle2Enabled ? pd2.AircraftTotalStage1Enemy : 0 ),
				};
				AirStage1Enemy.Text = string.Format( "-{0}/{1}", planeEnemy[0] + planeEnemy[2], planeEnemy[1] );
				ToolTipInfo.SetToolTip( AirStage1Enemy, string.Format( "第1次: -{0}/{1}\r\n第2次: -{2}/{3}\r\n",
					planeEnemy[0], planeEnemy[1], planeEnemy[2], planeEnemy[3] ) );

				if ( planeEnemy[1] > 0 && ( planeEnemy[0] == planeEnemy[1] || planeEnemy[2] == planeEnemy[3] ) )
					AirStage1Enemy.ForeColor = Utility.Configuration.Config.UI.FailedColor;
				else
					AirStage1Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;


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
					pd1.TouchAircraftFriend,
					isBattle2Enabled ? pd2.TouchAircraftFriend : -1
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
				AirStage1Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				ToolTipInfo.SetToolTip( AirStage1Friend, null );
				AirStage1Enemy.Text = "-";
				AirStage1Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;
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

				if ( planeFriend[1] > 0 && ( planeFriend[0] == planeFriend[1] || planeFriend[2] == planeFriend[3] ) )
					AirStage2Friend.ForeColor = Utility.Configuration.Config.UI.FailedColor;
				else
					AirStage2Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;


				int[] planeEnemy = { 
					pd1.AircraftLostStage2Enemy,
					pd1.AircraftTotalStage2Enemy,
					( isBattle2Enabled ? pd2.AircraftLostStage2Enemy : 0 ),
					( isBattle2Enabled ? pd2.AircraftTotalStage2Enemy : 0 ),
				};
				AirStage2Enemy.Text = string.Format( "-{0}/{1}", planeEnemy[0] + planeEnemy[2], planeEnemy[1] );
				ToolTipInfo.SetToolTip( AirStage2Enemy, string.Format( "第1次: -{0}/{1}\r\n第2次: -{2}/{3}\r\n",
					planeEnemy[0], planeEnemy[1], planeEnemy[2], planeEnemy[3] ) );

				if ( planeEnemy[1] > 0 && ( planeEnemy[0] == planeEnemy[1] || planeEnemy[2] == planeEnemy[3] ) )
					AirStage2Enemy.ForeColor = Utility.Configuration.Config.UI.FailedColor;
				else
					AirStage2Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;


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
				AirStage2Friend.ForeColor = Utility.Configuration.Config.UI.ForeColor;
				ToolTipInfo.SetToolTip( AirStage2Friend, null );
				AirStage2Enemy.Text = "-";
				AirStage2Enemy.ForeColor = Utility.Configuration.Config.UI.ForeColor;
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
		private void SetHPNormal( BattleData bd ) {

			KCDatabase db = KCDatabase.Instance;
			bool isPractice = ( bd.BattleType & BattleData.BattleTypeFlag.Practice ) != 0;

			var initialHPs = bd.Initial.InitialHPs;
			var maxHPs = bd.Initial.MaxHPs;
			var resultHPs = bd.ResultHPs;
			var attackDamages = bd.AttackDamages;
			var attackAirDamages = bd.AttackAirDamages;

			for ( int i = 0; i < 12; i++ ) {
				if ( initialHPs[i] != -1 ) {
					HPBars[i].Value = resultHPs[i];
					HPBars[i].PrevValue = initialHPs[i];
					HPBars[i].MaximumValue = maxHPs[i];
					HPBars[i].BackColor = Utility.Configuration.Config.UI.BackColor;
					HPBars[i].Visible = true;
					DamageLabels[i].ImageIndex = (int)ResourceManager.IconContent.ConditionNormal;
					DamageLabels[i].Visible = true;
				} else {
					HPBars[i].Visible = false;
					DamageLabels[i].Visible = false;
				}
			}


			for ( int i = 0; i < 6; i++ ) {
				if ( initialHPs[i] != -1 ) {
					ShipData ship = bd.Initial.FriendFleet.MembersInstance[i];

					DamageLabels[i].Text = ( attackAirDamages[i] + attackDamages[i] ) > 0 ? ( attackAirDamages[i] + attackDamages[i] ).ToString() : string.Empty;

					ToolTipInfo.SetToolTip( HPBars[i],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]\r\n\r\n造成伤害: {7}",
							ship.MasterShip.NameWithClass,
							ship.Level,
							Math.Max( HPBars[i].PrevValue, 0 ),
							Math.Max( HPBars[i].Value, 0 ),
							HPBars[i].MaximumValue,
							HPBars[i].Value - HPBars[i].PrevValue,
							Constants.GetDamageState( (double)HPBars[i].Value / HPBars[i].MaximumValue, isPractice, ship.MasterShip.IsLandBase ),
							( attackAirDamages[i] > 0 ) ? string.Format( "{0} (+{1})", attackDamages[i], attackAirDamages[i] ) : attackDamages[i].ToString()
							)
						);
				}
			}

			for ( int i = 0; i < 6; i++ ) {
				if ( initialHPs[i + 6] != -1 ) {
					ShipDataMaster ship = bd.Initial.EnemyMembersInstance[i];

					ToolTipInfo.SetToolTip( HPBars[i + 6],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]",
							ship.NameWithClass,
							bd.Initial.EnemyLevels[i],
							Math.Max( HPBars[i + 6].PrevValue, 0 ),
							Math.Max( HPBars[i + 6].Value, 0 ),
							HPBars[i + 6].MaximumValue,
							HPBars[i + 6].Value - HPBars[i + 6].PrevValue,
							Constants.GetDamageState( (double)HPBars[i + 6].Value / HPBars[i + 6].MaximumValue, isPractice, ship.IsLandBase )
							)
						);
				}
			}

			//HPBars[bd.MVPShipIndex].BackColor = Color.Moccasin;
			DamageLabels[bd.MVPShipIndex].ImageIndex = (int)ResourceManager.IconContent.ConditionSparkle;

			FleetCombined.Visible = false;
			for ( int i = 12; i < 18; i++ ) {
				HPBars[i].Visible = false;
			}

		}

		/// <summary>
		/// 両軍のHPゲージを設定します。(連合艦隊用)
		/// </summary>
		private void SetHPCombined( BattleData bd ) {

			KCDatabase db = KCDatabase.Instance;
			bool isPractice = ( bd.BattleType & BattleData.BattleTypeFlag.Practice ) != 0;

			var initialHPs = bd.Initial.InitialHPs;
			var maxHPs = bd.Initial.MaxHPs;
			var resultHPs = bd.ResultHPs;
			var attackDamages = bd.AttackDamages;
			var attackAirDamages = bd.AttackAirDamages;


			FleetCombined.Visible = true;
			for ( int i = 0; i < 18; i++ ) {
				if ( initialHPs[i] != -1 ) {
					HPBars[i].Value = resultHPs[i];
					HPBars[i].PrevValue = initialHPs[i];
					HPBars[i].MaximumValue = maxHPs[i];
					HPBars[i].Visible = true;
					if ( i < 12 ) {
						DamageLabels[i].ImageIndex = (int)ResourceManager.IconContent.ConditionNormal;
						DamageLabels[i].Visible = true;
					}
				} else {
					HPBars[i].Visible = false;
					if ( i < 12 )
						DamageLabels[i].Visible = false;
				}
			}



			for ( int i = 0; i < 6; i++ ) {
				if ( initialHPs[i] != -1 ) {
					ShipData ship = bd.Initial.FriendFleet.MembersInstance[i];
					bool isEscaped =  bd.Initial.FriendFleet.EscapedShipList.Contains( ship.MasterID );

					DamageLabels[i].Text = ( attackAirDamages[i] + attackDamages[i] ) > 0 ? ( attackAirDamages[i] + attackDamages[i] ).ToString() : string.Empty;

					ToolTipInfo.SetToolTip( HPBars[i],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]\r\n\r\n造成伤害: {7}",
							ship.MasterShip.NameWithClass,
							ship.Level,
							Math.Max( HPBars[i].PrevValue, 0 ),
							Math.Max( HPBars[i].Value, 0 ),
							HPBars[i].MaximumValue,
							HPBars[i].Value - HPBars[i].PrevValue,
							Constants.GetDamageState( (double)HPBars[i].Value / HPBars[i].MaximumValue, isPractice, ship.MasterShip.IsLandBase, isEscaped ),
							( attackAirDamages[i] > 0 ) ? string.Format( "{0} (+{1})", attackDamages[i], attackAirDamages[i] ) : attackDamages[i].ToString()
							)
						);

					if ( isEscaped ) HPBars[i].BackColor = Color.Silver;
					else HPBars[i].BackColor = Utility.Configuration.Config.UI.BackColor;
				}
			}

			for ( int i = 0; i < 6; i++ ) {
				if ( initialHPs[i + 6] != -1 ) {
					ShipDataMaster ship = bd.Initial.EnemyMembersInstance[i];

					ToolTipInfo.SetToolTip( HPBars[i + 6],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]",
							ship.NameWithClass,
							bd.Initial.EnemyLevels[i],
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
				if ( initialHPs[i + 12] != -1 ) {
					ShipData ship = db.Fleet[2].MembersInstance[i];
					bool isEscaped = db.Fleet[2].EscapedShipList.Contains( ship.MasterID );

					DamageLabels[i + 6].Text = ( attackAirDamages[i + 12] + attackDamages[i + 12] ) > 0 ? ( attackAirDamages[i + 12] + attackDamages[i + 12] ).ToString() : string.Empty;

					ToolTipInfo.SetToolTip( HPBars[i + 12],
						string.Format( "{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]\r\n\r\n造成伤害: {7}",
							ship.MasterShip.NameWithClass,
							ship.Level,
							Math.Max( HPBars[i + 12].PrevValue, 0 ),
							Math.Max( HPBars[i + 12].Value, 0 ),
							HPBars[i + 12].MaximumValue,
							HPBars[i + 12].Value - HPBars[i + 12].PrevValue,
							Constants.GetDamageState( (double)HPBars[i + 12].Value / HPBars[i + 12].MaximumValue, ship.MasterShip.IsLandBase, isEscaped ),
							( attackAirDamages[i + 12] > 0 ) ? string.Format( "{0} (+{1})", attackDamages[i + 12], attackAirDamages[i + 12] ) : attackDamages[i + 12].ToString()
							)
						);

					if ( isEscaped ) HPBars[i + 12].BackColor = Color.Silver;
					else HPBars[i + 12].BackColor = Utility.Configuration.Config.UI.BackColor;
				}
			}


			//HPBars[bd.MVPShipIndex].BackColor = Color.Moccasin;
			//HPBars[bd.MVPShipCombinedIndex].BackColor = Color.Moccasin;
			DamageLabels[bd.MVPShipIndex].ImageIndex = (int)ResourceManager.IconContent.ConditionSparkle;
			DamageLabels[bd.MVPShipCombinedIndex - 6].ImageIndex = (int)ResourceManager.IconContent.ConditionSparkle;
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
		private void SetDamageRateNormal( BattleData bd, int[] initialHPs ) {

			int friendbefore = 0;
			int friendafter = 0;
			double friendrate;
			int enemybefore = 0;
			int enemyafter = 0;
			double enemyrate;

			var resultHPs = bd.ResultHPs;

			for ( int i = 0; i < 6; i++ ) {
				friendbefore += Math.Max( initialHPs[i], 0 );
				friendafter += Math.Max( resultHPs[i], 0 );
				enemybefore += Math.Max( initialHPs[i + 6], 0 );
				enemyafter += Math.Max( resultHPs[i + 6], 0 );
			}

			friendrate = ( (double)( friendbefore - friendafter ) / friendbefore );
			DamageFriend.Text = string.Format( "{0:p1}", friendrate );
			enemyrate = ( (double)( enemybefore - enemyafter ) / enemybefore );
			DamageEnemy.Text = string.Format( "{0:p1}", enemyrate );


			//戦績判定
			{
				int countFriend = bd.Initial.FriendFleet.Members.Count( v => v != -1 );
				int countEnemy = ( bd.Initial.EnemyMembers.Count( v => v != -1 ) );
				int sunkFriend = resultHPs.Take( countFriend ).Count( v => v <= 0 );
				int sunkEnemy = resultHPs.Skip( 6 ).Take( countEnemy ).Count( v => v <= 0 );

				int rank = GetWinRank( countFriend, countEnemy, sunkFriend, sunkEnemy, friendrate, enemyrate, resultHPs[6] <= 0 );


				WinRank.Text = Constants.GetWinRank( rank );
				WinRank.ForeColor = rank >= 4 ? Utility.Configuration.Config.UI.ForeColor : Utility.Configuration.Config.UI.FailedColor;
			}
		}


		/// <summary>
		/// 損害率と戦績予測を設定します(連合艦隊用)。
		/// </summary>
		private void SetDamageRateCombined( BattleData bd, int[] initialHPs ) {

			int friendbefore = 0;
			int friendafter = 0;
			double friendrate;
			int enemybefore = 0;
			int enemyafter = 0;
			double enemyrate;

			var resultHPs = bd.ResultHPs;

			for ( int i = 0; i < 6; i++ ) {
				friendbefore += Math.Max( initialHPs[i], 0 );
				friendafter += Math.Max( resultHPs[i], 0 );
				friendbefore += Math.Max( initialHPs[i + 12], 0 );
				friendafter += Math.Max( resultHPs[i + 12], 0 );
				enemybefore += Math.Max( initialHPs[i + 6], 0 );
				enemyafter += Math.Max( resultHPs[i + 6], 0 );
			}

			friendrate = ( (double)( friendbefore - friendafter ) / friendbefore );
			DamageFriend.Text = string.Format( "{0:p1}", friendrate );
			enemyrate = ( (double)( enemybefore - enemyafter ) / enemybefore );
			DamageEnemy.Text = string.Format( "{0:p1}", enemyrate );


			//戦績判定
			{
				int countFriend = bd.Initial.FriendFleet.Members.Count( v => v != -1 );
				int countFriendCombined = KCDatabase.Instance.Fleet[2].Members.Count( v => v != -1 );
				int countEnemy = ( bd.Initial.EnemyMembers.Count( v => v != -1 ) );
				int sunkFriend = resultHPs.Take( countFriend ).Count( v => v <= 0 ) + resultHPs.Skip( 12 ).Take( countFriendCombined ).Count( v => v <= 0 );
				int sunkEnemy = resultHPs.Skip( 6 ).Take( countEnemy ).Count( v => v <= 0 );

				int rank = GetWinRank( countFriend + countFriendCombined, countEnemy, sunkFriend, sunkEnemy, friendrate, enemyrate, resultHPs[6] <= 0 );


				WinRank.Text = Constants.GetWinRank( rank );
				WinRank.ForeColor = rank >= 4 ? Utility.Configuration.Config.UI.ForeColor : Utility.Configuration.Config.UI.FailedColor;
			}
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
					AirStage1Enemy.Text = "#" + ( index );
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
					AirStage2Enemy.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage2Enemy.ImageIndex = (int)ResourceManager.EquipmentContent.Flare;
					ToolTipInfo.SetToolTip( AirStage2Enemy, "照明弾投射: " + pd.FlareEnemyInstance.NameWithClass );
				} else {
					ToolTipInfo.SetToolTip( AirStage2Enemy, null );
				}
			}
		}



		void ConfigurationChanged() {

			MainFont = TableTop.Font = TableBottom.Font = Font = Utility.Configuration.Config.UI.MainFont;
			SubFont = Utility.Configuration.Config.UI.SubFont;

			LinePen = new Pen( Utility.Configuration.Config.UI.LineColor.ColorData );

			bool shorten = Utility.Configuration.Config.FormBattle.IsShortDamage;
			bool isCombined = ( KCDatabase.Instance.Battle.BattleMode & BattleManager.BattleModes.CombinedMask ) != 0;

			int damageWidth = shorten ? 30 : 60;

			if ( isCombined ) {

				TableTop.ColumnStyles[1].Width = 84 + 2 * damageWidth;
				TableTop.ClientSize = new System.Drawing.Size( 252 + 2 * damageWidth, TableTop.ClientSize.Height );

				TableBottom.ColumnStyles[1].Width =
				TableBottom.ColumnStyles[3].Width = damageWidth;
				TableBottom.ColumnStyles[2].Width = 84;
				TableBottom.ClientSize = new System.Drawing.Size( 252 + 2 * damageWidth, TableBottom.ClientSize.Height );
			} else {

				TableTop.ColumnStyles[1].Width = 84;
				TableTop.ClientSize = new System.Drawing.Size( 252, TableTop.ClientSize.Height );

				TableBottom.ColumnStyles[1].Width = damageWidth;
				TableBottom.ColumnStyles[3].Width = 0;
				TableBottom.ColumnStyles[2].Width = 84 - damageWidth;
				TableBottom.ClientSize = new System.Drawing.Size( 252, TableBottom.ClientSize.Height );
			}

			if ( DamageLabels == null )
				return;


			for ( int i = 0; i < 12; i++ ) {

				DamageLabels[i].ShowText = !shorten;
			}

		}



		private void TableTop_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			if ( e.Row == 1 || e.Row == 3 )
				e.Graphics.DrawLine( LinePen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}

		private void TableBottom_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			if ( e.Row == 7 )
				e.Graphics.DrawLine( LinePen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}


		protected override string GetPersistString() {
			return "Battle";
		}


	}

}
