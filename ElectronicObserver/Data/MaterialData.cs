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
		public int Fuel {
			get { return (int)RawData[0].api_value; }
		}

		/// <summary>
		/// 弾薬
		/// </summary>
		public int Ammo {
			get { return (int)RawData[1].api_value; }
		}

		/// <summary>
		/// 鋼材
		/// </summary>
		public int Steel {
			get { return (int)RawData[2].api_value; }
		}

		/// <summary>
		/// ボーキサイト
		/// </summary>
		public int Bauxite {
			get { return (int)RawData[3].api_value; }
		}


		/// <summary>
		/// 高速建造材
		/// </summary>
		public int InstantConstruction {
			get { return (int)RawData[4].api_value; }
		}

		/// <summary>
		/// 高速修復材
		/// </summary>
		public int InstantRepair {
			get { return (int)RawData[5].api_value; }
		}

		/// <summary>
		/// 開発資材
		/// </summary>
		public int DevelopmentMaterial {
			get { return (int)RawData[6].api_value; }
		}



		public MaterialData()
			: base() {
		}


		//todo: このあたり、普通に変数に追加したほうがいいかも？あまりdynamicに書き換えるのは…

		/// <summary>
		/// 資源を増加させます。
		/// </summary>
		/// <param name="materialID">資源ID。0~3=燃弾鋼ボ, 4=高速建造, 5=高速修復, 6=開発資材</param>
		/// <param name="value">増分。</param>
		public void Increment( int materialID, int value ) {

			RawData[materialID].api_value += value;
		}

		/// <summary>
		/// 資源を減少させます。
		/// </summary>
		/// <param name="materialID">資源ID。0~3=燃弾鋼ボ, 4=高速建造, 5=高速修復, 6=開発資材</param>
		/// <param name="value">減分。</param>
		public void Decrement( int materialID, int value ) {

			RawData[materialID].api_value -= value;
		}

	}

}
