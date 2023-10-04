using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Engine_Graph_App.Data;
using Engine_Graph_App.Models;
using Engine_Graph_App.Views;
using ReactiveUI;

namespace Engine_Graph_App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private AppDatabaseContext _context;
        private DatabaseInit dbInit;

        public MainWindowViewModel()
        {
            // DB
            _context = new AppDatabaseContext();
            dbInit = new DatabaseInit(_context);
            dbInit.PopulateDatabaseWithDummyData();

            // Tree Menu
            TreeMenuViewModel = new TreeMenuViewModel(_context);
            Cylinders = new ObservableCollection<Cylinder>(TreeMenuViewModel.GetCylinders());
            LoadShips();
        }
        
        public void LoadShips()
        {
            var ships = TreeMenuViewModel.GetShipsWithEngines();
            foreach (var ship in ships)
            {
                var shipViewModel = new ShipViewModel(ship);
                Ships.Add(shipViewModel);
            }
        }
        
        
        public ObservableCollection<ShipViewModel> Ships { get; private set; } = new ObservableCollection<ShipViewModel>();
        public ReactiveCommand<object, Unit> CheckBoxCheckedCommand { get; }
        public ReactiveCommand<object, Unit> CheckBoxUncheckedCommand { get; }
        public ObservableCollection<Engine> checkedEngines { get; set; } = new ObservableCollection<Engine>();
        public ObservableCollection<Cylinder> Cylinders { get; }
        public TreeMenuViewModel TreeMenuViewModel { get; }
    }
}
