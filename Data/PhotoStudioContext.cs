using Microsoft.EntityFrameworkCore;
using Lab_3.Models;

namespace Lab_3.Data;

public class PhotoStudioContext : DbContext
{
    public PhotoStudioContext(DbContextOptions<PhotoStudioContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public DbSet<Service> Services => Set<Service>();

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
            .HasForeignKey(od => od.OrderID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Service)
            .WithMany(s => s.OrderDetails)
            .HasForeignKey(od => od.ServiceID);
    }
}