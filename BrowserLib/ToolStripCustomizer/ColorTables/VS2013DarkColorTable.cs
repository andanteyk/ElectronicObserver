using System.Drawing;

namespace ToolStripCustomizer.ColorTables {
	internal sealed class VS2013DarkColorTable : PresetColorTable {
		public VS2013DarkColorTable()
			: base( "Visual Studio 2013 Dark" ) {
		}

		public override Color ButtonSelectedHighlight {
			get { return ButtonSelectedGradientMiddle; }
		}

		public override Color ButtonSelectedHighlightBorder {
			get { return ButtonSelectedBorder; }
		}

		public override Color ButtonPressedHighlight {
			get { return ButtonPressedGradientMiddle; }
		}

		public override Color ButtonPressedHighlightBorder {
			get { return ButtonPressedBorder; }
		}

		public override Color ButtonCheckedHighlight {
			get { return ButtonCheckedGradientMiddle; }
		}

		public override Color ButtonCheckedHighlightBorder {
			get { return ButtonSelectedBorder; }
		}

		public override Color ButtonPressedBorder {
			get { return ButtonSelectedBorder; }
		}

		public override Color ButtonSelectedBorder {
			get { return Color.FromArgb( 255, 98, 98, 98 ); }
		}

		public override Color ButtonCheckedGradientBegin {
			get { return Color.FromArgb( 255, 144, 144, 144 ); }
		}

		public override Color ButtonCheckedGradientMiddle {
			get { return Color.FromArgb( 255, 170, 170, 170 ); }
		}

		public override Color ButtonCheckedGradientEnd {
			get { return Color.FromArgb( 255, 170, 170, 170 ); }
		}

		public override Color ButtonSelectedGradientBegin {
			get { return Color.FromArgb( 255, 170, 170, 170 ); }
		}

		public override Color ButtonSelectedGradientMiddle {
			get { return Color.FromArgb( 255, 170, 170, 170 ); }
		}

		public override Color ButtonSelectedGradientEnd {
			get { return Color.FromArgb( 255, 170, 170, 170 ); }
		}

		public override Color ButtonPressedGradientBegin {
			get { return Color.FromArgb( 255, 170, 170, 170 ); }
		}

		public override Color ButtonPressedGradientMiddle {
			get { return Color.FromArgb( 255, 170, 170, 170 ); }
		}

		public override Color ButtonPressedGradientEnd {
			get { return Color.FromArgb( 255, 170, 170, 170 ); }
		}

		public override Color CheckBackground {
			get { return Color.FromArgb( 255, 173, 173, 173 ); }
		}

		public override Color CheckSelectedBackground {
			get { return Color.FromArgb( 255, 173, 173, 173 ); }
		}

		public override Color CheckPressedBackground {
			get { return Color.FromArgb( 255, 140, 140, 140 ); }
		}

		public override Color GripDark {
			get { return Color.FromArgb( 255, 22, 22, 22 ); }
		}

		public override Color GripLight {
			get { return Color.FromArgb( 255, 83, 83, 83 ); }
		}

		public override Color ImageMarginGradientBegin {
			get { return Color.FromArgb( 255, 125, 125, 125 ); }
		}

		public override Color ImageMarginGradientMiddle {
			get { return Color.FromArgb( 255, 125, 125, 125 ); }
		}

		public override Color ImageMarginGradientEnd {
			get { return Color.FromArgb( 255, 125, 125, 125 ); }
		}

		public override Color ImageMarginRevealedGradientBegin {
			get { return Color.FromArgb( 255, 125, 125, 125 ); }
		}

		public override Color ImageMarginRevealedGradientMiddle {
			get { return Color.FromArgb( 255, 125, 125, 125 ); }
		}

		public override Color ImageMarginRevealedGradientEnd {
			get { return Color.FromArgb( 255, 125, 125, 125 ); }
		}

		public override Color MenuStripGradientBegin {
			get { return Color.FromArgb( 255, 138, 138, 138 ); }
		}

		public override Color MenuStripGradientEnd {
			get { return Color.FromArgb( 255, 138, 138, 138 ); }
		}

		public override Color MenuItemSelected {
			get { return Color.FromArgb( 255, 170, 170, 170 ); }
		}

		public override Color MenuItemBorder {
			get { return Color.FromArgb( 255, 170, 170, 170 ); }
		}

		public override Color MenuBorder {
			get { return Color.FromArgb( 255, 22, 22, 22 ); }
		}

		public override Color MenuItemSelectedGradientBegin {
			get { return Color.FromArgb( 255, 170, 170, 170 ); }
		}

		public override Color MenuItemSelectedGradientEnd {
			get { return Color.FromArgb( 255, 170, 170, 170 ); }
		}

		public override Color MenuItemPressedGradientBegin {
			get { return Color.FromArgb( 255, 125, 125, 125 ); }
		}

		public override Color MenuItemPressedGradientMiddle {
			get { return Color.FromArgb( 255, 125, 125, 125 ); }
		}

		public override Color MenuItemPressedGradientEnd {
			get { return Color.FromArgb( 255, 125, 125, 125 ); }
		}

		public override Color RaftingContainerGradientBegin {
			get { return Color.FromArgb( 255, 170, 170, 170 ); }
		}

		public override Color RaftingContainerGradientEnd {
			get { return Color.FromArgb( 255, 170, 170, 170 ); }
		}

		public override Color SeparatorDark {
			get { return Color.FromArgb( 255, 22, 22, 22 ); }
		}

		public override Color SeparatorLight {
			get { return Color.FromArgb( 255, 62, 62, 62 ); }
		}

		public override Color StatusStripGradientBegin {
			get { return Color.FromArgb( 255, 37, 37, 38 ); }
		}

		public override Color StatusStripGradientEnd {
			get { return Color.FromArgb( 255, 37, 37, 38 ); }
		}

		public override Color ToolStripBorder {
			get { return Color.FromArgb( 255, 22, 22, 22 ); }
		}

		public override Color ToolStripDropDownBackground {
			get { return Color.FromArgb( 255, 125, 125, 125 ); }
		}

		public override Color ToolStripGradientBegin {
			get { return Color.FromName( "DimGray" ); }
		}

		public override Color ToolStripGradientMiddle {
			get { return Color.FromArgb( 255, 89, 89, 89 ); }
		}

		public override Color ToolStripGradientEnd {
			get { return Color.FromArgb( 255, 88, 88, 88 ); }
		}

		public override Color ToolStripContentPanelGradientBegin {
			get { return Color.FromArgb( 255, 68, 68, 68 ); }
		}

		public override Color ToolStripContentPanelGradientEnd {
			get { return Color.FromArgb( 255, 68, 68, 68 ); }
		}

		public override Color ToolStripPanelGradientBegin {
			get { return Color.FromArgb( 255, 103, 103, 103 ); }
		}

		public override Color ToolStripPanelGradientEnd {
			get { return Color.FromArgb( 255, 103, 103, 103 ); }
		}

		public override Color OverflowButtonGradientBegin {
			get { return Color.FromArgb( 255, 103, 103, 103 ); }
		}

		public override Color OverflowButtonGradientMiddle {
			get { return Color.FromArgb( 255, 103, 103, 103 ); }
		}

		public override Color OverflowButtonGradientEnd {
			get { return Color.FromArgb( 255, 79, 79, 79 ); }
		}
	}
}