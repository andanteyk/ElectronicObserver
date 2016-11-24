using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 基地航空隊のデータを扱います。
	/// </summary>
	public class BaseAirCorpsData : APIWrapper, IIdentifiable {


		/// <summary>
		/// 飛行場が存在する海域ID
		/// </summary>
		public int MapAreaID {
			get {
				return RawData.api_area_id() ? (int)RawData.api_area_id : -1;
			}
		}

		/// <summary>
		/// 航空隊ID
		/// </summary>
		public int AirCorpsID {
			get {
				return (int)RawData.api_rid;
			}
		}

		/// <summary>
		/// 航空隊名
		/// </summary>
		public string Name {
			get {
				return RawData.api_name;
			}
			private set {
				RawData.api_name = value;
			}
		}

		/// <summary>
		/// 戦闘行動半径
		/// </summary>
		public int Distance {
			get {
				return (int)RawData.api_distance;
			}
			private set {
				RawData.api_distance = value;
			}
		}

		/// <summary>
		/// 行動指示
		/// 0=待機, 1=出撃, 2=防空, 3=退避, 4=休息
		/// </summary>
		public int ActionKind {
			get {
				return (int)RawData.api_action_kind;
			}
			private set {
				RawData.api_action_kind = value;
			}
		}


		/// <summary>
		/// 航空中隊情報
		/// </summary>
		public IDDictionary<BaseAirCorpsSquadron> Squadrons { get; private set; }

		public BaseAirCorpsSquadron this[int i] {
			get {
				return Squadrons[i];
			}
		}

		/// <summary>
		/// 配置転換中の装備固有IDリスト
		/// </summary>
		public static HashSet<int> RelocatedEquipments { get; private set; }


		static BaseAirCorpsData() {
			RelocatedEquipments = new HashSet<int>();
		}


		public BaseAirCorpsData() {
			Squadrons = new IDDictionary<BaseAirCorpsSquadron>();
		}


		public override void LoadFromRequest( string apiname, Dictionary<string, string> data ) {
			base.LoadFromRequest( apiname, data );

			switch ( apiname ) {
				case "api_req_air_corps/change_name":
					Name = data["api_name"];
					break;

				case "api_req_air_corps/set_action": {

						int[] ids = data["api_base_id"].Split( ",".ToCharArray() ).Select( s => int.Parse( s ) ).ToArray();
						int[] actions = data["api_action_kind"].Split( ",".ToCharArray() ).Select( s => int.Parse( s ) ).ToArray();

						int index = Array.IndexOf( ids, AirCorpsID );

						if ( index >= 0 ) {
							ActionKind = actions[index];
						}

					} break;
			}
		}

		public override void LoadFromResponse( string apiname, dynamic data ) {

			switch ( apiname ) {
				case "api_get_member/base_air_corps":
				default:
					base.LoadFromResponse( apiname, (object)data );

					SetSquadrons( apiname, data.api_plane_info );
					break;

				case "api_req_air_corps/set_plane": {
						var prev = Squadrons.Values.Select( sq => sq != null ? sq.EquipmentMasterID : 0 ).ToArray();
						SetSquadrons( apiname, data.api_plane_info );

						foreach ( var deleted in prev.Except( Squadrons.Values.Select( sq => sq != null && sq.State == 1 ? sq.EquipmentMasterID : 0 ) ) ) {
							var eq = KCDatabase.Instance.Equipments[deleted];

							if ( eq != null ) {
								eq.RelocatedTime = DateTime.Now;
								BaseAirCorpsData.RelocatedEquipments.Add( deleted );
							}
						}

						Distance = (int)data.api_distance;
					} break;

				case "api_req_air_corps/supply":
					SetSquadrons( apiname, data.api_plane_info );
					break;
			}
		}

		private void SetSquadrons( string apiname, dynamic data ) {

			foreach ( var elem in data ) {

				int id = (int)elem.api_squadron_id;

				if ( !Squadrons.ContainsKey( id ) ) {
					var a = new BaseAirCorpsSquadron();
					a.LoadFromResponse( apiname, elem );
					Squadrons.Add( a );

				} else {
					Squadrons[id].LoadFromResponse( apiname, elem );
				}
			}
		}


		public static void SetRelocatedEquipments( IEnumerable<int> values ) {
			if ( values != null )
				RelocatedEquipments = new HashSet<int>( values );
		}


		public override string ToString() {
			return string.Format( "[{0}:{1}] {2}", MapAreaID, AirCorpsID, Name );
		}


		public int ID {
			get { return GetID( RawData ); }
		}


		public static int GetID( int mapAreaID, int airCorpsID ) {
			return mapAreaID * 10 + airCorpsID;
		}
		public static int GetID( Dictionary<string, string> request ) {
			return GetID( int.Parse( request["api_area_id"] ), int.Parse( request["api_base_id"] ) );
		}
		public static int GetID( dynamic response ) {
			return GetID( response.api_area_id() ? (int)response.api_area_id : -1, (int)response.api_rid );
		}

	}
}
