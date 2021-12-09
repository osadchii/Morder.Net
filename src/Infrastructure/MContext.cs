using Infrastructure.Models.BotUsers;
using Infrastructure.Models.Companies;
using Infrastructure.Models.Interfaces;
using Infrastructure.Models.Prices;
using Infrastructure.Models.Products;
using Infrastructure.Models.Warehouses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure;

public class MContext : DbContext
{
    #region Company

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<PriceType> PriceTypes { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Company> Companies { get; set; }

    #endregion

    #region Telegram

    public DbSet<BotUser> BotUsers { get; set; }

    #endregion

#pragma warning disable CS8618
    public MContext(DbContextOptions<MContext> options) : base(options)
#pragma warning restore CS8618
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Price>(e => { e.HasKey(p => new { p.ProductId, p.PriceTypeId }); });

        modelBuilder.Entity<Stock>(e => { e.HasKey(p => new { p.ProductId, p.WarehouseId }); });

        modelBuilder.Entity<Product>(e => { e.HasIndex(p => p.Articul).IsUnique(); });

        foreach (IMutableEntityType? entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.GetInterface(nameof(IHasExternalId)) != null)
            {
                IMutableProperty? property = entityType.GetProperty("ExternalId");
                IMutableIndex index = entityType.AddIndex(property, "ExternalId");
                index.IsUnique = true;
            }

            if (entityType.ClrType.GetInterface(nameof(IHasId)) != null)
                entityType.AddProperty("CreatedAt", typeof(DateTime))
                    .SetDefaultValueSql("now() at time zone 'utc'");
        }

        base.OnModelCreating(modelBuilder);
    }
}