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
		private Plugin plugin;

		public Settings( Plugin plugin )
		{
			InitializeComponent();
			this.plugin = plugin;
		}


		public override bool Save()
		{
			var config = ElectronicObserver.Utility.Configuration.Config;

			//[データベース]
			config.Connection.SendDataToKancolleDB = Database_SendDataToKancolleDB.Checked;
			config.Connection.SendKancolleOAuth = Database_SendKancolleOAuth.Text;

			try
			{
				plugin.Settings.DBSender.send_with_proxy = Database_SendWithProxy.Checked;
			}
			catch ( Exception ex )
			{
				ElectronicObserver.Utility.ErrorReporter.SendErrorReport( ex, "保存DBSender设置出错。" );
			}
			finally
			{
				plugin.SaveSettings();
			}

			return true;
		}

		private void Settings_Load( object sender, EventArgs e )
		{
			var config = ElectronicObserver.Utility.Configuration.Config;

			//[データベース]
			Database_SendDataToKancolleDB.Checked = config.Connection.SendDataToKancolleDB;
			Database_SendKancolleOAuth.Text = config.Connection.SendKancolleOAuth;

			// 代理设置
			try
			{
				Database_SendWithProxy.Checked = plugin.Settings.DBSender.send_with_proxy;
			}
			catch ( Exception ex )
			{
				Database_SendWithProxy.Checked = true;
				ElectronicObserver.Utility.ErrorReporter.SendErrorReport( ex, "读取DBSender设置出错。" );
			}
		}

		private void Database_LinkKCDB_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
		{
			System.Diagnostics.Process.Start( "http://kancolle-db.net/" );
		}

	}
}
