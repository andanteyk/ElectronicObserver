using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// 各種資源量を保持します。
	/// </summary>
	public class MaterialData : APIWrapper {

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
			base.LoadFromResponse( apiname, (object)data );			//何か基幹とするデータ構造があった場合、switch文のなかに移動すること

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

				case "api_req_hokyu/charge":
					Fuel = (int)data[0];
					Ammo = (int)data[1];
					Steel = (int)data[2];
					Bauxite = (int)data[3];
					break;

			}
		}


		public override void LoadFromRequest( string apiname, Dictionary<string, string> data ) {
			base.LoadFromRequest( apiname, data );

			switch ( apiname ) {
				case "api_req_kousyou/createship":
					Fuel -= int.Parse( data["api_item1"] );
					Ammo -= int.Parse( data["api_item2"] );
					Steel -= int.Parse( data["api_item3"] );
					Bauxite -= int.Parse( data["api_item4"] );
					DevelopmentMaterial -= int.Parse( data["api_item5"] );
					break;
			}
		}
	}

}
