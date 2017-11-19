using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{

	/// <summary>
	/// IDを持つデータのリストを保持します。
	/// </summary>
	/// <typeparam name="TData"></typeparam>
	public class IDDictionary<TData> : IReadOnlyDictionary<int, TData> where TData : class, IIdentifiable
	{

		private readonly IDictionary<int, TData> dict;

		public IDDictionary()
			: this(new List<TData>())
		{
		}

		public IDDictionary(IEnumerable<TData> source)
		{
			dict = source.ToDictionary(x => x.ID);
		}


		internal void Add(TData data)
		{
			dict.Add(data.ID, data);
		}

		internal void Remove(TData data)
		{
			dict.Remove(data.ID);
		}

		internal void Remove(int id)
		{
			dict.Remove(id);
		}

		internal int RemoveAll(Predicate<TData> predicate)
		{
			var removekeys = dict.Values.Where(elem => predicate(elem)).Select(elem => elem.ID).ToArray();

			foreach (var key in removekeys)
			{
				dict.Remove(key);
			}

			return removekeys.Count();
		}

		internal void Clear()
		{
			dict.Clear();
		}


		public bool ContainsKey(int key)
		{
			return dict.ContainsKey(key);
		}

		public IEnumerable<int> Keys => dict.Keys;

		public bool TryGetValue(int key, out TData value)
		{
			return dict.TryGetValue(key, out value);
		}

		public IEnumerable<TData> Values => dict.Values;

		public TData this[int key] => dict.ContainsKey(key) ? dict[key] : null;

		public int Count => dict.Count;

		public IEnumerator<KeyValuePair<int, TData>> GetEnumerator()
		{
			return dict.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return dict.GetEnumerator();
		}
	}


}
