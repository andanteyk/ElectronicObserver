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

			Font = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			LogList.Font = Font;

			ElectronicObserver.Utility.Logger.Instance.LogAdded += new Utility.LogAddedEventHandler( ( Utility.Logger.LogData data ) => Invoke( new Utility.LogAddedEventHandler( Logger_LogAdded ), data ) );
		}
		
		private void FormLog_Load( object sender, EventArgs e ) {

			foreach ( var log in Utility.Logger.Log ) {
				LogList.Items.Add( log.ToString() );
			}
			LogList.TopIndex = LogList.Items.Count - 1;
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
