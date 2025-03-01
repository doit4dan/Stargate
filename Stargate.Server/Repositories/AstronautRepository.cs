using MediatR;
using Microsoft.EntityFrameworkCore;
using Stargate.Server.Data;
using Stargate.Server.Data.Models;
using System;
using System.Threading;

namespace Stargate.Server.Repositories;

public class AstronautRepository : IAstronautRepository
{
    private readonly StargateContext _context;
    
    public AstronautRepository(StargateContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateDetailAsync(AstronautDetail detail, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(detail, cancellationToken);
        var created = await _context.SaveChangesAsync(cancellationToken);
        return created > 0;
    }    

    public async Task<AstronautDetail?> GetDetailByPersonIdAsync(int personId, CancellationToken cancellationToken = default)
    {
        return await _context.AstronautDetails
            .FirstOrDefaultAsync(x => x.PersonId == personId, cancellationToken);
    }

    public async Task<bool> UpdateDetailAsync(AstronautDetail detail, CancellationToken cancellationToken = default)
    {
        _context.AstronautDetails.Update(detail);
        var updated = await _context.SaveChangesAsync(cancellationToken);
        return updated > 0;
    }

    public async Task<bool> CreateDutyAsync(AstronautDuty duty, CancellationToken cancellationToken = default)
    {
        var astronautDuty = await GetLastDutyByPersonIdAsync(duty.PersonId, cancellationToken);

        // set end date for previous astronaut duty ( if applicable )
        if (astronautDuty != null)
        {
            astronautDuty.DutyEndDate = duty.DutyStartDate.AddDays(-1).Date;
            _context.AstronautDuties.Update(astronautDuty);
        }

        // add new duty record
        await _context.AstronautDuties.AddAsync(duty, cancellationToken);
        var created = await _context.SaveChangesAsync(cancellationToken);
        return created > 0;
    }

    public async Task<IEnumerable<AstronautDuty>> GetDutiesByPersonIdAsync(int personId, CancellationToken cancellationToken = default)
    {
        return await _context.AstronautDuties
            .Where(x => x.PersonId == personId)
            .OrderByDescending(o => o.DutyStartDate)
            .ToListAsync(cancellationToken);
    }   

    private async Task<AstronautDuty?> GetLastDutyByPersonIdAsync(int personId, CancellationToken cancellationToken = default)
    {
        return await _context.AstronautDuties
                .Where(d => d.PersonId == personId)
                .OrderByDescending(o => o.DutyStartDate)
                .FirstOrDefaultAsync(cancellationToken);
    }
}
