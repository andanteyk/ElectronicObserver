using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource.Record {

	/// <summary>
	/// レコードの基底です。
	/// </summary>
	public abstract class RecordBase {


		/// <summary>
		/// レコードの要素の基底です。
		/// </summary>
		public abstract class RecordElementBase {

			public abstract void LoadLine( string line );
			public abstract string SaveLine();

			
			public RecordElementBase() {}

			public RecordElementBase( string line ) 
				: this() {
				LoadLine( line );
			}
		}



		/// <summary>
		/// ファイルからレコードを読み込みます。
		/// </summary>
		/// <param name="path">ファイルが存在するフォルダのパス。</param>
		public virtual void Load( string path ) {

			path = path.Trim( @" \\""".ToCharArray() ) + "\\" + FileName;

			try {

				//Excel様が読めるようにするための苦渋の決断
				using ( StreamReader sr = new StreamReader( path, Encoding.Default ) ) {

					ClearRecord();

					string line;
					sr.ReadLine();			//ヘッダを読み飛ばす
					
					while ( ( line = sr.ReadLine() ) != null ) {
						if ( line.StartsWith( "#" ) )
							continue;
						LoadLine( line );
					}

				}

			} catch ( FileNotFoundException ) {

				Utility.Logger.Add( 1, "レコード " + path + " は存在しません。" );


			} catch ( Exception ex ) {

				Utility.ErrorReporter.SaveErrorReport( ex, "レコード " + path + " の読み込みに失敗しました。" );

			}
		}


		/// <summary>
		/// ファイルにレコードを書き込みます。
		/// </summary>
		/// <param name="path">ファイルが存在するフォルダのパス。</param>
		public virtual void Save( string path ) {

			path = path.Trim( @" \\""".ToCharArray() ) + "\\" + FileName;

			try {

				bool exist = File.Exists( path );

				using ( StreamWriter sw = new StreamWriter( path, IsAppend, Encoding.Default ) ) {
					
					if ( !IsAppend || !exist )
						sw.WriteLine( RecordHeader );

					sw.Write( SaveLines() );
				}

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SaveErrorReport( ex, "レコード " + path + " の書き込みに失敗しました。" );
			
			}
		}


		
		/// <summary>
		/// ファイルから読み込んだデータを解析し、レコードに追加します。
		/// </summary>
		/// <param name="line">読み込んだ一行分のデータ。</param>
		protected abstract void LoadLine( string line );

		/// <summary>
		/// レコードのデータをファイルに書き込める文字列に変換します。
		/// </summary>
		/// <returns>ファイルに書き込む文字列。</returns>
		protected abstract string SaveLines();


		/// <summary>
		/// レコードをクリアします。ロード直前に呼ばれます。
		/// </summary>
		protected virtual void ClearRecord() { }


		/// <summary>
		/// ファイルに追記するかを指定します。
		/// </summary>
		protected virtual bool IsAppend { get { return false; } }

		/// <summary>
		/// レコードのヘッダを取得します。
		/// </summary>
		protected abstract string RecordHeader { get; }

		/// <summary>
		/// 保存するファイル名を取得します。
		/// </summary>
		public abstract string FileName { get; }

	}

}
