using System.Net.Http.Json;

namespace SmartLighting.Api.Application.Features.MachineLearning.GetMlDemo;

public sealed class MlDemoClient(HttpClient httpClient)
{
    public async Task<MlDemoResponse?> GetAsync(CancellationToken ct) =>
        await httpClient.GetFromJsonAsync<MlDemoResponse>("demo", ct);
}
