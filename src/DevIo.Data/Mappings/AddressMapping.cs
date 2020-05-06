using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevIo.Data.Mappings
{
    class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property("PublicPlace")
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property("Number")
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property("AddressComplement")
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property("ZipCode")
                .IsRequired()
                .HasColumnType("varchar(8)");

            builder.Property("Neighborhood")
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property("City")
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property("State")
                .IsRequired()
                .HasColumnType("varchar(50)");

            // 1 : 1 Address | Provider
        }
    }
}
