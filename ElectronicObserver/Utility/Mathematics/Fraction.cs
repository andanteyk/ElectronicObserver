using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.Mathematics
{

	public class Fraction
	{
		public int Current { get; set; }
		public int Max { get; set; }

		public double Rate => (double)Current / Math.Max(Max, 1);


		public Fraction()
		{
			Current = Max = 0;
		}

		public Fraction(int current, int max)
		{
			Current = current;
			Max = max;
		}

		public override string ToString() => $"{Current}/{Max}";

	}

}
