using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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


			public RecordElementBase() { }

			public RecordElementBase( string line )
				: this() {
				LoadLine( line );
			}
		}



		/// <summary>
		/// ファイルからレコードを読み込みます。
		/// </summary>
		/// <param name="path">ファイルが存在するフォルダのパス。</param>
		public virtual bool Load( string path ) {

			path = path.Trim( @" \\""".ToCharArray() ) + "\\" + FileName;

			try {

				using ( StreamReader sr = new StreamReader( path, Utility.Configuration.Config.Log.FileEncoding ) ) {

					ClearRecord();

					string line;
					int linecount = 1;
					sr.ReadLine();			//ヘッダを読み飛ばす

					while ( ( line = sr.ReadLine() ) != null ) {
						if ( line.Trim().StartsWith( "#" ) )
							continue;

						/*/
						// こちらのほうがよさげだが、エラーが多すぎた場合プログラムが起動できなくなるので自粛
						
						try {
							LoadLine( line );

						} catch ( Exception ex ) {
							Utility.Logger.Add( 3, string.Format( "{0}: エラーが発生したため行 {1} をスキップしました。 {2}", path, linecount, ex.Message ) );
						}
						
						/*/

						LoadLine( line );

						//*/

						linecount++;
					}

				}

				return true;

			} catch ( FileNotFoundException ) {

				Utility.Logger.Add( 1, "记录 " + path + " 不存在。" );


			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "记录 " + path + " 读取失败。" );

			}

			return false;
		}


		/// <summary>
		/// ファイルにレコードを書き込みます。
		/// </summary>
		/// <param name="path">ファイルが存在するフォルダのパス。</param>
		public virtual bool Save( string path ) {

			path = path.Trim( @" \\""".ToCharArray() ) + "\\" + FileName;

			try {

				bool exist = File.Exists( path );

				using ( StreamWriter sw = new StreamWriter( path, IsAppend, Utility.Configuration.Config.Log.FileEncoding ) ) {

					if ( !IsAppend || !exist )
						sw.WriteLine( RecordHeader );

					sw.Write( SaveLines() );
				}

				return true;

			} catch ( Exception ex ) {

				Utility.ErrorReporter.SendErrorReport( ex, "レコード " + path + " の書き込みに失敗しました。" );

			}

			return false;
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
		public abstract string RecordHeader { get; }

		/// <summary>
		/// 保存するファイル名を取得します。
		/// </summary>
		public abstract string FileName { get; }

	}

}
