using MediatR;
using Stargate.Server.Controllers;
using Stargate.Server.Data.Models;
using Stargate.Server.Repositories;

namespace Stargate.Server.Business.Queries
{
    public class GetAstronautDutiesByName : IRequest<GetAstronautDutiesByNameResult>
    {
        public string Name { get; set; } = string.Empty;
    }

    public class GetAstronautDutiesByNameHandler : IRequestHandler<GetAstronautDutiesByName, GetAstronautDutiesByNameResult>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IAstronautRepository _astronautRepository;

        public GetAstronautDutiesByNameHandler(IPersonRepository personRepository, IAstronautRepository astronautRepository)
        {
            _personRepository = personRepository;
            _astronautRepository = astronautRepository;
        }

        public async Task<GetAstronautDutiesByNameResult> Handle(GetAstronautDutiesByName request, CancellationToken cancellationToken)
        {
            var result = new GetAstronautDutiesByNameResult();

            var person = await _personRepository.GetPersonAstronautByNameAsync(request.Name, cancellationToken);

            if (person is null)
            {
                result.Message = $"Person not found with name: {request.Name}";
                result.ResponseCode = 404;
                result.Success = false;
                return result;
            }

            result.Person = person;

            var duties = await _astronautRepository.GetDutiesByPersonIdAsync(person.PersonId, cancellationToken);       

            result.AstronautDuties = duties.ToList();

            return result;
        }
    }

    public class GetAstronautDutiesByNameResult : BaseResponse
    {
        public PersonAstronaut? Person { get; set; }
        public List<AstronautDuty> AstronautDuties { get; set; } = new List<AstronautDuty>();
    }
}
