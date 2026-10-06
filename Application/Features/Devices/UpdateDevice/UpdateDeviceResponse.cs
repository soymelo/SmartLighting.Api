namespace SmartLighting.Api.Application.Features.Devices.UpdateDevice;

public abstract record UpdateDeviceResponse;

public sealed record DeviceUpdatedResponse(
    Guid DeviceId,
    string Name,
    int SensorCount,
    int LuminaireCount) : UpdateDeviceResponse;

public sealed record UpdateDeviceNotFoundResponse : UpdateDeviceResponse;

public sealed record UpdateDeviceInvalidZoneResponse : UpdateDeviceResponse;

public sealed record UpdateDeviceMissingSlotsResponse : UpdateDeviceResponse;
