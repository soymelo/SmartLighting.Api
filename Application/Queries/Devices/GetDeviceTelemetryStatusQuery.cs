using FastEndpoints;
using SmartLighting.Api.Application.Features.Devices.GetDeviceTelemetryStatus;

namespace SmartLighting.Api.Application.Queries.Devices;

public sealed class GetDeviceTelemetryStatusQuery(Guid deviceId) : ICommand<GetDeviceTelemetryStatusResponse?>
{
    public Guid DeviceId { get; } = deviceId;
}
