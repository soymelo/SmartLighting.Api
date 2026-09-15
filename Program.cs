using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using SmartLighting.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SmartLightingDatabase")
    ?? throw new InvalidOperationException("A connection string 'SmartLightingDatabase' não foi configurada.");

builder.Services.AddDbContext<SmartLightingDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseFastEndpoints()
   .UseSwaggerGen();

app.Run();
