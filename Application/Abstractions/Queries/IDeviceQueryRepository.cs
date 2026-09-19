using SmartLighting.Api.Application.ReadModels.Devices;

namespace SmartLighting.Api.Application.Abstractions.Queries;

public interface IDeviceQueryRepository
{
    Task<IReadOnlyCollection<DeviceSummaryDto>> GetAllAsync(CancellationToken ct);
    Task<DeviceTopologyDto?> GetTopologyByIdAsync(Guid deviceId, CancellationToken ct);
    Task<DeviceTelemetryStatusDto?> GetTelemetryStatusByIdAsync(Guid deviceId, CancellationToken ct);
    Task<DeviceLuminaireStatesDto?> GetLuminaireStatesByDeviceIdAsync(Guid deviceId, CancellationToken ct);
    Task<DeviceRecentSensorEventsDto?> GetRecentSensorEventsByDeviceIdAsync(
        Guid deviceId,
        int limit,
        CancellationToken ct);
}
