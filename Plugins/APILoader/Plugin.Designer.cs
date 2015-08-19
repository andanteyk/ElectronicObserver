using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APILoader
{
	partial class Plugin
	{

		void InitToolStripMenuItem()
		{
			this.StripMenu_Debug = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_Debug_LoadAPIFromFile = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_Debug_LoadInitialAPI = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.StripMenu_Debug_LoadRecordFromOld = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_Debug_DeleteOldAPI = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_Debug_RenameShipResource = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_Debug_LoadDataFromOld = new System.Windows.Forms.ToolStripMenuItem();

			// 
			// StripMenu_Debug
			// 
			this.StripMenu_Debug.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
			this.StripMenu_Debug_LoadAPIFromFile,
			this.StripMenu_Debug_LoadInitialAPI,
			this.toolStripSeparator,
			this.StripMenu_Debug_LoadRecordFromOld,
			this.StripMenu_Debug_DeleteOldAPI,
			this.StripMenu_Debug_RenameShipResource,
            this.StripMenu_Debug_LoadDataFromOld} );
			this.StripMenu_Debug.Name = "StripMenu_Debug";
			this.StripMenu_Debug.Size = new System.Drawing.Size( 137, 38 );
			this.StripMenu_Debug.Text = "调试(&D)";
			// 
			// StripMenu_Debug_LoadAPIFromFile
			// 
			this.StripMenu_Debug_LoadAPIFromFile.Name = "StripMenu_Debug_LoadAPIFromFile";
			this.StripMenu_Debug_LoadAPIFromFile.Size = new System.Drawing.Size( 484, 34 );
			this.StripMenu_Debug_LoadAPIFromFile.Text = "ファイルからAPIをロード(&L)...";
			this.StripMenu_Debug_LoadAPIFromFile.Click += new System.EventHandler( this.StripMenu_Debug_LoadAPIFromFile_Click );
			// 
			// StripMenu_Debug_LoadInitialAPI
			// 
			this.StripMenu_Debug_LoadInitialAPI.Name = "StripMenu_Debug_LoadInitialAPI";
			this.StripMenu_Debug_LoadInitialAPI.Size = new System.Drawing.Size( 484, 34 );
			this.StripMenu_Debug_LoadInitialAPI.Text = "APIリストをロード(&I)...";
			this.StripMenu_Debug_LoadInitialAPI.Click += new System.EventHandler( this.StripMenu_Debug_LoadInitialAPI_Click );
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator8";
			this.toolStripSeparator.Size = new System.Drawing.Size( 481, 6 );
			// 
			// StripMenu_Debug_LoadRecordFromOld
			// 
			this.StripMenu_Debug_LoadRecordFromOld.Name = "StripMenu_Debug_LoadRecordFromOld";
			this.StripMenu_Debug_LoadRecordFromOld.Size = new System.Drawing.Size( 484, 34 );
			this.StripMenu_Debug_LoadRecordFromOld.Text = "旧 api_start2 からレコードを構築(&O)...";
			this.StripMenu_Debug_LoadRecordFromOld.Click += new System.EventHandler( this.StripMenu_Debug_LoadRecordFromOld_Click );
			// 
			// StripMenu_Debug_DeleteOldAPI
			// 
			this.StripMenu_Debug_DeleteOldAPI.Name = "StripMenu_Debug_DeleteOldAPI";
			this.StripMenu_Debug_DeleteOldAPI.Size = new System.Drawing.Size( 484, 34 );
			this.StripMenu_Debug_DeleteOldAPI.Text = "古いAPIデータを削除(&D)";
			this.StripMenu_Debug_DeleteOldAPI.Click += new System.EventHandler( this.StripMenu_Debug_DeleteOldAPI_Click );
			// 
			// StripMenu_Debug_RenameShipResource
			// 
			this.StripMenu_Debug_RenameShipResource.Name = "StripMenu_Debug_RenameShipResource";
			this.StripMenu_Debug_RenameShipResource.Size = new System.Drawing.Size( 484, 34 );
			this.StripMenu_Debug_RenameShipResource.Text = "重命名舰船(&R)...";
			this.StripMenu_Debug_RenameShipResource.Click += new System.EventHandler( this.StripMenu_Debug_RenameShipResource_Click );
			// 
			// StripMenu_Debug_LoadDataFromOld
			// 
			this.StripMenu_Debug_LoadDataFromOld.Name = "StripMenu_Debug_LoadDataFromOld";
			this.StripMenu_Debug_LoadDataFromOld.Size = new System.Drawing.Size(498, 34);
			this.StripMenu_Debug_LoadDataFromOld.Text = "旧 api_start2から深海棲艦を復元(&A)...";
			this.StripMenu_Debug_LoadDataFromOld.Click += new System.EventHandler(this.StripMenu_Debug_LoadDataFromOld_Click);
		}

		private System.Windows.Forms.ToolStripMenuItem StripMenu_Debug;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_Debug_LoadAPIFromFile;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_Debug_LoadInitialAPI;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_Debug_LoadRecordFromOld;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_Debug_DeleteOldAPI;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_Debug_RenameShipResource;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_Debug_LoadDataFromOld;
	}
}
