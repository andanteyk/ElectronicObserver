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
		/// レベルに比例して上昇するパラメータの最小値と最大値(及びその推測値)を保持します。
		/// </summary>
		public class Parameter {

			[IgnoreDataMember]
			public int Min {
				get { return ( MinEstMin + MinEstMax ) / 2; }
			}

			public int Max { get; set; }

			public int MinEstMin { get; set; }
			public int MinEstMax { get; set; }

			public Parameter() {

				//てきとう
				MinEstMin = 0;
				MinEstMax = 9999;
				Max = 9999;

			}

		}


		/// <summary>
		/// 各艦船のパラメータを保持します。
		/// </summary>
		public class ShipParameterElement {

			public int ShipID { get; set; }

			public string ShipName {
				get { return KCDatabase.Instance.MasterShips[ShipID].Name; }
			}

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

			p.Max = max;

			if ( level != 99 ) {

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

				if ( value )
					APIObserver.Instance.ResponseList["api_port/port"].ResponseReceived += ParameterLoaded;
				else
					APIObserver.Instance.ResponseList["api_port/port"].ResponseReceived -= ParameterLoaded;

				_parameterLoadFlag = value;
			}
		}

		void ParameterLoaded( string apiname, dynamic data ) {

			foreach ( ShipData ship in KCDatabase.Instance.Ships.Values ) {

				UpdateParameter( ship );

			}

			ParameterLoadFlag = false;		//checkme

		}




		public override string SaveFileName {
			get { return "ShipParameterData.json"; }
		}
	}

}
