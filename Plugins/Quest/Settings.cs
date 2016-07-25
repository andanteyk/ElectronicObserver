using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Notifier;
using ElectronicObserver.Window.Plugins;

namespace Quest
{
	public partial class Settings : PluginSettingControl
	{
		public Settings()
		{
			InitializeComponent();
		}


		public override bool Save()
		{
			var config = ElectronicObserver.Utility.Configuration.Config;

			config.FormQuest.ShowRunningOnly = FormQuest_ShowRunningOnly.Checked;
			config.FormQuest.ShowOnce = FormQuest_ShowOnce.Checked;
			config.FormQuest.ShowDaily = FormQuest_ShowDaily.Checked;
			config.FormQuest.ShowWeekly = FormQuest_ShowWeekly.Checked;
			config.FormQuest.ShowMonthly = FormQuest_ShowMonthly.Checked;
			config.FormQuest.ShowOther = FormQuest_ShowOther.Checked;
			config.FormQuest.ProgressAutoSaving = FormQuest_ProgressAutoSaving.SelectedIndex;
			config.FormQuest.AllowUserToSortRows = FormQuest_AllowUserToSortRows.Checked;

			return true;
		}

		private void Settings_Load( object sender, EventArgs e )
		{
			var config = ElectronicObserver.Utility.Configuration.Config;

			FormQuest_ShowRunningOnly.Checked = config.FormQuest.ShowRunningOnly;
			FormQuest_ShowOnce.Checked = config.FormQuest.ShowOnce;
			FormQuest_ShowDaily.Checked = config.FormQuest.ShowDaily;
			FormQuest_ShowWeekly.Checked = config.FormQuest.ShowWeekly;
			FormQuest_ShowMonthly.Checked = config.FormQuest.ShowMonthly;
			FormQuest_ShowOther.Checked = config.FormQuest.ShowOther;
			FormQuest_ProgressAutoSaving.SelectedIndex = config.FormQuest.ProgressAutoSaving;
			FormQuest_AllowUserToSortRows.Checked = config.FormQuest.AllowUserToSortRows;
		}
	}
}
