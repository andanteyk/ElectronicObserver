using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 艦隊情報を統括して扱います。
	/// </summary>
	public class FleetManager : APIWrapper {

		public IDDictionary<FleetData> Fleets { get; private set; }


		/// <summary>
		/// 連合艦隊フラグ
		/// </summary>
		public int CombinedFlag { get; internal set; }

		/// <summary>
		/// 泊地修理タイマ
		/// </summary>
		public DateTime AnchorageRepairingTimer { get; private set; }



		/// <summary> 更新直前の艦船データ </summary>
		private IDDictionary<ShipData> PreviousShips;

		/// <summary> 更新直前に泊地修理が可能だったか </summary>
		private Dictionary<int, bool> IsAnchorageRepaired;

		/// <summary> 更新直前の入渠艦IDリスト </summary>
		private HashSet<int> PreviousDockingID;

		// conditions
		public static readonly TimeSpan ConditionHealingSpan = TimeSpan.FromSeconds( 180 );
		private double ConditionPredictMin;
		private double ConditionPredictMax;
		private DateTime LastConditionUpdated;

		/// <summary> コンディションが回復する秒オフセット </summary>
		public double ConditionBorderSeconds { get { return ConditionPredictMax % ConditionHealingSpan.TotalSeconds; } }

		/// <summary> コンディションが回復する秒オフセット の精度[秒] </summary>
		public double ConditionBorderAccuracy { get { return ConditionPredictMax - ConditionPredictMin; } }


		public FleetManager() {
			Fleets = new IDDictionary<FleetData>();
			AnchorageRepairingTimer = DateTime.MinValue;

			ConditionPredictMin = 0;
			ConditionPredictMax = ConditionHealingSpan.TotalSeconds * 2;
			LastConditionUpdated = DateTime.Now;
			PreviousShips = new IDDictionary<ShipData>();
			IsAnchorageRepaired = new Dictionary<int, bool>();
		}


		public FleetData this[int fleetID] {
			get {
				return Fleets[fleetID];
			}
		}


		public override void LoadFromResponse( string apiname, dynamic data ) {

			switch ( apiname ) {
				case "api_req_combined_battle/goback_port":
					foreach ( int index in KCDatabase.Instance.Battle.Result.EscapingShipIndex ) {
						Fleets[( index - 1 ) < 6 ? 1 : 2].Escape( ( index - 1 ) % 6 );
					}
					break;

				case "api_get_member/ndock":
					foreach ( var fleet in Fleets.Values ) {
						fleet.LoadFromResponse( apiname, data );
					}
					break;

				case "api_req_hensei/preset_select": {
						int id = (int)data.api_id;

						if ( !Fleets.ContainsKey( id ) ) {
							var a = new FleetData();
							a.LoadFromResponse( apiname, data );
							Fleets.Add( a );

						} else {
							Fleets[id].LoadFromResponse( apiname, data );
						}

					} break;

				default:
					base.LoadFromResponse( apiname, (object)data );

					//api_port/port, api_get_member/deck
					foreach ( var elem in data ) {

						int id = (int)elem.api_id;

						if ( !Fleets.ContainsKey( id ) ) {
							var a = new FleetData();
							a.LoadFromResponse( apiname, elem );
							Fleets.Add( a );

						} else {
							Fleets[id].LoadFromResponse( apiname, elem );
						}
					}
					break;
			}


			// 泊地修理・コンディションの処理
			if ( apiname == "api_port/port" ) {

				if ( ( DateTime.Now - AnchorageRepairingTimer ).TotalMinutes >= 20 )
					StartAnchorageRepairingTimer();
				else
					CheckAnchorageRepairingHealing();

				UpdateConditionPrediction();
			}
		}



		public override void LoadFromRequest( string apiname, Dictionary<string, string> data ) {
			base.LoadFromRequest( apiname, data );

			switch ( apiname ) {
				case "api_req_hensei/change": {
						int memberID = int.Parse( data["api_ship_idx"] );		//変更スロット
						if ( memberID != -1 )
							data.Add( "replaced_id", Fleets[int.Parse( data["api_id"] )].Members[memberID].ToString() );

						foreach ( int i in Fleets.Keys )
							Fleets[i].LoadFromRequest( apiname, data );

					} break;

				case "api_req_map/start": {
						int fleetID = int.Parse( data["api_deck_id"] );
						if ( CombinedFlag != 0 && fleetID == 1 ) {
							Fleets[2].IsInSortie = true;
						}
						Fleets[fleetID].IsInSortie = true;
					} goto default;

				case "api_req_hensei/combined":
					CombinedFlag = int.Parse( data["api_combined_type"] );
					break;

				case "api_req_practice/battle": {
						int fleetID = int.Parse( data["api_deck_id"] );
						Fleets[fleetID].IsInSortie = true;
					} break;

				default:
					foreach ( int i in Fleets.Keys )
						Fleets[i].LoadFromRequest( apiname, data );
					break;

			}

		}


		/// <summary>
		/// 泊地修理タイマを現在時刻にセットします。
		/// </summary>
		public void StartAnchorageRepairingTimer() {
			AnchorageRepairingTimer = DateTime.Now;
		}

		/// <summary>
		/// 泊地修理による回復が発生していたかをチェックし、発生していた場合は泊地修理タイマをリセットします。
		/// </summary>
		public void CheckAnchorageRepairingHealing() {
			foreach ( var f in Fleets.Values ) {
				if ( IsAnchorageRepaired.ContainsKey( f.FleetID ) && !IsAnchorageRepaired[f.FleetID] )
					continue;

				var prev = f.Members.Select( id => PreviousDockingID.Contains( id ) ? null : PreviousShips[id] ).ToArray();
				var now = f.MembersInstance.ToArray();

				for ( int i = 0; i < prev.Length; i++ ) {
					if ( prev[i] == null || now[i] == null )
						continue;

					// 回復検知
					if ( prev[i].RepairingDockID == -1 && prev[i].HPCurrent < now[i].HPCurrent ) {
						StartAnchorageRepairingTimer();

						//debug
						if ( Utility.Configuration.Config.Debug.EnableDebugMenu )
							Utility.Logger.Add( 1, "泊地修理: 回復を検知したためタイマーをリセットします。" );
						return;
					}

				}
			}
		}


		/// <summary>
		/// 更新直前の艦船データをコピーして退避します。
		/// </summary>
		public void EvacuatePreviousShips() {

			if ( Fleets.Values.Any( f => f != null && f.IsInSortie ) )
				return;

			PreviousShips = new IDDictionary<ShipData>( KCDatabase.Instance.Ships.Values );
			IsAnchorageRepaired = Fleets.ToDictionary( f => f.Key, f => f.Value.CanAnchorageRepair );
			PreviousDockingID = new HashSet<int>( KCDatabase.Instance.Docks.Values.Select( d => d.ShipID ) );
		}


		/// <summary>
		/// コンディションの更新予測パラメータを更新します。
		/// </summary>
		public void UpdateConditionPrediction() {

			var now = DateTime.Now;

			var conditionDiff = PreviousShips.Where( s => s.Value.Condition < 49 )
				.Join( KCDatabase.Instance.Ships.Values, pair => pair.Key, ship => ship.ID, ( pair, ship ) => ship.Condition - pair.Value.Condition );
			if ( !conditionDiff.Any() ) {
				goto LabelFinally;
			}

			int healed = (int)Math.Ceiling( conditionDiff.Max() / 3.0 );
			int predictedHealLow = (int)Math.Floor( ( now - LastConditionUpdated ).TotalSeconds / ConditionHealingSpan.TotalSeconds );


			if ( healed < predictedHealLow ) {
				goto LabelFinally;
			}

			double newPredictMin, newPredictMax;

			if ( healed <= predictedHealLow ) {
				newPredictMin = TimeSpan.FromTicks( now.Ticks % ConditionHealingSpan.Ticks ).TotalSeconds;
				newPredictMax = TimeSpan.FromTicks( LastConditionUpdated.Ticks % ConditionHealingSpan.Ticks ).TotalSeconds;
			} else {
				newPredictMin = TimeSpan.FromTicks( LastConditionUpdated.Ticks % ConditionHealingSpan.Ticks ).TotalSeconds;
				newPredictMax = TimeSpan.FromTicks( now.Ticks % ConditionHealingSpan.Ticks ).TotalSeconds;
			}

			if ( newPredictMax < newPredictMin )
				newPredictMax += ConditionHealingSpan.TotalSeconds;

			double amin, amax, apre, bmin, bmax, bpre;
			if ( ConditionPredictMin < newPredictMin ) {
				amin = ConditionPredictMin;
				amax = ConditionPredictMax;
				apre = ConditionPredictMax - ConditionHealingSpan.TotalSeconds;
				bmin = newPredictMin;
				bmax = newPredictMax;
				bpre = newPredictMax - ConditionHealingSpan.TotalSeconds;
			} else {
				bmin = ConditionPredictMin;
				bmax = ConditionPredictMax;
				bpre = ConditionPredictMax - ConditionHealingSpan.TotalSeconds;
				amin = newPredictMin;
				amax = newPredictMax;
				apre = newPredictMax - ConditionHealingSpan.TotalSeconds;
			}

			bool startsWithAmin = amin < bpre;
			bool startsWithBmin = bmin < amax;

			bool endsWithBpre = amin < bpre && bpre < amax;
			bool endsWithAmax = ( bmin < amax || amax <= bpre ) && amax < bmax;
			bool endsWidthBmax = bmax < amax;

			if ( ( startsWithAmin && startsWithBmin ) || ( endsWithBpre && endsWithAmax ) ) {
				// 二重領域; どちらか小さいほう
				if ( amax - amin < bmax - bmin ) {
					ConditionPredictMin = amin;
					ConditionPredictMax = amax;
				} else {
					ConditionPredictMin = bmin;
					ConditionPredictMax = bmax;
				}
			} else {
				if ( startsWithAmin )
					ConditionPredictMin = amin;
				else if ( startsWithBmin )
					ConditionPredictMin = bmin;
				else {
					ConditionPredictMin = newPredictMin;     // 空集合; 新しいほうを設定
				}

				if ( endsWithBpre )
					ConditionPredictMax = bpre;
				else if ( endsWithAmax )
					ConditionPredictMax = amax;
				else if ( endsWidthBmax )
					ConditionPredictMax = bmax;
				else {
					ConditionPredictMax = newPredictMax;     // 空集合; 新しいほうを設定
				}
			}


LabelFinally:
			LastConditionUpdated = now;

			foreach ( var f in Fleets.Values )
				f.UpdateConditionTime();

		}


		/// <summary>
		/// 指定された疲労が少なくとも回復するはずの時刻を取得します。
		/// </summary>
		/// <param name="healAmount">回復する cond 値の量(現在値からの増分)。</param>
		/// <returns></returns>
		public DateTime CalculateConditionHealingEstimation( int healAmount ) {
			healAmount = (int)Math.Ceiling( healAmount / 3.0 );

			if ( healAmount <= 0 )
				return DateTime.Now;

			double last = TimeSpan.FromTicks( LastConditionUpdated.Ticks % ConditionHealingSpan.Ticks ).TotalSeconds;

			var firstHeal = TimeSpan.FromSeconds( ConditionBorderSeconds - last );
			var afterHeal = TimeSpan.FromSeconds( ConditionHealingSpan.TotalSeconds * ( healAmount - 1 ) );

			if ( ConditionPredictMin <= last && last <= ConditionPredictMax )
				firstHeal = ConditionHealingSpan;
			if ( firstHeal.Ticks <= 0 )
				firstHeal += ConditionHealingSpan;

			var offset = firstHeal + afterHeal;


			return LastConditionUpdated + offset;

		}

	}

}
