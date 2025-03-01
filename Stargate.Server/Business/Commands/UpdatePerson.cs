using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Stargate.Server.Controllers;
using Stargate.Server.Data;
using Stargate.Server.Repositories;
using Stargate.Server.Validators;

namespace Stargate.Server.Business.Commands;

public class UpdatePerson : IRequest<UpdatePersonResult>
{
    public string Name { get; set; }

    public string NewName { get; set; }
}

public class UpdatePersonPreProcessor : IRequestPreProcessor<UpdatePerson>
{
    private readonly IValidator<UpdatePerson> _updatePersonValidator;
    public UpdatePersonPreProcessor(IValidator<UpdatePerson> updatePersonValidator)
    {
        _updatePersonValidator = updatePersonValidator;
    }
    public async Task Process(UpdatePerson request, CancellationToken cancellationToken)
    {
        await _updatePersonValidator.ValidateAndThrowAsync(request);
    }
}

public class UpdatePersonHandler : IRequestHandler<UpdatePerson, UpdatePersonResult>
{
    private readonly IPersonRepository _personRepository;

    public UpdatePersonHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }
    public async Task<UpdatePersonResult> Handle(UpdatePerson request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetPersonByNameAsync(request.Name);

        if (person is null) throw new BadHttpRequestException("Bad Request, Person does not exist..");

        person.Name = request.NewName;

        var success = await _personRepository.UpdateAsync(person);        

        return new UpdatePersonResult()
        {
            Id = person.Id,
            Name = person.Name,
            Success = success,
            Message = success ? "Person successfully updated" :
                                "Person updated failed..."
        };
    }
}

public class UpdatePersonResult : BaseResponse
{
    public int Id { get; set; }

    public string Name { get; set; }
}