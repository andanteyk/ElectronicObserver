using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource.SaveData {

	public class EnemyFleetData : SaveData {

		public class EnemyFleetElement {

			public int FleetID { get; set; }

			public int Formation { get; set; }

			public int[] FleetMember { get; set; }


			public EnemyFleetElement()
				: this( 6 ) { }

			public EnemyFleetElement( int memberSize ) {
				FleetMember = Enumerable.Repeat<int>( -1, memberSize ).ToArray();
			}

		}

		public class InternalData : InternalBaseData {

			[IgnoreDataMember]
			public Dictionary<int, EnemyFleetElement> EnemyFleet;

			//シリアライズ用
			public List<EnemyFleetElement> EnemyFleetList {
				get {
					return EnemyFleet.Values.ToList();
				}
				set {
					EnemyFleet = value.ToDictionary( n => n.FleetID );
				}
			}

			public InternalData() {
				EnemyFleet = new Dictionary<int, EnemyFleetElement>();
			}
		}


		public EnemyFleetData()
			: base() {

			DataInstance = new InternalData();
		}


		public InternalData Data {
			get { return (InternalData)DataInstance; }
			set { DataInstance = value; }
		}


		public override string SaveFileName {
			get { return "EnemyFleetData.json"; }
		}
	}

}
