using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Queries.Dashboard;
using SmartLighting.Api.Application.Features.Dashboard.GetProjectDashboard;

namespace SmartLighting.Api.Application.Queries.Dashboard;

public sealed class GetProjectDashboardHandler(IProjectDashboardQueryRepository dashboardQueryRepository)
    : ICommandHandler<GetProjectDashboardQuery, GetProjectDashboardResponse?>
{
    public async Task<GetProjectDashboardResponse?> ExecuteAsync(
        GetProjectDashboardQuery query,
        CancellationToken ct)
    {
        var dashboard = await dashboardQueryRepository.GetProjectDashboardByIdAsync(query.ProjectId, ct);

        if (dashboard is null)
        {
            return null;
        }

        var devices = dashboard.Devices
            .Select(device => new GetProjectDashboardResponse.DeviceResponse(
                device.DeviceId,
                device.Name,
                device.LastTelemetryAtUtc))
            .ToList();

        var zones = dashboard.Zones
            .Select(zone => new GetProjectDashboardResponse.ZoneResponse(
                zone.ZoneId,
                zone.Name,
                zone.DisplayOrder,
                zone.Sensors
                    .Select(sensor => new GetProjectDashboardResponse.SensorResponse(
                        sensor.SensorId,
                        sensor.DeviceId,
                        sensor.Name))
                    .ToList(),
                zone.Luminaires
                    .Select(luminaire => new GetProjectDashboardResponse.LuminaireResponse(
                        luminaire.LuminaireId,
                        luminaire.DeviceId,
                        luminaire.Name,
                        luminaire.NominalPowerWatts,
                        luminaire.BrightnessPercentage,
                        luminaire.UpdatedAtUtc))
                    .ToList()))
            .ToList();

        var sensorEvents = dashboard.RecentSensorEvents
            .Select(sensorEvent => new GetProjectDashboardResponse.SensorEventResponse(
                sensorEvent.EventIdentifier,
                sensorEvent.SensorId,
                sensorEvent.DeviceId,
                sensorEvent.SensorName,
                sensorEvent.ZoneId,
                sensorEvent.ZoneName,
                sensorEvent.OccurredAtUtc))
            .ToList();

        return new GetProjectDashboardResponse(
            dashboard.ProjectId,
            dashboard.Name,
            devices,
            zones,
            sensorEvents);
    }
}
