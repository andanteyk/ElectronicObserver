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

			
		}

		void Logger_LogAdded( Utility.Logger.LogData data ) {

			int index = LogList.Items.Add( data.ToString() );
			LogList.TopIndex = index;

		}

		protected override string GetPersistString() {
			return "Log";
		}
	}
}
