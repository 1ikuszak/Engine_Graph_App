using System.Collections.Generic;

namespace Engine_Graph_App.Models;

public class Ship
{
    public int Id { get; set; }
    public string ShipName { get; set; }
    public List<Engine> Engines { get; set; } = new List<Engine>();
}