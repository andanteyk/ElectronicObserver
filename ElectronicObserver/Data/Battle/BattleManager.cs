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
			Undefined,				//未定義
			Normal,					//昼夜戦(通常戦闘)
			NightOnly,				//夜戦
			NightDay,				//夜昼戦
			AirBattle,				//航空戦
			Practice,				//演習
			Combined = 0x10000,		//連合艦隊仕様
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

				case "api_req_combined_battle/battle":
					BattleMode = BattleModes.Normal | BattleModes.Combined;
					BattleDay = new BattleCombinedNormalDay();
					BattleDay.LoadFromResponse( apiname, data );
					break;

				case "api_req_combined_battle/midnight_battle":
					BattleNight = new BattleCombinedNormalNight();
					BattleNight.LoadFromResponse( apiname, data );
					break;

				case "api_req_combined_battle/sp_midnight":
					BattleMode = BattleModes.NightOnly | BattleModes.Combined;
					BattleNight = new BattleCombinedNightOnly();
					BattleNight.LoadFromResponse( apiname, data );
					break;

				case "api_req_combined_battle/airbattle":
					BattleMode = BattleModes.AirBattle | BattleModes.Combined;
					BattleDay = new BattleCombinedAirBattle();
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

	}

}
