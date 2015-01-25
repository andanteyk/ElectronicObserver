using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Notifier {
	
	/// <summary>
	/// 疲労回復通知を扱います。
	/// </summary>
	public class NotifierCondition : NotifierBase {

		private Dictionary<int, ConditionTimer> timers;


		private class ConditionTimer {
			public DateTime? Timer = null;
			public bool IsLocked = true;
		}

		public NotifierCondition()
			: base() {
			Initialize();
		}

		public NotifierCondition( Utility.Configuration.ConfigurationData.ConfigNotifierBase config )
			: base( config ) {
			Initialize();
		}


		private void Initialize() {
			DialogData.Title = "疲労回復";
			timers = new Dictionary<int, ConditionTimer>();

			for ( int i = 1; i <= 4; i++ )
				timers.Add( i, new ConditionTimer() );


			APIObserver o = APIObserver.Instance;

			o["api_req_hensei/change"].RequestReceived += ( string apiname, dynamic data ) => Updated( int.Parse( data["api_id"] ) );
			o["api_req_kaisou/remodeling"].RequestReceived += ( string apiname, dynamic data ) => Updated( int.Parse( data["api_id"] ) );

			o["api_req_nyukyo/start"].RequestReceived += Shorten;
			o["api_req_nyukyo/speedchange"].RequestReceived += Shorten;
			o["api_req_kousyou/destroyship"].RequestReceived += Shorten;
			o["api_port/port"].ResponseReceived += Shorten;
			o["api_get_member/ndock"].ResponseReceived += Shorten;
			o["api_req_kousyou/destroyship"].ResponseReceived += Shorten;
			o["api_get_member/ship3"].ResponseReceived += Shorten;
			o["api_req_kaisou/powerup"].ResponseReceived += Shorten;

			o["api_req_map/start"].RequestReceived += LockTimer;

			o["api_port/port"].ResponseReceived += UnlockTimer;

		}

		
		private void Updated( int fleetID ) {

			int minute = GetRecoveryMinute( KCDatabase.Instance.Fleet[fleetID].MembersInstance.Min( s => s != null ? s.Condition : 100 ) );

			if ( minute > 0 )
				timers[fleetID].Timer = DateTime.Now.AddMinutes( minute );
			else
				timers[fleetID].Timer = null;

			//Utility.Logger.Add( 1, string.Format( "疲労通知: 再設定 #{0} : {1}分", fleetID, minute ) );
		}

		private void Shorten( string apiname, dynamic data ) {

			foreach ( var fleet in KCDatabase.Instance.Fleet.Fleets.Values ) {

				int minute = GetRecoveryMinute( fleet.MembersInstance.Min( s => s != null ? s.Condition : 100 ) );

				if ( minute == 0 ) {
					timers[fleet.FleetID].Timer = null;

				} else {
					DateTime target = DateTime.Now.AddMinutes( minute );
					if ( target < timers[fleet.FleetID].Timer ) {
						timers[fleet.FleetID].Timer = target;
					}
				}

				/*/
				{
					var ts = ( timers[fleet.FleetID].Timer ?? DateTime.Now ) - DateTime.Now;
					Utility.Logger.Add( 1, string.Format( "疲労通知: 短縮 #{0} : {1}分 / {2:D2}:{3:D2}", fleet.FleetID, minute, (int)ts.TotalMinutes, (int)ts.Seconds ) );
				}
				//*/
			}

		}

		private void LockTimer( string apiname, dynamic data ) {

			foreach ( var fleet in KCDatabase.Instance.Fleet.Fleets.Values ) {
				if ( fleet.IsInSortie )
					timers[fleet.FleetID].IsLocked = true;
			}
		}

		private void UnlockTimer( string apiname, dynamic data ) {

			foreach ( var fleet in KCDatabase.Instance.Fleet.Fleets.Values ) {
				if ( timers[fleet.FleetID].IsLocked ) {

					timers[fleet.FleetID].IsLocked = false;
					Updated( fleet.FleetID );
				}
			}
		}

		private int GetRecoveryMinute( int cond ) {
			return Math.Max( (int)Math.Ceiling( ( Utility.Configuration.Config.Control.ConditionBorder - cond ) / 3.0 ) * 3, 0 );
		}
		

		protected override void UpdateTimerTick() {

			foreach ( var fleet in KCDatabase.Instance.Fleet.Fleets.Values ) {

				if ( fleet.ExpeditionState > 0 || fleet.IsInSortie ) continue;


				ConditionTimer timer = timers[fleet.FleetID];

				if ( timer != null && timer.Timer != null && !timer.IsLocked ) {

					if ( timer.Timer < DateTime.Now ) {

						timer.Timer = null;
						Notify( fleet.FleetID );

					}
				}
			}

		}

		public void Notify( int fleetID ) {

			DialogData.Message = string.Format( "#{0} {1} に所属する艦娘の疲労が回復しました。",
				fleetID, KCDatabase.Instance.Fleet[fleetID].Name );

			base.Notify();
		}
	
	}
}
