using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {
	
	/// <summary>
	/// 消費アイテムのデータを保持します。
	/// </summary>
	public class UseItem : IIdentifiable {

		/// <summary>
		/// アイテムID
		/// </summary>
		public int ItemID { get; private set; }
		
		/// <summary>
		/// 個数
		/// </summary>
		public int Count { get; private set; }



		public int ID {
			get { return ItemID; }
		}
	}
}
