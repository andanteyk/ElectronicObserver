using ElectronicObserver.Data.Battle.Phase;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Detail
{

	public static class BattleDetailDescriptor
	{

		public static string GetBattleDetail(BattleManager bm)
		{
			var sb = new StringBuilder();

			if (bm.IsPractice)
			{
				sb.AppendLine("演習");

			}
			else
			{
				sb.AppendFormat("{0} ({1}-{2})", bm.Compass.MapInfo.Name, bm.Compass.MapAreaID, bm.Compass.MapInfoID);
				if (bm.Compass.MapInfo.EventDifficulty > 0)
					sb.AppendFormat(" [{0}]", Constants.GetDifficulty(bm.Compass.MapInfo.EventDifficulty));
				sb.Append(" セル: ").Append(bm.Compass.Destination.ToString());
				if (bm.Compass.EventID == 5)
					sb.Append(" (ボス)");
				sb.AppendLine();

				var mapinfo = bm.Compass.MapInfo;
				if (!mapinfo.IsCleared)
				{
					if (mapinfo.RequiredDefeatedCount != -1)
					{
						sb.AppendFormat("撃破: {0} / {1} 回", mapinfo.CurrentDefeatedCount, mapinfo.RequiredDefeatedCount)
							.AppendLine();
					}
					else if (mapinfo.MapHPMax > 0)
					{
						int current = bm.Compass.MapHPCurrent > 0 ? bm.Compass.MapHPCurrent : mapinfo.MapHPCurrent;
						int max = bm.Compass.MapHPMax > 0 ? bm.Compass.MapHPMax : mapinfo.MapHPMax;
						sb.AppendFormat("{0}{1}: {2} / {3}", mapinfo.CurrentGaugeIndex > 0 ? "#" + mapinfo.CurrentGaugeIndex + " " : "", mapinfo.GaugeType == 3 ? "TP" : "HP", current, max)
							.AppendLine();
					}
				}
			}
			if (bm.Result != null)
			{
				sb.AppendLine(bm.Result.EnemyFleetName);
			}
			sb.AppendLine();


			sb.AppendFormat("◆ {0} ◆\r\n", bm.FirstBattle.BattleName).AppendLine(GetBattleDetail(bm.FirstBattle));
			if (bm.SecondBattle != null)
				sb.AppendFormat("◆ {0} ◆\r\n", bm.SecondBattle.BattleName).AppendLine(GetBattleDetail(bm.SecondBattle));


			if (bm.Result != null)
			{
				sb.AppendLine(GetBattleResult(bm));
			}

			return sb.ToString();
		}


		public static string GetBattleDetail(BattleData battle)
		{

			var sbmaster = new StringBuilder();
			bool isBaseAirRaid = battle.IsBaseAirRaid;


			foreach (var phase in battle.GetPhases())
			{

				var sb = new StringBuilder();

				switch (phase)
				{
					case PhaseBaseAirRaid p:

						sb.AppendLine("味方基地航空隊 参加中隊:");
						sb.Append("　").AppendLine(string.Join(", ", p.Squadrons.Where(sq => sq.EquipmentInstance != null).Select(sq => sq.ToString()).DefaultIfEmpty("(なし)")));

						GetBattleDetailPhaseAirBattle(sb, p);

						break;

					case PhaseAirBattle p:

						GetBattleDetailPhaseAirBattle(sb, p);

						break;

					case PhaseBaseAirAttack p:

						foreach (var a in p.AirAttackUnits)
						{
							sb.AppendFormat("〈第{0}波〉\r\n", a.AirAttackIndex + 1);

							sb.AppendLine("味方基地航空隊 参加中隊:");
							sb.Append("　").AppendLine(string.Join(", ", a.Squadrons.Where(sq => sq.EquipmentInstance != null).Select(sq => sq.ToString())));

							GetBattleDetailPhaseAirBattle(sb, a);
							sb.Append(a.GetBattleDetail());
						}

						break;

					case PhaseJetAirBattle p:
						GetBattleDetailPhaseAirBattle(sb, p);

						break;

					case PhaseJetBaseAirAttack p:

						foreach (var a in p.AirAttackUnits)
						{
							sb.AppendFormat("〈第{0}波〉\r\n", a.AirAttackIndex + 1);

							sb.AppendLine("味方基地航空隊 参加中隊:");
							sb.Append("　").AppendLine(string.Join(", ", a.Squadrons.Where(sq => sq.EquipmentInstance != null).Select(sq => sq.ToString())));

							GetBattleDetailPhaseAirBattle(sb, a);
							sb.Append(a.GetBattleDetail());
						}

						break;

					case PhaseInitial p:


						if (p.FriendFleetEscort != null)
							sb.Append("〈味方主力艦隊〉");
						else
							sb.Append("〈味方艦隊〉");


						void appendFleetInfo(FleetData fleet)
						{
							sb.Append(" 制空戦力 ");
							sb.Append(GetRangeString(Calculator.GetAirSuperiority(fleet, false), Calculator.GetAirSuperiority(fleet, true)));

							double truncate2(double value) => Math.Floor(value * 100) / 100;
							sb.AppendFormat(" / 索敵能力 [1] {0}, [2] {1}, [3] {2}, [4] {3}",
								truncate2(Calculator.GetSearchingAbility_New33(fleet, 1)),
								truncate2(Calculator.GetSearchingAbility_New33(fleet, 2)),
								truncate2(Calculator.GetSearchingAbility_New33(fleet, 3)),
								truncate2(Calculator.GetSearchingAbility_New33(fleet, 4)));
						}

						if (isBaseAirRaid)
						{
							sb.AppendLine();
							OutputFriendBase(sb, p.FriendInitialHPs, p.FriendMaxHPs);
						}
						else
						{
							appendFleetInfo(p.FriendFleet);
							sb.AppendLine();
							OutputFriendData(sb, p.FriendFleet, p.FriendInitialHPs, p.FriendMaxHPs);
						}

						if (p.FriendFleetEscort != null)
						{
							sb.AppendLine();
							sb.Append("〈味方随伴艦隊〉");
							appendFleetInfo(p.FriendFleetEscort);
							sb.AppendLine();

							OutputFriendData(sb, p.FriendFleetEscort, p.FriendInitialHPsEscort, p.FriendMaxHPsEscort);
						}


						sb.AppendLine();


						void appendEnemyFleetInfo(int[] members)
						{
							int air = 0;
							int airbase = 0;
							bool indeterminate = false;
							for (int i = 0; i < members.Length; i++)
							{
								var param = RecordManager.Instance.ShipParameter[members[i]];
								if (param == null) continue;

								if (param.DefaultSlot == null || param.Aircraft == null)
								{
									indeterminate = true;
									continue;
								}

								for (int s = 0; s < Math.Min(param.DefaultSlot.Length, param.Aircraft.Length); s++)
								{
									air += Calculator.GetAirSuperiority(param.DefaultSlot[s], param.Aircraft[s]);
									if (KCDatabase.Instance.MasterEquipments[param.DefaultSlot[s]]?.IsAircraft ?? false)
										airbase += Calculator.GetAirSuperiority(param.DefaultSlot[s], param.Aircraft[s], 0, 0, 1);
								}
							}
							sb.AppendFormat(" 制空戦力 {0} (対基地 {1})", air, airbase);
							if (indeterminate)
								sb.Append(" (未確定)");
						}

						if (p.EnemyMembersEscort != null)
							sb.Append("〈敵主力艦隊〉");
						else
							sb.Append("〈敵艦隊〉");

						appendEnemyFleetInfo(p.EnemyMembers);

						if (p.IsBossDamaged)
							sb.Append(" : 装甲破壊");
						sb.AppendLine();

						OutputEnemyData(sb, p.EnemyMembersInstance, p.EnemyLevels, p.EnemyInitialHPs, p.EnemyMaxHPs, p.EnemySlotsInstance, p.EnemyParameters);


						if (p.EnemyMembersEscort != null)
						{
							sb.AppendLine();
							sb.AppendLine("〈敵随伴艦隊〉");

							appendEnemyFleetInfo(p.EnemyMembersEscort);
                            sb.AppendLine();

							OutputEnemyData(sb, p.EnemyMembersEscortInstance, p.EnemyLevelsEscort, p.EnemyInitialHPsEscort, p.EnemyMaxHPsEscort, p.EnemySlotsEscortInstance, p.EnemyParametersEscort);
						}

						sb.AppendLine();

						if (battle.GetPhases().Where(ph => ph is PhaseBaseAirAttack || ph is PhaseBaseAirRaid).Any(ph => ph != null && ph.IsAvailable))
						{
							sb.AppendLine("〈基地航空隊〉");
							GetBattleDetailBaseAirCorps(sb, KCDatabase.Instance.Battle.Compass.MapAreaID);      // :(
							sb.AppendLine();
						}

						if (p.RationIndexes.Length > 0)
						{
							sb.AppendLine("〈戦闘糧食補給〉");
							foreach (var index in p.RationIndexes)
							{
								var ship = p.GetFriendShip(index);

								if (ship != null)
								{
									sb.AppendFormat("　{0} #{1}\r\n", ship.NameWithLevel, index + 1);
								}
							}
							sb.AppendLine();
						}

						break;

					case PhaseNightInitial p:

						{
							var eq = KCDatabase.Instance.MasterEquipments[p.TouchAircraftFriend];
							if (eq != null)
							{
								sb.Append("自軍夜間触接: ").AppendLine(eq.Name);
							}
							eq = KCDatabase.Instance.MasterEquipments[p.TouchAircraftEnemy];
							if (eq != null)
							{
								sb.Append("敵軍夜間触接: ").AppendLine(eq.Name);
							}
						}

						{
							int searchlightIndex = p.SearchlightIndexFriend;
							if (searchlightIndex != -1)
							{
								sb.AppendFormat("自軍探照灯照射: {0} #{1}\r\n", p.FriendFleet.MembersInstance[searchlightIndex].Name, searchlightIndex + 1);
							}
							searchlightIndex = p.SearchlightIndexEnemy;
							if (searchlightIndex != -1)
							{
								sb.AppendFormat("敵軍探照灯照射: {0} #{1}\r\n", p.EnemyMembersInstance[searchlightIndex].NameWithClass, searchlightIndex + 1);
							}
						}

						if (p.FlareIndexFriend != -1)
						{
							sb.AppendFormat("自軍照明弾投射: {0} #{1}\r\n", p.FlareFriendInstance.NameWithLevel, p.FlareIndexFriend + 1);
						}
						if (p.FlareIndexEnemy != -1)
						{
							sb.AppendFormat("敵軍照明弾投射: {0} #{1}\r\n", p.FlareEnemyInstance.NameWithClass, p.FlareIndexEnemy + 1);
						}

						sb.AppendLine();
						break;


					case PhaseSearching p:
						sb.Append("自軍陣形: ").Append(Constants.GetFormation(p.FormationFriend));
						sb.Append(" / 敵軍陣形: ").AppendLine(Constants.GetFormation(p.FormationEnemy));
						sb.Append("交戦形態: ").AppendLine(Constants.GetEngagementForm(p.EngagementForm));
						sb.Append("自軍索敵: ").Append(Constants.GetSearchingResult(p.SearchingFriend));
						sb.Append(" / 敵軍索敵: ").AppendLine(Constants.GetSearchingResult(p.SearchingEnemy));

						sb.AppendLine();

						break;

					case PhaseSupport p:
						if (p.IsAvailable)
						{
							sb.AppendLine("〈支援艦隊〉");
							OutputSupportData(sb, p.SupportFleet);
							sb.AppendLine();
						}
						break;

					case PhaseFriendlySupportInfo p:
						if (p.IsAvailable)
						{
							OutputFriendlySupportData(sb, p);
							sb.AppendLine();
						}
						break;

					case PhaseFriendlyShelling p:
						if (p.IsAvailable)
						{
							{
								int searchlightIndex = p.SearchlightIndexFriend;
								if (searchlightIndex != -1)
								{
									sb.AppendFormat("自軍探照灯照射: {0} #{1}\r\n", p.SearchlightFriendInstance.NameWithClass, searchlightIndex + 1);
								}
								searchlightIndex = p.SearchlightIndexEnemy;
								if (searchlightIndex != -1)
								{
									sb.AppendFormat("敵軍探照灯照射: {0} #{1}\r\n", p.SearchlightEnemyInstance.NameWithClass, searchlightIndex + 1);
								}
							}

							{
								int flareIndex = p.FlareIndexFriend;
								if (flareIndex != -1)
								{
									sb.AppendFormat("自軍照明弾投射: {0} #{1}\r\n", p.FlareFriendInstance.NameWithClass, flareIndex + 1);
								}
								flareIndex = p.FlareIndexEnemy;
								if (flareIndex != -1)
								{
									sb.AppendFormat("敵軍照明弾投射: {0} #{1}\r\n", p.FlareEnemyInstance.NameWithClass, flareIndex + 1);
								}
							}

							sb.AppendLine();
						}
						break;

					case PhaseFriendlyAirBattle p:

						GetBattleDetailPhaseAirBattle(sb, p);

						break;
				}


				if (!(phase is PhaseBaseAirAttack || phase is PhaseJetBaseAirAttack))       // 通常出力と重複するため
					sb.Append(phase.GetBattleDetail());

				if (sb.Length > 0)
				{
					sbmaster.AppendFormat("《{0}》\r\n", phase.Title).Append(sb);
				}
			}


			{
				sbmaster.AppendLine("《戦闘終了》");

				var friend = battle.Initial.FriendFleet;
				var friendescort = battle.Initial.FriendFleetEscort;
				var enemy = battle.Initial.EnemyMembersInstance;
				var enemyescort = battle.Initial.EnemyMembersEscortInstance;

				if (friendescort != null)
					sbmaster.AppendLine("〈味方主力艦隊〉");
				else
					sbmaster.AppendLine("〈味方艦隊〉");

				if (isBaseAirRaid)
				{

					for (int i = 0; i < 6; i++)
					{
						if (battle.Initial.FriendMaxHPs[i] <= 0)
							continue;

						OutputResultData(sbmaster, i, string.Format("第{0}基地", i + 1),
							battle.Initial.FriendInitialHPs[i], battle.ResultHPs[i], battle.Initial.FriendMaxHPs[i]);
					}

				}
				else
				{
					for (int i = 0; i < friend.Members.Count(); i++)
					{
						var ship = friend.MembersInstance[i];
						if (ship == null)
							continue;

						OutputResultData(sbmaster, i, ship.Name,
							battle.Initial.FriendInitialHPs[i], battle.ResultHPs[i], battle.Initial.FriendMaxHPs[i]);
					}
				}

				if (friendescort != null)
				{
					sbmaster.AppendLine().AppendLine("〈味方随伴艦隊〉");

					for (int i = 0; i < friendescort.Members.Count(); i++)
					{
						var ship = friendescort.MembersInstance[i];
						if (ship == null)
							continue;

						OutputResultData(sbmaster, i + 6, ship.Name,
							battle.Initial.FriendInitialHPsEscort[i], battle.ResultHPs[i + 6], battle.Initial.FriendMaxHPsEscort[i]);
					}

				}


				sbmaster.AppendLine();
				if (enemyescort != null)
					sbmaster.AppendLine("〈敵主力艦隊〉");
				else
					sbmaster.AppendLine("〈敵艦隊〉");

				for (int i = 0; i < enemy.Length; i++)
				{
					var ship = enemy[i];
					if (ship == null)
						continue;

					OutputResultData(sbmaster, i,
						ship.NameWithClass,
						battle.Initial.EnemyInitialHPs[i], battle.ResultHPs[i + 12], battle.Initial.EnemyMaxHPs[i]);
				}

				if (enemyescort != null)
				{
					sbmaster.AppendLine().AppendLine("〈敵随伴艦隊〉");

					for (int i = 0; i < enemyescort.Length; i++)
					{
						var ship = enemyescort[i];
						if (ship == null)
							continue;

						OutputResultData(sbmaster, i + 6, ship.NameWithClass,
							battle.Initial.EnemyInitialHPsEscort[i], battle.ResultHPs[i + 18], battle.Initial.EnemyMaxHPsEscort[i]);
					}
				}

				sbmaster.AppendLine();
			}

			return sbmaster.ToString();
		}


		private static string GetRangeString(int min, int max) => min != max ? $"{min} ～ {max}" : min.ToString();


		private static void GetBattleDetailBaseAirCorps(StringBuilder sb, int mapAreaID)
		{
			foreach (var corps in KCDatabase.Instance.BaseAirCorps.Values.Where(corps => corps.MapAreaID == mapAreaID))
			{
				sb.AppendFormat("{0} [{1}] 制空戦力 {2}\r\n　{3}\r\n",
					corps.Name, Constants.GetBaseAirCorpsActionKind(corps.ActionKind),
					GetRangeString(Calculator.GetAirSuperiority(corps, false), Calculator.GetAirSuperiority(corps, true)),
					string.Join(", ", corps.Squadrons.Values
						.Where(sq => sq.State == 1 && sq.EquipmentInstance != null)
						.Select(sq => sq.EquipmentInstance.NameWithLevel)));
			}
		}

		private static void GetBattleDetailPhaseAirBattle(StringBuilder sb, PhaseAirBattleBase p)
		{

			if (p.IsStage1Available)
			{
				sb.Append("Stage1: ").AppendLine(Constants.GetAirSuperiority(p.AirSuperiority));
				sb.AppendFormat("　自軍: -{0}/{1}\r\n　敵軍: -{2}/{3}\r\n",
					p.AircraftLostStage1Friend, p.AircraftTotalStage1Friend,
					p.AircraftLostStage1Enemy, p.AircraftTotalStage1Enemy);
				if (p.TouchAircraftFriend > 0)
					sb.AppendFormat("　自軍触接: {0}\r\n", KCDatabase.Instance.MasterEquipments[p.TouchAircraftFriend].Name);
				if (p.TouchAircraftEnemy > 0)
					sb.AppendFormat("　敵軍触接: {0}\r\n", KCDatabase.Instance.MasterEquipments[p.TouchAircraftEnemy].Name);
			}
			if (p.IsStage2Available)
			{
				sb.Append("Stage2: ");
				if (p.IsAACutinAvailable)
				{
					sb.AppendFormat("対空カットイン( {0}, {1}({2}) )", p.AACutInShipName, Constants.GetAACutinKind(p.AACutInKind), p.AACutInKind);
				}
				sb.AppendLine();
				sb.AppendFormat("　自軍: -{0}/{1}\r\n　敵軍: -{2}/{3}\r\n",
					p.AircraftLostStage2Friend, p.AircraftTotalStage2Friend,
					p.AircraftLostStage2Enemy, p.AircraftTotalStage2Enemy);
			}

			if (p.IsStage1Available || p.IsStage2Available)
				sb.AppendLine();
		}


		private static void OutputFriendData(StringBuilder sb, FleetData fleet, int[] initialHPs, int[] maxHPs)
		{

			for (int i = 0; i < fleet.MembersInstance.Count; i++)
			{
				var ship = fleet.MembersInstance[i];

				if (ship == null)
					continue;

				sb.AppendFormat("#{0}: {1} {2} HP: {3} / {4} - 火力{5}, 雷装{6}, 対空{7}, 装甲{8}{9}\r\n",
					i + 1,
					ship.MasterShip.ShipTypeName, ship.NameWithLevel,
					initialHPs[i], maxHPs[i],
					ship.FirepowerBase, ship.TorpedoBase, ship.AABase, ship.ArmorBase,
					fleet.EscapedShipList.Contains(ship.MasterID) ? " (退避中)" : "");

				sb.Append("　");
				sb.AppendLine(string.Join(", ", ship.AllSlotInstance.Zip(
					ship.ExpansionSlot > 0 ? ship.Aircraft.Concat(new[] { 0 }) : ship.Aircraft,
					(eq, aircraft) => eq == null ? null : ((eq.MasterEquipment.IsAircraft ? $"[{aircraft}] " : "") + eq.NameWithLevel)
				).Where(str => str != null)));
			}
		}

		private static void OutputFriendBase(StringBuilder sb, int[] initialHPs, int[] maxHPs)
		{

			for (int i = 0; i < initialHPs.Length; i++)
			{
				if (maxHPs[i] <= 0)
					continue;

				sb.AppendFormat("#{0}: 陸上施設 第{1}基地 HP: {2} / {3}\r\n\r\n",
					i + 1,
					i + 1,
					initialHPs[i], maxHPs[i]);
			}

		}

		public static void OutputSupportData(StringBuilder sb, FleetData fleet)
		{

			for (int i = 0; i < fleet.MembersInstance.Count; i++)
			{
				var ship = fleet.MembersInstance[i];

				if (ship == null)
					continue;

				sb.AppendFormat("#{0}: {1} {2} - 火力{3}, 雷装{4}, 対空{5}, 装甲{6}\r\n",
					i + 1,
					ship.MasterShip.ShipTypeName, ship.NameWithLevel,
					ship.FirepowerBase, ship.TorpedoBase, ship.AABase, ship.ArmorBase);

				sb.Append("　");
				sb.AppendLine(string.Join(", ", ship.AllSlotInstance.Where(eq => eq != null)));
			}

		}

		private static void OutputFriendlySupportData(StringBuilder sb, PhaseFriendlySupportInfo p)
		{

			for (int i = 0; i < p.FriendlyMembersInstance.Length; i++)
			{
				var ship = p.FriendlyMembersInstance[i];

				if (ship == null)
					continue;

				sb.AppendFormat("#{0}: {1} {2} Lv. {3} HP: {4} / {5} - 火力{6}, 雷装{7}, 対空{8}, 装甲{9}\r\n",
					i + 1,
					ship.ShipTypeName, p.FriendlyMembersInstance[i].NameWithClass, p.FriendlyLevels[i],
					p.FriendlyInitialHPs[i], p.FriendlyMaxHPs[i],
					p.FriendlyParameters[i][0], p.FriendlyParameters[i][1], p.FriendlyParameters[i][2], p.FriendlyParameters[i][3]);

				sb.Append("　");
				sb.AppendLine(string.Join(", ", p.FriendlySlots[i]
					.Concat(new[] { p.FriendlyExpansionSlots?[i] ?? -1 })
					.Select(id => KCDatabase.Instance.MasterEquipments[id])
					.Where(eq => eq != null)
					.Select(eq => eq.Name)));
			}
		}

		private static void OutputEnemyData(StringBuilder sb, ShipDataMaster[] members, int[] levels, int[] initialHPs, int[] maxHPs, EquipmentDataMaster[][] slots, int[][] parameters)
		{

			for (int i = 0; i < members.Length; i++)
			{
				if (members[i] == null)
					continue;

				sb.AppendFormat("#{0}: ID:{1} {2} {3} Lv. {4} HP: {5} / {6}",
					i + 1,
					members[i].ShipID,
					members[i].ShipTypeName, members[i].NameWithClass,
					levels[i],
					initialHPs[i], maxHPs[i]);

				if (parameters != null)
				{
					sb.AppendFormat(" - 火力{0}, 雷装{1}, 対空{2}, 装甲{3}",
					parameters[i][0], parameters[i][1], parameters[i][2], parameters[i][3]);
				}

				sb.AppendLine().Append("　");
				sb.AppendLine(string.Join(", ", slots[i].Where(eq => eq != null)));
			}
		}


		private static void OutputResultData(StringBuilder sb, int index, string name, int initialHP, int resultHP, int maxHP)
		{
			sb.AppendFormat("#{0}: {1} HP: ({2} → {3})/{4} ({5})\r\n",
				index + 1, name,
				Math.Max(initialHP, 0),
				Math.Max(resultHP, 0),
				Math.Max(maxHP, 0),
				resultHP - initialHP);
		}


		private static string GetBattleResult(BattleManager bm)
		{
			var result = bm.Result;

			var sb = new StringBuilder();


			sb.AppendLine("◆ 戦闘結果 ◆");
			sb.AppendFormat("ランク: {0}\r\n", result.Rank);

			if (bm.IsCombinedBattle)
			{
				sb.AppendFormat("MVP(主力艦隊): {0}\r\n",
					result.MVPIndex == -1 ? "(なし)" : bm.FirstBattle.Initial.FriendFleet.MembersInstance[result.MVPIndex - 1].NameWithLevel);
				sb.AppendFormat("MVP(随伴艦隊): {0}\r\n",
					result.MVPIndexCombined == -1 ? "(なし)" : bm.FirstBattle.Initial.FriendFleetEscort.MembersInstance[result.MVPIndexCombined - 1].NameWithLevel);

			}
			else
			{
				sb.AppendFormat("MVP: {0}\r\n",
					result.MVPIndex == -1 ? "(なし)" : bm.FirstBattle.Initial.FriendFleet.MembersInstance[result.MVPIndex - 1].NameWithLevel);
			}

			sb.AppendFormat("提督経験値: +{0}\r\n艦娘基本経験値: +{1}\r\n",
				result.AdmiralExp, result.BaseExp);


			if (!bm.IsPractice)
			{
				sb.AppendLine().AppendLine("ドロップ：");


				int length = sb.Length;

				var ship = KCDatabase.Instance.MasterShips[result.DroppedShipID];
				if (ship != null)
				{
					sb.AppendFormat("　{0} {1}\r\n", ship.ShipTypeName, ship.NameWithClass);
				}

				var eq = KCDatabase.Instance.MasterEquipments[result.DroppedEquipmentID];
				if (eq != null)
				{
					sb.AppendFormat("　{0} {1}\r\n", eq.CategoryTypeInstance.Name, eq.Name);
				}

				var item = KCDatabase.Instance.MasterUseItems[result.DroppedItemID];
				if (item != null)
				{
					sb.Append("　").AppendLine(item.Name);
				}

				if (length == sb.Length)
				{
					sb.AppendLine("　(なし)");
				}
			}


			return sb.ToString();
		}

	}
}
