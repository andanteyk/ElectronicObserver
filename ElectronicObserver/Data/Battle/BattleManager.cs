using ElectronicObserver.Resource.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Battle {

	/// <summary>
	/// 戦闘関連の処理を統括して扱います。
	/// </summary>
	public class BattleManager : ResponseWrapper {

		/// <summary>
		/// 羅針盤データ
		/// </summary>
		public CompassData Compass { get; private set; }

		/// <summary>
		/// 昼戦データ
		/// </summary>
		public BattleData BattleDay { get; private set; }

		/// <summary>
		/// 夜戦データ
		/// </summary>
		public BattleData BattleNight { get; private set; }

		/// <summary>
		/// 戦闘結果データ
		/// </summary>
		public BattleResultData Result { get; private set; }

		[Flags]
		public enum BattleModes {
			Undefined,						//未定義
			Normal,							//昼夜戦(通常戦闘)
			NightOnly,						//夜戦
			NightDay,						//夜昼戦
			AirBattle,						//航空戦
			Practice,						//演習
			BattlePhaseMask = 0xFFFF,		//戦闘形態マスク
			CombinedTaskForce = 0x10000,	//機動部隊
			CombinedSurface = 0x20000,		//水上部隊
			CombinedMask = 0x7FFF0000,		//連合艦隊仕様
		}

		/// <summary>
		/// 戦闘種別
		/// </summary>
		public BattleModes BattleMode { get; private set; }
		


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
					break;

				case "api_req_sortie/battle":
					BattleMode = BattleModes.Normal;
					BattleDay = new BattleNormalDay();
					BattleDay.LoadFromResponse( apiname, data );
					break;

				case "api_req_battle_midnight/battle":
					BattleNight = new BattleNormalNight();
					BattleNight.LoadFromResponse( apiname, data );
					break;

				case "api_req_battle_midnight/sp_midnight":
					BattleMode = BattleModes.NightOnly;
					BattleNight = new BattleNightOnly();
					BattleNight.LoadFromResponse( apiname, data );
					break;

				case "api_req_sortie/airbattle":
					BattleMode = BattleModes.AirBattle;
					BattleDay = new BattleAirBattle();
					BattleDay.LoadFromResponse( apiname, data );
					break;

				case "api_req_combined_battle/battle":
					BattleMode = BattleModes.Normal | BattleModes.CombinedTaskForce;
					BattleDay = new BattleCombinedNormalDay();
					BattleDay.LoadFromResponse( apiname, data );
					break;

				case "api_req_combined_battle/midnight_battle":
					BattleNight = new BattleCombinedNormalNight();
					BattleNight.LoadFromResponse( apiname, data );
					break;

				case "api_req_combined_battle/sp_midnight":
					BattleMode = BattleModes.NightOnly | BattleModes.CombinedMask;
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

				case "api_req_practice/battle":
					BattleMode = BattleModes.Practice;
					BattleDay = new BattlePracticeDay();
					BattleDay.LoadFromResponse( apiname, data );
					break;

				case "api_req_practice/midnight_battle":
					BattleNight = new BattlePracticeNight();
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
					break;

			}

		}


		/// <summary>
		/// 戦闘終了時に各種データの収集を行います。
		/// </summary>
		private void BattleFinished() {

			//敵編成記録
			switch ( BattleMode & BattleModes.BattlePhaseMask ) {
				case BattleModes.Normal:
				case BattleModes.AirBattle:
					RecordManager.Instance.EnemyFleet.Update( new EnemyFleetRecord.EnemyFleetElement( Compass.EnemyFleetID, Result.EnemyFleetName, (int)BattleDay.Data.api_formation[1], ( (int[])BattleDay.Data.api_ship_ke ).Skip( 1 ).ToArray() ) );
					break;

				case BattleModes.NightOnly:
				case BattleModes.NightDay:
					RecordManager.Instance.EnemyFleet.Update( new EnemyFleetRecord.EnemyFleetElement( Compass.EnemyFleetID, Result.EnemyFleetName, (int)BattleNight.Data.api_formation[1], ( (int[])BattleNight.Data.api_ship_ke ).Skip( 1 ).ToArray() ) );
					break;
			}


			//ドロップ艦記録
			if ( ( BattleMode & BattleModes.BattlePhaseMask ) != BattleModes.Practice ) {

				//checkme: とてもアレな感じ

				int dropID = Result.DroppedShipID;
				bool showLog = Utility.Configuration.Config.Log.ShowSpoiler;

				if ( dropID != -1 && showLog ) {
					ShipDataMaster ship = KCDatabase.Instance.MasterShips[dropID];
					Utility.Logger.Add( 2, string.Format( "{0}「{1}」が戦列に加わりました。", ship.ShipTypeName, ship.NameWithClass ) );
				}

				if ( dropID == -1 ) {

					int itemID = Result.DroppedItemID;

					if ( itemID != -1 ) {
						dropID = itemID + 1000;
						if ( showLog )
							Utility.Logger.Add( 2, string.Format( "アイテム「{0}」を入手しました。", KCDatabase.Instance.MasterUseItems[itemID].Name ) );
					}
				}

				if ( dropID == -1 ) {

					int eqID = Result.DroppedEquipmentID;

					if ( eqID != -1 ) {
						dropID = eqID + 2000;
						if ( showLog ) {
							EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[eqID];
							Utility.Logger.Add( 2, string.Format( "{0}「{1}」を入手しました。", eq.CategoryTypeInstance.Name, eq.Name ) );
						}
					}
				}

				if ( dropID == -1 && (
					KCDatabase.Instance.Admiral.MaxShipCount - KCDatabase.Instance.Ships.Count <= 0 ||
					KCDatabase.Instance.Admiral.MaxEquipmentCount - KCDatabase.Instance.Equipments.Count <= 3 ) ) {
					dropID = -2;
				}

				RecordManager.Instance.ShipDrop.Add( dropID, Compass.MapAreaID, Compass.MapInfoID, Compass.Destination, Compass.EventID == 5, Compass.EnemyFleetID, Result.Rank, KCDatabase.Instance.Admiral.Level );
			}

		}

	}

}
