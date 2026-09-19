using FastEndpoints;
namespace SmartLighting.Api.Application.Features.Devices.SubmitTelemetry;

public sealed class SubmitTelemetryEndpoint(SubmitTelemetryProcessor processor)
    : Endpoint<SubmitTelemetryRequest, SubmitTelemetryResponse>
{
    public override void Configure()
    {
        Post("/devices/{deviceId}/telemetry");
        Tags("Devices");
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Receber telemetria do dispositivo";
            summary.Description = "Recebe, em pequenos lotes, eventos de sensores e mudanças de estado das luminárias.";
        });
    }

    public override async Task HandleAsync(SubmitTelemetryRequest req, CancellationToken ct)
    {
        var response = await processor.HandleAsync(Route<Guid>("deviceId"), req, ct);

        if (response is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(response, ct);
    }
}
