using Microsoft.EntityFrameworkCore;
using PhotoStudioApp.Models;
using System.Reflection.Emit;


namespace PhotoStudioApp.Data;

public class PhotoStudioContext : DbContext
{
    private const string ConnectionString =
        "Server=localhost\\SQLEXPRESS01;Database=Photo studio;Trusted_Connection=True;TrustServerCertificate=True;";

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public DbSet<Service> Services => Set<Service>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>().ToTable("Clients");
        modelBuilder.Entity<Order>().ToTable("Orders");
        modelBuilder.Entity<OrderDetail>().ToTable("OrderDetails");
        modelBuilder.Entity<Service>().ToTable("Services");

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Client)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.ClientID);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderID);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Service)
            .WithMany(s => s.OrderDetails)
            .HasForeignKey(od => od.ServiceID);
    }
}