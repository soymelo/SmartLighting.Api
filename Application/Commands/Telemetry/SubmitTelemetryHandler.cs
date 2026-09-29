using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Repositories.Telemetry;
using SmartLighting.Api.Application.Features.Telemetry.SubmitTelemetry;
using SmartLighting.Api.Domain.Entities;

namespace SmartLighting.Api.Application.Commands.Telemetry;

public sealed class SubmitTelemetryHandler(ITelemetryRepository telemetryRepository)
    : ICommandHandler<SubmitTelemetryCommand, SubmitTelemetryResponse?>
{
    public async Task<SubmitTelemetryResponse?> ExecuteAsync(SubmitTelemetryCommand command, CancellationToken ct)
    {
        var device = await telemetryRepository.GetDeviceByIdAsync(command.DeviceId, ct);

        if (device is null || !await HasValidTopologyAsync(command, ct))
        {
            return null;
        }

        var sensorEvents = await CreateNewSensorEventsAsync(command.SensorEvents, ct);
        var stateChanges = await CreateNewLuminaireStateChangesAsync(command.LuminaireStateChanges, ct);
        var luminaireStatesUpdated = await UpdateLuminaireStatesAsync(stateChanges, ct);
        var receivedAtUtc = DateTime.UtcNow;

        await SaveChangesAsync(device, sensorEvents, stateChanges, receivedAtUtc, ct);

        return new SubmitTelemetryResponse(
            sensorEvents.Count,
            command.SensorEvents.Count - sensorEvents.Count,
            stateChanges.Count,
            command.LuminaireStateChanges.Count - stateChanges.Count,
            luminaireStatesUpdated,
            receivedAtUtc);
    }

    private async Task<bool> HasValidTopologyAsync(SubmitTelemetryCommand command, CancellationToken ct)
    {
        var sensorIds = command.SensorEvents
            .Select(sensorEvent => sensorEvent.SensorId)
            .Distinct()
            .ToList();
        var luminaireIds = command.LuminaireStateChanges
            .Select(stateChange => stateChange.LuminaireId)
            .Distinct()
            .ToList();

        var sensorsBelongToDevice = await telemetryRepository.DoSensorsBelongToDeviceAsync(
            command.DeviceId,
            sensorIds,
            ct);
        var luminairesBelongToDevice = await telemetryRepository.DoLuminairesBelongToDeviceAsync(
            command.DeviceId,
            luminaireIds,
            ct);

        return sensorsBelongToDevice && luminairesBelongToDevice;
    }

    private async Task<List<SensorEvent>> CreateNewSensorEventsAsync(
        IReadOnlyCollection<SubmitTelemetryRequest.SensorEventItem> sensorEvents,
        CancellationToken ct)
    {
        var eventIdentifiers = sensorEvents
            .Select(sensorEvent => sensorEvent.EventIdentifier)
            .ToList();
        var existingEventIdentifiers = (await telemetryRepository.GetExistingSensorEventIdentifiersAsync(
            eventIdentifiers,
            ct)).ToHashSet();

        return sensorEvents
            .Where(sensorEvent => !existingEventIdentifiers.Contains(sensorEvent.EventIdentifier))
            .Select(sensorEvent => new SensorEvent
            {
                EventIdentifier = sensorEvent.EventIdentifier,
                SensorId = sensorEvent.SensorId,
                OccurredAtUtc = sensorEvent.OccurredAtUtc
            })
            .ToList();
    }

    private async Task<List<LuminaireStateChange>> CreateNewLuminaireStateChangesAsync(
        IReadOnlyCollection<SubmitTelemetryRequest.LuminaireStateChangeItem> stateChanges,
        CancellationToken ct)
    {
        var eventIdentifiers = stateChanges
            .Select(stateChange => stateChange.EventIdentifier)
            .ToList();
        var existingEventIdentifiers = (await telemetryRepository.GetExistingLuminaireStateChangeEventIdentifiersAsync(
            eventIdentifiers,
            ct)).ToHashSet();

        return stateChanges
            .Where(stateChange => !existingEventIdentifiers.Contains(stateChange.EventIdentifier))
            .Select(stateChange => new LuminaireStateChange
            {
                EventIdentifier = stateChange.EventIdentifier,
                LuminaireId = stateChange.LuminaireId,
                BrightnessPercentage = stateChange.BrightnessPercentage,
                ChangedAtUtc = stateChange.ChangedAtUtc
            })
            .ToList();
    }

    private async Task<int> UpdateLuminaireStatesAsync(
        IReadOnlyCollection<LuminaireStateChange> stateChanges,
        CancellationToken ct)
    {
        var luminaireIds = stateChanges
            .Select(stateChange => stateChange.LuminaireId)
            .Distinct()
            .ToList();
        var statesByLuminaireId = (await telemetryRepository.GetLuminaireStatesAsync(luminaireIds, ct))
            .ToDictionary(state => state.Key, state => state.Value);
        var statesToAdd = new List<LuminaireState>();
        var updatedLuminaireIds = new HashSet<Guid>();

        foreach (var stateChange in stateChanges
                     .OrderBy(stateChange => stateChange.LuminaireId)
                     .ThenBy(stateChange => stateChange.ChangedAtUtc))
        {
            if (statesByLuminaireId.TryGetValue(stateChange.LuminaireId, out var currentState))
            {
                if (currentState.TryUpdate(stateChange.BrightnessPercentage, stateChange.ChangedAtUtc))
                {
                    updatedLuminaireIds.Add(stateChange.LuminaireId);
                }

                continue;
            }

            var state = new LuminaireState
            {
                LuminaireId = stateChange.LuminaireId,
                BrightnessPercentage = stateChange.BrightnessPercentage,
                UpdatedAtUtc = stateChange.ChangedAtUtc
            };

            statesByLuminaireId.Add(state.LuminaireId, state);
            statesToAdd.Add(state);
            updatedLuminaireIds.Add(state.LuminaireId);
        }

        telemetryRepository.AddLuminaireStates(statesToAdd);

        return updatedLuminaireIds.Count;
    }

    private async Task SaveChangesAsync(
        Device device,
        IReadOnlyCollection<SensorEvent> sensorEvents,
        IReadOnlyCollection<LuminaireStateChange> stateChanges,
        DateTime receivedAtUtc,
        CancellationToken ct)
    {
        telemetryRepository.AddSensorEvents(sensorEvents);
        telemetryRepository.AddLuminaireStateChanges(stateChanges);

        device.LastTelemetryAtUtc = receivedAtUtc;

        await telemetryRepository.SaveChangesAsync(ct);
    }
}
