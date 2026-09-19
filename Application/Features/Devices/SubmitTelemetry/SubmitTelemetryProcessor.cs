using FastEndpoints;
using SmartLighting.Api.Application.Commands.Devices;

namespace SmartLighting.Api.Application.Features.Devices.SubmitTelemetry;

public sealed class SubmitTelemetryProcessor
{
    public Task<SubmitTelemetryResponse?> HandleAsync(
        Guid deviceId,
        SubmitTelemetryRequest request,
        CancellationToken ct)
    {
        var command = new SubmitTelemetryCommand(
            deviceId,
            request.SentAtUtc,
            request.SensorEvents,
            request.LuminaireStateChanges);

        return command.ExecuteAsync(ct);
    }
}
