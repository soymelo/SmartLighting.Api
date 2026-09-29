using FastEndpoints;
using SmartLighting.Api.Application.Features.Telemetry.GetTelemetryStatus;

namespace SmartLighting.Api.Application.Queries.Telemetry;

public sealed class GetTelemetryStatusQuery(Guid deviceId) : ICommand<GetTelemetryStatusResponse?>
{
    public Guid DeviceId { get; } = deviceId;
}
