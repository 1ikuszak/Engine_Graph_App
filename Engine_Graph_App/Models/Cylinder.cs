using System;

namespace Engine_Graph_App.Models;

public class Cylinder
{
    public int CylinderId { get; set; }
    public DateTime Date { get; set; }
    public int EngineId { get; set; }
    public Engine Engine { get; set; }
    public string CylinderName { get; set; }
    public double Pscv { get; set; }
    public double TDC { get; set; }
    public double Pow { get; set; }
}