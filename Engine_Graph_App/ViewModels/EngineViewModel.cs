using System;
using System.Collections.Generic;
using Engine_Graph_App.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Engine_Graph_App.ViewModels;

public class EngineViewModel
{
    public Engine Engine { get; }
    private bool _isSelected;
    
    public event EventHandler<bool> EngineSelectedChanged;

    public EngineViewModel(Engine engine)
    {
        Engine = engine;
    }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                EngineSelectedChanged?.Invoke(this, value);
            }
        }
    }

    public IEnumerable<Cylinder> Cylinders => Engine.Cylinders;
}

