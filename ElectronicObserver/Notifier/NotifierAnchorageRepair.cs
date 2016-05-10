using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Notifier {

	public class NotifierAnchorageRepair : NotifierBase {

		/// <summary>
		/// 通知レベル
		/// 0 = いつでも、1 = 明石旗艦の時、2 = 修理艦もいる時
		/// </summary>
		public int NotificationLevel { get; set; }



		// いったん母港に行くまでは通知しない
		private bool processedFlag = true;

		public NotifierAnchorageRepair()
			: base() {
			Initialize();
		}

		public NotifierAnchorageRepair( Utility.Configuration.ConfigurationData.ConfigNotifierAnchorageRepair config )
			: base( config ) {
			Initialize();

			NotificationLevel = config.NotificationLevel;
		}

		private void Initialize() {
			DialogData.Title = "泊地修理発動";


			APIObserver o = APIObserver.Instance;

			o["api_port/port"].ResponseReceived += ClearFlag;
		}

		void ClearFlag( string apiname, dynamic data ) {

			processedFlag = false;
		}


		protected override void UpdateTimerTick() {

			var fleets = KCDatabase.Instance.Fleet;

			if ( !processedFlag ) {

				if ( ( DateTime.Now - fleets.AnchorageRepairingTimer ).TotalMilliseconds + AccelInterval >= 20 * 60 * 1000 ) {

					bool clear;
					switch ( NotificationLevel ) {

						case 0:		//いつでも
						default:
							clear = true;
							break;

						case 1:		//明石旗艦の時
							clear = fleets.Fleets.Values.Any( f => f.CanAnchorageRepairing );
							break;

						case 2:		//修理艦もいる時
							clear = fleets.Fleets.Values.Any( f =>
								f.CanAnchorageRepairing &&
								f.MembersInstance.Take( 2 + KCDatabase.Instance.Ships[f[0]].SlotInstanceMaster.Count( eq => eq != null && eq.CategoryType == 31 ) )
								.Any( s => s != null && s.HPRate < 1.0 && s.HPRate > 0.5 && s.RepairingDockID == -1 ) );
							break;
					}

					if ( clear ) {

						Notify();
						processedFlag = true;
					}
				}
			}

		}


		public override void Notify() {

			DialogData.Message = "泊地修理の開始から20分が経過しました。";

			base.Notify();
		}


		public override void ApplyToConfiguration( Utility.Configuration.ConfigurationData.ConfigNotifierBase config ) {
			base.ApplyToConfiguration( config );

			var c = config as Utility.Configuration.ConfigurationData.ConfigNotifierAnchorageRepair;

			if ( c != null ) {
				c.NotificationLevel = NotificationLevel;
			}
		}

	}
}
