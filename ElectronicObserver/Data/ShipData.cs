using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 個別の艦娘データを保持します。
	/// </summary>
	public class ShipData : IIdentifiable, IResponseLoader {

		//マスターデータか何かに定義しておくといいかも
		//public static const int SlotMax = 5;


		/// <summary>
		/// 艦娘を一意に識別するID
		/// </summary>
		public int MasterID { get; private set; }

		/// <summary>
		/// 並べ替えの順番
		/// </summary>
		public int SortID { get; private set; }

		/// <summary>
		/// 艦船ID
		/// </summary>
		public int ShipID { get; private set; }

		/// <summary>
		/// レベル
		/// </summary>
		public int Level { get; private set; }

		/// <summary>
		/// 累積経験値
		/// </summary>
		public int ExpTotal { get; private set; }

		/// <summary>
		/// 次のレベルに達するために必要な経験値
		/// </summary>
		public int ExpNext { get; private set; }

		/// <summary>
		/// パラメータ
		/// </summary>
		public ParameterBase Param { get; private set; }


		private List<int> _slot;
		/// <summary>
		/// 装備スロット
		/// </summary>
		public ReadOnlyCollection<int> Slot {
			get { return _slot.AsReadOnly(); }
		}
		
		private List<int> _aircraft;
		/// <summary>
		/// 各スロットの航空機搭載量
		/// </summary>
		public ReadOnlyCollection<int> Aircraft {
			get { return _aircraft.AsReadOnly(); }
		}
		

		/// <summary>
		/// 搭載燃料
		/// </summary>
		public Fraction Fuel { get; private set; }

		/// <summary>
		/// 搭載弾薬
		/// </summary>
		public Fraction Ammo { get; private set; }

		/// <summary>
		/// 入渠にかかる時間
		/// </summary>
		public int RepairTime { get; private set; }

		/// <summary>
		/// 入渠にかかる鋼材
		/// </summary>
		public int RepairSteel { get; private set; }
		
		/// <summary>
		/// 入渠にかかる燃料
		/// </summary>
		public int RepairFuel { get; private set; }

		/// <summary>
		/// コンディション
		/// </summary>
		public int Condition { get; private set; }

		/// <summary>
		/// 保護ロックの有無
		/// </summary>
		public bool IsLocked { get; private set; }

		/*/
		/// <summary>
		/// 出撃海域
		/// 備考：2014/09/02現在、この値は削除されています
		/// </summary>
		public int SallyArea { get; private set; }
		//*/



		public ShipData()
			: this( 0 ){
		}

		public ShipData( int id ) {
			MasterID = id;
		}



		public int ID {
			get { return MasterID; }
		}


		public bool LoadFromResponse( string apiname, dynamic data ) {

			MasterID = data.api_id;
			SortID = data.api_sortno;
			ShipID = data.api_ship_id;
			Level = data.api_lv;
			ExpTotal = data.api_exp[0];
			ExpNext = data.api_exp[1];
			
			Param.HP.Value = data.api_nowhp;
			Param.HP.Max = data.api_maxhp;
			Param.Range = data.leng;

			_slot = new List<int>( (int[])data.api_slot );
			_aircraft = new List<int>( (int[])data.api_onslot );

			Fuel = data.api_fuel;
			Ammo = data.api_bull;

			RepairTime = data.api_ndock_time;
			RepairFuel = data.api_ndock_item[0];
			RepairSteel = data.api_ndock_item[1];

			Condition = data.api_cond;

			Param.Firepower.Value = data.api_karyoku[0];
			Param.Torpedo.Value = data.api_raisou[0];
			Param.AA.Value = data.api_taiku[0];
			Param.Armor.Value = data.api_soukou[0];
			Param.Evasion.Value = data.api_kaihi[0];
			Param.ASW.Value = data.api_taisen[0];
			Param.LOS.Value = data.api_sakuteki[0];
			Param.Luck.Value = data.api_lucky[0];

			IsLocked = data.api_locked != 0;

			return true;
		}


	}


}

