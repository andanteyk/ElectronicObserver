﻿using System;
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

                Utility.ErrorReporter.SendErrorReport(ex, LoadResources.getter("DataStorage_1"));
			}

		}

		public DataStorage Load( string path ) {

			try {

				var serializer = new DataContractSerializer( this.GetType() );

				using ( XmlReader xr = XmlReader.Create( path ) ) {
					return (DataStorage)serializer.ReadObject( xr );
				}


			} catch ( FileNotFoundException ) {

                Utility.Logger.Add(3, string.Format(LoadResources.getter("DataStorage_2"), path));

			} catch( DirectoryNotFoundException ) {

                Utility.Logger.Add(3, string.Format(LoadResources.getter("DataStorage_2"), path));

			} catch ( Exception ex ) {

                Utility.ErrorReporter.SendErrorReport(ex, LoadResources.getter("DataStorage_3"));

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

                Utility.ErrorReporter.SendErrorReport(ex, LoadResources.getter("DataStorage_1"));
			}

		}


		public DataStorage Load( Stream stream ) {

			try {

				var serializer = new DataContractSerializer( this.GetType() );

				using ( XmlReader xr = XmlReader.Create( stream ) ) {
					return (DataStorage)serializer.ReadObject( xr );
				}

			} catch ( FileNotFoundException ) {

                Utility.Logger.Add(3, string.Format(LoadResources.getter("DataStorage_4")));

			} catch ( DirectoryNotFoundException ) {

                Utility.Logger.Add(3, string.Format(LoadResources.getter("DataStorage_4")));

			} catch ( Exception ex ) {

                Utility.ErrorReporter.SendErrorReport(ex, LoadResources.getter("DataStorage_3"));

			}

			return null;
		}

	}
}
