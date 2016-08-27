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

		/// <summary>
		/// 改修資材
		/// </summary>
		public int ModdingMaterial { get; internal set; }




		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );			//何か基幹とするデータ構造があった場合、switch文のなかに移動すること

			switch ( apiname ) {
				case "api_port/port":
				case "api_get_member/material":
					Fuel = (int)data[0].api_value;
					Ammo = (int)data[1].api_value;
					Steel = (int)data[2].api_value;
					Bauxite = (int)data[3].api_value;
					InstantConstruction = (int)data[4].api_value;
					InstantRepair = (int)data[5].api_value;
					DevelopmentMaterial = (int)data[6].api_value;
					ModdingMaterial = (int)data[7].api_value;
					break;

				case "api_req_hokyu/charge":
				case "api_req_kousyou/destroyship":
					Fuel = (int)data[0];
					Ammo = (int)data[1];
					Steel = (int)data[2];
					Bauxite = (int)data[3];
					break;

				case "api_req_kousyou/destroyitem2":
					Fuel += (int)data.api_get_material[0];
					Ammo += (int)data.api_get_material[1];
					Steel += (int)data.api_get_material[2];
					Bauxite += (int)data.api_get_material[3];
					break;

				case "api_req_kousyou/createitem":
				case "api_req_kousyou/remodel_slot":
					Fuel = (int)data[0];
					Ammo = (int)data[1];
					Steel = (int)data[2];
					Bauxite = (int)data[3];
					InstantConstruction = (int)data[4];
					InstantRepair = (int)data[5];
					DevelopmentMaterial = (int)data[6];
					ModdingMaterial = (int)data[7];
					break;

				case "api_req_air_corps/supply":
					Fuel = (int)data.api_after_fuel;
					Bauxite = (int)data.api_after_bauxite;
					break;

				case "api_req_air_corps/set_plane":
					if ( data.api_after_bauxite() )
						Bauxite = (int)data.api_after_bauxite;
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
