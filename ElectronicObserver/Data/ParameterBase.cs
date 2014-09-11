using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {


	[Obsolete( "開発初期の名残です", false )]
	public class ParameterBase {

		/// <summary>
		/// 耐久
		/// </summary>
		public Fraction HP { get; set; }

		/// <summary>
		/// 装甲
		/// </summary>
		public Fraction Armor { get; set; }

		/// <summary>
		/// 火力
		/// </summary>
		public Fraction Firepower { get; set; }

		/// <summary>
		/// 雷装
		/// </summary>
		public Fraction Torpedo { get; set; }

		/// <summary>
		/// 爆装
		/// </summary>
		public Fraction Bomber { get; set; }

		/// <summary>
		/// 対空
		/// </summary>
		public Fraction AA { get; set; }

		/// <summary>
		/// 対潜
		/// </summary>
		public Fraction ASW { get; set; }

		/// <summary>
		/// 命中
		/// </summary>
		public Fraction Accuracy { get; set; }

		/// <summary>
		/// 回避
		/// </summary>
		public Fraction Evasion { get; set; }

		/// <summary>
		/// 索敵
		/// </summary>
		public Fraction LOS { get; set; }

		/// <summary>
		/// 運
		/// </summary>
		public Fraction Luck { get; set; }

		/// <summary>
		/// 射程
		/// </summary>
		public ShipRange Range { get; set; }

	}

}
