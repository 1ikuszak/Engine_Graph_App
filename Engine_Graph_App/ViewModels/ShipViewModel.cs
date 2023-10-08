using System;
using System.Collections.ObjectModel;
using System.Linq;
using Engine_Graph_App.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Engine_Graph_App.ViewModels
{
    public class ShipViewModel : INotifyPropertyChanged
    {
        public Ship Ship { get; }
        public ObservableCollection<EngineViewModel> EngineViewModels { get; }
        private bool _hasCheckedEngine;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool HasCheckedEngine
        {
            get => _hasCheckedEngine;
            private set
            {
                if (_hasCheckedEngine != value)
                {
                    _hasCheckedEngine = value;
                    OnPropertyChanged();
                }
            }
        }

        public ShipViewModel(Ship ship)
        {
            Ship = ship;
            EngineViewModels = new ObservableCollection<EngineViewModel>();

            foreach (var engine in ship.Engines)
            {
                var engineViewModel = new EngineViewModel(engine);
                engineViewModel.EngineSelectedChanged += (sender, isSelected) => 
                {
                    HasCheckedEngine = EngineViewModels.Any(evm => evm.IsSelected);
                };

                EngineViewModels.Add(engineViewModel);
            }

            // Initialize HasCheckedEngine value.
            HasCheckedEngine = EngineViewModels.Any(evm => evm.IsSelected);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}