using FastEndpoints;

namespace SmartLighting.Api.Application.Features.Devices.GetDeviceById;

public sealed class GetDeviceByIdEndpoint(GetDeviceByIdProcessor processor)
    : Endpoint<GetDeviceByIdRequest, GetDeviceByIdResponse>
{
    public override void Configure()
    {
        Get("/devices/{deviceId}");
        Tags("Devices");
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Obter detalhes do dispositivo";
            summary.Description = "Retorna o controlador, seu projeto, último contato e componentes por zona.";
        });
    }

    public override async Task HandleAsync(GetDeviceByIdRequest req, CancellationToken ct)
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
