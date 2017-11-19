using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer
{


	public class APIDictionary : IReadOnlyDictionary<string, APIBase>
	{

		private readonly IDictionary<string, APIBase> dict;

		public APIDictionary()
			: this(new List<APIBase>())
		{
		}

		public APIDictionary(IEnumerable<APIBase> source)
		{
			dict = source.ToDictionary(x => x.APIName);
		}


		internal void Add(APIBase data)
		{
			dict.Add(data.APIName, data);
		}

		internal void Remove(APIBase data)
		{
			dict.Remove(data.APIName);
		}

		internal void Remove(string apiname)
		{
			dict.Remove(apiname);
		}

		internal void Clear()
		{
			dict.Clear();
		}


		public void OnRequestReceived(string apiname, Dictionary<string, string> data)
		{
			if (dict.ContainsKey(apiname) && dict[apiname].IsRequestSupported)
			{
				dict[apiname].OnRequestReceived(data);
			}
		}

		public void OnResponseReceived(string apiname, dynamic data)
		{
			if (dict.ContainsKey(apiname) && dict[apiname].IsResponseSupported)
			{
				dict[apiname].OnResponseReceived(data);
			}
		}


		public bool ContainsKey(string key)
		{
			return dict.ContainsKey(key);
		}

		public IEnumerable<string> Keys => dict.Keys;

		public bool TryGetValue(string key, out APIBase value)
		{
			return dict.TryGetValue(key, out value);
		}

		public IEnumerable<APIBase> Values => dict.Values;

		public APIBase this[string key] => dict.ContainsKey(key) ? dict[key] : null;

		public int Count => dict.Count;

		public IEnumerator<KeyValuePair<string, APIBase>> GetEnumerator()
		{
			return dict.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return dict.GetEnumerator();
		}
	}

}
