using Stargate.Server.Data.Models;

namespace Stargate.Server.Repositories;

public interface IPersonRepository
{
    Task<bool> CreateAsync(Person person, CancellationToken cancellationToken = default);    

    Task<IEnumerable<PersonAstronaut>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<PersonAstronaut?> GetByNameAsync(string name, CancellationToken cancellationToken = default);    

    Task<int> GetPersonIdByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(Person person, CancellationToken cancellationToken = default);
}
