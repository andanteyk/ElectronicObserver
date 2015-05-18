using ElectronicObserver.Resource;
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

	public partial class FormLog : DockContent {


		public FormLog( FormMain parent ) {
			InitializeComponent();

			ConfigurationChanged();
		}
		
		private void FormLog_Load( object sender, EventArgs e ) {

			foreach ( var log in Utility.Logger.Log ) {
				if ( log.Priority >= Utility.Configuration.Config.Log.LogLevel )
					LogList.Items.Add( log.ToString() );
			}
			LogList.TopIndex = LogList.Items.Count - 1;

			Utility.Logger.Instance.LogAdded += new Utility.LogAddedEventHandler( ( Utility.Logger.LogData data ) => {
				if ( InvokeRequired ) {
					// Invokeはメッセージキューにジョブを投げて待つので、別のBeginInvokeされたジョブが既にキューにあると、
					// それを実行してしまい、BeginInvokeされたジョブの順番が保てなくなる
					// GUIスレッドによる処理は、順番が重要なことがあるので、GUIスレッドからInvokeを呼び出してはいけない
					Invoke( new Utility.LogAddedEventHandler( Logger_LogAdded ), data );
				} else {
					Logger_LogAdded( data );
				}
			} );

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

			Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormLog] );
		}


		void ConfigurationChanged() {

			LogList.Font = Font = Utility.Configuration.Config.UI.MainFont;

			LogList.BackColor = Utility.Configuration.Config.UI.BackColor;
			LogList.ForeColor = Utility.Configuration.Config.UI.ForeColor;
		}


		void Logger_LogAdded( Utility.Logger.LogData data ) {

			int index = LogList.Items.Add( data.ToString() );
			LogList.TopIndex = index;

		}



		private void ContextMenuLog_Clear_Click( object sender, EventArgs e ) {

			LogList.Items.Clear();

		}



		protected override string GetPersistString() {
			return "Log";
		}

	
	}
}
