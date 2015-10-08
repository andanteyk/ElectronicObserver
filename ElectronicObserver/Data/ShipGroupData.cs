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
	public class ShipGroupData : DataStorage, IIdentifiable, ICloneable {


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
			[DataMember]
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

				column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;		//width 変更のためいったん戻す
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
		private List<SerializableKeyValuePair<string, ListSortDirection>> SortOrderSerializer {
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
		/// 包含フィルタ
		/// </summary>
		[IgnoreDataMember]
		public List<int> InclusionFilter { get; set; }

		[DataMember]
		private SerializableList<int> InclusionFilterSerializer {
			get { return InclusionFilter; }
			set { InclusionFilter = value; }
		}

		/// <summary>
		/// 除外フィルタ
		/// </summary>
		[IgnoreDataMember]
		public List<int> ExclusionFilter { get; set; }

		[DataMember]
		private SerializableList<int> ExclusionFilterSerializer {
			get { return ExclusionFilter; }
			set { ExclusionFilter = value; }
		}



		/// <summary>
		/// 艦船IDリスト
		/// </summary>
		[IgnoreDataMember]
		public List<int> Members { get; private set; }

		/// <summary>
		/// 艦船リスト
		/// </summary>
		[IgnoreDataMember]
		public IEnumerable<ShipData> MembersInstance {
			get {
				return Members.Select( id => KCDatabase.Instance.Ships[id] );
			}
		}

		[DataMember]
		private SerializableList<int> MembersSerializer {
			get { return Members; }
			set { Members = value; }
		}



		public ShipGroupData( int groupID )
			: base() {
			GroupID = groupID;
		}

		public override void Initialize() {
			GroupID = -1;
			ViewColumns = new Dictionary<string, ViewColumnData>();
			Name = "no title";
			ScrollLockColumnCount = 0;
			AutoSortEnabled = true;
			SortOrder = new List<KeyValuePair<string, ListSortDirection>>();
			Expressions = new ExpressionManager();
			InclusionFilter = new List<int>();
			ExclusionFilter = new List<int>();
			Members = new List<int>();
		}


		/// <summary>
		/// フィルタに基づいて検索を実行し、Members に結果をセットします。
		/// </summary>
		/// <param name="previousOrder">直前の並び替え順。なるべくこの順番を維持するように結果が生成されます。null もしくは 要素数 0 の場合は適当に生成されます。</param>
		public void UpdateMembers( IEnumerable<int> previousOrder = null ) {

			if ( Expressions == null )
				Expressions = new ExpressionManager();

			if ( InclusionFilter == null )
				InclusionFilter = new List<int>();

			if ( ExclusionFilter == null )
				ExclusionFilter = new List<int>();

			ValidateFilter();


			if ( !Expressions.IsAvailable )
				Expressions.Compile();

			var newdata = Expressions.GetResult( KCDatabase.Instance.Ships.Values ).Select( s => s.MasterID ).Union( InclusionFilter ).Except( ExclusionFilter );

			IEnumerable<int> prev = ( previousOrder != null && previousOrder.Count() > 0 ) ? previousOrder : ( Members ?? new List<int>() );

			// ソート順序を維持するため
			Members = prev.Except( prev.Except( newdata ) ).Union( newdata ).ToList();
		}


		public void AddInclusionFilter( IEnumerable<int> list ) {
			InclusionFilter = InclusionFilter.Union( list ).ToList();
			ExclusionFilter = ExclusionFilter.Except( list ).ToList();
		}

		public void AddExclusionFilter( IEnumerable<int> list ) {
			InclusionFilter = InclusionFilter.Except( list ).ToList();
			ExclusionFilter = ExclusionFilter.Union( list ).ToList();
		}

		public void ValidateFilter() {
			if ( KCDatabase.Instance.Ships.Count > 0 ) {
				var ships = KCDatabase.Instance.Ships.Keys;
				InclusionFilter = InclusionFilter.Intersect( ships ).Distinct().ToList();
				ExclusionFilter = ExclusionFilter.Intersect( ships ).Distinct().ToList();
			}
		}


		public int ID {
			get { return GroupID; }
		}


		public override string ToString() {
			return Name;
		}



		/// <summary>
		/// このオブジェクトの複製(ディープ コピー)を作成します。
		/// </summary>
		/// <remarks>複製したオブジェクトのIDは必ず -1 になります。適宜再設定してください。</remarks>
		public ShipGroupData Clone() {
			var clone = (ShipGroupData)MemberwiseClone();
			clone.GroupID = -1;
			clone.ViewColumns = ViewColumns.Select( p => p.Value.Clone() ).ToDictionary( p => p.Name );
			clone.SortOrder = new List<KeyValuePair<string, ListSortDirection>>( SortOrder );
			clone.Expressions = Expressions.Clone();
			clone.InclusionFilter = new List<int>( InclusionFilter );
			clone.ExclusionFilter = new List<int>( ExclusionFilter );
			clone.Members = new List<int>( Members );

			return clone;
		}

		object ICloneable.Clone() {
			return Clone();
		}



	}

}
