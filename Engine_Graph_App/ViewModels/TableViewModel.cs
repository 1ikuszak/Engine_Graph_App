using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;

namespace Engine_Graph_App.ViewModels;

public class TableViewModel : ViewModelBase
{
    private ObservableCollection<MeasurementViewModel> _measurements;
    public ObservableCollection<MeasurementViewModel> Measurements 
    { 
        get => _measurements; 
        set => this.RaiseAndSetIfChanged(ref _measurements, value); 
    }
    
    public TableViewModel()
    {
        Measurements = new ObservableCollection<MeasurementViewModel>();
    }

    public void AddMeasurement(MeasurementViewModel measurement)
    {
        Measurements.Add(measurement);
    }

    public void RemoveMeasurement(MeasurementViewModel measurement)
    {
        Measurements.Remove(measurement);
    }
}