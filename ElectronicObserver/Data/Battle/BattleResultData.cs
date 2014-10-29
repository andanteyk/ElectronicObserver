using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {
	
	public class BattleResultData : ResponseWrapper {

		public string Rank {
			get { return RawData.api_win_rank; }
		}

		public int AdmiralExp {
			get { return (int)RawData.api_get_exp; }
		}

		public int MVP {
			get { return (int)RawData.api_mvp; }
		}

		public int BaseExp {
			get { return (int)RawData.api_get_base_exp; }
		}

		//exp

		//lostflag

		//enemyinfo/name

		//undone: 複数の battleresult に対応させる

	}

}

