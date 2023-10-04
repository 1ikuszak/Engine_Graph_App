using System;
using System.Collections.ObjectModel;
using System.Linq;
using Engine_Graph_App.Models;

namespace Engine_Graph_App.ViewModels;

public class ShipViewModel
{
    public Ship Ship { get; }
    public ObservableCollection<EngineViewModel> EngineViewModels { get; }

    public ShipViewModel(Ship ship)
    {
        Ship = ship;
        EngineViewModels = new ObservableCollection<EngineViewModel>();

        foreach (var engine in ship.Engines)
        {
            var engineViewModel = new EngineViewModel(engine);
            EngineViewModels.Add(engineViewModel);
        }
    }
}

