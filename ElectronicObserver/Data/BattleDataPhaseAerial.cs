using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	[Obsolete]
	public class BattleDataPhaseAerial : BattleDataPhase {

		//注：使用の際は api_stage_flagを含ませてください。

		public BattleDataPhaseAerial( dynamic data ) {
			Initialize( data );
		}



	}

}
