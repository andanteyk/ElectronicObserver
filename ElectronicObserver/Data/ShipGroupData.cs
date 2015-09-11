using ElectronicObserver.Utility.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
		[IgnoreDataMember]
		public List<int> Members { get; internal set; }


		[DataMember]
		private SerializableList<int> SerializedMembers {
			get { return new SerializableList<int>( Members ); }
			set { Members = value.List; }
		}


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
		/// 排序列
		/// </summary>
		[DataMember]
		public string SortColumnName { get; set; }

		/// <summary>
		/// 升降序
		/// </summary>
		[DataMember]
		public int SortOrder { get; set; }

		/// <summary>
		/// グループ名
		/// </summary>
		[DataMember]
		public string Name { get; set; }


		/// <summary>
		/// 列フィルタ
		/// </summary>
		[IgnoreDataMember]
		public List<bool> ColumnFilter { get; set; }

		[DataMember]
		private SerializableList<bool> SerializedColumnFilter {
			get { return new SerializableList<bool>( ColumnFilter ); }
			set { ColumnFilter = value.List; }
		}

		/// <summary>
		/// 列の幅
		/// </summary>
		[IgnoreDataMember]
		public List<int> ColumnWidth { get; set; }

		[DataMember]
		private SerializableList<int> SerializedColumnWidth {
			get { return new SerializableList<int>( ColumnWidth ); }
			set { ColumnWidth = value.List; }
		}


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
