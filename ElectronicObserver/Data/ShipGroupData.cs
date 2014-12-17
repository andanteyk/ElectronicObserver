using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 艦船グループのデータを保持します。
	/// </summary>
	public class ShipGroupData : IIdentifiable {

		/// <summary>
		/// グループID
		/// </summary>
		public int GroupID { get; private set; }
		
		/// <summary>
		/// 所属艦のIDリスト
		/// </summary>
		public List<int> Members { get; private set; }
		
		/// <summary>
		/// 所属艦リスト
		/// </summary>
		public IEnumerable<ShipData> MembersInstance {
			get {
				return KCDatabase.Instance.Ships.Values.Where( ship => Members.Contains( ship.ShipID ) );
			}
		}

		public string Name { get; private set; }



		public ShipGroupData( int groupID ) {
			GroupID = groupID;
			Members = new List<int>();
			Name = "";
		}


		/// <summary>
		/// メンバーが存在するか確認し、存在しなければ削除します。
		/// </summary>
		public void CheckMembers() {

			var ships = KCDatabase.Instance.Ships;

			for ( int i = 0; i < Members.Count; i++ ) {
				if ( !ships.ContainsKey( Members[i] ) ) {
					Members.RemoveAt( i );
					i--;
				}
			}
		}


		public int ID {
			get { return GroupID; }
		}
	}

}
