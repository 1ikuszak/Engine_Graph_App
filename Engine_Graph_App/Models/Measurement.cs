using System;
using System.Collections.Generic;

namespace Engine_Graph_App.Models;

public class Measurement
{
    public int Id { get; set; }
    public int CylinderId { get; set; }
    public Cylinder Cylinder { get; set; }
    public DateTime Date { get; set; }
    public double Pscv { get; set; }
    public double TDC { get; set; }
    public double Pow { get; set; }
}