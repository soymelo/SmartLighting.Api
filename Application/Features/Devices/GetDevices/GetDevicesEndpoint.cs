using FastEndpoints;

namespace SmartLighting.Api.Application.Features.Devices.GetDevices;

public sealed class GetDevicesEndpoint(GetDevicesProcessor processor)
    : EndpointWithoutRequest<GetDevicesResponse>
{
    public override void Configure()
    {
        Get("/devices");
        Tags("Devices");
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Listar dispositivos";
            summary.Description = "Retorna os dispositivos cadastrados e o último contato de cada um.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await processor.HandleAsync(ct);

        await Send.OkAsync(response, ct);
    }
}
