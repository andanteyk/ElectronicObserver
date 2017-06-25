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

		private Dictionary<int, bool> _processedFlags;


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
			_processedFlags = new Dictionary<int, bool>();

			for ( int i = 1; i <= 4; i++ )
				_processedFlags.Add( i, false );


			APIObserver o = APIObserver.Instance;

			o["api_port/port"].ResponseReceived += ClearFlags;

		}


		private void ClearFlags( string apiname, dynamic data ) {

			foreach ( int key in _processedFlags.Keys.ToArray() ) {		//列挙中の変更によるエラーを防ぐため
				_processedFlags[key] = false;
			}
		}


		protected override void UpdateTimerTick() {

			foreach ( var fleet in KCDatabase.Instance.Fleet.Fleets.Values ) {

				if ( fleet.ExpeditionState > 0 || fleet.IsInSortie ) continue;

				if ( _processedFlags[fleet.FleetID] )
					continue;
				
				
				if ( fleet.ConditionTime != null && !fleet.IsInSortie ) {

					if ( ( (DateTime)fleet.ConditionTime - DateTime.Now ).TotalMilliseconds <= AccelInterval ) {

						Notify( fleet.FleetID );
						_processedFlags[fleet.FleetID] = true;
					}
				}
			}

		}

		public void Notify( int fleetID ) {

			DialogData.Message = string.Format( "#{0} 「{1}」に所属する艦娘の疲労が回復しました。",
				fleetID, KCDatabase.Instance.Fleet[fleetID].Name );

			base.Notify();
		}
	
	}
}
