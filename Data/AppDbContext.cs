using Microsoft.EntityFrameworkCore;
using wt_lab7_7postgres_efcore_antonov.Models;

namespace wt_lab7_7postgres_efcore_antonov.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Car> Cars => Set<Car>();

    // Метод находится ВНУТРИ класса, перед его закрывающей скобкой
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>().HasData(
            new Car { Id = 1, Brand = "Toyota", Model = "Camry", Year = 2021, PricePerDay = 4500, IsAvailable = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Car { Id = 2, Brand = "BMW", Model = "X5", Year = 2020, PricePerDay = 8000, IsAvailable = true, CreatedAt = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}