using Stargate.Server.Data.Models;

namespace Stargate.Server.Repositories;

public interface IAstronautRepository
{
    Task<bool> CreateDetailAsync(AstronautDetail detail, CancellationToken cancellationToken = default);

    Task<bool> UpdateDetailAsync(AstronautDetail detail, CancellationToken cancellationToken = default);

    Task<AstronautDetail?> GetDetailByPersonIdAsync(int personId, CancellationToken cancellationToken = default);

    Task<bool> CreateDutyAsync(AstronautDuty duty, CancellationToken cancellationToken = default);

    Task<IEnumerable<AstronautDuty>> GetDutiesByPersonIdAsync(int personId, CancellationToken cancellationToken = default);
}
