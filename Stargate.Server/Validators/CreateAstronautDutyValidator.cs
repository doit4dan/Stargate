using FluentValidation;
using MediatR;
using Stargate.Server.Business.Commands;
using Stargate.Server.Repositories;
using System.Text.RegularExpressions;
using System.Threading;

namespace Stargate.Server.Validators;

public class CreateAstronautDutyValidator : AbstractValidator<CreateAstronautDuty>
{
    private readonly IPersonRepository _personRepository;
    private readonly IAstronautRepository _astronautRepository;
    public CreateAstronautDutyValidator(IPersonRepository personRepository, IAstronautRepository astronautRepository)
    {
        _personRepository = personRepository;
        _astronautRepository = astronautRepository;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MustAsync(ValidatePersonExists)
            .WithMessage("The person you are attempting to update does not exist in the system");

        RuleFor(x => x.Rank) // could add specific list of valid ranks if I had them
            .NotEmpty()
            .MaximumLength(20)
            .Matches(new Regex("[A-Za-z0-9]")) // allow a-z and numbers, no spaces
            .WithMessage("Rank may only comprise of alphabetical letters and numbers");

        RuleFor(x => x.DutyTitle)  // could add specific list of valid ranks if I had them
            .NotEmpty()
            .MaximumLength(50)
            .Matches(new Regex("[A-Za-z0-9 ]")) // allow a-z, numbers and spaces
            .WithMessage("DutyTitle may only comprise of alphabetical letters, numbers and spaces");

        RuleFor(x => new { x.Name, x.DutyTitle, x.DutyStartDate })
            .MustAsync(async (x, cancellation) =>
            {
                return await ValidateExistingDutyAsync(x.Name, x.DutyTitle, x.DutyStartDate); // see if we should be checking rank as well
            })
            .WithMessage("This duty record has already been recorded in the system..");        
    }    

    private async Task<bool> ValidatePersonExists(CreateAstronautDuty createAstronautDuty, string name, CancellationToken cancellationToken = default)
    {
        return (await _personRepository.ExistsByNameAsync(name));
    }

    private async Task<bool> ValidateExistingDutyAsync(string name, string dutyTitle, DateTime dutyStartDate)
    {       
        var personId = await _personRepository.GetPersonIdByNameAsync(name);
        var duties = await _astronautRepository.GetDutiesByPersonIdAsync(personId);
        var verifyNoPreviousDuty = duties
            .FirstOrDefault(z => z.DutyTitle == dutyTitle && z.DutyStartDate == dutyStartDate);

        return (verifyNoPreviousDuty is null);
    }
}
