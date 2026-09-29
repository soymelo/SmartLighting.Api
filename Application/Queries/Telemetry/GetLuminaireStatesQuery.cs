using FastEndpoints;
using SmartLighting.Api.Application.Features.Telemetry.GetLuminaireStates;

namespace SmartLighting.Api.Application.Queries.Telemetry;

public sealed class GetLuminaireStatesQuery(Guid deviceId)
    : ICommand<GetLuminaireStatesResponse?>
{
    public Guid DeviceId { get; } = deviceId;
}
