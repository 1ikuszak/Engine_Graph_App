using System.Collections.ObjectModel;
using System.Linq;
using Engine_Graph_App.Data;
using ReactiveUI;

namespace Engine_Graph_App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly AppDatabaseContext _context;
        private readonly DatabaseInit _dbInit;
        
        public ObservableCollection<ShipViewModel> Ships { get; } = new ObservableCollection<ShipViewModel>();
        public ObservableCollection<EngineDataSheetViewModel> EngineDataSheets { get; } = new ObservableCollection<EngineDataSheetViewModel>();

        public TreeMenuViewModel TreeMenuViewModel { get; }
        public MeasurementViewModel MeasurementViewModel { get; }
        
        public TableViewModel SharedTableViewModel { get; } = new TableViewModel();
        
        private ViewModelBase _contentToDisplay;
        public ViewModelBase ContentToDisplay
        {
            get => _contentToDisplay;
            set => this.RaiseAndSetIfChanged(ref _contentToDisplay, value);
        }

        public MainWindowViewModel()
        {
            _context = new AppDatabaseContext();
            _dbInit = new DatabaseInit(_context);
            _dbInit.PopulateDatabaseWithDummyData();
            TreeMenuViewModel = new TreeMenuViewModel(_context);
            LoadShips();
            
            ContentToDisplay = SharedTableViewModel; // Set ContentToDisplay to SharedTableViewModel
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

                // Pass the shared TableViewModel instance when creating a new EngineDataSheetViewModel
                var newEngineDetailsViewModel = new EngineDataSheetViewModel(MeasurementViewModel, newSelectedCylindersMeasurements, SharedTableViewModel)
                {
                    EngineId = engineVm.Engine.EngineId,
                    EngineName = engineVm.Engine.Name
                };
                EngineDataSheets.Add(newEngineDetailsViewModel);
            }
            else
            {
                var engineDetailToRemove = EngineDataSheets.FirstOrDefault(e => e.EngineId == engineVm.Engine.EngineId);
                if (engineDetailToRemove != null)
                {
                    // Before removing the engineDetailToRemove from the EngineDataSheets,
                    // remove all its measurements from the shared TableViewModel
                    foreach (var measurementVm in engineDetailToRemove.SelectedCylindersMeasurements)
                    {
                        SharedTableViewModel.RemoveMeasurement(measurementVm);
                    }
                    EngineDataSheets.Remove(engineDetailToRemove);
                }
            }
        }
    }
}
