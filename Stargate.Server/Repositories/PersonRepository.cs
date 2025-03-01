using Microsoft.EntityFrameworkCore;
using Stargate.Server.Data;
using Stargate.Server.Data.Models;

namespace Stargate.Server.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly StargateContext _context;

    public PersonRepository(StargateContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(Person person, CancellationToken cancellationToken = default)
    {
        if (await ExistsByNameAsync(person.Name, cancellationToken))
        {
            return false;
        }

        await _context.People.AddAsync(person, cancellationToken);
        var created = await _context.SaveChangesAsync(cancellationToken);
        return created > 0;
    }   

    public async Task<IEnumerable<PersonAstronaut>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PersonAstronauts
                .FromSql($"""
                    SELECT a.Id as PersonId, a.Name, b.CurrentRank, b.CurrentDutyTitle, b.CareerStartDate, b.CareerEndDate 
                    FROM [Person] a 
                    LEFT JOIN [AstronautDetail] b on b.PersonId = a.Id
                """).ToListAsync(cancellationToken);
    }

    public async Task<PersonAstronaut?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.PersonAstronauts
                .FromSql($"""
                    SELECT a.Id as PersonId, a.Name, b.CurrentRank, b.CurrentDutyTitle, b.CareerStartDate, b.CareerEndDate 
                    FROM [Person] a 
                    LEFT JOIN [AstronautDetail] b on b.PersonId = a.Id
                    WHERE a.Name = {name}
                """).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> GetPersonIdByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.People.AsNoTracking()
            .Where(x => x.Name == name)
            .Select(p => p.Id)
            .FirstOrDefaultAsync(cancellationToken);            
    }    

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var person = (await _context.People.AsNoTracking()
            .Where(x => x.Name == name)
            .FirstOrDefaultAsync(cancellationToken));

        return (person is not null);
    }

    public async Task<bool> UpdateAsync(Person person, CancellationToken cancellationToken = default)
    {
        var personExists = await ExistsByIdAsync(person.Id, cancellationToken);
        if (!personExists)
        {
            return false;
        }

        _context.Update(person);
        var updated = await _context.SaveChangesAsync(cancellationToken);
        return updated > 0;
    }

    private async Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var person = await _context.People.AsNoTracking()
            .Where(x => x.Id == id)            
            .FirstOrDefaultAsync(cancellationToken);

        return (person != null);
    }    
}
