using FastEndpoints;

namespace SmartLighting.Api.Application.Features.Devices.GetDeviceTopology;

public sealed class GetDeviceTopologyEndpoint(GetDeviceTopologyProcessor processor)
    : Endpoint<GetDeviceTopologyRequest, GetDeviceTopologyResponse>
{
    public override void Configure()
    {
        Get("/devices/{deviceId}/topology");
        Tags("Devices");
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Obter topologia do dispositivo";
            summary.Description = "Retorna as zonas, sensores e luminárias configurados para um dispositivo.";
        });
    }

    public override async Task HandleAsync(GetDeviceTopologyRequest req, CancellationToken ct)
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
