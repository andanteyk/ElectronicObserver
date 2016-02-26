using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.Mathematics
{
    public class GMTHelper
    {
        /// <summary>  
        /// 本地时间转成GMT时间  
        /// </summary>  
        /// <returns>Thu, 29 Sep 2011 15:04:39 GMT</returns>
        /// <remarks>ToGMTString(DateTime.Now)</remarks>
        public static string ToGMTString(DateTime dt)
        {
            return dt.ToUniversalTime().ToString("r");
        }

        #region 未引用的函数

        /// <summary>  
        /// 本地时间转成GMT格式的时间  
        /// </summary>  
        /// <returns>Thu, 29 Sep 2011 15:04:39 GMT+0800</returns>
        public static string ToGMTFormat(DateTime dt)
        {
            return dt.ToString("r") + dt.ToString("zzz").Replace(":", "");
        }

        /// <summary>  
        /// GMT时间转成本地时间  
        /// </summary>  
        /// <param name="gmt">Thu, 29 Sep 2011 07:04:39 GMT</param>  
        /// <param name="gmt">Thu, 29 Sep 2011 15:04:39 GMT+0800</param>  
        /// <returns></returns>  
        public static DateTime GMT2Local(string gmt)
        {
            DateTime dt = DateTime.MinValue;
            try
            {
                string pattern = "";
                if (gmt.IndexOf("+0") != -1)
                {
                    gmt = gmt.Replace("GMT", "");
                    pattern = "ddd, dd MMM yyyy HH':'mm':'ss zzz";
                }
                if (gmt.ToUpper().IndexOf("GMT") != -1)
                {
                    pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
                }
                if (pattern != "")
                {
                    dt = DateTime.ParseExact(gmt, pattern, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal);
                    dt = dt.ToLocalTime();
                }
                else
                {
                    dt = Convert.ToDateTime(gmt);
                }
            }
            catch
            {
            }
            return dt;
        }

        #endregion
    }
}
