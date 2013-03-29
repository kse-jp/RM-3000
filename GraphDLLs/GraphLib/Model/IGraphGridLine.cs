using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
namespace GraphLib.Model
{   

    interface IGraphGridLine
    {
        //GridTypeEnum GridType { get; set; }
        double LineThick { get; set; }
        double DotSpace { get; set; }
        double MaxGridNoX { get; set; }
        double MaxGridNoY { get; set; }
        double MaxGridValueX { get; set; }
        double MaxGridValueY { get; set; }
        double MinGridValueX { get; set; }
        double MinGridValueY { get; set; }
        Color GridColor { get; set; }
        Thickness Margin { get; set; }
        Shape AxisShapeData { get; }
        Shape GridShapeData { get; }
        void Create(Size graphSize);
        Label AxisLabelX { get; }
        Label AxisLabelY { get; }
        string AxisNameX { get; set; }
        string AxisNameY { get; set; }
        string AxisFontName { get; set; }
        double AxisValuesFontSize { get; set; }
        double AxisNameFontSize { get; set; }
    }
}
