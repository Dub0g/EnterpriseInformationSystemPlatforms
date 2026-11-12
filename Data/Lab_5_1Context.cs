using Microsoft.EntityFrameworkCore;

public class Lab_5_1Context(DbContextOptions<Lab_5_1Context> options) : DbContext(options)
{
    public DbSet<Lab_5.Models.Orders> Orders { get; set; } = default!;
    public DbSet<Lab_5.Models.Clients> Clients { get; set; } = default!; 
    public DbSet<Lab_5.Models.Services> Services { get; set; } = default!;
    //public DbSet<Lab_5.Models.OrdersDetails> OrdersDetails { get; set; } = default!;



}
