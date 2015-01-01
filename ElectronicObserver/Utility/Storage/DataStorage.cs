using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ElectronicObserver.Utility.Storage {

	/// <summary>
	/// 汎用データ保存クラスの基底です。
	/// 使用時は DataContractAttribute を設定してください。
	/// </summary>
	[DataContract( Name = "DataStorage" )]
	public abstract class DataStorage : IExtensibleDataObject {

		public ExtensionDataObject ExtensionData { get; set; }

		public abstract void Initialize();



		public DataStorage() {
			Initialize();
		}

		[OnDeserializing]
		public void DefaultDeserializing( StreamingContext sc ) {
			Initialize();
		}


		public void Save( string path ) {

			try {

				var serializer = new DataContractSerializer( this.GetType() );
				var xmlsetting = new XmlWriterSettings();

				xmlsetting.Encoding = Encoding.UTF8;
				xmlsetting.Indent = true;
				xmlsetting.IndentChars = "\t";
				xmlsetting.NewLineHandling = NewLineHandling.Replace;

				using ( XmlWriter xw = XmlWriter.Create( path, xmlsetting ) ) {
					serializer.WriteObject( xw, this );
				}


			} catch ( Exception ex ) {

				Utility.ErrorReporter.SaveErrorReport( ex, "DataStorage の書き込みに失敗しました。" );
			}

		}

		public DataStorage Load( string path ) {

			try {

				var serializer = new DataContractSerializer( this.GetType() );

				using ( XmlReader xr = XmlReader.Create( path ) ) {
					return (DataStorage)serializer.ReadObject( xr );
				}

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SaveErrorReport( ex, "DataStorage の読み込みに失敗しました。" );

			}

			return null;
		}

	}
}
