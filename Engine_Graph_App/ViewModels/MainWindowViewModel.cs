using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using Engine_Graph_App.Data;
using ReactiveUI;

namespace Engine_Graph_App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // DB
        private readonly AppDatabaseContext _context;
        private readonly DatabaseInit _dbInit;
        public DataAccessRepository DataAccessRepository { get; }
        
        // Collections
        public ObservableCollection<object> TreeViewData { get; } = new ObservableCollection<object>();
        public ObservableCollection<ShipViewModel> Ships { get; } = new ObservableCollection<ShipViewModel>();
        private List<EngineViewModel> _engines;
        public List<EngineViewModel> Engines
        {
            get => _engines;
            set => this.RaiseAndSetIfChanged(ref _engines, value); 
        }        
        
        public ObservableCollection<EngineDataSheetViewModel> EngineDataSheets { get; } = new ObservableCollection<EngineDataSheetViewModel>();
        
        // ViewModels
        public TreeMenuViewModel TreeMenuViewModel { get; }
        public MeasurementViewModel MeasurementViewModel { get; }
        public TableViewModel SharedTableViewModel { get; } = new TableViewModel();
        public DefaultViewModel DefaultViewModel { get; } = new DefaultViewModel();
        public LineGraphViewModel LineGraphViewModel { get; } = new LineGraphViewModel();
        public ScatterGraphViewModel ScatterGraphViewModel { get; } = new ScatterGraphViewModel();
        public BigDataGraphViewModel BigDataGraphViewModel { get; } = new BigDataGraphViewModel();
        
        // Content Presenter
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
            DataAccessRepository = new DataAccessRepository(_context);
            
            _dbInit.PopulateDatabaseWithDummyDataAsync();
            LoadShipsAsync().Wait();
            
            TreeMenuViewModel = new TreeMenuViewModel(Ships, Engines);
            ContentToDisplay = DefaultViewModel; // Set ContentToDisplay to DefaultViewModel
        }
        
        public async Task LoadShipsAsync()
        {
            var shipsFromDb = await DataAccessRepository.GetShipsWithEnginesAsync();

            // Convert ships from DB into view models
            var shipViewModels = shipsFromDb.Select(s => new ShipViewModel(s)).ToList();
    
            Ships.AddRange(shipViewModels);
            _engines = shipViewModels.SelectMany(shipVm => shipVm.EngineViewModels).ToList();

            // Subscribe to the EngineSelectedChanged event
            foreach (var engineVm in _engines)
            {
                engineVm.EngineSelectedChanged += HandleEngineSelectionChanged;
            }
        }
        
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
                var newEngineDetailsViewModel = new EngineDataSheetViewModel(MeasurementViewModel, newSelectedCylindersMeasurements, SharedTableViewModel, LineGraphViewModel, ScatterGraphViewModel, BigDataGraphViewModel)
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
                        LineGraphViewModel.RemoveMeasurement(measurementVm);
                        ScatterGraphViewModel.RemoveMeasurement(measurementVm); 
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
        
        public void BigDataGraph()
        {
            ContentToDisplay = BigDataGraphViewModel;
        }
    
        public void ShowTable()
        {
            ContentToDisplay = SharedTableViewModel;
        }
    }
}
