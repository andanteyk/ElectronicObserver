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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHeadquarters));
            this.FlowPanelMaster = new System.Windows.Forms.FlowLayoutPanel();
            this.FlowPanelAdmiral = new System.Windows.Forms.FlowLayoutPanel();
            this.AdmiralName = new System.Windows.Forms.Label();
            this.AdmiralComment = new System.Windows.Forms.Label();
            this.HQLevel = new ElectronicObserver.Window.Control.ShipStatusLevel();
            this.FlowPanelFleet = new System.Windows.Forms.FlowLayoutPanel();
            this.ShipCount = new ElectronicObserver.Window.Control.ImageLabel();
            this.EquipmentCount = new ElectronicObserver.Window.Control.ImageLabel();
            this.FlowPanelUseItem = new System.Windows.Forms.FlowLayoutPanel();
            this.InstantRepair = new ElectronicObserver.Window.Control.ImageLabel();
            this.InstantConstruction = new ElectronicObserver.Window.Control.ImageLabel();
            this.DevelopmentMaterial = new ElectronicObserver.Window.Control.ImageLabel();
            this.ModdingMaterial = new ElectronicObserver.Window.Control.ImageLabel();
            this.FurnitureCoin = new ElectronicObserver.Window.Control.ImageLabel();
            this.FlowPanelResource = new System.Windows.Forms.FlowLayoutPanel();
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
            resources.ApplyResources(this.FlowPanelMaster, "FlowPanelMaster");
            this.FlowPanelMaster.Controls.Add(this.FlowPanelAdmiral);
            this.FlowPanelMaster.Controls.Add(this.HQLevel);
            this.FlowPanelMaster.Controls.Add(this.FlowPanelFleet);
            this.FlowPanelMaster.Controls.Add(this.FlowPanelUseItem);
            this.FlowPanelMaster.Controls.Add(this.FlowPanelResource);
            this.FlowPanelMaster.Name = "FlowPanelMaster";
            // 
            // FlowPanelAdmiral
            // 
            resources.ApplyResources(this.FlowPanelAdmiral, "FlowPanelAdmiral");
            this.FlowPanelAdmiral.Controls.Add(this.AdmiralName);
            this.FlowPanelAdmiral.Controls.Add(this.AdmiralComment);
            this.FlowPanelAdmiral.Name = "FlowPanelAdmiral";
            // 
            // AdmiralName
            // 
            resources.ApplyResources(this.AdmiralName, "AdmiralName");
            this.AdmiralName.Name = "AdmiralName";
            // 
            // AdmiralComment
            // 
            resources.ApplyResources(this.AdmiralComment, "AdmiralComment");
            this.AdmiralComment.Name = "AdmiralComment";
            // 
            // HQLevel
            // 
            resources.ApplyResources(this.HQLevel, "HQLevel");
            this.HQLevel.MainFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.HQLevel.MaximumValue = 999;
            this.HQLevel.Name = "HQLevel";
            // 
            // FlowPanelFleet
            // 
            resources.ApplyResources(this.FlowPanelFleet, "FlowPanelFleet");
            this.FlowPanelFleet.Controls.Add(this.ShipCount);
            this.FlowPanelFleet.Controls.Add(this.EquipmentCount);
            this.FlowPanelFleet.Name = "FlowPanelFleet";
            // 
            // ShipCount
            // 
            resources.ApplyResources(this.ShipCount, "ShipCount");
            this.ShipCount.BackColor = System.Drawing.Color.Transparent;
            this.ShipCount.Name = "ShipCount";
            // 
            // EquipmentCount
            // 
            resources.ApplyResources(this.EquipmentCount, "EquipmentCount");
            this.EquipmentCount.BackColor = System.Drawing.Color.Transparent;
            this.EquipmentCount.Name = "EquipmentCount";
            // 
            // FlowPanelUseItem
            // 
            resources.ApplyResources(this.FlowPanelUseItem, "FlowPanelUseItem");
            this.FlowPanelUseItem.Controls.Add(this.InstantRepair);
            this.FlowPanelUseItem.Controls.Add(this.InstantConstruction);
            this.FlowPanelUseItem.Controls.Add(this.DevelopmentMaterial);
            this.FlowPanelUseItem.Controls.Add(this.ModdingMaterial);
            this.FlowPanelUseItem.Controls.Add(this.FurnitureCoin);
            this.FlowPanelUseItem.Name = "FlowPanelUseItem";
            // 
            // InstantRepair
            // 
            resources.ApplyResources(this.InstantRepair, "InstantRepair");
            this.InstantRepair.BackColor = System.Drawing.Color.Transparent;
            this.InstantRepair.Name = "InstantRepair";
            // 
            // InstantConstruction
            // 
            resources.ApplyResources(this.InstantConstruction, "InstantConstruction");
            this.InstantConstruction.BackColor = System.Drawing.Color.Transparent;
            this.InstantConstruction.Name = "InstantConstruction";
            // 
            // DevelopmentMaterial
            // 
            resources.ApplyResources(this.DevelopmentMaterial, "DevelopmentMaterial");
            this.DevelopmentMaterial.BackColor = System.Drawing.Color.Transparent;
            this.DevelopmentMaterial.Name = "DevelopmentMaterial";
            // 
            // ModdingMaterial
            // 
            resources.ApplyResources(this.ModdingMaterial, "ModdingMaterial");
            this.ModdingMaterial.BackColor = System.Drawing.Color.Transparent;
            this.ModdingMaterial.Name = "ModdingMaterial";
            // 
            // FurnitureCoin
            // 
            resources.ApplyResources(this.FurnitureCoin, "FurnitureCoin");
            this.FurnitureCoin.BackColor = System.Drawing.Color.Transparent;
            this.FurnitureCoin.Name = "FurnitureCoin";
            // 
            // FlowPanelResource
            // 
            resources.ApplyResources(this.FlowPanelResource, "FlowPanelResource");
            this.FlowPanelResource.Controls.Add(this.Fuel);
            this.FlowPanelResource.Controls.Add(this.Ammo);
            this.FlowPanelResource.Controls.Add(this.Steel);
            this.FlowPanelResource.Controls.Add(this.Bauxite);
            this.FlowPanelResource.Name = "FlowPanelResource";
            // 
            // Fuel
            // 
            resources.ApplyResources(this.Fuel, "Fuel");
            this.Fuel.BackColor = System.Drawing.Color.Transparent;
            this.Fuel.Name = "Fuel";
            // 
            // Ammo
            // 
            resources.ApplyResources(this.Ammo, "Ammo");
            this.Ammo.BackColor = System.Drawing.Color.Transparent;
            this.Ammo.Name = "Ammo";
            // 
            // Steel
            // 
            resources.ApplyResources(this.Steel, "Steel");
            this.Steel.BackColor = System.Drawing.Color.Transparent;
            this.Steel.Name = "Steel";
            // 
            // Bauxite
            // 
            resources.ApplyResources(this.Bauxite, "Bauxite");
            this.Bauxite.BackColor = System.Drawing.Color.Transparent;
            this.Bauxite.Name = "Bauxite";
            // 
            // FormHeadquarters
            // 
            resources.ApplyResources(this, "$this");
            this.AutoHidePortion = 150D;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.FlowPanelMaster);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Name = "FormHeadquarters";
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
		private Control.ImageLabel ModdingMaterial;
	}
}