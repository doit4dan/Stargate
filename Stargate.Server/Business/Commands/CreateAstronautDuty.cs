using MediatR.Pipeline;
using MediatR;
using Stargate.Server.Data.Models;
using Stargate.Server.Controllers;
using Stargate.Server.Repositories;
using FluentValidation;
using Stargate.Server.Validators;
using System.Net;

namespace Stargate.Server.Business.Commands
{
    public class CreateAstronautDuty : IRequest<CreateAstronautDutyResult>
    {
        public required string Name { get; set; }

        public required string Rank { get; set; }

        public required string DutyTitle { get; set; }

        public DateTime DutyStartDate { get; set; }
    }

    public class CreateAstronautDutyPreProcessor : IRequestPreProcessor<CreateAstronautDuty>
    {
        private readonly IValidator<CreateAstronautDuty> _createAstronautDutyValidator;        

        public CreateAstronautDutyPreProcessor(IValidator<CreateAstronautDuty> createAstronautDutyValidator)
        {
            _createAstronautDutyValidator = createAstronautDutyValidator;
        }

        public async Task Process(CreateAstronautDuty request, CancellationToken cancellationToken)
        {
            await _createAstronautDutyValidator.ValidateAndThrowAsync(request);
        }
    }

    public class CreateAstronautDutyHandler : IRequestHandler<CreateAstronautDuty, CreateAstronautDutyResult>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IAstronautRepository _astronautRepository;

        public CreateAstronautDutyHandler(IPersonRepository personRepository, IAstronautRepository astronautRepository)
        {
            _personRepository = personRepository;
            _astronautRepository = astronautRepository;
        }

        public async Task<CreateAstronautDutyResult> Handle(CreateAstronautDuty request, CancellationToken cancellationToken)
        {
            var personId = await _personRepository.GetPersonIdByNameAsync(request.Name, cancellationToken);

            if (personId <= 0) throw new BadHttpRequestException($"Provided Person name does not exist in system");

            var astronautDetail = await _astronautRepository.GetDetailByPersonIdAsync(personId);

            var success = true;

            if (astronautDetail == null)
            {
                astronautDetail = new AstronautDetail();
                astronautDetail.PersonId = personId;
                astronautDetail.CurrentDutyTitle = request.DutyTitle;
                astronautDetail.CurrentRank = request.Rank;
                astronautDetail.CareerStartDate = request.DutyStartDate.Date;
                if (request.DutyTitle == "RETIRED")
                {
                    astronautDetail.CareerEndDate = request.DutyStartDate.AddDays(-1).Date;
                }

                success = await _astronautRepository.CreateDetailAsync(astronautDetail, cancellationToken);
            }
            else
            {
                astronautDetail.CurrentDutyTitle = request.DutyTitle;
                astronautDetail.CurrentRank = request.Rank;
                if (request.DutyTitle == "RETIRED")
                {
                    astronautDetail.CareerEndDate = request.DutyStartDate.AddDays(-1).Date;
                }
                success = await _astronautRepository.UpdateDetailAsync(astronautDetail, cancellationToken);
            }

            if (!success) throw new BadHttpRequestException($"Failed to add/update Astronaunt Detail in the system..");

            var newAstronautDuty = new AstronautDuty()
            {
                PersonId = personId,
                Rank = request.Rank,
                DutyTitle = request.DutyTitle,
                DutyStartDate = request.DutyStartDate.Date,
                DutyEndDate = null
            };

            success = await _astronautRepository.CreateDutyAsync(newAstronautDuty, cancellationToken);
            
            return new CreateAstronautDutyResult()
            {
                Id = newAstronautDuty.Id,
                Success = success,
                Message = success ? "Successfully recorded Astronaut Duty Details in system" :
                                    "Failed to record Astronaut Duty Details in system...",
                ResponseCode = success ? (int)HttpStatusCode.OK :
                               (int)HttpStatusCode.InternalServerError
            };
        }
    }

    public class CreateAstronautDutyResult : BaseResponse
    {
        public int? Id { get; set; }
    }
}
