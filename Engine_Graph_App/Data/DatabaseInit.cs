using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Engine_Graph_App.Models;

namespace Engine_Graph_App.Data;

public class DatabaseInit
{
    private readonly AppDatabaseContext _db;
    private readonly Random _random = new Random();

    public DatabaseInit(AppDatabaseContext db)
    {
        _db = db;
    }
    
    public List<Point> GenerateRealisticPoints(int count)
    {
        var points = new List<Point>();
        double frequency = 2 * Math.PI / count; // Adjust to change how many "waves" you get
        double amplitude = 500; // Adjust the height of the wave
        double trendSlope = 10; // This will make a linear upward trend. Adjust the slope as necessary.
        double noiseStrength = 100; // Adjust for how "noisy" you want your data

        for (int i = 0; i < count; i++)
        {
            double x = i;
            double baseY = amplitude * Math.Sin(frequency * x);
            double noise = (_random.NextDouble() - 0.5) * 2 * noiseStrength; // Random value between -noiseStrength and +noiseStrength
            double trend = trendSlope * x / count;

            points.Add(new Point
            {
                X = x,
                Y = baseY + noise + trend
            });
        }

        return points;
    }
    
    public async Task PopulateDatabaseWithDummyDataAsync()
    {
        if (!_db.Ships.Any())
        {
            var ship1 = new Ship { Name = "Ship 1"};
            var ship2 = new Ship { Name = "Ship 2" };
            var ship3 = new Ship { Name = "Ship 3" };
            
            var engine1 = new Engine { Name = "Engine 1"};
            var engine2 = new Engine { Name = "Engine 2" };
            var engine3 = new Engine { Name = "Engine 3" };
            var engine4 = new Engine { Name = "Engine 4" };
            var engine5 = new Engine { Name = "Engine 5" };
            var engine6 = new Engine { Name = "Engine 6" };

            var cylinder1 = new Cylinder { Name = "A1" };
            var cylinder2 = new Cylinder { Name = "A2" };
            var cylinder3 = new Cylinder { Name = "A3" };
            var cylinder4 = new Cylinder { Name = "A4" };
            var cylinder5 = new Cylinder { Name = "A5" };
            var cylinder6 = new Cylinder { Name = "A6" };
            var cylinder7 = new Cylinder { Name = "B1" };
            var cylinder8 = new Cylinder { Name = "B2" };
            var cylinder9 = new Cylinder { Name = "B3" };
            var cylinder10 = new Cylinder { Name = "B4" };
            var cylinder11 = new Cylinder { Name = "B5" };
            var cylinder12 = new Cylinder { Name = "B6" };

            var measurement1 = new Measurement()
            {
                Date = DateTime.Now,
                Pscv = 2.0,
                TDC = 4.0,
                Pow = 5.0,
                Points = GenerateRealisticPoints(3600)
            };
            
            var measurement2 = new Measurement()
            {
                Date = DateTime.Now,
                Pscv = 5.0,
                TDC = 18.0,
                Pow = 6.0,
                Points = GenerateRealisticPoints(3600)
            };
            
            var measurement3 = new Measurement
            {
                Date = DateTime.Now,
                Pscv = 12.0,
                TDC = 14.0,
                Pow = 4.0,
                Points = GenerateRealisticPoints(3600)
            };
            
            var measurement4 = new Measurement
            {
                Date = DateTime.Now.AddDays(+1),
                Pscv = 49.0,
                TDC = 21.0,
                Pow = 6.0,
                Points = GenerateRealisticPoints(3600)
            };
            
            var measurement5 = new Measurement
            {
                Date = DateTime.Now.AddDays(+1),
                Pscv = 14.5,
                TDC = 3.5,
                Pow = 44.5,                
                Points = GenerateRealisticPoints(3600)
            };
            
            var measurement6 = new Measurement
            {
                Date = DateTime.Now.AddDays(+1),
                Pscv = 1.5,
                TDC = 13.5,
                Pow = 14.5,
                Points = GenerateRealisticPoints(3600)
            };
            
            var measurement7 = new Measurement()
            {
                Date = DateTime.Now.AddDays(+2),
                Pscv = 11.5,
                TDC = 13.5,
                Pow = 4.5,
                Points = GenerateRealisticPoints(3600)
            };
                            
            var measurement8 = new Measurement()
            {
                Date = DateTime.Now.AddDays(+2),
                Pscv = 1.5,
                TDC = 34.5,
                Pow = 24.5,
                Points = GenerateRealisticPoints(3600)
            };
            
            var measurement9 = new Measurement()
            {
                Date = DateTime.Now.AddDays(+2),
                Pscv = 7.5,
                TDC = 8.5,
                Pow = 4.5,
                Points = GenerateRealisticPoints(3600)
            };
            
            var measurement10 = new Measurement()
            {
                Date = DateTime.Now.AddDays(+3),
                Pscv = 21.5,
                TDC = 9.5,
                Pow = 14.5,
                Points = GenerateRealisticPoints(3600)
            };
            
            var measurement11 = new Measurement()
            {
                Date = DateTime.Now.AddDays(+3),
                Pscv = 12.5,
                TDC = 33.5,
                Pow = 4.5,
                Points = GenerateRealisticPoints(3600)
            };
            
            var measurement12 = new Measurement()
            {
                Date = DateTime.Now.AddDays(+3),
                Pscv = 10.5,
                TDC = 30.5,
                Pow = 4.5,
                Points = GenerateRealisticPoints(3600)
            };
            
            var measurement13 = new Measurement()
            {
                Date = DateTime.Now.AddDays(+3),
                Pscv = 11.5,
                TDC = 18.5,
                Pow = 14.5,
                Points = GenerateRealisticPoints(3600)
            };
            
            var measurement14 = new Measurement()
            {
                Date = DateTime.Now.AddDays(+3),
                Pscv = 14.5,
                TDC = 13.5,
                Pow = 4.5,
                Points = GenerateRealisticPoints(3600)
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
            
            cylinder1.Measurements.Add(measurement1);
            cylinder2.Measurements.Add(measurement2);
            cylinder3.Measurements.Add(measurement3);
            cylinder4.Measurements.Add(measurement4);
            cylinder5.Measurements.Add(measurement5);
            cylinder6.Measurements.Add(measurement5);
            cylinder7.Measurements.Add(measurement6);
            cylinder8.Measurements.Add(measurement7);
            cylinder9.Measurements.Add(measurement8);
            cylinder10.Measurements.Add(measurement9);
            cylinder10.Measurements.Add(measurement10);
            cylinder10.Measurements.Add(measurement11);
            cylinder11.Measurements.Add(measurement12);
            cylinder11.Measurements.Add(measurement13);
            cylinder12.Measurements.Add(measurement14);
            
            await _db.Ships.AddRangeAsync(ship1, ship2, ship3);
            await _db.Engines.AddRangeAsync(engine1, engine2, engine3, engine4, engine5, engine6);
            await _db.Cylinders.AddRangeAsync(cylinder1, cylinder2, cylinder3, cylinder4, cylinder5, cylinder6,
                cylinder7, cylinder8, cylinder9, cylinder10, cylinder11, cylinder12);
            await _db.Measurements.AddRangeAsync(measurement1, measurement2, measurement3, measurement4, measurement5,
                measurement6, measurement7, measurement8, measurement9, measurement10, measurement11, measurement12,
                measurement13, measurement14);
                
            await _db.SaveChangesAsync();
        }
        
        Console.WriteLine($"Database path: {_db.DbPath}.");
    }
}