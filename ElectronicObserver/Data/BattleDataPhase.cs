using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	//undone
	[Obsolete]
	public abstract class BattleDataPhase {

		protected dynamic RawData { get; private set; }


		public BattleDataPhase() {
			RawData = null;
		}

		public BattleDataPhase( dynamic data ) {
			Initialize( data );
		}

		public void Initialize( dynamic data ) {
			RawData = data;
		}

		public bool IsAvailable() {
			return RawData != null;
		}

	}

}
