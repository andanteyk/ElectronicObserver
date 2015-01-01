using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ElectronicObserver.Utility.Storage {

	//checkme: unsafe?

	/// <summary>
	/// シリアル化可能な Font を扱います。
	/// </summary>
	public class SerializableFont {

		
		[IgnoreDataMember]
		public Font FontData { get; set; }


		public SerializableFont() {
			FontData = null;
		}

		public SerializableFont( Font font ) {
			FontData = font;
		}

		
		[DataMember]
		public string SerializeFontAttribute {
			get {
				try {
					if ( FontData != null ) {
						return TypeDescriptor.GetConverter( typeof( Font ) ).ConvertToString( FontData );
					} 
				} catch ( Exception ex ) {
					Utility.ErrorReporter.SaveErrorReport( ex, "SerializableFont.ToString failed" );
				}

				return null;
			}
			set {
				try {
					FontData = (Font)TypeDescriptor.GetConverter( typeof( Font ) ).ConvertFromString( value );
				} catch ( Exception ex ) {
					Utility.ErrorReporter.SaveErrorReport( ex, "SerializableFont.FromString failed" );
					FontData = null;
				}
			}
		}

		public static implicit operator Font( SerializableFont value ) {
			if ( value == null ) return null;
			return value.FontData;
		}

		public static implicit operator SerializableFont( Font value ) {
			return new SerializableFont( value );
		}

	}

}
