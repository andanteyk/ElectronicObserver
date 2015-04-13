using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Browser
{
    static class ScreenHelper
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr ptr);
        [DllImport("user32.dll")]
        static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern uint GetDeviceCaps(
        IntPtr hdc, // handle to DC
        int nIndex // index of capability
        );
        [DllImport("user32.dll")]
        static extern bool SetProcessDPIAware();

        const int LOGPIXELSX = 88;
        const int LOGPIXELSY = 90;

        static Dpi _dpi;
        public static Dpi GetSystemDpi()
        {
            if (_dpi == null)
            {
                SetProcessDPIAware(); //重要
                IntPtr screenDC = GetDC(IntPtr.Zero);
                uint dpi_x = GetDeviceCaps(screenDC, LOGPIXELSX);
                uint dpi_y = GetDeviceCaps(screenDC, LOGPIXELSY);
                _dpi = new Dpi(dpi_x, dpi_y);
                ReleaseDC(IntPtr.Zero, screenDC);
            }
            return _dpi;
        }
    }
}
