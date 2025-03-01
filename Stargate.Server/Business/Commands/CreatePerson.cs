using MediatR.Pipeline;
using MediatR;
using Stargate.Server.Controllers;
using Stargate.Server.Data.Models;
using Stargate.Server.Repositories;
using FluentValidation;

namespace Stargate.Server.Business.Commands
{
    public class CreatePerson : IRequest<CreatePersonResult>
    {
        public required string Name { get; set; } = string.Empty;
    }

    public class CreatePersonPreProcessor : IRequestPreProcessor<CreatePerson>
    {
        private readonly IValidator<CreatePerson> _createPersonValidator;
        public CreatePersonPreProcessor(IValidator<CreatePerson> createPersonValidator)
        {
            _createPersonValidator = createPersonValidator;
        }
        public async Task Process(CreatePerson request, CancellationToken cancellationToken)
        {
            await _createPersonValidator.ValidateAndThrowAsync(request);      
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
