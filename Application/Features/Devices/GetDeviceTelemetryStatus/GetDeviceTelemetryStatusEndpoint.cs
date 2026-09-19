using FastEndpoints;

namespace SmartLighting.Api.Application.Features.Devices.GetDeviceTelemetryStatus;

public sealed class GetDeviceTelemetryStatusEndpoint(GetDeviceTelemetryStatusProcessor processor)
    : Endpoint<GetDeviceTelemetryStatusRequest, GetDeviceTelemetryStatusResponse>
{
    public override void Configure()
    {
        Get("/devices/{deviceId}/telemetry/status");
        Tags("Devices");
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Obter último contato do dispositivo";
            summary.Description = "Retorna quando a API recebeu a última telemetria do dispositivo.";
        });
    }

    public override async Task HandleAsync(GetDeviceTelemetryStatusRequest req, CancellationToken ct)
    {
        var response = await processor.HandleAsync(req, ct);

        if (response is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(response, ct);
    }
}
