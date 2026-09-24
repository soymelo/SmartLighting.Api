using FastEndpoints;

namespace SmartLighting.Api.Application.Features.Devices.GetDeviceDashboard;

public sealed class GetDeviceDashboardEndpoint(GetDeviceDashboardProcessor processor)
    : Endpoint<GetDeviceDashboardRequest, GetDeviceDashboardResponse>
{
    public override void Configure()
    {
        Get("/devices/{deviceId}/dashboard");
        Tags("Devices");
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Obter dashboard do dispositivo";
            summary.Description = "Retorna os dados do dispositivo preparados para exibição no dashboard.";
        });
    }

    public override async Task HandleAsync(GetDeviceDashboardRequest req, CancellationToken ct)
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
