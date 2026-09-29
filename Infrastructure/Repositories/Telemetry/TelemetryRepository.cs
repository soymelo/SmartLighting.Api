using Microsoft.EntityFrameworkCore;
using SmartLighting.Api.Application.Abstractions.Repositories.Telemetry;
using SmartLighting.Api.Domain.Entities;
using SmartLighting.Api.Infrastructure.Persistence;

namespace SmartLighting.Api.Infrastructure.Repositories.Telemetry;

public sealed class TelemetryRepository(SmartLightingDbContext dbContext) : ITelemetryRepository
{
    public Task<Device?> GetDeviceByIdAsync(Guid deviceId, CancellationToken ct) =>
        dbContext.Devices.SingleOrDefaultAsync(device => device.Id == deviceId, ct);

    public async Task<bool> DoSensorsBelongToDeviceAsync(
        Guid deviceId,
        IReadOnlyCollection<Guid> sensorIds,
        CancellationToken ct)
    {
        var distinctSensorIds = sensorIds.Distinct().ToList();

        if (distinctSensorIds.Count == 0)
        {
            return true;
        }

        var sensorsInDeviceCount = await dbContext.Sensors.CountAsync(
            sensor => distinctSensorIds.Contains(sensor.Id) && sensor.Zone.DeviceId == deviceId,
            ct);

        return sensorsInDeviceCount == distinctSensorIds.Count;
    }

    public async Task<bool> DoLuminairesBelongToDeviceAsync(
        Guid deviceId,
        IReadOnlyCollection<Guid> luminaireIds,
        CancellationToken ct)
    {
        var distinctLuminaireIds = luminaireIds.Distinct().ToList();

        if (distinctLuminaireIds.Count == 0)
        {
            return true;
        }

        var luminairesInDeviceCount = await dbContext.Luminaires.CountAsync(
            luminaire => distinctLuminaireIds.Contains(luminaire.Id) && luminaire.Zone.DeviceId == deviceId,
            ct);

        return luminairesInDeviceCount == distinctLuminaireIds.Count;
    }

    public async Task<IReadOnlyCollection<Guid>> GetExistingSensorEventIdentifiersAsync(
        IReadOnlyCollection<Guid> eventIdentifiers,
        CancellationToken ct) =>
        await dbContext.SensorEvents
            .Where(sensorEvent => eventIdentifiers.Contains(sensorEvent.EventIdentifier))
            .Select(sensorEvent => sensorEvent.EventIdentifier)
            .ToListAsync(ct);

    public async Task<IReadOnlyCollection<Guid>> GetExistingLuminaireStateChangeEventIdentifiersAsync(
        IReadOnlyCollection<Guid> eventIdentifiers,
        CancellationToken ct) =>
        await dbContext.LuminaireStateChanges
            .Where(stateChange => eventIdentifiers.Contains(stateChange.EventIdentifier))
            .Select(stateChange => stateChange.EventIdentifier)
            .ToListAsync(ct);

    public async Task<IReadOnlyDictionary<Guid, LuminaireState>> GetLuminaireStatesAsync(
        IReadOnlyCollection<Guid> luminaireIds,
        CancellationToken ct) =>
        await dbContext.LuminaireStates
            .Where(state => luminaireIds.Contains(state.LuminaireId))
            .ToDictionaryAsync(state => state.LuminaireId, ct);

    public void AddSensorEvents(IEnumerable<SensorEvent> sensorEvents) =>
        dbContext.SensorEvents.AddRange(sensorEvents);

    public void AddLuminaireStateChanges(IEnumerable<LuminaireStateChange> stateChanges) =>
        dbContext.LuminaireStateChanges.AddRange(stateChanges);

    public void AddLuminaireStates(IEnumerable<LuminaireState> states) =>
        dbContext.LuminaireStates.AddRange(states);

    public Task SaveChangesAsync(CancellationToken ct) =>
        dbContext.SaveChangesAsync(ct);
}
