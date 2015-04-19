using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

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
		private void DefaultDeserializing( StreamingContext sc ) {
			Initialize();
		}


		public void Save( string path ) {

			try {

				var serializer = new DataContractSerializer( this.GetType() );
				var xmlsetting = new XmlWriterSettings();

				xmlsetting.Encoding = new System.Text.UTF8Encoding( false );
				xmlsetting.Indent = true;
				xmlsetting.IndentChars = "\t";
				xmlsetting.NewLineHandling = NewLineHandling.Replace;

				using ( XmlWriter xw = XmlWriter.Create( path, xmlsetting ) ) {
					serializer.WriteObject( xw, this );
				}


			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "DataStorage の書き込みに失敗しました。" );
			}

		}

		public void Save( StringBuilder stringBuilder ) {
			try {
				var serializer = new DataContractSerializer( this.GetType() );
				using ( XmlWriter xw = XmlWriter.Create( stringBuilder ) ) {
					serializer.WriteObject( xw, this );
				}
			} catch ( Exception ex ) {
				Utility.ErrorReporter.SendErrorReport( ex, "DataStorage の書き込みに失敗しました。" );
			}
		}

		public DataStorage Load( string path ) {

			try {

				var serializer = new DataContractSerializer( this.GetType() );

				using ( XmlReader xr = XmlReader.Create( path ) ) {
					return (DataStorage)serializer.ReadObject( xr );
				}


			} catch ( FileNotFoundException ) {

				Utility.Logger.Add( 3, string.Format( "DataStorage {0} は存在しません。", path ) );

			} catch ( DirectoryNotFoundException ) {

				Utility.Logger.Add( 3, string.Format( "DataStorage {0} は存在しません。", path ) );

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "DataStorage の読み込みに失敗しました。" );

			}

			return null;
		}



		public void Save( Stream stream ) {

			try {

				var serializer = new DataContractSerializer( this.GetType() );
				var xmlsetting = new XmlWriterSettings();


				xmlsetting.Encoding = new System.Text.UTF8Encoding( false );
				xmlsetting.Indent = true;
				xmlsetting.IndentChars = "\t";
				xmlsetting.NewLineHandling = NewLineHandling.Replace;

				using ( XmlWriter xw = XmlWriter.Create( stream, xmlsetting ) ) {

					serializer.WriteObject( xw, this );
				}


			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "DataStorage の書き込みに失敗しました。" );
			}

		}


		public DataStorage Load( Stream stream ) {

			try {

				var serializer = new DataContractSerializer( this.GetType() );

				using ( XmlReader xr = XmlReader.Create( stream ) ) {
					return (DataStorage)serializer.ReadObject( xr );
				}

			} catch ( FileNotFoundException ) {

				Utility.Logger.Add( 3, string.Format( "DataStorage ファイルは存在しません。" ) );

			} catch ( DirectoryNotFoundException ) {

				Utility.Logger.Add( 3, string.Format( "DataStorage ファイルは存在しません。" ) );

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "DataStorage の読み込みに失敗しました。" );

			}

			return null;
		}

		public DataStorage Load( TextReader reader ) {
			try {
				var serializer = new DataContractSerializer( this.GetType() );

				using ( XmlReader xr = XmlReader.Create( reader ) ) {
					return (DataStorage)serializer.ReadObject( xr );
				}
			} catch ( DirectoryNotFoundException ) {

				Utility.Logger.Add( 3, string.Format( "DataStorage ファイルは存在しません。" ) );

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "DataStorage の読み込みに失敗しました。" );

			}

			return null;
		}

	}
}
