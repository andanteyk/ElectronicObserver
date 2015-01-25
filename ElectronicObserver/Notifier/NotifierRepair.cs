using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Notifier {
	
	/// <summary>
	/// 入渠完了通知を扱います。
	/// </summary>
	public class NotifierRepair : NotifierBase {

		private Dictionary<int, bool> processedFlags;


		public NotifierRepair()
			: base() {
			Initialize();
		}

		public NotifierRepair( Utility.Configuration.ConfigurationData.ConfigNotifierBase config )
			: base( config ) {
			Initialize();
		}


		private void Initialize() {
			DialogData.Title = "入渠完了";
			processedFlags = new Dictionary<int, bool>();
		}


		protected override void UpdateTimerTick() {

			foreach ( var dock in KCDatabase.Instance.Docks.Values ) {

				if ( !processedFlags.ContainsKey( dock.DockID ) )
					processedFlags.Add( dock.DockID, false );

				if ( dock.State > 0 ) {
					if ( !processedFlags[dock.DockID] && (int)( dock.CompletionTime - DateTime.Now ).TotalMilliseconds <= AccelInterval ) {

						processedFlags[dock.DockID] = true;
						Notify( dock.DockID, dock.ShipID );

					}

				} else {
					processedFlags[dock.DockID] = false;
				}

			}

		}

		public void Notify( int dockID, int shipID ) {

			DialogData.Message = string.Format( "入渠ドック #{0} で {1} の修復が完了しました。",
				dockID, KCDatabase.Instance.Ships[shipID].Name );

			base.Notify();
		}

	}
}
