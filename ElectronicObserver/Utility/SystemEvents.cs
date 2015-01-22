using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility {
	
	/// <summary>
	/// システムイベントを扱います。
	/// </summary>
	public static class SystemEvents {

		public static event Action UpdateTimerTick = delegate { };
		public static event Action SystemShuttingDown = delegate { };


		internal static void OnUpdateTimerTick() { UpdateTimerTick(); }
		internal static void OnSystemShuttingDown() { SystemShuttingDown(); }

	}
}
