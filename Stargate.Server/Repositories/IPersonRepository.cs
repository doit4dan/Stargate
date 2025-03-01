using Stargate.Server.Data.Models;

namespace Stargate.Server.Repositories;

public interface IPersonRepository
{
    Task<bool> CreateAsync(Person person, CancellationToken cancellationToken = default);    

    Task<IEnumerable<PersonAstronaut>> GetPersonAstronautAllAsync(CancellationToken cancellationToken = default);

    Task<PersonAstronaut?> GetPersonAstronautByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<Person?> GetPersonByNameAsync(string name, CancellationToken cancellationToken = default);
    
    Task<int> GetPersonIdByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(Person person, CancellationToken cancellationToken = default);
}
