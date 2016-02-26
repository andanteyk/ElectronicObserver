using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.Cache
{
	static class TaskRecord
	{
		static ConcurrentDictionary<string, string> record = new ConcurrentDictionary<string, string>();
		//KEY: url, Value: filepath
		//只有在验证文件修改时间后，向客户端返回本地文件或者将文件保存到本地时才需要使用

		static public void Add( string url, string filepath )
		{
			record.AddOrUpdate( url, filepath, ( key, oldValue ) => filepath );
		}

		static public string GetAndRemove( string url )
		{
			string ret;
			record.TryRemove( url, out ret );
			return ret;
		}
		static public string Get( string url )
		{
			string ret;
			if ( record.TryGetValue( url, out ret ) )
				return ret;
			return "";
		}
	}
}
