namespace SmartLighting.Api.Application.Features.Devices.GetDeviceDashboard;

public sealed record GetDeviceDashboardResponse(
    Guid DeviceId,
    string Name,
    DateTime? LastTelemetryAtUtc,
    IReadOnlyCollection<GetDeviceDashboardResponse.LuminaireResponse> Luminaires,
    IReadOnlyCollection<GetDeviceDashboardResponse.SensorEventResponse> RecentSensorEvents)
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
