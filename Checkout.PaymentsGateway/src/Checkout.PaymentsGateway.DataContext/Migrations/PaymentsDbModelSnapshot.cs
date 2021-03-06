﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Checkout.PaymentsGateway.DataContext.Migrations
{
    [DbContext(typeof(PaymentsDb))]
    internal partial class PaymentsDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Checkout.PaymentsGateway.DataContext.Models.Payment", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("uniqueidentifier");

                b.Property<decimal>("Amount")
                    .HasColumnType("decimal(18,2)");

                b.Property<DateTime>("CardExpirationDate")
                    .HasColumnType("datetime2");

                b.Property<string>("CardName")
                    .HasColumnType("nvarchar(200)");

                b.Property<string>("CardNumber")
                    .HasColumnType("nvarchar(200)");

                b.Property<Guid>("CompanyId")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Currency")
                    .HasColumnType("nvarchar(3)");

                b.Property<string>("Cvv")
                    .HasColumnType("nvarchar(3)");

                b.Property<DateTime>("TransactionDate")
                    .HasColumnType("datetime2");

                b.Property<string>("TransactionStatus")
                    .HasColumnType("nvarchar(100)");

                b.HasKey("Id");

                b.ToTable("Payments");
            });
#pragma warning restore 612, 618
        }
    }
}