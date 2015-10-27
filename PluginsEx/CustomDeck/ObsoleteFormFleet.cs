using Codeplex.Data;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Support;
using ElectronicObserver;
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



namespace CustomDeck
{

    public partial class ObsoleteFormFleet : Form
    {

        private class TableFleetControl
        {
            public Label Name;
            public ImageLabel AirSuperiority;
            public ImageLabel SearchingAbility;
            public ImageLabel AAValue;
            public ToolTip ToolTipInfo;

            public TableFleetControl(ObsoleteFormFleet parent)
            {

                #region Initialize

                Name = new Label();
                Name.Text = "[" + parent.FleetID.ToString() + "]";
                Name.Anchor = AnchorStyles.Left;
                Name.Font = parent.MainFont;
                Name.ForeColor = parent.MainFontColor;
                Name.Padding = new Padding(0, 1, 0, 1);
                Name.Margin = new Padding(2, 0, 2, 0);
                Name.AutoSize = true;
                //Name.Visible = false;

                AirSuperiority = new ImageLabel();
                AirSuperiority.Anchor = AnchorStyles.Left;
                AirSuperiority.Font = parent.MainFont;
                AirSuperiority.ForeColor = parent.MainFontColor;
                AirSuperiority.ImageList = ResourceManager.Instance.Equipments;
                AirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;
                AirSuperiority.Padding = new Padding(2, 2, 2, 2);
                AirSuperiority.Margin = new Padding(2, 0, 2, 0);
                AirSuperiority.AutoSize = true;

                SearchingAbility = new ImageLabel();
                SearchingAbility.Anchor = AnchorStyles.Left;
                SearchingAbility.Font = parent.MainFont;
                SearchingAbility.ForeColor = parent.MainFontColor;
                SearchingAbility.ImageList = ResourceManager.Instance.Equipments;
                SearchingAbility.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedRecon;
                SearchingAbility.Padding = new Padding(2, 2, 2, 2);
                SearchingAbility.Margin = new Padding(2, 0, 2, 0);
                SearchingAbility.AutoSize = true;

                AAValue = new ImageLabel();
                AAValue.Anchor = AnchorStyles.Left;
                AAValue.Font = parent.MainFont;
                AAValue.ForeColor = parent.MainFontColor;
                AAValue.ImageList = ResourceManager.Instance.Equipments;
                AAValue.ImageIndex = (int)ResourceManager.EquipmentContent.AADirector;
                AAValue.Padding = new Padding(2, 2, 2, 2);
                AAValue.Margin = new Padding(2, 0, 2, 0);
                AAValue.AutoSize = true;

                ToolTipInfo = parent.ToolTipInfo;

                #endregion

            }

            public TableFleetControl(ObsoleteFormFleet parent, TableLayoutPanel table)
                : this(parent)
            {
                AddToTable(table);
            }

            public void AddToTable(TableLayoutPanel table)
            {

                table.SuspendLayout();
                table.Controls.Add(Name, 0, 0);
                table.Controls.Add(AirSuperiority, 1, 0);
                table.Controls.Add(SearchingAbility, 2, 0);
                table.Controls.Add(AAValue, 3, 0);
                table.ResumeLayout();

                int row = 0;
                #region set RowStyle
                RowStyle rs = new RowStyle(SizeType.Absolute, 21);

                if (table.RowStyles.Count > row)
                    table.RowStyles[row] = rs;
                else
                    while (table.RowStyles.Count <= row)
                        table.RowStyles.Add(rs);
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

                //制空戦力計算	
                {
                    int airSuperiority = fleet.GetAirSuperiority();
                    AirSuperiority.Text = airSuperiority.ToString();
                    ToolTipInfo.SetToolTip(AirSuperiority,
                        string.Format("確保: {0}\r\n優勢: {1}\r\n均衡: {2}\r\n劣勢: {3}\r\n",
                        (int)(airSuperiority / 3.0),
                        (int)(airSuperiority / 1.5),
                        (int)(airSuperiority * 1.5 - 1),
                        (int)(airSuperiority * 3.0 - 1)));
                }


                //索敵能力計算
                SearchingAbility.Text = fleet.GetSearchingAbilityString();
                ToolTipInfo.SetToolTip(SearchingAbility,
                    string.Format("(旧)2-5式: {0}\r\n2-5式(秋): {1}\r\n2-5新秋簡易式: {2}\r\n",
                    fleet.GetSearchingAbilityString(0),
                    fleet.GetSearchingAbilityString(1),
                    fleet.GetSearchingAbilityString(2)));

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


        private class TableMemberControl
        {
            public ImageLabel Name;
            public ImageLabel Level;

            public ShipStatusEquipment Equipments;

            private ToolTip ToolTipInfo;
            private ObsoleteFormFleet Parent;


            public TableMemberControl(ObsoleteFormFleet parent)
            {

                #region Initialize

                Name = new ImageLabel();
                Name.SuspendLayout();
                Name.Text = "*nothing*";
                Name.Anchor = AnchorStyles.Left;
                Name.TextAlign = ContentAlignment.MiddleLeft;
                Name.ImageAlign = ContentAlignment.MiddleCenter;
                Name.Font = parent.MainFont;
                Name.ForeColor = parent.MainFontColor;
                Name.Padding = new Padding(0, 1, 0, 1);
                Name.Margin = new Padding(2, 0, 2, 0);
                Name.AutoSize = true;
                //Name.AutoEllipsis = true;
                Name.Visible = false;
                Name.Cursor = Cursors.Help;
                Name.MouseDown += Name_MouseDown;
                Name.ResumeLayout();

                Level = new ImageLabel();
                Level.SuspendLayout();
                Level.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;

                Level.Font = parent.MainFont;

                //Level.TextNext = "n.";
                Level.Padding = new Padding(0, 0, 0, 0);
                Level.Margin = new Padding(2, 0, 2, 1);
                Level.AutoSize = true;
                Level.Visible = false;
                Level.ResumeLayout();


                Equipments = new ShipStatusEquipment();
                Equipments.SuspendLayout();
                Equipments.Anchor = AnchorStyles.Left;
                Equipments.Padding = new Padding(0, 2, 0, 1);
                Equipments.Margin = new Padding(2, 0, 2, 0);
                Equipments.Font = parent.SubFont;
                Equipments.Size = new Size(40, 20);
                Equipments.AutoSize = true;
                Equipments.Visible = false;
                Equipments.ResumeLayout();


                ToolTipInfo = parent.ToolTipInfo;
                Parent = parent;
                #endregion

            }


            public TableMemberControl(ObsoleteFormFleet parent, TableLayoutPanel table, int row)
                : this(parent)
            {
                AddToTable(table, row);
            }


            public void AddToTable(TableLayoutPanel table, int row)
            {

                table.SuspendLayout();
                table.Controls.Add(Name, 0, row);
                table.Controls.Add(Level, 1, row);
                table.Controls.Add(Equipments, 2, row);
                table.ResumeLayout();

                #region set RowStyle
                RowStyle rs = new RowStyle(SizeType.Absolute, 21);

                if (table.RowStyles.Count > row)
                    table.RowStyles[row] = rs;
                else
                    while (table.RowStyles.Count <= row)
                        table.RowStyles.Add(rs);
                #endregion
            }

            private double CalculateFire(ShipData ship)
            {
                return Math.Floor((ship.FirepowerTotal + ship.TorpedoTotal + Math.Floor(ship.BombTotal * 1.3)) * 1.5 + 55);
            }

            private double CalculateWeightingAA(ShipData ship)
            {
                double aatotal = ship.AABase;
                foreach (var eq in ship.AllSlotInstance)
                {
                    if (eq == null)
                        continue;

                    int ratio;
                    var eqmaster = eq.MasterEquipment;
                    switch (eqmaster.IconType)
                    {
                        case 15:	// 对空机枪
                            ratio = 6;
                            break;

                        case 16:	// 高角炮
                        case 30:	// 高射装置
                            ratio = 4;
                            break;

                        case 11:	// 电探
                            ratio = (eqmaster.AA > 0) ? 3 : 0;
                            break;

                        default:
                            ratio = 0;
                            break;
                    }
                    if (ratio <= 0)
                        continue;

                    aatotal += ratio * (eqmaster.AA + 0.7 * Math.Sqrt(eq.Level));
                }
                return aatotal;
            }

            public void Update(int shipMasterID)
            {

                KCDatabase db = KCDatabase.Instance;
                ShipData ship = db.Ships[shipMasterID];

                if (ship != null)
                {


                    Name.Text = ship.MasterShip.NameWithClass;
                    Name.Tag = ship.ShipID;
                    ToolTipInfo.SetToolTip(Name,//加上HP 消耗ship.HPMax.ToString();  ship.Fuel, ship.FuelMax, ship.Ammo, ship.AmmoMax
                        string.Format(
                            "{0} {1}\n火力: {2}/{3}\n雷装: {4}/{5}\n対空: {6}/{7}\n加权对空: {19:0.##}\n装甲: {8}/{9}\n対潜: {10}/{11}\n回避: {12}/{13}\n索敵: {14}/{15}\n運: {16}\n射程: {17} / 速力: {18}\n(点击右键查看图鉴)\n",
                            ship.MasterShip.ShipTypeName, ship.NameWithLevel,
                            ship.FirepowerBase,
                            (ship.MasterShip.ShipType == 7 ||	// 轻空母
                            ship.MasterShip.ShipType == 11 ||	// 正规空母
                            ship.MasterShip.ShipType == 18) ?	// 装甲空母
                            string.Format("{0}（空母火力：{1:F0}）", ship.FirepowerTotal, CalculateFire(ship)) :
                            ship.FirepowerTotal.ToString(),
                            ship.TorpedoBase, ship.TorpedoTotal,
                            ship.AABase, ship.AATotal,
                            ship.ArmorBase, ship.ArmorTotal,
                            ship.ASWBase, ship.ASWTotal,
                            ship.EvasionBase, ship.EvasionTotal,
                            ship.LOSBase, ship.LOSTotal,
                            ship.LuckTotal,
                            Constants.GetRange(ship.Range),
                            Constants.GetSpeed(ship.MasterShip.Speed),
                            CalculateWeightingAA(ship)
                            ));


                    Level.Text = "Lv:" + ship.Level.ToString();

                    Equipments.SetSlotList(ship);
                    ToolTipInfo.SetToolTip(Equipments, GetEquipmentString(ship));

                }
                else
                {
                    Name.Tag = -1;
                }


                Name.Visible =
                Level.Visible =
 
                Equipments.Visible = shipMasterID != -1;

            }

            void Name_MouseDown(object sender, MouseEventArgs e)
            {
                int? id = Name.Tag as int?;

                if (id != null && id != -1 && (e.Button & System.Windows.Forms.MouseButtons.Right) != 0)
                {
                    new DialogAlbumMasterShip((int)id).Show(Parent);
                }

            }


            private string GetEquipmentString(ShipData ship)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < ship.Slot.Count; i++)
                {
                    var eq = ship.SlotInstance[i];
                    if (eq != null)
                        sb.AppendFormat("[{0}] {1}\r\n",  ship.MasterShip.Aircraft[i], eq.NameWithLevel);
                }

                {
                    var exslot = ship.ExpansionSlotInstance;
                    if (exslot != null)
                        sb.AppendFormat("補強: {0}\r\n", exslot.NameWithLevel);
                }


                int[] slotmaster = ship.SlotMaster.ToArray();

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

                sb.AppendFormat("夜戦: {0}", Constants.GetNightAttackKind(Calculator.GetNightAttackKind(slotmaster, ship.ShipID, -1)));
                {
                    int night = ship.NightBattlePower;
                    if (night > 0)
                    {
                        sb.AppendFormat(" - 威力: {0}", night);
                    }
                }
                sb.AppendLine();

                {
                    int torpedo = ship.TorpedoPower;
                    //int asw = ship.MasterShip.ASW.;
                    if (torpedo > 0)
                    {
                        
                            sb.AppendFormat("雷撃: {0}\r\n", torpedo);
                    }
                  
                }

                {
                    int aacutin = Calculator.GetAACutinKind(ship.ShipID, slotmaster);
                    if (aacutin != 0)
                    {
                        sb.AppendFormat("対空: {0}\r\n", Constants.GetAACutinKind(aacutin));
                    }
                }
                {
                    int airsup = Calculator.GetAirSuperiority(ship);
                    int airbattle = ship.AirBattlePower;
                    if (airsup > 0)
                    {
                        if (airbattle > 0)
                            sb.AppendFormat("制空戦力: {0} / 航空威力: {1}\r\n", airsup, airbattle);
                        else
                            sb.AppendFormat("制空戦力: {0}\r\n", airsup);
                    }
                    else if (airbattle > 0)
                        sb.AppendFormat("航空威力: {0}\r\n", airbattle);
                }

                return sb.ToString();
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


        public ObsoleteFormFleet(FormMain parent, int fleetID)
        {
            this.SuspendLayoutForDpiScale();
            InitializeComponent();

            FleetID = fleetID;
            //ElectronicObserver.Utility
            ConfigurationChanged();

            MainFontColor = ElectronicObserver.Utility.Configuration.Config.UI.ForeColor;
            SubFontColor = ElectronicObserver.Utility.Configuration.Config.UI.SubForeColor;


            //ui init

            ControlHelper.SetDoubleBuffered(TableFleet);
            ControlHelper.SetDoubleBuffered(TableMember);


            TableFleet.Visible = false;
            TableFleet.SuspendLayout();
            TableFleet.BorderStyle = BorderStyle.FixedSingle;
            ControlFleet = new TableFleetControl(this, TableFleet);
            TableFleet.ResumeLayout();


            TableMember.SuspendLayout();
            ControlMember = new TableMemberControl[6];
            for (int i = 0; i < ControlMember.Length; i++)
            {
                ControlMember[i] = new TableMemberControl(this, TableMember, i);
            }
            TableMember.ResumeLayout();


            ConfigurationChanged();		//fixme: 苦渋の決断

            try
            {
                Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet]);
            }
            catch
            { }
            this.ResumeLayoutForDpiScale();
        }



        private void FormFleet_Load(object sender, EventArgs e)
        {

            Text = string.Format("#{0}", FleetID);

        }

        CustomFleet CustomFleet = null;

        void Updated(CustomFleet Fleet)
        {
            CustomFleet = Fleet;

            KCDatabase db = KCDatabase.Instance;

            FleetData fleet = db.Fleet.Fleets[FleetID];
            if (fleet == null) return;

            TableFleet.SuspendLayout();
            ControlFleet.Update(fleet);
            TableFleet.Visible = true;
            TableFleet.ResumeLayout();

            TableMember.SuspendLayout();
            for (int i = 0; i < ControlMember.Length; i++)
            {
                ControlMember[i].Update(fleet.Members[i]);
            }
            TableMember.ResumeLayout();

            if (Parent != null) Parent.Refresh();		//アイコンを更新するため

        }


        //艦隊編成のコピー
        private void ContextMenuFleet_CopyFleet_Click(object sender, EventArgs e)
        {

            StringBuilder sb = new StringBuilder();
            KCDatabase db = KCDatabase.Instance;
            FleetData fleet = db.Fleet[FleetID];
            if (fleet == null) return;

            sb.AppendFormat("{0}\t制空戦力{1}/索敵能力{2}\r\n", fleet.Name, fleet.GetAirSuperiority(), fleet.GetSearchingAbilityString());
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
                            sb.AppendFormat("{0}{1}", j == 0 ? "" : "/", eq[j].NameWithLevel);
                        }
                        else
                        {
                            sb.AppendFormat("{0}{1}x{2}", j == 0 ? "" : "/", eq[j].NameWithLevel, count);
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

            ContextMenuFleet_Capture.Visible = ElectronicObserver.Utility.Configuration.Config.Debug.EnableDebugMenu;

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

            sb.Append(@"{""version"":3,");

            foreach (var fleet in db.Fleet.Fleets.Values)
            {
                if (fleet == null) continue;

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

                    if (ship.ExpansionSlot <= 0)
                        sb.Append(@"""ix"":{},");
                    else
                        sb.AppendFormat(@"""ix"":{{""id"":{0}}},", ship.ExpansionSlotMaster);

                    int eqcount = 1;
                    foreach (var eq in ship.SlotInstance)
                    {
                        if (eq == null) break;
                        sb.AppendFormat(@"""i{0}"":{{""id"":{1},""rf"":{2}}},", eqcount, eq.EquipmentID, Math.Max(eq.Level, eq.AircraftLevel));

                        eqcount++;
                    }

                    sb.Remove(sb.Length - 1, 1);		// remove ","
                    sb.Append(@"}},");

                    shipcount++;
                }

                sb.Remove(sb.Length - 1, 1);		// remove ","
                sb.Append(@"},");

            }

            sb.Remove(sb.Length - 1, 1);		// remove ","
            sb.Append(@"}");

            Clipboard.SetData(DataFormats.StringFormat, sb.ToString());
        }


        private void ContextMenuFleet_Capture_Click(object sender, EventArgs e)
        {

            using (Bitmap bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height))
            {
                this.DrawToBitmap(bitmap, this.ClientRectangle);

                Clipboard.SetData(DataFormats.Bitmap, bitmap);
            }
        }




        void ConfigurationChanged()
        {

            var c = ElectronicObserver.Utility.Configuration.Config;

            MainFont = Font = c.UI.MainFont;
            SubFont = c.UI.SubFont;

            LinePen = new Pen(c.UI.LineColor.ColorData);

            AutoScroll = ContextMenuFleet_IsScrollable.Checked = c.FormFleet.IsScrollable;
            ContextMenuFleet_FixShipNameWidth.Checked = c.FormFleet.FixShipNameWidth;

            if (ControlFleet != null && KCDatabase.Instance.Fleet[FleetID] != null)
            {
                ControlFleet.Update(KCDatabase.Instance.Fleet[FleetID]);
            }

            if (ControlMember != null)
            {
                bool showAircraft = c.FormFleet.ShowAircraft;
                bool fixShipNameWidth = c.FormFleet.FixShipNameWidth;
                bool shortHPBar = c.FormFleet.ShortenHPBar;
                bool showNext = c.FormFleet.ShowNextExp;
                bool textProficiency = c.FormFleet.ShowTextProficiency;
                bool showEquipmentLevel = c.FormFleet.ShowEquipmentLevel;

                for (int i = 0; i < ControlMember.Length; i++)
                {
                    ControlMember[i].Equipments.ShowAircraft = showAircraft;
                    if (fixShipNameWidth)
                    {
                        ControlMember[i].Name.AutoSize = false;
                        ControlMember[i].Name.Size = new Size(40, 20);
                    }
                    else
                    {
                        ControlMember[i].Name.AutoSize = true;
                    }

                    //ControlMember[i].HP.Text = shortHPBar ? "" : "HP:";
                    //ControlMember[i].Level.TextNext = showNext ? "next:" : null;
                    ControlMember[i].Equipments.TextProficiency = textProficiency;
                    ControlMember[i].Equipments.ShowEquipmentLevel = showEquipmentLevel;
                }
            }
            TableMember.PerformLayout();		//fixme:サイズ変更に親パネルが追随しない

        }




        //よく考えたら別の艦隊タブと同期しないといけないので封印
        private void ContextMenuFleet_IsScrollable_Click(object sender, EventArgs e)
        {
            ElectronicObserver.Utility.Configuration.Config.FormFleet.IsScrollable = ContextMenuFleet_IsScrollable.Checked;
            ConfigurationChanged();
        }

        private void ContextMenuFleet_FixShipNameWidth_Click(object sender, EventArgs e)
        {
            ElectronicObserver.Utility.Configuration.Config.FormFleet.FixShipNameWidth = ContextMenuFleet_FixShipNameWidth.Checked;
            ConfigurationChanged();
        }


        private void TableMember_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            e.Graphics.DrawLine(LinePen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
        }





    }

}
