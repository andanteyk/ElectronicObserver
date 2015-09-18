using ElectronicObserver.Data.ShipGroup;
using ElectronicObserver.Utility.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ElectronicObserver.Data {


	/// <summary>
	/// 艦船グループのデータを保持します。
	/// </summary>
	[DataContract( Name = "ShipGroupData" )]
	[DebuggerDisplay( "[{GroupID}] : {Name} ({Members.Count} ships)" )]
	public class ShipGroupData : IIdentifiable {


		// fixme: アクセサは要検討

		/// <summary>
		/// 列のプロパティを保持します。
		/// </summary>
		[DataContract( Name = "ViewColumnData" )]
		public class ViewColumnData : IIdentifiable, ICloneable {

			/// <summary>
			/// 処理上の順番
			/// </summary>
			[DataMember]
			public int Index { get; set; }

			/// <summary>
			/// 幅
			/// </summary>
			[DataMember]
			public int Width { get; set; }

			/// <summary>
			/// 表示される順番
			/// </summary>
			[DataMember]
			public int DisplayIndex { get; set; }

			/// <summary>
			/// 可視かどうか
			/// </summary>
			[DataMember]
			public bool Visible { get; set; }

			/// <summary>
			/// 自動幅調整を行うか
			/// </summary>
			public bool AutoSize { get; set; }



			public ViewColumnData( int index ) {
				Index = index;
			}

			public ViewColumnData( int index, int width, int displayIndex, bool visible, bool autoSize )
				: this( index ) {
				Width = width;
				DisplayIndex = displayIndex;
				Visible = visible;
				AutoSize = autoSize;
			}

			public ViewColumnData( DataGridViewColumn column ) {
				FromColumn( column );
			}


			public void ToColumn( DataGridViewColumn column ) {
				if ( column.Index != Index )
					throw new ArgumentException( "設定する列と Index が異なります。" );

				column.Width = Width;
				column.DisplayIndex = DisplayIndex;
				column.Visible = Visible;
				column.AutoSizeMode = AutoSize ? DataGridViewAutoSizeColumnMode.AllCellsExceptHeader : DataGridViewAutoSizeColumnMode.NotSet;
			}

			public ViewColumnData FromColumn( DataGridViewColumn column ) {
				Index = column.Index;
				Width = column.Width;
				DisplayIndex = column.DisplayIndex;
				Visible = column.Visible;
				AutoSize = column.AutoSizeMode == DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
				return this;
			}

			public int ID {
				get { return Index; }
			}

			public ViewColumnData Clone() {
				return (ViewColumnData)MemberwiseClone();
			}

			object ICloneable.Clone() {
				return Clone();
			}
		}




		/// <summary>
		/// グループID
		/// </summary>
		[DataMember]
		public int GroupID { get; internal set; }


		/// <summary>
		/// グループ名
		/// </summary>
		[DataMember]
		public string Name { get; set; }


		/// <summary>
		/// 列の設定
		/// </summary>
		[IgnoreDataMember]
		public IDDictionary<ViewColumnData> ViewColumns { get; set; }

		[DataMember]
		private IEnumerable<ViewColumnData> ViewColumnsSerializer {
			get { return ViewColumns.Values.OrderBy( v => v.ID ); }
			set { ViewColumns = new IDDictionary<ViewColumnData>( value ); }
		}

		/// <summary>
		/// ロックされる列数(左端から)
		/// </summary>
		[DataMember]
		public int ScrollLockColumnCount { get; set; }

		/// <summary>
		/// 自動ソートの順番
		/// </summary>
		[DataMember]
		public List<KeyValuePair<int, ListSortDirection>> SortOrder { get; set; }


		/// <summary>
		/// フィルタデータ
		/// </summary>
		[DataMember]
		public ExpressionManager Expressions { get; set; }



		/// <summary>
		/// 艦船IDリスト（キャッシュ）
		/// </summary>
		[IgnoreDataMember]
		public List<int> Members { get; private set; }

		/// <summary>
		/// 艦船リスト（キャッシュ）
		/// </summary>
		[IgnoreDataMember]
		public IEnumerable<ShipData> MembersInstance {
			get {
				return Members.Select( id => KCDatabase.Instance.Ships[id] );
			}
		}




		public ShipGroupData( int groupID ) {
			GroupID = groupID;
			ViewColumns = new IDDictionary<ViewColumnData>();
			Name = "notitle #" + groupID;
			ScrollLockColumnCount = 0;
			Expressions = new ExpressionManager();
			Members = new List<int>();
		}


		/// <summary>
		/// フィルタに基づいて検索を実行し、Members に結果をセットします。
		/// </summary>
		public void UpdateMembers() {
			if ( !Expressions.IsAvailable )
				Expressions.Compile();

			Members = Expressions.GetResult( KCDatabase.Instance.Ships.Values ).Select( s => s.MasterID ).ToList();
		}



		public int ID {
			get { return GroupID; }
		}


		public override string ToString() {
			return Name;
		}

	}

}
