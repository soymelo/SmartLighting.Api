namespace SmartLighting.Api.Application.Features.Projects.GetProjects;

public sealed record GetProjectsResponse(IReadOnlyCollection<GetProjectsResponse.ProjectResponse> Projects)
{
    public sealed record ProjectResponse(Guid ProjectId, string Name);
}
