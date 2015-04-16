﻿using ElectronicObserver.Data;
using ElectronicObserver.Observer;
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

			public ResourceElement( int fuel, int ammo, int steel, int bauxite, int instantConstruction, int instantRepair, int developmentMaterial, int moddingMaterial, int hqLevel, int hqExp )
				: this() {
				Fuel = fuel;
				Ammo = ammo;
				Steel = steel;
				Bauxite = bauxite;
				InstantConstruction = instantConstruction;
				InstantRepair = instantRepair;
				DevelopmentMaterial = developmentMaterial;
				ModdingMaterial = moddingMaterial;
				HQLevel = hqLevel;
				HQExp = hqExp;
			}

			public override void LoadLine( string line ) {

				string[] elem = line.Split( ",".ToCharArray() );
                if (elem.Length < 11) throw new ArgumentException(LoadResources.getter("ResourceRecord_1"));

				Date = DateTimeHelper.CSVStringToTime( elem[0] );
				Fuel = int.Parse( elem[1] );
				Ammo = int.Parse( elem[2] );
				Steel = int.Parse( elem[3] );
				Bauxite = int.Parse( elem[4] );
				InstantConstruction = int.Parse( elem[5] );
				InstantRepair = int.Parse( elem[6] );
				DevelopmentMaterial = int.Parse( elem[7] );
				ModdingMaterial = int.Parse( elem[8] );
				HQLevel = int.Parse( elem[9] );
				HQExp = int.Parse( elem[10] );		

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
		private DateTime _prevTime;

		public ResourceRecord()
			: base() {

			Record = new List<ResourceElement>();
			_prevTime = DateTime.Now;

			APIObserver.Instance.APIList["api_port/port"].ResponseReceived += ResourceRecord_ResponseReceived;
		}


		void ResourceRecord_ResponseReceived( string apiname, dynamic data ) {

			if ( DateTimeHelper.IsCrossedHour( _prevTime ) ) {
				_prevTime = DateTime.Now;

				var material = KCDatabase.Instance.Material;
				var admiral = KCDatabase.Instance.Admiral;
				Record.Add( new ResourceElement( 
					material.Fuel,
					material.Ammo,
					material.Steel,
					material.Bauxite,
					material.InstantConstruction,
					material.InstantRepair,
					material.DevelopmentMaterial,
					material.ModdingMaterial,
					admiral.Level,
					admiral.Exp ) );
			}
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


		public override bool Save( string path ) {
			bool ret = base.Save( path );

			Record.Clear();
			return ret;
		}



		protected override string RecordHeader {
            get { return LoadResources.getter("ResourceRecord_2"); }
		}

		public override string FileName {
			get { return "ResourceRecord.csv"; }
		}
	}
}
