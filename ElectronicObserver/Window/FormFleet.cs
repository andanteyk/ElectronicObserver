﻿using Codeplex.Data;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Support;
using SwfExtractor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

	public partial class FormFleet : DockContent
	{

		private bool IsRemodeling = false;


		private class TableFleetControl : IDisposable
		{
			public Label Name;
			public FleetState State;
			public ImageLabel AirSuperiority;
			public ImageLabel SearchingAbility;
			public ImageLabel AntiAirPower;
			public ToolTip ToolTipInfo;

			public int BranchWeight { get; private set; } = 1;

			public TableFleetControl(FormFleet parent)
			{

				#region Initialize

				Name = new Label
				{
					Text = "[" + parent.FleetID.ToString() + "]",
					Anchor = AnchorStyles.Left,
					ForeColor = parent.MainFontColor,
					UseMnemonic = false,
					Padding = new Padding(0, 1, 0, 1),
					Margin = new Padding(2, 0, 2, 0),
					AutoSize = true,
					//Name.Visible = false;
					Cursor = Cursors.Help
				};

				State = new FleetState
				{
					Anchor = AnchorStyles.Left,
					ForeColor = parent.MainFontColor,
					Padding = new Padding(),
					Margin = new Padding(),
					AutoSize = true
				};

				AirSuperiority = new ImageLabel
				{
					Anchor = AnchorStyles.Left,
					ForeColor = parent.MainFontColor,
					ImageList = ResourceManager.Instance.Equipments,
					ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter,
					Padding = new Padding(2, 2, 2, 2),
					Margin = new Padding(2, 0, 2, 0),
					AutoSize = true
				};

				SearchingAbility = new ImageLabel
				{
					Anchor = AnchorStyles.Left,
					ForeColor = parent.MainFontColor,
					ImageList = ResourceManager.Instance.Equipments,
					ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedRecon,
					Padding = new Padding(2, 2, 2, 2),
					Margin = new Padding(2, 0, 2, 0),
					AutoSize = true
				};
				SearchingAbility.Click += (sender, e) => SearchingAbility_Click(sender, e, parent.FleetID);

				AntiAirPower = new ImageLabel
				{
					Anchor = AnchorStyles.Left,
					ForeColor = parent.MainFontColor,
					ImageList = ResourceManager.Instance.Equipments,
					ImageIndex = (int)ResourceManager.EquipmentContent.HighAngleGun,
					Padding = new Padding(2, 2, 2, 2),
					Margin = new Padding(2, 0, 2, 0),
					AutoSize = true
				};


				ConfigurationChanged(parent);

				ToolTipInfo = parent.ToolTipInfo;

				#endregion

			}

			public TableFleetControl(FormFleet parent, TableLayoutPanel table)
				: this(parent)
			{
				AddToTable(table);
			}

			public void AddToTable(TableLayoutPanel table)
			{

				table.SuspendLayout();
				table.Controls.Add(Name, 0, 0);
				table.Controls.Add(State, 1, 0);
				table.Controls.Add(AirSuperiority, 2, 0);
				table.Controls.Add(SearchingAbility, 3, 0);
				table.Controls.Add(AntiAirPower, 4, 0);
				table.ResumeLayout();

			}

			private void SearchingAbility_Click(object sender, EventArgs e, int fleetID)
			{
				switch (BranchWeight)
				{
					case 1:
						BranchWeight = 4;
						break;
					case 4:
						BranchWeight = 3;
						break;
					case 3:
						BranchWeight = 1;
						break;
				}
				Update(KCDatabase.Instance.Fleet[fleetID]);
			}

			public void Update(FleetData fleet)
			{

				KCDatabase db = KCDatabase.Instance;

				if (fleet == null) return;



				Name.Text = fleet.Name;
				{
					var members = fleet.MembersInstance.Where(s => s != null);

					int levelSum = members.Sum(s => s.Level);

					int fueltotal = members.Sum(s => Math.Max((int)Math.Floor(s.FuelMax * (s.IsMarried ? 0.85 : 1.00)), 1));
					int ammototal = members.Sum(s => Math.Max((int)Math.Floor(s.AmmoMax * (s.IsMarried ? 0.85 : 1.00)), 1));

					int fuelunit = members.Sum(s => Math.Max((int)Math.Floor(s.FuelMax * 0.2 * (s.IsMarried ? 0.85 : 1.00)), 1));
					int ammounit = members.Sum(s => Math.Max((int)Math.Floor(s.AmmoMax * 0.2 * (s.IsMarried ? 0.85 : 1.00)), 1));

					int speed = members.Select(s => s.Speed).DefaultIfEmpty(20).Min();

					string supporttype;
					switch (fleet.SupportType)
					{
						case 0:
						default:
							supporttype = "発動不能"; break;
						case 1:
							supporttype = "航空支援";break;
						case 2:
							supporttype = "支援射撃"; break;
						case 3:
							supporttype = "支援長距離雷撃"; break;
					}

					double expeditionBonus = Calculator.GetExpeditionBonus(fleet);
					int tp = Calculator.GetTPDamage(fleet);

					// 各艦ごとの ドラム缶 or 大発系 を搭載している個数
					var transport = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.TransportContainer));
					var landing = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.LandingCraft || eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank));


					ToolTipInfo.SetToolTip(Name, string.Format(
						"Lv合計: {0} / 平均: {1:0.00}\r\n" +
						"{2}艦隊\r\n" +
						"支援攻撃: {3}\r\n" +
						"合計対空 {4} / 対潜 {5} / 索敵 {6}\r\n" +
						"ドラム缶搭載: {7}個 ({8}艦)\r\n" +
						"大発動艇搭載: {9}個 ({10}艦, +{11:p1})\r\n" +
						"輸送量(TP): S {12} / A {13}\r\n" +
						"総積載: 燃 {14} / 弾 {15}\r\n" +
						"(1戦当たり 燃 {16} / 弾 {17})",
						levelSum,
						(double)levelSum / Math.Max(fleet.Members.Count(id => id != -1), 1),
						Constants.GetSpeed(speed),
						supporttype,
						members.Sum(s => s.AATotal),
						members.Sum(s => s.ASWTotal),
						members.Sum(s => s.LOSTotal),
						transport.Sum(),
						transport.Count(i => i > 0),
						landing.Sum(),
						landing.Count(i => i > 0),
						expeditionBonus,
						tp,
						(int)(tp * 0.7),
						fueltotal,
						ammototal,
						fuelunit,
						ammounit
						));

				}


				State.UpdateFleetState(fleet, ToolTipInfo);


				//制空戦力計算	
				{
					int airSuperiority = fleet.GetAirSuperiority();
					bool includeLevel = Utility.Configuration.Config.FormFleet.AirSuperiorityMethod == 1;
					AirSuperiority.Text = fleet.GetAirSuperiorityString();
					ToolTipInfo.SetToolTip(AirSuperiority,
						string.Format("確保: {0}\r\n優勢: {1}\r\n均衡: {2}\r\n劣勢: {3}\r\n({4}: {5})\r\n",
						(int)(airSuperiority / 3.0),
						(int)(airSuperiority / 1.5),
						Math.Max((int)(airSuperiority * 1.5 - 1), 0),
						Math.Max((int)(airSuperiority * 3.0 - 1), 0),
						includeLevel ? "熟練度なし" : "熟練度あり",
						includeLevel ? Calculator.GetAirSuperiorityIgnoreLevel(fleet) : Calculator.GetAirSuperiority(fleet)));
				}


				//索敵能力計算
				SearchingAbility.Text = fleet.GetSearchingAbilityString(BranchWeight);
				{
					StringBuilder sb = new StringBuilder();
					double probStart = fleet.GetContactProbability();
					var probSelect = fleet.GetContactSelectionProbability();

					sb.AppendFormat("新判定式(33) 分岐点係数: {0}\r\n　(クリックで切り替え)\r\n\r\n触接開始率: \r\n　確保 {1:p1} / 優勢 {2:p1}\r\n",
						BranchWeight,
						probStart,
						probStart * 0.6);

					if (probSelect.Count > 0)
					{
						sb.AppendLine("触接選択率: ");

						foreach (var p in probSelect.OrderBy(p => p.Key))
						{
							sb.AppendFormat("　命中{0} : {1:p1}\r\n", p.Key, p.Value);
						}
					}

					ToolTipInfo.SetToolTip(SearchingAbility, sb.ToString());
				}

				// 対空能力計算
				{
					var sb = new StringBuilder();
					double lineahead = Calculator.GetAdjustedFleetAAValue(fleet, 1);

					AntiAirPower.Text = lineahead.ToString("0.0");

					sb.AppendFormat("艦隊防空\r\n単縦陣: {0:0.0} / 複縦陣: {1:0.0} / 輪形陣: {2:0.0}\r\n",
						lineahead,
						Calculator.GetAdjustedFleetAAValue(fleet, 2),
						Calculator.GetAdjustedFleetAAValue(fleet, 3));

					ToolTipInfo.SetToolTip(AntiAirPower, sb.ToString());
				}
			}


			public void Refresh()
			{

				State.RefreshFleetState();

			}

			public void ConfigurationChanged(FormFleet parent)
			{
				Name.Font = parent.MainFont;
				State.Font = parent.MainFont;
				State.RefreshFleetState();
				AirSuperiority.Font = parent.MainFont;
				SearchingAbility.Font = parent.MainFont;
				AntiAirPower.Font = parent.MainFont;

				ControlHelper.SetTableRowStyles(parent.TableFleet, ControlHelper.GetDefaultRowStyle());
			}

			public void Dispose()
			{
				Name.Dispose();
				State.Dispose();
				AirSuperiority.Dispose();
				SearchingAbility.Dispose();
				AntiAirPower.Dispose();
			}
		}


		private class TableMemberControl : IDisposable
		{
			public ImageLabel Name;
			public ShipStatusLevel Level;
			public ShipStatusHP HP;
			public ImageLabel Condition;
			public ShipStatusResource ShipResource;
			public ShipStatusEquipment Equipments;

			private ToolTip ToolTipInfo;
			private FormFleet Parent;


			public TableMemberControl(FormFleet parent)
			{

				#region Initialize

				Name = new ImageLabel();
				Name.SuspendLayout();
				Name.Text = "*nothing*";
				Name.Anchor = AnchorStyles.Left;
				Name.TextAlign = ContentAlignment.MiddleLeft;
				Name.ImageAlign = ContentAlignment.MiddleCenter;
				Name.ForeColor = parent.MainFontColor;
				Name.Padding = new Padding(2, 1, 2, 1);
				Name.Margin = new Padding(2, 1, 2, 1);
				Name.AutoSize = true;
				//Name.AutoEllipsis = true;
				Name.Visible = false;
				Name.Cursor = Cursors.Help;
				Name.MouseDown += Name_MouseDown;
				Name.ResumeLayout();

				Level = new ShipStatusLevel();
				Level.SuspendLayout();
				Level.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
				Level.Value = 0;
				Level.MaximumValue = ExpTable.ShipMaximumLevel;
				Level.ValueNext = 0;
				Level.MainFontColor = parent.MainFontColor;
				Level.SubFontColor = parent.SubFontColor;
				//Level.TextNext = "n.";
				Level.Padding = new Padding(0, 0, 0, 0);
				Level.Margin = new Padding(2, 0, 2, 1);
				Level.AutoSize = true;
				Level.Visible = false;
				Level.Cursor = Cursors.Help;
				Level.MouseDown += Level_MouseDown;
				Level.ResumeLayout();

				HP = new ShipStatusHP();
				HP.SuspendUpdate();
				HP.Anchor = AnchorStyles.Left;
				HP.Value = 0;
				HP.MaximumValue = 0;
				HP.MaximumDigit = 999;
				HP.UsePrevValue = false;
				HP.MainFontColor = parent.MainFontColor;
				HP.SubFontColor = parent.SubFontColor;
				HP.Padding = new Padding(0, 0, 0, 0);
				HP.Margin = new Padding(2, 1, 2, 2);
				HP.AutoSize = true;
				HP.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				HP.Visible = false;
				HP.ResumeUpdate();

				Condition = new ImageLabel();
				Condition.SuspendLayout();
				Condition.Text = "*";
				Condition.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				Condition.ForeColor = parent.MainFontColor;
				Condition.TextAlign = ContentAlignment.BottomRight;
				Condition.ImageAlign = ContentAlignment.MiddleLeft;
				Condition.ImageList = ResourceManager.Instance.Icons;
				Condition.Padding = new Padding(2, 1, 2, 1);
				Condition.Margin = new Padding(2, 1, 2, 1);
				Condition.Size = new Size(40, 20);
				Condition.AutoSize = true;
				Condition.Visible = false;
				Condition.ResumeLayout();

				ShipResource = new ShipStatusResource(parent.ToolTipInfo);
				ShipResource.SuspendLayout();
				ShipResource.FuelCurrent = 0;
				ShipResource.FuelMax = 0;
				ShipResource.AmmoCurrent = 0;
				ShipResource.AmmoMax = 0;
				ShipResource.Anchor = AnchorStyles.Left;
				ShipResource.Padding = new Padding(0, 2, 0, 0);
				ShipResource.Margin = new Padding(2, 0, 2, 1);
				ShipResource.Size = new Size(30, 20);
				ShipResource.AutoSize = false;
				ShipResource.Visible = false;
				ShipResource.ResumeLayout();

				Equipments = new ShipStatusEquipment();
				Equipments.SuspendUpdate();
				Equipments.Anchor = AnchorStyles.Left;
				Equipments.Padding = new Padding(0, 1, 0, 1);
				Equipments.Margin = new Padding(2, 0, 2, 1);
				Equipments.Size = new Size(40, 20);
				Equipments.AutoSize = true;
				Equipments.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				Equipments.Visible = false;
				Equipments.ResumeUpdate();

				ConfigurationChanged(parent);

				ToolTipInfo = parent.ToolTipInfo;
				Parent = parent;
				#endregion

			}

			public TableMemberControl(FormFleet parent, TableLayoutPanel table, int row)
				: this(parent)
			{
				AddToTable(table, row);

				Equipments.Name = string.Format("{0}_{1}", parent.FleetID, row + 1);
			}


			public void AddToTable(TableLayoutPanel table, int row)
			{

				table.SuspendLayout();

				table.Controls.Add(Name, 0, row);
				table.Controls.Add(Level, 1, row);
				table.Controls.Add(HP, 2, row);
				table.Controls.Add(Condition, 3, row);
				table.Controls.Add(ShipResource, 4, row);
				table.Controls.Add(Equipments, 5, row);

				table.ResumeLayout();

			}

			public void Update(int shipMasterID)
			{

				KCDatabase db = KCDatabase.Instance;
				ShipData ship = db.Ships[shipMasterID];

				if (ship != null)
				{

					bool isEscaped = KCDatabase.Instance.Fleet[Parent.FleetID].EscapedShipList.Contains(shipMasterID);


					Name.Text = ship.MasterShip.NameWithClass;
					Name.Tag = ship.ShipID;
					ToolTipInfo.SetToolTip(Name,
						string.Format(
							"{0} {1}\r\n火力: {2}/{3}\r\n雷装: {4}/{5}\r\n対空: {6}/{7}\r\n装甲: {8}/{9}\r\n対潜: {10}/{11}\r\n回避: {12}/{13}\r\n索敵: {14}/{15}\r\n運: {16}\r\n射程: {17} / 速力: {18}\r\n(右クリックで図鑑)\n",
							ship.MasterShip.ShipTypeName, ship.NameWithLevel,
							ship.FirepowerBase, ship.FirepowerTotal,
							ship.TorpedoBase, ship.TorpedoTotal,
							ship.AABase, ship.AATotal,
							ship.ArmorBase, ship.ArmorTotal,
							ship.ASWBase, ship.ASWTotal,
							ship.EvasionBase, ship.EvasionTotal,
							ship.LOSBase, ship.LOSTotal,
							ship.LuckTotal,
							Constants.GetRange(ship.Range),
							Constants.GetSpeed(ship.Speed)
							));


					Level.Value = ship.Level;
					Level.ValueNext = ship.ExpNext;
					Level.Tag = ship.MasterID;

					{
						StringBuilder tip = new StringBuilder();
						tip.AppendFormat("Total: {0} exp.\r\n", ship.ExpTotal);

						if (!Utility.Configuration.Config.FormFleet.ShowNextExp)
							tip.AppendFormat("次のレベルまで: {0} exp.\r\n", ship.ExpNext);

						if (ship.MasterShip.RemodelAfterShipID != 0 && ship.Level < ship.MasterShip.RemodelAfterLevel)
						{
							tip.AppendFormat("改装まで: Lv. {0} / {1} exp.\r\n", ship.MasterShip.RemodelAfterLevel - ship.Level, ship.ExpNextRemodel);
						}
						else if (ship.Level <= 99)
						{
							tip.AppendFormat("Lv99まで: {0} exp.\r\n", Math.Max(ExpTable.GetExpToLevelShip(ship.ExpTotal, 99), 0));
						}
						else
						{
							tip.AppendFormat("Lv{0}まで: {1} exp.\r\n", ExpTable.ShipMaximumLevel, Math.Max(ExpTable.GetExpToLevelShip(ship.ExpTotal, ExpTable.ShipMaximumLevel), 0));
						}

						tip.AppendLine("(右クリックで必要Exp計算)");

						ToolTipInfo.SetToolTip(Level, tip.ToString());
					}


					HP.SuspendUpdate();
					HP.Value = HP.PrevValue = ship.HPCurrent;
					HP.MaximumValue = ship.HPMax;
					HP.UsePrevValue = false;
					HP.ShowDifference = false;
					{
						int dockID = ship.RepairingDockID;

						if (dockID != -1)
						{
							HP.RepairTime = db.Docks[dockID].CompletionTime;
							HP.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.Visible;
						}
						else
						{
							HP.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.Invisible;
						}
					}
					HP.Tag = (ship.RepairingDockID == -1 && 0.5 < ship.HPRate && ship.HPRate < 1.0) ? DateTimeHelper.FromAPITimeSpan(ship.RepairTime).TotalSeconds : 0.0;
					if (isEscaped)
					{
						HP.BackColor = Color.Silver;
					}
					else
					{
						HP.BackColor = SystemColors.Control;
					}
					{
						StringBuilder sb = new StringBuilder();
						double hprate = (double)ship.HPCurrent / ship.HPMax;

						sb.AppendFormat("HP: {0:0.0}% [{1}]\n", hprate * 100, Constants.GetDamageState(hprate));
						if (isEscaped)
						{
							sb.AppendLine("退避中");
						}
						else if (hprate > 0.50)
						{
							sb.AppendFormat("中破まで: {0} / 大破まで: {1}\n", ship.HPCurrent - ship.HPMax / 2, ship.HPCurrent - ship.HPMax / 4);
						}
						else if (hprate > 0.25)
						{
							sb.AppendFormat("大破まで: {0}\n", ship.HPCurrent - ship.HPMax / 4);
						}
						else
						{
							sb.AppendLine("大破しています！");
						}

						if (ship.RepairTime > 0)
						{
							var span = DateTimeHelper.FromAPITimeSpan(ship.RepairTime);
							sb.AppendFormat("入渠時間: {0} @ {1}",
								DateTimeHelper.ToTimeRemainString(span),
								DateTimeHelper.ToTimeRemainString(Calculator.CalculateDockingUnitTime(ship)));
						}

						ToolTipInfo.SetToolTip(HP, sb.ToString());
					}
					HP.ResumeUpdate();


					Condition.Text = ship.Condition.ToString();
					Condition.Tag = ship.Condition;
					SetConditionDesign(ship.Condition);

					if (ship.Condition < 49)
					{
						TimeSpan ts = new TimeSpan(0, (int)Math.Ceiling((49 - ship.Condition) / 3.0) * 3, 0);
						ToolTipInfo.SetToolTip(Condition, string.Format("完全回復まで 約 {0:D2}:{1:D2}", (int)ts.TotalMinutes, (int)ts.Seconds));
					}
					else
					{
						ToolTipInfo.SetToolTip(Condition, string.Format("あと {0} 回遠征可能", (int)Math.Ceiling((ship.Condition - 49) / 3.0)));
					}

					ShipResource.SetResources(ship.Fuel, ship.FuelMax, ship.Ammo, ship.AmmoMax);


					Equipments.SetSlotList(ship);
					ToolTipInfo.SetToolTip(Equipments, GetEquipmentString(ship));

				}
				else
				{
					Name.Tag = -1;
				}


				Name.Visible =
				Level.Visible =
				HP.Visible =
				Condition.Visible =
				ShipResource.Visible =
				Equipments.Visible = shipMasterID != -1;

			}

			void Name_MouseDown(object sender, MouseEventArgs e)
			{
				if (Name.Tag is int id && id != -1)
				{
					if ((e.Button & MouseButtons.Right) != 0)
					{
						new DialogAlbumMasterShip(id).Show(Parent);
					}
				}
			}

			private void Level_MouseDown(object sender, MouseEventArgs e)
			{
				if (Level.Tag is int id && id != -1)
				{
					if ((e.Button & MouseButtons.Right) != 0)
					{
						new DialogExpChecker(id).Show(Parent);
					}
				}
			}


			private string GetEquipmentString(ShipData ship)
			{
				StringBuilder sb = new StringBuilder();

				for (int i = 0; i < ship.Slot.Count; i++)
				{
					var eq = ship.SlotInstance[i];
					if (eq != null)
						sb.AppendFormat("[{0}/{1}] {2}\r\n", ship.Aircraft[i], ship.MasterShip.Aircraft[i], eq.NameWithLevel);
				}

				{
					var exslot = ship.ExpansionSlotInstance;
					if (exslot != null)
						sb.AppendFormat("補強: {0}\r\n", exslot.NameWithLevel);
				}

				int[] slotmaster = ship.AllSlotMaster.ToArray();

				sb.AppendFormat("\r\n昼戦: {0}", Constants.GetDayAttackKind(Calculator.GetDayAttackKind(slotmaster, ship.ShipID, -1)));
				{
					int shelling = ship.ShellingPower;
					int aircraft = ship.AircraftPower;
					if (shelling > 0)
					{
						if (aircraft > 0)
							sb.AppendFormat(" - 砲撃: {0} / 空撃: {1}", shelling, aircraft);
						else
							sb.AppendFormat(" - 威力: {0}", shelling);
					}
					else if (aircraft > 0)
						sb.AppendFormat(" - 威力: {0}", aircraft);
				}
				sb.AppendLine();

				if (ship.CanAttackAtNight)
				{
					sb.AppendFormat("夜戦: {0}", Constants.GetNightAttackKind(Calculator.GetNightAttackKind(slotmaster, ship.ShipID, -1)));
					{
						int night = ship.NightBattlePower;
						if (night > 0)
						{
							sb.AppendFormat(" - 威力: {0}", night);
						}
					}
					sb.AppendLine();
				}

				{
					int torpedo = ship.TorpedoPower;
					int asw = ship.AntiSubmarinePower;

					if (torpedo > 0)
					{
						sb.AppendFormat("雷撃: {0}", torpedo);
					}
					if (asw > 0)
					{
						if (torpedo > 0)
							sb.Append(" / ");

						sb.AppendFormat("対潜: {0}", asw);

						if (ship.CanOpeningASW)
							sb.Append(" (先制可能)");
					}
					if (torpedo > 0 || asw > 0)
						sb.AppendLine();
				}

				{
					int aacutin = Calculator.GetAACutinKind(ship.ShipID, slotmaster);
					if (aacutin != 0)
					{
						sb.AppendFormat("対空: {0}\r\n", Constants.GetAACutinKind(aacutin));
					}
					double adjustedaa = Calculator.GetAdjustedAAValue(ship);
					sb.AppendFormat("加重対空: {0} (割合撃墜: {1:p2})\r\n",
						adjustedaa,
						Calculator.GetProportionalAirDefense(adjustedaa)
						);

				}

				{
					int airsup_min;
					int airsup_max;
					if (Utility.Configuration.Config.FormFleet.AirSuperiorityMethod == 1)
					{
						airsup_min = Calculator.GetAirSuperiority(ship, false);
						airsup_max = Calculator.GetAirSuperiority(ship, true);
					}
					else
					{
						airsup_min = airsup_max = Calculator.GetAirSuperiorityIgnoreLevel(ship);
					}

					int airbattle = ship.AirBattlePower;
					if (airsup_min > 0)
					{

						string airsup_str;
						if (Utility.Configuration.Config.FormFleet.ShowAirSuperiorityRange && airsup_min < airsup_max)
						{
							airsup_str = string.Format("{0} ～ {1}", airsup_min, airsup_max);
						}
						else
						{
							airsup_str = airsup_min.ToString();
						}

						if (airbattle > 0)
							sb.AppendFormat("制空戦力: {0} / 航空威力: {1}\r\n", airsup_str, airbattle);
						else
							sb.AppendFormat("制空戦力: {0}\r\n", airsup_str);
					}
					else if (airbattle > 0)
						sb.AppendFormat("航空威力: {0}\r\n", airbattle);
				}

				return sb.ToString();
			}

			private void SetConditionDesign(int cond)
			{

				if (Condition.ImageAlign == ContentAlignment.MiddleCenter)
				{
					// icon invisible
					Condition.ImageIndex = -1;

					if (cond < 20)
						Condition.BackColor = Color.LightCoral;
					else if (cond < 30)
						Condition.BackColor = Color.LightSalmon;
					else if (cond < 40)
						Condition.BackColor = Color.Moccasin;
					else if (cond < 50)
						Condition.BackColor = Color.Transparent;
					else
						Condition.BackColor = Color.LightGreen;

				}
				else
				{
					Condition.BackColor = Color.Transparent;

					if (cond < 20)
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionVeryTired;
					else if (cond < 30)
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionTired;
					else if (cond < 40)
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionLittleTired;
					else if (cond < 50)
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionNormal;
					else
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionSparkle;

				}
			}

			public void ConfigurationChanged(FormFleet parent)
			{
				Name.Font = parent.MainFont;
				Level.MainFont = parent.MainFont;
				Level.SubFont = parent.SubFont;
				HP.MainFont = parent.MainFont;
				HP.SubFont = parent.SubFont;
				Condition.Font = parent.MainFont;
				SetConditionDesign((Condition.Tag as int?) ?? 49);
				Equipments.Font = parent.SubFont;
			}

			public void Dispose()
			{
				Name.Dispose();
				Level.Dispose();
				HP.Dispose();
				Condition.Dispose();
				ShipResource.Dispose();
				Equipments.Dispose();

			}
		}




		public int FleetID { get; private set; }


		public Font MainFont { get; set; }
		public Font SubFont { get; set; }
		public Color MainFontColor { get; set; }
		public Color SubFontColor { get; set; }


		private TableFleetControl ControlFleet;
		private TableMemberControl[] ControlMember;

		private int AnchorageRepairBound;


		public FormFleet(FormMain parent, int fleetID)
		{
			InitializeComponent();

			FleetID = fleetID;
			Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;

			ConfigurationChanged();

			MainFontColor = Color.FromArgb(0x00, 0x00, 0x00);
			SubFontColor = Color.FromArgb(0x88, 0x88, 0x88);

			AnchorageRepairBound = 0;

			//ui init

			ControlHelper.SetDoubleBuffered(TableFleet);
			ControlHelper.SetDoubleBuffered(TableMember);


			TableFleet.Visible = false;
			TableFleet.SuspendLayout();
			TableFleet.BorderStyle = BorderStyle.FixedSingle;
			ControlFleet = new TableFleetControl(this, TableFleet);
			TableFleet.ResumeLayout();


			TableMember.SuspendLayout();
			ControlMember = new TableMemberControl[7];
			for (int i = 0; i < ControlMember.Length; i++)
			{
				ControlMember[i] = new TableMemberControl(this, TableMember, i);
			}
			TableMember.ResumeLayout();


			ConfigurationChanged();     //fixme: 苦渋の決断

			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet]);

		}



		private void FormFleet_Load(object sender, EventArgs e)
		{

			Text = string.Format("#{0}", FleetID);

			APIObserver o = APIObserver.Instance;

			o["api_req_nyukyo/start"].RequestReceived += Updated;
			o["api_req_nyukyo/speedchange"].RequestReceived += Updated;
			o["api_req_hensei/change"].RequestReceived += Updated;
			o["api_req_kousyou/destroyship"].RequestReceived += Updated;
			o["api_req_member/updatedeckname"].RequestReceived += Updated;
			o["api_req_kaisou/remodeling"].RequestReceived += Updated;
			o["api_req_map/start"].RequestReceived += Updated;
			o["api_req_hensei/combined"].RequestReceived += Updated;

			o["api_port/port"].ResponseReceived += Updated;
			o["api_get_member/ship2"].ResponseReceived += Updated;
			o["api_get_member/ndock"].ResponseReceived += Updated;
			o["api_req_kousyou/getship"].ResponseReceived += Updated;
			o["api_req_hokyu/charge"].ResponseReceived += Updated;
			o["api_req_kousyou/destroyship"].ResponseReceived += Updated;
			o["api_get_member/ship3"].ResponseReceived += Updated;
			o["api_req_kaisou/powerup"].ResponseReceived += Updated;        //requestのほうは面倒なのでこちらでまとめてやる
			o["api_get_member/deck"].ResponseReceived += Updated;
			o["api_get_member/slot_item"].ResponseReceived += Updated;
			o["api_req_map/start"].ResponseReceived += Updated;
			o["api_req_map/next"].ResponseReceived += Updated;
			o["api_get_member/ship_deck"].ResponseReceived += Updated;
			o["api_req_hensei/preset_select"].ResponseReceived += Updated;
			o["api_req_kaisou/slot_exchange_index"].ResponseReceived += Updated;
			o["api_get_member/require_info"].ResponseReceived += Updated;
			o["api_req_kaisou/slot_deprive"].ResponseReceived += Updated;


			//追加するときは FormFleetOverview にも同様に追加してください

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}


		void Updated(string apiname, dynamic data)
		{

			if (IsRemodeling)
			{
				if (apiname == "api_get_member/slot_item")
					IsRemodeling = false;
				else
					return;
			}
			if (apiname == "api_req_kaisou/remodeling")
			{
				IsRemodeling = true;
				return;
			}

			KCDatabase db = KCDatabase.Instance;

			if (db.Ships.Count == 0) return;

			FleetData fleet = db.Fleet.Fleets[FleetID];
			if (fleet == null) return;

			TableFleet.SuspendLayout();
			ControlFleet.Update(fleet);
			TableFleet.Visible = true;
			TableFleet.ResumeLayout();

			AnchorageRepairBound = fleet.CanAnchorageRepair ? 2 + fleet.MembersInstance[0].SlotInstance.Count(eq => eq != null && eq.MasterEquipment.CategoryType == EquipmentTypes.RepairFacility) : 0;

			TableMember.SuspendLayout();
			TableMember.RowCount = fleet.Members.Count(id => id > 0);
			for (int i = 0; i < ControlMember.Length; i++)
			{
				ControlMember[i].Update(i < fleet.Members.Count ? fleet.Members[i] : -1);
			}
			TableMember.ResumeLayout();


			if (Icon != null) ResourceManager.DestroyIcon(Icon);
			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[ControlFleet.State.GetIconIndex()]);
			if (Parent != null) Parent.Refresh();       //アイコンを更新するため

		}


		void UpdateTimerTick()
		{

			FleetData fleet = KCDatabase.Instance.Fleet.Fleets[FleetID];

			TableFleet.SuspendLayout();
			{
				if (fleet != null)
					ControlFleet.Refresh();

			}
			TableFleet.ResumeLayout();

			TableMember.SuspendLayout();
			for (int i = 0; i < ControlMember.Length; i++)
			{
				ControlMember[i].HP.Refresh();
			}
			TableMember.ResumeLayout();


			// anchorage repairing
			if (fleet != null && Utility.Configuration.Config.FormFleet.ReflectAnchorageRepairHealing)
			{
				TimeSpan elapsed = DateTime.Now - KCDatabase.Instance.Fleet.AnchorageRepairingTimer;

				if (elapsed.TotalMinutes >= 20 && AnchorageRepairBound > 0)
				{

					for (int i = 0; i < AnchorageRepairBound; i++)
					{
						var hpbar = ControlMember[i].HP;

						double dockingSeconds = hpbar.Tag as double? ?? 0.0;

						if (dockingSeconds <= 0.0)
							continue;

						hpbar.SuspendUpdate();

						if (!hpbar.UsePrevValue)
						{
							hpbar.UsePrevValue = true;
							hpbar.ShowDifference = true;
						}

						int damage = hpbar.MaximumValue - hpbar.PrevValue;
						int healAmount = Math.Min(Calculator.CalculateAnchorageRepairHealAmount(damage, dockingSeconds, elapsed), damage);

						hpbar.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.MouseOver;
						hpbar.RepairTime = KCDatabase.Instance.Fleet.AnchorageRepairingTimer + Calculator.CalculateAnchorageRepairTime(damage, dockingSeconds, Math.Min(healAmount + 1, damage));
						hpbar.Value = hpbar.PrevValue + healAmount;

						hpbar.ResumeUpdate();
					}
				}
			}
		}


		//艦隊編成のコピー
		private void ContextMenuFleet_CopyFleet_Click(object sender, EventArgs e)
		{

			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;
			FleetData fleet = db.Fleet[FleetID];
			if (fleet == null) return;

			sb.AppendFormat("{0}\t制空戦力{1} / 索敵能力 {2} / 輸送能力 {3}\r\n", fleet.Name, fleet.GetAirSuperiority(), fleet.GetSearchingAbilityString(ControlFleet.BranchWeight), Calculator.GetTPDamage(fleet));
			for (int i = 0; i < fleet.Members.Count; i++)
			{
				if (fleet[i] == -1)
					continue;

				ShipData ship = db.Ships[fleet[i]];

				sb.AppendFormat("{0}/{1}\t", ship.MasterShip.Name, ship.Level);

				var eq = ship.AllSlotInstance;


				if (eq != null)
				{
					for (int j = 0; j < eq.Count; j++)
					{

						if (eq[j] == null) continue;

						int count = 1;
						for (int k = j + 1; k < eq.Count; k++)
						{
							if (eq[k] != null && eq[k].EquipmentID == eq[j].EquipmentID && eq[k].Level == eq[j].Level && eq[k].AircraftLevel == eq[j].AircraftLevel)
							{
								count++;
							}
							else
							{
								break;
							}
						}

						if (count == 1)
						{
							sb.AppendFormat("{0}{1}", j == 0 ? "" : ", ", eq[j].NameWithLevel);
						}
						else
						{
							sb.AppendFormat("{0}{1}x{2}", j == 0 ? "" : ", ", eq[j].NameWithLevel, count);
						}

						j += count - 1;
					}
				}

				sb.AppendLine();
			}


			Clipboard.SetData(DataFormats.StringFormat, sb.ToString());
		}


		private void ContextMenuFleet_Opening(object sender, CancelEventArgs e)
		{

			ContextMenuFleet_Capture.Visible = Utility.Configuration.Config.Debug.EnableDebugMenu;

		}



		/// <summary>
		/// 「艦隊デッキビルダー」用編成コピー
		/// <see cref="http://www.kancolle-calc.net/deckbuilder.html"/>
		/// </summary>
		private void ContextMenuFleet_CopyFleetDeckBuilder_Click(object sender, EventArgs e)
		{

			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;

			// 手書き json の悲しみ

			sb.Append(@"{""version"":4,");

			foreach (var fleet in db.Fleet.Fleets.Values)
			{
				if (fleet == null || fleet.MembersInstance.All(m => m == null)) continue;

				sb.AppendFormat(@"""f{0}"":{{", fleet.FleetID);

				int shipcount = 1;
				foreach (var ship in fleet.MembersInstance)
				{
					if (ship == null) break;

					sb.AppendFormat(@"""s{0}"":{{""id"":{1},""lv"":{2},""luck"":{3},""items"":{{",
						shipcount,
						ship.ShipID,
						ship.Level,
						ship.LuckBase);

					int eqcount = 1;
					foreach (var eq in ship.AllSlotInstance.Where(eq => eq != null))
					{
						if (eq == null) break;

						sb.AppendFormat(@"""i{0}"":{{""id"":{1},""rf"":{2},""mas"":{3}}},", eqcount >= 6 ? "x" : eqcount.ToString(), eq.EquipmentID, eq.Level, eq.AircraftLevel);

						eqcount++;
					}

					if (eqcount > 1)
						sb.Remove(sb.Length - 1, 1);        // remove ","
					sb.Append(@"}},");

					shipcount++;
				}

				if (shipcount > 0)
					sb.Remove(sb.Length - 1, 1);        // remove ","
				sb.Append(@"},");

			}

			sb.Remove(sb.Length - 1, 1);        // remove ","
			sb.Append(@"}");

			Clipboard.SetData(DataFormats.StringFormat, sb.ToString());
		}


		/// <summary>
		/// 「艦隊晒しページ」用編成コピー
		/// <see cref="http://kancolle-calc.net/kanmusu_list.html"/>
		/// </summary>
		private void ContextMenuFleet_CopyKanmusuList_Click(object sender, EventArgs e)
		{

			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;

			// version
			sb.Append(".2");

			// <たね艦娘(完全未改造時)のID, 艦娘リスト>　に分類
			Dictionary<int, List<ShipData>> shiplist = new Dictionary<int, List<ShipData>>();

			foreach (var ship in db.Ships.Values.Where(s => s.IsLocked))
			{
				var master = ship.MasterShip;
				while (master.RemodelBeforeShip != null)
					master = master.RemodelBeforeShip;

				if (!shiplist.ContainsKey(master.ShipID))
				{
					shiplist.Add(master.ShipID, new List<ShipData>() { ship });
				}
				else
				{
					shiplist[master.ShipID].Add(ship);
				}
			}

			// 上で作った分類の各項を文字列化
			foreach (var sl in shiplist)
			{
				sb.Append("|").Append(sl.Key).Append(":");

				foreach (var ship in sl.Value.OrderByDescending(s => s.Level))
				{
					sb.Append(ship.Level);

					// 改造レベルに達しているのに未改造の艦は ".<たね=1, 改=2, 改二=3, ...>" を付加
					if (ship.MasterShip.RemodelAfterShipID != 0 && ship.ExpNextRemodel == 0)
					{
						sb.Append(".");
						int count = 1;
						var master = ship.MasterShip;
						while (master.RemodelBeforeShip != null)
						{
							master = master.RemodelBeforeShip;
							count++;
						}
						sb.Append(count);
					}
					sb.Append(",");
				}

				// 余った "," を削除
				sb.Remove(sb.Length - 1, 1);
			}

			Clipboard.SetData(DataFormats.StringFormat, sb.ToString());
		}


		private void ContextMenuFleet_AntiAirDetails_Click(object sender, EventArgs e)
		{

			var dialog = new DialogAntiAirDefense();

			dialog.SetFleetID(FleetID);
			dialog.Show(this);

		}


		private void ContextMenuFleet_Capture_Click(object sender, EventArgs e)
		{

			using (Bitmap bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height))
			{
				this.DrawToBitmap(bitmap, this.ClientRectangle);

				Clipboard.SetData(DataFormats.Bitmap, bitmap);
			}
		}


		private void ContextMenuFleet_OutputFleetImage_Click(object sender, EventArgs e)
		{

			using (var dialog = new DialogFleetImageGenerator(FleetID))
			{
				dialog.ShowDialog(this);
			}
		}



		void ConfigurationChanged()
		{

			var c = Utility.Configuration.Config;

			MainFont = Font = c.UI.MainFont;
			SubFont = c.UI.SubFont;

			AutoScroll = c.FormFleet.IsScrollable;

			var fleet = KCDatabase.Instance.Fleet[FleetID];

			TableFleet.SuspendLayout();
			if (ControlFleet != null && fleet != null)
			{
				ControlFleet.ConfigurationChanged(this);
				ControlFleet.Update(fleet);
			}
			TableFleet.ResumeLayout();

			TableMember.SuspendLayout();
			if (ControlMember != null)
			{
				bool showAircraft = c.FormFleet.ShowAircraft;
				bool fixShipNameWidth = c.FormFleet.FixShipNameWidth;
				bool shortHPBar = c.FormFleet.ShortenHPBar;
				bool colorMorphing = c.UI.BarColorMorphing;
				Color[] colorScheme = c.UI.BarColorScheme.Select(col => col.ColorData).ToArray();
				bool showNext = c.FormFleet.ShowNextExp;
				bool showConditionIcon = c.FormFleet.ShowConditionIcon;
				var levelVisibility = c.FormFleet.EquipmentLevelVisibility;
				bool showAircraftLevelByNumber = c.FormFleet.ShowAircraftLevelByNumber;
				int fixedShipNameWidth = c.FormFleet.FixedShipNameWidth;
				bool isLayoutFixed = c.UI.IsLayoutFixed;

				for (int i = 0; i < ControlMember.Length; i++)
				{
					var member = ControlMember[i];

					member.Equipments.ShowAircraft = showAircraft;
					if (fixShipNameWidth)
					{
						member.Name.AutoSize = false;
						member.Name.Size = new Size(fixedShipNameWidth, 20);
					}
					else
					{
						member.Name.AutoSize = true;
					}

					member.HP.SuspendUpdate();
					member.HP.Text = shortHPBar ? "" : "HP:";
					member.HP.HPBar.ColorMorphing = colorMorphing;
					member.HP.HPBar.SetBarColorScheme(colorScheme);
					member.HP.MaximumSize = isLayoutFixed ? new Size(int.MaxValue, (int)ControlHelper.GetDefaultRowStyle().Height - member.HP.Margin.Vertical) : Size.Empty;
					member.HP.ResumeUpdate();
					member.Level.TextNext = showNext ? "next:" : null;
					member.Condition.ImageAlign = showConditionIcon ? ContentAlignment.MiddleLeft : ContentAlignment.MiddleCenter;
					member.Equipments.LevelVisibility = levelVisibility;
					member.Equipments.ShowAircraftLevelByNumber = showAircraftLevelByNumber;
					member.Equipments.MaximumSize = isLayoutFixed ? new Size(int.MaxValue, (int)ControlHelper.GetDefaultRowStyle().Height - member.Equipments.Margin.Vertical) : Size.Empty;
					member.ShipResource.BarFuel.ColorMorphing =
					member.ShipResource.BarAmmo.ColorMorphing = colorMorphing;
					member.ShipResource.BarFuel.SetBarColorScheme(colorScheme);
					member.ShipResource.BarAmmo.SetBarColorScheme(colorScheme);

					member.ConfigurationChanged(this);
					if (fleet != null)
						member.Update(i < fleet.Members.Count ? fleet.Members[i] : -1);
				}
			}

			ControlHelper.SetTableRowStyles(TableMember, ControlHelper.GetDefaultRowStyle());
			TableMember.ResumeLayout();

			TableMember.Location = new Point(TableMember.Location.X, TableFleet.Bottom /*+ Math.Max( TableFleet.Margin.Bottom, TableMember.Margin.Top )*/ );

			TableMember.PerformLayout();        //fixme:サイズ変更に親パネルが追随しない

		}



		private void TableMember_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
		}


		protected override string GetPersistString()
		{
			return "Fleet #" + FleetID.ToString();
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			ControlFleet.Dispose();
			for (int i = 0; i < ControlMember.Length; i++)
				ControlMember[i].Dispose();


			// --- auto generated ---
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}
	}

}
