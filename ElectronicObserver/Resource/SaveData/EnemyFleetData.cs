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
					var l = EnemyFleet.Values.ToList();
					l.Sort( ( EnemyFleetElement e1, EnemyFleetElement e2 ) => ( e1.FleetID - e2.FleetID ) );
					return l;
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


		public void Update( int fleetID, int formation, int[] fleetMember ) {
			
			if ( !Data.EnemyFleet.ContainsKey( fleetID ) ) {
			
				EnemyFleetElement e = new EnemyFleetElement();
				e.FleetID = fleetID;
				e.Formation = formation;
				e.FleetMember = fleetMember.Skip( 1 ).ToArray();
				Data.EnemyFleet.Add( fleetID, e );
			
			} else {
				//念のため書き換える
				EnemyFleetElement e = Data.EnemyFleet[fleetID];
				e.Formation = formation;
				e.FleetMember = fleetMember.Skip( 1 ).ToArray();

			}
		}


		public override string SaveFileName {
			get { return "EnemyFleetData.json"; }
		}
	}

}
