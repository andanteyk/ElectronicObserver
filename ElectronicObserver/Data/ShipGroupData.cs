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
	public class ShipGroupData : IIdentifiable, ICloneable {


		/// <summary>
		/// 列のプロパティを保持します。
		/// </summary>
		[DataContract( Name = "ViewColumnData" )]
		public class ViewColumnData : ICloneable {

			/// <summary>
			/// 列名
			/// </summary>
			[DataMember]
			public string Name { get; set; }

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



			public ViewColumnData( string name ) {
				Name = name;
			}

			public ViewColumnData( string name, int width, int displayIndex, bool visible, bool autoSize ) {
				Name = name;
				Width = width;
				DisplayIndex = displayIndex;
				Visible = visible;
				AutoSize = autoSize;
			}

			public ViewColumnData( DataGridViewColumn column ) {
				FromColumn( column );
			}


			/// <summary>
			/// 現在の設定を、列に対して適用します。
			/// </summary>
			/// <param name="column">対象となる列。</param>
			public void ToColumn( DataGridViewColumn column ) {
				if ( column.Name != Name )
					throw new ArgumentException( "設定する列と Name が異なります。" );

				column.Width = Width;
				column.DisplayIndex = DisplayIndex;
				column.Visible = Visible;
				column.AutoSizeMode = AutoSize ? DataGridViewAutoSizeColumnMode.AllCellsExceptHeader : DataGridViewAutoSizeColumnMode.NotSet;
			}

			/// <summary>
			/// 現在の列の状態から、設定を生成します。
			/// </summary>
			/// <param name="column">対象となる列。</param>
			/// <returns>このインスタンス自身を返します。</returns>
			public ViewColumnData FromColumn( DataGridViewColumn column ) {
				Name = column.Name;
				Width = column.Width;
				DisplayIndex = column.DisplayIndex;
				Visible = column.Visible;
				AutoSize = column.AutoSizeMode == DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
				return this;
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
		/// 排序列
		/// </summary>
		[DataMember]
		public string SortColumnName { get; set; }

		/// <summary>
		/// 升降序
		/// </summary>
		[DataMember]
		public int SortOrder { get; set; }

		/// <summary>
		/// グループ名
		/// </summary>
		[DataMember]
		public string Name { get; set; }


		/// <summary>
		/// 列の設定
		/// </summary>
		[IgnoreDataMember]
		public Dictionary<string, ViewColumnData> ViewColumns { get; set; }

		[DataMember]
		private IEnumerable<ViewColumnData> ViewColumnsSerializer {
			get { return ViewColumns.Values; }
			set { ViewColumns = value.ToDictionary( v => v.Name ); }
		}


		/// <summary>
		/// ロックされる列数(左端から)
		/// </summary>
		[DataMember]
		public int ScrollLockColumnCount { get; set; }


		/// <summary>
		/// 自動ソートの順番
		/// </summary>
		[IgnoreDataMember]
		public List<KeyValuePair<string, ListSortDirection>> SortOrder { get; set; }

		[DataMember]
		private List<SerializableKeyValuePair<string, ListSortDirection>> SerealizedSortOrder {
			get { return SortOrder == null ? null : SortOrder.Select( s => new SerializableKeyValuePair<string, ListSortDirection>( s ) ).ToList(); }
			set { SortOrder = value == null ? null : value.Select( s => new KeyValuePair<string, ListSortDirection>( s.Key, s.Value ) ).ToList(); }
		}


		/// <summary>
		/// 自動ソートを行うか
		/// </summary>
		[DataMember]
		public bool AutoSortEnabled { get; set; }


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
			ViewColumns = new Dictionary<string, ViewColumnData>();
			Name = "notitle #" + groupID;
			ScrollLockColumnCount = 0;
			SortOrder = new List<KeyValuePair<string, ListSortDirection>>();
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




		public ShipGroupData Clone() {
			var clone = (ShipGroupData)MemberwiseClone();
			clone.GroupID = -1;
			clone.ViewColumns = new Dictionary<string, ViewColumnData>( ViewColumns );
			clone.SortOrder = new List<KeyValuePair<string, ListSortDirection>>( SortOrder );

			clone.Members = new List<int>();		//とりあえず空に

			return clone;
		} 

		object ICloneable.Clone() {
			return Clone();
		}

	}

}
