using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Engine_Graph_App.ViewModels;

public class ScatterGraphViewModel : ViewModelBase
{
    public ISeries[] Series { get; set; } 
        = new ISeries[]
        {
            new ScatterSeries<ObservablePoint>
            {
                Values = new ObservablePoint[]
                {
                    new ObservablePoint(0, 2),
                    new ObservablePoint(1, 1),
                    new ObservablePoint(2, 3),
                    new ObservablePoint(3, 5),
                    new ObservablePoint(4, 3),
                    new ObservablePoint(5, 4),
                    new ObservablePoint(6, 6)
                },
                Stroke = new SolidColorPaint(new SKColor(0, 0, 0)) { StrokeThickness = 2 },
                Fill = null,
            }
        };
}