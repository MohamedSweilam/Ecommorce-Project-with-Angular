using Ecommorce.Core.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce_.infrastructure.Data.Configurations
{
    class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(m => m.Price).HasColumnType("decimal(18,2)");
            builder.HasData(new DeliveryMethod
            { Id = 1, DeliveryTime = "One week", Description = "asra3 delivery fe masr", Name = "Aramex", Price = 15 },
           new DeliveryMethod { Id = 2, DeliveryTime = "two week", Description = "tany asra3 delivery fe masr", Name = "FeedEX", Price = 11 }
            );
        }
    }
}
