using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 各種資源量を保持します。
	/// </summary>
	public class MaterialData : ResponseWrapper {

		/// <summary>
		/// 燃料
		/// </summary>
		public int Fuel { get; internal set; }

		/// <summary>
		/// 弾薬
		/// </summary>
		public int Ammo { get; internal set; }

		/// <summary>
		/// 鋼材
		/// </summary>
		public int Steel { get; internal set; }

		/// <summary>
		/// ボーキサイト
		/// </summary>
		public int Bauxite { get; internal set; }


		/// <summary>
		/// 高速建造材
		/// </summary>
		public int InstantConstruction { get; internal set; }

		/// <summary>
		/// 高速修復材
		/// </summary>
		public int InstantRepair { get; internal set; }

		/// <summary>
		/// 開発資材
		/// </summary>
		public int DevelopmentMaterial { get; internal set; }



		public MaterialData()
			: base() {
		}



		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			Fuel = (int)RawData[0].api_value;
			Ammo = (int)RawData[1].api_value;
			Steel = (int)RawData[2].api_value;
			Bauxite = (int)RawData[3].api_value;
			InstantConstruction = (int)RawData[4].api_value;
			InstantRepair = (int)RawData[5].api_value;
			DevelopmentMaterial = (int)RawData[6].api_value;

		}

	}

}
