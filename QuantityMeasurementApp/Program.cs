using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuantityMeasurementApp.Middleware;
using QuantityMeasurementRepository;
using QuantityMeasurementRepository.Interfaces;
using QuantityMeasurementRepository.Repositories;
using QuantityMeasurementService.Core;

var builder = WebApplication.CreateBuilder(args);

// Core services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Quantity Measurement API",
        Version = "v1"
    });
});

builder.Services.AddHealthChecks();
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("AllowAll", policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
});

// Database Configuration
string dbProvider = builder.Configuration.GetValue<string>("DatabaseProvider");
if (dbProvider == "SqlServer")
{
    builder.Services.AddDbContext<MeasurementDbContext>(opts => 
        opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}
else
{
    builder.Services.AddDbContext<MeasurementDbContext>(opts => 
        opts.UseInMemoryDatabase("QuantityMeasurementDb"));
}

// DI
builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementEfRepository>();
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementService.Core.QuantityMeasurementService>();

var app = builder.Build();

// Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement API v1");
    c.RoutePrefix = string.Empty;
});

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MeasurementDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();