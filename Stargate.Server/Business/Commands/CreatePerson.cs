using MediatR.Pipeline;
using MediatR;
using Stargate.Server.Controllers;
using Stargate.Server.Data.Models;
using Stargate.Server.Data;
using Microsoft.EntityFrameworkCore;
using Stargate.Server.Repositories;

namespace Stargate.Server.Business.Commands
{
    public class CreatePerson : IRequest<CreatePersonResult>
    {
        public required string Name { get; set; } = string.Empty;
    }

    public class CreatePersonPreProcessor : IRequestPreProcessor<CreatePerson>
    {
        private readonly IPersonRepository _personRepository;
        public CreatePersonPreProcessor(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }
        public async Task Process(CreatePerson request, CancellationToken cancellationToken)
        {
            var personExists = await _personRepository.ExistsByNameAsync(request.Name, cancellationToken);
            if (personExists) throw new BadHttpRequestException($"Person already exists with this name: {request.Name}");            
        }
    }

    public class CreatePersonHandler : IRequestHandler<CreatePerson, CreatePersonResult>
    {
        private readonly IPersonRepository _personRepository;

        public CreatePersonHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }
        public async Task<CreatePersonResult> Handle(CreatePerson request, CancellationToken cancellationToken)
        {

            var newPerson = new Person()
            {
                Name = request.Name
            };

            var success = await _personRepository.CreateAsync(newPerson);

            return new CreatePersonResult()
            {
                Id = newPerson.Id,
                Success = success
            };
        }
    }

    public class CreatePersonResult : BaseResponse
    {
        public int Id { get; set; }
    }
}
