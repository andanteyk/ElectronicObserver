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
		public int ExpeditionState { get; internal set;	}

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

		public bool IsInSortie { get; internal set; }



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
				case "api_port/port":
					_escapedShipList.Clear();
					if ( IsInSortie ) {
						Utility.Logger.Add( 2, string.Format( "#{0} {1}が帰投しました。", FleetID, Name ) );
					}
					IsInSortie = false;
					goto default;

				default:			//checkme
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

								//入れ替え
								for ( int i = 0; i < _members.Length; i++ ) {
									if ( _members[i] == shipID ) {
										_members[i] = replacedID;
										break;
									}
								}
								
								//入隊
								_members[index] = shipID;

							}


						} else {

							if ( index != -1 && shipID != -1 ) {
								//入れ替え
								for ( int i = 0; i < _members.Length; i++ ) {
									if ( _members[i] == shipID ) {
										_members[i] = replacedID;
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

			return Calculator.GetAirSuperiority( this );
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

			for ( int i = 0; i < Members.Count; i++ ) {

				if ( Members[i] == -1 )
					continue;

				ShipData ship = db.Ships[Members[i]];
				if ( ship == null || _escapedShipList.Contains( ship.MasterID ) )
					continue;

				los_other += ship.LOSBase;

				var slot = ship.SlotInstanceMaster;

				for ( int j = 0; j < slot.Count; j++ ) {

					if ( slot[j] == null ) continue;

					switch ( slot[j].EquipmentType[2] ) {
						case 9:		//艦偵
						case 10:	//水偵
						case 11:	//水爆
							if ( ship.Aircraft[j] > 0 )
								los_reconplane += slot[j].LOS * 2;
							break;

						case 12:	//小型電探
						case 13:	//大型電探
							los_radar += slot[j].LOS;
							break;

						default:
							los_other += slot[j].LOS;
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

			//memo: 泊地修理は工作艦が中破しているとできない、忘れないよう
				

			KCDatabase db = KCDatabase.Instance;


			//初期化
			tooltip.SetToolTip( label, null );
			label.BackColor = Color.Transparent;


			//所属艦なし
			if ( fleet.Members.Count( id => id != -1 ) == 0 ) {
				label.Text = "所属艦なし";
				label.ImageIndex = (int)ResourceManager.IconContent.FleetNoShip;

				return FleetStates.NoShip;
			}

			{	//入渠中
				long ntime = db.Docks.Values.Max(
						dock => {
							if ( dock.State == 1 && fleet.Members.Count( ( id => id == dock.ShipID ) ) > 0 )
								return dock.CompletionTime.ToBinary();
							else return 0;
						}
						);

				if ( ntime > 0 ) {	//入渠中

					timer = DateTime.FromBinary( ntime );
					label.Text = "入渠中 " + DateTimeHelper.ToTimeRemainString( timer );
					label.ImageIndex = (int)ResourceManager.IconContent.FleetDocking;

					tooltip.SetToolTip( label, "完了日時 : " + timer );

					return FleetStates.Docking;
				}

			}


			if ( fleet.IsInSortie ) {

				//大破出撃中
				if (  fleet.Members.Count( id =>
						( id != -1 && !fleet.EscapedShipList.Contains( id ) && (double)db.Ships[id].HPCurrent / db.Ships[id].HPMax <= 0.25 )
					 ) > 0 ) {

					label.Text = "！！大破進撃中！！";
					label.ImageIndex = (int)ResourceManager.IconContent.FleetSortieDamaged;

					return FleetStates.SortieDamaged;

				} else {	//出撃中

					label.Text = "出撃中";
					label.ImageIndex = (int)ResourceManager.IconContent.FleetSortie;

					return FleetStates.Sortie;
				}

			}
			

			//遠征中
			if ( fleet.ExpeditionState != 0 ) {

				timer = fleet.ExpeditionTime;
				label.Text = "遠征中 " + DateTimeHelper.ToTimeRemainString( timer );
				label.ImageIndex = (int)ResourceManager.IconContent.FleetExpedition;

				tooltip.SetToolTip( label, string.Format( "{0} : {1}\r\n完了日時 : {2}", KCDatabase.Instance.Mission[fleet.ExpeditionDestination].ID, KCDatabase.Instance.Mission[fleet.ExpeditionDestination].Name, timer ) );

				return FleetStates.Expedition;
			}

			//大破艦あり
			if ( fleet.Members.Count( id =>
				( id != -1 && !fleet.EscapedShipList.Contains( id ) && (double)db.Ships[id].HPCurrent / db.Ships[id].HPMax <= 0.25 )
			 ) > 0 ) {

				label.Text = "大破艦あり！";
				label.ImageIndex = (int)ResourceManager.IconContent.FleetDamaged;
				//label.BackColor = Color.LightCoral;

				return FleetStates.Damaged;
			}

			//泊地修理中
			{
				ShipData flagship = db.Ships[fleet.Members[0]];
				if ( flagship != null &&
					flagship.MasterShip.ShipType == 19 &&					//旗艦工作艦
					(double)flagship.HPCurrent / flagship.HPMax > 0.5 &&	//旗艦が中破未満
					flagship.RepairingDockID == -1 &&						//旗艦が入渠中でない
					fleet.Members.Take( 2 + flagship.SlotInstanceMaster.Count( eq => eq != null && eq.EquipmentType[2] == 31 ) ).Count( id => {		//(2+装備)以内に50%<HP<100%&&非入渠中の艦がいる
						ShipData ship = db.Ships[id];
						if ( id == -1 ) return false;
						if ( ship.RepairingDockID != -1 ) return false;
						double rate = (double)ship.HPCurrent / ship.HPMax;
						return 0.5 < rate && rate < 1.0;
					} ) >= 1 ) {

					if ( prevstate != FleetStates.AnchorageRepairing )
						timer = DateTime.Now;
					label.Text = "泊地修理中 " + DateTimeHelper.ToTimeElapsedString( timer );
					label.ImageIndex = (int)ResourceManager.IconContent.FleetAnchorageRepairing;

					tooltip.SetToolTip( label, string.Format( "開始日時 : {0}", timer ) );

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
					} ) * 5;
				int bauxite = aircraft * 5;

				if ( fuel > 0 || ammo > 0 || bauxite > 0 ) {

					label.Text = "未補給";
					label.ImageIndex = (int)ResourceManager.IconContent.FleetNotReplenished;

					tooltip.SetToolTip( label, string.Format( "燃 : {0}\r\n弾 : {1}\r\nボ : {2} ( {3}機 )", fuel, ammo, bauxite, aircraft ) );

					return FleetStates.NotReplenished;
				}
			}

			//疲労
			{
				int cond = fleet.Members.Min( id => id == -1 ? 100 : db.Ships[id].Condition );

				if ( cond < Configuration.Config.Control.ConditionBorder ) {

					DateTime recovertime = DateTime.Now.AddMinutes( (int)Math.Ceiling( ( Configuration.Config.Control.ConditionBorder - cond ) / 3.0 ) * 3 );

					if ( prevstate != FleetStates.Tired || recovertime < timer )
						timer = recovertime;
					label.Text = "疲労 " + DateTimeHelper.ToTimeRemainString( timer );

					if ( cond < 20 )
						label.ImageIndex = (int)ResourceManager.IconContent.ConditionVeryTired;
					else if ( cond < 30 )
						label.ImageIndex = (int)ResourceManager.IconContent.ConditionTired;
					else
						label.ImageIndex = (int)ResourceManager.IconContent.ConditionLittleTired;


					tooltip.SetToolTip( label, string.Format( "回復目安日時 : {0}", timer ) );

					return FleetStates.Tired;



				} else if ( cond >= 50 ) {		//戦意高揚

					label.Text = "戦意高揚！";
					label.ImageIndex = (int)ResourceManager.IconContent.ConditionSparkle;
					tooltip.SetToolTip( label, string.Format( "最低cond: {0}\r\nあと {1} 回遠征可能", cond, Math.Ceiling( ( cond - 49 ) / 3.0 ) ) );
					return FleetStates.Sparkled;

				}

			}

			//出撃可能！
			{
				label.Text = "出撃可能！";
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
					label.Text = "入渠中 " + DateTimeHelper.ToTimeRemainString( timer );
					break;
				case FleetStates.Expedition:
					label.Text = "遠征中 " + DateTimeHelper.ToTimeRemainString( timer );
					break;
				case FleetStates.Tired:
					label.Text = "疲労 " + DateTimeHelper.ToTimeRemainString( timer );
					break;
				case FleetStates.AnchorageRepairing:
					label.Text = "泊地修理中 " + DateTimeHelper.ToTimeElapsedString( timer );
					break;
			}

		}


	}

}
