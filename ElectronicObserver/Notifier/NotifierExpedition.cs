using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Notifier {

	/// <summary>
	/// 遠征帰投通知を扱います。
	/// </summary>
	public class NotifierExpedition : NotifierBase {

		private Dictionary<int, bool> processedFlags;


		public NotifierExpedition()
			: base() {
			Initialize();
		}

		public NotifierExpedition( Utility.Configuration.ConfigurationData.ConfigNotifierBase config )
			: base( config ) {
			Initialize();
		}


		private void Initialize() {
			DialogData.Title = "遠征帰投";
			processedFlags = new Dictionary<int, bool>();
		}


		protected override void UpdateTimerTick() {

			foreach ( var fleet in KCDatabase.Instance.Fleet.Fleets.Values ) {

				if ( !processedFlags.ContainsKey( fleet.FleetID ) )
					processedFlags.Add( fleet.FleetID, false );

				if ( fleet.ExpeditionState != 0 ) {
					if ( !processedFlags[fleet.FleetID] && (int)( fleet.ExpeditionTime - DateTime.Now ).TotalMilliseconds <= AccelInterval ) {

						processedFlags[fleet.FleetID] = true;
						Notify( fleet.FleetID, fleet.ExpeditionDestination );

					}

				} else {
					processedFlags[fleet.FleetID] = false;
				}

			}

		}

		public void Notify( int fleetID, int destination ) {

			DialogData.Message = string.Format( "#{0} 「{1}」が遠征「{2}: {3}」から帰投しました。",
				fleetID, KCDatabase.Instance.Fleet[fleetID].Name, destination, KCDatabase.Instance.Mission[destination].Name );

			base.Notify();
		}
	}
}
