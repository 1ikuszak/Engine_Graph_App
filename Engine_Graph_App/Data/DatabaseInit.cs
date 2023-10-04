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
                var ship1 = new Ship { ShipName = "Ship 1"};
                var ship2 = new Ship { ShipName = "Ship 2" };
                var ship3 = new Ship { ShipName = "Ship 3" };
                
                var engine1 = new Engine { EngineName = "Engine 1"};
                var engine2 = new Engine { EngineName = "Engine 2" };
                var engine3 = new Engine { EngineName = "Engine 3" };
                var engine4 = new Engine { EngineName = "Engine 4" };
                var engine5 = new Engine { EngineName = "Engine 5" };
                var engine6 = new Engine { EngineName = "Engine 6" };
            
                var cylinder1 = new Cylinder
                {
                    Date = DateTime.Now,
                    CylinderName = "A1",
                    Pscv = 1.0,
                    TDC = 3.0,
                    Pow = 4.0
                };

                var cylinder2 = new Cylinder
                {
                    Date = DateTime.Now,
                    CylinderName = "B1",
                    Pscv = 2.0,
                    TDC = 4.0,
                    Pow = 5.0
                };
                
                var cylinder3 = new Cylinder
                {
                    Date = DateTime.Now,
                    CylinderName = "A2",
                    Pscv = 1.0,
                    TDC = 3.0,
                    Pow = 4.0
                };
                
                var cylinder4 = new Cylinder
                {
                    Date = DateTime.Now,
                    CylinderName = "B2",
                    Pscv = 2.0,
                    TDC = 4.0,
                    Pow = 5.0
                };
                
                var cylinder5 = new Cylinder
                {
                    Date = DateTime.Now,
                    CylinderName = "A3",
                    Pscv = 3.0,
                    TDC = 5.0,
                    Pow = 6.0
                };
                
                var cylinder6 = new Cylinder
                {
                    Date = DateTime.Now,
                    CylinderName = "B3",
                    Pscv = 1.5,
                    TDC = 3.5,
                    Pow = 4.5
                };
                
                var cylinder7 = new Cylinder
                {
                    Date = DateTime.Now,
                    CylinderName = "A4",
                    Pscv = 1.5,
                    TDC = 3.5,
                    Pow = 4.5
                };
                
                var cylinder8 = new Cylinder
                {
                    Date = DateTime.Now,
                    CylinderName = "B4",
                    Pscv = 1.5,
                    TDC = 3.5,
                    Pow = 4.5
                };
                                
                var cylinder9 = new Cylinder
                {
                    Date = DateTime.Now,
                    CylinderName = "A5",
                    Pscv = 1.5,
                    TDC = 3.5,
                    Pow = 4.5
                };
                
                var cylinder10 = new Cylinder
                {
                    Date = DateTime.Now,
                    CylinderName = "B5",
                    Pscv = 1.5,
                    TDC = 3.5,
                    Pow = 4.5
                };
                
                var cylinder11 = new Cylinder
                {
                    Date = DateTime.Now,
                    CylinderName = "A6",
                    Pscv = 1.5,
                    TDC = 3.5,
                    Pow = 4.5
                };
                
                var cylinder12 = new Cylinder
                {
                    Date = DateTime.Now,
                    CylinderName = "B6",
                    Pscv = 1.5,
                    TDC = 3.5,
                    Pow = 4.5
                };
                
                ship1.Engines.Add(engine1);
                ship1.Engines.Add(engine2);
                ship2.Engines.Add(engine3);
                ship2.Engines.Add(engine4);
                ship3.Engines.Add(engine5);
                ship3.Engines.Add(engine6);
                
                engine1.Cylinders.Add(cylinder1);
                engine1.Cylinders.Add(cylinder2);
                engine2.Cylinders.Add(cylinder3);
                engine2.Cylinders.Add(cylinder4);
                engine3.Cylinders.Add(cylinder5);
                engine3.Cylinders.Add(cylinder6);
                engine4.Cylinders.Add(cylinder7);
                engine4.Cylinders.Add(cylinder8);
                engine5.Cylinders.Add(cylinder9);
                engine5.Cylinders.Add(cylinder10);
                engine6.Cylinders.Add(cylinder11);
                engine6.Cylinders.Add(cylinder12);

                
                _db.Ships.AddRange(ship1, ship2, ship3);
                _db.Engines.AddRange(engine1, engine2 ,engine3, engine4, engine5, engine6);
                _db.Cylinders.AddRange(cylinder1, cylinder2, cylinder3, cylinder4, cylinder5, cylinder6,
                    cylinder7, cylinder8, cylinder9, cylinder10, cylinder11, cylinder12);
                _db.SaveChanges();   
            }
            
            Console.WriteLine($"Database path: {_db.DbPath}.");
        }
    }