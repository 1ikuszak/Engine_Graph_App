using System.Collections.ObjectModel;
using Engine_Graph_App.Models;

namespace Engine_Graph_App.ViewModels;

public class ShipViewModel
{
    public Ship Ship { get; }
    public ObservableCollection<EngineViewModel> EngineViewModels { get; } = new ObservableCollection<EngineViewModel>();

    public ShipViewModel(Ship ship)
    {
        Ship = ship;
        foreach (var engine in ship.Engines)
        {
            EngineViewModels.Add(new EngineViewModel(engine));
        }
    }
}
