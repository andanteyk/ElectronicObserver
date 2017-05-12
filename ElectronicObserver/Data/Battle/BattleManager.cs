using ElectronicObserver.Data.Battle.Detail;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 戦闘関連の処理を統括して扱います。
	/// </summary>
	public class BattleManager : ResponseWrapper {

		public static readonly string BattleLogPath = "BattleLog";


		/// <summary>
		/// 羅針盤データ
		/// </summary>
		public CompassData Compass { get; private set; }

		/// <summary>
		/// 昼戦データ
		/// </summary>
		public BattleDay BattleDay { get; private set; }

		/// <summary>
		/// 夜戦データ
		/// </summary>
		public BattleNight BattleNight { get; private set; }

		/// <summary>
		/// 戦闘結果データ
		/// </summary>
		public BattleResultData Result { get; private set; }

		[Flags]
		public enum BattleModes {
			Undefined,						// 未定義
			Normal,							// 昼夜戦(通常戦闘)
			NightOnly,						// 夜戦
			NightDay,						// 夜昼戦
			AirBattle,						// 航空戦
			AirRaid,						// 長距離空襲戦
			Practice,						// 演習
			BaseAirRaid,					// 基地空襲戦
			BattlePhaseMask = 0xFF,			// 戦闘形態マスク
			CombinedTaskForce = 0x100,		// 機動部隊
			CombinedSurface = 0x200,		// 水上部隊
			CombinedMask = 0xFF00,			// 連合艦隊仕様
			EnemyCombinedFleet = 0x10000,	// 敵連合艦隊
		}

		/// <summary>
		/// 戦闘種別
		/// </summary>
		public BattleModes BattleMode { get; private set; }


		/// <summary>
		/// 昼戦から開始する戦闘かどうか
		/// </summary>
		public bool StartsFromDayBattle { get { return !StartsFromNightBattle; } }

		/// <summary>
		/// 夜戦から開始する戦闘かどうか
		/// </summary>
		public bool StartsFromNightBattle {
			get {
				var phase = BattleMode & BattleModes.BattlePhaseMask;
				return phase == BattleModes.NightOnly || phase == BattleModes.NightDay;
			}
		}

		/// <summary>
		/// 連合艦隊戦かどうか
		/// </summary>
		public bool IsCombinedBattle { get { return ( BattleMode & BattleModes.CombinedMask ) != 0; } }

		/// <summary>
		/// 演習かどうか
		/// </summary>
		public bool IsPractice { get { return ( BattleMode & BattleModes.BattlePhaseMask ) == BattleModes.Practice; } }

		/// <summary>
		/// 敵が連合艦隊かどうか
		/// </summary>
		public bool IsEnemyCombined { get { return ( BattleMode & BattleModes.EnemyCombinedFleet ) != 0; } }

		/// <summary>
		/// 基地空襲戦かどうか
		/// </summary>
		public bool IsBaseAirRaid { get { return ( BattleMode & BattleModes.BattlePhaseMask ) == BattleModes.BaseAirRaid; } }


		/// <summary>
		/// 1回目の戦闘
		/// </summary>
		public BattleData FirstBattle {
			get {
				if ( StartsFromDayBattle )
					return BattleDay;
				else
					return BattleNight;
			}
		}

		/// <summary>
		/// 2回目の戦闘
		/// </summary>
		public BattleData SecondBattle {
			get {
				if ( StartsFromDayBattle )
					return BattleNight;
				else
					return BattleDay;
			}
		}


		/// <summary>
		/// 出撃中に入手した艦船数
		/// </summary>
		public int DroppedShipCount { get; internal set; }

		/// <summary>
		/// 出撃中に入手した装備数
		/// </summary>
		public int DroppedEquipmentCount { get; internal set; }

		/// <summary>
		/// 出撃中に入手したアイテム - ID と 個数 のペア
		/// </summary>
		public Dictionary<int, int> DroppedItemCount { get; internal set; }


		/// <summary>
		/// 演習の敵提督名
		/// </summary>
		public string EnemyAdmiralName { get; internal set; }

		/// <summary>
		/// 演習の敵提督階級
		/// </summary>
		public string EnemyAdmiralRank { get; internal set; }



		public BattleManager() {
			DroppedItemCount = new Dictionary<int, int>();
		}


		public override void LoadFromResponse( string apiname, dynamic data ) {
			//base.LoadFromResponse( apiname, data );	//不要

			switch ( apiname ) {
				case "api_req_map/start":
				case "api_req_map/next":
					BattleDay = null;
					BattleNight = null;
					Result = null;
					BattleMode = BattleModes.Undefined;
					Compass = new CompassData();
					Compass.LoadFromResponse( apiname, data );

					if ( Compass.HasAirRaid ) {
						BattleMode = BattleModes.BaseAirRaid;
						BattleDay = new BattleBaseAirRaid();
						BattleDay.LoadFromResponse( apiname, Compass.AirRaidData );
						WriteBattleLog();
					}
					break;

				case "api_req_sortie/battle":
					BattleMode = BattleModes.Normal;
					BattleDay = new BattleNormalDay();
					BattleDay.LoadFromResponse( apiname, data );
					break;

				case "api_req_battle_midnight/battle":
					BattleNight = new BattleNormalNight();
					BattleNight.TakeOverParameters( BattleDay );
					BattleNight.LoadFromResponse( apiname, data );
					break;

				case "api_req_battle_midnight/sp_midnight":
					BattleMode = BattleModes.NightOnly;
					BattleDay = null;
					BattleNight = new BattleNightOnly();
					BattleNight.LoadFromResponse( apiname, data );
					break;

				case "api_req_sortie/airbattle":
					BattleMode = BattleModes.AirBattle;
					BattleDay = new BattleAirBattle();
					BattleDay.LoadFromResponse( apiname, data );
					break;

				case "api_req_sortie/ld_airbattle":
					BattleMode = BattleModes.AirRaid;
					BattleDay = new BattleAirRaid();
					BattleDay.LoadFromResponse( apiname, data );
					break;

				case "api_req_combined_battle/battle":
					BattleMode = BattleModes.Normal | BattleModes.CombinedTaskForce;
					BattleDay = new BattleCombinedNormalDay();
					BattleDay.LoadFromResponse( apiname, data );
					break;

				case "api_req_combined_battle/midnight_battle":
					BattleNight = new BattleCombinedNormalNight();
					//BattleNight.TakeOverParameters( BattleDay );		//checkme: 連合艦隊夜戦では昼戦での与ダメージがMVPに反映されない仕様？
					BattleNight.LoadFromResponse( apiname, data );
					BattleNight.Initial.TakeOverMaxHPs( BattleDay );
					break;

				case "api_req_combined_battle/sp_midnight":
					BattleMode = BattleModes.NightOnly | BattleModes.CombinedMask;
					BattleDay = null;
					BattleNight = new BattleCombinedNightOnly();
					BattleNight.LoadFromResponse( apiname, data );
					break;

				case "api_req_combined_battle/airbattle":
					BattleMode = BattleModes.AirBattle | BattleModes.CombinedTaskForce;
					BattleDay = new BattleCombinedAirBattle();
					BattleDay.LoadFromResponse( apiname, data );
					break;

				case "api_req_combined_battle/battle_water":
					BattleMode = BattleModes.Normal | BattleModes.CombinedSurface;
					BattleDay = new BattleCombinedWater();
					BattleDay.LoadFromResponse( apiname, data );
					break;

				case "api_req_combined_battle/ld_airbattle":
					BattleMode = BattleModes.AirRaid | BattleModes.CombinedTaskForce;
					BattleDay = new BattleCombinedAirRaid();
					BattleDay.LoadFromResponse( apiname, data );
					break;

				case "api_req_combined_battle/ec_battle":
					BattleMode = BattleModes.Normal | BattleModes.EnemyCombinedFleet;
					BattleDay = new BattleEnemyCombinedDay();
					BattleDay.LoadFromResponse( apiname, data );
					break;

				case "api_req_combined_battle/ec_midnight_battle":
					BattleNight = new BattleEnemyCombinedNight();
					BattleNight.TakeOverParameters( BattleDay );
					BattleNight.LoadFromResponse( apiname, data );
					BattleNight.Initial.TakeOverMaxHPs( BattleDay );
					break;

				case "api_req_combined_battle/each_battle":
					BattleMode = BattleModes.Normal | BattleModes.CombinedTaskForce | BattleModes.EnemyCombinedFleet;
					BattleDay = new BattleCombinedEachDay();
					BattleDay.LoadFromResponse( apiname, data );
					break;

				case "api_req_combined_battle/each_battle_water":
					BattleMode = BattleModes.Normal | BattleModes.CombinedSurface | BattleModes.EnemyCombinedFleet;
					BattleDay = new BattleCombinedEachWater();
					BattleDay.LoadFromResponse( apiname, data );
					break;


				case "api_req_member/get_practice_enemyinfo":
					EnemyAdmiralName = data.api_nickname;
					EnemyAdmiralRank = Constants.GetAdmiralRank( (int)data.api_rank );
					break;

				case "api_req_practice/battle":
					BattleMode = BattleModes.Practice;
					BattleDay = new BattlePracticeDay();
					BattleDay.LoadFromResponse( apiname, data );
					break;

				case "api_req_practice/midnight_battle":
					BattleNight = new BattlePracticeNight();
					BattleNight.TakeOverParameters( BattleDay );
					BattleNight.LoadFromResponse( apiname, data );
					break;

				case "api_req_sortie/battleresult":
				case "api_req_combined_battle/battleresult":
				case "api_req_practice/battle_result":
					Result = new BattleResultData();
					Result.LoadFromResponse( apiname, data );
					BattleFinished();
					break;

				case "api_port/port":
					Compass = null;
					BattleDay = null;
					BattleNight = null;
					Result = null;
					BattleMode = BattleModes.Undefined;
					DroppedShipCount = DroppedEquipmentCount = 0;
					DroppedItemCount.Clear();
					break;

				case "api_get_member/slot_item":
					DroppedEquipmentCount = 0;
					break;

			}

		}


		/// <summary>
		/// 戦闘終了時に各種データの収集を行います。
		/// </summary>
		private void BattleFinished() {

			//敵編成記録
			EnemyFleetRecord.EnemyFleetElement enemyFleetData = EnemyFleetRecord.EnemyFleetElement.CreateFromCurrentState();

			if ( enemyFleetData != null )
				RecordManager.Instance.EnemyFleet.Update( enemyFleetData );


			// ロギング
			if ( IsPractice ) {
				Utility.Logger.Add( 2,
					string.Format( "演習 で「{0}」{1}の「{2}」と交戦しました。( ランク: {3}, 提督Exp+{4}, 艦娘Exp+{5} )",
						EnemyAdmiralName, EnemyAdmiralRank, Result.EnemyFleetName, Result.Rank, Result.AdmiralExp, Result.BaseExp ) );
			} else {
				Utility.Logger.Add( 2,
					string.Format( "{0}-{1}-{2} で「{3}」と交戦しました。( ランク: {4}, 提督Exp+{5}, 艦娘Exp+{6} )",
						Compass.MapAreaID, Compass.MapInfoID, Compass.Destination, Result.EnemyFleetName, Result.Rank, Result.AdmiralExp, Result.BaseExp ) );
			}
			
			// Level up
			{
				var exps = Result.ExpList;
				var lvup = Result.LevelUpList;
				for ( int i = 0; i < lvup.Length; i++ ) {
					if ( lvup[i].Length >= 2 && lvup[i][0] + exps[i] >= lvup[i][1] ) {
						var ship = FirstBattle.Initial.FriendFleet.MembersInstance[i];
						int increment = Math.Max( lvup[i].Length - 2, 1 );

						Utility.Logger.Add( 2, string.Format( "{0} が Lv. {1} になりました。", ship.Name, ship.Level + increment ) );
					}
				}

				if ( IsCombinedBattle ) {
					exps = Result.ExpListCombined;
					lvup = Result.LevelUpListCombined;
					for ( int i = 0; i < lvup.Length; i++ ) {
						if ( lvup[i].Length >= 2 && lvup[i][0] + exps[i] >= lvup[i][1] ) {
							var ship = FirstBattle.Initial.FriendFleetEscort.MembersInstance[i];
							int increment = Math.Max( lvup[i].Length - 2, 1 );

							Utility.Logger.Add( 2, string.Format( "{0} が Lv. {1} になりました。", ship.Name, ship.Level + increment ) );
						}
					}
				}
			}



			//ドロップ艦記録
			if ( !IsPractice ) {

				//checkme: とてもアレな感じ

				int shipID = Result.DroppedShipID;
				int itemID  = Result.DroppedItemID;
				int eqID = Result.DroppedEquipmentID;
				bool showLog = Utility.Configuration.Config.Log.ShowSpoiler;

				if ( shipID != -1 ) {

					ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
					DroppedShipCount++;

					var defaultSlot = ship.DefaultSlot;
					if ( defaultSlot != null )
						DroppedEquipmentCount += defaultSlot.Count( id => id != -1 );

					if ( showLog )
						Utility.Logger.Add( 2, string.Format( "{0}「{1}」が戦列に加わりました。", ship.ShipTypeName, ship.NameWithClass ) );
				}

				if ( itemID != -1 ) {

					if ( !DroppedItemCount.ContainsKey( itemID ) )
						DroppedItemCount.Add( itemID, 0 );
					DroppedItemCount[itemID]++;

					if ( showLog ) {
						var item = KCDatabase.Instance.UseItems[itemID];
						var itemmaster = KCDatabase.Instance.MasterUseItems[itemID];
						Utility.Logger.Add( 2, string.Format( "アイテム「{0}」を入手しました。( 合計: {1}個 )", itemmaster != null ? itemmaster.Name : ( "不明なアイテム - ID:" + itemID ), ( item != null ? item.Count : 0 ) + DroppedItemCount[itemID] ) );
					}
				}

				if ( eqID != -1 ) {

					EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[eqID];
					DroppedEquipmentCount++;

					if ( showLog ) {
						Utility.Logger.Add( 2, string.Format( "{0}「{1}」を入手しました。", eq.CategoryTypeInstance.Name, eq.Name ) );
					}
				}


				// 満員判定
				if ( shipID == -1 && (
					KCDatabase.Instance.Admiral.MaxShipCount - ( KCDatabase.Instance.Ships.Count + DroppedShipCount ) <= 0 ||
					KCDatabase.Instance.Admiral.MaxEquipmentCount - ( KCDatabase.Instance.Equipments.Count + DroppedEquipmentCount ) <= 0 ) ) {
					shipID = -2;
				}

				RecordManager.Instance.ShipDrop.Add( shipID, itemID, eqID, Compass.MapAreaID, Compass.MapInfoID, Compass.Destination, Compass.MapInfo.EventDifficulty, Compass.EventID == 5, enemyFleetData.FleetID, Result.Rank, KCDatabase.Instance.Admiral.Level );
			}

			WriteBattleLog();



			//DEBUG
			/*/
			if ( Utility.Configuration.Config.Log.LogLevel <= 1 && Utility.Configuration.Config.Connection.SaveReceivedData ) {
				IEnumerable<int> damages;
				switch ( BattleMode & BattleModes.BattlePhaseMask ) {
					case BattleModes.Normal:
					case BattleModes.AirBattle:
					case BattleModes.Practice:
					default:
						damages = ( (BattleData)BattleNight ?? BattleDay ).AttackDamages;
						break;
					case BattleModes.NightOnly:
					case BattleModes.NightDay:
						damages = ( (BattleData)BattleDay ?? BattleNight ).AttackDamages;
						break;
				}

				damages = damages.Take( 6 ).Where( i => i > 0 );

				if ( damages.Count( i => i == damages.Max() ) > 1 ) {
					Utility.Logger.Add( 1, "MVP候補が複数存在します。ログを確認してください。" );
				}
			}
			//*/

		}



		/// <summary>
		/// 勝利ランクを予測します。
		/// </summary>
		/// <param name="friendrate">味方の損害率を出力します。</param>
		/// <param name="enemyrate">敵の損害率を出力します。</param>
		public int PredictWinRank( out double friendrate, out double enemyrate ) {

			int friendbefore = 0;
			int friendafter = 0;
			int friendcount = 0;
			int friendsunk = 0;

			int enemybefore = 0;
			int enemyafter = 0;
			int enemycount = 0;
			int enemysunk = 0;

			int[] initialHPs;
			BattleData activeBattle;

			if ( StartsFromDayBattle ) {
				initialHPs = BattleDay.Initial.InitialHPs;
				activeBattle = (BattleData)BattleNight ?? BattleDay;

			} else {
				initialHPs = BattleNight.Initial.InitialHPs;
				activeBattle = (BattleData)BattleDay ?? BattleNight;
			}


			var friend = activeBattle.Initial.FriendFleet.MembersWithoutEscaped;
			var friendescort = activeBattle.Initial.FriendFleetEscort == null ? null : activeBattle.Initial.FriendFleetEscort.MembersWithoutEscaped;

			var resultHPs = activeBattle.ResultHPs;


			for ( int i = 0; i < 6; i++ ) {
				if ( friend[i] != null ) {
					friendbefore += Math.Max( initialHPs[i], 0 );
					friendafter += Math.Max( resultHPs[i], 0 );
					friendcount++;
					if ( resultHPs[i] <= 0 )
						friendsunk++;
				}
				if ( friendescort != null && friendescort[i] != null ) {
					friendbefore += Math.Max( initialHPs[i + 12], 0 );
					friendafter += Math.Max( resultHPs[i + 12], 0 );
					friendcount++;
					if ( resultHPs[i + 12] <= 0 )
						friendsunk++;
				}

				if ( initialHPs[i + 6] >= 0 ) {
					enemybefore += Math.Max( initialHPs[i + 6], 0 );
					enemyafter += Math.Max( resultHPs[i + 6], 0 );
					enemycount++;
					if ( resultHPs[i + 6] <= 0 )
						enemysunk++;
				}
				if ( initialHPs[i + 18] >= 0 ) {
					enemybefore += Math.Max( initialHPs[i + 18], 0 );
					enemyafter += Math.Max( resultHPs[i + 18], 0 );
					enemycount++;
					if ( resultHPs[i + 18] <= 0 )
						enemysunk++;
				}
			}

			friendrate = (double)( friendbefore - friendafter ) / friendbefore;
			enemyrate = (double)( enemybefore - enemyafter ) / enemybefore;


			if ( ( BattleMode & BattleModes.BattlePhaseMask ) == BattleModes.AirRaid )
				return GetWinRankAirRaid( friendcount, friendsunk, friendrate );
			else
				return GetWinRank( friendcount, enemycount, friendsunk, enemysunk, friendrate, enemyrate, friend[0].HPRate <= 0.25, resultHPs[6] <= 0 );


		}


		/// <summary>
		/// 勝利ランクを計算します。連合艦隊は情報が少ないので正確ではありません。
		/// </summary>
		/// <param name="countFriend">戦闘に参加した自軍艦数。</param>
		/// <param name="countEnemy">戦闘に参加した敵軍艦数。</param>
		/// <param name="sunkFriend">撃沈された自軍艦数。</param>
		/// <param name="sunkEnemy">撃沈した敵軍艦数。</param>
		/// <param name="friendrate">自軍損害率。</param>
		/// <param name="enemyrate">敵軍損害率。</param>
		/// <param name="isfriendFlagshipHeavilyDamaged">自艦隊の旗艦が大破しているか。</param>
		/// <param name="defeatEnemyFlagship">敵旗艦を撃沈しているか。</param>
		/// <remarks>thanks: nekopanda</remarks>
		private static int GetWinRank(
			int countFriend, int countEnemy,
			int sunkFriend, int sunkEnemy,
			double friendrate, double enemyrate,
			bool isfriendFlagshipHeavilyDamaged, bool defeatEnemyFlagship ) {

			int rifriend = (int)( friendrate * 100 );
			int rienemy = (int)( enemyrate * 100 );


			// 轟沈艦なし
			if ( sunkFriend == 0 ) {

				// 敵艦全撃沈
				if ( sunkEnemy == countEnemy ) {
					if ( friendrate <= 0 )
						return 7;	// SS
					else
						return 6;	// S

				} else if ( countEnemy > 1 && sunkEnemy >= (int)( countEnemy * 0.7 ) )		// 敵の 70% 以上を撃沈
					return 5;	// A
			}

			// 敵旗艦撃沈 かつ 轟沈艦が敵より少ない
			if ( defeatEnemyFlagship && sunkFriend < sunkEnemy )
				return 4;	// B

			// 自艦隊1隻 かつ 旗艦大破
			if ( countFriend == 1 && isfriendFlagshipHeavilyDamaged )
				return 2;	// D

			// ゲージが 2.5 倍以上
			if ( rienemy > ( 2.5 * rifriend ) )
				return 4;	// B

			// ゲージが 0.9 倍以上
			if ( rienemy > ( 0.9 * rifriend ) )
				return 3;	// C

			// 轟沈艦あり かつ 残った艦が１隻のみ
			if ( sunkFriend > 0 && ( countFriend - sunkFriend ) == 1 ) {
				return 1;	// E
			}

			// 残りはD
			return 2;	// D
		}


		/// <summary>
		/// 空襲戦における勝利ランクを計算します。
		/// </summary>
		/// <param name="countFriend">戦闘に参加した自軍艦数。</param>
		/// <param name="sunkFriend">撃沈された自軍艦数。</param>
		/// <param name="friendrate">自軍損害率。</param>
		private static int GetWinRankAirRaid( int countFriend, int sunkFriend, double friendrate ) {
			int rank;

			if ( friendrate <= 0.0 )
				rank = 7;	//SS
			else if ( friendrate < 0.1 )
				rank = 5;	//A
			else if ( friendrate < 0.2 )
				rank = 4;	//B
			else if ( friendrate < 0.5 )
				rank = 3;	//C
			else if ( friendrate < 0.8 )
				rank = 2;	//D
			else
				rank = 1;	//E

			/*/// 撃沈艦があってもランクは変わらない(撃沈ありA勝利が確認されている)
			if ( sunkFriend > 0 )
				rank--;
			//*/

			return rank;
		}



		private void WriteBattleLog() {

			if ( !Utility.Configuration.Config.Log.SaveBattleLog )
				return;

			try {
				string parent = BattleLogPath;

				if ( !Directory.Exists( parent ) )
					Directory.CreateDirectory( parent );

				string info;
				if ( IsPractice )
					info = "practice";
				else
					info = string.Format( "{0}-{1}-{2}", Compass.MapAreaID, Compass.MapInfoID, Compass.Destination );

				string path = string.Format( "{0}\\{1}@{2}.txt", parent, DateTimeHelper.GetTimeStamp(), info );

				using ( var sw = new StreamWriter( path, false, Utility.Configuration.Config.Log.FileEncoding ) ) {
					sw.Write( BattleDetailDescriptor.GetBattleDetail( this ) );
				}

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "戦闘ログの出力に失敗しました。" );
			}
		}

	}

}
