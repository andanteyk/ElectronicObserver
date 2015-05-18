/*
Copyright (c) 2011-2014, Yaroslav Bugaria
All rights reserved.

Redistribution and use in source and binary forms, with or without 
modification, are permitted provided that the following conditions are met:
  * Redistributions of source code must retain the above copyright notice, 
    this list of conditions and the following disclaimer.
  * Redistributions in binary form must reproduce the above copyright notice, 
    this list of conditions and the following disclaimer in the documentation 
    and/or other materials provided with the distribution.
  * Neither the name of Yaroslav Bugaria nor the names of its contributors may 
    be used to endorse or promote products derived from this software without 
    specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE 
OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED 
OF THE POSSIBILITY OF SUCH DAMAGE.
 */
using System.Drawing;
using System.Windows.Forms;

namespace ToolStripCustomizer.ColorTables
{
    public sealed class VS2012LightColorTable : PresetColorTable
    {
        public VS2012LightColorTable()
            : base("Visual Studio 2012 Light")
        {
        }

        public override Color ButtonSelectedHighlight
        {
            get { return ButtonSelectedGradientMiddle; }
        }
        public override Color ButtonSelectedHighlightBorder
        {
            get { return ButtonSelectedBorder; }
        }
        public override Color ButtonPressedHighlight
        {
            get { return ButtonPressedGradientMiddle; }
        }
        public override Color ButtonPressedHighlightBorder
        {
            get { return ButtonPressedBorder; }
        }
        public override Color ButtonCheckedHighlight
        {
            get { return ButtonCheckedGradientMiddle; }
        }
        public override Color ButtonCheckedHighlightBorder
        {
            get { return ButtonSelectedBorder; }
        }
        public override Color ButtonPressedBorder
        {
            get { return ButtonSelectedBorder; }
        }
        public override Color ButtonSelectedBorder
        { 
            get { return Color.FromArgb(255, 239, 239, 242); }
        }
        public override Color ButtonCheckedGradientBegin
        { 
            get { return Color.FromArgb(255, 254, 254, 254); }
        }
        public override Color ButtonCheckedGradientMiddle
        { 
            get { return Color.FromArgb(255, 254, 254, 254); }
        }
        public override Color ButtonCheckedGradientEnd
        { 
            get { return Color.FromArgb(255, 254, 254, 254); }
        }
        public override Color ButtonSelectedGradientBegin
        { 
            get { return Color.FromArgb(255, 254, 254, 254); }
        }
        public override Color ButtonSelectedGradientMiddle
        { 
            get { return Color.FromArgb(255, 254, 254, 254); }
        }
        public override Color ButtonSelectedGradientEnd
        { 
            get { return Color.FromArgb(255, 254, 254, 254); }
        }
        public override Color ButtonPressedGradientBegin
        { 
            get { return Color.FromArgb(255, 32, 172, 232); }
        }
        public override Color ButtonPressedGradientMiddle
        { 
            get { return Color.FromArgb(255, 32, 172, 232); }
        }
        public override Color ButtonPressedGradientEnd
        { 
            get { return Color.FromArgb(255, 32, 172, 232); }
        }
        public override Color CheckBackground
        { 
            get { return Color.FromArgb(255, 254, 254, 254); }
        }
        public override Color CheckSelectedBackground
        { 
            get { return Color.FromArgb(255, 254, 254, 254); }
        }
        public override Color CheckPressedBackground
        { 
            get { return Color.FromArgb(255, 32, 172, 232); }
        }
        public override Color GripDark
        { 
            get { return Color.FromArgb(255, 221, 226, 236); }
        }
        public override Color GripLight
        { 
            get { return Color.FromArgb(255, 204, 204, 219); }
        }
        public override Color ImageMarginGradientBegin
        { 
            get { return Color.FromArgb(255, 231, 232, 236); }
        }
        public override Color ImageMarginGradientMiddle
        { 
            get { return Color.FromArgb(255, 231, 232, 236); }
        }
        public override Color ImageMarginGradientEnd
        { 
            get { return Color.FromArgb(255, 231, 232, 236); }
        }
        public override Color ImageMarginRevealedGradientBegin
        { 
            get { return Color.FromArgb(255, 231, 232, 236); }
        }
        public override Color ImageMarginRevealedGradientMiddle
        { 
            get { return Color.FromArgb(255, 231, 232, 236); }
        }
        public override Color ImageMarginRevealedGradientEnd
        { 
            get { return Color.FromArgb(255, 231, 232, 236); }
        }
        public override Color MenuStripGradientBegin
        { 
            get { return Color.FromArgb(255, 239, 239, 242); }
        }
        public override Color MenuStripGradientEnd
        { 
            get { return Color.FromArgb(255, 239, 239, 242); }
        }
        public override Color MenuItemSelected
        { 
            get { return Color.FromArgb(255, 248, 249, 250); }
        }
        public override Color MenuItemBorder
        { 
            get { return Color.FromArgb(255, 231, 232, 236); }
        }
        public override Color MenuBorder
        { 
            get { return Color.FromArgb(255, 204, 206, 219); }
        }
        public override Color MenuItemSelectedGradientBegin
        { 
            get { return Color.FromArgb(255, 254, 254, 254); }
        }
        public override Color MenuItemSelectedGradientEnd
        { 
            get { return Color.FromArgb(255, 254, 254, 254); }
        }
        public override Color MenuItemPressedGradientBegin
        { 
            get { return Color.FromArgb(255, 231, 232, 236); }
        }
        public override Color MenuItemPressedGradientMiddle
        { 
            get { return Color.FromArgb(255, 231, 232, 236); }
        }
        public override Color MenuItemPressedGradientEnd
        { 
            get { return Color.FromArgb(255, 231, 232, 236); }
        }
        public override Color RaftingContainerGradientBegin
        { 
            get { return Color.FromArgb(255, 186, 192, 201); }
        }
        public override Color RaftingContainerGradientEnd
        { 
            get { return Color.FromArgb(255, 186, 192, 201); }
        }
        public override Color SeparatorDark
        { 
            get { return Color.FromArgb(255, 204, 206, 219); }
        }
        public override Color SeparatorLight
        { 
            get { return Color.FromArgb(255, 246, 246, 246); }
        }
        public override Color StatusStripGradientBegin
        { 
            get { return Color.FromArgb(255, 239, 239, 242); }
        }
        public override Color StatusStripGradientEnd
        {
			get { return Color.FromArgb(255, 239, 239, 242); }
        }
        public override Color ToolStripBorder
        { 
            get { return Color.FromArgb(0, 0, 0, 0); }
        }
        public override Color ToolStripDropDownBackground
        { 
            get { return Color.FromArgb(255, 231, 232, 236); }
        }
        public override Color ToolStripGradientBegin
        { 
            get { return Color.FromArgb(255, 239, 239, 242); }
        }
        public override Color ToolStripGradientMiddle
        { 
            get { return Color.FromArgb(255, 239, 239, 242); }
        }
        public override Color ToolStripGradientEnd
        { 
            get { return Color.FromArgb(255, 239, 239, 242); }
        }
        public override Color ToolStripContentPanelGradientBegin
        { 
            get { return Color.FromArgb(255, 239, 239, 242); }
        }
        public override Color ToolStripContentPanelGradientEnd
        { 
            get { return Color.FromArgb(255, 239, 239, 242); }
        }
        public override Color ToolStripPanelGradientBegin
        { 
            get { return Color.FromArgb(255, 239, 239, 242); }
        }
        public override Color ToolStripPanelGradientEnd
        { 
            get { return Color.FromArgb(255, 239, 239, 242); }
        }
        public override Color OverflowButtonGradientBegin
        { 
            get { return Color.FromArgb(255, 239, 239, 242); }
        }
        public override Color OverflowButtonGradientMiddle
        { 
            get { return Color.FromArgb(255, 239, 239, 242); }
        }
        public override Color OverflowButtonGradientEnd
        { 
            get { return Color.FromArgb(255, 239, 239, 242); }
        }
    }
}