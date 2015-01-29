using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Resource.Record {
	
	//undone
	public class ResourceRecord : RecordBase {

		public class ResourceElement : RecordElementBase {

			/// <summary>
			/// 記録日時
			/// </summary>
			public DateTime Date { get; set; }

			/// <summary>
			/// 燃料
			/// </summary>
			public int Fuel { get; set; }

			/// <summary>
			/// 弾薬
			/// </summary>
			public int Ammo { get; set; }

			/// <summary>
			/// 鋼材
			/// </summary>
			public int Steel { get; set; }

			/// <summary>
			/// ボーキサイト
			/// </summary>
			public int Bauxite { get; set; }


			/// <summary>
			/// 高速建造材
			/// </summary>
			public int InstantConstruction { get; set; }

			/// <summary>
			/// 高速修復材
			/// </summary>
			public int InstantRepair { get; set; }

			/// <summary>
			/// 開発資材
			/// </summary>
			public int DevelopmentMaterial { get; set; }

			/// <summary>
			/// 改修資材
			/// </summary>
			public int ModdingMaterial { get; set; }

			/// <summary>
			/// 艦隊司令部Lv.
			/// </summary>
			public int HQLevel { get; set; }

			/// <summary>
			/// 提督経験値
			/// </summary>
			public int HQExp { get; set; }


			public ResourceElement() {
				Date = DateTime.Now;
			}

			public ResourceElement( string line ) 
			 : base( line ){}

			//ctor


			public override void LoadLine( string line ) {
				throw new NotImplementedException();
			}

			public override string SaveLine() {
				return string.Format( "{" + string.Join( "},{", Enumerable.Range( 0, 11 ) ) + "}",
					DateTimeHelper.TimeToCSVString( Date ),
					Fuel,
					Ammo,
					Steel,
					Bauxite,
					InstantConstruction,
					InstantRepair,
					DevelopmentMaterial,
					ModdingMaterial,
					HQLevel,
					HQExp );
			}

		}


		public List<ResourceElement> Record { get; private set; }


		public ResourceRecord()
			: base() {

			Record = new List<ResourceElement>();


			//undone: api register
			//SystemEvents.UpdateTimerTick += SystemEvents_UpdateTimerTick;
			
		}


		void SystemEvents_UpdateTimerTick() {
			throw new NotImplementedException();
		}


		public ResourceElement this[int i] {
			get { return Record[i]; }
			set { Record[i] = value; }
		}


		protected override void LoadLine( string line ) {
			Record.Add( new ResourceElement( line ) );
		}

		protected override string SaveLines() {

			StringBuilder sb = new StringBuilder();

			var list = new List<ResourceElement>( Record );
			list.Sort( ( e1, e2 ) => e1.Date.CompareTo( e2.Date ) );

			foreach ( var elem in list ) {
				sb.AppendLine( elem.SaveLine() );
			}

			return sb.ToString();
		}


		protected override void ClearRecord() {
			Record.Clear();
		}

		protected override bool IsAppend { get { return true; } }


		public override void Save( string path ) {
			base.Save( path );

			Record.Clear();
		}



		protected override string RecordHeader {
			get { return "日時,燃料,弾薬,鋼材,ボーキサイト,高速建造材,高速修復材,開発資材,改修資材,司令部Lv,提督Exp"; }
		}

		public override string FileName {
			get { return "ResourceRecord.csv"; }
		}
	}
}
