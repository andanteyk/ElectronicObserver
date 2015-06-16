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

		private DialogPlugins()
		{
			InitializeComponent();
			listViewPlugins.Font = this.Font = Program.Window_Font;
		}

		public DialogPlugins(FormMain parent ) : this()
		{
			mainForm = parent;
		}

		private void DialogPlugins_Load( object sender, EventArgs e )
		{
			foreach (var p in mainForm.Plugins )
			{
				var item = new ListViewItem( new[] { p.MenuTitle, p.GetType().Assembly.CodeBase } );
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
	}
}
