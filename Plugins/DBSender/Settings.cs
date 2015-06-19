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

namespace DBSender
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

			//[データベース]
			config.Connection.SendDataToKancolleDB = Database_SendDataToKancolleDB.Checked;
			config.Connection.SendKancolleOAuth = Database_SendKancolleOAuth.Text;

			return true;
		}

		private void Settings_Load( object sender, EventArgs e )
		{
			var config = ElectronicObserver.Utility.Configuration.Config;

			//[データベース]
			Database_SendDataToKancolleDB.Checked = config.Connection.SendDataToKancolleDB;
			Database_SendKancolleOAuth.Text = config.Connection.SendKancolleOAuth;
		}

		private void Database_LinkKCDB_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
		{
			System.Diagnostics.Process.Start( "http://kancolle-db.net/" );
		}

	}
}
