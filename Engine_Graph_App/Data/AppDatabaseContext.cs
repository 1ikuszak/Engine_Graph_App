using System;
using System.Collections.Generic;
using Engine_Graph_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Engine_Graph_App.Data;

public class AppDatabaseContext:DbContext
{
    public DbSet<Ship> Ships { get; set; }
    public DbSet<Engine> Engines { get; set; }
    public DbSet<Cylinder> Cylinders { get; set; }
    public DbSet<Measurement> Measurements { get; set; }

    public string DbPath { get; }

    public AppDatabaseContext() 
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "engineGraph.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}
