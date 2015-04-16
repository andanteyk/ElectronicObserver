﻿using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog {
	public partial class DialogAlbumMasterShip : Form {

		private int _shipID;

		private ImageLabel[] Aircrafts;
		private ImageLabel[] Equipments;


		public DialogAlbumMasterShip() {
			InitializeComponent();

			Aircrafts = new ImageLabel[] { Aircraft1, Aircraft2, Aircraft3, Aircraft4, Aircraft5 };
			Equipments = new ImageLabel[] { Equipment1, Equipment2, Equipment3, Equipment4, Equipment5 };


			TitleHP.ImageList =
			TitleFirepower.ImageList =
			TitleTorpedo.ImageList =
			TitleAA.ImageList =
			TitleArmor.ImageList =
			TitleASW.ImageList =
			TitleEvasion.ImageList =
			TitleLOS.ImageList =
			TitleLuck.ImageList =
			TitleSpeed.ImageList =
			TitleRange.ImageList =
			Rarity.ImageList =
			Fuel.ImageList =
			Ammo.ImageList =
			TitleBuildingTime.ImageList =
			MaterialFuel.ImageList =
			MaterialAmmo.ImageList =
			MaterialSteel.ImageList =
			MaterialBauxite.ImageList =
			PowerUpFirepower.ImageList =
			PowerUpTorpedo.ImageList =
			PowerUpAA.ImageList =
			PowerUpArmor.ImageList =
			RemodelBeforeLevel.ImageList =
			RemodelBeforeAmmo.ImageList =
			RemodelBeforeSteel.ImageList =
			RemodelAfterLevel.ImageList =
			RemodelAfterAmmo.ImageList =
			RemodelAfterSteel.ImageList =
				ResourceManager.Instance.Icons;

			TitleAirSuperiority.ImageList =
			TitleDayAttack.ImageList =
			TitleNightAttack.ImageList =
			Equipment1.ImageList =
			Equipment2.ImageList =
			Equipment3.ImageList =
			Equipment4.ImageList =
			Equipment5.ImageList =
				ResourceManager.Instance.Equipments;

			TitleHP.ImageIndex = (int)ResourceManager.IconContent.ParameterHP;
			TitleFirepower.ImageIndex = (int)ResourceManager.IconContent.ParameterFirepower;
			TitleTorpedo.ImageIndex = (int)ResourceManager.IconContent.ParameterTorpedo;
			TitleAA.ImageIndex = (int)ResourceManager.IconContent.ParameterAA;
			TitleArmor.ImageIndex = (int)ResourceManager.IconContent.ParameterArmor;
			TitleASW.ImageIndex = (int)ResourceManager.IconContent.ParameterASW;
			TitleEvasion.ImageIndex = (int)ResourceManager.IconContent.ParameterEvasion;
			TitleLOS.ImageIndex = (int)ResourceManager.IconContent.ParameterLOS;
			TitleLuck.ImageIndex = (int)ResourceManager.IconContent.ParameterLuck;
			TitleSpeed.ImageIndex = (int)ResourceManager.IconContent.ParameterSpeed;
			TitleRange.ImageIndex = (int)ResourceManager.IconContent.ParameterRange;
			Fuel.ImageIndex = (int)ResourceManager.IconContent.ResourceFuel;
			Ammo.ImageIndex = (int)ResourceManager.IconContent.ResourceAmmo;
			TitleBuildingTime.ImageIndex = (int)ResourceManager.IconContent.FormArsenal;
			MaterialFuel.ImageIndex = (int)ResourceManager.IconContent.ResourceFuel;
			MaterialAmmo.ImageIndex = (int)ResourceManager.IconContent.ResourceAmmo;
			MaterialSteel.ImageIndex = (int)ResourceManager.IconContent.ResourceSteel;
			MaterialBauxite.ImageIndex = (int)ResourceManager.IconContent.ResourceBauxite;
			PowerUpFirepower.ImageIndex = (int)ResourceManager.IconContent.ParameterFirepower;
			PowerUpTorpedo.ImageIndex = (int)ResourceManager.IconContent.ParameterTorpedo;
			PowerUpAA.ImageIndex = (int)ResourceManager.IconContent.ParameterAA;
			PowerUpArmor.ImageIndex = (int)ResourceManager.IconContent.ParameterArmor;
			RemodelBeforeAmmo.ImageIndex = (int)ResourceManager.IconContent.ResourceAmmo;
			RemodelBeforeSteel.ImageIndex = (int)ResourceManager.IconContent.ResourceSteel;
			RemodelAfterAmmo.ImageIndex = (int)ResourceManager.IconContent.ResourceAmmo;
			RemodelAfterSteel.ImageIndex = (int)ResourceManager.IconContent.ResourceSteel;
			TitleAirSuperiority.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter;
			TitleDayAttack.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
			TitleNightAttack.ImageIndex = (int)ResourceManager.EquipmentContent.Torpedo;

			TableBattle.Visible = false;
			BasePanelShipGirl.Visible = false;


			ControlHelper.SetDoubleBuffered( TableShipName );
			ControlHelper.SetDoubleBuffered( TableParameterMain );
			ControlHelper.SetDoubleBuffered( TableParameterSub );
			ControlHelper.SetDoubleBuffered( TableConsumption );
			ControlHelper.SetDoubleBuffered( TableEquipment );
			ControlHelper.SetDoubleBuffered( TableArsenal );
			ControlHelper.SetDoubleBuffered( TableRemodel );
			ControlHelper.SetDoubleBuffered( TableBattle );

			ControlHelper.SetDoubleBuffered( ShipView );


			//ShipView Initialize
			ShipView.SuspendLayout();

			ShipView_ShipID.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			ShipView_ShipType.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;


			ShipView.Rows.Clear();

			List<DataGridViewRow> rows = new List<DataGridViewRow>( KCDatabase.Instance.MasterShips.Values.Count( s => s.Name != "なし" ) );

			foreach ( var ship in KCDatabase.Instance.MasterShips.Values ) {

				if ( ship.Name == "なし" ) continue;

				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells( ShipView );
				row.SetValues( ship.ShipID, KCDatabase.Instance.ShipTypes[ship.ShipType].Name, ship.NameWithClass );
				rows.Add( row );

			}
			ShipView.Rows.AddRange( rows.ToArray() );

			ShipView_ShipID.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
			ShipView_ShipType.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;

			ShipView.Sort( ShipView_ShipID, ListSortDirection.Ascending );
			ShipView.ResumeLayout();
		}

		public DialogAlbumMasterShip( int shipID )
			: this() {

			UpdateAlbumPage( shipID );
			
		
			if ( KCDatabase.Instance.MasterShips.ContainsKey( shipID ) ) {
				var row = ShipView.Rows.OfType<DataGridViewRow>().First( r => (int)r.Cells[ShipView_ShipID.Index].Value == shipID );
				if ( row != null )
					ShipView.FirstDisplayedScrollingRowIndex = row.Index;
			}
			
		}



		private void DialogAlbumMasterShip_Load( object sender, EventArgs e ) {

			this.Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormAlbumShip] );

		}




		private void ShipView_SortCompare( object sender, DataGridViewSortCompareEventArgs e ) {

			if ( e.Column.Name == ShipView_ShipType.Name ) {
				e.SortResult =
					KCDatabase.Instance.MasterShips[(int)ShipView.Rows[e.RowIndex1].Cells[0].Value].ShipType -
					KCDatabase.Instance.MasterShips[(int)ShipView.Rows[e.RowIndex2].Cells[0].Value].ShipType;
			} else {
				e.SortResult = ( (IComparable)e.CellValue1 ).CompareTo( e.CellValue2 );
			}

			if ( e.SortResult == 0 ) {
				e.SortResult = (int)( ShipView.Rows[e.RowIndex1].Tag ?? 0 ) - (int)( ShipView.Rows[e.RowIndex2].Tag ?? 0 );
			}

			e.Handled = true;
		}

		private void ShipView_Sorted( object sender, EventArgs e ) {

			for ( int i = 0; i < ShipView.Rows.Count; i++ ) {
				ShipView.Rows[i].Tag = i;
			}

		}



		private void ShipView_CellMouseClick( object sender, DataGridViewCellMouseEventArgs e ) {

			if ( e.RowIndex >= 0 ) {
				int shipID = (int)ShipView.Rows[e.RowIndex].Cells[0].Value;

				if ( ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 ) {
					Cursor = Cursors.AppStarting;
					new DialogAlbumMasterShip( shipID ).Show();
					Cursor = Cursors.Default;

				} else if ( ( e.Button & System.Windows.Forms.MouseButtons.Left ) != 0 ) {
					UpdateAlbumPage( shipID );
				}
			}

		}




		private void UpdateAlbumPage( int shipID ) {

			KCDatabase db = KCDatabase.Instance;
			ShipDataMaster ship = db.MasterShips[shipID];

			if ( ship == null ) return;


			BasePanelShipGirl.SuspendLayout();

			//header
			TableShipName.SuspendLayout();
			_shipID = shipID;
			ShipID.Text = ship.ShipID.ToString();
			ToolTipInfo.SetToolTip( ShipID, ship.ResourceName );
			AlbumNo.Text = ship.AlbumNo.ToString();

            ShipType.Text = ship.IsLandBase ? LoadResources.getter("DialogAlbumMasterShip_1") : db.ShipTypes[ship.ShipType].Name;
			ShipName.Text = ship.NameWithClass;
			ToolTipInfo.SetToolTip( ShipName, !ship.IsAbyssalShip ? ship.NameReading : null );
			TableShipName.ResumeLayout();


			//main parameter
			TableParameterMain.SuspendLayout();

			if ( !ship.IsAbyssalShip ) {

                TitleParameterMin.Text = LoadResources.getter("DialogAlbumMasterShip_2");
                TitleParameterMax.Text = LoadResources.getter("DialogAlbumMasterShip_3");

				HPMin.Text = ship.HPMin.ToString();
				HPMax.Text = ship.HPMaxMarried.ToString();

				FirepowerMin.Text = ship.FirepowerMin.ToString();
				FirepowerMax.Text = ship.FirepowerMax.ToString();

				TorpedoMin.Text = ship.TorpedoMin.ToString();
				TorpedoMax.Text = ship.TorpedoMax.ToString();

				AAMin.Text = ship.AAMin.ToString();
				AAMax.Text = ship.AAMax.ToString();

				ArmorMin.Text = ship.ArmorMin.ToString();
				ArmorMax.Text = ship.ArmorMax.ToString();

				ASWMin.Text = GetParameterMinBound( ship.ASW );
				ASWMax.Text = GetParameterMax( ship.ASW );

				EvasionMin.Text = GetParameterMinBound( ship.Evasion );
				EvasionMax.Text = GetParameterMax( ship.Evasion );

				LOSMin.Text = GetParameterMinBound( ship.LOS );
				LOSMax.Text = GetParameterMax( ship.LOS );

				LuckMin.Text = ship.LuckMin.ToString();
				LuckMax.Text = ship.LuckMax.ToString();

			} else {

				int hp = ship.HPMin;
				int firepower = ship.FirepowerMax;
				int torpedo = ship.TorpedoMax;
				int aa = ship.AAMax;
				int armor = ship.ArmorMax;
				int asw = ship.ASW != null && ship.ASW.Maximum != ShipParameterRecord.Parameter.MaximumDefault ? ship.ASW.Maximum : 0;
				int evasion = ship.Evasion != null && ship.Evasion.Maximum != ShipParameterRecord.Parameter.MaximumDefault ? ship.Evasion.Maximum : 0;
				int los = ship.LOS != null && ship.LOS.Maximum != ShipParameterRecord.Parameter.MaximumDefault ? ship.LOS.Maximum : 0;
				int luck = ship.LuckMax;

				if ( ship.DefaultSlot != null ) {
					int count = ship.DefaultSlot.Count;
					for ( int i = 0; i < count; i++ ) {
						EquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[ship.DefaultSlot[i]];
						if ( eq == null ) continue;

						firepower += eq.Firepower;
						torpedo += eq.Torpedo;
						aa += eq.AA;
						armor += eq.Armor;
						asw += eq.ASW;
						evasion += eq.Evasion;
						los += eq.LOS;
						luck += eq.Luck;
					}
				}

                TitleParameterMin.Text = LoadResources.getter("DialogAlbumMasterShip_4");
                TitleParameterMax.Text = LoadResources.getter("DialogAlbumMasterShip_5");

				HPMin.Text = ship.HPMin.ToString();
				HPMax.Text = hp.ToString();

				FirepowerMin.Text = ship.FirepowerMax.ToString();
				FirepowerMax.Text = firepower.ToString();

				TorpedoMin.Text = ship.TorpedoMax.ToString();
				TorpedoMax.Text = torpedo.ToString();

				AAMin.Text = ship.AAMax.ToString();
				AAMax.Text = aa.ToString();

				ArmorMin.Text = ship.ArmorMax.ToString();
				ArmorMax.Text = armor.ToString();

				ASWMin.Text = ship.ASW != null && ship.ASW.Maximum != ShipParameterRecord.Parameter.MaximumDefault ? ship.ASW.Maximum.ToString() : "???";
				ASWMax.Text = asw.ToString();

				EvasionMin.Text = ship.Evasion != null && ship.Evasion.Maximum != ShipParameterRecord.Parameter.MaximumDefault ? ship.Evasion.Maximum.ToString() : "???";
				EvasionMax.Text = evasion.ToString();

				LOSMin.Text = ship.LOS != null && ship.LOS.Maximum != ShipParameterRecord.Parameter.MaximumDefault ? ship.LOS.Maximum.ToString() : "???";
				LOSMax.Text = los.ToString();

				LuckMin.Text = ship.LuckMax.ToString();
				LuckMax.Text = luck.ToString();

			}
			UpdateLevelParameter( ship.ShipID );

			TableParameterMain.ResumeLayout();


			//sub parameter
			TableParameterSub.SuspendLayout();

			Speed.Text = Constants.GetSpeed( ship.Speed );
			Range.Text = Constants.GetRange( ship.Range );
			Rarity.Text = Constants.GetShipRarity( ship.Rarity );
			Rarity.ImageIndex = (int)ResourceManager.IconContent.RarityRed + ship.Rarity;		//checkme

			TableParameterSub.ResumeLayout();

			TableConsumption.SuspendLayout();
			
			Fuel.Text = ship.Fuel.ToString();
			Ammo.Text = ship.Ammo.ToString();

			string tooltiptext = string.Format(
                LoadResources.getter("DialogAlbumMasterShip_6"),
				( ship.Fuel * 0.06 ),
				( ship.Fuel * 0.032 ),
				(int)( ship.Fuel * 0.06 * ( ship.HPMaxMarried - 1 ) ),
				(int)( ship.Fuel * 0.032 * ( ship.HPMaxMarried - 1 ) )
				) ;

			ToolTipInfo.SetToolTip( TableConsumption, tooltiptext );
			ToolTipInfo.SetToolTip( TitleConsumption, tooltiptext );
			ToolTipInfo.SetToolTip( Fuel, tooltiptext );
			ToolTipInfo.SetToolTip( Ammo, tooltiptext );

			TableConsumption.ResumeLayout();

			Description.Text = ship.MessageAlbum != "" ? ship.MessageAlbum : ship.MessageGet;
			Description.Tag = ship.MessageAlbum != "" ? 1 : 0;


			//equipment
			TableEquipment.SuspendLayout();

			for ( int i = 0; i < Equipments.Length; i++ ) {

				if ( ship.Aircraft[i] > 0 || i < ship.SlotSize )
					Aircrafts[i].Text = ship.Aircraft[i].ToString();
				else
					Aircrafts[i].Text = "";


				ToolTipInfo.SetToolTip( Equipments[i], null );

				if ( ship.DefaultSlot == null ) {
					if ( i < ship.SlotSize ) {
						Equipments[i].Text = "???";
						Equipments[i].ImageIndex = (int)ResourceManager.EquipmentContent.Unknown;
					} else {
						Equipments[i].Text = "";
						Equipments[i].ImageIndex = (int)ResourceManager.EquipmentContent.Locked;
					}

				} else if ( ship.DefaultSlot[i] != -1 ) {
					EquipmentDataMaster eq = db.MasterEquipments[ship.DefaultSlot[i]];
					Equipments[i].Text = eq.Name;

					int eqicon = eq.EquipmentType[3];
					if ( eqicon >= (int)ResourceManager.EquipmentContent.Locked )
						eqicon = (int)ResourceManager.EquipmentContent.Unknown;

					Equipments[i].ImageIndex = eqicon;
					
					{
						StringBuilder sb = new StringBuilder();

						sb.AppendFormat( "{0} {1}\r\n", eq.CategoryTypeInstance.Name, eq.Name );
                        if (eq.Firepower != 0) sb.AppendFormat(LoadResources.getter("DialogAlbumMasterShip_7"), eq.Firepower > 0 ? "+" : "", eq.Firepower);
                        if (eq.Torpedo != 0) sb.AppendFormat(LoadResources.getter("DialogAlbumMasterShip_8"), eq.Torpedo > 0 ? "+" : "", eq.Torpedo);
                        if (eq.AA != 0) sb.AppendFormat(LoadResources.getter("DialogAlbumMasterShip_9"), eq.AA > 0 ? "+" : "", eq.AA);
                        if (eq.Armor != 0) sb.AppendFormat(LoadResources.getter("DialogAlbumMasterShip_10"), eq.Armor > 0 ? "+" : "", eq.Armor);
                        if (eq.ASW != 0) sb.AppendFormat(LoadResources.getter("DialogAlbumMasterShip_11"), eq.ASW > 0 ? "+" : "", eq.ASW);
                        if (eq.Evasion != 0) sb.AppendFormat(LoadResources.getter("DialogAlbumMasterShip_12"), eq.Evasion > 0 ? "+" : "", eq.Evasion);
                        if (eq.LOS != 0) sb.AppendFormat(LoadResources.getter("DialogAlbumMasterShip_13"), eq.LOS > 0 ? "+" : "", eq.LOS);
                        if (eq.Accuracy != 0) sb.AppendFormat(LoadResources.getter("DialogAlbumMasterShip_14"), eq.Accuracy > 0 ? "+" : "", eq.Accuracy);
                        if (eq.Bomber != 0) sb.AppendFormat(LoadResources.getter("DialogAlbumMasterShip_15"), eq.Bomber > 0 ? "+" : "", eq.Bomber);
                        sb.AppendLine(LoadResources.getter("DialogAlbumMasterShip_16"));

						ToolTipInfo.SetToolTip( Equipments[i], sb.ToString() );
					}

				} else if ( i < ship.SlotSize ) {
                    Equipments[i].Text = LoadResources.getter("DialogAlbumMasterShip_17");
					Equipments[i].ImageIndex = (int)ResourceManager.EquipmentContent.Nothing;

				} else {
					Equipments[i].Text = "";
					Equipments[i].ImageIndex = (int)ResourceManager.EquipmentContent.Locked;
				}
			}

			TableEquipment.ResumeLayout();


			//arsenal
			TableArsenal.SuspendLayout();
			BuildingTime.Text = DateTimeHelper.ToTimeRemainString( new TimeSpan( 0, ship.BuildingTime, 0 ) );

			MaterialFuel.Text = ship.Material[0].ToString();
			MaterialAmmo.Text = ship.Material[1].ToString();
			MaterialSteel.Text = ship.Material[2].ToString();
			MaterialBauxite.Text = ship.Material[3].ToString();

			PowerUpFirepower.Text = ship.PowerUp[0].ToString();
			PowerUpTorpedo.Text = ship.PowerUp[1].ToString();
			PowerUpAA.Text = ship.PowerUp[2].ToString();
			PowerUpArmor.Text = ship.PowerUp[3].ToString();

			TableArsenal.ResumeLayout();


			//remodel
			if ( !ship.IsAbyssalShip ) {

				TableRemodel.SuspendLayout();

				if ( ship.RemodelBeforeShipID == 0 ) {
                    RemodelBeforeShipName.Text = LoadResources.getter("DialogAlbumMasterShip_17");
					RemodelBeforeLevel.Text = "";
					RemodelBeforeLevel.ImageIndex = -1;
					RemodelBeforeAmmo.Text = "-";
					RemodelBeforeSteel.Text = "-";
				} else {
					ShipDataMaster sbefore = ship.RemodelBeforeShip;
					RemodelBeforeShipName.Text = sbefore.Name;
					RemodelBeforeLevel.Text = string.Format( "Lv. {0}", sbefore.RemodelAfterLevel );
					RemodelBeforeLevel.ImageIndex = sbefore.NeedBlueprint > 0 ? (int)ResourceManager.IconContent.ItemBlueprint : -1;
					RemodelBeforeAmmo.Text = sbefore.RemodelAmmo.ToString();
					RemodelBeforeSteel.Text = sbefore.RemodelSteel.ToString();
				}

				if ( ship.RemodelAfterShipID == 0 ) {
                    RemodelAfterShipName.Text = LoadResources.getter("DialogAlbumMasterShip_17");
					RemodelAfterLevel.Text = "";
					RemodelAfterLevel.ImageIndex = -1;
					RemodelAfterAmmo.Text = "-";
					RemodelAfterSteel.Text = "-";
				} else {
					RemodelAfterShipName.Text = ship.RemodelAfterShip.Name;
					RemodelAfterLevel.Text = string.Format( "Lv. {0}", ship.RemodelAfterLevel );
					RemodelAfterLevel.ImageIndex = ship.NeedBlueprint > 0 ? (int)ResourceManager.IconContent.ItemBlueprint : -1;
					RemodelAfterAmmo.Text = ship.RemodelAmmo.ToString();
					RemodelAfterSteel.Text = ship.RemodelSteel.ToString();
				}
				TableRemodel.ResumeLayout();


				TableRemodel.Visible = true;
				TableBattle.Visible = false;


			} else {

				TableBattle.SuspendLayout();

				AirSuperiority.Text = Calculator.GetAirSuperiority( ship ).ToString();
				DayAttack.Text = Constants.GetDayAttackKind( Calculator.GetDayAttackKind( ship.DefaultSlot == null ? null : ship.DefaultSlot.ToArray(), ship.ShipID, -1 ) );
				NightAttack.Text = Constants.GetNightAttackKind( Calculator.GetNightAttackKind( ship.DefaultSlot == null ? null : ship.DefaultSlot.ToArray(), ship.ShipID, -1 ) );

				TableBattle.ResumeLayout();

				TableRemodel.Visible = false;
				TableBattle.Visible = true;

			}




			BasePanelShipGirl.ResumeLayout();
			BasePanelShipGirl.Visible = true;


            this.Text = LoadResources.getter("DialogAlbumMasterShip_18") + ship.NameWithClass;

		}


		private void UpdateLevelParameter( int shipID ) {

			ShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];

			if ( !ship.IsAbyssalShip ) {
				ASWLevel.Text = EstimateParameter( (int)ParameterLevel.Value, ship.ASW );
				EvasionLevel.Text = EstimateParameter( (int)ParameterLevel.Value, ship.Evasion );
				LOSLevel.Text = EstimateParameter( (int)ParameterLevel.Value, ship.LOS );
				ASWLevel.Visible = 
				ASWSeparater.Visible =
				EvasionLevel.Visible =
 				EvasionSeparater.Visible =
				LOSLevel.Visible = 
				LOSSeparater.Visible = true;

			} else {
				ASWLevel.Visible =
				ASWSeparater.Visible =
				EvasionLevel.Visible =
				EvasionSeparater.Visible =
				LOSLevel.Visible =
				LOSSeparater.Visible = false;
			}
		}

		private string EstimateParameter( int level, ShipParameterRecord.Parameter param ) {

			if ( param == null || param.Maximum == ShipParameterRecord.Parameter.MaximumDefault )
				return "???";

			int min = (int)( param.MinimumEstMin + ( param.Maximum - param.MinimumEstMin ) * level / 99.0 );
			int max = (int)( param.MinimumEstMax + ( param.Maximum - param.MinimumEstMax ) * level / 99.0 );

			if ( min == max )
				return min.ToString();
			else
				return string.Format( "{0}～{1}", Math.Min( min, max ), Math.Max( min, max ) );
		}


		private string GetParameterMinBound( ShipParameterRecord.Parameter param ) {

			if ( param == null || param.MinimumEstMax == ShipParameterRecord.Parameter.MaximumDefault )
				return "???";
			else if ( param.MinimumEstMin == param.MinimumEstMax )
				return param.MinimumEstMin.ToString();
			else if ( param.MinimumEstMin == ShipParameterRecord.Parameter.MinimumDefault && param.MinimumEstMax == param.Maximum )
				return "???";
			else
				return string.Format( "{0}～{1}", param.MinimumEstMin, param.MinimumEstMax );

		}

		private string GetParameterMax( ShipParameterRecord.Parameter param ) {

			if ( param == null || param.Maximum == ShipParameterRecord.Parameter.MaximumDefault )
				return "???";
			else
				return param.Maximum.ToString();

		}


		private void ParameterLevel_ValueChanged( object sender, EventArgs e ) {
			if ( _shipID != -1 ) {
				LevelTimer.Start();
				//UpdateLevelParameter( _shipID );
			}
		}

		private void LevelTimer_Tick( object sender, EventArgs e ) {
			if ( _shipID != -1 )
				UpdateLevelParameter( _shipID );
		}


		private void TableParameterMain_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
			/*/
			if ( e.Column == 0 )
				e.Graphics.DrawLine( Pens.Silver, e.CellBounds.Right - 1, e.CellBounds.Y, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
			//*/
		}

		private void TableParameterSub_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}

		private void TableConsumption_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}

		private void TableEquipment_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}

		private void TableArsenal_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}

		private void TableRemodel_CellPaint( object sender, TableLayoutCellPaintEventArgs e ) {
			if ( e.Row % 2 == 1 )
				e.Graphics.DrawLine( Pens.Silver, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1 );
		}



		private void RemodelBeforeShipName_MouseClick( object sender, MouseEventArgs e ) {

			if ( _shipID == -1 ) return;
			var ship = KCDatabase.Instance.MasterShips[_shipID];

			if ( ship != null && ship.RemodelBeforeShipID != 0 ) {

				if ( ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 )
					new DialogAlbumMasterShip( ship.RemodelBeforeShipID ).Show();

				else if ( ( e.Button & System.Windows.Forms.MouseButtons.Left ) != 0 )
					UpdateAlbumPage( ship.RemodelBeforeShipID );
			}
		}

		private void RemodelAfterShipName_MouseClick( object sender, MouseEventArgs e ) {

			if ( _shipID == -1 ) return;
			var ship = KCDatabase.Instance.MasterShips[_shipID];

			if ( ship != null && ship.RemodelAfterShipID != 0 ) {

				if ( ( e.Button & System.Windows.Forms.MouseButtons.Right ) != 0 )
					new DialogAlbumMasterShip( ship.RemodelAfterShipID ).Show();

				else if ( ( e.Button & System.Windows.Forms.MouseButtons.Left ) != 0 )
					UpdateAlbumPage( ship.RemodelAfterShipID );
			}
		}



		private void Equipment_MouseClick( object sender, MouseEventArgs e ) {

			if ( e.Button == System.Windows.Forms.MouseButtons.Right ) {

				for ( int i = 0; i < Equipments.Length; i++ ) {
					if ( sender == Equipments[i] ) {

						if ( _shipID != -1 ) {
							ShipDataMaster ship = KCDatabase.Instance.MasterShips[_shipID];

							if ( ship != null && ship.DefaultSlot != null && i < ship.DefaultSlot.Count && ship.DefaultSlot[i] != -1 ) {
								Cursor = Cursors.AppStarting;
								new DialogAlbumMasterEquipment( ship.DefaultSlot[i] ).Show();
								Cursor = Cursors.Default;
							}
						}
					}
				}

			}
		}




		private void StripMenu_File_OutputCSVUser_Click( object sender, EventArgs e ) {

			if ( SaveCSVDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

				try {

					using ( StreamWriter sw = new StreamWriter( SaveCSVDialog.FileName, false, Utility.Configuration.Config.Log.FileEncoding ) ) {

                        sw.WriteLine(LoadResources.getter("DialogAlbumMasterShip_19"));
						string arg = string.Format( "{{{0}}}", string.Join( "},{", Enumerable.Range( 0, 59 ) ) );

						foreach ( ShipDataMaster ship in KCDatabase.Instance.MasterShips.Values ) {

                            if (ship.Name == LoadResources.getter("DialogAlbumMasterShip_20")) continue;

							sw.WriteLine( arg,
								ship.ShipID,
								ship.AlbumNo,
								KCDatabase.Instance.ShipTypes[ship.ShipType].Name,
								ship.Name,
								ship.NameReading,
								ship.RemodelBeforeShipID > 0 ? ship.RemodelBeforeShip.Name : "-",
								ship.RemodelAfterShipID > 0 ? ship.RemodelAfterShip.Name : "-",
								ship.RemodelAfterLevel,
								ship.RemodelAmmo,
								ship.RemodelSteel,
                                ship.NeedBlueprint > 0 ? ship.NeedBlueprint + LoadResources.getter("DialogAlbumMasterShip_21") : "-",
								ship.HPMin,
								ship.HPMaxMarried,
								ship.FirepowerMin,
								ship.FirepowerMax,
								ship.TorpedoMin,
								ship.TorpedoMax,
								ship.AAMin,
								ship.AAMax,
								ship.ArmorMin,
								ship.ArmorMax,
								ship.ASW != null && !ship.ASW.IsMinimumDefault ? ship.ASW.Minimum.ToString() : "???",
								ship.ASW != null && !ship.ASW.IsMaximumDefault ? ship.ASW.Maximum.ToString() : "???",
								ship.Evasion != null && !ship.Evasion.IsMinimumDefault ? ship.Evasion.Minimum.ToString() : "???",
								ship.Evasion != null && !ship.Evasion.IsMaximumDefault ? ship.Evasion.Maximum.ToString() : "???",
								ship.LOS != null && !ship.LOS.IsMinimumDefault ? ship.LOS.Minimum.ToString() : "???",
								ship.LOS != null && !ship.LOS.IsMaximumDefault ? ship.LOS.Maximum.ToString() : "???",
								ship.LuckMin,
								ship.LuckMax,
								Constants.GetSpeed( ship.Speed ),
								Constants.GetRange( ship.Range ),
								Constants.GetShipRarity( ship.Rarity ),
								ship.SlotSize,
								ship.Aircraft[0],
								ship.Aircraft[1],
								ship.Aircraft[2],
								ship.Aircraft[3],
								ship.Aircraft[4],
								ship.DefaultSlot != null ? ( ship.DefaultSlot[0] != -1 ? KCDatabase.Instance.MasterEquipments[ship.DefaultSlot[0]].Name : ( ship.SlotSize > 0 ? "(なし)" : "" ) ) : "???",
								ship.DefaultSlot != null ? ( ship.DefaultSlot[1] != -1 ? KCDatabase.Instance.MasterEquipments[ship.DefaultSlot[1]].Name : ( ship.SlotSize > 1 ? "(なし)" : "" ) ) : "???",
								ship.DefaultSlot != null ? ( ship.DefaultSlot[2] != -1 ? KCDatabase.Instance.MasterEquipments[ship.DefaultSlot[2]].Name : ( ship.SlotSize > 2 ? "(なし)" : "" ) ) : "???",
								ship.DefaultSlot != null ? ( ship.DefaultSlot[3] != -1 ? KCDatabase.Instance.MasterEquipments[ship.DefaultSlot[3]].Name : ( ship.SlotSize > 3 ? "(なし)" : "" ) ) : "???",
								ship.DefaultSlot != null ? ( ship.DefaultSlot[4] != -1 ? KCDatabase.Instance.MasterEquipments[ship.DefaultSlot[4]].Name : ( ship.SlotSize > 4 ? "(なし)" : "" ) ) : "???",
								DateTimeHelper.ToTimeRemainString( new TimeSpan( 0, ship.BuildingTime, 0 ) ),
								ship.Material[0],
								ship.Material[1],
								ship.Material[2],
								ship.Material[3],
								ship.PowerUp[0],
								ship.PowerUp[1],
								ship.PowerUp[2],
								ship.PowerUp[3],
								ship.MessageGet.Replace( "\n", "<br>" ),
								ship.MessageAlbum.Replace( "\n", "<br>" ),
								ship.Fuel,
								ship.Ammo,
								Constants.GetVoiceFlag( ship.VoiceFlag ),
								ship.ResourceName,
								ship.ResourceVersion
								);

						}

					}

				} catch ( Exception ex ) {

                    Utility.ErrorReporter.SendErrorReport(ex, LoadResources.getter("DialogAlbumMasterShip_23"));
                    MessageBox.Show(LoadResources.getter("DialogAlbumMasterShip_24") + ex.Message, LoadResources.getter("DialogAlbumMasterShip_25"), MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

			}

		}


		private void StripMenu_File_OutputCSVData_Click( object sender, EventArgs e ) {

			if ( SaveCSVDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

				try {

					using ( StreamWriter sw = new StreamWriter( SaveCSVDialog.FileName, false, Utility.Configuration.Config.Log.FileEncoding ) ) {

                        sw.WriteLine(LoadResources.getter("DialogAlbumMasterShip_22"));
						string arg = string.Format( "{{{0}}}", string.Join( "},{", Enumerable.Range( 0, 69 ) ) );

						foreach ( ShipDataMaster ship in KCDatabase.Instance.MasterShips.Values ) {

							sw.WriteLine( arg,
								ship.ShipID,
								ship.AlbumNo,
								ship.Name,
								ship.NameReading,
								ship.ShipType,
								ship.RemodelBeforeShipID,
								ship.RemodelAfterShipID,
								ship.RemodelAfterLevel,
								ship.RemodelAmmo,
								ship.RemodelSteel,
								ship.NeedBlueprint,
								ship.HPMin,
								ship.HPMax,
								ship.HPMaxMarried,
								ship.FirepowerMin,
								ship.FirepowerMax,
								ship.TorpedoMin,
								ship.TorpedoMax,
								ship.AAMin,
								ship.AAMax,
								ship.ArmorMin,
								ship.ArmorMax,
								ship.ASW != null ? ship.ASW.MinimumEstMin : ShipParameterRecord.Parameter.MinimumDefault,
								ship.ASW != null ? ship.ASW.MinimumEstMax : ShipParameterRecord.Parameter.MaximumDefault,
								ship.ASW != null ? ship.ASW.Maximum : ShipParameterRecord.Parameter.MaximumDefault,
								ship.ASW != null ? ship.ASW.GetEstParameterMin( 150 ) : ShipParameterRecord.Parameter.MinimumDefault,
								ship.ASW != null ? ship.ASW.GetEstParameterMax( 150 ) : ShipParameterRecord.Parameter.MaximumDefault,
								ship.Evasion != null ? ship.Evasion.MinimumEstMin : ShipParameterRecord.Parameter.MinimumDefault,
								ship.Evasion != null ? ship.Evasion.MinimumEstMax : ShipParameterRecord.Parameter.MaximumDefault,
								ship.Evasion != null ? ship.Evasion.Maximum : ShipParameterRecord.Parameter.MaximumDefault,
								ship.Evasion != null ? ship.Evasion.GetEstParameterMin( 150 ) : ShipParameterRecord.Parameter.MinimumDefault,
								ship.Evasion != null ? ship.Evasion.GetEstParameterMax( 150 ) : ShipParameterRecord.Parameter.MaximumDefault,
								ship.LOS != null ? ship.LOS.MinimumEstMin : ShipParameterRecord.Parameter.MinimumDefault,
								ship.LOS != null ? ship.LOS.MinimumEstMax : ShipParameterRecord.Parameter.MaximumDefault,
								ship.LOS != null ? ship.LOS.Maximum : ShipParameterRecord.Parameter.MaximumDefault,
								ship.LOS != null ? ship.LOS.GetEstParameterMin( 150 ) : ShipParameterRecord.Parameter.MinimumDefault,
								ship.LOS != null ? ship.LOS.GetEstParameterMax( 150 ) : ShipParameterRecord.Parameter.MaximumDefault,
								ship.LuckMin,
								ship.LuckMax,
								ship.Speed,
								ship.Range,
								ship.Rarity,
								ship.SlotSize,
								ship.Aircraft[0],
								ship.Aircraft[1],
								ship.Aircraft[2],
								ship.Aircraft[3],
								ship.Aircraft[4],
								ship.DefaultSlot != null ? ship.DefaultSlot[0] : -1,
								ship.DefaultSlot != null ? ship.DefaultSlot[1] : -1,
								ship.DefaultSlot != null ? ship.DefaultSlot[2] : -1,
								ship.DefaultSlot != null ? ship.DefaultSlot[3] : -1,
								ship.DefaultSlot != null ? ship.DefaultSlot[4] : -1,
								ship.BuildingTime,
								ship.Material[0],
								ship.Material[1],
								ship.Material[2],
								ship.Material[3],
								ship.PowerUp[0],
								ship.PowerUp[1],
								ship.PowerUp[2],
								ship.PowerUp[3],
								ship.MessageGet.Replace( "\n", "<br>" ),
								ship.MessageAlbum.Replace( "\n", "<br>" ),
								ship.Fuel,
								ship.Ammo,
								ship.VoiceFlag,
								ship.ResourceName,
								ship.ResourceVersion
								);

						}

					}

				} catch ( Exception ex ) {

                    Utility.ErrorReporter.SendErrorReport(ex, LoadResources.getter("DialogAlbumMasterShip_23"));
                    MessageBox.Show(LoadResources.getter("DialogAlbumMasterShip_24") + ex.Message, LoadResources.getter("DialogAlbumMasterShip_25"), MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

			}

		}



		private void DialogAlbumMasterShip_FormClosed( object sender, FormClosedEventArgs e ) {

			ResourceManager.DestroyIcon( Icon );

		}



		private void Description_Click( object sender, EventArgs e ) {

			int tag = Description.Tag as int? ?? 0;
			ShipDataMaster ship = KCDatabase.Instance.MasterShips[_shipID];

			if ( ship == null ) return;

			if ( tag == 0 && ship.MessageAlbum.Length > 0 ) {
				Description.Text = ship.MessageAlbum;
				Description.Tag = 1;

			} else {
				Description.Text = ship.MessageGet;
				Description.Tag = 0;
			}
		}

	}
}
