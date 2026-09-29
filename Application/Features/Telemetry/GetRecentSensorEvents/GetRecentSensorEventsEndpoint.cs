using FastEndpoints;
using FastEndpoints.Swagger;

namespace SmartLighting.Api.Application.Features.Telemetry.GetRecentSensorEvents;

public sealed class GetRecentSensorEventsEndpoint(GetRecentSensorEventsProcessor processor)
    : Endpoint<GetRecentSensorEventsRequest, GetRecentSensorEventsResponse>
{
    public override void Configure()
    {
        Get("/devices/{deviceId}/telemetry/sensor-events");
        Tags("Devices");
        Description(builder => builder.AutoTagOverride("Telemetry"));
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Obter eventos recentes dos sensores";
            summary.Description = "Retorna as detecções mais recentes dos sensores do dispositivo.";
        });
    }

    public override async Task HandleAsync(GetRecentSensorEventsRequest req, CancellationToken ct)
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
