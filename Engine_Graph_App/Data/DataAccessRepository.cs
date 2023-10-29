using System.Collections.Generic;
using System.Threading.Tasks;
using Engine_Graph_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Engine_Graph_App.Data;

public class DataAccessRepository
{
    private readonly AppDatabaseContext _db;

    public DataAccessRepository(AppDatabaseContext db)
    {
        _db = db;
    }
    
    public async Task<List<Ship>> GetShipsWithEnginesAsync()
    {
        return await _db.Ships
            .Include(ship => ship.Engines)
            .ThenInclude(engine => engine.Cylinders)
            .ThenInclude(cylinder => cylinder.Measurements)
            .ThenInclude(measurement => measurement.Points )
            .ToListAsync();
    }
}