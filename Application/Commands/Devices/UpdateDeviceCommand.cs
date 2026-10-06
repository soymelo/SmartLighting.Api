using FastEndpoints;
using SmartLighting.Api.Application.Features.Devices.UpdateDevice;

namespace SmartLighting.Api.Application.Commands.Devices;

public sealed class UpdateDeviceCommand(
    Guid deviceId,
    string name,
    IReadOnlyCollection<UpdateDeviceRequest.SensorItem> sensors,
    IReadOnlyCollection<UpdateDeviceRequest.LuminaireItem> luminaires)
    : ICommand<UpdateDeviceResponse>
{
    public Guid DeviceId { get; } = deviceId;
    public string Name { get; } = name;
    public IReadOnlyCollection<UpdateDeviceRequest.SensorItem> Sensors { get; } = sensors;
    public IReadOnlyCollection<UpdateDeviceRequest.LuminaireItem> Luminaires { get; } = luminaires;
}
