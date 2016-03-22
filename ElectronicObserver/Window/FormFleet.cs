using Codeplex.Data;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Support;
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

namespace ElectronicObserver.Window {

	public partial class FormFleet : DockContent {

		private bool IsRemodeling = false;


		private class TableFleetControl {
			public Label Name;
			public ImageLabel StateMain;
			public ImageLabel AirSuperiority;
			public ImageLabel SearchingAbility;
			public ImageLabel AAValue;
			public ToolTip ToolTipInfo;
			public ElectronicObserver.Data.FleetData.FleetStates State;
			public DateTime Timer;

			public TableFleetControl( FormFleet parent ) {

				#region Initialize

				Name = new Label();
				Name.Text = "[" + parent.FleetID.ToString() + "]";
				Name.Anchor = AnchorStyles.Left;
				Name.ForeColor = parent.MainFontColor;
				Name.Padding = new Padding( 0, 1, 0, 1 );
				Name.Margin = new Padding( 2, 0, 2, 0 );
				Name.AutoSize = true;
				//Name.Visible = false;

				StateMain = new ImageLabel();
				StateMain.Anchor = AnchorStyles.Left;
				StateMain.ForeColor = parent.MainFontColor;
				StateMain.ImageList = ResourceManager.Instance.Icons;
				StateMain.Padding = new Padding( 2, 2, 2, 2 );
				StateMain.Margin = new Padding( 2, 0, 2, 0 );
				StateMain.AutoSize = true;

				AirSuperiority = new ImageLabel();
				AirSuperiority.Anchor = AnchorStyles.Left;
				AirSuperiority.ForeColor = parent.MainFontColor;
				AirSuperiority.ImageList = ResourceManager.Instance.Equipments;
				AirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;
				AirSuperiority.Padding = new Padding( 2, 2, 2, 2 );
				AirSuperiority.Margin = new Padding( 2, 0, 2, 0 );
				AirSuperiority.AutoSize = true;

				SearchingAbility = new ImageLabel();
				SearchingAbility.Anchor = AnchorStyles.Left;
				SearchingAbility.ForeColor = parent.MainFontColor;
				SearchingAbility.ImageList = ResourceManager.Instance.Equipments;
				SearchingAbility.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedRecon;
				SearchingAbility.Padding = new Padding( 2, 2, 2, 2 );
				SearchingAbility.Margin = new Padding( 2, 0, 2, 0 );
				SearchingAbility.AutoSize = true;

				AAValue = new ImageLabel();
				AAValue.Anchor = AnchorStyles.Left;
				AAValue.Font = parent.MainFont;
				AAValue.ForeColor = parent.MainFontColor;
				AAValue.ImageList = ResourceManager.Instance.Equipments;
				AAValue.ImageIndex = (int)ResourceManager.EquipmentContent.AADirector;
				AAValue.Padding = new Padding( 2, 2, 2, 2 );
				AAValue.Margin = new Padding( 2, 0, 2, 0 );
				AAValue.AutoSize = true;

				ConfigurationChanged( parent );
				ToolTipInfo = parent.ToolTipInfo;
				State = FleetData.FleetStates.NoShip;
				Timer = DateTime.Now;

				#endregion

			}

			public TableFleetControl( FormFleet parent, TableLayoutPanel table )
				: this( parent ) {
				AddToTable( table );
			}

			public void AddToTable( TableLayoutPanel table ) {

				table.SuspendLayout();
				table.Controls.Add( Name, 0, 0 );
				table.Controls.Add( StateMain, 1, 0 );
				table.Controls.Add( AirSuperiority, 2, 0 );
				table.Controls.Add( SearchingAbility, 3, 0 );
				table.Controls.Add( AAValue, 4, 0 );
				table.ResumeLayout();

				int row = 0;
				#region set RowStyle
				RowStyle rs = new RowStyle( SizeType.Absolute, 21 );

				if ( table.RowStyles.Count > row )
					table.RowStyles[row] = rs;
				else
					while ( table.RowStyles.Count <= row )
						table.RowStyles.Add( rs );
				#endregion
			}

            public void Update(FleetData fleet)
            {

                KCDatabase db = KCDatabase.Instance;

                if (fleet == null) return;



                Name.Text = fleet.Name;
                {
                    int levelSum = fleet.MembersInstance.Sum(s => s != null ? s.Level : 0);

                    int fueltotal = fleet.MembersInstance.Sum(s => s == null ? 0 : (int)Math.Floor(s.FuelMax * (s.IsMarried ? 0.85 : 1.00)));
                    int ammototal = fleet.MembersInstance.Sum(s => s == null ? 0 : (int)Math.Floor(s.AmmoMax * (s.IsMarried ? 0.85 : 1.00)));

                    int fuelunit = fleet.MembersInstance.Sum(s => s == null ? 0 : (int)Math.Floor(s.MasterShip.Fuel * 0.2 * (s.IsMarried ? 0.85 : 1.00)));
                    int ammounit = fleet.MembersInstance.Sum(s => s == null ? 0 : (int)Math.Floor(s.MasterShip.Ammo * 0.2 * (s.IsMarried ? 0.85 : 1.00)));

                    int speed = fleet.MembersWithoutEscaped.Min(s => s == null ? 10 : s.MasterShip.Speed);
                    ToolTipInfo.SetToolTip(Name, string.Format(
                        "Lv合計: {0} / 平均: {1:0.00}\r\n{2}艦隊\r\nドラム缶搭載: {3}個 ({4}艦)\r\n大発動艇搭載: {5}個\r\n総積載: 燃 {6} / 弾 {7}\r\n(1戦当たり 燃 {8} / 弾 {9})",
                        levelSum,
                        (double)levelSum / Math.Max(fleet.Members.Count(id => id != -1), 1),
                        Constants.GetSpeed(speed),
                        fleet.MembersInstance.Sum(s => s == null ? 0 : s.SlotInstanceMaster.Count(q => q == null ? false : q.CategoryType == 30)),
                        fleet.MembersInstance.Count(s => s == null ? false : s.SlotInstanceMaster.Count(q => q == null ? false : q.CategoryType == 30) > 0),
                        fleet.MembersInstance.Sum(s => s == null ? 0 : s.SlotInstanceMaster.Count(q => q == null ? false : q.CategoryType == 24)),
                        fueltotal,
                        ammototal,
                        fuelunit,
                        ammounit
                        ));

                }


                State = FleetData.UpdateFleetState(fleet, StateMain, ToolTipInfo, State, ref Timer);


                //制空戦力計算	
                {
                    int airSuperiority = fleet.GetAirSuperiority();
                    int airSuperiority_old = fleet.GetAirSuperiority_Old(1);//对7星舰战使用120内部熟练度进行制空计算
                    int airSuperiority_old2 = fleet.GetAirSuperiority_Old(15);//对7星所有有内部熟练制空使用120内部熟练度进行制空计算
                    AirSuperiority.Text = airSuperiority.ToString();
                    ToolTipInfo.SetToolTip(AirSuperiority,
                        string.Format("满熟练制空值: {0}/{5}\r\n確保: {1}\r\n優勢: {2}\r\n均衡: {3}\r\n劣勢: {4}\r\n",
                        airSuperiority_old,
                        (int)(airSuperiority / 3.0),
                        (int)(airSuperiority / 1.5),
                        (int)(airSuperiority * 1.5 - 1),
                        (int)(airSuperiority * 3.0 - 1),
                        airSuperiority_old2));
                }


				//索敵能力計算
				SearchingAbility.Text = fleet.GetSearchingAbilityString();
				{
					StringBuilder sb = new StringBuilder();
					double probStart = fleet.GetContactProbability();
					var probSelect = fleet.GetContactSelectionProbability();

					var ss = Utility.Configuration.Config.FormFleet.SearchingAbilities.Split( ';' );
					for ( int i = 0; i < ss.Length; i++ ) {
						sb.AppendFormat( "{0}: {1}\r\n", ss[i], fleet.GetSearchingAbilityString( i ) );
					}
					sb.AppendFormat( "\r\n触接開始率: \r\n　確保 {0:p1} / 優勢 {1:p1}\r\n",
						probStart,
						probStart * 0.6 );

					if ( probSelect.Count > 0 ) {
						sb.AppendLine( "触接選択率: " );

						foreach ( var p in probSelect.OrderBy( p => p.Key ) ) {
							sb.AppendFormat( "　命中{0} : {1:p1}\r\n", p.Key, p.Value );
						}
					}

					ToolTipInfo.SetToolTip( SearchingAbility, sb.ToString() );

					// 舰队防空值计算
					AAValue.Text = CalculatorEx.GetFleetAAValue(fleet, 0).ToString();
					ToolTipInfo.SetToolTip(AAValue,
					string.Format("单纵阵: {0}\r\n复纵阵: {1}\r\n轮形阵: {2}\r\n梯形阵: {3}\r\n单横阵: {4}\r\n",
						CalculatorEx.GetFleetAAValue(fleet, 1),
						CalculatorEx.GetFleetAAValue(fleet, 2),
						CalculatorEx.GetFleetAAValue(fleet, 3),
						CalculatorEx.GetFleetAAValue(fleet, 4),
						CalculatorEx.GetFleetAAValue(fleet, 5)));
				}

            }


			public void ResetState() {
				State = FleetData.FleetStates.NoShip;
			}

			public void Refresh() {

				FleetData.RefreshFleetState( StateMain, State, Timer );

			}

			public void ConfigurationChanged( FormFleet parent ) {
				Name.Font = parent.MainFont;
				StateMain.Font = parent.MainFont;
				StateMain.BackColor = Color.Transparent;
				AirSuperiority.Font = parent.MainFont;
				AirSuperiority.Font = parent.MainFont;
				SearchingAbility.Font = parent.MainFont;

			}

		}


		private class TableMemberControl {
			public ImageLabel Name;
			public ShipStatusLevel Level;
			public ShipStatusHP HP;
			public ImageLabel Condition;
			public ShipStatusResource ShipResource;
			public ShipStatusEquipment Equipments;

			private ToolTip ToolTipInfo;
			private FormFleet Parent;


			public TableMemberControl( FormFleet parent ) {

				#region Initialize

				Name = new ImageLabel();
				Name.SuspendLayout();
				Name.Text = "*nothing*";
				Name.Anchor = AnchorStyles.Left;
				Name.TextAlign = ContentAlignment.MiddleLeft;
				Name.ImageAlign = ContentAlignment.MiddleCenter;
				Name.ForeColor = parent.MainFontColor;
				Name.Padding = new Padding( 0, 1, 0, 1 );
				Name.Margin = new Padding( 2, 0, 2, 0 );
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
				Level.Padding = new Padding( 0, 0, 0, 0 );
				Level.Margin = new Padding( 2, 0, 2, 1 );
				Level.AutoSize = true;
				Level.Visible = false;
				Level.ResumeLayout();

				HP = new ShipStatusHP();
				HP.SuspendLayout();
				HP.Anchor = AnchorStyles.Left;
				HP.Value = 0;
				HP.MaximumValue = 0;
				HP.MaximumDigit = 999;
				HP.UsePrevValue = false;
				HP.MainFontColor = parent.MainFontColor;
				HP.SubFontColor = parent.SubFontColor;
				HP.RepairFontColor = parent.SubFontColor;
				HP.Padding = new Padding( 0, 0, 0, 0 );
				HP.Margin = new Padding( 2, 1, 2, 2 );
				HP.AutoSize = true;
				HP.Visible = false;
				HP.ResumeLayout();

				Condition = new ImageLabel();
				Condition.SuspendLayout();
				Condition.Text = "*";
				Condition.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				Condition.ForeColor = parent.MainFontColor;
				Condition.TextAlign = ContentAlignment.BottomRight;
				Condition.ImageAlign = ContentAlignment.MiddleLeft;
				Condition.ImageList = ResourceManager.Instance.Icons;
				Condition.Padding = new Padding( 2, 2, 2, 2 );
				Condition.Margin = new Padding( 2, 0, 2, 0 );
				Condition.Size = new Size( 40, 20 );
				Condition.AutoSize = true;
				Condition.Visible = false;
				Condition.ResumeLayout();

				ShipResource = new ShipStatusResource( parent.ToolTipInfo );
				ShipResource.SuspendLayout();
				ShipResource.FuelCurrent = 0;
				ShipResource.FuelMax = 0;
				ShipResource.AmmoCurrent = 0;
				ShipResource.AmmoMax = 0;
				ShipResource.Anchor = AnchorStyles.Left;
				ShipResource.Padding = new Padding( 0, 2, 0, 1 );
				ShipResource.Margin = new Padding( 2, 0, 2, 0 );
				ShipResource.Size = new Size( 30, 20 );
				ShipResource.AutoSize = false;
				ShipResource.Visible = false;
				ShipResource.ResumeLayout();

				Equipments = new ShipStatusEquipment();
				Equipments.SuspendLayout();
				Equipments.Anchor = AnchorStyles.Left;
				Equipments.Padding = new Padding( 0, 2, 0, 1 );
				Equipments.Margin = new Padding( 2, 0, 2, 0 );
				Equipments.Size = new Size( 40, 20 );
				Equipments.AutoSize = true;
				Equipments.Visible = false;
				Equipments.ResumeLayout();

				ConfigurationChanged( parent );

				ToolTipInfo = parent.ToolTipInfo;
				Parent = parent;
				#endregion

			}


			public TableMemberControl( FormFleet parent, TableLayoutPanel table, int row )
				: this( parent ) {
				AddToTable( table, row );
			}


			public void AddToTable( TableLayoutPanel table, int row ) {

				table.SuspendLayout();
				table.Controls.Add( Name, 0, row );
				table.Controls.Add( Level, 1, row );
				table.Controls.Add( HP, 2, row );
				table.Controls.Add( Condition, 3, row );
				table.Controls.Add( ShipResource, 4, row );
				table.Controls.Add( Equipments, 5, row );
				table.ResumeLayout();

				#region set RowStyle
				RowStyle rs = new RowStyle( SizeType.Absolute, 21 );

				if ( table.RowStyles.Count > row )
					table.RowStyles[row] = rs;
				else
					while ( table.RowStyles.Count <= row )
						table.RowStyles.Add( rs );
				#endregion
			}

			private double CalculateFire( ShipData ship ) {
				return CalculatorEx.CalculateFire( ship );
			}

			private double CalculateWeightingAA( ShipData ship )
			{
				return CalculatorEx.CalculateWeightingAA( ship );
			}

			public void Update( int shipMasterID ) {

				KCDatabase db = KCDatabase.Instance;
				ShipData ship = db.Ships[shipMasterID];

				if ( ship != null ) {

					bool isEscaped = KCDatabase.Instance.Fleet[Parent.FleetID].EscapedShipList.Contains( shipMasterID );


					Name.Text = ship.MasterShip.NameWithClass;
					Name.Tag = ship.ShipID;
					ToolTipInfo.SetToolTip( Name,
						string.Format(
							"{0} {1}\n火力: {2}/{3}\n雷装: {4}/{5}\n対空: {6}/{7}\n加权对空: {19:0.##}\n装甲: {8}/{9}\n対潜: {10}/{11}\n回避: {12}/{13}\n索敵: {14}/{15}\n運: {16}\n射程: {17} / 速力: {18}\n(右クリックで図鑑)\n",
							ship.MasterShip.ShipTypeName, ship.NameWithLevel,
							ship.FirepowerBase,
							(ship.MasterShip.ShipType == 7 ||	// 轻空母
							ship.MasterShip.ShipType == 11 ||	// 正规空母
							ship.MasterShip.ShipType == 18) ?	// 装甲空母
							string.Format( "{0}（空母火力：{1:F0}）", ship.FirepowerTotal, CalculateFire( ship ) ) :
							ship.FirepowerTotal.ToString(),
							ship.TorpedoBase, ship.TorpedoTotal,
							ship.AABase, ship.AATotal,
							ship.ArmorBase, ship.ArmorTotal,
							ship.ASWBase, ship.ASWTotal,
							ship.EvasionBase, ship.EvasionTotal,
							ship.LOSBase, ship.LOSTotal,
							ship.LuckTotal,
							Constants.GetRange( ship.Range ),
							Constants.GetSpeed( ship.MasterShip.Speed ),
							CalculateWeightingAA( ship )
							) );


					Level.Value = ship.Level;
					Level.ValueNext = ship.ExpNext;

					{
						StringBuilder tip = new StringBuilder();
						tip.AppendFormat( "Total: {0} exp.\r\n", ship.ExpTotal );

						if ( !Utility.Configuration.Config.FormFleet.ShowNextExp )
							tip.AppendFormat( "次のレベルまで: {0} exp.\r\n", ship.ExpNext );

						if ( ship.MasterShip.RemodelAfterShipID != 0 && ship.Level < ship.MasterShip.RemodelAfterLevel ) {
							tip.AppendFormat( "改装まで: Lv. {0} / {1} exp.\r\n", ship.MasterShip.RemodelAfterLevel - ship.Level, ship.ExpNextRemodel );

						} else if ( ship.Level <= 99 ) {

							// 判断隔代改装的经验
							var ship_m = ship.MasterShip.RemodelAfterShip;
							int nextRemodelLevel = 0;
							while ( ship_m != null && ship_m.RemodelAfterShipID != 0 )
							{
								int level = ship_m.RemodelAfterLevel;
								if ( ship.Level < level ) {
									tip.AppendFormat( "改装まで: Lv. {0} / {1} exp.\n", level - ship.Level, Math.Max( ExpTable.ShipExp[level].Total - ship.ExpTotal, 0 ) );
									break;
								}

								if ( level <= nextRemodelLevel ) {
									// 发现可能的循环改造，跳出
									break;
								}
								nextRemodelLevel = level;
								ship_m = ship_m.RemodelAfterShip;
							}

							tip.AppendFormat( "Lv99まで: {0} exp.", Math.Max( ExpTable.GetExpToLevelShip( ship.ExpTotal, 99 ), 0 ) );

						} else {
							tip.AppendFormat( "Lv{0}まで: {1} exp.\r\n", ExpTable.ShipMaximumLevel, Math.Max( ExpTable.GetExpToLevelShip( ship.ExpTotal, ExpTable.ShipMaximumLevel ), 0 ) );

						}

						ToolTipInfo.SetToolTip( Level, tip.ToString() );
					}


					HP.Value = ship.HPCurrent;
					HP.MaximumValue = ship.HPMax;
					{
						int dockID = ship.RepairingDockID;

						HP.RepairTime = null;
						if ( dockID != -1 ) {
							HP.RepairTime = db.Docks[dockID].CompletionTime;
						}
					}
					if ( isEscaped ) {
						HP.BackColor = Color.Silver;
					} else {
						HP.BackColor = Utility.Configuration.Config.UI.BackColor;
					}
					{
						StringBuilder sb = new StringBuilder();
						double hprate = (double)ship.HPCurrent / ship.HPMax;

						sb.AppendFormat( "HP: {0:0.0}% [{1}]\n", hprate * 100, Constants.GetDamageState( hprate ) );
						if ( isEscaped ) {
							sb.AppendLine( "退避中" );
						} else if ( hprate > 0.50 ) {
							sb.AppendFormat( "中破まで: {0} / 大破まで: {1}\n", ship.HPCurrent - ship.HPMax / 2, ship.HPCurrent - ship.HPMax / 4 );
						} else if ( hprate > 0.25 ) {
							sb.AppendFormat( "大破まで: {0}\n", ship.HPCurrent - ship.HPMax / 4 );
						} else {
							sb.AppendLine( "大破しています！" );
						}

						if ( ship.RepairTime > 0 ) {
							var span = DateTimeHelper.FromAPITimeSpan( ship.RepairTime );
							sb.AppendFormat( "入渠時間: {0} @ {1}",
								DateTimeHelper.ToTimeRemainString( span ),
								DateTimeHelper.ToTimeRemainString( new TimeSpan( span.Add( new TimeSpan( 0, 0, -30 ) ).Ticks / ( ship.HPMax - ship.HPCurrent ) ) ) );
						}

						ToolTipInfo.SetToolTip( HP, sb.ToString() );
					}



					Condition.Text = ship.Condition.ToString();
					if ( ship.Condition < 20 ) {
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionVeryTired;
					} else if ( ship.Condition < 30 ) {
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionTired;
					} else if ( ship.Condition < 40 ) {
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionLittleTired;
					} else if ( ship.Condition < 50 ) {
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionNormal;
					} else {
						Condition.ImageIndex = (int)ResourceManager.IconContent.ConditionSparkle;
					}
					if ( ship.Condition < 49 ) {
						TimeSpan ts = new TimeSpan( 0, (int)Math.Ceiling( ( 49 - ship.Condition ) / 3.0 ) * 3, 0 );
						ToolTipInfo.SetToolTip( Condition, string.Format( "完全回復まで 約 {0:D2}:{1:D2}", (int)ts.TotalMinutes, (int)ts.Seconds ) );
					} else {
						ToolTipInfo.SetToolTip( Condition, string.Format( "あと {0} 回遠征可能", (int)Math.Ceiling( ( ship.Condition - 49 ) / 3.0 ) ) );
					}

					ShipResource.SetResources( ship.Fuel, ship.FuelMax, ship.Ammo, ship.AmmoMax );


					Equipments.SetSlotList( ship );
					ToolTipInfo.SetToolTip( Equipments, GetEquipmentString( ship ) );

				} else {
					Name.Tag = -1;
				}


				Name.Visible =
				Level.Visible =
				HP.Visible =
				Condition.Visible =
				ShipResource.Visible =
				Equipments.Visible = shipMasterID != -1;

			}

			void Name_MouseDown( object sender, MouseEventArgs e ) {
				int? id = Name.Tag as int?;

				if ( id != null && id != -1 && ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 ) {
					new DialogAlbumMasterShip( (int)id ).Show( Parent );
				}

			}


			private string GetEquipmentString( ShipData ship ) {
				StringBuilder sb = new StringBuilder();

				for ( int i = 0; i < ship.Slot.Count; i++ ) {
					var eq = ship.SlotInstance[i];
					if ( eq != null )
						sb.AppendFormat( "[{0}/{1}] {2}\r\n", ship.Aircraft[i], ship.MasterShip.Aircraft[i], eq.NameWithLevel );
				}

				{
					var exslot = ship.ExpansionSlotInstance;
					if ( exslot != null )
						sb.AppendFormat( "補強: {0}\r\n", exslot.NameWithLevel );
				}


				int[] slotmaster = ship.SlotMaster.ToArray();

				sb.AppendFormat( "\r\n昼戦: {0}", Constants.GetDayAttackKind( Calculator.GetDayAttackKind( slotmaster, ship.ShipID, -1 ) ) );
				{
					int shelling = ship.ShellingPower;
					int aircraft = ship.AircraftPower;
					if ( shelling > 0 ) {
						if ( aircraft > 0 )
							sb.AppendFormat( " - 砲撃: {0} / 空撃: {1}", shelling, aircraft );
						else
							sb.AppendFormat( " - 威力: {0}", shelling );
					} else if ( aircraft > 0 )
						sb.AppendFormat( " - 威力: {0}", aircraft );
				}
				sb.AppendLine();

				sb.AppendFormat( "夜戦: {0}", Constants.GetNightAttackKind( Calculator.GetNightAttackKind( slotmaster, ship.ShipID, -1 ) ) );
				{
					int night = ship.NightBattlePower;
					if ( night > 0 ) {
						sb.AppendFormat( " - 威力: {0}", night );
					}
				}
				sb.AppendLine();

				{
					int torpedo = ship.TorpedoPower;
					int asw = ship.AntiSubmarinePower;
					if ( torpedo > 0 ) {
						if ( asw > 0 )
							sb.AppendFormat( "雷撃: {0} / 対潜: {1}\r\n", torpedo, asw );
						else
							sb.AppendFormat( "雷撃: {0}\r\n", torpedo );
					} else if ( asw > 0 )
						sb.AppendFormat( "対潜: {0}\r\n", asw );
				}

				{
					int aacutin = Calculator.GetAACutinKind( ship.ShipID, slotmaster );
					if ( aacutin != 0 ) {
						sb.AppendFormat( "対空: {0}\r\n", Constants.GetAACutinKind( aacutin ) );
					}
				}
				{
					int airsup = Calculator.GetAirSuperiority( ship );
					int airbattle = ship.AirBattlePower;
					if ( airsup > 0 ) {
						if ( airbattle > 0 )
							sb.AppendFormat( "制空戦力: {0} / 航空威力: {1}\r\n", airsup, airbattle );
						else
							sb.AppendFormat( "制空戦力: {0}\r\n", airsup );
					} else if ( airbattle > 0 )
						sb.AppendFormat( "航空威力: {0}\r\n", airbattle );
				}

				return sb.ToString();
			}


			public void ConfigurationChanged( FormFleet parent ) {
				Name.Font = parent.MainFont;
				Level.MainFont = parent.MainFont;
				Level.SubFont = parent.SubFont;
				HP.MainFont = parent.MainFont;
				HP.SubFont = parent.SubFont;
				Condition.Font = parent.MainFont;
				Equipments.Font = parent.SubFont;
			}
		}




		public int FleetID { get; private set; }


		public Font MainFont { get; set; }
		public Font SubFont { get; set; }
		public Color MainFontColor { get; set; }
		public Color SubFontColor { get; set; }


		private TableFleetControl ControlFleet;
		private TableMemberControl[] ControlMember;


		private Pen LinePen = Pens.Silver;


		public FormFleet( FormMain parent, int fleetID ) {
			this.SuspendLayoutForDpiScale();
			InitializeComponent();

			FleetID = fleetID;
			Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;

			ConfigurationChanged();

			MainFontColor = Utility.Configuration.Config.UI.ForeColor;
			SubFontColor = Utility.Configuration.Config.UI.SubForeColor;


			//ui init

			ControlHelper.SetDoubleBuffered( TableFleet );
			ControlHelper.SetDoubleBuffered( TableMember );


			TableFleet.Visible = false;
			TableFleet.SuspendLayout();
			TableFleet.BorderStyle = BorderStyle.FixedSingle;
			ControlFleet = new TableFleetControl( this, TableFleet );
			TableFleet.ResumeLayout();


			TableMember.SuspendLayout();
			ControlMember = new TableMemberControl[6];
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				ControlMember[i] = new TableMemberControl( this, TableMember, i );
			}
			TableMember.ResumeLayout();


			ConfigurationChanged();		//fixme: 苦渋の決断

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet] );

			this.ResumeLayoutForDpiScale();
		}



		private void FormFleet_Load( object sender, EventArgs e ) {

			Text = string.Format( "#{0}", FleetID );

			APIObserver o = APIObserver.Instance;

			o.APIList["api_req_hensei/change"].RequestReceived += ChangeOrganization;
			o.APIList["api_req_kousyou/destroyship"].RequestReceived += ChangeOrganization;
			o.APIList["api_req_kaisou/remodeling"].RequestReceived += ChangeOrganization;
			o.APIList["api_req_kaisou/powerup"].ResponseReceived += ChangeOrganization;
			o.APIList["api_req_hensei/preset_select"].ResponseReceived += ChangeOrganization;

			o.APIList["api_req_nyukyo/start"].RequestReceived += Updated;
			o.APIList["api_req_nyukyo/speedchange"].RequestReceived += Updated;
			o.APIList["api_req_hensei/change"].RequestReceived += Updated;
			o.APIList["api_req_kousyou/destroyship"].RequestReceived += Updated;
			o.APIList["api_req_member/updatedeckname"].RequestReceived += Updated;
			o.APIList["api_req_kaisou/remodeling"].RequestReceived += Updated;
			o.APIList["api_req_map/start"].RequestReceived += Updated;
			o.APIList["api_req_hensei/combined"].RequestReceived += Updated;

			o.APIList["api_port/port"].ResponseReceived += Updated;
			o.APIList["api_get_member/ship2"].ResponseReceived += Updated;
			o.APIList["api_get_member/ndock"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/getship"].ResponseReceived += Updated;
			o.APIList["api_req_hokyu/charge"].ResponseReceived += Updated;
			o.APIList["api_req_kousyou/destroyship"].ResponseReceived += Updated;
			o.APIList["api_get_member/ship3"].ResponseReceived += Updated;
			o.APIList["api_req_kaisou/powerup"].ResponseReceived += Updated;		//requestのほうは面倒なのでこちらでまとめてやる
			o.APIList["api_get_member/deck"].ResponseReceived += Updated;
			o.APIList["api_get_member/slot_item"].ResponseReceived += Updated;
			o.APIList["api_req_map/start"].ResponseReceived += Updated;
			o.APIList["api_req_map/next"].ResponseReceived += Updated;
			o.APIList["api_get_member/ship_deck"].ResponseReceived += Updated;
			o.APIList["api_req_hensei/preset_select"].ResponseReceived += Updated;
			o.APIList["api_req_kaisou/slot_exchange_index"].ResponseReceived += Updated;


			//追加するときは FormFleetOverview にも同様に追加してください

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}


		void Updated( string apiname, dynamic data ) {

			if ( IsRemodeling ) {
				if ( apiname == "api_get_member/slot_item" )
					IsRemodeling = false;
				else
					return;
			}
			if ( apiname == "api_req_kaisou/remodeling" ) {
				IsRemodeling = true;
				return;
			}

			KCDatabase db = KCDatabase.Instance;

			if ( db.Ships.Count == 0 ) return;

			FleetData fleet = db.Fleet.Fleets[FleetID];
			if ( fleet == null ) return;

			TableFleet.SuspendLayout();
			ControlFleet.Update( fleet );
			TableFleet.Visible = true;
			TableFleet.ResumeLayout();

			TableMember.SuspendLayout();
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				ControlMember[i].Update( fleet.Members[i] );
			}
			TableMember.ResumeLayout();


			if ( Icon != null ) ResourceManager.DestroyIcon( Icon );
			Icon = ResourceManager.ImageToIcon( ControlFleet.StateMain.Image );
			if ( Parent != null ) Parent.Refresh();		//アイコンを更新するため

		}

		void ChangeOrganization( string apiname, dynamic data ) {

			ControlFleet.ResetState();

		}


		void UpdateTimerTick() {

			TableFleet.SuspendLayout();
			{
				FleetData fleet = KCDatabase.Instance.Fleet.Fleets[FleetID];
				if ( fleet != null )
					ControlFleet.Refresh();

			}
			TableFleet.ResumeLayout();

			TableMember.SuspendLayout();
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				ControlMember[i].HP.Refresh();
			}
			TableMember.ResumeLayout();

		}


		//艦隊編成のコピー
		private void ContextMenuFleet_CopyFleet_Click( object sender, EventArgs e ) {

			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;
			FleetData fleet = db.Fleet[FleetID];
			if ( fleet == null ) return;

			sb.AppendFormat( "{0}\t制空戦力{1}/索敵能力{2}\r\n", fleet.Name, fleet.GetAirSuperiority(), fleet.GetSearchingAbilityString() );
			for ( int i = 0; i < fleet.Members.Count; i++ ) {
				if ( fleet[i] == -1 )
					continue;

				ShipData ship = db.Ships[fleet[i]];

				sb.AppendFormat( "{0}/{1}\t", ship.MasterShip.Name, ship.Level );

				var eq = ship.AllSlotInstance;


				if ( eq != null ) {
					for ( int j = 0; j < eq.Count; j++ ) {

						if ( eq[j] == null ) continue;

						int count = 1;
						for ( int k = j + 1; k < eq.Count; k++ ) {
							if ( eq[k] != null && eq[k].EquipmentID == eq[j].EquipmentID && eq[k].Level == eq[j].Level && eq[k].AircraftLevel == eq[j].AircraftLevel ) {
								count++;
							} else {
								break;
							}
						}

						if ( count == 1 ) {
							sb.AppendFormat( "{0}{1}", j == 0 ? "" : "/", eq[j].NameWithLevel );
						} else {
							sb.AppendFormat( "{0}{1}x{2}", j == 0 ? "" : "/", eq[j].NameWithLevel, count );
						}

						j += count - 1;
					}
				}

				sb.AppendLine();
			}


			Clipboard.SetData( DataFormats.StringFormat, sb.ToString() );
		}


		private void ContextMenuFleet_Opening( object sender, CancelEventArgs e ) {

			ContextMenuFleet_Capture.Visible = Utility.Configuration.Config.Debug.EnableDebugMenu;

		}



		/// <summary>
		/// 「艦隊デッキビルダー」用編成コピー
		/// <see cref="http://www.kancolle-calc.net/deckbuilder.html"/>
		/// </summary>
		private void ContextMenuFleet_CopyFleetDeckBuilder_Click( object sender, EventArgs e ) {

			StringBuilder sb = new StringBuilder();
			KCDatabase db = KCDatabase.Instance;

			// 手書き json の悲しみ

			sb.Append( @"{""version"":3," );

			foreach ( var fleet in db.Fleet.Fleets.Values ) {
				if ( fleet == null ) continue;

				sb.AppendFormat( @"""f{0}"":{{", fleet.FleetID );

				int shipcount = 1;
				foreach ( var ship in fleet.MembersInstance ) {
					if ( ship == null ) break;

					sb.AppendFormat( @"""s{0}"":{{""id"":{1},""lv"":{2},""luck"":{3},""items"":{{",
						shipcount,
						ship.ShipID,
						ship.Level,
						ship.LuckBase );

					if ( ship.ExpansionSlot <= 0 )
						sb.Append( @"""ix"":{}," );
					else
						sb.AppendFormat( @"""ix"":{{""id"":{0}}},", ship.ExpansionSlotMaster );

					int eqcount = 1;
					foreach ( var eq in ship.SlotInstance ) {
						if ( eq == null ) break;

						// 水偵は改修レベル優先(熟練度にすると改修レベルに誤解されて 33式 の結果がずれるため)
						sb.AppendFormat( @"""i{0}"":{{""id"":{1},""rf"":{2}}},", eqcount, eq.EquipmentID, eq.MasterEquipment.CategoryType == 10 ? eq.Level : Math.Max( eq.Level, eq.AircraftLevel ) );

						eqcount++;
					}

					if ( eqcount > 0 )
						sb.Remove( sb.Length - 1, 1 );		// remove ","
					sb.Append( @"}}," );

					shipcount++;
				}

				if ( shipcount > 0 )
					sb.Remove( sb.Length - 1, 1 );		// remove ","
				sb.Append( @"}," );

			}

			sb.Remove( sb.Length - 1, 1 );		// remove ","
			sb.Append( @"}" );

			Clipboard.SetData( DataFormats.StringFormat, sb.ToString() );
		}


		private void ContextMenuFleet_Capture_Click( object sender, EventArgs e ) {

			using ( Bitmap bitmap = new Bitmap( this.ClientSize.Width, this.ClientSize.Height ) ) {
				this.DrawToBitmap( bitmap, this.ClientRectangle );

				Clipboard.SetData( DataFormats.Bitmap, bitmap );
			}
		}




		void ConfigurationChanged() {

			var c = Utility.Configuration.Config;

			MainFont = Font = c.UI.MainFont;
			SubFont = c.UI.SubFont;

			LinePen = new Pen( c.UI.LineColor.ColorData );

			AutoScroll = ContextMenuFleet_IsScrollable.Checked = c.FormFleet.IsScrollable;
			ContextMenuFleet_FixShipNameWidth.Checked = c.FormFleet.FixShipNameWidth;

			if ( ControlFleet != null && KCDatabase.Instance.Fleet[FleetID] != null ) {
				ControlFleet.ConfigurationChanged( this );
				ControlFleet.Update( KCDatabase.Instance.Fleet[FleetID] );
			}

			if ( ControlMember != null ) {
				bool showAircraft = c.FormFleet.ShowAircraft;
				bool fixShipNameWidth = c.FormFleet.FixShipNameWidth;
				bool shortHPBar = c.FormFleet.ShortenHPBar;
				bool colorMorphing = c.UI.BarColorMorphing;
				//Color[] colorScheme = c.UI.BarColorScheme.Select( col => col.ColorData ).ToArray();
				bool showNext = c.FormFleet.ShowNextExp;
				bool textProficiency = c.FormFleet.ShowTextProficiency;
				bool showEquipmentLevel = c.FormFleet.ShowEquipmentLevel;

				for ( int i = 0; i < ControlMember.Length; i++ ) {
					ControlMember[i].Equipments.ShowAircraft = showAircraft;
					if ( fixShipNameWidth ) {
						ControlMember[i].Name.AutoSize = false;
						ControlMember[i].Name.Size = new Size( 40, 20 );
					} else {
						ControlMember[i].Name.AutoSize = true;
					}

					ControlMember[i].HP.Text = shortHPBar ? "" : "HP:";
					ControlMember[i].HP.HPBar.ColorMorphing = colorMorphing;
					ControlMember[i].HP.HPBar.ReloadBarSettings();
					ControlMember[i].Level.TextNext = showNext ? "next:" : null;
					ControlMember[i].Equipments.TextProficiency = textProficiency;
					ControlMember[i].Equipments.ShowEquipmentLevel = showEquipmentLevel;
					ControlMember[i].ShipResource.BarFuel.ColorMorphing =
					ControlMember[i].ShipResource.BarAmmo.ColorMorphing = colorMorphing;
					ControlMember[i].ShipResource.BarFuel.ReloadBarSettings();
					ControlMember[i].ShipResource.BarAmmo.ReloadBarSettings();

					ControlMember[i].ConfigurationChanged( this );
				}
			}
			TableMember.PerformLayout();		//fixme:サイズ変更に親パネルが追随しない

		}




		//よく考えたら別の艦隊タブと同期しないといけないので封印
		private void ContextMenuFleet_IsScrollable_Click( object sender, EventArgs e ) {
			Utility.Configuration.Config.FormFleet.IsScrollable = ContextMenuFleet_IsScrollable.Checked;
			ConfigurationChanged();
		}

		private void ContextMenuFleet_FixShipNameWidth_Click( object sender, EventArgs e ) {
			Utility.Configuration.Config.FormFleet.FixShipNameWidth = ContextMenuFleet_FixShipNameWidth.Checked;
			ConfigurationChanged();
		}


		private void TableMember_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( LinePen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}


		public override string GetPersistString()
		{
			return "Fleet #" + FleetID.ToString();
		}




	}

}
