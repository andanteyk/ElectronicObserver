namespace ElectronicObserver.Window.Dialog {
	partial class DialogConfiguration {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing ) {
			if ( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.label4 = new System.Windows.Forms.Label();
			this.Connection_PanelSaveData = new System.Windows.Forms.Panel();
			this.Connection_SaveOtherFile = new System.Windows.Forms.CheckBox();
			this.Connection_SaveSWF = new System.Windows.Forms.CheckBox();
			this.Connection_SaveResponse = new System.Windows.Forms.CheckBox();
			this.Connection_SaveRequest = new System.Windows.Forms.CheckBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.Connection_SaveDataPath = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.Connection_SaveDataFilter = new System.Windows.Forms.TextBox();
			this.Connection_SaveReceivedData = new System.Windows.Forms.CheckBox();
			this.Connection_Port = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.label5 = new System.Windows.Forms.Label();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.Log_SaveLogFlag = new System.Windows.Forms.CheckBox();
			this.Log_LogLevel = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.Control_ConditionBorder = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
			this.ButtonOK = new System.Windows.Forms.Button();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.Connection_PanelSaveData.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Connection_Port)).BeginInit();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Log_LogLevel)).BeginInit();
			this.tabPage4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Control_ConditionBorder)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(400, 259);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.label4);
			this.tabPage1.Controls.Add(this.Connection_PanelSaveData);
			this.tabPage1.Controls.Add(this.Connection_SaveReceivedData);
			this.tabPage1.Controls.Add(this.Connection_Port);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Location = new System.Drawing.Point(4, 24);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(392, 231);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "通信";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(140, 36);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(201, 15);
			this.label4.TabIndex = 5;
			this.label4.Text = "＊膨大なサイズになる可能性があります。";
			// 
			// Connection_PanelSaveData
			// 
			this.Connection_PanelSaveData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Connection_PanelSaveData.Controls.Add(this.Connection_SaveOtherFile);
			this.Connection_PanelSaveData.Controls.Add(this.Connection_SaveSWF);
			this.Connection_PanelSaveData.Controls.Add(this.Connection_SaveResponse);
			this.Connection_PanelSaveData.Controls.Add(this.Connection_SaveRequest);
			this.Connection_PanelSaveData.Controls.Add(this.button1);
			this.Connection_PanelSaveData.Controls.Add(this.label3);
			this.Connection_PanelSaveData.Controls.Add(this.Connection_SaveDataPath);
			this.Connection_PanelSaveData.Controls.Add(this.label2);
			this.Connection_PanelSaveData.Controls.Add(this.Connection_SaveDataFilter);
			this.Connection_PanelSaveData.Location = new System.Drawing.Point(8, 60);
			this.Connection_PanelSaveData.Name = "Connection_PanelSaveData";
			this.Connection_PanelSaveData.Size = new System.Drawing.Size(376, 83);
			this.Connection_PanelSaveData.TabIndex = 4;
			// 
			// Connection_SaveOtherFile
			// 
			this.Connection_SaveOtherFile.AutoSize = true;
			this.Connection_SaveOtherFile.Location = new System.Drawing.Point(234, 61);
			this.Connection_SaveOtherFile.Name = "Connection_SaveOtherFile";
			this.Connection_SaveOtherFile.Size = new System.Drawing.Size(59, 19);
			this.Connection_SaveOtherFile.TabIndex = 11;
			this.Connection_SaveOtherFile.Text = "Other";
			this.ToolTipInfo.SetToolTip(this.Connection_SaveOtherFile, "すべての通信ファイルを保存します。");
			this.Connection_SaveOtherFile.UseVisualStyleBackColor = true;
			// 
			// Connection_SaveSWF
			// 
			this.Connection_SaveSWF.AutoSize = true;
			this.Connection_SaveSWF.Location = new System.Drawing.Point(175, 61);
			this.Connection_SaveSWF.Name = "Connection_SaveSWF";
			this.Connection_SaveSWF.Size = new System.Drawing.Size(53, 19);
			this.Connection_SaveSWF.TabIndex = 10;
			this.Connection_SaveSWF.Text = "SWF";
			this.ToolTipInfo.SetToolTip(this.Connection_SaveSWF, "SWFファイルを保存します。");
			this.Connection_SaveSWF.UseVisualStyleBackColor = true;
			// 
			// Connection_SaveResponse
			// 
			this.Connection_SaveResponse.AutoSize = true;
			this.Connection_SaveResponse.Location = new System.Drawing.Point(88, 61);
			this.Connection_SaveResponse.Name = "Connection_SaveResponse";
			this.Connection_SaveResponse.Size = new System.Drawing.Size(81, 19);
			this.Connection_SaveResponse.TabIndex = 9;
			this.Connection_SaveResponse.Text = "Response";
			this.ToolTipInfo.SetToolTip(this.Connection_SaveResponse, "APIのResponse部を保存します。");
			this.Connection_SaveResponse.UseVisualStyleBackColor = true;
			// 
			// Connection_SaveRequest
			// 
			this.Connection_SaveRequest.AutoSize = true;
			this.Connection_SaveRequest.Location = new System.Drawing.Point(9, 61);
			this.Connection_SaveRequest.Name = "Connection_SaveRequest";
			this.Connection_SaveRequest.Size = new System.Drawing.Size(73, 19);
			this.Connection_SaveRequest.TabIndex = 8;
			this.Connection_SaveRequest.Text = "Request";
			this.ToolTipInfo.SetToolTip(this.Connection_SaveRequest, "APIのRequest部を保存します。");
			this.Connection_SaveRequest.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(341, 32);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(32, 23);
			this.button1.TabIndex = 7;
			this.button1.Text = "...";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 35);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 15);
			this.label3.TabIndex = 6;
			this.label3.Text = "保存先：";
			// 
			// Connection_SaveDataPath
			// 
			this.Connection_SaveDataPath.AllowDrop = true;
			this.Connection_SaveDataPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Connection_SaveDataPath.Location = new System.Drawing.Point(67, 32);
			this.Connection_SaveDataPath.Name = "Connection_SaveDataPath";
			this.Connection_SaveDataPath.Size = new System.Drawing.Size(268, 23);
			this.Connection_SaveDataPath.TabIndex = 5;
			this.Connection_SaveDataPath.TextChanged += new System.EventHandler(this.Connection_SaveDataPath_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 6);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(52, 15);
			this.label2.TabIndex = 4;
			this.label2.Text = "フィルタ：";
			// 
			// Connection_SaveDataFilter
			// 
			this.Connection_SaveDataFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Connection_SaveDataFilter.Enabled = false;
			this.Connection_SaveDataFilter.Location = new System.Drawing.Point(67, 3);
			this.Connection_SaveDataFilter.Name = "Connection_SaveDataFilter";
			this.Connection_SaveDataFilter.ReadOnly = true;
			this.Connection_SaveDataFilter.Size = new System.Drawing.Size(268, 23);
			this.Connection_SaveDataFilter.TabIndex = 3;
			this.ToolTipInfo.SetToolTip(this.Connection_SaveDataFilter, "＊未実装です＊");
			// 
			// Connection_SaveReceivedData
			// 
			this.Connection_SaveReceivedData.AutoSize = true;
			this.Connection_SaveReceivedData.Location = new System.Drawing.Point(8, 35);
			this.Connection_SaveReceivedData.Name = "Connection_SaveReceivedData";
			this.Connection_SaveReceivedData.Size = new System.Drawing.Size(126, 19);
			this.Connection_SaveReceivedData.TabIndex = 2;
			this.Connection_SaveReceivedData.Text = "通信内容を保存する";
			this.Connection_SaveReceivedData.UseVisualStyleBackColor = true;
			this.Connection_SaveReceivedData.CheckedChanged += new System.EventHandler(this.Connection_SaveReceivedData_CheckedChanged);
			// 
			// Connection_Port
			// 
			this.Connection_Port.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.Connection_Port.Location = new System.Drawing.Point(61, 6);
			this.Connection_Port.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.Connection_Port.Name = "Connection_Port";
			this.Connection_Port.Size = new System.Drawing.Size(80, 23);
			this.Connection_Port.TabIndex = 1;
			this.Connection_Port.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(47, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "ポート：";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.label5);
			this.tabPage2.Location = new System.Drawing.Point(4, 24);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(392, 231);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "UI";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(8, 3);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(87, 15);
			this.label5.TabIndex = 0;
			this.label5.Text = "＊未実装です＊";
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.Log_SaveLogFlag);
			this.tabPage3.Controls.Add(this.Log_LogLevel);
			this.tabPage3.Controls.Add(this.label6);
			this.tabPage3.Location = new System.Drawing.Point(4, 24);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(392, 231);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "ログ";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// Log_SaveLogFlag
			// 
			this.Log_SaveLogFlag.AutoSize = true;
			this.Log_SaveLogFlag.Location = new System.Drawing.Point(8, 6);
			this.Log_SaveLogFlag.Name = "Log_SaveLogFlag";
			this.Log_SaveLogFlag.Size = new System.Drawing.Size(139, 19);
			this.Log_SaveLogFlag.TabIndex = 2;
			this.Log_SaveLogFlag.Text = "ログをファイルに保存する";
			this.Log_SaveLogFlag.UseVisualStyleBackColor = true;
			// 
			// Log_LogLevel
			// 
			this.Log_LogLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.Log_LogLevel.Location = new System.Drawing.Point(304, 32);
			this.Log_LogLevel.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.Log_LogLevel.Name = "Log_LogLevel";
			this.Log_LogLevel.Size = new System.Drawing.Size(80, 23);
			this.Log_LogLevel.TabIndex = 1;
			this.Log_LogLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 34);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 15);
			this.label6.TabIndex = 0;
			this.label6.Text = "ログの出力レベル：";
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.Control_ConditionBorder);
			this.tabPage4.Controls.Add(this.label7);
			this.tabPage4.Location = new System.Drawing.Point(4, 24);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage4.Size = new System.Drawing.Size(392, 231);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "動作";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// Control_ConditionBorder
			// 
			this.Control_ConditionBorder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.Control_ConditionBorder.Location = new System.Drawing.Point(106, 6);
			this.Control_ConditionBorder.Name = "Control_ConditionBorder";
			this.Control_ConditionBorder.Size = new System.Drawing.Size(80, 23);
			this.Control_ConditionBorder.TabIndex = 3;
			this.Control_ConditionBorder.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 8);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(94, 15);
			this.label7.TabIndex = 2;
			this.label7.Text = "疲労度ボーダー：";
			// 
			// ToolTipInfo
			// 
			this.ToolTipInfo.AutoPopDelay = 60000;
			this.ToolTipInfo.InitialDelay = 500;
			this.ToolTipInfo.ReshowDelay = 100;
			// 
			// ButtonOK
			// 
			this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.ButtonOK.Location = new System.Drawing.Point(232, 265);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(75, 23);
			this.ButtonOK.TabIndex = 1;
			this.ButtonOK.Text = "OK";
			this.ButtonOK.UseVisualStyleBackColor = true;
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ButtonCancel.Location = new System.Drawing.Point(313, 265);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
			this.ButtonCancel.TabIndex = 2;
			this.ButtonCancel.Text = "キャンセル";
			this.ButtonCancel.UseVisualStyleBackColor = true;
			// 
			// DialogConfiguration
			// 
			this.AcceptButton = this.ButtonOK;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.ButtonCancel;
			this.ClientSize = new System.Drawing.Size(400, 300);
			this.Controls.Add(this.ButtonCancel);
			this.Controls.Add(this.ButtonOK);
			this.Controls.Add(this.tabControl1);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogConfiguration";
			this.ShowInTaskbar = false;
			this.Text = "設定";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.Connection_PanelSaveData.ResumeLayout(false);
			this.Connection_PanelSaveData.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.Connection_Port)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.Log_LogLevel)).EndInit();
			this.tabPage4.ResumeLayout(false);
			this.tabPage4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.Control_ConditionBorder)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel Connection_PanelSaveData;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button ButtonOK;
		private System.Windows.Forms.Button ButtonCancel;
		internal System.Windows.Forms.CheckBox Connection_SaveOtherFile;
		internal System.Windows.Forms.CheckBox Connection_SaveSWF;
		internal System.Windows.Forms.CheckBox Connection_SaveResponse;
		internal System.Windows.Forms.CheckBox Connection_SaveRequest;
		internal System.Windows.Forms.TextBox Connection_SaveDataPath;
		internal System.Windows.Forms.TextBox Connection_SaveDataFilter;
		internal System.Windows.Forms.CheckBox Connection_SaveReceivedData;
		internal System.Windows.Forms.NumericUpDown Connection_Port;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.Label label6;
		internal System.Windows.Forms.NumericUpDown Log_LogLevel;
		private System.Windows.Forms.TabPage tabPage4;
		internal System.Windows.Forms.NumericUpDown Control_ConditionBorder;
		private System.Windows.Forms.Label label7;
		internal System.Windows.Forms.CheckBox Log_SaveLogFlag;
	}
}