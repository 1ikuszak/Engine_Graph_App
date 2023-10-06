using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;  // Import the necessary namespace for Task
using Engine_Graph_App.Data;
using Engine_Graph_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Engine_Graph_App.ViewModels
{
    public class TreeMenuViewModel : ViewModelBase
    {
        private readonly AppDatabaseContext _db;

        public TreeMenuViewModel(AppDatabaseContext db)
        {
            _db = db;
        }
    
        public async Task<List<Ship>> GetShipsWithEnginesAsync()
        {
            return await _db.Ships
                .Include(ship => ship.Engines)
                .ThenInclude(engine => engine.Cylinders)
                .ThenInclude(cylinder => cylinder.Measurements)
                .ToListAsync();
        }
    }
}