using FastEndpoints;
using FluentValidation;

namespace SmartLighting.Api.Application.Features.Topology.GetTopology;

public sealed record GetTopologyRequest(Guid DeviceId);

public sealed class GetTopologyValidator : Validator<GetTopologyRequest>
{
    public GetTopologyValidator()
    {
        RuleFor(x => x.DeviceId).NotEmpty();
    }
}
