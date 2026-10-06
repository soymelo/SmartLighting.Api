using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using SmartLighting.Api.Application.Abstractions.Queries.Dashboard;
using SmartLighting.Api.Application.Abstractions.Queries.Devices;
using SmartLighting.Api.Application.Abstractions.Queries.Projects;
using SmartLighting.Api.Application.Abstractions.Queries.Topology;
using SmartLighting.Api.Application.Abstractions.Repositories.Devices;
using SmartLighting.Api.Application.Abstractions.Repositories.Telemetry;
using SmartLighting.Api.Application.Features.Dashboard.GetProjectDashboard;
using SmartLighting.Api.Application.Features.Devices.CreateDevice;
using SmartLighting.Api.Application.Features.Devices.GetDeviceById;
using SmartLighting.Api.Application.Features.Devices.GetDevices;
using SmartLighting.Api.Application.Features.Devices.UpdateDevice;
using SmartLighting.Api.Application.Features.Projects.GetProjects;
using SmartLighting.Api.Application.Features.Telemetry.SubmitTelemetry;
using SmartLighting.Api.Application.Features.Topology.GetTopology;
using SmartLighting.Api.Infrastructure.Persistence;
using SmartLighting.Api.Infrastructure.Queries.Dashboard;
using SmartLighting.Api.Infrastructure.Queries.Devices;
using SmartLighting.Api.Infrastructure.Queries.Projects;
using SmartLighting.Api.Infrastructure.Queries.Topology;
using SmartLighting.Api.Infrastructure.Repositories.Devices;
using SmartLighting.Api.Infrastructure.Repositories.Telemetry;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SmartLightingDatabase")
    ?? throw new InvalidOperationException("A connection string 'SmartLightingDatabase' não foi configurada.");

builder.Services.AddDbContext<SmartLightingDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IDeviceQueryRepository, DeviceQueryRepository>();
builder.Services.AddScoped<IProjectQueryRepository, ProjectQueryRepository>();
builder.Services.AddScoped<IProjectDashboardQueryRepository, ProjectDashboardQueryRepository>();
builder.Services.AddScoped<ITopologyQueryRepository, TopologyQueryRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<ITelemetryRepository, TelemetryRepository>();
builder.Services.AddScoped<CreateDeviceProcessor>();
builder.Services.AddScoped<GetDevicesProcessor>();
builder.Services.AddScoped<GetDeviceByIdProcessor>();
builder.Services.AddScoped<UpdateDeviceProcessor>();
builder.Services.AddScoped<GetProjectsProcessor>();
builder.Services.AddScoped<GetProjectDashboardProcessor>();
builder.Services.AddScoped<GetTopologyProcessor>();
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
