using ElectronicObserver.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
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
	public partial class FormShipGroup : DockContent {

		private readonly Color TabActiveColor = Color.White;
		private readonly Color TabInactiveColor = SystemColors.Control;


		public FormShipGroup( FormMain parent ) {
			InitializeComponent();
		}

		private void FormShipGroup_Load( object sender, EventArgs e ) {

			Font = new Font( "Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			ShipView.Font = Font;

			TabPanel.Controls.Add( CreateTabLabel( -1 ) );

		}


		private ImageLabel CreateTabLabel( int id ) {

			ImageLabel label = new ImageLabel();
			label.Text = KCDatabase.Instance.ShipGroup[id] != null ? KCDatabase.Instance.ShipGroup[id].Name : "全所属艦";
			label.Anchor = AnchorStyles.Left;
			label.Font = Font;
			label.BackColor = TabInactiveColor;
			label.BorderStyle = BorderStyle.FixedSingle;
			label.Padding = new Padding( 1, 1, 3, 3 );
			label.Margin = new Padding( 2, 0, 2, 0 );
			label.ImageAlign = ContentAlignment.MiddleCenter;
			label.AutoSize = true;

			//undone:イベントと固有IDの追加(内部データとの紐付)
			label.Click += TabLabel_Click;
			label.Tag = id;

			return label;
		}


		void TabLabel_Click( object sender, EventArgs e ) {
			//undone:指定されたタブに切り替える
			ChangeShipView( (int)( (ImageLabel)sender ).Tag );
		}


		private void ChangeShipView( int groupID ) {

			var group = KCDatabase.Instance.ShipGroup[groupID];

			ShipView.SuspendLayout();

			ShipView.Rows.Clear();

			IEnumerable<ShipData> ships = group != null ? group.MembersInstance.AsEnumerable() : KCDatabase.Instance.Ships.Values;
			List<DataGridViewRow> rows = new List<DataGridViewRow>( ships.Count() );

			foreach ( ShipData ship in ships ) {

				if ( ship == null ) continue;
				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells( ShipView );
				row.SetValues(
					ship.MasterID,
					KCDatabase.Instance.ShipTypes[ship.MasterShip.ShipType].Name,
					ship.MasterShip.Name,
					ship.Level,
					ship.ExpNext,
					-1,		//undone:nextremodel
					string.Format( "{0}/{1}", ship.HPCurrent, ship.HPMax ),
					ship.Condition,
					string.Format( "{0}/{1}", ship.Fuel, ship.MasterShip.Fuel ),
					string.Format( "{0}/{1}", ship.Ammo, ship.MasterShip.Ammo ),
					string.Format( "[{0}/{1}]{2}", ship.Aircraft[0], ship.MasterShip.Aircraft[0], ship.SlotInstance[0] != null ? ship.SlotInstance[0].NameWithLevel : "" ),
					string.Format( "[{0}/{1}]{2}", ship.Aircraft[1], ship.MasterShip.Aircraft[1], ship.SlotInstance[1] != null ? ship.SlotInstance[1].NameWithLevel : "" ),
					string.Format( "[{0}/{1}]{2}", ship.Aircraft[2], ship.MasterShip.Aircraft[2], ship.SlotInstance[2] != null ? ship.SlotInstance[2].NameWithLevel : "" ),
					string.Format( "[{0}/{1}]{2}", ship.Aircraft[3], ship.MasterShip.Aircraft[3], ship.SlotInstance[3] != null ? ship.SlotInstance[3].NameWithLevel : "" ),
					string.Format( "[{0}/{1}]{2}", ship.Aircraft[4], ship.MasterShip.Aircraft[4], ship.SlotInstance[4] != null ? ship.SlotInstance[4].NameWithLevel : "" ),
					DateTimeHelper.ToTimeRemainString( DateTimeHelper.FromAPITimeSpan( ship.RepairTime ) ),
					ship.FirepowerBase,
					ship.MasterShip.FirepowerMax - ship.FirepowerBase,
					ship.TorpedoBase,
					ship.MasterShip.TorpedoMax - ship.TorpedoBase,
					ship.AABase,
					ship.MasterShip.AAMax - ship.AABase,
					ship.ArmorBase,
					ship.MasterShip.ArmorMax - ship.ArmorBase,
					ship.ASWBase,
					ship.EvasionBase,
					ship.LOSBase,
					ship.LuckBase,
					ship.MasterShip.LuckMax - ship.LuckBase,
					ship.IsLocked ? "❤" : "",
					ship.SallyArea != -1 ? ship.SallyArea.ToString() : ""
					);

				rows.Add( row );

			}

			for ( int i = 0; i < rows.Count; i++ )
				rows[i].Tag = i;

			ShipView.Rows.AddRange( rows.ToArray() );
			ShipView.ResumeLayout();

		}


		protected override string GetPersistString() {
			return "ShipGroup";
		}

	}
}
