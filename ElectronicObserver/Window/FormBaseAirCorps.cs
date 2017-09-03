using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Support;
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {
	public partial class FormBaseAirCorps : DockContent {


		private class TableBaseAirCorpsControl {

			public ImageLabel Name;
			public ImageLabel ActionKind;
			public ImageLabel AirSuperiority;
			public ImageLabel Distance;
			public ShipStatusEquipment Squadrons;

			public ToolTip ToolTipInfo;

			public TableBaseAirCorpsControl( FormBaseAirCorps parent ) {

				#region Initialize

				Name = new ImageLabel();
				Name.Name = "Name";
				Name.Text = "*";
				Name.Anchor = AnchorStyles.Left;
				Name.TextAlign = ContentAlignment.MiddleLeft;
				Name.ImageAlign = ContentAlignment.MiddleRight;
				Name.ImageList = ResourceManager.Instance.Icons;
				Name.Padding = new Padding( 2, 2, 2, 2 );
				Name.Margin = new Padding( 2, 1, 2, 1 );		// ここを 2,0,2,0 にすると境界線の描画に問題が出るので
				Name.AutoSize = true;
				Name.ContextMenuStrip = parent.ContextMenuBaseAirCorps;
				Name.Visible = false;
				Name.Cursor = Cursors.Help;

				ActionKind = new ImageLabel();
				ActionKind.Text = "*";
				ActionKind.Anchor = AnchorStyles.Left;
				ActionKind.TextAlign = ContentAlignment.MiddleLeft;
				ActionKind.ImageAlign = ContentAlignment.MiddleCenter;
				//ActionKind.ImageList =
				ActionKind.Padding = new Padding( 2, 2, 2, 2 );
				ActionKind.Margin = new Padding( 2, 0, 2, 0 );
				ActionKind.AutoSize = true;
				ActionKind.Visible = false;

				AirSuperiority = new ImageLabel();
				AirSuperiority.Text = "*";
				AirSuperiority.Anchor = AnchorStyles.Left;
				AirSuperiority.TextAlign = ContentAlignment.MiddleLeft;
				AirSuperiority.ImageAlign = ContentAlignment.MiddleLeft;
				AirSuperiority.ImageList = ResourceManager.Instance.Equipments;
				AirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;
				AirSuperiority.Padding = new Padding( 2, 2, 2, 2 );
				AirSuperiority.Margin = new Padding( 2, 0, 2, 0 );
				AirSuperiority.AutoSize = true;
				AirSuperiority.Visible = false;

				Distance = new ImageLabel();
				Distance.Text = "*";
				Distance.Anchor = AnchorStyles.Left;
				Distance.TextAlign = ContentAlignment.MiddleLeft;
				Distance.ImageAlign = ContentAlignment.MiddleLeft;
				Distance.ImageList = ResourceManager.Instance.Icons;
				Distance.ImageIndex = (int)ResourceManager.IconContent.ParameterAircraftDistance;
				Distance.Padding = new Padding( 2, 2, 2, 2 );
				Distance.Margin = new Padding( 2, 0, 2, 0 );
				Distance.AutoSize = true;
				Distance.Visible = false;

				Squadrons = new ShipStatusEquipment();
				Squadrons.Anchor = AnchorStyles.Left;
				Squadrons.Padding = new Padding( 0, 1, 0, 2 );
				Squadrons.Margin = new Padding( 2, 0, 2, 0 );
				Squadrons.Size = new Size( 40, 20 );
				Squadrons.AutoSize = true;
				Squadrons.Visible = false;
				Squadrons.ResumeLayout();

				ConfigurationChanged( parent );

				ToolTipInfo = parent.ToolTipInfo;

				#endregion

			}


			public TableBaseAirCorpsControl( FormBaseAirCorps parent, TableLayoutPanel table, int row )
				: this( parent ) {
				AddToTable( table, row );
			}

			public void AddToTable( TableLayoutPanel table, int row ) {

				table.SuspendLayout();

				table.Controls.Add( Name, 0, row );
				table.Controls.Add( ActionKind, 1, row );
				table.Controls.Add( AirSuperiority, 2, row );
				table.Controls.Add( Distance, 3, row );
				table.Controls.Add( Squadrons, 4, row );
				table.ResumeLayout();

				ControlHelper.SetTableRowStyle( table, row, ControlHelper.GetDefaultRowStyle() );
			}


			public void Update( int baseAirCorpsID ) {

				KCDatabase db = KCDatabase.Instance;
				var corps = db.BaseAirCorps[baseAirCorpsID];

				if ( corps == null ) {
					baseAirCorpsID = -1;

				} else {

					Name.Text = string.Format( "#{0} - {1}", corps.MapAreaID, corps.Name );
					Name.Tag = corps.MapAreaID;
					var sb = new StringBuilder();


					string areaName = KCDatabase.Instance.MapArea.ContainsKey( corps.MapAreaID ) ? KCDatabase.Instance.MapArea[corps.MapAreaID].Name : "バミューダ海域";

					sb.AppendLine( "所属海域: " + areaName );

					// state 
					if ( corps.Squadrons.Values.Any( sq => sq != null && sq.AircraftCurrent < sq.AircraftMax ) ) {
						// 未補給
						Name.ImageAlign = ContentAlignment.MiddleRight;
						Name.ImageIndex = (int)ResourceManager.IconContent.FleetNotReplenished;
						sb.AppendLine( "未補給" );

					} else if ( corps.Squadrons.Values.Any( sq => sq != null && sq.Condition > 1 ) ) {
						// 疲労
						int tired = corps.Squadrons.Values.Max( sq => sq != null ? sq.Condition : 0 );

						if ( tired == 2 ) {
							Name.ImageAlign = ContentAlignment.MiddleRight;
							Name.ImageIndex = (int)ResourceManager.IconContent.ConditionTired;
							sb.AppendLine( "疲労" );

						} else {
							Name.ImageAlign = ContentAlignment.MiddleRight;
							Name.ImageIndex = (int)ResourceManager.IconContent.ConditionVeryTired;
							sb.AppendLine( "過労" );

						}

					} else {
						Name.ImageAlign = ContentAlignment.MiddleCenter;
						Name.ImageIndex = -1;

					}
					ToolTipInfo.SetToolTip( Name, sb.ToString() );


					ActionKind.Text = "[" + Constants.GetBaseAirCorpsActionKind( corps.ActionKind ) + "]";

					{
						int airSuperiority = Calculator.GetAirSuperiority( corps );
						if ( Utility.Configuration.Config.FormFleet.ShowAirSuperiorityRange ) {
							int airSuperiority_max = Calculator.GetAirSuperiority( corps, true );
							if ( airSuperiority < airSuperiority_max )
								AirSuperiority.Text = string.Format( "{0} ～ {1}", airSuperiority, airSuperiority_max );
							else
								AirSuperiority.Text = airSuperiority.ToString();
						} else {
							AirSuperiority.Text = airSuperiority.ToString();
						}

						ToolTipInfo.SetToolTip( AirSuperiority,
							string.Format( "確保: {0}\r\n優勢: {1}\r\n均衡: {2}\r\n劣勢: {3}\r\n",
							(int)( airSuperiority / 3.0 ),
							(int)( airSuperiority / 1.5 ),
							Math.Max( (int)( airSuperiority * 1.5 - 1 ), 0 ),
							Math.Max( (int)( airSuperiority * 3.0 - 1 ), 0 ) ) );
					}

					Distance.Text = corps.Distance.ToString();

					Squadrons.SetSlotList( corps );
					ToolTipInfo.SetToolTip( Squadrons, GetEquipmentString( corps ) );

				}


				Name.Visible =
				ActionKind.Visible =
				AirSuperiority.Visible =
				Distance.Visible =
				Squadrons.Visible =
					baseAirCorpsID != -1;
			}


			public void ConfigurationChanged( FormBaseAirCorps parent ) {

				var config = Utility.Configuration.Config;

				var mainfont = config.UI.MainFont;
				var subfont = config.UI.SubFont;

				Name.Font = mainfont;
				ActionKind.Font = mainfont;
				AirSuperiority.Font = mainfont;
				Distance.Font = mainfont;
				Squadrons.Font = subfont;

				Squadrons.ShowAircraft = config.FormFleet.ShowAircraft;
				Squadrons.ShowAircraftLevelByNumber = config.FormFleet.ShowAircraftLevelByNumber;
				Squadrons.LevelVisibility = config.FormFleet.EquipmentLevelVisibility;

			}


			private string GetEquipmentString( BaseAirCorpsData corps ) {
				var sb = new StringBuilder();

				if ( corps == null )
					return "(未開放)\r\n";

				foreach ( var squadron in corps.Squadrons.Values ) {
					if ( squadron == null )
						continue;

					var eq = squadron.EquipmentInstance;

					switch ( squadron.State ) {
						case 0:		// 未配属
						default:
							sb.AppendLine( "(なし)" );
							break;

						case 1:		// 配属済み
							if ( eq == null )
								goto case 0;
							sb.AppendFormat( "[{0}/{1}] ",
								squadron.AircraftCurrent,
								squadron.AircraftMax );

							switch ( squadron.Condition ) {
								case 1:
								default:
									break;
								case 2:
									sb.Append( "[疲労] " );
									break;
								case 3:
									sb.Append( "[過労] " );
									break;
							}

							sb.AppendFormat( "{0} (半径: {1})\r\n", eq.NameWithLevel, eq.MasterEquipment.AircraftDistance );
							break;

						case 2:		// 配置転換中
							sb.AppendFormat( "配置転換中 (開始時刻: {0})\r\n",
								DateTimeHelper.TimeToCSVString( squadron.RelocatedTime ) );
							break;
					}
				}

				return sb.ToString();
			}

		}


		private TableBaseAirCorpsControl[] ControlMember;

		public FormBaseAirCorps( FormMain parent ) {
			InitializeComponent();


			ControlMember = new TableBaseAirCorpsControl[9];
			TableMember.SuspendLayout();
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				ControlMember[i] = new TableBaseAirCorpsControl( this, TableMember, i );
			}
			TableMember.ResumeLayout();

			ConfigurationChanged();

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBaseAirCorps] );
		}

		private void FormBaseAirCorps_Load( object sender, EventArgs e ) {

			var api = Observer.APIObserver.Instance;

			api["api_port/port"].ResponseReceived += Updated;
			api["api_get_member/mapinfo"].ResponseReceived += Updated;
			api["api_get_member/base_air_corps"].ResponseReceived += Updated;
			api["api_req_air_corps/change_name"].ResponseReceived += Updated;
			api["api_req_air_corps/set_action"].ResponseReceived += Updated;
			api["api_req_air_corps/set_plane"].ResponseReceived += Updated;
			api["api_req_air_corps/supply"].ResponseReceived += Updated;
			api["api_req_air_corps/expand_base"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		}


		private void ConfigurationChanged() {

			var c = Utility.Configuration.Config;

			TableMember.SuspendLayout();

			Font = c.UI.MainFont;

			foreach ( var control in ControlMember )
				control.ConfigurationChanged( this );

			ControlHelper.SetTableRowStyles( TableMember, ControlHelper.GetDefaultRowStyle() );

			TableMember.ResumeLayout();

			if ( KCDatabase.Instance.BaseAirCorps.Any() )
				Updated( null, null );
		}


		void Updated( string apiname, dynamic data ) {

			var keys = KCDatabase.Instance.BaseAirCorps.Keys;

			if ( Utility.Configuration.Config.FormBaseAirCorps.ShowEventMapOnly ) {
				var eventAreaCorps = KCDatabase.Instance.BaseAirCorps.Values.Where( b => {
					var maparea = KCDatabase.Instance.MapArea[b.MapAreaID];
					return maparea != null && maparea.MapType == 1;
				} ).Select( b => b.ID );

				if ( eventAreaCorps.Any() )
					keys = eventAreaCorps;
			}


			TableMember.SuspendLayout();
			TableMember.RowCount = keys.Count();
			for ( int i = 0; i < ControlMember.Length; i++ ) {
				ControlMember[i].Update( i < keys.Count() ? keys.ElementAt( i ) : -1 );
			}
			TableMember.ResumeLayout();

			// set icon
			{
				var squadrons = KCDatabase.Instance.BaseAirCorps.Values.Where( b => b != null )
					.SelectMany( b => b.Squadrons.Values )
					.Where( s => s != null );
				bool isNotReplenished = squadrons.Any( s => s.State == 1 && s.AircraftCurrent < s.AircraftMax );
				bool isTired = squadrons.Any( s => s.State == 1 && s.Condition == 2 );
				bool isVeryTired = squadrons.Any( s => s.State == 1 && s.Condition == 3 );

				int imageIndex;

				if ( isNotReplenished )
					imageIndex = (int)ResourceManager.IconContent.FleetNotReplenished;
				else if ( isVeryTired )
					imageIndex = (int)ResourceManager.IconContent.ConditionVeryTired;
				else if ( isTired )
					imageIndex = (int)ResourceManager.IconContent.ConditionTired;
				else
					imageIndex = (int)ResourceManager.IconContent.FormBaseAirCorps;

				if ( Icon != null ) ResourceManager.DestroyIcon( Icon );
				Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[imageIndex] );
				if ( Parent != null ) Parent.Refresh();		//アイコンを更新するため
			}

		}


		private void ContextMenuBaseAirCorps_Opening( object sender, System.ComponentModel.CancelEventArgs e ) {
			if ( KCDatabase.Instance.BaseAirCorps.Count == 0 ) {
				e.Cancel = true;
				return;
			}

			if ( ContextMenuBaseAirCorps.SourceControl.Name == "Name" )
				ContextMenuBaseAirCorps_CopyOrganization.Tag = ContextMenuBaseAirCorps.SourceControl.Tag as int? ?? -1;
			else
				ContextMenuBaseAirCorps_CopyOrganization.Tag = -1;
		}

		private void ContextMenuBaseAirCorps_CopyOrganization_Click( object sender, EventArgs e ) {

			var sb = new StringBuilder();
			int areaid = ContextMenuBaseAirCorps_CopyOrganization.Tag as int? ?? -1;

			var baseaircorps = KCDatabase.Instance.BaseAirCorps.Values;
			if ( areaid != -1 )
				baseaircorps = baseaircorps.Where( c => c.MapAreaID == areaid );

			foreach ( var corps in baseaircorps ) {

				string areaName = KCDatabase.Instance.MapArea.ContainsKey( corps.MapAreaID ) ? KCDatabase.Instance.MapArea[corps.MapAreaID].Name : "バミューダ海域";

				sb.AppendFormat( "{0}\t[{1}] 制空戦力{2}/戦闘行動半径{3}\r\n",
					( areaid == -1 ? ( areaName + "：" ) : "" ) + corps.Name,
					Constants.GetBaseAirCorpsActionKind( corps.ActionKind ),
					Calculator.GetAirSuperiority( corps ),
					corps.Distance );

				var sq = corps.Squadrons.Values.ToArray();

				for ( int i = 0; i < sq.Length; i++ ) {
					if ( i > 0 )
						sb.Append( "/" );

					if ( sq[i] == null ) {
						sb.Append( "(消息不明)" );
						continue;
					}

					switch ( sq[i].State ) {
						case 0:
							sb.Append( "(未配属)" );
							break;
						case 1: {
								var eq = sq[i].EquipmentInstance;

								sb.Append( eq == null ? "(なし)" : eq.NameWithLevel );

								if ( sq[i].AircraftCurrent < sq[i].AircraftMax )
									sb.AppendFormat( "[{0}/{1}]", sq[i].AircraftCurrent, sq[i].AircraftMax );
							} break;
						case 2:
							sb.Append( "(配置転換中)" );
							break;
					}
				}

				sb.AppendLine();
			}

			Clipboard.SetData( DataFormats.StringFormat, sb.ToString() );
		}

		private void ContextMenuBaseAirCorps_DisplayRelocatedEquipments_Click( object sender, EventArgs e ) {

			string message = string.Join( "\r\n", KCDatabase.Instance.RelocatedEquipments.Values
				.Where( eq => eq.EquipmentInstance != null )
				.Select( eq => string.Format( "{0} ({1}～)", eq.EquipmentInstance.NameWithLevel, DateTimeHelper.TimeToCSVString( eq.RelocatedTime ) ) ) );

			if ( message.Length == 0 )
				message = "現在配置転換中の装備はありません。";

			MessageBox.Show( message, "配置転換中装備", MessageBoxButtons.OK, MessageBoxIcon.Information );
		}


		private void TableMember_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}

		protected override string GetPersistString() {
			return "BaseAirCorps";
		}




	}
}
