using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 装備の種別
	/// </summary>
	public class EquipmentType : IIdentifiable, IResponseLoader {

		/// <summary>
		/// 装備の種別
		/// </summary>
		public int TypeID { get; private set; }

		/// <summary>
		/// 名前
		/// </summary>
		public string Name { get; private set; }

		//show_flg


		public EquipmentType()
			: this( 0 ) { 
		}

		public EquipmentType( int id ) {
			TypeID = id;
		}



		public int ID {
			get { return TypeID; }
		}


		public bool LoadFromResponse( string apiname, dynamic data ) {

			TypeID = data.api_id;
			Name = data.api_name;

			return true;
		}
	}


}
