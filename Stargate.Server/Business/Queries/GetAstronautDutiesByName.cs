using MediatR;
using Stargate.Server.Controllers;
using Stargate.Server.Data.Models;
using Stargate.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Stargate.Server.Business.Queries
{
    public class GetAstronautDutiesByName : IRequest<GetAstronautDutiesByNameResult>
    {
        public string Name { get; set; } = string.Empty;
    }

    public class GetAstronautDutiesByNameHandler : IRequestHandler<GetAstronautDutiesByName, GetAstronautDutiesByNameResult>
    {
        private readonly StargateContext _context;

        public GetAstronautDutiesByNameHandler(StargateContext context)
        {
            _context = context;
        }

        public async Task<GetAstronautDutiesByNameResult> Handle(GetAstronautDutiesByName request, CancellationToken cancellationToken)
        {
            var result = new GetAstronautDutiesByNameResult();

            var person = await _context.PersonAstronauts
                .FromSql($"""
                    SELECT a.Id as PersonId, a.Name, b.CurrentRank, b.CurrentDutyTitle, b.CareerStartDate, b.CareerEndDate 
                    FROM [Person] a 
                    LEFT JOIN [AstronautDetail] b on b.PersonId = a.Id 
                    WHERE a.Name = {request.Name}
                """).FirstOrDefaultAsync(cancellationToken);

            if (person is null)
            {
                result.Message = $"Person not found with name {request.Name}";
                result.ResponseCode = 404;
                result.Success = false;
                return result;
            }

            result.Person = person;

            var duties = await _context.AstronautDuties
                .FromSql($"""
                    SELECT * FROM [AstronautDuty] 
                    WHERE PersonId = {person.PersonId} 
                    Order By DutyStartDate Desc
                """).ToListAsync(cancellationToken);                

            result.AstronautDuties = duties;

            return result;
        }
    }

    public class GetAstronautDutiesByNameResult : BaseResponse
    {
        public PersonAstronaut? Person { get; set; }
        public List<AstronautDuty> AstronautDuties { get; set; } = new List<AstronautDuty>();
    }
}
