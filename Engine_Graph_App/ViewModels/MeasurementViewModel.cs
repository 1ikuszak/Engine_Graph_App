using Engine_Graph_App.Models;

namespace Engine_Graph_App.ViewModels;

public class MeasurementViewModel
{
    public Measurement Measurement { get; }

    public MeasurementViewModel(Measurement measurement)
    {
        Measurement = measurement;
    }
}