using FastEndpoints;
using SmartLighting.Api.Application.Features.Devices.GetRecentSensorEvents;

namespace SmartLighting.Api.Application.Queries.Devices;

public sealed class GetRecentSensorEventsQuery(Guid deviceId, int limit) : ICommand<GetRecentSensorEventsResponse?>
{
    public Guid DeviceId { get; } = deviceId;
    public int Limit { get; } = limit;
}
