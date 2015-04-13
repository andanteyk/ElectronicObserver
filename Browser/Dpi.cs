using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser
{
    class Dpi
    {
        public static readonly Dpi Default = new Dpi(96, 96);

        public uint X { get; private set; }
        public uint Y { get; private set; }

        public Dpi(uint x, uint y)
        {
            this.X = x;
            this.Y = y;
        }

        public double ScaleX
        {
            get { return this.X / (double)Default.X; }
        }

        public double ScaleY
        {
            get { return this.Y / (double)Default.Y; }
        }
    }
}
