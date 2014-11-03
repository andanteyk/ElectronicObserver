using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource.SaveData {

	public class ShipParameterData : SaveData {

		/// <summary>
		/// 新規入手艦判別用ボーダー
		/// </summary>
		private int NewShipIDBorder;




		/// <summary>
		/// レベルに比例して上昇するパラメータの最小値と最大値(及びその推測値)を保持します。
		/// </summary>
		public class Parameter {

			/// <summary>
			/// 最小値(推測値)
			/// </summary>
			public int Min {
				get { return ( MinEstMin + MinEstMax ) / 2; }
			}

			/// <summary>
			/// 最大値
			/// </summary>
			public int Max { get; set; }


			/// <summary>
			/// 最小値の推測範囲の下限
			/// </summary>
			public int MinEstMin { get; set; }

			/// <summary>
			/// 最小値の推測範囲の上限
			/// </summary>
			public int MinEstMax { get; set; }


			/// <summary>
			/// 最小値の推測範囲の下限の初期値
			/// </summary>
			public const int MinEstMinDefault = 0;

			/// <summary>
			/// 最小値の推測範囲の上限の初期値
			/// </summary>
			public const int MinEstMaxDefault = 9999;


			public Parameter() {

				MinEstMin = MinEstMinDefault;
				MinEstMax = MinEstMaxDefault;
				Max = MinEstMaxDefault;

			}

		}


		/// <summary>
		/// 各艦船のパラメータを保持します。
		/// </summary>
		public class ShipParameterElement {

			public int ShipID { get; set; }

			//for debug
			//*/
			public string ShipName {
				get {
					ShipDataMaster ship = KCDatabase.Instance.MasterShips[ShipID];
					if ( ship != null )
						return ship.Name + " " + ship.NameReading;
					else
						return null;
				}
			}
			//*/

			public Parameter ASW { get; set; }
			public Parameter Evasion { get; set; }
			public Parameter LOS { get; set; }

			public int[] DefaultSlot { get; set; }

			public ShipParameterElement() {

				ASW = new Parameter();
				Evasion = new Parameter();
				LOS = new Parameter();

				DefaultSlot = null;
			}

		}


		public class InternalData : InternalBaseData {

			[IgnoreDataMember]
			public Dictionary<int, ShipParameterElement> ShipParameters;

			//シリアライズ用
			public List<ShipParameterElement> ShipParametersList {
				get {
					var l = ShipParameters.Values.ToList();
					l.Sort( ( ShipParameterElement e1, ShipParameterElement e2 ) => ( e1.ShipID - e2.ShipID ) );
					return l;
				}
				set {
					ShipParameters = value.ToDictionary( n => n.ShipID );
				}
			}

			public InternalData() {
				ShipParameters = new Dictionary<int, ShipParameterElement>();
			}
		}




		public ShipParameterData()
			: base() {

			DataInstance = new InternalData();
			ParameterLoadFlag = true;
			NewShipIDBorder = -1;


			APIObserver ao = APIObserver.Instance;

			ao.APIList["api_get_member/picture_book"].ResponseReceived += AlbumOpened;

			//戦闘系：最初のフェーズのみ要るから夜戦(≠開幕)は不要
			ao.APIList["api_req_sortie/battle"].ResponseReceived += BattleStart;
			//ao.APIList["api_req_battle_midnight/battle"].ResponseReceived += BattleStart;
			ao.APIList["api_req_battle_midnight/sp_midnight"].ResponseReceived += BattleStart;
			ao.APIList["api_req_combined_battle/battle"].ResponseReceived += BattleStart;
			//ao.APIList["api_req_combined_battle/midnight_battle"].ResponseReceived += BattleStart;
			ao.APIList["api_req_combined_battle/sp_midnight"].ResponseReceived += BattleStart;
			ao.APIList["api_req_combined_battle/airbattle"].ResponseReceived += BattleStart;

			ao.APIList["api_req_map/start"].ResponseReceived += SortieStart;
			ao.APIList["api_port/port"].ResponseReceived += SortieEnd;

			ao.APIList["api_req_kousyou/getship"].ResponseReceived += ConstructionReceived;

		}


		
		
		
		
		public InternalData Data {
			get { return (InternalData)DataInstance; }
			set { DataInstance = value; }
		}


		/// <summary>
		/// パラメータを更新します。
		/// </summary>
		/// <param name="ship">対象の艦船。</param>
		public void UpdateParameter( ShipData ship ) {

			if ( !Data.ShipParameters.ContainsKey( ship.ShipID ) ) {

				ShipParameterElement e = new ShipParameterElement();
				e.ShipID = ship.ShipID;

				e.ASW = SetEstParameter( ship.Level, ship.ASWMax, ship.ASWBase, e.ASW );
				e.Evasion = SetEstParameter( ship.Level, ship.EvasionMax, ship.EvasionBase, e.Evasion );
				e.LOS = SetEstParameter( ship.Level, ship.LOSMax, ship.LOSBase, e.LOS );

				Data.ShipParameters.Add( e.ShipID, e );

			} else {

				ShipParameterElement e = Data.ShipParameters[ship.ShipID];

				e.ASW = SetEstParameter( ship.Level, ship.ASWMax, ship.ASWBase, e.ASW );
				e.Evasion = SetEstParameter( ship.Level, ship.EvasionMax, ship.EvasionBase, e.Evasion );
				e.LOS = SetEstParameter( ship.Level, ship.LOSMax, ship.LOSBase, e.LOS );

			}

		}


		/// <summary>
		/// 初期装備を更新します。
		/// </summary>
		/// <param name="ship">対象の艦船。入手直後・改装直後のものに限ります。</param>
		public void UpdateDefaultSlot( ShipData ship ) {

			int[] slot = new int[ship.Slot.Count];
			for ( int i = 0; i < slot.Length; i++ ) {
				if ( ship.Slot[i] == -1 )
					slot[i] = -1;
				else
					slot[i] = KCDatabase.Instance.Equipments[ship.Slot[i]].EquipmentID;
			}

			UpdateDefaultSlot( ship.ShipID, slot );
		}


		/// <summary>
		/// 初期装備を更新します。
		/// </summary>
		/// <param name="shipID">艦船ID。</param>
		/// <param name="slot">装備スロット配列。</param>
		public void UpdateDefaultSlot( int shipID, int[] slot ) {

			if ( !Data.ShipParameters.ContainsKey( shipID ) ) {

				ShipParameterElement e = new ShipParameterElement();
				e.ShipID = shipID;

				e.DefaultSlot = slot;

				Data.ShipParameters.Add( e.ShipID, e );

			} else {

				ShipParameterElement e = Data.ShipParameters[shipID];
				e.DefaultSlot = slot;

			}

		}


		/// <summary>
		/// レベル依存パラメータの最小値を推測します。
		/// </summary>
		/// <param name="level">レベル。</param>
		/// <param name="max">最大値(Lv. 99におけるパラメータ)。</param>
		/// <param name="value">そのレベルでのパラメータ。</param>
		/// <param name="p">既知のパラメータ。</param>
		/// <returns>推測・修正した値を返します。</returns>
		private Parameter SetEstParameter( int level, int max, int value, Parameter p ) {

			if ( max != Parameter.MinEstMaxDefault )
				p.Max = max;


			if ( level == 1 ) {

				p.MinEstMin = p.MinEstMax = value;

			} else if ( level != 99 ) {

				double p1, p2;
				p1 = ( value - max * level / 99.0 ) / ( 1.0 - level / 99.0 );
				p2 = ( ( value + 1.0 ) - max * level / 99.0 ) / ( 1.0 - level / 99.0 );

				int estmin, estmax;
				estmin = (int)Math.Ceiling( Math.Min( p1, p2 ) );
				estmax = (int)Math.Floor( Math.Max( p1, p2 ) );

				if ( estmin < 0 ) estmin = 0;
				if ( estmin > max ) estmin = max;

				if ( estmax < 0 ) estmax = 0;
				if ( estmax > max ) estmax = max;

				p.MinEstMin = Math.Max( p.MinEstMin, estmin );
				p.MinEstMax = Math.Min( p.MinEstMax, estmax );

			}

			return p;
		}



		private bool _parameterLoadFlag;
		public bool ParameterLoadFlag {
			get { return _parameterLoadFlag; }
			set {

				if ( value ) {

					APIObserver.Instance.APIList["api_port/port"].ResponseReceived -= ParameterLoaded;
					APIObserver.Instance.APIList["api_port/port"].ResponseReceived += ParameterLoaded;

				} else {

					APIObserver.Instance.APIList["api_port/port"].ResponseReceived -= ParameterLoaded;

				}

				_parameterLoadFlag = value;
			}
		}


		/// <summary>
		/// 保有艦船から各パラメータの最大値を読み込み、最小値を推測します。
		/// </summary>
		void ParameterLoaded( string apiname, dynamic data ) {

			foreach ( ShipData ship in KCDatabase.Instance.Ships.Values ) {

				UpdateParameter( ship );

			}

			ParameterLoadFlag = false;		//一回限り(基本的に起動直後の1回)

		}


		/// <summary>
		/// 艦娘図鑑から回避・対潜の初期値を読み込みます。
		/// </summary>
		void AlbumOpened( string apiname, dynamic data ) {

			foreach ( dynamic elem in data.api_list ) {

				if ( !elem.api_yomi() ) break;		//装備図鑑だった場合終了


				int shipID = (int)elem.api_table_id[0];

				int evasion = (int)elem.api_kaih;
				int asw = (int)elem.api_tais;

				if ( !Data.ShipParameters.ContainsKey( shipID ) ) {

					ShipParameterElement e = new ShipParameterElement();
					e.ShipID = shipID;

					e.ASW = SetEstParameter( 1, Parameter.MinEstMaxDefault, asw, e.ASW );
					e.Evasion = SetEstParameter( 1, Parameter.MinEstMaxDefault, evasion, e.Evasion );

					Data.ShipParameters.Add( e.ShipID, e );

				} else {

					ShipParameterElement e = Data.ShipParameters[shipID];

					e.ASW = SetEstParameter( 1, Parameter.MinEstMaxDefault, asw, e.ASW );
					e.Evasion = SetEstParameter( 1, Parameter.MinEstMaxDefault, evasion, e.Evasion );

				}

			}

		}


		/// <summary>
		/// 戦闘開始時の情報から敵艦の装備を読み込みます。
		/// </summary>
		void BattleStart( string apiname, dynamic data ) {

			int[] efleet = (int[])data.api_ship_ke;


			//[0]はダミー(-1)
			for ( int i = 1; i < efleet.Length; i++ ) {
				if ( efleet[i] == -1 ) continue;

				UpdateDefaultSlot( efleet[i], (int[])( data.api_eSlot[i - 1] ) );
			}

		}


		/// <summary>
		/// 出撃開始時の最新艦を記録します。SortieEndで使用されます。
		/// </summary>
		void SortieStart( string apiname, dynamic data ) {

			NewShipIDBorder = KCDatabase.Instance.Ships.Max( ( KeyValuePair<int, ShipData> s ) => s.Value.MasterID );

		}


		/// <summary>
		/// 艦隊帰投時に新規入手艦を取得、情報を登録します。
		/// </summary>
		void SortieEnd( string apiname, dynamic data ) {

			if ( NewShipIDBorder == -1 ) return;


			foreach ( ShipData s in KCDatabase.Instance.Ships.Values.Where( ( ShipData s ) => s.MasterID > NewShipIDBorder ) ) {

				UpdateParameter( s );
				UpdateDefaultSlot( s );

			}

			NewShipIDBorder = -1;
		}


		/// <summary>
		/// 艦船建造時に新規入手艦を取得、情報を登録します。
		/// </summary>
		void ConstructionReceived( string apiname, dynamic data ) {

			int shipID = (int)data.api_ship_id;
			ShipData ship = KCDatabase.Instance.Ships.Values.FirstOrDefault( ( ShipData s ) => s.MasterID == shipID );

			if ( ship != null ) {
				UpdateParameter( ship );
				UpdateDefaultSlot( ship );
			}

		}





		public override string SaveFileName {
			get { return "ShipParameterData.json"; }
		}
	}

}
