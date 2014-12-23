using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {

	public partial class FormCompass : DockContent {


		private class TableEnemyMemberControl {

			public ImageLabel ShipName;
			public ShipStatusEquipment Equipments;

			public ToolTip ToolTipInfo;


			public TableEnemyMemberControl( FormCompass parent ) {

				#region Initialize

				ShipName = new ImageLabel();
				ShipName.Anchor = AnchorStyles.Left;
				ShipName.Font = parent.MainFont;
				ShipName.ForeColor = parent.MainFontColor;
				ShipName.ImageAlign = ContentAlignment.MiddleCenter;
				ShipName.Padding = new Padding( 0, 1, 0, 1 );
				ShipName.Margin = new Padding( 2, 0, 2, 0 );
				ShipName.MaximumSize = new Size( 60, 20 );
				ShipName.AutoEllipsis = true;
				ShipName.AutoSize = true;
				ShipName.Cursor = Cursors.Help;
				ShipName.MouseClick += ShipName_MouseClick;

				Equipments = new ShipStatusEquipment();
				Equipments.SuspendLayout();
				Equipments.Anchor = AnchorStyles.Left;
				Equipments.Font = parent.SubFont;
				Equipments.Padding = new Padding( 0, 2, 0, 1 );
				Equipments.Margin = new Padding( 2, 0, 2, 0 );
				Equipments.Size = new Size( 40, 20 );	//checkme: 要る？
				Equipments.AutoSize = true;
				Equipments.ResumeLayout();

				ToolTipInfo = parent.ToolTipInfo;
				#endregion

			}

		
			public TableEnemyMemberControl( FormCompass parent, TableLayoutPanel table, int row )
				: this( parent ) {

				AddToTable( table, row );
			}

			public void AddToTable( TableLayoutPanel table, int row ) {

				table.Controls.Add( ShipName, 0, row );
				table.Controls.Add( Equipments, 1, row );

				#region set RowStyle
				RowStyle rs = new RowStyle( SizeType.Absolute, 21 );

				if ( table.RowStyles.Count > row )
					table.RowStyles[row] = rs;
				else
					while ( table.RowStyles.Count <= row )
						table.RowStyles.Add( rs );
				#endregion
			}



			public void Update( int shipID ) {
				Update( shipID, shipID != -1 ? KCDatabase.Instance.MasterShips[shipID].DefaultSlot.ToArray() : null );
			}

			//fixme: slotがnullだと間違いなく死ぬ
			public void Update( int shipID, int[] slot ) {

				ShipName.Tag = shipID;

				if ( shipID == -1 ) {
					//なし
					ShipName.Text = "-";
					ShipName.ForeColor = Color.FromArgb( 0x00, 0x00, 0x00 );
					Equipments.Visible = false;
					ToolTipInfo.SetToolTip( ShipName, null );
					ToolTipInfo.SetToolTip( Equipments, null );

				} else {

					ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];


					ShipName.Text = ship.Name;
					switch ( ship.AbyssalShipClass ) {
						case 0:
						case 1:		//normal
							ShipName.ForeColor = Color.FromArgb( 0x00, 0x00, 0x00 ); break;
						case 2:		//elite
							ShipName.ForeColor = Color.FromArgb( 0xFF, 0x00, 0x00 ); break;
						case 3:		//flagship
							ShipName.ForeColor = Color.FromArgb( 0xFF, 0x88, 0x00 ); break;
						case 4:		//latemodel
							ShipName.ForeColor = Color.FromArgb( 0x00, 0x88, 0xFF ); break;
					}

					Equipments.SetSlotList( shipID, slot );
					Equipments.Visible = true;
					ToolTipInfo.SetToolTip( ShipName, ship.NameWithClass );
					ToolTipInfo.SetToolTip( Equipments, GetEquipmentString( shipID, slot ) );
				}

			}

			private string GetEquipmentString( int shipID, int[] slot ) {
				StringBuilder sb = new StringBuilder();
				ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];

				for ( int i = 0; i < slot.Length; i++ ) {
					if ( slot[i] != -1 )
						sb.AppendFormat( "[{0}] {1}\r\n", ship.Aircraft[i], KCDatabase.Instance.MasterEquipments[slot[i]].Name );
				}

				return sb.ToString();
			}


			void ShipName_MouseClick( object sender, MouseEventArgs e ) {

				if ( ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 ) {
					if ( ShipName.Tag != null )
						new DialogAlbumMasterShip( (int)ShipName.Tag ).Show();
				}

			}


		}



		public Font MainFont { get; set; }
		public Font SubFont { get; set; }
		public Color MainFontColor { get; set; }
		public Color SubFontColor { get; set; }


		private TableEnemyMemberControl[] ControlMember;


		
		public FormCompass( FormMain parent ) {
			InitializeComponent();


			//todo: 後々外部から設定できるように
			MainFont = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			SubFont = new Font( "Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel );
			MainFontColor = Color.FromArgb( 0x00, 0x00, 0x00 );
			SubFontColor = Color.FromArgb( 0x88, 0x88, 0x88 );


			ControlHelper.SetDoubleBuffered( BasePanel );
			ControlHelper.SetDoubleBuffered( TableEnemyFleet );
			ControlHelper.SetDoubleBuffered( TableEnemyMember );


			TableEnemyMember.SuspendLayout();
			ControlMember = new TableEnemyMemberControl[6];
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				ControlMember[i] = new TableEnemyMemberControl( this, TableEnemyMember, i );
			}
			TableEnemyMember.ResumeLayout();
			

			//BasePanel.SetFlowBreak( TextMapArea, true );
			BasePanel.SetFlowBreak( TextDestination, true );
			//BasePanel.SetFlowBreak( TextEventKind, true );
			BasePanel.SetFlowBreak( TextEventDetail, true );

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.HQCompass] );

		}


		private void FormCompass_Load( object sender, EventArgs e ) {

			BasePanel.Visible = false;
			TextAirSuperiority.ImageList = ResourceManager.Instance.Equipments;
			TextAirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;


			Font = MainFont;
			TextMapArea.Font = MainFont;
			TextDestination.Font = MainFont;
			TextEventKind.Font = MainFont;
			TextEventDetail.Font = MainFont;


			APIObserver o = APIObserver.Instance;

			APIReceivedEventHandler rec = ( string apiname, dynamic data ) => Invoke( new APIReceivedEventHandler( Updated ), apiname, data );

			o.APIList["api_port/port"].ResponseReceived += rec;
			o.APIList["api_req_map/start"].ResponseReceived += rec;
			o.APIList["api_req_map/next"].ResponseReceived += rec;
			o.APIList["api_req_member/get_practice_enemyinfo"].ResponseReceived += rec;

			APIReceivedEventHandler rec2 = ( string apiname, dynamic data ) => Invoke( new APIReceivedEventHandler( BattleStarted ), apiname, data );

			o.APIList["api_req_sortie/battle"].ResponseReceived += rec2;
			o.APIList["api_req_battle_midnight/sp_midnight"].ResponseReceived += rec2;
			o.APIList["api_req_combined_battle/battle"].ResponseReceived += rec2;
			o.APIList["api_req_combined_battle/sp_midnight"].ResponseReceived += rec2;
			o.APIList["api_req_combined_battle/airbattle"].ResponseReceived += rec2;
			o.APIList["api_req_combined_battle/battle_water"].ResponseReceived += rec2;
			o.APIList["api_req_practice/battle"].ResponseReceived += rec2;

		}


		private void Updated( string apiname, dynamic data ) {

			if ( apiname == "api_port/port" ) {

				BasePanel.Visible = false;

			} else if ( apiname == "api_req_member/get_practice_enemyinfo" ) {

				TextMapArea.Text = "演習";
				TextDestination.Text = string.Format( "{0} {1}", data.api_nickname, Constants.GetAdmiralRank( (int)data.api_rank ) );
				TextEventKind.Text = data.api_cmt;
				TextEventDetail.Text = string.Format( "Lv. {0} / {1} exp.", data.api_level, data.api_experience[0] );
				TextEnemyFleetName.Text = data.api_deckname;

			} else {

				CompassData compass = KCDatabase.Instance.Battle.Compass;


				BasePanel.SuspendLayout();
				PanelEnemyFleet.Visible = false;

				TextMapArea.Text = "出撃海域 : " + compass.MapAreaID + "-" + compass.MapInfoID;
				TextDestination.Text = "次のセル : " + compass.Destination + ( compass.IsEndPoint ? " (終点)" : "" );
				
				{
					string eventkind = "";
					switch ( compass.EventID ) {
						case 0:
							eventkind += "初期位置";
							TextEventDetail.Text = "どうしてこうなった";
							break;
						case 2:
							eventkind += "資源";
							{
								string materialname;
								if ( compass.GetItemID == 4 ) {		//"※"　大方資源専用ID

									materialname = MaterialData.GetMaterialName( compass.GetItemIDMetadata );
								
								} else {
									materialname = KCDatabase.Instance.MasterUseItems[compass.GetItemID].Name;
								}

								TextEventDetail.Text = materialname + " x " + compass.GetItemAmount;
							}

							break;
						case 3:
							eventkind += "渦潮";
							{
								string materialname = MaterialData.GetMaterialName( compass.WhirlpoolItemID );

								//fixme:第一艦隊以外の艦隊が出撃していた場合誤った値を返す
								int materialmax = KCDatabase.Instance.Fleet.Fleets[1].Members.Max( n => 
								{
									if ( n != -1 )
										if ( compass.WhirlpoolItemID == 1 )
											return KCDatabase.Instance.Ships[n].MasterShip.Fuel;
										else if ( compass.WhirlpoolItemID == 2 )
											return KCDatabase.Instance.Ships[n].MasterShip.Ammo;
										else return 0;
									else return 0;
								} );

								int percent = compass.WhirlpoolItemAmount * 100 / Math.Max( materialmax, 1 );

								TextEventDetail.Text = materialname + " x " + compass.WhirlpoolItemAmount + " (" + percent + "%)";
							}
							break;
						case 4:
							eventkind += "通常戦闘";
							UpdateEnemyFleet( compass.EnemyFleetID );
							break;
						case 5:
							eventkind += "ボス戦闘";
							UpdateEnemyFleet( compass.EnemyFleetID );
							break;
						case 6:
							eventkind += "気のせいだった";
							TextEventDetail.Text = "";
							break;
						case 7:
							eventkind += "機動部隊航空戦";
							UpdateEnemyFleet( compass.EnemyFleetID );
							break;
						default:
							eventkind += "不明";
							TextEventDetail.Text = "";
							break;
					}
					TextEventKind.Text = eventkind;
				}

				BasePanel.ResumeLayout();

				BasePanel.Visible = true;
			}


		}


		private void BattleStarted( string apiname, dynamic data ) {
			UpdateEnemyFleetInstant();
		}



		private void UpdateEnemyFleet( int fleetID ) {

			TextEventDetail.Text = string.Format( "敵艦隊ID : {0}", fleetID );


			var efleet = RecordManager.Instance.EnemyFleet;
			
			if ( !efleet.Record.ContainsKey( fleetID ) ) {

				//unknown
				TextEnemyFleetName.Text = "(敵艦隊情報不明)";
				TextFormation.Visible = false;
				TextAirSuperiority.Visible = false;
				TableEnemyMember.Visible = false;

			} else {

				var fdata = efleet[fleetID];

				TextEnemyFleetName.Text = fdata.FleetName;
				TextFormation.Text = Constants.GetFormationShort( fdata.Formation );
				TextFormation.Visible = true;
				TextAirSuperiority.Text = GetAirSuperiority( fdata.FleetMember ).ToString();
				TextAirSuperiority.Visible = true;

				TableEnemyMember.SuspendLayout();
				for ( int i = 0; i < ControlMember.Length; i++ ) {
					ControlMember[i].Update( fdata.FleetMember[i] );
				}
				TableEnemyMember.ResumeLayout();
				TableEnemyMember.Visible = true;

			}

			PanelEnemyFleet.Visible = true;

		}


		private void UpdateEnemyFleetInstant() {

			BattleManager bm = KCDatabase.Instance.Battle;
			BattleData bd;

			switch ( bm.BattleMode & BattleManager.BattleModes.BattlePhaseMask ) {
				case BattleManager.BattleModes.NightOnly:
				case BattleManager.BattleModes.NightDay:
					bd = bm.BattleNight;
					break;
				default:
					bd = bm.BattleDay;
					break;
			}

			TextFormation.Text = Constants.GetFormationShort( (int)bd.Data.api_formation[1] );
			TextFormation.Visible = true;
			TextAirSuperiority.Text = GetAirSuperiority( ( (int[])bd.Data.api_ship_ke ).Skip( 1 ).ToArray(), (int[][])bd.Data.api_eSlot ).ToString();
			TextAirSuperiority.Visible = true;

			TableEnemyMember.SuspendLayout();
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				int shipID = (int)bd.Data.api_ship_ke[i + 1];
				ControlMember[i].Update( shipID, shipID != -1 ? (int[])bd.Data.api_eSlot[i] : null );
			}
			TableEnemyMember.ResumeLayout();
			TableEnemyMember.Visible = true;

			PanelEnemyFleet.Visible = true;
			BasePanel.Visible = true;			//checkme

		}


		//これ、どうにかならないものだろうか…
		//纏めるにしてもつらいし
		private int GetAirSuperiority( int[] fleet ) {

			int airSuperiority = 0;

			for ( int i = 0; i < fleet.Length; i++ ) {

				if ( fleet[i] == -1 )
					continue;

				ShipDataMaster ship = KCDatabase.Instance.MasterShips[fleet[i]];

				if ( ship.DefaultSlot == null )
					continue;

				for ( int s = 0; s < ship.DefaultSlot.Count; s++ ) {
					if ( ship.DefaultSlot[s] == -1 )
						continue;

					EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[ship.DefaultSlot[s]];
					if ( eq == null )
						continue;

					switch ( eq.EquipmentType[2] ) {
						case 6:		//艦戦
						case 7:		//艦爆
						case 8:		//艦攻
						case 11:	//水爆
							airSuperiority += (int)( eq.AA * Math.Sqrt( ship.Aircraft[s] ) );
							break;
					}
				}
			}


			return airSuperiority;
		}


		//fixme: 暫定版　いずれどこかに纏めておく…
		private int GetAirSuperiority( int[] fleet, int[][] slot ) {

			int airSuperiority = 0;

			for ( int i = 0; i < fleet.Length; i++ ) {

				if ( fleet[i] == -1 )
					continue;

				ShipDataMaster ship = KCDatabase.Instance.MasterShips[fleet[i]];

				if ( ship == null || slot[i] == null )
					continue;

				for ( int s = 0; s < slot[i].Length; s++ ) {
					if ( slot[i][s] == -1 )
						continue;

					EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[slot[i][s]];
					if ( eq == null )
						continue;

					switch ( eq.EquipmentType[2] ) {
						case 6:		//艦戦
						case 7:		//艦爆
						case 8:		//艦攻
						case 11:	//水爆
							airSuperiority += (int)( eq.AA * Math.Sqrt( ship.Aircraft[s] ) );
							break;
					}
				}
			}

			return airSuperiority;
		}


		//for debug
		[Obsolete]
		private string GetEnemyFleetInformation( int fleetID ) {

			StringBuilder sb = new StringBuilder();
			var efleet = RecordManager.Instance.EnemyFleet;

			sb.AppendFormat( "敵艦隊ID : {0}\r\n", fleetID );


			if ( !efleet.Record.ContainsKey( fleetID ) ) {

				sb.AppendLine( "(敵艦隊情報不明)" );

			} else {

				var fdata = efleet[fleetID];

				if ( fdata.FleetName != null )
					sb.Append( fdata.FleetName + " - " );

				sb.AppendLine( Constants.GetFormationShort( fdata.Formation ) );


				int[] fmembers = fdata.FleetMember;

				for ( int i = 0; i < fmembers.Length; i++ ) {
					if ( fmembers[i] == -1 ) continue;

					ShipDataMaster ship = KCDatabase.Instance.MasterShips[fmembers[i]];
					sb.AppendLine( ship.NameWithClass );
				}
			}

			return sb.ToString();

		}
 


		protected override string GetPersistString() {
			return "Compass";
		}

		private void TableEnemyMember_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}

	}

}
