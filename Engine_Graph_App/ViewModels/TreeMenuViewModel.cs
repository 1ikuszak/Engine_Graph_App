using System;
using System.Collections.Generic;
using System.Linq;
using Engine_Graph_App.Data;
using Engine_Graph_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Engine_Graph_App.ViewModels;

public class TreeMenuViewModel:ViewModelBase
{
    private readonly AppDatabaseContext _db;

    public TreeMenuViewModel(AppDatabaseContext db)
    {
        _db = db;
    }
    
    public List<Ship> GetShipsWithEnginesAndCylinders()
    {
        using var db = new AppDatabaseContext();
        var ships = db.Ships
            .Include(ship => ship.Engines)
            .ThenInclude(engine => engine.Cylinders)
            .ToList();

        return ships;
    }
}