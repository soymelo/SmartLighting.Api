namespace SmartLighting.Api.Application.Features.Dashboard.GetDashboard;

public sealed record GetDashboardResponse(
    Guid DeviceId,
    string Name,
    DateTime? LastTelemetryAtUtc,
    IReadOnlyCollection<GetDashboardResponse.LuminaireResponse> Luminaires,
    IReadOnlyCollection<GetDashboardResponse.SensorEventResponse> RecentSensorEvents)
{
    public sealed record LuminaireResponse(
        Guid LuminaireId,
        string Name,
        string ZoneName,
        int? BrightnessPercentage,
        DateTime? UpdatedAtUtc);

    public sealed record SensorEventResponse(
        Guid EventIdentifier,
        Guid SensorId,
        string SensorName,
        string ZoneName,
        DateTime OccurredAtUtc);
}
