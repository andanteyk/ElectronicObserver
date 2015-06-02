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
		public DialogResourceChart() {
			InitializeComponent();
		}

		private void DialogResourceChart_Load( object sender, EventArgs e ) {

			//checkme: if レコードが未ロード or 0個の項目 then throw
			SetResourceDiffChart( ChartSpan.Week );
		}



		private void SetResourceChart( ChartSpan cspan ) {

			ResourceChart.ChartAreas.Clear();
			var area = ResourceChart.ChartAreas.Add( "ResourceChartArea" );
			area.AxisX = CreateAxisX( cspan );
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
				var record = RecordManager.Instance.Resource.Record.AsEnumerable<ResourceRecord.ResourceElement>();

				switch ( cspan ) {
					case ChartSpan.Day:
						record = record.Where( r => r.Date >= DateTime.Now.AddDays( -1 ) );
						break;
					case ChartSpan.Week:
						record = record.Where( r => r.Date >= DateTime.Now.AddDays( -7 ) );
						break;
					case ChartSpan.Month:
						record = record.Where( r => r.Date >= DateTime.Now.AddMonths( -1 ) );
						break;
					case ChartSpan.Year:
						record = record.Where( r => r.Date >= DateTime.Now.AddYears( -1 ) );
						break;
				}

				foreach ( var r in record ) {

					fuel.Points.AddXY( r.Date.ToOADate(), r.Fuel );
					ammo.Points.AddXY( r.Date.ToOADate(), r.Ammo );
					steel.Points.AddXY( r.Date.ToOADate(), r.Steel );
					bauxite.Points.AddXY( r.Date.ToOADate(), r.Bauxite );

				}

				int min = (int)new[] { fuel.Points.Min( p => p.YValues[0] ), ammo.Points.Min( p => p.YValues[0] ), steel.Points.Min( p => p.YValues[0] ), bauxite.Points.Min( p => p.YValues[0] ) }.Min();
				area.AxisY.Minimum = Math.Floor( min / 1000.0 ) * 1000;

				int max = (int)new[] { fuel.Points.Max( p => p.YValues[0] ), ammo.Points.Max( p => p.YValues[0] ), steel.Points.Max( p => p.YValues[0] ), bauxite.Points.Max( p => p.YValues[0] ) }.Max();
				area.AxisY.Maximum = Math.Ceiling( max / 1000.0 ) * 1000;
			}

		}


		private void SetResourceDiffChart( ChartSpan cspan ) {

			ResourceChart.ChartAreas.Clear();
			var area = ResourceChart.ChartAreas.Add( "ResourceChartArea" );
			area.AxisX = CreateAxisX( cspan );
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
				var record = RecordManager.Instance.Resource.Record.AsEnumerable<ResourceRecord.ResourceElement>();

				switch ( cspan ) {
					case ChartSpan.Day:
						record = record.Where( r => r.Date >= DateTime.Now.AddDays( -1 ) );
						break;
					case ChartSpan.Week:
						record = record.Where( r => r.Date >= DateTime.Now.AddDays( -7 ) );
						break;
					case ChartSpan.Month:
						record = record.Where( r => r.Date >= DateTime.Now.AddMonths( -1 ) );
						break;
					case ChartSpan.Year:
						record = record.Where( r => r.Date >= DateTime.Now.AddYears( -1 ) );
						break;
				}

				
				var prev = record.First();
				foreach ( var r in record ) {

					fuel.Points.AddXY( r.Date.ToOADate(), r.Fuel - prev.Fuel );
					ammo.Points.AddXY( r.Date.ToOADate(), r.Ammo - prev.Ammo );
					steel.Points.AddXY( r.Date.ToOADate(), r.Steel - prev.Steel );
					bauxite.Points.AddXY( r.Date.ToOADate(), r.Bauxite - prev.Bauxite );

					prev = r;
				}

				int min = (int)new[] { fuel.Points.Min( p => p.YValues[0] ), ammo.Points.Min( p => p.YValues[0] ), steel.Points.Min( p => p.YValues[0] ), bauxite.Points.Min( p => p.YValues[0] ) }.Min();
				area.AxisY.Minimum = Math.Floor( min / 1000.0 ) * 1000;

				int max = (int)new[] { fuel.Points.Max( p => p.YValues[0] ), ammo.Points.Max( p => p.YValues[0] ), steel.Points.Max( p => p.YValues[0] ), bauxite.Points.Max( p => p.YValues[0] ) }.Max();
				area.AxisY.Maximum = Math.Ceiling( max / 1000.0 ) * 1000;

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

		private Axis CreateAxisY( int interval ) {

			Axis axis = new Axis();

			axis.LabelStyle.Font = Font;
			axis.IsStartedFromZero = true;
			axis.Interval = interval * 5;
			axis.MajorGrid.LineColor = Color.FromArgb( 192, 192, 192 );
			axis.MinorGrid.Enabled = true;
			axis.MinorGrid.Interval = interval;
			axis.MinorGrid.LineDashStyle = ChartDashStyle.Dash;
			axis.MinorGrid.LineColor = Color.FromArgb( 224, 224, 224 );

			return axis;
		}



		private void ResourceChart_GetToolTipText( object sender, ToolTipEventArgs e ) {

			if ( e.HitTestResult.ChartElementType == ChartElementType.DataPoint ) {
				var dp = e.HitTestResult.Series.Points[e.HitTestResult.PointIndex];
				e.Text = string.Format( "{0:MM/dd HH:mm}\n{1} {2}", DateTime.FromOADate( dp.XValue ), e.HitTestResult.Series.LegendText, dp.YValues[0] );
			}

		}
	}


	public enum ChartSpan {
		Day,
		Week,
		Month,
		Year,
		All
	}
}
