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


		public List<NotifierManager> NotifierList { get; private set; }

		public NotifierExpedition Expedition { get; private set; }



		private NotifierManager() {
		}


		public void Initialize() {

			Expedition = new NotifierExpedition();

			NotifierList = new List<NotifierManager>();

			//debug: 暫定設定
			{
				Expedition.AutoClosingInterval = 30 * 1000;
				Expedition.ShowsNotificationDialog = true;
				Expedition.PlaysNotificationSound = true;
				//Expedition.LoadSound( @"music path" );
			}
		}

	}

}
