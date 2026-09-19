using FastEndpoints;

namespace SmartLighting.Api.Application.Features.Devices.GetDeviceLuminaireStates;

public sealed class GetDeviceLuminaireStatesEndpoint(GetDeviceLuminaireStatesProcessor processor)
    : Endpoint<GetDeviceLuminaireStatesRequest, GetDeviceLuminaireStatesResponse>
{
    public override void Configure()
    {
        Get("/devices/{deviceId}/telemetry/luminaire-states");
        Tags("Devices");
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Obter estados atuais das luminárias";
            summary.Description = "Retorna o último brilho conhecido de cada luminária do dispositivo.";
        });
    }

    public override async Task HandleAsync(GetDeviceLuminaireStatesRequest req, CancellationToken ct)
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
