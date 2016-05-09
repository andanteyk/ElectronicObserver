
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;


namespace ElectronicObserver.Utility
{
    /// <summary>
    /// IniFiles的类
    /// </summary>
    public class IniFile
    {
        public string FileName; //INI文件名
                                //声明读写INI文件的API函数
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] data, int size, string filePath);
        //类的构造函数，传递INI文件名
        public IniFile(string AFileName)
        {
            // 判断文件是否存在
            FileInfo fileInfo = new FileInfo(AFileName);
            //Todo:搞清枚举的用法
            if ((!fileInfo.Exists))
            { //|| (FileAttributes.Directory in fileInfo.Attributes))
              //文件不存在，建立文件
                System.IO.StreamWriter sw = new System.IO.StreamWriter(AFileName, false, System.Text.Encoding.Default);
                try
                {
                    sw.Close();
                }

                catch
                {
                    throw (new Exception("Ini文件无法建立"));
                }
            }
            //必须是完全路径，不能是相对路径
            FileName = fileInfo.FullName;
        }
        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="Section">段名</param>
        /// <param name="Ident">关键字</param>
        /// <param name="Value">值</param>
        public void WriteString(string Section, string Key, string Value)
        {
            if (!WritePrivateProfileString(Section, Key, Value, FileName))
            {

                throw (new Exception("写Ini文件出错"));
            }
        }
        //读取INI文件指定
        public string ReadString(string Section, string Key, string Default)
        {
            StringBuilder Buffer = new StringBuilder(4096);
            GetPrivateProfileString(Section, Key, Default, Buffer, Buffer.Capacity, FileName);

            return Buffer.ToString();
        }

        //读整数
        public int ReadInteger(string Section, string Ident, int Default)
        {
            string intStr = ReadString(Section, Ident, Convert.ToString(Default));
            try
            {
                return Convert.ToInt32(intStr);

            }
            catch (Exception ex)
            {

                return Default;
            }
        }

        //写整数
        public void WriteInteger(string Section, string Ident, int Value)
        {
            WriteString(Section, Ident, Value.ToString());
        }

        //读布尔
        public bool ReadBool(string Section, string Ident, bool Default)
        {
            try
            {
                return Convert.ToBoolean(ReadString(Section, Ident, Convert.ToString(Default)));
            }
            catch (Exception ex)
            {

                return Default;
            }
        }

        //写Bool
        public void WriteBool(string Section, string Ident, bool Value)
        {
            WriteString(Section, Ident, Convert.ToString(Value));
        }

        //从Ini文件中，将指定的Section名称中的所有Key添加到列表中
        public string[] ReadSection(string Section)
        {
            //Note:必须得用Bytes来实现，StringBuilder只能取到第一个Indent
            byte[] Buffer = new byte[10000];
            int bufLen = GetPrivateProfileString(Section, null, null, Buffer, Buffer.Length, FileName);
            if(bufLen>0)
            {
                string a = System.Text.Encoding.Default.GetString(Buffer, 0, bufLen - 1);
                return a.Split('\0');
            }
            else
            {
                return new string[0];
            }
        }

        /// <summary>
        /// 从Ini文件中，读取所有的Sections的名称
        /// </summary>
        /// <param name="SectionList"></param>
        public string[] ReadSections()
        {
            //Note:必须得用Bytes来实现，StringBuilder只能取到第一个Section
            byte[] Buffer = new byte[1024];
            int bufLen = GetPrivateProfileString(null, null, null, Buffer, Buffer.Length, FileName);
            if(bufLen>0)
            {
                string a = System.Text.Encoding.Default.GetString(Buffer, 0, bufLen - 1);
                return a.Split('\0');
            }
            else
            {
                return new string[0];
            }
        }
        //读取指定的Section的所有Value到列表中
        public Dictionary<string, string> ReadSectionValues(string Section)
        {
            Dictionary<string, string> lv = new Dictionary<string, string>();
            string[] KeyList = ReadSection(Section);
            foreach (string key in KeyList)
            {
                lv.Add(key, ReadString(Section, key, ""));
            }
            return lv;
        }
        //读取指定的Section的所有Value到列表中
        public void ReadSectionValues(string Section, NameValueCollection Values)
        {

            string[] KeyList = ReadSection(Section);
            Values.Clear();
            foreach (string key in KeyList)
            {
                Values.Add(key, ReadString(Section, key, ""));
            }
        }
        //清除某个Section
        public void EraseSection(string Section)
        {
            //
            if (!WritePrivateProfileString(Section, null, null, FileName))
            {
                throw (new Exception("无法清除Ini文件中的Section"));
            }
        }
        //删除某个Section下的键
        public void DeleteKey(string Section, string Ident)
        {
            WritePrivateProfileString(Section, Ident, null, FileName);
        }
        //Note:对于Win9X，来说需要实现UpdateFile方法将缓冲中的数据写入文件
        //在Win NT, 2000和XP上，都是直接写文件，没有缓冲，所以，无须实现UpdateFile
        //执行完对Ini文件的修改之后，应该调用本方法更新缓冲区。
        public void UpdateFile()
        {
            WritePrivateProfileString(null, null, null, FileName);
        }

        ////检查某个Section下的某个键值是否存在
        public bool KeyExists(string Section, string Key)
        {
            string[] Idents = ReadSection(Section);
            for (int i = 0; i < Idents.Length; i++)
            {
                if (Idents[i] == Key)
                    return true;
            }
            return false;
        }

        //确保资源的释放
        ~IniFile()
        {
            UpdateFile();
        }
    }
}

