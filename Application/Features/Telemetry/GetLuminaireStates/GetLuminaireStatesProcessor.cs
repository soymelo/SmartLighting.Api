using FastEndpoints;
using SmartLighting.Api.Application.Queries.Telemetry;

namespace SmartLighting.Api.Application.Features.Telemetry.GetLuminaireStates;

public sealed class GetLuminaireStatesProcessor
{
    public Task<GetLuminaireStatesResponse?> HandleAsync(GetLuminaireStatesRequest request, CancellationToken ct)
    {
        var query = new GetLuminaireStatesQuery(request.DeviceId);

        return query.ExecuteAsync(ct);
    }
}
