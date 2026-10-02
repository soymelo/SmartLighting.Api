using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using SmartLighting.Api.Application.Abstractions.Queries.Dashboard;
using SmartLighting.Api.Application.Abstractions.Queries.Devices;
using SmartLighting.Api.Application.Abstractions.Queries.Telemetry;
using SmartLighting.Api.Application.Abstractions.Queries.Topology;
using SmartLighting.Api.Application.Abstractions.Repositories.Telemetry;
using SmartLighting.Api.Application.Features.Dashboard.GetDeviceDashboard;
using SmartLighting.Api.Application.Features.Dashboard.GetProjectDashboard;
using SmartLighting.Api.Application.Features.Devices.GetDevices;
using SmartLighting.Api.Application.Features.Telemetry.GetLuminaireStates;
using SmartLighting.Api.Application.Features.Telemetry.GetRecentSensorEvents;
using SmartLighting.Api.Application.Features.Telemetry.GetTelemetryStatus;
using SmartLighting.Api.Application.Features.Telemetry.SubmitTelemetry;
using SmartLighting.Api.Application.Features.Topology.GetTopology;
using SmartLighting.Api.Infrastructure.Persistence;
using SmartLighting.Api.Infrastructure.Queries.Dashboard;
using SmartLighting.Api.Infrastructure.Queries.Devices;
using SmartLighting.Api.Infrastructure.Queries.Telemetry;
using SmartLighting.Api.Infrastructure.Queries.Topology;
using SmartLighting.Api.Infrastructure.Repositories.Telemetry;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SmartLightingDatabase")
    ?? throw new InvalidOperationException("A connection string 'SmartLightingDatabase' não foi configurada.");

builder.Services.AddDbContext<SmartLightingDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IDeviceQueryRepository, DeviceQueryRepository>();
builder.Services.AddScoped<IDeviceDashboardQueryRepository, DeviceDashboardQueryRepository>();
builder.Services.AddScoped<IProjectDashboardQueryRepository, ProjectDashboardQueryRepository>();
builder.Services.AddScoped<ITopologyQueryRepository, TopologyQueryRepository>();
builder.Services.AddScoped<ITelemetryQueryRepository, TelemetryQueryRepository>();
builder.Services.AddScoped<ITelemetryRepository, TelemetryRepository>();
builder.Services.AddScoped<GetDevicesProcessor>();
builder.Services.AddScoped<GetDeviceDashboardProcessor>();
builder.Services.AddScoped<GetProjectDashboardProcessor>();
builder.Services.AddScoped<GetLuminaireStatesProcessor>();
builder.Services.AddScoped<GetTelemetryStatusProcessor>();
builder.Services.AddScoped<GetTopologyProcessor>();
builder.Services.AddScoped<GetRecentSensorEventsProcessor>();
builder.Services.AddScoped<SubmitTelemetryProcessor>();

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

app.UseDefaultExceptionHandler(
    logger,
    logStructuredException: true,
    useGenericReason: true,
    useProblemDetails: true);

app.UseHttpsRedirection();

app.UseFastEndpoints()
   .UseSwaggerGen();

app.Run();
