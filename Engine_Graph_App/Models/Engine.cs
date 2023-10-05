using System.Collections.Generic;

namespace Engine_Graph_App.Models;

public class Engine
{
    public int EngineId { get; set; }
    public int ShipId { get; set; }
    public Ship Ship { get; set; }
    public string Name { get; set; }
    public List<Cylinder> Cylinders { get; set; } = new List<Cylinder>();
}