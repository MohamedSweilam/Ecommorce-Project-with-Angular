using Ecommorce.Core.Entities.AppUser;
using Ecommorce.Core.interfaces;
using Ecommorce.Core.Services;
using Ecommorce_.infrastructure.Data;
using Ecommorce_.infrastructure.Repositories;
using Ecommorce_.infrastructure.Repositories.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
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
            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            services.AddSingleton<IImageMangmentService, ImageMangmentService>();
            services.AddSingleton<IConnectionMultiplexer>(i =>
            {
                var config = ConfigurationOptions.Parse(confg.GetConnectionString("redis"));
                return ConnectionMultiplexer.Connect(config);
            });

            services.AddScoped<IGenerateToken, GenerateToken>();
            services.AddHttpContextAccessor();
            services.AddScoped<IJwtTokenService, JwtTokenService>();


            services.AddScoped<IUnitofwork, UnitOfWork>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddDbContext<ApplicationDbContext>(op =>
            {
                op.UseSqlServer(confg.GetConnectionString("EcomDatabase")); 
            });

            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


            //services.AddAuthentication(op =>
            //{
            //    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    op.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //}).AddCookie(o =>
            //{
            //    o.Cookie.Name = "token";
            //    o.Events.OnRedirectToLogin = context =>
            //    {
            //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //        return Task.CompletedTask;
            //    };
            //}).AddJwtBearer(op =>
            //{
            //    op.RequireHttpsMetadata = false;
            //    op.SaveToken = true;
            //    op.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(confg["Token:Secret"])),
            //        ValidateIssuer = true,
            //        ValidIssuer = confg["Token:Issuer"],
            //        ValidateAudience = false,
            //        ClockSkew = TimeSpan.Zero

            //    };
            //    op.Events = new JwtBearerEvents()
            //    {
            //        OnMessageReceived=context =>
            //        {
            //            context.Token = context.Request.Cookies["token"];
            //            return Task.CompletedTask;
            //        }
            //    };

            //});
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(confg["Token:Secret"])),
        ValidateIssuer = true,
        ValidIssuer = confg["Token:Issuer"],
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };

});


            return services;
        }

    }
}
