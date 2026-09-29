using FastEndpoints;
using SmartLighting.Api.Application.Queries.Topology;

namespace SmartLighting.Api.Application.Features.Topology.GetTopology;

public sealed class GetTopologyProcessor
{
    public Task<GetTopologyResponse?> HandleAsync(GetTopologyRequest req, CancellationToken ct)
    {
        var query = new GetTopologyQuery(req.DeviceId);

        return query.ExecuteAsync(ct);
    }
}
