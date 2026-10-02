using FastEndpoints;

namespace SmartLighting.Api.Application.Features.Projects.GetProjects;

public sealed class GetProjectsEndpoint(GetProjectsProcessor processor)
    : EndpointWithoutRequest<GetProjectsResponse>
{
    public override void Configure()
    {
        Get("/projects");
        Tags("Projects");
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Listar projetos";
            summary.Description = "Retorna os projetos cadastrados para seleção do dashboard.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await processor.HandleAsync(ct);

        await Send.OkAsync(response, ct);
    }
}
