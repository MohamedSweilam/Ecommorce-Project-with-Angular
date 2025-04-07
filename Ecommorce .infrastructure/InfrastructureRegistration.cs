using Ecommorce.Core.interfaces;
using Ecommorce_.infrastructure.Data;
using Ecommorce_.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce.infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services,IConfiguration confg)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            
            services.AddScoped<IUnitofwork, UnitOfWork>();
            services.AddDbContext<ApplicationDbContext>(op =>
            {
                op.UseSqlServer(confg.GetConnectionString("EcomDatabase")); 
            });
            return services;
        }

    }
}
