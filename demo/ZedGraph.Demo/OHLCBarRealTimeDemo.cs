//============================================================================
//ZedGraph Class Library - A Flexible Charting Library for .Net
//Copyright (C) 2006 John Champion and Jerry Vos
//
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this library; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Timer = System.Timers.Timer;

namespace ZedGraph.Demo
{
	/// <summary>
	/// Summary description for SimpleDemo.
	/// </summary>
	public class OHLCBarRealTimeDemo : DemoBase
	{

		public OHLCBarRealTimeDemo()
      : base("Demonstration of the OHLCBar Chart Type",
             "OHLCBar Real-Time Demo", DemoType.Financial)
    {
      m_Timer   = new Timer { Interval = 500, Enabled = false, SynchronizingObject = ZedGraphControl };
      m_Data    = new StockPointList(true);
      m_FilteredData = new DynFilteredPointList(new [] {0.0}, new [] {0.0});
      m_EMAData = new PointPairList();
      m_Rand    = new Random();
      var now   = fillSampleData();

      m_Pane    = base.GraphPane;

      //------------------------------------------------------------------------
      // Setup the pane and X/Y axis
      //------------------------------------------------------------------------
      m_Pane.Title.Text             = "OHLC Real-Time Bar Chart Demo";
      //m_Pane.XAxis.Title.Text     = "Trading Date";
      //m_Pane.Y2Axis.Title.Text    = "Share Price, $US";
      m_Pane.Title.IsVisible        = false;
      m_Pane.Legend.IsVisible       = false;
      m_Pane.Margin.Top             = 1;
      m_Pane.Margin.Left            = 1;
      m_Pane.Margin.Right           = 0;
      m_Pane.Margin.Bottom          = 1;
      m_Pane.Legend.Gap             = 0;
      m_Pane.MouseWheelAction       = MouseWheelActions.Zoom | MouseWheelActions.PanH;
      m_Pane.IsPenWidthScaled       = true;
      m_Pane.IsBoundedRanges        = true;
      m_Pane.IsIgnoreMissing        = true;
      m_Pane.IsAlignGrids           = true;
      
      // Customize X axis
      m_Pane.XAxis.Title.IsVisible             = false;
      m_Pane.XAxis.AxisGap                     = 5;
      m_Pane.XAxis.Scale.LabelGap              = 0.2f;
      m_Pane.XAxis.MajorGrid.IsVisible         = true;
      m_Pane.XAxis.MajorGrid.Color             = Color.DarkGray;
      m_Pane.XAxis.MajorGrid.DashOff           = 7;
      m_Pane.XAxis.MajorGrid.DashOn            = 1;
      m_Pane.XAxis.MajorTic.Size               = 3;
      m_Pane.XAxis.MinorTic.Size               = 1;
      m_Pane.XAxis.Type                        = AxisType.DateAsOrdinal;
      //m_Pane.XAxis.Scale.MajorUnit             = DateUnit.Minute;
      //m_Pane.XAxis.Scale.MinorUnit             = DateUnit.Second;
      m_Pane.XAxis.Scale.Format                = "yyyy-MM-dd\nHH:mm:ss";
      m_Pane.XAxis.Scale.FontSpec.Size         = 9;
      //m_Pane.XAxis.Scale.MajorStep             = 2;
      //m_Pane.XAxis.Scale.MinorStep             = 30;
      m_Pane.XAxis.Scale.MaxAuto               = true;
      m_Pane.XAxis.Scale.MinAuto               = true;
//      m_Pane.XAxis.Scale.MajorStep             = new XDate(0, 0, 0, 0, 2, 0).XLDate;
//      m_Pane.XAxis.Scale.MinorStep             = new XDate(0, 0, 0, 0, 0,15).XLDate;
//
//      m_Pane.XAxis.Scale.MajorStep             = 120.0f / XDate.SecondsPerDay; // 120s
//      m_Pane.XAxis.Scale.MinorStep             = 15.0f  / XDate.SecondsPerDay; // 15s

//      m_Pane.XAxis.Scale.BaseTic               = new XDate(0, 0, 0, 0, 0, 5);
//      m_Pane.XAxis.Scale.FontSpec.ScaleFactor  = 1.0f;
//      m_Pane.XAxis.Scale.MinAuto               = true;
      //m_Pane.XAxis.Scale.MaxAuto               = true;
//      m_Pane.XAxis.Scale.MinGrace              = 50;
      //m_Pane.XAxis.Scale.MaxGrace              = 50;
      m_Pane.XAxis.Scale.IsSkipFirstLabel      = true;
      m_Pane.XAxis.Scale.IsSkipLastLabel       = false;
      //m_Pane.XAxis.Scale.Max                   = new XDate(now);
      //m_Pane.XAxis.Scale.Min                   = new XDate(now) - 2*60;
//      m_Pane.XAxis.Scale.AlignH                = AlignH.Center;
//      m_Pane.XAxis.Scale.Align                 = AlignP.Inside;
      m_Pane.XAxis.MajorTic.IsBetweenLabels    = true;
      m_Pane.XAxis.MinorTic.Size               = 2.5f;
      m_Pane.XAxis.MinorTic.IsInside           = false;
      m_Pane.XAxis.MajorTic.IsInside           = false;
      m_Pane.XAxis.MinorTic.IsOutside          = true;

      m_Pane.XAxis.MajorGrid.IsVisible         = true;
      m_Pane.XAxis.MajorGrid.DashOff           = 10;
      m_Pane.XAxis.MajorGrid.DashOn            = 1;
      m_Pane.XAxis.MajorGrid.Color             = Color.SlateGray;
      m_Pane.XAxis.MinorGrid.IsVisible         = false;
      //      m_Pane.XAxis.Scale.MajorStep             = new XDate(now - TimeSpan.FromSeconds(15));

      // Disable left-side Y axis
      m_Pane.YAxis.IsVisible                   = false;
      m_Pane.YAxis.Title.IsVisible             = false;
      m_Pane.YAxis.MinSpace                    = 0;

      // Enable the Y2 axis display
      m_Pane.Y2Axis.IsVisible                  = true;
      m_Pane.Y2Axis.Title.IsVisible            = false;
      m_Pane.Y2Axis.MinSpace                   = 50;
      m_Pane.Y2Axis.AxisGap                    = 5;
      m_Pane.Y2Axis.Scale.LabelGap             = 0;
      m_Pane.Y2Axis.MajorGrid.IsVisible        = true;
      m_Pane.Y2Axis.MajorGrid.DashOff          = 10;
      m_Pane.Y2Axis.MajorGrid.DashOn           = 1;
      m_Pane.Y2Axis.MajorGrid.Color            = Color.SlateGray;
      m_Pane.Y2Axis.MinorGrid.PenWidth         = 1;

      m_Pane.Y2Axis.MinorGrid.IsVisible        = false;
      /*
      m_Pane.Y2Axis.MinorGrid.DashOff          = 15;
      m_Pane.Y2Axis.MinorGrid.DashOn           = 1;
      m_Pane.Y2Axis.MinorGrid.Color            = Color.DarkGray;
      m_Pane.Y2Axis.MinorGrid.PenWidth         = 1;
      */

      m_Pane.Y2Axis.MajorTic.Size              = 3;
      m_Pane.Y2Axis.MinorTic.Size              = 1;
      //m_Pane.Y2Axis.Scale.AlignH               = AlignH.Right;
      m_Pane.Y2Axis.Scale.Align                = AlignP.Outside;
      m_Pane.Y2Axis.Scale.MinAuto              = true;
      m_Pane.Y2Axis.Scale.MaxAuto              = true;
      m_Pane.Y2Axis.Scale.Format               = "0.00000";
      m_Pane.Y2Axis.MinorTic.IsInside          = false;
      m_Pane.Y2Axis.MajorTic.IsInside          = false;
      m_Pane.Y2Axis.MinorTic.IsOutside         = true;
      m_Pane.Y2Axis.MajorTic.IsOutside         = true;
      m_Pane.Y2Axis.Scale.FontSpec.Size        = 9;
//      m_Pane.Y2Axis.Scale.FontSpec.ScaleFactor = 1.0f;

      m_Pane.Chart.Fill                        = new Fill(Color.Black);
      m_Pane.Fill                              = new Fill(Color.SlateGray, Color.FromArgb(220, 220, 255), 45.0f);

      //------------------------------------------------------------------------
      // Cardinal spline smoothing function
      //------------------------------------------------------------------------
      LineItem curve                = m_Pane.AddCurve($"EMA({EMA_ALPHA:0.0})", m_EMAData, Color.LightCoral, SymbolType.None);
      curve.Line.IsSmooth           = true;
      curve.Line.SmoothTension      = 0.5F;
      curve.IsY2Axis                = true; // Associate this curve with the Y2 axis
      curve.YAxisIndex              = 0;    // Associate this curve with the first Y2 axis
      curve.IsSelectable            = true;
      curve.IsSelected              = true;

      //------------------------------------------------------------------------
      // Add OHCL time series
      //------------------------------------------------------------------------
      //OHLCBarItem myCurve           = m_Pane.AddOHLCBar("trades", m_Data, Color.Black);
      //      myCurve.Bar.Width             = 2;
      //      myCurve.Bar.IsAutoSize        = true;
      //      myCurve.Bar.Color             = Color.DodgerBlue;
      var myCurve                       = m_Pane.AddJapaneseCandleStick("trades", m_Data);
      myCurve.Stick.FallingColor        = Color.White;
      myCurve.Stick.Color               = Color.White;
      myCurve.Stick.RisingFill.Color    = Color.DarkOliveGreen;
      myCurve.Stick.FallingFill.Color   = Color.Black;
      myCurve.Stick.FallingBorder.Color = Color.AntiqueWhite;
      myCurve.Stick.RisingBorder.Color  = Color.AntiqueWhite;
      myCurve.IsY2Axis                  = true; // Associate this curve with the Y2 axis
      myCurve.YAxisIndex                = 0;    // Associate this curve with the first Y2 axis (this is actually default)
      myCurve.IsSelectable              = true;
      curve.IsSelected                  = true;

      m_Pane.IsAlignGrids               = true;
      m_Pane.IsFontsScaled              = false;

      //------------------------------------------------------------------------
      // Add a line to track last close
      //------------------------------------------------------------------------
      m_Line = m_Pane.Y2Axis.AddHLine(Color.Red, "close-price");
      m_Line.Style   = DashStyle.Custom;
      m_Line.DashOn  = 1;
      m_Line.DashOff = 3;
      m_Line.Width   = 1;
      ZedGraphControl.AxisChange();
      ZedGraphControl.Invalidate();

      m_Timer.Elapsed += (o, args) =>
      {
        calc(DateTime.Now, true);
        //base.ZedGraphControl.AxisChange();
        base.ZedGraphControl.Invalidate();
      };

      /*
      m_XHair = new LineObj(Color.SlateGray, 0, 0, m_Pane.Rect.Width, 0)
      {
        IsClippedToChartRect = true,
        ZOrder = ZOrder.A_InFront,
        IsVisible = false
      };
      m_YHair = new LineObj(Color.SlateGray, 0, 0, 0, m_Pane.Rect.Height)
      {
        IsClippedToChartRect = true,
        ZOrder = ZOrder.A_InFront,
        IsVisible = false
      };

      m_XHair.Line.Style = DashStyle.Dash;
      m_XHair.Location.CoordinateFrame = CoordType.AxisXYScale;
      m_XHair.Location.AlignH = AlignH.Left;
      m_XHair.Location.AlignV = AlignV.Top;
      m_YHair.Line.Style = DashStyle.Dash;
      m_YHair.Location.CoordinateFrame = CoordType.AxisXYScale;
      m_YHair.Location.AlignH = AlignH.Left;
      m_YHair.Location.AlignV = AlignV.Top;

      m_Pane.GraphObjList.Add(m_XHair);
      m_Pane.GraphObjList.Add(m_YHair);

      //ZedGraphControl.GraphPane.XAxis.Scale.Max = m_Pane.XAxis.Scale.Max;
      //ZedGraphControl.GraphPane.XAxis.Scale.Min = m_Pane.XAxis.Scale.Min;

      ZedGraphControl.MouseMove += ZedGraphControl_MouseMove;
      ZedGraphControl.MouseLeave += ZedGraphControl_MouseLeave;
      */

      m_Timer.Enabled = true;
    }

	  private DateTime fillSampleData()
	  {
      // First day is jan 1st
      var now = DateTime.Now;
      m_Now = new XDate(now) - 15.0f/XDate.MinutesPerDay;
      m_Open = 50.0;

      for (var i = 0; i < 60 * 15; i += 5)
      {
        m_Now.AddSeconds(5);
        calc(m_Now, false);
      }

      return now;
    }

    public override void Activate()
	  {
      ZedGraphControl.IsEnableGraphEdit = true;
      ZedGraphControl.IsZoomOnMouseCenter = true;
      //ZedGraphControl.IsShowHScrollBar = true;
      //ZedGraphControl.IsEnableHZoom = false;
      //ZedGraphControl.IsEnableVZoom = true;
      ZedGraphControl.IsAutoScrollRange = true;
      ZedGraphControl.ZoomResolution = 0.001;
      m_DistanceMeasurer.Coord = CoordType.AxisXY2Scale;
    }

    public override void Deactivate()
	  {
      ZedGraphControl.IsZoomOnMouseCenter = false;
      //ZedGraphControl.IsShowHScrollBar = false;
      //ZedGraphControl.IsEnableHZoom = true;
      //ZedGraphControl.IsEnableVZoom = true;
      ZedGraphControl.IsAutoScrollRange = false;
      m_DistanceMeasurer.Coord = CoordType.AxisXYScale;
    }

    private readonly Timer m_Timer;
    private readonly StockPointList m_Data;
    private readonly PointPairList  m_EMAData;
    private readonly DynFilteredPointList m_FilteredData;

    private XDate  m_Now;
    private double m_Open;
    private readonly Random m_Rand;
    private readonly GraphPane m_Pane;
    private readonly LineHObj  m_Line;

    private readonly LineObj m_XHair = null;
    private readonly LineObj m_YHair = null;

    private const double  EMA_ALPHA = 0.8;
    private double        m_EMA     = 0;
    private float         m_MinVal  = 0f;
    private float         m_MaxVal  = 0f;

    private StockPt   LastPoint    => m_Data.Count    > 0 ? ((StockPt)m_Data[m_Data.Count - 1]) : new StockPt();

    //------------------------------------------------------------------------
    // Update data on initialization or on timer
    //------------------------------------------------------------------------
    private void calc(XDate now, bool timer)
	  {
      m_Timer.Enabled = false;
      const double diff = 5.0f / XDate.SecondsPerDay;
      var tm = now - 5;
      var add = !timer || (m_Data.Count > 0 && ((now - LastPoint.Date) > diff));
      StockPt pt = null;
      var up = m_Rand.NextDouble() > 0.5;
      var val = up ? LastPoint.Low : LastPoint.High;

      Action<double, double, bool> set_min_max = (min, max, absolute) =>
      {
        var grace = (m_Pane.Y2Axis.Scale.Max - m_Pane.Y2Axis.Scale.Min) * m_Pane.Y2Axis.Scale.MaxGrace;
        var gap = grace; //m_Pane.Y2Axis.Scale.ReverseTransform(10) + grace;
        m_Pane.Y2Axis.Scale.Min = absolute ? min-gap : Math.Min(m_Pane.Y2Axis.Scale.Min, min - gap);
        m_Pane.Y2Axis.Scale.Max = absolute ? max+gap : Math.Max(m_Pane.Y2Axis.Scale.Max, max + gap);
      };

      if (add)
      {
        var open  = m_Open + m_Rand.NextDouble()*10.0 - 5.0;
        var close = m_Open + m_Rand.NextDouble()*10.0 - 5.0;
        var hi    = Math.Max(open, close) + m_Rand.NextDouble()*5.0;
        var low   = Math.Min(open, close) - m_Rand.NextDouble()*5.0;
        var vol   = m_Rand.NextDouble()*1000;

        var x = now.XLDate - (now.XLDate % diff);
        pt = new StockPt(x, hi, low, open, close, (int)vol);

        m_Data.Add(pt);
        m_Open = close;

        m_EMA = EMA_ALPHA * close + (1.0 - EMA_ALPHA) * m_EMA;
        m_EMAData.Add(x, m_EMA);

        if (timer)
        {
          //m_Pane.XAxis.Scale.Max = now + 5;
          //m_Pane.XAxis.Scale.Min += diff;

          if (Math.Abs(Math.Round(m_Pane.XAxis.Scale.Max) - m_Data.Count) < 5 )
          {
            var window = (int)(m_Pane.XAxis.Scale.Max - m_Pane.XAxis.Scale.Min);
            //m_Pane.XAxis.Scale.SetRange();
            m_Pane.XAxis.Scale.Max = m_Data.Count + 1;
            m_Pane.XAxis.Scale.Min = m_Pane.XAxis.Scale.Max - window;

            double min = double.MaxValue, max = double.MinValue;
            var xMin = Scale.MinMax(0, (int)m_Pane.XAxis.Scale.Min, m_Data.Count);
            var xMax = Scale.MinMax(0, (int)m_Pane.XAxis.Scale.Max, m_Data.Count);
            for (int i = xMin; i < xMax; ++i)
            {
              var d = (StockPt)m_Data[i];
              min = Math.Min(d.Low, min);
              max = Math.Max(d.High, max);
            }

            set_min_max(min, max, true);
            m_Pane.AxisChange();
          }

          if (m_Data.Count%1 == 0)
          {
            var yy = m_Pane.Y2Axis.Scale.ReverseTransform(m_Pane.Y2Axis.Scale.Transform(val) + (up ? 5 : -5));
            //var y2 = val * (up ? 0.96 : 1.03);
            var arrow = new PointObj(m_Data.Count-1, yy, 5,
                                     up ? SymbolType.ArrowUp : SymbolType.ArrowDown,
                                     up ? Color.Green : Color.Red)
            {
              IsMovable = false,
              IsY2Axis = true,
              YAxisIndex = 0,
              //Fill = {Type = FillType.None},
              IsClippedToChartRect = true
            };

            //arrow.Line.Width = 1;
            //arrow.Location.CoordinateFrame = CoordType.AxisXYScale;
            m_Pane.GraphObjList.Add(arrow);
          }
        }
      }
      else if (m_Data.Count > 0)
      {
        pt       = LastPoint;
        pt.Close = m_Open + m_Rand.NextDouble() * 10.0 - 5.0;
        pt.High  = Math.Max(pt.High, Math.Max(m_Open, pt.Close) + m_Rand.NextDouble() * 5.0);
        pt.Low   = Math.Min(pt.Low,  Math.Min(m_Open, pt.Close) - m_Rand.NextDouble() * 5.0);

        if (timer && Math.Abs(Math.Round(m_Pane.XAxis.Scale.Max) - m_Data.Count) < 5)
          set_min_max(pt.Low, pt.High, false);
      }

      if (m_Line != null)
      {
        m_Line.Value = pt.Close;
      }

      m_Now = now;
      m_Timer.Enabled = true;
    }

    private void ZedGraphControl_MouseLeave(object sender, EventArgs e)
    {
      if (!m_XHair.IsVisible) return;

      m_XHair.IsVisible = false;
      m_YHair.IsVisible = false;
      ZedGraphControl.Refresh();
    }

    private void ZedGraphControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
    {
      double x, y;
      m_Pane.ReverseTransform(e.Location, out x, out y);

      #region crosshair

      //out of the bounds
      if (x < m_Pane.XAxis.Scale.Min ||
          x > m_Pane.XAxis.Scale.Max ||
          y < m_Pane.Y2Axis.Scale.Min ||
          y > m_Pane.Y2Axis.Scale.Max)
      {
        ZedGraphControl_MouseLeave(new object(), new EventArgs());
      }
      else//ok draw
      {
        m_XHair.Location.X = m_Pane.XAxis.Scale.Min;
        m_XHair.Location.Y = y;
        m_XHair.Location.Width = m_Pane.XAxis.Scale.Max - m_Pane.XAxis.Scale.Min;
        m_XHair.Location.Height = 0;

        m_YHair.Location.X = x;
        m_YHair.Location.Y = m_Pane.Y2Axis.Scale.Min;
        m_YHair.Location.Width = 0;
        m_YHair.Location.Height = m_Pane.Y2Axis.Scale.Max - m_Pane.Y2Axis.Scale.Min;

        m_XHair.IsVisible = true;
        m_YHair.IsVisible = true;

        ZedGraphControl.Refresh();
      }

      #endregion
    }
  }
}