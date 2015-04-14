namespace ElectronicObserver.Window.Integrate {
	partial class FormIntegrate {
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose( bool disposing ) {
			if ( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.labelTitle = new System.Windows.Forms.Label();
			this.labelClassName = new System.Windows.Forms.Label();
			this.labelFileName = new System.Windows.Forms.Label();
			this.titleTextBox = new System.Windows.Forms.TextBox();
			this.classNameTextBox = new System.Windows.Forms.TextBox();
			this.fileNameTextBox = new System.Windows.Forms.TextBox();
			this.titleComboBox = new System.Windows.Forms.ComboBox();
			this.classNameComboBox = new System.Windows.Forms.ComboBox();
			this.fileNameComboBox = new System.Windows.Forms.ComboBox();
			this.settingPanel = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.infoLabel = new System.Windows.Forms.Label();
			this.integrateButton = new System.Windows.Forms.Button();
			this.tabContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.StripMenu_Detach = new System.Windows.Forms.ToolStripMenuItem();
			this.windowCaptureButton = new ElectronicObserver.Window.Control.WindowCaptureButton();
			this.settingPanel.SuspendLayout();
			this.tabContextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelTitle
			// 
			this.labelTitle.AutoSize = true;
			this.labelTitle.Location = new System.Drawing.Point(5, 54);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(40, 12);
			this.labelTitle.TabIndex = 0;
			this.labelTitle.Text = "タイトル";
			// 
			// labelClassName
			// 
			this.labelClassName.AutoSize = true;
			this.labelClassName.Location = new System.Drawing.Point(4, 78);
			this.labelClassName.Name = "labelClassName";
			this.labelClassName.Size = new System.Drawing.Size(42, 12);
			this.labelClassName.TabIndex = 1;
			this.labelClassName.Text = "クラス名";
			// 
			// labelFileName
			// 
			this.labelFileName.AutoSize = true;
			this.labelFileName.Location = new System.Drawing.Point(6, 104);
			this.labelFileName.Name = "labelFileName";
			this.labelFileName.Size = new System.Drawing.Size(38, 12);
			this.labelFileName.TabIndex = 2;
			this.labelFileName.Text = "EXE名";
			// 
			// titleTextBox
			// 
			this.titleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.titleTextBox.Location = new System.Drawing.Point(51, 51);
			this.titleTextBox.Name = "titleTextBox";
			this.titleTextBox.Size = new System.Drawing.Size(157, 19);
			this.titleTextBox.TabIndex = 3;
			// 
			// classNameTextBox
			// 
			this.classNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.classNameTextBox.Location = new System.Drawing.Point(51, 76);
			this.classNameTextBox.Name = "classNameTextBox";
			this.classNameTextBox.Size = new System.Drawing.Size(157, 19);
			this.classNameTextBox.TabIndex = 4;
			// 
			// fileNameTextBox
			// 
			this.fileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.fileNameTextBox.Location = new System.Drawing.Point(51, 101);
			this.fileNameTextBox.Name = "fileNameTextBox";
			this.fileNameTextBox.Size = new System.Drawing.Size(157, 19);
			this.fileNameTextBox.TabIndex = 5;
			// 
			// titleComboBox
			// 
			this.titleComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.titleComboBox.FormattingEnabled = true;
			this.titleComboBox.Location = new System.Drawing.Point(214, 50);
			this.titleComboBox.Name = "titleComboBox";
			this.titleComboBox.Size = new System.Drawing.Size(67, 20);
			this.titleComboBox.TabIndex = 6;
			// 
			// classNameComboBox
			// 
			this.classNameComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.classNameComboBox.FormattingEnabled = true;
			this.classNameComboBox.Location = new System.Drawing.Point(214, 75);
			this.classNameComboBox.Name = "classNameComboBox";
			this.classNameComboBox.Size = new System.Drawing.Size(67, 20);
			this.classNameComboBox.TabIndex = 7;
			// 
			// fileNameComboBox
			// 
			this.fileNameComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.fileNameComboBox.FormattingEnabled = true;
			this.fileNameComboBox.Location = new System.Drawing.Point(214, 100);
			this.fileNameComboBox.Name = "fileNameComboBox";
			this.fileNameComboBox.Size = new System.Drawing.Size(67, 20);
			this.fileNameComboBox.TabIndex = 8;
			// 
			// settingPanel
			// 
			this.settingPanel.Controls.Add(this.windowCaptureButton);
			this.settingPanel.Controls.Add(this.label1);
			this.settingPanel.Controls.Add(this.infoLabel);
			this.settingPanel.Controls.Add(this.integrateButton);
			this.settingPanel.Controls.Add(this.titleTextBox);
			this.settingPanel.Controls.Add(this.fileNameComboBox);
			this.settingPanel.Controls.Add(this.labelTitle);
			this.settingPanel.Controls.Add(this.classNameComboBox);
			this.settingPanel.Controls.Add(this.labelClassName);
			this.settingPanel.Controls.Add(this.titleComboBox);
			this.settingPanel.Controls.Add(this.labelFileName);
			this.settingPanel.Controls.Add(this.fileNameTextBox);
			this.settingPanel.Controls.Add(this.classNameTextBox);
			this.settingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.settingPanel.Location = new System.Drawing.Point(0, 0);
			this.settingPanel.Name = "settingPanel";
			this.settingPanel.Size = new System.Drawing.Size(284, 261);
			this.settingPanel.TabIndex = 9;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 36);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 12);
			this.label1.TabIndex = 12;
			this.label1.Text = "ウィンドウ検索設定";
			// 
			// infoLabel
			// 
			this.infoLabel.AutoSize = true;
			this.infoLabel.Location = new System.Drawing.Point(89, 12);
			this.infoLabel.Name = "infoLabel";
			this.infoLabel.Size = new System.Drawing.Size(47, 12);
			this.infoLabel.TabIndex = 11;
			this.infoLabel.Text = "起動中...";
			// 
			// integrateButton
			// 
			this.integrateButton.Location = new System.Drawing.Point(8, 7);
			this.integrateButton.Name = "integrateButton";
			this.integrateButton.Size = new System.Drawing.Size(75, 23);
			this.integrateButton.TabIndex = 10;
			this.integrateButton.Text = "取り込む";
			this.integrateButton.UseVisualStyleBackColor = true;
			this.integrateButton.Click += new System.EventHandler(this.integrateButton_Click);
			// 
			// tabContextMenu
			// 
			this.tabContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripMenu_Detach});
			this.tabContextMenu.Name = "tabContextMenu";
			this.tabContextMenu.Size = new System.Drawing.Size(153, 48);
			// 
			// StripMenu_Detach
			// 
			this.StripMenu_Detach.Enabled = false;
			this.StripMenu_Detach.Name = "StripMenu_Detach";
			this.StripMenu_Detach.Size = new System.Drawing.Size(152, 22);
			this.StripMenu_Detach.Text = "ウィンドウ開放";
			this.StripMenu_Detach.Click += new System.EventHandler(this.StripMenu_Detach_Click);
			// 
			// windowCaptureButton
			// 
			this.windowCaptureButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.windowCaptureButton.Location = new System.Drawing.Point(248, 4);
			this.windowCaptureButton.Name = "windowCaptureButton";
			this.windowCaptureButton.Size = new System.Drawing.Size(33, 29);
			this.windowCaptureButton.TabIndex = 9;
			this.windowCaptureButton.UseVisualStyleBackColor = true;
			this.windowCaptureButton.WindowCaptured += new ElectronicObserver.Window.Control.WindowCaptureButton.WindowCapturedDelegate(this.windowCaptureButton_WindowCaptured);
			// 
			// FormIntegrated
			// 
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.settingPanel);
			this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Name = "FormIntegrated";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormIntegrated_FormClosing);
			this.Resize += new System.EventHandler(this.FormIntegrated_Resize);
			this.settingPanel.ResumeLayout(false);
			this.settingPanel.PerformLayout();
			this.tabContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label labelTitle;
		private System.Windows.Forms.Label labelClassName;
		private System.Windows.Forms.Label labelFileName;
		private System.Windows.Forms.TextBox titleTextBox;
		private System.Windows.Forms.TextBox classNameTextBox;
		private System.Windows.Forms.TextBox fileNameTextBox;
		private System.Windows.Forms.ComboBox titleComboBox;
		private System.Windows.Forms.ComboBox classNameComboBox;
		private System.Windows.Forms.ComboBox fileNameComboBox;
		private System.Windows.Forms.Panel settingPanel;
		private System.Windows.Forms.Button integrateButton;
		private Control.WindowCaptureButton windowCaptureButton;
		private System.Windows.Forms.Label infoLabel;
		private System.Windows.Forms.ContextMenuStrip tabContextMenu;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_Detach;
		private System.Windows.Forms.Label label1;
	}
}
