namespace ElectronicObserver.Window {
	partial class FormHeadquarters {
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
			this.FlowPanelMaster = new System.Windows.Forms.FlowLayoutPanel();
			this.FlowPanelAdmiral = new System.Windows.Forms.FlowLayoutPanel();
			this.AdmiralName = new System.Windows.Forms.Label();
			this.AdmiralComment = new System.Windows.Forms.Label();
			this.FlowPanelFleet = new System.Windows.Forms.FlowLayoutPanel();
			this.FlowPanelUseItem = new System.Windows.Forms.FlowLayoutPanel();
			this.FlowPanelResource = new System.Windows.Forms.FlowLayoutPanel();
			this.HQLevel = new ElectronicObserver.Window.Control.ShipStatusLevel();
			this.ShipCount = new ElectronicObserver.Window.Control.ImageLabel();
			this.EquipmentCount = new ElectronicObserver.Window.Control.ImageLabel();
			this.InstantRepair = new ElectronicObserver.Window.Control.ImageLabel();
			this.InstantConstruction = new ElectronicObserver.Window.Control.ImageLabel();
			this.DevelopmentMaterial = new ElectronicObserver.Window.Control.ImageLabel();
			this.FurnitureCoin = new ElectronicObserver.Window.Control.ImageLabel();
			this.Fuel = new ElectronicObserver.Window.Control.ImageLabel();
			this.Ammo = new ElectronicObserver.Window.Control.ImageLabel();
			this.Steel = new ElectronicObserver.Window.Control.ImageLabel();
			this.Bauxite = new ElectronicObserver.Window.Control.ImageLabel();
			this.FlowPanelMaster.SuspendLayout();
			this.FlowPanelAdmiral.SuspendLayout();
			this.FlowPanelFleet.SuspendLayout();
			this.FlowPanelUseItem.SuspendLayout();
			this.FlowPanelResource.SuspendLayout();
			this.SuspendLayout();
			// 
			// FlowPanelMaster
			// 
			this.FlowPanelMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.FlowPanelMaster.Controls.Add(this.FlowPanelAdmiral);
			this.FlowPanelMaster.Controls.Add(this.HQLevel);
			this.FlowPanelMaster.Controls.Add(this.FlowPanelFleet);
			this.FlowPanelMaster.Controls.Add(this.FlowPanelUseItem);
			this.FlowPanelMaster.Controls.Add(this.FlowPanelResource);
			this.FlowPanelMaster.Location = new System.Drawing.Point(0, 0);
			this.FlowPanelMaster.Margin = new System.Windows.Forms.Padding(0);
			this.FlowPanelMaster.Name = "FlowPanelMaster";
			this.FlowPanelMaster.Size = new System.Drawing.Size(300, 200);
			this.FlowPanelMaster.TabIndex = 0;
			// 
			// FlowPanelAdmiral
			// 
			this.FlowPanelAdmiral.AutoSize = true;
			this.FlowPanelAdmiral.Controls.Add(this.AdmiralName);
			this.FlowPanelAdmiral.Controls.Add(this.AdmiralComment);
			this.FlowPanelAdmiral.Location = new System.Drawing.Point(0, 0);
			this.FlowPanelAdmiral.Margin = new System.Windows.Forms.Padding(0);
			this.FlowPanelAdmiral.Name = "FlowPanelAdmiral";
			this.FlowPanelAdmiral.Size = new System.Drawing.Size(139, 20);
			this.FlowPanelAdmiral.TabIndex = 0;
			// 
			// AdmiralName
			// 
			this.AdmiralName.AutoSize = true;
			this.AdmiralName.Location = new System.Drawing.Point(3, 0);
			this.AdmiralName.Name = "AdmiralName";
			this.AdmiralName.Padding = new System.Windows.Forms.Padding(0, 3, 0, 2);
			this.AdmiralName.Size = new System.Drawing.Size(53, 20);
			this.AdmiralName.TabIndex = 0;
			this.AdmiralName.Text = "(提督名)";
			this.AdmiralName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// AdmiralComment
			// 
			this.AdmiralComment.AutoSize = true;
			this.AdmiralComment.Location = new System.Drawing.Point(62, 0);
			this.AdmiralComment.Name = "AdmiralComment";
			this.AdmiralComment.Padding = new System.Windows.Forms.Padding(0, 3, 0, 2);
			this.AdmiralComment.Size = new System.Drawing.Size(74, 20);
			this.AdmiralComment.TabIndex = 1;
			this.AdmiralComment.Text = "(提督コメント)";
			this.AdmiralComment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FlowPanelFleet
			// 
			this.FlowPanelFleet.AutoSize = true;
			this.FlowPanelFleet.Controls.Add(this.ShipCount);
			this.FlowPanelFleet.Controls.Add(this.EquipmentCount);
			this.FlowPanelFleet.Location = new System.Drawing.Point(0, 20);
			this.FlowPanelFleet.Margin = new System.Windows.Forms.Padding(0);
			this.FlowPanelFleet.Name = "FlowPanelFleet";
			this.FlowPanelFleet.Size = new System.Drawing.Size(144, 20);
			this.FlowPanelFleet.TabIndex = 1;
			// 
			// FlowPanelUseItem
			// 
			this.FlowPanelUseItem.AutoSize = true;
			this.FlowPanelUseItem.Controls.Add(this.InstantRepair);
			this.FlowPanelUseItem.Controls.Add(this.InstantConstruction);
			this.FlowPanelUseItem.Controls.Add(this.DevelopmentMaterial);
			this.FlowPanelUseItem.Controls.Add(this.FurnitureCoin);
			this.FlowPanelUseItem.Location = new System.Drawing.Point(0, 40);
			this.FlowPanelUseItem.Margin = new System.Windows.Forms.Padding(0);
			this.FlowPanelUseItem.Name = "FlowPanelUseItem";
			this.FlowPanelUseItem.Size = new System.Drawing.Size(240, 20);
			this.FlowPanelUseItem.TabIndex = 2;
			// 
			// FlowPanelResource
			// 
			this.FlowPanelResource.AutoSize = true;
			this.FlowPanelResource.Controls.Add(this.Fuel);
			this.FlowPanelResource.Controls.Add(this.Ammo);
			this.FlowPanelResource.Controls.Add(this.Steel);
			this.FlowPanelResource.Controls.Add(this.Bauxite);
			this.FlowPanelResource.Location = new System.Drawing.Point(0, 60);
			this.FlowPanelResource.Margin = new System.Windows.Forms.Padding(0);
			this.FlowPanelResource.Name = "FlowPanelResource";
			this.FlowPanelResource.Size = new System.Drawing.Size(240, 20);
			this.FlowPanelResource.TabIndex = 5;
			// 
			// HQLevel
			// 
			this.HQLevel.AutoSize = true;
			this.HQLevel.Location = new System.Drawing.Point(142, 0);
			this.HQLevel.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.HQLevel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.HQLevel.MaximumValue = 999;
			this.HQLevel.Name = "HQLevel";
			this.HQLevel.Size = new System.Drawing.Size(88, 20);
			this.HQLevel.TabIndex = 0;
			this.HQLevel.Text = "HQ Lv.";
			// 
			// ShipCount
			// 
			this.ShipCount.BackColor = System.Drawing.Color.Transparent;
			this.ShipCount.Location = new System.Drawing.Point(3, 0);
			this.ShipCount.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.ShipCount.Name = "ShipCount";
			this.ShipCount.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.ShipCount.Size = new System.Drawing.Size(66, 20);
			this.ShipCount.TabIndex = 0;
			this.ShipCount.Text = "(艦船数)";
			// 
			// EquipmentCount
			// 
			this.EquipmentCount.BackColor = System.Drawing.Color.Transparent;
			this.EquipmentCount.Location = new System.Drawing.Point(75, 0);
			this.EquipmentCount.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.EquipmentCount.Name = "EquipmentCount";
			this.EquipmentCount.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.EquipmentCount.Size = new System.Drawing.Size(66, 20);
			this.EquipmentCount.TabIndex = 1;
			this.EquipmentCount.Text = "(装備数)";
			// 
			// InstantRepair
			// 
			this.InstantRepair.BackColor = System.Drawing.Color.Transparent;
			this.InstantRepair.Location = new System.Drawing.Point(3, 0);
			this.InstantRepair.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.InstantRepair.Name = "InstantRepair";
			this.InstantRepair.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.InstantRepair.Size = new System.Drawing.Size(54, 20);
			this.InstantRepair.TabIndex = 1;
			this.InstantRepair.Text = "(修復)";
			// 
			// InstantConstruction
			// 
			this.InstantConstruction.BackColor = System.Drawing.Color.Transparent;
			this.InstantConstruction.Location = new System.Drawing.Point(63, 0);
			this.InstantConstruction.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.InstantConstruction.Name = "InstantConstruction";
			this.InstantConstruction.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.InstantConstruction.Size = new System.Drawing.Size(54, 20);
			this.InstantConstruction.TabIndex = 2;
			this.InstantConstruction.Text = "(建造)";
			// 
			// DevelopmentMaterial
			// 
			this.DevelopmentMaterial.BackColor = System.Drawing.Color.Transparent;
			this.DevelopmentMaterial.Location = new System.Drawing.Point(123, 0);
			this.DevelopmentMaterial.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.DevelopmentMaterial.Name = "DevelopmentMaterial";
			this.DevelopmentMaterial.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.DevelopmentMaterial.Size = new System.Drawing.Size(54, 20);
			this.DevelopmentMaterial.TabIndex = 3;
			this.DevelopmentMaterial.Text = "(開発)";
			// 
			// FurnitureCoin
			// 
			this.FurnitureCoin.BackColor = System.Drawing.Color.Transparent;
			this.FurnitureCoin.Location = new System.Drawing.Point(183, 0);
			this.FurnitureCoin.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.FurnitureCoin.Name = "FurnitureCoin";
			this.FurnitureCoin.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.FurnitureCoin.Size = new System.Drawing.Size(54, 20);
			this.FurnitureCoin.TabIndex = 4;
			this.FurnitureCoin.Text = "(家具)";
			// 
			// Fuel
			// 
			this.Fuel.BackColor = System.Drawing.Color.Transparent;
			this.Fuel.Location = new System.Drawing.Point(3, 0);
			this.Fuel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.Fuel.Name = "Fuel";
			this.Fuel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.Fuel.Size = new System.Drawing.Size(54, 20);
			this.Fuel.TabIndex = 1;
			this.Fuel.Text = "(燃料)";
			// 
			// Ammo
			// 
			this.Ammo.BackColor = System.Drawing.Color.Transparent;
			this.Ammo.Location = new System.Drawing.Point(63, 0);
			this.Ammo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.Ammo.Name = "Ammo";
			this.Ammo.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.Ammo.Size = new System.Drawing.Size(54, 20);
			this.Ammo.TabIndex = 2;
			this.Ammo.Text = "(弾薬)";
			// 
			// Steel
			// 
			this.Steel.BackColor = System.Drawing.Color.Transparent;
			this.Steel.Location = new System.Drawing.Point(123, 0);
			this.Steel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.Steel.Name = "Steel";
			this.Steel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.Steel.Size = new System.Drawing.Size(54, 20);
			this.Steel.TabIndex = 3;
			this.Steel.Text = "(鋼材)";
			// 
			// Bauxite
			// 
			this.Bauxite.BackColor = System.Drawing.Color.Transparent;
			this.Bauxite.Location = new System.Drawing.Point(183, 0);
			this.Bauxite.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.Bauxite.Name = "Bauxite";
			this.Bauxite.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.Bauxite.Size = new System.Drawing.Size(54, 20);
			this.Bauxite.TabIndex = 4;
			this.Bauxite.Text = "(軽銀)";
			// 
			// FormHeadquarters
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.FlowPanelMaster);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormHeadquarters";
			this.Text = "司令部";
			this.Load += new System.EventHandler(this.FormHeadquarters_Load);
			this.FlowPanelMaster.ResumeLayout(false);
			this.FlowPanelMaster.PerformLayout();
			this.FlowPanelAdmiral.ResumeLayout(false);
			this.FlowPanelAdmiral.PerformLayout();
			this.FlowPanelFleet.ResumeLayout(false);
			this.FlowPanelFleet.PerformLayout();
			this.FlowPanelUseItem.ResumeLayout(false);
			this.FlowPanelUseItem.PerformLayout();
			this.FlowPanelResource.ResumeLayout(false);
			this.FlowPanelResource.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel FlowPanelMaster;
		private System.Windows.Forms.FlowLayoutPanel FlowPanelAdmiral;
		private System.Windows.Forms.Label AdmiralName;
		private System.Windows.Forms.Label AdmiralComment;
		private System.Windows.Forms.FlowLayoutPanel FlowPanelFleet;
		private Control.ShipStatusLevel HQLevel;
		private Control.ImageLabel ShipCount;
		private Control.ImageLabel EquipmentCount;
		private System.Windows.Forms.FlowLayoutPanel FlowPanelUseItem;
		private Control.ImageLabel InstantRepair;
		private Control.ImageLabel InstantConstruction;
		private Control.ImageLabel DevelopmentMaterial;
		private Control.ImageLabel FurnitureCoin;
		private System.Windows.Forms.FlowLayoutPanel FlowPanelResource;
		private Control.ImageLabel Fuel;
		private Control.ImageLabel Ammo;
		private Control.ImageLabel Steel;
		private Control.ImageLabel Bauxite;
	}
}