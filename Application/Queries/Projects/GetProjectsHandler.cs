using FastEndpoints;
using SmartLighting.Api.Application.Abstractions.Queries.Projects;
using SmartLighting.Api.Application.Features.Projects.GetProjects;

namespace SmartLighting.Api.Application.Queries.Projects;

public sealed class GetProjectsHandler(IProjectQueryRepository projectQueryRepository)
    : ICommandHandler<GetProjectsQuery, GetProjectsResponse>
{
    public async Task<GetProjectsResponse> ExecuteAsync(GetProjectsQuery query, CancellationToken ct)
    {
        var projects = await projectQueryRepository.GetAllAsync(ct);
        var responses = projects
            .Select(project => new GetProjectsResponse.ProjectResponse(
                project.ProjectId,
                project.Name))
            .ToList();

        return new GetProjectsResponse(responses);
    }
}
