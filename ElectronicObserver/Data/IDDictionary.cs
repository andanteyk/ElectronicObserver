using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// IDを持つデータのリストを保持します。
	/// </summary>
	/// <typeparam name="TData"></typeparam>
	public class IDDictionary<TData> : Dictionary<int, TData> where TData : class, IIdentifiable {

		public IDDictionary() 
			: this( new List<TData>() ){
		}

		public IDDictionary( IEnumerable<TData> source )
			: base( source.ToDictionary( x => x.ID ) ) {
		}


		internal void Add( TData data ) {
			Add( data.ID, data );
		}

		internal void Remove( TData data ) {
			Remove( data.ID );
		}

		internal int RemoveAll( Predicate<TData> predicate ) {
			var removekeys = Values.Where( elem => predicate( elem ) ).Select( elem => elem.ID ).ToArray();

			foreach ( var key in removekeys ) {
				Remove( key );
			}

			return removekeys.Count();
		}


		public new TData this[int key] {
			get { return ContainsKey( key ) ? base[key] : null; }
		}
	}


}
