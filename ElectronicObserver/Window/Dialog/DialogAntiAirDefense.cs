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
	public partial class DialogAntiAirDefense : Form
	{

		private class AACutinComboBoxData
		{
			public readonly int Kind;
			public AACutinComboBoxData(int kind)
			{
				Kind = kind;
			}

			public override string ToString() => $"{Kind}: {Constants.GetAACutinKind(Kind)}";


			public static implicit operator int(AACutinComboBoxData data)
			{
				if (data == null)
					return -1;
				return data.Kind;
			}
		}

		private class FormationComboBoxData
		{
			public readonly int Formation;
			public FormationComboBoxData(int formation)
			{
				Formation = formation;
			}

			public override string ToString() => Constants.GetFormation(Formation);


			public static implicit operator int(FormationComboBoxData data)
			{
				if (data == null)
					return -1;
				return data.Formation;
			}
		}


		public DialogAntiAirDefense()
		{
			InitializeComponent();
		}

		private void DialogAntiAirDefense_Load(object sender, EventArgs e)
		{

			if (!KCDatabase.Instance.Fleet.IsAvailable)
			{
				MessageBox.Show("艦隊データが読み込まれていません。\r\n艦これを起動してから開いてください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
				return;
			}

			FleetID.SelectedIndex = 0;
			Formation.SelectedIndex = 0;

			UpdateAACutinKind(ShowAll.Checked);
			UpdateFormation();

			this.Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormAntiAirDefense]);
		}

		private void DialogAntiAirDefense_FormClosed(object sender, FormClosedEventArgs e)
		{
			ResourceManager.DestroyIcon(Icon);
		}


		public void SetFleetID(int id)
		{
			FleetID.SelectedIndex = id - 1;
		}

		private void Updated()
		{

			ShipData[] ships = GetShips().ToArray();
			int formation = Formation.SelectedItem as FormationComboBoxData;
			int aaCutinKind = AACutinKind.SelectedItem as AACutinComboBoxData;
			int enemyAircraftCount = (int)EnemySlotCount.Value;


			// 加重対空値
			double[] adjustedAAs = ships.Select(s => s == null ? 0.0 : Calculator.GetAdjustedAAValue(s)).ToArray();

			// 艦隊防空値
			double adjustedFleetAA = Calculator.GetAdjustedFleetAAValue(ships, formation);

			// 割合撃墜
			double[] proportionalAAs = adjustedAAs.Select((val, i) => Calculator.GetProportionalAirDefense(val, IsCombined ? (i < 6 ? 1 : 2) : -1)).ToArray();

			// 固定撃墜
			int[] fixedAAs = adjustedAAs.Select((val, i) => Calculator.GetFixedAirDefense(val, adjustedFleetAA, aaCutinKind, IsCombined ? (i < 6 ? 1 : 2) : -1)).ToArray();



			int[] shootDownBoth = adjustedAAs.Select((val, i) => ships[i] == null ? 0 :
			   Calculator.GetShootDownCount(enemyAircraftCount, proportionalAAs[i], fixedAAs[i], aaCutinKind)).ToArray();

			int[] shootDownProportional = adjustedAAs.Select((val, i) => ships[i] == null ? 0 :
			   Calculator.GetShootDownCount(enemyAircraftCount, proportionalAAs[i], 0, aaCutinKind)).ToArray();

			int[] shootDownFixed = adjustedAAs.Select((val, i) => ships[i] == null ? 0 :
			   Calculator.GetShootDownCount(enemyAircraftCount, 0, fixedAAs[i], aaCutinKind)).ToArray();

			int[] shootDownFailed = adjustedAAs.Select((val, i) => ships[i] == null ? 0 :
			   Calculator.GetShootDownCount(enemyAircraftCount, 0, 0, aaCutinKind)).ToArray();



			ResultView.Rows.Clear();
			var rows = new DataGridViewRow[ships.Length];
			for (int i = 0; i < ships.Length; i++)
			{
				if (ships[i] == null)
					continue;

				rows[i] = new DataGridViewRow();
				rows[i].CreateCells(ResultView);

				rows[i].SetValues(ships[i].Name, ships[i].AABase, adjustedAAs[i], proportionalAAs[i], fixedAAs[i], shootDownBoth[i], shootDownProportional[i], shootDownFixed[i], shootDownFailed[i]);

			}
			ResultView.Rows.AddRange(rows.Where(r => r != null).ToArray());

			AdjustedFleetAA.Text = adjustedFleetAA.ToString("0.0");
			{
				var allShootDown = shootDownBoth.Concat(shootDownProportional).Concat(shootDownFixed).Concat(shootDownFailed);
				AnnihilationProbability.Text = (allShootDown.Count(i => i >= enemyAircraftCount) / Math.Max(ships.Count(s => s != null) * 4, 1.0)).ToString("p1");
			}
		}


		private IEnumerable<ShipData> GetShips()
		{
			if (FleetID.SelectedIndex < 4)
				return KCDatabase.Instance.Fleet[FleetID.SelectedIndex + 1].MembersWithoutEscaped;
			else
				return KCDatabase.Instance.Fleet[1].MembersWithoutEscaped.Concat(KCDatabase.Instance.Fleet[2].MembersWithoutEscaped);
		}

		private bool IsCombined => FleetID.SelectedIndex == 4;


		private void UpdateAACutinKind(bool showAll)
		{

			AACutinComboBoxData[] list;

			if (showAll)
			{

				int max = Calculator.AACutinFixedBonus.Keys.Max();
				list = Enumerable.Range(0, max + 1).Select(kind => new AACutinComboBoxData(kind)).ToArray();

			}
			else
			{

				list = GetShips()
					.Where(s => s != null)
					.Select(s => Calculator.GetAACutinKind(s.ShipID, s.AllSlotMaster.ToArray()))
					.Concat(Enumerable.Repeat(0, 1))
					.Distinct()
					.OrderBy(i => i)
					.Select(kind => new AACutinComboBoxData(kind)).ToArray();

			}

			AACutinKind.Items.Clear();
			AACutinKind.Items.AddRange(list);
			AACutinKind.SelectedIndex = 0;
		}

		private void UpdateFormation()
		{
			var items = (IsCombined ? Enumerable.Range(11, 4) : Enumerable.Range(1, 6))
				.Select(i => new FormationComboBoxData(i)).ToArray();

			int selected = Formation.SelectedItem as FormationComboBoxData;
			int index = Array.FindIndex(items, item => item == selected);

			Formation.Items.Clear();
			Formation.Items.AddRange(items);
			Formation.SelectedIndex = Math.Max(index, 0);
		}


		private void ResultView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{

			if (e.ColumnIndex == ResultView_ShootDownBoth.Index ||
				e.ColumnIndex == ResultView_ShootDownProportional.Index ||
				e.ColumnIndex == ResultView_ShootDownFixed.Index ||
				e.ColumnIndex == ResultView_ShootDownFailed.Index)
			{

				int value = e.Value as int? ?? 0;
				int enemySlot = (int)EnemySlotCount.Value;

				e.Value = string.Format("{0} ({1:p0})", value, (double)value / enemySlot);
				e.FormattingApplied = true;
				e.CellStyle.BackColor = e.CellStyle.SelectionBackColor =
					value >= enemySlot ? Color.MistyRose : SystemColors.Window;
			}

		}



		private void FleetID_SelectedIndexChanged(object sender, EventArgs e)
		{
			Updated();
			UpdateAACutinKind(ShowAll.Checked);
			UpdateFormation();
		}

		private void Formation_SelectedIndexChanged(object sender, EventArgs e)
		{
			Updated();
		}

		private void AACutinKind_SelectedIndexChanged(object sender, EventArgs e)
		{
			Updated();
		}

		private void EnemySlotCount_ValueChanged(object sender, EventArgs e)
		{
			Updated();
		}

		private void ShowAll_CheckedChanged(object sender, EventArgs e)
		{
			UpdateAACutinKind(ShowAll.Checked);
		}


	}
}
