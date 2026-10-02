using FastEndpoints;
using SmartLighting.Api.Application.Queries.Projects;

namespace SmartLighting.Api.Application.Features.Projects.GetProjects;

public sealed class GetProjectsProcessor
{
    public Task<GetProjectsResponse> HandleAsync(CancellationToken ct)
    {
        var query = new GetProjectsQuery();

        return query.ExecuteAsync(ct);
    }
}
