using ElectronicObserver.Data;
using ElectronicObserver.Resource.Record;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ElectronicObserver.Window.Dialog {

	public partial class DialogResourceChart : Form {


		private enum ChartType {
			Resource,
			ResourceDiff,
			Material,
			MaterialDiff,
			Experience,
			ExperienceDiff
		}

		private enum ChartSpan {
			Day,
			Week,
			Month,
			Season,
			Year,
			All
		}



		private ChartType SelectedChartType {
			get { return (ChartType)GetSelectedMenuStripIndex( Menu_Graph ); }
		}

		private ChartSpan SelectedChartSpan {
			get { return (ChartSpan)GetSelectedMenuStripIndex( Menu_Span ); }
		}




		public DialogResourceChart() {
			InitializeComponent();
		}



		private void DialogResourceChart_Load( object sender, EventArgs e ) {

			if ( !RecordManager.Instance.Resource.Record.Any() ) {
				MessageBox.Show( "レコード データが存在しません。\n一度母港に移動してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				Close();
				return;
			}


			SwitchMenuStrip( Menu_Graph, 0 );
			SwitchMenuStrip( Menu_Span, 2 );

			UpdateChart();
		}



		private void SetResourceChart() {

			ResourceChart.ChartAreas.Clear();
			var area = ResourceChart.ChartAreas.Add( "ResourceChartArea" );
			area.AxisX = CreateAxisX( SelectedChartSpan );
			area.AxisY = CreateAxisY( 2000 );

			ResourceChart.Legends.Clear();
			var legend = ResourceChart.Legends.Add( "ResourceLegend" );
			legend.Font = Font;


			ResourceChart.Series.Clear();

			var fuel = ResourceChart.Series.Add( "ResourceSeries_Fuel" );
			var ammo = ResourceChart.Series.Add( "ResourceSeries_Ammo" );
			var steel = ResourceChart.Series.Add( "ResourceSeries_Steel" );
			var bauxite = ResourceChart.Series.Add( "ResourceSeries_Bauxite" );

			var setSeries = new Action<Series>( s => {
				s.ChartType = SeriesChartType.Line;
				s.Font = Font;
				s.XValueType = ChartValueType.DateTime;
			} );

			setSeries( fuel );
			fuel.Color = Color.FromArgb( 0, 128, 0 );
			fuel.LegendText = "燃料";

			setSeries( ammo );
			ammo.Color = Color.FromArgb( 255, 128, 0 );
			ammo.LegendText = "弾薬";

			setSeries( steel );
			steel.Color = Color.FromArgb( 64, 64, 64 );
			steel.LegendText = "鋼材";

			setSeries( bauxite );
			bauxite.Color = Color.FromArgb( 255, 0, 0 );
			bauxite.LegendText = "ボーキ";


			//データ設定
			{
				var record = GetRecords();

				if ( record.Any() ) {
					var prev = record.First();
					foreach ( var r in record ) {

						if ( ShouldSkipRecord( r.Date - prev.Date ) )
							continue;

						fuel.Points.AddXY( r.Date.ToOADate(), r.Fuel );
						ammo.Points.AddXY( r.Date.ToOADate(), r.Ammo );
						steel.Points.AddXY( r.Date.ToOADate(), r.Steel );
						bauxite.Points.AddXY( r.Date.ToOADate(), r.Bauxite );

						prev = r;
					}
				}

				if ( KCDatabase.Instance.Material.IsAvailable ) {
					double now = DateTime.Now.ToOADate();
					fuel.Points.AddXY( now, KCDatabase.Instance.Material.Fuel );
					ammo.Points.AddXY( now, KCDatabase.Instance.Material.Ammo );
					steel.Points.AddXY( now, KCDatabase.Instance.Material.Steel );
					bauxite.Points.AddXY( now, KCDatabase.Instance.Material.Bauxite );
				}

				if ( fuel.Points.Count > 0 ) {
					int min = (int)new[] { fuel.Points.Min( p => p.YValues[0] ), ammo.Points.Min( p => p.YValues[0] ), steel.Points.Min( p => p.YValues[0] ), bauxite.Points.Min( p => p.YValues[0] ) }.Min();
					area.AxisY.Minimum = Math.Floor( min / 10000.0 ) * 10000;

					int max = (int)new[] { fuel.Points.Max( p => p.YValues[0] ), ammo.Points.Max( p => p.YValues[0] ), steel.Points.Max( p => p.YValues[0] ), bauxite.Points.Max( p => p.YValues[0] ) }.Max();
					area.AxisY.Maximum = Math.Ceiling( max / 10000.0 ) * 10000;
				}
			}

		}


		private void SetResourceDiffChart() {

			ResourceChart.ChartAreas.Clear();
			var area = ResourceChart.ChartAreas.Add( "ResourceChartArea" );
			area.AxisX = CreateAxisX( SelectedChartSpan );
			area.AxisY = CreateAxisY( 200 );

			ResourceChart.Legends.Clear();
			var legend = ResourceChart.Legends.Add( "ResourceLegend" );
			legend.Font = Font;


			ResourceChart.Series.Clear();

			var fuel = ResourceChart.Series.Add( "ResourceSeries_Fuel" );
			var ammo = ResourceChart.Series.Add( "ResourceSeries_Ammo" );
			var steel = ResourceChart.Series.Add( "ResourceSeries_Steel" );
			var bauxite = ResourceChart.Series.Add( "ResourceSeries_Bauxite" );

			var setSeries = new Action<Series>( s => {
				s.ChartType = SeriesChartType.Area;
				//s.SetCustomProperty( "PointWidth", "1.0" );		//棒グラフの幅
				//s.Enabled = false;	//表示するか
				s.Font = Font;
				s.XValueType = ChartValueType.DateTime;
			} );

			setSeries( fuel );
			fuel.Color = Color.FromArgb( 64, 0, 128, 0 );
			fuel.LegendText = "燃料";

			setSeries( ammo );
			ammo.Color = Color.FromArgb( 64, 255, 128, 0 );
			ammo.LegendText = "弾薬";

			setSeries( steel );
			steel.Color = Color.FromArgb( 64, 64, 64, 64 );
			steel.LegendText = "鋼材";

			setSeries( bauxite );
			bauxite.Color = Color.FromArgb( 64, 255, 0, 0 );
			bauxite.LegendText = "ボーキ";


			//データ設定
			{
				var record = GetRecords();

				ResourceRecord.ResourceElement prev = null;

				if ( record.Any() ) {
					prev = record.First();
					foreach ( var r in record ) {

						if ( ShouldSkipRecord( r.Date - prev.Date ) )
							continue;

						fuel.Points.AddXY( r.Date.ToOADate(), r.Fuel - prev.Fuel );
						ammo.Points.AddXY( r.Date.ToOADate(), r.Ammo - prev.Ammo );
						steel.Points.AddXY( r.Date.ToOADate(), r.Steel - prev.Steel );
						bauxite.Points.AddXY( r.Date.ToOADate(), r.Bauxite - prev.Bauxite );

						prev = r;
					}
				}

				if ( KCDatabase.Instance.Material.IsAvailable ) {
					double now = DateTime.Now.ToOADate();
					fuel.Points.AddXY( now, prev == null ? 0 : KCDatabase.Instance.Material.Fuel - prev.Fuel );
					ammo.Points.AddXY( now, prev == null ? 0 : KCDatabase.Instance.Material.Ammo - prev.Ammo );
					steel.Points.AddXY( now, prev == null ? 0 : KCDatabase.Instance.Material.Steel - prev.Steel );
					bauxite.Points.AddXY( now, prev == null ? 0 : KCDatabase.Instance.Material.Bauxite - prev.Bauxite );
				}

				if ( fuel.Points.Count > 0 ) {
					int min = (int)new[] { fuel.Points.Min( p => p.YValues[0] ), ammo.Points.Min( p => p.YValues[0] ), steel.Points.Min( p => p.YValues[0] ), bauxite.Points.Min( p => p.YValues[0] ) }.Min();
					area.AxisY.Minimum = Math.Floor( min / 1000.0 ) * 1000;

					int max = (int)new[] { fuel.Points.Max( p => p.YValues[0] ), ammo.Points.Max( p => p.YValues[0] ), steel.Points.Max( p => p.YValues[0] ), bauxite.Points.Max( p => p.YValues[0] ) }.Max();
					area.AxisY.Maximum = Math.Ceiling( max / 1000.0 ) * 1000;
				}
			}

		}



		private void SetMaterialChart() {

			ResourceChart.ChartAreas.Clear();
			var area = ResourceChart.ChartAreas.Add( "ResourceChartArea" );
			area.AxisX = CreateAxisX( SelectedChartSpan );
			area.AxisY = CreateAxisY( 50, 200 );

			ResourceChart.Legends.Clear();
			var legend = ResourceChart.Legends.Add( "ResourceLegend" );
			legend.Font = Font;


			ResourceChart.Series.Clear();

			var instantConstruction = ResourceChart.Series.Add( "ResourceSeries_InstantConstruction" );
			var instantRepair = ResourceChart.Series.Add( "ResourceSeries_InstantRepair" );
			var developmentMaterial = ResourceChart.Series.Add( "ResourceSeries_DevelopmentMaterial" );
			var moddingMaterial = ResourceChart.Series.Add( "ResourceSeries_ModdingMaterial" );

			var setSeries = new Action<Series>( s => {
				s.ChartType = SeriesChartType.Line;
				s.Font = Font;
				s.XValueType = ChartValueType.DateTime;
			} );

			setSeries( instantConstruction );
			instantConstruction.Color = Color.FromArgb( 255, 128, 0 );
			instantConstruction.LegendText = "高速建造材";

			setSeries( instantRepair );
			instantRepair.Color = Color.FromArgb( 0, 128, 0 );
			instantRepair.LegendText = "高速修復材";

			setSeries( developmentMaterial );
			developmentMaterial.Color = Color.FromArgb( 0, 0, 255 );
			developmentMaterial.LegendText = "開発資材";

			setSeries( moddingMaterial );
			moddingMaterial.Color = Color.FromArgb( 64, 64, 64 );
			moddingMaterial.LegendText = "改修資材";


			//データ設定
			{
				var record = GetRecords();

				if ( record.Any() ) {
					var prev = record.First();
					foreach ( var r in record ) {

						if ( ShouldSkipRecord( r.Date - prev.Date ) )
							continue;

						instantConstruction.Points.AddXY( r.Date.ToOADate(), r.InstantConstruction );
						instantRepair.Points.AddXY( r.Date.ToOADate(), r.InstantRepair );
						developmentMaterial.Points.AddXY( r.Date.ToOADate(), r.DevelopmentMaterial );
						moddingMaterial.Points.AddXY( r.Date.ToOADate(), r.ModdingMaterial );

						prev = r;
					}
				}

				if ( KCDatabase.Instance.Material.IsAvailable ) {
					double now = DateTime.Now.ToOADate();
					instantConstruction.Points.AddXY( now, KCDatabase.Instance.Material.InstantConstruction );
					instantRepair.Points.AddXY( now, KCDatabase.Instance.Material.InstantRepair );
					developmentMaterial.Points.AddXY( now, KCDatabase.Instance.Material.DevelopmentMaterial );
					moddingMaterial.Points.AddXY( now, KCDatabase.Instance.Material.ModdingMaterial );
				}

				if ( instantConstruction.Points.Count > 0 ) {
					int min = (int)new[] { instantConstruction.Points.Min( p => p.YValues[0] ), instantRepair.Points.Min( p => p.YValues[0] ), developmentMaterial.Points.Min( p => p.YValues[0] ), moddingMaterial.Points.Min( p => p.YValues[0] ) }.Min();
					area.AxisY.Minimum = Math.Floor( min / 200.0 ) * 200;

					int max = (int)new[] { instantConstruction.Points.Max( p => p.YValues[0] ), instantRepair.Points.Max( p => p.YValues[0] ), developmentMaterial.Points.Max( p => p.YValues[0] ), moddingMaterial.Points.Max( p => p.YValues[0] ) }.Max();
					area.AxisY.Maximum = Math.Ceiling( max / 200.0 ) * 200;
				}
			}

		}


		private void SetMateialDiffChart() {

			ResourceChart.ChartAreas.Clear();
			var area = ResourceChart.ChartAreas.Add( "ResourceChartArea" );
			area.AxisX = CreateAxisX( SelectedChartSpan );
			area.AxisY = CreateAxisY( 5, 20 );

			ResourceChart.Legends.Clear();
			var legend = ResourceChart.Legends.Add( "ResourceLegend" );
			legend.Font = Font;


			ResourceChart.Series.Clear();

			var instantConstruction = ResourceChart.Series.Add( "ResourceSeries_InstantConstruction" );
			var instantRepair = ResourceChart.Series.Add( "ResourceSeries_InstantRepair" );
			var developmentMaterial = ResourceChart.Series.Add( "ResourceSeries_DevelopmentMaterial" );
			var moddingMaterial = ResourceChart.Series.Add( "ResourceSeries_ModdingMaterial" );

			var setSeries = new Action<Series>( s => {
				s.ChartType = SeriesChartType.Area;
				//s.SetCustomProperty( "PointWidth", "1.0" );		//棒グラフの幅
				//s.Enabled = false;	//表示するか
				s.Font = Font;
				s.XValueType = ChartValueType.DateTime;
			} );

			setSeries( instantConstruction );
			instantConstruction.Color = Color.FromArgb( 64, 255, 128, 0 );
			instantConstruction.LegendText = "高速建造材";

			setSeries( instantRepair );
			instantRepair.Color = Color.FromArgb( 64, 0, 128, 0 );
			instantRepair.LegendText = "高速修復材";

			setSeries( developmentMaterial );
			developmentMaterial.Color = Color.FromArgb( 64, 0, 0, 255 );
			developmentMaterial.LegendText = "開発資材";

			setSeries( moddingMaterial );
			moddingMaterial.Color = Color.FromArgb( 64, 64, 64, 64 );
			moddingMaterial.LegendText = "改修資材";


			//データ設定
			{
				var record = GetRecords();

				ResourceRecord.ResourceElement prev = null;

				if ( record.Any() ) {
					prev = record.First();
					foreach ( var r in record ) {

						if ( ShouldSkipRecord( r.Date - prev.Date ) )
							continue;

						instantConstruction.Points.AddXY( r.Date.ToOADate(), r.InstantConstruction - prev.InstantConstruction );
						instantRepair.Points.AddXY( r.Date.ToOADate(), r.InstantRepair - prev.InstantRepair );
						developmentMaterial.Points.AddXY( r.Date.ToOADate(), r.DevelopmentMaterial - prev.DevelopmentMaterial );
						moddingMaterial.Points.AddXY( r.Date.ToOADate(), r.ModdingMaterial - prev.ModdingMaterial );

						prev = r;
					}
				}

				if ( KCDatabase.Instance.Material.IsAvailable ) {
					double now = DateTime.Now.ToOADate();
					instantConstruction.Points.AddXY( now, prev == null ? 0 : KCDatabase.Instance.Material.InstantConstruction - prev.InstantConstruction );
					instantRepair.Points.AddXY( now, prev == null ? 0 : KCDatabase.Instance.Material.InstantRepair - prev.InstantRepair );
					developmentMaterial.Points.AddXY( now, prev == null ? 0 : KCDatabase.Instance.Material.DevelopmentMaterial - prev.DevelopmentMaterial );
					moddingMaterial.Points.AddXY( now, prev == null ? 0 : KCDatabase.Instance.Material.ModdingMaterial - prev.ModdingMaterial );
				}

				if ( instantConstruction.Points.Count > 0 ) {
					int min = (int)new[] { instantConstruction.Points.Min( p => p.YValues[0] ), instantRepair.Points.Min( p => p.YValues[0] ), developmentMaterial.Points.Min( p => p.YValues[0] ), moddingMaterial.Points.Min( p => p.YValues[0] ) }.Min();
					area.AxisY.Minimum = Math.Floor( min / 20.0 ) * 20;

					int max = (int)new[] { instantConstruction.Points.Max( p => p.YValues[0] ), instantRepair.Points.Max( p => p.YValues[0] ), developmentMaterial.Points.Max( p => p.YValues[0] ), moddingMaterial.Points.Max( p => p.YValues[0] ) }.Max();
					area.AxisY.Maximum = Math.Ceiling( max / 20.0 ) * 20;
				}
			}

		}



		private void SetExperienceChart() {

			ResourceChart.ChartAreas.Clear();
			var area = ResourceChart.ChartAreas.Add( "ResourceChartArea" );
			area.AxisX = CreateAxisX( SelectedChartSpan );
			area.AxisY = CreateAxisY( 20000 );

			ResourceChart.Legends.Clear();
			var legend = ResourceChart.Legends.Add( "ResourceLegend" );
			legend.Font = Font;


			ResourceChart.Series.Clear();

			var exp = ResourceChart.Series.Add( "ResourceSeries_Experience" );

			var setSeries = new Action<Series>( s => {
				s.ChartType = SeriesChartType.Line;
				s.Font = Font;
				s.XValueType = ChartValueType.DateTime;
			} );

			setSeries( exp );
			exp.Color = Color.FromArgb( 0, 0, 255 );
			exp.LegendText = "提督経験値";


			//データ設定
			{
				var record = GetRecords();

				if ( record.Any() ) {
					var prev = record.First();
					foreach ( var r in record ) {

						if ( ShouldSkipRecord( r.Date - prev.Date ) )
							continue;

						exp.Points.AddXY( r.Date.ToOADate(), r.HQExp );
						prev = r;
					}
				}

				if ( KCDatabase.Instance.Admiral.IsAvailable ) {
					double now = DateTime.Now.ToOADate();
					exp.Points.AddXY( now, KCDatabase.Instance.Admiral.Exp );
				}

				if ( exp.Points.Count > 0 ) {
					int min = (int)exp.Points.Min( p => p.YValues[0] );
					area.AxisY.Minimum = Math.Floor( min / 100000.0 ) * 100000;

					int max = (int)exp.Points.Max( p => p.YValues[0] );
					area.AxisY.Maximum = Math.Ceiling( max / 100000.0 ) * 100000;
				}
			}

		}


		private void SetExperienceDiffChart() {

			ResourceChart.ChartAreas.Clear();
			var area = ResourceChart.ChartAreas.Add( "ResourceChartArea" );
			area.AxisX = CreateAxisX( SelectedChartSpan );
			area.AxisY = CreateAxisY( 2000 );

			ResourceChart.Legends.Clear();
			var legend = ResourceChart.Legends.Add( "ResourceLegend" );
			legend.Font = Font;


			ResourceChart.Series.Clear();

			var exp = ResourceChart.Series.Add( "ResourceSeries_Experience" );


			var setSeries = new Action<Series>( s => {
				s.ChartType = SeriesChartType.Area;
				//s.SetCustomProperty( "PointWidth", "1.0" );		//棒グラフの幅
				//s.Enabled = false;	//表示するか
				s.Font = Font;
				s.XValueType = ChartValueType.DateTime;
			} );

			setSeries( exp );
			exp.Color = Color.FromArgb( 192, 0, 0, 255 );
			exp.LegendText = "提督経験値";


			//データ設定
			{
				var record = GetRecords();

				ResourceRecord.ResourceElement prev = null;

				if ( record.Any() ) {
					prev = record.First();
					foreach ( var r in record ) {

						if ( ShouldSkipRecord( r.Date - prev.Date ) )
							continue;

						exp.Points.AddXY( r.Date.ToOADate(), r.HQExp - prev.HQExp );

						prev = r;
					}
				}

				if ( KCDatabase.Instance.Admiral.IsAvailable ) {
					double now = DateTime.Now.ToOADate();
					exp.Points.AddXY( now, prev == null ? 0 : KCDatabase.Instance.Admiral.Exp - prev.HQExp );
				}

				if ( exp.Points.Count > 0 ) {
					int min = (int)exp.Points.Min( p => p.YValues[0] );
					area.AxisY.Minimum = Math.Floor( min / 10000.0 ) * 10000;

					int max = (int)exp.Points.Max( p => p.YValues[0] );
					area.AxisY.Maximum = Math.Ceiling( max / 10000.0 ) * 10000;
				}
			}

		}


		private Axis CreateAxisX( ChartSpan span ) {

			Axis axis = new Axis();

			switch ( span ) {
				case ChartSpan.Day:
					axis.Interval = 2;
					axis.IntervalOffsetType = DateTimeIntervalType.Hours;
					axis.IntervalType = DateTimeIntervalType.Hours;
					break;
				case ChartSpan.Week:
					axis.Interval = 12;
					axis.IntervalOffsetType = DateTimeIntervalType.Hours;
					axis.IntervalType = DateTimeIntervalType.Hours;
					break;
				case ChartSpan.Month:
					axis.Interval = 3;
					axis.IntervalOffsetType = DateTimeIntervalType.Days;
					axis.IntervalType = DateTimeIntervalType.Days;
					break;
				case ChartSpan.Season:
					axis.Interval = 7;
					axis.IntervalOffsetType = DateTimeIntervalType.Days;
					axis.IntervalType = DateTimeIntervalType.Days;
					break;
				case ChartSpan.Year:
				case ChartSpan.All:
					axis.Interval = 1;
					axis.IntervalOffsetType = DateTimeIntervalType.Months;
					axis.IntervalType = DateTimeIntervalType.Months;
					break;
			}

			axis.LabelStyle.Format = "MM/dd HH:mm";
			axis.LabelStyle.Font = Font;
			axis.MajorGrid.LineColor = Color.FromArgb( 192, 192, 192 );

			return axis;
		}


		private Axis CreateAxisY( int minorInterval, int majorInterval ) {

			Axis axis = new Axis();

			axis.LabelStyle.Font = Font;
			axis.IsStartedFromZero = true;
			axis.Interval = majorInterval;
			axis.MajorGrid.LineColor = Color.FromArgb( 192, 192, 192 );
			axis.MinorGrid.Enabled = true;
			axis.MinorGrid.Interval = minorInterval;
			axis.MinorGrid.LineDashStyle = ChartDashStyle.Dash;
			axis.MinorGrid.LineColor = Color.FromArgb( 224, 224, 224 );

			return axis;
		}

		private Axis CreateAxisY( int interval ) {
			return CreateAxisY( interval, interval * 5 );
		}



		private void ResourceChart_GetToolTipText( object sender, ToolTipEventArgs e ) {

			if ( e.HitTestResult.ChartElementType == ChartElementType.DataPoint ) {
				var dp = e.HitTestResult.Series.Points[e.HitTestResult.PointIndex];
				e.Text = string.Format( "{0:MM/dd HH:mm}\n{1} {2}", DateTime.FromOADate( dp.XValue ), e.HitTestResult.Series.LegendText, dp.YValues[0] );
			}

		}


		private void SwitchMenuStrip( ToolStripMenuItem parent, int index ) {

			//すべての子アイテムに対して
			var items = parent.DropDownItems.Cast<ToolStripItem>().Where( i => i is ToolStripMenuItem ).Select( i => i as ToolStripMenuItem );
			int c = 0;

			foreach ( var item in items ) {
				if ( index == c ) {

					item.Checked = true;

				} else {

					item.Checked = false;
				}

				c++;
			}

			parent.Tag = index;
		}


		private int GetSelectedMenuStripIndex( ToolStripMenuItem parent ) {

			return parent.Tag as int? ?? -1;
		}


		private void UpdateChart() {

			switch ( SelectedChartType ) {
				case ChartType.Resource:
					SetResourceChart();
					break;
				case ChartType.ResourceDiff:
					SetResourceDiffChart();
					break;
				case ChartType.Material:
					SetMaterialChart();
					break;
				case ChartType.MaterialDiff:
					SetMateialDiffChart();
					break;
				case ChartType.Experience:
					SetExperienceChart();
					break;
				case ChartType.ExperienceDiff:
					SetExperienceDiffChart();
					break;
			}

		}

		private IEnumerable<ResourceRecord.ResourceElement> GetRecords() {

			var record = RecordManager.Instance.Resource.Record.AsEnumerable<ResourceRecord.ResourceElement>();

			switch ( SelectedChartSpan ) {
				case ChartSpan.Day:
					record = record.Where( r => r.Date >= DateTime.Now.AddDays( -1 ) );
					break;
				case ChartSpan.Week:
					record = record.Where( r => r.Date >= DateTime.Now.AddDays( -7 ) );
					break;
				case ChartSpan.Month:
					record = record.Where( r => r.Date >= DateTime.Now.AddMonths( -1 ) );
					break;
				case ChartSpan.Season:
					record = record.Where( r => r.Date >= DateTime.Now.AddMonths( -3 ) );
					break;
				case ChartSpan.Year:
					record = record.Where( r => r.Date >= DateTime.Now.AddYears( -1 ) );
					break;
			}

			return record;
		}


		private bool ShouldSkipRecord( TimeSpan span ) {

			if ( span.Ticks == 0 )		//初回のデータ( prev == First )は無視しない
				return false;

			switch ( SelectedChartSpan ) {
				case ChartSpan.Day:
				case ChartSpan.Week:
				default:
					return false;

				case ChartSpan.Month:
					return span.TotalHours < 12.0;

				case ChartSpan.Season:
				case ChartSpan.Year:
				case ChartSpan.All:
					return span.TotalDays < 1.0;
			}

		}


		private void Menu_Graph_Resource_Click( object sender, EventArgs e ) {
			SwitchMenuStrip( Menu_Graph, 0 );
			UpdateChart();
		}

		private void Menu_Graph_ResourceDiff_Click( object sender, EventArgs e ) {
			SwitchMenuStrip( Menu_Graph, 1 );
			UpdateChart();
		}

		private void Menu_Graph_Material_Click( object sender, EventArgs e ) {
			SwitchMenuStrip( Menu_Graph, 2 );
			UpdateChart();
		}

		private void Menu_Graph_MaterialDiff_Click( object sender, EventArgs e ) {
			SwitchMenuStrip( Menu_Graph, 3 );
			UpdateChart();
		}

		private void Menu_Graph_Experience_Click( object sender, EventArgs e ) {
			SwitchMenuStrip( Menu_Graph, 4 );
			UpdateChart();
		}

		private void Menu_Graph_ExperienceDiff_Click( object sender, EventArgs e ) {
			SwitchMenuStrip( Menu_Graph, 5 );
			UpdateChart();
		}


		private void Menu_Span_Day_Click( object sender, EventArgs e ) {
			SwitchMenuStrip( Menu_Span, 0 );
			UpdateChart();
		}

		private void Menu_Span_Week_Click( object sender, EventArgs e ) {
			SwitchMenuStrip( Menu_Span, 1 );
			UpdateChart();
		}

		private void Menu_Span_Month_Click( object sender, EventArgs e ) {
			SwitchMenuStrip( Menu_Span, 2 );
			UpdateChart();
		}

		private void Menu_Span_Season_Click( object sender, EventArgs e ) {
			SwitchMenuStrip( Menu_Span, 3 );
			UpdateChart();
		}

		private void Menu_Span_Year_Click( object sender, EventArgs e ) {
			SwitchMenuStrip( Menu_Span, 4 );
			UpdateChart();
		}

		private void Menu_Span_All_Click( object sender, EventArgs e ) {
			SwitchMenuStrip( Menu_Span, 5 );
			UpdateChart();
		}





	}

}
