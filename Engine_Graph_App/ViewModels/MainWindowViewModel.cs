using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Engine_Graph_App.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public DefaultViewModel DefaultViewModel { get; } = new DefaultViewModel();

        public LineGraphViewModel LineGraphViewModel { get; } = new LineGraphViewModel();
        public ScatterGraphViewModel ScatterGraphViewModel { get; } = new ScatterGraphViewModel();
        
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
            _dbInit.PopulateDatabaseWithDummyDataAsync();
            TreeMenuViewModel = new TreeMenuViewModel(_context);
            LoadShipsAsync();
            
            ContentToDisplay = DefaultViewModel; // Set ContentToDisplay to SharedTableViewModel
        }

        // Load ships and subscribe to engine selection changes
        public async Task LoadShipsAsync()
        {
            var ships = await TreeMenuViewModel.GetShipsWithEnginesAsync();
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
        private async void HandleEngineSelectionChanged(object sender, bool isSelected)
        {
            var engineVm = sender as EngineViewModel;

            if (engineVm == null)
                return;

            if (isSelected)
            {
                var newSelectedCylindersMeasurements = await Task.Run(() => 
                {
                    var measurements = new ObservableCollection<MeasurementViewModel>();
                    foreach (var cylinder in engineVm.Cylinders)
                    {
                        foreach (var measurement in cylinder.Measurements)
                        {
                            measurements.Add(new MeasurementViewModel(measurement));
                        }
                    }
                    return measurements;
                });

                if (_contentToDisplay == DefaultViewModel)
                {
                    ContentToDisplay = SharedTableViewModel;
                }

                // Pass the shared TableViewModel instance when creating a new EngineDataSheetViewModel
                var newEngineDetailsViewModel = new EngineDataSheetViewModel(MeasurementViewModel, newSelectedCylindersMeasurements, SharedTableViewModel, LineGraphViewModel, ScatterGraphViewModel)
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
                    // After removing, check if no engines are selected, if so, set to default view
                    if (!EngineDataSheets.Any())
                    {
                        ContentToDisplay = DefaultViewModel;
                    }
                }
            }
        }
        
        public void ShowLineGraph()
        {
            ContentToDisplay = LineGraphViewModel;
        }

        public void ShowScatterGraph()
        {
            ContentToDisplay = ScatterGraphViewModel;
        }
    
        public void ShowTable()
        {
            ContentToDisplay = SharedTableViewModel;
        }
    }
}
