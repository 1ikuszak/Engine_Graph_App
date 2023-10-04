using System;
using Engine_Graph_App.Models;

namespace Engine_Graph_App.ViewModels;

public class CylinderViewModel
{
    public Cylinder Cylinder { get; }

    public CylinderViewModel(Cylinder cylinder)
    {
        Cylinder = cylinder;
    }
    
}
