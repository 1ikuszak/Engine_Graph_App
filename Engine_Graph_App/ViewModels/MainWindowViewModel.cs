using System;
using System.Collections.ObjectModel;
using System.Linq;
using Engine_Graph_App.Data;

namespace Engine_Graph_App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private AppDatabaseContext _context;
        private DatabaseInit dbInit;

        public ObservableCollection<CylinderViewModel> SelectedCylinders { get; } = new ObservableCollection<CylinderViewModel>();
        public ObservableCollection<MeasurementViewModel> SelectedCylindersMeasurements { get; } = new ObservableCollection<MeasurementViewModel>();
        public ObservableCollection<ShipViewModel> Ships { get; } = new ObservableCollection<ShipViewModel>();
        public TreeMenuViewModel TreeMenuViewModel { get; }
        
        public MainWindowViewModel()
        {
            _context = new AppDatabaseContext();
            dbInit = new DatabaseInit(_context);
            dbInit.PopulateDatabaseWithDummyData();
            TreeMenuViewModel = new TreeMenuViewModel(_context);
            LoadShips();
        }

        public void LoadShips()
        {
            var ships = TreeMenuViewModel.GetShipsWithEngines();
            foreach (var ship in ships)
            {
                var shipViewModel = new ShipViewModel(ship);
                Ships.Add(shipViewModel);

                foreach (var engineViewModel in shipViewModel.EngineViewModels)
                {
                    engineViewModel.EngineSelectedChanged += HandleEngineSelectionChanged;
                }
            }

        }

        private void HandleEngineSelectionChanged(object sender, bool isSelected)
        {
            var engineVm = sender as EngineViewModel;

            if (isSelected)
            {
                foreach (var cylinder in engineVm.Cylinders)
                {
                    foreach (var measurement in cylinder.Measurements)
                    {
                        SelectedCylindersMeasurements.Add(new MeasurementViewModel(measurement));
                    }
                }
            }
            else
            {
                var toRemove = SelectedCylindersMeasurements
                    .Where(mv => engineVm.Cylinders.Any(c => c.Measurements.Contains(mv.Measurement)))
                    .ToList();

                foreach (var item in toRemove)
                {
                    SelectedCylindersMeasurements.Remove(item);
                }
            }
        }
    }
    
}

