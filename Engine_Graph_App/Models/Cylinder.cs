using System;
using System.Collections.Generic;

namespace Engine_Graph_App.Models;

public class Cylinder
{
    public int CylinderId { get; set; }
    public int EngineId { get; set; }
    public Engine Engine { get; set; }
    public string Name { get; set; }
    public List<Measurement> Measurements { get; set; } = new List<Measurement>();
}