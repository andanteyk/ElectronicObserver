using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{

	/// <summary>
	/// 消費アイテムのデータを保持します。
	/// </summary>
	public class UseItem : ResponseWrapper, IIdentifiable
	{

		/// <summary>
		/// アイテムID
		/// </summary>
		public int ItemID => (int)RawData.api_id;

		/// <summary>
		/// 個数
		/// </summary>
		public int Count => (int)RawData.api_count;


		public UseItemMaster MasterUseItem => KCDatabase.Instance.MasterUseItems[ItemID];


		public int ID => ItemID;
		public override string ToString() => $"[{ItemID}] {MasterUseItem.Name} x {Count}";
	}

}
