using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicObserver.Window.Dialog
{
	public partial class DialogShipGraphicViewer : Form
	{

		List<string> ImagePathList;
		int CurrentIndex;
		Bitmap CurrentImage;

		Point ImageOffset = new Point();
		Point PreviousMouseLocation = new Point();
		double ZoomRate = 1;
		InterpolationMode Interpolation = InterpolationMode.NearestNeighbor;
		bool AdvMode = false;
		bool RestrictedPatch = false;
		bool DrawsInformation = true;


		private DialogShipGraphicViewer()
		{
			InitializeComponent();
			ImagePathList = new List<string>();

			MouseWheel += DialogShipGraphicViewer_MouseWheel;

			SetStyle(ControlStyles.ResizeRedraw, true);
			ControlHelper.SetDoubleBuffered(DrawingPanel);

			OpenSwfDialog.InitialDirectory = Utility.Configuration.Config.Connection.SaveDataPath + @"kcs2\resources\ship";


			// 背景画像生成
			var back = new Bitmap(16, 16, PixelFormat.Format24bppRgb);
			using (var g = Graphics.FromImage(back))
			{
				g.Clear(Color.White);
				g.FillRectangle(Brushes.LightGray, new Rectangle(0, 0, 8, 8));
				g.FillRectangle(Brushes.LightGray, new Rectangle(8, 8, 8, 8));
			}
			DrawingPanel.BackgroundImage = back;

		}

		public DialogShipGraphicViewer(string path)
			: this()
		{

			Open(path);
		}

		public DialogShipGraphicViewer(string[] pathlist)
			: this()
		{
			Open(pathlist);
		}

		public DialogShipGraphicViewer(int shipID)
			: this()
		{
			Open(shipID);
		}



		private void DialogShipGraphicViewer_Load(object sender, EventArgs e)
		{

			this.Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormAlbumShip]);

			RefreshImage();
		}


		private void DialogShipGraphicViewer_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (DrawingPanel.BackgroundImage != null)
				DrawingPanel.BackgroundImage.Dispose();
			ResourceManager.DestroyIcon(Icon);
		}



		#region Input Controls

		private void DialogShipGraphicViewer_KeyDown(object sender, KeyEventArgs e)
		{

			if (ImagePathList.Count == 0)
				return;

			bool fileChanged = false;

			if (e.KeyCode == Keys.Left)
			{
				CurrentIndex--;
				fileChanged = true;
			}
			else if (e.KeyCode == Keys.Right)
			{
				CurrentIndex++;
				fileChanged = true;

			}
			else if (e.KeyCode == Keys.Up)
			{
				ZoomRate += 0.1;
			}
			else if (e.KeyCode == Keys.Down)
			{
				ZoomRate -= 0.1;

			}
			else if (e.KeyCode == Keys.Oemtilde)
			{   // "@"
				AdvMode = !AdvMode;
				RestrictedPatch = AdvMode && e.Shift;

			}
			else return;


			if (fileChanged)
			{
				CurrentIndex %= ImagePathList.Count;
				if (CurrentIndex < 0)
					CurrentIndex += ImagePathList.Count;

				try
				{
					CurrentImage?.Dispose();
					CurrentImage = null;
					using (var stream = new FileStream(ImagePathList[CurrentIndex], FileMode.Open, FileAccess.Read))
						CurrentImage = new Bitmap(stream);
				}
				catch (Exception ex)
				{
					Utility.Logger.Add(3, $"画像ビューア：画像ロード時にエラーが発生しました。{ex.Message}");
				}
				ImageOffset = new Point();
				ZoomRate = 1;
			}

			ValidateParameters();
			RefreshImage();
		}

		void DialogShipGraphicViewer_MouseWheel(object sender, MouseEventArgs e)
		{
			double wheel_delta = 120;
			ZoomRate += (e.Delta / wheel_delta) * 0.1;
			ValidateParameters();
			RefreshImage();
		}

		private void DrawingPanel_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				ImageOffset.X += e.Location.X - PreviousMouseLocation.X;
				ImageOffset.Y += e.Location.Y - PreviousMouseLocation.Y;
				RefreshImage();
			}
			PreviousMouseLocation = e.Location;
		}

		private void DrawingPanel_MouseClick(object sender, MouseEventArgs e)
		{
			PreviousMouseLocation = e.Location;
		}


		#endregion


		#region View Control


		private void TopMenu_View_InterpolationMode_Sharp_Click(object sender, EventArgs e)
		{

			foreach (ToolStripMenuItem item in TopMenu_View_InterpolationMode.DropDownItems)
			{
				if (object.ReferenceEquals(item, sender))
					item.CheckState = CheckState.Indeterminate;
				else
					item.CheckState = CheckState.Unchecked;
			}

			if (TopMenu_View_InterpolationMode_Sharp.Checked)
				Interpolation = InterpolationMode.NearestNeighbor;
			else if (TopMenu_View_InterpolationMode_Smooth.Checked)
				Interpolation = InterpolationMode.HighQualityBicubic;

			RefreshImage();
		}


		private void TopMenu_View_Zoom_In_Click(object sender, EventArgs e)
		{
			ZoomRate += 0.1;
			ValidateParameters();
		}

		private void TopMenu_View_Zoom_Out_Click(object sender, EventArgs e)
		{
			ZoomRate -= 0.1;
			ValidateParameters();
		}

		private void TopMenu_View_Zoom_100_Click(object sender, EventArgs e)
		{
			ZoomRate = 1;
			RefreshImage();
		}

		private void TopMenu_View_Zoom_Fit_Click(object sender, EventArgs e)
		{
			RefreshImage();
		}

		private void ValidateParameters()
		{
			ZoomRate = Math.Min(Math.Max(ZoomRate, 0.1), 16);
		}

		#endregion


		#region File Control

		private void Open(string path)
		{
			Open(new string[] { path });
		}

		private void Open(string[] pathlist)
		{
			try
			{
				ImagePathList = pathlist.Where(p => File.Exists(p)).ToList();

				if (ImagePathList.Count == 0)
				{
					MessageBox.Show("画像が見つかりませんでした。", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}

				CurrentIndex = 0;
				CurrentImage?.Dispose();
				CurrentImage = null;

				using (var stream = new FileStream(ImagePathList[CurrentIndex], FileMode.Open, FileAccess.Read))
					CurrentImage = new Bitmap(stream);

			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Join("\r\n", pathlist) + "を開けませんでした。\r\n" + ex.GetType().Name + "\r\n" + ex.Message);
				ImagePathList.Clear();
				CurrentImage?.Dispose();
				CurrentImage = null;
			}
			finally
			{
				RefreshImage();
			}

		}

		private void Open(int shipID)
		{
			var ship = KCDatabase.Instance.MasterShips[shipID];

			if (ship == null)
				return;

			var list = new LinkedList<string>();

			void Add(int id, bool isDamaged, string resourceType)
			{
				if (resourceType == KCResourceHelper.ResourceTypeShipName && isDamaged)
					return;

				var path = KCResourceHelper.GetShipImagePath(id, isDamaged, resourceType);
				if (path != null)
					list.AddLast(path);
			}
			void AddShip(int id)
			{
				Add(id, false, KCResourceHelper.ResourceTypeShipBanner);
				Add(id, true, KCResourceHelper.ResourceTypeShipBanner);
				Add(id, false, KCResourceHelper.ResourceTypeShipCard);
				Add(id, true, KCResourceHelper.ResourceTypeShipCard);
				Add(id, false, KCResourceHelper.ResourceTypeShipAlbumZoom);
				Add(id, true, KCResourceHelper.ResourceTypeShipAlbumZoom);
				Add(id, false, KCResourceHelper.ResourceTypeShipAlbumFull);
				Add(id, true, KCResourceHelper.ResourceTypeShipAlbumFull);
				Add(id, false, KCResourceHelper.ResourceTypeShipFull);
				Add(id, true, KCResourceHelper.ResourceTypeShipFull);
				Add(id, false, KCResourceHelper.ResourceTypeShipCutin);
				Add(id, true, KCResourceHelper.ResourceTypeShipCutin);
				Add(id, false, KCResourceHelper.ResourceTypeShipName);
				Add(id, false, KCResourceHelper.ResourceTypeShipSupply);
				Add(id, true, KCResourceHelper.ResourceTypeShipSupply);
			}

			AddShip(shipID);
			foreach (var rec in RecordManager.Instance.ShipParameter.Record.Values.Where(r => r.OriginalCostumeShipID == shipID))
				AddShip(rec.ShipID);

			Open(list.ToArray());
		}

		private void TopMenu_File_Open_Click(object sender, EventArgs e)
		{
			if (OpenSwfDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Open(OpenSwfDialog.FileNames);
			}
		}



		private void TopMenu_File_CopyToClipboard_Click(object sender, EventArgs e)
		{
			if (CurrentImage == null)
			{
				System.Media.SystemSounds.Exclamation.Play();
				return;
			}

			try
			{
				Clipboard.SetImage(CurrentImage);
			}
			catch (Exception)
			{
				System.Media.SystemSounds.Exclamation.Play();
			}
		}


		private void DialogShipGraphicViewer_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;
		}

		private void DialogShipGraphicViewer_DragDrop(object sender, DragEventArgs e)
		{
			Open((string[])e.Data.GetData(DataFormats.FileDrop));
		}

		#endregion


		#region Drawing

		private void DrawingPanel_Paint(object sender, PaintEventArgs e)
		{
			if (CurrentImage == null)
				return;

			e.Graphics.InterpolationMode = Interpolation;
			e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			var panelSize = DrawingPanel.Size;

			double zoomRate = ZoomRate;
			if (TopMenu_View_Zoom_Fit.Checked)
			{
				zoomRate = Math.Min((double)panelSize.Width / CurrentImage.Width, (double)panelSize.Height / CurrentImage.Height);
			}

			var imgSize = new SizeF((float)(CurrentImage.Width * zoomRate), (float)(CurrentImage.Height * zoomRate));


			if (DrawsInformation)
			{
				var ship = GetShipFromPath(ImagePathList[CurrentIndex]);

				e.Graphics.DrawString(
					string.Format("{0} / {1}\r\n{2} ({3})\r\nZoom {4:p1}\r\n(←/→キーでページめくり)",
						CurrentIndex + 1, ImagePathList.Count, Path.GetFileName(ImagePathList[CurrentIndex]), ship?.NameWithClass ?? "???", zoomRate),
					Font, Brushes.DimGray, new PointF(0, 0));
			}

			var location = new PointF((panelSize.Width - imgSize.Width) / 2 + ImageOffset.X, (panelSize.Height - imgSize.Height) / 2 + ImageOffset.Y);
			e.Graphics.DrawImage(CurrentImage, new RectangleF(location.X, location.Y, imgSize.Width, imgSize.Height));


			/*// face recognition test
			{
				var ship = GetShipFromPath(ImagePathList[CurrentIndex]);
				if (ship != null)
				{
					var rect = ship.GraphicData.WeddingArea;
					e.Graphics.DrawRectangle(Pens.Magenta,
						(float)(panelSize.Width / 2.0 + ((double)rect.X / CurrentImage.Width - 0.5) * CurrentImage.Width * zoomRate + ImageOffset.X),
						(float)(panelSize.Height / 2.0 + ((double)rect.Y / CurrentImage.Height - 0.5) * CurrentImage.Height * zoomRate + ImageOffset.Y),
						(float)(rect.Width * zoomRate),
						(float)(rect.Height * zoomRate));
				}
			}
			//*/

			if (AdvMode)
			{
				DrawAdvMode(e.Graphics);
			}

		}

		// かん☆これ!!
		private void DrawAdvMode(Graphics g)
		{

			var ship = GetShipFromPath(ImagePathList[CurrentIndex]);
			if (ship == null)
				return;

			var panelSize = DrawingPanel.Size;

			int mainHeight = 200;
			int nameHeight = 64;
			var mainBound = new Rectangle(16, panelSize.Height - mainHeight - 16, panelSize.Width - 32, mainHeight);
			var innerBound = new Rectangle(mainBound.X + 16, mainBound.Y + 16, mainBound.Width - 32, mainBound.Height - 32);
			var nameBound = new Rectangle(16, mainBound.Y - nameHeight - 16, 256, nameHeight);
			var nameInnerBound = new Rectangle(nameBound.X + 16, nameBound.Y + 16, nameBound.Width - 32, nameBound.Height - 32);

			var rand = new Random(401168);      // for fixed drawing



			// window shadow
			using (var pen = new Pen(Color.RoyalBlue, 8))
			{
				pen.LineJoin = LineJoin.Round;
				g.DrawRectangle(pen, new Rectangle(mainBound.X + 2, mainBound.Y + 2, mainBound.Width, mainBound.Height));
				g.DrawRectangle(pen, new Rectangle(nameBound.X + 2, nameBound.Y + 2, nameBound.Width, nameBound.Height));
			}

			// window base
			using (var grad = new LinearGradientBrush(mainBound, Color.FromArgb(0xcc, 0x44, 0xcc, 0xff), Color.FromArgb(0xcc, 0x44, 0x44, 0xff), 90))
			{
				g.FillRectangle(grad, mainBound);
			}
			using (var grad = new LinearGradientBrush(nameBound, Color.FromArgb(0xcc, 0x44, 0xcc, 0xff), Color.FromArgb(0xcc, 0x44, 0x44, 0xff), 90))
			{
				g.FillRectangle(grad, nameBound);
			}


			// decorations
			g.SetClip(mainBound);

			using (var pen = new Pen(Color.FromArgb(0x44, 0xcc, 0xee, 0xff), 4))
			{
				Point center = new Point(mainBound.X + mainHeight * 3 / 4, mainBound.Y + mainHeight * 3 / 4);

				// compass
				g.DrawEllipse(pen, new Rectangle(center.X - 150, center.Y - 150, 300, 300));
				g.DrawEllipse(pen, new Rectangle(center.X - 140, center.Y - 140, 280, 280));
				g.DrawEllipse(pen, new Rectangle(center.X - 16, center.Y - 16, 32, 32));

				int direct = (int)(120 * Math.Sin(Math.PI / 4));

				var path = new GraphicsPath();
				path.AddLines(new Point[] {
						new Point( center.X + direct, center.Y - direct ),
						new Point( center.X + 16, center.Y + 16 ),
						new Point( center.X - direct, center.Y + direct ),
						new Point( center.X - 16, center.Y - 16 )
					});
				path.CloseFigure();
				g.DrawPath(pen, path);


				// bubbles
				for (int i = 0; i < 16; i++)
				{
					int r = (int)(8 + rand.NextDouble() * 24 * i / 8);
					g.DrawEllipse(pen, new Rectangle(
						mainBound.Right - 96 + (int)((rand.NextDouble() * 128 - 64) * (i + 1) / 16),
						mainBound.Bottom - 16 - (i * mainBound.Height) / 16,
						r, r
						));
				}
			}

			g.ResetClip();


			// window frame
			using (var pen = new Pen(Color.White, 8))
			{
				pen.LineJoin = LineJoin.Round;
				g.DrawRectangle(pen, mainBound);
				g.DrawRectangle(pen, nameBound);
			}


			// text
			// don't think (about env that font is not installed), feel
			using (var font = new Font("にゃしぃフォント改二", 32, FontStyle.Regular, GraphicsUnit.Pixel))
			{
				var rec = Resource.Record.RecordManager.Instance.ShipParameter[ship.ShipID];
				string mes = null;

				if (rec == null)
				{
					mes = "……";

				}
				else
				{
					var processedShips = new LinkedList<ShipDataMaster>();
					processedShips.AddLast(ship);

					while (processedShips.Last.Value.RemodelBeforeShip != null && !processedShips.Any(s2 => processedShips.Last.Value.RemodelBeforeShipID == s2.ShipID))
					{
						processedShips.AddLast(processedShips.Last.Value.RemodelBeforeShip);
					}

					foreach (var s in processedShips)
					{
						string mescan = s.MessageGet.Replace("<br>", "\r\n");
						if (!string.IsNullOrWhiteSpace(mescan))
						{
							mes = mescan;
							break;
						}
					}

					if (mes == null)
					{
						foreach (var s in processedShips)
						{
							string mescan = s.MessageAlbum.Replace("<br>", "\r\n");
							if (!string.IsNullOrWhiteSpace(mescan))
							{
								mes = mescan;
								break;
							}
						}
					}

					if (mes == null)
						mes = "……";
				}

				if (RestrictedPatch)
				{
					mes = mes
						.Replace("。", "♥")
						.Replace(".\r\n", "♥\r\n")
						.Replace("……", "…")
						.Replace("…", "…♥")
						.Replace("！", "！♥")
						.Replace("？", "？♥")
						.Replace("!", "!♥")
						.Replace("?", "?♥")
						;
				}

				g.DrawString(mes, font, Brushes.RoyalBlue, new Rectangle(innerBound.Location + new Size(2, 2), innerBound.Size));
				g.DrawString(mes, font, Brushes.White, innerBound);

				g.DrawString(ship.NameWithClass, font, Brushes.RoyalBlue, new Rectangle(nameInnerBound.Location + new Size(2, 2), nameInnerBound.Size));
				g.DrawString(ship.NameWithClass, font, Brushes.White, nameInnerBound);
			}
		}

		private void DrawingPanel_Resize(object sender, EventArgs e)
		{
			RefreshImage();
		}


		private void RefreshImage()
		{
			DrawingPanel.Refresh();
		}

		#endregion



		private ShipDataMaster GetShipFromPath(string path)
		{
			path = Path.GetFileNameWithoutExtension(path);

			if (path.Length >= 4 && int.TryParse(path.Substring(0, 4), out var id))
			{
				return KCDatabase.Instance.MasterShips[id];
			}

			return null;
		}


	}
}
