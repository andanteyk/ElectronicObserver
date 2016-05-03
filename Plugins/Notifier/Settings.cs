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

namespace Notifier
{
	public partial class Settings : PluginSettingControl
	{
		public Settings()
		{
			InitializeComponent();
		}

		private void Notification_Expedition_Click( object sender, EventArgs e )
		{

			using ( var dialog = new DialogConfigurationNotifier( NotifierManager.Instance.Expedition ) )
			{
				dialog.ShowDialog( this );
			}
		}

		private void Notification_Construction_Click( object sender, EventArgs e )
		{

			using ( var dialog = new DialogConfigurationNotifier( NotifierManager.Instance.Construction ) )
			{
				dialog.ShowDialog( this );
			}
		}

		private void Notification_Repair_Click( object sender, EventArgs e )
		{

			using ( var dialog = new DialogConfigurationNotifier( NotifierManager.Instance.Repair ) )
			{
				dialog.ShowDialog( this );
			}
		}

		private void Notification_Condition_Click( object sender, EventArgs e )
		{

			using ( var dialog = new DialogConfigurationNotifier( NotifierManager.Instance.Condition ) )
			{
				dialog.ShowDialog( this );
			}
		}

		private void Notification_Damage_Click( object sender, EventArgs e )
		{

			using ( var dialog = new DialogConfigurationNotifier( NotifierManager.Instance.Damage ) )
			{
				dialog.ShowDialog( this );
			}
		}

        private void Notification_AnchorageRepair_Click( object sender, EventArgs e )
        {

            using ( var dialog = new DialogConfigurationNotifier( NotifierManager.Instance.AnchorageRepair ) )
            {
                dialog.ShowDialog( this );
            }
        }

        public override bool Save()
		{
			NotifierManager.Instance.ApplyToConfiguration();

			return true;
		}
	}
}
