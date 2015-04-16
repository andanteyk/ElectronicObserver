namespace ElectronicObserver.Window.Dialog {
	partial class DialogConfigurationNotifier {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogConfigurationNotifier));
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.GroupSound = new System.Windows.Forms.GroupBox();
            this.PlaysSound = new System.Windows.Forms.CheckBox();
            this.SoundPathSearch = new System.Windows.Forms.Button();
            this.SoundPath = new System.Windows.Forms.TextBox();
            this.ButtonTest = new System.Windows.Forms.Button();
            this.IsEnabled = new System.Windows.Forms.CheckBox();
            this.GroupImage = new System.Windows.Forms.GroupBox();
            this.DrawsImage = new System.Windows.Forms.CheckBox();
            this.ImagePathSearch = new System.Windows.Forms.Button();
            this.ImagePath = new System.Windows.Forms.TextBox();
            this.GroupDialog = new System.Windows.Forms.GroupBox();
            this.ShowWithActivation = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DrawsMessage = new System.Windows.Forms.CheckBox();
            this.HasFormBorder = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ClosingInterval = new System.Windows.Forms.NumericUpDown();
            this.BackColorPreview = new System.Windows.Forms.Label();
            this.BackColorSelect = new System.Windows.Forms.Button();
            this.ForeColorPreview = new System.Windows.Forms.Label();
            this.ForeColorSelect = new System.Windows.Forms.Button();
            this.CloseOnMouseOver = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AccelInterval = new System.Windows.Forms.NumericUpDown();
            this.TopMostFlag = new System.Windows.Forms.CheckBox();
            this.LocationY = new System.Windows.Forms.NumericUpDown();
            this.LocationX = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.Alignment = new System.Windows.Forms.ComboBox();
            this.ShowsDialog = new System.Windows.Forms.CheckBox();
            this.GroupDamage = new System.Windows.Forms.GroupBox();
            this.NotifiesAtEndpoint = new System.Windows.Forms.CheckBox();
            this.ContainsFlagship = new System.Windows.Forms.CheckBox();
            this.ContainsSafeShip = new System.Windows.Forms.CheckBox();
            this.ContainsNotLockedShip = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.LevelBorder = new System.Windows.Forms.NumericUpDown();
            this.NotifiesAfter = new System.Windows.Forms.CheckBox();
            this.NotifiesNow = new System.Windows.Forms.CheckBox();
            this.NotifiesBefore = new System.Windows.Forms.CheckBox();
            this.DialogColor = new System.Windows.Forms.ColorDialog();
            this.DialogOpenSound = new System.Windows.Forms.OpenFileDialog();
            this.DialogOpenImage = new System.Windows.Forms.OpenFileDialog();
            this.ToolTipText = new System.Windows.Forms.ToolTip(this.components);
            this.GroupSound.SuspendLayout();
            this.GroupImage.SuspendLayout();
            this.GroupDialog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClosingInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccelInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LocationY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LocationX)).BeginInit();
            this.GroupDamage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LevelBorder)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonCancel
            // 
            resources.ApplyResources(this.ButtonCancel, "ButtonCancel");
            this.ButtonCancel.Name = "ButtonCancel";
            this.ToolTipText.SetToolTip(this.ButtonCancel, resources.GetString("ButtonCancel.ToolTip"));
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // ButtonOK
            // 
            resources.ApplyResources(this.ButtonOK, "ButtonOK");
            this.ButtonOK.Name = "ButtonOK";
            this.ToolTipText.SetToolTip(this.ButtonOK, resources.GetString("ButtonOK.ToolTip"));
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // GroupSound
            // 
            resources.ApplyResources(this.GroupSound, "GroupSound");
            this.GroupSound.Controls.Add(this.PlaysSound);
            this.GroupSound.Controls.Add(this.SoundPathSearch);
            this.GroupSound.Controls.Add(this.SoundPath);
            this.GroupSound.Name = "GroupSound";
            this.GroupSound.TabStop = false;
            this.ToolTipText.SetToolTip(this.GroupSound, resources.GetString("GroupSound.ToolTip"));
            this.GroupSound.DragDrop += new System.Windows.Forms.DragEventHandler(this.GroupSound_DragDrop);
            this.GroupSound.DragEnter += new System.Windows.Forms.DragEventHandler(this.GroupSound_DragEnter);
            // 
            // PlaysSound
            // 
            resources.ApplyResources(this.PlaysSound, "PlaysSound");
            this.PlaysSound.Name = "PlaysSound";
            this.ToolTipText.SetToolTip(this.PlaysSound, resources.GetString("PlaysSound.ToolTip"));
            this.PlaysSound.UseVisualStyleBackColor = true;
            // 
            // SoundPathSearch
            // 
            resources.ApplyResources(this.SoundPathSearch, "SoundPathSearch");
            this.SoundPathSearch.Name = "SoundPathSearch";
            this.ToolTipText.SetToolTip(this.SoundPathSearch, resources.GetString("SoundPathSearch.ToolTip"));
            this.SoundPathSearch.UseVisualStyleBackColor = true;
            this.SoundPathSearch.Click += new System.EventHandler(this.SoundPathSearch_Click);
            // 
            // SoundPath
            // 
            resources.ApplyResources(this.SoundPath, "SoundPath");
            this.SoundPath.AllowDrop = true;
            this.SoundPath.Name = "SoundPath";
            this.ToolTipText.SetToolTip(this.SoundPath, resources.GetString("SoundPath.ToolTip"));
            this.SoundPath.TextChanged += new System.EventHandler(this.SoundPath_TextChanged);
            // 
            // ButtonTest
            // 
            resources.ApplyResources(this.ButtonTest, "ButtonTest");
            this.ButtonTest.Name = "ButtonTest";
            this.ToolTipText.SetToolTip(this.ButtonTest, resources.GetString("ButtonTest.ToolTip"));
            this.ButtonTest.UseVisualStyleBackColor = true;
            this.ButtonTest.Click += new System.EventHandler(this.ButtonTest_Click);
            // 
            // IsEnabled
            // 
            resources.ApplyResources(this.IsEnabled, "IsEnabled");
            this.IsEnabled.Name = "IsEnabled";
            this.ToolTipText.SetToolTip(this.IsEnabled, resources.GetString("IsEnabled.ToolTip"));
            this.IsEnabled.UseVisualStyleBackColor = true;
            // 
            // GroupImage
            // 
            resources.ApplyResources(this.GroupImage, "GroupImage");
            this.GroupImage.Controls.Add(this.DrawsImage);
            this.GroupImage.Controls.Add(this.ImagePathSearch);
            this.GroupImage.Controls.Add(this.ImagePath);
            this.GroupImage.Name = "GroupImage";
            this.GroupImage.TabStop = false;
            this.ToolTipText.SetToolTip(this.GroupImage, resources.GetString("GroupImage.ToolTip"));
            this.GroupImage.DragDrop += new System.Windows.Forms.DragEventHandler(this.GroupImage_DragDrop);
            this.GroupImage.DragEnter += new System.Windows.Forms.DragEventHandler(this.GroupImage_DragEnter);
            // 
            // DrawsImage
            // 
            resources.ApplyResources(this.DrawsImage, "DrawsImage");
            this.DrawsImage.Name = "DrawsImage";
            this.ToolTipText.SetToolTip(this.DrawsImage, resources.GetString("DrawsImage.ToolTip"));
            this.DrawsImage.UseVisualStyleBackColor = true;
            // 
            // ImagePathSearch
            // 
            resources.ApplyResources(this.ImagePathSearch, "ImagePathSearch");
            this.ImagePathSearch.Name = "ImagePathSearch";
            this.ToolTipText.SetToolTip(this.ImagePathSearch, resources.GetString("ImagePathSearch.ToolTip"));
            this.ImagePathSearch.UseVisualStyleBackColor = true;
            this.ImagePathSearch.Click += new System.EventHandler(this.ImagePathSearch_Click);
            // 
            // ImagePath
            // 
            resources.ApplyResources(this.ImagePath, "ImagePath");
            this.ImagePath.AllowDrop = true;
            this.ImagePath.Name = "ImagePath";
            this.ToolTipText.SetToolTip(this.ImagePath, resources.GetString("ImagePath.ToolTip"));
            this.ImagePath.TextChanged += new System.EventHandler(this.ImagePath_TextChanged);
            // 
            // GroupDialog
            // 
            resources.ApplyResources(this.GroupDialog, "GroupDialog");
            this.GroupDialog.Controls.Add(this.ShowWithActivation);
            this.GroupDialog.Controls.Add(this.label4);
            this.GroupDialog.Controls.Add(this.DrawsMessage);
            this.GroupDialog.Controls.Add(this.HasFormBorder);
            this.GroupDialog.Controls.Add(this.label6);
            this.GroupDialog.Controls.Add(this.label7);
            this.GroupDialog.Controls.Add(this.ClosingInterval);
            this.GroupDialog.Controls.Add(this.BackColorPreview);
            this.GroupDialog.Controls.Add(this.BackColorSelect);
            this.GroupDialog.Controls.Add(this.ForeColorPreview);
            this.GroupDialog.Controls.Add(this.ForeColorSelect);
            this.GroupDialog.Controls.Add(this.CloseOnMouseOver);
            this.GroupDialog.Controls.Add(this.label3);
            this.GroupDialog.Controls.Add(this.label2);
            this.GroupDialog.Controls.Add(this.AccelInterval);
            this.GroupDialog.Controls.Add(this.TopMostFlag);
            this.GroupDialog.Controls.Add(this.LocationY);
            this.GroupDialog.Controls.Add(this.LocationX);
            this.GroupDialog.Controls.Add(this.label1);
            this.GroupDialog.Controls.Add(this.Alignment);
            this.GroupDialog.Controls.Add(this.ShowsDialog);
            this.GroupDialog.Name = "GroupDialog";
            this.GroupDialog.TabStop = false;
            this.ToolTipText.SetToolTip(this.GroupDialog, resources.GetString("GroupDialog.ToolTip"));
            // 
            // ShowWithActivation
            // 
            resources.ApplyResources(this.ShowWithActivation, "ShowWithActivation");
            this.ShowWithActivation.Name = "ShowWithActivation";
            this.ToolTipText.SetToolTip(this.ShowWithActivation, resources.GetString("ShowWithActivation.ToolTip"));
            this.ShowWithActivation.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            this.ToolTipText.SetToolTip(this.label4, resources.GetString("label4.ToolTip"));
            // 
            // DrawsMessage
            // 
            resources.ApplyResources(this.DrawsMessage, "DrawsMessage");
            this.DrawsMessage.Name = "DrawsMessage";
            this.ToolTipText.SetToolTip(this.DrawsMessage, resources.GetString("DrawsMessage.ToolTip"));
            this.DrawsMessage.UseVisualStyleBackColor = true;
            // 
            // HasFormBorder
            // 
            resources.ApplyResources(this.HasFormBorder, "HasFormBorder");
            this.HasFormBorder.Name = "HasFormBorder";
            this.ToolTipText.SetToolTip(this.HasFormBorder, resources.GetString("HasFormBorder.ToolTip"));
            this.HasFormBorder.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            this.ToolTipText.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            this.ToolTipText.SetToolTip(this.label7, resources.GetString("label7.ToolTip"));
            // 
            // ClosingInterval
            // 
            resources.ApplyResources(this.ClosingInterval, "ClosingInterval");
            this.ClosingInterval.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.ClosingInterval.Name = "ClosingInterval";
            this.ToolTipText.SetToolTip(this.ClosingInterval, resources.GetString("ClosingInterval.ToolTip"));
            // 
            // BackColorPreview
            // 
            resources.ApplyResources(this.BackColorPreview, "BackColorPreview");
            this.BackColorPreview.Name = "BackColorPreview";
            this.ToolTipText.SetToolTip(this.BackColorPreview, resources.GetString("BackColorPreview.ToolTip"));
            this.BackColorPreview.ForeColorChanged += new System.EventHandler(this.BackColorPreview_ForeColorChanged);
            // 
            // BackColorSelect
            // 
            resources.ApplyResources(this.BackColorSelect, "BackColorSelect");
            this.BackColorSelect.Name = "BackColorSelect";
            this.ToolTipText.SetToolTip(this.BackColorSelect, resources.GetString("BackColorSelect.ToolTip"));
            this.BackColorSelect.UseVisualStyleBackColor = true;
            this.BackColorSelect.Click += new System.EventHandler(this.BackColorSelect_Click);
            // 
            // ForeColorPreview
            // 
            resources.ApplyResources(this.ForeColorPreview, "ForeColorPreview");
            this.ForeColorPreview.Name = "ForeColorPreview";
            this.ToolTipText.SetToolTip(this.ForeColorPreview, resources.GetString("ForeColorPreview.ToolTip"));
            this.ForeColorPreview.ForeColorChanged += new System.EventHandler(this.ForeColorPreview_ForeColorChanged);
            // 
            // ForeColorSelect
            // 
            resources.ApplyResources(this.ForeColorSelect, "ForeColorSelect");
            this.ForeColorSelect.Name = "ForeColorSelect";
            this.ToolTipText.SetToolTip(this.ForeColorSelect, resources.GetString("ForeColorSelect.ToolTip"));
            this.ForeColorSelect.UseVisualStyleBackColor = true;
            this.ForeColorSelect.Click += new System.EventHandler(this.ForeColorSelect_Click);
            // 
            // CloseOnMouseOver
            // 
            resources.ApplyResources(this.CloseOnMouseOver, "CloseOnMouseOver");
            this.CloseOnMouseOver.Name = "CloseOnMouseOver";
            this.ToolTipText.SetToolTip(this.CloseOnMouseOver, resources.GetString("CloseOnMouseOver.ToolTip"));
            this.CloseOnMouseOver.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.ToolTipText.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            this.ToolTipText.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // AccelInterval
            // 
            resources.ApplyResources(this.AccelInterval, "AccelInterval");
            this.AccelInterval.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.AccelInterval.Name = "AccelInterval";
            this.ToolTipText.SetToolTip(this.AccelInterval, resources.GetString("AccelInterval.ToolTip"));
            // 
            // TopMostFlag
            // 
            resources.ApplyResources(this.TopMostFlag, "TopMostFlag");
            this.TopMostFlag.Name = "TopMostFlag";
            this.ToolTipText.SetToolTip(this.TopMostFlag, resources.GetString("TopMostFlag.ToolTip"));
            this.TopMostFlag.UseVisualStyleBackColor = true;
            // 
            // LocationY
            // 
            resources.ApplyResources(this.LocationY, "LocationY");
            this.LocationY.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.LocationY.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.LocationY.Name = "LocationY";
            this.ToolTipText.SetToolTip(this.LocationY, resources.GetString("LocationY.ToolTip"));
            this.LocationY.Value = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            // 
            // LocationX
            // 
            resources.ApplyResources(this.LocationX, "LocationX");
            this.LocationX.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.LocationX.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.LocationX.Name = "LocationX";
            this.ToolTipText.SetToolTip(this.LocationX, resources.GetString("LocationX.ToolTip"));
            this.LocationX.Value = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.ToolTipText.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // Alignment
            // 
            resources.ApplyResources(this.Alignment, "Alignment");
            this.Alignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Alignment.FormattingEnabled = true;
            this.Alignment.Items.AddRange(new object[] {
            resources.GetString("Alignment.Items"),
            resources.GetString("Alignment.Items1"),
            resources.GetString("Alignment.Items2"),
            resources.GetString("Alignment.Items3"),
            resources.GetString("Alignment.Items4"),
            resources.GetString("Alignment.Items5"),
            resources.GetString("Alignment.Items6"),
            resources.GetString("Alignment.Items7"),
            resources.GetString("Alignment.Items8"),
            resources.GetString("Alignment.Items9"),
            resources.GetString("Alignment.Items10"),
            resources.GetString("Alignment.Items11")});
            this.Alignment.Name = "Alignment";
            this.ToolTipText.SetToolTip(this.Alignment, resources.GetString("Alignment.ToolTip"));
            // 
            // ShowsDialog
            // 
            resources.ApplyResources(this.ShowsDialog, "ShowsDialog");
            this.ShowsDialog.Name = "ShowsDialog";
            this.ToolTipText.SetToolTip(this.ShowsDialog, resources.GetString("ShowsDialog.ToolTip"));
            this.ShowsDialog.UseVisualStyleBackColor = true;
            // 
            // GroupDamage
            // 
            resources.ApplyResources(this.GroupDamage, "GroupDamage");
            this.GroupDamage.Controls.Add(this.NotifiesAtEndpoint);
            this.GroupDamage.Controls.Add(this.ContainsFlagship);
            this.GroupDamage.Controls.Add(this.ContainsSafeShip);
            this.GroupDamage.Controls.Add(this.ContainsNotLockedShip);
            this.GroupDamage.Controls.Add(this.label8);
            this.GroupDamage.Controls.Add(this.LevelBorder);
            this.GroupDamage.Controls.Add(this.NotifiesAfter);
            this.GroupDamage.Controls.Add(this.NotifiesNow);
            this.GroupDamage.Controls.Add(this.NotifiesBefore);
            this.GroupDamage.Name = "GroupDamage";
            this.GroupDamage.TabStop = false;
            this.ToolTipText.SetToolTip(this.GroupDamage, resources.GetString("GroupDamage.ToolTip"));
            // 
            // NotifiesAtEndpoint
            // 
            resources.ApplyResources(this.NotifiesAtEndpoint, "NotifiesAtEndpoint");
            this.NotifiesAtEndpoint.Name = "NotifiesAtEndpoint";
            this.ToolTipText.SetToolTip(this.NotifiesAtEndpoint, resources.GetString("NotifiesAtEndpoint.ToolTip"));
            this.NotifiesAtEndpoint.UseVisualStyleBackColor = true;
            // 
            // ContainsFlagship
            // 
            resources.ApplyResources(this.ContainsFlagship, "ContainsFlagship");
            this.ContainsFlagship.Name = "ContainsFlagship";
            this.ToolTipText.SetToolTip(this.ContainsFlagship, resources.GetString("ContainsFlagship.ToolTip"));
            this.ContainsFlagship.UseVisualStyleBackColor = true;
            // 
            // ContainsSafeShip
            // 
            resources.ApplyResources(this.ContainsSafeShip, "ContainsSafeShip");
            this.ContainsSafeShip.Name = "ContainsSafeShip";
            this.ToolTipText.SetToolTip(this.ContainsSafeShip, resources.GetString("ContainsSafeShip.ToolTip"));
            this.ContainsSafeShip.UseVisualStyleBackColor = true;
            // 
            // ContainsNotLockedShip
            // 
            resources.ApplyResources(this.ContainsNotLockedShip, "ContainsNotLockedShip");
            this.ContainsNotLockedShip.Name = "ContainsNotLockedShip";
            this.ToolTipText.SetToolTip(this.ContainsNotLockedShip, resources.GetString("ContainsNotLockedShip.ToolTip"));
            this.ContainsNotLockedShip.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            this.ToolTipText.SetToolTip(this.label8, resources.GetString("label8.ToolTip"));
            // 
            // LevelBorder
            // 
            resources.ApplyResources(this.LevelBorder, "LevelBorder");
            this.LevelBorder.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.LevelBorder.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LevelBorder.Name = "LevelBorder";
            this.ToolTipText.SetToolTip(this.LevelBorder, resources.GetString("LevelBorder.ToolTip"));
            this.LevelBorder.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // NotifiesAfter
            // 
            resources.ApplyResources(this.NotifiesAfter, "NotifiesAfter");
            this.NotifiesAfter.Name = "NotifiesAfter";
            this.ToolTipText.SetToolTip(this.NotifiesAfter, resources.GetString("NotifiesAfter.ToolTip"));
            this.NotifiesAfter.UseVisualStyleBackColor = true;
            // 
            // NotifiesNow
            // 
            resources.ApplyResources(this.NotifiesNow, "NotifiesNow");
            this.NotifiesNow.Name = "NotifiesNow";
            this.ToolTipText.SetToolTip(this.NotifiesNow, resources.GetString("NotifiesNow.ToolTip"));
            this.NotifiesNow.UseVisualStyleBackColor = true;
            // 
            // NotifiesBefore
            // 
            resources.ApplyResources(this.NotifiesBefore, "NotifiesBefore");
            this.NotifiesBefore.Name = "NotifiesBefore";
            this.ToolTipText.SetToolTip(this.NotifiesBefore, resources.GetString("NotifiesBefore.ToolTip"));
            this.NotifiesBefore.UseVisualStyleBackColor = true;
            // 
            // DialogColor
            // 
            this.DialogColor.AnyColor = true;
            this.DialogColor.FullOpen = true;
            // 
            // DialogOpenSound
            // 
            resources.ApplyResources(this.DialogOpenSound, "DialogOpenSound");
            // 
            // DialogOpenImage
            // 
            resources.ApplyResources(this.DialogOpenImage, "DialogOpenImage");
            // 
            // ToolTipText
            // 
            this.ToolTipText.AutoPopDelay = 30000;
            this.ToolTipText.InitialDelay = 500;
            this.ToolTipText.ReshowDelay = 100;
            // 
            // DialogConfigurationNotifier
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.GroupDamage);
            this.Controls.Add(this.GroupDialog);
            this.Controls.Add(this.GroupImage);
            this.Controls.Add(this.IsEnabled);
            this.Controls.Add(this.ButtonTest);
            this.Controls.Add(this.GroupSound);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogConfigurationNotifier";
            this.ShowInTaskbar = false;
            this.ToolTipText.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.Load += new System.EventHandler(this.DialogConfigurationNotifier_Load);
            this.GroupSound.ResumeLayout(false);
            this.GroupSound.PerformLayout();
            this.GroupImage.ResumeLayout(false);
            this.GroupImage.PerformLayout();
            this.GroupDialog.ResumeLayout(false);
            this.GroupDialog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClosingInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccelInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LocationY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LocationX)).EndInit();
            this.GroupDamage.ResumeLayout(false);
            this.GroupDamage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LevelBorder)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.Button ButtonOK;
		private System.Windows.Forms.GroupBox GroupSound;
		private System.Windows.Forms.Button ButtonTest;
		private System.Windows.Forms.CheckBox PlaysSound;
		private System.Windows.Forms.Button SoundPathSearch;
		internal System.Windows.Forms.TextBox SoundPath;
		private System.Windows.Forms.CheckBox IsEnabled;
		private System.Windows.Forms.GroupBox GroupImage;
		private System.Windows.Forms.CheckBox DrawsImage;
		private System.Windows.Forms.Button ImagePathSearch;
		internal System.Windows.Forms.TextBox ImagePath;
		private System.Windows.Forms.GroupBox GroupDialog;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown AccelInterval;
		private System.Windows.Forms.CheckBox TopMostFlag;
		private System.Windows.Forms.NumericUpDown LocationY;
		private System.Windows.Forms.NumericUpDown LocationX;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox Alignment;
		private System.Windows.Forms.CheckBox ShowsDialog;
		private System.Windows.Forms.Label BackColorPreview;
		private System.Windows.Forms.Button BackColorSelect;
		private System.Windows.Forms.Label ForeColorPreview;
		private System.Windows.Forms.Button ForeColorSelect;
		private System.Windows.Forms.CheckBox CloseOnMouseOver;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown ClosingInterval;
		private System.Windows.Forms.CheckBox HasFormBorder;
		private System.Windows.Forms.GroupBox GroupDamage;
		private System.Windows.Forms.CheckBox NotifiesAfter;
		private System.Windows.Forms.CheckBox NotifiesNow;
		private System.Windows.Forms.CheckBox NotifiesBefore;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown LevelBorder;
		private System.Windows.Forms.CheckBox ContainsSafeShip;
		private System.Windows.Forms.CheckBox ContainsNotLockedShip;
		private System.Windows.Forms.CheckBox ContainsFlagship;
		private System.Windows.Forms.CheckBox DrawsMessage;
		private System.Windows.Forms.ColorDialog DialogColor;
		private System.Windows.Forms.OpenFileDialog DialogOpenSound;
		private System.Windows.Forms.OpenFileDialog DialogOpenImage;
		private System.Windows.Forms.CheckBox NotifiesAtEndpoint;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ToolTip ToolTipText;
		private System.Windows.Forms.CheckBox ShowWithActivation;
	}
}