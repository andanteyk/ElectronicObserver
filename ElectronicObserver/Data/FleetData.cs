using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 艦隊の情報を保持します。
	/// </summary>
	[DebuggerDisplay( "[{ID}] : {Name}" )]
	public class FleetData : APIWrapper, IIdentifiable {

		/// <summary>
		/// 艦隊ID
		/// </summary>
		public int FleetID {
			get { return (int)RawData.api_id; }
		}

		/// <summary>
		/// 艦隊名
		/// </summary>
		public string Name { get; internal set; }

		/// <summary>
		/// 遠征状態
		/// 0=未出撃, 1=遠征中, 2=遠征帰投, 3=強制帰投中
		/// </summary>
		public int ExpeditionState { get; internal set; }

		/// <summary>
		/// 遠征先ID
		/// </summary>
		public int ExpeditionDestination { get; internal set; }

		/// <summary>
		/// 遠征帰投時間
		/// </summary>
		public DateTime ExpeditionTime { get; internal set; }


		private int[] _members;
		/// <summary>
		/// 艦隊メンバー(艦船ID)
		/// </summary>
		public ReadOnlyCollection<int> Members {
			get { return Array.AsReadOnly<int>( _members ); }
		}

		/// <summary>
		/// 艦隊メンバー(艦船データ)
		/// </summary>
		public ReadOnlyCollection<ShipData> MembersInstance {
			get {
				if ( _members == null ) return null;

				ShipData[] ships = new ShipData[_members.Length];
				for ( int i = 0; i < ships.Length; i++ ) {
					ships[i] = KCDatabase.Instance.Ships[_members[i]];
				}

				return Array.AsReadOnly<ShipData>( ships );
			}
		}

		/// <summary>
		/// 艦隊メンバー(艦船データ、退避艦を除く)
		/// </summary>
		public ReadOnlyCollection<ShipData> MembersWithoutEscaped {
			get {
				if ( _members == null ) return null;

				ShipData[] ships = new ShipData[_members.Length];
				for ( int i = 0; i < ships.Length; i++ ) {
					ships[i] = _escapedShipList.Contains( _members[i] ) ? null : KCDatabase.Instance.Ships[_members[i]];
				}

				return Array.AsReadOnly<ShipData>( ships );
			}
		}


		public int this[int i] {
			get {
				return _members[i];
			}
		}


		private List<int> _escapedShipList = new List<int>();
		/// <summary>
		/// 退避艦のIDリスト
		/// </summary>
		public ReadOnlyCollection<int> EscapedShipList {
			get { return _escapedShipList.AsReadOnly(); }
		}

		/// <summary>
		/// 出撃中かどうか
		/// </summary>
		public bool IsInSortie { get; internal set; }




		public int ID {
			get { return FleetID; }
		}



		public FleetData()
			: base() {

		}


		public override void LoadFromResponse( string apiname, dynamic data ) {

			switch ( apiname ) {

				case "api_port/port":
					base.LoadFromResponse( apiname, (object)data );

					Name = (string)RawData.api_name;
					_members = (int[])RawData.api_ship;
					ExpeditionState = (int)RawData.api_mission[0];
					ExpeditionDestination = (int)RawData.api_mission[1];
					ExpeditionTime = DateTimeHelper.FromAPITime( (long)RawData.api_mission[2] );

					_escapedShipList.Clear();
					if ( IsInSortie ) {
						Utility.Logger.Add( 2, string.Format( "#{0}「{1}」が帰投しました。", FleetID, Name ) );
					}
					IsInSortie = false;

					break;

				case "api_get_member/ndock":
				case "api_req_kousyou/destroyship":
				case "api_get_member/ship3":
				case "api_req_kaisou/powerup":
					break;

				default:	//checkme
					base.LoadFromResponse( apiname, (object)data );

					Name = (string)RawData.api_name;
					_members = (int[])RawData.api_ship;
					ExpeditionState = (int)RawData.api_mission[0];
					ExpeditionDestination = (int)RawData.api_mission[1];
					ExpeditionTime = DateTimeHelper.FromAPITime( (long)RawData.api_mission[2] );
					break;

			}

		}


		public override void LoadFromRequest( string apiname, Dictionary<string, string> data ) {
			base.LoadFromRequest( apiname, data );	//checkme


			switch ( apiname ) {
				case "api_req_hensei/change": {
						int fleetID = int.Parse( data["api_id"] );
						int index = int.Parse( data["api_ship_idx"] );
						int shipID = int.Parse( data["api_ship_id"] );
						int replacedID = data.ContainsKey( "replaced_id" ) ? int.Parse( data["replaced_id"] ) : -1;
						int flagshipID = _members[0];

						if ( FleetID == fleetID ) {
							if ( index == -1 ) {
								//旗艦以外全解除
								for ( int i = 1; i < _members.Length; i++ )
									_members[i] = -1;

							} else if ( shipID == -1 ) {
								//はずす
								RemoveShip( index );

							} else {
								//入隊

								for ( int y = index - 1; y >= 0; y-- ) {		// 変更位置よりも前に空欄があれば位置をずらす
									if ( _members[y] != -1 ) {
										index = y + 1;
										break;
									}
								}

								_members[index] = shipID;

								//入れ替え
								for ( int i = 0; i < _members.Length; i++ ) {
									if ( i != index && _members[i] == shipID ) {

										if ( replacedID != -1 )
											_members[i] = replacedID;
										else
											RemoveShip( i );

										break;
									}
								}

							}


							if ( index != -1 && IsFlagshipRepairShip )		//随伴艦一括解除を除く
								KCDatabase.Instance.Fleet.StartAnchorageRepairingTimer();

						} else {

							if ( index != -1 && shipID != -1 ) {
								//入れ替え
								for ( int i = 0; i < _members.Length; i++ ) {
									if ( _members[i] == shipID ) {

										if ( replacedID != -1 )
											_members[i] = replacedID;
										else
											RemoveShip( i );

										if ( IsFlagshipRepairShip )
											KCDatabase.Instance.Fleet.StartAnchorageRepairingTimer();

										break;
									}
								}

							}

						}


					} break;


				case "api_req_kousyou/destroyship": {
						int shipID = int.Parse( data["api_ship_id"] );

						for ( int i = 0; i < _members.Length; i++ ) {
							if ( _members[i] == shipID ) {
								RemoveShip( i );
								break;
							}
						}

					} break;

				case "api_req_kaisou/powerup": {
						foreach ( int id in data["api_id_items"].Split( ",".ToCharArray() ).Select( s => int.Parse( s ) ) ) {
							for ( int i = 0; i < _members.Length; i++ ) {
								if ( _members[i] == id ) {
									RemoveShip( i );
									break;
								}
							}
						}
					} break;

				case "api_req_mission/start":
					ExpeditionState = 1;
					ExpeditionDestination = int.Parse( data["api_mission_id"] );
					ExpeditionTime = DateTime.Now;	//暫定処理。実際の更新はResponseで行う

					break;


				case "api_req_member/updatedeckname":
					Name = data["api_name"];
					break;

			}

		}


		/// <summary>
		/// 指定した艦娘を艦隊からはずします。
		/// </summary>
		/// <param name="index">対象艦のインデックス。0-5</param>
		private void RemoveShip( int index ) {

			for ( int i = index + 1; i < _members.Length; i++ )
				_members[i - 1] = _members[i];

			_members[_members.Length - 1] = -1;

		}



		/// <summary>
		/// 護衛退避を実行します。
		/// </summary>
		/// <param name="index">対象艦の艦隊内でのインデックス。0-5</param>
		public void Escape( int index ) {
			_escapedShipList.Add( _members[index] );
		}


		/// <summary>
		/// 制空戦力を取得します。
		/// </summary>
		/// <returns>制空戦力。</returns>
		public int GetAirSuperiority() {
			switch ( Utility.Configuration.Config.FormFleet.AirSuperiorityMethod ) {
				case 0:
				default:
					return Calculator.GetAirSuperiorityIgnoreLevel( this );
				case 1:
					return Calculator.GetAirSuperiority( this );
			}
		}

		/// <summary>
		/// 現在の設定に応じて、制空戦力を表す文字列を取得します。
		/// </summary>
		/// <returns></returns>
		public string GetAirSuperiorityString() {
			switch ( Utility.Configuration.Config.FormFleet.AirSuperiorityMethod ) {
				case 0:
				default:
					return Calculator.GetAirSuperiorityIgnoreLevel( this ).ToString();
				case 1: {
						int min = Calculator.GetAirSuperiority( this, false );
						int max = Calculator.GetAirSuperiority( this, true );

						if ( Utility.Configuration.Config.FormFleet.ShowAirSuperiorityRange && min < max )
							return string.Format( "{0} ～ {1}", min, max );
						else
							return min.ToString();
					}
			}
		}

		/// <summary>
		/// 現在の設定に応じて、索敵能力を取得します。
		/// </summary>
		public double GetSearchingAbility() {
			switch ( Utility.Configuration.Config.FormFleet.SearchingAbilityMethod ) {
				default:
				case 0:
					return Calculator.GetSearchingAbility_Old( this );

				case 1:
					return Calculator.GetSearchingAbility_Autumn( this );

				case 2:
					return Calculator.GetSearchingAbility_TinyAutumn( this );

				case 3:
					return Calculator.GetSearchingAbility_33( this );

				case 4:
					return Calculator.GetSearchingAbility_New33( this, 1 );
			}
		}

		/// <summary>
		/// 現在の設定に応じて、索敵能力を表す文字列を取得します。
		/// </summary>
		public string GetSearchingAbilityString() {
			return this.GetSearchingAbilityString( Utility.Configuration.Config.FormFleet.SearchingAbilityMethod );
		}

		/// <summary>
		/// 指定の計算式で、索敵能力を表す文字列を取得します。
		/// </summary>
		/// <param name="index">計算式。0-3</param>
		public string GetSearchingAbilityString( int index ) {
			switch ( index ) {
				default:
				case 0:
					return Calculator.GetSearchingAbility_Old( this ).ToString();

				case 1:
					return Calculator.GetSearchingAbility_Autumn( this ).ToString( "F1" );

				case 2:
					return Calculator.GetSearchingAbility_TinyAutumn( this ).ToString();

				case 3:
					return ( Math.Floor( Calculator.GetSearchingAbility_33( this ) * 100 ) / 100 ).ToString( "F2" );

				case 4:
					return ( Math.Floor( Calculator.GetSearchingAbility_New33( this, 1 ) * 100 ) / 100 ).ToString( "F2" );
			}
		}

		/// <summary>
		/// 触接開始率を取得します。
		/// </summary>
		/// <returns></returns>
		public double GetContactProbability() {
			return Calculator.GetContactProbability( this );
		}

		public Dictionary<int, double> GetContactSelectionProbability() {
			return Calculator.GetContactSelectionProbability( this );
		}


		/// <summary>
		/// 旗艦が工作艦か
		/// </summary>
		public bool IsFlagshipRepairShip {
			get {
				ShipData flagship = KCDatabase.Instance.Ships[_members[0]];
				return flagship != null && flagship.MasterShip.ShipType == 19;
			}
		}

		/// <summary>
		/// 泊地修理が発動可能か
		/// </summary>
		public bool CanAnchorageRepair {
			get {
				// 流石に資源チェックまではしない
				var flagship = KCDatabase.Instance.Ships[_members[0]];

				return IsFlagshipRepairShip &&
					flagship.HPRate > 0.5 &&
					flagship.RepairingDockID == -1 &&
					ExpeditionState == 0 &&
					MembersInstance.Take( 2 + flagship.SlotInstance.Count( eq => eq != null && eq.MasterEquipment.CategoryType == 31 ) )
					.Any( ship => ship != null && 0.5 < ship.HPRate && ship.HPRate < 1.0 && ship.RepairingDockID == -1 );
			}
		}


		/// <summary>
		/// 疲労が回復すると予測される日時 (疲労していない場合は null)
		/// </summary>
		public DateTime? ConditionTime { get; private set; }

		public void UpdateConditionTime() {
			var ships = MembersInstance.Where( ship => ship != null && ship.Condition < Utility.Configuration.Config.Control.ConditionBorder );
			if ( !ships.Any() ) {
				ConditionTime = null;

			} else {
				ConditionTime = KCDatabase.Instance.Fleet.CalculateConditionHealingEstimation( Utility.Configuration.Config.Control.ConditionBorder - ships.Min( ship => ship.Condition ) );
			}
		}

	}

}
