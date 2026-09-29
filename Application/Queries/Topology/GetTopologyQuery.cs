using FastEndpoints;
using SmartLighting.Api.Application.Features.Topology.GetTopology;

namespace SmartLighting.Api.Application.Queries.Topology;

public sealed class GetTopologyQuery(Guid deviceId) : ICommand<GetTopologyResponse?>
{
    public Guid DeviceId { get; } = deviceId;
}
