using FastEndpoints;
using FastEndpoints.Swagger;

namespace SmartLighting.Api.Application.Features.Telemetry.GetLuminaireStates;

public sealed class GetLuminaireStatesEndpoint(GetLuminaireStatesProcessor processor)
    : Endpoint<GetLuminaireStatesRequest, GetLuminaireStatesResponse>
{
    public override void Configure()
    {
        Get("/devices/{deviceId}/telemetry/luminaire-states");
        Tags("Devices");
        Description(builder => builder.AutoTagOverride("Telemetry"));
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Obter estados atuais das luminárias";
            summary.Description = "Retorna o último brilho conhecido de cada luminária do dispositivo.";
        });
    }

    public override async Task HandleAsync(GetLuminaireStatesRequest req, CancellationToken ct)
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
