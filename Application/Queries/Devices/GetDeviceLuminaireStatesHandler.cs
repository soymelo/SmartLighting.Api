using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Queries;
using SmartLighting.Api.Application.Features.Devices.GetDeviceLuminaireStates;

namespace SmartLighting.Api.Application.Queries.Devices;

public sealed class GetDeviceLuminaireStatesHandler(IDeviceQueryRepository deviceQueryRepository)
    : ICommandHandler<GetDeviceLuminaireStatesQuery, GetDeviceLuminaireStatesResponse?>
{
    public async Task<GetDeviceLuminaireStatesResponse?> ExecuteAsync(
        GetDeviceLuminaireStatesQuery query,
        CancellationToken ct)
    {
        var states = await deviceQueryRepository.GetLuminaireStatesByDeviceIdAsync(query.DeviceId, ct);

        if (states is null)
        {
            return null;
        }

        var luminaires = states.Luminaires
            .Select(luminaire => new GetDeviceLuminaireStatesResponse.LuminaireResponse(
                luminaire.LuminaireId,
                luminaire.BrightnessPercentage,
                luminaire.UpdatedAtUtc)).ToList();

        return new GetDeviceLuminaireStatesResponse(states.DeviceId, luminaires);
    }
}
