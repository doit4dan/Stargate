using FluentValidation;
using Stargate.Server.Business.Commands;
using Stargate.Server.Repositories;

namespace Stargate.Server.Validators;

public class CreatePersonValidator : AbstractValidator<CreatePerson>
{
    private readonly IPersonRepository _personRepository;

    public CreatePersonValidator(IPersonRepository personRepository)
    {
        _personRepository = personRepository;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(ValidatePersonNotExists)
            .WithMessage("This person already exists in the system");
    }

    private async Task<bool> ValidatePersonNotExists(CreatePerson createPerson, string name, CancellationToken cancellationToken = default)
    {
        return !(await _personRepository.ExistsByNameAsync(name));
    }    
}