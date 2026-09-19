using SmartLighting.Api.Domain.Entities;

namespace SmartLighting.Api.Application.Abstractions.Repositories;

public interface ITelemetryRepository
{
    Task<Device?> GetDeviceByIdAsync(Guid deviceId, CancellationToken ct);

    Task<bool> DoSensorsBelongToDeviceAsync(
        Guid deviceId,
        IReadOnlyCollection<Guid> sensorIds,
        CancellationToken ct);

    Task<bool> DoLuminairesBelongToDeviceAsync(
        Guid deviceId,
        IReadOnlyCollection<Guid> luminaireIds,
        CancellationToken ct);

    Task<IReadOnlyCollection<Guid>> GetExistingSensorEventIdentifiersAsync(
        IReadOnlyCollection<Guid> eventIdentifiers,
        CancellationToken ct);

    Task<IReadOnlyCollection<Guid>> GetExistingLuminaireStateChangeEventIdentifiersAsync(
        IReadOnlyCollection<Guid> eventIdentifiers,
        CancellationToken ct);

    Task<IReadOnlyDictionary<Guid, LuminaireState>> GetLuminaireStatesAsync(
        IReadOnlyCollection<Guid> luminaireIds,
        CancellationToken ct);

    void AddSensorEvents(IEnumerable<SensorEvent> sensorEvents);
    void AddLuminaireStateChanges(IEnumerable<LuminaireStateChange> stateChanges);
    void AddLuminaireStates(IEnumerable<LuminaireState> states);

    Task SaveChangesAsync(CancellationToken ct);
}
