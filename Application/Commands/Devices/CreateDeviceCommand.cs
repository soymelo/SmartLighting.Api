using FastEndpoints;
using SmartLighting.Api.Application.Features.Devices.CreateDevice;

namespace SmartLighting.Api.Application.Commands.Devices;

public sealed class CreateDeviceCommand(
    Guid projectId,
    string name,
    IReadOnlyCollection<CreateDeviceRequest.SensorItem> sensors,
    IReadOnlyCollection<CreateDeviceRequest.LuminaireItem> luminaires)
    : ICommand<CreateDeviceCommandResult>
{
    public Guid ProjectId { get; } = projectId;
    public string Name { get; } = name;
    public IReadOnlyCollection<CreateDeviceRequest.SensorItem> Sensors { get; } = sensors;
    public IReadOnlyCollection<CreateDeviceRequest.LuminaireItem> Luminaires { get; } = luminaires;
}

public sealed record CreateDeviceCommandResult(
    CreateDeviceCommandStatus Status,
    CreateDeviceResponse? Response = null);

public enum CreateDeviceCommandStatus
{
    Created,
    ProjectNotFound,
    InvalidZone
}
