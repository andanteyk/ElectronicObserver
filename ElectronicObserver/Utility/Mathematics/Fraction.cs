using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.Mathematics {
	
	public class Fraction {

		protected int p_num, p_den;

		/*
		public int Numerator {
			get { return p_num; }
			set { p_num = value; }
		}

		public int Denominator {
			get { return p_den; }
			set { p_den = value; }
		}
		*/

		public int Value {
			get { return p_num; }
			set { p_num = value; }
		}

		public int Max {
			get { return p_den; }
			set { p_den = value; }
		}



		public Fraction() {
			p_num = p_den = 0;
		}

		public Fraction( int value ) {
			p_num = p_den = value;
		}

		public Fraction( int numerator, int denominator ) {
			p_num = numerator;
			p_den = denominator;
		}

		public Fraction( int[] value ) {
			if ( value.Length < 2 )
				throw new ArgumentException( "配列の長さが足りません。" );
			p_num = value[0];
			p_den = value[1];
		}

		public double GetPercentage() {
			if ( p_den == 0 )
				return 0;
			else
				return (double)p_num / p_den;
		}

		public override string ToString() {
			return p_num.ToString() + " / " + p_den.ToString();
		}
	}


}
