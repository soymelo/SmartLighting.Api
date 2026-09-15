using SmartLighting.Api.Application.Features.Devices.GetDeviceTopology;

namespace SmartLighting.Api.Application.Abstractions.Repositories;

public interface IDeviceRepository
{
    Task<GetDeviceTopologyResponse?> GetTopologyByIdentifierAsync(string deviceIdentifier,CancellationToken ct);
}
