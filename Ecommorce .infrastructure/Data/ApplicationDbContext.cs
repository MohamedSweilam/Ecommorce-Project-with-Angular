using Ecommorce.Core.Entities.AppUser;
using Ecommorce.Core.Entities.Order;
using Ecommorce.Core.Entities.Product;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce_.infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public virtual DbSet<Product> Products{get;set;}
        public virtual DbSet<Category> Categories{get;set;}
        public  virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Photo> Photos{get;set;}
        public virtual DbSet<Orders> Orders{get;set;}
        public virtual DbSet<OrderItem> OrderItems{get;set;}
        public virtual DbSet<DeliveryMethod> DeliveryMethods{get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // 🔥 This is crucial for Identity to work properly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }






    }
}
