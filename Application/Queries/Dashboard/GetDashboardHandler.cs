using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Queries.Dashboard;
using SmartLighting.Api.Application.Features.Dashboard.GetDashboard;

namespace SmartLighting.Api.Application.Queries.Dashboard;

public sealed class GetDashboardHandler(IDashboardQueryRepository dashboardQueryRepository)
    : ICommandHandler<GetDashboardQuery, GetDashboardResponse?>
{
    public async Task<GetDashboardResponse?> ExecuteAsync(
        GetDashboardQuery query,
        CancellationToken ct)
    {
        var dashboard = await dashboardQueryRepository.GetDashboardByIdAsync(query.DeviceId, ct);

        if (dashboard is null)
        {
            return null;
        }

        var luminaires = dashboard.Luminaires
            .Select(luminaire => new GetDashboardResponse.LuminaireResponse(
                luminaire.LuminaireId,
                luminaire.Name,
                luminaire.ZoneName,
                luminaire.BrightnessPercentage,
                luminaire.UpdatedAtUtc))
            .ToList();

        var sensorEvents = dashboard.RecentSensorEvents
            .Select(sensorEvent => new GetDashboardResponse.SensorEventResponse(
                sensorEvent.EventIdentifier,
                sensorEvent.SensorId,
                sensorEvent.SensorName,
                sensorEvent.ZoneName,
                sensorEvent.OccurredAtUtc))
            .ToList();

        return new GetDashboardResponse(
            dashboard.DeviceId,
            dashboard.Name,
            dashboard.LastTelemetryAtUtc,
            luminaires,
            sensorEvents);
    }
}
