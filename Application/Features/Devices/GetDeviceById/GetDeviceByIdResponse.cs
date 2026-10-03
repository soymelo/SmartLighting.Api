namespace SmartLighting.Api.Application.Features.Devices.GetDeviceById;

public sealed record GetDeviceByIdResponse(
    Guid DeviceId,
    string Name,
    Guid ProjectId,
    string ProjectName,
    DateTime? LastTelemetryAtUtc,
    IReadOnlyCollection<GetDeviceByIdResponse.ZoneResponse> Zones)
{
    public sealed record ZoneResponse(
        Guid ZoneId,
        string Name,
        int DisplayOrder,
        IReadOnlyCollection<SensorResponse> Sensors,
        IReadOnlyCollection<LuminaireResponse> Luminaires);

    public sealed record SensorResponse(
        Guid SensorId,
        string Name,
        int Slot,
        int GpioNumber);

    public sealed record LuminaireResponse(
        Guid LuminaireId,
        string Name,
        decimal NominalPowerWatts,
        int Slot,
        int GpioNumber);
}
