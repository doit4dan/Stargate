using MediatR;
using Stargate.Server.Controllers;
using Stargate.Server.Data.Models;
using Stargate.Server.Repositories;

namespace Stargate.Server.Business.Queries
{
    public class GetPersonByName : IRequest<GetPersonByNameResult>
    {
        public required string Name { get; set; } = string.Empty;
    }

    public class GetPersonByNameHandler : IRequestHandler<GetPersonByName, GetPersonByNameResult>
    {
        public readonly IPersonRepository _personRepository;
        public GetPersonByNameHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<GetPersonByNameResult> Handle(GetPersonByName request, CancellationToken cancellationToken)
        {
            var result = new GetPersonByNameResult();

            var person = await _personRepository.GetByNameAsync(request.Name, cancellationToken);
            
            if (person is null)
            {
                result.Message = $"Person not found with name: {request.Name}";
                result.ResponseCode = 404;
                result.Success = false;
                return result;
            }

            result.Person = person;

            return result;
        }
    }

    public class GetPersonByNameResult : BaseResponse
    {
        public PersonAstronaut? Person { get; set; }
    }
}
