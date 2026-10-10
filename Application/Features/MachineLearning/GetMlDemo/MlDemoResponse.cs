namespace SmartLighting.Api.Application.Features.MachineLearning.GetMlDemo;

public sealed record MlDemoResponse(
    string Source,
    string ModelVersion,
    MlDemoDatasetResponse? Dataset,
    IReadOnlyCollection<MlDemoMetricResponse> Metrics,
    IReadOnlyCollection<MlDemoExampleResponse> Examples);

public sealed record MlDemoDatasetResponse(
    int TrainingWindows,
    int ValidationWindows,
    int TestWindows,
    int SensorCount);

public sealed record MlDemoMetricResponse(
    string Detector,
    int TruePositives,
    int FalsePositives,
    int TrueNegatives,
    int FalseNegatives);

public sealed record MlDemoExampleResponse(
    string SensorId,
    DateTime WindowStartUtc,
    int EventCount5m,
    double AnomalyScore,
    bool IsAnomaly,
    bool ExpectedAnomaly,
    string Selection);
