using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 消費アイテムのマスターデータを保持します。
	/// </summary>
	[DebuggerDisplay( "[{ID}] : {Name}" )]
	public class UseItemMaster : ResponseWrapper, IIdentifiable {

		/// <summary>
		/// アイテムID
		/// </summary>
		public int ItemID {
			get { return (int)RawData.api_id; }
		}
		
		/// <summary>
		/// 使用形態
		/// 1=高速修復材, 2=高速建造材, 3=開発資材, 4=資源還元, その他
		/// </summary>
		public int UseType {
			get { return (int)RawData.api_usetype; }
		}
		
		/// <summary>
		/// カテゴリ
		/// </summary>
		public int Category {
			get { return (int)RawData.api_category; }
		}
		
		/// <summary>
		/// アイテム名
		/// </summary>
		public string Name {
			get { return RawData.api_name; }
		}
		
		/// <summary>
		/// 説明
		/// </summary>
		public string Description {
			get { return RawData.api_description[0]; }
		}

		//description[1]=家具コインの内容量　省略します


		public int ID {
			get { return ItemID; }
		}

	}

}
