using ElectronicObserver.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle.Detail {

	public static class BattleDetailDescriptor {

		public static string GetBattleDetail( BattleManager bm ) {
			var sb = new StringBuilder();

			if ( bm.StartsFromDayBattle ) {
				sb.AppendLine( "◆ 昼戦 ◆" ).Append( GetBattleDetail( bm.BattleDay ) );
				if ( bm.BattleNight != null )
					sb.AppendLine( "◆ 夜戦 ◆" ).Append( GetBattleDetail( bm.BattleNight ) );

			} else {
				sb.AppendLine( "◆ 夜戦 ◆" ).Append( GetBattleDetail( bm.BattleNight ) );
				if ( bm.BattleDay != null )
					sb.AppendLine( "◆ 昼戦 ◆" ).Append( GetBattleDetail( bm.BattleDay ) );
			}

			return sb.ToString();
		}


		public static string GetBattleDetail( BattleData battle ) {

			var sbmaster = new StringBuilder();

			foreach ( var phase in battle.GetPhases() ) {

				var sb = new StringBuilder();

				if ( phase is PhaseAirBattle ) {
					var p = phase as PhaseAirBattle;

					GetBattleDetailPhaseAirBattle( sb, p );


				} else if ( phase is PhaseBaseAirAttack ) {
					var p = phase as PhaseBaseAirAttack;

					foreach ( var a in p.AirAttackUnits ) {
						sb.AppendFormat( "〈第{0}波〉\r\n", a.AirAttackIndex + 1 );
						GetBattleDetailPhaseAirBattle( sb, a );
						sb.Append( a.GetBattleDetail() );
					}


				} else if ( phase is PhaseInitial ) {
					var p = phase as PhaseInitial;

					if ( p.FriendFleetEscort != null )
						sb.AppendLine( "〈味方主力艦隊〉" );
					else
						sb.AppendLine( "〈味方艦隊〉" );

					OutputFriendData( sb, p.FriendFleet, p.InitialHPs.Take( 6 ).ToArray(), p.MaxHPs.Take( 6 ).ToArray() );

					if ( p.FriendFleetEscort != null ) {
						sb.AppendLine();
						sb.AppendLine( "〈味方随伴艦隊〉" );

						OutputFriendData( sb, p.FriendFleetEscort, p.InitialHPs.Skip( 12 ).Take( 6 ).ToArray(), p.MaxHPs.Skip( 12 ).Take( 6 ).ToArray() );
					}

					sb.AppendLine();

					if ( p.EnemyMembersEscort != null )
						sb.Append( "〈敵主力艦隊〉" );
					else
						sb.Append( "〈敵艦隊〉" );

					if ( p.IsBossDamaged )
						sb.Append( " : 装甲破壊" );
					sb.AppendLine();

					OutputEnemyData( sb, p.EnemyMembersInstance, p.EnemyLevels, p.InitialHPs.Skip( 6 ).Take( 6 ).ToArray(), p.MaxHPs.Skip( 6 ).Take( 6 ).ToArray(), p.EnemySlotsInstance, p.EnemyParameters );


					if ( p.EnemyMembersEscort != null ) {
						sb.AppendLine();
						sb.AppendLine( "〈敵随伴艦隊〉" );

						OutputEnemyData( sb, p.EnemyMembersEscortInstance, p.EnemyLevelsEscort, p.InitialHPs.Skip( 18 ).Take( 6 ).ToArray(), p.MaxHPs.Skip( 18 ).Take( 6 ).ToArray(), p.EnemySlotsEscortInstance, p.EnemyParametersEscort );
					}

					sb.AppendLine();


				} else if ( phase is PhaseNightBattle ) {
					var p = phase as PhaseNightBattle;
					int length = sb.Length;

					{
						var eq = KCDatabase.Instance.MasterEquipments[p.TouchAircraftFriend];
						if ( eq != null ) {
							sb.Append( "自軍夜間触接: " ).AppendLine( eq.Name );
						}
						eq = KCDatabase.Instance.MasterEquipments[p.TouchAircraftEnemy];
						if ( eq != null ) {
							sb.Append( "敵軍夜間触接: " ).AppendLine( eq.Name );
						}
					}

					{
						int searchlightIndex = p.SearchlightIndexFriend;
						if ( searchlightIndex != -1 ) {
							sb.AppendFormat( "自軍探照灯照射: {0} #{1}\r\n", p.FriendFleet.MembersInstance[searchlightIndex].Name, searchlightIndex + 1 );
						}
						searchlightIndex = p.SearchlightIndexEnemy;
						if ( searchlightIndex != -1 ) {
							sb.AppendFormat( "敵軍探照灯照射: {0} #{1}\r\n", p.EnemyMembersInstance[searchlightIndex].NameWithClass, searchlightIndex + 1 );
						}
					}

					if ( p.FlareIndexFriend != -1 ) {
						sb.AppendFormat( "自軍照明弾投射: {0} #{1}\r\n", p.FriendFleet.MembersInstance[p.FlareIndexFriend].Name, p.FlareIndexFriend + 1 );
					}
					if ( p.FlareIndexEnemy != -1 ) {
						sb.AppendFormat( "敵軍照明弾投射: {0} #{1}\r\n", p.FlareEnemyInstance.NameWithClass, p.FlareIndexEnemy + 1 );
					}

					if ( sb.Length > length )		// 追加行があった場合
						sb.AppendLine();


				} else if ( phase is PhaseSearching ) {
					var p = phase as PhaseSearching;

					sb.Append( "自軍陣形: " ).Append( Constants.GetFormation( p.FormationFriend ) );
					sb.Append( " / 敵軍陣形: " ).AppendLine( Constants.GetFormation( p.FormationEnemy ) );
					sb.Append( "交戦形態: " ).AppendLine( Constants.GetEngagementForm( p.EngagementForm ) );
					sb.Append( "自軍索敵: " ).Append( Constants.GetSearchingResult( p.SearchingFriend ) );
					sb.Append( " / 敵軍索敵: " ).AppendLine( Constants.GetSearchingResult( p.SearchingEnemy ) );

					sb.AppendLine();
				}


				if ( !( phase is PhaseBaseAirAttack ) )		// 通常出力と重複するため
					sb.Append( phase.GetBattleDetail() );

				if ( sb.Length > 0 ) {
					sbmaster.AppendFormat( "《{0}》\r\n", phase.Title ).Append( sb );
				}
			}

			return sbmaster.ToString();
		}


		private static void GetBattleDetailPhaseAirBattle( StringBuilder sb, PhaseAirBattle p ) {

			if ( p.IsStage1Available ) {
				sb.Append( "Stage1: " ).AppendLine( Constants.GetAirSuperiority( p.AirSuperiority ) );
				sb.AppendFormat( "　自軍: -{0}/{1}\r\n　敵軍: -{2}/{3}\r\n",
					p.AircraftLostStage1Friend, p.AircraftTotalStage1Friend,
					p.AircraftLostStage1Enemy, p.AircraftTotalStage1Enemy );
			}
			if ( p.IsStage2Available ) {
				sb.Append( "Stage2: " );
				if ( p.IsAACutinAvailable ) {
					sb.AppendFormat( "対空カットイン( {0}, {1}({2}) )", p.AACutInShip.NameWithLevel, Constants.GetAACutinKind( p.AACutInKind ), p.AACutInKind );
				}
				sb.AppendLine();
				sb.AppendFormat( "　自軍: -{0}/{1}\r\n　敵軍: -{2}/{3}\r\n",
					p.AircraftLostStage2Friend, p.AircraftTotalStage2Friend,
					p.AircraftLostStage2Enemy, p.AircraftTotalStage2Enemy );
			}
			sb.AppendLine();
		}


		private static void OutputFriendData( StringBuilder sb, FleetData fleet, int[] initialHPs, int[] maxHPs ) {

			for ( int i = 0; i < fleet.MembersInstance.Count; i++ ) {
				var ship = fleet.MembersInstance[i];

				if ( ship == null )
					continue;

				sb.AppendFormat( "#{0}: ", i + 1 );

				sb.AppendFormat( "{0} {1} HP: {2} / {3} - 火力{4}, 雷装{5}, 対空{6}, 装甲{7}\r\n",
					ship.MasterShip.ShipTypeName, ship.NameWithLevel,
					initialHPs[i], maxHPs[i],
					ship.FirepowerBase, ship.TorpedoBase, ship.AABase, ship.ArmorBase );

				sb.Append( "　" );
				for ( int k = 0; k < ship.SlotInstance.Count; k++ ) {
					var eq = ship.SlotInstance[k];
					if ( eq != null ) {
						if ( k > 0 )
							sb.Append( ", " );
						sb.Append( eq.ToString() );
					}
				}
				sb.AppendLine();
			}
		}

		private static void OutputEnemyData( StringBuilder sb, ShipDataMaster[] members, int[] levels, int[] initialHPs, int[] maxHPs, EquipmentDataMaster[][] slots, int[][] parameters ) {

			for ( int i = 0; i < members.Length; i++ ) {
				if ( members[i] == null )
					continue;

				sb.AppendFormat( "#{0}: ", i + 1 );

				sb.AppendFormat( "{0} {1} Lv. {2} HP: {3} / {4} - 火力{5}, 雷装{6}, 対空{7}, 装甲{8}\r\n",
					members[i].ShipTypeName, members[i].NameWithClass,
					levels[i],
					initialHPs[i], maxHPs[i],
					parameters[i][0], parameters[i][1], parameters[i][2], parameters[i][3] );

				sb.Append( "　" );
				for ( int k = 0; k < slots[i].Length; k++ ) {
					var eq = slots[i][k];
					if ( eq != null ) {
						if ( k > 0 )
							sb.Append( ", " );
						sb.Append( eq.ToString() );
					}
				}
				sb.AppendLine();
			}
		}
	}
}
