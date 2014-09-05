using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 艦船のマスターデータを保持します。
	/// </summary>
	public class ShipDataMaster : IIdentifiable, IResponseLoader {

		/// <summary>
		/// 艦船ID
		/// </summary>
		public int ShipID { get; private set; }

		/// <summary>
		/// 並べ替え順
		/// </summary>
		public int SortID { get; private set; }

		/// <summary>
		/// 名前
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// 読み
		/// </summary>
		public string NameReading { get; private set; }

		/// <summary>
		/// 艦種
		/// </summary>
		public int ShipType { get; private set; }


		/// <summary>
		/// 改装Lv.
		/// </summary>
		public int RemodelAfterLevel { get; private set; }

		//参照にしてもいいかも
		/// <summary>
		/// 改装後の艦船ID
		/// </summary>
		public int RemodelAfterShipID { get; private set; }

		/// <summary>
		/// 改装前の艦船ID
		/// </summary>
		public int RemodelBeforeShipID { get; private set; }

		/// <summary>
		/// 改装に必要な燃料
		/// </summary>
		public int RemodelAmmo { get; private set; }

		/// <summary>
		/// 改装に必要な鋼材
		/// </summary>
		public int RemodelSteel { get; private set; }

		/// <summary>
		/// 改装に改装設計図が必要かどうか
		/// </summary>
		public int NeedBlueprint { get; private set; }


		/// <summary>
		/// パラメータ
		/// </summary>
		public ParameterBase Param { get; private set; }

		/// <summary>
		/// 速力
		/// </summary>
		public ShipSpeed Speed { get; private set; }

		/// <summary>
		/// 装備スロットの数
		/// </summary>
		public int SlotSize { get; private set; }

		private List<int> _aircraft;
		/// <summary>
		/// 各スロットの航空機搭載数
		/// </summary>
		public ReadOnlyCollection<int> Aircraft {
			get { return _aircraft.AsReadOnly(); }
		}

		private List<int> _defaultSlot;
		/// <summary>
		/// 初期装備のID
		/// </summary>
		public ReadOnlyCollection<int> DefaultSlot {
			get { return _defaultSlot.AsReadOnly(); }
		}


		/// <summary>
		/// 建造時間(分)
		/// </summary>
		public int BuildingTime { get; private set; }


		private List<int> _material;
		/// <summary>
		/// 解体資材
		/// </summary>
		public ReadOnlyCollection<int> Material {
			get { return _material.AsReadOnly(); }
		}

		private List<int> _powerup;
		/// <summary>
		/// 近代化改修の素材にしたとき上昇するパラメータの量
		/// </summary>
		public ReadOnlyCollection<int> PowerUp {
			get { return _powerup.AsReadOnly(); }
		}

		/// <summary>
		/// レアリティ
		/// </summary>
		public ShipRarity Rarity { get; private set; }

		/// <summary>
		/// ドロップ/ログイン時のメッセージ
		/// </summary>
		public string MessageGet { get; private set; }

		/// <summary>
		/// 艦船名鑑でのメッセージ
		/// </summary>
		public string MessageDict { get; private set; }

		/// <summary>
		/// 搭載燃料
		/// </summary>
		public int Fuel { get; private set; }
		
		/// <summary>
		/// 搭載弾薬
		/// </summary>
		public int Ammo { get; private set; }



		public ShipDataMaster()
			: this( 0 ) {
		}

		public ShipDataMaster( int id ) {
			ShipID = id;
		}


		public int ID {
			get { return ShipID; }
		}


		public bool LoadFromResponse( string apiname, dynamic data ) {

			ShipID = data.api_id;
			SortID = data.api_sortno;
			Name = data.api_name;
			NameReading = data.api_yomi;
			ShipType = data.api_stype;
			
			RemodelAfterLevel = data.api_afterlv;
			RemodelAfterShipID = int.Parse( data.api_aftershipid );

			Param.HP.Value = data.api_taik[0];
			Param.HP.Max = data.api_taik[1];
			Param.Armor.Value = data.api_souk[0];
			Param.Armor.Max = data.api_souk[1];
			Param.Firepower.Value = data.api_houg[0];
			Param.Firepower.Max = data.api_houg[1];
			Param.Torpedo.Value = data.api_raig[0];
			Param.Torpedo.Max = data.api_raig[1];
			Param.AA.Value = data.api_tyku[0];
			Param.AA.Max = data.api_tyku[1];
			Param.ASW.Value = data.api_tais[0];
			Param.ASW.Max = data.api_tais[1];
			Param.Evasion.Value = data.api_kaih[0];
			Param.Evasion.Max = data.api_kaih[1];
			Param.LOS.Value = data.api_saku[0];
			Param.LOS.Max = data.api_saku[1];
			Param.Luck.Value = data.api_luck[0];
			Param.Luck.Max = data.api_luck[1];
			Speed = (ShipSpeed)( (int)data.api_sokuh );
			Param.Range = (ShipRange)( (int)data.api_leng );

			SlotSize = data.api_slot_num;
			_aircraft = new List<int>( (int[])data.api_maxeq );
			_defaultSlot = new List<int>( (int[])data.api_defeq );

			BuildingTime = data.api_buildtime;
			_material = new List<int>( (int[])data.api_broken );
			_powerup = new List<int>( (int[])data.api_powup );

			Rarity = (ShipRarity)( (int)data.api_backs );

			MessageGet = data.api_getmes;
			MessageDict = data.api_sinfo;

			RemodelSteel = data.api_afterfuel;
			RemodelAmmo = data.api_afterbull;

			Fuel = data.api_fuel_max;
			Ammo = data.api_bull_max;

			return true;
		}

		
	}


}
