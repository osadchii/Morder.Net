using Infrastructure.Interfaces;
using Infrastructure.Models.Companies;
using Infrastructure.Models.Prices;
using Infrastructure.Models.Products;
using Infrastructure.Models.Warehouses;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class MContext : DbContext
{
    public MContext(DbContextOptions<MContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Price>(e =>
        {
            e.HasKey(p => new { p.ProductId, p.PriceTypeId });
            e.HasIndex(p => new { p.ProductId, p.PriceTypeId });
        });

        modelBuilder.Entity<Stock>(e =>
        {
            e.HasKey(p => new { p.ProductId, p.WarehouseId });
            e.HasIndex(p => new { p.ProductId, p.WarehouseId });
        });

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.GetInterface(nameof(IHasExternalId)) == null)
            {
                continue;
            }

            var property = entityType.GetProperty("ExternalId");
            entityType.AddIndex(property, "ExternalId");
        }

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.GetInterface(nameof(IHasId)) == null)
            {
                continue;
            }

            entityType.AddProperty("CreatedAt", typeof(DateTime)).SetDefaultValueSql("now() at time zone 'utc'");
        }

        base.OnModelCreating(modelBuilder);
    }

    #region Company

    public DbSet<Product> Products { get; set; }
    public DbSet<PriceType> PriceTypes { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Company> Companies { get; set; }

    #endregion
}