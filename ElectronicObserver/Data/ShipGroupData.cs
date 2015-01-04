using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 艦船グループのデータを保持します。
	/// </summary>
	[DataContract( Name = "ShipGroupData" )]
	[DebuggerDisplay( "[{GroupID}] : {Name} ({Members.Count} ships)" )]
	public class ShipGroupData : IIdentifiable {

		/// <summary>
		/// グループID
		/// </summary>
		[DataMember]
		public int GroupID { get; internal set; }
		
		/// <summary>
		/// 所属艦のIDリスト
		/// </summary>
		[DataMember]
		public List<int> Members { get; internal set; }
		
		/// <summary>
		/// 所属艦リスト
		/// </summary>
		[IgnoreDataMember]
		public IEnumerable<ShipData> MembersInstance {
			get {
				return Members.Select( id => KCDatabase.Instance.Ships[id] );
			}
		}

		/// <summary>
		/// グループ名
		/// </summary>
		[DataMember]
		public string Name { get; set; }


		/// <summary>
		/// 列フィルタ
		/// </summary>
		[DataMember]
		public List<bool> ColumnFilter { get; set; }

		/// <summary>
		/// 列の幅
		/// </summary>
		[DataMember]
		public List<int> ColumnWidth { get; set; }

		/// <summary>
		/// 列幅を自動調整するか
		/// </summary>
		[DataMember]
		public bool ColumnAutoSize { get; set; }


		/// <summary>
		/// 艦名をスクロールしない
		/// </summary>
		[DataMember]
		public bool LockShipNameScroll { get; set; }




		public ShipGroupData( int groupID ) {
			GroupID = groupID;
			Members = new List<int>();
			Name = "notitle #" + groupID;
			ColumnAutoSize = false;
			LockShipNameScroll = true;
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
