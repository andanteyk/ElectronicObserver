using ElectronicObserver.Data;
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
	public partial class FormFleetPreset : DockContent
	{

		private class TablePresetControl : IDisposable
		{
			public ImageLabel Name;
			public ImageLabel[] Ships;

			private ToolTip _tooltip;

			public TablePresetControl(FormFleetPreset parent)
			{
				ImageLabel CreateDefaultLabel()
				{
					return new ImageLabel
					{
						Text = "",
						Anchor = AnchorStyles.Left,
						ForeColor = parent.ForeColor,
						Tag = null,
						TextAlign = ContentAlignment.MiddleLeft,
						Padding = new Padding(0, 1, 0, 1),
						Margin = new Padding(2, 1, 2, 1),
						AutoEllipsis = false,
						AutoSize = true,
						Visible = true,

						ImageList = ResourceManager.Instance.Icons,
						ImageAlign = ContentAlignment.MiddleCenter,
						ImageIndex = -1
					};
				}

				Name = CreateDefaultLabel();
				Name.ImageAlign = ContentAlignment.MiddleRight;

				// TODO: 本体側がもし 7 隻編成に対応したら変更してください
				Ships = new ImageLabel[6];
				for (int i = 0; i < Ships.Length; i++)
				{
					Ships[i] = CreateDefaultLabel();

				}

				_tooltip = parent.ToolTipInfo;
			}


			public void AddToTable(TableLayoutPanel table, int row)
			{
				table.Controls.Add(Name, 0, row);
				for (int i = 0; i < Ships.Length; i++)
				{
					table.Controls.Add(Ships[i], 1 + i, row);
				}
			}

			public void Update(int presetID)
			{
				var preset = KCDatabase.Instance.FleetPreset[presetID];

				if (preset == null)
				{
					Name.Text = "----";
					_tooltip.SetToolTip(Name, null);

					foreach (var ship in Ships)
					{
						ship.Text = string.Empty;
						_tooltip.SetToolTip(ship, null);
					}
					return;
				}


				Name.Text = preset.Name;

				int lowestCondition = preset.MembersInstance.Select(s => s?.Condition ?? 49).DefaultIfEmpty(49).Min();
				FormFleet.SetConditionDesign(Name, lowestCondition);

				_tooltip.SetToolTip(Name, $"最低cond: {lowestCondition}");

				for (int i = 0; i < Ships.Length; i++)
				{
					var ship = i >= preset.Members.Count ? null : preset.MembersInstance.ElementAt(i);
					var label = Ships[i];

					Ships[i].Text = ship?.Name ?? "-";

					if (ship == null)
					{
						_tooltip.SetToolTip(Ships[i], null);
					}
					else
					{
						var sb = new StringBuilder();
						sb.AppendLine($"{ship.MasterShip.ShipTypeName} {ship.NameWithLevel}");
						sb.AppendLine($"HP: {ship.HPCurrent} / {ship.HPMax} ({ship.HPRate:p1}) [{Constants.GetDamageState(ship.HPRate)}]");
						sb.AppendLine($"cond: {ship.Condition}");
						sb.AppendLine();

						var slot = ship.AllSlotInstance;
						for (int e = 0; e < slot.Count; e++)
						{
							if (slot[e] == null)
								continue;

							if (e < ship.MasterShip.Aircraft.Count)
							{
								sb.AppendLine($"[{ship.Aircraft[e]}/{ship.MasterShip.Aircraft[e]}] {slot[e].NameWithLevel}");
							}
							else
							{
								sb.AppendLine(slot[e].NameWithLevel);
							}
						}

						_tooltip.SetToolTip(Ships[i], sb.ToString());
					}
				}

			}

			public void ConfigurationChanged(FormFleetPreset parent)
			{
				var config = Utility.Configuration.Config;
				var font = config.UI.MainFont;

				Name.Font = font;
				Name.ImageAlign = config.FormFleet.ShowConditionIcon ? ContentAlignment.MiddleRight : ContentAlignment.MiddleCenter;

				foreach (var ship in Ships)
				{
					ship.Font = font;

					if (config.FormFleet.FixShipNameWidth)
					{
						ship.AutoSize = false;
						ship.Size = new Size(config.FormFleet.FixedShipNameWidth, 20);
					}
					else
					{
						ship.AutoSize = true;
					}
				}
			}

			public void Dispose()
			{
				Name.Dispose();
				foreach (var ship in Ships)
					ship.Dispose();
			}
		}


		private List<TablePresetControl> TableControls;



		public FormFleetPreset(FormMain parent)
		{
			InitializeComponent();

			// some initialization
			TableControls = new List<TablePresetControl>();
			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleetPreset]);
		}

		private void FormFleetPreset_Load(object sender, EventArgs e)
		{
			KCDatabase.Instance.FleetPreset.PresetChanged += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		}

		private void ConfigurationChanged()
		{
			var config = Utility.Configuration.Config;
			Font = Utility.Configuration.Config.UI.MainFont;
			bool fixShipNameWidth = config.FormFleet.FixShipNameWidth;

			TablePresets.SuspendLayout();
			foreach (var item in TableControls)
				item.ConfigurationChanged(this);

			for (int i = 1; i < TablePresets.ColumnCount; i++)
				ControlHelper.SetTableColumnStyle(TablePresets, i, fixShipNameWidth ?
					new ColumnStyle(SizeType.Absolute, config.FormFleet.FixedShipNameWidth + 4) :
					new ColumnStyle(SizeType.AutoSize));
			ControlHelper.SetTableRowStyles(TablePresets, ControlHelper.GetDefaultRowStyle());
			TablePresets.ResumeLayout();
		}

		private void Updated()
		{
			var presets = KCDatabase.Instance.FleetPreset;
			if (presets == null || presets.MaximumCount <= 0)
				return;

			TablePresets.Enabled = false;
			TablePresets.SuspendLayout();

			if (TableControls.Count < presets.MaximumCount)
			{
				for (int i = TableControls.Count; i < presets.MaximumCount; i++)
				{
					var control = new TablePresetControl(this);
					control.ConfigurationChanged(this);
					TableControls.Add(control);
					control.AddToTable(TablePresets, i);
				}

				ControlHelper.SetTableRowStyles(TablePresets, ControlHelper.GetDefaultRowStyle());
			}

			for (int i = 0; i < TableControls.Count; i++)
			{
				TableControls[i].Update(i + 1);
			}

			TablePresets.ResumeLayout();
			TablePresets.Enabled = true;
		}


		private void TablePresets_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
		{
			e.Graphics.DrawLine(e.Row % 5 == 4 && e.Column == 0 ? Pens.Gray : Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
		}


		protected override string GetPersistString()
		{
			return "FleetPreset";
		}

		private void FormFleetPreset_Click(object sender, EventArgs e)
		{
			Utility.Logger.Add(1, Font.Name);
			ConfigurationChanged();
		}
	}
}
