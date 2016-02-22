using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
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

			/// <summary>
			/// 艦隊ID
			/// </summary>
			public uint FleetID { get { return ComputeHash(); } }

			/// <summary>
			/// 艦隊名
			/// </summary>
			public string FleetName { get; set; }

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
			/// 海域難易度(甲乙丙)
			/// </summary>
			public int Difficulty { get; set; }

			/// <summary>
			/// 陣形
			/// </summary>
			public int Formation { get; set; }

			/// <summary>
			/// 敵艦船リスト
			/// </summary>
			public int[] FleetMember { get; set; }
			
			/// <summary>
			/// 敵艦船名リスト
			/// </summary>
			public string[] FleetMemberName {
				get {
					return FleetMember.Select( id => KCDatabase.Instance.MasterShips[id] != null ? KCDatabase.Instance.MasterShips[id].NameWithClass : "-" ).ToArray();
				}
			}

			/// <summary>
			/// 艦娘の獲得経験値
			/// </summary>
			public int ExpShip { get; set; }



			public EnemyFleetElement()
				: base() { }

			public EnemyFleetElement( string line )
				: base( line ) { }

			public EnemyFleetElement( string fleetName, int mapAreaID, int mapInfoID, int cellID, int difficulty, int formation, int[] fleetMember, int expShip )
				: base() {
				FleetName = fleetName;
				MapAreaID = mapAreaID;
				MapInfoID = mapInfoID;
				CellID = cellID;
				Difficulty = difficulty;
				Formation = formation;
				FleetMember = fleetMember;
				ExpShip = expShip;
			}


			public override void LoadLine( string line ) {

				string[] elem = line.Split( ",".ToCharArray() );
				if ( elem.Length < 20 ) throw new ArgumentException( "要素数が少なすぎます。" );

				FleetName = elem[1];
				MapAreaID = int.Parse( elem[2] );
				MapInfoID = int.Parse( elem[3] );
				CellID = int.Parse( elem[4] );
				Difficulty = Constants.GetDifficulty( elem[5] );
				Formation = Constants.GetFormation( elem[6] );

				FleetMember = new int[6];
				for ( int i = 7; i < 7 + 6; i++ ) {
					FleetMember[i - 7] = int.Parse( elem[i] );
				}

				ExpShip = int.Parse( elem[19] );

				//if ( FleetID != uint.Parse( elem[0] ) ) //???

			}

			public override string SaveLine() {

				return string.Format( "{0},{1},{2},{3},{4}", FleetID, FleetName, SaveLinePart(), string.Join( ",", FleetMemberName ), ExpShip );
			}


			/// <summary>
			/// ハッシュ処理に用いる行を求めます。
			/// 処理の都合上、戦闘開始直後に判明するもののみ利用します。
			/// </summary>
			private string SaveLinePart() {

				return string.Format( "{0},{1},{2},{3},{4},{5}", MapAreaID, MapInfoID, CellID, Constants.GetDifficulty( Difficulty ), Constants.GetFormation( Formation ),
					string.Join( ",", FleetMember ) );

			}


			/// <summary>
			/// 現在のインスタンスのIDとなるハッシュ値を求めます。
			/// </summary>
			/// <returns></returns>
			private uint ComputeHash() {

				var md5 = System.Security.Cryptography.MD5.Create();
				byte[] hash = md5.ComputeHash( Encoding.UTF8.GetBytes( SaveLinePart() ) );
				md5.Clear();
				return (uint)hash[0] << 24 | (uint)hash[1] << 16 | (uint)hash[2] << 8 | (uint)hash[3];

			}


			/// <summary>
			/// 現在の状態からインスタンスを生成します。
			/// </summary>
			public static EnemyFleetElement CreateFromCurrentState() {

				var battle = KCDatabase.Instance.Battle;
				string fleetName = battle.Result != null ? battle.Result.EnemyFleetName : "";
				int baseExp = battle.Result != null ? battle.Result.BaseExp : 0;

				switch ( battle.BattleMode & BattleManager.BattleModes.BattlePhaseMask ) {
					case BattleManager.BattleModes.Normal:
					case BattleManager.BattleModes.AirBattle:
					case BattleManager.BattleModes.AirRaid:
					default:
						return new EnemyFleetElement(
							fleetName,
							battle.Compass.MapAreaID,
							battle.Compass.MapInfoID,
							battle.Compass.Destination,
							battle.Compass.MapInfo.EventDifficulty,
							battle.BattleDay.Searching.FormationEnemy,
							battle.BattleDay.Initial.EnemyMembers,
							baseExp );

					case BattleManager.BattleModes.NightOnly:
					case BattleManager.BattleModes.NightDay:
						return new EnemyFleetElement(
							fleetName,
							battle.Compass.MapAreaID,
							battle.Compass.MapInfoID,
							battle.Compass.Destination,
							battle.Compass.MapInfo.EventDifficulty,
							battle.BattleNight.Searching.FormationEnemy,
							battle.BattleNight.Initial.EnemyMembers,
							baseExp );

					case BattleManager.BattleModes.Practice:
						return null;

				}
			}


		}



		public Dictionary<uint, EnemyFleetElement> Record { get; private set; }



		public EnemyFleetRecord()
			: base() {
			Record = new Dictionary<uint, EnemyFleetElement>();
		}



		public EnemyFleetElement this[uint i] {
			get {
				return Record.ContainsKey( i ) ? Record[i] : null;
			}
			set {
				if ( i < 0 )
					return;
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
			list.Sort(
				( e1, e2 ) => {

					int areadiff = e1.MapAreaID - e2.MapAreaID;
					if ( areadiff != 0 )
						return areadiff;

					int infodiff = e1.MapInfoID - e2.MapInfoID;
					if ( infodiff != 0 )
						return infodiff;

					int celldiff = e1.CellID - e2.CellID;
					if ( celldiff != 0 )
						return celldiff;

					int diffdiff = e1.Difficulty - e2.Difficulty;
					if ( diffdiff != 0 )
						return diffdiff;

					for ( int i = 0; i < 6; i++ ) {
						int shipdiff = e1.FleetMember[i] - e2.FleetMember[i];
						if ( shipdiff != 0 )
							return shipdiff;
					}

					int formdiff = e1.Formation - e2.Formation;
					if ( formdiff != -1 )
						return formdiff;
					
					return e1.FleetID.CompareTo( e2.FleetID );

				} );

			foreach ( var elem in list ) {
				sb.AppendLine( elem.SaveLine() );
			}

			return sb.ToString();
		}

		protected override void ClearRecord() {
			Record.Clear();
		}

		protected override string RecordHeader {
			get { return "敵編成ID,敵艦隊名,海域,海域,セル,難易度,陣形,敵1番艦,敵2番艦,敵3番艦,敵4番艦,敵5番艦,敵6番艦,敵1番艦名,敵2番艦名,敵3番艦名,敵4番艦名,敵5番艦名,敵6番艦名,経験値"; }
		}

		public override string FileName {
			get { return "EnemyFleetRecord.csv"; }
		}
	}
}
