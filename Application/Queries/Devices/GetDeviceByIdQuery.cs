using FastEndpoints;
using SmartLighting.Api.Application.Features.Devices.GetDeviceById;

namespace SmartLighting.Api.Application.Queries.Devices;

public sealed class GetDeviceByIdQuery(Guid deviceId) : ICommand<GetDeviceByIdResponse?>
{
    public Guid DeviceId { get; } = deviceId;
}
