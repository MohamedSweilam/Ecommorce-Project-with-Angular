using Ecommorce.API.Helper;
using Ecommorce.API.Middleware;
using Ecommorce.infrastructure;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;

IdentityModelEventSource.ShowPII = true;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.InfrastructureConfiguration(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // ⬅️ مهم جدًا قبل أي request processing

app.UseCors("CORSPolicy"); // ⬅️ لازم يجي قبل Authentication

app.UseAuthentication();   // ⬅️ يجب أن يكون بعد CORS
app.UseAuthorization();

app.UseMiddleware<ExecptionsMiddleware>(); // Middleware مخصص

app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.MapControllers();

app.Run();
