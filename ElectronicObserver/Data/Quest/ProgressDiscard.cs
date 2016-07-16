using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest {

	/// <summary>
	/// 装備廃棄任務の進捗を管理します。
	/// </summary>
	[DataContract( Name = "ProgressDiscard" )]
	public class ProgressDiscard : ProgressData {

		/// <summary>
		/// 廃棄した個数をベースにカウントするか
		/// false = 回数、true = 個数
		/// </summary>
		[DataMember]
		private bool CountsAmount { get; set; }

		/// <summary>
		/// 対象となる装備カテゴリ
		/// null ならすべての装備を対象とする
		/// </summary>
		[DataMember]
		private HashSet<int> Categories { get; set; }



		public ProgressDiscard( QuestData quest, int maxCount, bool countsAmount, int[] categories )
			: base( quest, maxCount ) {

			CountsAmount = countsAmount;
			Categories = categories == null ? null : new HashSet<int>( categories );
		}


		public void Increment( IEnumerable<int> equipments ) {
			if ( !CountsAmount ) {
				Increment();
				return;
			}

			if ( Categories == null ) {
				foreach ( var i in equipments )
					Increment();
				return;
			}

			foreach ( var i in equipments ) {
				var eq = KCDatabase.Instance.Equipments[i];

				if ( Categories.Contains( eq.MasterEquipment.CategoryType ) )
					Increment();
			}
		}



		public override string GetClearCondition() {
			return ( Categories == null ? "" : string.Join( "・", Categories.OrderBy( s => s ).Select( s => KCDatabase.Instance.EquipmentTypes[s].Name ) ) ) + "廃棄" + ProgressMax + ( CountsAmount ? "個" : "回" );
		}

	}
}
