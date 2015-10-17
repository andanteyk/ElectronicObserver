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

using ElectronicObserver;
using ElectronicObserver.Window;

namespace MapAreaInfo
{

    public partial class FormFleetData : Form
    {

        private class TableEnemyMemberControl
        {

            public ImageLabel ShipName;
            public ShipStatusEquipment Equipments;

            public FormFleetData Parent;
            public ToolTip ToolTipInfo;


            public TableEnemyMemberControl(FormFleetData parent)
            {

                #region Initialize

                ShipName = new ImageLabel();
                ShipName.Anchor = AnchorStyles.Left;
                ShipName.Font = parent.MainFont;
                ShipName.ForeColor = parent.MainFontColor;
                ShipName.ImageAlign = ContentAlignment.MiddleCenter;
                ShipName.Padding = new Padding(0, 1, 0, 1);
                ShipName.Margin = new Padding(2, 0, 2, 0);
                //ShipName.MaximumSize = new Size( 60, 20 );
                ShipName.AutoEllipsis = true;
                ShipName.AutoSize = true;
                ShipName.Cursor = Cursors.Help;
                ShipName.MouseClick += ShipName_MouseClick;

                Equipments = new ShipStatusEquipment();
                Equipments.SuspendLayout();
                Equipments.Anchor = AnchorStyles.Left;
                Equipments.Font = parent.SubFont;
                Equipments.Padding = new Padding(0, 2, 0, 1);
                Equipments.Margin = new Padding(2, 0, 2, 0);
                Equipments.Size = new Size(40, 20);	//checkme: 要る？
                Equipments.AutoSize = true;
                Equipments.ResumeLayout();

                Parent = parent;
                ToolTipInfo = parent.ToolTipInfo;
                #endregion

            }


            public TableEnemyMemberControl(FormFleetData parent, TableLayoutPanel table, int row)
                : this(parent)
            {

                AddToTable(table, row);
            }

            public void AddToTable(TableLayoutPanel table, int row)
            {

                table.Controls.Add(ShipName, 0, row);
                table.Controls.Add(Equipments, 1, row);

                #region set RowStyle
                RowStyle rs = new RowStyle(SizeType.AutoSize);

                if (table.RowStyles.Count > row)
                    table.RowStyles[row] = rs;
                else
                    while (table.RowStyles.Count <= row)
                        table.RowStyles.Add(rs);
                #endregion
            }



            public void Update(int shipID)
            {
                var slot = shipID != -1 ? KCDatabase.Instance.MasterShips[shipID].DefaultSlot : null;
                Update(shipID, slot != null ? slot.ToArray() : null);
            }


            public void Update(int shipID, int[] slot)
            {

                ShipName.Tag = shipID;

                if (shipID == -1)
                {
                    //なし
                    ShipName.Text = "-";
                    ShipName.ForeColor = ElectronicObserver.Utility.Configuration.Config.UI.ForeColor;
                    Equipments.Visible = false;
                    ToolTipInfo.SetToolTip(ShipName, null);
                    ToolTipInfo.SetToolTip(Equipments, null);

                }
                else
                {

                    ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];


                    ShipName.Text = ship.Name;
                    switch (ship.AbyssalShipClass)
                    {
                        case 0:
                        case 1:		//normal
                        default:
                            ShipName.ForeColor = ElectronicObserver.Utility.Configuration.Config.UI.ForeColor; break;
                        case 2:		//elite
                            ShipName.ForeColor = ElectronicObserver.Utility.Configuration.Config.UI.EliteColor; break;
                        case 3:		//flagship
                            ShipName.ForeColor = ElectronicObserver.Utility.Configuration.Config.UI.FlagshipColor; break;
                        case 4:		//latemodel / flagship kai
                            ShipName.ForeColor = ElectronicObserver.Utility.Configuration.Config.UI.LateModelColor; break;
                        case 5:		//latemodel elite
                            ShipName.ForeColor = Color.FromArgb(0x88, 0x00, 0x00); break;
                        case 6:		//latemodel flagship
                            ShipName.ForeColor = Color.FromArgb(0x88, 0x44, 0x00); break;
                    }
                    ToolTipInfo.SetToolTip(ShipName, GetShipString(shipID, slot));

                    Equipments.SetSlotList(shipID, slot);
                    Equipments.Visible = true;
                    ToolTipInfo.SetToolTip(Equipments, GetEquipmentString(shipID, slot));
                }

            }


            public void UpdateEquipmentToolTip(int shipID)
            {
                ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
                var slot = shipID >0 ? KCDatabase.Instance.MasterShips[shipID].DefaultSlot : null;
                if (slot == null)
                {
                    ToolTipInfo.SetToolTip(ShipName, "无装备数据 shipID=" + shipID.ToString());
                }
                else
                    ToolTipInfo.SetToolTip(ShipName, GetShipString(shipID, slot.ToArray()));
            }


            private string GetShipString(int shipID, int[] slot)
            {

                ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
                if (ship == null) return null;

                return GetShipString(shipID, slot, -1, ship.HPMin, ship.FirepowerMax, ship.TorpedoMax, ship.AAMax, ship.ArmorMax,
                     ship.ASW != null && !ship.ASW.IsMaximumDefault ? ship.ASW.Maximum : -1,
                     ship.Evasion != null && !ship.Evasion.IsMaximumDefault ? ship.Evasion.Maximum : -1,
                     ship.LOS != null && !ship.LOS.IsMaximumDefault ? ship.LOS.Maximum : -1,
                     ship.LuckMin);
            }

            private string GetShipString(int shipID, int[] slot, int level, int hp, int firepower, int torpedo, int aa, int armor)
            {
                ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
                if (ship == null) return null;

                return GetShipString(shipID, slot, level, hp, firepower, torpedo, aa, armor,
                    ship.ASW != null && ship.ASW.IsAvailable ? ship.ASW.GetParameter(level) : -1,
                    ship.Evasion != null && ship.Evasion.IsAvailable ? ship.Evasion.GetParameter(level) : -1,
                    ship.LOS != null && ship.LOS.IsAvailable ? ship.LOS.GetParameter(level) : -1,
                    level > 99 ? Math.Min(ship.LuckMin + 3, ship.LuckMax) : ship.LuckMin);
            }

            private string GetShipString(int shipID, int[] slot, int level, int hp, int firepower, int torpedo, int aa, int armor, int asw, int evasion, int los, int luck)
            {

                ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
                if (ship == null) return null;

                int firepower_c = firepower;
                int torpedo_c = torpedo;
                int aa_c = aa;
                int armor_c = armor;
                int asw_c = asw;
                int evasion_c = evasion;
                int los_c = los;
                int luck_c = luck;
                int range = ship.Range;

                asw = Math.Max(asw, 0);
                evasion = Math.Max(evasion, 0);
                los = Math.Max(los, 0);

                if (slot != null)
                {
                    int count = slot.Length;
                    for (int i = 0; i < count; i++)
                    {
                        EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[slot[i]];
                        if (eq == null) continue;

                        firepower += eq.Firepower;
                        torpedo += eq.Torpedo;
                        aa += eq.AA;
                        armor += eq.Armor;
                        asw += eq.ASW;
                        evasion += eq.Evasion;
                        los += eq.LOS;
                        luck += eq.Luck;
                        range = Math.Max(range, eq.Range);
                    }
                }

                return string.Format(
                            "{0} {1}{2}\n耐久: {3}\n火力: {4}/{5}\n雷装: {6}/{7}\n対空: {8}/{9}\n加权对空: {22:0.##}\n装甲: {10}/{11}\n対潜: {12}/{13}\n回避: {14}/{15}\n索敵: {16}/{17}\n運: {18}/{19}\n射程: {20} / 速力: {21}\n(右键单击查看图鉴)\n",
                            ship.ShipTypeName, ship.NameWithClass, level < 1 ? "" : string.Format(" Lv. {0}", level),
                            hp,
                            firepower_c,

                            (ship.ShipType == 7 ||	// 轻空母
                            ship.ShipType == 11 ||	// 正规空母
                            ship.IsLandBase) ?		// 陆基
                            string.Format("{0}（空母火力：{1:F0}）", firepower, CalculatorEx.CalculateFireEnemy(shipID, slot, firepower_c, torpedo_c)) :
                            firepower.ToString(),

                            torpedo_c, torpedo,
                            aa_c, aa,
                            armor_c, armor,
                            asw_c == -1 ? "???" : asw_c.ToString(), asw,
                            evasion_c == -1 ? "???" : evasion_c.ToString(), evasion,
                            los_c == -1 ? "???" : los_c.ToString(), los,
                            luck_c, luck,
                            Constants.GetRange(range),
                            Constants.GetSpeed(ship.Speed),
                            CalculatorEx.CalculateWeightingAAEnemy(shipID, slot, aa_c)
                            );
            }

            private string GetEquipmentString(int shipID, int[] slot)
            {
                StringBuilder sb = new StringBuilder();
                ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];

                if (ship == null || slot == null) return null;

                for (int i = 0; i < slot.Length; i++)
                {
                    if (slot[i] != -1)
                        sb.AppendFormat("[{0}] {1}\r\n", ship.Aircraft[i], KCDatabase.Instance.MasterEquipments[slot[i]].Name);
                }

                sb.AppendFormat("\r\n昼戦: {0}\r\n夜戦: {1}\r\n",
                    Constants.GetDayAttackKind(Calculator.GetDayAttackKind(slot, ship.ShipID, -1)),
                    Constants.GetNightAttackKind(Calculator.GetNightAttackKind(slot, ship.ShipID, -1)));

                {
                    int aacutin = Calculator.GetAACutinKind(shipID, slot);
                    if (aacutin != 0)
                    {
                        sb.AppendFormat("対空: {0}\r\n", Constants.GetAACutinKind(aacutin));
                    }
                }
                {
                    int airsup = Calculator.GetAirSuperiority(slot, ship.Aircraft.ToArray());
                    if (airsup > 0)
                    {
                        sb.AppendFormat("制空戦力: {0}\r\n", airsup);
                    }
                }

                return sb.ToString();
            }


            void ShipName_MouseClick(object sender, MouseEventArgs e)
            {

                if ((e.Button & System.Windows.Forms.MouseButtons.Right) != 0)
                {
                    int? shipID = ShipName.Tag as int?;

                    if (shipID != null && shipID != -1)
                        new DialogAlbumMasterShip((int)ShipName.Tag).Show(Parent);
                }

            }


        }


        public Font MainFont { get; set; }
        public Font SubFont { get; set; }
        public Color MainFontColor { get; set; }
        public Color SubFontColor { get; set; }

        private Pen LinePen = Pens.Silver;


        private TableEnemyMemberControl[] ControlMember;

        public FormFleetData()
        {
            this.SuspendLayoutForDpiScale();
            InitializeComponent();

            TopLevel = false;

            ControlHelper.SetDoubleBuffered(BasePanel);
            ControlHelper.SetDoubleBuffered(TableEnemyMember);

            TableEnemyMember.SuspendLayout();
            ControlMember = new TableEnemyMemberControl[6];
            for (int i = 0; i < ControlMember.Length; i++)
            {
                ControlMember[i] = new TableEnemyMemberControl(this, TableEnemyMember, i);
            }
            TableEnemyMember.ResumeLayout();

            try
            {
                Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormCompass]);
            }
            catch
            {

            }

            this.ResumeLayoutForDpiScale();
        }


        private void Form_Load(object sender, EventArgs e)
        {

            BasePanel.Visible = false;
            TextAirSuperiority.ImageList = ResourceManager.Instance.Equipments;
            TextAirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;

            ConfigurationChanged();
        }

        public void UpdateEnemyFleet(FleetPart Fleet)
        {

            TextEnemyFleetName.Text = Fleet.Part;

            TextFormation.Text = Fleet.Formation + "陣";
            TextFormation.Visible = true;
            int airSuperiority = Calculator.GetAirSuperiority(Fleet.EnemyShips.ToArray());
            TextAirSuperiority.Text = airSuperiority.ToString();
            //string.Format( "{0}，优势 {1:F0}，确保 {2:F0}", airSuperiority, airSuperiority * 1.5, airSuperiority * 3 );
            ToolTipInfo.SetToolTip(TextAirSuperiority, string.Format("优势 {0:F0}，确保 {1:F0}", airSuperiority * 1.5, airSuperiority * 3));
            //TextAA.Text = CalculatorEx.GetEnemyFleetAAValue(enemies, bd.Searching.FormationEnemy).ToString();

            TableEnemyMember.SuspendLayout();
            for (int i = 0; i < ControlMember.Length; i++)
            {

                int shipID = -1;
                if (Fleet.EnemyShips.Count > i)
                    shipID = Fleet.EnemyShips[i];
                ControlMember[i].Update(shipID);

                if (shipID >0)
                    ControlMember[i].UpdateEquipmentToolTip(shipID);
            }
            TableEnemyMember.ResumeLayout();
            TableEnemyMember.Visible = true;

            PanelEnemyFleet.Visible = true;
            TextEnemyFleetName.Visible =
            TextFormation.Visible =
            TextAirSuperiority.Visible = true;
            BasePanel.Visible = true;			//checkme

        }

        void ConfigurationChanged()
        {

            Font = PanelEnemyFleet.Font = MainFont = ElectronicObserver.Utility.Configuration.Config.UI.MainFont;
            SubFont = ElectronicObserver.Utility.Configuration.Config.UI.SubFont;
            BackColor = ElectronicObserver.Utility.Configuration.Config.UI.BackColor;
            MainFontColor = ElectronicObserver.Utility.Configuration.Config.UI.ForeColor;
            SubFontColor = ElectronicObserver.Utility.Configuration.Config.UI.SubForeColor;

            LinePen = new Pen(ElectronicObserver.Utility.Configuration.Config.UI.LineColor.ColorData);

            if (ControlMember != null)
            {
                bool flag = ElectronicObserver.Utility.Configuration.Config.FormFleet.ShowAircraft;
                for (int i = 0; i < ControlMember.Length; i++)
                {
                    ControlMember[i].Equipments.ShowAircraft = flag;
                }
            }
        }

        private void TableEnemyMember_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            e.Graphics.DrawLine(LinePen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
        }

    }

}
