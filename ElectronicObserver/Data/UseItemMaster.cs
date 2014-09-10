using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 消費アイテムのマスターデータを保持します。
	/// </summary>
	public class UseItemMaster : IIdentifiable, IResponseLoader {

		/// <summary>
		/// アイテムID
		/// </summary>
		public int ItemID { get; private set; }
		
		/// <summary>
		/// 使用形態
		/// 1=高速修復材, 2=高速建造材, 3=開発資材, 4=資源還元, その他
		/// </summary>
		public int UseType { get; private set; }
		
		/// <summary>
		/// カテゴリ
		/// </summary>
		public int Category { get; private set; }
		
		/// <summary>
		/// アイテム名
		/// </summary>
		public string Name { get; private set; }
		
		/// <summary>
		/// 説明
		/// </summary>
		public string Description { get; private set; }

		//description[1]=家具コインの内容量　省略します


		public UseItemMaster()
			: this( 0 ) { }

		public UseItemMaster( int id ) {
			ItemID = id;
		}


		public int ID {
			get { return ItemID; }
		}


		bool LoadFromResponse( string apiname, dynamic data ) {

			return true;

		}
	}

}
