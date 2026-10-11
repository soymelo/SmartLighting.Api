using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Queries.Dashboard;
using SmartLighting.Api.Application.Features.Dashboard.GetProjectSensorEvents;

namespace SmartLighting.Api.Application.Queries.Dashboard;

public sealed class GetProjectSensorEventsHandler(IProjectSensorEventsQueryRepository repository)
    : ICommandHandler<GetProjectSensorEventsQuery, GetProjectSensorEventsResponse?>
{
    public async Task<GetProjectSensorEventsResponse?> ExecuteAsync(GetProjectSensorEventsQuery query, CancellationToken ct)
    {
        var page = await repository.GetPageAsync(
            query.ProjectId, query.Page, query.PageSize, query.SortBy, query.Descending, ct);

        return page is null
            ? null
            : new GetProjectSensorEventsResponse(
                query.Page,
                query.PageSize,
                page.TotalCount,
                page.Items.Select(item => new GetProjectSensorEventsResponse.SensorEventResponse(
                    item.EventIdentifier,
                    item.SensorId,
                    item.DeviceId,
                    item.SensorName,
                    item.ZoneId,
                    item.ZoneName,
                    item.OccurredAtUtc)).ToList());
    }
}
