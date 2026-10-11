using FastEndpoints;
using FastEndpoints.Swagger;

namespace SmartLighting.Api.Application.Features.Dashboard.GetProjectSensorEvents;

public sealed class GetProjectSensorEventsEndpoint(GetProjectSensorEventsProcessor processor)
    : Endpoint<GetProjectSensorEventsRequest, GetProjectSensorEventsResponse>
{
    public override void Configure()
    {
        Get("/projects/{projectId}/sensor-events");
        Tags("Projects");
        Description(builder => builder.AutoTagOverride("Dashboard"));
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Listar eventos dos sensores do projeto";
            summary.Description = "Retorna uma página de eventos e o total, com ordenação definida pela API.";
        });
    }

    public override async Task HandleAsync(GetProjectSensorEventsRequest req, CancellationToken ct)
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
