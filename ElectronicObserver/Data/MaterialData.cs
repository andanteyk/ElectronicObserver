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
		public int Fuel { get; private set; }

		/// <summary>
		/// 弾薬
		/// </summary>
		public int Ammo { get; private set; }

		/// <summary>
		/// 鋼材
		/// </summary>
		public int Steel { get; private set; }

		/// <summary>
		/// ボーキサイト
		/// </summary>
		public int Bauxite { get; private set; }


		/// <summary>
		/// 高速建造材
		/// </summary>
		public int InstantConstruction { get; private set; }

		/// <summary>
		/// 高速修復材
		/// </summary>
		public int InstantRepair { get; private set; }

		/// <summary>
		/// 開発資材
		/// </summary>
		public int DevelopmentMaterial { get; private set; }



		public MaterialData()
			: base() {
			Fuel = Ammo = Steel = Bauxite = 0;
			InstantConstruction = 0;
			InstantRepair = 0;
			DevelopmentMaterial = 0;
		}

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			switch ( apiname ) {
				case "api_port/port":
					Fuel = (int)RawData[0].api_value;
					Ammo = (int)RawData[1].api_value;
					Steel = (int)RawData[2].api_value;
					Bauxite = (int)RawData[3].api_value;
					InstantConstruction = (int)RawData[4].api_value;
					InstantRepair = (int)RawData[5].api_value;
					DevelopmentMaterial = (int)RawData[6].api_value;
					break;

			}
		}
	}

}
