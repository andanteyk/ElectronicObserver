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

namespace ElectronicObserver.Window
{

	public partial class FormBattle : DockContent
	{

		private readonly Color WinRankColor_Win = SystemColors.ControlText;
		private readonly Color WinRankColor_Lose = Color.Red;

		private readonly Size DefaultBarSize = new Size(80, 20);
		private readonly Size SmallBarSize = new Size(60, 20);

		private List<ShipStatusHP> HPBars;

		public Font MainFont { get; set; }
		public Font SubFont { get; set; }



		public FormBattle(FormMain parent)
		{
			InitializeComponent();

			ControlHelper.SetDoubleBuffered(TableTop);
			ControlHelper.SetDoubleBuffered(TableBottom);


			HPBars = new List<ShipStatusHP>(24);


			TableBottom.SuspendLayout();
			for (int i = 0; i < 24; i++)
			{
				HPBars.Add(new ShipStatusHP());
				HPBars[i].Size = DefaultBarSize;
				HPBars[i].AutoSize = false;
				HPBars[i].AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
				HPBars[i].Margin = new Padding(2, 0, 2, 0);
				HPBars[i].Anchor = AnchorStyles.Left | AnchorStyles.Right;
				HPBars[i].MainFont = MainFont;
				HPBars[i].SubFont = SubFont;
				HPBars[i].UsePrevValue = true;
				HPBars[i].ShowDifference = true;
				HPBars[i].MaximumDigit = 9999;

				if (i < 6)
				{
					TableBottom.Controls.Add(HPBars[i], 0, i + 1);
				}
				else if (i < 12)
				{
					TableBottom.Controls.Add(HPBars[i], 1, i - 5);
				}
				else if (i < 18)
				{
					TableBottom.Controls.Add(HPBars[i], 3, i - 11);
				}
				else
				{
					TableBottom.Controls.Add(HPBars[i], 2, i - 17);
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
			FleetFriend.ImageList =
				ResourceManager.Instance.Equipments;


			ConfigurationChanged();

			BaseLayoutPanel.Visible = false;


			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBattle]);

		}



		private void FormBattle_Load(object sender, EventArgs e)
		{

			APIObserver o = APIObserver.Instance;

			o["api_port/port"].ResponseReceived += Updated;
			o["api_req_map/start"].ResponseReceived += Updated;
			o["api_req_map/next"].ResponseReceived += Updated;
			o["api_req_sortie/battle"].ResponseReceived += Updated;
			o["api_req_sortie/battleresult"].ResponseReceived += Updated;
			o["api_req_battle_midnight/battle"].ResponseReceived += Updated;
			o["api_req_battle_midnight/sp_midnight"].ResponseReceived += Updated;
			o["api_req_sortie/airbattle"].ResponseReceived += Updated;
			o["api_req_sortie/ld_airbattle"].ResponseReceived += Updated;
			o["api_req_sortie/night_to_day"].ResponseReceived += Updated;
			o["api_req_sortie/ld_shooting"].ResponseReceived += Updated;
			o["api_req_combined_battle/battle"].ResponseReceived += Updated;
			o["api_req_combined_battle/midnight_battle"].ResponseReceived += Updated;
			o["api_req_combined_battle/sp_midnight"].ResponseReceived += Updated;
			o["api_req_combined_battle/airbattle"].ResponseReceived += Updated;
			o["api_req_combined_battle/battle_water"].ResponseReceived += Updated;
			o["api_req_combined_battle/ld_airbattle"].ResponseReceived += Updated;
			o["api_req_combined_battle/ec_battle"].ResponseReceived += Updated;
			o["api_req_combined_battle/ec_midnight_battle"].ResponseReceived += Updated;
			o["api_req_combined_battle/ec_night_to_day"].ResponseReceived += Updated;
			o["api_req_combined_battle/each_battle"].ResponseReceived += Updated;
			o["api_req_combined_battle/each_battle_water"].ResponseReceived += Updated;
			o["api_req_combined_battle/ld_shooting"].ResponseReceived += Updated;
			o["api_req_combined_battle/battleresult"].ResponseReceived += Updated;
			o["api_req_practice/battle"].ResponseReceived += Updated;
			o["api_req_practice/midnight_battle"].ResponseReceived += Updated;
			o["api_req_practice/battle_result"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		}


		private void Updated(string apiname, dynamic data)
		{

			KCDatabase db = KCDatabase.Instance;
			BattleManager bm = db.Battle;
			bool hideDuringBattle = Utility.Configuration.Config.FormBattle.HideDuringBattle;

			BaseLayoutPanel.SuspendLayout();
			TableTop.SuspendLayout();
			TableBottom.SuspendLayout();
			switch (apiname)
			{

				case "api_port/port":
					BaseLayoutPanel.Visible = false;
					ToolTipInfo.RemoveAll();
					break;

				case "api_req_map/start":
				case "api_req_map/next":
					if (!bm.Compass.HasAirRaid)
						goto case "api_port/port";

					SetFormation(bm);
					ClearSearchingResult();
					ClearBaseAirAttack();
					SetAerialWarfare(null, ((BattleBaseAirRaid)bm.BattleDay).BaseAirRaid);
					SetHPBar(bm.BattleDay);
					SetDamageRate(bm);

					BaseLayoutPanel.Visible = !hideDuringBattle;
					break;


				case "api_req_sortie/battle":
				case "api_req_practice/battle":
				case "api_req_sortie/ld_airbattle":
				case "api_req_sortie/ld_shooting":
					{

						SetFormation(bm);
						SetSearchingResult(bm.BattleDay);
						SetBaseAirAttack(bm.BattleDay.BaseAirAttack);
						SetAerialWarfare(bm.BattleDay.JetAirBattle, bm.BattleDay.AirBattle);
						SetHPBar(bm.BattleDay);
						SetDamageRate(bm);

						BaseLayoutPanel.Visible = !hideDuringBattle;
					}
					break;

				case "api_req_battle_midnight/battle":
				case "api_req_practice/midnight_battle":
					{

						SetNightBattleEvent(bm.BattleNight.NightInitial);
						SetHPBar(bm.BattleNight);
						SetDamageRate(bm);

						BaseLayoutPanel.Visible = !hideDuringBattle;
					}
					break;

				case "api_req_battle_midnight/sp_midnight":
					{

						SetFormation(bm);
						ClearBaseAirAttack();
						ClearAerialWarfare();
						ClearSearchingResult();
						SetNightBattleEvent(bm.BattleNight.NightInitial);
						SetHPBar(bm.BattleNight);
						SetDamageRate(bm);

						BaseLayoutPanel.Visible = !hideDuringBattle;
					}
					break;

				case "api_req_sortie/airbattle":
					{

						SetFormation(bm);
						SetSearchingResult(bm.BattleDay);
						SetBaseAirAttack(bm.BattleDay.BaseAirAttack);
						SetAerialWarfare(bm.BattleDay.JetAirBattle, bm.BattleDay.AirBattle, ((BattleAirBattle)bm.BattleDay).AirBattle2);
						SetHPBar(bm.BattleDay);
						SetDamageRate(bm);

						BaseLayoutPanel.Visible = !hideDuringBattle;
					}
					break;

				case "api_req_sortie/night_to_day":
					{
						// 暫定
						var battle = bm.BattleNight as BattleDayFromNight;

						SetFormation(bm);
						ClearAerialWarfare();
						ClearSearchingResult();
						ClearBaseAirAttack();
						SetNightBattleEvent(battle.NightInitial);

						if (battle.NextToDay)
						{
							SetSearchingResult(battle);
							SetBaseAirAttack(battle.BaseAirAttack);
							SetAerialWarfare(battle.JetAirBattle, battle.AirBattle);
						}

						SetHPBar(bm.BattleDay);
						SetDamageRate(bm);

						BaseLayoutPanel.Visible = !hideDuringBattle;
					}
					break;

				case "api_req_combined_battle/battle":
				case "api_req_combined_battle/battle_water":
				case "api_req_combined_battle/ld_airbattle":
				case "api_req_combined_battle/ec_battle":
				case "api_req_combined_battle/each_battle":
				case "api_req_combined_battle/each_battle_water":
				case "api_req_combined_battle/ld_shooting":
					{

						SetFormation(bm);
						SetSearchingResult(bm.BattleDay);
						SetBaseAirAttack(bm.BattleDay.BaseAirAttack);
						SetAerialWarfare(bm.BattleDay.JetAirBattle, bm.BattleDay.AirBattle);
						SetHPBar(bm.BattleDay);
						SetDamageRate(bm);

						BaseLayoutPanel.Visible = !hideDuringBattle;
					}
					break;

				case "api_req_combined_battle/airbattle":
					{

						SetFormation(bm);
						SetSearchingResult(bm.BattleDay);
						SetBaseAirAttack(bm.BattleDay.BaseAirAttack);
						SetAerialWarfare(bm.BattleDay.JetAirBattle, bm.BattleDay.AirBattle, ((BattleCombinedAirBattle)bm.BattleDay).AirBattle2);
						SetHPBar(bm.BattleDay);
						SetDamageRate(bm);

						BaseLayoutPanel.Visible = !hideDuringBattle;
					}
					break;

				case "api_req_combined_battle/midnight_battle":
				case "api_req_combined_battle/ec_midnight_battle":
					{

						SetNightBattleEvent(bm.BattleNight.NightInitial);
						SetHPBar(bm.BattleNight);
						SetDamageRate(bm);

						BaseLayoutPanel.Visible = !hideDuringBattle;
					}
					break;

				case "api_req_combined_battle/sp_midnight":
					{

						SetFormation(bm);
						ClearAerialWarfare();
						ClearSearchingResult();
						ClearBaseAirAttack();
						SetNightBattleEvent(bm.BattleNight.NightInitial);
						SetHPBar(bm.BattleNight);
						SetDamageRate(bm);

						BaseLayoutPanel.Visible = !hideDuringBattle;
					}
					break;

				case "api_req_combined_battle/ec_night_to_day":
					{
						var battle = bm.BattleNight as BattleDayFromNight;

						SetFormation(bm);
						ClearAerialWarfare();
						ClearSearchingResult();
						ClearBaseAirAttack();
						SetNightBattleEvent(battle.NightInitial);

						if (battle.NextToDay)
						{
							SetSearchingResult(battle);
							SetBaseAirAttack(battle.BaseAirAttack);
							SetAerialWarfare(battle.JetAirBattle, battle.AirBattle);
						}

						SetHPBar(battle);
						SetDamageRate(bm);

						BaseLayoutPanel.Visible = !hideDuringBattle;
					}
					break;

				case "api_req_sortie/battleresult":
				case "api_req_combined_battle/battleresult":
				case "api_req_practice/battle_result":
					{

						SetMVPShip(bm);

						BaseLayoutPanel.Visible = true;
					}
					break;

			}

			TableTop.ResumeLayout();
			TableBottom.ResumeLayout();

			BaseLayoutPanel.ResumeLayout();


			if (Utility.Configuration.Config.UI.IsLayoutFixed)
				TableTop.Width = TableTop.GetPreferredSize(BaseLayoutPanel.Size).Width;
			else
				TableTop.Width = TableBottom.ClientSize.Width;
			TableTop.Height = TableTop.GetPreferredSize(BaseLayoutPanel.Size).Height;

		}


		/// <summary>
		/// 陣形・交戦形態を設定します。
		/// </summary>
		private void SetFormation(BattleManager bm)
		{
			FormationFriend.Text = Constants.GetFormationShort(bm.FirstBattle.Searching.FormationFriend);
			FormationEnemy.Text = Constants.GetFormationShort(bm.FirstBattle.Searching.FormationEnemy);
			Formation.Text = Constants.GetEngagementForm(bm.FirstBattle.Searching.EngagementForm);

			if (bm.Compass != null && bm.Compass.EventID == 5)
				FleetEnemy.ForeColor = Color.Red;
			else
				FleetEnemy.ForeColor = SystemColors.ControlText;

			if (bm.IsEnemyCombined && bm.StartsFromDayBattle)
			{
				bool willMain = bm.WillNightBattleWithMainFleet();
				FleetEnemy.BackColor = willMain ? Color.LightSteelBlue : SystemColors.Control;
				FleetEnemyEscort.BackColor = willMain ? SystemColors.Control : Color.LightSteelBlue;
			}
			else
			{
				FleetEnemy.BackColor =
				FleetEnemyEscort.BackColor = SystemColors.Control;
			}
		}

		/// <summary>
		/// 索敵結果を設定します。
		/// </summary>
		private void SetSearchingResult(BattleData bd)
		{
			void SetResult(ImageLabel label, int search)
			{
				label.Text = Constants.GetSearchingResultShort(search);
				label.ImageAlign = search > 0 ? ContentAlignment.MiddleLeft : ContentAlignment.MiddleCenter;
				label.ImageIndex = search > 0 ? (int)(search < 4 ? ResourceManager.EquipmentContent.Seaplane : ResourceManager.EquipmentContent.Radar) : -1;
				ToolTipInfo.SetToolTip(label, null);
			}

			SetResult(SearchingFriend, bd.Searching.SearchingFriend);
			SetResult(SearchingEnemy, bd.Searching.SearchingEnemy);
		}

		/// <summary>
		/// 索敵結果をクリアします。
		/// 索敵フェーズが発生しなかった場合にこれを設定します。
		/// </summary>
		private void ClearSearchingResult()
		{
			void ClearResult(ImageLabel label)
			{
				label.Text = "-";
				label.ImageAlign = ContentAlignment.MiddleCenter;
				label.ImageIndex = -1;
				ToolTipInfo.SetToolTip(label, null);
			}

			ClearResult(SearchingFriend);
			ClearResult(SearchingEnemy);
		}

		/// <summary>
		/// 基地航空隊フェーズの結果を設定します。
		/// </summary>
		private void SetBaseAirAttack(PhaseBaseAirAttack pd)
		{
			if (pd != null && pd.IsAvailable)
			{

				Searching.Text = "基地航空隊";
				Searching.ImageAlign = ContentAlignment.MiddleLeft;
				Searching.ImageIndex = (int)ResourceManager.EquipmentContent.LandAttacker;

				var sb = new StringBuilder();
				int index = 1;

				foreach (var phase in pd.AirAttackUnits)
				{

					sb.AppendFormat("{0} 回目 - #{1} :\r\n",
						index, phase.AirUnitID);

					if (phase.IsStage1Available)
					{
						sb.AppendFormat("　St1: 自軍 -{0}/{1} | 敵軍 -{2}/{3} | {4}\r\n",
							phase.AircraftLostStage1Friend, phase.AircraftTotalStage1Friend,
							phase.AircraftLostStage1Enemy, phase.AircraftTotalStage1Enemy,
							Constants.GetAirSuperiority(phase.AirSuperiority));
					}
					if (phase.IsStage2Available)
					{
						sb.AppendFormat("　St2: 自軍 -{0}/{1} | 敵軍 -{2}/{3}\r\n",
							phase.AircraftLostStage2Friend, phase.AircraftTotalStage2Friend,
							phase.AircraftLostStage2Enemy, phase.AircraftTotalStage2Enemy);
					}

					index++;
				}

				ToolTipInfo.SetToolTip(Searching, sb.ToString());


			}
			else
			{
				ClearBaseAirAttack();
			}

		}

		/// <summary>
		/// 基地航空隊フェーズの結果をクリアします。
		/// </summary>
		private void ClearBaseAirAttack()
		{
			Searching.Text = "索敵";
			Searching.ImageAlign = ContentAlignment.MiddleCenter;
			Searching.ImageIndex = -1;
			ToolTipInfo.SetToolTip(Searching, null);
		}



		/// <summary>
		/// 航空戦表示用ヘルパー
		/// </summary>
		private class AerialWarfareFormatter
		{
			public readonly PhaseAirBattleBase Air;
			public string PhaseName;

			public AerialWarfareFormatter(PhaseAirBattleBase air, string phaseName)
			{
				Air = air;
				PhaseName = phaseName;
			}

			public bool Enabled => Air != null && Air.IsAvailable;
			public bool Stage1Enabled => Enabled && Air.IsStage1Available;
			public bool Stage2Enabled => Enabled && Air.IsStage2Available;

			public bool GetEnabled(int stage)
			{
				if (stage == 1)
					return Stage1Enabled;
				else if (stage == 2)
					return Stage2Enabled;
				else
					throw new ArgumentOutOfRangeException();
			}

			public int GetAircraftLost(int stage, bool isFriend)
			{
				if (stage == 1)
					return isFriend ? Air.AircraftLostStage1Friend : Air.AircraftLostStage1Enemy;
				else if (stage == 2)
					return isFriend ? Air.AircraftLostStage2Friend : Air.AircraftLostStage2Enemy;
				else
					throw new ArgumentOutOfRangeException();
			}

			public int GetAircraftTotal(int stage, bool isFriend)
			{
				if (stage == 1)
					return isFriend ? Air.AircraftTotalStage1Friend : Air.AircraftTotalStage1Enemy;
				else if (stage == 2)
					return isFriend ? Air.AircraftTotalStage2Friend : Air.AircraftTotalStage2Enemy;
				else
					throw new ArgumentOutOfRangeException();
			}

			public int GetTouchAircraft(bool isFriend) => isFriend ? Air.TouchAircraftFriend : Air.TouchAircraftEnemy;

		}

		void ClearAircraftLabel(ImageLabel label)
		{
			label.Text = "-";
			label.ForeColor = SystemColors.ControlText;
			label.ImageAlign = ContentAlignment.MiddleCenter;
			label.ImageIndex = -1;
			ToolTipInfo.SetToolTip(label, null);
		}



		private void SetAerialWarfare(PhaseAirBattleBase phaseJet, PhaseAirBattleBase phase1) => SetAerialWarfare(phaseJet, phase1, null);

		/// <summary>
		/// 航空戦情報を設定します。
		/// </summary>
		/// <param name="phaseJet">噴式航空戦のデータ。発生していなければ null</param>
		/// <param name="phase1">第一次航空戦（通常航空戦）のデータ。</param>
		/// <param name="phase2">第二次航空戦のデータ。発生していなければ null</param>
		private void SetAerialWarfare(PhaseAirBattleBase phaseJet, PhaseAirBattleBase phase1, PhaseAirBattleBase phase2)
		{
			var phases = new[] {
				new AerialWarfareFormatter( phaseJet, "噴式戦: " ),
				new AerialWarfareFormatter( phase1, "第1次: "),
				new AerialWarfareFormatter( phase2, "第2次: "),
			};

			if (!phases[0].Enabled && !phases[2].Enabled)
				phases[1].PhaseName = "";


			void SetShootdown(ImageLabel label, int stage, bool isFriend, bool needAppendInfo)
			{
				var phasesEnabled = phases.Where(p => p.GetEnabled(stage));

				if (needAppendInfo)
				{
					label.Text = string.Join(",", phasesEnabled.Select(p => "-" + p.GetAircraftLost(stage, isFriend)));
					ToolTipInfo.SetToolTip(label, string.Join("", phasesEnabled.Select(p => $"{p.PhaseName}-{p.GetAircraftLost(stage, isFriend)}/{p.GetAircraftTotal(stage, isFriend)}\r\n")));
				}
				else
				{
					label.Text = $"-{phases[1].GetAircraftLost(stage, isFriend)}/{phases[1].GetAircraftTotal(stage, isFriend)}";
					ToolTipInfo.SetToolTip(label, null);
				}

				if (phasesEnabled.Any(p => p.GetAircraftTotal(stage, isFriend) > 0 && p.GetAircraftLost(stage, isFriend) == p.GetAircraftTotal(stage, isFriend)))
					label.ForeColor = Color.Red;
				else
					label.ForeColor = SystemColors.ControlText;

				label.ImageAlign = ContentAlignment.MiddleCenter;
				label.ImageIndex = -1;
			}

			void ClearAACutinLabel()
			{
				AACutin.Text = "対空砲火";
				AACutin.ImageAlign = ContentAlignment.MiddleCenter;
				AACutin.ImageIndex = -1;
				ToolTipInfo.SetToolTip(AACutin, null);
			}



			if (phases[1].Stage1Enabled)
			{
				bool needAppendInfo = phases[0].Stage1Enabled || phases[2].Stage1Enabled;
				var phases1 = phases.Where(p => p.Stage1Enabled);

				AirSuperiority.Text = Constants.GetAirSuperiority(phases[1].Air.AirSuperiority);

				ToolTipInfo.SetToolTip(AirSuperiority,
					needAppendInfo ? string.Join("", phases1.Select(p => $"{p.PhaseName}{Constants.GetAirSuperiority(p.Air.AirSuperiority)}\r\n")) : null);


				SetShootdown(AirStage1Friend, 1, true, needAppendInfo);
				SetShootdown(AirStage1Enemy, 1, false, needAppendInfo);

				void SetTouch(ImageLabel label, bool isFriend)
				{
					if (phases1.Any(p => p.GetTouchAircraft(isFriend) > 0))
					{
						label.ImageAlign = ContentAlignment.MiddleLeft;
						label.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;

						ToolTipInfo.SetToolTip(label, ToolTipInfo.GetToolTip(label) +
							"触接中\r\n" + string.Join("\r\n", phases1.Select(p => $"{p.PhaseName}{(KCDatabase.Instance.MasterEquipments[p.GetTouchAircraft(isFriend)]?.Name ?? "(なし)")}")));
					}
					else
					{
						label.ImageAlign = ContentAlignment.MiddleCenter;
						label.ImageIndex = -1;
					}
				}
				SetTouch(AirStage1Friend, true);
				SetTouch(AirStage1Enemy, false);
			}
			else
			{
				AirSuperiority.Text = Constants.GetAirSuperiority(-1);
				ToolTipInfo.SetToolTip(AirSuperiority, null);

				ClearAircraftLabel(AirStage1Friend);
				ClearAircraftLabel(AirStage1Enemy);
			}


			if (phases[1].Stage2Enabled)
			{
				bool needAppendInfo = phases[0].Stage2Enabled || phases[2].Stage2Enabled;
				var phases2 = phases.Where(p => p.Stage2Enabled);

				SetShootdown(AirStage2Friend, 2, true, needAppendInfo);
				SetShootdown(AirStage2Enemy, 2, false, needAppendInfo);


				if (phases2.Any(p => p.Air.IsAACutinAvailable))
				{
					AACutin.Text = "#" + string.Join("/", phases2.Select(p => p.Air.IsAACutinAvailable ? (p.Air.AACutInIndex + 1).ToString() : "-"));
					AACutin.ImageAlign = ContentAlignment.MiddleLeft;
					AACutin.ImageIndex = (int)ResourceManager.EquipmentContent.HighAngleGun;

					ToolTipInfo.SetToolTip(AACutin, "対空カットイン\r\n" +
						string.Join("\r\n", phases2.Select(p => p.PhaseName + (p.Air.IsAACutinAvailable ? $"{p.Air.AACutInShipName}\r\nカットイン種別: {p.Air.AACutInKind} ({Constants.GetAACutinKind(p.Air.AACutInKind)})" : "(発動せず)"))));
				}
				else
				{
					ClearAACutinLabel();
				}
			}
			else
			{
				ClearAircraftLabel(AirStage2Friend);
				ClearAircraftLabel(AirStage2Enemy);
				ClearAACutinLabel();
			}
		}

		private void ClearAerialWarfare()
		{
			AirSuperiority.Text = "-";
			ToolTipInfo.SetToolTip(AirSuperiority, null);

			ClearAircraftLabel(AirStage1Friend);
			ClearAircraftLabel(AirStage1Enemy);
			ClearAircraftLabel(AirStage2Friend);
			ClearAircraftLabel(AirStage2Enemy);

			AACutin.Text = "-";
			AACutin.ImageAlign = ContentAlignment.MiddleCenter;
			AACutin.ImageIndex = -1;
			ToolTipInfo.SetToolTip(AACutin, null);
		}



		/// <summary>
		/// 両軍のHPゲージを設定します。
		/// </summary>
		private void SetHPBar(BattleData bd)
		{

			KCDatabase db = KCDatabase.Instance;
			bool isPractice = bd.IsPractice;
			bool isFriendCombined = bd.IsFriendCombined;
			bool isEnemyCombined = bd.IsEnemyCombined;
			bool isBaseAirRaid = bd.IsBaseAirRaid;
			bool hasFriend7thShip = bd.Initial.FriendMaxHPs.Count(hp => hp > 0) == 7;

			var initial = bd.Initial;
			var resultHPs = bd.ResultHPs;
			var attackDamages = bd.AttackDamages;


			foreach (var bar in HPBars)
				bar.SuspendUpdate();


			void EnableHPBar(int index, int initialHP, int resultHP, int maxHP)
			{
				HPBars[index].Value = resultHP;
				HPBars[index].PrevValue = initialHP;
				HPBars[index].MaximumValue = maxHP;
				HPBars[index].BackColor = SystemColors.Control;
				HPBars[index].Visible = true;
			}

			void DisableHPBar(int index)
			{
				HPBars[index].Visible = false;
			}



			// friend main
			for (int i = 0; i < initial.FriendInitialHPs.Length; i++)
			{
				int refindex = BattleIndex.Get(BattleSides.FriendMain, i);

				if (initial.FriendInitialHPs[i] != -1)
				{
					EnableHPBar(refindex, initial.FriendInitialHPs[i], resultHPs[refindex], initial.FriendMaxHPs[i]);

					string name;
					bool isEscaped;
					bool isLandBase;

					var bar = HPBars[refindex];

					if (isBaseAirRaid)
					{
						name = string.Format("第{0}基地", i + 1);
						isEscaped = false;
						isLandBase = true;
						bar.Text = "LB";        //note: Land Base (Landing Boat もあるらしいが考えつかなかったので)

					}
					else
					{
						ShipData ship = bd.Initial.FriendFleet.MembersInstance[i];
						name = ship.NameWithLevel;
						isEscaped = bd.Initial.FriendFleet.EscapedShipList.Contains(ship.MasterID);
						isLandBase = ship.MasterShip.IsLandBase;
						bar.Text = Constants.GetShipClassClassification(ship.MasterShip.ShipType);
					}

					ToolTipInfo.SetToolTip(bar, string.Format
						("{0}\r\nHP: ({1} → {2})/{3} ({4}) [{5}]\r\n与ダメージ: {6}\r\n\r\n{7}",
						name,
						Math.Max(bar.PrevValue, 0),
						Math.Max(bar.Value, 0),
						bar.MaximumValue,
						bar.Value - bar.PrevValue,
						Constants.GetDamageState((double)bar.Value / bar.MaximumValue, isPractice, isLandBase, isEscaped),
						attackDamages[refindex],
						bd.GetBattleDetail(refindex)
						));

					if (isEscaped) bar.BackColor = Color.Silver;
					else bar.BackColor = SystemColors.Control;
				}
				else
				{
					DisableHPBar(refindex);
				}
			}


			// enemy main
			for (int i = 0; i < initial.EnemyInitialHPs.Length; i++)
			{
				int refindex = BattleIndex.Get(BattleSides.EnemyMain, i);

				if (initial.EnemyInitialHPs[i] != -1)
				{
					EnableHPBar(refindex, initial.EnemyInitialHPs[i], resultHPs[refindex], initial.EnemyMaxHPs[i]);
					ShipDataMaster ship = bd.Initial.EnemyMembersInstance[i];

					var bar = HPBars[refindex];
					bar.Text = Constants.GetShipClassClassification(ship.ShipType);

					ToolTipInfo.SetToolTip(bar,
						string.Format("{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]\r\n\r\n{7}",
							ship.NameWithClass,
							initial.EnemyLevels[i],
							Math.Max(bar.PrevValue, 0),
							Math.Max(bar.Value, 0),
							bar.MaximumValue,
							bar.Value - bar.PrevValue,
							Constants.GetDamageState((double)bar.Value / bar.MaximumValue, isPractice, ship.IsLandBase),
							bd.GetBattleDetail(refindex)
							)
						);
				}
				else
				{
					DisableHPBar(refindex);
				}
			}


			// friend escort
			if (isFriendCombined)
			{
				FleetFriendEscort.Visible = true;

				for (int i = 0; i < initial.FriendInitialHPsEscort.Length; i++)
				{
					int refindex = BattleIndex.Get(BattleSides.FriendEscort, i);

					if (initial.FriendInitialHPsEscort[i] != -1)
					{
						EnableHPBar(refindex, initial.FriendInitialHPsEscort[i], resultHPs[refindex], initial.FriendMaxHPsEscort[i]);

						ShipData ship = bd.Initial.FriendFleetEscort.MembersInstance[i];
						bool isEscaped = bd.Initial.FriendFleetEscort.EscapedShipList.Contains(ship.MasterID);

						var bar = HPBars[refindex];
						bar.Text = Constants.GetShipClassClassification(ship.MasterShip.ShipType);

						ToolTipInfo.SetToolTip(bar, string.Format(
							"{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]\r\n与ダメージ: {7}\r\n\r\n{8}",
							ship.MasterShip.NameWithClass,
							ship.Level,
							Math.Max(bar.PrevValue, 0),
							Math.Max(bar.Value, 0),
							bar.MaximumValue,
							bar.Value - bar.PrevValue,
							Constants.GetDamageState((double)bar.Value / bar.MaximumValue, isPractice, ship.MasterShip.IsLandBase, isEscaped),
							attackDamages[refindex],
							bd.GetBattleDetail(refindex)
							));

						if (isEscaped) bar.BackColor = Color.Silver;
						else bar.BackColor = SystemColors.Control;
					}
					else
					{
						DisableHPBar(refindex);
					}
				}

			}
			else
			{
				FleetFriendEscort.Visible = false;

				foreach (var i in BattleIndex.FriendEscort.Skip(Math.Max(bd.Initial.FriendFleet.Members.Count - 6, 0)))
					DisableHPBar(i);
			}

			MoveHPBar(hasFriend7thShip);



			// enemy escort
			if (isEnemyCombined)
			{
				FleetEnemyEscort.Visible = true;

				for (int i = 0; i < 6; i++)
				{
					int refindex = BattleIndex.Get(BattleSides.EnemyEscort, i);

					if (initial.EnemyInitialHPsEscort[i] != -1)
					{
						EnableHPBar(refindex, initial.EnemyInitialHPsEscort[i], resultHPs[refindex], initial.EnemyMaxHPsEscort[i]);

						ShipDataMaster ship = bd.Initial.EnemyMembersEscortInstance[i];

						var bar = HPBars[refindex];
						bar.Text = Constants.GetShipClassClassification(ship.ShipType);

						ToolTipInfo.SetToolTip(bar,
							string.Format("{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]\r\n\r\n{7}",
								ship.NameWithClass,
								bd.Initial.EnemyLevelsEscort[i],
								Math.Max(bar.PrevValue, 0),
								Math.Max(bar.Value, 0),
								bar.MaximumValue,
								bar.Value - bar.PrevValue,
								Constants.GetDamageState((double)bar.Value / bar.MaximumValue, isPractice, ship.IsLandBase),
								bd.GetBattleDetail(refindex)
								)
							);
					}
					else
					{
						DisableHPBar(refindex);
					}
				}

			}
			else
			{
				FleetEnemyEscort.Visible = false;

				foreach (var i in BattleIndex.EnemyEscort)
					DisableHPBar(i);
			}




			if ((isFriendCombined || (hasFriend7thShip && !Utility.Configuration.Config.FormBattle.Display7thAsSingleLine)) && isEnemyCombined)
			{
				foreach (var bar in HPBars)
				{
					bar.Size = SmallBarSize;
					bar.Text = null;
				}
			}
			else
			{
				bool showShipType = Utility.Configuration.Config.FormBattle.ShowShipTypeInHPBar;

				foreach (var bar in HPBars)
				{
					bar.Size = DefaultBarSize;

					if (!showShipType)
						bar.Text = "HP:";
				}
			}


			{   // support
				PhaseSupport support = null;

				if (bd is BattleDayFromNight bddn)
				{
					if (bddn.NightSupport?.IsAvailable ?? false)
						support = bddn.NightSupport;
				}
				if (support == null)
					support = bd.Support;

				if (support?.IsAvailable ?? false)
				{

					switch (support.SupportFlag)
					{
						case 1:
							FleetFriend.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedTorpedo;
							break;
						case 2:
							FleetFriend.ImageIndex = (int)ResourceManager.EquipmentContent.MainGunL;
							break;
						case 3:
							FleetFriend.ImageIndex = (int)ResourceManager.EquipmentContent.Torpedo;
							break;
						case 4:
							FleetFriend.ImageIndex = (int)ResourceManager.EquipmentContent.DepthCharge;
							break;
						default:
							FleetFriend.ImageIndex = (int)ResourceManager.EquipmentContent.Unknown;
							break;
					}

					FleetFriend.ImageAlign = ContentAlignment.MiddleLeft;
					ToolTipInfo.SetToolTip(FleetFriend, "支援攻撃\r\n" + support.GetBattleDetail());

					if ((isFriendCombined || hasFriend7thShip) && isEnemyCombined)
						FleetFriend.Text = "自軍";
					else
						FleetFriend.Text = "自軍艦隊";

				}
				else
				{
					FleetFriend.ImageIndex = -1;
					FleetFriend.ImageAlign = ContentAlignment.MiddleCenter;
					FleetFriend.Text = "自軍艦隊";
					ToolTipInfo.SetToolTip(FleetFriend, null);

				}
			}


			if (bd.Initial.IsBossDamaged)
				HPBars[BattleIndex.EnemyMain1].BackColor = Color.MistyRose;

			if (!isBaseAirRaid)
			{
				foreach (int i in bd.MVPShipIndexes)
					HPBars[BattleIndex.Get(BattleSides.FriendMain, i)].BackColor = Color.Moccasin;

				if (isFriendCombined)
				{
					foreach (int i in bd.MVPShipCombinedIndexes)
						HPBars[BattleIndex.Get(BattleSides.FriendEscort, i)].BackColor = Color.Moccasin;
				}
			}

			foreach (var bar in HPBars)
				bar.ResumeUpdate();
		}


		private bool _hpBarMoved = false;
		/// <summary>
		/// 味方遊撃部隊７人目のHPゲージ（通常時は連合艦隊第二艦隊旗艦のHPゲージ）を移動します。
		/// </summary>
		private void MoveHPBar(bool hasFriend7thShip)
		{
			if (Utility.Configuration.Config.FormBattle.Display7thAsSingleLine && hasFriend7thShip)
			{
				if (_hpBarMoved)
					return;
				TableBottom.SetCellPosition(HPBars[BattleIndex.FriendEscort1], new TableLayoutPanelCellPosition(0, 7));
				bool fixSize = Utility.Configuration.Config.UI.IsLayoutFixed;
				bool showHPBar = Utility.Configuration.Config.FormBattle.ShowHPBar;
				ControlHelper.SetTableRowStyle(TableBottom, 7, fixSize ? new RowStyle(SizeType.Absolute, showHPBar ? 21 : 16) : new RowStyle(SizeType.AutoSize));
				_hpBarMoved = true;
			}
			else
			{
				if (!_hpBarMoved)
					return;
				TableBottom.SetCellPosition(HPBars[BattleIndex.FriendEscort1], new TableLayoutPanelCellPosition(1, 1));
				ControlHelper.SetTableRowStyle(TableBottom, 7, new RowStyle(SizeType.Absolute, 0));
				_hpBarMoved = false;
			}

		}


		/// <summary>
		/// 損害率と戦績予測を設定します。
		/// </summary>
		private void SetDamageRate(BattleManager bm)
		{
			int rank = bm.PredictWinRank(out double friendrate, out double enemyrate);

			DamageFriend.Text = friendrate.ToString("p1");
			DamageEnemy.Text = enemyrate.ToString("p1");

			if (bm.IsBaseAirRaid)
			{
				int kind = bm.Compass.AirRaidDamageKind;
				WinRank.Text = Constants.GetAirRaidDamageShort(kind);
				WinRank.ForeColor = (1 <= kind && kind <= 3) ? WinRankColor_Lose : WinRankColor_Win;
			}
			else
			{
				WinRank.Text = Constants.GetWinRank(rank);
				WinRank.ForeColor = rank >= 4 ? WinRankColor_Win : WinRankColor_Lose;
			}

			WinRank.MinimumSize = Utility.Configuration.Config.UI.IsLayoutFixed ? new Size(DefaultBarSize.Width, 0) : new Size(HPBars[0].Width, 0);
		}


		/// <summary>
		/// 夜戦における各種表示を設定します。
		/// </summary>
		private void SetNightBattleEvent(PhaseNightInitial pd)
		{

			FleetData fleet = pd.FriendFleet;

			//味方探照灯判定
			{
				int index = pd.SearchlightIndexFriend;

				if (index != -1)
				{
					ShipData ship = fleet.MembersInstance[index];

					AirStage1Friend.Text = "#" + (index + (pd.IsFriendEscort ? 6 : 0) + 1);
					AirStage1Friend.ForeColor = SystemColors.ControlText;
					AirStage1Friend.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage1Friend.ImageIndex = (int)ResourceManager.EquipmentContent.Searchlight;
					ToolTipInfo.SetToolTip(AirStage1Friend, "探照灯照射: " + ship.NameWithLevel);
				}
				else
				{
					ToolTipInfo.SetToolTip(AirStage1Friend, null);
				}
			}

			//敵探照灯判定
			{
				int index = pd.SearchlightIndexEnemy;
				if (index != -1)
				{
					AirStage1Enemy.Text = "#" + (index + (pd.IsEnemyEscort ? 6 : 0) + 1);
					AirStage1Enemy.ForeColor = SystemColors.ControlText;
					AirStage1Enemy.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage1Enemy.ImageIndex = (int)ResourceManager.EquipmentContent.Searchlight;
					ToolTipInfo.SetToolTip(AirStage1Enemy, "探照灯照射: " + pd.SearchlightEnemyInstance.NameWithClass);
				}
				else
				{
					ToolTipInfo.SetToolTip(AirStage1Enemy, null);
				}
			}


			//夜間触接判定
			if (pd.TouchAircraftFriend != -1)
			{
				SearchingFriend.Text = "夜間触接";
				SearchingFriend.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
				SearchingFriend.ImageAlign = ContentAlignment.MiddleLeft;
				ToolTipInfo.SetToolTip(SearchingFriend, "夜間触接中: " + KCDatabase.Instance.MasterEquipments[pd.TouchAircraftFriend].Name);
			}
			else
			{
				ToolTipInfo.SetToolTip(SearchingFriend, null);
			}

			if (pd.TouchAircraftEnemy != -1)
			{
				SearchingEnemy.Text = "夜間触接";
				SearchingEnemy.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
				SearchingFriend.ImageAlign = ContentAlignment.MiddleLeft;
				ToolTipInfo.SetToolTip(SearchingEnemy, "夜間触接中: " + KCDatabase.Instance.MasterEquipments[pd.TouchAircraftEnemy].Name);
			}
			else
			{
				ToolTipInfo.SetToolTip(SearchingEnemy, null);
			}

			//照明弾投射判定
			{
				int index = pd.FlareIndexFriend;

				if (index != -1)
				{
					AirStage2Friend.Text = "#" + (index + 1);
					AirStage2Friend.ForeColor = SystemColors.ControlText;
					AirStage2Friend.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage2Friend.ImageIndex = (int)ResourceManager.EquipmentContent.Flare;
					ToolTipInfo.SetToolTip(AirStage2Friend, "照明弾投射: " + pd.FlareFriendInstance.NameWithLevel);

				}
				else
				{
					ToolTipInfo.SetToolTip(AirStage2Friend, null);
				}
			}

			{
				int index = pd.FlareIndexEnemy;

				if (index != -1)
				{
					AirStage2Enemy.Text = "#" + (index + 1);
					AirStage2Enemy.ForeColor = SystemColors.ControlText;
					AirStage2Enemy.ImageAlign = ContentAlignment.MiddleLeft;
					AirStage2Enemy.ImageIndex = (int)ResourceManager.EquipmentContent.Flare;
					ToolTipInfo.SetToolTip(AirStage2Enemy, "照明弾投射: " + pd.FlareEnemyInstance.NameWithClass);
				}
				else
				{
					ToolTipInfo.SetToolTip(AirStage2Enemy, null);
				}
			}
		}


		/// <summary>
		/// 戦闘終了後に、MVP艦の表示を更新します。
		/// </summary>
		/// <param name="bm">戦闘データ。</param>
		private void SetMVPShip(BattleManager bm)
		{

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


			for (int i = 0; i < friend.Members.Count; i++)
			{
				if (friend.EscapedShipList.Contains(friend.Members[i]))
					HPBars[i].BackColor = Color.Silver;

				else if (br.MVPIndex == i + 1)
					HPBars[i].BackColor = Color.Moccasin;

				else
					HPBars[i].BackColor = SystemColors.Control;
			}

			if (escort != null)
			{
				for (int i = 0; i < escort.Members.Count; i++)
				{
					if (escort.EscapedShipList.Contains(escort.Members[i]))
						HPBars[i + 6].BackColor = Color.Silver;

					else if (br.MVPIndexCombined == i + 1)
						HPBars[i + 6].BackColor = Color.Moccasin;

					else
						HPBars[i + 6].BackColor = SystemColors.Control;
				}
			}

			/*// debug
			if ( WinRank.Text.First().ToString() != bm.Result.Rank ) {
				Utility.Logger.Add( 1, string.Format( "戦闘評価予測が誤っています。(予測: {0}, 実際: {1})", WinRank.Text.First().ToString(), bm.Result.Rank ) );
			}
			//*/

		}


		private void RightClickMenu_Opening(object sender, CancelEventArgs e)
		{

			var bm = KCDatabase.Instance.Battle;

			if (bm == null || bm.BattleMode == BattleManager.BattleModes.Undefined)
				e.Cancel = true;

			RightClickMenu_ShowBattleResult.Enabled = !BaseLayoutPanel.Visible;
		}

		private void RightClickMenu_ShowBattleDetail_Click(object sender, EventArgs e)
		{
			var bm = KCDatabase.Instance.Battle;

			if (bm == null || bm.BattleMode == BattleManager.BattleModes.Undefined)
				return;

			var dialog = new Dialog.DialogBattleDetail
			{
				BattleDetailText = BattleDetailDescriptor.GetBattleDetail(bm),
				Location = RightClickMenu.Location
			};
			dialog.Show(this);

		}

		private void RightClickMenu_ShowBattleResult_Click(object sender, EventArgs e)
		{
			BaseLayoutPanel.Visible = true;
		}




		void ConfigurationChanged()
		{

			var config = Utility.Configuration.Config;

			MainFont = TableTop.Font = TableBottom.Font = Font = config.UI.MainFont;
			SubFont = config.UI.SubFont;

			BaseLayoutPanel.AutoScroll = config.FormBattle.IsScrollable;


			bool fixSize = config.UI.IsLayoutFixed;
			bool showHPBar = config.FormBattle.ShowHPBar;

			TableBottom.SuspendLayout();
			if (fixSize)
			{
				ControlHelper.SetTableColumnStyles(TableBottom, new ColumnStyle(SizeType.AutoSize));
				ControlHelper.SetTableRowStyle(TableBottom, 0, new RowStyle(SizeType.Absolute, 21));
				for (int i = 1; i <= 6; i++)
					ControlHelper.SetTableRowStyle(TableBottom, i, new RowStyle(SizeType.Absolute, showHPBar ? 21 : 16));
				ControlHelper.SetTableRowStyle(TableBottom, 8, new RowStyle(SizeType.Absolute, 21));
			}
			else
			{
				ControlHelper.SetTableColumnStyles(TableBottom, new ColumnStyle(SizeType.AutoSize));
				ControlHelper.SetTableRowStyles(TableBottom, new RowStyle(SizeType.AutoSize));
			}
			if (HPBars != null)
			{
				foreach (var b in HPBars)
				{
					b.MainFont = MainFont;
					b.SubFont = SubFont;
					b.AutoSize = !fixSize;
					if (!b.AutoSize)
					{
						b.Size = (HPBars[12].Visible && HPBars[18].Visible) ? SmallBarSize : DefaultBarSize;
					}
					b.HPBar.ColorMorphing = config.UI.BarColorMorphing;
					b.HPBar.SetBarColorScheme(config.UI.BarColorScheme.Select(col => col.ColorData).ToArray());
					b.ShowHPBar = showHPBar;
				}
			}
			FleetFriend.MaximumSize =
			FleetFriendEscort.MaximumSize =
			FleetEnemy.MaximumSize =
			FleetEnemyEscort.MaximumSize =
			DamageFriend.MaximumSize =
			DamageEnemy.MaximumSize =
				fixSize ? DefaultBarSize : Size.Empty;

			WinRank.MinimumSize = fixSize ? new Size(80, 0) : new Size(HPBars[0].Width, 0);

			TableBottom.ResumeLayout();

			TableTop.SuspendLayout();
			if (fixSize)
			{
				ControlHelper.SetTableColumnStyles(TableTop, new ColumnStyle(SizeType.Absolute, 21 * 4));
				ControlHelper.SetTableRowStyles(TableTop, new RowStyle(SizeType.Absolute, 21));
				TableTop.Width = TableTop.GetPreferredSize(BaseLayoutPanel.Size).Width;
			}
			else
			{
				ControlHelper.SetTableColumnStyles(TableTop, new ColumnStyle(SizeType.Percent, 100));
				ControlHelper.SetTableRowStyles(TableTop, new RowStyle(SizeType.AutoSize));
				TableTop.Width = TableBottom.ClientSize.Width;
			}
			TableTop.Height = TableTop.GetPreferredSize(BaseLayoutPanel.Size).Height;
			TableTop.ResumeLayout();

		}



		private void TableTop_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			if (e.Row == 1 || e.Row == 3)
				e.Graphics.DrawLine(Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
		}

		private void TableBottom_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			if (e.Row == 8)
				e.Graphics.DrawLine(Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
		}


		protected override string GetPersistString()
		{
			return "Battle";
		}


	}

}
