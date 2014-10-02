using ElectronicObserver.Data;
using ElectronicObserver.Observer;
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

	public partial class FormInformation : DockContent {


		public FormInformation( FormMain parent ) {
			InitializeComponent();
		
		
		}


		private void FormInformation_Load( object sender, EventArgs e ) {

			APIObserver o = APIObserver.Instance;

			APIReceivedEventHandler rec = ( string apiname, dynamic data ) => Invoke( new APIReceivedEventHandler( Updated ), apiname, data );

			o.ResponseList["api_port/port"].ResponseReceived += rec;
			o.ResponseList["api_req_member/get_practice_enemyinfo"].ResponseReceived += rec;
			o.ResponseList["api_get_member/picture_book"].ResponseReceived += rec;
			

		}


		void Updated( string apiname, dynamic data ) {

			//checkme: かなり書き方がきたなくなるが、どうしたものか…

			switch ( apiname ) {

				case "api_port/port":
					{
						//debug
						TextInformation.Text = "port : " + DateTime.Now.ToString() ;

					} break;

				case "api_req_member/get_practice_enemyinfo":
					{
						//int exp = 0;

						//undone: 現段階では面倒過ぎるので後日実装
					} break;

				case "api_get_member/picture_book":
					{
						StringBuilder sb = new StringBuilder();
						sb.AppendLine( "中破絵未回収：" );

						foreach ( dynamic elem in data.api_list ) {

							if ( !elem.IsDefined( "api_yomi" ) )
								break;		//ないほうは装備

							dynamic[] state = elem.api_state;
							for ( int i = 0; i < state.Length; i++ ) {
								if ( (int)state[i][1] == 0 ) {
									sb.AppendLine( KCDatabase.Instance.MasterShips[(int)elem.api_table_id[i]].Name );
								}
							}
						}

						TextInformation.Text = sb.ToString();
					} break;

			}

		}



		protected override string GetPersistString() {
			return "Information";
		}
	
	}

}
