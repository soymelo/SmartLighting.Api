using FastEndpoints;
using SmartLighting.Api.Application.Features.Devices.GetDeviceLuminaireStates;

namespace SmartLighting.Api.Application.Queries.Devices;

public sealed class GetDeviceLuminaireStatesQuery(Guid deviceId)
    : ICommand<GetDeviceLuminaireStatesResponse?>
{
    public Guid DeviceId { get; } = deviceId;
}
