using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 艦船グループのデータを保持します。
	/// </summary>
	[DebuggerDisplay( "[{GroupID}] : {Name} ({Members.Count} ships)" )]
	public class ShipGroupData : IIdentifiable {

		/// <summary>
		/// グループID
		/// </summary>
		public int GroupID { get; private set; }
		
		/// <summary>
		/// 所属艦のIDリスト
		/// </summary>
		public List<int> Members { get; internal set; }
		
		/// <summary>
		/// 所属艦リスト
		/// </summary>
		public IEnumerable<ShipData> MembersInstance {
			get {
				return Members.Select( id => KCDatabase.Instance.Ships[id] );
			}
		}

		/// <summary>
		/// グループ名
		/// </summary>
		public string Name { get; set; }



		public ShipGroupData( int groupID ) {
			GroupID = groupID;
			Members = new List<int>();
			Name = "notitle #" + groupID;
		}


		/// <summary>
		/// メンバー配列をチェックし、除籍艦や重複艦を削除します。
		/// </summary>
		public void CheckMembers() {

			var ships = KCDatabase.Instance.Ships;

			if ( ships.Count > 0 )		//未初期化時にデータが破壊されるのを防ぐ
				Members = Members.Distinct().Intersect( ships.Keys ).ToList();
		}


		public int ID {
			get { return GroupID; }
		}


		public override string ToString() {
			return Name;
		}

	}

}
