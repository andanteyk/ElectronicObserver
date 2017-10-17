using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.Data {

	/// <summary>
	/// レコードに使用するための MD5 アルゴリズムによるハッシュ計算を行います。
	/// デフォルトの MD5 クラスを利用した場合、 FIPS が有効な環境下で InvalidOperationException や TargetInvocationException が発生する問題があります。
	/// この問題を回避するためのクラスです。
	/// </summary>
	/// <remarks>http://support.microsoft.com/kb/811833</remarks>
	public static class RecordHash {

		// こちらが使えればそのほうが2.5倍ぐらい処理が速い
		private static System.Security.Cryptography.MD5 originalHasher;

		private static readonly uint[] k;
		private static readonly int[] s;

		static RecordHash() {

			try {

				originalHasher = System.Security.Cryptography.MD5.Create();


			} catch ( Exception ) {

				Utility.Logger.Add( 1, "RecordHash: System.Security.Cryptography.MD5 ハッシュが利用できません。独自実装を利用します。" );

				k = new uint[64];

				for ( int i = 0; i < 64; i++ ) {
					k[i] = (uint)Math.Floor( Math.Abs( Math.Sin( i + 1 ) ) * 4294967296.0 );
				}

				s = new int[] { 
					7, 12, 17, 22,
					7, 12, 17, 22,
					7, 12, 17, 22,
					7, 12, 17, 22,

					5, 9, 14, 20,
					5, 9, 14, 20,
					5, 9, 14, 20,
					5, 9, 14, 20,

					4, 11, 16, 23,
					4, 11, 16, 23,
					4, 11, 16, 23,
					4, 11, 16, 23,

					6, 10, 15, 21,
					6, 10, 15, 21,
					6, 10, 15, 21,
					6, 10, 15, 21,
				};
			}

		}


		public static byte[] ComputeHash( byte[] input ) {

			if ( originalHasher != null ) {
				return originalHasher.ComputeHash( input );
			}


			// 既存実装が使えなかったら自前実装で計算

			uint a0 = 0x67452301;
			uint b0 = 0xefcdab89;
			uint c0 = 0x98badcfe;
			uint d0 = 0x10325476;

			byte[] inpad = new byte[(int)Math.Ceiling( ( input.Length + 9 ) / 64.0 ) * 64];
			Array.Copy( input, inpad, input.Length );

			// add bit "1"
			inpad[input.Length] = 0x80;
			// add input's length(bits)
			for ( int i = inpad.Length - 8; i < inpad.Length - 4; i++ ) {
				inpad[i] = (byte)( ( (uint)input.Length << 3 ) >> ( ( i - ( inpad.Length - 8 ) ) * 8 ) );
			}
			inpad[inpad.Length - 4] = (byte)( (uint)input.Length >> 29 );


			for ( int chunk = 0; chunk < inpad.Length / 64; chunk++ ) {

				uint[] m = ToUInt32( inpad, chunk * 64, 16 );

				uint a = a0;
				uint b = b0;
				uint c = c0;
				uint d = d0;

				for ( int i = 0; i < 64; i++ ) {

					uint f;
					int g;

					if ( i < 16 ) {
						f = ( b & c ) | ( ~b & d );
						g = i;
					} else if ( i < 32 ) {
						f = ( d & b ) | ( ~d & c );
						g = ( 5 * i + 1 ) % 16;
					} else if ( i < 48 ) {
						f = b ^ c ^ d;
						g = ( 3 * i + 5 ) % 16;
					} else {
						f = c ^ ( b | ~d );
						g = 7 * i % 16;
					}

					uint dtemp = d;
					d = c;
					c = b;
					b += RotateLeft( ( a + f + k[i] + m[g] ), s[i] );
					a = dtemp;
				}

				a0 += a;
				b0 += b;
				c0 += c;
				d0 += d;
			}

			return ToBytes( a0, b0, c0, d0 );
		}

		public static byte[] ComputeHash( string input ) {
			return ComputeHash( Encoding.UTF8.GetBytes( input ) );
		}


		private static uint[] ToUInt32( byte[] input, int index, int outLength ) {
			var ret = new uint[outLength];
			for ( int i = 0; i < outLength; i++ ) {

				if ( BitConverter.IsLittleEndian )
					ret[i] = BitConverter.ToUInt32( input, index + i * 4 );
				else
					ret[i] = (uint)System.Net.IPAddress.NetworkToHostOrder( BitConverter.ToUInt32( input, index + i * 4 ) );
			}
			return ret;
		}

		private static byte[] ToBytes( params uint[] values ) {
			var ret = new byte[values.Length * 4];
			for ( int i = 0; i < values.Length; i++ ) {
				ret[i * 4 + 0] = (byte)values[i];
				ret[i * 4 + 1] = (byte)( values[i] >> 8 );
				ret[i * 4 + 2] = (byte)( values[i] >> 16 );
				ret[i * 4 + 3] = (byte)( values[i] >> 24 );
			}
			return ret;
		}

		private static uint RotateLeft( uint value, int shift ) {
			return ( value << shift ) | ( value >> ( 32 - shift ) );
		}

	}
}
