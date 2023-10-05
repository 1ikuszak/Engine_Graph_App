using System.Collections.ObjectModel;
using System.Linq;
using Engine_Graph_App.Data;

namespace Engine_Graph_App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly AppDatabaseContext _context;
        private readonly DatabaseInit _dbInit;
        
        public ObservableCollection<ShipViewModel> Ships { get; } = new ObservableCollection<ShipViewModel>();
        public ObservableCollection<EngineDetailsViewModel> EngineDetails { get; } = new ObservableCollection<EngineDetailsViewModel>();

        public TreeMenuViewModel TreeMenuViewModel { get; }
        public MeasurementViewModel MeasurementViewModel { get; }

        public MainWindowViewModel()
        {
            _context = new AppDatabaseContext();
            _dbInit = new DatabaseInit(_context);
            _dbInit.PopulateDatabaseWithDummyData();
            TreeMenuViewModel = new TreeMenuViewModel(_context);
            LoadShips();
        }

        // Load ships and subscribe to engine selection changes
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

        // Handles engine selection changes by either adding or removing engine details
        private void HandleEngineSelectionChanged(object sender, bool isSelected)
        {
            var engineVm = sender as EngineViewModel;
            if (isSelected)
            {
                var newSelectedCylindersMeasurements = new ObservableCollection<MeasurementViewModel>();
                foreach (var cylinder in engineVm.Cylinders)
                {
                    foreach (var measurement in cylinder.Measurements)
                    {
                        newSelectedCylindersMeasurements.Add(new MeasurementViewModel(measurement));
                    }
                }

                var newEngineDetailsViewModel = new EngineDetailsViewModel(MeasurementViewModel, newSelectedCylindersMeasurements)
                {
                    EngineId = engineVm.Engine.EngineId,
                    EngineName = engineVm.Engine.Name
                };
                EngineDetails.Add(newEngineDetailsViewModel);
            }
            else
            {
                var engineDetailToRemove = EngineDetails.FirstOrDefault(e => e.EngineId == engineVm.Engine.EngineId);
                if (engineDetailToRemove != null)
                {
                    EngineDetails.Remove(engineDetailToRemove);
                }
            }
        }
    }
}
