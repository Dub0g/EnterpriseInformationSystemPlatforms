using Microsoft.EntityFrameworkCore;

public class Lab_4Context(DbContextOptions<Lab_4Context> options) : DbContext(options)
{
    public DbSet<Lab_4.Models.Client> Client { get; set; } = default!;
    public DbSet<Lab_4.Models.Services> Services { get; set; } = default!;
    public DbSet<Lab_4.Models.OrderDetails> OrderDetails { get; set; } = default!;
    public DbSet<Lab_4.Models.Orders> Orders { get; set; } = default!;
}
