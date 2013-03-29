using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media;
using System.Windows;

namespace GraphLib.Model
{
    /// <summary>
    /// class Thread Pool Calulate Data
    /// </summary>
    public class ThreadCalculate
    {
        #region Private Variable
        /// <summary>
        /// Graph Plot Data
        /// </summary>
        private List<double[]> _GraphPlotData;
        /// <summary>
        /// done Event
        /// </summary>
        private ManualResetEvent _doneEvent;
        /// <summary>
        /// Graph No
        /// </summary>
        private int _GraphNo;
        /// <summary>
        /// output Steam
        /// </summary>
        private StreamGeometry _OutputSteam;
        /// <summary>
        /// is line graph
        /// </summary>
        private bool _IsLineGraph;
        /// <summary>
        /// log class
        /// </summary>
        private LogClass _Log4NetClass;
        /// <summary>
        /// DotWidth
        /// </summary>
        private double _DotWidth;
        /// <summary>
        /// Max GraphWidth;
        /// </summary>
        private double _MaxGraphWidth;
        #endregion

        #region Public Properties
        /// <summary>
        /// output steam
        /// </summary>
        public StreamGeometry OutputSteam
        {
            get
            {
                return _OutputSteam;
            }
        }

        /// <summary>
        /// is line graph
        /// </summary>
        public bool IsLineGraph
        {
            set
            {
                _IsLineGraph = value;
            }
        }

        /// <summary>
        /// DotWidth
        /// </summary>
        public double DotWidth
        {
            set
            {
                _DotWidth = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="graphPlotData"></param>
        /// <param name="graphNo"></param>
        /// <param name="doneEvent"></param>
        public ThreadCalculate(List<double[]> graphPlotData, int graphNo, ManualResetEvent doneEvent, double maxGraphWidth)
        {
            _Log4NetClass = new LogClass(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _GraphPlotData = graphPlotData;
            _doneEvent = doneEvent;
            _GraphNo = graphNo;
            _MaxGraphWidth = maxGraphWidth;
        }
        #endregion

        #region Public Function
        /// <summary>
        ///  ThreadPoolCallback
        /// </summary>
        /// <param name="threadContext"></param>
        public void ThreadPoolCallback(Object threadContext)
        {
            _OutputSteam = Calculate();
            _doneEvent.Set();
        }

        /// <summary>
        /// Calulate SteamGeomatry
        /// </summary>
        /// <returns></returns>
        public StreamGeometry Calculate()
        {
            StreamGeometry steamgeo = new StreamGeometry();
            try
            {
                int countline = this._GraphPlotData.Count;
                steamgeo.FillRule = FillRule.EvenOdd;
                StreamGeometryContext ctx;
                Point p1 = new Point(0, 0);
                Point p2 = new Point(0, 0);
                Point p0 = new Point(0, 0);
                bool isbegin = false;
                List<Point> points = new List<Point>();
                double dotwidth = _DotWidth / (double)2;
                using (ctx = steamgeo.Open())
                {
                    for (int i = 0; i < countline; i++)
                    {

                        if (i == 0)
                        {
                            p0.X = this._GraphPlotData[i][0];
                            p0.Y = this._GraphPlotData[i][_GraphNo];

                            ctx.BeginFigure(p0, false, false);
                            isbegin = true;
                            if (!_IsLineGraph)
                            {
                                //ctx.BeginFigure(new Point(p0.X - dotwidth, p0.Y), false, false);
                                p2.X = p0.X + dotwidth;
                                p2.Y = p0.Y;

                                double x = p0.X - dotwidth;
                                if (x < 0)
                                    x = 0;

                                ctx.LineTo(new Point(x, p0.Y), false, false);
                                ctx.LineTo(p2, true, true);
                            }
                            //else
                            //    ctx.BeginFigure(p0, false, false);

                        }
                        else
                        {
                            p1.X = this._GraphPlotData[i][0];
                            p1.Y = this._GraphPlotData[i][_GraphNo];

                            if (_IsLineGraph)
                                points.Add(p1);
                            else
                            {
                                p2.X = p1.X + dotwidth;
                                p2.Y = p1.Y;

                                if (p2.X > _MaxGraphWidth)
                                    p2.X = _MaxGraphWidth;

                                ctx.LineTo(new Point(p1.X - dotwidth, p1.Y), false, false);
                                ctx.LineTo(p2, true, true);
                            }
                        }
                    }
                    if (_IsLineGraph && isbegin)
                        ctx.PolyLineTo(points, true, true);
                }
                steamgeo.Freeze();

                return steamgeo;
            }
            catch (ThreadAbortException)
            {
                return steamgeo;
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "Calculate");
                return steamgeo;
            }

        }
        #endregion
    }
}
