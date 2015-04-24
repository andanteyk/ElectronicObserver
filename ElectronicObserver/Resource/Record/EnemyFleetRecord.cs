using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource.Record {

	/// <summary>
	/// 敵艦隊編成のレコードです。
	/// </summary>
	[DebuggerDisplay( "{Record.Count} Records" )]
	public class EnemyFleetRecord : RecordBase {

		[DebuggerDisplay( "[{ID}] : {FleetName}" )]
		public class EnemyFleetElement : RecordElementBase {

			public int FleetID { get; set; }

			public string FleetName { get; set; }

			public int Formation { get; set; }

			public int[] FleetMember { get; set; }

			public EnemyFleetElement()
				: base() { }

			public EnemyFleetElement( string line )
				: base( line ) { }

			public EnemyFleetElement( int fleetID, string fleetName, int formation, int[] fleetMember )
				: base() {
				FleetID = fleetID;
				FleetName = fleetName;
				Formation = formation;
				FleetMember = fleetMember;
			}


			public override void LoadLine( string line ) {

				string[] elem = line.Split( ",".ToCharArray() );
				if ( elem.Length < 4 ) throw new ArgumentException( "要素数が少なすぎます。" );

				FleetID = int.Parse( elem[0] );
				FleetName = elem[1];
				Formation = int.Parse( elem[2] );

				FleetMember = new int[elem.Length - 3];
				for ( int i = 3; i < elem.Length; i++ ) {
					FleetMember[i - 3] = int.Parse( elem[i] );
				}
			}

			public override string SaveLine() {

				StringBuilder sb = new StringBuilder();

				sb.AppendFormat( "{0},{1},{2}", FleetID, FleetName, Formation );

				foreach ( int i in FleetMember ) {
					sb.AppendFormat( ",{0}", i );
				}

				return sb.ToString();
			}
		}



		public Dictionary<int, EnemyFleetElement> Record { get; private set; }



		public EnemyFleetRecord()
			: base() {
			Record = new Dictionary<int, EnemyFleetElement>();
		}



		public EnemyFleetElement this[int i] {
			get {
				return Record.ContainsKey( i ) ? Record[i] : null;
			}
			set {
				if ( !Record.ContainsKey( i ) ) {
					Record.Add( i, value );
				} else {
					Record[i] = value;
				}
			}
		}


		public void Update( EnemyFleetElement elem ) {
			this[elem.FleetID] = elem;
		}


		protected override void LoadLine( string line ) {
			Update( new EnemyFleetElement( line ) );
		}

		protected override string SaveLines() {

			StringBuilder sb = new StringBuilder();

			var list = Record.Values.ToList();
			list.Sort( ( e1, e2 ) => e1.FleetID - e2.FleetID );

			foreach ( var elem in list ) {
				sb.AppendLine( elem.SaveLine() );
			}

			return sb.ToString();
		}

		protected override void ClearRecord() {
			Record.Clear();
		}

		protected override string RecordHeader {
			get { return "敵編成ID,敵艦隊名,陣形,敵1番艦,敵2番艦,敵3番艦,敵4番艦,敵5番艦,敵6番艦"; }
		}

		public override string FileName {
			get { return "EnemyFleetRecord.csv"; }
		}
	}
}
