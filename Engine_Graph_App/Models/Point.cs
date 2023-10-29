namespace Engine_Graph_App.Models;

public class Point
{
    public int Id { get; set; }
    public int MeasurementId { get; set; }
    public Measurement Measurement { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
}