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
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

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

                    b.Property<string>("Shop")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.ToTable("Company", "dbo");
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

                    b.HasIndex("ProductId", "PriceTypeId");

                    b.ToTable("Price", "dbo");
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

                    b.HasIndex(new[] { "ExternalId" }, "ExternalId")
                        .IsUnique();

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

                    b.HasIndex(new[] { "ExternalId" }, "ExternalId")
                        .IsUnique()
                        .HasDatabaseName("ExternalId1");

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
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("integer");

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

                    b.HasKey("Id");

                    b.HasIndex("Articul")
                        .IsUnique();

                    b.HasIndex("CategoryId");

                    b.HasIndex(new[] { "ExternalId" }, "ExternalId")
                        .IsUnique()
                        .HasDatabaseName("ExternalId2");

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

                    b.HasIndex("ProductId", "WarehouseId");

                    b.ToTable("Stock", "dbo");
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

                    b.HasIndex(new[] { "ExternalId" }, "ExternalId")
                        .IsUnique()
                        .HasDatabaseName("ExternalId3");

                    b.ToTable("Warehouse", "dbo");
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

            modelBuilder.Entity("Infrastructure.Models.Products.Category", b =>
                {
                    b.HasOne("Infrastructure.Models.Products.Category", "Parent")
                        .WithMany()
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
#pragma warning restore 612, 618
        }
    }
}
