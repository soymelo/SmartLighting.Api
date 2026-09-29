using FastEndpoints;
using SmartLighting.Api.Application.Commands.Telemetry;

namespace SmartLighting.Api.Application.Features.Telemetry.SubmitTelemetry;

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
