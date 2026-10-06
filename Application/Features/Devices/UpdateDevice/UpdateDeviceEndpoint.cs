using FastEndpoints;

namespace SmartLighting.Api.Application.Features.Devices.UpdateDevice;

public sealed class UpdateDeviceEndpoint(UpdateDeviceProcessor processor)
    : Endpoint<UpdateDeviceRequest, DeviceUpdatedResponse>
{
    public override void Configure()
    {
        Put("/devices/{deviceId}");
        Tags("Devices");
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Atualizar dispositivo";
            summary.Description = "Atualiza o dispositivo e seus componentes, preservando os slots já cadastrados.";
        });
    }

    public override async Task HandleAsync(UpdateDeviceRequest req, CancellationToken ct)
    {
        var response = await processor.HandleAsync(req, ct);

        switch (response)
        {
            case UpdateDeviceNotFoundResponse:
                await Send.NotFoundAsync(ct);
                return;

            case UpdateDeviceInvalidZoneResponse:
                AddError("Todas as zonas informadas devem pertencer ao projeto do dispositivo.");
                await Send.ErrorsAsync(cancellation: ct);
                return;

            case UpdateDeviceMissingSlotsResponse:
                AddError("Os slots já cadastrados não podem ser removidos do dispositivo.");
                await Send.ErrorsAsync(cancellation: ct);
                return;

            case DeviceUpdatedResponse updated:
                await Send.OkAsync(updated, ct);
                return;

            default:
                throw new InvalidOperationException("A atualização do dispositivo retornou um resultado inválido.");
        }
    }
}
