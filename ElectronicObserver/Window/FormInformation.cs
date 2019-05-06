using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
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

    public partial class FormInformation : DockContent
    {

        private int _ignorePort;
        private List<int> _inSortie;
        private int[] _prevResource;

        public FormInformation(FormMain parent)
        {
            InitializeComponent();

            _ignorePort = 0;
            _inSortie = null;
            _prevResource = new int[4];

            ConfigurationChanged();

            Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormInformation]);
        }


        private void FormInformation_Load(object sender, EventArgs e)
        {

            APIObserver o = APIObserver.Instance;

            o["api_port/port"].ResponseReceived += Updated;
            o["api_req_member/get_practice_enemyinfo"].ResponseReceived += Updated;
            o["api_get_member/picture_book"].ResponseReceived += Updated;
            o["api_req_kousyou/createitem"].ResponseReceived += Updated;
            o["api_get_member/mapinfo"].ResponseReceived += Updated;
            o["api_req_mission/result"].ResponseReceived += Updated;
            o["api_req_practice/battle_result"].ResponseReceived += Updated;
            o["api_req_sortie/battleresult"].ResponseReceived += Updated;
            o["api_req_combined_battle/battleresult"].ResponseReceived += Updated;
            o["api_req_hokyu/charge"].ResponseReceived += Updated;
            o["api_req_map/start"].ResponseReceived += Updated;
            o["api_req_map/next"].ResponseReceived += Updated;
            o["api_req_practice/battle"].ResponseReceived += Updated;
            o["api_get_member/sortie_conditions"].ResponseReceived += Updated;
            o["api_req_mission/start"].RequestReceived += Updated;

            Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
        }


        void ConfigurationChanged()
        {

            Font = TextInformation.Font = Utility.Configuration.Config.UI.MainFont;
            TextInformation.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
        }


        void Updated(string apiname, dynamic data)
        {

            switch (apiname)
            {

                case "api_port/port":
                    if (_ignorePort > 0)
                        _ignorePort--;
                    else
                        TextInformation.Text = "";      //とりあえずクリア

                    if (_inSortie != null)
                    {
                        TextInformation.Text = GetConsumptionResource(data);
                    }
                    _inSortie = null;

                    RecordMaterials();

                    // '16 summer event
                    if (data.api_event_object() && data.api_event_object.api_m_flag2() && (int)data.api_event_object.api_m_flag2 > 0)
                    {
                        TextInformation.Text += "\r\n＊ギミック解除＊\r\n";
                        Utility.Logger.Add(2, "敵勢力の弱体化を確認しました！");
                    }
                    break;

                case "api_req_member/get_practice_enemyinfo":
                    TextInformation.Text = GetPracticeEnemyInfo(data);
                    RecordMaterials();
                    break;

                case "api_get_member/picture_book":
                    TextInformation.Text = GetAlbumInfo(data);
                    break;

                case "api_req_kousyou/createitem":
                    TextInformation.Text = GetCreateItemInfo(data);
                    break;

                case "api_get_member/mapinfo":
                    TextInformation.Text = GetMapGauge(data);
                    break;

                case "api_req_mission/result":
                    TextInformation.Text = GetExpeditionResult(data);
                    _ignorePort = 1;
                    break;

                case "api_req_practice/battle_result":
                case "api_req_sortie/battleresult":
                case "api_req_combined_battle/battleresult":
                    TextInformation.Text = GetBattleResult(data);
                    break;

                case "api_req_hokyu/charge":
                    TextInformation.Text = GetSupplyInformation(data);
                    break;

                case "api_req_mission/start":
                    if (Utility.Configuration.Config.Control.ShowExpeditionAlertDialog)
                        CheckExpedition(int.Parse(data["api_mission_id"]), int.Parse(data["api_deck_id"]));
                    break;

                case "api_get_member/sortie_conditions":
                    CheckSallyArea();
                    break;

                case "api_req_map/start":
                    _inSortie = KCDatabase.Instance.Fleet.Fleets.Values.Where(f => f.IsInSortie || f.ExpeditionState == 1).Select(f => f.FleetID).ToList();

                    RecordMaterials();
                    break;

                case "api_req_map/next":
                    {
                        var str = CheckGimmickUpdated(data);
                        if (!string.IsNullOrWhiteSpace(str))
                            TextInformation.Text = str;

                        if (data.api_destruction_battle())
                        {
                            str = CheckGimmickUpdated(data.api_destruction_battle);
                            if (!string.IsNullOrWhiteSpace(str))
                                TextInformation.Text = str;
                        }

                    }
                    break;

                case "api_req_practice/battle":
                    _inSortie = new List<int>() { KCDatabase.Instance.Battle.BattleDay.Initial.FriendFleetID };
                    break;

            }

        }


        private string GetPracticeEnemyInfo(dynamic data)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[演習情報]");
            sb.AppendLine("敵提督名 : " + data.api_nickname);
            sb.AppendLine("敵艦隊名 : " + data.api_deckname);

            {
                int ship1lv = (int)data.api_deck.api_ships[0].api_id != -1 ? (int)data.api_deck.api_ships[0].api_level : 1;
                int ship2lv = (int)data.api_deck.api_ships[1].api_id != -1 ? (int)data.api_deck.api_ships[1].api_level : 1;

                // 経験値テーブルが拡張されたとき用の対策
                ship1lv = Math.Min(ship1lv, ExpTable.ShipExp.Keys.Max());
                ship2lv = Math.Min(ship2lv, ExpTable.ShipExp.Keys.Max());

                double expbase = ExpTable.ShipExp[ship1lv].Total / 100.0 + ExpTable.ShipExp[ship2lv].Total / 300.0;
                if (expbase >= 500.0)
                    expbase = 500.0 + Math.Sqrt(expbase - 500.0);

                expbase = (int)expbase;

                sb.AppendFormat("獲得経験値: {0} / S勝利: {1}\r\n", expbase, (int)(expbase * 1.2));


                // 練巡ボーナス計算 - きたない
                var fleet = KCDatabase.Instance.Fleet[1];
                if (fleet.MembersInstance.Any(s => s != null && s.MasterShip.ShipType == ShipTypes.TrainingCruiser))
                {
                    var members = fleet.MembersInstance;
                    var subCT = members.Skip(1).Where(s => s != null && s.MasterShip.ShipType == ShipTypes.TrainingCruiser);

                    double bonus;

                    // 旗艦が練巡
                    if (members[0] != null && members[0].MasterShip.ShipType == ShipTypes.TrainingCruiser)
                    {

                        int level = members[0].Level;

                        if (subCT != null && subCT.Any())
                        {
                            // 旗艦+随伴
                            if (level < 10) bonus = 1.10;
                            else if (level < 30) bonus = 1.13;
                            else if (level < 60) bonus = 1.16;
                            else if (level < 100) bonus = 1.20;
                            else bonus = 1.25;

                        }
                        else
                        {
                            // 旗艦のみ
                            if (level < 10) bonus = 1.05;
                            else if (level < 30) bonus = 1.08;
                            else if (level < 60) bonus = 1.12;
                            else if (level < 100) bonus = 1.15;
                            else bonus = 1.20;
                        }

                    }
                    else
                    {

                        int level = subCT.Max(s => s.Level);

                        if (subCT.Count() > 1)
                        {
                            // 随伴複数	
                            if (level < 10) bonus = 1.04;
                            else if (level < 30) bonus = 1.06;
                            else if (level < 60) bonus = 1.08;
                            else if (level < 100) bonus = 1.12;
                            else bonus = 1.175;

                        }
                        else
                        {
                            // 随伴単艦
                            if (level < 10) bonus = 1.03;
                            else if (level < 30) bonus = 1.05;
                            else if (level < 60) bonus = 1.07;
                            else if (level < 100) bonus = 1.10;
                            else bonus = 1.15;
                        }
                    }

                    sb.AppendFormat("(練巡強化: {0} / S勝利: {1})\r\n", (int)(expbase * bonus), (int)((int)(expbase * 1.2) * bonus));


                }
            }

            return sb.ToString();
        }


        private string GetAlbumInfo(dynamic data)
        {

            StringBuilder sb = new StringBuilder();

            if (data != null && data.api_list() && data.api_list != null)
            {

                if (data.api_list[0].api_yomi())
                {
                    //艦娘図鑑
                    const int bound = 70;       // 図鑑1ページあたりの艦船数
                    int startIndex = (((int)data.api_list[0].api_index_no - 1) / bound) * bound + 1;
                    bool[] flags = Enumerable.Repeat<bool>(false, bound).ToArray();

                    sb.AppendLine("[中破絵未回収]");

                    foreach (dynamic elem in data.api_list)
                    {

                        flags[(int)elem.api_index_no - startIndex] = true;

                        dynamic[] state = elem.api_state;
                        for (int i = 0; i < state.Length; i++)
                        {
                            if ((int)state[i][1] == 0)
                            {

                                var target = KCDatabase.Instance.MasterShips[(int)elem.api_table_id[i]];
                                if (target != null)     //季節の衣替え艦娘の場合存在しないことがある
                                    sb.AppendLine(target.Name);
                            }
                        }

                    }

                    sb.AppendLine("[未保有艦]");
                    for (int i = 0; i < bound; i++)
                    {
                        if (!flags[i])
                        {
                            ShipDataMaster ship = KCDatabase.Instance.MasterShips.Values.FirstOrDefault(s => s.AlbumNo == startIndex + i);
                            if (ship != null)
                            {
                                sb.AppendLine(ship.Name);
                            }
                        }
                    }

                }
                else
                {
                    //装備図鑑
                    const int bound = 70;       // 図鑑1ページあたりの装備数
                    int startIndex = (((int)data.api_list[0].api_index_no - 1) / bound) * bound + 1;
                    bool[] flags = Enumerable.Repeat<bool>(false, bound).ToArray();

                    foreach (dynamic elem in data.api_list)
                    {

                        flags[(int)elem.api_index_no - startIndex] = true;
                    }

                    sb.AppendLine("[未保有装備]");
                    for (int i = 0; i < bound; i++)
                    {
                        if (!flags[i])
                        {
                            EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments.Values.FirstOrDefault(s => s.AlbumNo == startIndex + i);
                            if (eq != null)
                            {
                                sb.AppendLine(eq.Name);
                            }
                        }
                    }
                }
            }

            return sb.ToString();
        }


        private string GetCreateItemInfo(dynamic data)
        {

            if ((int)data.api_create_flag == 0)
            {

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("[開発失敗]");
                sb.AppendLine(data.api_fdata);

                EquipmentDataMaster eqm = KCDatabase.Instance.MasterEquipments[int.Parse(((string)data.api_fdata).Split(",".ToCharArray())[1])];
                if (eqm != null)
                    sb.AppendLine(eqm.Name);


                return sb.ToString();

            }
            else
                return "";
        }


        private string GetMapGauge(dynamic data)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[海域ゲージ]");


            foreach (var map in KCDatabase.Instance.MapInfo.Values)
            {
                int gaugeType = -1;
                int current = 0;
                int max = 0;

                if (map.RequiredDefeatedCount != -1 && map.CurrentDefeatedCount < map.RequiredDefeatedCount)
                {
                    gaugeType = 1;
                    current = map.CurrentDefeatedCount;
                    max = map.RequiredDefeatedCount;
                }
                else if (map.MapHPMax > 0)
                {
                    gaugeType = map.GaugeType;
                    current = map.MapHPCurrent;
                    max = map.MapHPMax;
                }

                if (gaugeType > 0)
                {
                    sb.AppendLine(string.Format("{0}-{1}{2}: {3}{4} {5} / {6}{7}",
                        map.MapAreaID, map.MapInfoID,
                        map.EventDifficulty > 0 ? $" [{Constants.GetDifficulty(map.EventDifficulty)}]" : "",
                        map.CurrentGaugeIndex > 0 ? $"#{map.CurrentGaugeIndex} " : "",
                        gaugeType == 1 ? "撃破" : gaugeType == 2 ? "HP" : "TP",
                        current, max,
                        gaugeType == 1 ? " 回" : ""));
                }
            }


            return sb.ToString();
        }


        private string GetExpeditionResult(dynamic data)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("[遠征帰投]");
            sb.AppendLine(data.api_quest_name);
            sb.AppendFormat("結果: {0}\r\n", Constants.GetExpeditionResult((int)data.api_clear_result));
            sb.AppendFormat("提督経験値: +{0}\r\n", (int)data.api_get_exp);
            sb.AppendFormat("艦娘経験値: +{0}\r\n", ((int[])data.api_get_ship_exp).Min());

            return sb.ToString();
        }


        private string GetBattleResult(dynamic data)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("[戦闘終了]");
            sb.AppendFormat("敵艦隊名: {0}\r\n", data.api_enemy_info.api_deck_name);
            sb.AppendFormat("勝敗判定: {0}\r\n", data.api_win_rank);
            sb.AppendFormat("提督経験値: +{0}\r\n", (int)data.api_get_exp);

            sb.Append(CheckGimmickUpdated(data));

            return sb.ToString();
        }

        private string CheckGimmickUpdated(dynamic data)
        {
            if (data.api_m1() && data.api_m1 == 1)
            {
                Utility.Logger.Add(2, "海域に変化を確認しました！");
                return "\r\n＊ギミック解除＊\r\n";
            }

            return "";
        }


        private string GetSupplyInformation(dynamic data)
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("[補給完了]");
            sb.AppendFormat("ボーキサイト: {0} ( {1}機 )\r\n", (int)data.api_use_bou, (int)data.api_use_bou / 5);

            return sb.ToString();
        }


        private string GetConsumptionResource(dynamic data)
        {

            StringBuilder sb = new StringBuilder();
            var material = KCDatabase.Instance.Material;


            int fuel_diff = material.Fuel - _prevResource[0],
                ammo_diff = material.Ammo - _prevResource[1],
                steel_diff = material.Steel - _prevResource[2],
                bauxite_diff = material.Bauxite - _prevResource[3];


            var ships = KCDatabase.Instance.Fleet.Fleets.Values
                .Where(f => _inSortie.Contains(f.FleetID))
                .SelectMany(f => f.MembersInstance)
                .Where(s => s != null);

            int fuel_supply = ships.Sum(s => s.SupplyFuel);
            int ammo = ships.Sum(s => s.SupplyAmmo);
            int bauxite = ships.Sum(s => s.Aircraft.Zip(s.MasterShip.Aircraft, (current, max) => new { Current = current, Max = max }).Sum(a => (a.Max - a.Current) * 5));

            int fuel_repair = ships.Sum(s => s.RepairFuel);
            int steel = ships.Sum(s => s.RepairSteel);


            sb.AppendLine("[艦隊帰投]");
            sb.AppendFormat("燃料: {0:+0;-0} ( 自然 {1:+0;-0} - 補給 {2} - 入渠 {3} )\r\n弾薬: {4:+0;-0} ( 自然 {5:+0;-0} - 補給 {6} )\r\n鋼材: {7:+0;-0} ( 自然 {8:+0;-0} - 入渠 {9} )\r\nボーキ: {10:+0;-0} ( 自然 {11:+0;-0} - 補給 {12} ( {13} 機 ) )",
                fuel_diff - fuel_supply - fuel_repair, fuel_diff, fuel_supply, fuel_repair,
                ammo_diff - ammo, ammo_diff, ammo,
                steel_diff - steel, steel_diff, steel,
                bauxite_diff - bauxite, bauxite_diff, bauxite, bauxite / 5);

            return sb.ToString();
        }


        private void CheckSallyArea()
        {
            if (KCDatabase.Instance.Ships.Values.First().SallyArea == -1)   // そもそも札情報がなければやる必要はない
                return;

            IEnumerable<IEnumerable<ShipData>> group;

            if (KCDatabase.Instance.Fleet.CombinedFlag != 0)
                group = new[] { KCDatabase.Instance.Fleet[1].MembersInstance.Concat(KCDatabase.Instance.Fleet[2].MembersInstance).Where(s => s != null) };
            else
                group = KCDatabase.Instance.Fleet.Fleets.Values
                    .Where(f => f?.ExpeditionState == 0)
                    .Select(f => f.MembersInstance.Where(s => s != null));


            group = group.Where(ss =>
                ss.All(s => s.RepairingDockID == -1) &&
                ss.Any(s => s.SallyArea == 0) &&
                ss.Select(s => s.SallyArea).Distinct().Count() <= 2);   // 札が(なしも含めて)3種類以上なら、出撃できない or 自由出撃海域なので除外


            if (group.Any())
            {
                var freeShips = group.SelectMany(f => f).Where(s => s.SallyArea == 0);

                TextInformation.Text = "[誤出撃警告]\r\n札なし艦娘：\r\n" + string.Join("\r\n", freeShips.Select(s => s.NameWithLevel));

                if (Utility.Configuration.Config.Control.ShowSallyAreaAlertDialog)
                    MessageBox.Show("出撃札がついていない艦娘が編成されています。\r\n注意して出撃してください。\r\n\r\n（この警告は 設定→動作 から無効化できます。）", "誤出撃警告",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CheckExpedition(int missionID, int fleetID)
        {
            var fleet = KCDatabase.Instance.Fleet[fleetID];
            var result = MissionClearCondition.Check(missionID, fleet);

            if (!result.IsSuceeded)
            {
                var mission = KCDatabase.Instance.Mission[missionID];
                MessageBox.Show(
                    $"#{fleet.FleetID} {fleet.Name} の遠征 {mission.DisplayID}:{mission.Name} は、失敗する可能性があります。\r\n\r\n{string.Join("\r\n", result.FailureReason)}\r\n\r\n（この警告は 設定→動作 から無効化できます。）",
                    "遠征失敗警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void RecordMaterials()
        {
            var material = KCDatabase.Instance.Material;
            _prevResource[0] = material.Fuel;
            _prevResource[1] = material.Ammo;
            _prevResource[2] = material.Steel;
            _prevResource[3] = material.Bauxite;
        }

        protected override string GetPersistString()
        {
            return "Information";
        }

    }

}
