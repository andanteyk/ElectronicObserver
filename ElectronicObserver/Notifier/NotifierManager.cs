using ElectronicObserver.Window;
using System;
using System.Collections.Generic;
using System.Drawing;
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
		public NotifierAnchorageRepair AnchorageRepair { get; private set; }

		private NotifierManager() {
		}


		public void Initialize( FormMain parent ) {

			_parentForm = parent;

			var c = Utility.Configuration.Config;

			Expedition = new NotifierExpedition( c.NotifierExpedition );
			Construction = new NotifierConstruction( c.NotifierConstruction );
			Repair = new NotifierRepair( c.NotifierRepair );
			Condition = new NotifierCondition( c.NotifierCondition );
			Damage = new NotifierDamage( c.NotifierDamage );
			AnchorageRepair = new NotifierAnchorageRepair( c.NotifierAnchorageRepair );

		}

		public void ApplyToConfiguration() {

			var c = Utility.Configuration.Config;

			Expedition.ApplyToConfiguration( c.NotifierExpedition );
			Construction.ApplyToConfiguration( c.NotifierConstruction );
			Repair.ApplyToConfiguration( c.NotifierRepair );
			Condition.ApplyToConfiguration( c.NotifierCondition );
			Damage.ApplyToConfiguration( c.NotifierDamage );
			AnchorageRepair.ApplyToConfiguration( c.NotifierAnchorageRepair );
		}

		public void ShowNotifier( ElectronicObserver.Window.Dialog.DialogNotifier form ) {

			if ( form.DialogData.Alignment == NotifierDialogAlignment.CustomRelative ) {		//cloneしているから書き換えても問題ないはず
				Point p = _parentForm.Browser.PointToScreen( new Point( _parentForm.Browser.ClientSize.Width / 2, _parentForm.Browser.ClientSize.Height / 2 ) );
				p.Offset( new Point( -form.Width / 2, -form.Height / 2 ) );
				p.Offset( form.DialogData.Location );

				form.DialogData.Location = p;
			}

			form.Show();
		}

		public IEnumerable<NotifierBase> GetNotifiers() {
			yield return Expedition;
			yield return Construction;
			yield return Repair;
			yield return Condition;
			yield return Damage;
			yield return AnchorageRepair;
		}

	}

}
