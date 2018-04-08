using ElectronicObserver.Data;
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

namespace ElectronicObserver.Window.Dialog
{
	public partial class DialogExpChecker : Form
	{
		public DialogExpChecker()
		{
			InitializeComponent();
		}

		private void DialogExpChecker_Load(object sender, EventArgs e)
		{
			var ships = KCDatabase.Instance.Ships.Values;

			if (!ships.Any())
			{
				MessageBox.Show("艦船が存在しません。\r\n母港まで移動してください。", "対象艦船なし", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
			}

			TextShip.Items.AddRange(ships
				.OrderBy(s => s.MasterShip.ShipType)
				.ThenByDescending(s => s.Level)
				.Select(s => new ComboShipData(s)).ToArray());



			this.Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormExpChecker]);
		}

		private void DialogExpChecker_FormClosed(object sender, FormClosedEventArgs e)
		{
			ResourceManager.DestroyIcon(Icon);
		}


		private void ButtonStart_Click(object sender, EventArgs e)
		{
			var selectedShip = (TextShip.SelectedItem as ComboShipData)?.Ship;

			if (selectedShip == null)
			{
				System.Media.SystemSounds.Asterisk.Play();
				return;
			}


			LevelView.SuspendLayout();

			LevelView.Rows.Clear();


			// 空母系は面倒なので省略
			int openingASWborder = selectedShip.MasterShip.ShipType == ShipTypes.Escort ? 60 : 100;

			var ASWEquipmentPairs = new Dictionary<int, string>();
			if (ShowAllASWEquipments.Checked)
			{

				var had = KCDatabase.Instance.Equipments.Values
					.Where(eq => eq.MasterEquipment.CategoryType == EquipmentTypes.Sonar || eq.MasterEquipment.CategoryType == EquipmentTypes.DepthCharge)
					.GroupBy(eq => eq.EquipmentID)
					.Select(g => new ASWEquipmentData { ID = g.Key, ASW = g.First().MasterEquipment.ASW, Name = g.First().MasterEquipment.Name, IsSonar = g.First().MasterEquipment.IsSonar, Count = g.Count() })
					.Concat(new[] { new ASWEquipmentData { ID = -1, ASW = 0, Name = "", Count = 99, IsSonar = false } })
					.OrderByDescending(a => a.ASW)
					.ToArray();

				var stack = Enumerable.Repeat(0, selectedShip.SlotSize).ToArray();

				var pair = new Dictionary<int, List<ASWEquipmentData[]>>();

				if (had.Length > 0 && stack.Length > 0)
				{

					while (stack[0] != -1)
					{
						var convert = stack.Select(i => had[i]).ToArray();


						if (convert.Any(c => c.IsSonar) && stack.GroupBy(s => s).All(s => had[s.Key].Count >= s.Count()))
						{
							int aswsum = convert.Sum(c => c.ASW);

							if (!pair.ContainsKey(aswsum))
								pair.Add(aswsum, new List<ASWEquipmentData[]>() { convert });
							else
								pair[aswsum].Add(convert);
						}

						for (int p = stack.Length - 1; p >= 0; p--)
						{
							stack[p]++;
							if (stack[p] < had.Length)
								break;
							stack[p] = -1;
						}
						for (int p = 1; p < stack.Length; p++)
						{
							if (stack[p] == -1)
								stack[p] = stack[p - 1];
						}
					}
				}

				foreach (var x in pair)
				{
					// 要するに下のようなフォーマットにする
					ASWEquipmentPairs.Add(openingASWborder - x.Key,
						string.Join(", ",
							x.Value.OrderBy(a => a.Count(b => b.ID > 0))
								.Select(a => $"[{string.Join(", ", a.Where(b => b.ID > 0).GroupBy(b => b.ID).Select(b => b.Count() == 1 ? b.First().Name : $"{b.First().Name}x{b.Count()}"))}]")));
				}
			}
			else
			{
				ASWEquipmentPairs.Add(openingASWborder - 36, "[四式水中聴音機x3]");
				ASWEquipmentPairs.Add(openingASWborder - 32, "[四式水中聴音機x2, 三式爆雷投射機]");
				ASWEquipmentPairs.Add(openingASWborder - 28, "[三式水中探信儀x2, 三式爆雷投射機]");
				ASWEquipmentPairs.Add(openingASWborder - 27, "[四式水中聴音機, 三式爆雷投射機, 二式爆雷]");
				ASWEquipmentPairs.Add(openingASWborder - 12, "[四式水中聴音機]");
			}




			int aswmin = selectedShip.MasterShip.ASW.Minimum;
			int aswmax = selectedShip.MasterShip.ASW.Maximum;
			int aswmod = (int)ASWModernization.Value;
			int currentlv = selectedShip.Level;
			int minlv = ShowAllLevel.Checked ? 1 : (currentlv + 1);
			int unitexp = Math.Max((int)numericUpDown1.Value, 1);

			var rows = new DataGridViewRow[ExpTable.ShipMaximumLevel - (minlv - 1)];

			for (int lv = minlv; lv <= ExpTable.ShipMaximumLevel; lv++)
			{
				int asw = aswmin + ((aswmax - aswmin) * lv / 99) + aswmod;

				int needexp = ExpTable.ShipExp[lv].Total - selectedShip.ExpTotal;

				var row = new DataGridViewRow();
				row.CreateCells(LevelView);
				row.SetValues(
					lv,
					Math.Max(needexp, 0),
					Math.Max((int)Math.Ceiling((double)needexp / unitexp), 0),
					asw,
					ASWEquipmentPairs.Where(k => asw >= k.Key).OrderByDescending(p => p.Key).FirstOrDefault().Value ?? "-"
					);


				if (lv % 5 == 0)
				{
					row.Cells[0].Style.BackColor = Color.Moccasin;
				}
				if (selectedShip.MasterShip.RemodelAfterLevel == lv)
				{
					row.Cells[0].Style.BackColor = Color.LightGreen;
				}

				rows[lv - minlv] = row;
			}

			LevelView.Rows.AddRange(rows);

			LevelView.ResumeLayout();
		}


		private void TextShip_SelectionChangeCommitted(object sender, EventArgs e)
		{
			var selectedShip = (TextShip.SelectedItem as ComboShipData)?.Ship;

			if (selectedShip == null)
			{
				LabelShip.Text = "-";
				return;
			}

			LabelShip.Text = $"Exp. {selectedShip.ExpTotal}, 対潜 {selectedShip.ASWBase} (改修+{selectedShip.ASWModernized})";

			ASWModernization.Value = selectedShip.ASWModernized;
		}



		private class ComboShipData
		{
			public ShipData Ship;

			public ComboShipData(ShipData ship)
			{
				Ship = ship;
			}

			public override string ToString() => $"{Ship.MasterShip.ShipTypeName} {Ship.NameWithLevel}";
		}

		private class ASWEquipmentData
		{
			public int ID;
			public int ASW;
			public string Name;
			public bool IsSonar;
			public int Count;

			public override string ToString() => Name;
		}

	}
}
