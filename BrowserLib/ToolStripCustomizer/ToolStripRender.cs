using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ToolStripCustomizer.ColorTables;

namespace ToolStripCustomizer {
	public static class ToolStripRender {

		static ToolStripProfessionalRenderer _render;

		public static Font Window_Font;

		public static ToolStripProfessionalRenderer Render {
			get {
				if ( _render == null ) {
					if ( RendererTheme == ToolStripRenderTheme.Dark ) {
						_render = new ToolStripProfessionalRenderer( new VS2013DarkColorTable() );
					} else {
						_render = new ToolStripProfessionalRenderer( new VS2012LightColorTable() );
					}
				}
				return _render;
			}
		}

		public static ToolStripRenderTheme RendererTheme { get; set; }

		public static void SetRender( ToolStrip toolStrip ) {
			toolStrip.Renderer = Render;
			toolStrip.Font = Window_Font;
		}
	}

	public enum ToolStripRenderTheme {
		Light,
		Dark
	}
}
