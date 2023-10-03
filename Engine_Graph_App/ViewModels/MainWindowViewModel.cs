using System.Collections.Generic;
using System.Collections.ObjectModel;
using Engine_Graph_App.Data;
using Engine_Graph_App.Models;
using Engine_Graph_App.Views;

namespace Engine_Graph_App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private AppDatabaseContext _context;
    private DatabaseInit dbInit;
    private TreeMenuView treeMenuView;
    
    
    public MainWindowViewModel()
    {
        // DB
        _context = new AppDatabaseContext();
        dbInit = new DatabaseInit(_context);
        dbInit.PopulateDatabaseWithDummyData();

        // Tree
        TreeMenuViewModel = new TreeMenuViewModel(_context);
        Ships = new ObservableCollection<Ship>(TreeMenuViewModel.GetShipsWithEnginesAndCylinders());
    }
    
    public ObservableCollection<Ship> Ships { get; }
    public TreeMenuViewModel TreeMenuViewModel { get; }
}