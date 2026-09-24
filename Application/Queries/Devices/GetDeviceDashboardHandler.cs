using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Queries;
using SmartLighting.Api.Application.Features.Devices.GetDeviceDashboard;

namespace SmartLighting.Api.Application.Queries.Devices;

public sealed class GetDeviceDashboardHandler(IDeviceQueryRepository deviceQueryRepository)
    : ICommandHandler<GetDeviceDashboardQuery, GetDeviceDashboardResponse?>
{
    public async Task<GetDeviceDashboardResponse?> ExecuteAsync(
        GetDeviceDashboardQuery query,
        CancellationToken ct)
    {
        var dashboard = await deviceQueryRepository.GetDashboardByIdAsync(query.DeviceId, ct);

        if (dashboard is null)
        {
            return null;
        }

        var luminaires = dashboard.Luminaires
            .Select(luminaire => new GetDeviceDashboardResponse.LuminaireResponse(
                luminaire.LuminaireId,
                luminaire.Name,
                luminaire.ZoneName,
                luminaire.BrightnessPercentage,
                luminaire.UpdatedAtUtc))
            .ToList();

        var sensorEvents = dashboard.RecentSensorEvents
            .Select(sensorEvent => new GetDeviceDashboardResponse.SensorEventResponse(
                sensorEvent.EventIdentifier,
                sensorEvent.SensorId,
                sensorEvent.SensorName,
                sensorEvent.ZoneName,
                sensorEvent.OccurredAtUtc))
            .ToList();

        return new GetDeviceDashboardResponse(
            dashboard.DeviceId,
            dashboard.Name,
            dashboard.LastTelemetryAtUtc,
            luminaires,
            sensorEvents);
    }
}
