namespace PhotoStudioApp.Models;

public class Service
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public List<OrderDetail> OrderDetails { get; set; } = new();
}