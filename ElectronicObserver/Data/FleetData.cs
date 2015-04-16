﻿using ElectronicObserver.Resource;
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



		public DateTime? ConditionTime { get; internal set; }
		public bool IsConditionTimeLocked { get; internal set; }
		public bool IsAnchorageRepairing { get; internal set; }


		public int ID {
			get { return FleetID; }
		}



		public FleetData()
			: base() {

			ConditionTime = null;
			IsConditionTimeLocked = true;
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
                        Utility.Logger.Add(2, string.Format(LoadResources.getter("FleetData_22"), FleetID, Name));
					}
					IsInSortie = false;

					UnlockConditionTimer();
					ShortenConditionTimer();
					UpdateAnchorageRepairingState();
					break;

				case "api_get_member/ndock":
				case "api_req_kousyou/destroyship":
				case "api_get_member/ship3":
				case "api_req_kaisou/powerup":
					ShortenConditionTimer();
					UpdateAnchorageRepairingState();
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


							SetConditionTimer();

						} else {

							if ( index != -1 && shipID != -1 ) {
								//入れ替え
								for ( int i = 0; i < _members.Length; i++ ) {
									if ( _members[i] == shipID ) {

										if ( replacedID != -1 )
											_members[i] = replacedID;
										else
											RemoveShip( i );

										break;
									}
								}

							}

						}

						UpdateAnchorageRepairingState();
						if ( flagshipID != _members[0] && MembersInstance[0] != null && MembersInstance[0].MasterShip.ShipType == 19 ) {
							KCDatabase.Instance.Fleet.ResetAnchorageRepairing();
						}

					} break;


				case "api_req_kousyou/destroyship": {
						int shipID = int.Parse( data["api_ship_id"] );

						for ( int i = 0; i < _members.Length; i++ ) {
							if ( _members[i] == shipID ) {
								RemoveShip( i );
								ShortenConditionTimer();
								break;
							}
						}
						UpdateAnchorageRepairingState();

					} break;

				case "api_req_kaisou/powerup": {
						foreach ( int id in data["api_id_items"].Split( ",".ToCharArray() ).Select( s => int.Parse( s ) ) ) {
							for ( int i = 0; i < _members.Length; i++ ) {
								if ( _members[i] == id ) {
									RemoveShip( i );
									ShortenConditionTimer();
									break;
								}
							}
						}
						UpdateAnchorageRepairingState();
					} break;

				case "api_req_kaisou/remodeling":	//fixme: ここでリセットしてもまだデータが送られてきてないので無意味
					if ( Members.Contains( int.Parse( data["api_id"] ) ) ) {
						SetConditionTimer();
						UpdateAnchorageRepairingState();
					}
					break;

				case "api_req_nyukyo/start":
				case "api_req_nyukyo/speedchange":
					ShortenConditionTimer();
					UpdateAnchorageRepairingState();
					break;

				case "api_req_mission/start":
					ExpeditionState = 1;
					ExpeditionDestination = int.Parse( data["api_mission_id"] );
					ExpeditionTime = DateTime.Now;	//暫定処理。実際の更新はResponseで行う

					break;

				case "api_req_map/start":
					if ( int.Parse( data["api_deck_id"] ) == FleetID )
						LockConditionTimer();
					break;

				case "api_req_member/updatedeckname":
					Name = data["api_name"];
					break;

			}

		}


		private void RemoveShip( int index ) {

			for ( int i = index + 1; i < _members.Length; i++ )
				_members[i - 1] = _members[i];

			_members[_members.Length - 1] = -1;

		}


		private int GetConditionRecoveryMinute( int cond ) {
			return Math.Max( (int)Math.Ceiling( ( Utility.Configuration.Config.Control.ConditionBorder - cond ) / 3.0 ) * 3, 0 );
		}

		private void SetConditionTimer() {

			int minute = GetConditionRecoveryMinute( MembersInstance.Min( s => s != null ? s.Condition : 100 ) );

			if ( minute > 0 )
				ConditionTime = DateTime.Now.AddMinutes( minute );
			else
				ConditionTime = null;

            //Utility.Logger.Add(1, string.Format(LoadResources.getter("FleetData_23"), FleetID, minute));
		}

		private void ShortenConditionTimer() {

			int minute = GetConditionRecoveryMinute( MembersInstance.Min( s => s != null ? s.Condition : 100 ) );

			if ( minute == 0 ) {
				ConditionTime = null;

			} else {
				DateTime target = DateTime.Now.AddMinutes( minute );

				if ( ConditionTime == null || target < ConditionTime ) {
					ConditionTime = target;
				}
			}

			/*/
			{
				TimeSpan ts = ( ConditionTime ?? DateTime.Now ) - DateTime.Now;
                Utility.Logger.Add(1, string.Format(LoadResources.getter("FleetData_24"), FleetID, minute, (int)ts.TotalMinutes, (int)ts.Seconds));
			}
			//*/
		}

		private void LockConditionTimer() {
			IsConditionTimeLocked = true;
		}

		private void UnlockConditionTimer() {
			if ( IsConditionTimeLocked ) {
				IsConditionTimeLocked = false;
				SetConditionTimer();
			}
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

			return Calculator.GetAirSuperiority( this );
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
		/// <param name="index">計算式。0-2</param>
		public string GetSearchingAbilityString( int index ) {
			switch ( index ) {
				default:
				case 0:
					return Calculator.GetSearchingAbility_Old( this ).ToString();

				case 1:
					return Calculator.GetSearchingAbility_Autumn( this ).ToString( "F1" );

				case 2:
					return Calculator.GetSearchingAbility_TinyAutumn( this ).ToString();
			}
		}



		/// <summary>
		/// 艦隊の状態を表します。
		/// </summary>
		public enum FleetStates {
			NoShip,
			Docking,
			SortieDamaged,
			Sortie,
			Expedition,
			Damaged,
			NotReplenished,
			Tired,
			Sparkled,
			AnchorageRepairing,
			Ready,
		}


		/// <summary>
		/// 艦隊の状態の情報をラベルに適用します。
		/// </summary>
		/// <param name="fleet">艦隊データ。</param>
		/// <param name="label">適用するラベル。</param>
		/// <param name="tooltip">適用するツールチップ。</param>
		/// <param name="prevstate">前回の状態。</param>
		/// <param name="timer">日時。</param>
		/// <returns>艦隊の状態を表す定数。</returns>
		public static FleetStates UpdateFleetState( FleetData fleet, ImageLabel label, ToolTip tooltip, FleetStates prevstate, ref DateTime timer ) {

			KCDatabase db = KCDatabase.Instance;


			//初期化
			tooltip.SetToolTip( label, null );
			label.BackColor = Color.Transparent;



			//所属艦なし
			if ( fleet == null || fleet.Members.Count( id => id != -1 ) == 0 ) {
                label.Text = LoadResources.getter("FleetData_1");
				label.ImageIndex = (int)ResourceManager.IconContent.FleetNoShip;

				return FleetStates.NoShip;
			}

			{	//入渠中
				long ntime = db.Docks.Values.Max(
						dock => {
							if ( dock.State == 1 && fleet.Members.Count( ( id => id == dock.ShipID ) ) > 0 )
								return dock.CompletionTime.Ticks;
							else return 0;
						}
						);

				if ( ntime > 0 ) {	//入渠中

					timer = new DateTime( ntime );
					label.Text = LoadResources.getter("FleetData_2") + DateTimeHelper.ToTimeRemainString( timer );
					label.ImageIndex = (int)ResourceManager.IconContent.FleetDocking;

                    tooltip.SetToolTip(label, LoadResources.getter("FleetData_3") + timer);

					return FleetStates.Docking;
				}

			}


			if ( fleet.IsInSortie ) {

				//大破出撃中
				if ( fleet.MembersInstance.Count( s =>
						( s != null && !fleet.EscapedShipList.Contains( s.MasterID ) && (double)s.HPCurrent / s.HPMax <= 0.25 )
					 ) > 0 ) {

                    label.Text = LoadResources.getter("FleetData_4");
					label.ImageIndex = (int)ResourceManager.IconContent.FleetSortieDamaged;

					return FleetStates.SortieDamaged;

				} else {	//出撃中

                    label.Text = LoadResources.getter("FleetData_5");
					label.ImageIndex = (int)ResourceManager.IconContent.FleetSortie;

					return FleetStates.Sortie;
				}

			}


			//遠征中
			if ( fleet.ExpeditionState != 0 ) {

				timer = fleet.ExpeditionTime;
                label.Text = LoadResources.getter("FleetData_6") + DateTimeHelper.ToTimeRemainString(timer);
				label.ImageIndex = (int)ResourceManager.IconContent.FleetExpedition;

                tooltip.SetToolTip(label, string.Format("{0} : {1}\r\n" + LoadResources.getter("FleetData_7") + "{2}", KCDatabase.Instance.Mission[fleet.ExpeditionDestination].ID, KCDatabase.Instance.Mission[fleet.ExpeditionDestination].Name, timer));

				return FleetStates.Expedition;
			}

			//大破艦あり
			if ( fleet.MembersInstance.Count( s =>
				( s != null && !fleet.EscapedShipList.Contains( s.MasterID ) && (double)s.HPCurrent / s.HPMax <= 0.25 )
			 ) > 0 ) {

                label.Text = LoadResources.getter("FleetData_8");
				label.ImageIndex = (int)ResourceManager.IconContent.FleetDamaged;
				//label.BackColor = Color.LightCoral;

				return FleetStates.Damaged;
			}

			//泊地修理中
			{
				if ( fleet.IsAnchorageRepairing ) {

                    label.Text = LoadResources.getter("FleetData_9") + DateTimeHelper.ToTimeElapsedString(KCDatabase.Instance.Fleet.AnchorageRepairingTimer);
					label.ImageIndex = (int)ResourceManager.IconContent.FleetAnchorageRepairing;

                    tooltip.SetToolTip(label, string.Format(LoadResources.getter("FleetData_10") + "{0}", KCDatabase.Instance.Fleet.AnchorageRepairingTimer));

					return FleetStates.AnchorageRepairing;
				}
			}

			//未補給
			{
				int fuel = fleet.MembersInstance.Sum( ship => ship == null ? 0 : (int)( ( ship.MasterShip.Fuel - ship.Fuel ) * ( ship.IsMarried ? 0.85 : 1.00 ) ) );
				int ammo = fleet.MembersInstance.Sum( ship => ship == null ? 0 : (int)( ( ship.MasterShip.Ammo - ship.Ammo ) * ( ship.IsMarried ? 0.85 : 1.00 ) ) );
				int aircraft = fleet.MembersInstance.Sum(
					ship => {
						if ( ship == null ) return 0;
						else {
							int c = 0;
							for ( int i = 0; i < ship.Slot.Count; i++ ) {
								c += ship.MasterShip.Aircraft[i] - ship.Aircraft[i];
							}
							return c;
						}
					} );
				int bauxite = aircraft * 5;

				if ( fuel > 0 || ammo > 0 || bauxite > 0 ) {

                    label.Text = LoadResources.getter("FleetData_11");
					label.ImageIndex = (int)ResourceManager.IconContent.FleetNotReplenished;

                    tooltip.SetToolTip(label, string.Format(LoadResources.getter("FleetData_12"), fuel, ammo, bauxite, aircraft));

					return FleetStates.NotReplenished;
				}
			}

			//疲労
			{
				int cond = fleet.MembersInstance.Min( s => s == null ? 100 : s.Condition );

				if ( cond < Configuration.Config.Control.ConditionBorder && fleet.ConditionTime != null ) {

					timer = (DateTime)fleet.ConditionTime;


                    label.Text = LoadResources.getter("FleetData_13") + DateTimeHelper.ToTimeRemainString(timer);

					if ( cond < 20 )
						label.ImageIndex = (int)ResourceManager.IconContent.ConditionVeryTired;
					else if ( cond < 30 )
						label.ImageIndex = (int)ResourceManager.IconContent.ConditionTired;
					else
						label.ImageIndex = (int)ResourceManager.IconContent.ConditionLittleTired;


					tooltip.SetToolTip( label, string.Format( LoadResources.getter("FleetData_14"), timer ) );

					return FleetStates.Tired;


				} else if ( cond >= 50 ) {		//戦意高揚

                    label.Text = LoadResources.getter("FleetData_15");
					label.ImageIndex = (int)ResourceManager.IconContent.ConditionSparkle;
                    tooltip.SetToolTip(label, string.Format(LoadResources.getter("FleetData_16"), cond, Math.Ceiling((cond - 49) / 3.0)));
					return FleetStates.Sparkled;

				}

			}

			//出撃可能！
			{
                label.Text = LoadResources.getter("FleetData_17");
				label.ImageIndex = (int)ResourceManager.IconContent.FleetReady;

				return FleetStates.Ready;
			}

		}


		/// <summary>
		/// 艦隊の状態の情報をもとにラベルを更新します。
		/// </summary>
		/// <param name="label">更新するラベル。</param>
		/// <param name="state">艦隊の状態。</param>
		/// <param name="timer">日時。</param>
		public static void RefreshFleetState( ImageLabel label, FleetStates state, DateTime timer ) {

			switch ( state ) {
				case FleetStates.Damaged:
				case FleetStates.SortieDamaged:
					label.BackColor = DateTime.Now.Second % 2 == 0 ? Color.LightCoral : Color.Transparent;
					break;
				case FleetStates.Docking:
                    label.Text = LoadResources.getter("FleetData_18") + DateTimeHelper.ToTimeRemainString(timer);
					break;
				case FleetStates.Expedition:
                    label.Text = LoadResources.getter("FleetData_19") + DateTimeHelper.ToTimeRemainString(timer);
					break;
				case FleetStates.Tired:
                    label.Text = LoadResources.getter("FleetData_20") + DateTimeHelper.ToTimeRemainString(timer);
					break;
				case FleetStates.AnchorageRepairing:
                    label.Text = LoadResources.getter("FleetData_21") + DateTimeHelper.ToTimeElapsedString(KCDatabase.Instance.Fleet.AnchorageRepairingTimer);
					break;
			}

		}


		private bool CanAnchorageRepair {
			get {
				KCDatabase db = KCDatabase.Instance;
				ShipData flagship = db.Ships[Members[0]];
				return (
					ExpeditionState == 0 &&
					flagship != null &&
					flagship.MasterShip.ShipType == 19 &&					//旗艦工作艦
					(double)flagship.HPCurrent / flagship.HPMax > 0.5 &&	//旗艦が中破未満
					flagship.RepairingDockID == -1 &&						//旗艦が入渠中でない
					Members.Take( 2 + flagship.SlotInstanceMaster.Count( eq => eq != null && eq.EquipmentType[2] == 31 ) ).Count( id => {		//(2+装備)以内に50%<HP<100%&&非入渠中の艦がいる
						ShipData ship = db.Ships[id];
						if ( ship == null ) return false;
						if ( ship.RepairingDockID != -1 ) return false;
						double rate = (double)ship.HPCurrent / ship.HPMax;
						return 0.5 < rate && rate < 1.0;
					} ) > 0 );
			}
		}

		public void UpdateAnchorageRepairingState() {

			bool prev = IsAnchorageRepairing;
			IsAnchorageRepairing = CanAnchorageRepair;
			if ( !prev && IsAnchorageRepairing )
				KCDatabase.Instance.Fleet.StartAnchorageRepairing();

		}
	}

}
