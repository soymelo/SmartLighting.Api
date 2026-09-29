using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Queries.Telemetry;
using SmartLighting.Api.Application.Features.Telemetry.GetLuminaireStates;

namespace SmartLighting.Api.Application.Queries.Telemetry;

public sealed class GetLuminaireStatesHandler(ITelemetryQueryRepository telemetryQueryRepository)
    : ICommandHandler<GetLuminaireStatesQuery, GetLuminaireStatesResponse?>
{
    public async Task<GetLuminaireStatesResponse?> ExecuteAsync(
        GetLuminaireStatesQuery query,
        CancellationToken ct)
    {
        var states = await telemetryQueryRepository.GetLuminaireStatesByDeviceIdAsync(query.DeviceId, ct);

        if (states is null)
        {
            return null;
        }

        var luminaires = states.Luminaires
            .Select(luminaire => new GetLuminaireStatesResponse.LuminaireResponse(
                luminaire.LuminaireId,
                luminaire.BrightnessPercentage,
                luminaire.UpdatedAtUtc)).ToList();

        return new GetLuminaireStatesResponse(states.DeviceId, luminaires);
    }
}
