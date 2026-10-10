using FastEndpoints;
using FastEndpoints.Swagger;

namespace SmartLighting.Api.Application.Features.Topology.GetTopology;

public sealed class GetTopologyEndpoint(GetTopologyProcessor processor)
    : Endpoint<GetTopologyRequest, GetTopologyResponse>
{
    public override void Configure()
    {
        Get("/devices/{deviceId}/topology");
        Tags("Devices");
        Description(builder => builder.AutoTagOverride("Topology"));
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Obter topologia do dispositivo";
            summary.Description = "Retorna as zonas do projeto e os sensores e luminárias participantes, com o dispositivo de cada componente, para controlar os LEDs locais.";
        });
    }

    public override async Task HandleAsync(GetTopologyRequest req, CancellationToken ct)
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
