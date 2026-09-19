using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Queries;
using SmartLighting.Api.Application.Features.Devices.GetDeviceTelemetryStatus;

namespace SmartLighting.Api.Application.Queries.Devices;

public sealed class GetDeviceTelemetryStatusHandler(IDeviceQueryRepository deviceQueryRepository) 
    : ICommandHandler<GetDeviceTelemetryStatusQuery, GetDeviceTelemetryStatusResponse?>
{
    public async Task<GetDeviceTelemetryStatusResponse?> ExecuteAsync(GetDeviceTelemetryStatusQuery query, CancellationToken ct)
    {
        var status = await deviceQueryRepository.GetTelemetryStatusByIdAsync(query.DeviceId, ct);

        return status is null
            ? null
            : new GetDeviceTelemetryStatusResponse(status.DeviceId, status.LastTelemetryAtUtc);
    }
}
