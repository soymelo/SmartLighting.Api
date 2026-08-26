using FastEndpoints;

namespace SmartLighting.Api.Features.Health;

public sealed class GetHealthEndpoint : EndpointWithoutRequest<GetHealthResponse>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/api/health");
        Tags("Health");
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        return Send.OkAsync(new GetHealthResponse("ok"), ct);
    }
}

public sealed record GetHealthResponse(string Status);
