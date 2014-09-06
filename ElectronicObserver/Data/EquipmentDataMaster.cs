using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 装備のマスターデータを保持します。
	/// </summary>
	public class EquipmentDataMaster : IIdentifiable, IResponseLoader {

		/// <summary>
		/// 装備ID
		/// </summary>
		public int EquipmentID { get; private set; }
		
		/// <summary>
		/// 並べ替え順
		/// </summary>
		public int SortID { get; private set; }
		
		/// <summary>
		/// 名前
		/// </summary>
		public string Name { get; private set; }


		private List<int> _equipmentType;
		/// <summary>
		/// 装備種別
		/// </summary>
		public ReadOnlyCollection<int> EquipmentType {
			get { return _equipmentType.AsReadOnly(); }
		}

		/// <summary>
		/// パラメータ
		/// </summary>
		public ParameterBase Param { get; private set; }

		/// <summary>
		/// レアリティ
		/// </summary>
		public EquipmentRarity Rarity { get; private set; }
		
		private List<int> _material;
		/// <summary>
		/// 廃棄資材
		/// </summary>
		public ReadOnlyCollection<int> Material {
			get { return _material.AsReadOnly(); }
		}
		
		/// <summary>
		/// 図鑑説明
		/// </summary>
		public string Message { get; private set; }



		public EquipmentDataMaster()
			: this( 0 ) {
		}

		public EquipmentDataMaster( int id ) {
			EquipmentID = id;
		}


		public int ID {
			get { return EquipmentID; }
		}

		public bool LoadFromResponse( string apiname, dynamic data ) {

			EquipmentID = data.api_id;
			SortID = data.api_sortno;
			Name = data.api_name;
			_equipmentType = new List<int>( (int[])data.api_type );

			Param.HP.Value = data.api_taik;
			Param.Armor.Value = data.api_souk;
			Param.Firepower.Value = data.api_houg;
			Param.Torpedo.Value = data.api_raig;
			//speed
			Param.Bomber.Value = data.api_baku;
			Param.AA.Value = data.api_tyku;
			Param.ASW.Value = data.api_tais;
			Param.Accuracy.Value = data.api_houm;
			Param.Evasion.Value = data.api_houk;
			Param.LOS.Value = data.api_saku;
			Param.Luck.Value = data.api_luck;
			Param.Range = data.api_leng;

			Rarity = (EquipmentRarity)( (int)data.api_rare );
			_material = new List<int>( (int[])data.api_broken );
			Message = data.api_info;

			return true;
		}

		
	}

}
