using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphLib.Model;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace GraphLib.Controller
{

    public class GraphController
    {
        #region Private Variable
        /// <summary>
        /// GraphModel
        /// </summary>
        private GraphModel _GraphModel;

        /// <summary>
        /// log class
        /// </summary>
        private LogClass _Log4NetClass;
        /// <summary>
        /// GraphInfo
        /// </summary>
        private GraphInfo _GraphInfo;
        #endregion

        #region Constructor
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="graphModel">input graphmodel</param>
        public GraphController(GraphModel graphModel)
        {
            _GraphModel = graphModel;
            _Log4NetClass = new LogClass(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }
        /// <summary>
        /// constructor
        /// </summary>
        public GraphController()
        {
            _Log4NetClass = new LogClass(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }
        #endregion

        #region Public Function
        /// <summary>
        /// GraphInfo
        /// </summary>
        public GraphInfo GraphInfo
        {
            set
            {
                _GraphInfo = value;
            }
        }

        /// <summary>
        /// create grid line.
        /// </summary>
        public void CreateGrid()
        {
            try
            {
                _GraphModel.CreateGrid();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// create measure model
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void CreateMeasure(double width, double height)
        {
            try
            {
                _GraphModel.CreateMeasure(width, height);
                _GraphModel.CreateMeasureLabel();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// create measure model
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void CreateCurrentLine(double width, double height)
        {
            try
            {
                _GraphModel.CreateCurrentLine(width, height);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// create graph
        /// </summary>
        public void CreateGraph(bool isRefresh)
        {
            _GraphModel.CreateGraphs(isRefresh);
        }

        /// <summary>
        /// read graph data
        /// </summary>
        /// <param name="inpData"></param>
        /// <returns></returns>
        public bool ReadData(List<double[]> inpData)
        {
            try
            {
                bool ret = false;
                List<double[]> listdata = inpData;


                int linecount = listdata.Count;
                if (_GraphModel.GraphRawData == null)
                    _GraphModel.GraphRawData = new List<double[]>();

                if (_GraphModel.GraphPlotData == null)
                    _GraphModel.GraphPlotData = new List<double[]>();


                if (linecount >= 0)
                {
                    int maxplot;
                    //when case maxplot > inputdata 
                    if (_GraphModel.MaxDataSizeX > linecount)
                    {
                        if (_GraphModel.GraphRawData.Count + linecount > _GraphModel.MaxDataSizeX)
                        {
                            int diff = (_GraphModel.GraphRawData.Count + linecount) - _GraphModel.MaxDataSizeX;
                            if (diff != 0)
                            {
                                _GraphModel.GraphRawData.RemoveRange(0, diff);
                            }
                            else
                            {
                                _GraphModel.GraphRawData.RemoveRange(0, linecount);
                            }

                        }
                        maxplot = linecount;
                    }
                    //case maxplot = inpdata
                    else
                    {
                        _GraphModel.GraphRawData.Clear();
                        maxplot = _GraphModel.MaxDataSizeX;
                    }

                    for (int i = 0; i < maxplot; i++)
                    {
                        double[] datas = (double[])listdata[i].Clone();
                        _GraphModel.GraphRawData.Add(datas);
                    }


                    if (_GraphModel.GraphMode == GraphMode.Normal)
                    {
                        if (_GraphModel.GraphRawData.Count > 0 && _GraphModel.ShotCount == 1)
                            _GraphModel.GridLineData.MinGridValueX = _GraphModel.GraphRawData[0][0];

                        if (_GraphModel.GridLineData.MinGridValueX == _GraphInfo.MaxValueX - _GraphInfo.MinMaxRangeX && _GraphModel.IncrementX > 1 && _GraphInfo.MinValueX < _GraphInfo.MaxValueX && _GraphModel.PlotCountX == _GraphModel.MaxDataSizeX)
                        {
                            _GraphModel.GridLineData.MaxGridValueX = _GraphInfo.MaxValueX;
                        }
                        else
                        {
                            _GraphModel.GridLineData.MaxGridValueX = (_GraphModel.PlotCountX * _GraphModel.IncrementX) + _GraphModel.GridLineData.MinGridValueX - (_GraphModel.AxisZoomX * _GraphModel.IncrementX);
                        }
                    }
                    else if (_GraphModel.GraphMode == GraphMode.Moving)
                    {
                        if (_GraphModel.GraphRawData.Count >= _GraphModel.PlotCountX)
                        {
                            if (_GraphModel.GraphRawData.Count > 0)
                                _GraphModel.GridLineData.MaxGridValueX = _GraphModel.GraphRawData[_GraphModel.GraphRawData.Count - 1][0];
                            _GraphModel.GridLineData.MinGridValueX = _GraphModel.GridLineData.MaxGridValueX - (_GraphModel.PlotCountX * _GraphModel.IncrementX) + (_GraphModel.AxisZoomX * _GraphModel.IncrementX);
                        }
                        else
                        {
                            if (_GraphModel.GraphRawData.Count > 0)
                                _GraphModel.GridLineData.MinGridValueX = _GraphModel.GraphRawData[0][0];
                            _GraphModel.GridLineData.MaxGridValueX = (_GraphModel.PlotCountX * _GraphModel.IncrementX) + _GraphModel.GridLineData.MinGridValueX;
                        }


                        if (_GraphModel.GridLineData.MinGridValueX < 0)
                            _GraphModel.GridLineData.MinGridValueX = 0;

                    }


                    _GraphModel.GridLineData.MaxGridValueY = _GraphModel.MaxPlotY - _GraphModel.AxisZoomY;
                    _GraphModel.GridLineData.MinGridValueY = _GraphModel.MinPlotY + _GraphModel.AxisZoomY;
                    //_Log4NetClass.ShowInfo("_GraphModel.AxisZoomY" + _GraphModel.AxisZoomY.ToString(), "ReadData");
                    ret = true;
                }

                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// update plot data
        /// </summary>
        public void UpdatePlotData()
        {
            try
            {
                if (_GraphModel.GraphMode == GraphMode.Normal)
                {
                    if (_GraphModel.GridLineData.MinGridValueX == _GraphInfo.MaxValueX - _GraphInfo.MinMaxRangeX && _GraphModel.IncrementX > 1 && _GraphInfo.MinValueX < _GraphInfo.MaxValueX && _GraphModel.PlotCountX == _GraphModel.MaxDataSizeX)
                    {
                        _GraphModel.GridLineData.MaxGridValueX = _GraphInfo.MaxValueX;
                    }
                    else
                    {
                        _GraphModel.GridLineData.MaxGridValueX = (_GraphModel.PlotCountX * _GraphModel.IncrementX) + _GraphModel.GridLineData.MinGridValueX - (_GraphModel.AxisZoomX * _GraphModel.IncrementX);
                    }
                }
                else if (_GraphModel.GraphMode == GraphMode.Moving)
                {
                    if (_GraphModel.GraphRawData.Count >= _GraphModel.PlotCountX)
                    {
                        _GraphModel.GridLineData.MinGridValueX = _GraphModel.GridLineData.MaxGridValueX - (_GraphModel.PlotCountX * _GraphModel.IncrementX) + (_GraphModel.AxisZoomX * _GraphModel.IncrementX);
                    }
                    else
                    {
                        _GraphModel.GridLineData.MaxGridValueX = (_GraphModel.PlotCountX * _GraphModel.IncrementX) + _GraphModel.GridLineData.MinGridValueX;
                    }


                    if (_GraphModel.GridLineData.MinGridValueX < 0)
                        _GraphModel.GridLineData.MinGridValueX = 0;

                }

                _GraphModel.GridLineData.MaxGridValueY = _GraphModel.MaxPlotY - _GraphModel.AxisZoomY;
                _GraphModel.GridLineData.MinGridValueY = _GraphModel.MinPlotY + _GraphModel.AxisZoomY;

                //_Log4NetClass.ShowInfo("_GraphModel.AxisZoomY" + _GraphModel.AxisZoomY.ToString(), "UpdatePlotData");
                //_GraphModel.GraphPlotData.Clear();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }      
        #endregion

        #region Private Function

        #endregion
    }
}
