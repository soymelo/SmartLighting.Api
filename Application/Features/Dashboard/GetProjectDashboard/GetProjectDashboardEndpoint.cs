using FastEndpoints;
using FastEndpoints.Swagger;

namespace SmartLighting.Api.Application.Features.Dashboard.GetProjectDashboard;

public sealed class GetProjectDashboardEndpoint(GetProjectDashboardProcessor processor)
    : Endpoint<GetProjectDashboardRequest, GetProjectDashboardResponse>
{
    public override void Configure()
    {
        Get("/projects/{projectId}/dashboard");
        Tags("Projects");
        Description(builder => builder.AutoTagOverride("Dashboard"));
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Obter dashboard do projeto";
            summary.Description = "Retorna os controladores, a topologia e a atividade por zona do projeto.";
        });
    }

    public override async Task HandleAsync(GetProjectDashboardRequest req, CancellationToken ct)
    {
        var response = await processor.HandleAsync(req, ct);

        if (response is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(response, ct);
    }
}
