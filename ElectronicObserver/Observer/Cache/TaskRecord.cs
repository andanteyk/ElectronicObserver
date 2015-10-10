using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.Cache
{
    static class TaskRecord
    {
        static Dictionary<string, string> record = new Dictionary<string, string>();
        //KEY: url, Value: filepath
        //只有在验证文件修改时间后，向客户端返回本地文件或者将文件保存到本地时才需要使用

        static public void Add(string url, string filepath)
        {
            if (record.ContainsKey(url))
                record[url] = filepath;
            else
                record.Add(url, filepath);
        }

        static public string GetAndRemove(string url)
        {
            string ret = Get(url);
            record.Remove(url);
            return ret;
        }
        static public string Get(string url)
        {
            if (record.ContainsKey(url))
                return record[url];
            return "";
        }
    }
}
