using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace GraphLib.Model
{
    public enum GraphTypeEnum
    {
        Line =0,
        Curve =1,
        Dot =2
    }

    interface IGraphModel
    {
        Size GraphSize { get; set; }        
        GraphTypeEnum GraphType { get; set; }
        GraphGridLine GridLineData { get; set; }
        List<double[]> GraphRawData { get; set; }
        List<double[]>GraphPlotData { get; set; }
        Color[] GraphColor { get; set; }
        int MaxGraphNo { get; set; }
        Color GraphBackgroundColor { get; set; }
        double MinZoom { get; set; }
        double MaxZoom { get; set; }
        double[] GraphLineSize { get; set; }
        void CreateGrid();

    }
}
