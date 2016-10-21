using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {
	
	/// <summary>
	/// 消費アイテムのデータを保持します。
	/// </summary>
	[DebuggerDisplay( "{KCDatabase.Instance.MasterUseItems[ItemID].Name} x {Count}" )]
	public class UseItem : ResponseWrapper, IIdentifiable {

		/// <summary>
		/// アイテムID
		/// </summary>
		public int ItemID {
			get { return (int)RawData.api_id; }
		}
		
		/// <summary>
		/// 個数
		/// </summary>
		public int Count {
			get { return (int)RawData.api_count; }
		}



		public int ID {
			get { return ItemID; }
		}
	}
}
