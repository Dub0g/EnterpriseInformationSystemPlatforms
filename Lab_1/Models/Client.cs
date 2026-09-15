namespace PhotoStudioApp.Models;

public class Client
{
    public int ID { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }

    public List<Order> Orders { get; set; } = new();
}