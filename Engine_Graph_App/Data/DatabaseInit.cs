    using System;
    using System.Linq;
    using Engine_Graph_App.Models;

    namespace Engine_Graph_App.Data;

    public class DatabaseInit
    {
        private readonly AppDatabaseContext _db;

        public DatabaseInit(AppDatabaseContext db)
        {
            _db = db;
        }

        public void PopulateDatabaseWithDummyData()
        {
            if (!_db.Ships.Any())
            {
                var engine1 = new Engine { EngineName = "Engine 1"};
                var engine2 = new Engine { EngineName = "Engine 2" };
            
                var ship1 = new Ship { ShipName = "Ship 1"};
                var ship2 = new Ship { ShipName = "Ship 2" };
            
                var cylinder1 = new Cylinder
                {
                    Date = DateTime.Now,
                    CylinderName = "A",
                    Pscv = 1.0,
                    TDC = 3.0,
                    Pow = 4.0
                };

                var cylinder2 = new Cylinder
                {
                    Date = DateTime.Now,
                    CylinderName = "B",
                    Pscv = 2.0,
                    TDC = 4.0,
                    Pow = 5.0
                };
            
                ship1.Engines.Add(engine1);
                ship2.Engines.Add(engine2);
                engine1.Cylinders.Add(cylinder1);
                engine1.Cylinders.Add(cylinder1);
                engine2.Cylinders.Add(cylinder2);

                _db.Ships.AddRange(ship1, ship2);
                _db.Engines.AddRange(engine1, engine2);
                _db.Cylinders.AddRange(cylinder1, cylinder2);
                _db.SaveChanges();   
            }
            
            Console.WriteLine($"Database path: {_db.DbPath}.");
        }
    }