using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data {

	/// <summary>
	/// IDを持つデータのリストを保持します。
	/// </summary>
	/// <typeparam name="TData"></typeparam>
	public class IDDictionary<TData> : IEnumerable<KeyValuePair<int, TData>>, IEnumerable where TData : class, IIdentifiable {

		private readonly IDictionary<int, TData> dict;

		public IDDictionary() 
			: this( new List<TData>() ){
		}

		public IDDictionary( IEnumerable<TData> source ) {
			dict = source.ToDictionary( x => x.ID );
		}


		internal void Add( TData data ) {
			dict.Add( data.ID, data );
		}

		internal void Remove( TData data ) {
			dict.Remove( data.ID );
		}

		internal void Remove( int id ) {
			dict.Remove( id );
		}

		internal int RemoveAll( Predicate<TData> predicate ) {
			var removekeys = dict.Values.Where( elem => predicate( elem ) ).Select( elem => elem.ID ).ToArray();

			foreach ( var key in removekeys ) {
				dict.Remove( key );
			}

			return removekeys.Count();
		}

		internal void Clear() {
			dict.Clear();
		}


		public bool ContainsKey( int key ) {
			return dict.ContainsKey( key );
		}

		public IEnumerable<int> Keys {
			get { return dict.Keys; }
		}

		public bool TryGetValue( int key, out TData value ) {
			return dict.TryGetValue( key, out value );
		}

		public IEnumerable<TData> Values {
			get { return dict.Values; }
		}

		public TData this[int key] {
			get { return dict.ContainsKey( key ) ? dict[key] : null; }
		}

		public int Count {
			get { return dict.Count; }
		}

		public IEnumerator<KeyValuePair<int, TData>> GetEnumerator() {
			return dict.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return dict.GetEnumerator();
		}
	}


}
