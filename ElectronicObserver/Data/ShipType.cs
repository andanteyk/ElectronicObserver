using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {
	
	/// <summary>
	/// 艦種
	/// </summary>
	public class ShipType : IIdentifiable, IResponseLoader {

		/// <summary>
		/// 艦種
		/// </summary>
		public int TypeID { get; private set; }

		/// <summary>
		/// 並べ替え順
		/// </summary>
		public int SortID { get; private set; }
		
		/// <summary>
		/// 艦種名
		/// </summary>
		public string Name { get; private set; }
		
		/// <summary>
		/// 入渠時間係数
		/// </summary>
		public int RepairTime { get; private set; }

		//TODO: api_kcnt

		/// <summary>
		/// 装備可否フラグ
		/// </summary>
		public Dictionary<int, bool> EquipmentType;


		public ShipType()
			: this( 0 ) {
		}

		public ShipType( int id ) {
			TypeID = id;
		}


		public int ID {
			get { return TypeID; }
		}

		public bool LoadFromResponse( string apiname, dynamic data ) {

			TypeID = data.api_id;
			SortID = data.api_sortno;
			Name = data.api_name;
			RepairTime = data.api_scnt;
			//kcnt
			EquipmentType = new Dictionary<int, bool>();
			foreach ( KeyValuePair<string, dynamic> t in data.api_equip_type ) {
				EquipmentType.Add( int.Parse( t.Key ), t.Value != 0 );
			}

			return true;
		}

	}


}
