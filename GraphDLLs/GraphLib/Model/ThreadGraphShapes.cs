using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Threading;
using System.ComponentModel;

namespace GraphLib.Model
{
    public class ThreadGraphShapes
    {
        #region Private variable
        /// <summary>
        /// GraphModel
        /// </summary>
        private GraphModel _GraphModel;

        /// <summary>
        /// log class
        /// </summary>
        private LogClass _Log4NetClass;

        private bool _IsRefresh = false;
        #endregion

        #region Delegate/Event
        /// <summary>
        /// delegate GraphCreatedEventHandler
        /// </summary>        
        public delegate void GraphCreatedEventHandler(StreamGeometry[] graphShapes, bool isRefresh, int startLine);
        /// <summary>
        /// event GraphCreatedEventHandler
        /// </summary>
        public event GraphCreatedEventHandler OnGraphCreated = null;
        #endregion

        #region Public Properties
        /// <summary>
        /// Parent Graph Model
        /// </summary>
        public GraphModel ParentGraphModel
        {
            get
            {
                return _GraphModel;
            }
            set
            {
                _GraphModel = value;
            }
        }
        /// <summary>
        /// Is Refresh graph
        /// </summary>
        public bool IsRefresh
        {
            get
            {
                return _IsRefresh;
            }
            set
            {
                _IsRefresh = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="graphModel"></param>
        public ThreadGraphShapes(GraphModel graphModel)
        {
            _Log4NetClass = new LogClass(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            _GraphModel = graphModel;
        }
        #endregion

        #region Public Function
        /// <summary>
        /// Create Model
        /// </summary>
        public void CreateModel()
        {
            try
            {
                object lockobj = new object();

                Point p1 = new Point(0, 0);
                Point p0 = new Point(0, 0);
                int startline = 0;
                _GraphModel.GraphPlotData.Clear();
                StreamGeometry[] steamgeo = new StreamGeometry[this._GraphModel.MaxGraphNo];

                lock (lockobj)
                {
                    // One event is used for each Fibonacci object
                    ManualResetEvent[] doneEvents = new ManualResetEvent[this._GraphModel.MaxGraphNo];
                    ThreadCalculate[] threadcalcarray = new ThreadCalculate[this._GraphModel.MaxGraphNo];
                    StreamGeometryContext[] ctx = new StreamGeometryContext[this._GraphModel.MaxGraphNo];

                    startline = CreatePlotData();

                    int countline = this._GraphModel.GraphPlotData.Count;
                    double graphwidth = _GraphModel.GraphSize.Width - _GraphModel.GridLineData.Margin.Left - _GraphModel.GridLineData.Margin.Right;

                    for (int gcount = 0; gcount < _GraphModel.MaxGraphNo; gcount++)
                    {
                        if (_GraphModel.GraphShow[gcount])
                        {
                            int chno = _GraphModel.ChannelNumber[gcount];
                            // Configure and launch threads calculate SteamGeomatry using ThreadPool
                            doneEvents[gcount] = new ManualResetEvent(false);
                            ThreadCalculate treadcalc = new ThreadCalculate(this._GraphModel.GraphPlotData, chno, doneEvents[gcount], graphwidth);
                            treadcalc.IsLineGraph = _GraphModel.IsLineGraph;
                            treadcalc.DotWidth = _GraphModel.DotWidth;
                            threadcalcarray[gcount] = treadcalc;
                            ThreadPool.QueueUserWorkItem(treadcalc.ThreadPoolCallback, gcount);

                        }

                    }

                    //Result
                    for (int gcount = 0; gcount < _GraphModel.MaxGraphNo; gcount++)
                    {
                        if (threadcalcarray[gcount] != null)
                        {
                            doneEvents[gcount].WaitOne(150);
                            steamgeo[gcount] = threadcalcarray[gcount].OutputSteam;
                        }
                    }
                }

                if (OnGraphCreated != null)
                    OnGraphCreated(steamgeo, _IsRefresh, startline);



            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "Create");
            }
        }
        #endregion

        #region Private Function
        /// <summary>
        /// Create Plot Data
        /// </summary>
        private int CreatePlotData()
        {
            try
            {
                double graphwidth = _GraphModel.GraphSize.Width - _GraphModel.GridLineData.Margin.Left - _GraphModel.GridLineData.Margin.Right;
                double graphheight = _GraphModel.GraphSize.Height - _GraphModel.GridLineData.Margin.Top - _GraphModel.GridLineData.Margin.Bottom;
                double plotvaluex = _GraphModel.GridLineData.MaxGridValueX - _GraphModel.GridLineData.MinGridValueX;
                double plotvaluey = _GraphModel.GridLineData.MaxGridValueY - _GraphModel.GridLineData.MinGridValueY;
                int countline = this._GraphModel.GraphRawData.Count;
                double startpos = 0;
                int startline = 0;

                if (_GraphModel.GraphMode == GraphMode.Moving)
                {
                    if (countline < _GraphModel.PlotCountX)
                    {
                        startpos = plotvaluex - (countline * _GraphModel.IncrementX);
                    }
                }

                for (int i = 0; i < countline; i++)
                {
                    double[] realpoint = (double[])this._GraphModel.GraphRawData[i].Clone();

                    if (realpoint[0] <= _GraphModel.GridLineData.MaxGridValueX && realpoint[0] >= _GraphModel.GridLineData.MinGridValueX)
                    {

                        if (_GraphModel.IsLineGraph && this._GraphModel.GraphPlotData.Count == 0 && _GraphModel.GraphMode == GraphMode.Normal)
                        {
                            realpoint[0] = 0;
                        }
                        else
                        {
                            double rawx = (realpoint[0] - _GraphModel.GridLineData.MinGridValueX + startpos) / (plotvaluex);
                            realpoint[0] = (rawx * graphwidth);
                        }

                        if (realpoint[0] > graphwidth)
                        {
                            realpoint[0] = graphwidth;
                        }

                        for (int gcount = 0; gcount < _GraphModel.MaxGraphNo; gcount++)
                        {
                            int chpos = _GraphModel.ChannelNumber[gcount];

                            double rawy = (realpoint[chpos] - _GraphModel.GridLineData.MinGridValueY) / (plotvaluey);

                            realpoint[chpos] = graphheight - (rawy * graphheight) - _GraphModel.GridLineData.LineThick;
                        }


                        this._GraphModel.GraphPlotData.Add(realpoint);
                        if (this._GraphModel.GraphPlotData.Count == 1)
                            startline = i;
                    }
                }

                return startline;
                //if (_GraphModel.IsLineGraph && this._GraphModel.GraphPlotData.Count > 0)
                //{
                //    if (_GraphModel.GraphPlotData[_GraphModel.GraphPlotData.Count - 1][0] != graphwidth)
                //        _GraphModel.GraphPlotData[_GraphModel.GraphPlotData.Count - 1][0] = graphwidth;
                //}
            }
            catch (Exception ex)
            {
                _Log4NetClass.ShowError(ex.ToString(), "CreatePlotData");
                return 0;
            }
        }
        #endregion
    }
}
