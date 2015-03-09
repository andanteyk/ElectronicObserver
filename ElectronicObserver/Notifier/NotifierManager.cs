using ElectronicObserver.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Notifier {

	public sealed class NotifierManager {

		#region Singleton

		private static readonly NotifierManager instance = new NotifierManager();

		public static NotifierManager Instance {
			get { return instance; }
		}

		#endregion

		private FormMain _parentForm;


		public NotifierExpedition Expedition { get; private set; }
		public NotifierConstruction Construction { get; private set; }
		public NotifierRepair Repair { get; private set; }
		public NotifierCondition Condition { get; private set; }
		public NotifierDamage Damage { get; private set; }


		private NotifierManager() {
		}


		public void Initialize( FormMain parent ) {

			_parentForm = parent;

			Expedition = new NotifierExpedition( Utility.Configuration.Config.NotifierExpedition );
			Construction = new NotifierConstruction( Utility.Configuration.Config.NotifierConstruction );
			Repair = new NotifierRepair( Utility.Configuration.Config.NotifierRepair );
			Condition = new NotifierCondition( Utility.Configuration.Config.NotifierCondition );
			Damage = new NotifierDamage( Utility.Configuration.Config.NotifierDamage );
			
		}

		public void ApplyToConfiguration() {

			Expedition.ApplyToConfiguration( Utility.Configuration.Config.NotifierExpedition );
			Construction.ApplyToConfiguration( Utility.Configuration.Config.NotifierConstruction );
			Repair.ApplyToConfiguration( Utility.Configuration.Config.NotifierRepair );
			Condition.ApplyToConfiguration( Utility.Configuration.Config.NotifierCondition );
			Damage.ApplyToConfiguration( Utility.Configuration.Config.NotifierDamage );

		}

		public void ShowNotifier( ElectronicObserver.Window.Dialog.DialogNotifier form ) {
			_parentForm.Invoke( (MethodInvoker)( () => form.Show() ) );
		}

	}

}
