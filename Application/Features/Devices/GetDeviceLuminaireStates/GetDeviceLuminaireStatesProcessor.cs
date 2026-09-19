using FastEndpoints;
using SmartLighting.Api.Application.Queries.Devices;

namespace SmartLighting.Api.Application.Features.Devices.GetDeviceLuminaireStates;

public sealed class GetDeviceLuminaireStatesProcessor
{
    public Task<GetDeviceLuminaireStatesResponse?> HandleAsync(GetDeviceLuminaireStatesRequest request, CancellationToken ct)
    {
        var query = new GetDeviceLuminaireStatesQuery(request.DeviceId);

        return query.ExecuteAsync(ct);
    }
}
