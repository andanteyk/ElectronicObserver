using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility
{
    public class PerformanceMonitor
    {
        private static Stopwatch sw = new Stopwatch();

        static PerformanceMonitor()
        {
            sw.Start();
        }

        public static void Print(string text)
        {
            System.Diagnostics.Trace.WriteLine("[" + sw.ElapsedMilliseconds + " ms] " + text);
        }
    }
}
