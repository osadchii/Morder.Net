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

    #endregion

    #region Telegram

    public DbSet<BotUser> BotUsers { get; set; }

    public DbSet<BotUserUsageCounter> BotUserUsageCounters { get; set; }

    #endregion

    #region Marketplace

    public DbSet<Marketplace> Marketplaces { get; set; }
    public DbSet<MarketplaceCategorySetting> MarketplaceCategorySettings { get; set; }
    public DbSet<MarketplaceProductSetting> MarketplaceProductSettings { get; set; }
    public DbSet<PriceChange> PriceChanges { get; set; }
    public DbSet<StockChange> StockChanges { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderChange> OrderChanges { get; set; }
    public DbSet<MarketplaceOrderTask> MarketplaceOrderTasks { get; set; }
    public DbSet<OrderSticker> OrderStickers { get; set; }
    public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

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

            e.HasIndex(s => new { s.MarketplaceId, s.ExternalId });
        });

        modelBuilder.Entity<PriceChange>(e =>
        {
            e.HasKey(p => new
            {
                p.MarketplaceId,
                p.ProductId
            });
            e.HasIndex(p => p.MarketplaceId);
        });

        modelBuilder.Entity<StockChange>(e =>
        {
            e.HasKey(p => new
            {
                p.MarketplaceId,
                p.ProductId
            });
            e.HasIndex(p => p.MarketplaceId);
        });

        modelBuilder.Entity<OrderChange>(e => { e.HasKey(o => o.OrderId); });

        modelBuilder.Entity<OrderStatusHistory>(e =>
        {
            e.Property(o => o.Status)
                .HasConversion(new EnumToStringConverter<OrderStatus>());
        });

        modelBuilder.Entity<OrderSticker>(e => { e.HasKey(o => o.OrderId); });

        modelBuilder.Entity<Category>(e =>
        {
            e.HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .IsRequired(false);
        });

        modelBuilder.Entity<Product>(e => { e.HasIndex(p => p.Articul).IsUnique(); });

        modelBuilder.Entity<Order>(e =>
        {
            e.OwnsMany(o => o.Items);
            e.OwnsMany(o => o.Boxes);
            e.HasIndex(o => o.ExternalId).IsUnique();
            e.HasIndex(o => new { o.MarketplaceId, o.Number }).IsUnique();
            e.HasIndex(o => o.MarketplaceId);
            e.Property(o => o.Status)
                .HasConversion(new EnumToStringConverter<OrderStatus>());
        });

        modelBuilder.Entity<Marketplace>(e =>
        {
            e.Property(p => p.Type)
                .HasConversion(new EnumToStringConverter<MarketplaceType>());
        });

        modelBuilder.Entity<MarketplaceOrderTask>(e =>
        {
            e.HasIndex(t => t.MarketplaceId);
            e.Property(t => t.Type)
                .HasConversion(new EnumToStringConverter<TaskType>());
        });

        modelBuilder.Entity<BotUserUsageCounter>(e => { e.HasKey(t => t.BotUserId); });

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