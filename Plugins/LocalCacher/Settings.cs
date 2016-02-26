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
using ElectronicObserver.Utility;
using System.IO;

namespace LocalCacher
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

			// [缓存]
			if ( checkCache.Checked ) {
				if ( !config.CacheSettings.CacheEnabled || config.CacheSettings.CacheFolder != textCacheFolder.Text ) {
					ElectronicObserver.Utility.Logger.Add( 2, string.Format( "缓存设置更新。“{0}”", textCacheFolder.Text ) );
				}
			} else if ( config.CacheSettings.CacheEnabled ) {
				ElectronicObserver.Utility.Logger.Add( 2, string.Format( "缓存已关闭。" ) );
			}

			config.CacheSettings.CacheEnabled = checkCache.Checked;
			config.CacheSettings.CacheFolder = textCacheFolder.Text;

			return true;
		}

		private void Settings_Load( object sender, EventArgs e )
		{
			var config = ElectronicObserver.Utility.Configuration.Config;

			// [缓存]
			textCacheFolder.Text = config.CacheSettings.CacheFolder;
			checkCache.Checked = config.CacheSettings.CacheEnabled;

			textCacheFolder_TextChanged( null, EventArgs.Empty );
		}


		private void buttonCacheFolderBrowse_Click( object sender, EventArgs e ) {

			textCacheFolder.Text = PathHelper.ProcessFolderBrowserDialog( textCacheFolder.Text, FolderBrowser );
		}

		private void textCacheFolder_TextChanged( object sender, EventArgs e ) {

			if ( Directory.Exists( textCacheFolder.Text ) ) {
				textCacheFolder.BackColor = SystemColors.Window;
				ToolTipInfo.SetToolTip( textCacheFolder, null );
			} else {
				textCacheFolder.BackColor = Color.MistyRose;
				ToolTipInfo.SetToolTip( textCacheFolder, "指定的文件夹不存在。" );
			}
		}
	}
}
