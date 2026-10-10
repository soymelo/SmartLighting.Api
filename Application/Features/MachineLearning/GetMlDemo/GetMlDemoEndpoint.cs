using FastEndpoints;
using System.Text.Json;

namespace SmartLighting.Api.Application.Features.MachineLearning.GetMlDemo;

public sealed class GetMlDemoEndpoint(MlDemoClient mlDemoClient)
    : EndpointWithoutRequest<MlDemoResponse>
{
    public override void Configure()
    {
        Get("/ml/demo");
        Tags("Machine Learning");
        AllowAnonymous();

        Summary(summary =>
        {
            summary.Summary = "Consultar demonstração experimental do modelo";
            summary.Description = "Analisa janelas sintéticas no serviço Python; não representa alertas reais da maquete.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            var result = await mlDemoClient.GetAsync(ct);
            if (result is null || result.Source != "simulado" || result.Dataset is null || result.Metrics is not { Count: > 0 })
            {
                await Send.ResponseAsync(Unavailable(), StatusCodes.Status502BadGateway, ct);
                return;
            }

            await Send.OkAsync(result, ct);
        }
        catch (HttpRequestException)
        {
            await Send.ResponseAsync(Unavailable(), StatusCodes.Status503ServiceUnavailable, ct);
        }
        catch (TaskCanceledException) when (!ct.IsCancellationRequested)
        {
            await Send.ResponseAsync(Unavailable(), StatusCodes.Status503ServiceUnavailable, ct);
        }
        catch (JsonException)
        {
            await Send.ResponseAsync(Unavailable(), StatusCodes.Status502BadGateway, ct);
        }
    }

    private static MlDemoResponse Unavailable() => new("indisponível", "", null, [], []);
}
