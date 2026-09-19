namespace SmartLighting.Api.Application.Features.Devices.SubmitTelemetry;

public sealed record SubmitTelemetryResponse
(
    int SensorEventsRecorded,
    int DuplicateSensorEventsIgnored,
    int LuminaireStateChangesRecorded,
    int DuplicateLuminaireStateChangesIgnored,
    int LuminaireStatesUpdated,
    DateTime ReceivedAtUtc
);
