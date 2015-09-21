using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.Storage {

	[DataContract( Name = "SerializableKeyValuePair" )]
	public struct SerializableKeyValuePair<TKey, TValue> {

	
		public SerializableKeyValuePair( TKey key, TValue value ) {
			Key = key;
			Value = value;
		}

		public SerializableKeyValuePair( KeyValuePair<TKey, TValue> value ) {
			Key = value.Key;
			Value = value.Value;
		}


		[DataMember]
		public TKey Key;

		[DataMember]
		public TValue Value;


		public static implicit operator KeyValuePair<TKey, TValue>( SerializableKeyValuePair<TKey, TValue> value ) {
			return new KeyValuePair<TKey, TValue>( value.Key, value.Value );
		}

		public static implicit operator SerializableKeyValuePair<TKey, TValue>( KeyValuePair<TKey, TValue> value ) {
			return new SerializableKeyValuePair<TKey, TValue>( value.Key, value.Value );
		}

	}
}
