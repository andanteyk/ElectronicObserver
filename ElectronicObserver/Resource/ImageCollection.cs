using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource {

	public sealed class ImageCollection : List<KeyValuePair<string, Image>> {

		private static readonly Size _default_size = new Size( 16, 16 );

		public Size ImageSize { get { return _default_size; } }

		public Image this[string key] {
			get {

				int index = this.FindIndex( kv => kv.Key == key );
				if ( index < 0 ) {
					throw new ArgumentOutOfRangeException( "key", "找不到key：" + key );
				}

				return base[index].Value;
			}
		}

		public new Image this[int index] {
			get {
				return base[index].Value;
			}
		}

		public ImageCollection Images { get { return this; } }

		public void Add( Image image ) {
			Add( base.Count.ToString(), image );
		}

		public void Add( string key, Image image ) {
			base.Add( new KeyValuePair<string, Image>( key, image ) );
		}

	}

}
