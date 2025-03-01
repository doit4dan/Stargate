using FluentValidation;
using Stargate.Server.Business.Commands;
using Stargate.Server.Repositories;
using System.Text.RegularExpressions;

namespace Stargate.Server.Validators;

public class UpdatePersonValidator : AbstractValidator<UpdatePerson>
{
    private readonly IPersonRepository _personRepository;

    public UpdatePersonValidator(IPersonRepository personRepository)
    {
        _personRepository = personRepository;

        RuleFor(x => x.Name)
            .MustAsync(ValidatePersonExists)
            .WithMessage("The person you are attempting to update does not exist in the system");

        RuleFor(x => x.NewName)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(new Regex("[A-Za-z] [A-Za-z]")) // only a-z and space
            .WithMessage("The updated name should only comprise of letters in the alphabet and a single space between first and last name")
            .MustAsync(ValidatePersonNotExists)
            .WithMessage("The updated name is already associated with an existing user in the system");
    }

    private async Task<bool> ValidatePersonExists(UpdatePerson updatePerson, string name, CancellationToken cancellationToken = default)
    {
        return (await _personRepository.ExistsByNameAsync(name));
    }

    private async Task<bool> ValidatePersonNotExists(UpdatePerson updatePerson, string name, CancellationToken cancellationToken = default)
    {
        return !(await _personRepository.ExistsByNameAsync(name));
    }
}
