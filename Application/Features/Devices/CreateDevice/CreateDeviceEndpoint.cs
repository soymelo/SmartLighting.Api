using FastEndpoints;
using SmartLighting.Api.Application.Commands.Devices;
using SmartLighting.Api.Application.Features.Devices.GetDeviceById;

namespace SmartLighting.Api.Application.Features.Devices.CreateDevice;

public sealed class CreateDeviceEndpoint(CreateDeviceProcessor processor)
    : Endpoint<CreateDeviceRequest, CreateDeviceResponse>
{
    public override void Configure()
    {
        Post("/projects/{projectId}/devices");
        Tags("Projects");
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Cadastrar dispositivo no projeto";
            summary.Description = "Cria o dispositivo, seus sensores e suas luminárias em uma única operação.";
        });
    }

    public override async Task HandleAsync(CreateDeviceRequest req, CancellationToken ct)
    {
        var result = await processor.HandleAsync(req, ct);

        switch (result.Status)
        {
            case CreateDeviceCommandStatus.ProjectNotFound:
                await Send.NotFoundAsync(ct);
                return;

            case CreateDeviceCommandStatus.InvalidZone:
                AddError("Todas as zonas informadas devem pertencer ao projeto do dispositivo.");
                await Send.ErrorsAsync(cancellation: ct);
                return;

            case CreateDeviceCommandStatus.Created when result.Response is not null:
                await Send.CreatedAtAsync<GetDeviceByIdEndpoint>(
                    new { DeviceId = result.Response.DeviceId },
                    result.Response,
                    cancellation: ct);
                return;

            default:
                throw new InvalidOperationException("O cadastro do dispositivo retornou um resultado inválido.");
        }
    }
}
