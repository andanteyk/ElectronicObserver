using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
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

	public partial class FormFleetOverview : DockContent
	{

		private class TableFleetControl : IDisposable
		{

			public ImageLabel Number;
			public FleetState State;
			public ToolTip ToolTipInfo;
			private int fleetID;

			public TableFleetControl(FormFleetOverview parent, int fleetID)
			{

				#region Initialize

				Number = new ImageLabel
				{
					Anchor = AnchorStyles.Left,
					ImageAlign = ContentAlignment.MiddleCenter,
					Padding = new Padding(0, 1, 0, 1),
					Margin = new Padding(2, 1, 2, 1),
					Text = $"#{fleetID}:",
					AutoSize = true
				};

				State = new FleetState
				{
					Anchor = AnchorStyles.Left,
					Padding = new Padding(),
					Margin = new Padding(),
					AutoSize = true
				};

				ConfigurationChanged(parent);

				this.fleetID = fleetID;
				ToolTipInfo = parent.ToolTipInfo;

				#endregion

			}

			public TableFleetControl(FormFleetOverview parent, int fleetID, TableLayoutPanel table)
				: this(parent, fleetID)
			{

				AddToTable(table, fleetID - 1);
			}

			public void AddToTable(TableLayoutPanel table, int row)
			{

				table.Controls.Add(Number, 0, row);
				table.Controls.Add(State, 1, row);

			}


			public void Update()
			{

				FleetData fleet = KCDatabase.Instance.Fleet[fleetID];
				if (fleet == null) return;

				State.UpdateFleetState(fleet, ToolTipInfo);

				ToolTipInfo.SetToolTip(Number, fleet.Name);
			}


			public void Refresh()
			{

				State.RefreshFleetState();
			}


			public void ConfigurationChanged(FormFleetOverview parent)
			{
				Number.Font = parent.Font;
				State.Font = parent.Font;
				State.BackColor = Color.Transparent;
				Update();
			}

			public void Dispose()
			{
				Number.Dispose();
				State.Dispose();
			}
		}


		private List<TableFleetControl> ControlFleet;
		private ImageLabel CombinedTag;
		private ImageLabel AnchorageRepairingTimer;


		public FormFleetOverview(FormMain parent)
		{
			InitializeComponent();

			ControlHelper.SetDoubleBuffered(TableFleet);


			ControlFleet = new List<TableFleetControl>(4);
			for (int i = 0; i < 4; i++)
			{
				ControlFleet.Add(new TableFleetControl(this, i + 1, TableFleet));
			}

			{
				AnchorageRepairingTimer = new ImageLabel
				{
					Anchor = AnchorStyles.Left,
					Padding = new Padding(0, 1, 0, 1),
					Margin = new Padding(2, 1, 2, 1),
					ImageList = ResourceManager.Instance.Icons,
					ImageIndex = (int)ResourceManager.IconContent.FleetAnchorageRepairing,
					Text = "-",
					AutoSize = true
				};
				//AnchorageRepairingTimer.Visible = false;

				TableFleet.Controls.Add(AnchorageRepairingTimer, 1, 4);

			}

			#region CombinedTag
			{
				CombinedTag = new ImageLabel
				{
					Anchor = AnchorStyles.Left,
					Padding = new Padding(0, 1, 0, 1),
					Margin = new Padding(2, 1, 2, 1),
					ImageList = ResourceManager.Instance.Icons,
					ImageIndex = (int)ResourceManager.IconContent.FleetCombined,
					Text = "-",
					AutoSize = true,
					Visible = false
				};

				TableFleet.Controls.Add(CombinedTag, 1, 5);

			}
			#endregion



			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet]);

			Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;
		}



		private void FormFleetOverview_Load(object sender, EventArgs e)
		{

			//api register
			APIObserver o = APIObserver.Instance;

			o["api_req_nyukyo/start"].RequestReceived += Updated;
			o["api_req_nyukyo/speedchange"].RequestReceived += Updated;
			o["api_req_hensei/change"].RequestReceived += Updated;
			o["api_req_kousyou/destroyship"].RequestReceived += Updated;
			o["api_req_member/updatedeckname"].RequestReceived += Updated;
			o["api_req_map/start"].RequestReceived += Updated;
			o["api_req_hensei/combined"].RequestReceived += Updated;
			o["api_req_kaisou/open_exslot"].RequestReceived += Updated;

			o["api_port/port"].ResponseReceived += Updated;
			o["api_get_member/ship2"].ResponseReceived += Updated;
			o["api_get_member/ndock"].ResponseReceived += Updated;
			o["api_req_kousyou/getship"].ResponseReceived += Updated;
			o["api_req_hokyu/charge"].ResponseReceived += Updated;
			o["api_req_kousyou/destroyship"].ResponseReceived += Updated;
			o["api_get_member/ship3"].ResponseReceived += Updated;
			o["api_req_kaisou/powerup"].ResponseReceived += Updated;        //requestのほうは面倒なのでこちらでまとめてやる
			o["api_get_member/deck"].ResponseReceived += Updated;
			o["api_req_map/start"].ResponseReceived += Updated;
			o["api_req_map/next"].ResponseReceived += Updated;
			o["api_get_member/ship_deck"].ResponseReceived += Updated;
			o["api_req_hensei/preset_select"].ResponseReceived += Updated;
			o["api_req_kaisou/slot_exchange_index"].ResponseReceived += Updated;
			o["api_get_member/require_info"].ResponseReceived += Updated;
			o["api_req_kaisou/slot_deprive"].ResponseReceived += Updated;
			o["api_req_kaisou/marriage"].ResponseReceived += Updated;
			o["api_req_map/anchorage_repair"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}

		void ConfigurationChanged()
		{

			TableFleet.SuspendLayout();

			Font = Utility.Configuration.Config.UI.MainFont;

			AutoScroll = Utility.Configuration.Config.FormFleet.IsScrollable;

			foreach (var c in ControlFleet)
				c.ConfigurationChanged(this);

			CombinedTag.Font = Font;
			AnchorageRepairingTimer.Font = Font;
			AnchorageRepairingTimer.Visible = Utility.Configuration.Config.FormFleet.ShowAnchorageRepairingTimer;

			LayoutSubInformation();

			ControlHelper.SetTableRowStyles(TableFleet, ControlHelper.GetDefaultRowStyle());

			TableFleet.ResumeLayout();
		}


		private void Updated(string apiname, dynamic data)
		{

			TableFleet.SuspendLayout();

			TableFleet.RowCount = KCDatabase.Instance.Fleet.Fleets.Values.Count(f => f.IsAvailable);
			for (int i = 0; i < ControlFleet.Count; i++)
			{
				ControlFleet[i].Update();
			}

			if (KCDatabase.Instance.Fleet.CombinedFlag > 0)
			{
				CombinedTag.Text = Constants.GetCombinedFleet(KCDatabase.Instance.Fleet.CombinedFlag);

				var fleet1 = KCDatabase.Instance.Fleet[1];
				var fleet2 = KCDatabase.Instance.Fleet[2];

				int tp = Calculator.GetTPDamage(fleet1) + Calculator.GetTPDamage(fleet2);

				var members = fleet1.MembersWithoutEscaped.Concat(fleet2.MembersWithoutEscaped).Where(s => s != null);

				// 各艦ごとの ドラム缶 or 大発系 を搭載している個数
				var transport = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.TransportContainer));
				var landing = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.LandingCraft || eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank));


				ToolTipInfo.SetToolTip(CombinedTag, string.Format("ドラム缶搭載: {0}個\r\n大発動艇搭載: {1}個\r\n輸送量(TP): S {2} / A {3}\r\n\r\n制空戦力合計: {4}\r\n索敵能力合計: {5:f2}\r\n新判定式(33):\r\n　分岐点係数1: {6:f2}\r\n　分岐点係数2: {7:f2}\r\n　分岐点係数3: {8:f2}\r\n　分岐点係数4: {9:f2}",
					transport.Sum(),
					landing.Sum(),
					tp,
					(int)Math.Floor(tp * 0.7),
					Calculator.GetAirSuperiority(fleet1) + Calculator.GetAirSuperiority(fleet2),
					Math.Floor(fleet1.GetSearchingAbility() * 100) / 100 + Math.Floor(fleet2.GetSearchingAbility() * 100) / 100,
					Math.Floor(Calculator.GetSearchingAbility_New33(fleet1, 1) * 100) / 100 + Math.Floor(Calculator.GetSearchingAbility_New33(fleet2, 1) * 100) / 100,
					Math.Floor(Calculator.GetSearchingAbility_New33(fleet1, 2) * 100) / 100 + Math.Floor(Calculator.GetSearchingAbility_New33(fleet2, 2) * 100) / 100,
					Math.Floor(Calculator.GetSearchingAbility_New33(fleet1, 3) * 100) / 100 + Math.Floor(Calculator.GetSearchingAbility_New33(fleet2, 3) * 100) / 100,
					Math.Floor(Calculator.GetSearchingAbility_New33(fleet1, 4) * 100) / 100 + Math.Floor(Calculator.GetSearchingAbility_New33(fleet2, 4) * 100) / 100
					));


				CombinedTag.Visible = true;
			}
			else
			{
				CombinedTag.Visible = false;
			}

			if (KCDatabase.Instance.Fleet.AnchorageRepairingTimer > DateTime.MinValue)
			{
				AnchorageRepairingTimer.Text = DateTimeHelper.ToTimeElapsedString(KCDatabase.Instance.Fleet.AnchorageRepairingTimer);
				AnchorageRepairingTimer.Tag = KCDatabase.Instance.Fleet.AnchorageRepairingTimer;
				ToolTipInfo.SetToolTip(AnchorageRepairingTimer, "泊地修理タイマ\r\n開始: " + DateTimeHelper.TimeToCSVString(KCDatabase.Instance.Fleet.AnchorageRepairingTimer) + "\r\n回復: " + DateTimeHelper.TimeToCSVString(KCDatabase.Instance.Fleet.AnchorageRepairingTimer.AddMinutes(20)));
			}


			LayoutSubInformation();


			TableFleet.ResumeLayout();
		}



		void UpdateTimerTick()
		{
			for (int i = 0; i < ControlFleet.Count; i++)
			{
				ControlFleet[i].Refresh();
			}

			if (AnchorageRepairingTimer.Visible && AnchorageRepairingTimer.Tag != null)
				AnchorageRepairingTimer.Text = DateTimeHelper.ToTimeElapsedString((DateTime)AnchorageRepairingTimer.Tag);
		}


		// 空欄があれば詰める
		void LayoutSubInformation()
		{
			if (CombinedTag.Visible && !AnchorageRepairingTimer.Visible)
			{
				if (TableFleet.GetPositionFromControl(AnchorageRepairingTimer).Row != 5)
				{
					TableFleet.Controls.Remove(CombinedTag);
					TableFleet.Controls.Remove(AnchorageRepairingTimer);
					TableFleet.Controls.Add(CombinedTag, 1, 4);
					TableFleet.Controls.Add(AnchorageRepairingTimer, 1, 5);
				}
			}
			else
			{
				if (TableFleet.GetPositionFromControl(AnchorageRepairingTimer).Row != 4)
				{
					TableFleet.Controls.Remove(CombinedTag);
					TableFleet.Controls.Remove(AnchorageRepairingTimer);
					TableFleet.Controls.Add(AnchorageRepairingTimer, 1, 4);
					TableFleet.Controls.Add(CombinedTag, 1, 5);
				}
			}
		}


		private void TableFleet_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);

		}



		protected override string GetPersistString()
		{
			return "FleetOverview";
		}

	}

}
