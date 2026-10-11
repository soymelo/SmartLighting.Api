namespace SmartLighting.Api.Application.Features.Dashboard.GetProjectDashboard;

public sealed record GetProjectDashboardResponse(
    Guid ProjectId,
    string Name,
    IReadOnlyCollection<GetProjectDashboardResponse.DeviceResponse> Devices,
    IReadOnlyCollection<GetProjectDashboardResponse.ZoneResponse> Zones,
    IReadOnlyCollection<GetProjectDashboardResponse.ZoneActivityResponse> ZoneActivity24h)
{
    public sealed record ZoneActivityResponse(Guid ZoneId, int EventCount);
    public sealed record DeviceResponse(Guid DeviceId, string Name, DateTime? LastTelemetryAtUtc);

    public sealed record ZoneResponse(
        Guid ZoneId,
        string Name,
        int DisplayOrder,
        IReadOnlyCollection<SensorResponse> Sensors,
        IReadOnlyCollection<LuminaireResponse> Luminaires);

    public sealed record SensorResponse(Guid SensorId, Guid DeviceId, string Name);

    public sealed record LuminaireResponse(
        Guid LuminaireId,
        Guid DeviceId,
        string Name,
        decimal NominalPowerWatts,
        int? BrightnessPercentage,
        DateTime? UpdatedAtUtc);
}
