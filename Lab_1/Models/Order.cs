namespace PhotoStudioApp.Models;

public class Order
{
    public int ID { get; set; }
    public int ClientID { get; set; }
    public DateTime Date { get; set; }
    public DateTime DeliveryDate { get; set; }

    public Client Client { get; set; } = null!;
    public List<OrderDetail> OrderDetails { get; set; } = new();
}