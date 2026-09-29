using FastEndpoints;

namespace SmartLighting.Api.Application.Features.Tests;

public class PingEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/ping");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await Send.OkAsync("API funcionando", ct);
    }
}