﻿// <auto-generated />
using System;
using GbStoreApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GbStoreApi.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240601194235_InvertResponsabilityToDerivedTypesOfPurchaseToTheirDerivedTables")]
    partial class InvertResponsabilityToDerivedTypesOfPurchaseToTheirDerivedTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GbStoreApi.Domain.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Neighbourhood")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Color", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.OrderItems", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductCount")
                        .HasColumnType("int");

                    b.Property<int>("ProductStockId")
                        .HasColumnType("int");

                    b.Property<decimal>("ProductStockPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("PurchaseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductStockId");

                    b.HasIndex("PurchaseId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Picture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<float?>("DiscountPercent")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("QuotasNumber")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitaryPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.ProductStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ColorId")
                        .HasColumnType("int");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("SizeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ColorId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SizeId");

                    b.ToTable("ProductStocks");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Purchases.Purchase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EstimatedDeliveryDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("FinalPrice")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TypeOfDelivery")
                        .HasColumnType("int");

                    b.Property<int>("TypeOfPayment")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Purchases.ShippingPurchase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DeliveryInstructions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PurchaseId")
                        .HasColumnType("int");

                    b.Property<int>("UserAddressId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserOwnerAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PurchaseId")
                        .IsUnique();

                    b.HasIndex("UserOwnerAddressId");

                    b.ToTable("ShippingPurchases");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Purchases.StorePickupPurchase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PurchaseId")
                        .HasColumnType("int");

                    b.Property<int>("StoreAddressId")
                        .HasColumnType("int");

                    b.Property<int>("UserBuyerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PurchaseId")
                        .IsUnique();

                    b.HasIndex("StoreAddressId");

                    b.HasIndex("UserBuyerId");

                    b.ToTable("StorePickupPurchases");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Size", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Sizes");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BirthdayDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TokenCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("TokenExpires")
                        .HasColumnType("datetime2");

                    b.Property<int>("TypeOfUser")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BirthdayDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Cpf = "00000000000",
                            Email = "admin@gmail.com",
                            Name = "Administrador",
                            Password = "$2a$11$t6XFpMSG74tuVVOCidtmQeXdqyteWbTBIqe29uC98goiLtzqiZdzC",
                            RefreshToken = "",
                            TypeOfUser = 2
                        });
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.UserAddress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("Complement")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAddresses");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.OrderItems", b =>
                {
                    b.HasOne("GbStoreApi.Domain.Models.ProductStock", "Stock")
                        .WithMany("OrderProductStockItems")
                        .HasForeignKey("ProductStockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GbStoreApi.Domain.Models.Purchases.Purchase", "Purchase")
                        .WithMany("OrderItems")
                        .HasForeignKey("PurchaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Purchase");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Picture", b =>
                {
                    b.HasOne("GbStoreApi.Domain.Models.Product", "Product")
                        .WithMany("Pictures")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Product", b =>
                {
                    b.HasOne("GbStoreApi.Domain.Models.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GbStoreApi.Domain.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.ProductStock", b =>
                {
                    b.HasOne("GbStoreApi.Domain.Models.Color", "Color")
                        .WithMany("Stocks")
                        .HasForeignKey("ColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GbStoreApi.Domain.Models.Product", "Product")
                        .WithMany("Stocks")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GbStoreApi.Domain.Models.Size", "Size")
                        .WithMany("Stocks")
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Color");

                    b.Navigation("Product");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Purchases.ShippingPurchase", b =>
                {
                    b.HasOne("GbStoreApi.Domain.Models.Purchases.Purchase", "Purchase")
                        .WithOne("ShippingPurchase")
                        .HasForeignKey("GbStoreApi.Domain.Models.Purchases.ShippingPurchase", "PurchaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GbStoreApi.Domain.Models.UserAddress", "UserOwnerAddress")
                        .WithMany()
                        .HasForeignKey("UserOwnerAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Purchase");

                    b.Navigation("UserOwnerAddress");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Purchases.StorePickupPurchase", b =>
                {
                    b.HasOne("GbStoreApi.Domain.Models.Purchases.Purchase", "Purchase")
                        .WithOne("StorePickupPurchase")
                        .HasForeignKey("GbStoreApi.Domain.Models.Purchases.StorePickupPurchase", "PurchaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GbStoreApi.Domain.Models.Address", "StoreAddress")
                        .WithMany()
                        .HasForeignKey("StoreAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GbStoreApi.Domain.Models.User", "UserBuyer")
                        .WithMany()
                        .HasForeignKey("UserBuyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Purchase");

                    b.Navigation("StoreAddress");

                    b.Navigation("UserBuyer");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.UserAddress", b =>
                {
                    b.HasOne("GbStoreApi.Domain.Models.Address", "Address")
                        .WithMany("UserAddresses")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GbStoreApi.Domain.Models.User", "User")
                        .WithMany("UserAddresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Address", b =>
                {
                    b.Navigation("UserAddresses");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Brand", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Color", b =>
                {
                    b.Navigation("Stocks");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Product", b =>
                {
                    b.Navigation("Pictures");

                    b.Navigation("Stocks");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.ProductStock", b =>
                {
                    b.Navigation("OrderProductStockItems");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Purchases.Purchase", b =>
                {
                    b.Navigation("OrderItems");

                    b.Navigation("ShippingPurchase");

                    b.Navigation("StorePickupPurchase");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.Size", b =>
                {
                    b.Navigation("Stocks");
                });

            modelBuilder.Entity("GbStoreApi.Domain.Models.User", b =>
                {
                    b.Navigation("UserAddresses");
                });
#pragma warning restore 612, 618
        }
    }
}
