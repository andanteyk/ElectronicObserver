using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {
	
	/// <summary>
	/// 艦種
	/// </summary>
	[DebuggerDisplay( "[{ID}] : {Name}" )]
	public class ShipType : ResponseWrapper, IIdentifiable {

		/// <summary>
		/// 艦種
		/// </summary>
		public int TypeID {
			get { return (int)RawData.api_id; }
		}

		/// <summary>
		/// 並べ替え順
		/// </summary>
		public int SortID {
			get { return (int)RawData.api_sortno; }
		}
		
		/// <summary>
		/// 艦種名
		/// </summary>
		public string Name {
			get { return RawData.api_name; }
		}
		
		/// <summary>
		/// 入渠時間係数
		/// </summary>
		public int RepairTime {
			get { return (int)RawData.api_scnt; }
		}

		
		//TODO: api_kcnt


		/// <summary>
		/// 装備可否フラグ
		/// </summary>
		private Dictionary<int, bool> _equipmentType;
		public ReadOnlyDictionary<int, bool> EquipmentType {
			get { return new ReadOnlyDictionary<int, bool>( _equipmentType ); }
		}


		public int ID {
			get { return TypeID; }
		}




		public ShipType()
			: base() {

			_equipmentType = new Dictionary<int, bool>();
		}

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );			//FIXME: そのままだとエラーを吐くため。もっとスマートな代替案があれば

			if ( IsAvailable ) {
				int i = 1;
				_equipmentType = new Dictionary<int, bool>();
				foreach ( KeyValuePair<string, object> type in RawData.api_equip_type ) {
					_equipmentType.Add( i, (double)type.Value != 0 );
					i++;
				}
			}
		}

	}

}
