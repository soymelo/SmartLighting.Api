using FastEndpoints;
using FastEndpoints.Swagger;

namespace SmartLighting.Api.Application.Features.Dashboard.GetDashboard;

public sealed class GetDashboardEndpoint(GetDashboardProcessor processor)
    : Endpoint<GetDashboardRequest, GetDashboardResponse>
{
    public override void Configure()
    {
        Get("/devices/{deviceId}/dashboard");
        Tags("Devices");
        Description(builder => builder.AutoTagOverride("Dashboard"));
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Obter dashboard do dispositivo";
            summary.Description = "Retorna os dados do dispositivo preparados para exibição no dashboard.";
        });
    }

    public override async Task HandleAsync(GetDashboardRequest req, CancellationToken ct)
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
