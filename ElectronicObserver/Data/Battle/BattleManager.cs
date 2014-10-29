using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	public class BattleManager {

		public CompassData Compass { get; private set; }
		public BattleData BattleDay { get; private set; }
		public BattleData BattleNight { get; private set; }
		public BattleResultData Result { get; private set; }
	}

}
