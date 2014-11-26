using ElectronicObserver.Resource;
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
		public int ExpeditionState { get; internal set;	}

		/// <summary>
		/// 遠征先ID
		/// </summary>
		public int ExpeditionDestination { get; internal set; }

		/// <summary>
		/// 遠征帰投時間
		/// </summary>
		public DateTime ExpeditionTime { get; internal set; }


		private int[] _fleetMember;
		/// <summary>
		/// 艦隊メンバー
		/// </summary>
		public ReadOnlyCollection<int> FleetMember {
			get { return Array.AsReadOnly<int>( _fleetMember ); }
		}


		public int this[int i] {
			get {
				return _fleetMember[i];
			}
		}


		private List<int> _escapedShipID = new List<int>();
		/// <summary>
		/// 退避艦のIDリスト
		/// </summary>
		public ReadOnlyCollection<int> EscapedShipID {
			get { return _escapedShipID.AsReadOnly(); }
		}


		public int ID {
			get { return FleetID; }
		}



		public override void LoadFromResponse( string apiname, dynamic data ) {
		
			switch ( apiname ) {
				/*
				case "api_req_mission/start":
					ExpeditionTime = DateTimeHelper.FromAPITime( (long)data.api_complatetime );	
					break;
				*/
				default:			//checkme
					base.LoadFromResponse( apiname, (object)data );

					Name = (string)RawData.api_name;
					_fleetMember = (int[])RawData.api_ship;
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


						if ( FleetID == fleetID ) {
							if ( index == -1 ) {
								//旗艦以外全解除
								for ( int i = 1; i < _fleetMember.Length; i++ )
									_fleetMember[i] = -1;

							} else if ( shipID == -1 ) {
								//はずす
								RemoveShip( index );

							} else {
								//入隊

								//入れ替え
								for ( int i = 0; i < _fleetMember.Length; i++ ) {
									if ( _fleetMember[i] == shipID ) {
										_fleetMember[i] = replacedID;
										break;
									}
								}
								
								//入隊
								_fleetMember[index] = shipID;

							}


						} else {

							if ( index != -1 && shipID != -1 ) {
								//入れ替え
								for ( int i = 0; i < _fleetMember.Length; i++ ) {
									if ( _fleetMember[i] == shipID ) {
										_fleetMember[i] = replacedID;
										break;
									}
								}

							}

						}

					} break;


				case "api_req_kousyou/destroyship": {
						int shipID = int.Parse( data["api_ship_id"] );

						for ( int i = 0; i < _fleetMember.Length; i++ ) {
							if ( _fleetMember[i] == shipID ) {
								RemoveShip( i );
								break;
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


		private void RemoveShip( int index ) {

			for ( int i = index + 1; i < _fleetMember.Length; i++ )
				_fleetMember[i - 1] = _fleetMember[i];

			_fleetMember[_fleetMember.Length - 1] = -1;

		}


		/// <summary>
		/// 制空戦力を取得します。
		/// </summary>
		/// <returns>制空戦力。</returns>
		public int GetAirSuperiority() {

			int airSuperiority = 0;

			for ( int i = 0; i < FleetMember.Count; i++ ) {

				if ( FleetMember[i] == -1 )
					continue;

				ShipData ship = KCDatabase.Instance.Ships[FleetMember[i]];
				for ( int j = 0; j < ship.Slot.Count; j++ ) {

					if ( ship.Slot[j] == -1 )
						continue;

					EquipmentDataMaster eq = KCDatabase.Instance.Equipments[ship.Slot[j]].MasterEquipment;

					switch ( eq.EquipmentType[2] ) {
						case 6:		//艦戦
						case 7:		//艦爆
						case 8:		//艦攻
						case 11:	//水爆
							airSuperiority += (int)( eq.AA * Math.Sqrt( ship.Aircraft[j] ) );
							break;
					}
				}
			}

			return airSuperiority;
		}


		/// <summary>
		/// 索敵能力を取得します。
		/// いわゆる 2-5式 計算法です。正確ではありませんが、参考にはなります。
		/// </summary>
		/// <returns>索敵能力。</returns>
		public int GetSearchingAbility() {

			KCDatabase db = KCDatabase.Instance;

			int los_reconplane = 0;
			int los_radar = 0;
			int los_other = 0;

			for ( int i = 0; i < FleetMember.Count; i++ ) {

				if ( FleetMember[i] == -1 )
					continue;

				ShipData ship = db.Ships[FleetMember[i]];

				los_other += ship.LOSBase;

				for ( int j = 0; j < ship.Slot.Count; j++ ) {

					if ( ship.Slot[j] == -1 )
						continue;

					EquipmentDataMaster eq = db.Equipments[ship.Slot[j]].MasterEquipment;

					switch ( eq.EquipmentType[2] ) {
						case 9:		//艦偵
						case 10:	//水偵
						case 11:	//水爆
							if ( ship.Aircraft[j] > 0 )
								los_reconplane += eq.LOS * 2;
							break;

						case 12:	//小型電探
						case 13:	//大型電探
							los_radar += eq.LOS;
							break;

						default:
							los_other += eq.LOS;
							break;
					}
				}
			}


			return (int)Math.Sqrt( los_other ) + los_radar + los_reconplane;
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
			AnchorageRepairing,
			Ready,
		}


		/// <summary>
		/// 艦隊の状態の情報をラベルに適用します。
		/// </summary>
		/// <param name="fleet">艦隊データ。</param>
		/// <param name="label">適用するラベル。</param>
		/// <param name="tooltip">適用するツールチップ。</param>
		/// <param name="timer">日時。</param>
		/// <returns>艦隊の状態を表す定数。</returns>
		public static FleetStates UpdateFleetState( FleetData fleet, ImageLabel label, ToolTip tooltip, ref DateTime timer ) {

			//memo: 泊地修理は工作艦が中破しているとできない、忘れないよう
				

			KCDatabase db = KCDatabase.Instance;


			//初期化
			tooltip.SetToolTip( label, null );
			label.BackColor = Color.Transparent;


			//所属艦なし
			if ( fleet.FleetMember.Count( id => id != -1 ) == 0 ) {
				label.Text = "所属艦なし";
				label.ImageIndex = (int)ResourceManager.IconContent.HQNoShip;

				return FleetStates.NoShip;
			}

			{	//入渠中
				long ntime = db.Docks.Values.Max(
						dock => {
							if ( dock.State == 1 && fleet.FleetMember.Count( ( id => id == dock.ShipID ) ) > 0 )
								return dock.CompletionTime.ToBinary();
							else return 0;
						}
						);

				if ( ntime > 0 ) {	//入渠中

					timer = DateTime.FromBinary( ntime );
					label.Text = "入渠中 " + DateTimeHelper.ToTimeRemainString( timer );
					label.ImageIndex = (int)ResourceManager.IconContent.HQDock;

					tooltip.SetToolTip( label, "完了日時 : " + timer );

					return FleetStates.Docking;
				}

			}


			//undone:大破出撃中

			//undone:出撃中


			//遠征中
			if ( fleet.ExpeditionState != 0 ) {

				timer = fleet.ExpeditionTime;
				label.Text = "遠征中 " + DateTimeHelper.ToTimeRemainString( timer );
				label.ImageIndex = (int)ResourceManager.IconContent.HQExpedition;

				tooltip.SetToolTip( label, string.Format( "{0}\r\n完了日時 : {1}", KCDatabase.Instance.Mission[fleet.ExpeditionDestination].Name, timer ) );

				return FleetStates.Expedition;
			}

			//大破艦あり
			if ( fleet.FleetMember.Count( id =>
				( id != -1 && (double)db.Ships[id].HPCurrent / db.Ships[id].HPMax <= 0.25 )
			 ) > 0 ) {

				label.Text = "大破艦あり！";
				label.ImageIndex = (int)ResourceManager.IconContent.ShipStateDamageL;
				label.BackColor = Color.LightCoral;

				return FleetStates.Damaged;
			}

			//未補給
			{
				int fuel = fleet.FleetMember.Sum( id => id == -1 ? 0 : db.Ships[id].MasterShip.Fuel - db.Ships[id].Fuel );
				int ammo = fleet.FleetMember.Sum( id => id == -1 ? 0 : db.Ships[id].MasterShip.Ammo - db.Ships[id].Ammo );
				int bauxite = fleet.FleetMember.Sum(
					id => {
						if ( id == -1 ) return 0;
						else {
							int c = 0;
							for ( int i = 0; i < db.Ships[id].Slot.Count; i++ ) {
								c += db.Ships[id].MasterShip.Aircraft[i] - db.Ships[id].Aircraft[i];
							}
							return c;
						}
					} ) * 5;

				if ( fuel > 0 || ammo > 0 || bauxite > 0 ) {

					label.Text = "未補給";
					label.ImageIndex = (int)ResourceManager.IconContent.HQNotReplenished;

					tooltip.SetToolTip( label, string.Format( "燃 : {0}\r\n弾 : {1}\r\nボ : {2}", fuel, ammo, bauxite ) );

					return FleetStates.NotReplenished;
				}
			}

			//疲労
			{
				int cond = fleet.FleetMember.Min( id => id == -1 ? 100 : db.Ships[id].Condition );

				if ( cond < 40 ) {

					timer = DateTime.Now.AddMinutes( (int)Math.Ceiling( ( 40.0 - cond ) / 3.0 ) * 3 );		//todo: いずれ変数化できるようになるといいかも
					label.Text = "疲労 " + DateTimeHelper.ToTimeRemainString( timer );

					if ( cond < 20 )
						label.ImageIndex = (int)ResourceManager.IconContent.ConditionVeryTired;
					else if ( cond < 30 )
						label.ImageIndex = (int)ResourceManager.IconContent.ConditionTired;
					else
						label.ImageIndex = (int)ResourceManager.IconContent.ConditionLittleTired;


					tooltip.SetToolTip( label, string.Format( "回復目安日時 : {0}", timer ) );

					return FleetStates.Tired;
				}
			}


			//undone:泊地修理中


			//出撃可能！
			{
				label.Text = "出撃可能！";
				label.ImageIndex = (int)ResourceManager.IconContent.HQShip;

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
					label.BackColor = DateTime.Now.Second % 2 == 0 ? Color.LightCoral : Color.Transparent;
					break;
				case FleetStates.Docking:
					label.Text = "入渠中 " + DateTimeHelper.ToTimeRemainString( timer );
					break;
				case FleetStates.Expedition:
					label.Text = "遠征中 " + DateTimeHelper.ToTimeRemainString( timer );
					break;
				case FleetStates.Tired:
					label.Text = "疲労 " + DateTimeHelper.ToTimeRemainString( timer );
					break;
			}

		}


	}

}
