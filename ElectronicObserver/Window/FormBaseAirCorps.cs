using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
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
	public partial class FormBaseAirCorps : DockContent {

		public FormBaseAirCorps( FormMain parent ) {
			InitializeComponent();

			ConfigurationChanged();
		}

		private void FormBaseAirCorps_Load( object sender, EventArgs e ) {

			var api = Observer.APIObserver.Instance;

			api["api_port/port"].ResponseReceived += Updated;
			api["api_get_member/base_air_corps"].ResponseReceived += Updated;
			api["api_req_air_corps/change_name"].ResponseReceived += Updated;
			api["api_req_air_corps/set_action"].ResponseReceived += Updated;
			api["api_req_air_corps/set_plane"].ResponseReceived += Updated;
			api["api_req_air_corps/supply"].ResponseReceived += Updated;

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		}


		private void ConfigurationChanged() {
			// undone
		}


		void Updated( string apiname, dynamic data ) {

			var sb = new StringBuilder();

			var aircorps = KCDatabase.Instance.BaseAirCorps.Values;

			foreach ( var corp in aircorps ) {
				sb.AppendFormat( "{0} [{1}] 制空: {2}\r\n", corp.Name, Constants.GetBaseAirCorpsActionKind( corp.ActionKind ), Calculator.GetAirSuperiority( corp ) );

				foreach ( var sq in corp.Squadrons.Values ) {
					var eq = sq.EquipmentInstance;
					switch ( sq.State ) {
						case 0:
							sb.AppendLine( "　　(未配属)" );
							break;
						case 1:
							sb.AppendFormat( "　　{0} {1}/{2}\r\n", eq != null ? eq.NameWithLevel : "(未配属)", sq.AircraftCurrent, sq.AircraftMax );
							break;
						case 2:
							sb.AppendFormat( "　　(配置転換中) {0}\r\n", DateTimeHelper.TimeToCSVString( sq.RelocatedTime ) );
							break;

					}
				}
				sb.AppendLine();
			}

			sb.AppendLine();

			var relocated = BaseAirCorpsData.RelocatedEquipments;
			if ( relocated.Any() ) {
				sb.AppendLine( "配置転換中装備：" );
				foreach ( var eqid in relocated ) {
					var eq = KCDatabase.Instance.Equipments[eqid];
					sb.AppendFormat( "　　{0} {1}\r\n", eq.NameWithLevel, eq.RelocatedTime );
				}
			}

			label1.Text = sb.ToString();
		}



		protected override string GetPersistString() {
			return "BaseAirCorps";
		}


	}
}
