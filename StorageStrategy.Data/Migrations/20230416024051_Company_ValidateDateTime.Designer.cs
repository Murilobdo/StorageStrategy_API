﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StorageStrategy.Data.Context;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    [DbContext(typeof(StorageDbContext))]
    [Migration("20230416024051_Company_ValidateDateTime")]
    partial class CompanyValidateDateTime
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StorageStrategy.Models.CategoryEntity", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("CategoryId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("StorageStrategy.Models.CommandEntity", b =>
                {
                    b.Property<int>("CommandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommandId"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("FinalDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("InitialDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Payment")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("CommandId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Command");
                });

            modelBuilder.Entity("StorageStrategy.Models.CommandItemEntity", b =>
                {
                    b.Property<int>("CommandItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommandItemId"));

                    b.Property<int>("CommandId")
                        .HasColumnType("int");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Qtd")
                        .HasColumnType("int");

                    b.HasKey("CommandItemId");

                    b.HasIndex("CommandId");

                    b.HasIndex("ProductId");

                    b.ToTable("CommandItems");
                });

            modelBuilder.Entity("StorageStrategy.Models.CompanyEntity", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CompanyId"));

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Validate")
                        .HasColumnType("datetime2");

                    b.HasKey("CompanyId");

                    b.ToTable("Company");

                    b.HasData(
                        new
                        {
                            CompanyId = 1,
                            CreateAt = new DateTime(2023, 4, 15, 23, 40, 51, 81, DateTimeKind.Local).AddTicks(9811),
                            Description = "Bar",
                            IsActive = true,
                            Name = "Bar do Murps",
                            Validate = new DateTime(2033, 4, 15, 23, 40, 51, 81, DateTimeKind.Local).AddTicks(9828)
                        },
                        new
                        {
                            CompanyId = 2,
                            CreateAt = new DateTime(2023, 4, 15, 23, 40, 51, 81, DateTimeKind.Local).AddTicks(9849),
                            Description = "Tabacaria",
                            IsActive = true,
                            Name = "Rei do Baco",
                            Validate = new DateTime(2026, 4, 15, 23, 40, 51, 81, DateTimeKind.Local).AddTicks(9849)
                        });
                });

            modelBuilder.Entity("StorageStrategy.Models.EmployeeEntity", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"));

                    b.Property<int>("Comission")
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("JobRole")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("EmployeeId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("StorageStrategy.Models.ExpensesEntity", b =>
                {
                    b.Property<int>("ExpenseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExpenseId"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ExpensesTypeId")
                        .HasColumnType("int");

                    b.HasKey("ExpenseId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ExpensesTypeId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("StorageStrategy.Models.ExpensesTypeEntity", b =>
                {
                    b.Property<int>("ExpenseTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExpenseTypeId"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ExpenseTypeId");

                    b.HasIndex("CompanyId");

                    b.ToTable("ExpensesType");
                });

            modelBuilder.Entity("StorageStrategy.Models.ProductEntity", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<decimal>("Cost")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("Qtd")
                        .HasColumnType("int");

                    b.Property<int>("StockAlert")
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("StorageStrategy.Models.CategoryEntity", b =>
                {
                    b.HasOne("StorageStrategy.Models.CompanyEntity", "Company")
                        .WithMany("Categorys")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("StorageStrategy.Models.CommandEntity", b =>
                {
                    b.HasOne("StorageStrategy.Models.CompanyEntity", "Company")
                        .WithMany("Commands")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StorageStrategy.Models.EmployeeEntity", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("StorageStrategy.Models.CommandItemEntity", b =>
                {
                    b.HasOne("StorageStrategy.Models.CommandEntity", "Command")
                        .WithMany("Items")
                        .HasForeignKey("CommandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StorageStrategy.Models.ProductEntity", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Command");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("StorageStrategy.Models.EmployeeEntity", b =>
                {
                    b.HasOne("StorageStrategy.Models.CompanyEntity", "Company")
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("StorageStrategy.Models.ExpensesEntity", b =>
                {
                    b.HasOne("StorageStrategy.Models.CompanyEntity", "Company")
                        .WithMany("Expenses")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StorageStrategy.Models.ExpensesTypeEntity", "ExpensesType")
                        .WithMany()
                        .HasForeignKey("ExpensesTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("ExpensesType");
                });

            modelBuilder.Entity("StorageStrategy.Models.ExpensesTypeEntity", b =>
                {
                    b.HasOne("StorageStrategy.Models.CompanyEntity", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("StorageStrategy.Models.ProductEntity", b =>
                {
                    b.HasOne("StorageStrategy.Models.CategoryEntity", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StorageStrategy.Models.CompanyEntity", "Company")
                        .WithMany("Products")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("StorageStrategy.Models.CategoryEntity", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("StorageStrategy.Models.CommandEntity", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("StorageStrategy.Models.CompanyEntity", b =>
                {
                    b.Navigation("Categorys");

                    b.Navigation("Commands");

                    b.Navigation("Employees");

                    b.Navigation("Expenses");

                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
