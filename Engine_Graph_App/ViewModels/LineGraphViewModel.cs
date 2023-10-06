using System.Collections.ObjectModel;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using SkiaSharp;

namespace Engine_Graph_App.ViewModels;

public class LineGraphViewModel:ViewModelBase
{
    public ISeries[] Series { get; set; } 
        = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                Stroke = new SolidColorPaint(new SKColor(0, 0, 0)) { StrokeThickness = 2 },
                Fill = null,
                GeometryFill = null,
                GeometryStroke = null
            }
        };
}