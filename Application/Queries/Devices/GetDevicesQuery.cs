using FastEndpoints;
using SmartLighting.Api.Application.Features.Devices.GetDevices;

namespace SmartLighting.Api.Application.Queries.Devices;

public sealed class GetDevicesQuery : ICommand<GetDevicesResponse>;
