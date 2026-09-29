using SmartLighting.Api.Application.ReadModels.Telemetry;

namespace SmartLighting.Api.Application.Abstractions.Queries.Telemetry;

public interface ITelemetryQueryRepository
{
    Task<DeviceTelemetryStatusDto?> GetTelemetryStatusByIdAsync(Guid deviceId, CancellationToken ct);
    Task<DeviceLuminaireStatesDto?> GetLuminaireStatesByDeviceIdAsync(Guid deviceId, CancellationToken ct);
    Task<DeviceRecentSensorEventsDto?> GetRecentSensorEventsByDeviceIdAsync(
        Guid deviceId,
        int limit,
        CancellationToken ct);
}
