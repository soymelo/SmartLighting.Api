using FastEndpoints;
using SmartLighting.Api.Application.Features.Devices.SubmitTelemetry;

namespace SmartLighting.Api.Application.Commands.Devices;

public sealed class SubmitTelemetryCommand(
    Guid deviceId,
    DateTime sentAtUtc,
    IReadOnlyCollection<SubmitTelemetryRequest.SensorEventItem> sensorEvents,
    IReadOnlyCollection<SubmitTelemetryRequest.LuminaireStateChangeItem> luminaireStateChanges)
    : ICommand<SubmitTelemetryResponse?>
{
    public Guid DeviceId { get; } = deviceId;
    public DateTime SentAtUtc { get; } = sentAtUtc;
    public IReadOnlyCollection<SubmitTelemetryRequest.SensorEventItem> SensorEvents { get; } = sensorEvents;
    public IReadOnlyCollection<SubmitTelemetryRequest.LuminaireStateChangeItem> LuminaireStateChanges { get; } = luminaireStateChanges;
}
