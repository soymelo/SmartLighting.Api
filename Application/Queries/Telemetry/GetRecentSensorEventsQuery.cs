using FastEndpoints;
using SmartLighting.Api.Application.Features.Telemetry.GetRecentSensorEvents;

namespace SmartLighting.Api.Application.Queries.Telemetry;

public sealed class GetRecentSensorEventsQuery(Guid deviceId, int limit) : ICommand<GetRecentSensorEventsResponse?>
{
    public Guid DeviceId { get; } = deviceId;
    public int Limit { get; } = limit;
}
