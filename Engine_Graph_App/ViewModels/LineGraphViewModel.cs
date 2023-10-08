using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using ReactiveUI;
using SkiaSharp;

namespace Engine_Graph_App.ViewModels;

public class LineGraphViewModel : ViewModelBase
{
    private ObservableCollection<MeasurementViewModel> _selectedMeasurements;
    //Used HashSet for quick lookups
    private HashSet<MeasurementViewModel> _selectedMeasurementsSet = new HashSet<MeasurementViewModel>();
    private Dictionary<MeasurementViewModel, SKColor> _measurementColors = new Dictionary<MeasurementViewModel, SKColor>();
    private Random _rand = new Random();
    
    private static readonly SKColor s_gray = new(195, 195, 195);
    private static readonly SKColor s_gray1 = new(160, 160, 160);
    private static readonly SKColor s_gray2 = new(90, 90, 90);
    
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

    public ISeries[] Series { get; private set; } = Array.Empty<ISeries>();

    public LineGraphViewModel()
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
                var yValues = new double[] { measurement.Measurement.Pow, measurement.Measurement.TDC, measurement.Measurement.Pscv };

                seriesList.Add(new LineSeries<double>
                {
                    Values = yValues,
                    Stroke = new SolidColorPaint(color) { StrokeThickness = 2 },
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
}
