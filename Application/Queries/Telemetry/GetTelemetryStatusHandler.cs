using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Queries.Telemetry;
using SmartLighting.Api.Application.Features.Telemetry.GetTelemetryStatus;

namespace SmartLighting.Api.Application.Queries.Telemetry;

public sealed class GetTelemetryStatusHandler(ITelemetryQueryRepository telemetryQueryRepository) 
    : ICommandHandler<GetTelemetryStatusQuery, GetTelemetryStatusResponse?>
{
    public async Task<GetTelemetryStatusResponse?> ExecuteAsync(GetTelemetryStatusQuery query, CancellationToken ct)
    {
        var status = await telemetryQueryRepository.GetTelemetryStatusByIdAsync(query.DeviceId, ct);

        return status is null
            ? null
            : new GetTelemetryStatusResponse(status.DeviceId, status.LastTelemetryAtUtc);
    }
}
