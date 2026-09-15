namespace PhotoStudioApp.Models;

public class OrderDetail
{
    public int ID { get; set; }
    public int OrderID { get; set; }
    public int ServiceID { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public Order Order { get; set; } = null!;
    public Service Service { get; set; } = null!;
}