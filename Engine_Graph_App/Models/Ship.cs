using System.Collections.Generic;

namespace Engine_Graph_App.Models;

public class Ship
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Engine> Engines { get; set; } = new List<Engine>();
}