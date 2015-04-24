using ElectronicObserver.Data;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource.Record {

	[DebuggerDisplay( "{Record.Count} Records" )]
	public class ShipDropRecord : RecordBase {

		[DebuggerDisplay( "[{ShipID}] : {FleetName}" )]
		public class ShipDropElement : RecordElementBase {

			/// <summary>
			/// ドロップした艦のID　-1=なし
			/// </summary>
			public int ShipID { get; set; }

			/// <summary>
			/// ドロップした艦の名前
			/// </summary>
			public string ShipName {
				get {
					if ( ShipID == -1 ) return "(なし)";
					if ( ShipID == -2 ) return "(満員)";

					if ( ShipID > 2000 ) {
						var eq = KCDatabase.Instance.MasterEquipments[ShipID - 2000];
						if ( eq != null )
							return eq.Name;

					} else if ( ShipID > 1000 ) {
						var item = KCDatabase.Instance.MasterUseItems[ShipID - 1000];
						if ( item != null )
							return item.Name;

					} else {
						var ship = KCDatabase.Instance.MasterShips[ShipID];
						if ( ship != null )
							return ship.NameWithClass;

					}

					return "???";
				}
			}

			/// <summary>
			/// ドロップした日時
			/// </summary>
			public DateTime Date { get; set; }

			/// <summary>
			/// 海域カテゴリID
			/// </summary>
			public int MapAreaID { get; set; }

			/// <summary>
			/// 海域カテゴリ内番号
			/// </summary>
			public int MapInfoID { get; set; }

			/// <summary>
			/// 海域セルID
			/// </summary>
			public int CellID { get; set; }

			/// <summary>
			/// ボスかどうか
			/// </summary>
			public bool IsBossNode { get; set; }

			/// <summary>
			/// 敵編成ID
			/// </summary>
			public int EnemyFleetID { get; set; }

			/// <summary>
			/// 勝利ランク
			/// </summary>
			public string Rank { get; set; }

			/// <summary>
			/// 司令部Lv.
			/// </summary>
			public int HQLevel { get; set; }


			public ShipDropElement() {
				ShipID = -1;
				Date = DateTime.Now;
			}

			public ShipDropElement( string line )
				: base( line ) { }

			public ShipDropElement( int shipID, int mapAreaID, int mapInfoID, int cellID, bool isBossNode, int enemyFleetID, string rank, int hqLevel ) {
				ShipID = shipID;
				Date = DateTime.Now;
				MapAreaID = mapAreaID;
				MapInfoID = mapInfoID;
				CellID = cellID;
				IsBossNode = isBossNode;
				EnemyFleetID = enemyFleetID;
				Rank = rank;
				HQLevel = hqLevel;
			}


			public override void LoadLine( string line ) {

				string[] elem = line.Split( ",".ToCharArray() );
				if ( elem.Length < 9 ) throw new ArgumentException( "要素数が少なすぎます。" );

				ShipID = int.Parse( elem[0] );
				//ShipName = elem[1] は読み飛ばす
				Date = DateTimeHelper.CSVStringToTime( elem[2] );
				MapAreaID = int.Parse( elem[3] );
				MapInfoID = int.Parse( elem[4] );
				CellID = int.Parse( elem[5] );
				IsBossNode = string.Compare( elem[6], "ボス" ) == 0;
				EnemyFleetID = int.Parse( elem[7] );
				Rank = elem[8];
				HQLevel = int.Parse( elem[9] );

			}

			public override string SaveLine() {

				return string.Format( "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
					ShipID,
					ShipName,
					DateTimeHelper.TimeToCSVString( Date ),
					MapAreaID,
					MapInfoID,
					CellID,
					IsBossNode ? "ボス" : "-",
					EnemyFleetID,
					Rank,
					HQLevel );

			}
		}



		public List<ShipDropElement> Record { get; private set; }


		public ShipDropRecord()
			: base() {
			Record = new List<ShipDropElement>();
		}

		public ShipDropElement this[int i] {
			get { return Record[i]; }
			set { Record[i] = value; }
		}

		public void Add( int shipID, int mapAreaID, int mapInfoID, int cellID, bool isBossNode, int enemyFleetID, string rank, int hqLevel ) {

			Record.Add( new ShipDropElement( shipID, mapAreaID, mapInfoID, cellID, isBossNode, enemyFleetID, rank, hqLevel ) );
		}


		protected override void LoadLine( string line ) {
			Record.Add( new ShipDropElement( line ) );
		}

		protected override string SaveLines() {

			StringBuilder sb = new StringBuilder();

			var list = new List<ShipDropElement>( Record );
			list.Sort( ( e1, e2 ) => e1.Date.CompareTo( e2.Date ) );

			foreach ( var elem in list ) {
				sb.AppendLine( elem.SaveLine() );
			}

			return sb.ToString();
		}


		protected override void ClearRecord() {
			Record.Clear();
		}


		protected override bool IsAppend { get { return true; } }


		public override bool Load( string path ) {
			return true;
		}

		public override bool Save( string path ) {
			bool ret = base.Save( path );

			Record.Clear();
			return ret;
		}



		protected override string RecordHeader {
			get { return "艦船ID,艦名,入手日時,海域,海域,セル,ボス,敵編成,ランク,司令部Lv"; }
		}

		public override string FileName {
			get { return "ShipDropRecord.csv"; }
		}
	}
}
