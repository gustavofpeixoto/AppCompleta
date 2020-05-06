﻿// <auto-generated />
using System;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DevIO.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200506183236_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DevIO.Business.Models.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AddressComplement")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Neighborhood")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<Guid>("ProviderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PublicPlace")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("varchar(50");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("varchar(8)");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("DevIO.Business.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Picture")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("ProviderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("DevIO.Business.Models.Provider", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Document")
                        .IsRequired()
                        .HasColumnType("varchar(14)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<int>("ProviderType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Providers");
                });

            modelBuilder.Entity("DevIO.Business.Models.Address", b =>
                {
                    b.HasOne("DevIO.Business.Models.Provider", "Providers")
                        .WithOne("Address")
                        .HasForeignKey("DevIO.Business.Models.Address", "ProviderId")
                        .IsRequired();
                });

            modelBuilder.Entity("DevIO.Business.Models.Product", b =>
                {
                    b.HasOne("DevIO.Business.Models.Provider", "Provider")
                        .WithMany("Products")
                        .HasForeignKey("ProviderId")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
