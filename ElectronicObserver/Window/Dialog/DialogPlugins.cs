using ElectronicObserver.Resource;
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
		private Dictionary<IPluginHost, UserControl> settings;

		private DialogPlugins()
		{
			InitializeComponent();
			listViewPlugins.Font = this.Font = Program.Window_Font;

			settings = new Dictionary<IPluginHost, UserControl>();
		}

		public DialogPlugins(FormMain parent ) : this()
		{
			mainForm = parent;
		}

		private void DialogPlugins_Load( object sender, EventArgs e )
		{
			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormConfiguration] );

			foreach (var p in mainForm.Plugins )
			{
				var item = new ListViewItem( new[] { p.MenuTitle, p.GetType().Assembly.Location } );
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

			UserControl control;
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
	}

	class PluginListView : ListView
	{
		public PluginListView()
		{
			SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true );
		}
	}
}
