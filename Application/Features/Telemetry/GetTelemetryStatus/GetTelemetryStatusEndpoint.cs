using FastEndpoints;
using FastEndpoints.Swagger;

namespace SmartLighting.Api.Application.Features.Telemetry.GetTelemetryStatus;

public sealed class GetTelemetryStatusEndpoint(GetTelemetryStatusProcessor processor)
    : Endpoint<GetTelemetryStatusRequest, GetTelemetryStatusResponse>
{
    public override void Configure()
    {
        Get("/devices/{deviceId}/telemetry/status");
        Tags("Devices");
        Description(builder => builder.AutoTagOverride("Telemetry"));
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Obter último contato do dispositivo";
            summary.Description = "Retorna quando a API recebeu a última telemetria do dispositivo.";
        });
    }

    public override async Task HandleAsync(GetTelemetryStatusRequest req, CancellationToken ct)
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
