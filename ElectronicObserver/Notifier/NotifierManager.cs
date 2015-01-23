using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Notifier {

	public sealed class NotifierManager {

		#region Singleton

		private static readonly NotifierManager instance = new NotifierManager();

		public static NotifierManager Instance {
			get { return instance; }
		}

		#endregion


		public NotifierExpedition Expedition { get; private set; }



		private NotifierManager() {
		}


		public void Initialize() {

			Expedition = new NotifierExpedition( Utility.Configuration.Config.NotificationExpedition );

			
			
		}

	}

}
