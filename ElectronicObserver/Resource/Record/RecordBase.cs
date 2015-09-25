using ElectronicObserver.Utility.Mathematics;
using ICSharpCode.SharpZipLib.Zip;
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
		/// アセットからデフォルトのレコードをコピーします。
		/// </summary>
		/// <param name="path">アセットファイルへのパス。</param>
		/// <param name="checkexist">ファイルの存在を確認するか。trueなら既にファイルが存在した場合上書きしません。</param>
		/// <returns>成功すれば true を返します。</returns>
		public bool CopyFromAssets( string path, bool checkexist = true ) {

			string destination = RecordManager.Instance.MasterPath + "\\" + FileName;

			if ( checkexist && File.Exists( destination ) ) {
				return false;
			}


			using ( var stream = File.OpenRead( path ) ) {

				using ( var archive = new ZipFile( stream ) ) {

					string entrypath = @"Assets/Record/" + FileName;

					var entry = archive.GetEntry( entrypath );

					if ( entry == null ) {
						Utility.Logger.Add( 3, string.Format( "デフォルトレコード {0} は存在しません。", entrypath ) );
						return false;
					}


					try {

						//entry.ExtractToFile( destination );
						using ( var fsout = File.OpenWrite( destination ) )
						using ( var fsin = archive.GetInputStream( entry ) ) {
							byte[] buffer = new byte[1024];
							int count;
							while ( ( count = fsin.Read( buffer, 0, 1024 ) ) > 0 ) {
								fsout.Write( buffer, 0, count );
							}
						}
						Utility.Logger.Add( 2, string.Format( "デフォルトレコード {0} をコピーしました。", FileName ) );

					} catch ( Exception ex ) {

						Utility.Logger.Add( 3, string.Format( "デフォルトレコード {0} のコピーに失敗しました。{1}", FileName, ex.Message ) );
						return false;
					}
				}
			}

			return true;
		}


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
