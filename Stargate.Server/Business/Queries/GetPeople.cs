using MediatR;
using Stargate.Server.Controllers;
using Stargate.Server.Data.Models;
using Stargate.Server.Repositories;

namespace Stargate.Server.Business.Queries
{
    public class GetPeople : IRequest<GetPeopleResult>
    {

    }

    public class GetPeopleHandler : IRequestHandler<GetPeople, GetPeopleResult>
    {
        public readonly IPersonRepository _personRepository;
        
        public GetPeopleHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }
        
        public async Task<GetPeopleResult> Handle(GetPeople request, CancellationToken cancellationToken)
        {
            var ppl = await _personRepository.GetAllAsync(cancellationToken);
            return new GetPeopleResult()
            {
                People = ppl.ToList()
            };                        
        }
    }

    public class GetPeopleResult : BaseResponse
    {
        public List<PersonAstronaut> People { get; set; } = new List<PersonAstronaut> { };

    }
}
