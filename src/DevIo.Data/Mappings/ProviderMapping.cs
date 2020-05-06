using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevIo.Data.Mappings
{
    class ProviderMapping : IEntityTypeConfiguration<Provider>
    {
        public void Configure(EntityTypeBuilder<Provider> builder)
        {
            builder.HasKey("Id");

            builder.Property("Name")
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property("Document")
                .IsRequired()
                .HasColumnType("varchar(14)");

            // 1 : 1 => Provider | Address
            builder.HasOne(p => p.Address)
                .WithOne(a => a.Providers);

            // 1 : N => Provider | Products
            builder.HasMany(p => p.Products)
                .WithOne(p => p.Provider)
                .HasForeignKey(p => p.ProviderId);

            builder.ToTable("Providers");
        }
    }
}
