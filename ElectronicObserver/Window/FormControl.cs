using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Integrate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window {

	public partial class FormControl : DockContent {

		FormMain mainForm;

		public FormControl( FormMain parent ) {
			InitializeComponent();

			this.mainForm = parent;
			this.Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormConfiguration] );
			this.windowCaptureButton.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormConfiguration];
		}

		private void windowCaptureButton_WindowCaptured( IntPtr hWnd ) {

			int capacity = WinAPI.GetWindowTextLength( hWnd ) * 2;
			StringBuilder stringBuilder = new StringBuilder( capacity );
			WinAPI.GetWindowText( hWnd, stringBuilder, stringBuilder.Capacity );

		    var result = MessageBox.Show(stringBuilder.ToString() + "\r\n" + 
				"このウィンドウをキャプチャします。よろしいですか？",
				SoftwareInformation.SoftwareNameJapanese, MessageBoxButtons.YesNoCancel);

			if ( result == System.Windows.Forms.DialogResult.Yes ) {
				FormIntegrated form = new FormIntegrated( mainForm );
				form.Show( hWnd );
			}
		}

	}

}
