using Engine_Graph_App.Data;

namespace Engine_Graph_App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private AppDatabaseContext _context;
    private DatabaseInit dbInit;
    
    public MainWindowViewModel()
    {
        _context = new AppDatabaseContext();
        dbInit = new DatabaseInit(_context);
        dbInit.PopulateDatabaseWithDummyData();
    }
}