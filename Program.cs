using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using SmartLighting.Api.Application.Abstractions.Queries;
using SmartLighting.Api.Application.Features.Devices.GetDeviceTopology;
using SmartLighting.Api.Infrastructure.Persistence;
using SmartLighting.Api.Infrastructure.Queries;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SmartLightingDatabase")
    ?? throw new InvalidOperationException("A connection string 'SmartLightingDatabase' não foi configurada.");

builder.Services.AddDbContext<SmartLightingDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IDeviceQueryRepository, DeviceQueryRepository>();
builder.Services.AddScoped<GetDeviceTopologyProcessor>();

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseFastEndpoints()
   .UseSwaggerGen();

app.Run();
