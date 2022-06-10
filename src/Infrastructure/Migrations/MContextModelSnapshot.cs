﻿// <auto-generated />
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(MContext))]
    partial class MContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Infrastructure.Models.BotUsers.BotUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Administrator")
                        .HasColumnType("boolean");

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("CurrentState")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CurrentStateKey")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("UserName")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<bool>("Verified")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("BotUser", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.BotUsers.BotUserUsageCounter", b =>
                {
                    b.Property<int>("BotUserId")
                        .HasColumnType("integer");

                    b.Property<long>("Count")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("LastUse")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("BotUserId");

                    b.ToTable("BotUserUsageCounter", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.Companies.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int?>("PriceTypeId")
                        .HasColumnType("integer");

                    b.Property<string>("Shop")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("PriceTypeId");

                    b.ToTable("Company", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.MarketplaceCategorySettings.MarketplaceCategorySetting", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<int>("MarketplaceId")
                        .HasColumnType("integer");

                    b.Property<bool>("Blocked")
                        .HasColumnType("boolean");

                    b.HasKey("CategoryId", "MarketplaceId");

                    b.HasIndex("MarketplaceId");

                    b.ToTable("MarketplaceCategorySetting", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.MarketplaceProductSettings.MarketplaceProductSetting", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("MarketplaceId")
                        .HasColumnType("integer");

                    b.Property<string>("ExternalId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<bool>("IgnoreRestrictions")
                        .HasColumnType("boolean");

                    b.Property<bool>("NullifyStock")
                        .HasColumnType("boolean");

                    b.HasKey("ProductId", "MarketplaceId");

                    b.HasIndex("MarketplaceId", "ExternalId");

                    b.ToTable("MarketplaceProductSetting", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.Marketplaces.Marketplace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<decimal>("MinimalPrice")
                        .HasColumnType("numeric");

                    b.Property<decimal>("MinimalStock")
                        .HasColumnType("numeric");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<bool>("NullifyStocks")
                        .HasColumnType("boolean");

                    b.Property<bool>("PriceChangesTracking")
                        .HasColumnType("boolean");

                    b.Property<int>("PriceSendLimit")
                        .HasColumnType("integer");

                    b.Property<int?>("PriceTypeId")
                        .HasColumnType("integer");

                    b.Property<string>("ProductTypes")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Settings")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("StockChangesTracking")
                        .HasColumnType("boolean");

                    b.Property<int>("StockSendLimit")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("WarehouseId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PriceTypeId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("Marketplace", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.Marketplaces.MarketplaceOrderTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MarketplaceId")
                        .HasColumnType("integer");

                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.Property<int>("TryCount")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MarketplaceId");

                    b.HasIndex("OrderId");

                    b.ToTable("MarketplaceOrderTask", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.Orders.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Archived")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ConfirmedTimeLimit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("Customer")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("CustomerAddress")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("ExpressOrder")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ExternalId")
                        .HasColumnType("uuid");

                    b.Property<int>("MarketplaceId")
                        .HasColumnType("integer");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<DateTime?>("PackingTimeLimit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ShippingDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ShippingTimeLimit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TrackNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ExternalId")
                        .IsUnique();

                    b.HasIndex("MarketplaceId");

                    b.HasIndex("MarketplaceId", "Number")
                        .IsUnique();

                    b.HasIndex(new[] { "ExternalId" }, "UNIQUE_INDEX_ExternalId")
                        .IsUnique();

                    b.ToTable("Order", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.Orders.OrderChange", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.HasKey("OrderId");

                    b.ToTable("OrderChange", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.Orders.OrderStatusHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("User")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderStatusHistory", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.Orders.OrderSticker", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<byte[]>("StickerData")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.HasKey("OrderId");

                    b.ToTable("OrderSticker", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.Prices.Price", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("PriceTypeId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("ProductId", "PriceTypeId");

                    b.HasIndex("PriceTypeId");

                    b.ToTable("Price", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.Prices.PriceChange", b =>
                {
                    b.Property<int>("MarketplaceId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.HasKey("MarketplaceId", "ProductId");

                    b.HasIndex("MarketplaceId");

                    b.HasIndex("ProductId");

                    b.ToTable("PriceChange", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.Prices.PriceType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<bool>("DeletionMark")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ExternalId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ExternalId" }, "UNIQUE_INDEX_ExternalId")
                        .IsUnique()
                        .HasDatabaseName("UNIQUE_INDEX_ExternalId1");

                    b.ToTable("PriceType", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.Products.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<bool>("DeletionMark")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ExternalId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex(new[] { "ExternalId" }, "UNIQUE_INDEX_ExternalId")
                        .IsUnique()
                        .HasDatabaseName("UNIQUE_INDEX_ExternalId2");

                    b.ToTable("Category", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.Products.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Articul")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Barcode")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Brand")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("CountryOfOrigin")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<bool>("DeletionMark")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ExternalId")
                        .HasColumnType("uuid");

                    b.Property<decimal?>("Height")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("Length")
                        .HasColumnType("numeric");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int?>("ProductType")
                        .HasColumnType("integer");

                    b.Property<int?>("Vat")
                        .HasColumnType("integer");

                    b.Property<string>("Vendor")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("VendorCode")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<decimal?>("Weight")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("Width")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("Articul")
                        .IsUnique();

                    b.HasIndex("CategoryId");

                    b.HasIndex(new[] { "ExternalId" }, "UNIQUE_INDEX_ExternalId")
                        .IsUnique()
                        .HasDatabaseName("UNIQUE_INDEX_ExternalId3");

                    b.ToTable("Product", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.Warehouses.Stock", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("WarehouseId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("ProductId", "WarehouseId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("Stock", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.Warehouses.StockChange", b =>
                {
                    b.Property<int>("MarketplaceId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.HasKey("MarketplaceId", "ProductId");

                    b.HasIndex("MarketplaceId");

                    b.HasIndex("ProductId");

                    b.ToTable("StockChange", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.Warehouses.Warehouse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<bool>("DeletionMark")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ExternalId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ExternalId" }, "UNIQUE_INDEX_ExternalId")
                        .IsUnique()
                        .HasDatabaseName("UNIQUE_INDEX_ExternalId4");

                    b.ToTable("Warehouse", "dbo");
                });

            modelBuilder.Entity("Infrastructure.Models.BotUsers.BotUserUsageCounter", b =>
                {
                    b.HasOne("Infrastructure.Models.BotUsers.BotUser", "BotUser")
                        .WithMany()
                        .HasForeignKey("BotUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BotUser");
                });

            modelBuilder.Entity("Infrastructure.Models.Companies.Company", b =>
                {
                    b.HasOne("Infrastructure.Models.Prices.PriceType", "PriceType")
                        .WithMany()
                        .HasForeignKey("PriceTypeId");

                    b.Navigation("PriceType");
                });

            modelBuilder.Entity("Infrastructure.Models.MarketplaceCategorySettings.MarketplaceCategorySetting", b =>
                {
                    b.HasOne("Infrastructure.Models.Products.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Models.Marketplaces.Marketplace", "Marketplace")
                        .WithMany()
                        .HasForeignKey("MarketplaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Marketplace");
                });

            modelBuilder.Entity("Infrastructure.Models.MarketplaceProductSettings.MarketplaceProductSetting", b =>
                {
                    b.HasOne("Infrastructure.Models.Marketplaces.Marketplace", "Marketplace")
                        .WithMany()
                        .HasForeignKey("MarketplaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Models.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Marketplace");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Infrastructure.Models.Marketplaces.Marketplace", b =>
                {
                    b.HasOne("Infrastructure.Models.Prices.PriceType", "PriceType")
                        .WithMany()
                        .HasForeignKey("PriceTypeId");

                    b.HasOne("Infrastructure.Models.Warehouses.Warehouse", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PriceType");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("Infrastructure.Models.Marketplaces.MarketplaceOrderTask", b =>
                {
                    b.HasOne("Infrastructure.Models.Marketplaces.Marketplace", "Marketplace")
                        .WithMany()
                        .HasForeignKey("MarketplaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Models.Orders.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Marketplace");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Infrastructure.Models.Orders.Order", b =>
                {
                    b.HasOne("Infrastructure.Models.Marketplaces.Marketplace", "Marketplace")
                        .WithMany()
                        .HasForeignKey("MarketplaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Infrastructure.Models.Orders.Order+OrderBox", "Boxes", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<decimal>("Count")
                                .HasColumnType("numeric");

                            b1.Property<int>("Number")
                                .HasColumnType("integer");

                            b1.Property<int>("OrderId")
                                .HasColumnType("integer");

                            b1.Property<int>("ProductId")
                                .HasColumnType("integer");

                            b1.HasKey("Id");

                            b1.HasIndex("OrderId");

                            b1.HasIndex("ProductId");

                            b1.ToTable("OrderBox", "dbo");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");

                            b1.HasOne("Infrastructure.Models.Products.Product", "Product")
                                .WithMany()
                                .HasForeignKey("ProductId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.Navigation("Product");
                        });

                    b.OwnsMany("Infrastructure.Models.Orders.Order+OrderItem", "Items", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<bool>("Canceled")
                                .HasColumnType("boolean");

                            b1.Property<decimal>("Count")
                                .HasColumnType("numeric");

                            b1.Property<string>("ExternalId")
                                .HasColumnType("text");

                            b1.Property<int>("OrderId")
                                .HasColumnType("integer");

                            b1.Property<decimal>("Price")
                                .HasColumnType("numeric");

                            b1.Property<int>("ProductId")
                                .HasColumnType("integer");

                            b1.Property<decimal>("Sum")
                                .HasColumnType("numeric");

                            b1.HasKey("Id");

                            b1.HasIndex("OrderId");

                            b1.HasIndex("ProductId");

                            b1.ToTable("OrderItem", "dbo");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");

                            b1.HasOne("Infrastructure.Models.Products.Product", "Product")
                                .WithMany()
                                .HasForeignKey("ProductId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.Navigation("Product");
                        });

                    b.Navigation("Boxes");

                    b.Navigation("Items");

                    b.Navigation("Marketplace");
                });

            modelBuilder.Entity("Infrastructure.Models.Orders.OrderChange", b =>
                {
                    b.HasOne("Infrastructure.Models.Orders.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Infrastructure.Models.Orders.OrderStatusHistory", b =>
                {
                    b.HasOne("Infrastructure.Models.Orders.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Infrastructure.Models.Orders.OrderSticker", b =>
                {
                    b.HasOne("Infrastructure.Models.Orders.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Infrastructure.Models.Prices.Price", b =>
                {
                    b.HasOne("Infrastructure.Models.Prices.PriceType", "PriceType")
                        .WithMany()
                        .HasForeignKey("PriceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Models.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PriceType");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Infrastructure.Models.Prices.PriceChange", b =>
                {
                    b.HasOne("Infrastructure.Models.Marketplaces.Marketplace", "Marketplace")
                        .WithMany()
                        .HasForeignKey("MarketplaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Models.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Marketplace");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Infrastructure.Models.Products.Category", b =>
                {
                    b.HasOne("Infrastructure.Models.Products.Category", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Infrastructure.Models.Products.Product", b =>
                {
                    b.HasOne("Infrastructure.Models.Products.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Infrastructure.Models.Warehouses.Stock", b =>
                {
                    b.HasOne("Infrastructure.Models.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Models.Warehouses.Warehouse", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("Infrastructure.Models.Warehouses.StockChange", b =>
                {
                    b.HasOne("Infrastructure.Models.Marketplaces.Marketplace", "Marketplace")
                        .WithMany()
                        .HasForeignKey("MarketplaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Models.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Marketplace");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Infrastructure.Models.Products.Category", b =>
                {
                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}
