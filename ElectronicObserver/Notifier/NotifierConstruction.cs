using ElectronicObserver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Notifier {
	
	/// <summary>
	/// 建造完了通知を扱います。
	/// </summary>
	public class NotifierConstruction : NotifierBase {

		private Dictionary<int, bool> processedFlags;


		public NotifierConstruction()
			: base() {
			Initialize();
		}

		public NotifierConstruction( Utility.Configuration.ConfigurationData.ConfigNotifierBase config )
			: base( config ) {
			Initialize();
		}


		private void Initialize() {
			DialogData.Title = "建造完了";
			processedFlags = new Dictionary<int, bool>();
		}


		protected override void UpdateTimerTick() {

			foreach ( var arsenal in KCDatabase.Instance.Arsenals.Values ) {

				if ( !processedFlags.ContainsKey( arsenal.ArsenalID ) )
					processedFlags.Add( arsenal.ArsenalID, false );

				if ( arsenal.State > 0 ) {
					if ( !processedFlags[arsenal.ArsenalID] && (int)( arsenal.CompletionTime - DateTime.Now ).TotalMilliseconds <= AccelInterval ) {

						processedFlags[arsenal.ArsenalID] = true;
						Notify( arsenal.ArsenalID, arsenal.ShipID );
					}

				} else {
					processedFlags[arsenal.ArsenalID] = false;
				}
				
			}

		}

		public void Notify( int arsenalID, int shipID ) {

			DialogData.Message = string.Format( "工廠ドック #{0} で {1} の建造が完了しました。",
				arsenalID, Utility.Configuration.Config.FormArsenal.ShowShipName ? KCDatabase.Instance.MasterShips[shipID].NameWithClass : "艦娘" );

			base.Notify();
		}
	}
}
