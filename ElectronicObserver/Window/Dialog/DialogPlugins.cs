using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Plugins;
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
	public partial class DialogPlugins : Form
	{
		private FormMain mainForm;
		private Dictionary<IPluginHost, PluginSettingControl> settings;

		private DialogPlugins()
		{
			InitializeComponent();
			listViewPlugins.Font = this.Font = Program.Window_Font;

			settings = new Dictionary<IPluginHost, PluginSettingControl>();
		}

		public DialogPlugins( FormMain parent )
			: this()
		{
			mainForm = parent;
		}

		private void DialogPlugins_Load( object sender, EventArgs e )
		{
			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormConfiguration] );

			foreach ( var p in mainForm.Plugins )
			{
				var item = new ListViewItem( new[]
				{
					p.MenuTitle,
					PathHelper.GetRelativePath( p.GetType().Assembly.Location )
				} );

				item.Tag = p;
				switch ( p.PluginType )
				{
					case Plugins.PluginType.DockContent:
						item.Group = listViewPlugins.Groups["groupDockContent"];
						break;

					case Plugins.PluginType.Service:
						item.Group = listViewPlugins.Groups["groupService"];
						break;

					case Plugins.PluginType.Dialog:
						item.Group = listViewPlugins.Groups["groupDialog"];
						break;

					case Plugins.PluginType.Observer:
						item.Group = listViewPlugins.Groups["groupObserver"];
						break;
				}
				listViewPlugins.Items.Add( item );
			}

		}

		private void listViewPlugins_SelectedIndexChanged( object sender, EventArgs e )
		{
			if ( listViewPlugins.SelectedItems.Count < 1 )
				return;

			var item = listViewPlugins.SelectedItems[0];
			var plugin = (IPluginHost)item.Tag;
			if ( plugin == null )
				return;

			PluginSettingControl control;
			if ( !settings.TryGetValue( plugin, out control ) )
			{
				settings[plugin] = ( control = plugin.GetSettings() );
			}

			panelSettings.Controls.Clear();
			if ( control != null )
			{
				control.Dock = DockStyle.Fill;
				panelSettings.Controls.Add( control );
			}
		}

		private void buttonOK_Click( object sender, EventArgs e )
		{
			foreach ( var kv in settings )
			{
				var page = kv.Value;
				if ( page != null && page.Save() )
				{
					Utility.Logger.Add( 1, string.Format( "插件 {0}({1}) 设置已保存。", kv.Key.MenuTitle, kv.Key.Version ) );
				}
			}

			Utility.Configuration.Instance.OnConfigurationChanged();
			this.Close();
		}

		private void buttonCancel_Click( object sender, EventArgs e )
		{
			this.Close();
		}
	}

	class PluginListView : ListView
	{
		public PluginListView()
		{
			SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true );
		}
	}
}
