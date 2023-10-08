using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using SkiaSharp;

namespace Engine_Graph_App.ViewModels;

public class ScatterGraphViewModel : ViewModelBase
{
    private ObservableCollection<MeasurementViewModel> _selectedMeasurements;
    //Used HashSet for quick lookups
    private HashSet<MeasurementViewModel> _selectedMeasurementsSet = new HashSet<MeasurementViewModel>();
    private Dictionary<MeasurementViewModel, SKColor> _measurementColors = new Dictionary<MeasurementViewModel, SKColor>();
    private Random _rand = new Random();

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

    public ScatterGraphViewModel()
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

                // Assigning a default X value (like an index or constant value)
                var defaultXValue = 1; // This could be any value or it can be incremented for each point

                // Creating ObservablePoints for each Y value
                var scatterPoints = new List<ObservablePoint>
                {
                    new ObservablePoint(defaultXValue++, measurement.Measurement.Pow),
                    new ObservablePoint(defaultXValue++, measurement.Measurement.TDC),
                    new ObservablePoint(defaultXValue++, measurement.Measurement.Pscv)
                };

                seriesList.Add(new ScatterSeries<ObservablePoint>
                {
                    Values = scatterPoints,
                    Stroke = new SolidColorPaint(color) { StrokeThickness = 1 },
                    Fill = null,  // You can adjust fill/stroke for the scatter points
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
