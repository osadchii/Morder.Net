using Infrastructure.Marketplaces;
using Infrastructure.Models.BotUsers;
using Infrastructure.Models.Companies;
using Infrastructure.Models.Interfaces;
using Infrastructure.Models.MarketplaceCategorySettings;
using Infrastructure.Models.MarketplaceProductSettings;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Orders;
using Infrastructure.Models.Prices;
using Infrastructure.Models.Products;
using Infrastructure.Models.Warehouses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
    public DbSet<Order> Orders { get; set; }

    #endregion

    #region Telegram

    public DbSet<BotUser> BotUsers { get; set; }

    #endregion

    #region Marketplace

    public DbSet<Marketplace> Marketplaces { get; set; }
    public DbSet<MarketplaceCategorySetting> MarketplaceCategorySettings { get; set; }
    public DbSet<MarketplaceProductSetting> MarketplaceProductSettings { get; set; }

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

        modelBuilder.Entity<MarketplaceCategorySetting>(e =>
        {
            e.HasKey(p => new
            {
                p.CategoryId,
                p.MarketplaceId
            });
        });

        modelBuilder.Entity<MarketplaceProductSetting>(e =>
        {
            e.HasKey(p => new
            {
                p.ProductId,
                p.MarketplaceId
            });
        });

        modelBuilder.Entity<Product>(e => { e.HasIndex(p => p.Articul).IsUnique(); });

        modelBuilder.Entity<Order>(e => e.OwnsMany(o => o.Items));

        modelBuilder.Entity<Marketplace>(e =>
        {
            e.Property(p => p.Type)
                .HasConversion(new EnumToStringConverter<MarketplaceType>());
        });

        foreach (IMutableEntityType? entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.GetInterface(nameof(IHasExternalId)) != null)
            {
                IMutableProperty? property = entityType.GetProperty("ExternalId");
                IMutableIndex index = entityType.AddIndex(property, $"UNIQUE_INDEX_ExternalId");
                index.IsUnique = true;
            }

            if (entityType.ClrType.GetInterface(nameof(IHasId)) != null)
                entityType.AddProperty("CreatedAt", typeof(DateTime))
                    .SetDefaultValueSql("now() at time zone 'utc'");
        }

        base.OnModelCreating(modelBuilder);
    }
}