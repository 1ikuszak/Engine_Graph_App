using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using ReactiveUI;
using SkiaSharp;

namespace Engine_Graph_App.ViewModels;

public class BigDataGraphViewModel : ViewModelBase
{
    private ObservableCollection<MeasurementViewModel> _selectedMeasurements;
    //Used HashSet for quick lookups
    private HashSet<MeasurementViewModel> _selectedMeasurementsSet = new HashSet<MeasurementViewModel>();
    private Dictionary<MeasurementViewModel, SKColor> _measurementColors = new Dictionary<MeasurementViewModel, SKColor>();
    private Random _rand = new Random();
    
    private static readonly SKColor s_gray = new(195, 195, 195);
    private static readonly SKColor s_gray1 = new(240, 240, 240);
    private static readonly SKColor s_gray2 = new(170, 170, 170);
    
    public ISeries[] Series { get; private set; } = Array.Empty<ISeries>();
    
    public SolidColorPaint LegendTextPaint { get; set; } = 
        new SolidColorPaint 
        { 
            Color = new SKColor(50, 50, 50),
        }; 
    
    public Axis[] XAxes { get; set; } =
    {
        new Axis
        {
            NamePaint = new SolidColorPaint(s_gray1),
            TextSize = 12,
            Padding = new Padding(5, 15, 5, 5),
            LabelsPaint = new SolidColorPaint(s_gray),
            SeparatorsPaint = new SolidColorPaint
            {
                Color = s_gray,
                StrokeThickness = 1,
                PathEffect = new DashEffect(new float[] { 3, 3 })
            },
            SubseparatorsPaint = new SolidColorPaint
            {
                Color = s_gray2,
                StrokeThickness = 0.5f
            },
        }
    };

    public Axis[] YAxes { get; set; } =
    {
        new Axis
        {
            NamePaint = new SolidColorPaint(s_gray1),
            TextSize = 12,
            Padding = new Padding(5, 0, 15, 0),
            LabelsPaint = new SolidColorPaint(s_gray),
            SeparatorsPaint = new SolidColorPaint
            {
                Color = s_gray,
                StrokeThickness = 1,
                PathEffect = new DashEffect(new float[] { 3, 3 })
            },
            SubseparatorsPaint = new SolidColorPaint
            {
                Color = s_gray2,
                StrokeThickness = 0.5f
            }, 
        }
    };
    
    public ObservableCollection<MeasurementViewModel> SelectedMeasurements
    {
        get => _selectedMeasurements;
        set
        {
            if (_selectedMeasurements != value)
            {
                if (_selectedMeasurements != null)
                {
                    _selectedMeasurements.CollectionChanged -= OnSelectedMeasurementsChanged;
                }

                _selectedMeasurements = value;
                _selectedMeasurementsSet = new HashSet<MeasurementViewModel>(_selectedMeasurements);
                _selectedMeasurements.CollectionChanged += OnSelectedMeasurementsChanged;
                UpdateSeriesAsync();
            }
        }
    }
    
    public BigDataGraphViewModel()
    {
        _selectedMeasurements = new ObservableCollection<MeasurementViewModel>();
        _selectedMeasurements.CollectionChanged += OnSelectedMeasurementsChanged;
    }

    private void OnSelectedMeasurementsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (MeasurementViewModel item in e.NewItems)
            {
                _selectedMeasurementsSet.Add(item);
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (MeasurementViewModel item in e.OldItems)
            {
                _selectedMeasurementsSet.Remove(item);
            }
        }

        UpdateSeriesAsync();
    }

    public void AddMeasurement(MeasurementViewModel measurement)
    {
        if (!_selectedMeasurementsSet.Contains(measurement))
        {
            _measurementColors[measurement] = GetRandomColor();
            _selectedMeasurements.Add(measurement);
        }
    }

    public void RemoveMeasurement(MeasurementViewModel measurement)
    {
        if (_selectedMeasurementsSet.Contains(measurement))
        {
            _selectedMeasurements.Remove(measurement);
            _measurementColors.Remove(measurement);
        }
    }

    private async Task UpdateSeriesAsync()
    {
        await Task.Run(() => 
        {
            var seriesList = new List<ISeries>(_selectedMeasurements.Count);
    
            foreach (var measurement in _selectedMeasurements)
            {
                if (!_measurementColors.TryGetValue(measurement, out var color)) continue;

                // Extract the Points data and transform it into X & Y values for the LineSeries
                var pointValues = measurement.Measurement.Points;
                // Convert pointValues (List<Point>) to a list of ObservablePoint
                var observablePoints = pointValues.Select(p => new ObservablePoint(p.X, p.Y)).ToList();

                // Then, apply the Douglas-Peucker algorithm
                var tolerance = 20; // You can adjust this value to get the desired reduction level
                var reducedPoints = DouglasPeuckerReduction(observablePoints, tolerance);

                // Since reducedPoints is already a list of ObservablePoint, you can assign it directly
                var xyValues = reducedPoints.ToArray();

                seriesList.Add(new LineSeries<ObservablePoint>
                {
                    Values = xyValues,
                    Stroke = new SolidColorPaint(color) { StrokeThickness = 1 },
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    Name = $"{measurement.Measurement.Cylinder.Engine.Name} {measurement.Measurement.Cylinder.Name} {measurement.Measurement.Date:MM/dd/yyyy}"
                });
            }
            Series = seriesList.ToArray();
        });
    
        ((IReactiveObject)this).RaisePropertyChanged(nameof(Series));
    }

    private SKColor GetRandomColor()
    {
        return new SKColor((byte)_rand.Next(256), (byte)_rand.Next(256), (byte)_rand.Next(256));
    }
    
    private List<ObservablePoint> DouglasPeuckerReduction(List<ObservablePoint> points, double tolerance)
{
    if (points == null || points.Count < 3) return points;

    int firstPoint = 0;
    int lastPoint = points.Count - 1;
    var pointIndexsToKeep = new List<int>();

    pointIndexsToKeep.Add(firstPoint);
    pointIndexsToKeep.Add(lastPoint);

    DouglasPeuckerReduction(points, firstPoint, lastPoint, tolerance, ref pointIndexsToKeep);

    var returnPoints = new List<ObservablePoint>();
    pointIndexsToKeep.Sort();
    foreach (int index in pointIndexsToKeep)
    {
        returnPoints.Add(points[index]);
    }
    return returnPoints;
}

private void DouglasPeuckerReduction(List<ObservablePoint> points, int firstPoint, int lastPoint, double tolerance, ref List<int> pointIndexsToKeep)
{
    double maxDistance = 0;
    int indexFarthest = 0;

    for (int index = firstPoint; index < lastPoint; index++)
    {
        double distance = PerpendicularDistance(points[index], points[firstPoint], points[lastPoint]);
        if (distance > maxDistance)
        {
            maxDistance = distance;
            indexFarthest = index;
        }
    }

    if (maxDistance > tolerance && indexFarthest != 0)
    {
        pointIndexsToKeep.Add(indexFarthest);
        DouglasPeuckerReduction(points, firstPoint, indexFarthest, tolerance, ref pointIndexsToKeep);
        DouglasPeuckerReduction(points, indexFarthest, lastPoint, tolerance, ref pointIndexsToKeep);
    }
}

private double PerpendicularDistance(ObservablePoint point, ObservablePoint lineStart, ObservablePoint lineEnd)
{
    if (!point.X.HasValue || !point.Y.HasValue ||
        !lineStart.X.HasValue || !lineStart.Y.HasValue ||
        !lineEnd.X.HasValue || !lineEnd.Y.HasValue)
    {
        throw new InvalidOperationException("One or more points do not have a valid X or Y value.");
    }

    double areaValue = 0.5 * (lineStart.X.Value * lineEnd.Y.Value + lineEnd.X.Value * point.Y.Value +
                              point.X.Value * lineStart.Y.Value - lineEnd.X.Value * lineStart.Y.Value -
                              point.X.Value * lineEnd.Y.Value - lineStart.X.Value * point.Y.Value);
    
    double area = Math.Abs(areaValue);

    double bottom = Math.Sqrt(Math.Pow(lineStart.X.Value - lineEnd.X.Value, 2) +
                              Math.Pow(lineStart.Y.Value - lineEnd.Y.Value, 2));
    
    return (area / bottom * 2);
}



}
