using System;
using Engine_Graph_App.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Engine_Graph_App.ViewModels;

public class EngineViewModel : INotifyPropertyChanged
{
    private Engine _engine;
    private bool _isEngineSelected;

    public EngineViewModel(Engine engine)
    {
        _engine = engine;
    }

    public string EngineName => _engine.EngineName;

    public bool IsEngineSelected
    {
        get => _isEngineSelected;
        set
        {
            if (_isEngineSelected != value)
            {
                _isEngineSelected = value;
                OnPropertyChanged();

                if (_isEngineSelected) 
                {
                    Console.WriteLine($"{_engine.EngineName} is selected");
                } 
                else 
                {
                    Console.WriteLine($"{_engine.EngineName} unselected");
                }
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
