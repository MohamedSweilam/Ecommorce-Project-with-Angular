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
    public class OrderConfiguration : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> builder)
        {
            builder.OwnsOne(x => x.shippingAddress, n => { n.WithOwner(); });
            builder.HasMany(x => x.orderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Property(o => o.status).HasConversion(o => o.ToString(), o => (Status)Enum.Parse(typeof(Status), o));
            builder.Property(o => o.SubTotal).HasColumnType("decimal(18,2)");
        }
    }
}
