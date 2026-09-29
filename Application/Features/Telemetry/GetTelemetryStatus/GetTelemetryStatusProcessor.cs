using FastEndpoints;
using SmartLighting.Api.Application.Queries.Telemetry;

namespace SmartLighting.Api.Application.Features.Telemetry.GetTelemetryStatus;

public sealed class GetTelemetryStatusProcessor
{
    public Task<GetTelemetryStatusResponse?> HandleAsync(GetTelemetryStatusRequest request,CancellationToken ct)
    {
        var query = new GetTelemetryStatusQuery(request.DeviceId);

        return query.ExecuteAsync(ct);
    }
}
